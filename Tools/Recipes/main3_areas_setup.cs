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
Main3AreaSet.DeckTarget DLO(string l, float x, float y, float z, string obj) => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), loose = true, objectPath = obj };   // loose, its own object never hides it (8.24 gate)
Main3AreaSet.DeckTarget DHA(string l, float x, float y, float z, string obj) => new Main3AreaSet.DeckTarget { label = l, point = V(x, y, z), hard = true, objectPath = obj, anyEye = true };   // hard, passes from any deck eye (Wren 2026-10-02: the lake's blanket and bowl, as the office door)
Main3AreaSet.WalkLine WL(string l, params UnityEngine.Vector3[] pts) => new Main3AreaSet.WalkLine { label = l, points = pts };
Main3AreaSet.InventoryItem I(string l, string p, string k, string n, int e) => new Main3AreaSet.InventoryItem { label = l, path = p, kind = k, name = n, expected = e };
// ground Valley.md 8 closes with thicket (as main3_reach_check_8_16a.cs)
set.closedZones = new[] { R(346f, -10f, 395f, 99f), R(141f, -10f, 248f, 25f) };
// stops (Main3AreaSet.stopRoots), set here so the asset follows this file (the asset had kept its first list, without the lake's):
// 8.23 round 2 adds the dock rails, their RailStops and the slip rails (Marlow 823 finding 1)
set.stopRoots = new[] { "FrontZone/BrushBands", "FrontZone/ShiftWalls", "FrontZone/Gate/PlayerBlocker", "Ground815/Stops", "Fence", "Lake/WadeLimit", "Lake/Boathouse/Layout823/StakeLine",
    "Lake/Dock/Rail", "Lake/Dock/Layout823/RailStops", "Lake/Boathouse/Layout823/SlipRails",
    "Campsites/Camp_2/StackTop/Layout825/TopRail", "Campsites/Camp_2/Layout825/LandingRailN", "Campsites/Camp_2/Layout825/Skirts" };   // 8.25: the top rail, the landing rail and the stair skirts
// closed water (8.23 round 2): the lake inside the wade ring, the stake line and the house skirts, filled from mid water
set.closedFills = new[] { new Main3AreaSet.ClosedFill { label = "lake", seed = new UnityEngine.Vector2(190f, 60f), surface = -5.5f, within = R(133f, 25f, 262f, 130f), cell = 0.5f, body = 0.35f } };
// inventory (Vesper, Docs/Review/2026-10-02-Meeting/Vesper.md 4): expected counts as built by the runner on 2026-10-02
set.inventory = new[] {
    I("Ridge fire cards, night", "Ward/StandInFire", "quads", "RidgeFlames", 132), I("Ridge fire cards, day", "Ward/StandInFire", "quads", "RidgeFlamesDay", 68),
    I("Valley fire cards, night", "Ward/StandInFire", "quads", "ValleyFlames", 297), I("Valley fire cards, day", "Ward/StandInFire", "quads", "ValleyFlamesDay", 167),
    I("Smoke sheet, day one", "Ward/SmokeSheet", "quads", "Body", 689), I("Smoke lid, night", "Ward/SmokeSheet", "quads", "Lid", 171),
    I("Smoke columns, day two", "Ward/StandInFire/SmokeColumns", "quads", "", 128), I("Ward stones", "Ward/Stones", "renderers", "", 3),
    I("Camp 3 tent", "Campsites/Camp_3/Dressing", "renderers", "CS_Tent", 8), I("Camp 3 fire", "Campsites/Camp_3/Dressing/Fire", "renderers", "", 15),
    I("Camp 3 easel", "Campsites/Camp_3/Dressing/Easel", "renderers", "", 13), I("North ruin", "Places/NorthRuin", "renderers", "", 47),
    I("Cave lights", "Cave", "lights", "", 5), I("Lanterns and lamps", "", "practicals", "", 30),   // 8.23: the boathouse lamp goes; 8.25 round 2: the Camp 2 top lamp is lit
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
    inventory = new[] { "Lanterns and lamps" }, deckListWritten = true, hardSeesTower = true,   // Wren 2026-10-02: the lake deck test counts the tower's rails and cab
    deckSee = new[] { DHA("her blanket", 240.5f, -3.6f, 56.62f, "Lake/Boathouse/Dressing/Step/Blanket"), DHA("her bowl", 241.2f, -3.7f, 56.5f, "Lake/Boathouse/Dressing/Step/CITW_Bowl_Small"),
        DL("boathouse roof", 240f, -1.0f, 52.4f), DL("pump", 190f, -3.6f, 94.8f), DL("dock end", 190f, -4.3f, 86.4f), DL("mid water", 190f, -5.4f, 60f), DL("reed bed", 241.5f, -4.6f, 62.2f),
        DL("stake line", 240f, -4.8f, 60.4f), DL("rowboat", 226.47f, G, 87.28f), DL("shore path east", 255f, G, 55f) },
    deckHide = new Main3AreaSet.DeckTarget[0] };
// north (8.24; NorthLayout.md draft 2 section 5, Sable 2026-10-02): places on their objects; the spar top is the hard deck target (mesh
// rays, as 8.22); the kid's table, tent and tripod loose; the ruin and SS1 to SS3 hidden by the pixel check with its 20 m control (SS1 60 m: it stands under fir and
// giant crowns that hid a 20 and a 35 m control).
// Interactions with no collider yet (R1's spot, the cookfire pot) wait for their milestone; the report box and forage C's shrubs have them.
var northArea = new Main3AreaSet.Area { id = "north", task = "8.24", title = "Camp 1 and the north loop", bounds = new[] { R(116f, 200f, 310f, 305f) }, warps = new[] { "Camp_1", "North_Loop_Ruin" },
    places = new[] { P("Camp 1", 282f, G, 238f), PO("Spar", 284f, G, 240f, 24f, "Campsites/Camp_1/Dressing/Spar"), PO("Kid's table", 285.2f, G, 236.5f, 0.8f, "Campsites/Camp_1/Dressing/KidTable"),
        P("R1's spot", 285f, G, 244f), PO("Tent", 271f, G, 234.5f, 3.0f, "Campsites/Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1"), PO("Cookfire", 276f, G, 232f, 1.0f, "Campsites/Camp_1/Dressing/Cookfire"),
        PO("Latrine shed", 264f, G, 207f, 2.2f, "PointsOfInterest/POI_Latrine_shed"), PO("Blaze stump", 271.7f, G, 247.3f, 1.4f, "Ground815/JunctionMarkers/Blaze_Camp1_Stump"),
        PO("Forage C", 228.73f, G, 271.05f, 1.0f, "Places/ForageC"), PO("SS1", 251.6f, G, 272.0f, 0.5f, "Places/NorthLoop/SS1"), PO("SS2", 150.5f, G, 276.5f, 0.45f, "Places/NorthLoop/SS2"),
        PO("SS3", 132.3f, G, 262.3f, 0.3f, "Places/NorthLoop/SS3"), PO("Fallen giant", 138.65f, G, 264.2f, 2.7f, "Places/NorthLoop/FallenGiant"), PO("North ruin", 172f, G, 281f, 3.0f, "Places/NorthRuin"),
        PO("Ruin doorway", 169.88f, G, 278.88f, 2.15f, "Places/NorthRuin"), PO("Report post", 171.22f, G, 278.38f, 1.3f, "Places/NorthRuin/Layout824/ReportPost") },
    interactions = new[] { IA("report box", "Places/NorthRuin/Layout824/ReportPost", 170.16f, G, 277.32f), IA("forage C", "Places/ForageC", 228.03f, G, 268.85f) },   // forage C has solid shrubs since 8.24 (Pim, Wren)
    frames = new[] { FR("F1 JG TRAIL INTO THE CLEARING, HEADING 31: TABLE, COOKFIRE, SPAR", V(276.5f, G, 221.5f), V(285.2f, 6.0f, 236.5f)),
        FR("F2 THE KID'S TABLE HEADING NW: THE BLAZE", V(284.2f, G, 237.5f), V(271.7f, 5.5f, 247.3f)),
        FR("F3 HEADING 298: FORAGE C (ROUND 2: AT THE TREAD EDGE)", V(239.8f, G, 261.5f), V(228.73f, 4.9f, 271.05f)),
        FR("F4 THE STOVEPIPE OVER THE RUIN (N14)", V(192.7f, G, 276.6f), V(171.43f, 8.0f, 283.40f)),
        FR("F5 SIDE-PATH MOUTH HEADING 9: DOORWAY AND REPORT BOX", V(168.1f, G, 267.1f), V(169.88f, 5.0f, 278.88f)),
        FR("F6 SS1 FROM THE LOOP", V(263.5f, G, 253.4f), V(251.6f, 5.0f, 272.0f)),
        FR("F7 SS2 FROM THE LOOP", V(161.2f, G, 269.4f), V(150.5f, 4.5f, 276.5f)),
        FR("F8 THE GIANT'S SW END", V(142.9f, G, 260.7f), V(131.5f, 7.0f, 257.3f)),
        FR("F9 FROM SS3 TOWARD THE FALLEN GIANT (VESPER: THE FAR SIDE OF THE LOG)", V(132.3f, G, 262.3f), V(134.4f, 7.6f, 260.1f)),
        FR("THE RUIN FROM ITS DOORWAY", V(169.6f, G, 278.6f), V(172.4f, 4.5f, 281.4f)) },
    walkLines = new[] { WL("side path to the ruin", V(168.1f, G, 267.1f), V(169.88f, G, 278.88f)) },   // the report box is found from the side path in (NorthLayout_UI 5)
    playChecks = new[] { "main3_8_24_north_check.cs" },
    inventory = new[] { "North ruin" }, deckListWritten = true,
    deckSee = new[] { DH("spar top", 284f, 29f, 240f, "Campsites/Camp_1/Dressing/Spar"), DLO("kid's table", 285.2f, 5.8f, 236.5f, "Campsites/Camp_1/Dressing/KidTable"), DLO("tent", 271f, 6.5f, 234.5f, "Campsites/Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1"), DLO("tripod", 276f, 6.3f, 232f, "Campsites/Camp_1/Dressing/Cookfire") },
    deckHide = new[] { D("north ruin", 172f, 4f, 281f, "Places/NorthRuin"), new Main3AreaSet.DeckTarget { label = "SS1", point = V(251.6f, G, 272.0f), treeCoverPath = "Places/NorthLoop/SS1", controlRaise = 60f }, D("SS2", 150.5f, G, 276.5f, "Places/NorthLoop/SS2"), D("SS3", 132.3f, G, 262.3f, "Places/NorthLoop/SS3") } };
// camp 2 (8.25; Camp2Layout.md draft 2 section 6, Sable 2026-10-02): places on their objects (main3_8_25_camp2.cs builds them under
// Campsites/Camp_2/Layout825 and StackTop/Layout825); the frames at the doc's eyes and headings; deck must-see hard: the lamp's cold
// core, every mesh; the rest loose; must-hide none. The top is 24.00, the camp floor about 4.
const float c2Top = 24f; const string c2L = "Campsites/Camp_2/Layout825/", c2T = "Campsites/Camp_2/StackTop/Layout825/";
// a place over an object's own collider, read from the scene: its foot at the collider's bottom, its height the collider's (the phone stands
// 1.2 m up the booth's back wall, so a foot on the ground puts the found lines under it); the ring box's centre for its frame
Main3AreaSet.Place POb(string l, string obj) { var t = Main3AreaSet.At(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), obj); var c = t != null ? t.GetComponentInChildren<UnityEngine.Collider>() : null; if (c == null) return new Main3AreaSet.Place { label = l, point = V(0f, G, 0f), objectPath = obj }; var b = c.bounds; return new Main3AreaSet.Place { label = l, point = V(b.center.x, b.min.y, b.center.z), height = b.size.y, objectPath = obj }; }
var coreT = Main3AreaSet.At(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), c2T + "Lamp/LampCore"); var coreAt = coreT != null ? coreT.position : V(291.6f, 25.3f, 107.4f);
var ringT = Main3AreaSet.At(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), c2T + "RingBox"); var ringAt = ringT != null ? ringT.position : V(289.3f, c2Top + 0.79f, 108.0f);
UnityEngine.Vector3 Hd(UnityEngine.Vector3 eye, float heading, float lookY) { float r = heading * UnityEngine.Mathf.Deg2Rad; return V(eye.x + UnityEngine.Mathf.Sin(r) * 10f, lookY, eye.z + UnityEngine.Mathf.Cos(r) * 10f); }
var c2F5 = V(298.3f, c2Top + eyeH, 107.25f);
var camp2Area = new Main3AreaSet.Area { id = "camp2", task = "8.25", title = "Camp 2", bounds = new[] { R(262f, 56f, 345f, 172f) }, warps = new[] { "Camp_2", "Camp_2_Top" },
    places = new[] { P("Camp 2", 294.3f, G, 95.0f), PO("Granite stack (south face)", 292f, G, 101.6f, 20f, "Campsites/Camp_2/GraniteStack"), P("Ramp foot", 298.9f, G, 107.8f),
        PO("Landing top", 298.3f, c2Top, 107.25f, 1.05f, "Campsites/Camp_2/StackPath/LandingTop"), PO("His chair on top", 294.94f, c2Top, 110.30f, 1.2f, c2T + "HisChair"),
        PO("Tent", 288.86f, c2Top, 108.0f, 1.5f, c2T + "Tent"), PO("Lamp", 291.6f, c2Top, 107.4f, 1.9f, c2T + "Lamp"), PO("Payphone", 300f, G, 99f, 3.2f, "Campsites/Camp_2/Dressing/Payphone"),
        POb("Hook", "Campsites/Camp_2/Dressing/Payphone/Telephone_Booth/Handset"), PO("Card table", 298.555f, G, 97.755f, 0.98f, c2L + "CardTable"),
        PO("His seat at the table", 299.40f, G, 97.76f, 1.2f, c2L + "CardTable/HisChair"), PO("Your seat", 297.71f, G, 97.76f, 1.2f, c2L + "CardTable/YourChair"), PO("Third place", 298.55f, G, 96.91f, 1.2f, c2L + "CardTable/ThirdPlace"),
        PO("Barrel", 291.4f, G, 101.4f, 1.19f, c2L + "Barrel"), PO("Phone pole", 272.8f, G, 72f, 8f, "PointsOfInterest/POI_Phone_pole"), PO("Food lockers", 317.2f, G, 136.0f, 1.2f, "PointsOfInterest/POI_Food_lockers"),
        PO("Start blaze", 304.4f, G, 106.4f, 1.8f, "Campsites/Camp_2/Dressing/StartBlaze"), PO("PS1", 288.14f, c2Top + 1.05f, 109.6f, 0.3f, c2L + "PaperSpots/PS1"), PO("PS2", 288.8f, c2Top + 1.05f, 111.2f, 0.3f, c2L + "PaperSpots/PS2"),
        PO("PS3", 284.95f, 4.43f, 107.40f, 0.3f, c2L + "PaperSpots/PS3") },
    interactions = new[] { IA("hook", "Campsites/Camp_2/Dressing/Payphone/Telephone_Booth/Handset", 299.65f, G, 99.05f), IA("ring box", c2T + "RingBox", 290.3f, c2Top, 108.0f), IA("R2 at the table", c2L + "CardTable/HisChair", 298.5f, G, 98.7f),
        IA("R2 on top", c2T + "HisChair", 293.8f, c2Top, 109.5f), IA("barrel", c2L + "Barrel", 292.3f, G, 100.4f) },
    frames = new[] { FR("F1 HEADING 7: TABLE 13.2 M, BOOTH 14.7 M", V(297.2f, G, 84.6f), Hd(V(297.2f, 0f, 84.6f), 7f, 5.0f)),
        FR("F2 HEADING 337: THE BARREL 12.15 M", V(296.1f, G, 90.2f), Hd(V(296.1f, 0f, 90.2f), 337f, 4.8f)),
        FR("F3 FROM THE T LEG HEADING 190: BOOTH AND TABLE", V(304.0f, G, 114.0f), Hd(V(304.0f, 0f, 114.0f), 190f, 5.0f)),
        FR("F4 RAMP FOOT HEADING 104: THE BLAZE THROUGH THE OPEN BAY", V(298.9f, G, 107.8f), Hd(V(298.9f, 0f, 107.8f), 104f, 5.2f)),
        FR("F5 TOP LANDING HEADING 300: CHAIR, TENT, LAMP", c2F5, Hd(c2F5, 300f, c2Top + 0.8f)),
        FR("F6 THE PHONE POLE FROM THE BOATHOUSE LEG", V(278.22f, G, 73.70f), V(272.8f, 7.0f, 72.0f)),
        FR("F7 THE FOOD LOCKERS FROM THE T LEG", V(310.15f, G, 129.43f), V(317.2f, 4.8f, 136.0f)),
        FR("THE PHONE FROM THE BOOTH MOUTH, FACING THE BACK WALL", V(299.65f, G, 99.05f), V(300.4f, 5.6f, 99.05f)),
        FR("THE RING BOX FROM THE TENT DOOR, FACING 270", V(290.3f, c2Top + eyeH, 108.0f), ringAt),
        FR("CAMP_2 WARP LANDING, FACING 55", V(294.3f, G, 95.0f), Hd(V(294.3f, 0f, 95.0f), 55f, 5.6f)),
        FR("CAMP_2_TOP WARP LANDING, FACING 20", V(294.5f, c2Top + eyeH, 107.2f), Hd(V(294.5f, 0f, 107.2f), 20f, c2Top + eyeH)) },
    // walk lines where no trail runs, for the found rule (doc 4: the ramp foot up the stair to his chair; your seat to the booth door): the
    // stair, the top round inside its rail, the table to the booth mouth
    walkLines = new[] { WL("the stair to the top", V(298.9f, G, 107.8f), V(298.9f, 9f, 118.75f), V(300.4f, 9f, 118.75f), V(300.4f, 14f, 107.25f), V(298.9f, 14f, 107.25f), V(298.9f, 19f, 118.75f), V(300.4f, 19f, 118.75f), V(300.4f, c2Top, 107.25f), V(296.0f, c2Top, 107.25f)),
        WL("round the top", V(295.3f, c2Top, 106.6f), V(292f, c2Top, 104.4f), V(290.3f, c2Top, 105.6f), V(290.3f, c2Top, 110.4f), V(292f, c2Top, 111.6f), V(294.0f, c2Top, 109.6f), V(295.3f, c2Top, 106.6f)),
        WL("to PS1 on the west rail", V(291.6f, c2Top, 109.7f), V(288.8f, c2Top, 109.7f)), WL("to PS2 at the NW vertex", V(291.6f, c2Top, 111.3f), V(289.4f, c2Top, 111.3f)),
        WL("the table to the booth mouth", V(297.5f, G, 98.6f), V(299.0f, G, 99.0f), V(299.6f, G, 99.2f)) },
    // Wren 2026-10-02: the hook and his table chair stand together by design (Camp2Layout C2: he answers without standing)
    // Wren 2026-10-03: the stand on the top rail box is the flood's teleport overlap (3424 flood moves from the top floor did not repeat it)
    acceptedStops = new[] { new Main3AreaSet.AcceptedStop { point = V(293.1f, 25.1f, 112.1f), radius = 0.5f, reason = "flood teleport overlap; 3424 flood-style moves from the top floor never stood there (Wren 2026-10-03)" } },
    spacingExceptions = new[] { new Main3AreaSet.SpacingException { a = "hook", b = "R2 at the table", reason = "he answers the phone from his seat (Camp2Layout C2, Wren 2026-10-02)", maxEdge = 0.5f } },
    playChecks = new[] { "main3_8_25_camp2_check.cs", "main3_8_25_camp2_frames.cs?look=Day_one", "main3_8_25_camp2_frames.cs?look=Night" },
    inventory = new[] { "Lanterns and lamps" }, deckListWritten = true,
    deckSee = new[] { DH("lamp core", coreAt.x, coreAt.y, coreAt.z, c2T + "Lamp"), DLO("top rail, west faces", 287.82f, 24.8f, 108.0f, c2T + "TopRail"), DLO("his chair on top", 294.94f, 24.8f, 110.30f, c2T + "HisChair"),
        DLO("tent", 288.86f, 25.0f, 108.0f, c2T + "Tent"), DL("PS1", 288.14f, 24.1f, 109.6f), DL("PS2", 288.8f, 24.1f, 111.2f), DL("PS3", 284.95f, 4.6f, 107.40f), DL("booth hood light", 299.0f, 6.6f, 99.0f) },
    deckHide = none };
// camp 3 (8.26; Camp3Layout.md draft 2 section 6, Sable 2026-10-02): places on their objects, read from the scene where main3_8_26_camp3.cs
// builds them (POn: the object's own x, z, ground y); interactions with a collider (R3's spot waits for its milestone, as R1's: no collider);
// deck must-see hard: Snag line pieces 1 to 3 (every mesh); loose: piece 4, the Snag lantern, the hoist, the east rim spot, the fire, the
// tent; must-hide by the pixel check with its 20 m control: the easel's painted face and the faced canvases' faces. The floor is about -4.0.
const string c3D = "Campsites/Camp_3/Dressing/", c3L = "Campsites/Camp_3/Layout826/";
Main3AreaSet.Place POn(string l, string obj, float h) { var t = Main3AreaSet.At(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), obj); var p = t != null ? t.position : V(0f, G, 0f); return new Main3AreaSet.Place { label = l, point = V(p.x, G, p.z), height = h, objectPath = obj }; }
var camp3Area = new Main3AreaSet.Area { id = "camp3", task = "8.26", title = "Camp 3 and the west trails", bounds = new[] { R(50f, 62f, 160f, 207f) }, warps = new[] { "Camp_3", "Camp_3_Rim", "Junction_W1" },
    places = new[] { P("Camp 3", 75.0f, G, 143.6f), PO("Fire", 76.0f, G, 150.5f, 0.6f, c3D + "Fire"), PO("Seat log", 73.8f, G, 150.2f, 0.45f, c3D + "CS_Log_Large_Long_Seat_1"),
        PO("Tent", 75.0f, G, 140.7f, 1.65f, c3D + "CS_Tent_Old_2"), PO("Easel", 80.3f, G, 148.8f, 1.8f, c3D + "Easel"), PO("Table", 78.5f, G, 147.3f, 0.75f, c3L + "PlankTable"),
        PO("Faced canvases", 70.3f, G, 145.95f, 1.3f, c3L + "FacedCanvases"), PO("Snag", 96f, G, 146.5f, 54f, "Giants/Heroes/Snag"),
        // the Snag line's pieces hang 2.85 m or more over the ground and over the ravine: deck targets (deckSee), not places to reach
        PO("Hoist cleat", 98.86f, G, 144.85f, 1.2f, c3L + "Hoist"), PO("Stake", 80f, G, 163f, 3.6f, c3L + "SnagLine/Stake"), P("East rim spot", 94f, G, 138.5f), P("Steps' top", 96.5f, G, 142.8f),
        PO("Pool", 83.6f, G, 149.7f, 0.3f, c3L + "Creek/Pool"), PO("Sink", 83.6f, G, 148.45f, 0.2f, c3L + "Sink"), PO("Spring", 85.8f, G, 126.0f, 0.6f, c3L + "Spring"),
        PO("W1 sign", 127.0f, G, 72.8f, 2.0f, c3L + "W1Sign"), POn("Blaze_W1_Camp3", "Ground815/JunctionMarkers/Blaze_W1_Camp3", 1.8f), POn("Camper trailer", "PointsOfInterest/POI_Camper_trailer", 2.4f),
        POn("Stepping stones", "PointsOfInterest/POI_Stepping_stones", 0.3f) },   // the lamppost spot (58, 150) is kept free off the trail (C7, homage): no found rule
    interactions = new[] { IA("fire", c3D + "Fire", 74.6f, G, 151.2f), IA("easel", c3D + "Easel", 79.0f, G, 149.9f), IA("form and table", c3L + "PlankTable;" + c3D + "JobForm", 78.6f, G, 148.4f), IA("sink", c3L + "Sink", 82.6f, G, 147.2f) },
    frames = new[] { FR("F1 STEPS' TOP HEADING 270: FIRE, POOL, CANVASES", V(96.5f, G, 142.8f), Hd(V(96.5f, 0f, 142.8f), 270f, -3.5f)),
        FR("F2 W1 ARRIVAL HEADING 342: FIRE, EASEL BACK", V(84f, G, 132f), Hd(V(84f, 0f, 132f), 342f, -3.0f)),
        FR("F3 THE FLOOR HEADING 60: THE LINE'S BACKS", V(78f, G, 147f), Hd(V(78f, 0f, 147f), 60f, 2.0f)),
        FR("F4 THE STONES HEADING 250: THE W1 SIGN", V(131.71f, G, 72.82f), Hd(V(131.71f, 0f, 72.82f), 250f, -3.0f)),
        FR("F5 W1 HEADING 300: THE BLAZE", V(128f, G, 70f), Hd(V(128f, 0f, 70f), 300f, -3.0f)),
        FR("F6 W1 LEG AT THE SPRING HEADING 20: WATER OUT OF THE ROCKS", V(84.43f, G, 122.24f), V(85.8f, -3.8f, 126.0f)),
        FR("CAMP_3 WARP LANDING, FACING 30", V(75.0f, G, 143.6f), Hd(V(75.0f, 0f, 143.6f), 30f, -2.6f)),
        FR("CAMP_3_RIM WARP LANDING, FACING 268", V(97.8f, G, 142.0f), Hd(V(97.8f, 0f, 142.0f), 268f, 2.0f)) },
    walkLines = new[] { WL("the floor from the arrival", V(79.06f, G, 145.39f), V(78f, G, 147f), V(76f, G, 148.5f)) },
    playChecks = new[] { "main3_8_26_camp3_check.cs" },
    inventory = new[] { "Camp 3 tent", "Camp 3 fire", "Camp 3 easel" }, deckListWritten = true,
    deckSee = new[] { DH("Snag line piece 1", 92.8f, 6.7f, 149.8f, c3L + "SnagLine/Piece1"), DH("Snag line piece 2", 89.6f, 6.8f, 153.1f, c3L + "SnagLine/Piece2"), DH("Snag line piece 3", 86.4f, 6.9f, 156.4f, c3L + "SnagLine/Piece3"),
        DLO("Snag line piece 4", 83.2f, 7.0f, 159.7f, c3L + "SnagLine/Piece4"), DLO("Snag lantern", 96f, 7.3f, 146.5f, c3D + "SnagLantern"), DLO("hoist", 98.86f, 5.0f, 144.85f, c3L + "Hoist"),
        DL("east rim spot", 94f, 4.5f, 138.5f), DLO("fire", 76.0f, -3.6f, 150.5f, c3D + "Fire"), DLO("tent", 75.0f, -3.2f, 140.7f, c3D + "CS_Tent_Old_2") },
    deckHide = new[] { D("the easel's painted face", 80.3f, -2.8f, 148.8f, c3D + "Easel/TheCanvas/Painting"), D("the faced canvases' faces", 70.3f, -3.4f, 145.95f, c3L + "FacedCanvases/Faces") } };
// cave (8.27; CaveLayout.md draft 2 section 6, Sable 2026-10-02): places on their objects (main3_8_27_cave.cs builds Cave/Layout827); the
// interactions with a collider (R7's spot waits for its milestone, as R1's and R3's); the deck must-hide by land rays (C-1, nothing depends
// on trees); the rim must-hide is the 8.27 check's RIM line. The chamber and side-room floor is -18.
const string cvL = "Cave/Layout827/";
var caveArea = new Main3AreaSet.Area { id = "cave", task = "8.27", title = "Cave and ravine", bounds = new[] { R(40f, 0f, 132f, 72f) }, warps = new[] { "Cave_Mouth", "Cave_Chamber", "Cave_SideRoom", "Spur_Descent" },   // the closed campground is the front's (8.22, Wren 2026-10-02)
    places = new[] { PO("Mouth", 52f, -6f, 37.9f, 2.25f, "Cave/Mouth"), PO("Boards", 52f, -5.4f, 37.6f, 1.8f, "Cave/Mouth/DayOneBoard"), POn("Bulbs", "PointsOfInterest/POI_Coloured_bulbs", 2.8f),
        POn("Rope rail", "PointsOfInterest/RopeRail827", 1.0f), PO("Toilet", 47.64f, G, 41.20f, 0.1f, cvL + "Toilet"), PO("Drip", 51.2f, -6f, 28f, 0.3f, cvL + "Drip"), PO("Niche", 54.5f, -6f, 32.5f, 2f, cvL + "Niche"),
        P("Chamber", 80f, -18f, 12f), PO("Seat shelf", 88.4f, -18f, 6.0f, 0.6f, cvL + "Chamber/SeatShelf"), PO("Sleep", 85f, -18f, 4f, 0.5f, cvL + "Chamber/Sleep"), PO("Food", 73f, -18f, 14.75f, 0.5f, cvL + "Chamber/Food"),
        PO("Stack", 86.75f, -18f, 18.75f, 3f, cvL + "Chamber/Stack"), PO("Battery bank", 84.5f, -18f, 20.7f, 0.8f, cvL + "Chamber/BatteryBank"), P("Side room", 93.5f, -18f, 11.5f),
        PO("Guest chair", 92.85f, -18f, 11.5f, 1.0f, "Cave/SideRoom/RouletteTable/Chair"), PO("His chair", 95.05f, -18f, 11.3f, 1.0f, "Cave/SideRoom/RouletteTable"), PO("Deeper opening", 97.75f, -18f, 13.8f, 2.1f, cvL + "Deeper/DeeperClosed") },
    interactions = new[] { IA("guest chair", "Cave/SideRoom/RouletteTable/Chair", 91.0f, -18f, 12.0f), IA("event 16 (deeper)", cvL + "Deeper/Wall_End", 101.0f, -18f, 14.0f) },
    frames = new[] { FR("F1 HEADING 222: THE OPENING (FOUND AT 10 M)", V(56.31f, G, 46.60f), Hd(V(56.31f, 0f, 46.60f), 222f, -4.6f)),
        FR("F2 HEADING 300: THE BULBS", V(91.3f, G, 47.4f), V(82.15f, 6.5f, 52.96f)),
        FR("F3 HEADING 180: THE LEG 1 OPENING AND THE CABLE", V(52f, -6f + eyeH, 24f), Hd(V(52f, 0f, 24f), 180f, -5.6f)),
        FR("F4 HEADING 90: STACK, DOORWAY, R7", V(71f, -18f + eyeH, 12f), Hd(V(71f, 0f, 12f), 90f, -16.8f)),
        FR("F5 THE DOORWAY HEADING 90: THE GUEST CHAIR", V(89.25f, -18f + eyeH, 12f), V(92.85f, -17.4f, 11.5f)),
        FR("F6 THE STANDING POINT HEADING 270", V(92.0f, -18f + eyeH, 12f), Hd(V(92.0f, 0f, 12f), 270f, -16.6f)),
        FR("CAVE_SIDEROOM WARP LANDING, FACING 90", V(91.0f, -18f + eyeH, 12f), Hd(V(91.0f, 0f, 12f), 90f, -17.0f)),
        FR("SPUR_DESCENT WARP LANDING, FACING 250", V(77.3f, G, 48.6f), Hd(V(77.3f, 0f, 48.6f), 250f, -2.0f)) },
    walkLines = new[] { WL("the descent", V(52f, -6f, 37f), V(52f, -6f, 22f), V(68f, -10f, 22f), V(68f, -10f, 17f), V(52f, -14f, 17f), V(52f, -14f, 12f), V(68f, -18f, 12f), V(71f, -18f, 12f)),
        WL("the chamber to the side room", V(71f, -18f, 12f), V(89.25f, -18f, 12f), V(92.0f, -18f, 12f)) },
    playChecks = new[] { "main3_8_27_cave_check.cs" },
    inventory = new[] { "Cave lights" }, deckListWritten = true, deckSee = none,
    deckHide = new[] { D("mouth", 52f, -4.5f, 37.9f), D("boards", 52f, -4.4f, 37.6f), D("jamb top", 57.7f, -0.2f, 37.9f), D("bulbs", 82.15f, 6.5f, 52.96f), D("toilet", 47.64f, -5.85f, 41.2f),
        D("rope rail", 70.9f, 0.7f, 44.0f), D("spur below the rise", 65f, -0.5f, 47.3f), D("ravine floor", 60f, -5.8f, 40f) } };
var areas = new System.Collections.Generic.List<Main3AreaSet.Area> {
    camp,
    front,
    lakeArea,
    northArea,
    camp2Area,
    camp3Area,
    caveArea,
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
