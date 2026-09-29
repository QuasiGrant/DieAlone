// Release-build check for dev-only code without a player build (a player build rewrites
// PC_RPAsset, URP global settings, preloadedAssets and recreates Assets/DefaultVolumeProfile.asset).
// Compiles the player scripts for Windows with release defines (no DEVELOPMENT_BUILD, no UNITY_EDITOR)
// into Temp/ReleaseScripts, then lists the declared members of each named type in DieAlone.dll.
// Edit TypeNames to the dev-only classes being checked. Touches no assets.
if (UnityEditor.EditorApplication.isPlaying) return "edit mode only";
string[] TypeNames = { "DevMenu", "LookPreview" };
const string OutDir = "Temp/ReleaseScripts";
var settings = new UnityEditor.Build.Player.ScriptCompilationSettings
{
    target = UnityEditor.BuildTarget.StandaloneWindows64,
    group = UnityEditor.BuildTargetGroup.Standalone,
    options = UnityEditor.Build.Player.ScriptCompilationOptions.None,
};
var result = UnityEditor.Build.Player.PlayerBuildInterface.CompilePlayerScripts(settings, OutDir);
var log = new System.Text.StringBuilder();
log.AppendLine("assemblies: " + result.assemblies.Count);
var dll = System.IO.Path.GetFullPath(System.IO.Path.Combine(OutDir, "DieAlone.dll"));
var asm = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(dll));
const System.Reflection.BindingFlags All = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
    | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly;
foreach (var name in TypeNames)
{
    System.Type t = null;
    foreach (var candidate in asm.GetTypes()) if (candidate.Name == name) t = candidate;
    if (t == null) { log.AppendLine(name + ": not found"); continue; }
    var members = new System.Collections.Generic.List<string>();
    foreach (var m in t.GetMembers(All)) members.Add(m.Name);
    foreach (var n in t.GetNestedTypes(All)) members.Add("nested " + n.Name);
    log.AppendLine(name + ": " + string.Join(", ", members));
}
return log.ToString();
