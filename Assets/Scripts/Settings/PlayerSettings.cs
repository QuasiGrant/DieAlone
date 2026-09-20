using System;
using System.IO;
using UnityEngine;

/// Player-chosen settings that persist between sessions, saved as JSON in
/// Application.persistentDataPath. To add a setting: add a field with a default
/// to PlayerSettingsData, then expose it in a menu. Nothing else changes.
[Serializable]
public class PlayerSettingsData
{
    [Tooltip("Multiplier on look speed for mouse and stick. 1 is the tuned default.")]
    public float lookSensitivity = 1f;
    public bool invertLook = false;
}

public static class PlayerSettings
{
    public const float MinLookSensitivity = 0.2f;
    public const float MaxLookSensitivity = 3f;

    private static PlayerSettingsData current;

    /// Raised after Save. Menus and gameplay can refresh from Current.
    public static event Action Changed;

    public static PlayerSettingsData Current
    {
        get
        {
            if (current == null) Load();
            return current;
        }
    }

    private static string FilePath => Path.Combine(Application.persistentDataPath, "settings.json");

    public static void Load()
    {
        current = new PlayerSettingsData();
        try
        {
            if (File.Exists(FilePath))
                JsonUtility.FromJsonOverwrite(File.ReadAllText(FilePath), current);
        }
        catch (Exception e)
        {
            Debug.LogWarning("PlayerSettings: could not read settings, using defaults. " + e.Message);
        }
        Clamp();
    }

    public static void Save()
    {
        Clamp();
        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(Current, true));
        }
        catch (Exception e)
        {
            Debug.LogWarning("PlayerSettings: could not write settings. " + e.Message);
        }
        Changed?.Invoke();
    }

    private static void Clamp()
    {
        current.lookSensitivity = Mathf.Clamp(current.lookSensitivity, MinLookSensitivity, MaxLookSensitivity);
    }
}
