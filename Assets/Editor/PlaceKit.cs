using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Shared placing tools for the Main3 place recipes (8.17, Tools/Recipes/main3_8_17_*.cs): owned pack prefabs set on the ground or on
/// a surface, scaled to a box, fitted colliders, retinted project materials, practical lights and board labels. Editor only; the recipes
/// create one kit per run and read its report. Every prefab path is relative to Assets/ and has no .prefab extension.
public sealed class PlaceKit
{
    public const string CI = "Revolving Pizza Games/Cabin In The Woods/Prefabs/";
    public const string CS = "Revolving Pizza Games/Campsite/Prefabs/";
    public const string CC = "Revolving Pizza Games/Catacombs/Prefabs/";
    public const string CE = "Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/";
    public const string BK = "BK/PureNature_Redwood/Prefabs/";
    public const string SP = "PSX Supplies Pack/Prefabs/";
    public const string FT = "PSX Farm Tools Pack/Prefabs/Default/";
    public const string MH = "Effigy GameWorks/Menhir Stone Circle/Prefabs/";
    const string MaterialDir = "Assets/Materials/Places";
    const float Smoothness = 0.15f;       // ShaderSwap 2.2: nothing wet or glossy
    const float LabelScale = 0.01f, LabelFace = 0.035f;

    public readonly Scene Scene;
    public readonly Terrain Terrain;
    public readonly LookTuning Look;
    public readonly List<string> Missing = new List<string>();
    public int Props { get; private set; }
    readonly Dictionary<string, Material> mats = new Dictionary<string, Material>();
    readonly Font font;

    public PlaceKit(Scene scene)
    {
        Scene = scene;
        var t = Root("Terrain"); Terrain = t != null ? t.GetComponent<Terrain>() : null;
        Look = AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (!AssetDatabase.IsValidFolder(MaterialDir)) AssetDatabase.CreateFolder("Assets/Materials", "Places");
    }

    public GameObject Root(string name) { foreach (var r in Scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
    public float H(float x, float z) => Terrain.SampleHeight(new Vector3(x, 0f, z)) + Terrain.transform.position.y;

    /// An empty group, replacing any earlier one of the same name under the parent (so a place recipe can be rerun).
    public Transform Fresh(string name, Transform parent, Vector3 worldPos, float yaw)
    {
        var old = parent.Find(name); if (old != null) Object.DestroyImmediate(old.gameObject);
        return Group(name, parent, worldPos, yaw);
    }
    public Transform Group(string name, Transform parent, Vector3 worldPos, float yaw)
    {
        var g = new GameObject(name).transform; g.SetParent(parent, false); g.SetPositionAndRotation(worldPos, Quaternion.Euler(0f, yaw, 0f)); return g;
    }
    /// Destroys a gray blockout piece this place replaces; safe when it is already gone.
    public static void Remove(Transform t) { if (t != null) Object.DestroyImmediate(t.gameObject); }

    public GameObject Spawn(string path, Transform parent)
    {
        var src = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + path + ".prefab") ?? FindInPack(path);
        if (src == null) { if (!Missing.Contains(path)) Missing.Add(path); return null; }
        Props++; return (GameObject)PrefabUtility.InstantiatePrefab(src, parent);
    }

    /// A prefab named like the path's last part, anywhere under the path's pack folder (its first two folders), when the path's
    /// subfolders are not exact; one exact file-name match only.
    GameObject FindInPack(string path)
    {
        var parts = path.Split('/'); if (parts.Length < 2) return null;
        string name = parts[parts.Length - 1], pack = "Assets/" + parts[0] + "/" + parts[1];
        if (!AssetDatabase.IsValidFolder(pack)) pack = "Assets/" + parts[0];
        foreach (var g in AssetDatabase.FindAssets(name + " t:Prefab", new[] { pack }))
        {
            var p = AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return AssetDatabase.LoadAssetAtPath<GameObject>(p);
        }
        return null;
    }

    /// World bounds of the mesh renderers (LOD 0 only when the prefab has a LODGroup).
    public static Bounds MeshBounds(GameObject g)
    {
        var rs = new List<Renderer>();
        var lod = g.GetComponentInChildren<LODGroup>();
        if (lod != null && lod.GetLODs().Length > 0) rs.AddRange(lod.GetLODs()[0].renderers);
        else rs.AddRange(g.GetComponentsInChildren<Renderer>());
        bool any = false; var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (var r in rs) { if (r == null || !(r is MeshRenderer || r is SkinnedMeshRenderer)) continue; if (!any) { b = r.bounds; any = true; } else b.Encapsulate(r.bounds); }
        return b;
    }

    /// A prefab in a parent's space: its lowest mesh point on local height lp.y, its mesh centre over (lp.x, lp.z) when centred.
    public GameObject On(string path, Transform parent, Vector3 lp, float yaw, float scale = 1f, bool collide = false, Vector3? tilt = null, bool centred = false)
    {
        var g = Spawn(path, parent); if (g == null) return null;
        g.transform.localPosition = lp; g.transform.localRotation = Quaternion.Euler(tilt.HasValue ? tilt.Value.x : 0f, yaw, tilt.HasValue ? tilt.Value.z : 0f);
        g.transform.localScale = Vector3.one * scale;
        var b = MeshBounds(g); var want = parent.TransformPoint(lp);
        var shift = new Vector3(centred ? want.x - b.center.x : 0f, want.y - b.min.y, centred ? want.z - b.center.z : 0f);
        g.transform.position += shift;
        if (collide) FitCollider(g); return g;
    }

    /// A prefab on the terrain at world (x, z): its lowest mesh point on the lowest ground under its footprint, less sink.
    public GameObject Ground(string path, Transform parent, float x, float z, float yaw, float scale = 1f, bool collide = false, float sink = 0.02f, Vector3? tilt = null)
    {
        var g = Spawn(path, parent); if (g == null) return null;
        g.transform.SetPositionAndRotation(new Vector3(x, 0f, z), Quaternion.Euler(tilt.HasValue ? tilt.Value.x : 0f, yaw, tilt.HasValue ? tilt.Value.z : 0f));
        g.transform.localScale = Vector3.one * scale;
        var b = MeshBounds(g);
        float gy = Mathf.Min(Mathf.Min(H(b.min.x, b.min.z), H(b.max.x, b.max.z)), Mathf.Min(Mathf.Min(H(b.min.x, b.max.z), H(b.max.x, b.min.z)), H(x, z)));
        g.transform.position += new Vector3(0f, gy - b.min.y - sink, 0f);
        if (collide) FitCollider(g); return g;
    }

    /// A prefab scaled to fill a box of the given size (parent axes, after yaw), bottom centre at lp; its own colliders removed.
    public GameObject Fill(string path, Transform parent, Vector3 lp, Vector3 size, float yaw = 0f, bool collide = false)
    {
        var g = Spawn(path, parent); if (g == null) return null;
        StripColliders(g);
        g.transform.localPosition = Vector3.zero; g.transform.localRotation = Quaternion.Euler(0f, yaw, 0f); g.transform.localScale = Vector3.one;
        var b = LocalBounds(g, parent);
        var s = new Vector3(size.x / Mathf.Max(0.01f, b.size.x), size.y / Mathf.Max(0.01f, b.size.y), size.z / Mathf.Max(0.01f, b.size.z));
        var inv = Quaternion.Inverse(g.transform.localRotation);
        var sLocal = inv * s; g.transform.localScale = new Vector3(Mathf.Abs(sLocal.x), Mathf.Abs(sLocal.y), Mathf.Abs(sLocal.z));
        b = LocalBounds(g, parent);
        g.transform.localPosition = new Vector3(lp.x - b.center.x, lp.y - b.min.y, lp.z - b.center.z);
        if (collide) FitCollider(g); return g;
    }

    /// Mesh bounds of g in the parent's axes.
    public static Bounds LocalBounds(GameObject g, Transform parent)
    {
        bool any = false; var b = new Bounds();
        foreach (var r in g.GetComponentsInChildren<Renderer>())
        {
            if (!(r is MeshRenderer)) continue; var wb = r.bounds;
            for (int i = 0; i < 8; i++)
            {
                var c = parent.InverseTransformPoint(new Vector3((i & 1) == 0 ? wb.min.x : wb.max.x, (i & 2) == 0 ? wb.min.y : wb.max.y, (i & 4) == 0 ? wb.min.z : wb.max.z));
                if (!any) { b = new Bounds(c, Vector3.zero); any = true; } else b.Encapsulate(c);
            }
        }
        return b;
    }

    public static void StripColliders(GameObject g) { foreach (var c in g.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c); }

    /// One box round the meshes (in g's own axes), unless the prefab already carries colliders.
    public static void FitCollider(GameObject g)
    {
        if (g.GetComponentInChildren<Collider>() != null) return;
        var b = LocalBounds(g, g.transform); var bc = g.AddComponent<BoxCollider>(); bc.center = b.center; bc.size = b.size;
    }

    /// A project material with a retint, saved under Assets/Materials/Places (one per name).
    public Material Tinted(string name, string fromPath, Color tint, Vector2 tiling)
    {
        if (mats.TryGetValue(name, out var have)) return have;
        var from = AssetDatabase.LoadAssetAtPath<Material>(fromPath); if (from == null) { Missing.Add(fromPath); return null; }
        string path = MaterialDir + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null) { m = new Material(from); AssetDatabase.CreateAsset(m, path); } else m.CopyPropertiesFromMaterial(from);
        m.SetColor("_BaseColor", tint); m.SetTextureScale("_BaseMap", tiling); m.SetFloat("_Smoothness", Smoothness);
        EditorUtility.SetDirty(m); mats[name] = m; return m;
    }
    /// An unlit glow (DieAlone/FireStandIn) for lit signs and window panes; the colour and strength come from LookTuning.
    public Material Glow(string name, Color colour, float intensity)
    {
        if (mats.TryGetValue(name, out var have)) return have;
        var sh = Shader.Find("DieAlone/FireStandIn"); if (sh == null) { Missing.Add("shader DieAlone/FireStandIn"); return null; }
        string path = MaterialDir + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path); if (m == null) { m = new Material(sh); AssetDatabase.CreateAsset(m, path); }
        m.shader = sh; m.SetColor("_Color", colour); m.SetFloat("_Intensity", intensity); EditorUtility.SetDirty(m); mats[name] = m; return m;
    }

    /// A textured box (never an untextured primitive), colliderless unless asked.
    public GameObject Slab(string name, Transform parent, Vector3 lp, Vector3 size, Material mat, Vector3 euler = default, bool collide = false)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cube); g.name = name; g.transform.SetParent(parent, false);
        g.transform.localPosition = lp; g.transform.localRotation = Quaternion.Euler(euler); g.transform.localScale = size;
        if (!collide) Object.DestroyImmediate(g.GetComponent<Collider>());
        g.GetComponent<Renderer>().sharedMaterial = mat; return g;
    }
    /// An invisible box collider (for pack meshes that ship without one); it always sits inside a drawn mesh.
    public GameObject Blocker(string name, Transform parent, Vector3 lp, Vector3 size, float yaw = 0f)
    {
        var g = new GameObject(name); g.transform.SetParent(parent, false); g.transform.localPosition = lp; g.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        g.AddComponent<BoxCollider>().size = size; return g;
    }

    /// A practical light (#FFA860 from LookTuning, no shadows) whose brightness follows the look.
    public Light Practical(string name, Transform parent, Vector3 lp, float range, PracticalLight.Kind kind, PracticalLight.ByDay byDay)
    {
        var g = new GameObject(name); g.transform.SetParent(parent, false); g.transform.localPosition = lp;
        var l = g.AddComponent<Light>(); l.type = LightType.Point; l.range = range; l.intensity = Look.firePitIntensity; l.shadows = LightShadows.None; l.color = Look.practicalColor;
        var pl = g.AddComponent<PracticalLight>(); var so = new SerializedObject(pl);
        so.FindProperty("tuning").objectReferenceValue = Look; so.FindProperty("kind").enumValueIndex = (int)kind; so.FindProperty("byDay").enumValueIndex = (int)byDay; so.ApplyModifiedPropertiesWithoutUndo();
        return l;
    }

    /// World-space text on the front face of a board (the project's sign pattern, build_signs.cs); front = the board's -z side, or +z onBack.
    public void Label(Transform board, string text, Color colour, int maxSize = 24, bool onBack = false)
    {
        var cg = new GameObject("Label", typeof(RectTransform)); cg.transform.SetParent(board.parent, false);
        var canvas = cg.AddComponent<Canvas>(); canvas.renderMode = RenderMode.WorldSpace;
        cg.GetComponent<RectTransform>().sizeDelta = new Vector2(board.localScale.x / LabelScale, board.localScale.y / LabelScale);
        float side = onBack ? -1f : 1f;
        cg.transform.SetPositionAndRotation(board.position - side * board.forward * (board.lossyScale.z * 0.5f + LabelFace * 0.2f), board.rotation * Quaternion.Euler(0f, onBack ? 180f : 0f, 0f));
        cg.transform.localScale = Vector3.one * LabelScale;
        var tg = new GameObject("Text", typeof(RectTransform)); tg.transform.SetParent(cg.transform, false);
        var trt = tg.GetComponent<RectTransform>(); trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one; trt.offsetMin = trt.offsetMax = Vector2.zero;
        var tx = tg.AddComponent<UnityEngine.UI.Text>(); tx.font = font; tx.fontSize = maxSize; tx.fontStyle = FontStyle.Bold; tx.alignment = TextAnchor.MiddleCenter;
        tx.color = colour; tx.text = text; tx.horizontalOverflow = HorizontalWrapMode.Wrap; tx.resizeTextForBestFit = true; tx.resizeTextMinSize = 6; tx.resizeTextMaxSize = maxSize;
    }

    /// A working door (Door system, Rules and Tips: DOORS): the hinge at one jamb, the panel along the hinge's +x, the pack door
    /// mesh filled into the panel box. Swings away from the user.
    public GameObject Door(Transform parent, Vector3 hingeLocal, float yaw, Vector3 panelSize, string visualPath)
    {
        var tuning = AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset"); if (tuning == null) { Missing.Add("PlayerTuning"); return null; }
        var hinge = Group("Door", parent, parent.TransformPoint(hingeLocal), 0f); hinge.localRotation = Quaternion.Euler(0f, yaw, 0f);
        var rb = hinge.gameObject.AddComponent<Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
        var panel = Blocker("Panel", hinge, new Vector3(panelSize.x * 0.5f, panelSize.y * 0.5f, 0f), panelSize);
        Fill(visualPath, hinge, new Vector3(panelSize.x * 0.5f, 0f, 0f), panelSize);
        var door = hinge.gameObject.AddComponent<global::Door>(); var so = new SerializedObject(door);
        so.FindProperty("prompt").stringValue = "Open"; so.FindProperty("tuning").objectReferenceValue = tuning;
        so.FindProperty("panel").objectReferenceValue = panel.GetComponent<BoxCollider>(); so.ApplyModifiedPropertiesWithoutUndo();
        return hinge.gameObject;
    }

    /// Shows the targets only in the given looks (LookVisibility on a holder under the parent).
    public void ShowIn(Transform parent, string name, LookVisibility.Show show, List<GameObject> targets)
    {
        var host = new GameObject(name); host.transform.SetParent(parent, false);
        var v = host.AddComponent<LookVisibility>(); var so = new SerializedObject(v);
        so.FindProperty("show").enumValueIndex = (int)show;
        so.FindProperty("dayTwo").objectReferenceValue = AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset");
        var tp = so.FindProperty("targets"); tp.arraySize = targets.Count; for (int i = 0; i < targets.Count; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = targets[i];
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    /// A gray resident capsule becomes a bare marker: same place and name, nothing drawn.
    public static void MarkerOnly(Transform t)
    {
        if (t == null) return;
        foreach (var r in t.GetComponentsInChildren<Renderer>()) { var mf = r.GetComponent<MeshFilter>(); Object.DestroyImmediate(r); if (mf != null) Object.DestroyImmediate(mf); }
        foreach (var c in t.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
    }
    public Transform Marker(string name, Transform parent, Vector3 lp, float yaw)
    {
        var g = new GameObject(name).transform; g.SetParent(parent, false); g.localPosition = lp; g.localRotation = Quaternion.Euler(0f, yaw, 0f); return g;
    }

    /// One capsule on a dead tree's trunk (Celestia Tree_Dead, standing or lying), measured from its mesh: the median reach of the
    /// vertices in the lowest band of its height about their centre, along the mesh's own up axis for share of its height (limbs start
    /// above). The pack's convex hull takes in the limbs, so it goes. Used by 8.7 (Ward snags) and 8.16 (burn snags and fallen trunks).
    public static void DeadTrunkCapsule(GameObject g, float band, float share)
    {
        if (g == null) return;
        foreach (var c in g.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
        var mf = g.GetComponentInChildren<MeshFilter>(); if (mf == null || mf.sharedMesh == null) return;
        var m = mf.sharedMesh; float y0 = m.bounds.min.y, h = m.bounds.size.y, cx = 0f, cz = 0f; int n = 0; var vs = m.vertices;
        foreach (var v in vs) if (v.y < y0 + h * band) { cx += v.x; cz += v.z; n++; }
        if (n == 0) return; cx /= n; cz /= n;
        var d = new List<float>(); foreach (var v in vs) if (v.y < y0 + h * band) d.Add(Mathf.Sqrt((v.x - cx) * (v.x - cx) + (v.z - cz) * (v.z - cz))); d.Sort();
        var cap = mf.gameObject.AddComponent<CapsuleCollider>(); cap.direction = 1; cap.radius = d[d.Count / 2]; cap.height = h * share; cap.center = new Vector3(cx, y0 + h * share * 0.5f, cz);
    }

    /// Clears the terrain's detail layers (grass, ferns) within radius m of a world point, so a camp's ground reads trampled and
    /// props are not buried; returns the cells cleared.
    public int ClearDetail(Vector3 centre, float radius)
    {
        var data = Terrain.terrainData; int res = data.detailResolution; var org = Terrain.transform.position;
        float cw = data.size.x / res, ch = data.size.z / res;
        int x0 = Mathf.Clamp(Mathf.FloorToInt((centre.x - radius - org.x) / cw), 0, res - 1), x1 = Mathf.Clamp(Mathf.CeilToInt((centre.x + radius - org.x) / cw), 0, res - 1);
        int z0 = Mathf.Clamp(Mathf.FloorToInt((centre.z - radius - org.z) / ch), 0, res - 1), z1 = Mathf.Clamp(Mathf.CeilToInt((centre.z + radius - org.z) / ch), 0, res - 1);
        int w = x1 - x0 + 1, h = z1 - z0 + 1, cleared = 0;
        for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
        {
            var map = data.GetDetailLayer(x0, z0, w, h, layer);
            for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
            {
                float px = org.x + (x0 + i + 0.5f) * cw, pz = org.z + (z0 + j + 0.5f) * ch;
                if ((px - centre.x) * (px - centre.x) + (pz - centre.z) * (pz - centre.z) > radius * radius || map[j, i] == 0) continue;
                map[j, i] = 0; cleared++;
            }
            data.SetDetailLayer(x0, z0, layer, map);
        }
        EditorUtility.SetDirty(data); return cleared;
    }

    /// Removes 8.9f's scattered ground cover (SliceLook/OpenGround and ShotGround: grass, ferns, leaves) whose pieces stand within
    /// radius m of a world point; returns the pieces removed. The terrain's own detail layers are cleared with ClearDetail.
    public int ClearCover(Vector3 centre, float radius)
    {
        var slice = Root("SliceLook"); if (slice == null) return 0; int n = 0;
        foreach (var group in new[] { "OpenGround", "ShotGround" })
        {
            var g = slice.transform.Find(group); if (g == null) continue;
            foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<Transform>(g)))
            {
                var p = t.position; if ((p.x - centre.x) * (p.x - centre.x) + (p.z - centre.z) * (p.z - centre.z) > radius * radius) continue;
                Object.DestroyImmediate(t.gameObject); n++;
            }
        }
        return n;
    }

    public string Report() => "props " + Props + ", missing: " + (Missing.Count == 0 ? "none" : string.Join(", ", Missing));
}
