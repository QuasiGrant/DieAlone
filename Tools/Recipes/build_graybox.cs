var scenePath = "Assets/Scenes/Graybox.unity";
var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
    UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
    UnityEditor.SceneManagement.NewSceneMode.Single);

UnityEngine.GameObject Box(string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 size)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name;
    go.transform.position = pos;
    go.transform.localScale = size;
    return go;
}

// Ground: 50 m across, 1 m thick, top surface at y = 0
Box("Ground", new UnityEngine.Vector3(0, -0.5f, 0), new UnityEngine.Vector3(50, 1, 50));

// Tower stand-in
Box("Tower", new UnityEngine.Vector3(0, 10, 12), new UnityEngine.Vector3(6, 20, 6));

// Scale boxes
Box("Box_Small_A",  new UnityEngine.Vector3(-6, 0.25f, -4),  new UnityEngine.Vector3(0.5f, 0.5f, 0.5f));
Box("Box_Small_B",  new UnityEngine.Vector3( 7, 0.5f, -7),   new UnityEngine.Vector3(1, 1, 1));
Box("Box_Medium_A", new UnityEngine.Vector3(-12, 1, 4),      new UnityEngine.Vector3(2, 2, 2));
Box("Box_Medium_B", new UnityEngine.Vector3( 12, 0.75f, 6),  new UnityEngine.Vector3(3, 1.5f, 2));
Box("Box_Large_A",  new UnityEngine.Vector3(-8, 2, 16),      new UnityEngine.Vector3(4, 4, 4));
Box("Box_Tall_A",   new UnityEngine.Vector3( 15, 3, -14),    new UnityEngine.Vector3(1.5f, 6, 1.5f));

// Directional light
var lightGo = new UnityEngine.GameObject("Directional Light");
var light = lightGo.AddComponent<UnityEngine.Light>();
light.type = UnityEngine.LightType.Directional;
light.intensity = 1.0f;
light.shadows = UnityEngine.LightShadows.Soft;
lightGo.transform.rotation = UnityEngine.Quaternion.Euler(50, -30, 0);

// Camera, elevated, looking at the layout
var camGo = new UnityEngine.GameObject("Main Camera");
camGo.tag = "MainCamera";
camGo.AddComponent<UnityEngine.Camera>();
camGo.AddComponent<UnityEngine.AudioListener>();
camGo.transform.position = new UnityEngine.Vector3(-22, 14, -26);
camGo.transform.LookAt(new UnityEngine.Vector3(0, 3, 4));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, scenePath);
UnityEditor.AssetDatabase.Refresh();
return "saved=" + saved + " path=" + scenePath + " roots=" + scene.rootCount;
