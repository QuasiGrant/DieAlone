if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var sb = new System.Text.StringBuilder();

// ---- Flames: additive transparent copy of the pack material, on every flame system in the scene.
var fp = UnityEngine.Object.FindFirstObjectByType<FirePit>();
UnityEngine.Material src = null;
foreach (var ps in fp.GetComponentsInChildren<UnityEngine.ParticleSystem>(true)) if (ps.name.StartsWith("FX_Flames")) src = ps.GetComponent<UnityEngine.ParticleSystemRenderer>().sharedMaterial;
string flamePath = "Assets/Materials/Flames_Additive.mat";
var flames = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(flamePath);
if (flames == null) { flames = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Particles/Unlit")); UnityEditor.AssetDatabase.CreateAsset(flames, flamePath); }
flames.SetTexture("_BaseMap", src.GetTexture("_BaseMap"));
flames.SetColor("_BaseColor", new UnityEngine.Color(1f, 0.85f, 0.6f, 1f));
flames.SetFloat("_Surface", 1f); flames.SetFloat("_Blend", 2f); flames.SetFloat("_ZWrite", 0f);
flames.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha); flames.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
flames.SetFloat("_SrcBlendAlpha", (float)UnityEngine.Rendering.BlendMode.One); flames.SetFloat("_DstBlendAlpha", (float)UnityEngine.Rendering.BlendMode.One);
flames.SetOverrideTag("RenderType", "Transparent"); flames.renderQueue = 3000;
flames.EnableKeyword("_SURFACE_TYPE_TRANSPARENT"); flames.DisableKeyword("_ALPHABLEND_ON"); flames.DisableKeyword("_ALPHAPREMULTIPLY_ON"); flames.DisableKeyword("_ALPHAMODULATE_ON");
flames.EnableKeyword("_FLIPBOOKBLENDING_OFF");
UnityEditor.EditorUtility.SetDirty(flames);
int flameCount = 0;
foreach (var ps in UnityEngine.Object.FindObjectsByType<UnityEngine.ParticleSystem>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var r = ps.GetComponent<UnityEngine.ParticleSystemRenderer>();
    if (r.sharedMaterial != null && r.sharedMaterial.name == "CS_Flames") { r.sharedMaterial = flames; flameCount++; }
}
sb.Append("flames swapped=" + flameCount + " ");

// ---- Cliff rock layer: darker and finer so the face reads as stone, not mud.
var rock = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_Rock.terrainlayer");
rock.tileSize = new UnityEngine.Vector2(3f, 3f); rock.diffuseRemapMax = new UnityEngine.Vector4(0.3f, 0.28f, 0.27f, 1f); rock.diffuseRemapMin = new UnityEngine.Vector4(0.02f, 0.02f, 0.02f, 0f);
UnityEditor.EditorUtility.SetDirty(rock);

// ---- Boundary line at x 0: a row of trees on each side straddles it.
var main = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var valley = UnityEngine.GameObject.Find("ValleyTerrain").GetComponent<UnityEngine.Terrain>();
var rng = new System.Random(41);
int mainAdded = 0, valleyAdded = 0;
{
    var d = main.terrainData; float size = d.size.x; int first = -1; for (int i = 0; i < d.treePrototypes.Length; i++) if (d.treePrototypes[i].prefab.name == "Valley_Pine1") first = i;
    var list = new System.Collections.Generic.List<UnityEngine.TreeInstance>(d.treeInstances);
    for (float z = 2f; z < 398f; z += 4.5f) { float x = 0.6f + (float)rng.NextDouble() * 1.8f, zz = z + (float)(rng.NextDouble() - 0.5) * 3f; float hs = 1.3f + (float)rng.NextDouble() * 0.7f; list.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / size, 0f, zz / size), prototypeIndex = first + rng.Next(0, 3), heightScale = hs, widthScale = 1f, rotation = (float)(rng.NextDouble() * 6.283), color = new UnityEngine.Color(0.7f, 0.65f, 0.6f), lightmapColor = UnityEngine.Color.white }); mainAdded++; }
    d.SetTreeInstances(list.ToArray(), true); UnityEditor.EditorUtility.SetDirty(d);
}
{
    var d = valley.terrainData; float sx = d.size.x, sz = d.size.z;
    var list = new System.Collections.Generic.List<UnityEngine.TreeInstance>(d.treeInstances);
    for (float z = 160f; z < 560f; z += 4.5f) { float xl = 240f - 0.6f - (float)rng.NextDouble() * 1.8f, zl = z + (float)(rng.NextDouble() - 0.5) * 3f; float hs = 1.3f + (float)rng.NextDouble() * 0.7f; list.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(xl / sx, 0f, zl / sz), prototypeIndex = rng.Next(0, 3), heightScale = hs, widthScale = 1f, rotation = (float)(rng.NextDouble() * 6.283), color = new UnityEngine.Color(0.7f, 0.65f, 0.6f), lightmapColor = UnityEngine.Color.white }); valleyAdded++; }
    d.SetTreeInstances(list.ToArray(), true); UnityEditor.EditorUtility.SetDirty(d);
}
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " " + sb + "seamTrees main=" + mainAdded + " valley=" + valleyAdded;
