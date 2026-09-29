# Look slice: keeper's camp and tower

**DRAFT, 2026-09-29, Vesper, for Rook. Nothing here is decided until it is a line in DECISIONS.md and Grant has confirmed it.** Grant's default (2026-09-29): distance layers first, then this one dressed slice, before the rest of the dressing. Purpose: Grant sees the designed look in the game before judging it (DECISIONS 2026-09-29, "needs to see it designed"). Scene Assets/Scenes/Main3.unity. Positions and heights from Main3.md revision 14 (3.4, 5.1). Palette and rules from Style.md; board from LookBoards.md 1. Every value is **P**; Rook sets LookTuning, I propose.

## 0. Before the slice

1. The distance layers (Main3.md revision 15, Sable) are built first, so the cab shots show them.
2. The Ward move to the high plateau does not touch this slice. The tower, knoll and camp stay as built.
3. Pack prefab names below were checked in the project folders on 2026-09-29. Names marked (check) I have not confirmed; Rook lists the real name or the gap.

## 1. Scope

The 36 m clearing around (170, 160) on the 15 m knoll, the tower at (164, 166), the knoll flank to 20 m outside the clearing edge, and the view out from the cab. Nothing else is dressed.

## 2. Structures and pack assets

Pack materials only on pack meshes (PLAN.md Rules and Tips, PACK MATERIALS ON PLAIN GEOMETRY). ProBuilder pieces use the project materials Planks023A, Concrete034 or PaintedMetal006, retinted.

| Structure | Where | Assets | Notes |
|---|---|---|---|
| Cabin shell | centred (178, 168), inside 6 x 4.5 m, ceiling 2.7 m | CITW_Log_Wall, CITW_Log_Corner_Wall, CITW_Log_Window_Wall (west wall, desk window), CITW_Log_Doorway, CITW_Door_1 (south wall, centred), CITW_Window_Frame, CITW_Window_Glass, CITW_Log_Wall_Triangle (gables), CITW_Roof_1, CITW_Roof_1_Top, CITW_Roof_1_Side, CITW_Roof_Tip, CITW_Floor | Three bands: porch and steps, log body, roof with stovepipe. Roof overhangs at least 0.5 m. |
| Porch | south face, full width, 1.8 m deep | CITW_Floor, CITW_Wood_Pillar, CITW_Railing, CITW_Railing_Pillar, CITW_Stairs | Lantern on a stool by the door. |
| Stovepipe | through the roof over the north-east corner | CITW_Fireplace_Flue_1 or _2, CITW_Fireplace_Chimney | Retint rust #8B4A2B. |
| Cabin interior | as Main3.md 3.4 | CITW_Bed (north wall, west half), CITW_Wood_Stove (north-east corner), CITW_Table as the desk under the west window, CITW_Chair, CITW_Shelf, CITW_Rug_1, CITW_Oil_Lamp_1 with CITW_Oil_Lamp_Flame_FX on the desk, CITW_Kettle on the stove, CITW_Mug, CITW_Book_1 and _3, CITW_Canned_Food_1 and _2, Supplies MatchesV1, Water | Report box on the desk: CITW_Crate scaled down, or Supplies Toolbox (check). Keep the 1.5 m aisle. |
| Fire pit cluster | (172, 163) | CS_Campfire_1 with FX_Flames_Short (Flames_Additive.mat), CS_Campfire_Tripod_Wood, CS_Cookware_Kettle_3 hung from it, three CS_Log_Large_Seat_1 to _3 tangent at 2.3 m, CS_Chair_1, CS_Log_Stool_1, CS_Tableware_Mug_Metal_1 on a seat | Day one and day two: fire lit, kettle on. |
| Woodpile and block | east of the porch, against the cabin | CS_Firewood_Logs (check), CS_Log_Firewood, CITW_Firewood_1 and _2, CITW_Tree_Stump as the chopping block, CS_Tool_Axe_Metal_Rusted set into it at 30 degrees | The axe in the block is the mid-task object. |
| Generator | (181.5, 171.5), behind the cabin | Gap: ProBuilder 1.2 x 0.7 x 0.8 m on PaintedMetal006 retinted #8B4A2B and #1E1916, Farm Tools OilCanister_Worn and Canister beside it | No pack generator (PLAN.md PACK PREFABS). |
| Tower legs and stair | base (164, 166), 7.5 m square, deck 56 m | ProBuilder on Planks023A retinted #5C4632: square posts, beams every 8 m, X braces, the ten-flight spiral inside. PaintedMetal006 retinted rust only on bolt plates, stair treads' nosing and flight rails | Timber, never lattice (Main3.md 3 note 3). Silhouette: legs taper 0 percent; the cab is the only wide shape at the top. |
| Deck and cab | deck 8 x 8 m at 56; cab 4.4 x 4.4 m, walkway 1.8 m | ProBuilder deck and cab walls on Planks023A; window mullions on Planks023A; glazing a single URP Lit transparent material, #5E6878 at 12 percent alpha, smoothness 0.3; roof overhang 0.8 m on all sides; walkway rail ProBuilder plus PaintedMetal006 caps | Glazed on all four sides. Wind vane on the roof peak: ProBuilder arrow on a rod, rust. |
| Cab interior | inside the cab | Lectern facing north: CITW_Table narrow or ProBuilder (check); logbook CITW_Book_5 open on it; CITW_Hanging_Oil_Lamp from the ceiling; CITW_Stool_1; Farm Tools Flashlight_Worn on the sill; Supplies Water | The cab lamp is the night marker from the Ward trail. |
| Tower foot cluster | around the base | Farm Tools Bucket_Worn, WateringCan_Worn, Rope, ShovelSquare_Worn leaning on a leg; CITW_Barrel_1; CS_Lantern_Old_Rusted hung on a leg at 2 m | Third prop cluster (LookBoards 1: fire ring, porch and woodpile, tower foot). |
| Ground | the clearing and flank | Terrain floor #58583A day one, #4F4A2C day two; Redwood DeadLeaves1-2, GrassMoss, Clovers1-2, ThinFern1-3 on the edges; suffercord Grass1-3 retinted olive; CS_Rock_1-4 at the clearing edge | No bare ground within 10 m of any shot camera (Style.md 4.7). Vegetation still, no wind (DECISIONS 2026-09-29). |
| Clearing edge trees | outside the 36 m clearing | Redwood RedFir1-8 and RedPine1-5, at most 35 m tall on the knoll (Main3.md 2.8); no Sequoia on the knoll | Trees clustered, never on a grid. |

Density target: 25 props or more in the three clusters, none floating, every pack prefab given a fitted collider where the player can touch it.

## 3. Materials

1. Every material on URP Lit, URP Unlit, URP Particles or a project shader (Water, SkyGradient, HorizonGlow, DieAlone/Smoke). No pack shader (Style.md 4.5). Run list_material_shaders.cs after dressing; pack shaders = 0.
2. Retints: wood #5C4632, canvas and paper #D8CCB4 (brightest surface), rust #8B4A2B, char #1E1916, granite #6E6660. Nothing brighter than #D8CCB4 on a surface.
3. Import Max Size 512. Texel density 128 px per metre for props within 2 m, 64 for walls, ground and legs, 32 allowed above 10 m on the tower legs.
4. Emission only on lamp glass, flames and the cab lamp bulb.
5. Day two ash on surfaces: skip for the slice unless a project shader already supports it. Falling ash only (section 5).

## 4. Lights

All practicals #FFA860, the warmest light on the map. The fire pit is the brightest practical; nothing else beats it.

| Light | Type | Range | Day one | Day two on | Night |
|---|---|---|---|---|---|
| Sun | directional, LookTuning sun fields | | #FFC98A, elevation 14, bearing 270, intensity 1.1 | #FF8C40, elevation 6, bearing 270, intensity 1.2 | off |
| Ambient | flat | | #6E6658 | #734D42 | #07080A |
| Fire pit | point, FirePit flicker | 8 m | on, low | on, low | on, full |
| Stove | point, inside, flicker | 4 m | on | on | on, shows through the door and window |
| Desk lamp | point | 3 m | on, modest | on, modest | on |
| Porch lantern | point | 4 m | off | off | on |
| Cab lamp | point plus emissive bulb | 5 m | on, modest | on, modest | on, the marker; bulb emission kept out of fog or it disappears past the night fog end (Rook: say how) |
| Tower foot lantern | point | 3 m | off | off | on |
| Fire glow fill | directional or HorizonFire | | none | HorizonFire on, fireGlow as LookTuning_DayTwo | faint warm fill from the west, silhouettes only |

Shadows: two cascades, sun only. Practicals without shadows except the fire pit if the budget allows.

## 5. Looks

Use the look presets from 7.7: LookTuning_DayOne and LookTuning_DayTwo, and the scene's night look. Filter values at the Style.md 3 targets. Fog per the distance layer note (Main3.md revision 15).

| | Day one | Day two on | Night |
|---|---|---|---|
| Sky | #5E6878 top, #E3A968 horizon | #381C1A top, #D9662E horizon | #05080D, west lit #5A2412 to 30 degrees up |
| Fog | #A8A08E, 40 to 600 m | #9E5C38, 25 to 420 m | #05080D, 8 to 60 m |
| Fire | none: HorizonFire off, no smoke, no ash, no glow | glow and smoke per Style.md 6.3.6 to 6.3.9; NM Fire_Ashes_01 falling in the clearing, retinted #8A8078 | full front per Style.md 6.3 |
| Feel | an ordinary job, warm and safe | the same camp, the sky wrong | the camp is the one warm island |

## 6. The four fixed shots

Camera at eye height 1.6 m above ground, vertical FOV as the player camera, filter on. Rook moves a camera up to 3 m to clear a collider and records the move.

| Shot | Position (x, y, z) | Look at | What it must show |
|---|---|---|---|
| S1 Camp from the edge | (156, 16.6, 148), south-west clearing edge | (178, 16, 168), the cabin door | fire pit mid-frame in front of the cabin, tower foot on the left, three clusters, three value bands (sky, haze, dark foreground) |
| S2 Tower from the burn | (235, ground + 1.6, 168), on Camp to Jg in the old burn | (164, 38, 166) | the whole tower base to cab as the tallest vertical, knoll and camp roof under it; backlit on day looks |
| S3 Cab west | (164, 57.6, 166) | level, bearing 270 | day one: clear far ridge, layers stepping paler, no glow; day two: glow and smoke over at least 90 degrees; night: the fire; the spur's plateau skyline at the right edge, the Ward never in view |
| S4 Cab east | (166, 57.6, 166) | (360, 20, 185) | the old burn lane to the lot, office, mast and red lamp, then the east hills beyond the fence stepping paler |

Deliverable: 12 captures (4 shots x 3 looks) in Docs/Look/Slice/<Look>_<Shot>.png, plus S1 day one with the filter off (Style.md 3 note 2). Recipe in Tools/Recipes, named by Rook.

## 7. Pass checks (Vesper reviews the 12 captures)

1. Value structure in every frame: sky brightest, haze mid, foreground darkest.
2. Cabin and tower read by silhouette at 20 m in S1 and S2 with the filter on.
3. No bare ground in the lower half of any frame; nothing floats; no untextured primitive.
4. Day one: no fire, glow, smoke, ash or orange above #E3A968 in any frame.
5. Night: the cab lamp and cabin window visible in S2; nothing brighter than the fire in S3.
6. Filter on and off differ at a glance.
7. At least three distance layers visible in S3 and S4 on the day looks.

Vesper
