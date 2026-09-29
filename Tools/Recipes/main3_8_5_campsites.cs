// Main3 task 8.5: the three campsites, gray. Run after 8.4 in Main3, edit mode.
// Camp 1 (282, 238) ground 5, 60 m workshop camp: one tent, workbenches, cookfire (Food), landmark a lashed timber spar
//   24 m (top 29) with a string of bulbs, at the centre as drawn on the map.
// Camp 2 (292, 108) ground 4, 40 m boulder field around a granite stack 20 m tall (top 24), 12 m across (map), a ladder up,
//   one tent on top with a cold white lamp, a rain barrel (Water). The stack is a mesh with a mesh collider.
// Camp 3 (78, 146) hollow floor -4, 25 m: tent, fire (Warmth), green-glass lantern hung on the Snag (landmark, built in 8.3).
// Each has a Resident_*_Spot capsule (no collider).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Lake") == null) return "run 8.4 first";
if (Root("Campsites") != null) return "Campsites already exist; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.GameObject Group(string name, UnityEngine.Transform parent, float x, float z, float yaw, float yOff = 0f)
{ var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false); g.transform.position = V(x, H(x, z) + yOff, z); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); return g; }
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 lp, UnityEngine.Vector3 sc, UnityEngine.Vector3? eul = null, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.localPosition = lp; g.transform.localScale = sc;
    if (eul.HasValue) g.transform.localRotation = UnityEngine.Quaternion.Euler(eul.Value);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Cyl = UnityEngine.PrimitiveType.Cylinder; var Cap = UnityEngine.PrimitiveType.Capsule; var Sph = UnityEngine.PrimitiveType.Sphere;
// A-frame tent: two slanted slabs meeting at a ridge; w across, d long, h tall
void Tent(UnityEngine.Transform g, float w, float d, float h)
{
    float half = w * 0.5f, slope = UnityEngine.Mathf.Sqrt(half * half + h * h), ang = UnityEngine.Mathf.Atan2(h, half) * UnityEngine.Mathf.Rad2Deg;
    Prim(Cube, "RoofL", g, V(-half * 0.5f, h * 0.5f, 0f), V(slope, 0.08f, d), V(0f, 0f, ang));
    Prim(Cube, "RoofR", g, V(half * 0.5f, h * 0.5f, 0f), V(slope, 0.08f, d), V(0f, 0f, -ang));
}
void Fire(UnityEngine.Transform g) { for (int i = 0; i < 7; i++) { float a = i * UnityEngine.Mathf.PI * 2f / 7f; Prim(Cube, "Stone", g, V(UnityEngine.Mathf.Cos(a) * 0.8f, 0.15f, UnityEngine.Mathf.Sin(a) * 0.8f), V(0.35f, 0.3f, 0.35f)); } Prim(Cube, "Logs", g, V(0f, 0.12f, 0f), V(0.8f, 0.24f, 0.8f), V(0f, 25f, 0f)); }
var root = new UnityEngine.GameObject("Campsites").transform;

// ================= CAMP 1 =================
var c1 = Group("Camp_1", root, 282f, 238f, 0f).transform;
var spar = Group("Spar", c1, 282f, 238f, 0f).transform;
Prim(Cyl, "Pole", spar, V(0f, 12f - 0.5f, 0f), V(0.45f, 12.5f, 0.45f));
foreach (var s in new[] { 0f, 120f, 240f }) { var a = s * UnityEngine.Mathf.Deg2Rad; Prim(Cube, "Lashing", spar, V(0f, 3f, 0f), V(2.4f, 0.12f, 0.12f), V(0f, s, 0f), false); }
// bulb string: spar top down to a stake 14 m west, bulbs every 2 m (small cubes, no colliders)
var sparTop = V(282f, H(282f, 238f) + 24f, 238f); var stake = V(268f, H(268f, 238f) + 1f, 238f);
Prim(Cube, "Stake", spar, spar.InverseTransformPoint(stake) + V(0f, -0.4f, 0f), V(0.12f, 1.2f, 0.12f));
var str = Prim(Cube, "BulbString", spar, spar.InverseTransformPoint((sparTop + stake) * 0.5f), V(0.03f, 0.03f, UnityEngine.Vector3.Distance(sparTop, stake)), null, false);
str.transform.rotation = UnityEngine.Quaternion.LookRotation(sparTop - stake, UnityEngine.Vector3.up);
for (int i = 1; i < 12; i++) { var p = UnityEngine.Vector3.Lerp(stake, sparTop, i / 12f); var bulb = Prim(Sph, "Bulb", spar, spar.InverseTransformPoint(p) + V(0f, -0.15f, 0f), V(0.18f, 0.18f, 0.18f), null, false); }
var t1 = Group("Tent", c1, 292f, 247f, 20f).transform; Tent(t1, 4f, 5f, 2.4f);
foreach (var wb in new[] { (270f, 245f, 10f), (275f, 251f, 40f), (293f, 232f, -30f) }) { var g = Group("Workbench", c1, wb.Item1, wb.Item2, wb.Item3).transform; Prim(Cube, "Top", g, V(0f, 0.85f, 0f), V(2.2f, 0.1f, 0.9f)); foreach (var lx in new[] { -1f, 1f }) Prim(Cube, "Legs", g, V(lx, 0.4f, 0f), V(0.1f, 0.8f, 0.8f)); }
var lumber = Group("LumberPile", c1, 300f, 240f, 70f).transform; for (int i = 0; i < 4; i++) Prim(Cube, "Board", lumber, V(0f, 0.1f + i * 0.2f, 0f), V(0.3f, 0.18f, 4f), V(0f, i * 6f, 0f));
Fire(Group("Cookfire", c1, 276f, 232f, 0f).transform);
Prim(Cap, "Resident_Camp1_Spot", Group("Resident", c1, 285f, 244f, 200f).transform, V(0f, 0.9f, 0f), V(0.6f, 0.9f, 0.6f), null, false);

// ================= CAMP 2 =================
var c2 = Group("Camp_2", root, 292f, 108f, 0f).transform;
float g2 = H(292f, 108f);
// granite stack: ProBuilder cylinder, 12 m across, top at 24, sunk 1 m, slightly tapered by scaling the top ring
var stackPb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cylinder);
var stack = stackPb.gameObject; stack.name = "GraniteStack"; stack.transform.SetParent(c2, false);
{
    var verts = stackPb.positions; var nv = new UnityEngine.Vector3[verts.Count]; float top = 24f, bottom = g2 - 1f;
    var b = new UnityEngine.Bounds(verts[0], UnityEngine.Vector3.zero); foreach (var v0 in verts) b.Encapsulate(v0);
    for (int i = 0; i < verts.Count; i++)
    {
        var v = verts[i]; float ty = (v.y - b.min.y) / b.size.y; float rScale = UnityEngine.Mathf.Lerp(6f, 4.8f, ty);
        var xz = new UnityEngine.Vector2(v.x - b.center.x, v.z - b.center.z) / (b.size.x * 0.5f);
        nv[i] = V(xz.x * rScale, UnityEngine.Mathf.Lerp(bottom, top, ty) - g2, xz.y * rScale);
    }
    stackPb.positions = nv; stackPb.ToMesh(); stackPb.Refresh();
    stack.transform.position = V(292f, g2, 108f);
    stack.AddComponent<UnityEngine.MeshCollider>().sharedMesh = stack.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    // ProBuilder's default material uses a Built-in shader that draws nothing under URP; use the same default gray as the primitives
    var tmp = UnityEngine.GameObject.CreatePrimitive(Cube); stack.GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = tmp.GetComponent<UnityEngine.MeshRenderer>().sharedMaterial; UnityEngine.Object.DestroyImmediate(tmp);
}
var top2 = Group("StackTop", c2, 292f, 108f, 0f).transform; top2.position = V(292f, 24f, 108f);
var t2 = new UnityEngine.GameObject("Tent"); t2.transform.SetParent(top2, false); t2.transform.localPosition = V(0.5f, 0f, 0.5f); Tent(t2.transform, 2.2f, 2.6f, 1.4f);
Prim(Cube, "TentLamp", top2, V(0.5f, 1.1f, -0.9f), V(0.15f, 0.25f, 0.15f), null, false);
Prim(Cap, "Resident_Camp2_Spot", top2, V(-1.8f, 0.9f, 0.5f), V(0.6f, 0.9f, 0.6f), null, false);
// ladder on the west-south-west face toward the arriving trail (looks only: a ladder climb is not in the game yet)
var lad = new UnityEngine.GameObject("Ladder"); lad.transform.SetParent(c2, false);
float la = 200f * UnityEngine.Mathf.Deg2Rad; var ladBase = V(292f + UnityEngine.Mathf.Sin(la) * 6.3f, g2, 108f + UnityEngine.Mathf.Cos(la) * 6.3f);
lad.transform.position = ladBase; lad.transform.rotation = UnityEngine.Quaternion.Euler(-3f, 200f + 180f, 0f);
foreach (var sx in new[] { -0.25f, 0.25f }) Prim(Cube, "Rail", lad.transform, V(sx, 10f, 0f), V(0.06f, 20.5f, 0.06f), null, false);
for (int i = 1; i < 68; i++) Prim(Cube, "Rung", lad.transform, V(0f, i * 0.3f, 0f), V(0.5f, 0.04f, 0.04f), null, false);
var barrel = Group("RainBarrel", c2, 286.5f, 101.5f, 0f).transform; Prim(Cyl, "Barrel", barrel, V(0f, 0.45f, 0f), V(0.6f, 0.45f, 0.6f));
// boulder field: 16 boulders on a ring 8 to 19 m out, kept 4 m clear of the two trails into the stack
var rng = new System.Random(8005); var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform p in leg) trailPts.Add(new UnityEngine.Vector2(p.position.x, p.position.z));
int boulders = 0;
for (int tries = 0; tries < 400 && boulders < 16; tries++)
{
    float ang = (float)rng.NextDouble() * UnityEngine.Mathf.PI * 2f, rr = 8f + (float)rng.NextDouble() * 11f, sz = 1f + (float)rng.NextDouble() * 2f;
    var p = new UnityEngine.Vector2(292f + UnityEngine.Mathf.Cos(ang) * rr, 108f + UnityEngine.Mathf.Sin(ang) * rr);
    bool ok = UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(ladBase.x, ladBase.z)) > 4f && UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(286.5f, 101.5f)) > 3f;
    foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < sz + 3f) { ok = false; break; }
    if (!ok) continue;
    var bg = Group("Boulder", c2, p.x, p.y, (float)rng.NextDouble() * 360f).transform;
    Prim(Sph, "Rock", bg, V(0f, sz * 0.3f, 0f), V(sz * 1.3f, sz, sz * 1.1f)); boulders++;
}

// ================= CAMP 3 =================
var c3 = Group("Camp_3", root, 78f, 146f, 0f).transform;
var t3 = Group("Tent", c3, 71f, 151f, 60f).transform; Tent(t3, 2.6f, 3.2f, 1.6f);
Fire(Group("Fire", c3, 73.5f, 141.5f, 0f).transform);
var log3 = Group("SeatLog", c3, 71.2f, 139.6f, 70f).transform; Prim(Cyl, "Log", log3, V(0f, 0.25f, 0f), V(0.5f, 1.1f, 0.5f), V(0f, 0f, 90f));
// green-glass lantern hung on the Snag's hollow side, 3 m above the rim ground
var snag = Root("Giants").transform.Find("Heroes/Snag");
var lan = new UnityEngine.GameObject("SnagLantern"); lan.transform.SetParent(c3, false);
var toHollow = (V(78f, 0f, 146f) - V(snag.position.x, 0f, snag.position.z)).normalized;
lan.transform.position = V(snag.position.x, snag.position.y + 3f, snag.position.z) + toHollow * 3.3f;
Prim(Cube, "Lantern", lan.transform, V(0f, 0f, 0f), V(0.25f, 0.35f, 0.25f), null, false);
Prim(Cube, "Hook", lan.transform, -toHollow * 0.2f + V(0f, 0.25f, 0f), V(0.4f, 0.05f, 0.05f), null, false);
Prim(Cap, "Resident_Camp3_Spot", Group("Resident", c3, 75f, 149.5f, 150f).transform, V(0f, 0.9f, 0f), V(0.6f, 0.9f, 0.6f), null, false);

// warps: Camp 2 warp at the ladder foot
var w2 = Root("DevWarps").transform.Find("Camp_2");
if (w2 != null) { w2.position = ladBase + (V(ladBase.x, 0f, ladBase.z) - V(292f, 0f, 108f)).normalized * 2f + V(0f, 0.2f, 0f); w2.rotation = UnityEngine.Quaternion.LookRotation(V(292f - ladBase.x, 0f, 108f - ladBase.z).normalized); }

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " camp1 ground " + H(282f, 238f).ToString("F1") + " spar top " + sparTop.y.ToString("F1") + " | camp2 ground " + g2.ToString("F1") + " stack top " + stack.GetComponent<UnityEngine.Renderer>().bounds.max.y.ToString("F1") + " boulders " + boulders
    + " ladder base " + ladBase.ToString("F1") + " | camp3 floor " + H(78f, 146f).ToString("F1") + " lantern " + lan.transform.position.ToString("F1");
