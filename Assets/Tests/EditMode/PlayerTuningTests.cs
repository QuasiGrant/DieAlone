using NUnit.Framework;
using UnityEngine;

/// Sample EditMode test: proves the test assembly can see the game assembly.
public class PlayerTuningTests
{
    [Test]
    public void NewTuning_WalkIsSlowerThanSprint()
    {
        var tuning = ScriptableObject.CreateInstance<PlayerTuning>();
        Assert.Less(tuning.walkSpeed, tuning.sprintSpeed);
        Object.DestroyImmediate(tuning);
    }
}
