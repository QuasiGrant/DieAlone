// Task 7.4: import the bought packs from the local Asset Store cache, one per run.
// Each run imports the first pack whose folder is not in the project yet, non-interactive.
// Run again after the import settles (editor_status ready, no compile) until it reports "all imported".
var cache = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Unity", "Asset Store-5.x");
var packs = new[]
{
    new[] { "PolyPlex/3D ModelsPropsExterior/Modular Chain Link Fence.unitypackage", "Assets/Modular Chain Link Fence" },
    new[] { "Rosemary3d/3D ModelsPropsTools/PSX Rural Farm Tools Pack 32 Lowpoly Tool Props.unitypackage", "Assets/PSX Farm Tools Pack" },
    new[] { "Rosemary3d/3D ModelsPropsTools/PSX Supplies Pack 46 Lowpoly Supplies Props.unitypackage", "Assets/PSX Supplies Pack" },
    new[] { "Revolving Pizza Games/3D ModelsEnvironments/Catacombs - Retro Style Modular Environment Pack.unitypackage", "Assets/Revolving Pizza Games/Catacombs" },
    new[] { "FANNC/3D ModelsEnvironmentsUrban/PSX Edition - Modular Parking Lot.unitypackage", "Assets/PSX Edition - Modular Parking Lot" },
    new[] { "NatureManufacture/Particle SystemsFire/Fire Smoke - Dynamic Nature.unitypackage", "Assets/NatureManufacture Assets" },
    new[] { "Effigy GameWorks/3D ModelsProps/Menhir Stone Circle.unitypackage", "Assets/Effigy GameWorks" },
    new[] { "BK/3D ModelsEnvironments/Pure Nature 2 Redwood.unitypackage", "Assets/BK" },
};
foreach (var p in packs)
{
    if (UnityEditor.AssetDatabase.IsValidFolder(p[1])) continue;
    var file = System.IO.Path.Combine(cache, p[0]);
    if (!System.IO.File.Exists(file)) return "MISSING in cache: " + file;
    UnityEditor.AssetDatabase.ImportPackage(file, false);
    return "importing " + p[0] + " -> " + p[1];
}
return "all imported";
