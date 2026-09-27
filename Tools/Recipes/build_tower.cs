var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene: " + scene.path;

var old = UnityEngine.GameObject.Find("Tower");
if (old == null) return "no Tower to replace";
if (old.transform.childCount > 0) return "Tower already replaced";
UnityEngine.Object.DestroyImmediate(old);

var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

UnityEngine.GameObject Group(string name, UnityEngine.Transform parent)
{
    var g = new UnityEngine.GameObject(name);
    g.transform.SetParent(parent, false);
    return g;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name;
    go.transform.SetParent(parent, false);
    go.transform.localPosition = pos;
    go.transform.localScale = size;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    return go;
}

var tower = new UnityEngine.GameObject("Tower");
var T = tower.transform;
float cz = 12f;                 // tower center z (same spot as the old box)
float deckTop = 10f, deckThick = 0.5f, deckHalf = 4f;

// ---- Legs and deck
var structure = Group("Structure", T);
foreach (var sx in new[] { -3.25f, 3.25f })
    foreach (var sz in new[] { -3.25f, 3.25f })
        Box("Leg", structure.transform, V(sx, deckTop * 0.5f, cz + sz), V(0.5f, deckTop, 0.5f));
Box("Deck", structure.transform, V(0f, deckTop - deckThick * 0.5f, cz), V(deckHalf * 2f, deckThick, deckHalf * 2f));

// ---- Deck rails, 1.1 m tall, gap on the south edge where the stairs arrive (x -3 .. -1.5)
var deckRails = Group("DeckRails", T);
float rh = 1.1f, ry = deckTop + rh * 0.5f;
Box("RailN", deckRails.transform, V(0f, ry, cz + deckHalf), V(8f, rh, 0.1f));
Box("RailE", deckRails.transform, V(deckHalf, ry, cz), V(0.1f, rh, 8f));
Box("RailW", deckRails.transform, V(-deckHalf, ry, cz), V(0.1f, rh, 8f));
Box("RailS_East", deckRails.transform, V(1.25f, ry, cz - deckHalf), V(5.5f, rh, 0.1f));
Box("RailS_West", deckRails.transform, V(-3.5f, ry, cz - deckHalf), V(1.0f, rh, 0.1f));

// ---- Switchback stairs on the south side. Two lanes along x, landings at x = +-2.25.
// Lane 1 (north) z = 8.4, lane 2 (south) z = 7.0. Flights rise 2.5 m over 3 m (10 steps).
var stairs = Group("Stairs", T);
float laneN = cz - 3.6f, laneS = cz - 5.0f, laneW = 1.2f;
float stepThick = 0.5f, landThick = 0.5f, stairRail = 1.0f;
float landDepthZ = 2.6f, landCenterZ = cz - 4.3f;   // spans z 6.4 .. 9.0

void Flight(int n, float z, float h0, bool eastward)
{
    var g = Group("Flight" + n, stairs.transform);
    for (int i = 0; i < 10; i++)
    {
        float top = h0 + 0.25f * (i + 1);
        float x = eastward ? (-1.5f + 0.15f + 0.3f * i) : (1.5f - 0.15f - 0.3f * i);
        Box("Step" + (i + 1), g.transform, V(x, top - stepThick * 0.5f, z), V(0.3f, stepThick, laneW));
    }
    // Sloped rails on both long sides, following the step tops.
    float midY = (h0 + 0.25f + h0 + 2.5f) * 0.5f + stairRail * 0.5f;
    float len = UnityEngine.Mathf.Sqrt(3f * 3f + 2.25f * 2.25f);
    float ang = UnityEngine.Mathf.Atan2(2.25f, 3f) * UnityEngine.Mathf.Rad2Deg * (eastward ? 1f : -1f);
    Box("RailN", g.transform, V(0f, midY, z + laneW * 0.5f), V(len, stairRail, 0.05f), V(0f, 0f, ang));
    Box("RailS", g.transform, V(0f, midY, z - laneW * 0.5f), V(len, stairRail, 0.05f), V(0f, 0f, ang));
}
void Landing(int n, float x, float top, bool railNorth, bool railSouth, bool railOuter)
{
    var g = Group("Landing" + n, stairs.transform);
    Box("Slab", g.transform, V(x, top - landThick * 0.5f, landCenterZ), V(1.5f, landThick, landDepthZ));
    float y = top + stairRail * 0.5f;
    if (railNorth) Box("RailN", g.transform, V(x, y, landCenterZ + landDepthZ * 0.5f), V(1.5f, stairRail, 0.05f));
    if (railSouth) Box("RailS", g.transform, V(x, y, landCenterZ - landDepthZ * 0.5f), V(1.5f, stairRail, 0.05f));
    if (railOuter) Box("RailOuter", g.transform, V(x + (x > 0 ? 0.75f : -0.75f), y, landCenterZ), V(0.05f, stairRail, landDepthZ));
}

Flight(1, laneN, 0.0f, true);
Landing(1,  2.25f, 2.5f, true, true, true);
Flight(2, laneS, 2.5f, false);
Landing(2, -2.25f, 5.0f, true, true, true);
Flight(3, laneN, 5.0f, true);
Landing(3,  2.25f, 7.5f, true, true, true);
Flight(4, laneS, 7.5f, false);
Landing(4, -2.25f, 10.0f, false, true, true);   // north side opens onto the deck

// ---- Room on the deck: 4 x 4 interior, walls 2.5 m, roof, windows on all sides, doorway on the south.
var room = Group("TopRoom", T);
room.transform.localPosition = V(0f, deckTop, cz);
float wh = 2.5f, wt = 0.2f, sill = 1.2f, winH = 0.8f, winW = 1.0f;
float topBand = wh - sill - winH;   // 0.5
// North wall (length 4.4 along x) with centered window.
Box("N_Bottom", room.transform, V(0f, sill * 0.5f, 2.1f), V(4.4f, sill, wt));
Box("N_Top", room.transform, V(0f, wh - topBand * 0.5f, 2.1f), V(4.4f, topBand, wt));
Box("N_PierW", room.transform, V(-1.35f, sill + winH * 0.5f, 2.1f), V(1.7f, winH, wt));
Box("N_PierE", room.transform, V( 1.35f, sill + winH * 0.5f, 2.1f), V(1.7f, winH, wt));
// East and west walls (length 4.0 along z) with centered windows.
foreach (var side in new[] { -2.1f, 2.1f })
{
    string p = side > 0 ? "E_" : "W_";
    Box(p + "Bottom", room.transform, V(side, sill * 0.5f, 0f), V(wt, sill, 4f));
    Box(p + "Top", room.transform, V(side, wh - topBand * 0.5f, 0f), V(wt, topBand, 4f));
    Box(p + "PierS", room.transform, V(side, sill + winH * 0.5f, -1.25f), V(wt, winH, 1.5f));
    Box(p + "PierN", room.transform, V(side, sill + winH * 0.5f,  1.25f), V(wt, winH, 1.5f));
}
// South wall: doorway centered at x = -1.1 (1.0 wide, 2.1 tall), window centered at x = +1.1.
Box("S_PierW", room.transform, V(-1.9f, wh * 0.5f, -2.1f), V(0.6f, wh, wt));
Box("S_DoorLintel", room.transform, V(-1.1f, 2.3f, -2.1f), V(1.0f, 0.4f, wt));
Box("S_PierMid", room.transform, V(0f, wh * 0.5f, -2.1f), V(1.2f, wh, wt));
Box("S_WinBottom", room.transform, V(1.1f, sill * 0.5f, -2.1f), V(winW, sill, wt));
Box("S_WinTop", room.transform, V(1.1f, wh - topBand * 0.5f, -2.1f), V(winW, topBand, wt));
Box("S_PierE", room.transform, V(1.9f, wh * 0.5f, -2.1f), V(0.6f, wh, wt));
// Roof
Box("Roof", room.transform, V(0f, wh + 0.1f, 0f), V(4.4f, 0.2f, 4.4f));
// Furniture stand-ins
Box("Desk", room.transform, V(1.2f, 0.375f, 1.6f), V(1.2f, 0.75f, 0.6f));
Box("Bunk", room.transform, V(-1.45f, 0.25f, 0.5f), V(0.9f, 0.5f, 2.0f));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
int count = 0;
foreach (var t in tower.GetComponentsInChildren<UnityEngine.Transform>()) count++;
return "saved=" + saved + " roots=" + scene.rootCount + " towerObjects=" + count;
