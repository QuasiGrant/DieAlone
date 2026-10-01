// Main3 8.18a, pocket fills (Marlow's Breaks_2026-10-01.md traps 2 to 5). Edit mode, Main3; rerunnable; in the runner after
// main3_8_18a_smoke.cs and before main3_8_18a_solid.cs (the fills were sized with the solid pass's colliders in; its rocks get theirs from
// it). Each pocket the player slid into and could not leave gets a solid floor
// at the height that let 14 to 31 of 48 escapes out in Play (Tools/Recipes/main3_breaks_recheck.cs, 2026-10-01), a box collider with a
// BK rock over it so the floor has a visible reason. Rerunning removes the Pockets group and rebuilds it.
// (name, centre x, z, footprint x, z, top y, rock):
//   cave mouth pit, between the entrance's east wall, the east jamb and the overhang (trap 2): top at the pit's east rim;
//   pump trench south pocket by Hedge_Burn_2 and FaceRock BigBoulders_5 (3b): 0.5 m of floor;
//   south of the camp between Hedge_Burn_1 and CS_Rock_3 (4): 2 to 3 m of floor, level with the ground the player came from;
//   pump trench north pocket by Hedge_Burn_0 (3a, opened by the solid pass): 0.5 m of floor;
//   forage patch B on the knoll's west flank among its bushes (5): 0.5 m of floor;
//   the P1 hide west of P1 under the overhang (7d, ClimbRim): 0.5 m of floor.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
var pockets = new (string n, float x, float z, float sx, float sz, float top, string rock)[]
{
    ("CaveMouthPit", 56f, 36.1f, 4f, 3.5f, -2.5f, "Boulder_4"),
    ("TrenchSouth", 182.1f, 106.3f, 2f, 2f, 1.8f, "Boulder_2"),
    ("CampSouth", 177f, 141.1f, 4f, 3f, 13.6f, "Boulder_3"),
    ("TrenchNorth", 172.3f, 119.8f, 2f, 2f, 5.2f, "Boulder_0"),
    ("ForageB", 140.6f, 167.8f, 2f, 2f, 8.4f, "Boulder_1"),
    ("P1Hide", 47.6f, 213.4f, 2f, 2f, 29.8f, "Boulder_5"),
};
const float depth = 4f, rockSink = 0.15f;   // the box reaches depth m down from its top; the rock's top sits rockSink under the box top
var root = kit.Root("Ground815"); if (root == null) return "run 8.15 first";
PlaceKit.Remove(root.transform.Find("Pockets"));
// the cave pit's fill belongs to the cave (Cave/Pockets): E-1 counts a ray down through the mouth's terrain hole that meets the cave as land
var caveRoot = kit.Root("Cave"); if (caveRoot == null) return "no Cave"; PlaceKit.Remove(caveRoot.transform.Find("Pockets"));
var caveGroup = new UnityEngine.GameObject("Pockets").transform; caveGroup.SetParent(caveRoot.transform, false);
var group = new UnityEngine.GameObject("Pockets").transform; group.SetParent(root.transform, false);
var made = new System.Collections.Generic.List<string>();
foreach (var p in pockets)
{
    var holder = new UnityEngine.GameObject(p.n); holder.transform.SetParent(p.n.StartsWith("Cave") ? caveGroup : group, false); holder.transform.position = new UnityEngine.Vector3(p.x, p.top - depth * 0.5f, p.z);
    var box = holder.AddComponent<UnityEngine.BoxCollider>(); box.size = new UnityEngine.Vector3(p.sx, depth, p.sz);
    var rock = kit.Spawn(PlaceKit.BK + "Rocks/" + p.rock, holder.transform);
    if (rock != null)
    {
        PlaceKit.StripColliders(rock); rock.transform.rotation = UnityEngine.Quaternion.Euler(0f, p.x * 37f, 0f); rock.transform.localScale = UnityEngine.Vector3.one;
        var b = PlaceKit.MeshBounds(rock); float s = UnityEngine.Mathf.Max(p.sx, p.sz) / UnityEngine.Mathf.Max(0.1f, UnityEngine.Mathf.Max(b.size.x, b.size.z));
        rock.transform.localScale = UnityEngine.Vector3.one * s; b = PlaceKit.MeshBounds(rock);
        rock.transform.position += new UnityEngine.Vector3(p.x - b.center.x, p.top - rockSink - b.max.y, p.z - b.center.z);
    }
    made.Add(p.n + " top " + p.top);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | pockets filled " + made.Count + " (" + string.Join(", ", made) + ") | " + kit.Report();
