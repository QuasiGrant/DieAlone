# Gate booth: the window, papers and stamps

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-29: gate minigame option B (a booth just inside the gate; admitted cars drive up a gravel spur inside the fence to a closed campground loop behind a chain; refused cars turn and leave; the Wardkeeper stays inside the fence); cars keep arriving until the office resident's storyline ends (completed, or he dies), and do not thin out as WARD falls; talking to the office resident counts as Social and starts his minigame, the booth itself does not count as Social; how many cars and days is set by playtest and must not last the whole game; during a shift, trying to follow a car hits an invisible wall with the message "You can't abandon your post." Also 2026-09-29: nothing forces the player to act; no time budget for the day. Terms follow the other UI specs: stamp words in capitals, the word never colour alone (TowerCheck.md 5). Colours and type per Style.md 7 and Fonts.md. Logbook layout is Logbook.md; the on-screen line is Objective.md 3. Revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Overpass for forms, dialogue and warnings; Patrick Hand for logbook handwriting): section 6.8. Revised 2026-10-02 to FrontLayout.md draft 2 (booth south of the drive, lift barrier, refused cars reverse onto the apron, shift starts on entering the booth per Wren, IW3 on while an admitted car is on the spur): sections 2, 5, 7.2, 9, 11, 12, 13.

## 1. Purpose

The office resident's minigame. The player works the booth window: reads a car's papers against the day's rule sheet and stamps ADMIT or REFUSE. It is a job at a counter, like the tower check. Rules, papers and what a wrong verdict costs are content **[GAP: Tully, Quill]**; this spec covers the screen, the inputs and the records.

## 2. Terms

| Term | Meaning |
|---|---|
| Booth | The hut just inside the gate, south of the drive. Window on the north face, onto the lane; door on the south face. |
| Barrier | The lift arm across the lane by the booth. Cars stop at it. |
| Window | The interact point inside the booth. Using it enters the counter view. |
| Counter view | The fixed first-person view at the window. |
| Papers | What a driver hands over. One to three sheets per car. |
| Rule sheet | The day's rules, pinned at the left of the counter. |
| ADMIT, REFUSE | The two stamps, and the verdict words in every record. |
| Shift | From the player entering the booth until the day's last car has gone, or until the player leaves the booth with no car waiting (Wren, 2026-10-02). |
| Gate active | From the day the office resident starts his minigame until his storyline ends. |

## 3. When the booth works

1. Gate not yet active: the window is shuttered. No prompt. No cars stop.
2. Gate active, by day: cars arrive one at a time (section 5). The window prompt shows whenever the player stands at it.
3. Night (report filed): the shutter is down, no prompt, no cars. A car still waiting when the report is filed drives off during the fade to night. No record for it (section 10.3).
4. Storyline ended: the shutter comes down for good. The booth stays as a building. The Logbook keeps the past gate lines.
5. Cars per day and number of days: set by playtest (DECISIONS 2026-09-29) **[GAP: playtest]**.

## 4. Entering at the window

1. The player walks into the booth and faces the window. Prompt, same style as `Use binoculars`: `Work the window`.
2. `E` or pad `Y` (Interact). 0.3 s fade through black, then the counter view. Same timing as the binoculars (TowerCheck.md 7.1).
3. The player can enter with no car waiting. The counter shows the rule sheet and the stamps; the papers slot is empty.
4. Carrying an object: no prompt (PlayerInteractor only offers Set down while carrying).
5. The Logbook does not open in the counter view (Logbook.md 8.1.2: a minigame is open). The pause menu does.

## 5. How a car's arrival is signalled

All in the world first. One screen line, once a day.

1. Sound: engine and tyres on the road, then gravel as it pulls up to the barrier. Audible across the lot (Hollis to set the distance).
2. Light: headlights on against the sunset, lighting the lane by the booth.
3. If the booth is empty 10 s after the car stops: one horn. Repeats every 90 s, at most three horns per car, then the car idles without horning. It waits until served or until the report is filed (nothing forces the player).
4. The first car each day posts the on-screen line `Logbook: a car at the gate.` through the Objective.md queue. Later cars that day post nothing.
5. From the tower the waiting car is visible at the gate (DECISIONS 2026-09-29: the tower sees more of the office and lot). It is not a tower stamp and is not on the tower sheet.
6. Only one car at a time. The next car arrives no sooner than 20 s after the last one has left (tuning number). No queue of cars builds up.

## 6. The counter view

Fixed camera at the window, seated height. The upper third looks out through the window at the driver's door; the lower two thirds look down at the counter. No free look.

```
+--------------------------------------------------------------+
|            window: driver's door, face, headlights           |
|                                                              |
|                                                              |
+--------------------------------------------------------------+
|  +------------+    +----------+ +----------+    .-------.    |
|  | RULES      |    | PERMIT   | | ID       |   ( ADMIT )   |
|  | DAY 14     |    |          | |          |    '-------'   |
|  | 1. ....... |    |          | |          |                |
|  | 2. ....... |    +----------+ +----------+    +--------+   |
|  | 3. NEW ... |                                 |/REFUSE/|   |
|  +------------+                                 +--------+   |
|                                                              |
|         [LMB] pick   [RMB] back   [E] leave                  |
+--------------------------------------------------------------+
```

1. Left: the rule sheet, pinned, readable at this size. Section 8.
2. Centre: the papers slot. Up to three sheets side by side, each showing its heading and a small readable body.
3. Right: the two stamps on their pads. ADMIT is a round stamp with a round print. REFUSE is a square stamp with a square print and a diagonal bar. Different word lengths, shapes and prints, so they read through the VHS filter without colour. Ink colours: ADMIT the text colour of Style.md 7.3, REFUSE the warm warning colour **[GAP: Style]**, same as CHECK ON FOOT.
4. Bottom: the input hint, for the first 3 s of each entry, by last device used (TowerCheck.md 3.5). Pad version: `(A) pick   (B) back   (Y) leave`.
5. Focus: on a gamepad, one item is focused at a time, shown by a drawn pencil bracket around it. With a mouse, a small dot cursor (uGUI Image) follows the pointer and hover sets focus. The system cursor stays hidden.
6. Focus order, left to right: rule sheet, paper 1, paper 2, paper 3, ADMIT, REFUSE. Empty paper slots are skipped.
7. The driver's lines, if any, show in the dialogue format **[GAP: DialogueFormats.md]**. They never cover the counter.
8. Type: all text TextMeshPro.

| Text | Font |
|---|---|
| Rule sheet (`RULES`, `DAY n`, rules, `NEW`) and the printed parts of papers | Overpass; headings caps SemiBold **P**, body Regular |
| Anything a driver wrote on a paper | Texture, drawn into the paper art **[Vesper]**. Never Patrick Hand (the keeper's hand only) and never Overpass as a stand-in (Examine.md 5). |
| Stamp faces and prints `ADMIT`, `REFUSE` | Images (art in Overpass caps), not text |
| Driver's lines | Overpass (dialogue) |
| Input hint, `Work the window` prompt, `You can't abandon your post.` | Overpass, as the interact prompt and Objective.md 3 |
| Logbook `Gate` block (section 10) | Patrick Hand, as the rest of the Today page (Logbook.md 2.1) |

No VT323.

## 7. Papers and stamps

### 7.1 Reading

1. Choosing a paper lifts it: it fills the centre and right of the screen, full size. The rule sheet stays in place at the left, so the two read side by side.
2. Choosing the rule sheet lifts it the same way, into the centre.
3. Back puts a lifted sheet down.
4. Reading has no tool, no highlighter, no hint of what is wrong. Spotting is the minigame.

### 7.2 Stamping

Two steps, so a stamp is never made by a stray press.

1. Choose a stamp: it lifts off its pad and hangs over the counter. Focus jumps to paper 1.
2. Choose a paper (on the counter or lifted): the stamp comes down on it. Thump sound (Hollis). The print shows on that paper.
3. Back with a stamp in hand: the stamp goes back on its pad. Nothing is stamped.
4. One stamp decides the car. Stamping any one of its papers is the verdict. The other stamp is then locked (dimmed on its pad) for that car.
5. No undo. 0.5 s after the stamp, the papers slide back out of the window. The car goes: ADMIT, the arm lifts, the car turns north up the spur, the chain drops for it, and it parks on the next empty pitch; REFUSE, the car reverses out through the gate onto the apron, turns, and leaves down the road.
6. The player stays in the counter view while the car goes. The papers slot is empty until the next car.
7. What happens after a wrong verdict, and whether the player is told, is content **[GAP: Tully]**. This spec shows nothing on screen about right or wrong.

### 7.3 Choosing the verdict before reading

Allowed. A player can stamp without lifting a paper. The game does not stop them.

## 8. The day's rule sheet

1. One sheet per day the gate is active. Heading `RULES`, `DAY n`, then numbered rules, one line each where possible.
2. A rule that is new today is marked `NEW` in front of its number text. The word, not colour.
3. At the first entry of a day with a new rule, the rule sheet is lifted automatically for 2 s, then set down. Only on that entry.
4. The sheet on the counter is the only copy. The Logbook does not copy the rules (section 10 records outcomes, not rules).
5. Words and rules: Quill and Tully. Must fit on one sheet at the counter reading size; if a day needs more than six rules the layout is revised **[GAP: Tully]**.

## 9. Leaving

### 9.1 Leaving the window

1. Leave works at any time, including with a car at the window. Leaving with a stamp in hand puts the stamp back. Leaving with a sheet lifted puts it down. Nothing is stamped by leaving.
2. 0.3 s fade through black, back to standing in the booth, facing the window.
3. A car at the window keeps waiting (section 5.3) and the shift stays on.

### 9.2 Leaving between cars

1. With no car waiting, leaving the booth ends the shift. The player may go anywhere, except past IW3 while an admitted car is still on the spur (9.3).
2. The next car arrives whether or not the player is there, and signals as in section 5.

### 9.3 The invisible wall

1. IW1 blocks the gate opening at all times (player only). IW3 crosses the spur at z 210 from the arm lifting for an admitted car until that car is parked, shift or not.
2. Touching IW1 during a shift, or IW3 while it is on, shows the line `You can't abandon your post.`
3. Style: exactly the Objective.md 3 line. Top left, 5 percent safe margin, same font (Overpass), colour and 1 px shadow, fade in 0.3 s, hold 3 s, fade out 0.5 s. No `Logbook:` prefix, because the logbook does not change. No pencil sound (that sound means the logbook changed). Sound, if any: Hollis.
4. It jumps the Objective queue: it shows at once, and any queued logbook line waits until it fades.
5. It does not repeat while it is on screen, and not again until 5 s after it fades, however often the player pushes the wall.
6. Outside a shift, IW1 still blocks the gate, silently. With IW3 off, the spur chain is a world object the player can walk round by its east post. No message.

## 10. What the Logbook records

The booth is not Social (DECISIONS 2026-09-29). It never ticks Social on the Today page. Talking to the office resident does, as with any resident.

1. Today tab, right page, under the needs, a `Gate` block. Shown only on days the gate is active:

```
|  Today                      |
|    Food     -               |
|    Water    met             |
|    Warmth   -               |
|    Social   met             |
|                             |
|  Gate                       |
|    ADMIT    2               |
|    REFUSE   1               |
```

2. Counts only. No right or wrong, no car details. The book never says a verdict was wrong unless content writes a note (item 4).
3. A car that drove off unserved is not counted. Draft reading of "nothing forces the player to act".
4. Questions tab: the office resident's storyline writes questions and notes as content (Logbook.md 5). A gate note posts `Logbook: new note.` as usual.
5. Past days keep their `Gate` block, read-only.
6. After the storyline ends, the block no longer appears on new days.
7. Logbook.md 4.2 gains this block in its next revision.

## 11. States

| State | What shows |
|---|---|
| Gate not active | Shutter down. No prompt. |
| Gate active, no car, player outside | World only. |
| Car arriving | Engine, gravel, headlights. First car of the day: `Logbook: a car at the gate.` |
| Car waiting, booth empty | Horn after 10 s, at most three. |
| At the window, no car | Rule sheet and stamps. Papers slot empty. Stamps can be lifted; choosing a paper slot does nothing. |
| At the window, car waiting | Papers in the slot. |
| Sheet lifted | Sheet full size, rule sheet at the left. |
| Stamp in hand | Stamp over the counter; other stamp still on its pad. |
| Stamped | Print on the paper, other stamp dimmed, papers slide out after 0.5 s, car leaves. |
| Shift, player walking | Touching IW1 shows the line. |
| Admitted car on the spur | IW3 on; touching it shows the line, shift or not. |
| Report filed | Shutter down. A waiting car drives off unserved. |
| Event ends the day at the window | Counter view closes at once (0.3 s fade), nothing stamped. Event sequence, then WardNight.md 2.4. |
| Paused | Pause menu over the counter view. Resume returns to the same state, stamp still in hand if it was. |
| Storyline ended | Shutter down for good. |

## 12. Input paths

Keyboard and mouse:
1. Walk to the window. `E` to enter.
2. Mouse moves the dot cursor. Hover focuses.
3. Left mouse or `Enter` chooses: lift a sheet, lift a stamp, stamp a paper.
4. Right mouse or `Backspace` is Back: put the stamp back, or put the sheet down. With nothing in hand or lifted, Back leaves the window.
5. `E` leaves the window from any state (stamp returns, sheet goes down).
6. `Esc` opens the pause menu, as in the binoculars.

Gamepad:
1. Walk to the window. `Y` to enter.
2. D-pad or left stick moves focus in the order of section 6.6.
3. `A` chooses.
4. `B` is Back: put the stamp back, or put the sheet down. With nothing in hand or lifted, `B` leaves the window.
5. `Y` leaves the window from any state.
6. `Start` opens the pause menu.

Proposal on the pad leave button (Crouch is `B` in the Player map): leave is `B` as UI Cancel, one level at a time, plus `Y` as a direct leave mirroring the enter press. This matches the binoculars (TowerCheck.md 8: `Y` or `B` lower). `B` is not Crouch here because the Player map is off in the counter view.

Requirements for Rook (flagged):
1. Player map off from entering the counter view until leaving: Move, Look, Jump, Crouch, Sprint, Attack, Previous, Next, Interact, Throw. Pause stays live.
2. The counter component reads UI Navigate, Submit and Cancel, plus its own bindings for `E`, `Y`, right mouse and `Backspace`. UI Cancel is `*/{Cancel}` in Assets/InputSystem_Actions.inputactions; that it maps to pad `B` (buttonEast) is expected, unverified. `Esc` is on both Player/Pause and UI/Cancel: in the counter view `Esc` must open pause, not act as Back. Rook to settle which wins.
3. **Crouch on exit.** Crouch is hold. If `B` is still held when the Player map comes back on, the player could exit crouched. Re-enable the Player map only after `B` is released, or confirm the Input System does not start a Button action that is already held when its map is enabled (unverified).
4. The same press that leaves must not also re-enter: ignore Interact for 0.3 s after leaving (the fade).
5. Counter component in GamePause's gameplay list.
6. IW1 always on, speaking only during a shift; IW3 on from the arm lifting for an admitted car until it is parked. Both off PlayerInteractor's mask. The message goes through the Objective line component with a priority flag and a 5 s cooldown (section 9.3).
7. Forced close API for events, as the book (Logbook.md 8.3).

## 13. Open questions

1. Rules, papers, driver lines, and what a wrong verdict does: Tully and Quill.
2. Where the car stops: FrontLayout draft 2 puts the driver's door 1.2 m off the window, at the barrier. Rook proves the swept paths.
3. Can the player walk up the spur, past the chain? FrontLayout draft 2: yes, round the east post, when IW3 is off.
4. Should an unserved car count as REFUSE? Draft: no, not counted.
5. Cars per day and days: playtest.

Pim
