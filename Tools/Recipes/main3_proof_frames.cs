// Main3 proof frames (8.18a, Wren's fix list 2026-10-01: a close-up proof frame of every disputed object). Play mode, Main3.
// Reads shots from shotsFile, one per line: name|look|eye x,y,z|target x,y,z[|fov]   (look: "Day one", "Night" or "Day two")
// and renders each shot whose look is the current one at 1920 x 988 from Camera.main (the look filter draws, the noise band off for
// the render, restored after) into outDir/<name>.png. Shots in another look are skipped and named; select that look (LookPreview) and
// run again. Lines starting with # are notes. Nothing in the scene or any asset is saved.
// Usage: bash Tools/Recipes/main3_proof_frames.sh <shots file> <outDir>
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
string shotsFile = System.IO.Path.GetFullPath("Tools/Recipes/main3_proof_shots.txt");
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_818a/Proof");
string wantLook = "";
const int shotW = 1920, shotH = 988; const float defaultFov = 60f;
var inv = System.Globalization.CultureInfo.InvariantCulture;
UnityEngine.Application.runInBackground = true;
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>(); if (preview == null) return "no LookPreview";
if (wantLook != "" && preview.CurrentLabel != wantLook)
{
    for (int i = 0; i < preview.Count; i++) if (preview.Label(i) == wantLook) { preview.Select(i); return "selected " + wantLook + "; run again"; }
    return "no look row " + wantLook;
}
if (!System.IO.File.Exists(shotsFile)) return "no shots file " + shotsFile;
System.IO.Directory.CreateDirectory(outDir);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; float fov0 = cam.fieldOfView;
var tune = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); float band = tune.noiseBandStrength; tune.noiseBandStrength = 0f;
UnityEngine.Vector3 P(string s) { var a = s.Split(','); return new UnityEngine.Vector3(float.Parse(a[0], inv), float.Parse(a[1], inv), float.Parse(a[2], inv)); }
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32);
var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var done = new System.Collections.Generic.List<string>(); var skipped = new System.Collections.Generic.List<string>();
try
{
    foreach (var raw in System.IO.File.ReadAllLines(shotsFile))
    {
        var line = raw.Trim(); if (line.Length == 0 || line.StartsWith("#")) continue;
        var f = line.Split('|'); if (f.Length < 4) return "bad line: " + line;
        if (f[1] != preview.CurrentLabel) { skipped.Add(f[0] + " (" + f[1] + ")"); continue; }
        var eyeP = P(f[2]); var look = P(f[3]); cam.fieldOfView = f.Length > 4 ? float.Parse(f[4], inv) : defaultFov;
        var dir = look - eyeP; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z); if (flat.sqrMagnitude < 1e-6f) flat = UnityEngine.Vector3.forward;
        cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = eyeP - pc.transform.rotation * camLocal;
        cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
        UnityEngine.Physics.SyncTransforms();
        cam.targetTexture = rt; cam.Render(); cam.targetTexture = null;
        UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, f[0] + ".png"), shot.EncodeToPNG()); done.Add(f[0]);
    }
}
finally { tune.noiseBandStrength = band; cam.fieldOfView = fov0; cam.targetTexture = null; rt.Release(); UnityEngine.Object.Destroy(rt); UnityEngine.Object.Destroy(shot); }
return preview.CurrentLabel + ": wrote " + done.Count + " (" + string.Join(", ", done) + ") to " + outDir + (skipped.Count > 0 ? " | other looks: " + string.Join(", ", skipped) : "");
