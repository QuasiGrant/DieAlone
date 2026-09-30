// Main3 task 8.4: lake water, wade limit, dock with the pump, boathouse on stilts with the lake resident's spot. Gray.
// Main3.md 2.1 and 3: water (190, 60) 110 x 55 at -5.5; dock root and pump (190, 96), ground -4.5, deck -4.8; map dock
// x 188.4 to 191.6, z 86.4 to 96. Boathouse (240, 52) on stilts, floor -3.8, map footprint 6.4 x 5.6. Run after 8.3.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Trails") == null) return "run 8.3 first";
if (Root("Lake") != null) return "Lake already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sc, UnityEngine.Vector3? eul = null, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.localPosition = pos; g.transform.localScale = sc;
    if (eul.HasValue) g.transform.localRotation = UnityEngine.Quaternion.Euler(eul.Value);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Cyl = UnityEngine.PrimitiveType.Cylinder; var Cap = UnityEngine.PrimitiveType.Capsule;

// blockout water: one dark blue-gray URP Lit material, shared by later blockout water
const string matDir = "Assets/Materials/Blockout", matPath = matDir + "/Blockout_Water.mat";
if (!UnityEditor.AssetDatabase.IsValidFolder(matDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Materials", "Blockout");
var waterMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(matPath);
if (waterMat == null)
{
    var sh = UnityEngine.Shader.Find("Universal Render Pipeline/Lit"); if (sh == null) return "URP Lit shader not found";
    waterMat = new UnityEngine.Material(sh); waterMat.SetColor("_BaseColor", new UnityEngine.Color(0.28f, 0.33f, 0.38f)); waterMat.SetFloat("_Smoothness", 0.7f);
    UnityEditor.AssetDatabase.CreateAsset(waterMat, matPath);
}

var lake = new UnityEngine.GameObject("Lake"); var Lk = lake.transform;
const float cx = 190f, cz = 60f, a = 54.8f, b = 27.6f, water = -5.5f;
var w = Prim(Cyl, "Water", Lk, V(cx, water - 0.01f, cz), V(a * 2f, 0.01f, b * 2f), null, false);
w.GetComponent<UnityEngine.Renderer>().sharedMaterial = waterMat;

// wade limit: invisible boxes on the water line (8.15; the rev 7 ring stood 3 percent inside it and the thicket held the shore), tops
// wadeOver over the ground there, so the water's visible edge is the stop (Valley.md 8); under the dock deck and the boathouse floor they stay low
var wade = new UnityEngine.GameObject("WadeLimit"); wade.transform.SetParent(Lk, false);
const int segs = 96; const float wadeRing = 1f, wadeOver = 1.5f;
for (int i = 0; i < segs; i++)
{
    float t0 = i * 2f * UnityEngine.Mathf.PI / segs, t1 = (i + 1) * 2f * UnityEngine.Mathf.PI / segs;
    var p0 = V(cx + a * wadeRing * UnityEngine.Mathf.Cos(t0), 0f, cz + b * wadeRing * UnityEngine.Mathf.Sin(t0));
    var p1 = V(cx + a * wadeRing * UnityEngine.Mathf.Cos(t1), 0f, cz + b * wadeRing * UnityEngine.Mathf.Sin(t1));
    var mid = (p0 + p1) * 0.5f; var dir = p1 - p0;
    var box = new UnityEngine.GameObject("W" + i); box.transform.SetParent(wade.transform, false);
    // -5.0 under the dock deck, only as wide as the deck, so the notch beside the dock root is closed at the water too
    bool underDock = mid.x > 188.4f && mid.x < 191.6f && mid.z > 80f; float boxTop = underDock ? -5.0f : H(mid.x, mid.z) + wadeOver;
    if (mid.x > 236f && mid.x < 244f && mid.z > 49f && mid.z < 56f) boxTop = UnityEngine.Mathf.Min(boxTop, -4.05f);   // under the boathouse floor (-3.8)
    box.transform.position = V(mid.x, (boxTop - 8.5f) * 0.5f, mid.z); box.transform.rotation = UnityEngine.Quaternion.LookRotation(dir.normalized, UnityEngine.Vector3.up);
    box.AddComponent<UnityEngine.BoxCollider>().size = V(0.4f, boxTop + 8.5f, dir.magnitude + 0.3f);   // y -8.5 to the top
}

// dock: map x 188.4 to 191.6, z 86.4 to 96; deck -4.8 over the water, rising to the ground (-4.5) at the root
var dock = new UnityEngine.GameObject("Dock"); dock.transform.SetParent(Lk, false); var D = dock.transform;
const float dx0 = 188.4f, dx1 = 191.6f, dz0 = 86.4f, dzFlat = 90f, dz1 = 96f, deck = -4.8f, root = -4.5f, th = 0.12f;
float dW = dx1 - dx0, dcx = (dx0 + dx1) * 0.5f;
Prim(Cube, "DeckFlat", D, V(dcx, deck - th * 0.5f, (dz0 + dzFlat) * 0.5f), V(dW, th, dzFlat - dz0));
float runZ = dz1 - dzFlat, riseY = root - deck, len = UnityEngine.Mathf.Sqrt(runZ * runZ + riseY * riseY), pitch = -UnityEngine.Mathf.Atan2(riseY, runZ) * UnityEngine.Mathf.Rad2Deg;
Prim(Cube, "DeckRamp", D, V(dcx, (deck + root) * 0.5f - th * 0.5f, (dzFlat + dz1) * 0.5f), V(dW, th, len), V(pitch, 0f, 0f));
// rails where the deck stands over the water (sides from the dock end to z 88.6, and the end), so the player cannot step off
// into the lake (Marlow finding 2: stepping off the end dropped 1 m to the bed with no way back)
foreach (var rx in new[] { dx0 + 0.04f, dx1 - 0.04f }) Prim(Cube, "Rail", D, V(rx, deck + 0.5f, (dz0 + 88.6f) * 0.5f), V(0.08f, 1.0f, 88.6f - dz0));
Prim(Cube, "RailEnd", D, V(dcx, deck + 0.5f, dz0 + 0.04f), V(dW, 1.0f, 0.08f));
foreach (var px in new[] { dx0 + 0.15f, dx1 - 0.15f }) foreach (var pz in new[] { dz0 + 0.15f, 88.2f, dzFlat })
    Prim(Cube, "Post", D, V(px, (deck + H(px, pz)) * 0.5f - 0.2f, pz), V(0.2f, deck - H(px, pz) + 0.8f, 0.2f));
// pump on the dock root (Water need), gray stand-in
var pump = new UnityEngine.GameObject("Pump"); pump.transform.SetParent(D, false); pump.transform.position = V(190f, root, 94.8f);
Prim(Cyl, "Body", pump.transform, V(0f, 0.55f, 0f), V(0.35f, 0.55f, 0.35f));
Prim(Cube, "Handle", pump.transform, V(0f, 1.05f, 0.35f), V(0.08f, 0.08f, 0.7f), V(-20f, 0f, 0f), false);
Prim(Cube, "Spout", pump.transform, V(0f, 0.85f, -0.3f), V(0.1f, 0.1f, 0.3f), null, false);

// boathouse on stilts: map x 236.8 to 243.2, z 49.6 to 55.2, floor -3.8; door in the east wall toward the shore,
// a gangway ramp from the shore up to the door sill (0.7 m over about 3.5 m)
var bh = new UnityEngine.GameObject("Boathouse"); bh.transform.SetParent(Lk, false); var B = bh.transform;
const float bx0 = 236.8f, bx1 = 243.2f, bz0 = 49.6f, bz1 = 55.2f, floorY = -3.8f, wallH = 2.6f, wt = 0.15f;
float bcx = (bx0 + bx1) * 0.5f, bcz = (bz0 + bz1) * 0.5f, bw = bx1 - bx0, bd = bz1 - bz0;
Prim(Cube, "Floor", B, V(bcx, floorY - 0.1f, bcz), V(bw, 0.2f, bd));
foreach (var sx in new[] { bx0 + 0.2f, bcx, bx1 - 0.2f }) foreach (var sz in new[] { bz0 + 0.2f, bz1 - 0.2f })
{ float g = H(sx, sz); Prim(Cube, "Stilt", B, V(sx, (floorY - 0.2f + g - 0.5f) * 0.5f, sz), V(0.25f, floorY - 0.2f - g + 0.5f, 0.25f)); }
Prim(Cube, "Wall_W", B, V(bx0 + wt * 0.5f, floorY + wallH * 0.5f, bcz), V(wt, wallH, bd));
Prim(Cube, "Wall_N", B, V(bcx, floorY + wallH * 0.5f, bz1 - wt * 0.5f), V(bw, wallH, wt));
Prim(Cube, "Wall_S", B, V(bcx, floorY + wallH * 0.5f, bz0 + wt * 0.5f), V(bw, wallH, wt));
float doorZ = 52.4f, doorW = 1.2f;
Prim(Cube, "Wall_E_South", B, V(bx1 - wt * 0.5f, floorY + wallH * 0.5f, (bz0 + doorZ - doorW * 0.5f) * 0.5f), V(wt, wallH, doorZ - doorW * 0.5f - bz0));
Prim(Cube, "Wall_E_North", B, V(bx1 - wt * 0.5f, floorY + wallH * 0.5f, (bz1 + doorZ + doorW * 0.5f) * 0.5f), V(wt, wallH, bz1 - doorZ - doorW * 0.5f));
Prim(Cube, "Wall_E_Lintel", B, V(bx1 - wt * 0.5f, floorY + 2.25f, doorZ), V(wt, 0.7f, doorW));
Prim(Cube, "Roof", B, V(bcx, floorY + wallH + 0.1f, bcz), V(bw + 0.6f, 0.2f, bd + 0.6f), V(0f, 0f, 6f));   // tin roof, gray here
float gx0 = bx1, gx1 = bx1 + 3.8f, gy0 = floorY, gy1 = H(gx1, doorZ);
float glen = UnityEngine.Mathf.Sqrt((gx1 - gx0) * (gx1 - gx0) + (gy0 - gy1) * (gy0 - gy1)), groll = UnityEngine.Mathf.Atan2(gy0 - gy1, gx1 - gx0) * UnityEngine.Mathf.Rad2Deg;
Prim(Cube, "Gangway", B, V((gx0 + gx1) * 0.5f, (gy0 + gy1) * 0.5f - 0.06f, doorZ), V(glen, 0.12f, 1.4f), V(0f, 0f, -groll));
// bank pockets beside the gangway (rev 15 leftover 4, 8.9e re-walk): the bank drops from the gangway foot to 0.2 m over the
// water north and south of the boathouse's east wall, with no walk out. Each pocket gets a plank deck level with the bank at the
// gangway foot and a rail on its lake side, both visible (Valley.md rev 10 section 8: no invisible stops; 8.14 replaced the
// invisible fill and rail, which are in git history)
const float pocketW = 1.9f, pocketGap = 0.75f, pocketLen = 3f, pocketDeck = 0.12f, pocketRail = 1.0f, pocketPost = 0.1f;
float pocketTop = H(bx1 + pocketW + 0.3f, doorZ);   // the bank at the gangway foot
foreach (var sgn in new[] { -1f, 1f })
{
    Prim(Cube, sgn > 0f ? "PocketDeck_N" : "PocketDeck_S", B, V(bx1 + pocketW * 0.5f, pocketTop - pocketDeck * 0.5f, doorZ + sgn * (pocketGap + pocketLen * 0.5f)), V(pocketW, pocketDeck, pocketLen));
    // the deck's underside down to the water, so nothing walks under it
    Prim(Cube, sgn > 0f ? "PocketSkirt_N" : "PocketSkirt_S", B, V(bx1 + pocketW * 0.5f, (pocketTop + water) * 0.5f - pocketDeck, doorZ + sgn * (pocketGap + pocketLen)), V(pocketW, pocketTop - water, pocketPost));
    Prim(Cube, sgn > 0f ? "PocketRail_N" : "PocketRail_S", B, V(bx1 + pocketW * 0.5f, pocketTop + pocketRail, doorZ + sgn * (pocketGap + pocketLen)), V(pocketW + 0.6f, pocketPost, pocketPost));
    foreach (var px in new[] { bx1 + 0.1f, bx1 + pocketW - 0.1f }) Prim(Cube, "PocketPost", B, V(px, pocketTop + pocketRail * 0.5f, doorZ + sgn * (pocketGap + pocketLen)), V(pocketPost, pocketRail, pocketPost));
}
var spot = Prim(Cap, "Resident_Lake_Spot", B, V(238.6f, floorY + 0.9f, 51.2f), V(0.6f, 0.9f, 0.6f), null, false);

// warp update: the lake boathouse warp stands at the gangway foot
var warps = Root("DevWarps").transform; var wbh = warps.Find("Lake_Boathouse");
if (wbh != null) { wbh.position = V(gx1 + 1.5f, H(gx1 + 1.5f, doorZ) + 0.2f, doorZ); wbh.rotation = UnityEngine.Quaternion.Euler(0f, 270f, 0f); }

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " dock deck " + deck + " root " + root + " pump " + pump.transform.position.ToString("F1") + " boathouse floor " + floorY + " gangway " + gy1.ToString("F2") + " to " + gy0 + " (" + groll.ToString("F1") + " deg) wade boxes " + segs;
