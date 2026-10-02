# Keeper's camp: interaction points for 8.21 (Pim, DRAFT 2026-10-02, nothing decided)
Inputs: DailyLoop.md r6, Main3.md 3.4, UI/Logbook, DayEndConfirm, Examine, Objective, TowerCheck, DECISIONS 2026-09-30 (lamp) and 2026-10-02 (livable cabin). Verified in code: reach 2 m (PlayerTuning.interactReach), one eye ray, first collider hit wins, one prompt line lower centre, Interact = E / pad Y.

| Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|
| Wake | none | spawn in bunk, see Wake frame below | none; Objective line `Logbook: climb the tower.` top left, then `[Tab] Logbook` / `(View) Logbook` hint lower centre at +1 s for 4 s |
| Logbook | carried, no world point | Tab / View anywhere | none |
| File the report | carried, no world point | Logbook Today, `File the report`, then the card "Filing the report ends your day." (focus `Not yet`) | none in the world |
| Report box on desk | 2 m | box lid on the desk top | `Examine`; line (Quill) points to the logbook. It files nothing (Main3.md 3.4) |
| Bunk | 2 m | blanket top edge, not the wall | Day and night 1: none. Night 2 on: `Sleep` (counts as Give nothing, DailyLoop 2.5) |
| Cabin door | 2 m | door leaf at handle height | `Open` / `Close` (Door.cs). Open at wake by day so leaving costs 0 presses |
| Lamp at nightfall | none | none | none. Carried, on by itself at night, no toggle (DECISIONS 2026-09-30). Place no lamp object with a prompt |
| Lectern (tower check) | 2 m | lectern top inside the cab, facing north | `Use binoculars` (TowerCheck.md). Day only; night none |
| Chopping block | 2 m | block top with the axe in it | `Split wood` (day). Warmth chore part 1 |
| Stove | 2 m | stove front door | `Light the stove` once wood is split; before that `Examine` status line. Lit = Warmth met |
| Food shelf, water crock | 2 m | shelf face, crock rim | `Examine` only. No stockpile (DailyLoop): line says the store / pump; they meet no need |
| Wash stand, toilet, others | 2 m | the object's top or front | `Examine`, one line each, only where Quill writes a line; otherwise no prompt |
### Spacing (prompts cannot overlap on screen; the risk is one target stealing another's ray):
1. Interactable colliders at least 1.2 m apart centre to centre and 0.5 m apart edge to edge (14 degrees at 2 m). No interactable collider in front of another along the approach within 2 m.
2. Stove, bunk, desk and door each reached from the 1.5 m aisle with a straight look; shelf and crock not within 1.2 m of the stove or door. Chopping block outside, 3 m or more from the door and the fire pit.
3. The fire pit has FirePit.cs `Light the fire`. Warmth is the stove only; Sable/Wren to say if the pit keeps a prompt. If it does, 3 m from the block.
4. Every collider above sits on a layer in the interactor mask; dressing near them has no collider that pokes in front of the target face.
Wake frame (first frame, Grant's 3840 x 1976 view): seated on the bunk edge, facing about 190 degrees (south, slightly west). Must show the open door with daylight, left of centre, and the west window with a tower leg or the stair foot in it, right of centre. The eye ray hits no interactable within 2 m for the first 5 s (the logbook hint uses the prompt row). Wake to binoculars: walk plus 1 press, pad or keyboard. Pim
