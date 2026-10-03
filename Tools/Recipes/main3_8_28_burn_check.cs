// Main3 8.28 burn check (Play mode, Main3; BurnLayout.md draft 2, BurnLayout_UI.md). In main3_review_capture.sh --area burn's Play checks;
// also runs alone. Never saves; restores the player and every temporary collider. Every move is PlayerController.Step (dt 0.02).
// PROMPTS (BurnLayout_UI 1): from each stand, eye 1.6, the interactor's own test (its mask, triggers ignored, interactReach):
//   1.1 forage A from (241.6, 160.6) facing 301, 36 down (the doc's look, not an aim): `Forage`;
//   1.2 forage B from the Camp to Camp 3 tread edge nearest its nearest bush, aimed at that bush: `Forage`;
//   1.3 forage C from Places/ForageC/ForageC_Stand, aimed at its nearest shrub: `Forage`;
//   1.5 the T board from (339.5, 172.5), aimed at the board: `Examine`;
//   1.4 the Jg signpost (from the junction facing 135), 1.6 the Gate Tree stub (from its nearest Jg to T point), 1.7 the Hollow Giant
//   hollow (from (203.9, 143.6) facing 208), 1.8 the first-sight stake (from its nearest trail point): no prompt (nothing with a prompt
//   met within reach).
// FORAGE A: the near bush surfaces in plan from the stand (Marlow 1.52 and 1.76); KEEP CLEAR: nothing over clearOver m in a clearHalf m band
//   from the Camp to Jg centreline to each of the two nearest bushes (renderers under Forest, SliceLook and Ground815 stops). FORAGE C: the
//   shrubs' colliders solid, tops 0.6 m or more over the ground, neighbours' gaps gapMax m or less.
// HOLLOW: the Hollow Giant carries no collider wider than the pack capsule (the r 4.5 one is gone); the body stands in front of the hollow;
//   walks from Camp to pump P60 (186.69, 133.44) to the inspect stand and back each arrive. T4 DROP: from the Camp to Jg markers m 10 to 28
//   (scene marker chainage), walk and sprint-jump south (the drop side) for dropTime s; each run that leaves the tread then walks to P60.
// JG: the post 2.55 m from the junction (jgPost tolerance), 2.5 m or more off each of its three lines; no Hedge_Burn_7 box inside r 4.
// GATE TREE: one capsule on the stub; its bark jgTBark m or more off the Jg to T centreline; from the nearest centreline point at eye height
//   the first thing met toward the stub is the stub; no other trunk collider of radius bigTrunk m or more within bigTrunkR m.
// STONES: no Ground815/TrailEdges piece within scopeOff m of a burn trail has a collider; CS_Stone_3 stoneOff m (stoneTol) off Jg to Camp 1.
// VERGE: no Forest/BurnDeadwood or SliceLook/BurnRegrowth foot within vergeClear m in plan of the deck-to-verge line, x 250 to 345.
// OFFICE: no LOD 0 vertex of any Forest or SliceLook piece within officeBand m in plan of the deck-to-office line (lowest deck eye to
//   (344, 4.3, 199)), x 230 to 340, over the cap (the line less capDrop).
// REGROWTH: every SliceLook/BurnRegrowth piece within regrowthBand m of the Jg to T line tops at regrowthMax m or less; no tree within
//   regrowthBand m of the line from its m 50 point to 40 m east of it tops over eastMax m.
// FORAGE B FIRS: the two RedFir8 trunks stand clear of the Camp to Camp 3 tread (trunk surface treadHalf m or more off the centreline).
// WALKS (doc 4): the four burn trails, each end to end, with times.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Transform RootT(string n) { var r = Root(n); return r != null ? r.transform : null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z); UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z); UnityEngine.Vector2 XZ(UnityEngine.Vector3 v) => new UnityEngine.Vector2(v.x, v.z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset"); var set = Main3AreaSet.Load();
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, eyeH = 1.6f;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var places = RootT("Places"); var poi = RootT("PointsOfInterest"); var slice = RootT("SliceLook"); var forest = RootT("Forest"); var ground815 = RootT("Ground815"); var giants = RootT("Giants"); var trails = RootT("Trails");
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
System.Collections.Generic.List<UnityEngine.Vector3> Marks(string leg) { var l = new System.Collections.Generic.List<UnityEngine.Vector3>(); var t = trails != null ? trails.Find(leg) : null; if (t != null) foreach (UnityEngine.Transform p in t) l.Add(p.position); return l; }
System.Collections.Generic.List<UnityEngine.Vector2> Flat(System.Collections.Generic.List<UnityEngine.Vector3> l) { var o = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (var p in l) o.Add(XZ(p)); return o; }
UnityEngine.Vector2 FootOn(System.Collections.Generic.List<UnityEngine.Vector2> l, UnityEngine.Vector2 q) { var best = l.Count > 0 ? l[0] : q; float bd = float.MaxValue; for (int i = 1; i < l.Count; i++) { var ab = l[i] - l[i - 1]; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - l[i - 1], ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); var f = l[i - 1] + ab * t; float d = UnityEngine.Vector2.Distance(q, f); if (d < bd) { bd = d; best = f; } } return best; }
float DistTo(System.Collections.Generic.List<UnityEngine.Vector2> l, UnityEngine.Vector2 q) => l.Count == 0 ? float.MaxValue : UnityEngine.Vector2.Distance(q, FootOn(l, q));
float LineD(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = (b - a).normalized; return UnityEngine.Mathf.Abs(UnityEngine.Vector2.Dot(q - a, P(-ab.y, ab.x))); }
System.Collections.Generic.List<UnityEngine.Transform> Pieces(UnityEngine.Transform root) { var o = new System.Collections.Generic.List<UnityEngine.Transform>(); void Walk(UnityEngine.Transform t) { foreach (UnityEngine.Transform c in t) { if (c.GetComponent<UnityEngine.LODGroup>() != null || c.GetComponent<UnityEngine.Renderer>() != null) o.Add(c); else Walk(c); } } if (root != null) Walk(root); return o; }
var treeName = new System.Text.RegularExpressions.Regex("RedFir|RedPine|Sequoia|Tree_Dead|Sapling");
UnityEngine.Renderer[] Lod0(UnityEngine.Transform t) { var lg = t.GetComponentInChildren<UnityEngine.LODGroup>(); return lg != null && lg.GetLODs().Length > 0 ? lg.GetLODs()[0].renderers : t.GetComponentsInChildren<UnityEngine.Renderer>(); }
var burnLegs = new[] { "Camp to Jg", "Jg to T", "Jg to Camp 1", "Camp 2 to T" };
try
{
    if (places == null || poi == null || slice == null || forest == null || ground815 == null || giants == null || trails == null) return "missing a root (run the rebuild and main3_8_28_burn.cs)";
    var burn = places.Find("Burn828"); if (burn == null) return "no Places/Burn828 (run main3_8_28_burn.cs)";
    var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
    int mask = pi != null && maskF != null ? ((UnityEngine.LayerMask)maskF.GetValue(pi)).value : ~0; float reach = tuning != null ? tuning.interactReach : 2f;
    if (pi == null || maskF == null || tuning == null) Line(false, "no PlayerInteractor, its mask or PlayerTuning (the rays below use every layer and 2 m)");
    var campToJg = Flat(Marks("Camp to Jg")); var jgToT = Flat(Marks("Jg to T")); var jgToC1 = Flat(Marks("Jg to Camp 1")); var campToC3 = Flat(Marks("Camp to Camp 3"));
    // ---- PROMPTS
    string Meet(UnityEngine.Vector3 eye, UnityEngine.Vector3 dir, out Interactable it, out UnityEngine.Transform hitT)
    {
        it = null; hitT = null; if (!UnityEngine.Physics.Raycast(eye, dir.normalized, out var hit, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) return "nothing within " + F1(reach) + " m";
        hitT = hit.collider.transform; it = hit.collider.GetComponentInParent<Interactable>(); return WalkIns.PathOf(hitT) + " at " + F(hit.distance) + " m, prompt \"" + (it != null ? it.Prompt : "none") + "\"";
    }
    UnityEngine.Vector3 Look(float facing, float down) => UnityEngine.Quaternion.Euler(down, facing, 0f) * UnityEngine.Vector3.forward;
    UnityEngine.Transform NearestBush(UnityEngine.Transform patch, string prefix, UnityEngine.Vector2 from) { UnityEngine.Transform best = null; float bd = float.MaxValue; if (patch != null) foreach (UnityEngine.Transform b in patch) { if (!b.name.StartsWith(prefix)) continue; float d = UnityEngine.Vector2.Distance(XZ(b.position), from); if (d < bd) { bd = d; best = b; } } return best; }
    void Prompt(string label, UnityEngine.Transform want, UnityEngine.Vector3 eye, UnityEngine.Vector3 dir, string word)
    {
        if (want == null) { Line(false, "PROMPT " + label + ": no object"); return; }
        var got = Meet(eye, dir, out var it, out var hitT); bool ok = it != null && it.Prompt == word && hitT != null && hitT.IsChildOf(want);
        float down = -UnityEngine.Mathf.Asin(dir.normalized.y) * UnityEngine.Mathf.Rad2Deg, bearing = (UnityEngine.Mathf.Atan2(dir.x, dir.z) * UnityEngine.Mathf.Rad2Deg + 360f) % 360f;
        Line(ok, "PROMPT " + label + " (\"" + word + "\"): from (" + F1(eye.x) + ", " + F1(eye.z) + ") bearing " + F1(bearing) + ", " + F1(down) + " down: " + got);
    }
    void NoPrompt(string label, UnityEngine.Vector3 eye, UnityEngine.Vector3 dir)
    {
        var got = Meet(eye, dir, out var it, out _); bool ok = it == null || string.IsNullOrEmpty(it.Prompt);
        Line(ok, "PROMPT " + label + " (none): from (" + F1(eye.x) + ", " + F1(eye.z) + "): " + got);
    }
    UnityEngine.Vector3 Eye(float x, float z) => V(x, H(x, z) + eyeH, z);
    var patchA = poi.Find("POI_Forage_patch_A"); var patchB = poi.Find("POI_Forage_patch_B"); var patchC = places.Find("ForageC");
    { var eye = Eye(241.6f, 160.6f); Prompt("1.1 forage A", patchA, eye, Look(301f, 36f), "Forage"); }
    {
        // forage B: the tread edge (treadHalf m off the centreline) nearest its nearest bush, aimed at that bush's collider
        var b = patchB != null ? NearestBush(patchB, "Bush", XZ(patchB.position)) : null;
        if (b == null || campToC3.Count < 2) Line(false, "PROMPT 1.2 forage B: no bush or no Trails/Camp to Camp 3");
        else
        {
            const float treadHalf = 2f; var bq = XZ(b.position); var f = FootOn(campToC3, bq); var dir2 = (bq - f).normalized; var near = NearestBush(patchB, "Bush", f); bq = XZ(near.position); f = FootOn(campToC3, bq); dir2 = (bq - f).normalized;
            var st = f + dir2 * UnityEngine.Mathf.Min(treadHalf, UnityEngine.Vector2.Distance(f, bq) - 1f); var eye = Eye(st.x, st.y); var aim = near.GetComponent<UnityEngine.Collider>() != null ? near.GetComponent<UnityEngine.Collider>().bounds.center : near.position;
            Prompt("1.2 forage B", patchB, eye, aim - eye, "Forage");
        }
    }
    {
        var stand = patchC != null ? patchC.Find("ForageC_Stand") : null;
        if (stand == null) Line(false, "PROMPT 1.3 forage C: no Places/ForageC/ForageC_Stand");
        else { var near = NearestBush(patchC, "Bush_ForageC_", XZ(stand.position)); var eye = Eye(stand.position.x, stand.position.z); var aim = near != null && near.GetComponent<UnityEngine.Collider>() != null ? near.GetComponent<UnityEngine.Collider>().bounds.center : patchC.position; Prompt("1.3 forage C", patchC, eye, aim - eye, "Forage"); }
    }
    {
        var board = ground815.Find("JunctionMarkers/Trailhead_Board/Board"); var eye = Eye(339.5f, 172.5f);
        Prompt("1.5 T board", board, eye, (board != null ? board.position : V(338f, eye.y, 172.5f)) - eye, "Examine");
    }
    {
        var sign = ground815.Find("JunctionMarkers/Sign_Jg"); var eye = Eye(262f, 172f);
        NoPrompt("1.4 Jg signpost, from the junction facing 135", eye, Look(135f, sign != null ? -UnityEngine.Mathf.Atan2(sign.position.y + 2.2f - eye.y, UnityEngine.Vector2.Distance(XZ(sign.position), P(262f, 172f))) * UnityEngine.Mathf.Rad2Deg : 0f));
        var stub = giants.Find("Heroes/Gate_Tree"); if (stub != null) { var f = FootOn(jgToT, XZ(stub.position)); var e2 = Eye(f.x, f.y); NoPrompt("1.6 Gate Tree stub, from its nearest Jg to T point", e2, V(stub.position.x, e2.y, stub.position.z) - e2); } else Line(false, "PROMPT 1.6 Gate Tree: no Giants/Heroes/Gate_Tree");
        NoPrompt("1.7 Hollow Giant hollow, from (203.9, 143.6) facing 208", Eye(203.9f, 143.6f), Look(208f, 10f));
        var stake = poi.Find("POI_First_sight_of_the_lot");
        if (stake != null) { UnityEngine.Vector2 best = XZ(stake.position); float bd = float.MaxValue; foreach (var n in burnLegs) { var l = Flat(Marks(n)); if (l.Count < 2) continue; var f = FootOn(l, XZ(stake.position)); float d = UnityEngine.Vector2.Distance(f, XZ(stake.position)); if (d < bd) { bd = d; best = f; } } var e3 = Eye(best.x, best.y); NoPrompt("1.8 first-sight stake, from its nearest trail point", e3, stake.position + V(0f, 0.6f, 0f) - e3); }
        else Line(false, "PROMPT 1.8 stake: no PointsOfInterest/POI_First_sight_of_the_lot");
    }
    // ---- FORAGE A, KEEP CLEAR, FORAGE C
    {
        const float clearOver = 0.5f, clearHalf = 0.6f, gapMax = 0.3f, topMin = 0.6f;
        if (patchA == null) Line(false, "FORAGE A: no patch");
        else
        {
            var stand = P(241.6f, 160.6f); var near = new System.Collections.Generic.List<(float, string)>(); var bushes = new System.Collections.Generic.List<UnityEngine.Transform>();
            foreach (UnityEngine.Transform b in patchA) { if (b.name != "Bush") continue; bushes.Add(b); var c = b.GetComponent<UnityEngine.Collider>(); if (c == null) continue; var cp = c.ClosestPoint(V(stand.x, c.bounds.center.y, stand.y)); near.Add((UnityEngine.Vector2.Distance(XZ(cp), stand), b.name)); }
            near.Sort((p, q) => p.Item1.CompareTo(q.Item1));
            sb.Append("note: FORAGE A near bush surfaces in plan from the stand: " + string.Join(", ", System.Linq.Enumerable.Select(near, x => F(x.Item1))) + " m (Marlow 1.52 and 1.76)\n");
            bushes.Sort((p, q) => DistTo(campToJg, XZ(p.position)).CompareTo(DistTo(campToJg, XZ(q.position))));
            var hits = new System.Collections.Generic.List<string>();
            foreach (var root in new[] { forest, slice, ground815 != null ? ground815.Find("Stops") : null })
                foreach (var t in Pieces(root))
                    foreach (var r in Lod0(t))
                    {
                        if (r == null || !r.enabled) continue; var q = XZ(r.bounds.center); bool inBand = false;
                        for (int i = 0; i < UnityEngine.Mathf.Min(2, bushes.Count); i++) { var bq = XZ(bushes[i].position); var f = FootOn(campToJg, bq); var ab = bq - f; float u = UnityEngine.Vector2.Dot(q - f, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude); if (u >= 0f && u <= 1f && UnityEngine.Vector2.Distance(q, f + ab * u) <= clearHalf + r.bounds.extents.x) inBand = true; }
                        if (inBand && r.bounds.max.y - H(q.x, q.y) > clearOver) { hits.Add(WalkIns.PathOf(r.transform) + " top " + F(r.bounds.max.y - H(q.x, q.y))); break; }
                    }
            Line(hits.Count == 0, "FORAGE A KEEP CLEAR: over " + F1(clearOver) + " m between the centreline and the two near bushes: " + (hits.Count == 0 ? "nothing" : string.Join("; ", hits)));
        }
        if (patchC == null) Line(false, "FORAGE C: no Places/ForageC");
        else
        {
            var cs = new System.Collections.Generic.List<UnityEngine.Collider>(); var bad = new System.Collections.Generic.List<string>();
            foreach (UnityEngine.Transform s in patchC) { if (!s.name.StartsWith("Bush_ForageC_")) continue; var c = s.GetComponent<UnityEngine.Collider>(); if (c == null || c.isTrigger) { bad.Add(s.name + " no solid collider"); continue; } cs.Add(c); float top = c.bounds.max.y - H(c.bounds.center.x, c.bounds.center.z); if (top < topMin) bad.Add(s.name + " top " + F(top)); }
            float worstGap = 0f; foreach (var a in cs) { float g = float.MaxValue; foreach (var b in cs) if (b != a) { var pa = a.ClosestPoint(b.bounds.center); var pb = b.ClosestPoint(pa); g = UnityEngine.Mathf.Min(g, UnityEngine.Vector3.Distance(pa, pb)); } if (g < float.MaxValue) worstGap = UnityEngine.Mathf.Max(worstGap, g); }
            Line(bad.Count == 0 && cs.Count == 5 && worstGap <= gapMax, "FORAGE C: " + cs.Count + " solid shrub colliders, widest gap to a neighbour " + F(worstGap) + " m (" + F(gapMax) + " or less)" + (bad.Count > 0 ? "; " + string.Join("; ", bad) : ""));
        }
    }
    // ---- HOLLOW, T4 DROP, WALKS
    bool Walk(UnityEngine.Vector3 to, bool jump, ref float time, out float left)
    {
        for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, jump, jump, dt); time += dt; }
        var e = pc.transform.position; left = P(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
    }
    var p60 = V(186.69f, H(186.69f, 133.44f), 133.44f);
    {
        var hg = giants.Find("Heroes/Hollow_Giant"); var hollow = burn.Find("Hollow");
        if (hg == null || hollow == null) Line(false, "HOLLOW: no Hollow_Giant or Places/Burn828/Hollow");
        else
        {
            int wide = 0; foreach (var c in hg.GetComponentsInChildren<UnityEngine.Collider>()) wide++;
            Line(wide == 0, "HOLLOW: colliders left on the gray Hollow_Giant (its r 4.5 trunk): " + wide);
            var front = hollow.position + hollow.forward * 1.2f; Put(V(front.x, H(front.x, front.z), front.z)); var at = pc.transform.position;
            Line(P(at.x - front.x, at.z - front.z).magnitude < 0.3f && at.y < H(at.x, at.z) + 0.5f, "HOLLOW STAND: the body set 1.2 m in front of the hollow settles at (" + F1(at.x) + ", " + F1(at.y) + ", " + F1(at.z) + ")");
            var stand = V(203.9f, H(203.9f, 143.6f), 143.6f);
            foreach (var (name, a, b) in new[] { ("in, P60 to the inspect stand", p60, stand), ("out, the inspect stand to P60", stand, p60) })
            { Put(a); float time = 0f; bool ok = Walk(b, false, ref time, out float left); Line(ok, "HOLLOW WALK " + name + ": " + F1(time) + " s" + (ok ? "" : ", stops " + F(left) + " m short at (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.y) + ", " + F1(pc.transform.position.z) + ")")); }
        }
        // T4: Camp to Jg markers m 10 to 28 (2 m apart), runs south off the drop
        const float dropFrom = 10f, dropTo = 28f, markStep = 2f, dropTime = 2.5f, fellBy = 2f; var cj = Marks("Camp to Jg"); int runs = 0, fell = 0; var stuck = new System.Collections.Generic.List<string>();
        for (int i = UnityEngine.Mathf.RoundToInt(dropFrom / markStep); i <= UnityEngine.Mathf.RoundToInt(dropTo / markStep) && i + 1 < cj.Count; i++)
        {
            var tan = XZ(cj[i + 1] - cj[i]).normalized; var right = P(tan.y, -tan.x); var south = right.y <= 0f ? right : -right;
            foreach (var (jump, how) in new[] { (false, "walk"), (true, "sprint-jump") })
            {
                Put(cj[i]); runs++; for (float t = 0f; t < dropTime; t += dt) pc.Step(V(south.x, 0f, south.y), jump, jump, dt);
                var e = pc.transform.position; if (e.y > cj[i].y - fellBy) continue; fell++;
                float time = 0f; if (!Walk(p60, false, ref time, out float left)) stuck.Add("from m " + F1(i * markStep) + " " + how + ", landed (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + "), stops " + F(left) + " m short of P60");
            }
        }
        Line(stuck.Count == 0, "T4 DROP: " + runs + " runs south off Camp to Jg m " + F1(dropFrom) + " to " + F1(dropTo) + ", " + fell + " fell, walked out to P60 " + (fell - stuck.Count) + (stuck.Count > 0 ? "; stuck: " + string.Join("; ", stuck) : ""));
        float speed = tuning != null ? tuning.walkSpeed : 2.5f;
        foreach (var n in burnLegs)
        {
            var pts = Marks(n); if (pts.Count < 2) { Line(false, "WALK: no Trails/" + n); continue; }
            Put(pts[0]); float time = 0f, len = 0f; bool ok = true; string where = ""; for (int i = 1; i < pts.Count; i++) len += P(pts[i].x - pts[i - 1].x, pts[i].z - pts[i - 1].z).magnitude;
            for (int i = 1; i < pts.Count && ok; i++) if (!Walk(pts[i], false, ref time, out float left)) { ok = false; where = ", stops " + F(left) + " m short of (" + F1(pts[i].x) + ", " + F1(pts[i].z) + ")"; }
            Line(ok, "WALK: " + n + ", " + F1(len) + " m, " + F1(time) + " s at " + F1(speed) + " m/s" + where);
        }
    }
    // ---- JG, GATE TREE, STONES
    {
        const float jgPost = 2.55f, jgPostTol = 0.15f, jgOff = 2.5f, jgOpen = 4f; var jg = P(262f, 172f);
        var sign = ground815.Find("JunctionMarkers/Sign_Jg"); var post = sign != null ? sign.Find("Post") : null;
        if (post == null) Line(false, "JG: no Sign_Jg/Post");
        else
        {
            var pq = XZ(post.position); float dj = UnityEngine.Vector2.Distance(pq, jg); var offs = new System.Collections.Generic.List<string>(); bool offOk = true;
            foreach (var (n, l) in new[] { ("camp", Flat(Marks("Camp to Jg"))), ("T", jgToT), ("Camp 1", jgToC1) }) { float d = DistTo(l, pq); offs.Add(n + " " + F(d)); if (d < jgOff) offOk = false; }
            Line(UnityEngine.Mathf.Abs(dj - jgPost) <= jgPostTol && offOk, "JG POST at (" + F(pq.x) + ", " + F(pq.y) + "): " + F(dj) + " m from the junction (" + F(jgPost) + "), off the lines " + string.Join(", ", offs) + " (" + F(jgOff) + " or more)");
        }
        var h7 = ground815.Find("Stops/Hedge_Burn_7"); var inside = new System.Collections.Generic.List<string>();
        if (h7 != null) foreach (var c in h7.GetComponentsInChildren<UnityEngine.BoxCollider>()) { var cp = c.ClosestPoint(V(jg.x, c.bounds.center.y, jg.y)); float d = UnityEngine.Vector2.Distance(XZ(cp), jg); if (d < jgOpen) inside.Add("(" + F(c.bounds.center.x) + ", " + F(c.bounds.center.z) + ") at " + F(d)); }
        Line(inside.Count == 0, "JG OPEN: Hedge_Burn_7 boxes inside r " + F1(jgOpen) + " of the junction: " + (inside.Count == 0 ? "none" : string.Join("; ", inside)));
    }
    {
        const float jgTBark = 1.0f, bigTrunk = 2f, bigTrunkR = 20f;
        var gt = giants.Find("Heroes/Gate_Tree"); UnityEngine.CapsuleCollider cap = null; var gtTrees = slice.Find("GiantTrees");
        if (gt != null && gtTrees != null) foreach (UnityEngine.Transform t in gtTrees) if ((XZ(t.position) - XZ(gt.position)).sqrMagnitude < 0.01f) cap = t.GetComponentInChildren<UnityEngine.CapsuleCollider>();
        if (cap == null) Line(false, "GATE TREE: no trunk capsule on the drawn stub");
        else
        {
            var c = XZ(cap.bounds.center); float bark = DistTo(jgToT, c) - cap.bounds.extents.x; int gray = gt.GetComponentsInChildren<UnityEngine.Collider>().Length;
            Line(bark >= jgTBark && gray == 0, "GATE TREE: one capsule, half width " + F(cap.bounds.extents.x) + ", bark " + F(bark) + " m off the Jg to T centreline (" + F(jgTBark) + " or more); gray colliders left " + gray);
            var f = FootOn(jgToT, c); var eye = V(f.x, H(f.x, f.y) + eyeH, f.y); var to = V(c.x, eye.y, c.y);
            string first = UnityEngine.Physics.Raycast(eye, (to - eye).normalized, out var hit, (to - eye).magnitude + 1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore) ? WalkIns.PathOf(hit.collider.transform) : "nothing";
            Line(hit.collider == cap, "GATE TREE FACE: from the nearest centreline point (" + F1(f.x) + ", " + F1(f.y) + ") toward the stub the first thing met is " + first);
            var big = new System.Collections.Generic.List<string>();
            foreach (var cc2 in UnityEngine.Object.FindObjectsByType<UnityEngine.CapsuleCollider>(UnityEngine.FindObjectsSortMode.None)) { if (cc2 == cap || cc2.transform.IsChildOf(pc.transform)) continue; float r = cc2.bounds.extents.x; if (r >= bigTrunk && UnityEngine.Vector2.Distance(XZ(cc2.bounds.center), c) <= bigTrunkR) big.Add(WalkIns.PathOf(cc2.transform) + " r " + F(r)); }
            foreach (var mc in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshCollider>(UnityEngine.FindObjectsSortMode.None)) { if (mc.name != "Trunk") continue; float r = mc.bounds.extents.x; if (r >= bigTrunk && UnityEngine.Vector2.Distance(XZ(mc.bounds.center), c) <= bigTrunkR) big.Add(WalkIns.PathOf(mc.transform) + " r " + F(r)); }
            Line(big.Count == 0, "GATE TREE ALONE: trunk colliders of r " + F1(bigTrunk) + " or more within " + F1(bigTrunkR) + " m: " + (big.Count == 0 ? "none" : string.Join("; ", big)));
        }
    }
    {
        const float scopeOff = 8f, stoneOff = 1.4f, stoneTol = 0.1f; var edges = ground815.Find("TrailEdges"); var with = new System.Collections.Generic.List<string>(); float s3 = float.NaN;
        if (edges != null) foreach (UnityEngine.Transform t in edges)
        {
            var q = XZ(t.position); bool near = false; foreach (var n in burnLegs) if (DistTo(Flat(Marks(n)), q) <= scopeOff) near = true;
            if (near && t.GetComponentInChildren<UnityEngine.Collider>() != null) with.Add(t.name + " (" + F1(q.x) + ", " + F1(q.y) + ")");
            if (t.name == "CS_Stone_3" && UnityEngine.Vector2.Distance(q, P(261.4f, 173.3f)) < stoneOff + 0.5f) s3 = DistTo(jgToC1, q);
        }
        Line(edges != null && with.Count == 0, "STONES: trail-edge pieces within " + F1(scopeOff) + " m of a burn trail with a collider: " + (with.Count == 0 ? "none" : string.Join("; ", with)));
        Line(!float.IsNaN(s3) && UnityEngine.Mathf.Abs(s3 - stoneOff) <= stoneTol, "STONES: CS_Stone_3 " + (float.IsNaN(s3) ? "not found" : F(s3) + " m off the Jg to Camp 1 centreline (" + F(stoneOff) + ")"));
    }
    // ---- VERGE, OFFICE, REGROWTH, FORAGE B FIRS
    {
        const float vergeClear = 15f, officeBand = 3.5f, capDrop = 2f, regrowthBand = 10f, regrowthMax = 6f, eastFromM = 50f, eastSpan = 40f, eastMax = 8f, treadHalf = 2f;
        var tower = RootT("Camp").Find("Tower"); var cab = tower.Find("Cab"); var deck = XZ(tower.position); float eyeLow = cab.position.y + (set != null ? set.deckEye : 1.6f);
        var verge = P(419f, 139f); var door = V(344f, 4.3f, 199f);
        var near = new System.Collections.Generic.List<string>();
        foreach (var root in new[] { forest.Find("BurnDeadwood"), slice.Find("BurnRegrowth") }) if (root != null) foreach (UnityEngine.Transform t in root) { var q = XZ(t.position); if (q.x >= 250f && q.x <= 345f && LineD(q, deck, verge) < vergeClear) near.Add(root.name + "/" + t.name + " (" + F1(q.x) + ", " + F1(q.y) + ") " + F1(LineD(q, deck, verge)) + " m"); }
        Line(near.Count == 0, "VERGE: snags and regrowth within " + F1(vergeClear) + " m in plan of the deck-to-verge line, x 250 to 345: " + (near.Count == 0 ? "none" : string.Join("; ", near)));
        var over = new System.Collections.Generic.List<string>(); var dd = XZ(door) - deck;
        foreach (var root in new[] { forest, slice })
            foreach (var t in Pieces(root))
            {
                var tq = XZ(t.position); if (tq.x < 225f || tq.x > 345f || LineD(tq, deck, XZ(door)) > officeBand + 20f) continue;
                foreach (var r in Lod0(t))
                {
                    var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; bool hitCap = false;
                    foreach (var v in mf.sharedMesh.vertices)
                    {
                        var w = r.transform.TransformPoint(v); var q = XZ(w); if (q.x < 230f || q.x > 340f || LineD(q, deck, XZ(door)) > officeBand) continue;
                        float u = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - deck, dd) / dd.sqrMagnitude); float cap = UnityEngine.Mathf.Lerp(eyeLow, door.y, u) - capDrop;
                        if (w.y > cap) { over.Add(WalkIns.PathOf(t) + " at (" + F1(w.x) + ", " + F1(w.y) + ", " + F1(w.z) + "), cap " + F1(cap)); hitCap = true; break; }
                    }
                    if (hitCap) break;
                }
            }
        Line(over.Count == 0, "OFFICE: LOD 0 vertices within " + F1(officeBand) + " m in plan of the deck-to-office line over the cap (the lowest eye's line less " + F1(capDrop) + "): " + (over.Count == 0 ? "none" : string.Join("; ", over.GetRange(0, UnityEngine.Mathf.Min(10, over.Count)))));
        var tall = new System.Collections.Generic.List<string>(); var br = slice.Find("BurnRegrowth");
        if (br != null) foreach (UnityEngine.Transform t in br) { if (DistTo(jgToT, XZ(t.position)) > regrowthBand) continue; float top = PlaceKit.MeshBounds(t.gameObject).max.y - H(t.position.x, t.position.z); if (top > regrowthMax) tall.Add(t.name + " (" + F1(t.position.x) + ", " + F1(t.position.z) + ") " + F1(top) + " m"); }
        Line(tall.Count == 0, "REGROWTH BESIDE JG TO T: within " + F1(regrowthBand) + " m of the line, over " + F1(regrowthMax) + " m: " + (tall.Count == 0 ? "none" : string.Join("; ", tall)));
        var jt = Marks("Jg to T"); int m50 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(eastFromM / 2f), 0, UnityEngine.Mathf.Max(0, jt.Count - 1)); var east = new System.Collections.Generic.List<string>();
        if (jt.Count > 0)
        {
            float x0 = jt[m50].x, x1 = x0 + eastSpan;
            foreach (var root in new[] { forest, slice }) foreach (var t in Pieces(root)) { if (!treeName.IsMatch(t.name)) continue; var q = XZ(t.position); if (q.x < x0 || q.x > x1 || DistTo(jgToT, q) > regrowthBand) continue; float top = PlaceKit.MeshBounds(t.gameObject).max.y - H(q.x, q.y); if (top > eastMax) east.Add(WalkIns.PathOf(t) + " " + F1(top) + " m"); }
            Line(east.Count == 0, "REGROWTH EAST OF M 50: trees within " + F1(regrowthBand) + " m of Jg to T from x " + F1(x0) + " to " + F1(x1) + " over " + F1(eastMax) + " m: " + (east.Count == 0 ? "none" : string.Join("; ", east)));
        }
        var firs = burn.Find("ForageB_Firs"); var firLines = new System.Collections.Generic.List<string>(); bool firOk = firs != null && campToC3.Count >= 2;
        if (firs != null) foreach (UnityEngine.Transform t in firs) { var cap = t.GetComponentInChildren<UnityEngine.CapsuleCollider>(); var c = cap != null ? XZ(cap.bounds.center) : XZ(t.position); float r = cap != null ? cap.bounds.extents.x : 0f; float d = DistTo(campToC3, c) - r; firLines.Add(t.name + " (" + F1(c.x) + ", " + F1(c.y) + ") " + F(d) + " m"); if (d < treadHalf) firOk = false; }
        Line(firOk, "FORAGE B FIRS: trunk surface off the Camp to Camp 3 centreline (" + F1(treadHalf) + " or more): " + (firLines.Count == 0 ? "none found" : string.Join("; ", firLines)));
    }
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.28 burn check (BurnLayout.md draft 2)\n" + sb;
