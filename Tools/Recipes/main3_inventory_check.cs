// Main3 inventory (Vesper, Docs/Review/2026-10-02-Meeting/Vesper.md 4): one line per place and effect, expected count and found count,
// so no gate grades a build that silently dropped dressing. Read-only; edit or Play mode, Main3. Run last in main3_rebuild.sh and first
// in main3_review_capture.sh. The items and their expected counts live in Assets/Settings/Main3Areas.asset (main3_areas_setup.cs).
// area "" checks every item; an area id (main3_review_capture.sh --area sets it) checks that area's items only.
// Returns "ALL PASS" when nothing is zero and nothing differs; a count that differs from the expected one is DIFF (a recipe changed what
// it builds: update the expected number in main3_areas_setup.cs in the same commit as that recipe). Counts include inactive objects.
string area = "";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "FAIL open Main3 first";
var set = Main3AreaSet.Load(); if (set == null) return "FAIL no Assets/Settings/Main3Areas.asset (run main3_areas_setup.cs)";
Main3AreaSet.Area a = null; if (area != "") { a = set.Find(area); if (a == null) return "FAIL no area " + area; }
var sb = new System.Text.StringBuilder(); int zeros = 0, diffs = 0;
var items = set.ItemsFor(a);
foreach (var it in items)
{
    int found = Main3AreaSet.Count(scene, it, out bool missingRoot);
    string verdict = found == 0 ? "ZERO" : it.expected >= 0 && found != it.expected ? "DIFF" : "ok";
    if (found == 0) zeros++; else if (verdict == "DIFF") diffs++;
    sb.Append(verdict + " " + it.label + ": expected " + (it.expected >= 0 ? it.expected.ToString() : "?") + ", found " + found + (missingRoot ? " (no " + it.path + ")" : "") + "\n");
}
if (items.Count == 0) sb.Append("no inventory items for this area\n");
return (zeros == 0 && diffs == 0 ? "ALL PASS" : zeros > 0 ? "FAIL " + zeros + " zero" : "FAIL " + diffs + " differ") + " | inventory" + (area != "" ? " " + area : "") + "\n" + sb;
