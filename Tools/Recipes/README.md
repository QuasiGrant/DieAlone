# Rebuild recipes

C# snippets that built or tested the scenes through the Editor bridge (`unity command eval`, `eval_file`). Each one runs as a method body: no `using` lines, fully qualified names, and most start by refusing to run in Play mode or outside the scene they target (Main, Main2 or Graybox). They are not compiled by Unity because this folder is outside Assets. Screenshot scripts write PNGs to Code's scratchpad folder; walk scripts drive the CharacterController along waypoints in Play mode and report stalls.

Copied from the scratchpad on 2026-09-27. Later scripts supersede earlier ones for the same object; the order used is listed under PLAN.md Rules and Tips. New recipes are saved here in the same commit as the work they built.

## Scene setup, prefabs, player (Milestones 1 to 4)

| File | Builds or does |
|---|---|
| build_graybox.cs | Graybox: 50 m ground, the tall tower box and six scale boxes, one directional light. |
| build_player.cs | Player object with CharacterController, camera pivot and PlayerController; removes the standalone camera. |
| build_tuning.cs | Creates Assets/Settings/PlayerTuning.asset and wires it to the player scripts. |
| fill_carry_tuning.cs | Writes the carry fields PlayerTuning.asset was missing, with the class default values (task 6.2). |
| pt_head.cs, pt_body.cs, pc_body.cs, lt_body.cs | Text of PlayerTuning, PlayerController and LookTuning written through the bridge (script bodies). |
| build_course.cs | Graybox test course: sprint lane, tunnel, step, log, fence, ramp, stairs, test room. |
| build_tower.cs | Graybox firewatch tower: legs, deck, four stair flights with landings, top room. |
| stair_ramps.cs | Collider-only StairRamp boxes over every stair flight (the stairs rule). |
| drive_test.cs | Pushes the controller up each flight to prove the ramps work. |
| build_interact.cs | HUD canvas with centre dot and prompt label, InteractPromptUI, first test usables. |
| build_doors.cs | Test room door hinge and panel with the Door component. |
| door_test.cs | Opens the door from both sides and checks the swing direction. |
| build_carry.cs | Carryables in Graybox, HoldPoint under the camera, PlayerCarry on the player. |
| carry_test1.cs, carry_test2.cs, drop_test.cs, throw_test1.cs | Pick up, set down, drop and throw checks in Play. |
| add_bodies.cs | Rigidbodies on the loose test objects. |
| add_pause_action.cs, add_throw_action.cs | Adds the Pause and Throw actions to InputSystem_Actions. |
| build_pause_ui.cs | EventSystem with the Input System module, PauseMenu canvas, slider, toggle, buttons; GamePause on the Game object. |
| pause_test.cs | Presses Escape through the Input System and checks the pause state. |
| import_textures.cs | Imports the CC0 textures with max size 512 and makes the tiled materials. |
| swap_metal.cs | Swaps tower materials to PaintedMetal006 and checks nothing points at the old texture. |
| build_look.cs | Night lighting rig, fog, the LookFilterFeature on PC_Renderer, the LookTuning asset. |
| wire_look.cs, wire_fog.cs | Wires the feature and fog settings into the renderer and the scene. |
| shoot_look*.cs, shoot_filter.cs, shoot_course.cs, shoot_tower.cs | Milestone 3 screenshots from the three fixed camera spots at various filter settings. |
| make_prefabs.cs | Player, GameSystems and NightLighting prefabs with runtime fallbacks for cross-prefab links. |
| build_base_scene.cs | Templates/Base.unity: the three prefabs, a starter ground, night ambient; source of the scene template. |
| devmenu_test.cs | Presses F1 through the Input System and checks the dev menu opens. |
| check_release_dll.cs | Loads the release build's Assembly-CSharp.dll and proves DevMenu compiled to an empty class. |
| inventory_packs.cs | Lists pack materials by shader and prefab counts after import. |

## Main scene 1.0 (Milestone 5)

| File | Builds or does |
|---|---|
| build_main_terrain.cs | Main.unity from the base recipe: 400 x 400 terrain, cliff, trail polylines, clearings, bounds walls, sunset lighting. |
| build_ward.cs | First Ward: six tapered ProBuilder stones in a ring, rune texture, WardStone material. |
| build_forest.cs | Tree prefab variants with colliders, first tree placement, first ground cover. |
| build_camp.cs | Camp: log cabin, fire pit, seats, props, firewatch tower, generator under Era_Modern, player spawn in the cabin. |
| fix_camp.cs, fix_camp_applied.cs | Camp fix pass: opaque material copies, roof rebuilt with ProBuilder gables, camp pulled in, tower moved. |
| build_campsites.cs | Three campsites: tent, lean-to, fire ring, with usables. |
| build_horizon.cs | Burning ridge: 60 cones, glow strips and patches, smoke and ember systems, sky material, HorizonFire. |
| build_signs.cs | Signposts with arrow boards, tent icon sprite, SignRed material. |
| build_trailhead_signs.cs | Flat trailhead boards at the camp facing the fire. |
| build_valley.cs | ValleyTerrain west of the cliff with 3,500 trees and fire patches. |
| build_devwarps.cs | DevWarps root with the Ward warp for the dev menu. |
| raise_ward.cs | Ward plateau raised 16 m, path A climbing, trails narrowed and flattened across, objects re-seated. |
| build_ward2.cs | Three stones by the cliff, big varied rune sheet, per-stone rune scale. |
| ward_stones_tweak.cs | Outer stones re-sized and leaned; flames scaled down. |
| ward_cliff.cs | Valley floor matched to the valley terrain formula, rock layer on steep ground, trees below the cliff, clearing shrunk. |
| ward_edge.cs | Stones on the ledge edge north of the approach, sign removed, path A extended into the clearing, trees thinned by a tenth. |
| build_forest2.cs | Dense forest: 3 m grid, bigger pines, bushes as tree prototypes, instanced detail material, ground cover everywhere. |
| forest_instancing.cs, valley_instancing.cs | Instanced material copies and prefab variants for every tree and bush prototype (the draw call fix). |
| foliage_variety.cs | Twelve ground-cover kinds with size ranges and tints. |
| moss_sign.cs | Moss layer off trails and clearings; DO NOT ENTER board moved to the mouth of path A. |
| build_tower2.cs | Tower rebuilt with the deck at 15 m, six flights, cross braces. |
| camp_polish.cs | Seats tangent to the fire, tripod, chopping block, wood pile, table, lantern, ladder, ground cover thinned. |
| fire_cliff_fix.cs | Additive flame material, darker rock layer, seam trees along x 0. |
| build_leanto2.cs, build_leanto3.cs | Campsite 2 lean-to from tent pack canvas and poles (leanto3 is the tilted version that was kept). |
| build_office.cs | Entrance: road, fence, double gate, ranger office, parking with tinted cars, lights, bench, phone booth, OFFICE sign, warp. |
| build_east.cs | Road extended to the map edge and EastTerrain beyond the fence. |
| build_lake.cs | Lake: shoreline polygon, bed, water plane, wade limit, dock, rocks, spur, sign, warp. |
| build_tentsite.cs | Campsite 1 as five tents around a copied lit fire pit. |
| build_cabins.cs | Cabin builder; campsite 2 log cabin with porch and campsite 3 two-room plank cabin. |
| build_cave.cs | Cave hill, trench, first ProBuilder ceiling, props, spur, warp. |
| cave_fix.cs, cave_walls.cs, cave_fix2.cs | Rock paint and box walls for the first cave (superseded). |
| cave_widen.cs | Trench widened to 7 m and 9 m, cut painted rock. |
| build_cave2.cs | Cave lined with 180 pack rocks with convex colliders (run after deleting the old rocks). |
| cave_clear.cs | Bounding-box clearance, too coarse, superseded. |
| cave_clear2.cs | Exact clearance: rocks pushed out of capsule probes with Physics.ComputePenetration. |
| copy_main2.cs | Copies Main to Main2 with its own terrain data and adds it to the build list. |

## Main scene 2.0

| File | Builds or does |
|---|---|
| m2_loops.cs | Three loop trails: dock to cabin 2, cabin 2 to cabin 3, cabin 3 to the road. |
| m2_tents_move.cs | Tent site moved to the lake's north shore, new branch, old spur reforested, fork board re-aimed. |
| m2_beats.cs | Trail beats, roadside car, notice board with map texture, player car dressing, spur candles, lantern posts. |
| m2_rolls.cs | Rolling ground outside clearings, trails kept flat across, every object re-seated, cave hill trees and rocks. |

## Screenshot and walk checks

| File | Does |
|---|---|
| shoot_main.cs, shoot_main_final.cs | Milestone 5 camera spots (the Docs/Look/Main shots). |
| shoot_spots.cs, shoot_after.cs, shoot_after2.cs | Trail, ward and tower comparison shots before and after the lighting change. |
| shoot_camp*.cs, shoot_campsites.cs, shoot_signs*.cs, shoot_forest.cs, shoot_horizon*.cs, shoot_ward*.cs, shoot_lake.cs, shoot_office.cs, shoot_cave.cs, shoot_cabins.cs, shoot_tents*.cs, shoot_beats.cs | Reference shots of each area after its build. |
| walk_routes*.cs, walk_A.cs, walk_B.cs, walk_C.cs | Automated walks of the cabin to Ward, Ward to camp and campsite routes. |
| walk_office.cs, walk_lake.cs, walk_cabins.cs, walk_cave.cs, walk_loops.cs, walk_tents2.cs, walk_trunk.cs | Automated walks of the road, lake spur, cabin doors, cave spur, loop trails, new tent branch and path A up to the trunk. |

## Loop and economy (Milestone 7)

| File | Builds or does |
|---|---|
| create_loop_tuning.cs | Creates Assets/Settings/LoopTuning.asset with the draft loop numbers (task 7.1). |
| create_simulator_tuning.cs | Creates Assets/Settings/SimulatorTuning.asset pointing at LoopTuning (task 7.2). |

## Pack shaders (task 7.6)

| File | Builds or does |
|---|---|
| swap_pack_shaders_7_6.cs | Moves every bought-pack material off BK, NatureManufacture and Legacy shaders onto URP Lit, Unlit, Particles Unlit or DieAlone Smoke, Sky and Water. Rerun after any pack re-import. |
| fix_swap_keywords_7_6.cs | One-off cleanup of materials swapped by the first run (stale keywords, detail maps, flame emission). |
| list_material_shaders.cs | Counts every material by shader; done-check is pack shaders = 0 and broken shaders = 0. |
| shoot_shader_swap_7_6.cs | One shot per swapped group in a temporary additive scene, closed unsaved. |

## Look preview (task 7.7)

| File | Builds or does |
|---|---|
| create_look_previews_7_7.cs | LookTuning_DayOne and LookTuning_DayTwo from Style.md (copies of LookTuning.asset, which stays untouched) and the dev-only LookPreview on GameSystems. |
| shoot_look_preview_7_7.cs | Graybox from the three look spots in the active preview look, then selects the next; run once per look in Play mode. Writes Docs/Look/Preview. |

## Main3 blockout (Milestone 8)

One recipe per task, run in task order from a clean Main3 rebuild (see PLAN.md Rules and Tips, MAIN3 TERRAIN).

| File | Builds or does |
|---|---|
| main3_8_1_scene_ground.cs | Main3.unity from the base template, 400 x 300 terrain from Main3.md table 2.1, gray layers, fence on x 396, invisible walls, spawn, DevWarps. |
| main3_8_2_camp_tower.cs | Camp: timber-frame tower with 12 ramped flights, deck, cab, lectern; cabin with door, bunk, desk, report box, stove; fire pit; generator; spawn; deck and cabin warps. |
| main3_walk.cs | Play-mode walk template: drives the CharacterController along waypoint routes and reports stalls. |
| main3_topdown.cs | Top-down image in 100 m tiles to Docs/Layout/Main3/Main3_top.png. |
