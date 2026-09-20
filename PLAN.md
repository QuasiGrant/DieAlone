# Plan

How this file works:
- Cowork writes and orders the tasks. Code does them one at a time, top to bottom.
- Code may tick a box and add lines under Rules and Tips. Code does not add, remove, reorder, or reword tasks.
- A task is finished when its done-check passes, its box is ticked, and the work is committed and pushed.
- Only the current milestone is broken into tasks. Later milestones stay as headlines until they are next.

## Milestone 1: Walk around a gray test scene

- [ ] 1.1 Test scene. Create `Assets/Scenes/Graybox.unity`: flat ground about 50 m across, one tall box standing in for the tower, five or six smaller boxes of different sizes scattered around for scale, one directional light. Use Unity's built-in shapes and the default gray material. Build it through the Editor bridge or an Editor script, never by writing the scene file by hand. Done when: the scene is saved, opens with no console errors, and a screenshot shows the layout.
- [ ] 1.2 Player that walks and looks. In Graybox, add a Player with a CharacterController and a camera at about 1.6 m eye height. One script, `Assets/Scripts/Player/PlayerController.cs`: walk with the Move action, look with the Look action, gravity, mouse cursor locked during play. Read input from the Player map in the existing `Assets/InputSystem_Actions.inputactions` so keyboard/mouse and gamepad both work. Walk speed slow and steady, starting at 2.5 m/s. No jump, sprint, crouch, or interact yet. Done when: clean compile, Play mode starts with no console errors, and the owner has walked around the scene with keyboard and mouse (and a gamepad if one is plugged in) and confirmed it.
- [ ] 1.3 Remove template leftovers. Delete `Assets/TutorialInfo`, `Assets/Readme.asset`, and `Assets/Scenes/SampleScene.unity` with their .meta files, through the Editor so references stay clean. Make Graybox the only scene in the build list. Done when: clean compile, no console errors, Graybox still plays.

## Milestone 2: PSX look on the test scene

Blocked by the open decision on the PSX look approach. Gets broken into tasks once that is decided.

## Milestone 3: One full day of the loop, no events

Wake, chores, check the fire, report, maintain the Ward, sleep. HP, MIND, and WARD working. Autosave at sleep.

## Later, from DESIGN.md

Event deck and tracks, chapters and endings, voices, scrapbook, inventory, map, and Ward as physical objects, Ahmee, minigames.

## Open decisions (owner)

- Level blocking tool: Unity's built-in shapes or the ProBuilder package. Needed before the real tower area is blocked out. Not needed for Milestone 1.
- PSX look approach: a shader written for this project or an existing kit. Needed before Milestone 2.

## Rules and Tips

Lessons from finished tasks that later tasks need. One line each, newest at the bottom.
