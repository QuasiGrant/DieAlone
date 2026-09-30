// Check E-1 (Docs/Design/Edges.md 1.8; 8.9k): every look toward the map edge ends on land. Edit mode, Main3 open; saves
// nothing (probe colliders live in a temporary additive scene that is closed unsaved). Writes Docs/Layout/Main3/Main3_E1.md.
// Origins: the tower deck grid (1 m over the 8 x 8 m deck, eye 57.6 and jump 58.2), every place (DevWarps point, 1.6 m eye;
// stands in for the platforms until Valley.md builds them) and every trail point at 10 m steps (1.6 m eye).
// Rays: every 2 degrees of bearing, at 5, 4 ... 0 ... -4, -5 degrees of elevation, out to the camera far clip.
// Land is the terrain (its collider) and the landscape meshes (everything under root "Backdrop" and the stand-in fire's
// ridge and floor under Ward/StandInFire; flame and smoke cards excluded), which get temporary MeshColliders here.
// Trees, buildings and props in the map are ignored: E-1 is about what stands behind them. Backdrop trees count by their LOD 0
// meshes. Places under the terrain surface (the cave chamber) have no view of an edge and are skipped.
// A ray passes when it ends on terrain, on a landscape mesh that is not a flat plane, in the sky at or above level, or in the cave
// (down through the cave mouth's terrain hole, counted with terrain).
// It fails as "void" when it points below level and meets nothing (the land ends before the far clip), or as
// "flat plane" when it ends on a planar landscape mesh (its whole surface within PlaneTolerance of one height: a floor slab, not land).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const float BearingStep = 2f, MaxElevation = 5f, ElevationStep = 1f, EyeHeight = 1.6f, TrailStep = 10f;
const int ProbeLayer = 31;   // unused layer: the probes are the only colliders on it
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
if (UnityEngine.LayerMask.LayerToName(ProbeLayer) != "") return "layer " + ProbeLayer + " is in use";
var tower = Root("Camp").transform.Find("Tower"); var caveRoot = Root("Cave") != null ? Root("Cave").transform : null;
var cam = UnityEngine.Object.FindFirstObjectByType<PlayerController>(UnityEngine.FindObjectsInactive.Include)?.GetComponentInChildren<UnityEngine.Camera>(true);
float reach = cam != null ? cam.farClipPlane : 3500f;
// a flat plane is a landscape mesh whose whole surface lies within PlaneTolerance of one height (like rev 16's Ground_* planes);
// rolling ground (the outer ground, 0 to 10) and every ridge are land
const float PlaneTolerance = 1f;
var planar = new System.Collections.Generic.HashSet<UnityEngine.Collider>();

// ---- landscape meshes to probe colliders in a temporary scene
var land = new System.Collections.Generic.List<UnityEngine.MeshFilter>();
var lodOnly = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); var lodAny = new System.Collections.Generic.HashSet<UnityEngine.Renderer>();
foreach (var lg in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{ var lods = lg.GetLODs(); for (int i = 0; i < lods.Length; i++) foreach (var rr in lods[i].renderers) if (rr != null) { lodAny.Add(rr); if (i == 0) lodOnly.Add(rr); } }
void AddLand(UnityEngine.Transform root)
{
    if (root == null) return;
    foreach (var mf in root.GetComponentsInChildren<UnityEngine.MeshFilter>(true))
    {
        var mr = mf.GetComponent<UnityEngine.MeshRenderer>(); if (mr == null || mf.sharedMesh == null) continue;
        if (lodOnly.Contains(mr) == false && lodAny.Contains(mr)) continue;   // a tree: only its LOD 0 meshes, as seen up close
        string sh = mr.sharedMaterial != null ? mr.sharedMaterial.shader.name : "";
        if (sh.Contains("Flame") || sh.Contains("Smoke")) continue;
        land.Add(mf);
    }
}
AddLand(Root("Backdrop") != null ? Root("Backdrop").transform : null);
var fireRoot = Root("Ward") != null ? Root("Ward").transform.Find("StandInFire") : null;
if (fireRoot != null) { AddLand(fireRoot.Find("Ridge")); AddLand(fireRoot.Find("ValleyFloor")); }
var probeScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Additive);
var probeName = new System.Collections.Generic.Dictionary<UnityEngine.Collider, string>();
try
{
    foreach (var mf in land)
    {
        var go = new UnityEngine.GameObject("E1Probe"); UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, probeScene);
        go.layer = ProbeLayer; go.transform.SetPositionAndRotation(mf.transform.position, mf.transform.rotation); go.transform.localScale = mf.transform.lossyScale;
        var mc = go.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh;
        string path = mf.name; for (var p = mf.transform.parent; p != null; p = p.parent) path = p.name + "/" + path;
        probeName[mc] = path; if (mf.GetComponent<UnityEngine.MeshRenderer>().bounds.size.y < PlaneTolerance) planar.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
    var terrainCols = new System.Collections.Generic.List<UnityEngine.TerrainCollider>();
    foreach (var tc in UnityEngine.Object.FindObjectsByType<UnityEngine.TerrainCollider>(UnityEngine.FindObjectsSortMode.None)) if (tc.enabled) terrainCols.Add(tc);
    // a terrain collider raycast in segments of TerrainRaySegment metres (8.14): one long PhysX heightfield raycast (2 km) missed a
    // plain crossing 450 m out (J to Ward/P110 at bearing 110, -5 degrees) while the same ray cast from 300 m out hit it
    const float TerrainRaySegment = 200f;
    bool TerrainRay(UnityEngine.TerrainCollider tc, UnityEngine.Ray ray, out UnityEngine.RaycastHit hit)
    {
        for (float s = 0f; s < reach; s += TerrainRaySegment)
            if (tc.Raycast(new UnityEngine.Ray(ray.origin + ray.direction * s, ray.direction), out hit, UnityEngine.Mathf.Min(TerrainRaySegment, reach - s))) { hit.distance += s; return true; }
        hit = default; return false;
    }

    // ---- origins
    var origins = new System.Collections.Generic.List<(string kind, string name, UnityEngine.Vector3 p)>();
    float deckTop = tower.Find("Cab").position.y;   // the deck top, as main3_8_9_sightlines.cs (8.14: was tower.position.y + 48, 7 m high since rev 13)
    foreach (var h in new[] { ("eye", deckTop + EyeHeight), ("jump", deckTop + 2.2f) })
        for (float gx = -3.5f; gx <= 3.5f; gx += 1f) for (float gz = -3.5f; gz <= 3.5f; gz += 1f)
            origins.Add(("deck", h.Item1, V(tower.position.x + gx, h.Item2, tower.position.z + gz)));
    var warps = Root("DevWarps");
    var terrainMain = Root("Terrain").GetComponent<UnityEngine.Terrain>(); var underground = new System.Collections.Generic.List<string>();
    bool Underground(UnityEngine.Vector3 p) => terrainMain.SampleHeight(p) + terrainMain.transform.position.y > p.y;   // inside the cave: no view of any edge
    if (warps != null) foreach (UnityEngine.Transform w in warps.transform) { var p = w.position + V(0f, EyeHeight, 0f); if (Underground(p)) underground.Add(w.name); else origins.Add(("place", w.name, p)); }
    var trails = Root("Trails");
    if (trails != null) foreach (UnityEngine.Transform leg in trails.transform) foreach (UnityEngine.Transform pt in leg)
        if (pt.name.Length > 1 && int.TryParse(pt.name.Substring(1), out int metres) && metres % (int)TrailStep == 0) origins.Add(("trail", leg.name + "/" + pt.name, pt.position + V(0f, EyeHeight, 0f)));

    // ---- rays
    var dirs = new System.Collections.Generic.List<(float bearing, float elev, UnityEngine.Vector3 d)>();
    for (float b = 0f; b < 360f; b += BearingStep) for (float e = -MaxElevation; e <= MaxElevation + 0.01f; e += ElevationStep)
        dirs.Add((b, e, UnityEngine.Quaternion.Euler(-e, b, 0f) * UnityEngine.Vector3.forward));
    int mask = 1 << ProbeLayer;
    var byKind = new System.Collections.Generic.Dictionary<string, int[]>();   // rays, terrain, landscape, sky, void, flat
    var failBearings = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedSet<int>>();
    var failOrigins = new System.Collections.Generic.Dictionary<string, int>();
    var flatBy = new System.Collections.Generic.Dictionary<string, int>();
    foreach (var o in origins)
    {
        if (!byKind.ContainsKey(o.kind)) byKind[o.kind] = new int[6];
        var c = byKind[o.kind];
        foreach (var r in dirs)
        {
            c[0]++;
            var ray = new UnityEngine.Ray(o.p, r.d);
            float tDist = float.MaxValue;
            foreach (var tc in terrainCols) if (TerrainRay(tc, ray, out var th)) tDist = UnityEngine.Mathf.Min(tDist, th.distance);
            bool landHit = UnityEngine.Physics.Raycast(ray, out var lh, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore) && lh.distance < tDist;
            string fail = null;
            if (landHit) { if (planar.Contains(lh.collider)) { fail = "flat plane"; c[5]++; string n = probeName.TryGetValue(lh.collider, out var pn) ? pn : lh.collider.name; flatBy[n] = (flatBy.TryGetValue(n, out var k) ? k : 0) + 1; } else c[2]++; }
            else if (tDist < float.MaxValue) c[1]++;
            else if (r.elev >= 0f) c[3]++;
            else if (caveRoot != null && UnityEngine.Physics.Raycast(ray, out var ch, reach, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) && ch.collider.transform.IsChildOf(caveRoot)) c[1]++;   // down through the cave mouth's terrain hole: the cave, not an edge
            else { fail = "void"; c[4]++; }
            if (fail != null)
            {
                string key = o.kind + " " + fail; if (!failBearings.ContainsKey(key)) failBearings[key] = new System.Collections.Generic.SortedSet<int>();
                failBearings[key].Add((int)r.bearing);
                string on = o.kind + " " + o.name; failOrigins[on] = (failOrigins.TryGetValue(on, out var q) ? q : 0) + 1;
            }
        }
    }

    // ---- report
    string Ranges(System.Collections.Generic.SortedSet<int> set)
    {
        var parts = new System.Collections.Generic.List<string>(); int start = -1, prev = -1;
        foreach (var b in set) { if (start < 0) { start = prev = b; continue; } if (b == prev + (int)BearingStep) { prev = b; continue; } parts.Add(start == prev ? start.ToString() : start + " to " + prev); start = prev = b; }
        if (start >= 0) parts.Add(start == prev ? start.ToString() : start + " to " + prev);
        return string.Join(", ", parts);
    }
    var md = new System.Text.StringBuilder();
    md.AppendLine("# Main3 E-1 edge check\n");
    md.AppendLine("Generated by Tools/Recipes/main3_e1_edges.cs (Edges.md 1.8). Rays every " + BearingStep + " degrees of bearing at " + (-MaxElevation) + " to " + MaxElevation + " degrees elevation in " + ElevationStep + " degree steps, out to " + reach + " m. Land: terrain collider, and " + land.Count + " landscape meshes (Backdrop, stand-in fire ridge and floor). A ray fails as void (below level, meets nothing) or flat plane (ends on a planar landscape mesh, within " + PlaneTolerance + " m of one height). Places are every DevWarps point, which include P3, P4 and the ledge path end (Ward); P1, P2 and the rest of the climb are covered by the trail points every 10 m. Trees count by their LOD 0 meshes. Skipped as underground: " + (underground.Count == 0 ? "none" : string.Join(", ", underground)) + ".\n");
    md.AppendLine("| Origins | Count | Rays | Terrain | Landscape mesh | Sky (level or up) | Void | Flat plane |");
    md.AppendLine("|---|---|---|---|---|---|---|---|");
    bool pass = true; string line = "";
    foreach (var kv in byKind)
    {
        int count = 0; foreach (var o in origins) if (o.kind == kv.Key) count++;
        var c = kv.Value; if (c[4] + c[5] > 0) pass = false;
        md.AppendLine("| " + kv.Key + " | " + count + " | " + c[0] + " | " + c[1] + " | " + c[2] + " | " + c[3] + " | " + c[4] + " | " + c[5] + " |");
        line += kv.Key + " fails " + (c[4] + c[5]) + "/" + c[0] + " (void " + c[4] + ", flat " + c[5] + "); ";
    }
    if (failBearings.Count > 0)
    {
        md.AppendLine("\n## Failing bearings (degrees, 0 north, 90 east)\n");
        foreach (var kv in failBearings) md.AppendLine("- " + kv.Key + ": " + Ranges(kv.Value));
        md.AppendLine("\n## Origins with the most failing rays\n");
        var ord = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>(failOrigins); ord.Sort((p, q) => q.Value.CompareTo(p.Value));
        for (int i = 0; i < ord.Count && i < 15; i++) md.AppendLine("- " + ord[i].Key + ": " + ord[i].Value);
        if (flatBy.Count > 0) { md.AppendLine("\n## Flat planes hit\n"); foreach (var kv in flatBy) md.AppendLine("- " + kv.Key + ": " + kv.Value + " rays"); }
    }
    // Marlow's grazing rays (ValleyNumbers R4.3): from the tower deck centre and P4, aimed at his paper landing points; where each lands
    var deckEye = V(tower.position.x, deckTop + EyeHeight, tower.position.z);
    UnityEngine.Vector3 WarpEye(string n) { var w = warps != null ? warps.transform.Find(n) : null; return w != null ? w.position + V(0f, EyeHeight, 0f) : V(float.NaN, 0f, 0f); }
    var p4 = WarpEye("Ward_P4");
    var probes = new (string n, UnityEngine.Vector3 from, UnityEngine.Vector3 aim)[] {
        ("tower over the E crest at z -40, aim (1225, 10, -612)", deckEye, V(1225f, 10f, -612f)), ("tower over the E crest at z -40, aim (1448, 0, -775)", deckEye, V(1448f, 0f, -775f)),
        ("P4 over the S crest end, aim (742, 10, -216)", p4, V(742f, 10f, -216f)), ("P4 over the S crest end, aim (826, 0, -266)", p4, V(826f, 0f, -266f)),
        ("tower over the N saddle, aim (703, 10, 1319)", deckEye, V(703f, 10f, 1319f)), ("tower over the N saddle, aim (787, 2.5, 1500)", deckEye, V(787f, 2.5f, 1500f)) };
    md.AppendLine("\n## Marlow's grazing rays (R4.3)\n");
    string probeLine = ""; bool probesOk = true;
    foreach (var pr in probes)
    {
        if (float.IsNaN(pr.from.x)) { md.AppendLine("- " + pr.n + ": origin missing"); probesOk = false; continue; }
        var ray = new UnityEngine.Ray(pr.from, (pr.aim - pr.from).normalized);
        float tDist = float.MaxValue; UnityEngine.Vector3 tPt = default;
        foreach (var tc in terrainCols) if (TerrainRay(tc, ray, out var th) && th.distance < tDist) { tDist = th.distance; tPt = th.point; }
        string what;
        if (UnityEngine.Physics.Raycast(ray, out var lh, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore) && lh.distance < tDist)
            what = (probeName.TryGetValue(lh.collider, out var pn) ? pn : lh.collider.name) + (planar.Contains(lh.collider) ? " (FLAT PLANE)" : "") + " at " + lh.point.ToString("F0");
        else if (tDist < float.MaxValue) what = "terrain at " + tPt.ToString("F0");
        else { what = "VOID"; probesOk = false; }
        if (what.Contains("FLAT")) probesOk = false;
        md.AppendLine("- " + pr.n + ": " + what); probeLine += what.Split(' ')[0] + "; ";
    }
    md.AppendLine("\n## Result\n\n- E-1 " + (pass ? "passes" : "FAILS") + ": " + line);
    md.AppendLine("- Marlow's grazing rays land on land: " + (probesOk ? "yes" : "NO"));
    line += "| grazing rays ok " + probesOk + " (" + probeLine + ") ";
    string outPath = System.IO.Path.GetFullPath("Docs/Layout/Main3/Main3_E1.md");
    System.IO.File.WriteAllText(outPath, md.ToString());
    return "E-1 pass: " + pass + " | origins " + origins.Count + ", rays per origin " + dirs.Count + ", landscape meshes " + land.Count + " | " + line + "| report " + outPath;
}
finally
{
    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(probeScene, true);
    UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene(scene);
}
