// Main3 task 8.7: Ward pieces, gray. Run after 8.6 in Main3, edit mode.
// 8.14 (Valley.md rev 10): the cairn 2 m up the approach from J; the chain and IW2 across the 3 m gap in the W foot rock band at the
// chute mouth (the climb is closed by day, DECISIONS 2026-09-25 and 2026-09-30; a later night rule opens it, the dev warps reach the
// ledge meanwhile). The climb itself is 8.3's J to Ward trail on 8.1's carved ground and rock; here are its gray pieces: the cut steps
// of the chute, a landmark per leg, a hiding place per platform. Stones on the ledge at (-4, 226), (1, 223), (-7, 221), 3.6 x 4 m,
// tops 70, left of the path end (-8.5, 246), behind the knob from the tower. The rev 7 gate, stones and fire are in git history.
// The stand-in fire west of the W ridge (Valley.md 5), always on (DECISIONS 2026-09-29: the land hides it): a gray floor at -40
// from the terrain's west edge out past the far ridge; the far ridge rising from the floor at x -200 to its crest (30) at x -240;
// the far front on it, x -240 to -400, z -60 to 650 (rev 10 trims the south end; Wren 2026-09-30 keeps the north end at 650), burning
// giants and flame tops at 105; valley fires on the floor at x -110 to -220, z 0 to 500, flame tops 20; the day-one smoke sheet
// streaming west is main3_8_18a_smoke.cs. No colliders.
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
var Cube = UnityEngine.PrimitiveType.Cube; var Sph = UnityEngine.PrimitiveType.Sphere; var Cyl = UnityEngine.PrimitiveType.Cylinder;
var ward = new UnityEngine.GameObject("Ward").transform;

// the cairn at J and the day gate at the chute mouth (Valley.md rev 10, 1.6 and 8): a pale cairn 2 m up the approach from J; the
// chain hangs across the 3 m gap in the W foot rock band between the two rock arms (x 86, z 211.5 to 214.5), and IW2 fills the same
// gap exactly: invisible, on the Ignore Raycast layer, taller than the arms, so the climb is closed by day (a later night rule turns
// CairnGate off). The rev 7 gate across the old trail start is in git history.
var legT = Root("Trails").transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
var jPt = legT.GetChild(0).position; var jNext = legT.GetChild(1).position;
var along = V(jNext.x - jPt.x, 0f, jNext.z - jPt.z).normalized; var side = V(along.z, 0f, -along.x);
var cairnGate = new UnityEngine.GameObject("CairnGate").transform; cairnGate.SetParent(ward, false);
const float cairnAlong = 2f, cairnSide = 2.2f;
var cairnPos = jPt + along * cairnAlong + side * cairnSide; cairnPos.y = H(cairnPos.x, cairnPos.z);
float cy = cairnPos.y;
// pale matte stone (8.12 fix): the default Lit primitive material is smooth and mirrors the blue default reflection, so the cairn read see-through
var cairnStone = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat"); if (cairnStone == null) return "no Concrete034_1.0x1.0.mat";
foreach (var s in new[] { (1.4f, 0.5f), (1.1f, 0.45f), (0.85f, 0.4f), (0.6f, 0.35f), (0.4f, 0.3f) })
{ Prim(Sph, "Stone", cairnGate, V(cairnPos.x, cy + s.Item2 * 0.45f, cairnPos.z), V(s.Item1, s.Item2, s.Item1)).GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = cairnStone; cy += s.Item2 * 0.8f; }
var lookCairn = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (lookCairn == null) return "no LookTuning";
// the cairn lamp (Valley.md 1.6: "the cairn lamp at J is lit" at night; 8.14a) and, 8.15a (Gate.md 4 night rule N1), a lantern at the chute
// foot and on each platform: owned lanterns with a practical light, on at night and off by day (PracticalLight, LookTuning's lantern
// brightness), and a small unfogged glow at the flame (DieAlone/FireStandIn, shown at night only) so each reads from the one before
// through the 8 to 60 m night fog
const float lanternRange = 8f, glowSize = 0.6f, glowUp = 0.3f;   // a 0.6 m glow reads as a point from 60 m through the filter (0.25 was sub-pixel)
var nightGlows = new System.Collections.Generic.List<UnityEngine.GameObject>();
var lanternPf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Revolving Pizza Games/Campsite/Prefabs/CS_Lantern_Old.prefab"); if (lanternPf == null) return "missing CS_Lantern_Old";
var glowMatL = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_LanternGlow.mat");
if (glowMatL == null) { glowMatL = new UnityEngine.Material(UnityEngine.Shader.Find("DieAlone/FireStandIn")); UnityEditor.AssetDatabase.CreateAsset(glowMatL, "Assets/Materials/Blockout/Blockout_LanternGlow.mat"); }
glowMatL.shader = UnityEngine.Shader.Find("DieAlone/FireStandIn"); glowMatL.SetColor("_Color", lookCairn.practicalColor); glowMatL.SetFloat("_Intensity", lookCairn.farMarkerIntensity); UnityEditor.EditorUtility.SetDirty(glowMatL);
// RebuildSpecs 4.10 (Vesper: the J cairn lamp read as a flat yellow disc): the cairn lamp's glow is a small flame in its glass, two
// crossed flame cards (lampFlameW by lampFlameH m, flipbook frame lampFlameFrame) on DieAlone/FlameCard with the flame colours; the
// other lanterns keep the glow ball the night rule (N1) reads from the lantern before
const float lampFlameW = 0.14f, lampFlameH = 0.24f; const int lampFlameFrame = 30, lampFlameTiles = 8;
var lampFlameTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/NatureManufacture Assets/Fire and Smoke Particles/Textures/T_fire_flipbook_big_01.png"); if (lampFlameTex == null) return "flame flipbook missing";
var lampFlameMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_LanternFlame.mat");
if (lampFlameMat == null) { lampFlameMat = new UnityEngine.Material(UnityEngine.Shader.Find("DieAlone/FlameCard")); UnityEditor.AssetDatabase.CreateAsset(lampFlameMat, "Assets/Materials/Blockout/Blockout_LanternFlame.mat"); }
lampFlameMat.shader = UnityEngine.Shader.Find("DieAlone/FlameCard"); lampFlameMat.SetTexture("_BaseMap", lampFlameTex); lampFlameMat.SetFloat("_Intensity", lookCairn.farMarkerIntensity); lampFlameMat.SetFloat("_Ramp", 1f);
lampFlameMat.SetColor("_RampBase", lookCairn.flameBaseColor); lampFlameMat.SetColor("_RampBody", lookCairn.flameBodyColor); lampFlameMat.SetColor("_RampTip", lookCairn.flameTipColor); UnityEditor.EditorUtility.SetDirty(lampFlameMat);
UnityEngine.Mesh lampFlameMesh;
{
    float s = 1f / lampFlameTiles, u0 = (lampFlameFrame % lampFlameTiles) * s, v0 = (lampFlameTiles - 1 - lampFlameFrame / lampFlameTiles) * s;
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>(); var tris = new System.Collections.Generic.List<int>();
    foreach (var a in new[] { V(1f, 0f, 0f), V(0f, 0f, 1f) })
    {
        int i0 = vs.Count; var h = a * (lampFlameW * 0.5f);
        vs.Add(-h); vs.Add(h); vs.Add(-h + V(0f, lampFlameH, 0f)); vs.Add(h + V(0f, lampFlameH, 0f));
        uvs.Add(new UnityEngine.Vector2(u0, v0)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0)); uvs.Add(new UnityEngine.Vector2(u0, v0 + s)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0 + s));
        tris.AddRange(new[] { i0, i0 + 2, i0 + 1, i0 + 1, i0 + 2, i0 + 3 });
    }
    lampFlameMesh = new UnityEngine.Mesh { name = "LanternFlame" }; lampFlameMesh.SetVertices(vs); lampFlameMesh.SetUVs(0, uvs); lampFlameMesh.SetTriangles(tris, 0); lampFlameMesh.RecalculateBounds();
    lampFlameMesh.SetColors(new System.Collections.Generic.List<UnityEngine.Color>(System.Linq.Enumerable.Repeat(UnityEngine.Color.white, vs.Count)));
    const string lampFlamePath = "Assets/Terrain/Main3/LanternFlame.asset"; UnityEditor.AssetDatabase.DeleteAsset(lampFlamePath); UnityEditor.AssetDatabase.CreateAsset(lampFlameMesh, lampFlamePath);
}
void NightLantern(string name, UnityEngine.Transform parent, UnityEngine.Vector3 at, bool flame = false)
{
    var lantern = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(lanternPf, parent); lantern.name = name;
    foreach (var c in lantern.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    lantern.transform.position = at;
    var lg = new UnityEngine.GameObject(name + "Light"); lg.transform.SetParent(lantern.transform, false); lg.transform.localPosition = V(0f, glowUp, 0f);
    var l = lg.AddComponent<UnityEngine.Light>(); l.type = UnityEngine.LightType.Point; l.range = lanternRange; l.intensity = lookCairn.firePitIntensity; l.shadows = UnityEngine.LightShadows.None; l.color = lookCairn.practicalColor;
    var pl = lg.AddComponent<PracticalLight>(); var so = new UnityEditor.SerializedObject(pl);
    so.FindProperty("tuning").objectReferenceValue = lookCairn; so.FindProperty("kind").enumValueIndex = (int)PracticalLight.Kind.Lantern; so.FindProperty("byDay").enumValueIndex = (int)PracticalLight.ByDay.Off; so.ApplyModifiedPropertiesWithoutUndo();
    if (flame)
    {
        var fl = new UnityEngine.GameObject(name + "Flame"); fl.transform.SetParent(lantern.transform, false); fl.transform.localPosition = V(0f, glowUp, 0f) - V(0f, lampFlameH * 0.5f, 0f) / UnityEngine.Mathf.Max(0.01f, lantern.transform.lossyScale.x);
        fl.transform.localScale = UnityEngine.Vector3.one / UnityEngine.Mathf.Max(0.01f, lantern.transform.lossyScale.x);
        fl.AddComponent<UnityEngine.MeshFilter>().sharedMesh = lampFlameMesh; var fr = fl.AddComponent<UnityEngine.MeshRenderer>(); fr.sharedMaterial = lampFlameMat; fr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; nightGlows.Add(fl);
        return;
    }
    var glow = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere); glow.name = name + "Glow"; UnityEngine.Object.DestroyImmediate(glow.GetComponent<UnityEngine.Collider>());
    glow.transform.SetParent(lantern.transform, false); glow.transform.localPosition = V(0f, glowUp, 0f); glow.transform.localScale = V(glowSize, glowSize, glowSize) / UnityEngine.Mathf.Max(0.01f, lantern.transform.lossyScale.x);
    var gr = glow.GetComponent<UnityEngine.MeshRenderer>(); gr.sharedMaterial = glowMatL; gr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; nightGlows.Add(glow);
}
NightLantern("CairnLamp", cairnGate.parent, V(cairnPos.x, cy, cairnPos.z), true);
const float gapX = 86f, gapZ0 = 211.5f, gapZ1 = 214.5f, chainH = 0.9f, iw2Over = 3f, iw2Thick = 0.5f;
float gapG = H(gapX + 0.5f, (gapZ0 + gapZ1) * 0.5f);
var chainA = V(gapX, gapG + chainH, gapZ0); var chainB = V(gapX, gapG + chainH, gapZ1);
var chain = Prim(Cube, "Chain", cairnGate, (chainA + chainB) * 0.5f, V(0.06f, 0.06f, UnityEngine.Vector3.Distance(chainA, chainB)), 0f, false);
chain.transform.rotation = UnityEngine.Quaternion.LookRotation(chainB - chainA, UnityEngine.Vector3.up);
// IW2: from under the ground to iw2Over above the highest rock-arm top beside the gap
float armTop = gapG; foreach (var r in new[] { gapZ0 - 1f, gapZ1 + 1f }) if (UnityEngine.Physics.Raycast(V(gapX + 0.5f, 200f, r), UnityEngine.Vector3.down, out var ah, 300f)) armTop = UnityEngine.Mathf.Max(armTop, ah.point.y);
var gateBlock = new UnityEngine.GameObject("GateBlocker"); gateBlock.transform.SetParent(cairnGate, false); gateBlock.layer = 2;
float iwBot = gapG - 1f, iwTop = armTop + iw2Over;
gateBlock.transform.position = V(gapX, (iwBot + iwTop) * 0.5f, (gapZ0 + gapZ1) * 0.5f);
gateBlock.AddComponent<UnityEngine.BoxCollider>().size = V(iw2Thick, iwTop - iwBot, gapZ1 - gapZ0 + 0.4f);

// the climb's pieces (Valley.md rev 10 section 4), gray stand-ins with no colliders (they must not catch the player): leg 1's three
// flights of 18 cut steps on the chute floor (the ground under them is their nosing line, 8.1), and one landmark per leg and a hiding
// place per platform at their chainage along the trail from J: the seep and tin cup (54), the rune post (88, on the outer edge),
// the split snag (144, the trail passes through it), the bent fir (191, knee height beside the trail); P1's boulder overhang, P2's
// rock roof, P3's root plate
var climbPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in legT) climbPts.Add(m.position);
var climbS = new float[climbPts.Count]; for (int i = 1; i < climbPts.Count; i++) climbS[i] = climbS[i - 1] + UnityEngine.Vector2.Distance(new UnityEngine.Vector2(climbPts[i - 1].x, climbPts[i - 1].z), new UnityEngine.Vector2(climbPts[i].x, climbPts[i].z));
(UnityEngine.Vector3 p, UnityEngine.Vector3 dir) AtS(float s)
{
    int i = 1; while (i < climbPts.Count - 1 && climbS[i] < s) i++;
    float t = UnityEngine.Mathf.InverseLerp(climbS[i - 1], climbS[i], s); var d = climbPts[i] - climbPts[i - 1]; d.y = 0f;
    return (UnityEngine.Vector3.Lerp(climbPts[i - 1], climbPts[i], t), d.normalized);
}
float L2(float a, float b, float t) => a + (b - a) * t;
var climbRoot = new UnityEngine.GameObject("Climb").transform; climbRoot.SetParent(ward, false);
var steps = new UnityEngine.GameObject("ChuteSteps").transform; steps.SetParent(climbRoot, false);
{
    // 8.1's leg 1: from the chute mouth, ramp and flight in turn; the chute runs straight, so the flights start at fixed distances
    var mouth = new UnityEngine.Vector2(86f, 213f); var p1In = new UnityEngine.Vector2(52f, 216f) + (mouth - new UnityEngine.Vector2(52f, 216f)).normalized * 2f;
    const int stepsPerFlight = 18; const float stepRise = 0.25f, stepRun = 0.3f, stepWidth = 3f, stairRampThick = 0.2f;
    float leg1 = UnityEngine.Vector2.Distance(mouth, p1In), flightRun = stepsPerFlight * stepRun, rampRun = (leg1 - 3f * flightRun) / 4f;
    var dir2 = (p1In - mouth).normalized; float yaw = UnityEngine.Mathf.Atan2(dir2.x, dir2.y) * UnityEngine.Mathf.Rad2Deg;
    for (int f = 0; f < 3; f++)
    {
        float s0 = rampRun * (f + 1) + flightRun * f;
        for (int k = 0; k < stepsPerFlight; k++)
        {
            var q = mouth + dir2 * (s0 + (k + 0.5f) * stepRun); float top = H(mouth.x + dir2.x * (s0 + (k + 1) * stepRun), mouth.y + dir2.y * (s0 + (k + 1) * stepRun));
            Prim(Cube, "Step", steps, V(q.x, top - stepRise * 0.5f, q.y), V(stepWidth, stepRise, stepRun), yaw, false);
        }
        // STAIRS RULE: a collider-only StairRamp whose top face runs along the flight's nosing line, from one run below the first
        // nosing to the top nosing (8.14: on the bare terrain the flight's triangles reach past the 45 degree slope limit and stop the player)
        var a2 = mouth + dir2 * (s0 - stepRun); var b2 = mouth + dir2 * (s0 + flightRun);
        var a3 = V(a2.x, H(a2.x, a2.y), a2.y); var b3 = V(b2.x, H(b2.x, b2.y), b2.y); var along3 = b3 - a3;
        var nUp = UnityEngine.Vector3.Cross(along3, V(dir2.y, 0f, -dir2.x)).normalized; if (nUp.y < 0f) nUp = -nUp;
        var ramp = new UnityEngine.GameObject("StairRamp"); ramp.transform.SetParent(steps, false);
        ramp.transform.SetPositionAndRotation((a3 + b3) * 0.5f - nUp * stairRampThick * 0.5f, UnityEngine.Quaternion.LookRotation(along3.normalized, nUp));
        ramp.AddComponent<UnityEngine.BoxCollider>().size = V(stepWidth, stairRampThick, along3.magnitude);
    }
}
void Mark(string name, float s, float sideOff, UnityEngine.PrimitiveType t, UnityEngine.Vector3 size, float up, float tilt = 0f)
{
    var a = AtS(s); var sd = V(a.dir.z, 0f, -a.dir.x); var p = a.p + sd * sideOff; p.y = H(p.x, p.z) + up;
    var g = Prim(t, name, climbRoot, p, size, UnityEngine.Mathf.Atan2(a.dir.x, a.dir.z) * UnityEngine.Mathf.Rad2Deg, false);
    if (tilt != 0f) g.transform.rotation = g.transform.rotation * UnityEngine.Quaternion.Euler(0f, 0f, tilt);
}
// chainage of the trail point nearest a map point; the landmarks sit by 8.1's climb points (the doc's chainages are from the paper map)
float SAt(float x, float z) { int bi = 0; float bd = float.MaxValue; for (int i = 0; i < climbPts.Count; i++) { float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(climbPts[i].x, climbPts[i].z), new UnityEngine.Vector2(x, z)); if (d < bd) { bd = d; bi = i; } } return climbS[bi]; }
const float landmarkShare = 0.6f;
float sP1 = SAt(52f, 216f), sP2 = SAt(57f, 276f), sP3 = SAt(26f, 304f), sP4 = SAt(26f, 262f), sSeep = sP1 - 3.5f, sRune = (sP1 + sP2) * 0.5f;
float sSnag = L2(sP2, sP3, landmarkShare), sFir = L2(sP3, sP4, landmarkShare);
// 8.16b (Marlow 6, Vesper: a grey plank floating across the trail on leg 4, grey stand-ins on the climb): the seep, the rune post and the
// bent fir are owned meshes now (the gray primitives are in git history): a BK boulder with a bowl at its foot for the seep, a small carved
// menhir for the rune post, a young red fir leaning hard over the trail's valley side for the bent fir (named RedFir_Bent, so the trunk check
// takes it as one tree), a trunk capsule on it
Owned("Seep_Rock", "Assets/BK/PureNature_Redwood/Prefabs/Rocks/Boulder_3.prefab", sSeep, -2.4f, 0.3f, V(0.45f, 0.45f, 0.45f), V(0f, 30f, 0f));
Owned("Seep_Cup", "Assets/Revolving Pizza Games/Cabin In The Woods/Prefabs/Props/CITW_Bowl_Small.prefab", sSeep, -1.5f, 0f, UnityEngine.Vector3.one, UnityEngine.Vector3.zero);
Owned("RunePost", "Assets/Effigy GameWorks/Menhir Stone Circle/Prefabs/Single/Rock/Rock - Carved Runes/StoneMenhir_1_Rock_CarvedRunes.prefab", sRune, 2.6f, 0.2f, V(0.27f, 0.27f, 0.27f), V(0f, 15f, 4f));
PlaceKit.DeadTrunkCapsule(Owned("RedFir_Bent", "Assets/BK/PureNature_Redwood/Prefabs/Trees/RedFir4.prefab", sFir, 2.2f, 0.1f, UnityEngine.Vector3.one, V(0f, 0f, -35f)), 0.1f, 0.5f);
// 8.14a (Vesper: pale untextured pillars, a flat untextured overhang slab, a white disc): the split snag, the platform hides and the root
// plate from owned art, colliders removed (the climb's pieces never catch the player); the chute steps take 8.1's rock
var bandRock = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_BandRock.mat"); if (bandRock == null) return "no Blockout_BandRock (8.1)";
// 8.15 gate (Marlow: the chute tread read the same as its floor): the steps take a worn, paler copy of the rock, stepLift times as light
const float stepLift = 1.5f; const string treadPath = "Assets/Materials/Blockout/Blockout_TreadRock.mat";
var treadRock = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(treadPath); if (treadRock == null) { treadRock = new UnityEngine.Material(bandRock); UnityEditor.AssetDatabase.CreateAsset(treadRock, treadPath); } else treadRock.CopyPropertiesFromMaterial(bandRock);
{ var bc = bandRock.GetColor("_BaseColor"); treadRock.SetColor("_BaseColor", new UnityEngine.Color(bc.r * stepLift, bc.g * stepLift, bc.b * stepLift, 1f)); } UnityEditor.EditorUtility.SetDirty(treadRock);
foreach (var r in steps.GetComponentsInChildren<UnityEngine.MeshRenderer>()) r.sharedMaterial = treadRock;
const string climbDeadTree = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab", bkRocks = "Assets/BK/PureNature_Redwood/Prefabs/Rocks/";
const float deadTreeUnitGirth = 0.6f;
UnityEngine.GameObject Owned(string name, string path, float s, float side, float sink, UnityEngine.Vector3 scale, UnityEngine.Vector3 euler)
{
    var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path); if (pf == null) return null;
    var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf, climbRoot); g.name = name;
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    var a = AtS(s); var sd = V(a.dir.z, 0f, -a.dir.x); var p = a.p + sd * side; p.y = H(p.x, p.z) - sink;
    g.transform.localScale = scale; g.transform.SetPositionAndRotation(p, UnityEngine.Quaternion.Euler(0f, UnityEngine.Mathf.Atan2(a.dir.x, a.dir.z) * UnityEngine.Mathf.Rad2Deg, 0f) * UnityEngine.Quaternion.Euler(euler));
    return g;
}
float deadUnitTall = 0f; { var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(climbDeadTree); if (pf == null) return "missing Tree_Dead"; foreach (var r in pf.GetComponentsInChildren<UnityEngine.Renderer>()) deadUnitTall = UnityEngine.Mathf.Max(deadUnitTall, r.bounds.max.y); }
UnityEngine.Vector3 DeadScale(float tall, float girth) => V(girth / deadTreeUnitGirth, tall / UnityEngine.Mathf.Max(0.5f, deadUnitTall), girth / deadTreeUnitGirth);
// 8.16b (Marlow: the player walked into both halves): a capsule on each trunk (PlaceKit.DeadTrunkCapsule, as the burn snags)
const float splitTrunkBand = 0.15f, splitTrunkShare = 0.5f;
PlaceKit.DeadTrunkCapsule(Owned("SplitSnag_L", climbDeadTree, sSnag, -1.2f, 0.3f, DeadScale(7f, 1.2f), V(0f, 20f, -4f)), splitTrunkBand, splitTrunkShare);   // the two halves of one split trunk, leaning apart
PlaceKit.DeadTrunkCapsule(Owned("SplitSnag_R", climbDeadTree, sSnag, 1.2f, 0.3f, DeadScale(6f, 1.1f), V(0f, 200f, 5f)), splitTrunkBand, splitTrunkShare);
Owned("P1_Overhang", bkRocks + "Boulder_2.prefab", sP1, -2.6f, 0.6f, V(0.55f, 0.55f, 0.55f), V(8f, 30f, 0f));
Owned("P2_RockRoof", bkRocks + "BigBoulders_3.prefab", sP2, -3f, 1f, V(0.5f, 0.45f, 0.5f), V(-10f, 70f, 6f));
Owned("P3_RootPlate", climbDeadTree, sP3, 2.6f, -0.4f, DeadScale(6f, 1.4f), V(0f, 60f, 84f));   // a fallen snag, its root end toward the trail
// 8.15a: the climb's lanterns, at the chute foot and on each platform, beside the tread on the wall side, each in sight of the next
const float chuteFootIn = 3f, lanternSide = -1.8f; float chuteFootS = SAt(86f, 213f) + chuteFootIn;   // 3 m inside the chute mouth
const float chuteMidBeforeP1 = 6f;   // 8.16a: with the chute walls down the P1 lantern drops out of view on the flights (Gate.md 4 N1 at 30 and 40 m); one more on the last landing
foreach (var ln in new[] { ("ChuteFootLantern", chuteFootS), ("ChuteMidLantern", sP1 - chuteMidBeforeP1), ("P1Lantern", sP1), ("P2Lantern", sP2), ("P3Lantern", sP3), ("P4Lantern", sP4) })
{
    var a = AtS(ln.Item2); var sd = V(a.dir.z, 0f, -a.dir.x); var p = a.p + sd * lanternSide; p.y = H(p.x, p.z);
    NightLantern(ln.Item1, climbRoot, p);
}
// 8.16a (Pim, Gate_816_Pim.md 3: Camp to J N1 0 and 30, no light ahead leaving camp): lanterns beside the tread of Camp to J, the first
// in view as the night walk leaves the camp centre, the second where the first has passed and the cairn lamp is still out of sight;
// the climb's lanterns lead on from J
var campLanternAlong = new[] { 9f, 38f };
{
    var campLeg = Root("Trails").transform.Find("Camp to J"); if (campLeg == null || campLeg.childCount < 2) return "no Camp to J trail (8.3)";
    for (int li = 0; li < campLanternAlong.Length; li++)
    {
        float lAlong = campLanternAlong[li];
        UnityEngine.Vector3 cPrev = campLeg.GetChild(0).position, cHere = cPrev, cNext = cPrev; float cAcc = 0f;
        for (int ci = 1; ci < campLeg.childCount; ci++) { cNext = campLeg.GetChild(ci).position; float cD = UnityEngine.Vector3.Distance(V(cPrev.x, 0f, cPrev.z), V(cNext.x, 0f, cNext.z)); if (cAcc + cD >= lAlong) { cHere = UnityEngine.Vector3.Lerp(cPrev, cNext, (lAlong - cAcc) / UnityEngine.Mathf.Max(cD, 1e-4f)); break; } cAcc += cD; cPrev = cNext; }
        var cDir = V(cNext.x - cPrev.x, 0f, cNext.z - cPrev.z).normalized; var cAt = cHere + V(cDir.z, 0f, -cDir.x) * lanternSide; cAt.y = H(cAt.x, cAt.z);
        NightLantern("CampToJLantern_" + (li + 1), cairnGate.parent, cAt);
    }
}
{   // the lantern glows show at night only (the practical lights switch themselves)
    var glowVis = new UnityEngine.GameObject("LanternGlows"); glowVis.transform.SetParent(ward, false);
    var vis = glowVis.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(vis); so.FindProperty("show").enumValueIndex = (int)LookVisibility.Show.Night;
    var tp = so.FindProperty("targets"); tp.arraySize = nightGlows.Count; for (int i = 0; i < nightGlows.Count; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = nightGlows[i]; so.ApplyModifiedPropertiesWithoutUndo();
}

// Ward stones (Valley.md rev 10, 4): on the ledge south of the path end and off to one side of the approach, 3.6 x 4 m, tops 70
const float stoneTop = 70f;
var stones = new UnityEngine.GameObject("Stones").transform; stones.SetParent(ward, false);
var stonePos = new[] { P2(-4f, 226f), P2(1f, 223f), P2(-7f, 221f) };
for (int n = 0; n < stonePos.Length; n++)
{
    float g = H(stonePos[n].x, stonePos[n].y);
    Prim(Cube, "Stone_" + (n + 1), stones, V(stonePos[n].x, g + (stoneTop - g) * 0.5f - 0.5f, stonePos[n].y), V(3.6f, stoneTop - g + 1f, 4f));
}

// ---- the stand-in fire west of the ridge (always on, no colliders, no shadows)
var fire = new UnityEngine.GameObject("StandInFire"); fire.transform.SetParent(ward, false);
var lookT = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (lookT == null) return "no LookTuning";
var backSh = UnityEngine.Shader.Find("DieAlone/Backdrop"); if (backSh == null) return "DieAlone/Backdrop shader not found";
var grayMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_WestGround.mat");
if (grayMat == null) { grayMat = new UnityEngine.Material(backSh); UnityEditor.AssetDatabase.CreateAsset(grayMat, "Assets/Materials/Blockout/Blockout_WestGround.mat"); }
grayMat.shader = backSh; grayMat.SetColor("_Color", lookT.backdropGroundColor); grayMat.SetFloat("_HazeBlend", lookT.backdropGroundHaze); UnityEditor.EditorUtility.SetDirty(grayMat);
const string fireDir = "Assets/Terrain/Main3/Fire";
if (!UnityEditor.AssetDatabase.IsValidFolder(fireDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain/Main3", "Fire");
var rng = new System.Random(8013);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
// the far side: floor at -40 (a 2 m roll, zero at the terrain's edge, so it is land and not a flat plane) and the far ridge
const float floorY = -40f, floorRoll = 2f, floorX0 = -1100f, floorX1 = -40f, floorZ0 = -1000f, floorZ1 = 1250f, floorStep = 50f;
float FloorY(float x, float z) => floorY + floorRoll * UnityEngine.Mathf.PerlinNoise(x / 170f + 2.3f, z / 170f + 8.1f) * UnityEngine.Mathf.Clamp01((floorX1 - x) / floorStep);
// the far ridge (Valley.md 5.1): rises from the floor at x -200 to its crest at x -240, holds it under the far front to x -400,
// and falls back to the floor by x -500
const float ridgeFoot = -200f, ridgeFront = -240f, ridgeBack = -400f, ridgeBackFoot = -500f, ridgeX0 = -520f, ridgeX1 = -190f, ridgeZ0 = -750f, ridgeZ1 = 1250f, ridgeStep = 10f, ridgeCrestLo = 25f, ridgeCrest = 30f;
float RidgeY(float x, float z)
{
    float crest = UnityEngine.Mathf.Lerp(ridgeCrestLo, ridgeCrest, UnityEngine.Mathf.PerlinNoise(z / 140f + 4.4f, 0.37f));
    float w = x > ridgeFront ? UnityEngine.Mathf.SmoothStep(0f, 1f, (ridgeFoot - x) / (ridgeFoot - ridgeFront)) : x > ridgeBack ? 1f : UnityEngine.Mathf.SmoothStep(0f, 1f, (x - ridgeBackFoot) / (ridgeBack - ridgeBackFoot));
    return UnityEngine.Mathf.Max(FloorY(x, z), floorY + (crest - floorY) * UnityEngine.Mathf.Clamp01(w));
}
UnityEngine.GameObject Grid(string name, float x0, float x1, float z0, float z1, float step, System.Func<float, float, float> y)
{
    int cols = UnityEngine.Mathf.CeilToInt((x1 - x0) / step) + 1, rows = UnityEngine.Mathf.CeilToInt((z1 - z0) / step) + 1;
    var vs = new UnityEngine.Vector3[cols * rows]; var tris = new System.Collections.Generic.List<int>();
    for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++) { float x = UnityEngine.Mathf.Min(x0 + c * step, x1), z = UnityEngine.Mathf.Min(z0 + r * step, z1); vs[r * cols + c] = V(x, y(x, z), z); }
    for (int r = 0; r < rows - 1; r++) for (int c = 0; c < cols - 1; c++) { int a = r * cols + c, b = a + 1, d = a + cols, e = d + 1; tris.AddRange(new[] { a, d, b, b, d, e }); }
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.vertices = vs; mesh.triangles = tris.ToArray(); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/" + name + ".asset");
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(fire.transform, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = grayMat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false; return g;
}
Grid("ValleyFloor", floorX0, floorX1, floorZ0, floorZ1, floorStep, FloorY);
Grid("Ridge", ridgeX0, ridgeX1, ridgeZ0, ridgeZ1, ridgeStep, RidgeY);
// 8.14a (Vesper, Marlow: by day the far ridge was a black slab): the fire's land takes a stronger blend toward the fog colour, so by day
// it reads as far, hazed ground and at night (a near-black fog) it stays dark
const float fireLandHaze = 0.6f;
var fireLand = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_FireLand.mat");
if (fireLand == null) { fireLand = new UnityEngine.Material(backSh); UnityEditor.AssetDatabase.CreateAsset(fireLand, "Assets/Materials/Blockout/Blockout_FireLand.mat"); }
fireLand.shader = backSh; fireLand.SetColor("_Color", lookT.backdropGroundColor); fireLand.SetFloat("_HazeBlend", fireLandHaze); UnityEditor.EditorUtility.SetDirty(fireLand);
foreach (var landName in new[] { "ValleyFloor", "Ridge" }) fire.transform.Find(landName).GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = fireLand;
// ---- flames as textured cards (8.9f, Vesper): vertical quads from an 8 x 8 flipbook of the Fire & Smoke pack on DieAlone/FlameCard
// (additive, no fog), combined per group into mesh assets. Measured in 8.9k (frames 16 to 47 of the flipbook, lit rows): the flame reaches
// up to 0.93 of its card (frame 47), so a card is only as tall as its flame top: it rises from its base b to the stated top T.
const string nmDir = "Assets/NatureManufacture Assets/Fire and Smoke Particles/";
var flameTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(nmDir + "Textures/T_fire_flipbook_big_01.png");
var cardSh = UnityEngine.Shader.Find("DieAlone/FlameCard");
if (flameTex == null || cardSh == null) return "fire card assets missing";
var flameMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_FlameCard.mat");
if (flameMat == null) { flameMat = new UnityEngine.Material(cardSh); UnityEditor.AssetDatabase.CreateAsset(flameMat, "Assets/Materials/Blockout/Blockout_FlameCard.mat"); }
flameMat.shader = cardSh; flameMat.SetTexture("_BaseMap", flameTex); flameMat.SetColor("_Color", UnityEngine.Color.white); flameMat.SetFloat("_Intensity", lookT.fireGlowIntensity); UnityEditor.EditorUtility.SetDirty(flameMat);
const int sheetTiles = 8;   // the pack flipbooks are 8 x 8
const float flameShare = 1f;   // the card top is the flame top (was 2/3, an unmeasured guess; 8.9k)
var faceTo = new UnityEngine.Vector2(-8.5f, 246f);   // cards turn toward the path end on the ledge
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
// ---- 8.18 (RebuildSpecs 4, Vesper 2026-10-01; Edges.md 6, Style.md 6.3): the flame cards colour by brightness (core flameBaseColor,
// body flameBodyColor, fading tips flameTipColor, LookTuning) and stand in clusters, not a picket row: clusterMin to clusterMax cards,
// each cluster clusterWLow to clusterWHigh m along the front with gaps clusterGapLow to clusterGapHigh m; a cluster's mean flame
// height is clusterMeanLow to 1 of the room under the stated top, each card that times 1 plus or minus cardVary, never over the top
// (F-1 is checked against the stated tops). Night shows every card; day two shows only whole clusters covering dayShare of the front's
// length or less (the RidgeFlamesDay and ValleyFlamesDay meshes) and LookTuning dims every card by fireCardDim in that look.
flameMat.SetFloat("_Ramp", 1f); flameMat.SetColor("_RampBase", lookT.flameBaseColor); flameMat.SetColor("_RampBody", lookT.flameBodyColor); flameMat.SetColor("_RampTip", lookT.flameTipColor); UnityEditor.EditorUtility.SetDirty(flameMat);
const int clusterMin = 3, clusterMax = 9; const float clusterWLow = 15f, clusterWHigh = 60f, clusterGapLow = 10f, clusterGapHigh = 50f, clusterMeanLow = 0.75f, cardVary = 0.4f, dayShare = 1f / 3f;
int FlameFrame() => 16 + rng.Next(32);   // the middle rows: full flames, not the first flicker or the dying tail
float CardH(float baseY, float top) => (top - baseY) / flameShare;
var dayTwoVis = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset"); if (dayTwoVis == null) return "no LookTuning_DayTwo";
void ShowIn(UnityEngine.GameObject host, LookVisibility.Show show, params UnityEngine.GameObject[] targets)
{
    var vis = host.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(vis);
    so.FindProperty("show").enumValueIndex = (int)show; so.FindProperty("dayTwo").objectReferenceValue = dayTwoVis;
    var tp = so.FindProperty("targets"); tp.arraySize = targets.Length; for (int i = 0; i < targets.Length; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = targets[i]; so.ApplyModifiedPropertiesWithoutUndo();
}
// clusters along one line (z from z0 to z1): (centre z, width, card count)
System.Collections.Generic.List<(float z, float w, int n)> Clusters(float z0, float z1)
{
    var list = new System.Collections.Generic.List<(float, float, int)>();
    for (float z = z0 + R(0f, clusterGapLow); z < z1; ) { float w = R(clusterWLow, clusterWHigh); if (z + w > z1) w = z1 - z; if (w > 2f) list.Add((z + w * 0.5f, w, clusterMin + rng.Next(clusterMax - clusterMin + 1))); z += w + R(clusterGapLow, clusterGapHigh); }
    return list;
}
// day two: whole clusters, taken in a random order, while their summed width stays dayShare of the front's length or less
System.Collections.Generic.HashSet<int> DayPick(System.Collections.Generic.List<(float z, float w, int n)> cl, float length)
{
    var order = new System.Collections.Generic.List<int>(); for (int i = 0; i < cl.Count; i++) order.Add(i);
    for (int i = order.Count - 1; i > 0; i--) { int j = rng.Next(i + 1); (order[i], order[j]) = (order[j], order[i]); }
    var pick = new System.Collections.Generic.HashSet<int>(); float sum = 0f; foreach (var i in order) if (sum + cl[i].w <= length * dayShare) { pick.Add(i); sum += cl[i].w; }
    return pick;
}
// the far front: burning giants on the far ridge (the pack's dead tree stretched to a giant), a near and a far row, flame tops at most 105
const string deadTreePath = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab";
var deadTree = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(deadTreePath); if (deadTree == null) return "dead tree prefab missing";
const float deadTreeGirth = 0.6f, giantGirth = 6f;   // the pack tree's trunk is about 0.6 m across; a giant's about 6 m
const float frontZ0 = -60f, frontZ1 = 650f, frontTop = 105f, flameSink = 8f, giantLow = 40f, giantHigh = 50f;
var frontRows = new[] { (-240f, -300f), (-330f, -400f) };
float deadTreeTall = 0f; foreach (var rr in deadTree.GetComponentsInChildren<UnityEngine.Renderer>()) deadTreeTall = UnityEngine.Mathf.Max(deadTreeTall, rr.bounds.max.y);
var giants = new UnityEngine.GameObject("BurningGiants").transform; giants.SetParent(fire.transform, false);
var flameCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>(); var flameDay = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
int frontClusters = 0;
foreach (var row in frontRows)
{
    var cl = Clusters(frontZ0, frontZ1); var day = DayPick(cl, frontZ1 - frontZ0); frontClusters += cl.Count;
    for (int ci = 0; ci < cl.Count; ci++)
    {
        var c = cl[ci]; float mean = R(clusterMeanLow, 1f);
        float tx = R(row.Item2, row.Item1), tBase = RidgeY(tx, c.z);   // one burning giant per cluster
        var t = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadTree, giants); t.name = "Trunk";
        t.transform.position = V(tx, tBase, c.z); t.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
        float sxz = giantGirth / deadTreeGirth, sy = R(giantLow, giantHigh) / UnityEngine.Mathf.Max(0.5f, deadTreeTall); t.transform.localScale = V(sxz, sy, sxz);
        foreach (var cc in t.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(cc);
        foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        for (int k = 0; k < c.n; k++)
        {
            float cz = UnityEngine.Mathf.Clamp(c.z + R(-c.w * 0.5f, c.w * 0.5f), frontZ0, frontZ1), x = R(row.Item2, row.Item1), fb = RidgeY(x, cz) - flameSink;
            float top = UnityEngine.Mathf.Min(frontTop, fb + (frontTop - fb) * mean * (1f + R(-cardVary, cardVary)));
            var card = (V(x, fb, cz), R(20f, 40f), CardH(fb, top), FlameFrame(), R(-4f, 4f), 1f);
            flameCards.Add(card); if (day.Contains(ci)) flameDay.Add(card);
        }
    }
}
var ridgeNight = Cards("RidgeFlames", giants, flameCards, flameMat, sheetTiles); var ridgeDay = Cards("RidgeFlamesDay", giants, flameDay, flameMat, sheetTiles);
// the valley fires on the -40 floor, x -110 to -220, z 0 to 500, in clusters along north-south lines valleyLineStep m apart; flame tops at
// most valleyTop (1.5 to 2 times a giant, Style.md 6.3.2). Under each cluster a soft glow card on the floor (RebuildSpecs 4.5): floorGlowWide
// times the cluster's width, LookTuning floorGlowColor, fading to nothing at its edge, so the floor reads as a burning sea between
// clusters, never a flat field (it replaces 8.14a's opaque lit floor sheet)
const float valleyX0 = -220f, valleyX1 = -110f, valleyZ0 = 0f, valleyZ1 = 500f, valleyTop = 45f, valleyLineStep = 22f, valleyJitter = 6f, floorGlowWide = 2.5f, glowLift = 0.5f, floorGlowIntensity = 1f;
var valley = new UnityEngine.GameObject("ValleyFires").transform; valley.SetParent(fire.transform, false);
var valleyCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>(); var valleyDay = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
var glowCards = new System.Collections.Generic.List<(UnityEngine.Vector3 c, float r)>(); int valleyClusters = 0;
for (float lx = valleyX1; lx >= valleyX0; lx -= valleyLineStep)
{
    var cl = Clusters(valleyZ0, valleyZ1); var day = DayPick(cl, valleyZ1 - valleyZ0); valleyClusters += cl.Count;
    for (int ci = 0; ci < cl.Count; ci++)
    {
        var c = cl[ci]; float mean = R(clusterMeanLow, 1f), cx = UnityEngine.Mathf.Clamp(lx + R(-valleyJitter, valleyJitter), valleyX0, valleyX1);
        for (int k = 0; k < c.n; k++)
        {
            float fx = UnityEngine.Mathf.Clamp(cx + R(-valleyJitter, valleyJitter), valleyX0, valleyX1), fz = UnityEngine.Mathf.Clamp(c.z + R(-c.w * 0.5f, c.w * 0.5f), valleyZ0, valleyZ1), fb = RidgeY(fx, fz) - 2f;
            float top = UnityEngine.Mathf.Min(valleyTop, fb + (valleyTop - fb) * mean * (1f + R(-cardVary, cardVary)));
            var card = (V(fx, fb, fz), R(20f, 40f), CardH(fb, top), FlameFrame(), 0f, 1f); valleyCards.Add(card); if (day.Contains(ci)) valleyDay.Add(card);
        }
        glowCards.Add((V(cx, RidgeY(cx, c.z) + glowLift, c.z), c.w * floorGlowWide * 0.5f));
    }
}
var valleyNight = Cards("ValleyFlames", valley, valleyCards, flameMat, sheetTiles); var valleyDayG = Cards("ValleyFlamesDay", valley, valleyDay, flameMat, sheetTiles);
// the floor glow: flat round cards on the FlameCard shader (additive, unfogged) over a radial falloff texture made here
const int glowTexSize = 64; const string glowTexPath = fireDir + "/FloorGlowFalloff.asset";
var glowTex = new UnityEngine.Texture2D(glowTexSize, glowTexSize, UnityEngine.TextureFormat.RGBA32, false) { name = "FloorGlowFalloff", wrapMode = UnityEngine.TextureWrapMode.Clamp };
for (int y = 0; y < glowTexSize; y++) for (int x = 0; x < glowTexSize; x++) { float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x + 0.5f, y + 0.5f), new UnityEngine.Vector2(glowTexSize * 0.5f, glowTexSize * 0.5f)) / (glowTexSize * 0.5f); float a = UnityEngine.Mathf.SmoothStep(1f, 0f, d); glowTex.SetPixel(x, y, new UnityEngine.Color(1f, 1f, 1f, a * a)); }
glowTex.Apply(); UnityEditor.AssetDatabase.DeleteAsset(glowTexPath); UnityEditor.AssetDatabase.CreateAsset(glowTex, glowTexPath);
var floorGlowMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_FloorGlow.mat");
if (floorGlowMat == null) { floorGlowMat = new UnityEngine.Material(cardSh); UnityEditor.AssetDatabase.CreateAsset(floorGlowMat, "Assets/Materials/Blockout/Blockout_FloorGlow.mat"); }
floorGlowMat.shader = cardSh; floorGlowMat.SetTexture("_BaseMap", glowTex); floorGlowMat.SetColor("_Color", lookT.floorGlowColor); floorGlowMat.SetFloat("_Intensity", floorGlowIntensity); floorGlowMat.SetFloat("_Ramp", 0f); UnityEditor.EditorUtility.SetDirty(floorGlowMat);
UnityEngine.GameObject floorGlow;
{
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>(); var cols = new System.Collections.Generic.List<UnityEngine.Color>(); var tris = new System.Collections.Generic.List<int>();
    foreach (var g in glowCards)
    {
        int i0 = vs.Count; vs.Add(g.c + V(-g.r, 0f, -g.r)); vs.Add(g.c + V(g.r, 0f, -g.r)); vs.Add(g.c + V(-g.r, 0f, g.r)); vs.Add(g.c + V(g.r, 0f, g.r));
        uvs.Add(new UnityEngine.Vector2(0f, 0f)); uvs.Add(new UnityEngine.Vector2(1f, 0f)); uvs.Add(new UnityEngine.Vector2(0f, 1f)); uvs.Add(new UnityEngine.Vector2(1f, 1f));
        for (int k = 0; k < 4; k++) cols.Add(UnityEngine.Color.white); tris.AddRange(new[] { i0, i0 + 2, i0 + 1, i0 + 1, i0 + 2, i0 + 3 });
    }
    var mesh = new UnityEngine.Mesh { name = "FloorGlow", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 }; mesh.SetVertices(vs); mesh.SetUVs(0, uvs); mesh.SetColors(cols); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/FloorGlow.asset");
    floorGlow = new UnityEngine.GameObject("FloorGlow"); floorGlow.transform.SetParent(valley, false); floorGlow.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh;
    var mr = floorGlow.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = floorGlowMat; mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false;
}
// night shows every card; day two the day clusters (LookVisibility); the floor glow at night and on day two
var fireVis = new UnityEngine.GameObject("FireVisibility"); fireVis.transform.SetParent(fire.transform, false);
ShowIn(fireVis, LookVisibility.Show.Night, ridgeNight, valleyNight);
var fireVisDay = new UnityEngine.GameObject("FireVisibilityDayTwo"); fireVisDay.transform.SetParent(fire.transform, false);
ShowIn(fireVisDay, LookVisibility.Show.DayTwo, ridgeDay, valleyDayG);
var fireVisGlow = new UnityEngine.GameObject("FireVisibilityGlow"); fireVisGlow.transform.SetParent(fire.transform, false);
ShowIn(fireVisGlow, LookVisibility.Show.NightAndDayTwo, floorGlow);
// the smoke (the day-one and night sheet, the day-two columns and roof) is built by main3_8_18a_smoke.cs, run after main3_8_18_look.cs
// the front's width as the path end sees it (RebuildSpecs 4.1 asks 154 degrees; Valley.md rev 11 keeps the far front at z -60 to 650):
// the bearings from the path end eye to the front's two ends
var pathEnd = V(faceTo.x, H(faceTo.x, faceTo.y), faceTo.y);
float Bearing(float x, float z) => UnityEngine.Mathf.Atan2(x - pathEnd.x, z - pathEnd.z) * UnityEngine.Mathf.Rad2Deg;
float frontSpan = UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(Bearing(frontRows[0].Item1, frontZ0), Bearing(frontRows[0].Item1, frontZ1)));
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cairn at " + cairnPos.ToString("F1") + " | chain and IW2 across the chute gap, IW2 " + iwBot.ToString("F1") + " to " + iwTop.ToString("F1") + " | steps " + steps.childCount + ", climb pieces " + climbRoot.childCount
    + " | stones ground " + H(-4f, 226f).ToString("F1") + " tops " + stoneTop + " | stand-in fire: floor " + floorY + ", far ridge crest " + ridgeCrest + ", " + flameCards.Count + " front cards to " + frontTop + " (z " + frontZ0 + " to " + frontZ1 + "), " + valleyCards.Count + " valley cards to " + valleyTop + " | clusters: front " + frontClusters + ", valley " + valleyClusters + "; day two " + flameDay.Count + " front and " + valleyDay.Count + " valley cards; floor glows " + glowCards.Count + "; front span from the path end " + frontSpan.ToString("F0") + " degrees";
