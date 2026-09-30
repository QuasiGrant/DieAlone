// Main3 task 8.8: cultist cave, gray. Run after 8.7 in Main3, edit mode. Main3.md 3.3 and the map's side section.
// Mouth: a terrain hole (8.1 choice) over the mouth face x 50.3 to 53.7, z 33.6 to 37.4, where the ground falls from 0 to the
// -6 ravine floor; a rock block fills the hole above the passage ceiling (-2 to 0.2) so the opening is 3 m wide, 4 m tall.
// Passage pieces (3 m wide): entrance x 50.5..53.5 z 20.5..37.4 floor -6 ceiling -2; three legs between x 53.5 and 66.5 on
// z 22 / 17 / 12 falling 4 m each (-6..-10, -10..-14, -14..-18) with level 3 x 8 m turns at x 68 and x 52; a level passage
// to the chamber x 71..89, z 3..21, floor -18, ceiling -10. Each piece: floor, ceiling and walls 0.5 m thick, openings where
// pieces join. The day-one board across the mouth is solid (8.9a). Prints the terrain clearance over every piece and
// the terrain holes, the done-check of 8.8.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Ward") == null) return "run 8.7 first";
if (Root("Cave") != null) return "Cave already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>(); var data = terrain.terrainData; float baseY = terrain.transform.position.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + baseY;
const float T = 0.5f;   // rock skin thickness
var cave = new UnityEngine.GameObject("Cave").transform;
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 c, UnityEngine.Vector3 s, float rollZ = 0f, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = name; g.transform.SetParent(parent, false);
    g.transform.position = c; g.transform.localScale = s; g.transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, rollZ);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
// piece footprint x0..x1, z0..z1; floor f0 at x0 and f1 at x1 (level when equal); clear height h.
// open: list of (side, from, to) gaps in the walls; side N/S run along x at z1/z0, E/W run along z at x1/x0.
var pieces = new System.Collections.Generic.List<(string n, float x0, float x1, float z0, float z1, float f0, float f1, float h)>();
void Piece(string n, float x0, float x1, float z0, float z1, float f0, float f1, float h, (char side, float a, float b)[] open)
{
    pieces.Add((n, x0, x1, z0, z1, f0, f1, h));
    var g = new UnityEngine.GameObject(n).transform; g.SetParent(cave, false);
    float lx = x1 - x0, cx = (x0 + x1) * 0.5f, cz = (z0 + z1) * 0.5f, lz = z1 - z0;
    float roll = UnityEngine.Mathf.Atan2(f1 - f0, lx) * UnityEngine.Mathf.Rad2Deg, slopeLen = UnityEngine.Mathf.Sqrt(lx * lx + (f1 - f0) * (f1 - f0));
    float fm = (f0 + f1) * 0.5f; float cosr = UnityEngine.Mathf.Cos(roll * UnityEngine.Mathf.Deg2Rad);
    float sinr = UnityEngine.Mathf.Sin(roll * UnityEngine.Mathf.Deg2Rad);   // the slab offsets run along its own up vector (-sin, cos)
    Box("Floor", g, V(cx + sinr * T * 0.5f, fm - cosr * T * 0.5f, cz), V(slopeLen + 0.02f, T, lz + 2f * T), roll);
    Box("Ceiling", g, V(cx - sinr * T * 0.5f, fm + h + cosr * T * 0.5f, cz), V(slopeLen + 0.02f, T, lz + 2f * T), roll);
    float lo = UnityEngine.Mathf.Min(f0, f1) - T, hi = UnityEngine.Mathf.Max(f0, f1) + h + T;
    void Wall(char side, float a, float b)
    {
        if (b - a < 0.01f) return;
        if (side == 'N' || side == 'S') { float z = side == 'N' ? z1 + T * 0.5f : z0 - T * 0.5f; Box("Wall_" + side, g, V((a + b) * 0.5f, (lo + hi) * 0.5f, z), V(b - a, hi - lo, T)); }
        else { float x = side == 'E' ? x1 + T * 0.5f : x0 - T * 0.5f; Box("Wall_" + side, g, V(x, (lo + hi) * 0.5f, (a + b) * 0.5f), V(T, hi - lo, b - a)); }
    }
    foreach (var side in new[] { 'N', 'S', 'E', 'W' })
    {
        bool alongX = side == 'N' || side == 'S'; float s0 = alongX ? x0 - T : z0 - T, s1 = alongX ? x1 + T : z1 + T;
        var gaps = new System.Collections.Generic.List<(float a, float b)>(); foreach (var o in open) if (o.side == side) gaps.Add((o.a, o.b));
        gaps.Sort((p, q) => p.a.CompareTo(q.a)); float cur = s0;
        foreach (var gp in gaps) { Wall(side, cur, gp.a); cur = gp.b; }
        Wall(side, cur, s1);
    }
}
Piece("Entrance", 50.5f, 53.5f, 20.5f, 37.4f, -6f, -6f, 4f, new[] { ('N', 50.5f, 53.5f), ('E', 20.5f, 23.5f) });
Piece("Leg1", 53.5f, 66.5f, 20.5f, 23.5f, -6f, -10f, 3.5f, new[] { ('W', 20.5f, 23.5f), ('E', 20.5f, 23.5f) });
Piece("TurnEast", 66.5f, 69.5f, 15.5f, 23.5f, -10f, -10f, 3.5f, new[] { ('W', 20.5f, 23.5f), ('W', 15.5f, 18.5f) });
Piece("Leg2", 53.5f, 66.5f, 15.5f, 18.5f, -14f, -10f, 3.5f, new[] { ('W', 15.5f, 18.5f), ('E', 15.5f, 18.5f) });
Piece("TurnWest", 50.5f, 53.5f, 10.5f, 18.5f, -14f, -14f, 3.5f, new[] { ('E', 15.5f, 18.5f), ('E', 10.5f, 13.5f) });
Piece("Leg3", 53.5f, 66.5f, 10.5f, 13.5f, -14f, -18f, 3.5f, new[] { ('W', 10.5f, 13.5f), ('E', 10.5f, 13.5f) });
Piece("ChamberPassage", 66.5f, 71f, 10.5f, 13.5f, -18f, -18f, 3.5f, new[] { ('W', 10.5f, 13.5f), ('E', 10.5f, 13.5f) });
Piece("Chamber", 71f, 89f, 3f, 21f, -18f, -18f, 8f, new[] { ('W', 10.5f, 13.5f) });

// mouth: rock block over the passage inside the hole, and the terrain hole itself
var mouth = new UnityEngine.GameObject("Mouth").transform; mouth.SetParent(cave, false);
Box("RockAbove", mouth, V(52f, (-2f + 0.2f) * 0.5f, 35.45f), V(4.4f, 2.2f, 4.3f));   // covers the hole cells (x 50.0 to 53.9, z 33.4 to 37.5)
// day-one board: CLOSED, UNSAFE planks across the opening, solid; a later day system takes them down from day 2 (dev warp Cave_Chamber reaches inside)
var board = new UnityEngine.GameObject("DayOneBoard").transform; board.SetParent(mouth, false);
foreach (var y in new[] { -5.2f, -4.4f, -3.6f }) Box("Plank", board, V(52f, y, 37.6f), V(3.2f, 0.25f, 0.06f), y == -4.4f ? 6f : -4f, true);   // solid: boarded on day 1 (8.9a)
Box("Sign", board, V(52f, -4.0f, 37.66f), V(1.4f, 0.6f, 0.04f), 0f, false);
int hres = data.holesResolution; float hx = data.size.x / hres, hz = data.size.z / hres;
var tO = terrain.transform.position;   // 8.9j: hole cells count from the terrain origin (-40, -200)
int ix0 = UnityEngine.Mathf.FloorToInt((50.3f - tO.x) / hx), ix1 = UnityEngine.Mathf.CeilToInt((53.7f - tO.x) / hx) - 1, iz0 = UnityEngine.Mathf.FloorToInt((33.6f - tO.z) / hz), iz1 = UnityEngine.Mathf.CeilToInt((37.4f - tO.z) / hz) - 1;
var holes = new bool[iz1 - iz0 + 1, ix1 - ix0 + 1];   // false = hole
// 8.9j: the heightmap cells are 0.62 x 0.68 m, so the hole rounds out past the mouth strip by up to a cell; the rock above the
// passage ceiling is sized to cover the hole cells exactly (plus 0.1 m), so no gap shows round it
{
    var rock = mouth.Find("RockAbove"); float hX0 = tO.x + ix0 * hx - 0.1f, hX1 = tO.x + (ix1 + 1) * hx + 0.1f, hZ0 = tO.z + iz0 * hz - 0.1f, hZ1 = tO.z + (iz1 + 1) * hz + 0.1f;
    rock.position = V((hX0 + hX1) * 0.5f, rock.position.y, (hZ0 + hZ1) * 0.5f); rock.localScale = V(hX1 - hX0, rock.localScale.y, hZ1 - hZ0);
}
data.SetHoles(ix0, iz0, holes);
UnityEditor.EditorUtility.SetDirty(data);

// resident spot and warp
var spot = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Capsule); spot.name = "Resident_Cave_Spot"; spot.transform.SetParent(cave, false);
spot.transform.position = V(84f, -17.1f, 15f); spot.transform.localScale = V(0.6f, 0.9f, 0.6f); UnityEngine.Object.DestroyImmediate(spot.GetComponent<UnityEngine.Collider>());
var warp = new UnityEngine.GameObject("Cave_Chamber"); warp.transform.SetParent(Root("DevWarps").transform, false);
warp.transform.position = V(74f, -17.8f, 12f); warp.transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f);

// ---- check: over every piece's floor plan the terrain must stay above the ceiling's rock top, except inside the mouth hole
var all = data.GetHoles(0, 0, hres, hres);
bool IsHole(float x, float z) { int i = UnityEngine.Mathf.Clamp((int)((x - tO.x) / hx), 0, hres - 1), k = UnityEngine.Mathf.Clamp((int)((z - tO.z) / hz), 0, hres - 1); return !all[k, i]; }
var sb = new System.Text.StringBuilder(); bool ok = true;
foreach (var p in pieces)
{
    float minClear = float.MaxValue; string at = "";
    for (float x = p.x0; x <= p.x1 + 1e-3f; x += 0.25f)
        for (float z = p.z0; z <= p.z1 + 1e-3f; z += 0.25f)
        {
            if (IsHole(x, z)) continue;
            float t = UnityEngine.Mathf.Clamp01((x - p.x0) / (p.x1 - p.x0));
            float top = L(p.f0, p.f1, t) + p.h + T;   // rock top of the ceiling at this x
            float c = H(x, z) - top; if (c < minClear) { minClear = c; at = "(" + x.ToString("F1") + ", " + z.ToString("F1") + ")"; }
        }
    if (minClear < 0f) ok = false;
    sb.Append(p.n + ": ground over rock top min " + minClear.ToString("F2") + " m at " + at + "\n");
}
float L(float a, float b, float t) => a + (b - a) * t;
int holeCells = 0; float hx0 = 999, hx1 = -999, hz0 = 999, hz1 = -999;
for (int k = 0; k < hres; k++) for (int i = 0; i < hres; i++) if (!all[k, i]) { holeCells++; hx0 = UnityEngine.Mathf.Min(hx0, tO.x + i * hx); hx1 = UnityEngine.Mathf.Max(hx1, tO.x + (i + 1) * hx); hz0 = UnityEngine.Mathf.Min(hz0, tO.z + k * hz); hz1 = UnityEngine.Mathf.Max(hz1, tO.z + (k + 1) * hz); }
bool onlyMouth = hx0 >= 50.3f - hx && hx1 <= 53.7f + hx && hz0 >= 33.6f - hz && hz1 <= 37.4f + hz;   // the mouth strip, within one hole cell (8.9j)
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | terrain never enters the passage or chamber: " + (ok ? "YES" : "NO") + " | terrain holes: " + holeCells + " cells, x " + hx0.ToString("F2") + " to " + hx1.ToString("F2") + ", z " + hz0.ToString("F2") + " to " + hz1.ToString("F2") + " -> only at the mouth: " + (onlyMouth ? "YES" : "NO") + "\n" + sb;
