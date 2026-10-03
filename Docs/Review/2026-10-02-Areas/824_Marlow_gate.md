# 8.24 gate, Marlow: checklist from the sheets and hand walk (Camp 1 and the north loop)
2026-10-02, Marlow. **FAIL** (0 blocks, 1 hurt, 4 cosmetic).

**Setup:**
- Sheets: Docs/Captures/Main3Review_north (capture 20:27).
- Scene: the working Main3.unity (last commit 12a15e2), unchanged by me.
- Play: one session, which I entered at about 21:00 and stopped at 21:07. The Editor was stopped and not compiling before I entered.
- Mover: every move is PlayerController.Step (dt 0.02; walk 2.5, sprint 5.5, jump 0.6, crouch 1.0).
- Prompt test: the interactor's own FindTarget, called by reflection with the camera set to each look.
- Frames: rendered from a copy of Camera.main.

## Gate step 2 checklist (from the sheets)
| # | Item | Result | Frames |
|---|---|---|---|
| 1 | Every trail both ways, path reads apart from ground | PASS by eye. The sheets step every 10 m, not the 5 m the gate asks for. Jg to Camp 1 shows FWD 40 to 80 and BACK 0 to 40 only; Camp 1 to J shows FWD 0 to 200 and BACK 30 to 230. The pale tread reads in every frame shown | Trail_Jg_to_Camp_1, Trail_Camp_1_to_J |
| 2 | Every invisible stop, from 2 m back, has a visible reason | PASS. S2 is the spar's dressing; S3, S4 and S5 are the fallen giant, whose log fills each frame | Stops_1 |
| 3 | Every trail end and warp: the path goes on, or ends at a place | PASS. Jg to Camp 1 start and Camp 1 to J end both end in the clearing; the second at the kid's table. W26 North_Loop_Ruin N shows the ruin; E and W show the trail. W6 Camp_1 lands in tall grass: S and W frames are mostly blades (cosmetic 5) | Trail_Ends, Warps_NESW |
| 4 | Every place, outside and in: size and purpose read | PASS. Camp 1's tent, table and kid's table read from 15 to 20 m. The ruin reads as a log cabin from the warp, east, north and the doorway; the doorway is a dark opening with the report box beside it | Places, AreaFrames_north |
| 5 | Lot and office: the road beyond the gate in frame | not this area | |
| 6 | Each climb leg: legs do not look alike | not this area | |
| 7 | Top-down plus four compass views: no empty quarter | PASS for the north quarter. The north view is forest under the ridge and backdrop; the loop reads on the map | Compass_Views, Map_TopDown |
| 8 | Day and night of the same spots | PASS. Camp 1 warp: the bulbs on the spar carry the night frame; the tent and tripod read by day | Pairs_DayOne_Night |
| 9 | F1 in Play: every section by keyboard and pad | not done this round (Pim's press counts) | |
| 10 | Hand walk of every new or changed trail | below | |

## Hand walk, asked items
1. **Forage C prompt coverage. Hurt.**
   - **On the tread, none.** I put the player at 180 points: every 0.5 m along Camp 1 to J within 9 m of the patch (30 points), at 0.0, ±0.4, ±0.8 and +1.2 m across the tread. From each I tried every look, yaw every 5 degrees and pitch 0 to 75 down. **None** got `Forage`.
   - **Further off the trail, a few.** Measured from the centreline toward the patch:

     | Distance off the centreline | Points | Points with `Forage` | Chainage |
     |---|---|---|---|
     | 1.4, 1.6 and 1.8 m | 30 each | 0 | |
     | 2.0 m | 30 | 2 | 63.6 to 64.1 |
     | 2.2 m | 30 | 3 | 63.6 to 64.6 |
     | 2.4 m | 30 | 6 | 63.5 to 65.6 |

     Every hit is on Bush_ForageC_1 or Bush_ForageC_2, looking about 30 degrees down, at 1.70 to 2.00 m.
   - **At Rook's stand.** ForageC_Stand (229.20, 270.60) is itself 2.02 m off the centreline. From it, 3 of 1,152 looks get `Forage`, the first at yaw 5, 30 down, on Bush_ForageC_1. That matches Pim's 1.94 m single stand.
   - **Effect.** A player on the trail never sees the prompt. They must step about 2 m off the centreline at chainage 63.5 to 65.6 and look down at the near shrub.
   - **Repro:** stand on the centreline at (228.78, 4.38, 268.61), marker P62, and look at Bush_ForageC_1 (229.5, 272.5).
2. **Ruin side path. PASS.** Walked straight from the mouth (168.1, 267.1) to outside the doorway (169.88, 278.88), on to the report stand (170.16, 277.32) and back to the mouth: 23.1 m, 9.2 s, no stall. Inside, the routes from the doorway to stands by each piece all walk with no stall, 0.78 to 1.08 s each: bunk (173.4, 280.4), cache trunk (same stand, 1.41 m from its centre), table and rocking chair (172.6, 279.0), stove (171.0, 282.8), report box (170.8, 278.6).
3. **Doorway step. PASS.**
   - Heights: ground 3.26 at 1.0 m outside and 3.27 at 0.5 m outside, sill 3.28, floor 3.33 at 0.5 m inside. The step is 0.05 m (the limit is 0.1).
   - Walks: 42 of 42 pass, in and out, at -0.3 to +0.3 m across the 1.2 m opening, walking, sprinting and crouched.
4. **Rocking-chair route. PASS, with a note.**
   - **The routes.** The routes to the bunk, trunk and table pass the chair with no stall. Free-standing floor in the room (0.2 m cells): 265 cells, 224 joined to the doorway by my walk grid.
   - **The nook.** Between the chair, bunk, trunk and table, (173.6 to 174.0, 279.4 to 280.2), the walk grid finds free floor it cannot join. A sprint-jump from the room gets there: 106 of 5,888 jumps end in it.
   - **Not a trap.** From each of 4 nook cells, 28 to 48 of 160 moves (32 headings: walk, sprint, both jumps, crouch) reach the joined floor, including a plain walk heading 0. So the gap is a grid artifact.
   - **What can be jumped onto.** From round the chair, 960 jumps land on the rocking chair (67), the bunk (88), the trunk (26), the table top (13; 0.83 over the floor, so a jump, not a step), the roof slab (3; it slides) and air (1). None leads anywhere higher.
5. **Full loop time. PASS.** Walking the markers:

   | Leg | Distance | Time |
   |---|---|---|
   | Camp to Jg | 105.2 m | 42.1 s |
   | Jg to Camp 1 | 80.4 m | 32.1 s |
   | Camp 1 to J | 230.8 m | 92.3 s |
   | J to camp (Camp to J, reversed) | 77.7 m | 31.5 s |
   | **Total** | **494.0 m** | **198.0 s** |

   - No stall on any leg.
   - The markers start 15 to 20 m from the camp centre at each end. NorthLayout 4's round is 559 m and 224 s from the door, plus stops.
6. **Forage C to the ruin, loop m 80 to 132, where the tower shows.**
   - **Frames.** 14 frames along Camp 1 to J, every 4 m, eye 1.6 m, facing along the trail (headings 227 to 318).
   - **The tower is never in frame.** The cab bears 182 to 206 from every frame. At m 112 to 120 it sits just above the top-left edge (viewport y 1.01 to 1.09), so a small pitch up would show it. Elsewhere it is behind the left shoulder or behind the player.
   - **What shows by eye.**
     - The ruin's log walls, right of the trail at m 100 to 108 (c1j_100, c1j_104, c1j_108).
     - A pale grey rock spire of the Ward ridge at the top centre at m 104 to 124 (c1j_104 to c1j_124). It is the only bright shape in that stretch and reads close to the tower's silhouette.
     - Otherwise dense forest, dark.
   - Frames: scratchpad g824/c1j_080 to c1j_132 (not committed).

## Cosmetic
7. **White rubble on the trail at Camp 1 to J m 184 to 186.** Five Forest/GapClumps RubbleSparse_1 clusters, 3.6 to 7.6 m across and 0.7 to 1.0 m tall, sit 1.7 to 4.8 m from the centreline. The nearest overlaps the tread edge. The "Rocks" material renders them near-white and shiny against the dark forest floor. Frames: Trail_Camp_1_to_J FWD 180 and BACK 40, Found_north (the frame with the pale pile).
8. **Camp_1 warp lands in grass.** The S and W frames are mostly near blades (Warps_NESW W6 S and W).
9. **The table can be jumped onto.** Its top is 0.83 m over the floor, so a jump reaches it (doc 4.2: "table 0.78, not climbable"). Nothing higher is in reach from it.
10. **Sheet coverage.** Trail frames are every 10 m (the gate asks for 5 m), and each direction is cut part way (item 1).

## Done-check, word for word (my lines)
| Line | Result |
|---|---|
| Marlow's flood, trap and walk-into checks find 0 problems in it | PASS for the ruin room, the doorway and the loop as walked. The chair nook escapes; no trap |
| every warp in it lands | PASS by Rook's warp check and the sheets (Camp_1, North_Loop_Ruin) |
| every place in it can be reached and found from its trails | Pim's. Forage C is reached, but its prompt does not show from the trail (finding 1) |
| seen or hidden from the tower deck as the design requires | Rook's pixel checks pass on the sheets; I did not rerun them |

Marlow
