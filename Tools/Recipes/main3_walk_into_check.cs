// Main3 walk-into check (8.18a; Marlow's breaks hunt 2026-10-01, Breaks_2026-10-01.md 9 to 11). Edit mode or Play; never saves.
// FAIL for every solid mesh over WalkIns.MinTall m tall that a standing player's body can enter without touching a collider first
// (Assets/Editor/WalkIns.cs has the method). The climb and Ward pieces (Ward/Climb, Rock/ClimbRing), which PLAN 8.20 rebuilds, are listed
// apart and do not fail. In the runner (after main3_8_18a_solid.cs) and the capture (Checks.md).
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
string[] laterRoots = { "Ward/Climb/", "Rock/ClimbRing/" };
var all = WalkIns.Find(tuning.capsuleRadius, tuning.standHeight);
var fails = new System.Text.StringBuilder(); var later = new System.Text.StringBuilder(); int nFail = 0, nLater = 0;
foreach (var w in all)
{
    bool isLater = false; foreach (var lr in laterRoots) if (w.Path.StartsWith(lr)) isLater = true;
    var line = w.Path + " at " + w.At.ToString("F1") + ", " + w.Stands + " stands\n";
    if (isLater) { later.Append(line); nLater++; } else { fails.Append(line); nFail++; }
}
return (nFail == 0 ? "ALL PASS" : "FAILS " + nFail) + ": walk-in meshes over " + WalkIns.MinTall + " m with no collider; climb and Ward (8.20) " + nLater + "\n" + fails + (nLater > 0 ? "8.20:\n" + later : "");
