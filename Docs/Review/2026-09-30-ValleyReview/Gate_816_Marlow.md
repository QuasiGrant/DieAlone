# Gate step 2, PLAN 8.14a, 8.15, 8.15a and 8.16 (the whole valley)

Marlow, 2026-09-30. Sheets: Docs/Captures/Main3Review/ (15:35, committed with e84e5ec). Every sheet opened; trails, climb, ends, stops, pairs, warps (six strips), markers, walk views and both grey sheets cropped and read.
Editor work on 869c796 plus Rook's uncommitted perf edits (PerfSpots.cs, PC_RPAsset, ProjectSettings). Play mode, PlayerController.Step, dt 0.02. Play stopped after; no files written into the project.
Stage rule: places (8.17) listed, not failed. Gate.md step 4 now has Wren's by-eye clause for day legs that fail at 20 m (869c796); I judge item 1 under it.

## Recheck of my 8.15 FAILs (Gate_8_15_Marlow.md)

1. Item 1, trails read apart from ground: **PASS by eye.** The six legs that fail at 20 m (Camp 2 to T, Jg to T, Jg to Camp 1, W1 to Camp 3, W1 to cave, J to Ward) all pass at 5 m (diff 32 to 50). In every frame the pale tread shows to the next bend, with brush or walls holding the line. Two soft spots:
   - J to Ward FWD 50: the stair top is a flat tan block, and the trail ahead cannot be told from it.
   - J to Ward FWD 210 to 230 and BACK 0 to 30: rock fills the frame. The slot holds the line, so nobody gets lost.
   - Capture fault still open: trail frames are every 10 m. Gate.md 2.1 asks for 5 m.
2. 15.1, grey slab at J: **fixed.** Pairs J DAY ONE and NIGHT, Trail_Ends CAMP 1 TO J START: cairn and tread, no slab.
3. 15.2, rock on slopes: **still FAIL.**
   - Pump trench: Walk_Views PUMP TRENCH 30 M LEFT and RIGHT, and Trail_Camp_to_pump FWD 20 to 50 and BACK 30 to 50. The banks are smooth planes with vertical streaks and a toothed top edge. They read as poured walls, not rock.
   - Camp 3 hollow: W8 S and W, W1 to Camp 3 FWD 80 to 110. Same smooth walls.
   - Hurts.
4. Slide pit (48.7, 38.3, 282.0): **gone.** Ground there is now 43.5 (pits filled in fa554fa). None of 399 drops on a 1 m grid over x 40 to 58, z 272 to 292 settle in it.
5. Still open from 8.15, unchanged:
   - Climb oppressive: rock fills 70 percent or more of 11 of 26 Climb frames (20 to 50, 120, 130, 150, 160, 210 to 230 M). Hurts (DECISIONS 2026-09-30: "never oppressive").
   - Masonry: W22 E and J TO WARD BACK 0 and 10 M. Vertical flutes like a curtain. Hurts.
   - Ward by day, looking west: W22 W and Pairs WARD PATH END DAY ONE. One flat grey plane to the horizon. Hurts.
   - Ward at night: an even row of flame cards with black rectangles behind. Vesper to grade.
   - J sign doubled label: W17 N and E, "NORT" over "TH LOOP". Cosmetic.
   - Blocky bark close up: W9 W, Jg to T FWD 30. Cosmetic.
   - Night grass cards speckle pale: Pairs Camp 1, S1 and lot NIGHT. Cosmetic.

## Checklist, forest stage (8.16)

6. Item 1, trails: **PASS by eye** (see 1).
7. Item 2, invisible stops have a visible reason: **PASS.**
   - Stops_1 S1 to S41 each face a bush, rim boulder, stone steps or rock.
   - Word for word, the 8.15 line wants invisible walls removed; 509 renderer-less colliders remain (Ground815/Stops 334, WadeLimit 96). They meet Wren's visible-reason rule, not the PLAN wording.
8. Item 3, trail ends and warps: **PASS.** Trail_Ends, 28 frames. Each goes on or ends at a place. Cave, boathouse and Camp 2 ends face grey stand-ins (8.17).
9. Item 4, places: **expected, 8.17.** Two gaps a player will see before any place:
   - The grey stand-ins below are not in the 8.17 list. 8.9a says they belong to Milestone 11 dressing.
     - POI_Boulder: four grey spheres 5 x 8 m, at (147, 182), (124, 198), (103, 102) and (312, 117). They line Camp to J, the first trail out of camp: FWD 0 to 50, BACK 10 to 50, SIGN J.
     - POI_Washed_out_truck (154, 91): Pump to W1 FWD 20 and 30.
     - POI_Camper_trailer (89, 112): W1 to Camp 3 FWD 20 and BACK 80.
     - Grey cubes at the dock and pump (Pairs Lake pump).
     - Resident spot capsules at Camp 1, Camp 2, Camp 3 and the cave: pale capsule outlines in W8 N and Pairs CAMP 3.
     - Camp 3 tent roofs, fire stones and seat log.
     - Ward/Climb RunePost, BentFir and Seep_Rock.
     - Hurts.
   - Camp 3 is not named in 8.17. Its tent and fire are grey cubes. Wren to place it.
10. Item 5, lot and office: **PASS.** W15 E and Pairs Lot: gate, highway, reflector posts and the forest beyond the road.
11. Item 6, climb legs unlike: **PASS.** Leg 1 walled chute, leg 2 open bench and knob, leg 3 dark shelf with snags, leg 4 boulders and sky, then the cleft. Oppressive: see 5.
12. Item 7, top-down and compass: **PASS.** No empty quarter.
    - North shows the crest tree line; south shows the S line; the east floor now has clumps and knots.
    - Compass EAST and W14 E: past the road the trees sit as one flat grey band. Cosmetic.
13. Item 8, day and night pairs: **PASS.** 10 of 10 comparable. At night the lamp pool reads.
14. Item 9, F1 by keyboard and pad: **PASS by pad; keyboard not re-verified.**
    - dev_panel_8_11_check.cs, run twice: all pad tasks pass (night 2 presses, Ward 3, J at night 5).
    - Every keyboard step failed both times: the panel never opened on the virtual F1 ("open state: selected none"). That caused 15 failures a run.
    - DevMenu.cs has not changed since 8eea6a4, and it passed keyboard at the 8.15 gate. So I read this as an Editor state fault, likely Game view focus. The cause is unverified. Pim's keyboard test decides.
15. Item 10, hand walk: **PASS.** Every trail both ways, sprint and sprint-hop on every landing: 56 walks, gate off. 0 stalls, except the 2 known stops 0.6 m short of Camp 1 to J's end point inside the Camp 1 pole (282, 238), a route artifact.

## Forest, as a player

16. **Trees have no colliders. You walk through every forest tree.** Blocks.
    - Forest root: 5732 objects, 0 colliders. The 41 old Giants keep theirs.
    - I walked the player at each tree from 8 m out. It reached within 1 m of the trunk axis at 89 of 93 Sequoia, 663 of 796 RedFir and RedPine, and 34 of 37 hollow logs. The rest were stopped by hedges or ground, not by the tree.
    - 10 giants stand within 10 m of a trail.
    - What you see: at 1.5 m from the axis the screen is solid brown bark. At the axis the trunk disappears and you see through it.
    - Repro: F1, warp Keepers Camp, walk Camp to J about 25 m, step 3.6 m west into Grove_Knoll/Sequoia2 at (146, 13, 174).
17. On the trails the forest is the best the valley has looked.
    - Camp 1 to J FWD 0 to 230 and BACK 0 to 230: giants with firs under them, brush at both edges, and the tread clear to the next bend.
    - Jg to Camp 1 and Boathouse to Camp 2 are close behind.
    - Floor under the groves: red and green blotches, like camouflage, in off-trail views and W6. Vesper to grade.
    - RedwoodHollowLog shows bright red and green stripes: Walk_Views PUMP TRENCH 15 M LEFT, Stops S5, S9 and S17. Cosmetic.
18. **50 m to trail rule (Valley.md 9.2): FAIL.**
    - Method: a flood fill with the real mover from every trail point, sprint moves between neighbours on a 4 m grid (9271 moves). 5851 reachable cells; 1136 are more than 50 m from a trail centre line.
    - SE, x 260 to 340, z under 150: 125 cells, worst 92 m at (339, -3).
    - Lake south shore, z under 50: 154 cells, worst 81 m at (185, 1).
    - NE, x 260 to 340, z over 180: 107 cells, worst 84 m at (339, 299).
    - East of x 340 (the front zone and the fenced SE corner): 739 cells, worst 131 m at (395, -4).
    - Valley.md 8 says the SE corner and the lake's south shore are closed by thicket. Both are open floor.
19. Can you get lost off-trail? Briefly, yes. Trapped, no.
    - At (320, 30) and (325, 290), looking N, E, S and W: trunks all round, no tower, no trail, no marker. Only the ridge face gives a direction.
    - At (185, 4) the lake shows north. At (380, 10) the fence and lot show.
    - The forest is open enough to see 40 to 60 m, so walking downhill or toward light finds a trail in under a minute.
    - Hurts.
20. Frame rate, as reported by Rook and not measured by me: dev player 1% lows 55 to 61 at Camp and S1.
    - The 60 floor is ForestPlan 7, a proposal. It is not in DECISIONS.
    - Temp/perf_baseline.txt (13:42, Editor, 3840 x 1976): Camp 118.2 average and 72.0 1% low, S1 116.7 and 84.0, Office 188.0 and 89.7.
    - No dev player result file found.
21. Night lamp: the lamp pool carries the tread about 5 to 8 m in every Grey_Trails_Night frame. The lanterns at J, the chute foot and the platforms show as points ahead on J to Ward. On Camp to J there is no light ahead between the camp and J (N1 fails at 0 and 30). Pim's rule decides.

## Done-checks, word for word

22. 8.14a, "the gate passes for the 8.14 stage lines": passes (its own FAILs closed at the 8.15 gate).
23. 8.15, "the eye-height gate passes, with Pim's W1 to W4 counts at zero failures":
    - W1 by day counts 6 of 14 failures. They pass only under the by-eye clause added in 869c796.
    - The task text "rock on slopes" is not met (3).
    - Not passed by me.
24. 8.15a, "no errors, and Pim's night rule passes on Camp to J and J to Ward":
    - Counts: Camp to J N1 6 of 8, N2 6 of 8. J to Ward N1 24 of 26, N2 12 of 15.
    - Word for word, that is not a pass unless Pim's "only where the line can be lost" clause removes those frames. Pim's call. Console errors not checked by me.
25. 8.16, "the eye-height gate passes and the frame rate is reported":
    - The frame rate is reported, so that half passes.
    - The gate does not pass (16, 18).
    - Even if the done-check passed, the forest would still be bad to walk off-trail: a player's first step off the tread goes through a tree.

## Verdict

**FAIL.** Findings 16 (no tree colliders), 18 (50 m rule), 3 (rock on slopes).
What a player notices first:
- the grey 5 x 8 m spheres on Camp to J, the first trail out of camp (9);
- then, the first time they step off a trail, a giant they walk straight through (16).

Marlow
