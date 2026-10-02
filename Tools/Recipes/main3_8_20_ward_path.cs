// Main3 task 8.20, the Ward path rebuild (Docs/Design/WardPath.md draft 2; WardPath_Marlow.md draft 2 conditions; Wren 2026-10-01).
// Run after main3_8_17_stones.cs in Main3, edit mode (the runner runs it before the 8.18 look); rerunnable on the open scene: on its first
// run it keeps the heights of the region it reshapes (Assets/Terrain/Main3/WardPathBase.bytes) and every rerun starts from them, so the
// cuts never stack; what it builds lives under Ward/WardPath and is rebuilt each run.
// The route: J, the gate and the draw (the old chute, its banks filled), P1, the shelf as built to (54.4, 244.5), then west across the
// bench to the stair foot and up three flights cut into the face (flight 1 north-west to landing 1, flight 2 south to landing 2, flight 3
// north to the lookout at the old P4), then the throat, the cleft and the fin as built, and on along the ledge to the rock prow. Everything
// north of z 272 (the old shelf north, P2, the cwm, P3, leg 4 past the lookout) is closed by a fallen giant.
// Ground measured 2026-10-01 along every line before any cut (Rook, read-only): the stair crosses a gully at z 249 to 256 (36.5 to 37 on
// the bench, 46 to 52 on the face) and the landings stand 6 m into the face; the lookout ground is 58 to 60; the prow ground falls from 62
// at x -10 to 37 at x -11.
// 1. Terrain: for every flight and landing a bench: fill first (the tread minus its depth, falling away at fillDeg), then cut (rising at
//    cutDeg), then the tread itself, each cell to its nearest piece. Flights 1 and 2 are graded earth (the terrain is the tread); flight 3
//    is a timber stair on stringers over a bench stairDepth under its nosing line, so no post stands over 1.5 m (Marlow 15). The draw:
//    both banks filled bankH over the tread within bankRun m (laid back about 30 degrees), falling at bankFallDeg beyond, from drawX0 to
//    drawX1 only, so the fill stays clear of Band_S_W and IW2 (Marlow 22, Wren condition 4). Objects standing on reshaped ground move with it.
// 2. Paint: the trail layer along the new line (as 8.3: 0.9 m half width, 0.4 m blend); the rock layer on every reshaped cell steeper
//    than rockDeg.
// 3. Clear: in every flight and landing corridor (plus clearMargin) the old climb pieces: ClimbRim and ClimbRing rock triangles, rim
//    boulders, climb ground dressing, stops, forest; the terrain's detail cover.
// 4. Trail: Trails/J to Ward keeps J to the shelf point and the lookout to the ledge; the new points between, and the end moved to the prow.
// 5. Pieces (Ward/WardPath): crib logs on the downhill edge of flights 1 and 2; flight 3 treads, stringers, posts and a StairRamp; the
//    landing decks and the lookout floor, each with a rail on its open sides; StairRamps on every join over 0.1 m; three lanterns (stair
//    foot, landing 1, landing 2) moved from the old P2 and P3 and copied from P4; the bent fir over landing 1's corner (the hide), the split
//    snag at the throat; the fallen giant in two runs (leg 4 and the bench, Marlow 16), its colliders ringed 1.3 m over the ground, ends
//    buried in the faces; the prow with its rail round all three open sides, joined to the lip (Marlow 14); old P2 and P3 pieces removed.
// 6. Warps: Ward_P3 becomes Ward_Stair (landing 2), Ward_P4 becomes Ward_Lookout.
// The climb's old recipe parts this replaces are noted in Tools/Recipes/Archive/README_8_20.md.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
var wardGo = kit.Root("Ward"); if (wardGo == null) return "run 8.7 first";
var trails = kit.Root("Trails"); if (trails == null) return "no Trails";
var legT = trails.transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
UnityEngine.Physics.SyncTransforms();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
float L(float a, float b, float t) => a + (b - a) * t;
var terrain = kit.Terrain; var data = terrain.terrainData; var tOrg = terrain.transform.position; var size = data.size;
int res = data.heightmapResolution; float cellX = size.x / (res - 1), cellZ = size.z / (res - 1);
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + tOrg.y;
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t) { var ab = b - a; t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f)); return UnityEngine.Vector2.Distance(p, a + ab * t); }

// ---- the route (world metres; heights are tread tops)
var shelfJoin = P(54.4f, 244.5f);
var f1A = P(44.5f, 246f); var f1B = P(37.2f, 261f); const float f1HA = 35.8f, f1HB = 44f;               // flight 1, graded earth, 26 degrees
var l1Min = P(34f, 260f); var l1Max = P(38f, 264f); const float l1H = 44f;                             // landing 1
var f2A = P(34.9f, 260.5f); var f2B = P(34.0f, 249f); const float f2HA = 44f, f2HB = 49f;              // flight 2, graded earth, 23 degrees
var l2Min = P(32.6f, 246f); var l2Max = P(35.5f, 249.5f); const float l2H = 49f;                       // landing 2 (its west side is flight 3's foot)
var l2sMin = P(30.4f, 244.6f); var l2sMax = P(35.5f, 246.2f);                                         // and its south strip: flight 3 is climbed from its foot, not its side
var f3A = P(31.5f, 246.2f); var f3B = P(29.6f, 260.3f); const float f3HA = 49f, f3HB = 60f;            // flight 3, timber stair, 37 degrees
var lookMin = P(27f, 258.5f); var lookMax = P(31.2f, 264f);                                            // the lookout floor, flush with the ground
var lookC = P(29.2f, 261.2f);
var gullyMin = P(25.5f, 252.5f); var gullyMax = P(29.3f, 257.5f); const float gullyH = 55.5f;
const float earthHalf = 1.25f, stairHalf = 1.0f, stairDepth = 0.35f, fillDeg = 60f, cutDeg = 70f, reshapeReach = 12f, rockDeg = 40f, lookKeep = 1f;
// the draw's banks (WardPath 3, Marlow 22)
const float drawX0 = 54f, drawX1 = 74f, drawHalf = 1.5f, bankH = 3.5f, bankRun = 6f, bankFallDeg = 42f;
// the region kept for reruns (heights and splat)
const float regX0 = 20f, regX1 = 82f, regZ0 = 196f, regZ1 = 276f;
const string basePath = "Assets/Terrain/Main3/WardPathBase.bytes";

// ---- 0. the base heights: kept on the first run, restored on every rerun
int hx0 = UnityEngine.Mathf.FloorToInt((regX0 - tOrg.x) / cellX), hx1 = UnityEngine.Mathf.CeilToInt((regX1 - tOrg.x) / cellX), hz0 = UnityEngine.Mathf.FloorToInt((regZ0 - tOrg.z) / cellZ), hz1 = UnityEngine.Mathf.CeilToInt((regZ1 - tOrg.z) / cellZ);
int hw = hx1 - hx0 + 1, hh = hz1 - hz0 + 1;
int ares = data.alphamapResolution; float aX = size.x / ares, aZ = size.z / ares; int layersN = data.alphamapLayers;
int ax0 = UnityEngine.Mathf.FloorToInt((regX0 - tOrg.x) / aX), ax1 = UnityEngine.Mathf.CeilToInt((regX1 - tOrg.x) / aX), az0 = UnityEngine.Mathf.FloorToInt((regZ0 - tOrg.z) / aZ), az1 = UnityEngine.Mathf.CeilToInt((regZ1 - tOrg.z) / aZ);
int aw = ax1 - ax0 + 1, ah = az1 - az0 + 1;
var cur = data.GetHeights(hx0, hz0, hw, hh);   // what stands now (the base, or the last run's reshape)
float[,] baseH; float[,,] baseA; bool firstRun = !System.IO.File.Exists(basePath);
if (firstRun)
{
    baseH = cur; baseA = data.GetAlphamaps(ax0, az0, aw, ah);
    using (var bw = new System.IO.BinaryWriter(System.IO.File.Create(basePath)))
    {
        bw.Write(hx0); bw.Write(hz0); bw.Write(hw); bw.Write(hh); for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++) bw.Write(baseH[z, x]);
        bw.Write(ax0); bw.Write(az0); bw.Write(aw); bw.Write(ah); bw.Write(layersN); for (int z = 0; z < ah; z++) for (int x = 0; x < aw; x++) for (int k = 0; k < layersN; k++) bw.Write(baseA[z, x, k]);
    }
    UnityEditor.AssetDatabase.ImportAsset(basePath);
}
else
{
    using (var br = new System.IO.BinaryReader(System.IO.File.OpenRead(basePath)))
    {
        if (br.ReadInt32() != hx0 || br.ReadInt32() != hz0 || br.ReadInt32() != hw || br.ReadInt32() != hh) return "WardPathBase.bytes is for another region; delete it only on a fresh Main3";
        baseH = new float[hh, hw]; for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++) baseH[z, x] = br.ReadSingle();
        if (br.ReadInt32() != ax0 || br.ReadInt32() != az0 || br.ReadInt32() != aw || br.ReadInt32() != ah || br.ReadInt32() != layersN) return "WardPathBase.bytes splat block does not match";
        baseA = new float[ah, aw, layersN]; for (int z = 0; z < ah; z++) for (int x = 0; x < aw; x++) for (int k = 0; k < layersN; k++) baseA[z, x, k] = br.ReadSingle();
    }
}
float WX(int x) => tOrg.x + (hx0 + x) * cellX; float WZ(int z) => tOrg.z + (hz0 + z) * cellZ;
float ToY(float n) => n * size.y + tOrg.y; float ToN(float y) => UnityEngine.Mathf.Clamp01((y - tOrg.y) / size.y);

// ---- 1. the reshape: pieces as (centre line a to b with heights, or a rectangle), each with a half width and a depth under its tread
var pieces = new System.Collections.Generic.List<(string n, UnityEngine.Vector2 a, UnityEngine.Vector2 b, float ha, float hb, float half, float depth, bool rect)>
{
    ("Flight1", f1A, f1B, f1HA, f1HB, earthHalf, 0f, false),
    ("Landing1", l1Min, l1Max, l1H, l1H, 0f, 0f, true),
    ("Flight2", f2A, f2B, f2HA, f2HB, earthHalf, 0f, false),
    ("Landing2", l2Min, l2Max, l2H, l2H, 0f, 0f, true),
    ("Landing2S", l2sMin, l2sMax, l2H, l2H, 0f, 0f, true),
    ("Flight3", f3A, f3B, f3HA, f3HB, stairHalf, stairDepth, false),
    ("GullyFill", gullyMin, gullyMax, gullyH, gullyH, 0f, 0f, true),   // the old gully west of flight 3, which the stair's fill closed into a basin (hand walk TRAPS 2026-10-01): a shelf level with the flight's side
};
// distance outside a piece (0 inside) and its tread height there
float Out(int i, UnityEngine.Vector2 p, out float tread)
{
    var pc = pieces[i];
    if (pc.rect) { float dx = UnityEngine.Mathf.Max(pc.a.x - p.x, 0f, p.x - pc.b.x), dz = UnityEngine.Mathf.Max(pc.a.y - p.y, 0f, p.y - pc.b.y); tread = pc.ha; return UnityEngine.Mathf.Sqrt(dx * dx + dz * dz); }
    float d = SegD(p, pc.a, pc.b, out float t); tread = L(pc.ha, pc.hb, t); return UnityEngine.Mathf.Max(0f, d - pc.half);
}
var nh = new float[hh, hw]; for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++) nh[z, x] = ToY(baseH[z, x]);
float tanFill = UnityEngine.Mathf.Tan(fillDeg * UnityEngine.Mathf.Deg2Rad), tanCut = UnityEngine.Mathf.Tan(cutDeg * UnityEngine.Mathf.Deg2Rad), tanBank = UnityEngine.Mathf.Tan(bankFallDeg * UnityEngine.Mathf.Deg2Rad);
// the draw banks first (the stair is far from them)
var drawPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform t in legT) { var q = t.position; if (q.x >= drawX0 - 2f && q.x <= drawX1 + 2f && q.z < 216.5f) drawPts.Add(q); }
for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++)
{
    var p = P(WX(x), WZ(z)); if (p.x < drawX0 || p.x > drawX1 || drawPts.Count < 2) continue;
    float best = float.MaxValue, tread = 0f;
    for (int i = 1; i < drawPts.Count; i++) { float d = SegD(p, P(drawPts[i - 1].x, drawPts[i - 1].z), P(drawPts[i].x, drawPts[i].z), out float t); if (d < best) { best = d; tread = L(drawPts[i - 1].y, drawPts[i].y, t); } }
    if (best <= drawHalf) continue;
    float crest = tread + bankH, target = best <= drawHalf + bankRun ? L(tread, crest, (best - drawHalf) / bankRun) : crest - (best - drawHalf - bankRun) * tanBank;
    nh[z, x] = UnityEngine.Mathf.Max(nh[z, x], target);
}
// the stair: fill, then cut, then each cell's tread from its nearest piece
for (int pass = 0; pass < 3; pass++)
    for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++)
    {
        var p = P(WX(x), WZ(z)); int nearI = -1; float nearD = float.MaxValue, nearT = 0f;
        bool keepLook = p.x >= lookMin.x - lookKeep && p.x <= lookMax.x + lookKeep && p.y >= f3B.y && p.y <= lookMax.y + lookKeep;   // the lookout floor is the ground as it stands: no cut under it (flight 2's 70 degree cut took 3 m off its east edge)
        for (int i = 0; i < pieces.Count; i++)
        {
            float d = Out(i, p, out float tr); if (d > reshapeReach) continue; float floor = tr - pieces[i].depth;
            if (pass == 0) nh[z, x] = UnityEngine.Mathf.Max(nh[z, x], floor - d * tanFill);
            else if (pass == 1) { if (!keepLook) nh[z, x] = UnityEngine.Mathf.Min(nh[z, x], floor + d * tanCut); }
            else if (d < nearD) { nearD = d; nearI = i; nearT = floor; }
        }
        if (pass == 2 && nearI >= 0 && nearD <= 0f && !(keepLook && pieces[nearI].depth > 0f)) nh[z, x] = nearT;   // the stair's top meets the lookout ground as it stands (a 0.35 m step down stalled the walk back, 2026-10-01)
    }
// the lookout: flush with the ground (no reshape); objects standing on reshaped ground move with it (by the change since the last run)
var outH = new float[hh, hw]; int reshaped = 0; for (int z = 0; z < hh; z++) for (int x = 0; x < hw; x++) { outH[z, x] = ToN(nh[z, x]); if (UnityEngine.Mathf.Abs(outH[z, x] - cur[z, x]) * size.y > 0.02f) reshaped++; }
float DeltaAt(float wx, float wz) { int x = UnityEngine.Mathf.RoundToInt((wx - tOrg.x) / cellX) - hx0, z = UnityEngine.Mathf.RoundToInt((wz - tOrg.z) / cellZ) - hz0; if (x < 0 || z < 0 || x >= hw || z >= hh) return 0f; return (outH[z, x] - cur[z, x]) * size.y; }
int movedObjs = 0;
foreach (var root in new[] { "Forest", "Ground815", "SliceLook", "Giants", "PointsOfInterest" })
{
    var r = kit.Root(root); if (r == null) continue;
    foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>())
    {
        if (!UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject)) continue;
        var q = t.position; if (q.x < regX0 || q.x > regX1 || q.z < regZ0 || q.z > regZ1) continue;
        float dy = DeltaAt(q.x, q.z); if (UnityEngine.Mathf.Abs(dy) < 0.05f) continue;
        t.position = q + V(0f, dy, 0f); movedObjs++;
    }
}
data.SetHeights(hx0, hz0, outH);

// ---- 2. paint: back to the base splat, then the trail layer along the new line and the rock layer on steep reshaped cells
int trailLayer = -1, rockLayer = -1; for (int k = 0; k < data.terrainLayers.Length; k++) { if (data.terrainLayers[k].name == "Layer_Trail") trailLayer = k; if (data.terrainLayers[k].name == "Layer_Rock") rockLayer = k; }
if (trailLayer < 0 || rockLayer < 0) return "no Layer_Trail or Layer_Rock";
var alpha = (float[,,])baseA.Clone();
const float trailHalf = 0.9f, trailBlend = 0.4f;
var route = new System.Collections.Generic.List<UnityEngine.Vector3>();   // the new points (filled in section 4)
UnityEngine.Vector3 R3(UnityEngine.Vector2 p, float h) => V(p.x, h, p.y);
void Line(UnityEngine.Vector3 a, UnityEngine.Vector3 b, float step) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(new UnityEngine.Vector3(a.x, 0f, a.z), new UnityEngine.Vector3(b.x, 0f, b.z)) / step)); for (int i = 1; i <= n; i++) route.Add(UnityEngine.Vector3.Lerp(a, b, i / (float)n)); }
var shelf3 = R3(shelfJoin, H(shelfJoin.x, shelfJoin.y));
route.Add(shelf3);
Line(shelf3, R3(f1A, f1HA), 2f); Line(R3(f1A, f1HA), R3(f1B, f1HB), 2f);
Line(R3(f1B, f1HB), R3((l1Min + l1Max) * 0.5f, l1H), 2f); Line(R3((l1Min + l1Max) * 0.5f, l1H), R3(f2A, f2HA), 2f);
Line(R3(f2A, f2HA), R3(f2B, f2HB), 2f); var f3Foot = P(f3A.x, (l2sMin.y + l2sMax.y) * 0.5f); Line(R3(f2B, f2HB), R3((l2Min + l2Max) * 0.5f, l2H), 2f); Line(R3((l2Min + l2Max) * 0.5f, l2H), R3(f3Foot, l2H), 2f); Line(R3(f3Foot, l2H), R3(f3A, f3HA), 2f);
Line(R3(f3A, f3HA), R3(f3B, f3HB), 2f); Line(R3(f3B, f3HB), R3(lookC, H(lookC.x, lookC.y)), 2f);
for (int z = 0; z < ah; z++) for (int x = 0; x < aw; x++)
{
    var p = P(tOrg.x + (ax0 + x + 0.5f) * aX, tOrg.z + (az0 + z + 0.5f) * aZ); float d = float.MaxValue;
    for (int i = 1; i < route.Count; i++) d = UnityEngine.Mathf.Min(d, SegD(p, P(route[i - 1].x, route[i - 1].z), P(route[i].x, route[i].z), out _));
    float w = 1f - UnityEngine.Mathf.Clamp01((d - trailHalf) / trailBlend);
    int hxI = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt((p.x - tOrg.x) / cellX) - hx0, 0, hw - 1), hzI = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt((p.y - tOrg.z) / cellZ) - hz0, 0, hh - 1);
    bool changed = UnityEngine.Mathf.Abs(outH[hzI, hxI] - baseH[hzI, hxI]) * size.y > 0.05f;
    float steep = data.GetSteepness((p.x - tOrg.x) / size.x, (p.y - tOrg.z) / size.z);
    int layer = w > 0f ? trailLayer : changed && steep > rockDeg ? rockLayer : -1; if (w <= 0f) w = layer >= 0 ? 1f : 0f; if (layer < 0) continue;
    for (int k = 0; k < layersN; k++) alpha[z, x, k] *= 1f - w; alpha[z, x, layer] += w;
}
data.SetAlphamaps(ax0, az0, alpha);
UnityEditor.EditorUtility.SetDirty(data);

// ---- 3. clear the old climb in every corridor
const float clearMargin = 0.6f;
bool InCorridor(UnityEngine.Vector3 q, float extra) { var p = P(q.x, q.z); for (int i = 0; i < pieces.Count; i++) { float d = Out(i, p, out float tr); if (d <= clearMargin + extra && q.y > tr - 3f && q.y < tr + 6f) return true; } return p.x >= lookMin.x - extra && p.x <= lookMax.x + extra && p.y >= lookMin.y - extra && p.y <= lookMax.y + extra && q.y > 55f; }
int trisCut = 0;
UnityEngine.Mesh CutMesh(UnityEngine.Mesh m, UnityEngine.Transform t, System.Func<UnityEngine.Vector3, bool> drop, string assetPath)
{
    var vs = m.vertices; var keep = new System.Collections.Generic.List<int>(); var tris = m.triangles;
    for (int i = 0; i < tris.Length; i += 3) { var c = t.TransformPoint((vs[tris[i]] + vs[tris[i + 1]] + vs[tris[i + 2]]) / 3f); if (drop(c)) { trisCut++; continue; } keep.Add(tris[i]); keep.Add(tris[i + 1]); keep.Add(tris[i + 2]); }
    if (keep.Count == tris.Length) return m;
    var nm = UnityEngine.Object.Instantiate(m); nm.name = System.IO.Path.GetFileNameWithoutExtension(assetPath); nm.subMeshCount = 1; nm.SetTriangles(keep, 0); nm.RecalculateBounds();
    UnityEditor.AssetDatabase.DeleteAsset(assetPath); UnityEditor.AssetDatabase.CreateAsset(nm, assetPath); return nm;
}
var ring = kit.Root("Rock") != null ? kit.Root("Rock").transform.Find("ClimbRing") : null;
if (ring != null)
{
    var rc = ring.Find("ClimbRim_Collider"); if (rc != null) { var mc = rc.GetComponent<UnityEngine.MeshCollider>(); if (mc != null) { var nm = CutMesh(mc.sharedMesh, rc, c => InCorridor(c, 0f), "Assets/Terrain/Main3/ClimbRim_WardPath.asset"); mc.sharedMesh = null; mc.sharedMesh = nm; } }
    var rr = ring.Find("ClimbRing_Rock"); if (rr != null) { var mf = rr.GetComponent<UnityEngine.MeshFilter>(); if (mf != null) { var nm = CutMesh(mf.sharedMesh, rr, c => InCorridor(c, 0f), "Assets/Terrain/Main3/ClimbRingRock_WardPath.asset"); mf.sharedMesh = nm; var mc2 = rr.GetComponent<UnityEngine.MeshCollider>(); if (mc2 != null) { mc2.sharedMesh = null; mc2.sharedMesh = nm; } } }
}
int removed = 0;
void ClearUnder(UnityEngine.Transform root, float extra)
{
    if (root == null) return;
    foreach (var t in root.GetComponentsInChildren<UnityEngine.Transform>(true))
    {
        if (t == null || t == root) continue;
        bool unit = UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject) || (t.parent == root || t.parent.parent == root) && t.GetComponent<UnityEngine.Renderer>() != null;
        if (!unit) continue;
        var r = t.GetComponentInChildren<UnityEngine.Renderer>(); var c = r != null ? r.bounds.center : t.position; var lo = r != null ? r.bounds.min : t.position;
        if (InCorridor(V(c.x, lo.y, c.z), extra) || InCorridor(t.position, extra)) { UnityEngine.Object.DestroyImmediate(t.gameObject); removed++; }
    }
}
if (ring != null) ClearUnder(ring.Find("RimBoulders"), 0.5f);
var g815 = kit.Root("Ground815"); if (g815 != null) foreach (var n in new[] { "ClimbGrounds", "Stops", "TrailEdges" }) ClearUnder(g815.transform.Find(n), 0.5f);
ClearUnder(kit.Root("Forest") != null ? kit.Root("Forest").transform : null, 1f);
int detailCleared = 0; foreach (var q in route) detailCleared += kit.ClearDetail(q, 2f);
foreach (var n in new[] { "P2_RockRoof", "P3_RootPlate", "P1_Overhang_Old" }) { var t = wardGo.transform.Find("Climb/" + n); if (t != null) { PlaceKit.Remove(t); removed++; } }

// ---- 4. the trail: J to the shelf point, the new points, the lookout on (from the first old point west of x 25.5 at z 261 to 263)
var oldPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform t in legT) oldPts.Add(t.position);
int cutA = oldPts.FindIndex(q => q.x > 50f && q.z >= shelfJoin.y - 0.01f); int cutB = oldPts.FindIndex(q => q.x < 25.5f && q.z > 261f && q.z < 263f);
if (cutA < 0 || cutB < 0 || cutB < cutA) return "J to Ward: shelf point " + cutA + " or lookout point " + cutB + " not found";
const float prowX0 = -12.2f, prowX1 = -10f, prowZ0 = 243.4f, prowZ1 = 248.6f, ledgeY = 62f, prowEye = -11.5f;
var newPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (int i = 0; i < cutA; i++) newPts.Add(oldPts[i]);
newPts.AddRange(route);
for (int i = cutB; i < oldPts.Count; i++) { var q = oldPts[i]; if (q.x < -7f) break; newPts.Add(q); }   // the ledge points to the old path end
newPts.Add(V(-8.5f, ledgeY, 246f)); newPts.Add(V(-10.5f, ledgeY, 246f)); newPts.Add(V(prowEye, ledgeY, 246f));   // on to the prow
for (int i = legT.childCount - 1; i >= 0; i--) UnityEngine.Object.DestroyImmediate(legT.GetChild(i).gameObject);
{ float s = 0f; for (int i = 0; i < newPts.Count; i++) { if (i > 0) s += UnityEngine.Vector3.Distance(newPts[i - 1], newPts[i]); var g = new UnityEngine.GameObject("P" + UnityEngine.Mathf.RoundToInt(s)); g.transform.SetParent(legT, false); g.transform.position = newPts[i]; } }   // named by chainage, as 8.3 does

// ---- 5. pieces
var wp = kit.Fresh("WardPath", wardGo.transform, V(40f, 40f, 250f), 0f);
const string planks = "Assets/Materials/Planks023A_1.0x1.0.mat";
var plankMat = kit.Tinted("Places_WardPlank", planks, Hex("#6B5540"), new UnityEngine.Vector2(1f, 3f));
string LogPath = PlaceKit.CS + "Wood/CS_Log_Large_Long", PlankPath = PlaceKit.CC + "Props/C_Plank_A_Thick", PostPath = PlaceKit.CE + "Building_Parts/RailingPost_Wood";
const float stairRampThick = 0.2f, railH = 1.0f, railCol = 1.1f, railBar = 0.1f, postStep = 2f, cribR = 0.25f, stepRun = 0.3f;
// a collider-only ramp whose top face runs from a to b (both on the walking surface), w wide (the STAIRS RULE, 8.7)
void Ramp(string name, UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float w)
{
    var along = b - a; var side = V(along.z, 0f, -along.x).normalized; var up = UnityEngine.Vector3.Cross(along, side).normalized; if (up.y < 0f) up = -up;
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false); g.transform.SetPositionAndRotation((a + b) * 0.5f - up * stairRampThick * 0.5f, UnityEngine.Quaternion.LookRotation(along.normalized, up));
    g.AddComponent<UnityEngine.BoxCollider>().size = V(w, stairRampThick, along.magnitude);
}
// a straight piece of a prefab from a to b (its long axis along the line), girth g
UnityEngine.GameObject Along(string path, UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float g)
{
    var go = kit.Spawn(path, parent); if (go == null) return null; PlaceKit.StripColliders(go);
    go.transform.rotation = UnityEngine.Quaternion.identity; go.transform.localScale = UnityEngine.Vector3.one; var bb = PlaceKit.MeshBounds(go);
    int ax = bb.size.x >= bb.size.y && bb.size.x >= bb.size.z ? 0 : bb.size.y >= bb.size.z ? 1 : 2; var along = b - a; float len = along.magnitude;
    var sc = V(g / UnityEngine.Mathf.Max(0.01f, bb.size.x), g / UnityEngine.Mathf.Max(0.01f, bb.size.y), g / UnityEngine.Mathf.Max(0.01f, bb.size.z)); sc[ax] = len / UnityEngine.Mathf.Max(0.01f, bb.size[ax]); go.transform.localScale = sc;
    var axis = ax == 0 ? UnityEngine.Vector3.right : ax == 1 ? UnityEngine.Vector3.up : UnityEngine.Vector3.forward;
    go.transform.rotation = UnityEngine.Quaternion.FromToRotation(axis, along / len);
    bb = PlaceKit.MeshBounds(go); go.transform.position += (a + b) * 0.5f - bb.center; return go;
}
// a rail along a to b (posts every postStep m, a top bar at railH) with a collider railCol high; a and b on the walking surface
void Rail(string name, UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b)
{
    var g = kit.Group(name, parent, a, 0f); var along = b - a; float len = new UnityEngine.Vector2(along.x, along.z).magnitude; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / postStep));
    for (int i = 0; i <= n; i++) { var q = UnityEngine.Vector3.Lerp(a, b, i / (float)n); kit.Fill(PostPath, g, g.InverseTransformPoint(q), V(0.12f, railH, 0.12f)); }
    var bar = Along(LogPath, g, a + UnityEngine.Vector3.up * railH, b + UnityEngine.Vector3.up * railH, railBar);
    var col = new UnityEngine.GameObject("RailCollider"); col.transform.SetParent(g, false); var mid = (a + b) * 0.5f + UnityEngine.Vector3.up * railCol * 0.5f;
    col.transform.SetPositionAndRotation(mid, UnityEngine.Quaternion.LookRotation(V(along.x, 0f, along.z).normalized)); col.AddComponent<UnityEngine.BoxCollider>().size = V(0.15f, railCol + UnityEngine.Mathf.Abs(along.y), len);
}
// flights 1 and 2: graded earth with crib logs on the downhill edge, a lantern at the foot; the downhill side is the one with lower ground
int cribs = 0;
void Cribs(string name, UnityEngine.Vector2 a, UnityEngine.Vector2 b, float ha, float hb)
{
    var g = kit.Group(name, wp, R3(a, ha), 0f); var dir = (b - a).normalized; var side = P(dir.y, -dir.x); float len = UnityEngine.Vector2.Distance(a, b);
    var mid = (a + b) * 0.5f; var qL = mid - side * (earthHalf + 2f); var qR = mid + side * (earthHalf + 2f); float lowSide = H(qL.x, qL.y) < H(qR.x, qR.y) ? -1f : 1f;   // the side whose ground falls away
    for (float s = 0f; s < len - 0.5f; s += 3f)
    {
        float e = UnityEngine.Mathf.Min(len, s + 3f); var qa = a + dir * s + side * lowSide * (earthHalf - cribR); var qb = a + dir * e + side * lowSide * (earthHalf - cribR);
        Along(LogPath, g, R3(qa, L(ha, hb, s / len) + cribR * 0.6f), R3(qb, L(ha, hb, e / len) + cribR * 0.6f), cribR * 2f); cribs++;
    }
}
Cribs("Flight1", f1A, f1B, f1HA, f1HB); Cribs("Flight2", f2A, f2B, f2HA, f2HB);
// flight 3: treads on two stringers, posts down to the bench, a StairRamp on the nosing line, a rail each side
{
    var g = kit.Group("Flight3", wp, R3(f3A, f3HA), 0f); var dir = (f3B - f3A).normalized; var side = P(dir.y, -dir.x); float len = UnityEngine.Vector2.Distance(f3A, f3B);
    int steps = UnityEngine.Mathf.CeilToInt(len / stepRun); float rise = (f3HB - f3HA) / steps, run = len / steps;
    for (int k = 0; k < steps; k++)
    {
        var q = f3A + dir * ((k + 0.5f) * run); var tr = kit.Fill(PlankPath, g, g.InverseTransformPoint(R3(q, f3HA + (k + 1) * rise - 0.06f)), V(stairHalf * 2f, 0.06f, run * 1.05f), UnityEngine.Mathf.Atan2(dir.x, dir.y) * UnityEngine.Mathf.Rad2Deg);
        if (tr != null) foreach (var r in tr.GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = plankMat;
    }
    foreach (var s in new[] { -1f, 1f })
    {
        bool east = side.x * s > 0f; float uS = east ? (l2Max.y + 0.3f - f3A.y) / UnityEngine.Mathf.Max(0.01f, dir.y) : 0f;   // the east stringer starts past landing 2, the way on
        var a = f3A + dir * uS + side * s * (stairHalf - 0.1f); var b = f3B + side * s * (stairHalf - 0.1f);
        Along(LogPath, g, R3(a, L(f3HA, f3HB, uS / len) - 0.2f), R3(b, f3HB - 0.2f), 0.25f);
        for (float u = 1f; u < len - uS; u += postStep) { var q = a + dir * u; float top = L(f3HA, f3HB, (u + uS) / len) - 0.3f, gy = H(q.x, q.y); if (top - gy > 0.15f) kit.Fill(PostPath, g, g.InverseTransformPoint(R3(q, gy)), V(0.15f, top - gy, 0.15f)); }
    }
    // one rail, on the downhill (east, flight 2) side, from past landing 2 (whose west edge is the way on) to the top; the west is the cut face
    { float u0 = (l2Max.y + 0.3f - f3A.y) / UnityEngine.Mathf.Max(0.01f, dir.y); var eastS = side.x > 0f ? 1f : -1f; var ra = f3A + dir * u0 + side * eastS * stairHalf; var rb = f3B + side * eastS * stairHalf; Rail("RailE", g, R3(ra, L(f3HA, f3HB, u0 / len)), R3(rb, f3HB)); }
    Ramp("StairRamp", g, R3(f3A - dir * run, f3HA), R3(f3B, f3HB), stairHalf * 2f);
}
// the landing decks: planks over the bench, flush, a rail on the open (downhill) sides
void Deck(string name, UnityEngine.Vector2 mn, UnityEngine.Vector2 mx, float h, params (UnityEngine.Vector2 a, UnityEngine.Vector2 b)[] rails)
{
    var g = kit.Group(name, wp, R3((mn + mx) * 0.5f, h), 0f);
    kit.Slab("Deck", g, V(0f, -0.03f, 0f), V(mx.x - mn.x, 0.06f, mx.y - mn.y), plankMat);
    int i = 0; foreach (var r in rails) Rail("Rail" + (i++), g, R3(r.a, h), R3(r.b, h));
}
// landing 1: flight 1 comes in over the south-east corner and flight 2 leaves from the south-west, so the rails run on the east side north
// of flight 1 and along the north side; landing 2: flight 2 comes in from the north, flight 3 leaves from the west, the south is the cut face
Deck("Landing1", l1Min, l1Max, l1H, (P(l1Max.x, f1B.y + earthHalf + 0.3f), P(l1Max.x, l1Max.y)), (P(l1Max.x, l1Max.y), P(l1Min.x, l1Max.y)));
Deck("Landing2", l2Min, l2Max, l2H, (P(l2Max.x, l2Min.y), P(l2Max.x, l2Max.y)));
Deck("Landing2S", l2sMin, l2sMax, l2H, (P(l2sMax.x, l2sMin.y), P(l2sMax.x, l2sMax.y)));
{   // the lookout floor: planks on the ground, a rail on the east (the face) and south edges
    var g = kit.Group("Lookout", wp, R3(lookC, H(lookC.x, lookC.y)), 0f);
    for (float z = lookMin.y + 0.15f; z < lookMax.y; z += 0.3f) { float gy = UnityEngine.Mathf.Max(H(lookMin.x, z), H(lookMax.x, z), H(lookC.x, z)); kit.Slab("Plank", g, g.InverseTransformPoint(V(lookC.x, gy + 0.02f, z)), V(lookMax.x - lookMin.x, 0.05f, 0.28f), plankMat); }
    Rail("RailE", g, V(lookMax.x, H(lookMax.x, f3B.y + 1.2f), f3B.y + 1.2f), V(lookMax.x, H(lookMax.x, lookMax.y), lookMax.y));
}
// joins over 0.1 m: shelf to flight 1, flight 1 to landing 1, landing 1 to flight 2, flight 2 to landing 2 are graded earth (none); the
// lookout planks sit 0.02 to 0.07 m over the ground (none); flight 3's foot and top are covered by its StairRamp, which starts one run
// before the first tread
// lanterns (Quill: one at the stair foot and on both landings): copies of the P4 lantern (8.7, flame and glow); the old P2 and P3 ones go
var climbRoot = wardGo.transform.Find("Climb"); if (climbRoot == null) return "no Ward/Climb";
var p4Lantern = climbRoot.Find("P4Lantern"); if (p4Lantern == null) return "no Ward/Climb/P4Lantern";
foreach (var n in new[] { "P2Lantern", "P3Lantern", "StairFootLantern", "Landing1Lantern", "Landing2Lantern" }) PlaceKit.Remove(climbRoot.Find(n));
void Lantern(string name, UnityEngine.Vector3 at) { var t = UnityEngine.Object.Instantiate(p4Lantern.gameObject, climbRoot).transform; t.name = name; var b = PlaceKit.MeshBounds(t.gameObject); t.position += at - V(b.center.x, b.min.y, b.center.z); }
Lantern("StairFootLantern", V(f1A.x + 1.5f, H(f1A.x + 1.5f, f1A.y - 1.5f), f1A.y - 1.5f));
Lantern("Landing1Lantern", V(l1Max.x - 0.4f, l1H, l1Max.y - 0.4f));
Lantern("Landing2Lantern", V(l2Max.x - 0.4f, l2H, l2Min.y + 0.4f));
// the bent fir across landing 1's corner (the hide under its overhang) and the split snag at the throat (WardPath 1, 4.2)
var bent = climbRoot != null ? climbRoot.Find("RedFir_Bent") : null; if (bent != null) { var b = PlaceKit.MeshBounds(bent.gameObject); bent.position += V(l1Min.x + 0.3f - b.center.x, H(l1Min.x + 0.3f, l1Max.y + 0.6f) - b.min.y - 0.2f, l1Max.y + 0.6f - b.center.z); }
foreach (var (n, dx) in new[] { ("SplitSnag_L", -0.6f), ("SplitSnag_R", 0.6f) }) { var s = climbRoot != null ? climbRoot.Find(n) : null; if (s == null) continue; var b = PlaceKit.MeshBounds(s.gameObject); float sx = 25.5f, sz = 262f + dx * 2.2f; s.position += V(sx - b.center.x, H(sx, sz) - b.min.y - 0.3f, sz - b.center.z); }
// the fallen giant across z 272 (WardPath 3.2, Marlow 16): broken in two where it struck the face. Run A across leg 4's tread (x 20.5 to
// 31, root plate up at the crest face), run B on the bench (x 31.5 to 59.5, its butt buried in the face foot, its top on the valley rim).
// Each run: a capsule giantR m round lying on the ground under its middle, so it stands 2 giantR - giantSink over the ground on both sides
const float giantZ = 272f, giantR = 1.3f, giantColR = 1.5f, giantSink = 0.3f, benchEndX = 63f, tieX0 = 55.5f, tieX1 = 62f, leg4X0 = 22f, leg4X1 = 29f, tieHalfZ = 1.2f, tieOver = 1.4f, trunkBand = 0.03f, buryEnds = 2f;   // buryEnds: the drawn trunk runs this much further into the faces
var giant = kit.Group("FallenGiant", wp, V(40f, 41f, giantZ), 0f); int giantRuns = 0;
foreach (var (n, x0, x1, path) in new[] { ("RunLeg4", 20.5f, 31f, PlaceKit.BK + "Trees/Sequoia2"), ("RunBench", 31.5f, benchEndX, PlaceKit.BK + "Trees/Sequoia4") })
{
    float gy = float.MaxValue; for (float x = x0 + 2f; x <= UnityEngine.Mathf.Min(x1, 58f) - 2f; x += 1f) gy = UnityEngine.Mathf.Min(gy, H(x, giantZ)); float cy = gy + giantColR - giantSink;   // the bench end runs on out over the east drop
    var run = kit.Group(n, giant, V((x0 + x1) * 0.5f, cy, giantZ), 0f);
    // the collider: on the bench a level capsule at cy; on leg 4 (the ground falls from the crest face to the tread and over the face edge)
    // a capsule tilted along the ground line between its ends, so the face foot at x 22 never stands over it (hand walk 2026-10-01: a box
    // at the root plate left a ledge on the trunk the player stuck on)
    var colGo = new UnityEngine.GameObject("Collider"); colGo.transform.SetParent(run, false); var cap = colGo.AddComponent<UnityEngine.CapsuleCollider>(); cap.direction = 0; cap.radius = giantColR;
    if (n == "RunLeg4")   // from the face foot to the tread's east edge
    {
        var ea = V(leg4X0, H(leg4X0, giantZ) + giantColR - giantSink, giantZ); var eb = V(leg4X1, H(leg4X1, giantZ) + giantColR - giantSink, giantZ);
        colGo.transform.SetPositionAndRotation((ea + eb) * 0.5f, UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, (eb - ea).normalized)); cap.height = UnityEngine.Vector3.Distance(ea, eb) + 2f * giantColR;
    }
    else cap.height = x1 - x0;   // giantColR: its top 1.3 m over the shelf rim the player can stand on (closure check)
    var tree = kit.Spawn(path, run); if (tree != null)
    {
        PlaceKit.StripColliders(tree); tree.transform.rotation = UnityEngine.Quaternion.identity; tree.transform.localScale = UnityEngine.Vector3.one;
        var b = PlaceKit.MeshBounds(tree);
        // the trunk's width at its base: the LOD0 vertices in the lowest trunkBand of the tree's height (the crown is far wider)
        float baseW = 0f; { var lod = tree.GetComponentInChildren<UnityEngine.LODGroup>(); var r0 = lod != null ? lod.GetLODs()[0].renderers[0] : tree.GetComponentInChildren<UnityEngine.Renderer>(); var mf0 = r0 != null ? r0.GetComponent<UnityEngine.MeshFilter>() : null;
            if (mf0 != null) { float x0b = float.MaxValue, x1b = float.MinValue; foreach (var v in mf0.sharedMesh.vertices) { var w = mf0.transform.TransformPoint(v); if (w.y > b.min.y + b.size.y * trunkBand) continue; x0b = UnityEngine.Mathf.Min(x0b, w.x); x1b = UnityEngine.Mathf.Max(x1b, w.x); } baseW = x1b - x0b; } }
        float girth = 2f * giantR / UnityEngine.Mathf.Max(0.3f, baseW);
        tree.transform.localScale = V(girth, (x1 - x0 + buryEnds) / b.size.y, girth);
        tree.transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, -90f);   // its up axis east: the butt (root plate) at the west end
        tree.transform.position = V(x0 - buryEnds * 0.5f, cy, giantZ);   // the pack trees stand on their pivot at the trunk's base centre
    }
    giantRuns++;
}
// the ends tied in (WardPath 3.2; closure check 2026-10-01: from the shelf rim a sprint-jump cleared the trunk at x 59, and on leg 4 the
// face foot at x 22 stands over the trunk): at the rim (x tieX0 to tieX1) and at the root plate on the crest face (x rootX0 to rootX1),
// a box across the trunk whose top is tieOver m over the highest rim or ground under it
void Tie(string name, float x0, float x1)
{
    float top = float.MinValue, low = float.MaxValue;
    for (float x = x0; x <= x1; x += 0.5f) for (float dz = -tieHalfZ; dz <= tieHalfZ; dz += 0.5f)
    {
        foreach (var th in UnityEngine.Physics.RaycastAll(V(x, 120f, giantZ + dz), UnityEngine.Vector3.down, 120f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!th.collider.transform.IsChildOf(giant)) top = UnityEngine.Mathf.Max(top, th.point.y);
        low = UnityEngine.Mathf.Min(low, H(x, giantZ + dz));
    }
    kit.Blocker(name, giant, giant.InverseTransformPoint(V((x0 + x1) * 0.5f, (low + top + tieOver) * 0.5f, giantZ)), V(x1 - x0, top + tieOver - low, 2f * tieHalfZ));
}
Tie("RimTie", tieX0, tieX1);
// the prow (WardPath 2.2, Marlow 14): a rock platform 2.2 m past the lip, top level with the ledge, a rail on its N, W and S sides joined
// to the lip, the lip opened between them
var prow = kit.Group("Prow", wp, V((prowX0 + prowX1) * 0.5f, ledgeY, 246f), 0f);
{
    var rock = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_1", prow); if (rock != null) { PlaceKit.StripColliders(rock); var b = PlaceKit.MeshBounds(rock); rock.transform.localScale = V((prowX1 - prowX0 + 1.2f) / b.size.x, 6f / b.size.y, (prowZ1 - prowZ0 + 0.6f) / b.size.z); b = PlaceKit.MeshBounds(rock); rock.transform.position += V((prowX0 + prowX1) * 0.5f - 0.3f - b.center.x, ledgeY - 0.05f - b.max.y, 246f - b.center.z); }
    var top = kit.Blocker("ProwTop", prow, prow.InverseTransformPoint(V((prowX0 + prowX1) * 0.5f, ledgeY - 2f, 246f)), V(prowX1 - prowX0 + 0.4f, 4f, prowZ1 - prowZ0));
    Rail("RailN", prow, V(prowX1, ledgeY, prowZ1), V(prowX0, ledgeY, prowZ1));
    Rail("RailW", prow, V(prowX0, ledgeY, prowZ1), V(prowX0, ledgeY, prowZ0));
    Rail("RailS", prow, V(prowX0, ledgeY, prowZ0), V(prowX1, ledgeY, prowZ0));
}
var ledgeRoot = kit.Root("Rock") != null ? kit.Root("Rock").transform.Find("Ledge/Lip_End") : null;
if (ledgeRoot != null)
{
    System.Func<UnityEngine.Vector3, bool> open = c => c.z > prowZ0 + 0.05f && c.z < prowZ1 - 0.05f;
    var mf = ledgeRoot.GetComponent<UnityEngine.MeshFilter>(); var mc = ledgeRoot.GetComponent<UnityEngine.MeshCollider>();
    if (mf != null) { var nm = CutMesh(mf.sharedMesh, ledgeRoot, open, "Assets/Terrain/Main3/LipEnd_WardPath.asset"); mf.sharedMesh = nm; if (mc != null) { mc.sharedMesh = null; mc.sharedMesh = nm; } }
}

// ---- 6. warps
var warps = kit.Root("DevWarps").transform;
void Warp(string oldName, string name, UnityEngine.Vector3 at, UnityEngine.Vector3 look)
{
    var w = warps.Find(name) ?? warps.Find(oldName); if (w == null) { w = new UnityEngine.GameObject(name).transform; w.SetParent(warps, false); }
    w.name = name; w.position = at + V(0f, 0.2f, 0f); var d = look - at; w.rotation = UnityEngine.Quaternion.LookRotation(V(d.x, 0f, d.z).normalized);
}
Warp("Ward_P3", "Ward_Stair", V((l2Min.x + l2Max.x) * 0.5f + 0.6f, l2H, (l2Min.y + l2Max.y) * 0.5f), V(164f, 58f, 166f));
Warp("Ward_P4", "Ward_Lookout", V(lookC.x, H(lookC.x, lookC.y), lookC.y), V(164f, 58f, 166f));

UnityEngine.Physics.SyncTransforms();
UnityEditor.AssetDatabase.SaveAssets();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
float gF3Max = 0f; { float len = UnityEngine.Vector2.Distance(f3A, f3B); for (float u = 0f; u <= len; u += 0.5f) { var q = UnityEngine.Vector2.Lerp(f3A, f3B, u / len); gF3Max = UnityEngine.Mathf.Max(gF3Max, L(f3HA, f3HB, u / len) - H(q.x, q.y)); } }
return "saved=" + saved + " | " + (firstRun ? "base kept" : "base restored") + "; cells reshaped " + reshaped + ", objects moved with the ground " + movedObjs + " | flight 3 nosing over its bench at most " + gF3Max.ToString("F2") + " m"
    + " | cleared " + removed + " pieces, " + trisCut + " rim and lip triangles, " + detailCleared + " detail | trail " + oldPts.Count + " to " + newPts.Count + " points | crib logs " + cribs + ", giant runs " + giantRuns + " | " + kit.Report();
