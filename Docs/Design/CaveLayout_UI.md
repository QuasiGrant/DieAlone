# Cave and ravine: interaction points for 8.27 (Pim, DRAFT 1 2026-10-02, nothing decided)
Inputs: Valley.md 1.5, 6 (M10, E17), 8, 9.6; Main3.md 3.3, 4 (W1 to cave); DailyLoop.md 3; Events.md 3, 4 (event 16); Dialogue.md; GateBooth.md 12; Gate.md 4; DECISIONS 2026-09-29 (unsigned spur, roulette in the cave, chases never enter); Docs/Private RouletteUI.md (table view, not repeated here). Code (verified): reach 2 m (PlayerTuning.interactReach), one eye ray, first hit wins, Interact E / pad Y; DevMenu warp sets yaw only, pitch is kept. No talk, sit, table or event code. Built (Main3.unity and main3_8_17_cave.cs, read): trail W1 to cave through (128, 70), (108, 48), (84, 64), (52, 37.5); rope handrail (106.8, 57.6); coloured bulbs (82, 52); mouth opening x 50.5 to 53.5 at z 37.9, floor about -6, boulder jambs 3.4 m apart at the floor, overhang underside 2.25 m, 1.5 m proud; east jamb, scree and ferns 2.7 to 5.9 m east of the opening centre at z 38.9 to 39.7; day-one board with `CLOSED - UNSAFE`; entrance boxes near-black; chamber x 71 to 89, z 3 to 21, floor -18; Resident_Cave_Spot (84, 15); chamber lantern (87.8, 13.6); side room x 89.5 to 97.5, z 7.5 to 15.5, 3 m high; doorway in the chamber's east wall x 89.25, z 11.4 to 12.6, 1.2 wide, 2.1 high; table (93.8, 11.5); guest chair (92.85, 11.5) facing east; his chair (95.05, 11.3); revolver, candle, bottle, two glasses on the table; cage light and one lamp over it; bulb string (86.5, 12.3) to (96.8, 11.2) through the doorway; roof crack aimed at (91.8, 11.5); rock BigBoulders_0 seated near (89.25, 9.5); crate (96.6, 14.6); pillar (96.2, 8.4); Wall_E_Future on the room's east side. By day only: no cave prompt at night (the night walk is camp to the Ward, Valley 9.6).

## 1. Interactables
| # | Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|---|
| 1.1 | Day-one board, mouth | none | planks and the red `CLOSED - UNSAFE` sign | none; read by eye. Solid on day 1. From day 2 the boards are down: nothing here |
| 1.2 | Rope handrail, coloured bulbs, candle stubs at the mouth | none | | none, off the mask |
| 1.3 | R7's spot, chamber (84, 15) | 2 m | head and shoulders, 1.4 to 1.7 high | Not yet met, by day from day 2: `Talk` (Dialogue.md; Social; meeting him opens the table). Walk plus 1 press. When he is at the table: no figure here, none [GAP: Quill/Sable, where he stands on a day after the meeting] |
| 1.4 | Chamber lantern (87.8, 13.6) | none | | none, off the mask |
| 1.5 | Guest chair, side room (92.85, 11.5), back to the door | 2 m | chair back and seat, 0.45 to 1.0 high | Table open (RouletteUI 2): `Sit`. Walk plus 1 press. Chair pushed in (not met, session done today, night): none. Carrying something: none (carry rule). Rook: two chair poses, pulled out and pushed in; the pose is the state |
| 1.6 | R7 at the table, his chair (95.05, 11.3) | none | | none, off the mask; his talk opens on `Sit`, so his collider must not take the chair's ray. He sits 2.2 m behind the chair: from 2 m or closer the chair is hit first |
| 1.7 | Revolver, glasses, bottle, candle, crate, pillar | none | | none, off the mask. The revolver is used only in the table view |
| 1.8 | Event 16, "deeper" for the day | 2 m | [GAP: Sable/Quill, where it opens and what is inspected] | Event day only: the inspect word Events.md uses (none fixed yet). If it opens through Wall_E_Future, the side room has two ways that day: M10 "one way in" then holds only on other days (Sable) |
| 1.9 | Ravine rim, rock band and boulders | none | | none. Stops read by eye (Valley 8) |

### Spacing (camp rules: 1.2 m centre to centre, 0.5 m edge to edge, none in front of another within 2 m along the approach)
1. R7's spot to the lantern 3.9 m, to the doorway 5.9 m. Chair to his chair 2.2 m (only one has a prompt). Pass on paper.

## 2. The side room: in and out
1. **Way in.** Leg 3 of the descent ends in a short level passage into the chamber's west side at (71, 12), heading 90. The doorway sits on the same line, 18 m ahead. Walk through it: no door, no leaf, no prompt. The guest chair is 3.6 m inside, 8 degrees right of the line, back to you; the prompt shows from 2 m.
2. **Sit.** Keyboard: walk, look at the chair, `E`. Pad: walk, look at the chair, `Y`. 1 press from the chair. The table view and every table input are RouletteUI.md (private).
3. **Get up.** Keyboard: `E`, or right mouse / `Backspace` with nothing in hand. Pad: `Y`, or `B` with nothing in hand. 1 press from the base table state. 0.3 s fade.
4. **Standing point after getting up:** (92.0, 12.0), on the doorway line, facing 270 (the doorway 2.75 m ahead, filling the centre of the frame). Leaving is then forward only: `W` or left stick, no look input.
5. **Crouch on exit (pad):** `B` gets up and is Crouch in the Player map. GateBooth.md 12.3 applies: the Player map returns only after `B` is released.
6. **Way out of the chamber:** from the doorway heading 270, the exit passage at (71, 12) is 18 m ahead on the same line. Then up the three ramps to the mouth; the mouth's daylight is the target from the leg 1 turn.
7. **Task test (Gate 4, both devices, from the doorway):** sit, 1 press; get up, 1 press; doorway to sit and back out, 2 presses plus walking. Pass on paper.

## 3. Found rule (clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
The spur is unsigned and the cave is not seen from W1 (DECISIONS 2026-09-29, the one exception); the rule starts on the spur.
1. **Mouth, from the spur's last leg** (84, 64) to (52, 37.5), heading 230. At 30 m out (75.1, 56.7) the opening is dead ahead, 2.25 m tall, 4.3 degrees. Coloured bulbs (82, 52) at 32 m out mark the leg from day 2. Pass on angle and size.
2. **Mouth, line blocked (likely):** the opening faces north; the leg comes in 51 degrees off its axis from the north-east. The sight line to the opening centre passes about (54.3, 39.8) 3 m out and (55.9, 41.1) 5 m out, over the east jamb, its scree and ferns. The void may not show until the player is in front of it. Unverified; the frame decides. **Ask (Sable):** the last 10 m of the spur comes in from the north, heading 180 plus or minus 20, along x 50 to 54; or nothing taller than 0.3 m on the east side of the opening inside the sector bearing 20 to 70 from (52, 37.9), out to 30 m. Day 1: the board is the target, same line.
3. **Chamber, from the level passage** at (71, 12) heading 90: the doorway 18.3 m, 0 degrees, 6.7 degrees tall; R7's spot 13.3 m, 13 degrees; lantern 16.9 m, 5 degrees. Pass on paper. **Keep clear:** z 10.8 to 13.2, x 71 to 89.25, nothing over 0.3 m, so the whole doorway shows. Built rubble (76, 6), (83, 19), (74, 18) is clear.
4. **Side room, from the doorway** heading 90: guest chair 3.6 m, 8 degrees; table 4.6 m; R7 seated 5.8 m. Pass on paper. **Keep clear:** z 11 to 13, x 89.25 to 92.4 (the walk to the chair), and the doorway jambs: BigBoulders_0 near (89.25, 9.5), about 0.9 m south of the opening, must stay out of it.
5. **Ways back:** the doorway from the chair (2.75 m, dead ahead from the standing point) and the exit passage from the doorway (18 m, 0 degrees): one frame each.
6. Frames for Rook: mouth from 30, 10 and 5 m on the spur (day 1 and day 2); chamber from (71, 12) facing 90; side room from the doorway facing 90; standing point facing 270; doorway facing 270.

## 4. Dark-interior readability (layout needs only)
1. **Mouth:** the opening reads as a dark void in grey rock from the spur's last leg (3.2). The day-1 sign reads from 10 m.
2. **Entrance passage, 17 m south:** grey and dark by design. At its far end the way on is a left turn onto leg 1. Something lit or pale must show on the left (east) side before the end, so the player turns rather than stops at the far wall. It must not show from the mouth (the grey outside is the point, Main3.md 3.3).
3. **Each hairpin turn** (x 68, then x 52): from 5 m before the turn, the opening onto the next leg shows as lighter than the dead wall ahead. Legs 2 and 3 have the colour leak up the rock; leg 1 and its turn need their own marker. Diegetic option: Quill's bulb string continued up the descent from the chamber to the leg 1 turn (Quill, Sable).
4. **Chamber, from the passage:** the doorway reads as the brightest rectangle on the east wall (the table lamp spill), with the lantern beside it. R7 reads as a figure, not a shadow. On leaving, the exit passage reads as an opening in the west wall, not wall: lit edge, bulbs, or the floor of leg 3 lighter than the rock.
5. **Side room, from the doorway:** the guest chair is the most readable object, back toward you under the one light; the table, revolver and R7 behind it.
6. **Reduce flashing on:** 4.4 and 4.5 hold under the steady chamber light alone, not only on light peaks. Rave light cues never carry wayfinding.
7. **Bulb string at the doorway (computed, unverified):** the wire runs about 2.45 m over the floor at x 89.25, over the 2.1 m lintel, so it enters the rock. Lower it to 1.9 m at the doorway so the bulbs lead through the opening.
8. **Grey rule, proposed (Wren/Grant):** inside the cave, the floor 5 m ahead or the next turn's far face 20 grey or more above the darkest wall in frame, every frame of the descent and both ways through the chamber.

## 5. Warps (F1, CAVE group)
| Plain name | Warp object | Lands | Facing | State |
|---|---|---|---|---|
| Cave mouth | Cave_Mouth | (58, 44) on the spur's last leg, 9 m north-east of the opening; ground there not read | 211, the opening 13 degrees left of centre | built |
| Cave chamber | Cave_Chamber | (74, 12), just inside the chamber's west side | 90, the doorway 15 m ahead | built |
| Cave side room | Cave_SideRoom | (91.0, 12.0), 1.85 m from the guest chair | 90 | **new**: Rook adds the warp and the row `Cave side room (table)` under `Cave chamber` in DevWarpLabels.cs. Pitch is kept from before the warp: look down to the chair back |
| Ravine rim (cave trail) | Ravine_Rim | at the rope handrail (106.8, 57.6) on the spur | down the trail | **proposed** for Marlow's rim checks; Wren decides |

Not a gate task; press counts from F1 are measured on the built panel.

## Open
1. Where R7 stands after the meeting, and on days he is not at the table (Quill/Sable).
2. Event 16: where "deeper" opens and its inspect point (Sable/Quill); the inspect prompt word (Events).
3. Spur's last 10 m from the north, or the east side of the mouth cleared (Sable).
4. Leg 1 marker (Quill/Sable); grey rule for the interior (Wren/Grant).
5. Ravine rim warp (Wren).
Figures on paper; built frames decide. Pim
