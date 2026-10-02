# 8.23 gate, design (Sable), 2026-10-02
Against LakeLayout.md draft 2 and PLAN 8.23 ("its layout serves the loop ... with times stated"). Build ed9df85. Capture Docs/Captures/Main3Review_lake (16:44) with Checks.md and index.md; S2.png, S3.png, AreaFrames_lake.jpg, Compass_Views.jpg looked at; recipes main3_8_23_lake.cs and main3_8_23_lake_check.cs read for intent; Marlow's 823 paper check read. Read only; no Play run by me. Fishing UI, the cat as a creature and the events' scripts are later milestones; this gate judges the places they need.

**PASS.** No blocks. Every draft 2 place is built where drawn, every leg is timed on the build within a second of paper, the wade limit holds (flood 0 leaks, 0 traps) and the deck sees her bowl and blanket from 128 of 128 eyes. S2's sky number is a measuring fault, not a missing sky (section 3). Two proof frames are owed (4.1, 4.2).

## 1. A lake day, as built
| Leg | Built | Verdict |
|---|---|---|
| Water chore at the pump | Pump on the dock root, trail ends facing south, eye ray hits the pump at 1.2 m from (190, 96) (reach 2). Frame: pump dead ahead, dock rails and open water behind it. Found from Camp to pump at 21.1 m. Lake_Pump warp lands. | PASS. One press, by day, at the end of the trail you walk every day. Kiosk's counter: the sure, dull job, done in sight of the water you will need again tomorrow. |
| Shore | Pump to boathouse trail 83.2 m; rowboat, stake line and soft ground found from it; boathouse frame from 20 m out reads as a house on stilts with the stakes beyond. | PASS |
| The house | Gangway, east doorway, empty room, slip cut with rails on three sides, boat door open to the wall top, lamp gone (0 practicals), empty hook. Slip frame from the east doorway: rails, black water, the door. | PASS. Nobody lives here, and nothing lights it. |
| Feeding the cat at the step | North rail gone; open edge over the shallows. Step frame from the north doorway: chair left, blanket and bowl ahead, stakes and reeds past the edge. Bowl (241.2, 56.5) and chair (238.8, 56.4) 2.4 m apart on one floor. Deck: blanket and bowl 128 of 128 rays clear by every mesh. S1 chair to cab clear. | PASS. Bowl and chair within one step of each other is right: you choose standing between them. Papers, Please's inspection runs backwards here: the deck checks on her. |
| Fishing from the slip | Rest clamped to the east rail, collider top 1.3 m over the floor (no step over the rail). Stand (241.3, 52.4) facing 270, 5.0 m from the step. S3: rod tip down the middle of the boat door, black water, the far bank, ceiling over the top third, 0 percent sky. | PASS. Dark, close, framed: Dredge's deep water in a room. It reads nothing like the reeds, which is Quill 8's must. |
| Fishing from the reeds | Reed bed in water only, outside the stake line. Rest on the bank, stand (244.1, 62.0) facing 290, eye ray hits the rest at 0.7 m. S2: stakes and rope left, reeds at the foot, open lake, far shore, the ridge and sky over it. | PASS, see 3. |
| Wading events | Shallows bed -6.0 (knee deep), beach in at 24 degrees or less, stake line and rope as the visible stop, deep water past it west of x 240 only. Markers Event_UnderStilts, Event_OffStep, Event_RopeDrift on draft 2's points. Walked: off the open edge out by the beach to the reeds 11.0 m; gangway foot down the beach, round the step skirt to under the stilts and back out 24.9 m. Shallows frame: stakes and rope read as the edge; the house skirt reads as a drop to black. | PASS. All three are reachable, and every way off the step or gangway lands on something you can walk out of. Fears to Fathom's real-scale wade, with a stop you can see. |
| Dock events 1 and 2 | IntakePipe, drop and elbow at the dock's east edge; IntakeFixPoint (191.3, -4.6, 86.7) and stand; SampleBand at (188.0, 88.2) on the notch bank. Nothing filled in the notch. | PASS on build and Marlow's paper stands; no frame (4.1). |

## 2. Walk times, on the build (2.5 m/s, PlayerController.Step)
| Leg | m | s | Doc |
|---|---|---|---|
| Pump to gangway foot, the trail | 83.2 | 33.1 | 83.6 / 33 |
| Gangway foot to the step, through the house | 10.3 | 4.0 | 9.4 / 4 |
| Step to slip stand | 5.0 | 1.8 | 4.1 / 2 |
| Off the open edge, out by the beach, to the reeds stand | 11.0 | 4.1 | 12 / 5 |
| Gangway foot, round the step skirt, under the stilts and back out | 24.9 | 9.8 | 13 one way |
| Door to pump | not walked | | 120 / 48 |

Seconds run a little under metres / 2.5 (5.0 m in 1.8 s); the check stops on arrival within 0.5 m. Immaterial. Camp to pump trail on the sheet is 95 m; the cabin door to its start is not measured, so 120 m door to pump stays paper (4.3).

The order holds. Water alone: about 240 m, 96 s from the door and back. Her bowl or your plate adds pump to step and back, about 95 m and 38 s, plus the fish. So the cat is never on the way to the chore; you go past the pump on purpose. That is the price we want: small in seconds, paid every day, and the fish it costs is Food (Dredge's one catch, spent on someone else).

## 3. S2's lack of sky
Checks.md reports 2 percent sky in the upper half of S2, and passes on "more than 0". The frame shows more: a clear band of sky across the top of the frame over the ridge, wider to the right, by eye well over 2 percent. The check's SkyShare casts physics rays on every layer except Ignore Raycast to 400 m (main3_8_23_lake_check.cs line 69), so any collider without a renderer counts as "not sky". The Ward climb and ring colliders (index.md: Rock/ClimbRing up to y 64, Ward/WardPath up to y 64) stand on that bearing; I believe they are what eats the number. Unverified.
Design call: S2 meets draft 2 5.4 ("far shore and sky in frame") by the frame, which is what the player sees. It is less sky than Quill 8 imagined: the ridge stands about 20 degrees up (Marlow paper 2), and a rod look pitched down at the float will trim it further. I keep the stance and heading. The reeds are the open, slow choice against the slip's shut, dark one, and S2 against S3 makes that difference plain. If Quill or Grant wants more sky, the lever is the heading, not a new stance; that is open 2.
The grey lumps right of centre in S2 are the Ward rock's top, the same mass the deck's west view already shows (Compass_Views). Not smoke, not a new reveal.

## 4. Owed (not blocking the tick)
1. **Dock events, one frame each:** from IntakeFixStand toward IntakeFixPoint, and from the band stand (187.9, 88.7) toward SampleBand, Day one look. The pipe must read as a thing that can break; the band must read as wrong, not as a decal.
2. **SkyShare without invisible colliders:** Rook skips colliders with no renderer (or measures by pixel against the sky colour) and reruns S2, so the number matches the frame.
3. **Door to pump:** one timed walk from the cabin door to the pump stand, so the Water chore's figure is measured.

## 5. Noted for other owners
1. Camp to pump fails Day one trail grey at 20 m (diff -6). It is the most walked trail in the game. Pim, Vesper.
2. No water slowdown: the shallows walk at 2.5 m/s, so wading reads as walking. Tuning for the event milestone, not 8.23; open 3.
3. No cat in the scene yet. The step frame proves the place (bowl, blanket, chair, open edge), not her.

## 6. Open questions
1. Grant (carried from draft 2): the slip as the dark rest with a 1.86 m boat door and no sky; the shallows behind stakes and rope; the step's north rail removed. All built; judge on the walk.
2. Quill, Grant: S2 keeps a band of sky over the ridge. Enough, or turn the reeds stance north of 290 for more open water and sky (unmeasured)?
3. Grant: a wading slowdown in the shallows (a speed tuning, no new system), decided with the wading events.
4. Rook: 4.1 to 4.3.

Sable
