if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Transform ward = null; foreach (var r0 in scene.GetRootGameObjects()) if (r0.name == "Ward") ward = r0.transform;
var sb = new System.Text.StringBuilder();
// name, width, height, depth, yaw, lean toward +z (degrees), sideways lean, taper
var specs = new (string name, float w, float h, float d, float yaw, float leanZ, float leanX, float taper)[] {
    ("Stone_1", 2.6f, 5.5f, 1.7f, 22f, 13f, 4f, 0.6f),      // north stone: short, leaning north and a touch toward the cliff
    ("Stone_3", 3.1f, 10.0f, 2.0f, -25f, -9f, -3f, 0.8f),   // south stone: tall, leaning south
};
foreach (var s in specs)
{
    var go = ward.Find(s.name).gameObject;
    var pb = go.GetComponent<UnityEngine.ProBuilder.ProBuilderMesh>();
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    // Positions are the tapered cube from the first build; recover the unit cube from the sign of each coordinate.
    for (int i = 0; i < pos.Count; i++)
    {
        var p = pos[i]; float ux = p.x < 0f ? -0.5f : 0.5f, uy = p.y < 2f ? -0.5f : 0.5f, uz = p.z < 0f ? -0.5f : 0.5f;
        float taper = uy > 0f ? s.taper : 1f;
        pos[i] = new UnityEngine.Vector3(ux * s.w * taper, (uy + 0.5f) * s.h - 1f, uz * s.d * taper);
    }
    pb.positions = pos; pb.ToMesh(); pb.Refresh();
    go.transform.rotation = UnityEngine.Quaternion.Euler(s.leanZ, s.yaw, s.leanX);
    var col = go.GetComponent<UnityEngine.MeshCollider>(); col.sharedMesh = null; col.sharedMesh = go.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    var mat = go.GetComponent<UnityEngine.Renderer>().sharedMaterial;
    mat.SetTextureScale("_BaseMap", new UnityEngine.Vector2(1f / (s.w * 1.4f), 1f / (s.h - 1f))); UnityEditor.EditorUtility.SetDirty(mat);
    sb.Append(s.name + " h=" + s.h + " rot=" + go.transform.eulerAngles.ToString("F0") + " bounds=" + go.GetComponent<UnityEngine.Renderer>().bounds.size.ToString("F1") + "; ");
}
// Fire: smaller flames.
var shortF = UnityEngine.GameObject.Find("Camp/FirePit/FX_Flames_Short"); if (shortF != null) shortF.transform.localScale = V(0.55f, 0.55f, 0.55f);
var tallF = UnityEngine.GameObject.Find("Camp/FirePit/FX_Flames_Tall"); if (tallF != null) tallF.transform.localScale = V(0.3f, 0.3f, 0.3f);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);

// Shots: from the warp point and from the ledge.
var outDir = System.IO.Path.Combine(@"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad", "ward3");
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("R1_from_warp", V(62f, 41.6f, 322f), V(47f, 44f, 322.5f)),
    ("R2_from_ledge_side", V(56f, 41.6f, 338f), V(47f, 45f, 320f)),
};
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render(); UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
}
cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "saved=" + saved + " " + sb;
