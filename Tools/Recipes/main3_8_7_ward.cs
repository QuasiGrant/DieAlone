// Main3 task 8.7: Ward pieces, gray. Run after 8.6 in Main3, edit mode.
// Cairn gate 4 m up the J to Ward trail: pale cairn on its west side, clear of the Camp to J trail end (Marlow finding 9), and a
// solid chain at 0.9 m across it to a post: the climb is closed by day (DECISIONS 2026-09-25); a later night rule opens it,
// and the dev warps reach the plateau meanwhile. The climb itself is 8.3's J to Ward trail; the Wall and plateau are 8.1's terrain.
// Ward on the high plateau (rev 15, 3.6): stones at (16, 266), (21, 263), (18, 270), 3.6 x 4 m, tops 82, to the right of the
// rock lip at (13, 256), 0.8 m high. The Tor and the stone screen are gone (the Wall hides the Ward from the tower).
// Stand-in burning ridge and valley fire beyond the west edge (3.7), built switched off.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("FrontZone") == null) return "run 8.6 first";
if (Root("Ward") != null) return "Ward already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P2(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 wp, UnityEngine.Vector3 sc, float yaw = 0f, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.position = wp; g.transform.localScale = sc;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Sph = UnityEngine.PrimitiveType.Sphere;
var ward = new UnityEngine.GameObject("Ward").transform;

// the J to Ward trail start, for placing the cairn gate across it
var legT = Root("Trails").transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
var p0 = legT.GetChild(0).position; var p1 = legT.GetChild(2).position;
var along = V(p1.x - p0.x, 0f, p1.z - p0.z).normalized; var side = V(along.z, 0f, -along.x);
var gateAt = p0 + along * 4.0f;
var cairnGate = new UnityEngine.GameObject("CairnGate").transform; cairnGate.SetParent(ward, false);
var cairnPos = gateAt - side * 2.2f; cairnPos.y = H(cairnPos.x, cairnPos.z);
float cy = cairnPos.y;
foreach (var s in new[] { (1.4f, 0.5f), (1.1f, 0.45f), (0.85f, 0.4f), (0.6f, 0.35f), (0.4f, 0.3f) })
{ Prim(Sph, "Stone", cairnGate, V(cairnPos.x, cy + s.Item2 * 0.45f, cairnPos.z), V(s.Item1, s.Item2, s.Item1)); cy += s.Item2 * 0.8f; }
var postPos = gateAt + side * 2.2f; postPos.y = H(postPos.x, postPos.z);
Prim(Cube, "ChainPost", cairnGate, postPos + V(0f, 0.55f, 0f), V(0.15f, 1.1f, 0.15f));
var chainA = cairnPos + V(0f, 0.9f, 0f); var chainB = postPos + V(0f, 0.9f, 0f);
var chain = Prim(Cube, "Chain", cairnGate, (chainA + chainB) * 0.5f, V(0.06f, 0.06f, UnityEngine.Vector3.Distance(chainA, chainB)), 0f, true);   // solid: the climb is closed by day
chain.transform.rotation = UnityEngine.Quaternion.LookRotation(chainB - chainA, UnityEngine.Vector3.up);
// the gate proper (8.9b): an invisible block 3 m tall across the whole trail corridor and into the side walls, on the
// Ignore Raycast layer, so the climb cannot be crouched under the chain, jumped over it or passed round the cairn end
var gateBlock = new UnityEngine.GameObject("GateBlocker"); gateBlock.transform.SetParent(cairnGate, false); gateBlock.layer = 2;
gateBlock.transform.position = V(gateAt.x, H(gateAt.x, gateAt.z) + 1.2f, gateAt.z); gateBlock.transform.rotation = UnityEngine.Quaternion.LookRotation(along, UnityEngine.Vector3.up);
gateBlock.AddComponent<UnityEngine.BoxCollider>().size = V(6.0f, 3.4f, 0.5f);

// Ward stones (rev 15, 3.6.6): to the player's right from the lip, at the cliff edge facing the fire
const float stoneTop = 82f, lipH = 0.8f;
var stones = new UnityEngine.GameObject("Stones").transform; stones.SetParent(ward, false);
var stonePos = new[] { P2(16f, 266f), P2(21f, 263f), P2(18f, 270f) };
for (int n = 0; n < stonePos.Length; n++)
{
    float g = H(stonePos[n].x, stonePos[n].y);
    Prim(Cube, "Stone_" + (n + 1), stones, V(stonePos[n].x, g + (stoneTop - g) * 0.5f - 0.5f, stonePos[n].y), V(3.6f, stoneTop - g + 1f, 4f));
}
// rock lip (3.6.5): 0.8 m high across the path end at (13, 256), 3 m in from the cliff edge at x 10
float gl = H(13f, 256f);
Prim(Cube, "RockLip", ward, V(13f, gl + lipH * 0.5f - 0.3f, 256f), V(1.2f, lipH + 0.6f, 7f));

// ---- stand-in burning ridge and valley fire (3.7), beyond the west map edge. Gray box, so the stand-in is brought near
// (ridge about 200 to 300 m out instead of 300 to 500) to make the lip frame read inside the camera's 1000 m far clip:
// fire across 150 degrees, flames 10 to 15 degrees over the ridge, the valley below burning as a sea, smoke over the sky.
// Flame, glow and smoke use DieAlone/FireStandIn (unlit, no fog) so they read through the night and day-two haze;
// colours from LookTuning (fireGlowColor and fireGlowIntensity for fire, smokeFireColor for the fire-lit smoke).
// Built inactive: hidden on day 1 by day; a day and night system turns it on. No renderer casts shadows.
var fire = new UnityEngine.GameObject("StandInFire"); fire.transform.SetParent(ward, false);
var fireSh = UnityEngine.Shader.Find("DieAlone/FireStandIn"); if (fireSh == null) return "DieAlone/FireStandIn shader not found";
var lookT = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (lookT == null) return "no LookTuning";
UnityEngine.Material FireMat(string path, UnityEngine.Color c, float intensity)
{
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(fireSh); UnityEditor.AssetDatabase.CreateAsset(m, path); }
    m.shader = fireSh; m.SetColor("_Color", c); m.SetFloat("_Intensity", intensity); UnityEditor.EditorUtility.SetDirty(m); return m;
}
var smokeMat = FireMat("Assets/Materials/Blockout/Blockout_Smoke.mat", lookT.smokeFireColor, 1f);
UnityEngine.GameObject Stand(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sc, UnityEngine.Material mat, float lean = 0f)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.position = pos; g.transform.localScale = sc;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, lean);
    UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());   // far scenery, never touched
    var rr = g.GetComponent<UnityEngine.Renderer>(); rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; rr.receiveShadows = false;
    if (mat != null) rr.sharedMaterial = mat;
    return g;
}
var rng = new System.Random(8013);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
const float zMin = -700f, zMax = 1200f;   // 75 degrees either side of west from the lip at 250 m out, and a margin
const float smokeAlpha = 0.85f;   // smoke card opacity
// the valley floor glows as embers in the fire-lit smoke colour, so the brighter fires on it read as one burning sea
Stand(UnityEngine.PrimitiveType.Cube, "ValleyFloor", fire.transform, V(-160f, -40.5f, 250f), V(330f, 1f, zMax - zMin), smokeMat);
var ridge = new UnityEngine.GameObject("Ridge").transform; ridge.SetParent(fire.transform, false);
var hills = new System.Collections.Generic.List<(UnityEngine.Vector3 c, UnityEngine.Vector3 r)>();   // centre and semi-axes, for setting fires on the slopes
for (float z = zMin; z <= zMax; z += 60f)
{
    var hc = V(R(-275f, -245f), -40f, z + R(-12f, 12f)); var hs = V(R(150f, 190f), R(120f, 150f), R(110f, 150f));
    Stand(UnityEngine.PrimitiveType.Sphere, "Hill", ridge, hc, hs, null); hills.Add((hc, hs * 0.5f));
}
float Ground(float x, float z)   // the valley floor, or the highest hill surface over (x, z)
{
    float y = -40f;
    foreach (var h in hills) { float dx = (x - h.c.x) / h.r.x, dz = (z - h.c.z) / h.r.z, q = 1f - dx * dx - dz * dz; if (q > 0f) y = UnityEngine.Mathf.Max(y, h.c.y + h.r.y * UnityEngine.Mathf.Sqrt(q)); }
    return y;
}
// ---- flames, glow and smoke as textured cards (8.9f, Vesper: no flat ovals or hard rectangles, Style.md Forbidden).
// Each card is a vertical quad turned toward the lip, cut from an 8 x 8 flipbook of the Fire & Smoke pack (flames on
// DieAlone/FlameCard: additive, no fog; smoke on DieAlone/Smoke with its fog amount 0), so tops are soft and broken.
// No glow strip: its straight card edges read as hard rectangles; the overlapping flame cards carry the glow. Cards are combined per group into mesh assets.
const string fireDir = "Assets/Terrain/Main3/Fire";
if (!UnityEditor.AssetDatabase.IsValidFolder(fireDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain/Main3", "Fire");
const string nmDir = "Assets/NatureManufacture Assets/Fire and Smoke Particles/";
var flameTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(nmDir + "Textures/T_fire_flipbook_big_01.png");
var nmSmoke = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(nmDir + "Materials/Others/M_Fire_Smoke_01.mat");
var cardSh = UnityEngine.Shader.Find("DieAlone/FlameCard");
if (flameTex == null || nmSmoke == null || cardSh == null) return "fire card assets missing";
UnityEngine.Material ProjectMat(string path, UnityEngine.Shader sh, UnityEngine.Material from)
{
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = from != null ? new UnityEngine.Material(from) : new UnityEngine.Material(sh); UnityEditor.AssetDatabase.CreateAsset(m, path); }
    else if (from != null) m.CopyPropertiesFromMaterial(from);
    m.shader = sh; UnityEditor.EditorUtility.SetDirty(m); return m;
}
var flameMat = ProjectMat("Assets/Materials/Blockout/Blockout_FlameCard.mat", cardSh, null);
flameMat.SetTexture("_BaseMap", flameTex); flameMat.SetColor("_Color", UnityEngine.Color.white); flameMat.SetFloat("_Intensity", lookT.fireGlowIntensity);
var smokeCardMat = ProjectMat("Assets/Materials/Blockout/Blockout_SmokeCard.mat", nmSmoke.shader, nmSmoke); smokeCardMat.SetFloat("_FogAmount", 0f);
const int sheetTiles = 8;   // the pack flipbooks are 8 x 8
var faceTo = new UnityEngine.Vector2(13f, 256f);   // cards turn toward the rock lip
UnityEngine.GameObject Cards(string name, UnityEngine.Transform parent, System.Collections.Generic.List<(UnityEngine.Vector3 b, float w, float h, int frame, float lean, float alpha)> cs, UnityEngine.Material mat, int tiles)
{
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>();
    var cols = new System.Collections.Generic.List<UnityEngine.Color>(); var tris = new System.Collections.Generic.List<int>();
    foreach (var c in cs)
    {
        var n = new UnityEngine.Vector2(faceTo.x - c.b.x, faceTo.y - c.b.z).normalized; var right = V(n.y, 0f, -n.x) * (c.w * 0.5f);
        var up = V(c.lean, c.h, 0f); int i0 = vs.Count;
        vs.Add(c.b - right); vs.Add(c.b + right); vs.Add(c.b - right + up); vs.Add(c.b + right + up);
        float s = 1f / tiles, u0 = (c.frame % tiles) * s, v0 = (tiles - 1 - c.frame / tiles) * s;
        uvs.Add(new UnityEngine.Vector2(u0, v0)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0)); uvs.Add(new UnityEngine.Vector2(u0, v0 + s)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0 + s));
        for (int k = 0; k < 4; k++) cols.Add(new UnityEngine.Color(1f, 1f, 1f, c.alpha));
        tris.AddRange(new[] { i0, i0 + 2, i0 + 1, i0 + 1, i0 + 2, i0 + 3 });
    }
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetUVs(0, uvs); mesh.SetColors(cols); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/" + name + ".asset");
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = mat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false; return g;
}
int FlameFrame() => 16 + rng.Next(32);   // the middle rows: full flames, not the first flicker or the dying tail
// the burning giants: the pack's dead tree (bark, taper, broken limbs) stretched to each giant's height, fogged like the
// scene so the haze sits between the trunks; the flame cards rise well past their tops
const string deadTreePath = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab";
var deadTree = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(deadTreePath); if (deadTree == null) return "dead tree prefab missing";
const float deadTreeGirth = 0.6f, giantGirth = 6f;   // the pack tree's trunk is about 0.6 m across; a giant's about 6 m
float deadTreeTall = 0f; foreach (var rr in deadTree.GetComponentsInChildren<UnityEngine.Renderer>()) deadTreeTall = UnityEngine.Mathf.Max(deadTreeTall, rr.bounds.max.y);
var giants = new UnityEngine.GameObject("BurningGiants").transform; giants.SetParent(fire.transform, false);
var flameCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
for (float z = zMin + 10f; z <= zMax - 10f; z += 30f)
{
    float x = R(-265f, -205f), baseY = 25f, tall = R(40f, 50f), flame = R(120f, 150f);   // cards to 137 to 167; the flame in each frame fills about its lower two thirds, so visible tops about 100 to 125, over the trunks (65 to 75)
    var t = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadTree, giants); t.name = "Trunk";
    t.transform.position = V(x, baseY, z); t.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    float sxz = giantGirth / deadTreeGirth, sy = tall / UnityEngine.Mathf.Max(0.5f, deadTreeTall); t.transform.localScale = V(sxz, sy, sxz);
    foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    for (int k = 0; k < 3; k++) flameCards.Add((V(x + R(-6f, 6f), baseY - 8f, z + R(-10f, 10f)), R(40f, 56f), flame * R(0.8f, 1f), FlameFrame(), R(-4f, 4f), 1f));
}
Cards("RidgeFlames", giants, flameCards, flameMat, sheetTiles);
// valley and ridge face: overlapping low fires from about 60 m out, across the floor and up the ridge's face to under its
// crest (30 degrees below level from the lip up the lower face), so the front reads as one burning sea
const float faceTop = -10f;   // face fires stay low enough that the crest on the run still hides them (Ward 3.6: only flame tops before the crest)
var valley = new UnityEngine.GameObject("ValleyFires").transform; valley.SetParent(fire.transform, false);
var valleyCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
for (float z = zMin; z <= zMax; z += 26f)
    for (float x = -60f; x >= -260f; x -= 20f)
    {
        float fx = x + R(-8f, 8f), fz = z + R(-8f, 8f), gy = Ground(fx, fz), hgt = R(12f, 30f);
        if (gy > faceTop) continue;
        valleyCards.Add((V(fx, gy - 2f, fz), R(40f, 56f), hgt, FlameFrame(), 0f, 1f));
    }
Cards("ValleyFlames", valley, valleyCards, flameMat, sheetTiles);
// smoke lit from below by the fire: five columns 400 to 500 m, each a stack of puffs widening and leaning east as they rise,
// past the frame top at level gaze; and a bank of low puffs behind the ridge filling the sky up to about 18 degrees
var smoke = new UnityEngine.GameObject("Smoke").transform; smoke.SetParent(fire.transform, false);
var smokeCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
foreach (var z in new[] { -350f, 0f, 250f, 520f, 850f })
{
    float hgt = R(400f, 500f); const int puffs = 5; float step = hgt / puffs;
    for (int k = 0; k < puffs; k++)
    {
        float t = k / (float)(puffs - 1), w = UnityEngine.Mathf.Lerp(120f, 240f, t);
        smokeCards.Add((V(-230f + hgt * 0.15f * t, 30f + k * step * 0.85f, z + R(-20f, 20f)), w, step * 1.6f, 20 + rng.Next(30), R(20f, 50f), smokeAlpha));
    }
}
for (float z = zMin; z < zMax; z += 110f) smokeCards.Add((V(-330f + R(-20f, 20f), 20f, z + R(-20f, 20f)), R(200f, 260f), R(110f, 150f), 20 + rng.Next(30), 25f, smokeAlpha));
Cards("SmokeColumns", smoke, smokeCards, smokeCardMat, sheetTiles);
fire.SetActive(false);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cairn at " + cairnPos.ToString("F1") + " solid chain 4 m up the trail | stones ground " + H(18f, 266f).ToString("F1") + " tops " + stoneTop + " | lip ground " + gl.ToString("F1") + " | stand-in fire built, off";
