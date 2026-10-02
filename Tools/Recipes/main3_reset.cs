// Main3 rebuild step 0 (run by main3_rebuild.sh): opens Graybox and moves Main3.unity and Assets/Terrain/Main3 (with their .meta
// files) out of Assets into Archive/SceneBackups/<yyyy-MM-dd_HHmmss>/ (git-ignored), so the task recipes can build Main3 again from
// 8.1 and a rebuild deletes nothing. Refuses in Play mode or with unsaved changes open. To restore a backup: close the Editor, move
// the folder's Main3.unity, Main3.unity.meta, Main3 and Main3.meta back to Assets/Scenes and Assets/Terrain.
if (UnityEngine.Application.isPlaying) return "FAIL stop play mode first";
for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
    if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).isDirty) return "FAIL unsaved changes in " + UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path;
const string scenePath = "Assets/Scenes/Main3.unity", terrainPath = "Assets/Terrain/Main3", backupRoot = "Archive/SceneBackups";
UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Graybox.unity");
string backup = System.IO.Path.Combine(backupRoot, System.DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
if (System.IO.Directory.Exists(backup)) return "FAIL backup folder exists: " + backup;
System.IO.Directory.CreateDirectory(backup);
// moves path and path.meta into the backup folder; true when nothing is left at path
bool MoveOut(string path, bool folder)
{
    string dest = System.IO.Path.Combine(backup, System.IO.Path.GetFileName(path));
    if (folder ? !System.IO.Directory.Exists(path) : !System.IO.File.Exists(path)) return true;
    if (folder) System.IO.Directory.Move(path, dest); else System.IO.File.Move(path, dest);
    if (System.IO.File.Exists(path + ".meta")) System.IO.File.Move(path + ".meta", dest + ".meta");
    return folder ? !System.IO.Directory.Exists(path) : !System.IO.File.Exists(path);
}
bool a = MoveOut(scenePath, false);
bool b = MoveOut(terrainPath, true);
UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
return (a && b ? "saved=True reset: " : "FAIL reset: ") + "scene moved " + a + ", terrain folder moved " + b + " to " + backup;
