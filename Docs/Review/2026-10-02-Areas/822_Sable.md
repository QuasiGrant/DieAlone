# 8.22 gate, design (Sable), 2026-10-02
Against FrontLayout.md draft 2 and PLAN 8.22 ("its layout serves the loop ... with times stated"). Build 0a963df. Capture Docs/Captures/Main3Review_front (13:53) with Checks.md and index.md; recipe Tools/Recipes/main3_8_22_front.cs read for intent; Marlow's 822 paper check read. Read only; no Play run by me. Car behaviour, the shift UI and R5-2's search are Milestone 10; this gate judges the places they need.

**PASS.** No blocks. Every draft 2 place is built where drawn or within a stated deviation, the swept car paths are proven, and every leg is timed on the build. Two proof frames are owed (3.1, 3.2) and one Milestone 10 conflict is carried (open 1).

## 1. A visit, as built
| Leg | Built | Verdict |
|---|---|---|
| Gate shift | Booth x 390.6 to 393.2, z 164.2 to 166.6, window north, 1.0 m door south, counter 1.0, stool under the counter, roof light; Gate_Booth warp (392, 162.5) faces north into the door. Inside frame: the window holds the arm, the lane and the fence, nothing else. Car stop (391.8, 168.7) puts the driver's door 1.2 m off the window. Post to booth 1.00 m (doc 1.2): out of the 0.6 to 1.0 band, accepted. IW3 off with no car, on while an admitted car is on the spur, off once parked: Marlow's hurt 5 is closed. | PASS. Papers, Please's window, with the exit 4 m behind your back. |
| Admit | Paths_Gate: the car leaves the arm, turns north through the flared mouth, runs the spur clear of the store and the brush; 187 poses clear. | PASS |
| Refuse | Paths_Gate: backs 14 m out through the open gate (front at 403.5), turns on the apron (432 poses clear), drives out to the T. The refused car leaves by the road the player cannot take. | PASS. That is the hurt we want. |
| Office and R5 | West door open inward at 90, no prompt; held shut stays shut, reopens at wake. Porch frame: door open on a lit doorway, counter inside, sign moved south of the door, lamp over it. Talk spot 1.1 m in (doc 2); talk spot round the counter to the cot 13.0 m, 4.9 s. | PASS. Both resident spots (stool, cot) are one short walk apart, so "asleep in back" costs the player 5 s, not a search. |
| Store | Door 1.0 m (x 364.5 to 365.5), run kept at 12 m; Store/Inside kept as depth 0; Checkout_Counter moved flush to the wall (recipe 307); canopy and posts as drawn. Store frame: door, windows showing the lit room, ICE sign, chest. | PASS. One store through window and door, so the level opens in the room the lot already showed (Exit 8's relearned room needs that). |
| Ice chest | Built freezer at x 367.9 to 369.7 against the wall, 2.5 m east of the door, ICE sign over it; found from the spur walk (1.8 degrees tall). | PASS. The search happens on the porch, in view of the lot, the car and the road: exposed, which is right for going through another man's things. |
| Toilet | x 340.4 to 342.0, z 185.0 to 187.0, door east onto the lot, clear of Hedge_Burn_8; frame reads as a vault toilet at the lot's west edge. | PASS |
| Campground | Spur to the chain (79.2 m from the booth door), ring counter-clockwise clear (120 poses), P1 to P8 nose-in clear. Paths_Ring: the ring sits in the trees with the deadfall left between pitches. | PASS on layout; frame owed (3.2). |
| S1 | Hood eye (372.5, 4.2, 179.4) to the verge trunk, 64.8 m, every mesh clear. Frame: the dead tree centred over the booth and the lowered arm. | PASS. Better than drawn: R6's windscreen holds the exit and the thing that keeps him from it. |
| S2 | Lookout to the T, 413.3 m, 7 of 7 road points clear and in frame. | PASS on rays. By eye at sheet size the road does not separate from the haze; loose under Grant's 2026-09-30 rule (play over sightlines). Grant judges it on the walk. |

## 2. Rook's deviations
| Deviation | Call | Why |
|---|---|---|
| Toilet z 185.0 to 187.0 (doc 185.1 to 186.9) | Accept | The outhouse's own 2.0 m face; still clear of the hedge boxes; door east unchanged. FrontLayout 2.10 takes the built figure. |
| Mast 0.5 m north, one base collider | Accept | Closes a 0.64 to 0.95 m slot by the brush band. The mast is a landmark on arrival, no loop job; 0.5 m is not visible from the T. |
| 122 forest pieces moved, 1 log removed | Accept | The ring and pitches are the job; the deadfall still reads between pitches in Paths_Ring. Rook lists the removed log by name in his report. |
| 2 reflector posts at the T removed | Accept | A refused car hit one. Refuse must read as a clean exit; posts are dressing. |
| Porch lamp fallback | Accept | Draft 2 2.7 names it as the fallback. It also tells a careful eye "open" in one light, which is what the Office CHECK row needs. See 3.1 on how clearly. |

## 3. Owed (not blocking the tick)
1. **Door open against shut, at a size a person can judge.** Checks pass on numbers (opening 71 percent changed, mean 21.2; lamp 29.0 levels). At sheet size I cannot tell DeckDoor_Open_Binoculars from _Shut by eye. Also the lamp line reads "29.0 ... (bar 32.0 over the noise)" next to PASS; Rook states which bar the lamp meets. Fix: a 4x crop of the office in both binocular frames, side by side, on the sheet. If Grant cannot see it in the crop, the lamp goes brighter; the opening stays as built.
2. **The cars you let in.** The front's one beat no inspiration has (section 6) is walking the ring and counting parked, empty cars. The ring frame from the join is dark and shows no pads. Fix: one frame from the join with static car bodies on P1 to P3, Day one look. Layout proof only; behaviour is Milestone 10.

## 4. Wren's call: the office door seen from the deck's east side
Accept for 8.22: the deck sees the door (hard must-see 128 of 128; binocular frames from (167.5, 57.6, 166.5)). The lectern eye loses it behind Camp/Tower/Cab/E_Sill at 2.3 m.
Consequence for Milestone 10: TowerCheck.md 3 puts the Office on the tower sheet, and 7.5 fixes binoculars at the lectern with every designated view in a window. As built, the Office row cannot be stamped by sight from the lectern. My call: move the lectern stand to the cab's east side so the office falls in a window, rather than freeing the binoculars (the fixed stand is what makes the check a ritual, Firewatch's lookout). Open 1.

## 5. Walk times, on the build (2.5 m/s, PlayerController.Step)
| Leg | m | s | Doc |
|---|---|---|---|
| T board to store porch | 33.4 | 13.3 | 38 / 15 |
| Store porch to west door | 30.3 | 11.9 | 31 / 12 |
| West door to talk spot | 1.1 | 0.4 | 2 / 1 |
| Talk spot to R5's cot | 13.0 | 4.9 | none |
| West door to R6's driver door | 38.0 | 15.0 | 34 / 14 |
| R6's bay to booth, in | 32.1 | 13.8 | 26 / 10 |
| Booth door to T board | 54.9 | 21.8 | 55 / 22 |
| Booth door to the chain | 79.2 | 31.4 | 78 / 31 |
| Round the chain's post, the ring, back | 152.4 | 60.5 | 118 / 47 |
| T board to toilet, in | 17.6 | 6.8 | 14 / 6 |
| West door to toilet, in | 16.0 | 6.2 | 13 / 5 |

Store run from the cabin: about 539 m, 216 s plus the store (doc 548, 219). All of it, ring included, back by the chain and T: about 955 m, 382 s plus talk, store and shift (doc 914, 366; paper sum on the timed legs, mostly the longer ring). The order holds: Food is the far sure thing, over three and a half minutes of walking for one need, against forage near and unsure (Dredge). The ring adds a minute that only someone who wants to count the cars will spend. That is the right price. FrontLayout section 3 takes these figures in draft 3.

## 6. Open questions
1. Wren, Pim: the lectern stand moves to the cab's east side in Milestone 10 (TowerCheck 7.5), so the Office row is read from it. Until a dated line, the Office row has no lectern sightline.
2. Rook: the lamp's bar in Checks.md (29.0 against 32.0) and the removed log's name.
3. Grant: S2 at 413 m is rays-clear but not legible to my eye at sheet size; judge on the walk.

Sable
