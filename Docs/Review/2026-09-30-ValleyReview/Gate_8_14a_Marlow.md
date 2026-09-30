# Gate step 2, PLAN 8.14a (valley ground fix pass)

Marlow, 2026-09-30. Commits 79f2afc and 61322d3. Sheets: Docs/Captures/Main3Review/ (2026-09-30 11:13). Every sheet opened; Warps_NESW, Pairs, Climb, Walk_Views and J to Ward cropped to full size. Editor walk this session (Play mode, PlayerController.Step from eval, dt 0.02; sprint, jump on every landing) plus full-size renders of the lot, gate, stack top, P4 and path end. Scene left clean, Play stopped, runInBackground false.
Stage rule (Wren): trail bands and ground textures (8.15), forest (8.16) and places (8.17) are listed, not failed.

## Checklist

1. Trails read apart from ground: **expected, 8.15.** Grey tables in index.md fail on all 14 trails, day and night (best: Camp to J 18 at 20 m by day).
2. Invisible stops have a visible reason: **PASS.** Stops_1 S1 to S47 all face a bush, log, rim boulder, stone steps or water. My scan of all 535 below.
3. Trail ends go on or end at a place: **PASS.** Trail_Ends, 28 frames.
4. Places read without labels: **expected, 8.17.**
5. Lot and office, road beyond the gate in frame: **FAIL.** Fence is now chain link and see-through (fixed). But from the lot (Pairs "Lot facing the highway", W15 E, full-size render at (360, 172) facing east) and the office (W10 E, Pairs "Office", render at (346, 196)) the drive past the gate, the T and the highway are 1 to 5 px, not readable. They read only from the gate itself (render at (393, 170): drive, pole, sign, highway line). Behind the highway a flat brown band runs the full width like a wall (also W13 E, stack top render at (295, 107)). Hurts.
6. Climb legs do not look alike: **PASS.** Climb_J_to_Ward: leg 1 walled chute (20 to 50 M), leg 2 open bench with the knob (60 to 110 M), leg 3 dark shelf with snags and gray posts (120 to 160 M), leg 4 red ground and open sky over boulders (170 to 200 M), cleft (210 to 230 M).
7. Top-down plus four compass views: **PASS for 8.14.** Compass_Views now exist. East is flat bare floor to the foothills (8.16 forest, expected). Capture fault: the views are from inside the cab; window posts, two lamps and an interior wall cover the right half of SOUTH. W3 TOWER_DECK N to W (walkway) are usable instead.
8. Day and night of the same 10 spots: **FAIL.** 10 spots now, but 2 day frames show only leaves: LAKE PUMP DAY ONE (camera inside a bush) and OFFICE DAY ONE (bush fills the left third, office wall the rest). 8 of 10 comparable. See 13.1.
9. F1 by keyboard and pad: **PASS.** Ran Tools/Recipes/dev_panel_8_11_check.cs in Play: 0 failures; night 2 presses, Ward 3, J at night 5 (at the limit), section jumps both ways by Page Up/Down and LB/RB. Virtual devices, not a physical pad.
10. Hand walk of new or changed trails: **PASS.** Every trail both ways, sprinting and sprint-hopping on every landing, steered point to point (day gates off): 0 stalls, never more than 3 m off the line. Not a keyboard walk in the Game view; driven by PlayerController.Step.

## Recheck of the 8.14 findings (Gate_8_14_Marlow.md 11)

11. 1. P4 look-back walled in: **PASS.** W21 E, render at (28, 262) facing east: valley, lake, tower cab and cabin over the 1.2 m boulder rim. Cosmetic: five near-identical boulders fill the lower 40 percent of the frame and read as a placed row.
    2. Climb oppressive: **improved, hurts.** Rock fills 70 percent or more in 10 of 26 frames (was 16): chute 20 to 50 M, leg 3 at 120 to 150 M, cleft 210 to 230 M. CLIMB 120 M: a flat dark slab (P2 roof) covers the top half of the frame.
    3. Ledge reveal: **FAIL, blocks.** Measured from the Ward warp eye (-9, 63.6, 246), rays to 9 points on each of the 120 ValleyFlames cards, invisible colliders off, temporary mesh colliders on every renderer within 60 m: **0 of 120 cards visible, no card top visible.** The lip top is 63.2 at x -10, 0.4 m under the eye and 1.0 m out, so it clears rays only to 22 degrees down; card tops sit 8.5 to 30 degrees down, bases 14.7 to 45.9. Lip blocks 113, path-end boulders (CS_Rock_1, _4) block 7. Pairs WARD PATH END DAY ONE, W22 W, Walk_Views PATH END: lip, dark face, brown far slope, gray range; no valley fire. Night: the far front as one even row like a fence, and the four ColumnLit smoke columns as flat dark-red boards. Walk_Views counts RidgeFlames only, so no check catches this.
    4. W ridge fortress: **improved, hurts.** Broken crest reads as a ridge from the deck (W3 W, Compass WEST). At J it does not: Pairs J DAY ONE, W17 N and W, CLIMB 0 and 10 M: a straight band with a flat cobbled top like a wall, the pale V slab, cones; the knob is still a smooth pyramid (CLIMB 100 and 110 M).
    5. Arms as wedges: **improved, cosmetic.** N arm has a broken crest from Camp 1 (W6 N). From the front (W12 N, W13 N) it is still one smooth plane.
    6. Masonry: **hurts.** W22 E, J TO WARD BACK 0 and 10 M: the back wall is regular vertical flutes like a curtain. BACK 120 and 190 M: a flat-topped notched block like a battlement.
    7. Pump trench: **hurts.** Banks 4 to 8 m high within 6 m of both sides from 20 to 45 m before the pump ((176, 108) to (185, 130)); side pushes stop at 1.5 to 1.8 m on hedges. Lake opens from 15 m on (Walk_Views 15 M and 5 M RIGHT). The 2 of 28 "climb outs" are both at 0 m, left side, up a 1.7 to 2.6 m bank onto open shore by the pump: not a trench escape and not a problem.
    8. Lake as a pit: **improved, cosmetic.** Shore reads open from the trail (Walk_Views 15 M RIGHT). W1 pond has a vertical dark cut along its bank (W18 E and S).
    9. Spikes: **PASS.** None left (Checks.md SPIKES). New fault in their place, 13.3.
    10. Fence as a wall: **PASS.** Chain link, see-through (W12 N, W13 E).
    11. Verge tree as a chimney: **PASS.** Dead broken tree.
    12. Leg 2 straight: closed by Wren's call (Status.md 2026-09-30).
    13. PLAN wording: the 8.14 line still says "Valley revision 8". Cosmetic.

## The lip: fix

12. Numbers from the same measurement, one change at a time:
    - Lip 0.8 m: 47 of 120 cards, tops only (120 of 1080 points). Lip 0.6 m: 76, 300 of 1080.
    - Eye 0.4 m from the lip (x -9.6): 23. Eye at Valley.md's x -8.5: 0 (lip 90, boulders 30).
    - Valley fires moved west 50 / 100 / 200 m: 14 / 43 / 103 cards, tops only (15 / 82 / 375 of 1080). 200 m puts them at x -310 to -417, on the far ridge (face x -200 to -240, front x -240 to -400): no valley left. **Moving the fires west is not enough.**
    - What I would do: at the path-end stretch (about z 238 to 254) drop the inner lip to a 0.5 m curb and put a flat catch shelf 2 m wide, about 1.5 m lower, beyond it, closed by the 1.1 m barrier rim at about x -13 with its top at 61.3 or lower. From eye 63.6 a 30 degree ray is at 61.3 at 4 m out, so every card top clears; a hop over the curb lands on the shelf, and the outer rim is the height that held all 118 lip pushes in Checks.md. Keep 1.1 m elsewhere. Check before building: the shelf under 45 degrees (steep slide), push checks from the shelf, F-1 rerun.
    - Also needed: Valley.md 4.7 still says the path end is 1.5 m from the lip "so rays to about 28 degrees clear"; with 1.1 m that is 18 degrees. Add a valley-card count from the path end to the capture's walk views with a pass line (Wren sets it). The night row and red boards (11.3) need Vesper's eye.

## The 535 colliders without a renderer

13. No unexplained stop found.
    - Method: every reachable face of every active box collider (4384 points at walk height, ground under 45 degrees, not inside another collider) checked for a visible mesh within 0.2 m; and 5592 grid walks (8 m grid, x 44 to 394, z -2 to 302, four ways, 10 m) with a capsule cast at each stall.
    - Hedges (363): 6 bare face points of 4384, each with a bush 0.2 m or less beside it ((336, 168), (173, 136), (345, 93)). Rim and hedge stalls: bush, hollow log or rim boulder within 0.7 m at every sampled stall. One weak spot: ClimbRim_Collider at (58, 254) has only 0.6 m sparse rubble in front. Cosmetic.
    - WadeLimit (96): water is the reason; 3 of 45 walks into the lake stop 1.1 to 1.5 m short on dry ground ((245, 63), (244, 53), (139, 49)). Cosmetic.
    - Tower RailStops and StairRamps (48), ChuteSteps and log-step ramps (5), EdgeTree capsules (18, trunks), FirePit: inside visible meshes. FrontZone/Gate/PlayerBlocker (IW1) and CairnGate/GateBlocker (IW2): allowed exceptions. IW3 inactive.

## New findings

14. 1. **Warps land in bushes.** W4 LAKE_PUMP N, S, W; W10 OFFICE N and W; W16 JUNCTION_JG E, S, W: the camera is inside leaves. Repro: F1, warp Lake pump. Hurts.
    2. **Stepped rims.** Camp 3 hollow (ring r 12.5 round (78, 146)) and the ravine above the cave (x 38 to 95, z 21 to 62): Ground815/Stops/Rims is 0.5 m blocks, each its own height, so every top edge is a staircase. Pairs CAMP 3 DAY ONE, W8 N and E, render at (74, 142) facing NE. Hurts.
    3. **J sign labels overlap.** W17 E: "NO NORTH LOOP", the back label showing past the arm. Cosmetic.
    4. **Slide pockets.** 6 spots where the steep-ground slide leaves the player wedged against rock or hedge with no way out: (172.9, 4.8, 117.9), (186.8, -1.7, 102.8) by the pump trench; (44.7, 49.4, 290.9), (42.7, 50.6, 294.9), (48.7, 38.3, 282.0) on leg 3's outer face. None reached by walking from the trail or camp (flood walk, 848 cells); the first is reached from (169, 121, 10.5), which walking did not reach. Jumps not flooded. Repro: teleport to the spot, try all eight ways. Cosmetic unless a way in is found.
    5. **Smoke columns by day.** All 16 SmokeColumns are inactive in day one (checked in Play). Intent not verified; the Ward path is closed by day, so low. Cosmetic.
    6. **Done-check.** Rook's scripted checks say ALL PASS while 11.3 blocks; they have no valley-fire line. Hurts.

## Expected, 8.15+ (not failed)

15. Trail grey on all 14 trails; red and green blotch ground (W6 to W14); rock textures; gray placeholders (Ward stones, cave, P2 roof, snag posts, P3 sphere at W20 N); the empty east floor and NE/SE; night frames almost black (Vesper).

## Verdict

**FAIL.** Items 5 and 8 and finding 11.3. Everything else on the 8.14 lines passes or is hurts/cosmetic.

Marlow
