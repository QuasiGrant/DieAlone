// Main3 rebuild step 0 (run by main3_rebuild.sh): opens Graybox and deletes Main3.unity and Assets/Terrain/Main3 through the
// Editor, so the task recipes can build Main3 again from 8.1. Refuses in Play mode or with unsaved changes open.
if (UnityEngine.Application.isPlaying) return "FAIL stop play mode first";
for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
    if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).isDirty) return "FAIL unsaved changes in " + UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path;
UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Graybox.unity");
bool a = !System.IO.File.Exists("Assets/Scenes/Main3.unity") || UnityEditor.AssetDatabase.DeleteAsset("Assets/Scenes/Main3.unity");
bool b = !UnityEditor.AssetDatabase.IsValidFolder("Assets/Terrain/Main3") || UnityEditor.AssetDatabase.DeleteAsset("Assets/Terrain/Main3");
return (a && b ? "saved=True reset: " : "FAIL reset: ") + "scene deleted " + a + ", terrain folder deleted " + b;
