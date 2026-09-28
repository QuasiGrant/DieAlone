using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// Versioned save (task 7.3): round trip at sleep, and old or broken files are refused
/// without a crash. Files go to a temp folder, never the real persistentDataPath.
public class GameSaveTests
{
    private LoopTuning tuning;
    private string folder;
    private string path;

    [SetUp]
    public void SetUp()
    {
        tuning = ScriptableObject.CreateInstance<LoopTuning>();
        tuning.anomalyChanceStart = 1f;
        tuning.anomalyChanceCap = 1f;
        folder = Path.Combine(Path.GetTempPath(), "DieAloneSaveTests");
        Directory.CreateDirectory(folder);
        path = Path.Combine(folder, GameSave.FileName);
        if (File.Exists(path)) File.Delete(path);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(tuning);
        Directory.Delete(folder, true);
    }

    /// Plays two days so the state has spent stats, a met need and open CHECKs.
    private GameState PlayedState()
    {
        var rng = new System.Random(3);
        var state = GameState.NewRun(tuning);
        GameSave.SaveOnSleep(state, path);
        for (int day = 0; day < 2; day++)
        {
            state.MeetNeed(Need.Water);
            state.FileReport();
            state.GiveToWard(1, 1);
            state.Sleep(rng);
        }
        state.MeetNeed(Need.Food);
        return state;
    }

    [Test]
    public void SaveAtSleep_LoadGivesSameState()
    {
        var state = PlayedState();
        Assert.IsTrue(File.Exists(path), "Sleep wrote the save.");
        GameSave.Save(state, path);

        Assert.AreEqual(GameSave.LoadResult.Loaded, GameSave.Load(path, tuning, out var loaded));
        Assert.AreEqual(JsonUtility.ToJson(state), JsonUtility.ToJson(loaded));
        Assert.AreEqual(state.Day, loaded.Day);
        Assert.AreEqual(state.Phase, loaded.Phase);
        Assert.AreEqual(state.Hp, loaded.Hp);
        Assert.AreEqual(state.Mind, loaded.Mind);
        Assert.AreEqual(state.Ward, loaded.Ward);
        Assert.AreEqual(state.OpenChecks, loaded.OpenChecks);
        Assert.Greater(loaded.OpenChecks, 0);
        for (int i = 0; i < state.LocationCount; i++)
        {
            Assert.AreEqual(state.GetLocation(i).Status, loaded.GetLocation(i).Status);
            Assert.AreEqual(state.GetLocation(i).Stage, loaded.GetLocation(i).Stage);
        }
        foreach (Need need in System.Enum.GetValues(typeof(Need)))
            Assert.AreEqual(state.IsNeedMet(need), loaded.IsNeedMet(need), need.ToString());
        Assert.AreSame(tuning, loaded.Tuning);

        // The loaded state keeps playing.
        loaded.FileReport();
        loaded.GiveToWard(0, 0);
        loaded.Sleep(new System.Random(4));
        Assert.AreEqual(state.Day + 1, loaded.Day);
    }

    [Test]
    public void OldVersion_IsRefusedWithoutCrash()
    {
        GameSave.Save(PlayedState(), path);
        string json = File.ReadAllText(path).Replace(
            $"\"version\": {GameSave.CurrentVersion}", "\"version\": 0");
        StringAssert.Contains("\"version\": 0", json);
        File.WriteAllText(path, json);

        LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("version 0"));
        Assert.AreEqual(GameSave.LoadResult.UnsupportedVersion, GameSave.Load(path, tuning, out var loaded));
        Assert.IsNull(loaded);
    }

    [Test]
    public void MissingFile_IsReported()
    {
        Assert.AreEqual(GameSave.LoadResult.Missing, GameSave.Load(path, tuning, out var loaded));
        Assert.IsNull(loaded);
    }

    [Test]
    public void BrokenJson_IsCorrupt()
    {
        File.WriteAllText(path, "{ not json");
        LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("could not be read"));
        Assert.AreEqual(GameSave.LoadResult.Corrupt, GameSave.Load(path, tuning, out _));
    }

    [Test]
    public void ChangedLocationList_IsTuningMismatch()
    {
        GameSave.Save(GameState.NewRun(tuning), path);
        tuning.checkedLocations = new[] { "Lake" };
        Assert.AreEqual(GameSave.LoadResult.TuningMismatch, GameSave.Load(path, tuning, out _));
    }
}
