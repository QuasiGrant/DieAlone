if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
string root = "Assets/Revolving Pizza Games/Campsite/Prefabs/Tents/Large Old/Parts/";
UnityEngine.GameObject Part(string n, UnityEngine.Transform parent, UnityEngine.Vector3 localPos, UnityEngine.Vector3 localEuler, UnityEngine.Vector3 localScale)
{
    var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(root + n + ".prefab");
    if (p == null) throw new System.Exception("missing " + n);
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(p, scene);
    go.transform.SetParent(parent, false); go.transform.localPosition = localPos; go.transform.localRotation = UnityEngine.Quaternion.Euler(localEuler); go.transform.localScale = localScale;
    return go;
}
var lean = UnityEngine.GameObject.Find("Campsites/Campsite_2_LeanTo/LeanTo");
var L = lean.transform;
for (int i = L.childCount - 1; i >= 0; i--) UnityEngine.Object.DestroyImmediate(L.GetChild(i).gameObject);
// Canvas awning: high edge at local z 0 (open side faces the cold fire), sloping to the ground behind. Two pack poles hold the high corners.
var canvas = Part("CS_Tent_Large_Old_Awning_Front", L, V(0f, 0f, -2.2f), UnityEngine.Vector3.zero, UnityEngine.Vector3.one); canvas.name = "Canvas";
Part("CS_Tent_Large_Old_Support_1", L, V(-1.45f, 0f, 0.05f), V(0f, 0f, 0f), V(1f, 0.85f, 1f)).name = "PoleW";
Part("CS_Tent_Large_Old_Support_1", L, V(1.45f, 0f, 0.05f), V(0f, 180f, 0f), V(1f, 0.85f, 1f)).name = "PoleE";
var tci = lean.GetComponent<ToggleColorInteractable>();
var so = new UnityEditor.SerializedObject(tci); so.FindProperty("target").objectReferenceValue = canvas.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo();
var col = lean.GetComponent<UnityEngine.BoxCollider>(); col.center = V(0f, 1.0f, 0.9f); col.size = V(3.4f, 2.2f, 2.0f);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);

// Shot from beside the cold fire looking at the shelter.
var outDir = System.IO.Path.Combine(@"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad", "spots_after");
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("L1_leanto_from_fire", V(241.5f, 25.6f, 84.5f), V(243.5f, 25.0f, 90.0f)),
    ("L2_leanto_side", V(248.5f, 25.6f, 88.0f), V(243.5f, 25.0f, 90.0f)),
};
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render(); UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
}
cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "saved=" + saved + " children=" + L.childCount + " target=" + so.FindProperty("target").objectReferenceValue.name;
