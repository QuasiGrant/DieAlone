// Main3 review capture (Play mode, Main3). Shows the map the way a player sees it, for Marlow, Pim and Vesper before every
// Grant walk. Run it with Tools/Recipes/main3_review_capture.sh, which enters Play mode, runs step "day" then step "night",
// leaves Play mode and resets runInBackground. Every frame is 1920 x 988 (Grant's Game view shape at half size), rendered
// from Camera.main into a RenderTexture (the look filter draws; screen-space UI does not), eye 1.6 m over the ground,
// then shrunk into labelled contact sheets (JPG) in outDir with an index.md. Nothing in the scene or any asset is saved.
// step "day" (day one look): one sheet per trail (both directions, a frame every 10 m, looking along the trail), the climb
// (J to Ward, up, every 10 m), warps (N, E, S, W), trail ends (facing out), stops (every invisible collider that faces a
// walker within 5 m of a trail centre line, one frame per 8 m cluster, facing it), the top-down map with trails, warps and
// stops marked, and the day halves of the ten day and night pairs (kept in Temp/ReviewCapture).
// 8.14a (Gate.md 2.7, 2.8 and 4; Marlow's hand-walk views): four compass views from the tower deck; the walk views (P4 looking
// east, the path from the slot exit round the fin with the flame tops in sight per frame, the pump trench to both sides, the lot,
// store, booth and office facing east); every collider without a renderer in the scene, active or not, listed in
// InvisibleColliders.md; and, in both steps, a grayscale trail sheet (a frame every 20 m along each trail, looking along it) with
// the mean grey (0 to 255, filter on) of the trail and of the floor 3 m to either side, 5 m and 20 m ahead, in index.md.
// step "night" (Night look): the night halves, the pairs sheet and the night grayscale sheet. If the look is wrong the step
// selects it and returns "run again" (the look applies on the next frame).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
string step = "day";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review");
string tempDir = System.IO.Path.GetFullPath("Temp/ReviewCapture");
UnityEngine.Application.runInBackground = true;
const int shotW = 1920, shotH = 988, jpgQuality = 85;
const float eye = 1.6f, trailStep = 10f, stopReach = 5f, stopCluster = 8f, stopFacing = 0.7f, stopProbeStep = 1f, stopProbeHeight = 1f;
const int stopRays = 24, trailDiv = 3, trailCols = 5, warpDiv = 3, listDiv = 3, listCols = 5, pairDiv = 2, stopsPerSheet = 60;
const int mapTiles = 5, mapPx = 512; const float mapTileM = 100f, mapX0 = -50f, mapZ0 = -75f;
// 8.14a walk views: compass views dip 15 m over 100 m; P4 pitched 10 degrees down; the slot exit (Valley.md rev 10 8.3) and a frame
// every 2 m round the fin, facing the far front (x -230, y 40) and, at the path end, down to the valley fires (y -40); the pump
// trench 30, 15 and 5 m short of the pump; the front views face x 440, past the fence and the highway; the valley card count gives
// every drawn mesh within wardNear m of the Ward warp a temporary collider
// compassOut: the deck walkway, the cab's half width 2.2 plus half the 2.2 m walkway; markerBack: marker frames stand this far along
// the trail from the point nearest the marker
const float markerBack = 5f, markerLookUp = 1.4f, markerRise = 1.5f, hedgeSample = 0.5f, hedgeNeed = 0.8f, hedgeTopSlack = 0.5f, warpNear = 1.5f, roadZoomFov = 15f;   // hedge and warp checks (Pim 8.15), the road zoom   // markerRise: a stand this much above or below the marker is on a bank
const float wardNear = 60f, compassOut = 3.3f, compassDip = 15f, p4Dip = 10f, finStep = 2f, fireLookX = -230f, fireLookY = 40f, valleyLookY = -40f, eastLookX = 440f;
var slotExit = new UnityEngine.Vector2(4f, 257.3f); var pumpBack = new[] { 30f, 15f, 5f };
var inv = System.Globalization.CultureInfo.InvariantCulture;

// ---- look
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (preview == null) return "no LookPreview";
string wantLook = step == "day" ? "Day one" : "Night";
if (preview.CurrentLabel != wantLook)
{
    for (int i = 0; i < preview.Count; i++) if (preview.Label(i) == wantLook) { preview.Select(i); return "selected " + wantLook + "; run again"; }
    return "no look row " + wantLook;
}

// ---- scene pieces
UnityEngine.GameObject trailsRoot = null, warpsRoot = null;
foreach (var r in scene.GetRootGameObjects()) { if (r.name == "Trails") trailsRoot = r; if (r.name == "DevWarps") warpsRoot = r; }
if (trailsRoot == null || warpsRoot == null) return "no Trails or DevWarps root";
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
var terrain = UnityEngine.Terrain.activeTerrain;
float Ground(float x, float z, float fromY)
{
    if (UnityEngine.Physics.Raycast(new UnityEngine.Vector3(x, fromY + 1.5f, z), UnityEngine.Vector3.down, out var h, 6f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) return h.point.y;
    return terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
}
var legs = new System.Collections.Generic.List<(string name, System.Collections.Generic.List<UnityEngine.Vector3> pts)>();
foreach (UnityEngine.Transform leg in trailsRoot.transform)
{
    var l = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) l.Add(p.position);
    if (l.Count > 1) legs.Add((leg.name, l));
}
float Length(System.Collections.Generic.List<UnityEngine.Vector3> l) { float s = 0f; for (int i = 1; i < l.Count; i++) s += UnityEngine.Vector3.Distance(l[i - 1], l[i]); return s; }
UnityEngine.Vector3 At(System.Collections.Generic.List<UnityEngine.Vector3> l, float s)
{
    if (s <= 0f) return l[0];
    for (int i = 1; i < l.Count; i++) { float d = UnityEngine.Vector3.Distance(l[i - 1], l[i]); if (s <= d) return UnityEngine.Vector3.Lerp(l[i - 1], l[i], d > 0f ? s / d : 0f); s -= d; }
    var a = l[l.Count - 2]; var b = l[l.Count - 1]; return b + (b - a).normalized * s;   // past the end: carry on straight
}

// ---- rendering
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32);
var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
void Pose(UnityEngine.Vector3 camPos, UnityEngine.Vector3 look)
{
    var dir = look - camPos; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z); if (flat.sqrMagnitude < 1e-6f) flat = UnityEngine.Vector3.forward;
    cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = camPos - pc.transform.rotation * camLocal;
    cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
}
UnityEngine.Color32[] Capture()   // full size, bottom row first
{
    cam.targetTexture = rt; cam.Render(); cam.targetTexture = null;
    UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
    return shot.GetPixels32();
}
UnityEngine.Color32[] Render(int div) => Shrink(Capture(), div);
UnityEngine.Color32[] Shrink(UnityEngine.Color32[] src, int div)
{
    int w = shotW / div, h = shotH / div; var dst = new UnityEngine.Color32[w * h]; int n = div * div;
    for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
    {
        int r = 0, g = 0, b = 0;
        for (int dy = 0; dy < div; dy++) for (int dx = 0; dx < div; dx++) { var c = src[(y * div + dy) * shotW + x * div + dx]; r += c.r; g += c.g; b += c.b; }
        dst[y * w + x] = new UnityEngine.Color32((byte)(r / n), (byte)(g / n), (byte)(b / n), 255);
    }
    return dst;
}

// ---- 5 x 7 pixel font for labels (upper case)
var glyphs = new System.Collections.Generic.Dictionary<char, int[]>();
foreach (var g in ("A0E11111F111111 B1E11111E11111E C0E11101010110E D1C121111111210 E1F10101E10101F F1F10101E101010 G0E11101711110F H1111111F111111 "
    + "I0E04040404040E J0702020202120C K11121418141211 L1010101010101F M111B1515111111 N11111915131111 O0E11111111110E P1E11111E101010 Q0E11111115120D "
    + "R1E11111E141211 S0F10100E01011E T1F040404040404 U1111111111110E V11111111110A04 W1111111515150A X11110A040A1111 Y1111110A040404 Z1F01020408101F "
    + "00E11131519110E 1040C040404040E 20E11010204081F 31F02040201110E 402060A121F0202 51F101E0101110E 60608101E11110E 71F010204080808 80E11110E11110E "
    + "90E11110F01020C -0000001F000000 .0000000000000C (02040808080402 )08040202020408 /00010204081000 ,000000000C0408 :000C0C000C0C00 >08040201020408 "
    + "_0000000000001F #0A0A1F0A1F0A0A '04040800000000 =00001F001F0000 ?0E110102040004").Split(' '))
{
    if (g.Length != 15) return "bad glyph entry " + g;
    var rows = new int[7]; for (int i = 0; i < 7; i++) rows[i] = System.Convert.ToInt32(g.Substring(1 + i * 2, 2), 16); glyphs[g[0]] = rows;
}
glyphs['+'] = new[] { 0x00, 0x04, 0x04, 0x1F, 0x04, 0x04, 0x00 };
// sheet = top-down canvas; stored bottom-up for Texture2D
UnityEngine.Color32[] canvas = null; int cW = 0, cH = 0;
void NewCanvas(int w, int h) { cW = w; cH = h; canvas = new UnityEngine.Color32[w * h]; var bg = new UnityEngine.Color32(18, 18, 20, 255); for (int i = 0; i < canvas.Length; i++) canvas[i] = bg; }
void Px(int x, int yTop, UnityEngine.Color32 c) { if (x >= 0 && x < cW && yTop >= 0 && yTop < cH) canvas[(cH - 1 - yTop) * cW + x] = c; }
int Text(int x, int yTop, string s, int scale, UnityEngine.Color32 c)
{
    foreach (var ch0 in s.ToUpperInvariant())
    {
        var ch = glyphs.ContainsKey(ch0) || ch0 == ' ' ? ch0 : '?';
        if (ch != ' ') { var rows = glyphs[ch]; for (int ry = 0; ry < 7; ry++) for (int rx = 0; rx < 5; rx++) if ((rows[ry] >> (4 - rx) & 1) == 1) for (int sy = 0; sy < scale; sy++) for (int sx = 0; sx < scale; sx++) Px(x + rx * scale + sx, yTop + ry * scale + sy, c); }
        x += 6 * scale;
    }
    return x;
}
void Blit(UnityEngine.Color32[] thumb, int tw, int th, int x, int yTop) { for (int y = 0; y < th; y++) for (int xx = 0; xx < tw; xx++) Px(x + xx, yTop + (th - 1 - y), thumb[y * tw + xx]); }
var white = new UnityEngine.Color32(235, 235, 235, 255); var gold = new UnityEngine.Color32(255, 205, 90, 255);
const int labelH = 22, headH = 44, gap = 4;
var written = new System.Collections.Generic.List<(string file, string what, int frames)>();
void SaveCanvas(string file, string what, int frames)
{
    var tex = new UnityEngine.Texture2D(cW, cH, UnityEngine.TextureFormat.RGB24, false); tex.SetPixels32(canvas); tex.Apply();
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), file.EndsWith(".png") ? tex.EncodeToPNG() : tex.EncodeToJPG(jpgQuality));
    UnityEngine.Object.DestroyImmediate(tex); canvas = null; written.Add((file, what, frames));
}
// one frame spec: camera, look-at, label
void Sheet(string file, string title, System.Collections.Generic.List<(UnityEngine.Vector3 cam, UnityEngine.Vector3 look, string label)> frames, int div, int cols)
{
    int tw = shotW / div, th = shotH / div; int rows = (frames.Count + cols - 1) / cols;
    NewCanvas(cols * (tw + gap) + gap, headH + rows * (labelH + th + gap) + gap);
    Text(gap + 4, 12, title, 3, gold);
    for (int i = 0; i < frames.Count; i++)
    {
        int x = gap + (i % cols) * (tw + gap), y = headH + (i / cols) * (labelH + th + gap);
        Pose(frames[i].cam, frames[i].look); Blit(Render(div), tw, th, x, y + labelH); Text(x + 2, y + 4, frames[i].label, 2, white);
    }
    SaveCanvas(file, title, frames.Count);
}
string Safe(string n) => n.Replace(' ', '_');
UnityEngine.Vector3 Eye(UnityEngine.Vector3 p) => new UnityEngine.Vector3(p.x, Ground(p.x, p.z, p.y) + eye, p.z);
UnityEngine.Vector3 Toward(UnityEngine.Vector3 camPos, float yaw, float dist) => camPos + UnityEngine.Quaternion.Euler(0f, yaw, 0f) * UnityEngine.Vector3.forward * dist;
// ten day and night spots (Gate.md 2.8): (name, stand x, z, look-at x, y, z); NaN look y means level
var pairs = new (string n, float x, float z, float lx, float ly, float lz)[] {
    ("Lake pump (Lake Pump warp)", 190f, 99f, 190f, float.NaN, 60f),
    ("Camp 1 (Camp 1 warp)", 268f, 226f, 282f, float.NaN, 238f),
    ("Camp 3 (Camp 3 warp)", 74f, 142f, 90f, float.NaN, 158f),
    ("Lot facing the highway (Lot Highway warp)", 382f, 168f, 440f, float.NaN, 170f),
    ("Camp (Keepers Camp warp)", 172f, 150f, 172f + 40f * UnityEngine.Mathf.Sin(333f * UnityEngine.Mathf.Deg2Rad), float.NaN, 150f + 40f * UnityEngine.Mathf.Cos(333f * UnityEngine.Mathf.Deg2Rad)),
    ("S1 camp from the edge (LookSlice 6)", 156f, 148f, 178f, float.NaN, 168f),
    ("Office (Office warp)", 340f, 196f, 340f + 40f * UnityEngine.Mathf.Sin(68f * UnityEngine.Mathf.Deg2Rad), float.NaN, 196f + 40f * UnityEngine.Mathf.Cos(68f * UnityEngine.Mathf.Deg2Rad)),
    ("Lot centre looking north to office and store", 358f, 170f, 358f, float.NaN, 200f),
    ("J (Junction J warp)", 106f, 203f, 106f + 40f * UnityEngine.Mathf.Sin(316f * UnityEngine.Mathf.Deg2Rad), float.NaN, 203f + 40f * UnityEngine.Mathf.Cos(316f * UnityEngine.Mathf.Deg2Rad)),
    ("Ward path end (Ward warp)", -8.5f, 246f, -48.5f, float.NaN, 246f) };   // Valley.md rev 10 (8.14)
System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)> PairFrames(string half)
{
    var f = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
    foreach (var p in pairs) { var c = Eye(new UnityEngine.Vector3(p.x, 200f, p.z)); var lk = new UnityEngine.Vector3(p.lx, float.IsNaN(p.ly) ? c.y : p.ly, p.lz); f.Add((c, lk, p.n + " " + half)); }
    return f;
}
// ---- grayscale trail frames and mean grey (Gate.md 4: trail at least 20 above or below the floor beside it, 5 m and 20 m ahead)
const float greyStep = 20f, greyNear = 5f, greyFar = 20f, greySide = 3f, greyPatch = 0.3f, greyNeed = 20f, greyHidden = 8f;   // greyHidden: mean grey change when the grass is hidden, over which the trail patch counts as covered const int greyDiv = 4, greyCols = 6;
const int greyDiv = 4, greyCols = 6;
float Grey(UnityEngine.Color32 c) => 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
// mean grey of the pixels within greyPatch metres of p on the full frame, or -1 when p is off screen, behind or hidden
float PatchGrey(UnityEngine.Color32[] src, UnityEngine.Vector3 p, UnityEngine.Color32[] bare)   // bare: the same frame without terrain grass; when given, a patch the grass changes is hidden
{
    cam.targetTexture = rt; var vp = cam.WorldToViewportPoint(p); float fov = cam.fieldOfView; cam.targetTexture = null;
    if (vp.z <= 0.5f || vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f) return -1f;
    var from = cam.transform.position; var to = p + UnityEngine.Vector3.up * 0.1f;
    if (UnityEngine.Physics.Linecast(from, to, out var block, ~0, UnityEngine.QueryTriggerInteraction.Ignore) && block.distance < UnityEngine.Vector3.Distance(from, to) - 0.5f) return -1f;
    int cx = (int)(vp.x * shotW), cy = (int)(vp.y * shotH);
    int r = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(greyPatch / (vp.z * UnityEngine.Mathf.Tan(fov * 0.5f * UnityEngine.Mathf.Deg2Rad)) * shotH * 0.5f));
    float sum = 0f, change = 0f; int n = 0;
    for (int y = cy - r; y <= cy + r; y++) for (int x = cx - r; x <= cx + r; x++)
        if (x >= 0 && x < shotW && y >= 0 && y < shotH && (x - cx) * (x - cx) + (y - cy) * (y - cy) <= r * r) { float g = Grey(src[y * shotW + x]); sum += g; if (bare != null) change += UnityEngine.Mathf.Abs(g - Grey(bare[y * shotW + x])); n++; }
    if (n == 0 || (bare != null && change / n > greyHidden)) return -1f;
    return sum / n;
}
var greyMd = new System.Text.StringBuilder();
string GreyTrails(string lookName)
{
    var frames = new System.Collections.Generic.List<(UnityEngine.Color32[] px, string label)>();
    string file = "Grey_Trails_" + lookName.Replace(' ', '_') + ".jpg";
    greyMd.Append("\n## Trail grey, " + lookName + "\n\nSheet [" + file + "](" + file + "). Mean grey (0 to 255, filter on) of the trail centre and of the floor " + greySide.ToString("F0", inv) + " m to either side, " + greyNear.ToString("F0", inv) + " m and " + greyFar.ToString("F0", inv) + " m ahead, over frames every " + greyStep.ToString("F0", inv) + " m looking along the trail. A point is left out when it is off screen or behind a collider (hedges included), and a trail point also when the terrain grass covers it (its patch changes by more than " + greyHidden.ToString("F0", inv) + " grey with the grass hidden); the frame label shows - for it. Pass: the difference is " + greyNeed.ToString("F0", inv) + " or more either way.\n\n");
    greyMd.Append("| Trail | Frames | Trail 5 m | Floor 5 m | Diff 5 m | Trail 20 m | Floor 20 m | Diff 20 m | Pass |\n|---|---|---|---|---|---|---|---|---|\n");
    int passN = 0;
    foreach (var leg in legs)
    {
        float len = Length(leg.pts); float[] tSum = new float[2], fSum = new float[2]; int[] tN = new int[2], fN = new int[2]; int nFrames = 0; float ofT = 0f, ofF = 0f; int ofTN = 0, ofFN = 0;
        for (float s = 0f; s <= len + 0.01f; s += greyStep)
        {
            var p = At(leg.pts, s); var c = p + UnityEngine.Vector3.up * eye; var ahead = At(leg.pts, s + 3f); var far = At(leg.pts, s + trailStep);
            var yawDir = new UnityEngine.Vector3(ahead.x - p.x, 0f, ahead.z - p.z).normalized;
            Pose(c, c + yawDir * 10f + UnityEngine.Vector3.up * (far.y - p.y)); var src = Capture(); nFrames++;
            terrain.drawTreesAndFoliage = false; var bare = Capture(); terrain.drawTreesAndFoliage = true;
            bool fogWas = UnityEngine.RenderSettings.fog; UnityEngine.RenderSettings.fog = false; var fogless = Capture(); UnityEngine.RenderSettings.fog = fogWas;
            string label = leg.name + " " + s.ToString("F0", inv) + " M";
            var marks = new System.Collections.Generic.List<(float x, float y, bool trail)>();
            for (int k = 0; k < 2; k++)
            {
                float ds = k == 0 ? greyNear : greyFar; var q = At(leg.pts, s + ds); var q2 = At(leg.pts, s + ds + 1f);
                var t = new UnityEngine.Vector3(q2.x - q.x, 0f, q2.z - q.z).normalized; var side = new UnityEngine.Vector3(t.z, 0f, -t.x);
                var tp = new UnityEngine.Vector3(q.x, Ground(q.x, q.z, q.y), q.z); float tg = PatchGrey(src, tp, bare);
                float fg = 0f; int fn = 0;
                foreach (var sgn in new[] { -1f, 1f })
                {
                    var fx = q + side * greySide * sgn; var fp = new UnityEngine.Vector3(fx.x, Ground(fx.x, fx.z, q.y), fx.z); float g = PatchGrey(src, fp, null);
                    if (g >= 0f) { fg += g; fn++; }
                }
                if (tg >= 0f) { tSum[k] += tg; tN[k]++; }
                if (fn > 0) { fSum[k] += fg / fn; fN[k]++; }
                if (k == 1) { float ot = PatchGrey(fogless, tp, bare); if (ot >= 0f) { ofT += ot; ofTN++; } float of = 0f; int on = 0; foreach (var sgn in new[] { -1f, 1f }) { var fx = q + side * greySide * sgn; float g = PatchGrey(fogless, new UnityEngine.Vector3(fx.x, Ground(fx.x, fx.z, q.y), fx.z), null); if (g >= 0f) { of += g; on++; } } if (on > 0) { ofF += of / on; ofFN++; } }
                label += " " + ds.ToString("F0", inv) + ": " + (tg >= 0f ? tg.ToString("F0", inv) : "-") + "/" + (fn > 0 ? (fg / fn).ToString("F0", inv) : "-");
            }
            var small = Shrink(src, greyDiv); for (int i = 0; i < small.Length; i++) { byte g = (byte)UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(Grey(small[i])), 0, 255); small[i] = new UnityEngine.Color32(g, g, g, 255); }
            frames.Add((small, label));
        }
        string Cell(float sum, int n) => n > 0 ? (sum / n).ToString("F0", inv) : "-";
        float d5 = tN[0] > 0 && fN[0] > 0 ? tSum[0] / tN[0] - fSum[0] / fN[0] : 0f, d20 = tN[1] > 0 && fN[1] > 0 ? tSum[1] / tN[1] - fSum[1] / fN[1] : 0f;
        bool pass = UnityEngine.Mathf.Abs(d5) >= greyNeed && UnityEngine.Mathf.Abs(d20) >= greyNeed; if (pass) passN++;
        greyMd.Append("| " + leg.name + " | " + nFrames + " | " + Cell(tSum[0], tN[0]) + " | " + Cell(fSum[0], fN[0]) + " | " + d5.ToString("F0", inv) + " | " + Cell(tSum[1], tN[1]) + " | " + Cell(fSum[1], fN[1]) + " | " + d20.ToString("F0", inv) + " | " + (ofTN > 0 && ofFN > 0 ? (ofT / ofTN - ofF / ofFN).ToString("F0", inv) : "-") + " | " + (pass ? "PASS" : "FAIL") + " |\n");
    }
    int tw = shotW / greyDiv, th = shotH / greyDiv, rows = (frames.Count + greyCols - 1) / greyCols;
    NewCanvas(greyCols * (tw + gap) + gap, headH + rows * (labelH + th + gap) + gap);
    Text(gap + 4, 12, "Grayscale trails, " + lookName + ": every 20 m, trail/floor grey at 5 m and 20 m", 3, gold);
    for (int i = 0; i < frames.Count; i++)
    {
        int x = gap + (i % greyCols) * (tw + gap), y = headH + (i / greyCols) * (labelH + th + gap);
        Blit(frames[i].px, tw, th, x, y + labelH); Text(x + 2, y + 4, frames[i].label, 1, white);
    }
    SaveCanvas(file, "Grayscale trails, " + lookName + ", a frame every 20 m with trail and floor grey (index.md, Trail grey)", frames.Count);
    return "grey " + lookName + ": " + passN + " of " + legs.Count + " trails differ by " + greyNeed.ToString("F0", inv) + " or more at 5 m and 20 m";
}
var sb = new System.Text.StringBuilder();
System.IO.Directory.CreateDirectory(outDir); System.IO.Directory.CreateDirectory(tempDir);
var clock = System.Diagnostics.Stopwatch.StartNew();
try
{
    if (step == "night")
    {
        var night = PairFrames("NIGHT"); var nights = new System.Collections.Generic.List<UnityEngine.Color32[]>();
        foreach (var f in night) { Pose(f.Item1, f.Item2); nights.Add(Render(pairDiv)); }
        int tw = shotW / pairDiv, th = shotH / pairDiv;
        NewCanvas(2 * (tw + gap) + gap, headH + pairs.Length * (labelH + th + gap) + gap);
        Text(gap + 4, 12, "Day one and Night pairs", 3, gold);
        var tmp = new UnityEngine.Texture2D(2, 2);
        for (int i = 0; i < pairs.Length; i++)
        {
            string dayFile = System.IO.Path.Combine(tempDir, "pair_day_" + i + ".png");
            if (!System.IO.File.Exists(dayFile)) return "missing " + dayFile + ": run step day first";
            UnityEngine.ImageConversion.LoadImage(tmp, System.IO.File.ReadAllBytes(dayFile));
            int y = headH + i * (labelH + th + gap);
            Blit(tmp.GetPixels32(), tw, th, gap, y + labelH); Text(gap + 2, y + 4, pairs[i].n + " DAY ONE", 2, white);
            Blit(nights[i], tw, th, gap * 2 + tw, y + labelH); Text(gap * 2 + tw + 2, y + 4, pairs[i].n + " NIGHT", 2, white);
        }
        UnityEngine.Object.DestroyImmediate(tmp);
        SaveCanvas("Pairs_DayOne_Night.jpg", "Day one and Night pairs", pairs.Length * 2);
        string greyNight = GreyTrails("Night");
        string indexPath = System.IO.Path.Combine(outDir, "index.md");
        if (!System.IO.File.Exists(indexPath)) return "missing " + indexPath + ": run step day first";
        System.IO.File.AppendAllText(indexPath, greyMd.ToString().Replace("\r", ""));
        System.IO.Directory.Delete(tempDir, true);
        return "night done: wrote Pairs_DayOne_Night.jpg (" + pairs.Length + " pairs), Grey_Trails_Night.jpg in " + clock.Elapsed.TotalSeconds.ToString("F0") + " s | " + greyNight;
    }

    // 1. trails, both directions
    float worstY = 0f;
    foreach (var leg in legs)
    {
        foreach (var p in leg.pts) worstY = UnityEngine.Mathf.Max(worstY, UnityEngine.Mathf.Abs(p.y - Ground(p.x, p.z, p.y)));
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
        foreach (var back in new[] { false, true })
        {
            var l = new System.Collections.Generic.List<UnityEngine.Vector3>(leg.pts); if (back) l.Reverse();
            float len = Length(l);
            for (float s = 0f; s <= len + 0.01f; s += trailStep)
            {
                var p = At(l, s); var c = p + UnityEngine.Vector3.up * eye;
                var ahead = At(l, s + 3f); var far = At(l, s + trailStep);
                var yawDir = new UnityEngine.Vector3(ahead.x - p.x, 0f, ahead.z - p.z).normalized;
                float grade = (far.y - p.y) / trailStep;   // look pitched with the trail over the next 10 m
                frames.Add((c, c + yawDir * 10f + UnityEngine.Vector3.up * grade * 10f, leg.name + (back ? " BACK " : " FWD ") + s.ToString("F0", inv) + " M"));
            }
        }
        Sheet("Trail_" + Safe(leg.name) + ".jpg", "Trail " + leg.name + ": forward then back, every 10 m (" + Length(leg.pts).ToString("F0", inv) + " m)", frames, trailDiv, trailCols);
    }
    sb.Append("trails " + legs.Count + " sheets; worst trail point off the ground " + worstY.ToString("F2", inv) + " m\n");

    // 4. the climb, J to Ward, up
    var climb = legs.Find(q => q.name == "J to Ward");
    if (climb.pts == null) return "no leg J to Ward";
    {
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>(); float len = Length(climb.pts);
        for (float s = 0f; s <= len + 0.01f; s += trailStep)
        {
            var p = At(climb.pts, s); var c = p + UnityEngine.Vector3.up * eye; var ahead = At(climb.pts, s + 3f); var far = At(climb.pts, s + trailStep);
            var yawDir = new UnityEngine.Vector3(ahead.x - p.x, 0f, ahead.z - p.z).normalized;
            frames.Add((c, c + yawDir * 10f + UnityEngine.Vector3.up * (far.y - p.y), "CLIMB " + s.ToString("F0", inv) + " M  Y " + p.y.ToString("F0", inv)));
        }
        Sheet("Climb_J_to_Ward.jpg", "Climb J to Ward, up, every 10 m (" + len.ToString("F0", inv) + " m)", frames, trailDiv, trailCols);
    }

    // 2. warps, N E S W
    var warpList = new System.Collections.Generic.List<UnityEngine.Transform>(); foreach (UnityEngine.Transform w in warpsRoot.transform) warpList.Add(w);
    {
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>(); var dirs = new[] { ("N", 0f), ("E", 90f), ("S", 180f), ("W", 270f) };
        for (int i = 0; i < warpList.Count; i++) { var c = Eye(warpList[i].position); foreach (var d in dirs) frames.Add((c, Toward(c, d.Item2, 10f), "W" + (i + 1) + " " + warpList[i].name + " " + d.Item1)); }
        Sheet("Warps_NESW.jpg", "Warps: N, E, S, W from each (" + warpList.Count + " warps)", frames, warpDiv, 4);
    }

    // 3a. trail ends, facing out along the trail
    {
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
        foreach (var leg in legs) foreach (var back in new[] { false, true })
        {
            var l = new System.Collections.Generic.List<UnityEngine.Vector3>(leg.pts); if (!back) l.Reverse();   // l runs toward the end being shot
            var end = l[l.Count - 1]; var before = At(l, Length(l) - 4f); var c = end + UnityEngine.Vector3.up * eye;
            var outDirV = new UnityEngine.Vector3(end.x - before.x, 0f, end.z - before.z).normalized;
            frames.Add((c, c + outDirV * 10f, leg.name + (back ? " START" : " END") + " (" + end.x.ToString("F0", inv) + ", " + end.z.ToString("F0", inv) + ")"));
        }
        Sheet("Trail_Ends.jpg", "Trail ends: each end of each trail, facing out along it", frames, listDiv, listCols);
    }

    // 3b. stops: invisible colliders (no Renderer on the collider's object, not terrain) hit within 5 m of a trail centre line whose
    // face turns toward the walker (|normal . trail direction| over 0.7), so side walls running along the trail are left out
    var hits = new System.Collections.Generic.List<(UnityEngine.Vector3 hit, UnityEngine.Vector3 from, string what, string leg, float s, float d)>();
    foreach (var leg in legs)
    {
        float len = Length(leg.pts);
        for (float s = 0f; s <= len; s += stopProbeStep)
        {
            var p = At(leg.pts, s); var q = At(leg.pts, UnityEngine.Mathf.Min(len, s + 1f)); if (s + 1f > len) { q = p; p = At(leg.pts, s - 1f); }
            var t = new UnityEngine.Vector3(q.x - p.x, 0f, q.z - p.z).normalized; var o = At(leg.pts, s) + UnityEngine.Vector3.up * stopProbeHeight;
            for (int k = 0; k < stopRays; k++)
            {
                var dir = UnityEngine.Quaternion.Euler(0f, k * 360f / stopRays, 0f) * UnityEngine.Vector3.forward;
                if (!UnityEngine.Physics.Raycast(o, dir, out var h, stopReach, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) continue;
                if (h.collider is UnityEngine.TerrainCollider || h.collider.GetComponent<UnityEngine.Renderer>() != null) continue;
                var n = new UnityEngine.Vector3(h.normal.x, 0f, h.normal.z); if (n.sqrMagnitude < 0.25f) continue; n.Normalize();
                if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(n, t)) < stopFacing) continue;
                string path = h.collider.name; var tr = h.collider.transform.parent; if (tr != null) path = tr.name + "/" + path;
                hits.Add((h.point, o, path, leg.name, s, h.distance));
            }
        }
    }
    var stops = new System.Collections.Generic.List<(UnityEngine.Vector3 hit, UnityEngine.Vector3 from, string what, string leg, float s, float d)>();
    foreach (var h in System.Linq.Enumerable.OrderBy(hits, q => q.d))
    {
        bool near = false; foreach (var st in stops) if (UnityEngine.Vector3.Distance(st.hit, h.hit) < stopCluster) { near = true; break; }
        if (!near) stops.Add(h);
    }
    stops.Sort((a, b) => a.leg == b.leg ? a.s.CompareTo(b.s) : string.CompareOrdinal(a.leg, b.leg));
    var stopFrames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
    for (int i = 0; i < stops.Count; i++)
    {
        var st = stops[i]; var c = Eye(st.from); var lk = new UnityEngine.Vector3(st.hit.x, c.y, st.hit.z);
        stopFrames.Add((c, lk, "S" + (i + 1) + " " + st.leg + " " + st.s.ToString("F0", inv) + " M, " + st.d.ToString("F1", inv) + " M TO " + st.what));
    }
    for (int part = 0; part * stopsPerSheet < stopFrames.Count; part++)
        Sheet("Stops_" + (part + 1) + ".jpg", "Invisible stops within 5 m of a trail, facing them (" + (part * stopsPerSheet + 1) + " to " + UnityEngine.Mathf.Min(stopFrames.Count, (part + 1) * stopsPerSheet) + " of " + stopFrames.Count + ")",
            stopFrames.GetRange(part * stopsPerSheet, UnityEngine.Mathf.Min(stopsPerSheet, stopFrames.Count - part * stopsPerSheet)), listDiv, listCols);
    sb.Append("stops " + stops.Count + " from " + hits.Count + " hits\n");

    // 3c. four compass views from the tower deck (Gate.md 2.7)
    var towerT = UnityEngine.GameObject.Find("Camp/Tower"); if (towerT == null) return "no Camp/Tower";
    var cabT = towerT.transform.Find("Cab"); if (cabT == null) return "no Camp/Tower/Cab";
    {
        var deckEye = new UnityEngine.Vector3(towerT.transform.position.x, cabT.position.y + eye, towerT.transform.position.z);
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
        foreach (var d in new[] { ("NORTH", 0f), ("EAST", 90f), ("SOUTH", 180f), ("WEST", 270f) })
            { var e = deckEye + UnityEngine.Quaternion.Euler(0f, d.Item2, 0f) * UnityEngine.Vector3.forward * compassOut; frames.Add((e, Toward(e, d.Item2, 100f) + UnityEngine.Vector3.down * compassDip, "TOWER DECK WALKWAY FACING " + d.Item1)); }   // 8.14a gate: on the walkway, outside the cab (inside it the posts and lamps covered the view)
        Sheet("Compass_Views.jpg", "Compass views from the tower deck: north, east, south, west", frames, pairDiv, 2);
    }

    // 3d. Marlow's hand-walk views (Gate_8_14_Marlow.md 13): P4 east, round the fin with flame tops in sight, the pump trench, east from the front
    var flameCardShader = UnityEngine.Shader.Find("DieAlone/FlameCard");
    var flameTops = new System.Collections.Generic.List<(UnityEngine.Vector3 top, string group)>();
    foreach (var mf in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshFilter>(UnityEngine.FindObjectsSortMode.None))
    {
        var mr = mf.GetComponent<UnityEngine.MeshRenderer>(); if (mr == null || !mr.enabled || mr.sharedMaterial == null || mr.sharedMaterial.shader != flameCardShader || mf.sharedMesh == null) continue;
        var vs = mf.sharedMesh.vertices; for (int i = 0; i + 3 < vs.Length; i += 4) flameTops.Add((mf.transform.TransformPoint((vs[i + 2] + vs[i + 3]) * 0.5f), mf.name));
    }
    // a line is clear when nothing drawn stands on it: colliders without a renderer (rim colliders inside boulders, IW walls) do not hide
    bool Clear(UnityEngine.Vector3 a, UnityEngine.Vector3 b) { var d = b - a; foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (h.collider is UnityEngine.TerrainCollider || h.collider.GetComponent<UnityEngine.Renderer>() != null) return false; return true; }
    string InSight(UnityEngine.Vector3 from)   // flame tops in sight by group
    {
        var seen = new System.Collections.Generic.SortedDictionary<string, int>();
        foreach (var f in flameTops) if (Clear(from, f.top)) seen[f.group] = seen.TryGetValue(f.group, out var k) ? k + 1 : 1;
        if (seen.Count == 0) return "NO FLAME";
        var parts = new System.Collections.Generic.List<string>(); foreach (var kv in seen) parts.Add(kv.Key + " " + kv.Value); return string.Join(", ", parts);
    }
    var walkNotes = new System.Text.StringBuilder();
    {
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
        var p4 = warpsRoot.transform.Find("Ward_P4"); if (p4 == null) return "no warp Ward_P4";
        var p4Eye = Eye(p4.position); frames.Add((p4Eye, p4Eye + p4.forward * 40f + UnityEngine.Vector3.down * 40f * UnityEngine.Mathf.Tan(p4Dip * UnityEngine.Mathf.Deg2Rad), "P4 LOOK EAST (WARD P4 WARP)"));
        // round the fin: from the J to Ward point nearest the slot exit to the path end, every finStep m, facing the fire
        float climbLen = Length(climb.pts), sExit = 0f, best = float.MaxValue;
        for (float s = 0f; s <= climbLen; s += 0.5f) { var q = At(climb.pts, s); float d = new UnityEngine.Vector2(q.x - slotExit.x, q.z - slotExit.y).magnitude; if (d < best) { best = d; sExit = s; } }
        string firstFlame = "none";
        for (float s = sExit; s <= climbLen + 0.01f; s += finStep)
        {
            var c = At(climb.pts, s) + UnityEngine.Vector3.up * eye; string seen = InSight(c);
            if (firstFlame == "none" && seen != "NO FLAME") firstFlame = (s - sExit).ToString("F0", inv) + " m past the slot exit (" + seen + ")";
            frames.Add((c, new UnityEngine.Vector3(fireLookX, fireLookY, c.z), "FIN +" + (s - sExit).ToString("F0", inv) + " M: " + seen));
        }
        var end = At(climb.pts, climbLen) + UnityEngine.Vector3.up * eye;
        frames.Add((end, new UnityEngine.Vector3(fireLookX, valleyLookY, end.z), "PATH END, DOWN TO THE VALLEY FIRES: " + InSight(end)));
        walkNotes.Append("- Round the fin: first flame in sight " + firstFlame + "; from the path end: " + InSight(end) + ".\n");
        // valley cards from the Ward warp eye (Gate_8_14a_Marlow.md 11.3): every drawn mesh within wardNear m gets a temporary collider
        // so boulders and rock without colliders hide too; a card counts when its top is in sight
        var wardW = warpsRoot.transform.Find("Ward"); if (wardW == null) return "no warp Ward";
        var wardEye = Eye(wardW.position); var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
        foreach (var mf in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshFilter>(UnityEngine.FindObjectsSortMode.None))
        {
            var mr = mf.GetComponent<UnityEngine.MeshRenderer>(); if (mr == null || !mr.enabled || mf.sharedMesh == null || mf.GetComponent<UnityEngine.Collider>() != null) continue;
            if (mr.sharedMaterial != null && mr.sharedMaterial.shader == flameCardShader) continue;
            if (mr.bounds.SqrDistance(wardEye) > wardNear * wardNear) continue;
            temps.Add(mf.gameObject.AddComponent<UnityEngine.MeshCollider>());
        }
        UnityEngine.Physics.SyncTransforms();
        int valleyAll = 0, valleySeen = 0;
        foreach (var f in flameTops) if (f.group == "ValleyFlames") { valleyAll++; if (Clear(wardEye, f.top)) valleySeen++; }
        foreach (var t in temps) UnityEngine.Object.DestroyImmediate(t);
        UnityEngine.Physics.SyncTransforms();
        walkNotes.Append("- Valley fire cards with the top in sight from the Ward warp eye (" + wardEye.x.ToString("F1", inv) + ", " + wardEye.y.ToString("F1", inv) + ", " + wardEye.z.ToString("F1", inv) + "): " + valleySeen + " of " + valleyAll + ".\n");
        sb.Append("valley cards seen from the Ward warp eye " + valleySeen + " of " + valleyAll + "\n");
        // the pump trench: the last metres of Camp to pump, facing each side
        var pumpLeg = legs.Find(q => q.name == "Camp to pump"); if (pumpLeg.pts == null) return "no leg Camp to pump";
        float pumpLen = Length(pumpLeg.pts);
        foreach (var back in pumpBack)
        {
            var q = At(pumpLeg.pts, pumpLen - back); var q2 = At(pumpLeg.pts, pumpLen - back + 1f); var t = new UnityEngine.Vector3(q2.x - q.x, 0f, q2.z - q.z).normalized; var c = q + UnityEngine.Vector3.up * eye;
            frames.Add((c, c + new UnityEngine.Vector3(t.z, 0f, -t.x) * 10f, "PUMP TRENCH " + back.ToString("F0", inv) + " M FROM THE PUMP, RIGHT"));
            frames.Add((c, c - new UnityEngine.Vector3(t.z, 0f, -t.x) * 10f, "PUMP TRENCH " + back.ToString("F0", inv) + " M FROM THE PUMP, LEFT"));
        }
        // east from the front: the road, the T and the reflector posts past the chain-link
        foreach (var w in new[] { "Lot_Highway", "Store", "Gate_Booth", "Office" })
        {
            var wt = warpsRoot.transform.Find(w); if (wt == null) return "no warp " + w;
            var c = Eye(wt.position); frames.Add((c, new UnityEngine.Vector3(eastLookX, c.y, c.z), w.Replace('_', ' ').ToUpperInvariant() + " FACING EAST"));
        }
        Sheet("Walk_Views.jpg", "Hand-walk views: P4 east, round the fin (flame tops in sight), the pump trench, east from the front", frames, listDiv, listCols);
    }
    // 3d2. every junction marker from the trail side (Gate_8_14a_Pim.md 2): markerBack m from it toward the trail point nearest it,
    // facing it; the gate T set from the Lot Highway warp
    {
        var frames = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
        var jm = UnityEngine.GameObject.Find("Ground815/JunctionMarkers"); if (jm == null) return "no Ground815/JunctionMarkers";
        foreach (UnityEngine.Transform m in jm.transform)
        {
            var mp = m.position; float best = float.MaxValue; System.Collections.Generic.List<UnityEngine.Vector3> bl = null; float bs = 0f;
            foreach (var leg in legs) { float len = Length(leg.pts); for (float s = 0f; s <= len; s += 1f) { var q = At(leg.pts, s); float d = new UnityEngine.Vector2(q.x - mp.x, q.z - mp.z).magnitude; if (d < best) { best = d; bl = leg.pts; bs = s; } } }
            if (bl == null) continue;
            var toTrail = new UnityEngine.Vector3(At(bl, bs).x - mp.x, 0f, At(bl, bs).z - mp.z); if (toTrail.sqrMagnitude < 0.01f) toTrail = UnityEngine.Vector3.forward;
            var stand = mp + toTrail.normalized * markerBack; if (UnityEngine.Mathf.Abs(Ground(stand.x, stand.z, mp.y + markerBack) - Ground(mp.x, mp.z, mp.y)) > markerRise) stand = mp - toTrail.normalized * markerBack;   // off a bank: the other side
            var c = Eye(stand);
            frames.Add((c, new UnityEngine.Vector3(mp.x, Ground(mp.x, mp.z, mp.y) + markerLookUp, mp.z), m.name.Replace('_', ' ').ToUpperInvariant()));
        }
        var gb = warpsRoot.transform.Find("Lot_Highway"); var gt = UnityEngine.GameObject.Find("FrontZone/GateT");
        if (gb != null && gt != null) { var c = Eye(gb.position); frames.Add((c, new UnityEngine.Vector3(gt.transform.GetChild(0).position.x, c.y - 0.3f, gt.transform.GetChild(0).position.z), "GATE T SET FROM THE LOT")); }
        Sheet("Markers.jpg", "Junction markers from their trails, " + markerBack.ToString("F0", inv) + " m back", frames, listDiv, listCols);
    }

    // 3e. every collider without a renderer, scene-wide, active or not (Pim, Gate_8_14_Pim.md: only IW1 to IW3 may be invisible walls)
    int invisibleCount = 0; var invGroups = new System.Collections.Generic.SortedDictionary<string, (int n, int active, UnityEngine.Bounds b)>();
    {
        var list = new System.Text.StringBuilder("# Main3 colliders without a renderer\n\nEvery Collider in Main3 (active or not) whose GameObject has no Renderer, terrain and the player left out, from Tools/Recipes/main3_review_capture.cs. Each sits inside a visible mesh, is a stair or step ramp under visible steps, or is an invisible wall; the reviewers judge which. Positions are the collider's bounds centre and size in metres (x east, z north).\n\n");
        list.Append("| Collider | Kind | Active | Trigger | Layer | Centre x, y, z | Size x, y, z |\n|---|---|---|---|---|---|---|\n");
        var all = new System.Collections.Generic.List<UnityEngine.Collider>(UnityEngine.Object.FindObjectsByType<UnityEngine.Collider>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None));
        string PathOf(UnityEngine.Transform t) { var s = t.name; for (var p = t.parent; p != null; p = p.parent) s = p.name + "/" + s; return s; }
        all.Sort((a, b) => string.CompareOrdinal(PathOf(a.transform), PathOf(b.transform)));
        foreach (var col in all)
        {
            if (col is UnityEngine.TerrainCollider || col.GetComponent<UnityEngine.Renderer>() != null || col.transform.IsChildOf(pc.transform) || col.gameObject.scene != scene) continue;
            bool active = col.enabled && col.gameObject.activeInHierarchy;
            var b = col.bounds;   // bounds of an inactive collider read zero: from its transform instead
            if (!active) { var bc = col as UnityEngine.BoxCollider; b = bc != null ? new UnityEngine.Bounds(col.transform.TransformPoint(bc.center), UnityEngine.Vector3.Scale(bc.size, col.transform.lossyScale)) : new UnityEngine.Bounds(col.transform.position, col.transform.lossyScale); }   // 8.14a gate (Pim): an inactive box reports its own size
            string path = PathOf(col.transform); invisibleCount++;
            list.Append("| " + path + " | " + col.GetType().Name + " | " + (active ? "yes" : "no") + " | " + (col.isTrigger ? "yes" : "no") + " | " + UnityEngine.LayerMask.LayerToName(col.gameObject.layer) + " | " + b.center.x.ToString("F1", inv) + ", " + b.center.y.ToString("F1", inv) + ", " + b.center.z.ToString("F1", inv) + " | " + b.size.x.ToString("F1", inv) + ", " + b.size.y.ToString("F1", inv) + ", " + b.size.z.ToString("F1", inv) + " |\n");
            var parts = path.Split('/'); string group = parts.Length > 2 ? parts[0] + "/" + parts[1] : parts[0];
            if (invGroups.TryGetValue(group, out var g)) { g.b.Encapsulate(b); invGroups[group] = (g.n + 1, g.active + (active ? 1 : 0), g.b); } else invGroups[group] = (1, active ? 1 : 0, b);
        }
        System.IO.File.WriteAllText(System.IO.Path.Combine(outDir, "InvisibleColliders.md"), list.ToString().Replace("\r", ""));
    }
    sb.Append("invisible colliders " + invisibleCount + " in " + invGroups.Count + " groups (InvisibleColliders.md)\n");

    // 3e2. hedge boxes under brush (Gate_8_15_Pim.md 6): each HedgeCollider's top face sampled every hedgeSample m; a sample is covered
    // when a drawn bush or log (Ground815/Stops) stands over it (its bounds hold the point across and reach the box's top)
    int hedgeBoxes = 0, hedgeUnder = 0; float hedgeWorst = 1f; string hedgeWorstAt = "";
    {
        var stopsT = UnityEngine.GameObject.Find("Ground815/Stops"); if (stopsT == null) return "no Ground815/Stops";
        var brushB = new System.Collections.Generic.List<UnityEngine.Bounds>(); foreach (var r in stopsT.GetComponentsInChildren<UnityEngine.Renderer>()) if (r.name != "Rims") brushB.Add(r.bounds);
        var hl = new System.Text.StringBuilder("# Hedge boxes under brush\n\nFrom Tools/Recipes/main3_review_capture.cs. Each hedge collider's top face is sampled every " + hedgeSample.ToString("F1", inv) + " m; a sample is covered when a bush or log placed with the hedge stands over it (its bounds hold the point across and reach the box's top). Listed: every box under " + (hedgeNeed * 100f).ToString("F0", inv) + " percent.\n\n| Box | Centre x, z | Covered |\n|---|---|---|\n");
        foreach (var bc in stopsT.GetComponentsInChildren<UnityEngine.BoxCollider>())
        {
            if (bc.name != "HedgeCollider") continue; hedgeBoxes++; int n = 0, cov = 0; var tr = bc.transform;
            for (float lx = -bc.size.x * 0.5f; lx <= bc.size.x * 0.5f; lx += hedgeSample) for (float lz = -bc.size.z * 0.5f; lz <= bc.size.z * 0.5f; lz += hedgeSample)
            {
                var w = tr.TransformPoint(bc.center + new UnityEngine.Vector3(lx, bc.size.y * 0.5f, lz)); n++;
                foreach (var b in brushB) if (w.x >= b.min.x && w.x <= b.max.x && w.z >= b.min.z && w.z <= b.max.z && b.max.y >= w.y - hedgeTopSlack) { cov++; break; }
            }
            float share = n > 0 ? cov / (float)n : 1f; if (share >= hedgeNeed) hedgeUnder++; else hl.Append("| " + tr.parent.name + "/" + tr.name + " | " + tr.position.x.ToString("F0", inv) + ", " + tr.position.z.ToString("F0", inv) + " | " + (share * 100f).ToString("F0", inv) + " percent |\n");
            if (share < hedgeWorst) { hedgeWorst = share; hedgeWorstAt = tr.position.x.ToString("F0", inv) + ", " + tr.position.z.ToString("F0", inv); }
        }
        System.IO.File.WriteAllText(System.IO.Path.Combine(outDir, "HedgeCover.md"), hl.ToString().Replace("\r", ""));
    }
    sb.Append("hedge boxes " + hedgeUnder + " of " + hedgeBoxes + " at least " + (hedgeNeed * 100f).ToString("F0", inv) + " percent under brush; least " + (hedgeWorst * 100f).ToString("F0", inv) + " percent at (" + hedgeWorstAt + ")\n");

    // 3e3. foliage at each warp (Gate_8_15_Pim.md 7): the nearest bush, fir, pine or branch renderer within warpNear of a point 1 m ahead of
    // the eye, facing N, E, S and W
    var warpLines = new System.Collections.Generic.List<string>();
    {
        var fol = new System.Collections.Generic.List<UnityEngine.Renderer>();
        foreach (var r in UnityEngine.Object.FindObjectsByType<UnityEngine.Renderer>(UnityEngine.FindObjectsSortMode.None)) { var n = r.name; if (n.StartsWith("CS_Bush") || n.StartsWith("RedFir") || n.StartsWith("RedPine") || n.StartsWith("Branchs") || n.StartsWith("Bush")) fol.Add(r); }
        foreach (var w in warpList)
        {
            var e = Eye(w.position);
            foreach (var d in new[] { ("N", 0f), ("E", 90f), ("S", 180f), ("W", 270f) })
            {
                var q = Toward(e, d.Item2, 1f); float best = float.MaxValue; string what = "";
                foreach (var r in fol) { float dd = UnityEngine.Mathf.Sqrt(r.bounds.SqrDistance(q)); if (dd < best) { best = dd; what = r.name; } }
                if (best < warpNear) warpLines.Add(w.name + " " + d.Item1 + ": " + what + " " + best.ToString("F1", inv) + " m");
            }
        }
    }
    sb.Append("warp directions with foliage within " + warpNear.ToString("F1", inv) + " m: " + warpLines.Count + "\n");

    // 3e4. the road from the lot at full size (Wren, Vesper, Pim): the Lot Highway warp's own view, and a zoom (fov roadZoomFov) on the gate T set
    {
        var lw = warpsRoot.transform.Find("Lot_Highway"); var gt = UnityEngine.GameObject.Find("FrontZone/GateT"); if (lw == null || gt == null) return "no Lot_Highway warp or GateT";
        var c = Eye(lw.position); float fov0 = cam.fieldOfView;
        void Full(string file, UnityEngine.Vector3 look)
        {
            Pose(c, look); var px = Capture(); var t = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false); t.SetPixels32(px); t.Apply();
            System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), t.EncodeToJPG(jpgQuality)); UnityEngine.Object.DestroyImmediate(t); written.Add((file, "full size, 1920 x 988", 1));
        }
        Full("Lot_Road_Full.jpg", c + lw.forward * 40f);
        cam.fieldOfView = roadZoomFov; try { Full("GateT_Zoom.jpg", new UnityEngine.Vector3(gt.transform.GetChild(0).position.x, c.y - 0.5f, gt.transform.GetChild(0).position.z)); } finally { cam.fieldOfView = fov0; }
    }

    // 3f. grayscale trail frames, day one
    sb.Append(GreyTrails("Day one") + "\n");

    // 5. day halves of the pairs, kept for step night
    {
        var day = PairFrames("DAY ONE");
        for (int i = 0; i < day.Count; i++)
        {
            Pose(day[i].Item1, day[i].Item2); var px = Render(pairDiv);
            var t = new UnityEngine.Texture2D(shotW / pairDiv, shotH / pairDiv, UnityEngine.TextureFormat.RGB24, false); t.SetPixels32(px); t.Apply();
            System.IO.File.WriteAllBytes(System.IO.Path.Combine(tempDir, "pair_day_" + i + ".png"), t.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(t);
        }
    }

    // 6. top-down map (preview camera: no look filter), fog and lights off for the shot and restored, trails, warps and stops marked
    {
        bool fog = UnityEngine.RenderSettings.fog; UnityEngine.RenderSettings.fog = false;
        var lights = UnityEngine.Object.FindObjectsByType<UnityEngine.Light>(UnityEngine.FindObjectsSortMode.None); var wasOn = new bool[lights.Length];
        for (int i = 0; i < lights.Length; i++) { wasOn[i] = lights[i].enabled; lights[i].enabled = false; }
        var lightGo = new UnityEngine.GameObject("ReviewTopLight"); var lt = lightGo.AddComponent<UnityEngine.Light>(); lt.type = UnityEngine.LightType.Directional; lt.intensity = 1.1f; lt.shadows = UnityEngine.LightShadows.None;
        lightGo.transform.rotation = UnityEngine.Quaternion.Euler(55f, 315f, 0f);
        var camGo = new UnityEngine.GameObject("ReviewTopCam"); var tc = camGo.AddComponent<UnityEngine.Camera>(); tc.cameraType = UnityEngine.CameraType.Preview;
        tc.orthographic = true; tc.orthographicSize = mapTileM / 2f; tc.aspect = 1f; tc.nearClipPlane = 1f; tc.farClipPlane = 500f;
        tc.clearFlags = UnityEngine.CameraClearFlags.SolidColor; tc.backgroundColor = UnityEngine.Color.black; tc.enabled = false;
        var trt = new UnityEngine.RenderTexture(mapPx, mapPx, 24, UnityEngine.RenderTextureFormat.ARGB32); var tile = new UnityEngine.Texture2D(mapPx, mapPx, UnityEngine.TextureFormat.RGB24, false);
        int M = mapPx * mapTiles; NewCanvas(M, M);
        try
        {
            tc.targetTexture = trt;
            for (int tz = 0; tz < mapTiles; tz++) for (int tx = 0; tx < mapTiles; tx++)
            {
                camGo.transform.position = new UnityEngine.Vector3(mapX0 + (tx + 0.5f) * mapTileM, 300f, mapZ0 + (tz + 0.5f) * mapTileM); camGo.transform.rotation = UnityEngine.Quaternion.Euler(90f, 0f, 0f);
                tc.Render(); UnityEngine.RenderTexture.active = trt; tile.ReadPixels(new UnityEngine.Rect(0, 0, mapPx, mapPx), 0, 0); tile.Apply(); UnityEngine.RenderTexture.active = null;
                var tp = tile.GetPixels32(); for (int y = 0; y < mapPx; y++) System.Array.Copy(tp, y * mapPx, canvas, (tz * mapPx + y) * M + tx * mapPx, mapPx);
            }
        }
        finally
        {
            tc.targetTexture = null; trt.Release(); UnityEngine.Object.DestroyImmediate(trt); UnityEngine.Object.DestroyImmediate(tile);
            UnityEngine.Object.DestroyImmediate(camGo); UnityEngine.Object.DestroyImmediate(lightGo);
            for (int i = 0; i < lights.Length; i++) lights[i].enabled = wasOn[i]; UnityEngine.RenderSettings.fog = fog;
        }
        float ppm = mapPx / mapTileM;
        int MX(float x) => UnityEngine.Mathf.RoundToInt((x - mapX0) * ppm); int MY(float z) => M - 1 - UnityEngine.Mathf.RoundToInt((z - mapZ0) * ppm);
        void Dot(float x, float z, int r, UnityEngine.Color32 c) { for (int dy = -r; dy <= r; dy++) for (int dx = -r; dx <= r; dx++) if (dx * dx + dy * dy <= r * r) Px(MX(x) + dx, MY(z) + dy, c); }
        var trailC = new UnityEngine.Color32(255, 220, 60, 255); var warpC = new UnityEngine.Color32(80, 220, 255, 255); var stopC = new UnityEngine.Color32(255, 60, 60, 255);
        foreach (var leg in legs) { float len = Length(leg.pts); for (float s = 0f; s <= len; s += 0.25f) { var p = At(leg.pts, s); Dot(p.x, p.z, 1, trailC); } }
        for (int i = 0; i < stops.Count; i++) { Dot(stops[i].hit.x, stops[i].hit.z, 4, stopC); Text(MX(stops[i].hit.x) + 6, MY(stops[i].hit.z) - 7, "S" + (i + 1), 2, stopC); }
        for (int i = 0; i < warpList.Count; i++) { var w = warpList[i].position; Dot(w.x, w.z, 5, warpC); Text(MX(w.x) + 7, MY(w.z) + 2, "W" + (i + 1), 2, warpC); }
        Text(12, 12, "MAIN3 TOP-DOWN, NORTH UP, X " + mapX0 + " TO " + (mapX0 + mapTiles * mapTileM) + ", Z " + mapZ0 + " TO " + (mapZ0 + mapTiles * mapTileM) + ". YELLOW TRAILS, BLUE W WARPS, RED S STOPS", 2, white);
        SaveCanvas("Map_TopDown.jpg", "Top-down map, trails, warps (W) and stops (S) marked", 1);
    }

    // index
    var md = new System.Text.StringBuilder();
    md.Append("# Main3 review sheets\n\nMade by Tools/Recipes/main3_review_capture.sh (recipe main3_review_capture.cs), " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm", inv) + ". ");
    md.Append("Each frame is 1920 x 988 with the look filter on, eye 1.6 m over the ground, shrunk onto the sheet (trails, climb, warps, ends, stops at 1/3, pairs at 1/2). ");
    md.Append("Day one look except the Night halves of the pairs. Trail frames look along the trail, pitched with its grade over the next 10 m. Positions are metres, x east, z north.\n\n");
    md.Append("| Sheet | What | Frames |\n|---|---|---|\n");
    foreach (var w in written) md.Append("| [" + w.file + "](" + w.file + ") | " + w.what + " | " + w.frames + " |\n");
    md.Append("| [Pairs_DayOne_Night.jpg](Pairs_DayOne_Night.jpg) | Day one (left) and Night (right) at the " + pairs.Length + " pair spots below | " + (pairs.Length * 2) + " |\n");
    md.Append("| [Grey_Trails_Night.jpg](Grey_Trails_Night.jpg) | Grayscale trails, Night (step night; its table is at the end) | |\n");
    md.Append("| [InvisibleColliders.md](InvisibleColliders.md) | Every collider without a renderer, scene-wide, active or not (" + invisibleCount + ") | |\n");
    md.Append("| [HedgeCover.md](HedgeCover.md) | Hedge boxes under brush: " + hedgeUnder + " of " + hedgeBoxes + " at least " + (hedgeNeed * 100f).ToString("F0", inv) + " percent covered | |\n");
    md.Append("\n## Warp foliage\n\nWarp directions with a bush, fir, pine or branch within " + warpNear.ToString("F1", inv) + " m of a point 1 m ahead of the eye: " + (warpLines.Count == 0 ? "none" : string.Join("; ", warpLines)) + ".\n");
    md.Append("\n## Hand-walk views\n\n" + walkNotes + "\n## Colliders without a renderer, by group\n\n| Group | Colliders | Active | Bounds x | Bounds y | Bounds z |\n|---|---|---|---|---|---|\n");
    foreach (var kv in invGroups) md.Append("| " + kv.Key + " | " + kv.Value.n + " | " + kv.Value.active + " | " + kv.Value.b.min.x.ToString("F0", inv) + " to " + kv.Value.b.max.x.ToString("F0", inv) + " | " + kv.Value.b.min.y.ToString("F0", inv) + " to " + kv.Value.b.max.y.ToString("F0", inv) + " | " + kv.Value.b.min.z.ToString("F0", inv) + " to " + kv.Value.b.max.z.ToString("F0", inv) + " |\n");
    md.Append("\n## Warps\n\n");
    for (int i = 0; i < warpList.Count; i++) md.Append("- W" + (i + 1) + " " + warpList[i].name + " (" + warpList[i].position.x.ToString("F0", inv) + ", " + warpList[i].position.z.ToString("F0", inv) + ")\n");
    md.Append("\n## Stops\n\nAn invisible collider (no renderer, not terrain) within 5 m of a trail centre line whose face turns toward the walker (more than 45 degrees off the trail's side). Walls running along the trail sides are left out; the trail frames show those. One frame per 8 m cluster, taken from the trail centre facing the stop.\n\n");
    md.Append("| Stop | Trail | Metres | x, z | Distance | Collider |\n|---|---|---|---|---|---|\n");
    for (int i = 0; i < stops.Count; i++) md.Append("| S" + (i + 1) + " | " + stops[i].leg + " | " + stops[i].s.ToString("F0", inv) + " | " + stops[i].hit.x.ToString("F0", inv) + ", " + stops[i].hit.z.ToString("F0", inv) + " | " + stops[i].d.ToString("F1", inv) + " | " + stops[i].what + " |\n");
    md.Append("\n## Pair spots\n\n");
    foreach (var p in pairs) md.Append("- " + p.n + ": stand (" + p.x.ToString("F0", inv) + ", " + p.z.ToString("F0", inv) + "), facing (" + p.lx.ToString("F0", inv) + ", " + p.lz.ToString("F0", inv) + ")\n");
    md.Append(greyMd);
    System.IO.File.WriteAllText(System.IO.Path.Combine(outDir, "index.md"), md.ToString().Replace("\r", ""));
}
finally { cam.targetTexture = null; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot); }
return "day done: " + written.Count + " sheets in " + clock.Elapsed.TotalSeconds.ToString("F0") + " s\n" + sb;
