# Plan

How this file works:
- Cowork writes and orders the tasks. Code does them one at a time, top to bottom.
- Code may tick a box and add lines under Rules and Tips. Code does not add, remove, reorder, or reword tasks.
- A task is finished when its done-check passes, its box is ticked, and the work is committed and pushed.
- Only the current milestone is broken into tasks. Later milestones stay as headlines until they are next.

## Milestone 1: Walk around a gray test scene

- [x] 1.1 Test scene. Create `Assets/Scenes/Graybox.unity`: flat ground about 50 m across, one tall box standing in for the tower, five or six smaller boxes of different sizes scattered around for scale, one directional light. Use Unity's built-in shapes and the default gray material. Build it through the Editor bridge or an Editor script, never by writing the scene file by hand. Done when: the scene is saved, opens with no console errors, and a screenshot shows the layout.
- [x] 1.2 Player that walks and looks. In Graybox, add a Player with a CharacterController and a camera at about 1.6 m eye height. One script, `Assets/Scripts/Player/PlayerController.cs`: walk with the Move action, look with the Look action, gravity, mouse cursor locked during play. Read input from the Player map in the existing `Assets/InputSystem_Actions.inputactions` so keyboard/mouse and gamepad both work. Walk speed slow and steady, starting at 2.5 m/s. No jump, sprint, crouch, or interact yet. Done when: clean compile, Play mode starts with no console errors, and the owner has walked around the scene with keyboard and mouse (and a gamepad if one is plugged in) and confirmed it.
- [x] 1.3 Remove template leftovers. Delete `Assets/TutorialInfo`, `Assets/Readme.asset`, and `Assets/Scenes/SampleScene.unity` with their .meta files, through the Editor so references stay clean. Make Graybox the only scene in the build list. Done when: clean compile, no console errors, Graybox still plays.

## Milestone 2: Controls and interaction

In every task below, "no errors" means no console errors from project code.

- [x] 2.1 Test course. In Graybox, add gray test pieces built from Unity's basic shapes: a long straight lane for sprinting, a low tunnel that only fits a crouched player, a low step and a log to hop over, a fence too tall to jump, a ramp, a short flight of stairs, a small room with a doorway ready for the door task, and a table and a shelf holding a few small loose objects. Build through the Editor bridge or an Editor script. Done when: the scene opens with no errors, a screenshot shows each piece, and the owner has walked the course and confirmed nothing blocks the player by accident.
- [x] 2.2 Tower with stairs and a top room. Replace the tall box with a gray tower about 10 m tall on four legs, switchback stairs up to the top, and simple rails so the player cannot walk off the edge. On top, a room about 4 by 4 m with a walkway around it, a doorway ready for the door task, window openings on all four sides, and inside a table and a bed-sized box as stand-ins for the desk and bunk. Sizes are for testing, not final. No fall damage. Done when: no errors, screenshots show the stairs and the room, and the owner has climbed to the top, walked around inside, and come back down without getting stuck.
- [x] 2.3 Sprint. Hold the Sprint action to move at 4.5 m/s instead of 2.5. No stamina limit. Done when: clean compile, no errors, and the owner has confirmed it on the sprint lane.
- [x] 2.4 Crouch. Hold the Crouch action to lower the camera and the CharacterController and move slower. The player cannot stand up under a low ceiling. Done when: clean compile, no errors, and the owner has gone through the low tunnel and confirmed they cannot stand up inside it.
- [x] 2.5 Jump. A small hop of about 0.6 m on the Jump action, only while grounded. Done when: clean compile, no errors, and the owner has confirmed the player clears the low step and the log and does not clear the fence.
- [ ] 2.6 Interact base. A small dot in the center of the screen. Looking at a usable object within 2 m shows a short prompt, and the Interact action uses it. One reusable way to mark an object as usable, so doors and pickups build on it. Add one test box that reacts visibly when used. Done when: clean compile, no errors, and the owner has used the test box with keyboard and mouse.
- [ ] 2.7 Doors. A hinged door that opens and shuts with Interact, built on 2.6. Place one in the test room doorway and one in the tower room doorway. The door must not shove or trap the player. Done when: clean compile, no errors, and the owner has opened and shut both doors from both sides.
- [ ] 2.8 Pick up and set down. Built on 2.6. Pick up one loose object, carry it in front of the player, and set it down where the player is looking. One object at a time. Not an inventory. Done when: clean compile, no errors, and the owner has moved an object from the table to the shelf.
- [ ] 2.9 Pause menu. Escape or gamepad Start pauses the game, frees the mouse cursor, and shows Resume and Quit plus two settings: look sensitivity and invert look. Settings persist between sessions. Before building, say which Unity UI system you will use (UI Toolkit or uGUI) and why. Done when: clean compile, no errors, and the owner has paused, changed both settings, resumed, and seen the settings kept after restarting Play mode.

## Milestone 3: PSX look on the test scene

Blocked by the open decision on the PSX look approach. Gets broken into tasks once that is decided.

## Milestone 4: One full day of the loop, no events

Wake, chores, check the fire, report, maintain the Ward, sleep. HP, MIND, and WARD working. Autosave at sleep.

## Later, from DESIGN.md

Main menu with New run and Continue comes after Milestone 4, since it depends on saving. Volume, graphics, and control rebinding settings come once there is sound and the PSX look to adjust.

Event deck and tracks, chapters and endings, voices, scrapbook, inventory, map, and Ward as physical objects, Ahmee, minigames.

## Open decisions (owner)

- Level blocking tool: Unity's built-in shapes or the ProBuilder package. Needed before the real tower area is blocked out. Not needed for Milestone 1.
- PSX look approach: a shader written for this project or an existing kit. Needed before Milestone 3.

## Rules and Tips

Lessons from finished tasks that later tasks need. One line each, newest at the bottom.
- Editor eval (eval_file / unity command eval) wraps code in a method body: no `using` lines, use fully qualified names like UnityEditor.SceneManagement.EditorSceneManager. Screenshot save_path lands under Assets/, so save to the scratchpad copy and delete the Assets/Temp asset through the Editor afterward.
- Play mode does not tick while the Editor is unfocused. During bridge-driven play checks, eval `UnityEngine.Application.runInBackground = true` (runtime only, not saved) before waiting on frames.
- Deleting scripts through the Editor triggers a domain reload that drops the bridge mid-batch; wait for editor_status ready and retry the remaining calls. Build-list edits stay in memory until `UnityEditor.AssetDatabase.SaveAssets()` writes ProjectSettings/EditorBuildSettings.asset.
- Course geometry in Graybox: tunnel clearance 1.2 m, step 0.4 m, log 0.4 m, fence 1.5 m, ramp 14 degrees to a 1.5 m platform, stairs 0.25 rise 0.3 run, room doorway 1.0 x 2.1 m. For per-piece screenshots, render a temporary hidden camera to a RenderTexture from eval and write PNGs to the scratchpad; it leaves the scene clean.
- Tower layout: deck top at y 10, room interior 4 x 4 centered at (0, 10, 12), doorway in the south wall centered at x -1.1 (1.0 x 2.1 m), stair entry at ground on the north lane at x -1.5, z 8.4. Stacked stair flights must be slabs, not ground-to-top solids, or the upper flight swallows the lower one.
- Sprint speed is 5.5 m/s in code, owner raised it from the 4.5 in task 2.3 after testing. Cowork to record in DECISIONS.md.
- Player capsule: stand 1.8 m, crouch 1.0 m, radius 0.35, eye 1.6 / 0.8. Stand-up check is a SphereCast up from the crouched capsule top; to test it from eval, teleport the Player (disable the CharacterController around the position set, then Physics.SyncTransforms) and invoke HasHeadroom by reflection.
- STAIRS RULE: the Player CharacterController has stepOffset 0.1 and skinWidth 0.035, so it cannot climb steps. Every staircase gets a collider-only sloped box (a GameObject named StairRamp with just a BoxCollider) whose top face runs along the tread nosings, from one run below the first nosing to the top nosing. See Tower/Stairs/Flight1/StairRamp for the pattern. Reason: with stepOffset 0.3 the capsule slid up over any 0.35 to 0.65 m edge when moving sideways, so hop-only obstacles were climbable without jumping.
