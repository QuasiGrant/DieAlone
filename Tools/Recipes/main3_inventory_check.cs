// Main3 inventory (Vesper, Docs/Review/2026-10-02-Meeting/Vesper.md 4): one line per place and effect, expected count and found count,
// so no gate grades a build that silently dropped dressing. Read-only; edit or Play mode, Main3. Run last in main3_rebuild.sh and first
// in main3_review_capture.sh. Returns "ALL PASS" when nothing is zero; a count that differs from the expected one is listed as DIFF
// (a recipe changed what it builds: update the expected number in the same commit as that recipe). Counts include inactive objects
// (LookVisibility switches the day and night pieces). Expected numbers: as built by the runner on 2026-10-02.
// Kinds: quads = mesh quads (vertices / 4) on MeshFilters named `name` under the path; renderers = Renderers under the path whose
// object name starts with `name` ("" for all); lights = Light components under the path; practicals = PracticalLight components in the scene.
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "FAIL open Main3 first";
var items = new (string label, string path, string kind, string name, int expected)[] {
    ("Ridge fire cards, night",   "Ward/StandInFire",                "quads",      "RidgeFlames",     132),
    ("Ridge fire cards, day",     "Ward/StandInFire",                "quads",      "RidgeFlamesDay",   68),
    ("Valley fire cards, night",  "Ward/StandInFire",                "quads",      "ValleyFlames",    297),
    ("Valley fire cards, day",    "Ward/StandInFire",                "quads",      "ValleyFlamesDay", 167),
    ("Smoke sheet, day one",      "Ward/SmokeSheet",                 "quads",      "Body",            689),
    ("Smoke lid, night",          "Ward/SmokeSheet",                 "quads",      "Lid",             171),
    ("Smoke columns, day two",    "Ward/StandInFire/SmokeColumns",   "quads",      "",                128),
    ("Ward stones",               "Ward/Stones",                     "renderers",  "",                  3),
    ("Camp 3 tent",               "Campsites/Camp_3/Dressing",       "renderers",  "CS_Tent",           8),
    ("Camp 3 fire",               "Campsites/Camp_3/Dressing/Fire",  "renderers",  "",                 15),
    ("Camp 3 easel",              "Campsites/Camp_3/Dressing/Easel", "renderers",  "",                 13),
    ("North ruin",                "Places/NorthRuin",                "renderers",  "",                 44),
    ("Cave lights",               "Cave",                            "lights",     "",                  5),
    ("Lanterns and lamps",        "",                                "practicals", "",                 28),
};
UnityEngine.Transform At(string path)   // the first path part is a scene root (a find by name can return a child, e.g. DevWarps/Ward)
{
    var parts = path.Split(new[] { '/' }, 2); UnityEngine.Transform root = null;
    foreach (var r in scene.GetRootGameObjects()) if (r.name == parts[0]) { root = r.transform; break; }
    return root == null || parts.Length == 1 ? root : root.Find(parts[1]);
}
bool Under(UnityEngine.Transform t, UnityEngine.Transform top, string name)   // t or a parent below top starts with name
{
    if (name == "") return true;
    for (var p = t; p != null && p != top.parent; p = p.parent) if (p.name.StartsWith(name)) return true;
    return false;
}
var sb = new System.Text.StringBuilder(); int zeros = 0, diffs = 0;
foreach (var it in items)
{
    int found = 0; bool missingRoot = false;
    if (it.kind == "practicals") found = UnityEngine.Object.FindObjectsByType<PracticalLight>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None).Length;
    else
    {
        var top = At(it.path);
        if (top == null) missingRoot = true;
        else if (it.kind == "quads") { foreach (var mf in top.GetComponentsInChildren<UnityEngine.MeshFilter>(true)) if ((it.name == "" || mf.name == it.name) && mf.sharedMesh != null) found += mf.sharedMesh.vertexCount / 4; }
        else if (it.kind == "renderers") { foreach (var r in top.GetComponentsInChildren<UnityEngine.Renderer>(true)) if (Under(r.transform, top, it.name)) found++; }
        else if (it.kind == "lights") found = top.GetComponentsInChildren<UnityEngine.Light>(true).Length;
    }
    string verdict = found == 0 ? "ZERO" : it.expected >= 0 && found != it.expected ? "DIFF" : "ok";
    if (found == 0) zeros++; else if (verdict == "DIFF") diffs++;
    sb.Append(verdict + " " + it.label + ": expected " + (it.expected >= 0 ? it.expected.ToString() : "?") + ", found " + found + (missingRoot ? " (no " + it.path + ")" : "") + "\n");
}
return (zeros == 0 && diffs == 0 ? "ALL PASS" : zeros > 0 ? "FAIL " + zeros + " zero" : "FAIL " + diffs + " differ") + " | inventory\n" + sb;
