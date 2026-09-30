# Gate step 2, PLAN 8.16a (recheck of Gate_816_Marlow.md)

Marlow, 2026-09-30. Sheets: Docs/Captures/Main3Review (18:27, capture of eb7d8cd). Climb up and back, Walk_Views, Camp to pump, W1 to Camp 3 and Checks.md cropped and read.
Editor work: Play mode on Main3 as committed at eb7d8cd (HEAD 3b3bbf5 changes docs only; Rook's untracked PlaceKit.cs and 8.17 recipe present, scene unmodified). PlayerController.Step, dt 0.02. Play stopped after; nothing written into the project.
Stage rule: places (8.17) and fire and night look (8.18) listed, not failed.

## Rechecks

1. Trees walked through: **mostly fixed; 3 gaps.**
   - My own walk: every tree, snag and log in the scene (1428, including 43 SliceLook giants that stand on 8.3 giants), 4549 sprint-jump walks from 6 m at 4 headings. 0 into a giant, 0 into a hollow log.
   - My 8.16 repro (Sequoia2 at (146, 174)): no tree stands there now.
   - Gap a: Ward/Climb/SplitSnag_L (36.8, 47.3, 290.9) and SplitSnag_R (39.0, 47.3, 291.9) have no collider. They are the named leg 3 beat, 1 m off the tread. The player walks into either one and the screen fills with blocky bark. Repro: warp Ward path, walk up to chainage 145, step 2 m south into the snag. Hurts.
   - Gap b: 10 fallen Tree_Dead in Forest/BurnDeadwood have no collider, 4 within 6 m of a trail: (250, 167), (296, 187), (277, 196), (317, 146). The player stands inside the trunk. Repro: from (250, 5.6, 161) walk toward (254, 164). Hurts.
   - Gap c: Grove_BoathouseE RedPine1 (284.7, 37.4) and RedPine3 (291.0, 31.6): the walker's centre reached 0.15 m from the axis, inside the 0.25 to 0.28 m collider radius. Cause not found. Cosmetic until repeated by hand.
   - Rook's check counts 1226 trees; the brief says 1210. His check does not cover BurnDeadwood fallen trees or the Ward snags.
2. 50 m rule: **word for word FAIL; SE corner and lake south shore PASS.**
   - My flood (4 m grid, jump-walks, 8 neighbours, gate off): 5060 cells reached, 0 in the SE corner rectangle, 0 on the lake south shore.
   - Over 50 m from a trail centre line: 407 cells. Over 50 m from a trail or front-zone road: 19 cells, worst 63 m.
   - 18 of the 19 are in the lake (see 3). The other is (135, -1, 5), 51 m.
   - Valley.md 9.2 still reads "No walkable point is more than 50 m from a trail." Counting roads as paths, and passing far cells when the tower or a trail is in sight, is Rook's reading of Wren's fix list. Neither is a dated line in DECISIONS.md or a Valley.md edit. Under Valley.md as written, 407 cells fail. Wren's call.
3. **New: the lake wade limit leaks. Blocks.**
   - From Pump to W1 point P48 (144.4, -4.3, 81.7), sprint-jump south-south-east toward (147.5, 72.6). The player clears the wade limit near (144.7, 76.5). Walking and sprinting without a jump hold there.
   - From there you walk the lake bed to (187, -7.4, 63) and beyond. The water plane is at -5.5; the eye reaches -5.9, under the water. You then see the water sheet from below, with the sky through it and a dark lake floor.
   - Valley.md 8: "wade limit at the visible edge".
   - Rook's reach check did not find it.
4. SE corner and lake south shore: **closed** (2). Hedges hold from every direction my flood tried.
5. Rock on the pump trench and Camp 3 hollow: **still FAIL. Hurts.**
   - Trench: Camp to pump FWD 20 to 40 and BACK 30 to 40. The new boulders cover the foot. Above them the smooth poured wall, vertical streaks and toothed top still fill the upper frame.
   - Hollow: W1 to Camp 3 FWD 80, 100 and 110. Boulders ring the floor; the smooth wall fills the frame above them.
   - Red and green striped log and ground patch still show: Camp to pump FWD 20, BACK 30; Walk_Views PUMP TRENCH 15 M LEFT.
6. The climb after ClimbFix: **improved, still FAIL. Hurts.**
   - Skips: a flood from each leg (3 m grid, sprint and sprint-jump, gate off) found no route to a later leg that saves 25 m or more of trail. ClimbFix 2.5 holds on this grid.
   - ClimbFix 3.1, rock 30 percent or less outside the cleft (sheet numbers): 2 of 44 frames (BACK 16, 6). The sheet's rock count includes the tread, so this overstates the rock. Vesper's new count is not yet in a capture.
   - ClimbFix 3.2, sky 15 percent or more outside the cleft: 18 of 44. Every cleft frame should show a sky strip; FWD 230, BACK 226 and 236 show none.
   - ClimbFix 3.5 "out" items still in:
     - pale quad at FWD 220;
     - pleated curtain and box pillar at BACK 256 and 246;
     - a grey plank floating across the trail at BACK 206 and 196.
   - ClimbFix 3.3, the valley in the right third of every leg 2 frame going up: mostly rim boulders there.
7. F1 by keyboard: **PASS.** Ran dev_panel_8_11_check.cs: 26 PASS, 0 failures, keyboard and pad. The check now forces input to the Game view, so it proves the panel, not focus handling in the Editor.
8. Hand walk: **PASS.** Every trail both ways, sprint and sprint-jump, gates off: 56 walks. 2 stalls, both on Camp 2 to T going back, 1.1 m short of P0 (298.9, 107.8). The end point sits behind StackPath/Ramp1_Rail. That is a route artifact; the place is reachable around the rail. Cosmetic.

## Is the climb still oppressive? (as a player, from the frames)

9. Yes, in three of five stretches. About 19 of 52 frames are mostly wall.
   - Leg 1 chute, FWD 20 to 40: the stepped head wall fills the top half, sky 4 to 10. Boulders shoulder the tread. It reads as a walled gully, not "close, not shut".
   - Leg 2 shelf, FWD 60 to 110 and BACK 96 to 136: open, with trees, sky and a drop. This is the good stretch.
   - Leg 3 cwm, FWD 120 to 160: no sky. The head wall covers two thirds of the frame. The snags help, but you walk into a dead end of rock.
   - Leg 4 going up, FWD 170 to 200: open to sky and horizon. Coming down, BACK 176 to 206: walls both sides, sky 0 to 6, and the grey plank.
   - Cleft, FWD 210 to 230 and BACK 216 to 256: dark box slabs, the pale quad and the curtain. It is exempt from all-wall, but not from the look.
   - The descent below 166 m is the best view in the valley: the grove, the lake and the tower below.

## Not rechecked this pass (still open from 8.16)

10. Not rechecked:
    - masonry curtain (see 6);
    - Ward by day looking west, one flat plane (Walk_Views FIN frames, 8.18);
    - J sign doubled label;
    - blocky bark;
    - night grass speckle;
    - grey stand-ins (8.17).

## Done-check, word for word

11. 8.16a: "the gate passes for every stage line through 8.16". **Not passed.**
    - Open: the lake leak (3), rock on the trench and hollow (5), the climb against ClimbFix 3 (6), and the 50 m rule as Valley.md 9.2 reads (2).
    - Collider gaps: the Ward snags and the BurnDeadwood fallen trees (1).
    - Closed: tree colliders for standing forest trees, the SE corner, the lake south shore, F1 keyboard, trail walks, and leg skips.

## Verdict

**FAIL.** Blocks: 3 (you can walk the lake bed under the water). Hurts: 5, 6, 1a and 1b. Word-for-word: 2.

Marlow
