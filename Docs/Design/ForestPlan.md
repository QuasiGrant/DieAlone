# Forest and ground plan for 8.15 and 8.16 (Vesper, 2026-09-30)
Draft. From Valley.md rev 10 sections 3, 8, 10, 12 and Valley_map.svg; bar is Style.md 5.8 and 10; assets from AssetCatalogue.md. Every number is **P** until it is in DECISIONS.md and Grant confirms. Valley.md wins on positions and heights.
## 1. Owned assets (paths under Assets/)
- **BK** = BK/PureNature_Redwood/Prefabs. Giants: BK/Trees/Sequoia1-5 (34 to 46 m unscaled; bark import 1024, Style 4.1). Firs 8 to 20 m: BK/Trees/RedFir5-8 (scale 1 to 1.8), BK/Trees/RedPine1-5. Saplings in clumps: BK/Trees/RedFir1-4. Crest firs 18 to 24 m: RedFir5-8 or RedPine scaled 2 to 2.5.
- Foot of every grove: suffercord/PSX Autumn Forest Asset Pack/Models/Bush1-4 (retinted off autumn orange), BK/Plants/ThinFern1-5, BK/HollowLogs/RedwoodHollowLog_0-2 and 2_0-2, BK/Plants/DeadLeaves1-2, BK/Plants/Branchs, BK/Plants/RedFirBranches, Cabin In The Woods/Prefabs/Vegetation/CITW_Tree_Stump.
- Brush mass over 2 m (GAP): stack Revolving Pizza Games/Campsite/Prefabs/Vegetation/CS_Bush_Large_1* and _2*, a hollow log and BK/Rocks/RubbleDense_1-3.
- Forbidden in the forest: any non-BK tree (suffercord Pine, Aspen, Birch; Celestia; CS_Tree; CITW_Tree) next to BK trees, since they read PSX-style; the CS_Bush_Large_Flowers set; trees in rows.
## 2. Groves per area (8.16). Every grove: firs under the giants, not beside them; per grove 8 Bush, 12 ThinFern, 1 or 2 hollow logs, 2 DeadLeaves patches, Branchs
| Area | Groves (giants, firs) | Giants | Firs |
|---|---|---|---|
| North | N1 densest (8, 15; one light gap over the ruin), N2 (6, 12), C1 ring (7, 15), NE (5, 12), Mid (5, 15, thicket core), J (6, 12), NW foot (6, 12) | 43 | 93 |
| West | Rim (6, 12), SW (6, 12), Ravine (5, 10); brush along both edges of the W trails | 17 | 34 |
| Lake and south | Lake S1 (7, 13), Lake S2 (6, 13), Boathouse E (5, 12), Camp 2 E (5, 12, south of the stack-to-T line), SE (6, 13, in the thicket) | 29 | 63 |
| Knoll and burn | Knoll (4 at 35 m, 18 edge firs), Hollow Giant stand (4, 10), Burn edge (5, 10, at (285, 130)); old burn as built plus 20 snags, 10 fallen trunks | 13 | 38 |
| Open east | 2 knots of 3 giants; 10 fir clumps of about 20, clear of the deck lines to the verge tree, office west door and highway, and the stack-to-T line | 6 | 200 |
| Crests | W belt and hooks, N line, S line (section 3) | 12 | 440 |
- Floor giants top out at 50 absolute (height = 50 minus ground). Giants 8 to 15 m apart inside a grove, 40 to 60 m between groves.
## 3. Crests: look only, never cover (F-1 is land)
- **W belt and hooks**, x 5 to 30, ground 76 to 82: 250 firs 18 to 24 m, 12 giants in 3 or 4 knots. Fir tops ragged 96 to 106; knot giants break above that. Plant in knots and scatter, never a line along x 10 to 15. Solid trunk band up to 82, gaps only above it.
- The knob (x 8 to 20, z 200 to 245) stays bare rock, the one break seen from the deck. Keep the ledge, the fin top and the cleft's strip of sky clear of trees.
- **N line**, z 320 to 350: 150 firs 15 to 22 m, broken into 4 or 5 runs. Leave gaps over the N arm east of the hook (x 120 to 170), so J sees the far range over it (Valley 7.7).
- **S line**, z -30 to -50: 40 firs, sparse; the S saddle (x 170) stays bare for J's second layer.
- Night: the belt reads as a ragged black edge on #0C1016 (night 1) and on the glow (day 2 on); never a hedge.
## 4. North fir wall
- x 90 to 300, z 285 to 310, 25 m deep, ground 10 to 14, the N foot rock band behind it. 150 firs 8 to 24 m (RedFir1-8, RedPine1-5 mixed), ragged top; 5 Sequoia at 34 to 38 m breaking the top line, tops 50 or lower.
- Front edge wanders 5 m in and out; saplings (RedFir1-4) and Bush1-4 in front; deadfall (hollow logs, RedFirBranches) and brush at the foot on the collider side. SS1 (255, 284) sits at its foot, seen from the loop.
## 5. Open-east verge
- Fence to highway (x 396 to 440): no trees except the dead verge tree at (418, 136). Verge grass (BK/Plants/Grass1-3, GrassMoss), ditches, reflector posts, power poles. The road must read from the deck and the lot.
- Verge tree (GAP): no owned dead giant. Rook names the old-burn snag mesh; it must read broken-topped at 250 m from the deck.
- Beyond the road, 100 to 400 m out: rolling masses of RedFir and RedPine, 20 to 35 m tall, about 200, seen at LOD2 or as impostors.
## 6. Ground layers per area (8.15). Target: trail at least 20 grey (0 to 255) from the floor at 5 m and 20 m, day and night
Grey figures are raw texture means (AssetCatalogue x 255). Floors: GrassPine 89, SoilPine 79, Mud_darker 61. Trail: Ground054 140. Rock: Rocks_a 140 before retint. Texture paths: Textures/Ground054/Ground054_Color.jpg; BK/PureNature_Redwood/Textures/Surfaces/*_a.png with TerrainLayers/*.terrainlayer; rock is BK/PureNature_Redwood/Models/Rocks/Textures/Rocks_a.png (needs a new layer, import 512, retint #6E6660).

| Area | Floor | Trail | Rock (over 35 degrees) and scree fallback |
|---|---|---|---|
| North and West groves | SoilPine (needles) under N1, the wall and grove cores; GrassPine between | Ground054, 1.4 m, 0.4 blend | Rocks_a; ravine rim and W foot band BK/Rocks/BigBoulders_0-5, Boulder_0-5 |
| Lake and south | GrassPine; GrassMud at the shore | Ground054 | Rocks_a on the granite; shore rocks Boulder_0-5 |
| Knoll and burn | GrassPine on the knoll; GrassMud in the burn (no ash: day one) | Ground054 | Rocks_a on the switchback banks |
| Open east and front | GrassPine; BK Grass for the verge | Ground054 | none needed |
| Climb | chute stone steps; leg 2 scree; leg 3 ash (GAP: Mud_darker greyed, with Campsite CS_Wood_1_Burnt debris); leg 4 SoilPine | the leg itself, edged with Campsite/Prefabs/Rocks and Stones/CS_Stone_1-8 | scree fallback (GAP): Rocks_a at a small tile plus BK/Rocks/RubbleSparse_1-3 |
- Risk: in grove shade, texture contrast shrinks with the light (51 raw may fall under 20). Hence the darker SoilPine floor there, which gives 61 raw. At night texture contrast is not enough; pale CS_Stone edge stones every 3 to 5 m carry the trail. Both unverified until Pim measures.
## 7. Tree count and frame rate
- Count: about 126 giants (41 now) and about 1020 firs, plus the old burn as built and 200 beyond the road: about 1500, 2.6 times the 581 in the baseline.
- Baseline (AssetCatalogue, 4K Editor): Camp 170.6 avg / 97.5 1% low; S1 157.1 / 102.5; Office 258.1 / 156.6. My guess (unverified): Camp falls to 100 to 120 avg, 1% low near 60. Proposed floor: 1% low 60 at all three spots.
- Thin first, in this order: open-east clumps 200 to 120; N crest line 150 to 100; wall back half (z 300 to 310) 150 to 110; W belt interior firs behind the front knots 250 to 180; then grove firs down to 10 each. Never thin giants, the knoll, C1 ring, N1, or trees within 30 m of a trail.
## 8. Density 2026-10-01
Grant, 2026-10-01: "Forest is far too sparse". DECISIONS 2026-10-01: Milestone 8 needs a forest that feels like a forest. Sections 2 and 7 counts are replaced by this section; positions in 2 to 5 stand. All numbers **P**. Area sizes are read off Map_TopDown.jpg, plus or minus 20 percent, unverified; Rook measures.
### 8.1 Bar (replaces Style.md 10 Forest 8.16 open-ground line)
- Top-down: no open floor over 15 m across except named clearings, lake, lot, burn core, Ward plateau.
- Eye height, every trail frame: trunks within 10 m on both sides in 4 of 5 frames; at trunk height (0 to 10 m) no sky or far ridge seen through a grove past 40 m; floor 20 m off the trail mostly hidden by fern, brush or trunks.
- Spacing varies 50 percent either way; 60 percent of firs in clumps of 3 to 7. No rows, no even nursery spacing (Style.md forbidden 5).
### 8.2 Canopy (firs and pines over 8 m, giants) per area
| Area | Approx floor | Mean spacing | Canopy | Of which giants | Saplings (RedFir1-4, clumps of 3 to 5) |
|---|---|---|---|---|---|
| North groves N1, N2, Mid, J, NW foot, C1 ring | 1.6 ha | 4 to 5 m | 700 | 50 | 600 |
| North fir wall (section 4, depth 25 to 30 m) | 0.6 ha | 4 m | 250 | 7 | 150 |
| West: Rim, SW, Ravine, W foot | 0.8 ha | 5 m | 320 | 20 | 250 |
| Lake south and SE: S1, S2, Boathouse E, Camp 2 E, SE | 1.4 ha | 5 to 6 m | 450 | 30 | 400 |
| Centre: knoll, Hollow Giant, camp ring edge | 0.9 ha | 6 m | 250 | 20 | 150 |
| Burn (x 190 to 340, z 140 to 200) | 0.8 ha | edges 8 m live | 120 live, 150 snags, 30 fallen trunks | 5 | 0 |
| Open east clumps (section 2), off the highway sight lines | | clumps | 300 | 6 | 100 |
| Crests (section 3) and beyond the road (section 5) | | as written | 652 | 12 | 0 |
- Total about 3,040 canopy (about 1.9 times today's 1,600) and about 150 giants (126 today), plus 1,650 saplings.
### 8.3 Understory, per 100 m² of grove floor (cores; edges at half)
- ThinFern1-5: 8 to 12. Bush1-4 (retinted off orange): 2 to 3. Saplings: 1 to 2. DeadLeaves1-2: 1 patch. Branchs or RedFirBranches: 2. CITW_Tree_Stump: 1 per 200 m². RedwoodHollowLog_0-2 and 2_0-2: 1 per 300 m², lying at angles, never parallel to a trail.
- Core floor is SoilPine needles with fern and litter, 50 percent covered; terrain grass (BK Grass1-3, GrassMoss) only in light gaps, edges and clearings.
- Brush mass over 2 m (CS_Bush_Large_1*, _2*, RubbleDense_1-3): wherever a hedge collider stands, plus one per 400 m² in cores to block long floor views.
### 8.4 Forest edge at trails and places
- Standard trail: 1 m verge of short grass and litter, fern band 1 to 3 m, first trunks 2.5 to 5 m from the centre line, at least one trunk within 6 m every 10 m on each side; brush at 4 to 8 m; edge wanders 2 m in and out; at bends a trunk or clump sits on the outside to close the view.
- Lake trails (pump, boathouse, W1): water side fern and reeds only, no trees within 6 m of the north shore; land side standard.
- Burn trails (Camp to Jg, Jg to T, Jg to Camp 1, Camp 2 to T): keep the brush; add a snag over each hedge every 8 m and fallen trunks angled at the trail, so the hedge never reads as a hedge.
- Climb legs: fir pockets on benches, saplings and RedFirBranches on the downslope; no trunk wall.
- Camp: clearing 25 m from the fire, giants and firs closing at 25 to 30 m all round. Camps 1, 2, 3: clearing 12 to 15 m, saplings to 8 m from the table. Ruin: the one light gap (section 2). Cave mouth: firs to the rock face. Boathouse and dock: shore open 6 m.
- Office, store, lot: forest edge 20 m behind the buildings on the north and west; east to the highway stays open (verge, section 5).
- Warps and trail ends: no foliage within 1.5 m of the eye (index.md warp foliage check).
### 8.5 Frame rate (baseline Perf_8_16.md: pass 2 Camp 123.8 / 66.4, S1 114.3 / 80.8; pass 1 first-view hitches under the floor)
- Canopy stays BK prefabs (only BK has LODs and impostors, AssetCatalogue), drawn through the GPU Resident Drawer. No colliders on trees over 40 m from any trail or on crests.
- Proposed, unverified (Rook checks): ferns, grass, DeadLeaves and Bush1-4 as terrain detail meshes (instanced); saplings and trees over 40 m from a trail as terrain trees. Rook confirms whether terrain trees go through the Resident Drawer in 6000.3 and whether BK LODGroups survive as terrain tree prototypes.
- Gate: PerfSpots at Camp, S1, office and a new spot in the N1 core; floor 1% low 60 on pass 2. If it fails, thin in the section 7 order, but core spacing never over 6 m and no trail frame loses the 8.1 bar.
Vesper
