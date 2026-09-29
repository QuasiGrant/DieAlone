// Task 7.4: Fire & Smoke - Dynamic Nature ships Built-in materials and shaders. Its readme says to
// import the nested URP 17.3 support package for Unity 6.3, which overwrites them with URP versions.
// Run once after import_packs_7_4.cs has brought the pack in.
var pkg = "Assets/NatureManufacture Assets/HD and URP Support Packs Smoke and Fire/URP 17.3 Unity 6.3 Fire and Smoke.unitypackage";
if (!System.IO.File.Exists(pkg)) return "MISSING " + pkg;
UnityEditor.AssetDatabase.ImportPackage(System.IO.Path.GetFullPath(pkg), false);
return "importing " + pkg;
