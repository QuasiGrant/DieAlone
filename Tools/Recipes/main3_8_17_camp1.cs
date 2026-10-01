// Main3 task 8.17, Camp 1 (Valley.md rev 11 1.3 and 6 M9; Check1_Story: the resident's plans laid out as real kit; a family camp whose
// adult is never there). Run after 8.16 in Main3, edit mode; rerunnable. Replaces 8.5's gray spar, tent, lumber and cookfire; keeps the
// three workbenches (owned tables since 8.15). Three clusters round the spar (Style 5.6): the kid's table in the middle beside the spar
// with the dream-game kit (pots, cans, a cooler lid), the grown-up workbenches at the edge with tools left mid-job, and the family tent
// with the cold cookfire, the adult's empty chair and untouched pack. The spar (the tower's landmark, 8.9 targets its top at 29 and a
// point at 20) is stacked owned logs, lashed with rope, bulbs on a line to a stake.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var c1 = kit.Root("Campsites").transform.Find("Camp_1"); if (c1 == null) return "no Campsites/Camp_1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float cx = 282f, cz = 238f, sparX = 284f, sparZ = 240f,   // the spar 2.8 m north-east of the camp centre, off the end of the Camp 1 to J trail (8.17 gate, Marlow 6; 8.9 aims at it here)
      sparTop = 24f, logLen = 2f, logOverlap = 0.3f, logGirth = 0.37f, sparGirth = 0.46f, stakeX = 268f, bulbDrop = 0.15f; const int bulbs = 11;
float gy = kit.H(cx, cz);
// 8.5's gray pieces go; the resident capsule becomes a bare marker
foreach (var n in new[] { "Spar", "Tent", "LumberPile", "Cookfire" }) PlaceKit.Remove(c1.Find(n));
PlaceKit.MarkerOnly(c1.Find("Resident/Resident_Camp1_Spot"));
var d = kit.Fresh("Dressing", c1, V(cx, gy, cz), 0f);
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
var bulbGlow = kit.Glow("Places_BulbGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity);

// ---- the spar: owned logs end to end, each turned a little, rope at every joint; one capsule to stop the player
var spar = kit.Group("Spar", d, V(sparX, kit.H(sparX, sparZ), sparZ), 0f);
for (int i = 0; i < sparTop / logLen; i++)
{
    // the pack log lies along its local x (2 m): stood up by a quarter turn about z, stretched to logLen plus an overlap, turned a little per piece
    var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", spar);
    if (lg != null)
    {
        PlaceKit.StripColliders(lg); lg.transform.localRotation = UnityEngine.Quaternion.Euler(0f, i * 37f, 90f); lg.transform.localScale = V((logLen + logOverlap) / logLen, sparGirth / logGirth, sparGirth / logGirth);
        lg.transform.localPosition = UnityEngine.Vector3.zero; var b = PlaceKit.LocalBounds(lg, spar); lg.transform.localPosition = V(-b.center.x, i * logLen - logOverlap - b.min.y, -b.center.z);
    }
    if (i > 0) kit.Fill(PlaceKit.FT + "Rope", spar, V(0f, i * logLen - 0.25f, 0f), V(sparGirth + 0.12f, 0.35f, sparGirth + 0.12f), i * 50f);
}
var cap = spar.gameObject.AddComponent<UnityEngine.CapsuleCollider>(); cap.radius = sparGirth * 0.5f; cap.height = sparTop; cap.center = V(0f, sparTop * 0.5f, 0f);
foreach (var a in new[] { 0f, 120f, 240f }) kit.Ground(PlaceKit.CI + "Props/CITW_Log_1", spar, sparX + UnityEngine.Mathf.Sin(a * UnityEngine.Mathf.Deg2Rad) * 1.2f, sparZ + UnityEngine.Mathf.Cos(a * UnityEngine.Mathf.Deg2Rad) * 1.2f, a, 1f, false, 0.1f, V(0f, 0f, 35f));   // foot braces
// the bulb line from the top down to a stake 14 m west
var top = V(sparX, spar.position.y + sparTop, sparZ); var stake = V(stakeX, kit.H(stakeX, sparZ) + 1f, sparZ);
kit.Ground(PlaceKit.CS + "Wood/CS_Log_Large", spar, stakeX, sparZ, 0f, 1f, false, 0.4f, V(0f, 0f, 90f));
var line = kit.Slab("BulbLine", spar, spar.InverseTransformPoint((top + stake) * 0.5f), V(0.03f, 0.03f, UnityEngine.Vector3.Distance(top, stake)), steel);
line.transform.rotation = UnityEngine.Quaternion.LookRotation(top - stake, UnityEngine.Vector3.up);
for (int i = 1; i <= bulbs; i++) { var p = UnityEngine.Vector3.Lerp(stake, top, i / (bulbs + 1f)); var bl = kit.Slab("Bulb", spar, spar.InverseTransformPoint(p) - V(0f, bulbDrop, 0f), V(0.16f, 0.2f, 0.16f), bulbGlow); bl.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; }

// ---- cluster 1: the kid's table in the middle, beside the spar (M9), set for a dream game; small shoes, a bear, crayons
var kid = kit.Group("KidTable", d, V(cx + 3.2f, kit.H(cx + 3.2f, cz - 1.5f), cz - 1.5f), 25f);
var table = kit.On(PlaceKit.CS + "Tables/CS_Table_Small_Modern_1", kid, V(0f, 0f, 0f), 0f, 1.3f, true, null, true);
float tTop = table != null ? PlaceKit.LocalBounds(table, kid).max.y : 0.78f;
foreach (var s in new[] { (-0.8f, 0f, 90f), (0.8f, 0.1f, -90f) }) kit.On(PlaceKit.CS + "CS_Log_Stool_1", kid, V(s.Item1, 0f, s.Item2), s.Item3, 0.8f, true, null, true);
// 8.17 gate (Quill): the three dream games read on his table: four pots upturned in a row with a spoon, the ingredients in a row, and
// the lid for the word magnets on its own log stool; a drawing weighted by a cup (Vesper: one thing mid-task)
string[] pots = { "Cookware/CS_Cookware_Pot_1", "Cookware/CS_Cookware_Pot_2", "Cookware/CS_Cookware_Pot_3", "Cookware/CS_Cookware_Pot_4" };
for (int i = 0; i < pots.Length; i++) kit.On(PlaceKit.CS + pots[i], kid, V(-0.33f + i * 0.22f, tTop, 0.2f), i * 30f, 0.6f, false, V(180f, 0f, 0f), true);
kit.On(PlaceKit.CS + "Tableware/CS_Tableware_Spoon_Wood", kid, V(0.45f, tTop, 0.2f), 80f, 1f, false, null, true);
string[] food = { "Food/CS_Food_Can_2", "Food/CS_Food_Bread_1_Half", "Food/CS_Food_Sausage_Raw_1", "Food/CS_Food_Marshmallow_Raw_1" };
for (int i = 0; i < food.Length; i++) kit.On(PlaceKit.CS + food[i], kid, V(-0.33f + i * 0.22f, tTop, -0.05f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Kitchen/Apple_Whole", kid, V(0.55f, tTop, -0.05f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Paper", kid, V(-0.1f, tTop, -0.3f), 12f, 0.5f, false, null, true);   // a drawing
kit.On(PlaceKit.CS + "Tableware/CS_Tableware_Mug_Metal_1", kid, V(-0.05f, tTop, -0.28f), 0f, 1f, false, null, true);   // weighting it
kit.On(PlaceKit.CE + "Decoration_Home/Pastel_Crayons_Box", kid, V(0.3f, tTop, -0.32f), 10f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Toy_Teddy_Bear", kid, V(-0.85f, 0.45f, 0f), 80f, 1f, false, null, true);   // on its stool, waiting
var lidStool = kit.On(PlaceKit.CS + "CS_Log_Stool_2", kid, V(0f, 0f, -1.1f), 0f, 0.8f, true, null, true); float lidY = lidStool != null ? PlaceKit.LocalBounds(lidStool, kid).max.y : 0.36f;
kit.On(PlaceKit.CE + "Decoration_Out/Drum_Grey_Lid", kid, V(0f, lidY, -1.1f), 0f, 1f, false, null, true);   // the lid for the word magnets
kit.On(PlaceKit.CE + "Decoration_Home/Toy_Wooden_Car", kid, V(1.2f, 0f, -0.9f), 140f, 1f, false, null, true);
// 8.17 gate (Marlow 6): the Camp 1 to J trail ran through 8.5's west workbench at (270, 245); it moves 4.6 m off the line
foreach (UnityEngine.Transform wb in c1) if (wb.name == "Workbench" && UnityEngine.Vector2.Distance(new UnityEngine.Vector2(wb.position.x, wb.position.z), new UnityEngine.Vector2(270f, 245f)) < 0.5f) wb.position = V(266.5f, kit.H(266.5f, 243f), 243f);
// ---- cluster 2: the workbenches at the edge (8.5, kept), tools left mid-job on them, lumber and a sawhorse log
foreach (UnityEngine.Transform wb in c1)
{
    if (wb.name != "Workbench") continue; var w = kit.Fresh("Kit", wb, wb.position, wb.eulerAngles.y);
    kit.On(PlaceKit.FT + "Saw", w, V(-0.5f, 0.9f, 0f), 80f, 1f, false, null, true);
    kit.On(PlaceKit.FT + "Hammer", w, V(0.3f, 0.9f, 0.1f), 20f, 1f, false, null, true);
    kit.On(PlaceKit.FT + "ToolBoxOpened", w, V(0.8f, 0.9f, -0.1f), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CC + "Props/C_Plank_B_Thick", w, V(0f, 0.9f, 0.2f), 5f, 1f, false, null, true);
}
var lumber = kit.Group("Lumber", d, V(300f, kit.H(300f, 240f), 240f), 70f);
for (int i = 0; i < 6; i++) kit.On(PlaceKit.CC + "Props/C_Plank_A_Thick", lumber, V((i % 3) * 0.32f - 0.32f, 0.06f * (i / 3), 0f), 0f, 1.6f, false, null, true);
kit.On(PlaceKit.CS + "Wood/CS_Log_Large_Long", lumber, V(0f, 0f, 2.6f), 90f, 1f, true, null, true);
kit.On(PlaceKit.FT + "Axe", lumber, V(0.3f, 0.37f, 2.6f), 40f, 1f, false, V(0f, 0f, 70f), true);
// ---- cluster 3: the family tent, the cold cookfire, the adult's empty chair and untouched pack
// 8.17 gate (Vesper, Camp 1 D: the tent was lost in the grass at the frame edge): the tent on the open ground about 9 m in front of
// the Camp 1 warp (268, 226), 5 m from the cookfire, a sleeping bag half out of its door, the kid's shoes paired at its step
const float tentX = 271f, tentZ = 234.5f, tentYaw = 200f;   // 9 m from the warp
var tent = kit.Ground(PlaceKit.CS + "Tents/Large Modern/Presets/CS_Tent_Large_Modern_Preset_1", d, tentX, tentZ, tentYaw, 1f, true);
var step = kit.Group("TentStep", d, V(tentX, kit.H(tentX, tentZ), tentZ), tentYaw);
kit.On(PlaceKit.CS + "Beds/CS_Bedroll_Modern_1", step, V(0.3f, 0f, -2.6f), 10f, 1f, false, null, true);   // half out of the door
kit.On(PlaceKit.CE + "Decoration_Home/Kids_Shoes", step, V(-0.9f, 0f, -2.7f), 0f, 1f, false, null, true);
var fire = kit.Group("Cookfire", d, V(276f, kit.H(276f, 232f), 232f), 0f);
kit.On(PlaceKit.CS + "CS_Campfire_1", fire, V(0f, 0f, 0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Wood/CS_Firewood_Logs_Burnt", fire, V(0f, 0.05f, 0f), 30f, 1f, false, null, true);
kit.On(PlaceKit.CS + "CS_Campfire_Tripod_Metal", fire, V(0f, 0f, 0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Cookware/CS_Cookware_Kettle_2", fire, V(1.1f, 0f, 0.4f), 60f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Cookware/CS_Cookware_Pot_2", fire, V(0f, 0.1f, 0f), 0f, 1f, false, null, true);   // a cold pot on it
kit.On(PlaceKit.CS + "CS_Chair_1", fire, V(-1.6f, 0f, 1.4f), 130f, 1f, true, null, true);   // the adult's chair, empty, facing the fire
kit.On(PlaceKit.CS + "CS_Chair_3", fire, V(1.7f, 0f, 1.5f), 220f, 0.8f, true, null, true);
kit.On(PlaceKit.CS + "Bags/CS_Backpack_Modern_2", fire, V(-2.3f, 0f, 2.3f), 10f, 1f, false, null, true);   // packed, never opened
kit.On(PlaceKit.CS + "Beds/CS_Bedroll_Modern_Rolled_1", fire, V(-2.6f, 0f, 1.7f), 80f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Drinks/CS_Drink_Thermos_1", fire, V(-1.3f, 0f, 2.0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Food/CS_Food_Marshmallow_Package_Open", fire, V(1.3f, 0f, 2.1f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "CS_Lantern_Modern", fire, V(-1.6f, 0f, 0.6f), 0f, 1f, false, null, true);

// trampled ground round the tent, the cookfire and the kid's table (Vesper: grass hid the base of everything)
const float tentClear = 4f, clusterClear = 3f;
int grassCut = 0; foreach (var cc in new[] { (V(tentX, 0f, tentZ), tentClear), (fire.position, clusterClear), (kid.position, clusterClear) }) grassCut += kit.ClearDetail(cc.Item1, cc.Item2) + kit.ClearCover(cc.Item1, cc.Item2);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " camp 1: grass cells cleared " + grassCut + ", spar top " + (gy + sparTop).ToString("F1") + ", renderers " + d.GetComponentsInChildren<UnityEngine.Renderer>().Length + " | " + kit.Report();
