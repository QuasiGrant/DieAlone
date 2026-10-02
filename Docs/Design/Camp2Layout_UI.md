# Camp 2: interaction points for 8.25 (Pim, DRAFT 1 2026-10-02, nothing decided)
Inputs: Valley.md 1.2, 6 (M5); Main3.md 3, 4; DailyLoop.md; Collectibles.md 2; Docs/Private Minigames 3.2, Places817_Quill; recipe main3_8_17_camp2.cs. Code (verified): reach 2 m, one eye ray, first hit wins, Interact E / pad Y. No phone, card, barrel or talk code. Built (Main3.unity, read): Camp 2 root (292, 4, 108); booth (300, 99), open front west (x 299.3), walls z 98.3 to 99.7, back x 300.6 (Wren: enterable); hood light (299, 99) 2.6 high; CardTable (297, 98) turned 15; his chair (296.75, 97.1), guest chair (297.25, 98.9); ramp foot (298.9, 107.8); Resident_Camp2_Spot on top (290.2, 24.9, 108.5), his top chair (290.2, 107.4) facing 70; lantern (292.5, 107); barrel (286.5, 101.5); ladder about (289.8, 102.1); start blaze about (302, 112.6).
| Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|
| Payphone handset, booth back wall | 2 m, from inside or the opening (1.2 m) | handset on its hook, 1.4 to 1.6 high (height in the pack mesh unverified) | Ringing: `Answer` [GAP: Events, which rings are the player's]. Idle: `Lift the receiver` (Collectibles 2, nothing says it is special). During a hand: none; his call plays at the table. Loss: handset gone from the hook, none. Walk plus 1 press |
| R2 at the table (his chair) | 2 m | head and shoulders, seated, 1.0 to 1.3 high | When a hand is due today: `Talk` (Dialogue.md); the ACT reply `Deal me in` seats the player and opens the card game. Walk plus 2 presses. Otherwise he is not here |
| Guest chair, table top, cards, mug, lantern | none | | none, off the mask. The guest chair's collider must not take the ray to R2 (back 0.9, his head above it) |
| Loss state: handset on his cards | none | | none. Discovery fires on sight [GAP: Events trigger]. `Examine` only if Quill writes a line |
| R2's spot, stack top (290.2, 108.5) | 2 m | head and shoulders in his top chair, facing the road | Home and no hand due: `Talk`. Absent or dead: none |
| Stack path (ramps, east face) and top landing | none | ramps, rails | none. Walked up; no climb prompt. Rails stop with no message. Ladder (WSW) none, off the mask |
| Tent lamp, tent, letters under stones | none | lamp read by eye (lit, flicker, dark) | none. Lamp has no toggle. Letters: `Examine` only if Quill writes a line |
| Rain barrel (286.5, 101.5) | 2 m | lid and tap, 0.8 to 1.1 high | Some days, Water not met: `Take water` (proposed). Dry or met: none |
| Start blaze (T leg), phone pole (boathouse leg 41), food lockers (T leg 45) | none | blaze face, pole box, lockers | none; read by eye. Dial tone at the pole is sound only. No Camp 2 signpost is specified |
### Spacing (camp rules: 1.2 m centre to centre, 0.5 m edge to edge, none in front of another within 2 m along the approach)
1. Handset to R2 at the table 4.3 m, to guest chair 3.4 m; R2's two places are never live together. Table to ramp foot 9.9 m; barrel to ladder 3.3 m. Top: spot to lantern 2.7 m, to letters 3.8 m. Pass on paper.
### Found targets (clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
2. Boathouse leg heading north at (297.2, 84.6): table 13.4 m, 0 degrees; booth 14.7 m, 11 degrees; hood light at night. At (296.1, 90.2): barrel 14.8 m, about 29 degrees off travel; talus and boulders may hide it (unverified).
3. T leg heading south at (300.05, 116.7): booth 17.7 m, 0 degrees; table 19 m, 9 degrees. Both lines run past the ramps and rails on the east face: blocked or not is unverified. Ramp foot heading NE: start blaze 5.7 m (open item, Camp 2 to T fwd 0).
4. Top landing heading west: R2's top chair and tent within 7 m. Phone pole and lockers: one frame each from their own legs.
Open: (1) which rings the player answers (Events, private). (2) R2's hours at the table vs the top (Sable). (3) `Take water` wording. Figures on paper; built frames decide. Pim
