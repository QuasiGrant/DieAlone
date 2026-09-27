// Proof that the dev menu is compiled out of the release build: read the built
// game assembly with Mono.Cecil and list what the DevMenu type contains.
string dll = System.IO.Path.GetFullPath("Build/Release/DieAlone_Data/Managed/Assembly-CSharp.dll");
if (!System.IO.File.Exists(dll)) return "no built assembly at " + dll;
var sb = new System.Text.StringBuilder();
using (var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(dll))
{
    var type = asm.MainModule.GetType("DevMenu");
    if (type == null) { sb.Append("DevMenu type missing from release assembly"); }
    else
    {
        sb.Append("release DevMenu: methods=");
        foreach (var m in type.Methods) sb.Append(m.Name + " ");
        sb.Append("| fields=" + type.Fields.Count + " | hasUpdate=" + type.Methods.Any(m => m.Name == "Update") + " hasBuild=" + type.Methods.Any(m => m.Name == "Build"));
    }
    // Sanity: a type that should be fully present.
    var pause = asm.MainModule.GetType("GamePause");
    sb.Append(" || GamePause methods=" + (pause != null ? pause.Methods.Count : -1));
}
// Compare with the Editor's own compiled DevMenu (dev context) for contrast.
var editorType = typeof(DevMenu);
sb.Append(" || editor DevMenu: hasUpdate=" + (editorType.GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) != null) + " hasBuild=" + (editorType.GetMethod("Build", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) != null));
sb.Append(" || exe exists=" + System.IO.File.Exists(System.IO.Path.GetFullPath("Build/Release/DieAlone.exe")));
return sb.ToString();
