# Asset catalogue for 8.15 to 8.17 (Rook, 2026-09-30)
Owned assets only (Assets/SOURCES.md). Paths are under Assets/. Sizes are unscaled bounds in metres; "col" means the prefab has a collider. Grey is mean luma 0 to 1. GAP marks a need that nothing owned fits.
## Terrain layers
- Note: all six Main3 layers use Ground054 today, so floor and trail cannot differ (Pim W1).
- Floor: BK/PureNature_Redwood/Textures/Surfaces/GrassPine_a.png (grey 0.35; TerrainLayers/GrassPine.terrainlayer). Alternatives: GrassMud_a (0.34), SoilPine_a (0.31), Moss01_a (0.34).
- Dirt trail: Textures/Ground054/Ground054_Color.jpg (0.55, lighter than every floor). Darker option: BK Surfaces/Mud_a (0.28), Mud_darker_a (0.24).
- Rock slope: BK/PureNature_Redwood/Models/Rocks/Textures/Rocks_a.png (0.55, 2048 px; no terrain layer yet). Also Revolving Pizza Games/Campsite/Textures/CS_Rock.png (0.34, 256 px).
- Scree: GAP, no scree texture. Nearest: Rocks_a at a small tile, dressed with BK RubbleSparse meshes (below).
## Visible stops
- Boulders: BK/PureNature_Redwood/Prefabs/Rocks/Boulder_0-5 (6 x 4, col), BigBoulders_0-5 (6 x 6, col). Campsite/Prefabs CS_Rock_1-8 (2 to 4 m). Effigy GameWorks Rock and DarkRock (30 x 9 outcrops, no col).
- Rubble: BK Prefabs/Rocks/RubbleSparse_1-3 and RubbleDense_1-3 (5 to 7 m wide, 0.8 high, no col).
- Logs and deadfall: BK Prefabs/HollowLogs/RedwoodHollowLog_0-2 and 2_0-2 (4 to 7 m, col). CITW_Log_1/2 (2 m), CITW_Tree_Stump, Celestia Tree_Dead (4.3, col), CS_Log_Large*. Ground debris: BK Plants/Branchs, RedFirBranches.
- Brush: suffercord Bush1-4 (1.1 to 1.3), CITW_Bush_1/2 (1 to 1.2), CS_Bush_Large_* (2.0), CS_Bush_Small_* (1.1). GAP: no brush mass over 2 m and no deadfall tangle; stack logs, rubble and bushes.
## Trees by height class (unscaled)
- Under 7 m: suffercord Aspen1-6 and Birch1-6, plus Leafless versions (5 to 6). BK RedFir1-4 (2 to 8). Celestia Tree_Leafy (5.4), Tree_Dead (4.3).
- 8 to 14 m: BK RedFir5-8 (9 to 11), RedPine1-5 (6.5 to 12.8). suffercord Pine1-6 (8.2). CITW_Tree_1-4 (8.5 to 10). CS_Tree_1_x and 2_x (14), CS_Tree_3_x and 4_x (11 to 14).
- Giants: BK Sequoia1-5 (34 to 46).
- Rev 8's 18 to 24 m firs are BK RedFir or RedPine scaled 2 to 2.5 times; only BK has LODs and impostors (TreeCover.md). The Pizza, suffercord and Celestia trees are low-poly PSX style and look different next to BK.
## Building kits
- Store and office: Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Building_Parts, Roof, Marketplace_Assets (shelves, counters), Decoration_*.
- Camps: Campsite/Prefabs CS_Tent_Large_Modern_* and CS_Tent_Large_Old_*, awnings, campfires, seats, lanterns.
- Boathouse: Cabin In The Woods/Prefabs CITW_Plank_* walls, CITW_Roof_1/2, doors, windows; Catacombs C_Plank_* for deck boards. GAP: no dock piles, pier or boat.
- Ruin: Revolving Pizza Games/Catacombs/Prefabs/Building (C_Pillar*, C_Wall_*, arches); Effigy StoneMenhir_*.
- Lot and fence: PSX Edition - Modular Parking Lot/Prefabs (Road_Post 6.2, lot ground); Modular Chain Link Fence/Prefabs.
## Signposts and markers
- GAP: no signpost or trail marker prefab in any pack. Use the project board pattern (Tools/Recipes/build_signs.cs, Planks023A). Markers: CS_Stone_1-8 for cairns, Celestia RailingPost_Wood (0.8), Road_Post.
## Frame-rate baseline (8.16 compares against this)
Main3 rev 7, day one, Editor Play mode at 3840 x 1976, RTX 2080 Ti and i9-9900K, vSync off. 600 frames per spot after 60 warm-up. Recipe: Tools/Recipes/perf_baseline_8_16.cs.
| Spot | Average fps | 1% low fps |
|---|---|---|
| Camp (172, 150), heading 333 | 170.6 | 97.5 |
| S1 (156, 148), toward (178, 168) | 157.1 | 102.5 |
| Office (340, 196), heading 68 | 258.1 | 156.6 |
