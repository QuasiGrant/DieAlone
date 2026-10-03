# 8.26 gate, Marlow: checklist from the sheets and hand walk (Camp 3 and the west trails)
2026-10-03, Marlow. **No blocks, 7 hurts, 9 cosmetic.** Nothing traps the player. The hurts are about how the steps, the invisible fills, the rim views and the placeholders look and read. Rook's lines all stand as measured.

**Setup:**
- Sheets: Docs/Captures/Main3Review_camp3 (capture 03:21).
- Scene: the working tree at 8e77eb1. I changed nothing.
- Play: one session, entered at 03:39 (the Editor was stopped and not compiling) and stopped at 03:50.
- Mover: every move is PlayerController.Step (dt 0.02; walk 2.5, sprint 5.5, jump 0.6).
- Frames: rendered from a copy of Camera.main into the scratchpad, folders g826 and g826b. They are not committed.

## Gate step 2 checklist (from the sheets)
| # | Item | Result | Frames |
|---|---|---|---|
| 1 | Every trail both ways: path reads apart from ground | PASS. The tread reads in every frame. Frames are every 10 m (the gate asks for 5 m). Pump to W1 shows only FWD 40 to 70 and BACK 0 to 40 | Trail_Camp_to_Camp_3, Trail_W1_to_Camp_3, Trail_Pump_to_W1 |
| 2 | Every invisible stop, from 2 m back, has a visible reason | PASS. The only stop in the area is S48 (WadeLimit/W43, Pump to W1 74 m), and the lake edge is in frame | Stops_1 |
| 3 | Every trail end and warp: the path goes on, or ends at a place | PASS. Both Camp 3 trail ends face the fire and hanger. The W1 ends face the lake and the junction. Camp_3 N shows the camp. Camp_3_Rim N faces the Snag trunk at about 1 m (cosmetic 9) | Trail_Ends, Warps_NESW |
| 4 | Every place, outside and in: size and purpose read | **FAIL** for the camper trailer and the washed-out truck: both are untextured grey cubes (hurt 4). Camp 3 itself reads from the easel and creek frames. "Camp 3 from the rim" and "at 20 m" show only brush | Places, Found_camp3_15 and 18, Trail_W1_to_Camp_3 FWD 50 and 60, BACK 30 and 40, Trail_Pump_to_W1 BACK 20 to 40 |
| 5 to 6 | Lot road; climb legs | not this area | |
| 7 | Top-down plus compass views | PASS for this quarter | Compass_Views |
| 8 | Day and night pairs | PASS on reading. At night the Camp 3 pair has no fire light, and the water and easel show as white shapes (cosmetic 1) | Pairs_DayOne_Night |
| 9 | F1 by keyboard and pad | Pim's | |
| 10 | Hand walk | below | |

**Area frames:**
- **F1** (steps' top, heading 270) is titled "fire, pool, canvases", but shows the log line, the rocks and the Snag trunk. None of the three is in view (hurt 5).
- **F6** (the spring) is filled by a fern a metre from the eye (hurt 5).

## Hand walk, asked items
1. **The tent and its box. PASS on size; hurt on its roof (item 4 below).**
   - The box is 4.07 x 1.64 x 2.13, the same as the mesh. The door faces north, and the ground sheet at the door reads as a porch (frame 09).
   - 0 snags beside it in the walk test (item 3).
2. **The creek's fall and the water to tread edge. PASS (Rook's figures stand); cosmetic on the look.**
   - Upper, pool: 0 vertices stand above all ground within 1 m.
   - Lower: 6 of 636 vertices do, worst 0.11 m at (87.74, 124.00), at the spring.
   - All water is Blockout_Water: flat pale-blue strips with hard edges. The cascade reads as stepped blue bars and the spring as a blue zigzag (frames 16, 17, 23, 25, 35, 36).
3. **FaceRock hulls, T1, the three fills and the canvases. PASS: 0 traps, 0 snags.**

   | Test | Runs | Result |
   |---|---|---|
   | Hollow sweep: every 0.5 m over x 64 to 102, z 128 to 168, from the highest surface; 12 headings each of walk, sprint, walk-jump and sprint-jump | 198,480 runs from 4,135 starts | 1,549 cells ended off the ground. Every one escaped on a 16-heading, four-mode retry: 0 traps |
   | Walk test round the canvases, fills, tent, seat log, fire, table and easel: 2 s on 16 headings from every floor spot every 0.5 m | 9,316 runs | No snags. Every run under 0.4 m was head-on into a face or a rock corner |

   - **T1:** from the steps you can jump onto Boulder_1, BigBoulders_3 and BigBoulders_5 (x 83 to 90, z 137 to 147), and step back off each one.
   - **The fills:** **stood on, yes; snag, no.** The floor never reaches BehindCanvases, TentRockGap or TentRockGapWest. A sprint off the west rim (y +4) does (item 4).
4. **Hurt: invisible tops you land on from the west rim.** A sprint off the rim drops about 6 m onto the flat top of a box:

   | Box | Run (start, heading) | Lands at | Ends logged |
   |---|---|---|---|
   | Tent | (68.5, 4.02, 137.5) heading 60 | (74.22, -2.45, 140.80) | 34 |
   | Fire | (72.0, 4.02, 157.0) heading 150 | (75.30, -2.34, 151.28) | 6 |
   | BehindCanvases | (64.0, 4.02, 145.0) heading 90 | (70.67, -2.72, 145.05) | 31 |
   | TentRockGapWest | (66.0, 4.02, 141.0) heading 90 | (72.63, -1.92, 140.92) | 48 |
   | TentRockGap | (67.5, 4.02, 139.0) heading 90 | (74.10, -2.05, 139.00) | 19 |

   - Standing there you float over the tent ridge (frames 10, 11), above the pot hanger (frame 14), above the canvas tops (frame 08), and in air beside the tent (frame 13). Each one walks off.
   - Layout T6 says "the area flood finds no place on it". My sweep does find places on the tent roof. It is no longer a trap.
5. **Log steps P80 to P94. PASS on walking; hurt on the look.**
   - **Walk:** down and up, walk and sprint, 0 stalls. Walking each way is 13.6 m, 5.44 s; sprinting is 2.48 s.
   - **The logs run along the trail.** Each of the 19 logs is 1.82 m east-west by 0.3 to 0.8 m north-south, one every 0.73 m down the trail's middle. So they overlap into one segmented beam (F1; frames 01 to 03, 22, 32, 33). From the foot you see a stack of log ends (frame 01). Nothing reads as a step across the path.
   - **Feet sink into the logs.** The ramp top is 0.22 to 0.26 m under every log's top, at terrain height.
   - **One side stall:** at the foot, walking onto the ramp from the north side stalls at (83.12, -3.63, 144.94), heading 132, 0.14 m in 2 s. The trail line itself is clear.
6. **The rim spot. PASS on reach (steps' top to the spot: 4.65 m, 1.86 s, no stall); hurt on the view.**
   - Nothing marks the 4 x 3 flat (frame 21).
   - From its centre and west edge the view is the trench, the log line and rock walls. The floor's fire, tent, table and easel are not in view (frames 22, 32, 33). Collider rays: Boulder_1 blocks the pool; the table and easel rays are clear only through meshes that have no collider.
   - Layout section 1 promises "the line's backs, the whole floor below".
7. **The Snag line from the steps' top. Hurt.** From (96.5, 142.8), all four piece rays hit the Snag trunk collider at 0.83 to 0.91 m. In frame 31 the trunk fills the view.
   - From the floor at (80, 148), the rope and at least one piece read (frame 34). Rays from (78, 147) are clear to all four pieces.
8. **The seat log. PASS.** It reads beside the pit (frame 15). You can jump onto it (top -3.6) and step off it.
9. **The easel, the dam stand and the fire. PASS on the walks; hurt: no prompts.**
   - There is no Interactable anywhere in x 50 to 160, z 60 to 210. So `Talk`, `Sit by the fire`, `Study the painting`, `Examine` and `Clear the dam` show nothing yet.
   - The plank table stands between the arrival and the easel and form stands. A straight walk stalls at (79.04, -4.02, 146.62) and must go round it. Rook's routed times stand.
   - The pool from the dam stand reads as a flat blue tarp (frame 16). The sink stone is 0.15 m across, and 2 ends logged a stand on it.
10. **Walk times. PASS, no stall on any:**

    | Walk | Distance | Time | Rook |
    |---|---|---|---|
    | Camp to Camp 3, markers, each way | 102.7 m | 41.1 s | 103.8 m, 41.3 s |
    | W1 to Camp 3, markers, each way | 110.9 m | 44.4 s | 112.4 m, 44.6 s |
    | Pump to W1, markers, each way | 76.7 m | 30.7 s | 78.3 m, 30.9 s |
    | Steps P80 to P94, by log centres, each way | 13.6 m | 5.4 s (sprint 2.5 s) | |
    | Arrival to fire stand | 8.1 m | 4.9 s | 7.3 m, 5.0 s |
    | Arrival to dam stand | 3.6 m | 1.4 s | 4.0 m, 1.5 s |
    | Arrival to R3 stand | 4.7 m | 1.9 s | |
    | Camp_3 warp to fire stand | 7.4 m | 3.1 s | |
    | Steps' top to east rim spot | 4.7 m | 1.9 s | 5.0 m, 1.9 s |

## Hurts
1. Log steps lie along the line of travel, so they read as one beam, and feet sink 0.25 m into them (item 5).
2. Five invisible tops can be landed on from the west rim: the tent box, the fire box and the three fills (item 4).
3. The east rim spot does not see the floor, and the steps' top does not see the line (items 6 and 7).
4. The camper trailer (Body, a Cube with the Lit material, at 89.06, 112.12) and the washed-out truck (Bed and Cab cubes at 155.9, 91.8) are grey primitives (frames 26, 27). This fails checklist item 4.
5. Area frames: F1 shows none of its three targets, and F6 is a fern close-up. My retake from (83.8, 121.0) is blocked the same way (frame 24). The spring shows from the W1 tread at P72 and P74 (frames 23, 36).
6. No prompts anywhere in Camp 3 (item 9).
7. Pump to W1 sheet: 9 of the 16 frames are missing (FWD 0 to 30, BACK 50 to 70).

## Cosmetic
1. Blockout water everywhere (item 2). At night the pool and easel glow white, and the fire gives no light (Pairs).
2. Stepping stones (POI_Stepping_stones/Stone, 0.3 m thick at y -4.5) read as dark crossed sticks, not stones (frame 28; Trail_Pump_to_W1 FWD 70).
3. The W1 sign is a bare post and arms, with no lettering (frame 29; F4).
4. Side stall at the steps' foot from the north (item 5).
5. The plank table sits on the straight line from the arrival to the easel and form stands (item 9).
6. Deck, loose targets: Snag lantern and tent 0 of 128 rays, blocked by the Snag trunk (Checks.md).
7. "Camp 3 from the rim" and "Camp 3 at 20 m" (Places) show brush only.
8. Trail sheets are every 10 m, not 5.
9. The Snag trunk's texture is coarse pixel blocks within a few metres: Camp_3_Rim warp N, F1, frames 02 and 31.

## Done-check (my lines)
| Line | Result |
|---|---|
| Marlow's flood, trap and walk-into checks find 0 problems in it | **PASS on traps** (0 in 198,480 sweep runs). **Not clean on stand-ons:** 5 invisible tops can be stood on from the west rim, including T6's tent roof (hurt 2). |
| every warp in it lands | PASS (Rook; Camp_3 and Camp_3_Rim frames read) |

The done-check passes, but the task is still weak at eye height: the steps read as a beam, the rim spot sees no camp, and the trailer and truck are grey boxes.

Marlow
