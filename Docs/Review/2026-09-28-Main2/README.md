# Main2 review captures, 2026-09-28

Scene Assets/Scenes/Main2.unity, Play mode, current LookTuning. 1280 x 720, FOV 60, temporary camera copied from Camera.main (the player camera was not moved). VHS look filter ON unless the name says filter_off. Eye height is Main terrain ground + 1.6 m unless a fixed y is given. Capture script: scratchpad review_shots.cs (not committed; this is not a build task).

## Shots

| File | Location | Camera position | Look-at |
|---|---|---|---|
| 01_cabin_interior.png | Camp cabin, player spawn on wake, facing the door | (245.2, 25.7, 190.6) | 4 m ahead at yaw 180, (245.2, 25.6, 186.6) |
| 02_camp_from_path_A.png | Camp from the mouth of path A (MAIN CAMERA SPOT B) | (232, 25.6, 190) | (247, 25, 184) |
| 03_fire_pit_close.png | Camp fire pit, from the cabin side | (248.3, 25.6, 185.8) | (251.5, 24.4, 182.5) |
| 04_tower_top.png | Firewatch tower top looking west (SPOT C) | (234, 40.9, 203) | (-150, 44, 260) |
| 05_path_A_middle.png | Path A middle, facing west toward the hunting stand bend | (166, 25.7, 222.5) | (140, ground + 1.4, 215) |
| 06_path_A_fallen_trunk.png | Path A fallen trunk beat, second bend | (118.5, 31.9, 244.5) | (123.4, 31.8, 252.2) |
| 07_ward_approach.png | Path A last climb toward the Ward clearing | (96, 41.6, 306) | (46, 43, 330) |
| 08_ward_ledge_fire.png | Ward ledge facing the horizon fire (SPOT A) | (66, 41.6, 323) | (-150, 44, 318) |
| 09_ward_stones_close.png | Ward stones close | (52, 41.6, 329) | (43, 44, 336) |
| 10_entrance_road_gate.png | Entrance road east toward gate and office | (348, 25.6, 176) | (388, 25.5, 170) |
| 11_office_interior.png | Ranger office interior from the west door corner | (369, 25.8, 164) | (375.5, 25.1, 168) |
| 12_lake_dock.png | Lake and dock from the spur end | (244, 26.1, 106) | (218, 23.8, 102) |
| 13_tent_site.png | Tent site (Campsite_1_Tents) from its trail | (223, 25.6, 147) | (208, 24.6, 147) |
| 14_campsite2_cabin.png | Campsite 2 cabin from the approach (DevWarps position) | (252, 25.4, 96) | (246, 25.5, 85) |
| 15_campsite3_cabin.png | Campsite 3 cabin and fire ring | (311, 26.1, 73) | (323, 25.5, 58) |
| 16_cave_mouth.png | Cave mouth from the spur | (300, 23.4, 244) | (300, 26, 262) |
| 17_cave_chamber.png | Cave chamber from the tunnel end | (300, 25.6, 274.5) | (300, 24.8, 284) |
| 18a_trail_shrine.png | Path A stone ring shrine, 3.5 m off trail | (136.5, 26.9, 219) | (139.6, ground + 0.5, 222.2) |
| 18b_hunting_stand.png | Path A hunting stand | (158, 25.9, 219) | (148, 28, 221) |
| 18c_path_B_fork.png | Path B fork and signpost | (262, 26.8, 147) | (271, 26, 131) |
| 18d_roadside_car.png | Road, pulled-off car beat | (305, 26.6, 176) | (318, 25, 179.5) |
| 18e_notice_board.png | Road notice board by camp | (264, 25.6, 177.5) | (268.6, 25.3, 174.5) |
| 18f_lake_shore_loop.png | Lake shore loop toward campsite 2 cabin | (236, 25.3, 96) | (247, 24.8, 81) |
| 18g_cave_spur.png | Faint cave spur off the road | (308, 25.5, 200) | (314, 24.5, 212) |
| 20_camp_from_path_A_filter_off.png | Same as 02, filter off | (232, 25.6, 190) | (247, 25, 184) |
| 21_ward_ledge_fire_filter_off.png | Same as 08, filter off | (66, 41.6, 323) | (-150, 44, 318) |

Not captured: 19 top-down overview. An orthographic 1600 x 1600 render from (200, 400, 200) with tree distance raised to 2000 m crashed the Editor (D3D12 device lost in GfxTaskExecutorD3D12, crash report Crash_2026-09-28_212122576). The approved layout drawings are Docs/Layout/Main_layout.svg and Docs/Layout/Main_layout_v2.svg.

## Facts

Source: PLAN.md Rules and Tips and a scene query on 2026-09-28. Walk distances are sums of trail polyline segments from the cabin door (245, 187), not a measured walk; time at 2.5 m/s.

- Main terrain 400 x 400 m (height range 60 m, heightmap 513). ValleyTerrain 240 x 720 m west (visual only, no collider). EastTerrain 240 x 400 m east of x 400 (visual only).
- Playable area: inside the Bounds walls on the Main terrain, west limit the cliff wall at x 40, the gate and fence at x 388 on the east. Roughly 350 x 400 m.
- Terrain trees: Main 16,822, Valley 3,577, East 7,870. Ground cover about 340k detail instances.
- Trails: path A about 274 m (255 m polyline plus the run into the Ward clearing), path B to the fork 41 m, three loop connectors 214 m, road 122 m.

| Destination | Route | Distance (m) | Time at 2.5 m/s |
|---|---|---|---|
| Fire pit | across clearing | 8 | 3 s |
| Firewatch tower base | across clearing | 13 | 5 s |
| Ward clearing (58, 323) | path A | 281 | 1 min 52 s |
| Path B fork (270, 130) | path B | 62 | 25 s |
| Lake dock (236, 104) | path B, branch 2, lake spur | 109 | 44 s |
| Campsite 2 cabin (247, 85) | path B, branch 2 | 110 | 44 s |
| Tent site fire (208, 147) | path B, new tent branch | 127 | 51 s |
| Campsite 3 fire ring (316, 64) | path B, branch 3 | 143 | 57 s |
| Office door (368, 166) | road | 126 | 50 s |
| Gate (388, 174) | road | 145 | 58 s |
| Cave mouth (300, 258) | road, cave spur | 151 | 1 min 0 s |
| Cave chamber (300, 281) | as above plus tunnel | 170 | 1 min 8 s |

Objects per location (direct children of each scene group):

| Location | Group | Count | Buildings |
|---|---|---|---|
| Camp | Camp + Era_Modern | 26 + 3 | cabin 6 x 4 m, firewatch tower (deck 15 m), generator blockout |
| Ward | Ward | 3 stones | none |
| Path A beats | Beats | fallen trunk, hunting stand, shrine (11 parts), broken signpost | hunting stand |
| Entrance | Entrance | 10 (Office 44 parts, Fence 113 panels, Parking 5 cars and props, gate, 3 street lights, bench, phone booth, barrel) | office 8 x 6 m |
| Road beats | Beats | roadside car, notice board, player pack, blanket, key, 3 spur candles, 4 lantern posts | none |
| Lake | Lake | 14 (water, wade limit, dock 32 parts, 10 shore rocks, pack) | dock |
| Tent site | Campsite_1_Tents | 21 (5 tents, fire, seats, table, props) | none |
| Campsite 2 | Campsite_2_Cabin | 6 (cabin, cold fire, tripod, pot, seat, bucket) | cabin 4 x 4 m with porch |
| Campsite 3 | Campsite_3_Cabin | 8 (cabin, fire ring, seat, bottle, rock, shovel, barrel, firewood) | cabin 8 x 4 m, two rooms |
| Cave | Cave | 203 (8 props, 195 rock pieces) | none |
| Signs | Signs | 5 signs | none |
