using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;

/// DieAlone > New Scene From Base. Makes a scene from the base scene template
/// (prefabs, night lighting, fog, look filter) and adds it to the build list.
/// This is the one documented way to start a new scene. Recipe in PLAN.md Rules and Tips.
public static class NewSceneMenu
{
    public const string TemplatePath = "Assets/Scenes/Templates/DieAloneBase.scenetemplate";

    [MenuItem("DieAlone/New Scene From Base...")]
    public static void NewSceneInteractive()
    {
        string path = EditorUtility.SaveFilePanelInProject("New scene from base", "NewScene", "unity", "Where to save the new scene", "Assets/Scenes");
        if (string.IsNullOrEmpty(path)) return;
        CreateScene(path);
    }

    /// Creates the scene at scenePath from the template and adds it to the build list.
    /// Returns true on success. Used by the menu item and by automation.
    public static bool CreateScene(string scenePath)
    {
        var template = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(TemplatePath);
        if (template == null)
        {
            Debug.LogError("NewSceneMenu: template not found at " + TemplatePath);
            return false;
        }

        var result = SceneTemplateService.Instantiate(template, false, scenePath);
        if (result == null || !result.scene.IsValid())
        {
            Debug.LogError("NewSceneMenu: could not create scene at " + scenePath);
            return false;
        }

        AddToBuildList(scenePath);
        return true;
    }

    private static void AddToBuildList(string scenePath)
    {
        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        foreach (var s in scenes) if (s.path == scenePath) return;
        scenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
    }
}
