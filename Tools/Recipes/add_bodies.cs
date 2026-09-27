var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
int added = 0, had = 0;
foreach (var c in UnityEngine.Object.FindObjectsByType<Carryable>(UnityEngine.FindObjectsSortMode.None))
{
    var rb = c.GetComponent<UnityEngine.Rigidbody>();
    if (rb == null) { rb = c.gameObject.AddComponent<UnityEngine.Rigidbody>(); added++; } else had++;
    rb.mass = 0.5f;
    rb.interpolation = UnityEngine.RigidbodyInterpolation.Interpolate;
    rb.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Continuous;
}
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " bodiesAdded=" + added + " alreadyHad=" + had;
