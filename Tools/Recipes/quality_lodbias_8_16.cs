// 8.16 gate (Vesper, LightingOptions.md 7; Wren): lodBias on the PC quality level from 2 to pcLodBias (never under lodBiasFloor), so the
// BK trees drop to their lighter LODs nearer. Edit mode; sets the level through QualitySettings and restores the level that was active.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string level = "PC"; const float pcLodBias = 1.25f, lodBiasFloor = 1f;
int was = UnityEngine.QualitySettings.GetQualityLevel(), pc = System.Array.IndexOf(UnityEngine.QualitySettings.names, level); if (pc < 0) return "no quality level " + level;
UnityEngine.QualitySettings.SetQualityLevel(pc, false); UnityEngine.QualitySettings.lodBias = UnityEngine.Mathf.Max(lodBiasFloor, pcLodBias); float now = UnityEngine.QualitySettings.lodBias;
UnityEngine.QualitySettings.SetQualityLevel(was, false);
UnityEditor.AssetDatabase.SaveAssets();
return "PC lodBias " + now + " (active level " + UnityEngine.QualitySettings.names[was] + ")";
