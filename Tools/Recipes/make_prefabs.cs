var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.Application.isPlaying) return "stop play mode first";
if (UnityEngine.GameObject.Find("GameSystems") != null) return "already restructured";
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Prefabs")) UnityEditor.AssetDatabase.CreateFolder("Assets", "Prefabs");
var sb = new System.Text.StringBuilder();

UnityEngine.GameObject F(string n) { var g = UnityEngine.GameObject.Find(n); if (g == null) throw new System.Exception("missing " + n); return g; }
UnityEngine.GameObject FindInactiveRoot(string n)
{
    foreach (var go in scene.GetRootGameObjects()) if (go.name == n) return go;
    throw new System.Exception("missing root " + n);
}

// ---- GameSystems: Game, EventSystem, HUD, PauseMenu under one root
var systems = new UnityEngine.GameObject("GameSystems");
foreach (var n in new[] { "Game", "EventSystem", "HUD", "PauseMenu" })
    F(n).transform.SetParent(systems.transform, true);

// ---- NightLighting: Moon plus the disabled daytime light renamed Sun
var lighting = new UnityEngine.GameObject("NightLighting");
var night = F("Night");
var moon = night.transform.Find("Moon");
moon.SetParent(lighting.transform, true);
var sun = FindInactiveRoot("Directional Light");
sun.name = "Sun";
sun.transform.SetParent(lighting.transform, true);
UnityEngine.Object.DestroyImmediate(night);

// ---- Save prefabs and connect the scene objects to them
var player = F("Player");
var mode = UnityEditor.InteractionMode.AutomatedAction;
var pPlayer = UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(player, "Assets/Prefabs/Player.prefab", mode);
var pSystems = UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(systems, "Assets/Prefabs/GameSystems.prefab", mode);
var pLighting = UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(lighting, "Assets/Prefabs/NightLighting.prefab", mode);
sb.Append("prefabs: " + (pPlayer != null) + " " + (pSystems != null) + " " + (pLighting != null));

// ---- Check the cross-prefab links survived on the scene instances
var gp = F("Game").GetComponent<GamePause>();
var soG = new UnityEditor.SerializedObject(gp);
var arr = soG.FindProperty("gameplayBehaviours");
int wired = 0; for (int i = 0; i < arr.arraySize; i++) if (arr.GetArrayElementAtIndex(i).objectReferenceValue != null) wired++;
var soI = new UnityEditor.SerializedObject(player.GetComponent<PlayerInteractor>());
bool promptWired = soI.FindProperty("promptUI").objectReferenceValue != null;
sb.Append(" | instance links: pauseBehaviours=" + wired + "/" + arr.arraySize + " promptUI=" + promptWired);

// What the assets themselves hold (expected: those links null there, filled at runtime by the fallbacks)
var assetGp = pSystems.GetComponentInChildren<GamePause>();
var soAG = new UnityEditor.SerializedObject(assetGp);
var aArr = soAG.FindProperty("gameplayBehaviours");
int aWired = 0; for (int i = 0; i < aArr.arraySize; i++) if (aArr.GetArrayElementAtIndex(i).objectReferenceValue != null) aWired++;
var soAI = new UnityEditor.SerializedObject(pPlayer.GetComponent<PlayerInteractor>());
sb.Append(" | asset links: pauseBehaviours=" + aWired + "/" + aArr.arraySize + " promptUI=" + (soAI.FindProperty("promptUI").objectReferenceValue != null));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
sb.Append(" | saved=" + saved + " roots=" + scene.rootCount);
return sb.ToString();
