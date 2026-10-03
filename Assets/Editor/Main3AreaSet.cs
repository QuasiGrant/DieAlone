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
        [Tooltip("The place's foot (world metres); y -999 is the ground there.")]
        public Vector3 point;
        [Tooltip("The place's height, metres (found: it must subtend foundMinDeg degrees); 0 means 2.")]
        public float height;
        [Tooltip("Scene path of the place's object, if any: a ray that reaches it counts as reaching the place.")]
        public string objectPath;
    }

    [System.Serializable]
    public struct Interaction
    {
        public string label;
        [Tooltip("Scene path of the object whose colliders are the interaction's body.")]
        public string path;
        [Tooltip("Where the player stands to use it (world metres; y -999 is the ground there; the eye is 1.6 m over it).")]
        public Vector3 approach;
        [Tooltip("The prompt word the area's UI doc gives it (Wren 2026-10-03: every interactable carries a stand-in usable): the area check's PROMPT line wants the interactor's ray from the approach to meet an Interactable with this prompt. Empty: no PROMPT line yet.")]
        public string prompt;
        [Tooltip("The approach's facing and pitch down for the PROMPT ray (degrees); NaN facing aims at the body's centre.")]
        public float facing, pitchDown;
    }

    [System.Serializable]
    public struct Frame
    {
        public string label;
        [Tooltip("Eye (world metres; y -999 is the ground plus 1.6).")]
        public Vector3 eye;
        [Tooltip("Look-at point (world metres; y -999 is level with the eye).")]
        public Vector3 look;
    }

    [System.Serializable]
    public struct DeckTarget
    {
        public string label;
        public Vector3 point;
        [Tooltip("Trees are the cover (the ruin, the rune post): must-hide is a render pixel count of this object, not rays.")]
        public string treeCoverPath;
        [Tooltip("Must-see only (FrontLayout.md 5.4): a hard target is tested against every drawn mesh, not colliders and trees only.")]
        public bool hard;
        [Tooltip("Must-see only (FrontLayout.md 5.4): a loose target is reported, never failed.")]
        public bool loose;
        [Tooltip("Hard must-see: scene path of the target's own object; a ray that reaches it counts as clear.")]
        public string objectPath;
        [Tooltip("Must-hide pixel check: how high the control lifts the target, metres; 0 is 20. A target under a fir and giant crowns needs a higher one (8.24, SS1).")]
        public float controlRaise;
        [Tooltip("Hard must-see: passes when any deck eye sees it (Wren 2026-10-02: the tower check is done from anywhere on the deck; the office door, her blanket and bowl), not the share bar.")]
        public bool anyEye;
    }

    [System.Serializable]
    public struct ClosedFill
    {
        public string label;
        [Tooltip("A point (x, z) in the closed water.")] public Vector2 seed;
        [Tooltip("The water surface, world metres: only ground under it fills, and only a place under it is in the water.")] public float surface;
        [Tooltip("The fill never leaves this rectangle (x, z).")] public Rect within;
        [Tooltip("Fill grid cell, metres.")] public float cell;
        [Tooltip("A cell is shut when any collider stands within this many metres of it (the body's radius), so gaps the body cannot pass hold.")] public float body;
    }

    /// The water a closed fill's seed reaches: grid cells over ground lower than the surface, shut by any collider (not the terrain, not
    /// one the skip test names) within the body's radius between the ground and half a metre over the surface; 4-way from the seed. The
    /// result says whether a standing place lies in that water and under its surface. Call Physics.SyncTransforms first.
    public static System.Func<Vector3, bool> ClosedWater(ClosedFill f, System.Func<float, float, float> ground, System.Func<Collider, bool> skip)
    {
        int nx = Mathf.Max(1, Mathf.CeilToInt(f.within.width / f.cell)), nz = Mathf.Max(1, Mathf.CeilToInt(f.within.height / f.cell));
        var state = new sbyte[nx, nz];   // 0 unknown, 1 open, -1 shut
        bool Open(int i, int j)
        {
            if (state[i, j] != 0) return state[i, j] > 0;
            float x = f.within.xMin + (i + 0.5f) * f.cell, z = f.within.yMin + (j + 0.5f) * f.cell, g = ground(x, z); bool open = g < f.surface;
            if (open)
            {
                float lo = g + 0.05f, hi = f.surface + 0.5f; var half = new Vector3(f.cell * 0.5f + f.body, (hi - lo) * 0.5f, f.cell * 0.5f + f.body);
                foreach (var c in Physics.OverlapBox(new Vector3(x, (lo + hi) * 0.5f, z), half, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore))
                    if (!(c is TerrainCollider) && (skip == null || !skip(c))) { open = false; break; }
            }
            state[i, j] = (sbyte)(open ? 1 : -1); return open;
        }
        var reached = new bool[nx, nz]; int si = Mathf.FloorToInt((f.seed.x - f.within.xMin) / f.cell), sj = Mathf.FloorToInt((f.seed.y - f.within.yMin) / f.cell);
        if (si >= 0 && si < nx && sj >= 0 && sj < nz && Open(si, sj))
        {
            var q = new Queue<Vector2Int>(); q.Enqueue(new Vector2Int(si, sj)); reached[si, sj] = true;
            while (q.Count > 0)
            {
                var c = q.Dequeue();
                foreach (var d in new[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) })
                { int i = c.x + d.x, j = c.y + d.y; if (i < 0 || j < 0 || i >= nx || j >= nz || reached[i, j] || !Open(i, j)) continue; reached[i, j] = true; q.Enqueue(new Vector2Int(i, j)); }
            }
        }
        return p =>
        {
            if (p.y >= f.surface) return false; int i = Mathf.FloorToInt((p.x - f.within.xMin) / f.cell), j = Mathf.FloorToInt((p.z - f.within.yMin) / f.cell);
            return i >= 0 && j >= 0 && i < nx && j < nz && reached[i, j];
        };
    }

    /// How many cells the fill reached (for reports): the predicate's grid is private, so this counts by sampling cell centres.
    public static int ReachedCells(ClosedFill f, System.Func<Vector3, bool> inWater)
    {
        int n = 0; for (float x = f.within.xMin + f.cell * 0.5f; x < f.within.xMax; x += f.cell) for (float z = f.within.yMin + f.cell * 0.5f; z < f.within.yMax; z += f.cell) if (inWater(new Vector3(x, f.surface - 1f, z))) n++; return n;
    }

    [System.Serializable]
    public struct WalkLine
    {
        public string label;
        [Tooltip("Where people walk where no trail runs (the front zone's lot, walk and spur; FrontLayout_UI.md found targets): points in order, world metres, y -999 is the ground.")]
        public Vector3[] points;
    }

    [System.Serializable]
    public struct SpacingException
    {
        [Tooltip("The two interaction labels, either order.")] public string a, b;
        [Tooltip("Why they stand together (Wren's ruling).")] public string reason;
        [Tooltip("The pair passes when its edge gap is at most this (they read as one unit), outside the 0.6 to 1.0 m slot band, and the flood finds no trap or stop stand within snagRadius m of either.")] public float maxEdge;
    }

    [System.Serializable]
    public struct AcceptedStop
    {
        [Tooltip("A flood stand on a stop within radius m of this point is listed as accepted, not failed.")] public Vector3 point;
        public float radius;
        [Tooltip("Why (Wren's ruling).")] public string reason;
    }

    [System.Serializable]
    public class Area
    {
        [Tooltip("Flood stands on a stop ruled artifacts (8.25: the top rail box, a teleport-overlap that did not reproduce).")]
        public AcceptedStop[] acceptedStops;
        [Tooltip("Interaction pairs that stand together by design (8.25: the hook and his table chair).")]
        public SpacingException[] spacingExceptions;
        [Tooltip("Extra walking lines for the found rule, beside the Trails (8.22).")]
        public WalkLine[] walkLines;
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
        [Tooltip("Hard must-see deck lines meet the tower's own rails, floor and cab (8.23 round 2, Wren 2026-10-02: the lake read 128 of 128 eyes where Marlow saw the step from 6 of 9 south-deck eyes). Off: the tower is left out, as 8.21 and 8.22 were judged.")]
        public bool hardSeesTower;
        public DeckTarget[] deckSee;
        public DeckTarget[] deckHide;
        [Tooltip("Interaction points measured for spacing (CampLayout_UI rule 1: centre and edge distances, the first eye-ray hit from the approach).")]
        public Interaction[] interactions;
        [Tooltip("Extra full-size frames the area capture shoots (AreaFrames_<id>.jpg).")]
        public Frame[] frames;
        [Tooltip("Play-mode check recipes (Tools/Recipes) the area capture also runs; each must return ALL PASS.")]
        public string[] playChecks;

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
    [Tooltip("Within stopNear m of a stop (stopRoots or Ignore Raycast) the flood keys places on these finer cells and levels (8.21b, Marlow round 2: 2 m cells never sampled a 0.1 m step from a pocket fill onto a hedge top).")] public float stopCell = 0.5f, stopLevel = 0.5f, stopNear = 2f;
    [Tooltip("Headings per move kind.")] public int floodHeadings = 8;
    [Tooltip("Seconds per flood move.")] public float floodMoveTime = 1f;
    [Tooltip("Metres the flood reaches past the area's bounds before it stops expanding.")] public float floodMargin = 10f;
    [Tooltip("A standing place this far under the terrain fell through the map.")] public float fellUnder = 2f;
    [Tooltip("Unless a floor collider lies within this many metres under it (a cave), metres.")] public float fellProbe = 2.5f;
    [Tooltip("Escape headings per move kind in the trap retest (Marlow: 16 x walk, sprint, sprint-jump = 48).")] public int escapeHeadings = 16;
    [Tooltip("Seconds per escape try.")] public float escapeTime = 3f;
    [Tooltip("Ground closed by thicket (Valley.md 8): a reached place inside one is a leak.")] public Rect[] closedZones;
    [Tooltip("Closed water (8.23 round 2): a reached place below a fill's surface inside the water its seed reaches is a leak.")] public ClosedFill[] closedFills;
    [Tooltip("Stops (8.22 round 2, Marlow 1: a jump onto the brush band and off its far side): a flood place standing on a collider under one of these scene paths, or on the Ignore Raycast layer (hedge boxes, invisible walls), is an escape over a stop and fails.")] public string[] stopRoots = { "FrontZone/BrushBands", "FrontZone/ShiftWalls", "FrontZone/Gate/PlayerBlocker", "Ground815/Stops", "Fence", "Lake/WadeLimit", "Lake/Boathouse/Layout823/StakeLine" };

    [Header("Collider size (PLAN 8.33, ColliderFit)")]
    [Tooltip("A collider face may stand this many metres past its drawn mesh, or colliderShare of the mesh size on that axis, whichever is larger, before it counts as inflated (the turned-prop boxes: the Camp 1 tent stood 1.46 m out).")]
    public float colliderSlack = 0.3f, colliderShare = 0.25f;
    [Tooltip("Colliders under these scene paths are not measured: the pocket fills of 8.18a are boxes that fill a pocket on purpose, the rock only their look (Wren 2026-10-02).")]
    public string[] colliderSkipRoots = { "Ground815/Pockets" };

    [Header("Reach and found (Pim, Wren 2026-10-02)")]
    [Tooltip("A place counts as reached when a flood place lies within this many metres (xz) of its point.")] public float reachRadius = 4f;
    [Tooltip("And within this many metres in height.")] public float reachRise = 3f;
    [Tooltip("Found: a trail point within this many metres with a clear eye line to the place.")] public float foundReach = 30f;
    [Tooltip("Found (Pim, 8.21 gate): the place within this many degrees of the direction of travel along the trail.")] public float foundMaxAngle = 45f;
    [Tooltip("Found: the place subtends at least this many degrees vertically from the trail eye.")] public float foundMinDeg = 1f;
    [Tooltip("Found: a walk point more than this many metres over the terrain (a stair, a stack top) puts the eye on the point, not the terrain.")] public float raisedWalk = 1f;
    [Header("Interaction spacing (CampLayout_UI rule 1)")]
    public float spacingCentre = 1.2f, spacingEdge = 0.5f, approachRay = 2f;
    [Tooltip("Spacing: the 0.6 to 1.0 m band a gap may not fall in (a slot the body cannot pass but a view says it can), and how near a trap or stop stand may come to an excepted pair.")]
    public float slotLow = 0.6f, slotHigh = 1.0f, snagRadius = 2f;

    [Header("Deck (Valley.md 7, CampLayout.md 5)")]
    [Tooltip("Eye grid spacing over the deck, metres.")] public float deckGrid = 1f;
    [Tooltip("Half the deck's side, metres.")] public float deckHalf = 3.5f;
    [Tooltip("Eye and jump heights over the deck floor.")] public float deckEye = 1.6f, deckJump = 2.2f;
    [Tooltip("Rail eyes for must-see (Wren 2026-10-03, Marlow in Play): standing eyes this far inside each deck rail's inner face (the body centre's stop), metres.")] public float railEyeInset = 0.38f;
    [Tooltip("Spacing of the rail eyes along each side of the walkway, metres.")] public float railEyeStep = 1f;
    [Tooltip("Must-see passes when this share of rays is clear (Wren 2026-10-02: 25 percent).")] public float mustSeeShare = 0.25f;    [Tooltip("A ray stops this short of its target, so the target's own collider never blocks it.")] public float rayEndSkip = 1.5f;
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


    public struct FoundResult { public bool found; public Vector3 eye, aim; public string leg; public float dist, angle, tall; public int tried; }

    /// Pim's found rule (8.21 gate, replacing the 30 m line): from a trail point within foundReach m (eye 1.6 m), clear lines to the
    /// place's centre and top (clear(eye, target) decides, by meshes), the place within foundMaxAngle degrees of the direction of travel
    /// in at least one direction along that trail, and at least foundMinDeg degrees tall. Of the points that pass, the farthest (where a
    /// walker first finds it). legs: each trail's points in order; ground(x, z): the walking height.
    public FoundResult Found(Place pl, List<(string leg, List<Vector3> pts)> legs, System.Func<float, float, float> ground, System.Func<Vector3, Vector3, bool> clear)
    {
        var r = new FoundResult { leg = "" };
        float foot = pl.point.y <= -900f ? ground(pl.point.x, pl.point.z) : pl.point.y, h = pl.height > 0f ? pl.height : 2f;
        var footP = new Vector3(pl.point.x, foot, pl.point.z); var centre = footP + Vector3.up * (h * 0.5f); var top = footP + Vector3.up * (h * 0.9f);
        foreach (var (leg, pts) in legs)
            for (int i = 0; i < pts.Count; i++)
            {
                var p = pts[i]; var to = new Vector2(pl.point.x - p.x, pl.point.z - p.z); float d = to.magnitude; if (d > foundReach || d < 0.5f) continue;
                var a = pts[Mathf.Max(0, i - 1)]; var b = pts[Mathf.Min(pts.Count - 1, i + 1)]; var dir = new Vector2(b.x - a.x, b.z - a.z); if (dir.sqrMagnitude < 1e-4f) continue;
                float ang = Mathf.Min(Vector2.Angle(dir, to), Vector2.Angle(-dir, to)); if (ang > foundMaxAngle) continue;
                float g = ground(p.x, p.z); var eye = new Vector3(p.x, (p.y > g + raisedWalk || p.y < g - fellUnder ? p.y : g) + 1.6f, p.z); float tall = Vector3.Angle(footP - eye, footP + Vector3.up * h - eye); if (tall < foundMinDeg) continue;   // a walk line over the terrain (a stair, a stack top) or under it (a cave, 8.27) keeps its own height
                r.tried++;
                if (r.found && d <= r.dist) continue;
                if (!clear(eye, centre) || !clear(eye, top)) continue;
                r.found = true; r.eye = eye; r.aim = centre; r.leg = leg; r.dist = d; r.angle = ang; r.tall = tall;
            }
        return r;
    }
    public static Main3AreaSet Load() => UnityEditor.AssetDatabase.LoadAssetAtPath<Main3AreaSet>("Assets/Settings/Main3Areas.asset");

    public List<InventoryItem> ItemsFor(Area a)
    {
        var list = new List<InventoryItem>();
        foreach (var it in inventory) if (a == null || System.Array.IndexOf(a.inventory, it.label) >= 0) list.Add(it);
        return list;
    }
}
