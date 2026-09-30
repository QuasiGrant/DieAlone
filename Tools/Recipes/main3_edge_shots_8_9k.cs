// Main3 8.9k (Play mode, day one): one shot of each map edge for Vesper (Edges.md 9): N, S, E and W from the valley floor, and the
// view west from the ledge path end. Poses the player camera at 1.6 m eye height on the ground, renders Camera.main (look filter
// included) into a RenderTexture and writes Docs/Look/Edges/<shot>.png (WalkChecks.md: no capture_game_view with a save path).
// Leaves the controller off; leave Play mode after. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
const int shotW = 1920, shotH = 988; const float eye = 1.6f;   // Grant's Game view shape (3840 x 1976) at half size
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
// (name, stand x, z, look-at x, y, z)
var shots = new (string n, float x, float z, float lx, float ly, float lz)[] {
    ("North_from_Camp1", 282f, 238f, 282f, 60f, 350f),
    ("South_from_the_dock", 190f, 92f, 190f, 25f, -50f),
    ("East_road_cut_from_T", 337f, 170f, 445f, 12f, 170f),
    ("West_from_the_camp", 170f, 160f, 0f, 95f, 160f),
    ("Ledge_west_from_the_path_end", -2f, 258f, -300f, 70f, 258f) };
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
string outDir = System.IO.Path.GetFullPath("Docs/Look/Edges"); System.IO.Directory.CreateDirectory(outDir);
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var tex = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder();
try
{
    foreach (var s in shots)
    {
        var at = new UnityEngine.Vector3(s.x, H(s.x, s.z) + eye, s.z); var look = new UnityEngine.Vector3(s.lx, s.ly, s.lz);
        var dir = look - at; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
        cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = at - pc.transform.rotation * camLocal; cc.enabled = true;
        cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
        cam.targetTexture = rt; cam.Render(); cam.targetTexture = null;
        UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
        string path = System.IO.Path.Combine(outDir, s.n + ".png"); System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        sb.Append(s.n + ": camera " + cam.transform.position.ToString("F1") + " toward " + look + "\n");
    }
}
finally { cam.targetTexture = null; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); }
return "wrote " + shots.Length + " shots to " + outDir + "\n" + sb;
