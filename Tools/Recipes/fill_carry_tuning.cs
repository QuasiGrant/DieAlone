// Task 6.2: write the carry fields that PlayerTuning.asset was missing.
// Unity fills missing serialized fields with the class defaults on load, so the values
// the game uses now are already in memory. Marking the asset dirty and saving writes them out.
if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return "refused: Play mode";
var t = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
if (t == null) return "PlayerTuning.asset not found";
UnityEditor.EditorUtility.SetDirty(t);
UnityEditor.AssetDatabase.SaveAssetIfDirty(t);
return "saved: carryHoldOffset=" + t.carryHoldOffset.ToString("F2") + " carryFollowSpeed=" + t.carryFollowSpeed
    + " placeReach=" + t.placeReach + " placeMinUpNormal=" + t.placeMinUpNormal + " dropForwardSpeed=" + t.dropForwardSpeed
    + " pushPower=" + t.pushPower + " throwSpeed=" + t.throwSpeed + " throwLift=" + t.throwLift;
