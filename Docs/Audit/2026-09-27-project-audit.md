# DieAlone project audit, 2026-09-27

Read-only audit for Cowork. Nothing in the project was changed. Head at the time of the audit: 2b8b995 (Dev menu: styled column with friendly scene names, current scene marked, warps indented under it), committed 2026-09-25.

The Unity Editor bridge (com.unity.pipeline) was not reachable during this audit: `unity status` listed no connected Editor and `unity pipeline list` reported the server as not reachable. Everything below comes from the files on disk, git, and the numbers recorded during the 2026-09-25 session. Anything that needs a running Editor (frame timing, draw calls, console output) is marked as last measured, not re-measured.

## 1. Scripts

Twenty C# files under Assets, none in the packs. Scene usage was read from the scene files (MonoBehaviour script GUIDs) and the prefab files. All twenty are used; none is unused.

Player
- Assets/Scripts/Player/PlayerController.cs: first-person walk, sprint, crouch, jump and look from the Player action map; applies capsule values from PlayerTuning in Awake; shoves rigidbodies on collision; applies saved look sensitivity and invert. On the Player prefab (Graybox, Main, Main2, Base).
- Assets/Scripts/Player/PlayerTuning.cs: ScriptableObject holding every feel number (movement, jump, crouch, look, capsule, interaction, doors, carry, throw). Asset at Assets/Settings/PlayerTuning.asset, referenced by PlayerController, PlayerInteractor, PlayerCarry and every Door.

Interaction
- Assets/Scripts/Interaction/Interactable.cs: abstract base with a prompt string, CanUse and Use. Base of everything below.
- Assets/Scripts/Interaction/PlayerInteractor.cs: raycasts from the eye within interactReach, shows the prompt, calls Use on Interact, sets a carried object down, handles Throw. On the Player prefab.
- Assets/Scripts/Interaction/InteractPromptUI.cs: owns the centre dot and prompt label. On the HUD inside the GameSystems prefab.
- Assets/Scripts/Interaction/Carryable.cs: a loose object the player can pick up. Graybox test objects only; no instance in Main or Main2.
- Assets/Scripts/Interaction/PlayerCarry.cs: holds one Carryable in front of the camera, set down, drop, throw. On the Player prefab.
- Assets/Scripts/Interaction/Door.cs: hinged door that swings away from the user, waits when the player is in the way, and since 2026-09-25 swings relative to its own closed rotation. Instances: Graybox 2, Main 7, Main2 7 (cabin, tower room, office, two campsite cabins, two gate leaves).
- Assets/Scripts/Interaction/FirePit.cs: toggles the burning objects and flickers the fire light. Instances: Main 2, Main2 2 (camp fire, tent site fire).
- Assets/Scripts/Interaction/ToggleColorInteractable.cs: test usable that flips a colour and pops scale. Instances: Graybox 1, Main 5, Main2 5 (tent, bunk, table, fire ring and one older stand-in).
- Assets/Scripts/Interaction/WardStone.cs: toggles the rune emission between dim and bright through a MaterialPropertyBlock. Instances: Main 1, Main2 1 (Stone_2).

Core, settings, UI
- Assets/Scripts/Core/GamePause.cs: single owner of the paused state, time scale, cursor, and which behaviours sleep; finds PlayerController and PlayerInteractor when its list is empty. On the Game object in the GameSystems prefab.
- Assets/Scripts/Settings/PlayerSettings.cs: static settings store with JSON save and load in persistentDataPath. Used by PauseMenu and PlayerController.
- Assets/Scripts/UI/PauseMenu.cs: pause panel on the Pause action, resume and quit buttons, look sensitivity slider and invert toggle bound to PlayerSettings. In the GameSystems prefab.

Look
- Assets/Scripts/Look/LookTuning.cs: ScriptableObject holding the VHS filter, fog, sunset sky, ambient and horizon fire numbers. Asset at Assets/Settings/LookTuning.asset.
- Assets/Scripts/Look/LookFilterFeature.cs: Render Graph ScriptableRendererFeature, game camera only, before post-processing; downsamples camera colour to lowResHeight and blits back through Assets/Shaders/LookFilter.shader. On Assets/Settings/PC_Renderer.asset.
- Assets/Scripts/Look/LookEnvironment.cs: applies fog, sky colours, camera clear flags and sunset ambient each frame from LookTuning; FogSet enum Night or Sunset per scene. On the Game object; Graybox uses Night, Main and Main2 use Sunset.
- Assets/Scripts/Look/HorizonFire.cs: drives glow strip colour and intensity with per-patch flicker, and smoke and ember rates, from LookTuning. On the Horizon root in Main and Main2.

Dev and Editor
- Assets/Scripts/Dev/DevMenu.cs: F1 panel listing build scenes and DevWarps children, compiled only under UNITY_EDITOR or DEVELOPMENT_BUILD. On the Game object in the GameSystems prefab.
- Assets/Editor/NewSceneMenu.cs: menu item DieAlone > New Scene From Base that instantiates Assets/Scenes/Templates/DieAloneBase.scenetemplate and adds the scene to the build list. Editor only.

Shaders (not C#, listed for completeness): Assets/Shaders/LookFilter.shader (VHS filter), SkyGradient.shader (sunset sky), HorizonGlow.shader (additive unlit glow), Water.shader (lake water with waves and sun glint). All four are used.

## 2. Scenes and prefabs

Build list: Graybox, Main, Main2, in that order.

Assets/Scenes/Graybox.unity (test scene, Night fog set). Root objects: Ground; Box_Large_A, Box_Medium_A, Box_Medium_B, Box_Small_A, Box_Small_B, Box_Tall_A (scale boxes); TestCourse (46 objects: tunnel, step, log, fence, ramp, stairs with StairRamp colliders, test room with a door, loose Carryables); TestInteractBox (ToggleColorInteractable); Tower (114 objects: legs, deck, stairs, top room, lamp); Player, GameSystems and NightLighting prefab instances with explicit links kept as instance overrides.

Assets/Scenes/Main.unity (version 1.0, frozen at tag main-scene-1.0, Sunset fog set). Root objects (plain object counts exclude prefab instances):
- Terrain: 400 x 400 m, Main_TerrainData.
- ValleyTerrain: 240 x 720 m west of the cliff, Valley_TerrainData, no collider.
- EastTerrain: 240 x 400 m east of the fence, East_TerrainData, no collider.
- Bounds: four invisible walls (cliff wall 60 m tall at x 40, east, north, south).
- Ward: Stone_1 to Stone_3 on the cliff edge, Stone_2 carries WardStone.
- Camp: Cabin (log modules, door, bunk, table, lamp, roof, gables), FirePit, seats, chair, stool, tripod and pot, chopping block and axe, wood pile, table, lanterns, ladder, barrels, FirewatchTower (15 m deck, six flights with StairRamps, top room with desk and door).
- Era_Modern: Generator.
- Campsites: Campsite_1_Tents (five tents, lit fire, seats, table, gear), Campsite_2_Cabin (log cabin with porch, cold fire, tripod, seat, bucket), Campsite_3_Cabin (plank cabin, two rooms, fire ring group, barrel, wood pile).
- Horizon: Ridge (60 ProBuilder cones and a floor slab), Glow (21 strips, 14 patches, 16 valley patches), FX (six smoke, six ember systems), HorizonFire component on the root.
- Signs: Sign_TrailA (DO NOT ENTER), Sign_TrailB (CAMPSITES), Sign_Fork (three tent boards), Sign_Road (OFFICE), Sign_Lake (LAKE), each with Board children.
- Lake: Water plane, WadeLimit (84 box colliders), Dock (planks, posts, rail, lantern), ShoreBench, ten ShoreRocks, ShorePack.
- Entrance: Fence (58 panels, 59 posts), Gate (two Door leaves with rails and posts), Office (plank modules, door, counter, chair, shelf, trunk, lamp, light, sign), Parking (four Vehicle_Body cars with tinted materials, one open door), three Street_Lights with point lights, Bench, PhoneBooth, barrel.
- Cave: 180 pack rocks (tunnel walls and ceiling, chamber ring and ceiling, mouth), cold fire, fur bedroll, crate with candles, red point light. Terrain trench under it.
- DevWarps: Camp, Tower deck, Ward, Lake, Campsite 1 tents, Campsite 2 cabin, Campsite 3 cabin, Cave, Office.
- Player, GameSystems, NightLighting prefab instances (Sun active, Moon off).

Assets/Scenes/Main2.unity (version 2.0, the working scene, Sunset fog set). Same roots as Main plus Beats, on its own terrain data (Main2_TerrainData, Main2_Valley_TerrainData, Main2_East_TerrainData). Differences from Main: three loop trails (dock to cabin 2, cabin 2 to cabin 3, cabin 3 to the road); the tent site moved to the lake's north shore at fire (208, 147) with a new branch from the fork and the old spur reforested; rolling ground outside clearings with a rise between camp and lake; trees and 15 rocks on the cave hill; Beats root holding FallenTrunk (crouch-under), HuntingStand, TrailShrine, OldSignpost, RoadsideCar, NoticeBoard (CampMap material), PlayerPack, SeatBlanket, DroppedKey, three SpurCandles, four LanternPost groups. Prefabs present only in Main2: CITW_Blanket, Car_Key, CITW_Book_4, CS_Backpack_Modern_3, CS_Backpack_Old_2.

Assets/Scenes/Templates/Base.unity: Ground plane plus the three prefab instances; source of DieAloneBase.scenetemplate.

Project prefabs (Assets/Prefabs) and where they are instanced:
- Player.prefab (PlayerController, PlayerInteractor, PlayerCarry, Main Camera, HoldPoint): once in Graybox, Main, Main2, Base.
- GameSystems.prefab (Game with GamePause, LookEnvironment, DevMenu; EventSystem with InputSystemUIInputModule; HUD with dot and prompt label; PauseMenu canvas with panel, slider, toggle, buttons): once in each of the four scenes.
- NightLighting.prefab (Moon directional light, Sun directional light disabled by default; Main and Main2 enable Sun and disable Moon as overrides): once in each of the four scenes.
- Forest/Tree_Pine1, Tree_Pine2, Tree_Pine3, Tree_Pine5, Tree_Aspen1, Tree_Birch1: variants of the suffercord tree prefabs with a CapsuleCollider and instanced materials; used as terrain tree prototypes on Main, Main2 and East terrains (about 18,900 trees on Main2).
- Forest/Bush_Bush1 to Bush_Bush4: variants of the suffercord bushes, instanced materials, no collider; terrain tree prototypes on Main and Main2 (about 5,200).
- Forest/Valley_Pine1, Valley_Pine2, Valley_Pine4, Valley_Aspen1Leafless, Valley_Aspen3Leafless, Valley_Birch1Leafless: variants with instanced materials; prototypes on the valley terrains and on the main terrains for the strip below the cliff.
- Forest/Detail_Grass1 to Grass5, Detail_Fern1 to Fern3, Detail_Nettle1, Detail_Nettle2, Detail_Mushroom1, Detail_Mushroom3: variants on Assets/Materials/Foliage_Detail.mat; terrain detail prototypes on Main and Main2 (about 340,000 clumps).

Pack prefabs instanced in Main2 (count): Chain_Link_Fence_Post 59, Chain_Link_Fence 58, CITW_Floor 32, CS_Rock_1 to CS_Rock_8 207 in all, CITW_Plank_Wall 16, CITW_Log_Wall 10, CITW_Plank_Window_Wall 9, FX_Embers 7, FX_Smoke_Thick_Tall 6, CITW_Log_Window_Wall 6, CITW_Candle_2 6, Vehicle_Body 5, CS_Lantern_Old_Rusted 5, CITW_Door_Frame 4, CITW_Candle_1 4, CITW_Bed 4, Street_Light 3, CS_Stone_1 3, CS_Stone_2 3, CS_Log_Large_Seat_3 3, CS_Firewood_Logs 3, CITW_Window_Frame 3, CITW_Table 3, CITW_Railing 3, CITW_Plank_Doorway 3, CITW_Door_1 3, CITW_Crate 3, and one or two each of Car_Door, CS_Stone_3 to CS_Stone_8, CS_Log_Large_Short, CS_Log_Large_Seat_1 and 2, CS_Lantern_Old, CS_Lantern_Modern, CS_Firewood_Logs_Burnt, CS_Campfire_Tripod_Wood, CS_Campfire_2, CS_Backpack_Modern_1, CITW_Wood_Pillar, CITW_Shelf, CITW_Oil_Lamp_1 and 2, CITW_Log_Doorway, CITW_Ladder, CITW_Chair, CITW_Bucket, Bench, Telephone_Booth, FX_Smoke_Thin, FX_Flames_Tall, FX_Flames_Short, Checkout_Counter, Car_Key, CS_Tool_Shovel_Wood_Rusted, CS_Tool_Axe_Wood, CS_Tent_Old_1, CS_Tent_Old_3, CS_Tent_Modern_1, CS_Tent_Modern_2, CS_Tent_Large_Old_Preset_1, CS_Table_Small_Primitive_1, CS_Table_Small_Modern_1, CS_Table_Big_Modern_1, CS_Stool_1 and 2, CS_Log_Stool_1, CS_Log_Large_Long_Seat_2, CS_Firewood_Short_1 and 2, CS_Drink_Whiskey, CS_Cookware_Pot_1 and 2, CS_Cookware_Pan_1, CS_Cookware_Kettle_1 and 2, CS_Chair_1 and 2, CS_Campfire_Tripod_Metal, CS_Campfire_1, CS_Bedroll_Modern_Rolled_1, CS_Bedroll_Fur_2, CS_Backpack_Old_1 to 4, CS_Backpack_Modern_2 and 3, CITW_Wood_Stove, CITW_Trunk_1, CITW_Stool_1 and 2, CITW_Nightstand, CITW_Mug, CITW_Door_2 and 3, CITW_Canned_Food_2, CITW_Book_2 and 4, CITW_Blanket, CITW_Barrel_1 to 4. Main has the same set less the five Main2-only prefabs.

## 3. Tuning and settings

Assets/Settings/PlayerTuning.asset (PlayerTuning). Serialized values: walkSpeed 2.5, sprintSpeed 5.5, crouchSpeed 1.5, gravity 20, jumpHeight 0.6, coyoteTime 0.1, standHeight 1.8, crouchHeight 1, standEyeHeight 1.6, crouchEyeHeight 0.8, crouchTransitionSpeed 6, mouseSensitivity 0.1, gamepadLookSpeed 120, pitchLimit 85, capsuleRadius 0.35, stepOffset 0.1, skinWidth 0.035, interactReach 2, doorOpenAngle 90, doorSwingSpeed 150. The carry and throw fields (carryHoldOffset (0.3, -0.25, 0.7), carryFollowSpeed 14, placeReach 2, placeMinUpNormal 0.7, dropForwardSpeed 1, pushPower 2, throwSpeed 7, throwLift 1.5) are not present in the asset file and take the script defaults; they will be written the next time the asset is saved.

Assets/Settings/LookTuning.asset (LookTuning), current values including the owner's Play-mode tweaks of 2026-09-25: filterEnabled on, lowResHeight 875, colorBleed 0.508, washOut 0.068, crushBlacks 0.382, grainStrength 0.067, grainSpeed 7.7, noiseBandStrength 0, noiseBandSpeed 0.6, noiseBandInterval 6, scanLines 0.3, blur 0.109, darkCorners 0.6, fogEnabled on, fogColor (0.02, 0.03, 0.05), fogStart 20, fogEnd 200, sunsetFogColor (0.62, 0.36, 0.22), sunsetFogStart 25, sunsetFogEnd 320, sunsetAmbient (0.62, 0.44, 0.36), skyTop (0.22, 0.11, 0.10), skyHorizon (0.85, 0.40, 0.18), skyGround (0.30, 0.16, 0.10), sunGlowColor (1, 0.55, 0.25), sunGlowSize 24, fireGlowColor (1, 0.42, 0.10), fireGlowIntensity 2, fireSmoke 1, fireEmbers 1, fireFlicker 0.3, fireFlickerSpeed 0.8. Note the Night fog values (20 to 200) differ from the script defaults (6 to 40); Graybox uses these.

Other ScriptableObject assets: none written by the project. Assets/Settings/DefaultVolumeProfile.asset is the URP template volume profile with many overrides (LiftGammaGain, SplitToning, MotionBlur, ColorAdjustments, FilmGrain, Tonemapping, Bloom, Vignette, DepthOfField and others) plus two components whose scripts no longer resolve. It has no effect in play: the Player prefab's camera has no UniversalAdditionalCameraData saved, so URP adds one at runtime with post-processing off, and no Volume component exists in any scene. SampleSceneProfile.asset is the template's sample profile, unused.

Persistent JSON settings: Application.persistentDataPath/settings.json, on this machine C:\Users\grant\AppData\LocalLow\DefaultCompany\DieAlone\settings.json. Keys: lookSensitivity (float, clamped 0.2 to 3, currently 1.0014), invertLook (bool, currently false). Loaded on first access, saved by the pause menu.

Input: Assets/InputSystem_Actions.inputactions. Player map: Move, Look, Sprint, Crouch, Jump, Interact, Throw, Pause, plus the template's Attack, Previous, Next, Sprint bindings; UI map: Navigate, Submit, Cancel, Point, Click, RightClick, MiddleClick, ScrollWheel, TrackedDevicePosition, TrackedDeviceOrientation. Control schemes Keyboard and Mouse, Gamepad, Touch, Joystick, XR.

URP. Active pipeline asset: Assets/Settings/PC_RPAsset.asset (Graphics settings and the PC quality level; the Mobile quality level points at Mobile_RPAsset, render scale 0.8). PC_RPAsset: render scale 0.5, upscaling filter automatic, MSAA off, HDR on, main light shadows on at 2048, additional light shadows on at 2048, additional lights per pixel, shadow distance 50 m, two cascades, soft shadows on. Renderer Assets/Settings/PC_Renderer.asset carries two features, both active: LookFilterFeature (the VHS look) and ScreenSpaceAmbientOcclusion (the template's SSAO, never tuned for this project). Post-processing is off on the game camera. Assets/Settings/UniversalRenderPipelineGlobalSettings.asset is the default.

Project settings of note: productName DieAlone, companyName DefaultCompany (still the template value, and it names the persistent data folder), bundleVersion 0.1.0, runInBackground 0.

## 4. Packages

Packages/manifest.json dependencies: com.unity.ai.navigation 2.0.14, com.unity.collab-proxy 2.13.6, com.unity.ide.rider 3.0.40, com.unity.ide.visualstudio 2.0.26, com.unity.inputsystem 1.20.0, com.unity.multiplayer.center 1.0.1, com.unity.pipeline 0.7.0-exp.1 (the Editor bridge used by Code), com.unity.probuilder 6.1.2, com.unity.render-pipelines.universal 17.3.0, com.unity.test-framework 1.6.0, com.unity.timeline 1.8.13, com.unity.ugui 2.0.0, com.unity.visualscripting 1.9.12, plus the standard built-in modules (accessibility through xr). TextMeshPro is not imported. Navigation, multiplayer center, timeline, visual scripting and collab-proxy are template leftovers and unused.

Asset Store packs, present locally and git-ignored, listed in Assets/SOURCES.md with store links:
- Celestia Studio PSX Modular Complete Pack: Assets/Celestia_Studio/PSX_Modular_Complete_Pack, 1,534 files, 54 MB. Used for the fence, gate panels, cars, street lights, bench, phone booth, counter, car key.
- Revolving Pizza Games Campsite: Assets/Revolving Pizza Games/Campsite, and Cabin In The Woods: Assets/Revolving Pizza Games/Cabin In The Woods, together 1,355 files, 110 MB. Used for cabins, tents, campfire and FX, rocks, stones, seats, tools, cookware, lanterns, props.
- suffercord PSX Autumn Forest Pack: Assets/suffercord/PSX Autumn Forest Asset Pack, 62 files, 4.6 MB. Used for every tree, bush and ground cover through the project's prefab variants.
Also ignored: the pack folder .meta files, Claude outputs/, UpgradeLog.htm, Build/, UserSettings/, .vs/, the generated csproj and sln files.

CC0 textures from ambientCG, committed through LFS: Ground054, PaintedMetal006, Concrete034, Planks023A (colour maps only, max size 512). Project-generated textures: Runes_Emissive.png, TentIcon.png, CampMap.png.

## 5. DESIGN.md systems

- HP, MIND, WARD stats: none. No stat code exists.
- Weekly drain: none.
- Day and sleep cycle: none. No time-of-day or sleep logic; the scene is a fixed sunset. Milestone 6 headline only.
- Autosave: none. The only persistence is settings.json.
- Event deck and tracks: none.
- Chapters and bottoming out: none.
- Forage and chores: none. The closest thing is carrying one loose object in Graybox.
- Check the fire: stub. The tower deck at 15 m looks at a burning ridge driven by HorizonFire, but nothing records or reacts to looking.
- Report and journal: none.
- Ward interaction and feeding: stub. WardStone.Use flips rune emission on Stone_2 with the prompt "Touch the stone"; no cost, no stat.
- Inventory: none. PlayerCarry holds one visible object and the design says carrying is not the inventory.
- Memories: none.
- Scrapbook: none.
- Map: stub. The 2.0 notice board shows a drawn map texture; it is not an item and cannot be picked up.
- Ward runes as objects: none. Runes are a texture on the stones.
- Voices: none.
- Ahmee: none.
- Minigames: none.
- Monster and hiding: none.
- Endings: none.
- Main menu: none. Play starts in the scene; there is no title or menu scene.
- Settings menu: working for its current size. PauseMenu on Escape or Start with resume, quit, look sensitivity and invert look, saved to JSON.
- Controller support: working for movement, look, jump, sprint, crouch, interact, throw, pause and UI navigation through the Input System action asset; verified with keyboard and mouse, gamepad bindings exist and the owner reported a gamepad check in Milestone 2.

## 6. Known problems

Bugs and rough edges
- The standing player cannot pass the fallen trunk on path A in Main2; crouching is required and the automated walk stops there. Intended, but there is no prompt or hint.
- Cave: the walls are 180 pack rocks pushed out of a walkable corridor; up close the overlap reads as a pile, and small gaps can show terrain between rocks. The tunnel stalls briefly at the mouth in the automated walk (65 stalls, passes).
- Gate leaves are chain-link panels stretched to 4 m; they read thin. The four lot cars are one model in four paints.
- Lantern posts in 2.0 have no lights. Street lights and cabin lights are on during a sunset scene.
- Terrain trench walls under the cave hill are near-vertical terrain; the rock paint blends with the floor dirt at the cell boundary.
- The office door and cabin doors open away from the user by the sign of the hinge forward; nothing stops a door from swinging into a wall corner.
- The lake WadeLimit is 84 separate box colliders at fixed height; if the water level changes they need moving.
- ToggleColorInteractable stand-ins carry prompts such as "Search the table" and "Check the bunk" that do nothing but flash.
- Carry, throw and Carryable exist only in Graybox; Main has no loose objects.
- The old fork board in Main (1.0) still points at the tents' old position because that scene is frozen; Main2 re-aimed it.
- PlayerTuning.asset lacks the carry and throw fields on disk (script defaults apply until it is re-saved).
- DefaultVolumeProfile.asset carries two components whose scripts are missing; harmless while post-processing is off, but it will warn if a Volume is ever added.

Performance (last measured 2026-09-25, PC_RPAsset, render scale 0.5, Editor Game view at 3840 x 1976, not re-measured in this audit because the bridge was unreachable)
- Before the instancing fix: 16,323 draw calls, 15,630 SetPass calls, 21.3 million triangles, 36 million vertices at the cabin spawn.
- After instanced tree materials, tree distance 160 m, detail distance 50 m, two cascades: 333 draw calls, 80 batches, 6.55 million triangles, 8.3 million vertices, GPU 1.9 ms, CPU main thread 9 ms at the cabin spawn; about 11 ms per frame averaged over 700 frames at the Ward ledge.
- The lake, the gate, the 2.0 tent site and the cave were not timed. The lake adds a 130 x 94 m transparent plane, the entrance adds 117 fence pieces and five cars (23 colliders each), the cave adds 180 convex mesh colliders. Expect the office lot to be the heaviest spot after the cabin because of the cars and lights.
- SSAO is active on the PC renderer and was never measured on or off.
- Memory in the Editor was about 2.1 GB allocated, 1.1 GB mono heap.

Console on Play (last seen 2026-09-25)
- "The tree X must use the Nature/Soft Occlusion shader. Otherwise billboarding/lighting will not work correctly." for every tree prototype, twice each on scene load. Harmless for mesh trees, but it is 30 lines per load.
- "Reduced additional punctual light shadows resolution by 2 to make 12 shadow maps fit in the 2048x2048 shadow atlas." repeated per frame near the camp and office. Point lights with shadows: fire pit, cabin, office, three street lights, cave glow, tent fire.
- Bridge messages "Main thread operation timed out after 5000ms" come from com.unity.pipeline, not project code.
- Two terrains with different heightmap resolutions warned about neighbouring until allowAutoConnect was turned off; it is off on all three.

Fragile
- Every rebuild recipe referenced in Rules and Tips (build_lake.cs, build_cave2.cs, m2_rolls.cs and about 110 more) lives in Code's session scratchpad under AppData\Local\Temp, not in the repo. If that folder is cleared the scenes can still be edited, but nothing can be regenerated from parameters. Copying the scratchpad into Docs or Tools would remove this risk.
- Terrain edits are tied to polyline lists duplicated across scripts; changing a trail means updating several copies.
- GameObject.Find by name returns DevWarps children for Ward, Lake and Cave; scripts must look up scene roots.
- MaterialPropertyBlocks are not saved with scenes; anything persistent must be a material asset.
- The Editor bridge runs in whatever mode the Editor is in; the owner enters Play often, and one warp once saved the player at the Ward in edit mode.

## 7. Drift

In the project but not in Rules and Tips or DECISIONS.md
- ScreenSpaceAmbientOcclusion is active on PC_Renderer. Not decided, not noted.
- Render scale 0.5 and two shadow cascades are noted under Rules and Tips (performance) but not recorded as a decision.
- companyName is DefaultCompany, which fixes the persistent data folder name. Not decided.
- The template's DefaultVolumeProfile and SampleSceneProfile remain in Assets/Settings, unused.
- Unused template packages remain in the manifest (ai.navigation, multiplayer.center, timeline, visualscripting, collab-proxy).
- The lake water shader, Water.mat values, and the lake outline control points are in Rules and Tips only through the script name; the outline itself lives in the scratchpad.
- The forest tree counts in Rules and Tips (18.9k after thinning) predate the 2.0 tent move, reforestation, hill trees and loops, so Main2 differs by a few hundred either way.
- The Night fog values in LookTuning (20 to 200) differ from the script defaults and from the "6 to 40" that Milestone 3 tuning implied; not recorded.

In DECISIONS.md or DESIGN.md that the project no longer matches
- DECISIONS 2026-09-22 says textures are listed in Assets/Textures/SOURCES.md. The file is Assets/SOURCES.md, which holds both packs and textures.
- DECISIONS 2026-09-24 says "three small campsites off the camp". There are now six locations: tents, two cabins, lake with dock, front office with gate and parking, cave.
- DECISIONS 2026-09-24 says the Ward is "a cluster of huge rune-covered standing stones on a cliff edge". It is three stones in a row on the ledge edge; close enough, but the earlier six-stone ring and its position are gone.
- DECISIONS 2026-09-24 says unique assets stay as blockout. The tower, cabins, office, dock and cave are still blockout or pack assembly, which matches; the Ward stones are ProBuilder, which matches.
- DECISIONS 2026-09-20 says the PS1 effects stay off. Matches; the look is VHS. CLAUDE.md's first line still says "PS1/PSX-style".
- DESIGN.md setting: "Nearby: the Ward, forage spots, a creek, forest edge" and "Some locations only exist once an event track unlocks them (meadow, black lake, cave, grave, shrine, pipes, cliffs)". The lake and cave exist from the start and are open; there is no creek, meadow, grave, pipes or shrine (the 2.0 trail shrine is a prop, not a location).
- DESIGN.md says the player is stationed alone at a firewatch tower. The player wakes in a cabin beside the tower, and other campers are implied by the campsites and cars.
- DESIGN.md's mid-17th century Ancerra setting is superseded by the two-era decision, and the scene is Modern era (cars, generator, phone booth, chain-link fence); DESIGN.md has not been updated as the 2026-09-24 decision said it would be.
- DESIGN.md Presentation says vertex jitter, affine textures, dithering and limited palette; DECISIONS replaced that with the VHS look on 2026-09-20. DESIGN.md still carries the old line.
- PLAN.md 5.10 describes a walk with the VHS look; the work done under it became a full layout pass and a second scene. Nothing in PLAN.md names Main2 as a task.

## 8. Repo state

- Head: 2b8b99589396ec394fd62785c0174a81e8836603 on main, 139 commits, last commit 2026-09-25 17:58 local time.
- git status: clean, nothing staged or untracked.
- Tags: graybox-m3 (Graybox test scene at the end of Milestone 3), main-scene-1.0 (Main scene 1.0 blockout). Both pushed to origin.
- Remote: https://github.com/QuasiGrant/DieAlone.git, public.
- LFS: 13 files tracked (seven textures, six screenshots), all present as real content locally. Scenes and terrain data are plain YAML, Main.unity 5.0 MB, Main2.unity 5.2 MB, Main_TerrainData 5.2 MB, Main2_TerrainData 5.2 MB, .git 38 MB.
- Ignored and material: the three pack folders (about 170 MB, 2,951 files) and their .meta files; Build/ (a release build was made once for the dev-menu check); UserSettings/; Claude outputs/; UpgradeLog.htm; the generated solution and project files.
- Not in the repo and not ignored by rule: nothing. The scratchpad scripts are outside the project folder entirely.
- Restore: cloning the repo and re-importing the four packs from the owner's Unity account (Package Manager, My Assets) restores the project. The scenes reference pack prefabs by GUID, and the packs keep their GUIDs on re-import, so instances resolve. The URP conversion of pack materials must be run again after import (Rules and Tips has the MaterialUpgrader recipe) or the pack materials show pink, and the instanced material copies under Assets/Materials/Forest and Cars are committed so the trees and cars keep their tints. Library is rebuilt on open. The only things that cannot be restored are the scratchpad rebuild scripts and the release build.
