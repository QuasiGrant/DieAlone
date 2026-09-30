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

## Day-one look, menus and the dev panel (tasks 8.9g to 8.9i), in run order

| File | Builds or does |
|---|---|
| look_day_one_8_9g.cs | Sets LookPreview.startLook to Day one on GameSystems and checks the day-one and night values (in the Main3 runner). |
| look_day_one_8_9g_pose.cs | Play mode: poses the camera for the day-one shots (under the giants, the cabin, S1) for Docs/Look/DayOneFix. |
| ui_game_view_size_8_9h.cs | Sets the Game view to a fixed test size or back to Free Aspect, and resets its Scale slider to 1x. |
| ui_canvas_fit_8_9h.cs | Adds CanvasFit to the PauseMenu and HUD canvases and spans the HUD prompt across the screen (run before the next). |
| ui_reference_1080_8_9h.cs | Moves the player canvases to the 1080-row reference with a 0.667 floor, scaling their layout by 1.5 once. |
| ui_fit_check_8_9h.cs | Play mode: checks the dev panel, pause menu or HUD fits the screen and names the smallest text (14.7 px for player menus). |
| ui_pad_nav_check_8_9h.cs | Play mode: walks the dev panel with a virtual gamepad down to the LOOK rows, checking each focused row is in view. |
| dev_look_night_8_9i.cs | Names the scene's own look "Night" in the dev panel's LOOK rows. |
| dev_look_switch_8_9i.cs | Play mode: clicks a LOOK row in the dev panel at S1 and reports the night or day state for the day and night shots. |

## Main3 blockout (Milestone 8)

One recipe per task, run in task order from a clean Main3 rebuild (see PLAN.md Rules and Tips, MAIN3 TERRAIN).

| File | Builds or does |
|---|---|
| main3_8_1_scene_ground.cs | Main3.unity from the base template, 400 x 300 terrain from Main3.md table 2.1, gray layers, fence on x 396, invisible walls, spawn, DevWarps. |
| main3_8_2_camp_tower.cs | Camp: timber-frame tower with 12 ramped flights, deck, cab, lectern; cabin with door, bunk, desk, report box, stove; fire pit; generator; spawn; deck and cabin warps. |
| main3_8_3_trails_giants.cs | Every route flattened and painted to its table 4 length, points of interest as gray stand-ins, the three hero trees with mesh colliders and the giant field; prints route lengths. |
| main3_8_4_lake.cs | Lake water, wade limit, dock with the pump, boathouse on stilts with gangway and the resident spot. |
| main3_8_5_campsites.cs | Camp 1 workshop with spar and bulbs, Camp 2 granite stack with boulder field, ladder, tent on top and rain barrel, Camp 3 tent, fire and Snag lantern; resident spots. |
| main3_8_6_front_zone.cs | Lot, drive, turning circle, spur and loop surfaces, office, store, mast, resident car, booth, chain, gate with the always-on blocker, inactive shift walls. |
| main3_8_6_fence_check.cs | Play-mode check that the player cannot pass the gate or fence. |
| main3_8_7_ward.cs | Cairn gate at J, the Tor dome, the three Ward stone stand-ins on the ledge. |
| main3_8_8_cave.cs | Cave mouth hole and rock block, entrance, three switchback legs, turns, chamber, day-one board; prints the terrain clearance and hole check. |
| main3_8_9_sightlines.cs | Tower sightline report: places seen from the deck grid, W-1 and C-1 hidden margins, cab from the junctions; F-1 (8.9j): every flame top hidden by terrain from every place, trail point at 10 m and the deck grid, except past the fin (the reveal). |
| main3_rebuild.sh, main3_reset.cs | Runner: rebuilds Main3 from 8.1 in order as detached Editor jobs and stops at the first failure. |
| main3_8_9a_fix_check.cs | Play-mode check (walk, hop, crouch) of every spot the 8.9a and 8.9b fix batches changed. |
| main3_walk_legs.cs | Play-mode reach check along chained trail legs and waypoints. |
| main3_walk_trails.cs | Play-mode walk of every leg under Trails, both ways. |
| main3_walk.cs | Play-mode walk template: drives the CharacterController along waypoint routes and reports stalls. |
| main3_topdown.cs | Top-down image in 100 m tiles to Docs/Layout/Main3/Main3_top.png. |
| main3_e1_edges.cs | E-1 (Edges.md 1.8): every look toward the map edge ends on land (terrain, landscape meshes) or sky above level; Marlow's grazing rays; report Docs/Layout/Main3/Main3_E1.md. In the runner since 8.9j. |
| main3_8_9j_climb_check.cs | Play-mode walk of the Ward climb from J to the ledge path end, timed at walk speed; ledge edge and bench edge stops. |
| main3_day_one_state_check.cs | Edit mode, WalkChecks 12: the fire on (8.9j), the cairn gate and cave board on, shift walls off, scene not dirty. |
| main3_edge_shots_8_9k.cs | Play mode: one shot of each map edge (N, S, E, W from the floor, and west from the ledge) to Docs/Look/Edges. |
| main3_climb_push_check_8_9k.cs | Play mode: side pushes (25 m walks, 12-direction sprint-jumps) from every Ward climb trail point, judged against the trail where they land, plus the ledge lip pushed west. |
| main3_straight_view_climb_8_9k.cs | Edit mode, WalkChecks 11 for the climb: longest straight view per part (J to leg 1, legs, leg 5, cleft, exit and ramp). |
