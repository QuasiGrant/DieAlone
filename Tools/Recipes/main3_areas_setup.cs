// Main3 areas (PLAN 8.21 to 8.30; Wren 2026-10-02): writes Assets/Settings/Main3Areas.asset (Assets/Editor/Main3AreaSet.cs), the data
// the area check (main3_area_check.cs), the inventory check and main3_review_capture.sh --area read. Edit mode; rerunnable (rewrites the
// asset from this file, so change areas here, not in the Inspector). Bounds are Rook's drafts from Valley.md 1 and Main3_map.svg (warp
// and place positions as built); Sable confirms an area's bounds and writes its deck list when the area starts, until then
// deckListWritten is false and the check prints "no deck list". Camp's deck list is CampLayout.md draft 2 section 5.
// Wren 2026-10-02: the woodpile is off the deck must-see list (the cabin roof hides it); the Ward stones are proven by land rays (W-1),
// so the pixel check stays only where trees are the cover (the ruin) or the rune post screen.
// 8.22 (FrontLayout.md draft 2): the front area as the doc; the closed campground moves from the cave to the front (Wren 2026-10-02).
// 8.22 round 2: the lectern stands on the cab's east side (Wren 2026-10-02), on the walkway's north-east corner, so its reader stands at (167.3, 168.9) facing east.
// A place or deck point with y = G (-999) stands on the ground: the check uses the ground under it plus 1 m.
const float G = -999f;
const string path = "Assets/Settings/Main3Areas.asset";
var set = UnityEditor.AssetDatabase.LoadAssetAtPath<Main3AreaSet>(path);
if (set == null) { set = UnityEngine.ScriptableObject.CreateInstance<Main3AreaSet>(); UnityEditor.AssetDatabase.CreateAsset(set, path); }
UnityEngine.Rect R(float x0, float z0, float x1, float z1) => new UnityEngine.Rect(x0, z0, x1 - x0, z1 - z0);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
Main3AreaSet.Place P(string l, float x, float y, float z) => new Main3AreaSet.Place { label = l, point = V(x, y, z) };
Main3AreaSet.Place PO(string l, float x, float y, float z, float h, string obj) => new Main3AreaSet.Place { label = l, point = V(x, y, z), height = h, objectPath = obj };
Main3AreaSet.Interaction IA(string l, string p, float x, float y, float z) => new Main3AreaSet.Interaction { label = l, path = p, approach = V(x, y, z) };
Main3AreaSet.Frame FR(string l, UnityEngine.Vector3 eye, UnityEngine.Vector3 look) => new Main3AreaSet.Frame { label = l, eye = eye, look = look };
Main3AreaSet.DeckTarget D(string l, float x, float y, float z, string cover = "") => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), treeCoverPath = cover };
Main3AreaSet.DeckTarget DH(string l, float x, float y, float z, string obj) => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), hard = true, objectPath = obj };   // hard must-see, every mesh (8.22)
Main3AreaSet.DeckTarget DL(string l, float x, float y, float z) => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), loose = true };   // loose must-see, reported only (8.22)
Main3AreaSet.WalkLine WL(string l, params UnityEngine.Vector3[] pts) => new Main3AreaSet.WalkLine { label = l, points = pts };
Main3AreaSet.InventoryItem I(string l, string p, string k, string n, int e) => new Main3AreaSet.InventoryItem { label = l, path = p, kind = k, name = n, expected = e };
// ground Valley.md 8 closes with thicket (as main3_reach_check_8_16a.cs)
set.closedZones = new[] { R(346f, -10f, 395f, 99f), R(141f, -10f, 248f, 25f) };
// stops (Main3AreaSet.stopRoots), set here so the asset follows this file (the asset had kept its first list, without the lake's):
// 8.23 round 2 adds the dock rails, their RailStops and the slip rails (Marlow 823 finding 1)
set.stopRoots = new[] { "FrontZone/BrushBands", "FrontZone/ShiftWalls", "FrontZone/Gate/PlayerBlocker", "Ground815/Stops", "Fence", "Lake/WadeLimit", "Lake/Boathouse/Layout823/StakeLine",
    "Lake/Dock/Rail", "Lake/Dock/Layout823/RailStops", "Lake/Boathouse/Layout823/SlipRails" };
// closed water (8.23 round 2): the lake inside the wade ring, the stake line and the house skirts, filled from mid water
set.closedFills = new[] { new Main3AreaSet.ClosedFill { label = "lake", seed = new UnityEngine.Vector2(190f, 60f), surface = -5.5f, within = R(133f, 25f, 262f, 130f), cell = 0.5f, body = 0.35f } };
// inventory (Vesper, Docs/Review/2026-10-02-Meeting/Vesper.md 4): expected counts as built by the runner on 2026-10-02
set.inventory = new[] {
    I("Ridge fire cards, night", "Ward/StandInFire", "quads", "RidgeFlames", 132), I("Ridge fire cards, day", "Ward/StandInFire", "quads", "RidgeFlamesDay", 68),
    I("Valley fire cards, night", "Ward/StandInFire", "quads", "ValleyFlames", 297), I("Valley fire cards, day", "Ward/StandInFire", "quads", "ValleyFlamesDay", 167),
    I("Smoke sheet, day one", "Ward/SmokeSheet", "quads", "Body", 689), I("Smoke lid, night", "Ward/SmokeSheet", "quads", "Lid", 171),
    I("Smoke columns, day two", "Ward/StandInFire/SmokeColumns", "quads", "", 128), I("Ward stones", "Ward/Stones", "renderers", "", 3),
    I("Camp 3 tent", "Campsites/Camp_3/Dressing", "renderers", "CS_Tent", 8), I("Camp 3 fire", "Campsites/Camp_3/Dressing/Fire", "renderers", "", 15),
    I("Camp 3 easel", "Campsites/Camp_3/Dressing/Easel", "renderers", "", 13), I("North ruin", "Places/NorthRuin", "renderers", "", 44),
    I("Cave lights", "Cave", "lights", "", 5), I("Lanterns and lamps", "", "practicals", "", 29),   // 8.23: the boathouse lamp goes
    // camp (8.21, Wren 2026-10-02)
    I("Cabin", "Camp/Cabin", "renderers", "", 106), I("Tower", "Camp/Tower", "renderers", "", 393), I("Fire pit", "Camp/FirePit", "renderers", "", 32),
    I("Generator", "Camp/GeneratorDressing", "renderers", "", 3), I("Privy", "Camp/Privy", "renderers", "", 2), I("Stump", "Camp/Cabin/Woodpile", "renderers", "CITW_Tree_Stump", 1),
    I("Woodpile", "Camp/Cabin/Woodpile", "renderers", "C", 6), I("Rune post", "Ward/Climb/RunePost", "renderers", "", 4), I("Rune post screen", "Ward/Climb/RuneScreen", "renderers", "", 5),
    // front (8.22)
    I("Office", "FrontZone/Office", "renderers", "", 218), I("Store", "FrontZone/Store", "renderers", "", 151), I("Gate booth", "FrontZone/GateBooth", "renderers", "", 17),
    I("Barrier", "FrontZone/Gate/Barrier", "renderers", "", 8), I("Vault toilet", "FrontZone/VaultToilet", "renderers", "", 2), I("Resident car", "FrontZone/Resident_Car", "renderers", "", 2),
    I("Verge tree", "FrontZone/VergeTree", "renderers", "", 2), I("Chain", "FrontZone/Chain", "renderers", "", 8),   // 8.22 round 2: reflector bands and the CLOSED plate
};
string[] fireItems = { "Ridge fire cards, night", "Ridge fire cards, day", "Valley fire cards, night", "Valley fire cards, day", "Smoke sheet, day one", "Smoke lid, night", "Smoke columns, day two", "Ward stones" };
var none = new Main3AreaSet.DeckTarget[0];
// camp (8.21; gate round 2, 2026-10-02): places at their built spots with heights for Pim's found rule; Wren: the deck need not see the
// camp under its own tower (must-see is the far valley only); the cabin floor is at 15.03 and the deck floor at 56.03
const float campFloor = 15.03f, deckFloor = 56.03f, eyeH = 1.6f;
var wakeEye = V(179.9f, campFloor + eyeH, 168.95f); float wakeYaw = 245f * UnityEngine.Mathf.Deg2Rad;
var camp = new Main3AreaSet.Area { id = "camp", task = "8.21", title = "Keeper's camp, cabin and tower", bounds = new[] { R(138f, 130f, 208f, 192f) },
    warps = new[] { "Keepers_Camp", "Cabin", "Tower_Deck" },
    places = new[] { PO("Cabin door", 178f, G, 165f, 2.1f, "Camp/Cabin"), PO("Fire pit", 169f, G, 156f, 1.0f, "Camp/FirePit"), PO("Generator", 181f, G, 172.8f, 0.9f, "Camp/Generator"),
        PO("Stump (chopping block)", 182.8f, G, 165.4f, 0.6f, "Camp/Cabin/Woodpile/CITW_Tree_Stump"), PO("Privy", 176.5f, G, 178f, 2.6f, "Camp/Privy"), PO("Woodpile", 181.7f, G, 168f, 1.0f, "Camp/Cabin/Woodpile"),
        PO("Tower stair foot", 164f, G, 160f, 2.5f, "Camp/Tower"), PO("Forage patch B", 141f, G, 167f, 1.0f, "") },
    interactions = new[] { IA("bunk", "Camp/Cabin/Bunk", 179.9f, campFloor, 168.85f), IA("stove", "Camp/Cabin/Stove", 175.8f, campFloor, 168.65f), IA("desk, report box and chair", "Camp/Cabin/Desk;Camp/Cabin/ReportBox;Camp/Cabin/Interior/CITW_Chair", 176.4f, campFloor, 168.0f),
        IA("door", "Camp/Cabin/Door/Panel", 178f, campFloor, 164.75f), IA("shelf and counter", "Camp/Cabin/Interior/Counter", 178f, campFloor, 169.15f),
        IA("washstand", "Camp/Cabin/Interior/Washstand", 179.3f, campFloor, 166.85f), IA("stump", "Camp/Cabin/Woodpile/CITW_Tree_Stump", 182.0f, G, 164.4f), IA("lectern", "Camp/Tower/Cab/Lectern", 167.3f, deckFloor, 168.9f) },
    frames = new[] { FR("WAKE, FACING 245 (DOOR LEFT, DESK AND WINDOW CENTRE, STOVE RIGHT)", wakeEye, wakeEye + V(UnityEngine.Mathf.Sin(wakeYaw), 0f, UnityEngine.Mathf.Cos(wakeYaw)) * 10f),
        FR("WOODPILE AND STUMP FROM THE SOUTH, 4 M OUT", V(182.6f, G, 161.4f), V(182.3f, campFloor + 0.5f, 166.8f)),
        FR("SEATED AT THE DESK, FACING WEST TO THE WINDOW", V(176.1f, campFloor + 1.2f, 167.85f), V(170f, campFloor + 1.4f, 167.85f)),
        FR("PORCH TO THE STAIR FOOT", V(178f, campFloor + 0.13f + eyeH, 162.8f), V(164f, campFloor + 0.8f, 160.5f)) },
    playChecks = new[] { "main3_8_21_camp_check.cs", "main3_tower_stairs_check.cs", "main3_breaks_recheck.cs", "main3_8_21b_stop_check.cs" },
    inventory = new[] { "Lanterns and lamps", "Cabin", "Tower", "Fire pit", "Generator", "Privy", "Stump", "Woodpile", "Rune post", "Rune post screen" }, deckListWritten = true,
    deckSee = new[] { D("lake, mid water", 190f, -5.4f, 60f),
        D("Camp 1 spar top", 284f, 29f, 240f), D("Camp 2 stack top (over its tent)", 292f, 26f, 108f), D("Camp 3 Snag line (top)", 96f, 54f, 146.5f), D("office west door", 341f, 4.2f, 199f),
        D("cat step (boathouse)", 240f, -3.3f, 56f), D("verge tree", 419f, 25f, 139f), D("lot centre", 358f, 3.1f, 170f), D("highway", 430f, G, 185f) },
    deckHide = new[] { D("Ward stones, tops (W-1, land rays)", -3f, 70f, 224f), D("rune post (behind its rim rock)", 55.1f, 36.5f, 246.1f, "Ward/Climb/RunePost"), D("north ruin", 172f, 4f, 281f, "Places/NorthRuin"),
        D("far fire front, z 40", -240f, 105f, 40f), D("far fire front, z 170", -240f, 105f, 170f), D("far fire front, z 300", -240f, 105f, 300f), D("day-one sheet top", -500f, 110f, 170f), D("cave mouth", 52f, -4f, 37f) } };
// front (8.22; FrontLayout.md draft 2 section 5, Sable 2026-10-02): bounds, warps and places as the doc; places on their objects' own
// transforms (Marlow 8.22 paper 10); deck must-see hard: the office west door opening and the verge tree, tested against every drawn mesh
// (Marlow 8.22 paper 15); the rest loose; must-hide none. The ground is 3.00 over the whole zone.
const float frontG = 3.00f;
var front = new Main3AreaSet.Area { id = "front", task = "8.22", title = "Front zone", bounds = new[] { R(318f, 128f, 448f, 290f) },
    warps = new[] { "Office", "Store", "Trailhead_T", "Lot_Highway", "Gate_Booth", "Closed_Campground" },
    places = new[] { PO("Office west door", 344.1f, G, 199f, 2.15f, "FrontZone/Office"), PO("Store door", 365f, G, 195.6f, 2.1f, "FrontZone/Store/StoreDoor"),
        PO("Booth door", 391.9f, G, 164.2f, 2.5f, "FrontZone/GateBooth"), P("Lot centre", 358f, G, 170f), PO("Trailhead board", 338f, G, 172.5f, 2.1f, "Ground815/JunctionMarkers/Trailhead_Board"),
        P("First sight of the lot", 327f, G, 168f), PO("Ice chest", 368.8f, G, 194.9f, 0.85f, "FrontZone/Store/Front/Ice_Cream_Freezer"), PO("R6's car", 370f, G, 179.4f, 1.55f, "FrontZone/Resident_Car"),
        PO("Toilet door", 342f, G, 186f, 2.4f, "FrontZone/VaultToilet"), PO("Chain", 390f, G, 238f, 1.0f, "FrontZone/Chain") },
    interactions = new[] { IA("booth window counter", "FrontZone/GateBooth/Counter", 391.9f, frontG, 165.2f), IA("back-room door", "FrontZone/Office/BackDoor", 351.1f, frontG + 0.05f, 201.0f),
        IA("trailhead board", "Ground815/JunctionMarkers/Trailhead_Board", 339.5f, G, 172.5f) },
    frames = new[] { FR("THE BOOTH FROM THE LOT'S EAST EDGE, THE BARRIER AND THE GATE", V(373f, G, 170f), V(392f, frontG + 1.5f, 166f)),
        FR("INSIDE THE BOOTH AT THE WINDOW, THE LANE", V(391.9f, frontG + eyeH, 165.0f), V(391.9f, frontG + 1.2f, 172f)),
        FR("THE OFFICE WEST DOOR FROM THE PORCH STEP", V(341.0f, G, 199.0f), V(345.5f, frontG + 1.4f, 199.0f)),
        FR("THE STORE DOOR AND THE ICE CHEST FROM THE WALK", V(365f, G, 190.5f), V(366.5f, frontG + 1.2f, 195.5f)),
        FR("THE TOILET FROM THE LOT", V(346f, G, 186f), V(341.2f, frontG + 1.2f, 186f)),
        FR("THE CHAIN FROM THE SPUR", V(390f, G, 228f), V(390f, frontG + 0.8f, 238f)),
        FR("THE CAMPGROUND RING FROM THE JOIN", V(387.5f, G, 250f), V(372f, frontG + 1f, 262f)) },
    // walk lines where no trail runs, for the found rule (FrontLayout_UI.md 3 and 4: from the T east across the lot to the booth, from
    // the lot centre north, along the walk, up the spur to the chain)
    walkLines = new[] { WL("T east across the lot to the gate", V(340f, G, 170f), V(358f, G, 170f), V(373f, G, 170f), V(386f, G, 169f)), WL("lot centre north to the walk", V(358f, G, 170f), V(358f, G, 190.5f)),
        WL("the walk", V(343f, G, 191.75f), V(372f, G, 191.75f)), WL("spur to the chain", V(385f, G, 172.5f), V(385f, G, 186f), V(390f, G, 196f), V(390f, G, 236f)) },
    playChecks = new[] { "main3_8_22_front_check.cs", "main3_8_22_deck_frames.cs?look=Day_one", "main3_8_22_deck_frames.cs?look=Night" },
    inventory = new[] { "Lanterns and lamps", "Office", "Store", "Gate booth", "Barrier", "Vault toilet", "Resident car", "Verge tree", "Chain" }, deckListWritten = true,
    deckSee = new[] { DH("office west door opening", 344.0f, 4.3f, 199f, "FrontZone/Office/WestDoor"), DH("verge tree", 419f, 25f, 139f, "FrontZone/VergeTree"),
        DL("store roof", 366f, 5.9f, 200f), DL("lot centre", 358f, 3.1f, 170f), DL("R6's car", 370f, 4.2f, 179.4f), DL("booth roof light", 391.9f, 5.8f, 165.4f),
        DL("barrier arm", 389.3f, 4.0f, 169.5f), DL("highway", 430f, G, 185f), DL("gate T stop sign", 423.05f, 5.3f, 166f) },
    deckHide = new Main3AreaSet.DeckTarget[0] };
// lake (8.23; LakeLayout.md draft 2 section 5, Sable 2026-10-02): places on their objects; deck must-see hard: her blanket and the bowl,
// every mesh; the rest loose; must-hide none. The water is -5.5, the house floor -3.8, the dock deck -4.8.
const float lakeFloor = -3.8f, dockDeck = -4.8f;
var lakeArea = new Main3AreaSet.Area { id = "lake", task = "8.23", title = "Lake", bounds = new[] { R(133f, 25f, 262f, 130f) }, warps = new[] { "Lake_Pump", "Lake_Boathouse" },
    places = new[] { PO("Pump", 190f, dockDeck, 94.8f, 1.3f, "Lake/Dock/Pump"), PO("Dock end rail", 190f, dockDeck, 86.44f, 1.0f, "Lake/Dock/RailEnd"),
        PO("Rowboat", 226.47f, G, 87.28f, 0.8f, "PointsOfInterest/POI_Overturned_rowboat"), PO("Water tank", 186.34f, G, 124.53f, 3f, "PointsOfInterest/POI_Water_tank"),
        PO("East doorway", 243.2f, lakeFloor, 52.4f, 2.0f, "Lake/Boathouse"), PO("Step", 240f, lakeFloor, 56.1f, 1.0f, "Lake/Boathouse/Dressing/Step"),
        PO("Slip rest", 240.85f, lakeFloor, 52.4f, 1.3f, "Lake/Boathouse/Layout823/SlipRest"), PO("Reeds rest", 243.6f, G, 62.0f, 1.3f, "Lake/Boathouse/Layout823/ReedsRest"),
        P("Beach", 243.6f, G, 57.0f), PO("Stake line", 240f, G, 60.4f, 1.3f, "Lake/Boathouse/Layout823/StakeLine"), PO("Soft ground", 250f, G, 58.35f, 0.3f, "Lake/Boathouse/Layout823/SoftGround") },
    interactions = new[] { IA("pump", "Lake/Dock/Pump", 190f, dockDeck, 96f), IA("slip rest", "Lake/Boathouse/Layout823/SlipRest", 241.3f, lakeFloor, 52.4f), IA("reeds rest", "Lake/Boathouse/Layout823/ReedsRest", 244.1f, G, 62.0f) },
    frames = new[] { FR("THE PUMP FROM THE TRAIL END, FACING SOUTH", V(190f, G, 96.5f), V(190f, dockDeck + 1.1f, 94.8f)),
        FR("THE SLIP FROM THE EAST DOORWAY", V(242.6f, lakeFloor + eyeH, 52.4f), V(238f, lakeFloor, 52.4f)),
        FR("THE STEP FROM THE NORTH DOORWAY", V(240f, lakeFloor + eyeH, 54.6f), V(240f, lakeFloor + 0.3f, 56.6f)),
        FR("THE SHALLOWS AND STAKE LINE FROM THE BEACH", V(244.5f, G, 57.5f), V(238f, -5.6f, 59f)),
        FR("THE BOATHOUSE FROM THE SHORE PATH, 20 M OUT", V(252f, G, 70f), V(240f, -3f, 54f)) },
    playChecks = new[] { "main3_8_23_lake_check.cs", "main3_8_23_lake_frames.cs?look=Day_one" },
    inventory = new[] { "Lanterns and lamps" }, deckListWritten = true,
    deckSee = new[] { DH("her blanket", 240.5f, -3.6f, 56.62f, "Lake/Boathouse/Dressing/Step/Blanket"), DH("her bowl", 241.2f, -3.7f, 56.5f, "Lake/Boathouse/Dressing/Step/CITW_Bowl_Small"),
        DL("boathouse roof", 240f, -1.0f, 52.4f), DL("pump", 190f, -3.6f, 94.8f), DL("dock end", 190f, -4.3f, 86.4f), DL("mid water", 190f, -5.4f, 60f), DL("reed bed", 241.5f, -4.6f, 62.2f),
        DL("stake line", 240f, -4.8f, 60.4f), DL("rowboat", 226.47f, G, 87.28f), DL("shore path east", 255f, G, 55f) },
    deckHide = new Main3AreaSet.DeckTarget[0] };
var areas = new System.Collections.Generic.List<Main3AreaSet.Area> {
    camp,
    front,
    lakeArea,
    new Main3AreaSet.Area { id = "camp1", task = "8.24", title = "Camp 1 and the north loop", bounds = new[] { R(150f, 195f, 300f, 305f) }, warps = new[] { "Camp_1", "North_Loop_Ruin" },
        places = new[] { P("Camp 1", 282f, G, 238f), P("Latrine shed", 264f, G, 207f), P("North ruin", 172f, G, 281f) },
        inventory = new[] { "North ruin" }, deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "camp2", task = "8.25", title = "Camp 2", bounds = new[] { R(255f, 55f, 335f, 165f) }, warps = new[] { "Camp_2", "Camp_2_Top" },
        places = new[] { P("Camp 2", 292f, G, 104f), P("Stack top", 294.5f, 24f, 107.2f), P("Payphone", 298.5f, G, 99f), P("Food lockers", 317f, G, 136f), P("Phone pole", 273f, G, 72f) },
        inventory = new string[0], deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "camp3", task = "8.26", title = "Camp 3 and the west trails", bounds = new[] { R(55f, 60f, 160f, 180f) }, warps = new[] { "Camp_3", "Camp_3_Rim", "Junction_W1" },
        places = new[] { P("Camp 3", 78f, G, 146f), P("Easel", 83.5f, G, 149f), P("Rim", 100f, G, 148f), P("Camper trailer", 89f, G, 112f), P("Stepping stones", 132f, G, 73f), P("Footbridge", 115f, G, 85f), P("Washed-out truck", 156f, G, 92f) },
        inventory = new[] { "Camp 3 tent", "Camp 3 fire", "Camp 3 easel" }, deckSee = none, deckHide = none },
    new Main3AreaSet.Area { id = "cave", task = "8.27", title = "Cave and ravine", bounds = new[] { R(20f, -20f, 120f, 65f) }, warps = new[] { "Cave_Mouth", "Cave_Chamber" },   // the closed campground is the front's (8.22, Wren 2026-10-02)
        places = new[] { P("Cave mouth", 52f, G, 40f), P("Chamber", 80f, -18f, 12f), P("Side room", 94f, -18f, 12f), P("Rope handrail", 107f, G, 58f), P("Coloured bulbs", 83f, G, 49f) },
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
