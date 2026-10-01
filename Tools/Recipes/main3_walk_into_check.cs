// Main3 walk-into check (8.18a; Marlow's breaks hunt 2026-10-01, Breaks_2026-10-01.md 9 to 11). Edit mode or Play; never saves.
// FAIL for every solid mesh over WalkIns.MinTall m tall that a standing player's body can enter without touching a collider first
// (Assets/Editor/WalkIns.cs has the method). The climb and Ward pieces (Ward/Climb, Rock/ClimbRing), which PLAN 8.20 rebuilds, are listed
// apart (none since 8.20) and do not fail, and so are meshes drawn into a trail tread (no collider by rule; listed for the owning recipe to move). In the runner (after main3_8_18a_solid.cs) and the capture (Checks.md).
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
string[] laterRoots = { "Rock/ClimbRing/RimBoulders" };   // the rim rocks stand on the stops; a hull on one is a step over it (main3_8_18a_solid.cs): listed, not failed
var all = WalkIns.Find(tuning.capsuleRadius, tuning.standHeight); var treads = WalkIns.Treads();
var tread = new System.Text.StringBuilder(); int nTread = 0;   // drawn into a trail tread: no collider by rule (main3_8_18a_solid.cs); the owning recipe moves it
var fails = new System.Text.StringBuilder(); var later = new System.Text.StringBuilder(); int nFail = 0, nLater = 0;
foreach (var w in all)
{
    bool isLater = false; foreach (var lr in laterRoots) if (w.Path.StartsWith(lr)) isLater = true;
    var line = w.Path + " at " + w.At.ToString("F1") + ", " + w.Stands + " stands\n";
    if (isLater) { later.Append(line); nLater++; } else if (WalkIns.MeshInTread(w.Renderer, treads)) { tread.Append(line); nTread++; } else { fails.Append(line); nFail++; }
}
return (nFail == 0 ? "ALL PASS" : "FAILS " + nFail) + ": walk-in meshes over " + WalkIns.MinTall + " m with no collider; drawn into a trail tread (move in the owning recipe) " + nTread + (nLater > 0 ? "; left for later " + nLater : "") + "\n" + fails + (nTread > 0 ? "IN A TREAD:\n" + tread : "") + (nLater > 0 ? "LATER:\n" + later : "");
