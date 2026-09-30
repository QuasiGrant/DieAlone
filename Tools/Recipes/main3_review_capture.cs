// Main3 review capture (Play mode, Main3). Shows the map the way a player sees it, for Marlow, Pim and Vesper before every
// Grant walk. Run it with Tools/Recipes/main3_review_capture.sh, which enters Play mode, runs step "day" then step "night",
// leaves Play mode and resets runInBackground. Every frame is 1920 x 988 (Grant's Game view shape at half size), rendered
// from Camera.main into a RenderTexture (the look filter draws; screen-space UI does not), eye 1.6 m over the ground,
// then shrunk into labelled contact sheets (JPG) in outDir with an index.md. Nothing in the scene or any asset is saved.
// step "day" (day one look): one sheet per trail (both directions, a frame every 10 m, looking along the trail), the climb
// (J to Ward, up, every 10 m), warps (N, E, S, W), trail ends (facing out), stops (every invisible collider that faces a
// walker within 5 m of a trail centre line, one frame per 8 m cluster, facing it), the top-down map with trails, warps and
// stops marked, and the day halves of the six day and night pairs (kept in Temp/ReviewCapture).
// step "night" (Night look): the night halves and the pairs sheet. If the look is wrong the step selects it and returns
// "run again" (the look applies on the next frame).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
string step = "day";
string outDir = System.IO.Path.GetFullPath("Docs/Review/2026-09-30-ValleyReview/sheets");
string tempDir = System.IO.Path.GetFullPath("Temp/ReviewCapture");
UnityEngine.Application.runInBackground = true;
const int shotW = 1920, shotH = 988, jpgQuality = 85;
const float eye = 1.6f, trailStep = 10f, stopReach = 5f, stopCluster = 8f, stopFacing = 0.7f, stopProbeStep = 1f, stopProbeHeight = 1f;
const int stopRays = 24, trailDiv = 3, trailCols = 5, warpDiv = 3, listDiv = 3, listCols = 5, pairDiv = 2, stopsPerSheet = 60;
const int mapTiles = 5, mapPx = 512; const float mapTileM = 100f, mapX0 = -50f, mapZ0 = -75f;
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
UnityEngine.Color32[] Render(int div)
{
    cam.targetTexture = rt; cam.Render(); cam.targetTexture = null;
    UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
    var src = shot.GetPixels32(); int w = shotW / div, h = shotH / div; var dst = new UnityEngine.Color32[w * h]; int n = div * div;
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
// six day and night spots: (name, stand x, z, look-at x, y, z); NaN look y means level
var pairs = new (string n, float x, float z, float lx, float ly, float lz)[] {
    ("Camp (Keepers Camp warp)", 172f, 150f, 172f + 40f * UnityEngine.Mathf.Sin(333f * UnityEngine.Mathf.Deg2Rad), float.NaN, 150f + 40f * UnityEngine.Mathf.Cos(333f * UnityEngine.Mathf.Deg2Rad)),
    ("S1 camp from the edge (LookSlice 6)", 156f, 148f, 178f, float.NaN, 168f),
    ("Office (Office warp)", 340f, 196f, 340f + 40f * UnityEngine.Mathf.Sin(68f * UnityEngine.Mathf.Deg2Rad), float.NaN, 196f + 40f * UnityEngine.Mathf.Cos(68f * UnityEngine.Mathf.Deg2Rad)),
    ("Lot centre looking north to office and store", 358f, 170f, 358f, float.NaN, 200f),
    ("J (Junction J warp)", 106f, 203f, 106f + 40f * UnityEngine.Mathf.Sin(316f * UnityEngine.Mathf.Deg2Rad), float.NaN, 203f + 40f * UnityEngine.Mathf.Cos(316f * UnityEngine.Mathf.Deg2Rad)),
    ("Ward path end (Ward warp)", -2f, 258f, -42f, float.NaN, 258f) };
System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)> PairFrames(string half)
{
    var f = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, string)>();
    foreach (var p in pairs) { var c = Eye(new UnityEngine.Vector3(p.x, 200f, p.z)); var lk = new UnityEngine.Vector3(p.lx, float.IsNaN(p.ly) ? c.y : p.ly, p.lz); f.Add((c, lk, p.n + " " + half)); }
    return f;
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
        System.IO.Directory.Delete(tempDir, true);
        return "night done: wrote Pairs_DayOne_Night.jpg (" + pairs.Length + " pairs) in " + clock.Elapsed.TotalSeconds.ToString("F0") + " s";
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
    md.Append("| [Pairs_DayOne_Night.jpg](Pairs_DayOne_Night.jpg) | Day one (left) and Night (right) from camp, S1, the office, the lot, J and the Ward path end | " + (pairs.Length * 2) + " |\n");
    md.Append("\n## Warps\n\n");
    for (int i = 0; i < warpList.Count; i++) md.Append("- W" + (i + 1) + " " + warpList[i].name + " (" + warpList[i].position.x.ToString("F0", inv) + ", " + warpList[i].position.z.ToString("F0", inv) + ")\n");
    md.Append("\n## Stops\n\nAn invisible collider (no renderer, not terrain) within 5 m of a trail centre line whose face turns toward the walker (more than 45 degrees off the trail's side). Walls running along the trail sides are left out; the trail frames show those. One frame per 8 m cluster, taken from the trail centre facing the stop.\n\n");
    md.Append("| Stop | Trail | Metres | x, z | Distance | Collider |\n|---|---|---|---|---|---|\n");
    for (int i = 0; i < stops.Count; i++) md.Append("| S" + (i + 1) + " | " + stops[i].leg + " | " + stops[i].s.ToString("F0", inv) + " | " + stops[i].hit.x.ToString("F0", inv) + ", " + stops[i].hit.z.ToString("F0", inv) + " | " + stops[i].d.ToString("F1", inv) + " | " + stops[i].what + " |\n");
    md.Append("\n## Pair spots\n\n");
    foreach (var p in pairs) md.Append("- " + p.n + ": stand (" + p.x.ToString("F0", inv) + ", " + p.z.ToString("F0", inv) + "), facing (" + p.lx.ToString("F0", inv) + ", " + p.lz.ToString("F0", inv) + ")\n");
    System.IO.File.WriteAllText(System.IO.Path.Combine(outDir, "index.md"), md.ToString().Replace("\r", ""));
}
finally { cam.targetTexture = null; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot); }
return "day done: " + written.Count + " sheets in " + clock.Elapsed.TotalSeconds.ToString("F0") + " s\n" + sb;
