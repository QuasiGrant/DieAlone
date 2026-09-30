# Gate step 4, PLAN 8.14a (fix pass on Gate_8_14_Pim.md), Pim, 2026-09-30

Source: Docs/Captures/Main3Review (Rook, 2026-09-30 11:13): index.md, Checks.md, InvisibleColliders.md, Walk_Views, Pairs, trail sheets, Trail_Ends, Stops_1, Map_TopDown, grey tables. Code at HEAD 61322d3: DevWarpLabels.cs, DevMenu.cs, GameSystems.prefab (LookPreview), Main3.unity object names (read only).
Method limits: no shell, no Editor. Dev panel read from code; Rook's J-at-night Play test is the proof. Frame calls are by eye.

**Verdict: FAIL, narrow.** Dev panel order, J cairn lamp, pump and Jg junctions, north loop trail: fixed. Left: the "North loop: ruin" warp; 9 frames with no next place; 4 markers not seen in any frame; hedge collider cover unproven.

## 1. Dev panel against Valley.md 14

| Item | Result |
|---|---|
| WARD CLIMB directly under the Ward row | PASS. Rows start Junction_J, Ward_P3, Ward_P4 |
| J at night in 5 | PASS by code. Keyboard F1, Left, Down, Down, Enter. Pad View, d-pad Left, Down, Down, A. Play opens on "Day one" (startLook 1), one Left is Night. Group headers are text, not stops. Rook's Play test confirms |
| Ward in 3 | PASS, unchanged: F1, Down, Enter (pad View, Down, A) |
| Jg follows Camp 1 in CAMPS | PASS |
| P3, P4 renames; "Lot, facing the highway" | PASS |
| "North loop: ruin" | **FAIL.** No warp, no row (25 warps in index.md). The trail is built, so a warp on the loop south of the ruin site (Valley.md E11, about (168, 270)) needs no ruin. If Wren moves it to 8.17 with the ruin, this closes |

## 2. Junction markers (Valley.md 11)

All ten are in Main3 by name: Sign_Camp, Sign_Pump, Sign_Jg, Trailhead_Board, Blaze_W1_Camp3, Sign_J plus the cairn, Blaze_Camp1_Stump, StopSign, EntranceSign, Mailbox. Ruin is 8.17.

| Junction | Seen in capture |
|---|---|
| Pump | PASS. Arms CAMP, WEST SHORE, BOATHOUSE (Pump to boathouse fwd 0) |
| Jg | PASS. Arms CAMP and "CMP1" read by eye (Trail_Ends, Camp to Jg start; S38). Spec says "Camp 1"; the Lot arm is not in frame. Check the arm text at full size |
| T | PASS. Board at Camp 2 to T start |
| J | PASS. Cairn, and the sign with NORTH LOOP and CAMP (Trail_Ends; Warps W17) |
| Camp four-arm | UNVERIFIED. Not in any frame |
| W1 blaze | UNVERIFIED. Not in any frame |
| Camp 1 stump blaze | UNVERIFIED. A stump shows at Camp 1 to J fwd 10; no blaze can be read |
| Gate T set | UNVERIFIED. Not in any frame |

Rook: one eye-height frame per marker from the trail mouth it serves, at 5 m. Marlow's four-mouth check at the Camp sign stands.
W4 baseline now: 4 of 9 seen and correct, 4 unverified, ruin 8.17.

## 3. Next place in view (was 19 frames)

| Leg | Was | Now |
|---|---|---|
| Camp to pump | fwd 40, 50, 60 | fwd 40, 50 FAIL (trench banks). fwd 60 a water glint, pass. Walk_Views: trench 15 m and 5 m show the lake; 30 m does not |
| Pump to boathouse | fwd 0, 10 | PASS, the boathouse shows at the right edge |
| Pump to W1 | fwd 0 | PASS, lake and ridge |
| Camp to Camp 3 | fwd 10 to 50 | fwd 10 to 30 a tall bare snag right of the line and fwd 40 the hollow rim: pass by eye, UNVERIFIED that the snag is the Snag (96, 146.5). fwd 50 FAIL (trunks only) |
| W1 to Camp 3 | fwd 60 to 90 | fwd 60 the camper trailer, pass. fwd 70, 80, 90 FAIL (creek banks) |
| Climb | 210, 220, 230 | FAIL, unchanged: rock fills the frame, no sky strip. Marlow's walk item stands |
| J at night | 1 | PASS for the cairn: the lamp is lit (Pairs, J night). The chute gap 18 m on still cannot be seen; that is night trail read, 8.15 |

W3 baseline now: **9 frames** (was 19).
New, not a fail from my list: Camp to pump back 70 shows ridge only, no tower. Near-camera foliage draws as large pixel blocks (Pairs Lake pump and Office day; Warps W4, W10, W16) and blocks the view; Vesper's call.

## 4. North loop trail

PASS. Camp 1 to J, 231 m, walked both ways (Checks.md, 28 of 28 trail walks). The band reads in every frame. Back frames show the Camp 1 spar or the tower; fwd 220 and 230 show the cairn and the J sign. The middle frames (fwd 50 to 100, 130 to 170) have no next place until the ruin is built (8.17).

## 5. Colliders without a renderer (535)

| Class | Count | Call |
|---|---|---|
| IW1 FrontZone/Gate/PlayerBlocker | 1 | allowed. z 167.4 to 172.6 against spec 167 to 173 |
| IW2 Ward/CairnGate/GateBlocker | 1 | allowed. z 211.3 to 214.7 |
| IW3 FrontZone/ShiftWalls/IW3_SpurGap | 1 | allowed, **size unverified**: inactive, listed 1 x 1 x 1 at (391, 210); spec is x 386 to 396. The IW3 push check passed (Checks.md). Rook: print its bounds with a shift on |
| Walkable ramps (chute 3, log steps 2) | 5 | not stops |
| Tower rail stops | 48 | inside the rails, PASS |
| Camp fire pit | 1 | inside the pit, PASS |
| Camp edge tree trunks | 18 | trunk capsules, PASS if each sits on a drawn tree; not framed |
| Lake wade limit | 96 | at the water's edge (S47), PASS |
| Climb rim mesh | 1 | rock in every climb stop frame (S21 to S34); climb, ledge and IW2 push checks 0 fail, PASS |
| Hedge colliders (Ground815/Stops) | 363 | **UNVERIFIED** |

No fourth invisible wall by name. Hedges: 28 stops near trails are framed. 25 face brush. **S3, S12, S13** face open ground at frame centre, brush only at the side: Marlow pushes those three by hand. The other ~335 hedge boxes are off trail and unframed, and some are large (Hedge_Burn_0 has a box 29.1 x 7.9 x 10.5 m). Rook: a top-down with each hedge box outlined over the brush, or the percent of each box's footprint under brush renderers. Stops off the climb are 8.15 per Wren's stage rule; this is recorded, not failed.

## 6. W1 grey baseline (8.15, not a fail)

Pass rule: trail minus floor 20 or more at 5 m and at 20 m.

| Trail | Day 5 m | Day 20 m | Night 5 m | Night 20 m |
|---|---|---|---|---|
| Camp to pump | 53 | 3 | -9 | 12 |
| Pump to boathouse | 29 | 3 | -4 | 5 |
| Boathouse to Camp 2 | 35 | 7 | 0 | -3 |
| Camp 2 to T | 43 | 7 | -4 | -9 |
| Camp to Jg | 36 | 11 | -3 | 2 |
| Jg to T | 19 | 7 | -9 | -3 |
| Jg to Camp 1 | 26 | 0 | -2 | -1 |
| Camp to Camp 3 | 27 | 4 | -3 | -4 |
| Pump to W1 | 25 | 8 | -12 | 6 |
| W1 to Camp 3 | 17 | -3 | -2 | 0 |
| W1 to cave | 17 | 0 | 0 | -6 |
| Camp to J | 39 | 18 | -5 | 14 |
| Camp 1 to J | 40 | 8 | 0 | -1 |
| J to Ward | -2 | 8 | 1 | 2 |

Baseline: day 5 m, 10 of 14 pass (fail Jg to T, W1 to Camp 3, W1 to cave, J to Ward). Day 20 m, 0 of 14 (best 18, Camp to J). Night, 0 of 14 at either distance; trail and floor both read 2 to 22, so there is nothing to contrast.
The day drop from 5 m to 20 m on every trail suggests the band fades with distance. Cause unverified (Rook).

## Baselines for 8.15 (W1 to W4)

W1 day 10 of 14 at 5 m, 0 of 14 at 20 m, night 0 of 14. W2 3 suspect hedge stops near trails, 335 hedge boxes unjudged. W3 9 frames. W4 4 of 9 seen and correct, 4 unverified.

Pim
