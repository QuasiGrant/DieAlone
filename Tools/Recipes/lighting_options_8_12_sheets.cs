// 8.12 (Play mode, Main3): the lighting option sheets in Docs/Design/LightingOptions.md section 3. Six fixed spots
// (Camp, S1, under the giants = Jg to Camp 1 FWD 40, the office, J, the cabin interior from the doorway), same camera,
// height and heading in every look; filter on, 1920 x 988, no dev panel (camera render). Looks are picked by their TIME
// row label (lighting_options_8_12_looks.cs builds them).
// Sheet 1 (Vesper): D1 now plus V1 to V10, six rows, eleven columns. Sheet 2 (Grant): D1 now, A, A Tri, B, C, large.
// 8.14a: sheet 3 (Vesper), day one before 8.14a against A Tri (day one now), to Docs/Captures/Main3Review/Lighting_D1_before_vs_ATri.jpg.
// 8.15: sheet 4 (Vesper), sun elevation 24 (the A Tri row) against 20 (day one now), to Docs/Captures/Main3Review/Lighting_Sun24_vs_Sun20.jpg.
// 8.15 gate: sheet 5 (Vesper), the gold band at 30 against 6 (day one now), to Docs/Captures/Main3Review/Lighting_Band30_vs_Band6.jpg.
// Writes to Docs/Captures/LightingOptions (git-ignored): the two sheets, Options_notes.md with the spots, and per look
// whether camp, J, W1, Camp 3 and the lake pump are in sun (terrain shade from the sun direction; B's lower sun is the
// risk, LightingOptions.md 2) and, for D1, where the sky meets the land in each outdoor frame (the gold band check).
// Runs from EditorApplication.update, one step per few frames, and removes itself; poll for "done" in the notes file.
// Leaves the player where it was, the look on Day one and the controller back on.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
const int shotW = 1920, shotH = 988, jpgQuality = 88, FramesPerStep = 3, SettleSteps = 2, Div1 = 5, Div2 = 2;
const float Eye = 1.6f, TrailAt = 40f, SunProbeHeight = 1.6f, SunProbeReach = 2000f, CabinYaw = -15f, CabinReach = 4f;
const int SkyColumnStep = 16, SkyMatch = 6, SkyLineOffset = 4;
string outDir = System.IO.Path.GetFullPath("Docs/Captures/LightingOptions");
System.IO.Directory.CreateDirectory(outDir);
string notesPath = System.IO.Path.Combine(outDir, "Options_notes.md");
if (System.IO.File.Exists(notesPath)) System.IO.File.Delete(notesPath);
var inv = System.Globalization.CultureInfo.InvariantCulture;
UnityEngine.Application.runInBackground = true;

var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
if (preview == null || pc == null) return "no LookPreview or player";
var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation;
var startPos = pc.transform.position; var startRot = pc.transform.rotation;
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var terrain = UnityEngine.Terrain.activeTerrain;
var terrainCollider = terrain.GetComponent<UnityEngine.TerrainCollider>();
float Ground(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
int LookIndex(string label) { for (int i = 0; i < preview.Count; i++) if (preview.Label(i) == label) return i; return -1; }
var sheet1 = new[] { "Day one", "V1 Ambient", "V2 Fog colour", "V3 Fog range", "V4 Crush", "V5 Wash", "V6 Sun height", "V7 Sun bearing", "V8 Sun strength", "V9 Corners", "V10 Glow" };
var sheet2 = new[] { "Day one", "A Vesper", "A Tri", "B Late gold", "C Overcast" };
var sheet3 = new[] { "D1 before 8.14a", "Day one" };   // 8.14a (Vesper): before and after A Tri became day one, into the Main3 review folder
var sheet4 = new[] { "A Tri", "Day one" };   // 8.15 (Vesper): sun elevation 24 (A Tri) against 20 (day one now), one change
var sheet5 = new[] { "D1 band 30", "Day one" };   // 8.15 gate (Vesper): the gold band at 30 degrees against 6 (day one now), one change
string reviewDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review"); System.IO.Directory.CreateDirectory(reviewDir);
var allLooks = new System.Collections.Generic.List<string>(sheet1); foreach (var l in sheet2) if (!allLooks.Contains(l)) allLooks.Add(l); foreach (var l in sheet3) if (!allLooks.Contains(l)) allLooks.Add(l); foreach (var l in sheet4) if (!allLooks.Contains(l)) allLooks.Add(l); foreach (var l in sheet5) if (!allLooks.Contains(l)) allLooks.Add(l);
foreach (var l in allLooks) if (LookIndex(l) < 0) return "no TIME row " + l + "; run lighting_options_8_12_looks.cs";

// ---- the six spots: (name, camera, look-at, outdoor)
UnityEngine.Vector3 At(float x, float z) => new UnityEngine.Vector3(x, Ground(x, z) + Eye, z);
UnityEngine.Vector3 Ahead(UnityEngine.Vector3 c, float yaw) => c + UnityEngine.Quaternion.Euler(0f, yaw, 0f) * UnityEngine.Vector3.forward * 40f;
var trails = Root("Trails"); var leg = trails != null ? trails.transform.Find("Jg to Camp 1") : null;
if (leg == null || leg.childCount < 2) return "no Trails/Jg to Camp 1";
var legPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) legPts.Add(p.position);
UnityEngine.Vector3 Along(float s)
{
    for (int i = 1; i < legPts.Count; i++) { float d = UnityEngine.Vector3.Distance(legPts[i - 1], legPts[i]); if (s <= d) return UnityEngine.Vector3.Lerp(legPts[i - 1], legPts[i], s / d); s -= d; }
    return legPts[legPts.Count - 1];
}
var giantsFoot = Along(TrailAt); var giantsAhead = Along(TrailAt + 3f); var giantsFar = Along(TrailAt + 10f);
var giantsCam = giantsFoot + UnityEngine.Vector3.up * Eye;
var giantsDir = new UnityEngine.Vector3(giantsAhead.x - giantsFoot.x, 0f, giantsAhead.z - giantsFoot.z).normalized;
var giantsLook = giantsCam + giantsDir * 10f + UnityEngine.Vector3.up * (giantsFar.y - giantsFoot.y);
var cabinWarp = Root("DevWarps").transform.Find("Cabin");
float cabinEye = cabinWarp.position.y + Eye;
var spots = new (string name, UnityEngine.Vector3 cam, UnityEngine.Vector3 look, bool outdoor)[] {
    ("Camp", At(172f, 150f), Ahead(At(172f, 150f), 333f), true),
    ("S1", At(156f, 148f), new UnityEngine.Vector3(178f, At(156f, 148f).y, 168f), true),
    ("Under the giants", giantsCam, giantsLook, true),
    ("Office", At(340f, 196f), Ahead(At(340f, 196f), 68f), true),
    ("J", At(106f, 203f), Ahead(At(106f, 203f), 316f), true),
    ("Cabin interior", new UnityEngine.Vector3(178.6f, cabinEye, 166.0f), new UnityEngine.Vector3(178.6f, cabinEye - 0.6f, 166.0f) + UnityEngine.Quaternion.Euler(0f, CabinYaw, 0f) * UnityEngine.Vector3.forward * CabinReach, false) };   // just inside the door, turned left so the desk (west) and the stove (east) both fit
var sunPlaces = new (string name, float x, float z)[] { ("Camp", 172f, 150f), ("J", 106f, 203f), ("W1", 130f, 72f), ("Camp 3", 74f, 142f), ("Lake pump", 190f, 99f) };

// ---- rendering
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32);
var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
void Pose(UnityEngine.Vector3 camPos, UnityEngine.Vector3 look)
{
    var dir = look - camPos; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
    cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = camPos - pc.transform.rotation * camLocal;
    cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
}
UnityEngine.Color32[] RenderFull(int mask)
{
    int saved = cam.cullingMask; cam.cullingMask = mask;
    cam.targetTexture = rt; cam.Render(); cam.targetTexture = null; cam.cullingMask = saved;
    UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
    return shot.GetPixels32();
}
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

// ---- 5 x 7 pixel font for labels (as main3_review_capture.cs)
var glyphs = new System.Collections.Generic.Dictionary<char, int[]>();
foreach (var g in ("A0E11111F111111 B1E11111E11111E C0E11101010110E D1C121111111210 E1F10101E10101F F1F10101E101010 G0E11101711110F H1111111F111111 "
    + "I0E04040404040E J0702020202120C K11121418141211 L1010101010101F M111B1515111111 N11111915131111 O0E11111111110E P1E11111E101010 Q0E11111115120D "
    + "R1E11111E141211 S0F10100E01011E T1F040404040404 U1111111111110E V11111111110A04 W1111111515150A X11110A040A1111 Y1111110A040404 Z1F01020408101F "
    + "00E11131519110E 1040C040404040E 20E11010204081F 31F02040201110E 402060A121F0202 51F101E0101110E 60608101E11110E 71F010204080808 80E11110E11110E "
    + "90E11110F01020C -0000001F000000 .0000000000000C (02040808080402 )08040202020408 /00010204081000 ,000000000C0408 :000C0C000C0C00").Split(' '))
{
    var rows = new int[7]; for (int i = 0; i < 7; i++) rows[i] = System.Convert.ToInt32(g.Substring(1 + i * 2, 2), 16); glyphs[g[0]] = rows;
}
UnityEngine.Color32[] canvas = null; int cW = 0, cH = 0;
void NewCanvas(int w, int h) { cW = w; cH = h; canvas = new UnityEngine.Color32[w * h]; var bg = new UnityEngine.Color32(18, 18, 20, 255); for (int i = 0; i < canvas.Length; i++) canvas[i] = bg; }
void Px(int x, int yTop, UnityEngine.Color32 c) { if (x >= 0 && x < cW && yTop >= 0 && yTop < cH) canvas[(cH - 1 - yTop) * cW + x] = c; }
void Text(int x, int yTop, string s, int scale, UnityEngine.Color32 c)
{
    foreach (var ch0 in s.ToUpperInvariant())
    {
        var ch = glyphs.ContainsKey(ch0) || ch0 == ' ' ? ch0 : '-';
        if (ch != ' ') { var rows = glyphs[ch]; for (int ry = 0; ry < 7; ry++) for (int rx = 0; rx < 5; rx++) if ((rows[ry] >> (4 - rx) & 1) == 1) for (int sy = 0; sy < scale; sy++) for (int sx = 0; sx < scale; sx++) Px(x + rx * scale + sx, yTop + ry * scale + sy, c); }
        x += 6 * scale;
    }
}
void Blit(UnityEngine.Color32[] thumb, int tw, int th, int x, int yTop) { for (int y = 0; y < th; y++) for (int xx = 0; xx < tw; xx++) Px(x + xx, yTop + (th - 1 - y), thumb[y * tw + xx]); }
var white = new UnityEngine.Color32(235, 235, 235, 255); var gold = new UnityEngine.Color32(255, 205, 90, 255);
const int labelH = 26, headH = 48, gap = 4, rowLabelW = 230;
void MakeSheet(string dir, string file, string title, string[] looks, System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]> thumbs, int div, string nowLabel)
{
    int tw = shotW / div, th = shotH / div;
    NewCanvas(rowLabelW + looks.Length * (tw + gap) + gap, headH + labelH + spots.Length * (th + gap) + gap);
    Text(gap + 4, 12, title, 3, gold);
    for (int c = 0; c < looks.Length; c++) Text(rowLabelW + c * (tw + gap) + 4, headH + 6, looks[c] == "Day one" ? nowLabel : looks[c], 2, gold);
    for (int s = 0; s < spots.Length; s++)
    {
        int y = headH + labelH + s * (th + gap);
        Text(gap + 4, y + th / 2 - 7, spots[s].name, 2, white);
        for (int c = 0; c < looks.Length; c++) Blit(thumbs[looks[c]][s], tw, th, rowLabelW + c * (tw + gap), y);
    }
    var tex = new UnityEngine.Texture2D(cW, cH, UnityEngine.TextureFormat.RGB24, false); tex.SetPixels32(canvas); tex.Apply();
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(dir, file), tex.EncodeToJPG(jpgQuality));
    UnityEngine.Object.DestroyImmediate(tex); canvas = null;
}

// ---- checks
string SunReport()
{
    var sun = UnityEngine.RenderSettings.sun; if (sun == null) return "no sun";
    var toSun = -sun.transform.forward; var parts = new System.Collections.Generic.List<string>();
    foreach (var p in sunPlaces)
    {
        var from = new UnityEngine.Vector3(p.x, Ground(p.x, p.z) + SunProbeHeight, p.z);
        bool shade = terrainCollider.Raycast(new UnityEngine.Ray(from, toSun), out _, SunProbeReach);
        parts.Add(p.name + (shade ? " SHADE" : " sun"));
    }
    return string.Join(", ", parts);
}
// Where the sky meets the land: every SkyColumnStep columns, scan down from the top while the frame matches a sky-only
// render (culling mask 0); the last match is the skyline. Reports the share of columns with sky, the lowest and the
// median skyline elevation, and how far SkyGradient has blended from horizon to top at the median
// (t = saturate(1.8 * (sin(elevation) - sin(skyBandHeight))), the shader's own formula), with the frame's colour there.
string SkyLine(UnityEngine.Color32[] full, UnityEngine.Color32[] skyOnly)
{
    float f = (shotH * 0.5f) / UnityEngine.Mathf.Tan(cam.fieldOfView * 0.5f * UnityEngine.Mathf.Deg2Rad);
    var elevs = new System.Collections.Generic.List<(float e, UnityEngine.Color32 c)>(); int columns = 0;
    for (int x = 0; x < shotW; x += SkyColumnStep)
    {
        columns++; int line = -1;
        for (int y = shotH - 1; y >= 0; y--)
        {
            var a = full[y * shotW + x]; var b = skyOnly[y * shotW + x];
            if (System.Math.Abs(a.r - b.r) + System.Math.Abs(a.g - b.g) + System.Math.Abs(a.b - b.b) > SkyMatch) break;
            line = y;
        }
        if (line < 0) continue;
        var ray = cam.transform.forward * f + cam.transform.right * (x - shotW * 0.5f) + cam.transform.up * (line - shotH * 0.5f);
        elevs.Add((UnityEngine.Mathf.Asin(ray.normalized.y) * UnityEngine.Mathf.Rad2Deg, full[UnityEngine.Mathf.Min(line + SkyLineOffset, shotH - 1) * shotW + x]));
    }
    if (elevs.Count == 0) return "no sky in frame";
    elevs.Sort((p, q) => p.e.CompareTo(q.e));
    var mid = elevs[elevs.Count / 2];
    var bandLook = LookOverride.Tuning; float band = bandLook != null ? UnityEngine.Mathf.Sin(bandLook.skyBandHeight * UnityEngine.Mathf.Deg2Rad) : 0f;
    float t = UnityEngine.Mathf.Clamp01(1.8f * (UnityEngine.Mathf.Sin(mid.e * UnityEngine.Mathf.Deg2Rad) - band));
    return "sky in " + (100 * elevs.Count / columns) + " pct of columns; skyline lowest " + elevs[0].e.ToString("F1", inv) + " deg, median " + mid.e.ToString("F1", inv)
        + " deg, where the gradient is " + (t * 100f).ToString("F0", inv) + " pct of the way from horizon to top; sky there #" + UnityEngine.ColorUtility.ToHtmlStringRGB(mid.c);
}

// ---- steps
var thumbs1 = new System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]>();
var thumbs2 = new System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]>();
var thumbs3 = new System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]>();
var thumbs4 = new System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]>();
var thumbs5 = new System.Collections.Generic.Dictionary<string, UnityEngine.Color32[][]>();
var notes = new System.Text.StringBuilder();
notes.Append("# Lighting options (8.12)\n\nGenerated by Tools/Recipes/lighting_options_8_12_sheets.cs. Frames 1920 x 988, filter on, eye " + Eye.ToString("F1", inv) + " m.\n\n## Spots\n\n");
foreach (var s in spots) notes.Append("- " + s.name + ": camera (" + s.cam.x.ToString("F1", inv) + ", " + s.cam.y.ToString("F1", inv) + ", " + s.cam.z.ToString("F1", inv) + "), looking at (" + s.look.x.ToString("F1", inv) + ", " + s.look.y.ToString("F1", inv) + ", " + s.look.z.ToString("F1", inv) + ")\n");
notes.Append("\n## Sun or terrain shade (ray toward the sun from 1.6 m over the ground, terrain only)\n\n");
var steps = new System.Collections.Generic.List<System.Action>();
pc.enabled = false;
foreach (var look in allLooks)
{
    string l = look;
    steps.Add(() => preview.Select(LookIndex(l)));
    for (int i = 0; i < SettleSteps; i++) steps.Add(() => { });
    for (int si = 0; si < spots.Length; si++)
    {
        int s = si;
        steps.Add(() =>
        {
            Pose(spots[s].cam, spots[s].look);
            var full = RenderFull(cam.cullingMask);
            if (System.Array.IndexOf(sheet1, l) >= 0) { if (!thumbs1.ContainsKey(l)) thumbs1[l] = new UnityEngine.Color32[spots.Length][]; thumbs1[l][s] = Shrink(full, Div1); }
            if (System.Array.IndexOf(sheet2, l) >= 0) { if (!thumbs2.ContainsKey(l)) thumbs2[l] = new UnityEngine.Color32[spots.Length][]; thumbs2[l][s] = Shrink(full, Div2); }
            if (System.Array.IndexOf(sheet3, l) >= 0) { if (!thumbs3.ContainsKey(l)) thumbs3[l] = new UnityEngine.Color32[spots.Length][]; thumbs3[l][s] = Shrink(full, Div2); }
            if (System.Array.IndexOf(sheet4, l) >= 0) { if (!thumbs4.ContainsKey(l)) thumbs4[l] = new UnityEngine.Color32[spots.Length][]; thumbs4[l][s] = Shrink(full, Div2); }
            if (System.Array.IndexOf(sheet5, l) >= 0) { if (!thumbs5.ContainsKey(l)) thumbs5[l] = new UnityEngine.Color32[spots.Length][]; thumbs5[l][s] = Shrink(full, Div2); }
            if (l == "Day one" && spots[s].outdoor) notes.Append("- sky, D1 now, " + spots[s].name + ": " + SkyLine(full, RenderFull(0)) + "\n");
        });
    }
    steps.Add(() => notes.Append("- " + l + ": " + SunReport() + "\n"));
}
steps.Add(() => MakeSheet(outDir, "Options_Sheet1_Vesper_D1_V1-V10.jpg", "Sheet 1: D1 now and V1 to V10 (one change each)", sheet1, thumbs1, Div1, "D1 now"));
steps.Add(() => MakeSheet(outDir, "Options_Sheet2_Grant_D1_A_B_C.jpg", "Sheet 2: D1 now, A, A Tri, B, C", sheet2, thumbs2, Div2, "D1 now"));
steps.Add(() => MakeSheet(reviewDir, "Lighting_D1_before_vs_ATri.jpg", "Day one before 8.14a (left) and A Tri, day one now (right)", sheet3, thumbs3, Div2, "A Tri, day one now"));
steps.Add(() => MakeSheet(reviewDir, "Lighting_Sun24_vs_Sun20.jpg", "Sun elevation 24 (A Tri, left) and 20 (day one now, right)", sheet4, thumbs4, Div2, "Sun 20, day one now"));
steps.Add(() => MakeSheet(reviewDir, "Lighting_Band30_vs_Band6.jpg", "Sky: gold band at 30 degrees (left) and 6 (day one now, right)", sheet5, thumbs5, Div2, "Band 6, day one now"));

int index = 0, lastFrame = UnityEngine.Time.frameCount;
UnityEditor.EditorApplication.CallbackFunction tick = null;
tick = () =>
{
    bool finish = !UnityEngine.Application.isPlaying || index >= steps.Count;
    if (!finish && UnityEngine.Time.frameCount - lastFrame >= FramesPerStep)
    {
        lastFrame = UnityEngine.Time.frameCount;
        try { steps[index](); } catch (System.Exception e) { notes.Append("\nFAIL step " + index + ": " + e.Message + "\n"); index = steps.Count; }
        index++;
    }
    if (finish)
    {
        UnityEditor.EditorApplication.update -= tick;
        if (UnityEngine.Application.isPlaying)
        {
            preview.Select(LookIndex("Day one"));
            pc.transform.position = startPos; pc.transform.rotation = startRot; cam.transform.localRotation = camRot; cc.enabled = true; pc.enabled = true;
        }
        UnityEngine.Object.Destroy(rt); UnityEngine.Object.Destroy(shot);
        notes.Append("\ndone\n");
        System.IO.File.WriteAllText(notesPath, notes.ToString());
    }
};
UnityEditor.EditorApplication.update += tick;
return "started " + steps.Count + " steps; notes " + notesPath;
