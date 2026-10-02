// Main3 areas (PLAN 8.21 to 8.30; Wren 2026-10-02): writes Assets/Settings/Main3Areas.asset (Assets/Editor/Main3AreaSet.cs), the data
// the area check (main3_area_check.cs), the inventory check and main3_review_capture.sh --area read. Edit mode; rerunnable (rewrites the
// asset from this file, so change areas here, not in the Inspector). Bounds are Rook's drafts from Valley.md 1 and Main3_map.svg (warp
// and place positions as built); Sable confirms an area's bounds and writes its deck list when the area starts, until then
// deckListWritten is false and the check prints "no deck list". Camp's deck list is CampLayout.md draft 2 section 5.
// A place or deck point with y = G (-999) stands on the ground: the check uses the ground under it plus 1 m.
const float G = -999f;
const string path = "Assets/Settings/Main3Areas.asset";
var set = UnityEditor.AssetDatabase.LoadAssetAtPath<Main3AreaSet>(path);
if (set == null) { set = UnityEngine.ScriptableObject.CreateInstance<Main3AreaSet>(); UnityEditor.AssetDatabase.CreateAsset(set, path); }
UnityEngine.Rect R(float x0, float z0, float x1, float z1) => new UnityEngine.Rect(x0, z0, x1 - x0, z1 - z0);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
Main3AreaSet.Place P(string l, float x, float y, float z) => new Main3AreaSet.Place { label = l, point = V(x, y, z) };
Main3AreaSet.DeckTarget D(string l, float x, float y, float z, string cover = "") => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), treeCoverPath = cover };
Main3AreaSet.InventoryItem I(string l, string p, string k, string n, int e) => new Main3AreaSet.InventoryItem { label = l, path = p, kind = k, name = n, expected = e };
// ground Valley.md 8 closes with thicket (as main3_reach_check_8_16a.cs)
set.closedZones = new[] { R(346f, -10f, 395f, 99f), R(141f, -10f, 248f, 25f) };
// inventory (Vesper, Docs/Review/2026-10-02-Meeting/Vesper.md 4): expected counts as built by the runner on 2026-10-02
set.inventory = new[] {
    I("Ridge fire cards, night", "Ward/StandInFire", "quads", "RidgeFlames", 132), I("Ridge fire cards, day", "Ward/StandInFire", "quads", "RidgeFlamesDay", 68),
    I("Valley fire cards, night", "Ward/StandInFire", "quads", "ValleyFlames", 297), I("Valley fire cards, day", "Ward/StandInFire", "quads", "ValleyFlamesDay", 167),
    I("Smoke sheet, day one", "Ward/SmokeSheet", "quads", "Body", 689), I("Smoke lid, night", "Ward/SmokeSheet", "quads", "Lid", 171),
    I("Smoke columns, day two", "Ward/StandInFire/SmokeColumns", "quads", "", 128), I("Ward stones", "Ward/Stones", "renderers", "", 3),
    I("Camp 3 tent", "Campsites/Camp_3/Dressing", "renderers", "CS_Tent", 8), I("Camp 3 fire", "Campsites/Camp_3/Dressing/Fire", "renderers", "", 15),
    I("Camp 3 easel", "Campsites/Camp_3/Dressing/Easel", "renderers", "", 13), I("North ruin", "Places/NorthRuin", "renderers", "", 44),
    I("Cave lights", "Cave", "lights", "", 5), I("Lanterns and lamps", "", "practicals", "", 28),
};
string[] fireItems = { "Ridge fire cards, night", "Ridge fire cards, day", "Valley fire cards, night", "Valley fire cards, day", "Smoke sheet, day one", "Smoke lid, night", "Smoke columns, day two", "Ward stones" };
var none = new Main3AreaSet.DeckTarget[0];
var camp = new Main3AreaSet.Area { id = "camp", task = "8.21", title = "Keeper's camp, cabin and tower", bounds = new[] { R(138f, 130f, 208f, 192f) },
    warps = new[] { "Keepers_Camp", "Cabin", "Tower_Deck" },
    places = new[] { P("Cabin door", 178f, G, 165f), P("Fire pit", 172f, G, 163f), P("Generator", 183f, G, 173f), P("Privy", 176.5f, G, 178f), P("Woodpile", 182.5f, G, 168f), P("Tower stair foot", 164f, G, 160f), P("Forage patch B", 141f, G, 167f) },
    inventory = new[] { "Lanterns and lamps" }, deckListWritten = true,
    deckSee = new[] { D("cabin roof", 178f, 19.5f, 168f), D("fire pit", 172f, 15.6f, 163f), D("woodpile", 182f, 16.5f, 168f), D("lake, mid water", 190f, -5.4f, 60f),
        D("Camp 1 spar top", 284f, 29f, 240f), D("Camp 2 stack top", 292f, 24.2f, 108f), D("Camp 3 Snag line (top)", 96f, 54f, 146.5f), D("office west door", 341f, 4.2f, 199f),
        D("cat step (boathouse)", 240f, -3.3f, 56f), D("verge tree", 419f, 25f, 139f), D("lot centre", 358f, 3.1f, 170f), D("highway", 430f, G, 185f) },
    deckHide = new[] { D("Ward stones", -3f, 66f, 224f, "Ward/Stones"), D("rune post", 57f, 36f, 246f, "Ward/Climb/RunePost"), D("north ruin", 172f, 4f, 281f, "Places/NorthRuin"),
        D("far fire front, z 40", -240f, 105f, 40f), D("far fire front, z 170", -240f, 105f, 170f), D("far fire front, z 300", -240f, 105f, 300f), D("day-one sheet top", -500f, 110f, 170f), D("cave mouth", 52f, -4f, 37f) } };
var areas = new System.Collections.Generic.List<Main3AreaSet.Area> {
    camp,
    new Main3AreaSet.Area { id = "front", task = "8.22", title = "Front zone", bounds = new[] { R(318f, 140f, 448f, 270f) }, warps = new[] { "Office", "Store", "Gate_Booth", "Trailhead_T", "Lot_Highway" },
        places = new[] { P("Office west door", 341f, G, 199f), P("Store door", 366f, G, 194f), P("Gate booth", 392f, G, 176f), P("Lot", 358f, G, 170f), P("The T", 337f, G, 170f), P("First sight of the lot", 327f, G, 168f) },
        inventory = new string[0], deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "lake", task = "8.23", title = "Lake", bounds = new[] { R(150f, 25f, 262f, 130f) }, warps = new[] { "Lake_Pump", "Lake_Boathouse" },
        places = new[] { P("Pump", 190f, G, 97f), P("Boathouse", 244f, G, 52f), P("Cat step", 233f, G, 62f), P("Dock end", 190f, G, 88f), P("Overturned rowboat", 226f, G, 87f), P("Water tank", 186f, G, 125f) },
        inventory = new string[0], deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "camp1", task = "8.24", title = "Camp 1 and the north loop", bounds = new[] { R(150f, 195f, 300f, 305f) }, warps = new[] { "Camp_1", "North_Loop_Ruin" },
        places = new[] { P("Camp 1", 282f, G, 238f), P("Latrine shed", 264f, G, 207f), P("North ruin", 172f, G, 281f) },
        inventory = new[] { "North ruin" }, deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "camp2", task = "8.25", title = "Camp 2", bounds = new[] { R(255f, 55f, 335f, 165f) }, warps = new[] { "Camp_2", "Camp_2_Top" },
        places = new[] { P("Camp 2", 292f, G, 104f), P("Stack top", 294.5f, 24f, 107.2f), P("Payphone", 298.5f, G, 99f), P("Food lockers", 317f, G, 136f), P("Phone pole", 273f, G, 72f) },
        inventory = new string[0], deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "camp3", task = "8.26", title = "Camp 3 and the west trails", bounds = new[] { R(55f, 60f, 160f, 180f) }, warps = new[] { "Camp_3", "Camp_3_Rim", "Junction_W1" },
        places = new[] { P("Camp 3", 78f, G, 146f), P("Easel", 83.5f, G, 149f), P("Rim", 100f, G, 148f), P("Camper trailer", 89f, G, 112f), P("Stepping stones", 132f, G, 73f), P("Footbridge", 115f, G, 85f), P("Washed-out truck", 156f, G, 92f) },
        inventory = new[] { "Camp 3 tent", "Camp 3 fire", "Camp 3 easel" }, deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "cave", task = "8.27", title = "Cave and ravine", bounds = new[] { R(20f, -20f, 120f, 65f), R(365f, 215f, 405f, 265f) }, warps = new[] { "Cave_Mouth", "Cave_Chamber", "Closed_Campground" },
        places = new[] { P("Cave mouth", 52f, G, 40f), P("Chamber", 80f, -18f, 12f), P("Side room", 94f, -18f, 12f), P("Rope handrail", 107f, G, 58f), P("Coloured bulbs", 83f, G, 49f), P("Closed campground", 385f, G, 232f) },
        inventory = new[] { "Cave lights" }, deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "burn", task = "8.28", title = "Old burn and forage", bounds = new[] { R(205f, 140f, 335f, 200f) }, warps = new[] { "Old_Burn", "Junction_Jg" },
        places = new[] { P("Old burn", 240f, G, 160f), P("Forage patch A", 240f, G, 163f), P("Jg", 262f, G, 170f) },
        inventory = new string[0], deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "ward", task = "8.29", title = "Ward path and ledge", bounds = new[] { R(-70f, 185f, 115f, 305f) }, warps = new[] { "Junction_J", "Ward_Stair", "Ward_Lookout", "Ward" },
        places = new[] { P("J", 106f, G, 203f), P("Plank bridge", 105f, G, 205f), P("Ward stair", 35f, G, 248f), P("Lookout", 29f, G, 261f), P("Ward stones", -3f, G, 230f), P("Path end", -9f, G, 246f) },
        inventory = fireItems, deckSee = none, deckHide = none },
};
// 8.30, the whole map: every warp, place and inventory item
var allWarps = new System.Collections.Generic.List<string>(); var allPlaces = new System.Collections.Generic.List<Main3AreaSet.Place>();
foreach (var a in areas) { foreach (var w in a.warps) if (!allWarps.Contains(w)) allWarps.Add(w); allPlaces.AddRange(a.places); }
var allItems = new System.Collections.Generic.List<string>(); foreach (var it in set.inventory) allItems.Add(it.label);
areas.Add(new Main3AreaSet.Area { id = "whole", task = "8.30", title = "Whole map", bounds = new[] { R(-80f, -80f, 460f, 460f) }, warps = allWarps.ToArray(), places = allPlaces.ToArray(), inventory = allItems.ToArray(), deckSee = none, deckHide = none });
set.areas = areas.ToArray();
UnityEditor.EditorUtility.SetDirty(set); UnityEditor.AssetDatabase.SaveAssets();
var ids = new System.Collections.Generic.List<string>(); foreach (var a in set.areas) ids.Add(a.id + " (" + a.task + ", " + a.warps.Length + " warps, " + a.places.Length + " places, deck list " + (a.deckListWritten ? "yes" : "no") + ")");
return "saved=True | areas " + set.areas.Length + ": " + string.Join(", ", ids) + " | inventory items " + set.inventory.Length;
