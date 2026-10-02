using System.Collections.Generic;
using UnityEngine;

/// The Main3 areas of PLAN 8.21 to 8.30 and the numbers their hard checks use (Tools/Recipes/main3_area_check.cs,
/// main3_inventory_check.cs and main3_review_capture.sh --area). Lives at Assets/Settings/Main3Areas.asset, filled by
/// Tools/Recipes/main3_areas_setup.cs. Bounds are drafts from Valley.md 1 and Main3_map.svg; Sable confirms each area's bounds and
/// writes its deck list when the area starts (Wren 2026-10-02). Editor only.
[CreateAssetMenu(fileName = "Main3Areas", menuName = "DieAlone/Main3 Areas")]
public class Main3AreaSet : ScriptableObject
{
    [System.Serializable]
    public struct InventoryItem
    {
        public string label;
        [Tooltip("Scene path; the first part is a scene root. Empty for practicals.")]
        public string path;
        [Tooltip("quads, renderers, lights or practicals")]
        public string kind;
        [Tooltip("quads: the MeshFilter object name (empty for all); renderers: an object name prefix on the renderer or a parent under the path.")]
        public string name;
        public int expected;
    }

    [System.Serializable]
    public struct Place
    {
        public string label;
        [Tooltip("The point that must be reached and seen from a trail (world metres).")]
        public Vector3 point;
    }

    [System.Serializable]
    public struct DeckTarget
    {
        public string label;
        public Vector3 point;
        [Tooltip("Trees are the cover (the ruin, the rune post): must-hide is a render pixel count of this object, not rays.")]
        public string treeCoverPath;
    }

    [System.Serializable]
    public class Area
    {
        public string id;
        public string task;
        public string title;
        [Tooltip("x, z rectangles (Rect x = world x, y = world z).")]
        public Rect[] bounds;
        [Tooltip("DevWarps children in this area.")]
        public string[] warps;
        public Place[] places;
        [Tooltip("Labels of the inventory items that belong to this area.")]
        public string[] inventory;
        [Tooltip("False until Sable writes this area's deck list: the check prints \"no deck list\" and does not pass.")]
        public bool deckListWritten;
        public DeckTarget[] deckSee;
        public DeckTarget[] deckHide;

        public bool Contains(Vector3 p)
        {
            if (bounds == null) return false;
            foreach (var r in bounds) if (r.Contains(new Vector2(p.x, p.z))) return true;
            return false;
        }
    }

    [Header("Flood and traps (Marlow's method, Breaks_2026-10-01.md)")]
    [Tooltip("Grid cell, metres.")] public float floodCell = 2f;
    [Tooltip("Height band that keys a standing place, metres (floors above one another are separate places).")] public float floodLevel = 2f;
    [Tooltip("Headings per move kind.")] public int floodHeadings = 8;
    [Tooltip("Seconds per flood move.")] public float floodMoveTime = 1f;
    [Tooltip("Metres the flood reaches past the area's bounds before it stops expanding.")] public float floodMargin = 10f;
    [Tooltip("A standing place this far under the terrain fell through the map.")] public float fellUnder = 2f;
    [Tooltip("Escape headings per move kind in the trap retest (Marlow: 16 x walk, sprint, sprint-jump = 48).")] public int escapeHeadings = 16;
    [Tooltip("Seconds per escape try.")] public float escapeTime = 3f;
    [Tooltip("Ground closed by thicket (Valley.md 8): a reached place inside one is a leak.")] public Rect[] closedZones;

    [Header("Reach and found (Pim, Wren 2026-10-02)")]
    [Tooltip("A place counts as reached when a flood place lies within this many metres (xz) of its point.")] public float reachRadius = 4f;
    [Tooltip("And within this many metres in height.")] public float reachRise = 3f;
    [Tooltip("Found: a trail point within this many metres with a clear eye line to the place.")] public float foundReach = 30f;
    [Tooltip("Metres over the place point the eye line aims at.")] public float foundAimUp = 1f;

    [Header("Deck (Valley.md 7, CampLayout.md 5)")]
    [Tooltip("Eye grid spacing over the deck, metres.")] public float deckGrid = 1f;
    [Tooltip("Half the deck's side, metres.")] public float deckHalf = 3.5f;
    [Tooltip("Eye and jump heights over the deck floor.")] public float deckEye = 1.6f, deckJump = 2.2f;
    [Tooltip("Must-see passes when this share of rays is clear (Wren 2026-10-02: 25 percent).")] public float mustSeeShare = 0.25f;
    [Tooltip("A ray stops this short of its target, so the target's own collider never blocks it.")] public float rayEndSkip = 1.5f;
    [Tooltip("Tree-cover test: frame size and field of view toward the target, and the pixel change that counts as seen.")] public int pixelSize = 512;
    public float pixelFov = 10f;
    public int pixelTolerance = 4;

    public InventoryItem[] inventory;
    public Area[] areas;

    public Area Find(string id)
    {
        if (areas != null) foreach (var a in areas) if (a.id == id) return a;
        return null;
    }

    /// Counts one inventory item in the open scene (inactive objects included).
    public static int Count(UnityEngine.SceneManagement.Scene scene, InventoryItem it, out bool missingRoot)
    {
        missingRoot = false;
        if (it.kind == "practicals") return Object.FindObjectsByType<PracticalLight>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;
        var top = At(scene, it.path);
        if (top == null) { missingRoot = true; return 0; }
        int found = 0;
        if (it.kind == "quads") { foreach (var mf in top.GetComponentsInChildren<MeshFilter>(true)) if ((it.name == "" || mf.name == it.name) && mf.sharedMesh != null) found += mf.sharedMesh.vertexCount / 4; }
        else if (it.kind == "renderers") { foreach (var r in top.GetComponentsInChildren<Renderer>(true)) if (Under(r.transform, top, it.name)) found++; }
        else if (it.kind == "lights") found = top.GetComponentsInChildren<Light>(true).Length;
        return found;
    }

    /// A scene path whose first part is a scene root (GameObject.Find by name can return a child, e.g. DevWarps/Ward).
    public static Transform At(UnityEngine.SceneManagement.Scene scene, string path)
    {
        var parts = path.Split(new[] { '/' }, 2); Transform root = null;
        foreach (var r in scene.GetRootGameObjects()) if (r.name == parts[0]) { root = r.transform; break; }
        return root == null || parts.Length == 1 ? root : root.Find(parts[1]);
    }

    static bool Under(Transform t, Transform top, string name)
    {
        if (string.IsNullOrEmpty(name)) return true;
        for (var p = t; p != null && p != top.parent; p = p.parent) if (p.name.StartsWith(name)) return true;
        return false;
    }

    public static Main3AreaSet Load() => UnityEditor.AssetDatabase.LoadAssetAtPath<Main3AreaSet>("Assets/Settings/Main3Areas.asset");

    public List<InventoryItem> ItemsFor(Area a)
    {
        var list = new List<InventoryItem>();
        foreach (var it in inventory) if (a == null || System.Array.IndexOf(a.inventory, it.label) >= 0) list.Add(it);
        return list;
    }
}
