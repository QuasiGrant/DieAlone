// Main3 8.22 front check (Play mode, Main3; FrontLayout.md draft 2). In main3_review_capture.sh --area front's Play checks; also runs
// alone. Never saves; restores the player, the doors, IW3, the camera and the GPU Resident Drawer. Frames go to outDir at Grant's size.
// DOOR (doc O1): the office west door stands open into the room at Play start, offers no prompt, shuts when held (an Office CHECK), stays
//   shut through a wake while held, and opens again when let go and at the next wake.
// DECK DOOR FRAMES (doc 2.7): from the lectern, eye and binoculars (binoFov degrees vertical, TowerCheck.md 7.2), the door open and shut,
//   in the current look; in the binocular frame, open against shut (the largest RGB channel change), the share of the opening's pixels that change by doorDiffGrey or more
//   must be doorDiffShare or more, or else (doc 2.7: the porch lamp over the door, lit only while it is open) the lamp's pixels must change
//   by doorLampGrey on average ("differ clearly"). From the lectern eye, or, when the cab hides the door from it, the nearest deck eye.
// GATE (doc 2.1, 2.2, 4.1, 4.2): the arm down with a collider; post to booth 1.2 m; IW1 flush with the gate posts; IW1 and IW3 on
//   Ignore Raycast and left out of the PlayerInteractor's mask; IW3 off with no car, on from CarAdmitted to CarParked.
// SWEPT PATHS (doc 2.2): a carL x carW car box, from carLow to carHigh over the ground, every sweepStep m: ADMIT from the gate stop
//   through a turnR m right turn into the spur, up the spur past the chain to the ring join; REFUSE backing refuseBack m out onto the
//   apron; the apron and the drive to the T clear; the ring road all round and every pitch with its car nose-in. Allowed: the barrier arm
//   (lifted), the chain (dropped), IW1 and IW3 (player only). Top-down frames with the swept boxes drawn: Paths_Gate.png, Paths_Ring.png.
// SIGHTLINES (Wren; doc 5.5): S1 the car's hood (372.5, ground + 1.2, 179.4) to the verge tree trunk (418, 20, 136); S2 the Ward
//   lookout (29, ground + 1.6, 261) to the T (428, ground + 1, 170), with the road z 140 to 200 in frame. Every drawn mesh blocks
//   (temporary exact colliders); a hit on the target's own object reaches it. Frames S1.png and S2.png.
// PLACES ON OBJECTS (doc 5.3): every front place with an objectPath lies on its object (within placeSlack m of its drawn bounds).
// GAPS (doc header: every gap under 0.6 m or 1.0 m and over): between built front-zone pieces and anything near them, grouped by object,
//   reaching to within reachLow m of the ground, a slot of gapLow to gapHigh m with nothing else in it fails.
// WALKS (doc 3): the doc's legs with the real mover (PlayerController.Step, dt 0.02, walk speed), each arriving, with times.
string look = "";   // empty: the current look; or a LookPreview row label, selected first ("run again")
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_front");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
if (look != "") { var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>(); if (pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; } }
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
const float dt = 0.02f, arrive = 0.5f, legTime = 60f, eyeH = 1.6f, binoFov = 15f, doorDiffGrey = 16f, doorDiffShare = 0.5f, doorLampGrey = 32f;
const float carL = 4.5f, carW = 1.8f, carLow = 0.3f, carHigh = 1.5f, sweepStep = 0.5f, turnR = 5.5f, refuseBack = 14f, ringCX = 372f, ringCZ = 262f, ringR = 17.5f;
const float placeSlack = 0.3f, gapLow = 0.6f, gapHigh = 1.0f, gapTol = 0.005f, bodyLow = 0.3f, bodyHigh = 1.8f, reachLow = 0.5f, gapNear = 1.0f;
const int shotW = 3840, shotH = 1976;
var fz = Root("FrontZone").transform; var office = fz.Find("Office");
var sb = new System.Text.StringBuilder(); int fails = 0;
void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
System.IO.Directory.CreateDirectory(outDir);
// ---- rendering at Grant's size, the GPU Resident Drawer off in memory (Camera.Render skips its objects)
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
void Pose(UnityEngine.Vector3 eye, UnityEngine.Vector3 lookAt)
{
    var dir = lookAt - eye; var flat = V(dir.x, 0f, dir.z); if (flat.sqrMagnitude < 1e-6f) flat = UnityEngine.Vector3.forward;
    cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = eye - pc.transform.rotation * camLocal;
    cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
}
UnityEngine.Color32[] Shoot(string file)   // rendered twice, the second kept (the first after a camera move differs)
{
    cam.targetTexture = rt; cam.Render(); cam.Render(); cam.targetTexture = null;
    UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
    if (file != null) System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), shot.EncodeToPNG());
    return shot.GetPixels32();
}
UnityEngine.Rect ScreenRect(UnityEngine.Vector3[] pts)
{
    cam.targetTexture = rt; float x0 = float.MaxValue, y0 = float.MaxValue, x1 = float.MinValue, y1 = float.MinValue;
    foreach (var p in pts) { var s = cam.WorldToScreenPoint(p); x0 = UnityEngine.Mathf.Min(x0, s.x); y0 = UnityEngine.Mathf.Min(y0, s.y); x1 = UnityEngine.Mathf.Max(x1, s.x); y1 = UnityEngine.Mathf.Max(y1, s.y); }
    cam.targetTexture = null; return new UnityEngine.Rect(x0, y0, x1 - x0, y1 - y0);
}
// ---- every drawn mesh as a blocker along given segments (temporary exact colliders, removed in finally)
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
void MeshBlockers(System.Collections.Generic.List<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)> segs)
{
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>();
    foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var lods = lod.GetLODs(); for (int li = 1; li < lods.Length; li++) foreach (var r in lods[li].renderers) if (r != null) notLod0.Add(r); }
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform) || mr.name.Contains("Glass")) continue;   // glass is seen through
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var b = mr.bounds; bool crossed = false;
        foreach (var (a, e) in segs) { var d = e - a; if (b.IntersectRay(new UnityEngine.Ray(a, d.normalized), out float dist) && dist <= d.magnitude) { crossed = true; break; } }
        if (!crossed) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
}
string FirstBlock(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own)   // null when clear
{
    var d = b - a; float firstOther = float.MaxValue, firstOwn = float.MaxValue; string what = null;
    foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
    {
        var ht = h.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2) continue;
        if (own != null && ht.IsChildOf(own)) firstOwn = UnityEngine.Mathf.Min(firstOwn, h.distance); else if (h.distance < firstOther) { firstOther = h.distance; what = WalkIns.PathOf(ht) + " at " + F1(h.distance) + " m" + (temps.Contains(h.collider) ? " (drawn mesh)" : ""); }
    }
    return firstOther == float.MaxValue || firstOwn < firstOther ? null : what;
}
UnityEngine.Transform westDoorT = office.Find("WestDoor"); var westDoor = westDoorT != null ? westDoorT.GetComponent<Door>() : null;
var backDoorT = office.Find("BackDoor"); UnityEngine.Collider backPanel = backDoorT != null ? backDoorT.GetComponentInChildren<UnityEngine.BoxCollider>() : null;
var spurWall = UnityEngine.Object.FindFirstObjectByType<SpurWall>();
var drawn = new System.Collections.Generic.List<UnityEngine.GameObject>();
try
{
    // ================= DOOR =================
    if (westDoor == null) Line(false, "DOOR: no FrontZone/Office/WestDoor");
    else
    {
        var panel = westDoorT.GetComponentInChildren<UnityEngine.BoxCollider>(); float inside = panel.bounds.center.x;
        bool startOk = westDoor.IsOpen && westDoor.CurrentAngle > 0f && inside > westDoorT.position.x + 0.3f;
        bool noPrompt = !westDoor.CanUse(null);
        westDoor.HoldShut(true, true); bool shut = !westDoor.IsOpen && UnityEngine.Mathf.Abs(westDoor.CurrentAngle) < 0.01f;
        WakeSignal.Raise(); bool heldThroughWake = !westDoor.IsOpen;
        westDoor.HoldShut(false, true); bool reopened = westDoor.IsOpen && westDoor.CurrentAngle > 0f;
        WakeSignal.Raise(); bool wakeOpen = westDoor.IsOpen && westDoor.CurrentAngle > 0f;
        Line(startOk && noPrompt && shut && heldThroughWake && reopened && wakeOpen, "DOOR: at Play start open " + startOk + " (angle " + F1(westDoor.CurrentAngle) + ", leaf centre x " + F(inside) + ", inside the room), no prompt " + noPrompt + "; held shut " + shut + ", still shut after a wake " + heldThroughWake + ", let go opens " + reopened + ", open after a wake " + wakeOpen);
    }
    // ================= DECK DOOR FRAMES =================
    if (westDoor != null)
    {
        var set = Main3AreaSet.Load(); var campA = set != null ? set.Find("camp") : null; UnityEngine.Vector3 stand = V(164f, 56.03f, 166.3f);
        if (campA != null && campA.interactions != null) foreach (var ia in campA.interactions) if (ia.label == "lectern") stand = ia.approach;
        var lecternEye = stand + UnityEngine.Vector3.up * eyeH; var opening = V(344.0f, 4.3f, 199.0f);
        var corners = new[] { V(344.0f, 3.05f, 198.4f), V(344.0f, 3.05f, 199.6f), V(344.0f, 5.5f, 198.4f), V(344.0f, 5.5f, 199.6f) };   // the opening and the porch lamp over it
        // the lectern eye first; when the cab hides the door from it, the nearest deck eye (the area check's grid) that sees the opening
        // past every drawn mesh, the tower included
        var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y;
        var cands = new System.Collections.Generic.List<UnityEngine.Vector3> { lecternEye };
        if (set != null) for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) cands.Add(V(tower.position.x + gx, deckTop + set.deckEye, tower.position.z + gz));
        var dsegs = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3)>(); foreach (var c in cands) dsegs.Add((c, opening)); MeshBlockers(dsegs);
        string lecternBlock = FirstBlock(lecternEye, opening, westDoorT); var eye = lecternEye; float bestD = float.MaxValue;
        if (lecternBlock != null) foreach (var c in cands) { if (FirstBlock(c, opening, westDoorT) != null) continue; float d = UnityEngine.Vector3.Distance(c, lecternEye); if (d < bestD) { bestD = d; eye = c; } }
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        if (lecternBlock != null) { cam.fieldOfView = camFov; Pose(lecternEye, opening); Shoot("DeckDoor_Lectern_Eye.png"); }
        sb.Append("NOTE DECK DOOR EYE: from the lectern stand (" + F1(stand.x) + ", " + F1(stand.z) + ") the opening is " + (lecternBlock == null ? "clear" : "hidden by " + lecternBlock + " (DeckDoor_Lectern_Eye.png); the frames below are from the nearest clear deck eye (" + F1(eye.x) + ", " + F1(eye.y) + ", " + F1(eye.z) + "), " + (bestD == float.MaxValue ? "NONE CLEAR" : F1(bestD) + " m from it")) + "\n");
        UnityEngine.Color32[] binoOpen = null, binoShut = null, binoShut2 = null; UnityEngine.Rect binoRect = default, eyeRect = default;
        foreach (var state in new[] { "Open", "Shut" })
        {
            westDoor.HoldShut(state == "Shut", true); foreach (var dl in UnityEngine.Object.FindObjectsByType<DoorLamp>(UnityEngine.FindObjectsSortMode.None)) dl.Sync();
            foreach (var bino in new[] { false, true })
            {
                cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, opening);
                var r = ScreenRect(corners); if (bino) binoRect = r; else eyeRect = r;
                var px = Shoot("DeckDoor_" + state + (bino ? "_Binoculars" : "_Eye") + ".png");
                if (bino) { if (state == "Open") binoOpen = px; else { binoShut = px; binoShut2 = Shoot(null); } }   // a second shut frame: the render noise between two frames of one state
            }
        }
        westDoor.HoldShut(false, true); foreach (var dl in UnityEngine.Object.FindObjectsByType<DoorLamp>(UnityEngine.FindObjectsSortMode.None)) dl.Sync(); cam.fieldOfView = camFov;
        (float share, float mean, int n) Diff(UnityEngine.Rect rr, UnityEngine.Color32[] pa, UnityEngine.Color32[] pb)
        {
            int total = 0, changed = 0; float sum = 0f;
            for (int y = UnityEngine.Mathf.Max(0, UnityEngine.Mathf.FloorToInt(rr.yMin)); y <= UnityEngine.Mathf.Min(shotH - 1, UnityEngine.Mathf.CeilToInt(rr.yMax)); y++)
                for (int x = UnityEngine.Mathf.Max(0, UnityEngine.Mathf.FloorToInt(rr.xMin)); x <= UnityEngine.Mathf.Min(shotW - 1, UnityEngine.Mathf.CeilToInt(rr.xMax)); x++)
                {
                    var a = pa[y * shotW + x]; var b = pb[y * shotW + x]; float d = UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(a.r - b.r), UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(a.g - b.g), UnityEngine.Mathf.Abs(a.b - b.b)));   // the largest channel change: the lamp is a hue change as much as a grey one
                    total++; sum += d; if (d >= doorDiffGrey) changed++;
                }
            return (total > 0 ? changed / (float)total : 0f, total > 0 ? sum / total : 0f, total);
        }
        // the opening alone, and the porch lamp's glow over it (doc 2.7's fallback when the opening does not differ clearly)
        cam.fieldOfView = binoFov; Pose(eye, opening); var openRect = ScreenRect(new[] { corners[0], corners[1], V(344.0f, 5.2f, 198.4f), V(344.0f, 5.2f, 199.6f) });
        var lampT = office.Find("Porch/DoorLamp/Lamp/Glow"); UnityEngine.Rect lampRect = default; if (lampT != null) { var lb = lampT.GetComponent<UnityEngine.Renderer>().bounds; lampRect = ScreenRect(new[] { lb.min, lb.max, V(lb.min.x, lb.min.y, lb.max.z), V(lb.max.x, lb.max.y, lb.min.z) }); }
        cam.fieldOfView = camFov;
        var dOpen = Diff(openRect, binoOpen, binoShut); var dNoise = Diff(openRect, binoShut2, binoShut); (float share, float mean, int n) dLamp = lampT != null ? Diff(lampRect, binoOpen, binoShut) : (0f, 0f, 0); (float share, float mean, int n) lNoise = lampT != null ? Diff(lampRect, binoShut2, binoShut) : (0f, 0f, 0);
        var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
        Line(dOpen.share - dNoise.share >= doorDiffShare || dLamp.mean - lNoise.mean >= doorLampGrey, "DECK DOOR FRAMES (" + (pv != null ? pv.CurrentLabel : "?") + " look, deck eye " + F1(eye.x) + ", " + F1(eye.y) + ", " + F1(eye.z) + ", " + F1(UnityEngine.Vector3.Distance(eye, opening)) + " m): the opening is " + F1(eyeRect.width) + " x " + F1(eyeRect.height) + " px by eye, " + F1(binoRect.width) + " x " + F1(binoRect.height) + " px with the lamp in binoculars; open against shut in binoculars, the opening " + (dOpen.share * 100f).ToString("F0", inv) + " percent of " + dOpen.n + " px change by " + F1(doorDiffGrey) + " levels or more in a channel (bar " + (doorDiffShare * 100f).ToString("F0", inv) + "), mean " + F1(dOpen.mean) + ", against render noise between two shut frames " + (dNoise.share * 100f).ToString("F0", inv) + " percent, mean " + F1(dNoise.mean) + "; the porch lamp's " + dLamp.n + " px change by " + F1(dLamp.mean) + " levels on average, noise " + F1(lNoise.mean) + " (bar " + F1(doorLampGrey) + " over the noise) | DeckDoor_Open_Eye.png, _Binoculars.png, DeckDoor_Shut_Eye.png, _Binoculars.png");
    }
    // ================= GATE =================
    {
        var barrier = fz.Find("Gate/Barrier"); var armC = barrier != null ? barrier.Find("BarrierArm")?.GetComponent<UnityEngine.Collider>() : null; var postC = barrier != null ? barrier.Find("Post")?.GetComponent<UnityEngine.Collider>() : null;
        var boothB = new UnityEngine.Bounds(); bool anyB = false; var booth = fz.Find("GateBooth"); if (booth != null) foreach (var c in booth.GetComponentsInChildren<UnityEngine.Collider>()) { if (!anyB) { boothB = c.bounds; anyB = true; } else boothB.Encapsulate(c.bounds); }
        float postToBooth = postC != null && anyB ? boothB.min.x - postC.bounds.max.x : -1f;
        Line(armC != null && armC.enabled && postC != null && postToBooth >= 1.0f - gapTol, "BARRIER: arm down with its collider " + (armC != null && armC.enabled) + " (z " + (armC != null ? F(armC.bounds.min.z) + " to " + F(armC.bounds.max.z) + ", " + F(armC.bounds.min.y - H(389.3f, 169f)) + " to " + F(armC.bounds.max.y - H(389.3f, 169f)) + " m up" : "-") + "); post to booth " + F(postToBooth) + " m");
        var iw1 = fz.Find("Gate/PlayerBlocker"); var iw1C = iw1 != null ? iw1.GetComponent<UnityEngine.BoxCollider>() : null; var posts = new System.Collections.Generic.List<UnityEngine.Collider>(); foreach (UnityEngine.Transform c in fz.Find("Gate")) if (c.name == "Post") posts.Add(c.GetComponent<UnityEngine.Collider>());
        bool flush = iw1C != null && posts.Count == 2 && UnityEngine.Mathf.Abs(iw1C.bounds.min.z - UnityEngine.Mathf.Min(posts[0].bounds.max.z, posts[1].bounds.max.z)) < 0.01f && UnityEngine.Mathf.Abs(iw1C.bounds.max.z - UnityEngine.Mathf.Max(posts[0].bounds.min.z, posts[1].bounds.min.z)) < 0.01f;
        var pi = pc.GetComponent<PlayerInteractor>(); int mask = pi != null ? new UnityEditor.SerializedObject(pi).FindProperty("mask").intValue : -1;
        var iw3 = spurWall != null ? spurWall.transform : null;
        bool layers = iw1 != null && iw1.gameObject.layer == 2 && iw3 != null && iw3.gameObject.layer == 2 && (mask & (1 << 2)) == 0;
        Line(flush && layers, "IW1: z " + (iw1C != null ? F(iw1C.bounds.min.z) + " to " + F(iw1C.bounds.max.z) : "-") + ", flush with the gate posts " + flush + "; IW1 and IW3 on Ignore Raycast and out of the interactor's mask " + layers + " (mask " + mask + ")");
        if (spurWall == null) Line(false, "IW3: no SpurWall");
        else { bool off0 = !spurWall.IsOn; spurWall.CarAdmitted(); bool on1 = spurWall.IsOn; spurWall.CarParked(); bool off2 = !spurWall.IsOn; var b = spurWall.GetComponent<UnityEngine.BoxCollider>(); b.enabled = true; var bb = b.bounds; b.enabled = false;
            Line(off0 && on1 && off2, "IW3: x " + F(bb.min.x) + " to " + F(bb.max.x) + " at z " + F(bb.center.z) + "; off with no car " + off0 + ", on while an admitted car is on the spur " + on1 + ", off once it is parked " + off2); }
    }
    // ================= SWEPT PATHS =================
    {
        var allowed = new System.Collections.Generic.List<UnityEngine.Transform> { fz.Find("Gate/Barrier/BarrierArm"), fz.Find("Chain/Chain"), fz.Find("Gate/PlayerBlocker"), spurWall != null ? spurWall.transform : null };
        bool Allowed(UnityEngine.Collider c) { if (c is UnityEngine.TerrainCollider) return true; foreach (var a in allowed) if (a != null && c.transform.IsChildOf(a)) return true; return false; }
        var stopsT = fz.Find("CarStops"); UnityEngine.Vector3 Stop(string n) { var t = stopsT != null ? stopsT.Find(n) : null; return t != null ? t.position : V(float.NaN, 0f, 0f); }
        var swept = new System.Collections.Generic.List<(UnityEngine.Vector3 c, float yaw)>();
        string Sweep(string name, System.Collections.Generic.List<(UnityEngine.Vector3 c, float yaw)> poses)
        {
            var hits = new System.Collections.Generic.SortedDictionary<string, int>();
            foreach (var (c, yaw) in poses)
            {
                swept.Add((c, yaw)); float g = H(c.x, c.z);
                foreach (var col in UnityEngine.Physics.OverlapBox(V(c.x, g + (carLow + carHigh) * 0.5f, c.z), V(carW * 0.5f, (carHigh - carLow) * 0.5f, carL * 0.5f), UnityEngine.Quaternion.Euler(0f, yaw, 0f), ~0, UnityEngine.QueryTriggerInteraction.Ignore))
                    if (!Allowed(col)) { var k = WalkIns.PathOf(col.transform); hits[k] = hits.TryGetValue(k, out var n) ? n + 1 : 1; }
            }
            return hits.Count == 0 ? null : string.Join("; ", System.Linq.Enumerable.Select(hits, kv => kv.Key + " (" + kv.Value + " poses)"));
        }
        System.Collections.Generic.List<(UnityEngine.Vector3, float)> Poly(params UnityEngine.Vector3[] pts)
        {
            var l = new System.Collections.Generic.List<(UnityEngine.Vector3, float)>();
            for (int i = 1; i < pts.Length; i++) { var a = pts[i - 1]; var b = pts[i]; var d = V(b.x - a.x, 0f, b.z - a.z); float yaw = UnityEngine.Quaternion.LookRotation(d.normalized).eulerAngles.y; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(d.magnitude / sweepStep)); for (int k = 0; k <= n; k++) l.Add((UnityEngine.Vector3.Lerp(a, b, k / (float)n), yaw)); }
            return l;
        }
        var gs = Stop("Gate_Stop"); var back = Stop("Refused_Backed");
        // ADMIT: west from the stop, a right turn of turnR m onto the spur centre (x 385), then the spur to the ring join
        var admit = new System.Collections.Generic.List<(UnityEngine.Vector3, float)>(); float xc = 385f + turnR, zc = gs.z + turnR;
        admit.AddRange(Poly(gs, V(xc, gs.y, gs.z)));
        for (float a = 0f; a <= 90f; a += 5f) { float r = a * UnityEngine.Mathf.Deg2Rad; admit.Add((V(xc - turnR * UnityEngine.Mathf.Sin(r), gs.y, zc - turnR * UnityEngine.Mathf.Cos(r)), 270f + a)); }
        admit.AddRange(Poly(V(385f, gs.y, zc), V(385f, gs.y, 186f), V(390f, gs.y, 196f), V(390f, gs.y, 242f), V(387.5f, gs.y, 252f)));
        var admitHit = Sweep("admit", admit);
        Line(admitHit == null, "ADMIT PATH: gate stop (" + F1(gs.x) + ", " + F1(gs.z) + ") through a " + F1(turnR) + " m turn into the spur, past the chain to the ring join, " + admit.Count + " car poses: " + (admitHit ?? "clear"));
        var refuse = Poly(gs, back); var apron = new System.Collections.Generic.List<(UnityEngine.Vector3, float)>();
        for (float x = 400f + carW; x <= 412f - carW; x += 1f) for (float z = 162f + carL * 0.5f; z <= 178f - carL * 0.5f; z += 1f) foreach (var yaw in new[] { 0f, 45f, 90f, 135f }) apron.Add((V(x, gs.y, z), yaw));
        var outPoly = Poly(V(406f, gs.y, 170f), V(424.25f, gs.y, 170f));
        var refuseHit = Sweep("refuse", refuse); var apronHit = Sweep("apron", apron); var outHit = Sweep("out", outPoly);
        Line(refuseHit == null && apronHit == null && outHit == null, "REFUSE PATH: backing " + F1(refuseBack) + " m to (" + F1(back.x) + ", " + F1(back.z) + "), front at x " + F1(back.x - carL * 0.5f) + ": " + (refuseHit ?? "clear") + "; the apron x 400 to 412, z 162 to 178 (" + apron.Count + " turning poses): " + (apronHit ?? "clear") + "; the drive out to the T: " + (outHit ?? "clear"));
        int gateSwept = swept.Count;
        var ring = new System.Collections.Generic.List<(UnityEngine.Vector3, float)>(); for (float a = 0f; a < 360f; a += 3f) { float r = a * UnityEngine.Mathf.Deg2Rad; var p = V(ringCX + ringR * UnityEngine.Mathf.Cos(r), gs.y, ringCZ + ringR * UnityEngine.Mathf.Sin(r)); ring.Add((p, UnityEngine.Quaternion.LookRotation(V(-UnityEngine.Mathf.Sin(r), 0f, UnityEngine.Mathf.Cos(r))).eulerAngles.y)); }
        var ringHit = Sweep("ring", ring); var pitchHits = new System.Text.StringBuilder(); int pitchBad = 0;
        for (int i = 1; i <= 8; i++) { var t = stopsT != null ? stopsT.Find("P" + i) : null; if (t == null) { pitchBad++; pitchHits.Append("P" + i + " missing; "); continue; } var h = Sweep("P" + i, new System.Collections.Generic.List<(UnityEngine.Vector3, float)> { (t.position, t.eulerAngles.y) }); if (h != null) { pitchBad++; pitchHits.Append("P" + i + ": " + h + "; "); } }
        Line(ringHit == null && pitchBad == 0, "RING AND PITCHES: the ring road all round (" + ring.Count + " poses, counter-clockwise): " + (ringHit ?? "clear") + "; P1 to P8 with a car nose-in: " + (pitchBad == 0 ? "clear" : pitchHits.ToString()));
        // top-down frames of the swept boxes (temporary flat unlit quads, removed in finally)
        var mat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit")); mat.SetColor("_BaseColor", new UnityEngine.Color(1f, 0.25f, 0.1f, 1f));
        for (int i = 0; i < swept.Count; i += 2) { var (c, yaw) = swept[i]; var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad); UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>()); q.transform.position = V(c.x, H(c.x, c.z) + 0.12f, c.z); q.transform.rotation = UnityEngine.Quaternion.Euler(90f, yaw, 0f); q.transform.localScale = V(carW, carL, 1f); q.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; drawn.Add(q); }
        cam.fieldOfView = camFov; Pose(V(394f, 3f + 70f, 186f), V(394f, 3f, 186.01f)); Shoot("Paths_Gate.png");
        Pose(V(375f, 3f + 80f, 230f), V(375f, 3f, 230.01f)); Shoot("Paths_Ring.png");
        foreach (var q in drawn) UnityEngine.Object.DestroyImmediate(q); drawn.Clear();
    }
    // ================= SIGHTLINES =================
    {
        float Ground(float x, float z) { foreach (var h in UnityEngine.Physics.RaycastAll(V(x, H(x, z) + 30f, z), UnityEngine.Vector3.down, 60f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (h.collider is UnityEngine.TerrainCollider || h.normal.y > 0.7f) return h.point.y; return H(x, z); }
        var hood = V(372.5f, H(372.5f, 179.4f) + 1.2f, 179.4f); var trunk = V(418f, 20f, 136f);
        var lookout = V(29f, Ground(29f, 261f) + eyeH, 261f); var tPt = V(428f, H(428f, 170f) + 1f, 170f);
        var roadPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (float z = 140f; z <= 200.01f; z += 10f) roadPts.Add(V(428f, H(428f, z) + 0.3f, z));
        var segs = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3)> { (hood, trunk), (lookout, tPt) }; foreach (var p in roadPts) segs.Add((lookout, p));
        MeshBlockers(segs);
        var verge = fz.Find("VergeTree"); var car = fz.Find("Resident_Car");
        string s1 = FirstBlock(hood, trunk, verge); Line(s1 == null, "S1: the car's hood " + "(" + F1(hood.x) + ", " + F1(hood.y) + ", " + F1(hood.z) + ") to the verge tree trunk (418, 20, 136), " + F1(UnityEngine.Vector3.Distance(hood, trunk)) + " m, every drawn mesh: " + (s1 ?? "clear") + " | S1.png");
        string s2 = FirstBlock(lookout, tPt, null); int roadClear = 0; var roadBlocks = new System.Collections.Generic.List<string>(); foreach (var p in roadPts) { var bl = FirstBlock(lookout, p, null); if (bl == null) roadClear++; else roadBlocks.Add("z " + F1(p.z) + ": " + bl); }
        cam.fieldOfView = camFov; Pose(lookout, tPt); cam.targetTexture = rt; int inFrame = 0; foreach (var p in roadPts) { var vp = cam.WorldToViewportPoint(p); if (vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f) inFrame++; } cam.targetTexture = null;
        Shoot("S2.png");
        Line(s2 == null && roadClear == roadPts.Count && inFrame == roadPts.Count, "S2: the Ward lookout (" + F1(lookout.x) + ", " + F1(lookout.y) + ", " + F1(lookout.z) + ") to the T (428, " + F1(tPt.y) + ", 170), " + F1(UnityEngine.Vector3.Distance(lookout, tPt)) + " m: " + (s2 ?? "clear") + "; road points z 140 to 200 clear " + roadClear + " of " + roadPts.Count + (roadBlocks.Count > 0 ? " (" + string.Join("; ", roadBlocks) + ")" : "") + ", in frame " + inFrame + " of " + roadPts.Count + " | S2.png");
        Pose(hood, trunk); Shoot("S1.png");
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
    }
    // ================= PLACES ON OBJECTS =================
    {
        var set = Main3AreaSet.Load(); var A = set != null ? set.Find("front") : null; int bad = 0; var pl = new System.Text.StringBuilder();
        if (A == null) Line(false, "PLACES: no front area in Assets/Settings/Main3Areas.asset");
        else
        {
            foreach (var p in A.places)
            {
                if (string.IsNullOrEmpty(p.objectPath)) continue; var t = Main3AreaSet.At(scene, p.objectPath); if (t == null) { bad++; pl.Append(p.label + ": no " + p.objectPath + "; "); continue; }
                var b = PlaceKit.MeshBounds(t.gameObject); foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) b.Encapsulate(c.bounds); b.Expand(2f * placeSlack);
                bool on = p.point.x >= b.min.x && p.point.x <= b.max.x && p.point.z >= b.min.z && p.point.z <= b.max.z; if (!on) { bad++; pl.Append(p.label + " (" + F1(p.point.x) + ", " + F1(p.point.z) + ") off " + p.objectPath + " (x " + F1(b.min.x) + " to " + F1(b.max.x) + ", z " + F1(b.min.z) + " to " + F1(b.max.z) + "); "); }
            }
            Line(bad == 0, "PLACES ON OBJECTS: every front place with an object lies on it" + (bad > 0 ? ": " + pl : ""));
        }
    }
    // ================= GAPS =================
    {
        UnityEngine.Physics.SyncTransforms();
        UnityEngine.Transform Group(UnityEngine.Collider c) { var root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(c.gameObject); return root != null ? root.transform : c.transform; }
        bool Body(UnityEngine.Bounds b) { float g = H(b.center.x, b.center.z); return b.max.y > g + bodyLow && b.min.y < g + reachLow; }
        var groups = new System.Collections.Generic.Dictionary<UnityEngine.Transform, UnityEngine.Bounds>(); var ofGroup = new System.Collections.Generic.Dictionary<UnityEngine.Collider, UnityEngine.Transform>();
        void Add(UnityEngine.Collider c) { if (!c.enabled || c.isTrigger || c is UnityEngine.TerrainCollider || c.transform.IsChildOf(pc.transform)) return; var g = Group(c); ofGroup[c] = g; var b = c.bounds; if (!Body(b)) return; if (groups.TryGetValue(g, out var gb)) { gb.Encapsulate(b); groups[g] = gb; } else groups[g] = b; }
        var built = new System.Collections.Generic.HashSet<UnityEngine.Transform>();
        foreach (var c in fz.GetComponentsInChildren<UnityEngine.Collider>()) { var p = WalkIns.PathOf(c.transform); if (p.StartsWith("FrontZone/Highway") || p.StartsWith("FrontZone/Surfaces")) continue; Add(c); if (ofGroup.TryGetValue(c, out var g)) built.Add(g); }
        var bands = new System.Collections.Generic.List<string>(); var seenPair = new System.Collections.Generic.HashSet<string>();
        foreach (var g in System.Linq.Enumerable.ToArray(built))
        {
            if (!groups.TryGetValue(g, out var gb)) continue; var probe = gb; probe.Expand(V(2f * gapNear, 0f, 2f * gapNear));
            foreach (var c in UnityEngine.Physics.OverlapBox(probe.center, probe.extents, UnityEngine.Quaternion.identity, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!ofGroup.ContainsKey(c)) Add(c);
            foreach (var kv in System.Linq.Enumerable.ToArray(groups))
            {
                var o = kv.Key; if (o == g) continue; var ob = kv.Value; if (!ob.Intersects(probe)) continue;
                string key = g.GetInstanceID() < o.GetInstanceID() ? g.GetInstanceID() + "_" + o.GetInstanceID() : o.GetInstanceID() + "_" + g.GetInstanceID(); if (!seenPair.Add(key)) continue;
                float gx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(gb.min.x - ob.max.x, ob.min.x - gb.max.x)), gz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(gb.min.z - ob.max.z, ob.min.z - gb.max.z)); float gap = UnityEngine.Mathf.Sqrt(gx * gx + gz * gz);
                if (gap < gapLow || gap >= gapHigh - gapTol) continue;
                var p1 = V(UnityEngine.Mathf.Clamp(ob.center.x, gb.min.x, gb.max.x), 0f, UnityEngine.Mathf.Clamp(ob.center.z, gb.min.z, gb.max.z)); var p2 = V(UnityEngine.Mathf.Clamp(p1.x, ob.min.x, ob.max.x), 0f, UnityEngine.Mathf.Clamp(p1.z, ob.min.z, ob.max.z));
                var m = (p1 + p2) * 0.5f; m.y = H(m.x, m.z) + 1f; bool other = false;   // anything else in the slot (a wall, a third piece): no slot to walk into
                foreach (var c in UnityEngine.Physics.OverlapSphere(m, gap * 0.5f - 0.01f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (c is UnityEngine.TerrainCollider || !c.enabled) continue; var cg = ofGroup.TryGetValue(c, out var x) ? x : Group(c); if (cg != g && cg != o) { other = true; break; } }
                if (!other) bands.Add(F(gap) + " m between " + WalkIns.PathOf(g) + " and " + WalkIns.PathOf(o) + " at (" + F1(m.x) + ", " + F1(m.z) + ")");
            }
        }
        Line(bands.Count == 0, "GAPS: " + built.Count + " built front-zone pieces and their neighbours, slots of " + F1(gapLow) + " to " + F1(gapHigh) + " m: " + bands.Count + (bands.Count > 0 ? "\n  " + string.Join("\n  ", bands) : ""));
    }
    // ================= WALKS =================
    {
        void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = V(p.x, H(p.x, p.z) + 0.1f, p.z); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 10; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
        bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        if (backPanel != null) backPanel.enabled = false;   // the back-room door stands open for the walk (the player opens it with Interact)
        var legs = new (string name, UnityEngine.Vector3[] pts)[] {
            ("T board to store porch", new[] { V(339.5f, 0f, 171f), V(365f, 0f, 192.6f) }),
            ("store porch to west door", new[] { V(365f, 0f, 192.6f), V(343.0f, 0f, 192.4f), V(342.9f, 0f, 199.0f), V(344.6f, 0f, 199.0f) }),
            ("west door to talk spot", new[] { V(344.6f, 0f, 199.0f), V(345.75f, 0f, 199.0f) }),
            ("talk spot round the counter to the back room, R5's cot", new[] { V(345.75f, 0f, 199.0f), V(345.6f, 0f, 202.2f), V(350.0f, 0f, 202.0f), V(351.3f, 0f, 201.0f), V(353.3f, 0f, 201.0f), V(354.05f, 0f, 202.55f) }),
            ("west door to R6's driver door", new[] { V(344.6f, 0f, 199.0f), V(342.9f, 0f, 199.0f), V(343.0f, 0f, 192.4f), V(370.6f, 0f, 181.3f) }),
            ("R6's bay to the booth door, in", new[] { V(370.6f, 0f, 181.3f), V(374f, 0f, 175.5f), V(380f, 0f, 166.0f), V(389.9f, 0f, 163.5f), V(391.9f, 0f, 163.5f), V(391.9f, 0f, 165.4f) }),
            ("booth door to the T board", new[] { V(391.9f, 0f, 165.4f), V(391.9f, 0f, 163.5f), V(380f, 0f, 166.0f), V(339.5f, 0f, 171f) }),
            ("booth door to the chain", new[] { V(391.9f, 0f, 165.4f), V(391.9f, 0f, 163.5f), V(389.9f, 0f, 163.5f), V(388.6f, 0f, 165.6f), V(386.5f, 0f, 172.8f), V(385f, 0f, 176f), V(385f, 0f, 186f), V(390f, 0f, 196f), V(390f, 0f, 236.6f) }),
            ("round the chain's east post, the ring and back", RingWalk()),
            ("T board to the toilet, in", new[] { V(339.5f, 0f, 171f), V(342.6f, 0f, 174f), V(342.6f, 0f, 186f), V(341.3f, 0f, 186f) }),
            ("west door to the toilet, in", new[] { V(344.6f, 0f, 199.0f), V(342.9f, 0f, 199.0f), V(342.7f, 0f, 192.4f), V(342.6f, 0f, 186f), V(341.3f, 0f, 186f) }) };
        UnityEngine.Vector3[] RingWalk() { var l = new System.Collections.Generic.List<UnityEngine.Vector3> { V(390f, 0f, 236.6f), V(393.9f, 0f, 236.6f), V(393.9f, 0f, 239.6f), V(387.5f, 0f, 252f) }; float a0 = UnityEngine.Mathf.Atan2(252f - ringCZ, 387.5f - ringCX); for (int k = 1; k <= 24; k++) { float a = a0 + k * UnityEngine.Mathf.PI * 2f / 24f; l.Add(V(ringCX + ringR * UnityEngine.Mathf.Cos(a), 0f, ringCZ + ringR * UnityEngine.Mathf.Sin(a))); } l.Add(V(393.9f, 0f, 239.6f)); l.Add(V(393.9f, 0f, 236.6f)); l.Add(V(390f, 0f, 236.6f)); return l.ToArray(); }
        float speed = tuning != null ? tuning.walkSpeed : 2.5f;
        foreach (var (name, pts) in legs)
        {
            Put(pts[0]); float time = 0f, len = 0f; bool ok = true; string where = ""; for (int i = 1; i < pts.Length; i++) len += V(pts[i].x - pts[i - 1].x, 0f, pts[i].z - pts[i - 1].z).magnitude;
            for (int i = 1; i < pts.Length && ok; i++) if (!Walk(pts[i], ref time, out float left)) { ok = false; where = ", stops " + F(left) + " m short of (" + F1(pts[i].x) + ", " + F1(pts[i].z) + ") at (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.z) + ")"; }
            Line(ok, "WALK: " + name + ", " + F1(len) + " m, " + F1(time) + " s at " + F1(speed) + " m/s" + where);
        }
        if (backPanel != null) backPanel.enabled = true;
    }
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); foreach (var q in drawn) if (q != null) UnityEngine.Object.DestroyImmediate(q);
    if (westDoor != null) westDoor.HoldShut(false, true); if (backPanel != null) backPanel.enabled = true;
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas;
    UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.22 front check (FrontLayout.md draft 2); frames in " + outDir + "\n" + sb;
