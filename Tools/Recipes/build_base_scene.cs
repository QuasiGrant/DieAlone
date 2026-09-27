if (UnityEngine.Application.isPlaying) return "stop play mode first";
var current = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (current.isDirty) return "active scene is dirty, save it first";
const string basePath = "Assets/Scenes/Templates/Base.unity";
const string templatePath = "Assets/Scenes/Templates/DieAloneBase.scenetemplate";
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Scenes/Templates")) UnityEditor.AssetDatabase.CreateFolder("Assets/Scenes", "Templates");

// ---- Base scene: three prefabs, a starter ground, night ambient.
var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
UnityEngine.GameObject Spawn(string prefabPath)
{
    var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(prefabPath);
    if (prefab == null) throw new System.Exception("missing " + prefabPath);
    return (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, scene);
}
var lighting = Spawn("Assets/Prefabs/NightLighting.prefab");
var systems = Spawn("Assets/Prefabs/GameSystems.prefab");
var player = Spawn("Assets/Prefabs/Player.prefab");
player.transform.position = new UnityEngine.Vector3(0f, 0f, 0f);

var ground = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
ground.name = "Ground";
ground.transform.position = new UnityEngine.Vector3(0f, -0.5f, 0f);
ground.transform.localScale = new UnityEngine.Vector3(50f, 1f, 50f);
var groundMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Ground054_25.0x25.0.mat");
if (groundMat != null) ground.GetComponent<UnityEngine.Renderer>().sharedMaterial = groundMat;

UnityEngine.RenderSettings.skybox = null;
UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
UnityEngine.RenderSettings.ambientLight = new UnityEngine.Color(0.1f, 0.13f, 0.2f);
UnityEngine.RenderSettings.sun = lighting.transform.Find("Moon").GetComponent<UnityEngine.Light>();
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
UnityEngine.RenderSettings.fog = tuning.fogEnabled;
UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Linear;
UnityEngine.RenderSettings.fogColor = tuning.fogColor;
UnityEngine.RenderSettings.fogStartDistance = tuning.fogStart;
UnityEngine.RenderSettings.fogEndDistance = tuning.fogEnd;

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, basePath);
if (!saved) return "could not save base scene";

// ---- Template asset from the base scene.
var sceneAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(basePath);
var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneTemplate.SceneTemplateAsset>(templatePath);
if (existing != null) UnityEditor.AssetDatabase.DeleteAsset(templatePath);
var template = UnityEditor.SceneTemplate.SceneTemplateService.CreateTemplateFromScene(sceneAsset, templatePath);
template.templateName = "DieAlone Base";
template.description = "Player, game systems, night lighting, fog, and the VHS look. Start every new scene from this.";
template.addToDefaults = true;
// Reference everything (prefabs, materials, assets); clone nothing.
if (template.dependencies != null)
    for (int i = 0; i < template.dependencies.Length; i++)
        template.dependencies[i].instantiationMode = UnityEditor.SceneTemplate.TemplateInstantiationMode.Reference;
UnityEditor.EditorUtility.SetDirty(template);
UnityEditor.AssetDatabase.SaveAssets();

var sb = new System.Text.StringBuilder();
sb.Append("base=" + basePath + " template=" + UnityEditor.AssetDatabase.GetAssetPath(template) + " deps=" + (template.dependencies != null ? template.dependencies.Length : 0) + " roots=" + scene.rootCount);

// ---- Back to Graybox.
UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Graybox.unity", UnityEditor.SceneManagement.OpenSceneMode.Single);
sb.Append(" | reopened=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
return sb.ToString();
