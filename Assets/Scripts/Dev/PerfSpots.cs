#if DEVELOPMENT_BUILD
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Development builds only: the frame-rate run of Tools/Recipes/perf_baseline_8_16.cs inside a player. Started with the command line
/// "-perfspots <output file>", it loads Main3, poses the player's camera at the same three spots (Camp, S1, the office), skips the
/// warm-up frames, records the frame times, writes the average and the 1% low per spot to the file and quits; it goes round the spots
/// twice, so the second pass shows each view once its shaders and data are loaded. Without the flag it
/// does nothing. With "-perfdetail" it also logs profiler counters for the slowest frames (see Detail).
public class PerfSpots : MonoBehaviour
{
    private const string Flag = "-perfspots", SceneName = "Main3";
    private const int WarmFrames = 60, DefaultFrames = 600, Passes = 2;
    private const string FramesFlag = "-perfframes";   // optional: frames recorded per spot (default 600)
    private int frames = DefaultFrames;
    private const float Eye = 1.6f, LookAhead = 40f;

    private string outPath;
    // "-perfdetail": per frame, the profiler counters below (the previous frame's value, as the frame time is); for the slowest 1% of
    // frames each counter's mean against the median frame's, and where the slow frames fall, so the cause of the 1% low shows
    private const string DetailFlag = "-perfdetail";
    private static readonly string[] Counters = { "Main Thread", "Render Thread", "PlayerLoop", "GC Allocated In Frame", "GC.Collect", "Gfx.WaitForPresentOnGfxThread", "Gfx.WaitForGfxCommandsFromMainThread", "Gfx.PresentFrame", "Physics.Simulate", "Camera.Render", "Shader.CreateGPUProgram", "Loading.ReadObject" };
    private bool detail;
    private readonly List<Unity.Profiling.ProfilerRecorder> recorders = new List<Unity.Profiling.ProfilerRecorder>();
    private readonly List<string> recorderNames = new List<string>();

    private void StartRecorders()
    {
        var all = new List<Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle>(); Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.GetAvailable(all);
        foreach (var name in Counters)
            foreach (var h in all)
                if (Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle.GetDescription(h).Name == name)
                { var r = new Unity.Profiling.ProfilerRecorder(h, 1, Unity.Profiling.ProfilerRecorderOptions.Default); r.Start(); recorders.Add(r); recorderNames.Add(name); break; }
    }

    private void OnDestroy() { foreach (var r in recorders) r.Dispose(); }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
            if (args[i] == Flag)
            {
                var go = new GameObject("PerfSpots"); DontDestroyOnLoad(go);
                var ps = go.AddComponent<PerfSpots>(); ps.outPath = args[i + 1];
                for (int j = 0; j < args.Length - 1; j++) if (args[j] == FramesFlag && int.TryParse(args[j + 1], out int n) && n > 0) ps.frames = n;
                foreach (var a in args) if (a == DetailFlag) ps.detail = true;
                return;
            }
    }

    private string Detail(List<(float dt, long[] v, int index)> rows, int worst)
    {
        var sorted = new List<(float dt, long[] v, int index)>(rows); sorted.Sort((a, b) => a.dt.CompareTo(b.dt));
        var median = sorted[sorted.Count / 2]; var slow = sorted.GetRange(sorted.Count - worst, worst);
        var sb = new StringBuilder("  slow frames (worst " + worst + ") at frame: ");
        var idx = new List<int>(); foreach (var r in slow) idx.Add(r.index); idx.Sort(); sb.Append(string.Join(",", idx)).Append("\n  frame ms slow mean ");
        float dts = 0f; foreach (var r in slow) dts += r.dt; sb.Append((dts / worst * 1000f).ToString("F1")).Append(", median ").Append((median.dt * 1000f).ToString("F1")).Append("\n");
        for (int k = 0; k < recorderNames.Count; k++)
        {
            double s = 0; foreach (var r in slow) s += r.v[k]; s /= worst;
            bool bytes = recorderNames[k].StartsWith("GC Allocated");
            string F(double x) => bytes ? (x / 1024.0).ToString("F0") + " KB" : (x / 1e6).ToString("F2") + " ms";
            sb.Append("  ").Append(recorderNames[k]).Append(": slow ").Append(F(s)).Append(", median ").Append(F(median.v[k])).Append("\n");
        }
        return sb.ToString();
    }

    private IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync(SceneName);
        yield return null;
        var pc = FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<CharacterController>(); var cam = Camera.main; var terrain = Terrain.activeTerrain;
        var camLocal = cam.transform.localPosition; pc.enabled = false;
        Vector3 At(float x, float z) => new Vector3(x, terrain.SampleHeight(new Vector3(x, 0f, z)) + terrain.transform.position.y + Eye, z);
        Vector3 Ahead(Vector3 c, float yaw) => c + Quaternion.Euler(0f, yaw, 0f) * Vector3.forward * LookAhead;
        var spots = new (string name, Vector3 cam, Vector3 look)[] {
            ("Camp", At(172f, 150f), Ahead(At(172f, 150f), 333f)),
            ("S1", At(156f, 148f), new Vector3(178f, At(156f, 148f).y, 168f)),
            ("Office", At(340f, 196f), Ahead(At(340f, 196f), 68f)) };
        var log = new StringBuilder("player " + Application.version + ", screen " + Screen.width + " x " + Screen.height + ", " + frames + " frames per spot after " + WarmFrames + " warm-up, vSync " + QualitySettings.vSyncCount + "\n");
        var times = new List<float>(); var rows = new List<(float dt, long[] v, int index)>();
        if (detail) StartRecorders();
        for (int pass = 1; pass <= Passes; pass++)
        foreach (var s in spots)
        {
            var dir = s.look - s.cam; var flat = new Vector3(dir.x, 0f, dir.z);
            cc.enabled = false; pc.transform.rotation = Quaternion.LookRotation(flat.normalized); pc.transform.position = s.cam - pc.transform.rotation * camLocal;
            cam.transform.localRotation = Quaternion.Euler(-Mathf.Atan2(dir.y, flat.magnitude) * Mathf.Rad2Deg, 0f, 0f);
            for (int i = 0; i < WarmFrames; i++) yield return null;
            times.Clear();
            rows.Clear();
            for (int i = 0; i < frames; i++) { yield return null; times.Add(Time.unscaledDeltaTime); if (detail) { var v = new long[recorders.Count]; for (int k = 0; k < v.Length; k++) v[k] = recorders[k].LastValue; rows.Add((Time.unscaledDeltaTime, v, i)); } }
            times.Sort(); int worst = Mathf.Max(1, times.Count / 100); float sum = 0f, worstSum = 0f;
            foreach (var t in times) sum += t; for (int i = times.Count - worst; i < times.Count; i++) worstSum += times[i];
            log.Append("pass " + pass + " " + s.name + ": average " + (times.Count / sum).ToString("F1") + " fps, 1% low " + (worst / worstSum).ToString("F1") + " fps\n");
            if (detail && rows.Count > 0) log.Append(Detail(rows, worst));
        }
        log.Append("done\n");
        System.IO.File.WriteAllText(outPath, log.ToString());
        Application.Quit();
    }
}
#endif
