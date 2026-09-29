using System;
using System.IO;
using UnityEngine;

/// Writes GameState as versioned JSON and reads it back. The file lives in
/// persistentDataPath and is written at sleep (SaveOnSleep). A file from another
/// format version is refused, not loaded, so an old save never crashes the game.
public static class GameSave
{
    /// Bump when the saved fields change, and add a migration in Migrate.
    public const int CurrentVersion = 2;
    public const string FileName = "run.json";

    public enum LoadResult { Loaded, Missing, Corrupt, UnsupportedVersion, TuningMismatch }

    [Serializable]
    private class SaveFile
    {
        public int version;
        public GameState state;
    }

    public static string DefaultPath => Path.Combine(Application.persistentDataPath, FileName);

    /// Saves the state every time it sleeps. Returns the handler so a caller can unsubscribe.
    public static Action<GameState> SaveOnSleep(GameState state, string path)
    {
        Action<GameState> handler = slept => Save(slept, path);
        state.Slept += handler;
        return handler;
    }

    public static void Save(GameState state, string path)
    {
        string json = JsonUtility.ToJson(new SaveFile { version = CurrentVersion, state = state }, true);
        string temp = path + ".tmp";
        File.WriteAllText(temp, json);
        if (File.Exists(path)) File.Replace(temp, path, null);
        else File.Move(temp, path);
    }

    public static LoadResult Load(string path, LoopTuning tuning, out GameState state)
    {
        state = null;
        if (!File.Exists(path)) return LoadResult.Missing;

        SaveFile file;
        try
        {
            file = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        }
        catch (Exception exception) when (exception is ArgumentException || exception is IOException)
        {
            Debug.LogWarning($"Save at {path} could not be read: {exception.Message}");
            return LoadResult.Corrupt;
        }
        if (file == null || file.state == null) return LoadResult.Corrupt;

        if (!Migrate(file))
        {
            Debug.LogWarning($"Save at {path} is version {file.version}; this build reads version {CurrentVersion}.");
            return LoadResult.UnsupportedVersion;
        }
        if (!file.state.Bind(tuning)) return LoadResult.TuningMismatch;

        state = file.state;
        return LoadResult.Loaded;
    }

    /// Brings an older file up to CurrentVersion. Any version without a step is refused.
    private static bool Migrate(SaveFile file)
    {
        // 1 to 2 (task 7.5): CHECK stages were dropped; the new fields (needs waived,
        // recovered today) read as false and 0, which is right at sleep.
        if (file.version == 1) file.version = 2;
        return file.version == CurrentVersion;
    }
}
