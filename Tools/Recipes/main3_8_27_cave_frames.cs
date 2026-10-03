// Main3 8.27 cave frames (Play mode, Main3; the 8.27 gate round 2, Wren 2026-10-03: Vesper's V9 frames, Pim's open-day frames, Sable's
// cable). Run by main3_review_capture.sh --area cave as "main3_8_27_cave_frames.cs?look=Day_one" and "?look=Day_two"; `look` selects that
// LookPreview row first and asks to be run again so the look applies. Never saves; restores the player, the camera, DeeperClosed, the
// day-one board, the day-2 boards and the GPU Resident Drawer. Frames at shotW x shotH in outDir, named with the look:
//   V9Open_DeadEnd: from the inspect point (101.0, 14.0) facing 0, the passage open (DeeperClosed hidden).
//   V9Open_FromSideRoom: from the standing point (92.0, 12.0) at the opening, open.   V9Shut_FromChamber: from (80, 12) at the shut rock.
//   DoorwayToExit: from the doorway (89.25, 12.0) heading 270, toward the exit passage (71, 12).
//   MouthDayTwo: from F1's spot (56.31, 46.60) at the mouth, the day-one board down and the day-2 boards up.
//   BulbsTrail: from F2's spot at the bulb tree; its Day2String on for the Day two look. BULBS: on day two, bulbsMin or more bulbs read bulbOver
//   grey over the frame mean (CaveRock.md D, Gate.md N1).
// CABLE (Sable): in the frame from F3's eye (52, 24) heading 180, the cable's pixels at the leg 1 turn (x 53.35, z cableZ0 to cableZ1) read
//   cableOver grey or more over the wall cablePx px above them (the mean of each).
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_cave");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (look != "" && pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; }
string lookName = pv != null ? pv.CurrentLabel : "scene"; string tag = lookName.Replace(' ', '_');
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float eyeH = 1.6f, floorY = -18f, cableZ0 = 21.0f, cableZ1 = 23.5f, cableX = 53.35f, cableY = -5.94f, cableOver = 20f, bulbOver = 40f; const int shotW = 1920, shotH = 988, cablePx = 20, cablePad = 2, bulbPad = 3, bulbsMin = 6;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var cave = Root("Cave").transform; var closed = cave.Find("Layout827/Deeper/DeeperClosed"); var board1 = cave.Find("Mouth/DayOneBoard"); var board2 = cave.Find("Layout827/Day2Boards");
bool closedWas = closed != null && closed.gameObject.activeSelf, b1Was = board1 != null && board1.gameObject.activeSelf, b2Was = board2 != null && board2.gameObject.activeSelf;
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
try
{
    void Pose(UnityEngine.Vector3 e, UnityEngine.Vector3 aim)
    {
        var dir = aim - e; var flat = V(dir.x, 0f, dir.z); cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = e - pc.transform.rotation * camLocal;
        cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f); UnityEngine.Physics.SyncTransforms();
    }
    void Shoot(string file) { cam.targetTexture = rt; cam.Render(); cam.Render(); cam.targetTexture = null; UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null; System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, "CaveFrames_" + tag + "_" + file + ".jpg"), shot.EncodeToJPG(90)); }
    UnityEngine.Vector3 Eye(float x, float z, float floor) => V(x, floor + eyeH, z);
    // V9, open and shut
    if (closed != null)
    {
        closed.gameObject.SetActive(false);
        Pose(Eye(101.0f, 14.0f, floorY), Eye(101.0f, 16.0f, floorY)); Shoot("V9Open_DeadEnd");
        Pose(Eye(92.0f, 12.0f, floorY), V(99.5f, floorY + 1.0f, 14.0f)); Shoot("V9Open_FromSideRoom");
        closed.gameObject.SetActive(true);
        Pose(Eye(80.0f, 12.0f, floorY), V(97.5f, floorY + 1.0f, 13.8f)); Shoot("V9Shut_FromChamber");
        closed.gameObject.SetActive(closedWas);
    }
    else Line(false, "FRAMES: no Layout827/Deeper/DeeperClosed");
    Pose(Eye(89.25f, 12.0f, floorY), V(71.0f, floorY + eyeH, 12.0f)); Shoot("DoorwayToExit");
    // the mouth on day two
    if (board1 != null) board1.gameObject.SetActive(false); if (board2 != null) board2.gameObject.SetActive(true);
    Pose(V(56.31f, H(56.31f, 46.60f) + eyeH, 46.60f), V(52f, -4.5f, 37.9f)); Shoot("MouthDayTwo");
    if (board1 != null) board1.gameObject.SetActive(b1Was); if (board2 != null) board2.gameObject.SetActive(b2Was);
    // the trail's bulbs
    // the bulb tree: its string on for a Day two look only (the day system's state), then BULBS on day two: bulbsMin or more bulbs read
    // bulbOver grey or more over the frame mean (Gate.md N1), each its brightest pixel in a bulbPad px box round its centre
    var bulbPoi = Root("PointsOfInterest").transform.Find("POI_Coloured_bulbs"); var str = bulbPoi != null ? bulbPoi.Find("Day2String") : null; bool strWas = str != null && str.gameObject.activeSelf; bool dayTwo = lookName.ToLowerInvariant().Contains("two");
    if (bulbPoi != null)
    {
        if (str != null) str.gameObject.SetActive(dayTwo);
        Pose(V(91.3f, H(91.3f, 47.4f) + eyeH, 47.4f), bulbPoi.position + V(0f, 3f, 0f)); Shoot("BulbsTrail");
        if (dayTwo)
        {
            var bp = shot.GetPixels32(); double sum = 0; foreach (var c in bp) sum += (c.r + c.g + c.b) / 3.0; float mean = (float)(sum / bp.Length); int lit = 0, n = 0; cam.targetTexture = rt;
            if (str != null) foreach (UnityEngine.Transform b in str) { if (b.name != "Bulb") continue; n++; var sp = cam.WorldToScreenPoint(b.position); if (sp.z <= 0f) continue; int sx = (int)sp.x, sy = (int)sp.y; float best = 0f; for (int dy = -bulbPad; dy <= bulbPad; dy++) for (int dx = -bulbPad; dx <= bulbPad; dx++) { int x = sx + dx, y = sy + dy; if (x < 0 || y < 0 || x >= shotW || y >= shotH) continue; var c = bp[y * shotW + x]; best = UnityEngine.Mathf.Max(best, (c.r + c.g + c.b) / 3f); } if (best - mean >= bulbOver) lit++; }
            cam.targetTexture = null;
            Line(lit >= bulbsMin, "BULBS (" + lookName + "): " + lit + " of " + n + " bulbs read " + bulbOver.ToString("F0") + " grey or more over the frame mean " + mean.ToString("F0") + " in BulbsTrail (at least " + bulbsMin + ")");
        }
        if (str != null) str.gameObject.SetActive(strWas);
    }
    // CABLE: the cable's pixels against the wall's, at the leg 1 turn
    Pose(V(52f, -6f + eyeH, 24f), V(52f, -6f + eyeH, 14f)); Shoot("Cable_Leg1Turn");
    var px = shot.GetPixels32(); double cSum = 0, wSum = 0; int cN = 0, wN = 0; cam.targetTexture = rt;
    for (float z = cableZ0; z <= cableZ1 + 1e-3f; z += 0.25f)
    {
        var sp = cam.WorldToScreenPoint(V(cableX, cableY, z)); if (sp.z <= 0f) continue; int sx = (int)sp.x, sy = (int)sp.y; if (sx < cablePad || sx >= shotW - cablePad || sy < cablePad || sy + cablePx >= shotH) continue;
        float best = 0f; for (int dy = -cablePad; dy <= cablePad; dy++) for (int dx = -cablePad; dx <= cablePad; dx++) { var c = px[(sy + dy) * shotW + sx + dx]; best = UnityEngine.Mathf.Max(best, (c.r + c.g + c.b) / 3f); }
        cSum += best; cN++; var w = px[(sy + cablePx) * shotW + sx]; wSum += (w.r + w.g + w.b) / 3f; wN++;
    }
    cam.targetTexture = null;
    float cableMean = cN > 0 ? (float)(cSum / cN) : 0f, wallMean = wN > 0 ? (float)(wSum / wN) : 0f;
    Line(cN > 0 && cableMean - wallMean >= cableOver, "CABLE (" + lookName + "): at the leg 1 turn the cable reads " + cableMean.ToString("F0") + " grey against the wall's " + wallMean.ToString("F0") + " (" + (cableMean - wallMean).ToString("F0") + " over, at least " + cableOver.ToString("F0") + "; " + cN + " samples)");
    sb.Append("frames CaveFrames_" + tag + "_*.jpg: V9Open_DeadEnd, V9Open_FromSideRoom, V9Shut_FromChamber, DoorwayToExit, MouthDayTwo, BulbsTrail, Cable_Leg1Turn\n");
}
finally
{
    if (closed != null) closed.gameObject.SetActive(closedWas); if (board1 != null) board1.gameObject.SetActive(b1Was); if (board2 != null) board2.gameObject.SetActive(b2Was);
    cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.27 cave frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
