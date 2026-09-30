// Main3 task 8.9: tower sightline check (Main3.md 5.3, 5.7 W-1 and C-1, 4.1). Edit mode, Main3 open; changes nothing that
// is saved (temporary crown colliders are removed before it returns). Writes Docs/Layout/Main3/Main3_sightlines.md.
// Eyes: a 1 m grid over the 8 x 8 m deck (64 points) at eye height 57.6 and jump height 58.2, then both raised 3 m.
// Tower geometry is ignored (the player can stand anywhere on the deck).
// Places: a target is seen if a ray reaches it (a hit within 2 m of the target counts); margin = how far the best line
//   passes above anything under it (terrain, rock, buildings, giants with crowns), from the eye to 5 m short of the target.
// W-1 / C-1 (trees off): rays to every Ward stone corner and every cave mouth corner must hit the terrain (the Wall, rev 15; the rim for the cave); hidden
//   margin = how deep the least-hidden line passes under the blocking surface. Repeated with eyes and targets raised 3 m.
// 4.1: from each junction at 1.6 m eye height, is any part of the cab visible (trees on)?
// F-1 (8.14, Valley.md rev 10 section 7): rays from every place, trail point at 10 m, the daytime eyes of section 7 and the deck grid to every
// flame top and the day-one smoke sheet must hit terrain or solid rock (never trees); see its section.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>(); float baseY = terrain.transform.position.y;
float Hg(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + baseY;
var tower = Root("Camp").transform.Find("Tower"); var giants = Root("Giants").transform; var wardRoot = Root("Ward").transform;
bool wasDirty = scene.isDirty;
// temporary colliders on the crowns so trees block lines (they have none in the scene)
var temp = new System.Collections.Generic.List<UnityEngine.Component>();
foreach (var mf in giants.GetComponentsInChildren<UnityEngine.MeshFilter>()) if (mf.name == "Crown" && mf.GetComponent<UnityEngine.Collider>() == null) temp.Add(mf.gameObject.AddComponent<UnityEngine.MeshCollider>());
UnityEngine.Physics.SyncTransforms();
bool IsUnder(UnityEngine.Transform t, UnityEngine.Transform root) { for (var p = t; p != null; p = p.parent) if (p == root) return true; return false; }
// first hit along a segment that is not the tower (and, with trees off, not a giant)
bool FirstHit(UnityEngine.Vector3 a, UnityEngine.Vector3 b, bool treesOn, out UnityEngine.RaycastHit hit)
{
    var d = b - a; var hits = UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore);
    System.Array.Sort(hits, (p, q) => p.distance.CompareTo(q.distance));
    foreach (var h in hits) { if (IsUnder(h.collider.transform, tower)) continue; if (!treesOn && IsUnder(h.collider.transform, giants)) continue; hit = h; return true; }
    hit = default; return false;
}
// height of whatever is under (x, z): terrain, rock, buildings, and giants when trees are on
float Surface(float x, float z, bool treesOn)
{
    var hits = UnityEngine.Physics.RaycastAll(V(x, 200f, z), UnityEngine.Vector3.down, 260f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore);
    float best = -999f; foreach (var h in hits) { if (IsUnder(h.collider.transform, tower)) continue; if (!treesOn && IsUnder(h.collider.transform, giants)) continue; if (h.point.y > best) best = h.point.y; }
    return best;
}
// line clearance: min over samples of (line - surface), from 3 m after the eye to 'endSkip' short of the target
float Clearance(UnityEngine.Vector3 a, UnityEngine.Vector3 b, bool treesOn, float endSkip)
{
    float len = UnityEngine.Vector3.Distance(a, b), best = float.MaxValue;
    for (float s = 3f; s < len - endSkip; s += 1.5f) { var p = UnityEngine.Vector3.Lerp(a, b, s / len); best = UnityEngine.Mathf.Min(best, p.y - Surface(p.x, p.z, treesOn)); }
    return best;
}
var eyes = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (float gx = -3.5f; gx <= 3.5f; gx += 1f) for (float gz = -3.5f; gz <= 3.5f; gz += 1f) eyes.Add(new UnityEngine.Vector2(tower.position.x + gx, tower.position.z + gz));
// the deck top is the cab's floor (8.2 puts the Cab group at the deck top, 56). Before 8.14 this read tower.position.y + 48, right
// only while the tower stood at 8 (rev 13 raised the knoll to 15, so the checks since 8.9c looked from 63: 7 m high, stricter for
// F-1 and W-1)
float deckTop = tower.Find("Cab").position.y; var heights = new[] { ("eye", deckTop + 1.6f), ("jump", deckTop + 2.2f) };
var md = new System.Text.StringBuilder();
md.AppendLine("# Main3 tower sightlines (task 8.9)\n");
md.AppendLine("Generated by Tools/Recipes/main3_8_9_sightlines.cs from the gray blockout. Eyes: 1 m grid over the 8 x 8 m deck (64 points), eye 57.6 m and jump 58.2 m, then both raised 3 m. Tower geometry ignored. Margins in metres.\n");

// ---------------- places ----------------
var places = new (string place, (string n, UnityEngine.Vector3 p)[] targets)[] {
    ("Lake", new[] { ("far water (south shore)", V(190f, -5.4f, 34f)), ("mid water", V(190f, -5.4f, 60f)), ("near water", V(190f, -5.4f, 86f)), ("boathouse roof", V(240f, -1.0f, 52.4f)) }),
    ("Camp 1", new[] { ("spar top", V(282f, 29f, 238f)), ("spar at 20 m", V(282f, 20f, 238f)), ("tent", V(292f, Hg(292f, 247f) + 2f, 247f)) }),
    ("Camp 2", new[] { ("stack top", V(292f, 24.2f, 108f)), ("stack west edge", V(287.4f, 23.6f, 108f)), ("boulder field", V(274f, Hg(274f, 104f) + 1f, 104f)) }),
    ("Camp 3", new[] { ("Snag top", V(96f, 54f, 146.5f)), ("Snag at 40 m, east face", V(98.9f, 40f, 147.2f)) }),
    ("Office and lot", new[] { ("lot west edge", V(343.5f, 3.1f, 170f)), ("lot centre", V(358f, 3.1f, 170f)), ("resident's car", V(370f, 4.2f, 179.4f)), ("office roof", V(350f, 6.4f, 200f)), ("store roof", V(366f, 6.2f, 200f)), ("mast lamp", V(357f, 33.3f, 205f)) }),
};
md.AppendLine("## Places seen from the deck (trees on)\n");
md.AppendLine("| Place | Target | Seen from (eye) | Seen from (jump) | Best-line margin (eye) | Deck-centre margin (eye) |");
md.AppendLine("|---|---|---|---|---|---|");
var summary = new System.Text.StringBuilder(); bool allPlaces = true;
foreach (var pl in places)
{
    bool placeSeen = false;
    foreach (var t in pl.targets)
    {
        var seen = new int[2]; float bestMargin = float.MinValue;
        for (int hi = 0; hi < 2; hi++)
            foreach (var e in eyes)
            {
                var a = V(e.x, heights[hi].Item2, e.y); var toT = t.p - a; var end = t.p - toT.normalized * 0.3f;
                bool vis = !FirstHit(a, end, true, out var h) || UnityEngine.Vector3.Distance(h.point, t.p) < 2f;
                if (vis) seen[hi]++;
            }
        // margins from every eye at eye height (sampled every 1.5 m), best line reported; and from the deck centre
        foreach (var e in eyes) { var a = V(e.x, heights[0].Item2, e.y); bestMargin = UnityEngine.Mathf.Max(bestMargin, Clearance(a, t.p, true, 5f)); }
        float centreMargin = Clearance(V(tower.position.x, heights[0].Item2, tower.position.z), t.p, true, 5f);
        if (seen[0] > 0) placeSeen = true;
        md.AppendLine("| " + pl.place + " | " + t.n + " | " + seen[0] + "/64 | " + seen[1] + "/64 | " + bestMargin.ToString("F1") + " | " + centreMargin.ToString("F1") + " |");
    }
    if (!placeSeen) allPlaces = false;
    summary.Append(pl.place + (placeSeen ? " seen" : " NOT SEEN") + "; ");
}

// ---------------- W-1 and C-1 ----------------
var stoneCorners = new System.Collections.Generic.List<UnityEngine.Vector3>();
foreach (UnityEngine.Transform s in wardRoot.Find("Stones"))
{
    var b = s.GetComponent<UnityEngine.Renderer>().bounds;
    foreach (var cx in new[] { b.min.x, b.max.x }) foreach (var cz in new[] { b.min.z, b.max.z })
    { stoneCorners.Add(V(cx, b.max.y, cz)); stoneCorners.Add(V(cx, Hg(cx, cz) + 0.1f, cz)); }
}
var mouthCorners = new System.Collections.Generic.List<UnityEngine.Vector3> { V(50.5f, -6f, 37.4f), V(53.5f, -6f, 37.4f), V(50.5f, -2f, 37.4f), V(53.5f, -2f, 37.4f) };
md.AppendLine("\n## W-1 (Ward) and C-1 (cave), trees off\n");
md.AppendLine("Every ray must hit the terrain (the W ridge and the Ward knob for the Ward, 8.9j; the rim for the cave) before its target. Hidden margin: how deep the least-hidden line passes under the blocking surface.\n");
md.AppendLine("| Check | Eyes and targets | Rays | Rays blocked | Blocked by | Hidden margin, all eyes | Hidden margin, deck centre |");
md.AppendLine("|---|---|---|---|---|---|---|");
bool wardHidden = true, caveHidden = true; string headline = ""; float w1Plus3 = float.MaxValue;
foreach (var chk in new[] { ("W-1", stoneCorners), ("C-1", mouthCorners) })
    foreach (var raise in new[] { 0f, 3f })
        for (int hi = 0; hi < 2; hi++)
        {
            int rays = 0, blocked = 0; float worst = float.MaxValue, centreWorst = float.MaxValue; var by = new System.Collections.Generic.HashSet<string>();
            foreach (var e in eyes)
                foreach (var c in chk.Item2)
                {
                    var a = V(e.x, heights[hi].Item2 + raise, e.y); var tgt = c + V(0f, raise, 0f);
                    var end = tgt - (tgt - a).normalized * 0.4f; rays++;
                    bool hit = FirstHit(a, end, false, out var h);
                    bool ok = hit && h.collider is UnityEngine.TerrainCollider;   // the Wall is terrain (rev 15)
                    if (ok) { blocked++; by.Add("terrain"); }
                    // depth under the blocker: max over samples of (surface - line), trees off, stopping 3 m short of the target
                    float len = UnityEngine.Vector3.Distance(a, tgt), depth = float.MinValue;
                    for (float s = 3f; s < len - 3f; s += 1.5f) { var p = UnityEngine.Vector3.Lerp(a, tgt, s / len); depth = UnityEngine.Mathf.Max(depth, Surface(p.x, p.z, false) - p.y); }
                    worst = UnityEngine.Mathf.Min(worst, depth);
                    if (UnityEngine.Mathf.Abs(e.x - tower.position.x) < 0.6f && UnityEngine.Mathf.Abs(e.y - tower.position.z) < 0.6f) centreWorst = UnityEngine.Mathf.Min(centreWorst, depth);
                }
            if (blocked < rays) { if (chk.Item1 == "W-1") wardHidden = false; else caveHidden = false; }
            md.AppendLine("| " + chk.Item1 + " | " + (raise > 0f ? "+3 m, " : "") + heights[hi].Item1 + " | " + rays + " | " + blocked + " | " + string.Join(", ", by) + " | " + worst.ToString("F1") + " | " + centreWorst.ToString("F1") + " |");
            if (raise == 0f && hi == 0) headline += chk.Item1 + " hidden margin " + worst.ToString("F1") + " m (eye); ";
            if (raise == 3f && hi == 1) headline += chk.Item1 + " +3 m jump " + worst.ToString("F1") + " m; ";
            if (raise == 3f && chk.Item1 == "W-1") w1Plus3 = UnityEngine.Mathf.Min(w1Plus3, worst);
        }

// ---------------- F-1: the fire is hidden by the land (Valley.md rev 10 section 7; 8.14) ----------------
// Cover is terrain and solid rock (8.1's "Rock" root) only; trees never count. Targets are read from the scene, so the check follows
// the fire when it moves: every mesh drawn with DieAlone/FlameCard (active or not) is a set of 4-vertex cards (main3_8_7_ward.cs
// Cards); a card's top is the midpoint of its top edge, its highest lit point 0.93 up (measured on the flipbook in 8.9k); the pass
// counts the whole card, to its top. The day-one smoke sheet's top edge (Ward/SmokeSheet/SheetTop_*) is a target too.
// Origins: every DevWarps point, every trail point at 10 m steps, the daytime eyes of Valley.md section 7 and the deck grid; each at
// eye height (1.6), jump height (2.2) and jump height with eye and target raised 3 m. A ray passes if it hits a terrain collider or a
// rock collider before its target, or runs under the terrain surface anywhere (a terrain collider cannot be hit from below).
// The ledge is the reveal: origins on it (west of x 6.2, z 214 to 286) are left out, and the path from the slot end round the fin is
// sampled every 0.5 m to show where flame tops first show. The crest must stand at 80 or more over x 5 to 20 for z 40 to 345 (7.1).
const float FlameVisibleShare = 0.93f, EyeHeight = 1.6f, JumpHeight = 2.2f, Raise = 3f, TrailStep = 10f, F1Sample = 1f, F1Near = 15f, F1NearSample = 0.25f;
const float CrestNeed = 80f, CrestZ0 = 40f, CrestZ1 = 345f, CrestX0 = 5f, CrestX1 = 20f, CleftZ0 = 261f, CleftZ1 = 268f;
var flameCardShader = UnityEngine.Shader.Find("DieAlone/FlameCard");
var flameTops = new System.Collections.Generic.List<(UnityEngine.Vector3 card, UnityEngine.Vector3 visible, string group)>();
foreach (var mf in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshFilter>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var mr = mf.GetComponent<UnityEngine.MeshRenderer>();
    if (mr == null || mr.sharedMaterial == null || mr.sharedMaterial.shader != flameCardShader || mf.sharedMesh == null) continue;
    var vs = mf.sharedMesh.vertices;
    for (int i = 0; i + 3 < vs.Length; i += 4)
    {
        var b = mf.transform.TransformPoint((vs[i] + vs[i + 1]) * 0.5f); var top = mf.transform.TransformPoint((vs[i + 2] + vs[i + 3]) * 0.5f);
        flameTops.Add((top, UnityEngine.Vector3.Lerp(b, top, FlameVisibleShare), mf.name));
    }
}
int flameCount = flameTops.Count;
var sheetT = wardRoot.Find("SmokeSheet");
if (sheetT != null) foreach (UnityEngine.Transform m in sheetT) if (m.name.StartsWith("SheetTop_")) flameTops.Add((m.position, m.position, "SmokeSheet"));
int sheetCount = flameTops.Count - flameCount;
var rockRoot = Root("Rock") != null ? Root("Rock").transform : null;
var f1Origins = new System.Collections.Generic.List<(string kind, string name, UnityEngine.Vector3 p)>();
var warpRoot = Root("DevWarps");
if (warpRoot != null) foreach (UnityEngine.Transform w in warpRoot.transform) f1Origins.Add(("place", w.name, w.position));
var trailRoot = Root("Trails");
if (trailRoot != null) foreach (UnityEngine.Transform leg in trailRoot.transform) foreach (UnityEngine.Transform pt in leg)
    if (pt.name.Length > 1 && int.TryParse(pt.name.Substring(1), out int metres) && metres % (int)TrailStep == 0) f1Origins.Add(("trail", leg.name + "/" + pt.name, pt.position));
// Valley.md 7: the daytime eyes, on the ground (the Camp 2 stack top is 24)
var eyes7 = new (string n, float x, float z, float y)[] {
    ("Camp 2 stack top", 292f, 108f, 24f), ("Lot and office porch", 358f, 170f, float.NaN), ("Camp 1", 282f, 238f, float.NaN), ("Camp and knoll", 185f, 160f, float.NaN),
    ("Ruin, north loop", 168f, 278f, float.NaN), ("NE: closed campground", 390f, 250f, float.NaN), ("NE: loop's north side", 385f, 295f, float.NaN),
    ("NE: fence at the N band", 393f, 300f, float.NaN), ("NE: loop's south side", 360f, 230f, float.NaN), ("SE: west of the thicket", 330f, 5f, float.NaN), ("SE", 340f, 15f, float.NaN) };
foreach (var e7 in eyes7) f1Origins.Add(("section 7", e7.n, V(e7.x, float.IsNaN(e7.y) ? Hg(e7.x, e7.z) : e7.y, e7.z)));
foreach (var e in eyes) f1Origins.Add(("deck", "deck", V(e.x, deckTop, e.y)));
var terrainCols = new System.Collections.Generic.List<UnityEngine.TerrainCollider>();
foreach (var tc in UnityEngine.Object.FindObjectsByType<UnityEngine.TerrainCollider>(UnityEngine.FindObjectsSortMode.None)) if (tc.enabled) terrainCols.Add(tc);
var rockCols = new System.Collections.Generic.List<UnityEngine.Collider>(); if (rockRoot != null) rockCols.AddRange(rockRoot.GetComponentsInChildren<UnityEngine.Collider>());
var tPos = terrain.transform.position; var tSize = terrain.terrainData.size;
bool LandBlocks(UnityEngine.Vector3 a, UnityEngine.Vector3 b)   // terrain or solid rock, either way along the line
{
    var d = b - a; var ray = new UnityEngine.Ray(a, d.normalized); var back = new UnityEngine.Ray(b, -d.normalized);
    foreach (var tc in terrainCols) if (tc.Raycast(ray, out _, d.magnitude)) return true;
    foreach (var rc in rockCols) if (rc.Raycast(ray, out _, d.magnitude) || rc.Raycast(back, out _, d.magnitude)) return true;
    return false;
}
float UnderDepth(UnityEngine.Vector3 a, UnityEngine.Vector3 b)   // how far the line runs under the terrain at its deepest, inside the terrain
{
    float len = UnityEngine.Vector3.Distance(a, b), depth = float.MinValue;
    for (float s = 0f; s < len; s += s < F1Near ? F1NearSample : F1Sample)
    {
        var p = UnityEngine.Vector3.Lerp(a, b, s / len);
        if (p.x < tPos.x || p.z < tPos.z || p.x > tPos.x + tSize.x || p.z > tPos.z + tSize.z) continue;
        depth = UnityEngine.Mathf.Max(depth, Hg(p.x, p.z) - p.y);
    }
    return depth;
}
bool Seen(UnityEngine.Vector3 a, UnityEngine.Vector3 b) => UnderDepth(a, b) <= 0f && !LandBlocks(a, b);
md.AppendLine("\n## F-1 (fire hidden by the land and solid rock; trees never count)\n");
md.AppendLine("Targets read from the scene: " + flameCount + " flame cards and " + sheetCount + " points on the day-one smoke sheet's top edge. Card top = top of the card (the pass); highest lit point = 0.93 up it (8.9k). Each origin at eye (1.6), jump (2.2), and jump with eye and target +3 m. A ray passes when terrain or rock (the Rock root) blocks it or it runs under the terrain surface. Hidden margin = depth of the least-hidden line under the terrain (terrain only; negative: the line passes over it, a rock may still block it).\n");
md.AppendLine("| Origins | Height | Count | Rays | Seen (card top, the pass) | Seen (highest lit point) | Hidden margin (card top) |");
md.AppendLine("|---|---|---|---|---|---|---|");
bool f1Pass = flameCount > 0 && sheetCount > 0; float f1Least = float.MaxValue; string f1Line = "";
var worstSeen = new System.Collections.Generic.List<string>();
bool Reveal(UnityEngine.Vector3 q) => q.x < 6.2f && q.z > 214f && q.z < 286f;   // on the ledge: the fire is meant to show there (4.6, 4.7)
int revealLeft = 0; foreach (var o in f1Origins) if (Reveal(o.p)) revealLeft++;
var heightsF1 = new[] { ("eye", EyeHeight, 0f), ("jump", JumpHeight, 0f), ("jump +3", JumpHeight, Raise) };
foreach (var kind in new[] { "place", "trail", "section 7", "deck" })
    foreach (var hh in heightsF1)
    {
        int count = 0, rays = 0, seenCard = 0, seenVis = 0; float least = float.MaxValue;
        var seenBy = new System.Collections.Generic.Dictionary<string, int>();
        foreach (var o in f1Origins)
        {
            if (o.kind != kind || Reveal(o.p)) continue; count++; int oSeen = 0;
            var a = o.p + V(0f, hh.Item2 + hh.Item3, 0f);
            foreach (var f in flameTops)
            {
                rays++; var tc = f.card + V(0f, hh.Item3, 0f); var tv = f.visible + V(0f, hh.Item3, 0f);
                float dCard = UnderDepth(a, tc);
                if (dCard <= 0f && !LandBlocks(a, tc)) { seenCard++; oSeen++; if (Seen(a, tv)) seenVis++; }
                least = UnityEngine.Mathf.Min(least, dCard);
            }
            if (oSeen > 0) seenBy[o.name] = (seenBy.TryGetValue(o.name, out var n) ? n : 0) + oSeen;
        }
        if (seenCard > 0) f1Pass = false;
        f1Least = UnityEngine.Mathf.Min(f1Least, least);
        md.AppendLine("| " + kind + " | " + hh.Item1 + " | " + count + " | " + rays + " | " + seenCard + " | " + seenVis + " | " + least.ToString("F1") + " |");
        f1Line += kind + " " + hh.Item1 + " " + seenCard + "/" + rays + "; ";
        var ordered = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>(seenBy); ordered.Sort((p, q) => q.Value.CompareTo(p.Value));
        for (int i = 0; i < ordered.Count && i < 8; i++) worstSeen.Add(kind + " " + hh.Item1 + ": " + ordered[i].Key + " sees " + ordered[i].Value);
    }
if (worstSeen.Count > 0) { md.AppendLine("\nOrigins that see a target:\n"); foreach (var s in worstSeen) md.AppendLine("- " + s); }
// the reveal: from the slot's last straight, south round the fin and on to the path end, every 0.5 m, at eye and jump height
var revealPath = new[] { new UnityEngine.Vector2(14.5f, 265.5f), new UnityEngine.Vector2(4f, 265.5f), new UnityEngine.Vector2(4f, 257.3f), new UnityEngine.Vector2(-8.5f, 246f) };
string firstShow = ""; bool revealEarly = false;
foreach (var hh in new[] { ("eye", EyeHeight), ("jump", JumpHeight) })
{
    string at = "never"; float walked = 0f;
    for (int i = 0; i < revealPath.Length - 1 && at == "never"; i++)
    {
        float segL = UnityEngine.Vector2.Distance(revealPath[i], revealPath[i + 1]);
        for (float s = 0f; s < segL && at == "never"; s += 0.5f)
        {
            var q = UnityEngine.Vector2.Lerp(revealPath[i], revealPath[i + 1], s / segL); var o = V(q.x, Hg(q.x, q.y) + hh.Item2, q.y);
            int seenHere = 0; foreach (var fl in flameTops) if (Seen(o, fl.card)) seenHere++;
            if (seenHere > 0 && !Reveal(o)) { f1Pass = false; revealEarly = true; }   // flame tops may first show only on the ledge
            if (seenHere > 0) at = "(" + q.x.ToString("F2") + ", " + q.y.ToString("F2") + "), " + (walked + s).ToString("F1") + " m from the dogleg's end, " + seenHere + " tops";
        }
        walked += segL;
    }
    firstShow += hh.Item1 + " " + at + "; ";
}
// the crest (7.1): the highest ground across x 5 to 20 at every metre of z 40 to 345 (the cleft left out)
float crestLow = float.MaxValue, crestLowZ = 0f;
for (float z = CrestZ0; z <= CrestZ1; z += 1f)
{
    if (z > CleftZ0 && z < CleftZ1) continue;
    float hi = float.MinValue; for (float x = CrestX0; x <= CrestX1; x += 0.5f) hi = UnityEngine.Mathf.Max(hi, Hg(x, z));
    if (hi < crestLow) { crestLow = hi; crestLowZ = z; }
}
bool crestOk = crestLow >= CrestNeed - 0.05f; if (!crestOk) f1Pass = false;
md.AppendLine("\nOrigins on the ledge (the reveal, not counted): " + revealLeft + ". Flame tops first show on the way round the fin: " + firstShow);
md.AppendLine("\nW crest, lowest top over x 5 to 20, z 40 to 345 (cleft left out): " + crestLow.ToString("F2") + " at z " + crestLowZ.ToString("F0") + " (needs 80): " + (crestOk ? "yes" : "NO") + ".");
string f1Head = "F-1 " + (f1Pass ? "hidden: True" : "hidden: False") + ", least margin " + f1Least.ToString("F1") + " m, cards " + flameCount + ", sheet points " + sheetCount + ", rock colliders " + rockCols.Count + ", " + f1Line + "crest low " + crestLow.ToString("F2") + " at z " + crestLowZ.ToString("F0") + ", reveal first shows: " + firstShow + (revealEarly ? "EARLY (before the ledge); " : "on the ledge; ");

// ---------------- 4.1: cab from the junctions ----------------
var cab = tower.Find("Cab");
var cabPts = new System.Collections.Generic.List<UnityEngine.Vector3>();
foreach (var cx in new[] { -2.2f, 0f, 2.2f }) foreach (var cz in new[] { -2.2f, 0f, 2.2f }) foreach (var cy in new[] { 1.0f, 2.7f }) cabPts.Add(cab.TransformPoint(V(cx, cy, cz)));
var junctions = new (string n, float x, float z)[] { ("J", 104f, 206f), ("Jg", 262f, 172f), ("W1", 128f, 70f), ("T", 340f, 170f), ("Pump", 190f, 97f), ("Camp 1", 276f, 232f), ("Camp 2 (stack foot)", 286f, 99f), ("Camp 3 floor", 78f, 146f) };
md.AppendLine("\n## 4.1 Tower cab seen from the junctions (eye 1.6 m, trees on)\n");
md.AppendLine("| Junction | Cab points seen (of " + cabPts.Count + ") | First blocker of a missed ray |");
md.AppendLine("|---|---|---|");
string junc = "";
foreach (var j in junctions)
{
    var a = V(j.x, Hg(j.x, j.z) + 1.6f, j.z); int seen = 0; string blocker = "";
    foreach (var c in cabPts)
    {
        var d = c - a; var hits = UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude + 0.5f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore);
        System.Array.Sort(hits, (p, q) => p.distance.CompareTo(q.distance));
        bool vis = true;
        foreach (var h in hits) { if (IsUnder(h.collider.transform, tower)) break; if (h.distance < d.magnitude - 0.3f) { vis = false; if (blocker == "") blocker = h.collider.transform.parent != null ? h.collider.transform.parent.name + "/" + h.collider.name : h.collider.name; break; } }
        if (vis) seen++;
    }
    md.AppendLine("| " + j.n + " | " + seen + " | " + (seen == cabPts.Count ? "none" : blocker) + " |");
    junc += j.n + " " + (seen > 0 ? "yes" : "no") + " (" + seen + "); ";
}

// ---------------- next destination from the junctions Marlow found blind (8.9a), eye 1.6 m, trees on ----------------
bool SeesPoint(UnityEngine.Vector3 a, UnityEngine.Vector3 tgt, UnityEngine.Transform goal, out string blocker)   // a hit on the goal object counts as seen
{
    var d = tgt - a; var hits = UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore);
    System.Array.Sort(hits, (p, q) => p.distance.CompareTo(q.distance)); blocker = "";
    foreach (var h in hits) { if (IsUnder(h.collider.transform, goal)) return true; if (UnityEngine.Vector3.Distance(h.point, tgt) < 1.5f) return true; blocker = (h.collider.transform.parent != null ? h.collider.transform.parent.name + "/" : "") + h.collider.name; return false; }
    return true;
}
var snagT = giants.Find("Heroes/Snag"); var boathouseT = Root("Lake").transform.Find("Boathouse");
var nexts = new (string from, UnityEngine.Vector2 at, string to, UnityEngine.Vector3[] tgts, UnityEngine.Transform goal)[] {
    ("Pump (190, 97)", new UnityEngine.Vector2(190f, 97f), "the Snag (W1 to Camp 3)", new[] { V(96f, 54f, 146.5f), V(98.7f, 45f, 145.2f), V(98.7f, 30f, 145.2f) }, snagT),
    ("Camp 2, 8 m out toward the lake (286.5, 102.2)", new UnityEngine.Vector2(286.5f, 102.2f), "boathouse roof", new[] { V(240f, -1.0f, 52.4f), V(242.8f, -1.2f, 52.4f), V(240f, -1.0f, 55.0f) }, boathouseT),
    ("Camp 3 floor (79, 145)", new UnityEngine.Vector2(79f, 145f), "tower cab", cabPts.ToArray(), tower),
    ("Camp 3 centre (78, 146)", new UnityEngine.Vector2(78f, 146f), "tower cab", cabPts.ToArray(), tower),
};
md.AppendLine("\n## Next destination from the junctions found blind in the walk (eye 1.6 m, trees and thicket on)\n");
md.AppendLine("| From | Toward | Points seen | First blocker of a missed ray |");
md.AppendLine("|---|---|---|---|");
string nextLine = ""; bool nextAll = true;
foreach (var nx in nexts)
{
    var a = V(nx.at.x, Hg(nx.at.x, nx.at.y) + 1.6f, nx.at.y); int seenN = 0; string blk = "";
    foreach (var tp in nx.tgts) { if (SeesPoint(a, tp, nx.goal, out var b)) seenN++; else if (blk == "") blk = b; }
    if (seenN == 0) nextAll = false;
    md.AppendLine("| " + nx.from + " | " + nx.to + " | " + seenN + " of " + nx.tgts.Length + " | " + (blk == "" ? "none" : blk) + " |");
    nextLine += nx.from + " " + seenN + "/" + nx.tgts.Length + "; ";
}
// ---------------- Hollow Giant crown against the Camp 2 view (cone rule 3 m, 5.2) ----------------
var hgCrown = giants.Find("Heroes/Hollow_Giant/Crown"); float crownClear = float.MaxValue;
if (hgCrown != null)
{
    var mfC = hgCrown.GetComponent<UnityEngine.MeshFilter>().sharedMesh; var eyeC = V(tower.position.x, heights[0].Item2, tower.position.z);
    var camp2Tgts = new[] { V(292f, 24.2f, 108f), V(287.2f, 24f, 108f), V(292f, 24f, 103.2f), V(292f, 24f, 112.8f), V(296.8f, 24f, 108f) };
    foreach (var v in mfC.vertices)
    {
        var wv = hgCrown.TransformPoint(v);
        foreach (var t in camp2Tgts) { var ab = t - eyeC; float s = UnityEngine.Mathf.Clamp01(UnityEngine.Vector3.Dot(wv - eyeC, ab) / ab.sqrMagnitude); crownClear = UnityEngine.Mathf.Min(crownClear, UnityEngine.Vector3.Distance(wv, eyeC + ab * s)); }
    }
}
md.AppendLine("\nHollow Giant crown to the deck-centre lines to the Camp 2 stack top and its edges: " + crownClear.ToString("F1") + " m (cone rule 3 m).");
md.AppendLine("\n## Result\n");
md.AppendLine("- Places: " + summary);
md.AppendLine("- Ward hidden from every eye point (W-1, with +3 m): " + (wardHidden ? "yes" : "NO") + ". Cave hidden (C-1, with +3 m): " + (caveHidden ? "yes" : "NO") + ".");
md.AppendLine("- " + headline);
md.AppendLine("- Cab from junctions: " + junc);
md.AppendLine("- W-1 with eyes and targets raised 3 m keeps " + w1Plus3.ToString("F1") + " m (needs 3): " + (w1Plus3 >= 3f ? "yes" : "NO") + ".");
md.AppendLine("- Next destination from the blind junctions: " + nextLine + (nextAll ? "all seen" : "NOT ALL SEEN"));
md.AppendLine("- Hollow Giant crown clearance to the Camp 2 lines: " + crownClear.ToString("F1") + " m (needs 3): " + (crownClear >= 3f ? "yes" : "NO") + ".");
md.AppendLine("- " + f1Head);

foreach (var c in temp) UnityEngine.Object.DestroyImmediate(c);
if (!wasDirty) UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);   // the temporary colliders are gone; saving the identical scene clears the dirty flag
string outPath = System.IO.Path.GetFullPath("Docs/Layout/Main3/Main3_sightlines.md");
System.IO.File.WriteAllText(outPath, md.ToString());
return "places: " + summary + " | all seen: " + allPlaces + " | Ward hidden: " + wardHidden + ", cave hidden: " + caveHidden + " | " + headline + "| W-1 +3 m " + w1Plus3.ToString("F1") + " ok " + (w1Plus3 >= 3f) + " | next: " + nextLine + "all " + nextAll + " | Hollow Giant crown clearance " + crownClear.ToString("F1") + " ok " + (crownClear >= 3f) + " | cab from junctions: " + junc + "| " + f1Head + "| report " + outPath;
