# Gate step 2, PLAN 8.16b and 8.17 (recheck of Gate_816a_Marlow.md and Gate_817_Marlow.md)

Marlow, 2026-10-01. Sheets: Docs/Captures/Main3Review_816b (11:47, capture of 9ff222d): Places.jpg row by row, Camp to pump, W1 to Camp 3, Climb up and back, index.md, Checks.md.
Editor work: Play mode on Main3 as committed at 9ff222d (scene clean; HEAD moved to 30a25e3 during the run, docs and memory only). PlayerController.Step, dt 0.02. Walk = no sprint, no jump; sprint-jump = sprint with jump held every step. Play stopped after; nothing written into the project.
Known and deferred by Wren, not counted: 3 of 4795 trunk walks on fallen burn trunks at (190, 174) and (276, 149). Night rule is 8.18.

## Rechecks

1. **Camp 2 trap: still there. Blocks.**
   - Where: (289.6, 4.6, 113.3), 0.6 m over the ground, wedged between Camp_2/GraniteStack and Dressing/BigBoulders_0 to _3. 0.6 m from my 8.17 spot.
   - Repro: stand at (288, 110), sprint toward (290.2, 113.5). Also from (289, 118), (289, 119), (289, 120) and (290, 118) with sprint-jump. 6 of 324 approaches from a 1 m grid end there.
   - Escape: 0 of 48 tries (16 headings x walk, sprint, sprint-jump, 3 s each) move the player even 0.1 m, from each of 3 entries. The player has to quit.
   - Rook's CAMP 2 POCKET tries only my old line from (287, 107); that line now misses the pocket, so the check passes. My 1 m flood from the Camp 2 warp also reports 0 traps: it never enters the pocket at the grid's cell centres. Only the grid of direct approaches finds it.
2. **Dock end: PASS.**
   - W23_Dock now tops out at -5.0, under the deck. Walk, sprint and sprint-jump from the pump reach z 86.83 to 86.87, the end rail, at x 189.4, 190 and 190.6.
   - One stall: a straight sprint down x 190 hits Pump/Body at z 95.4. That is a route artifact; the same line at walk speed, or 0.6 m to either side, reaches the end.
   - Frame: DOCK END LOOKING BACK now stands where a player can.
3. **Boathouse step, bowl and chair: PASS.**
   - From the Lake_Boathouse warp through the boathouse and out the north doorway: walk and sprint reach the step centre (240, 56.0), the chair side (239.1, 56.0) and the bowl side (241.0, 56.3), all within 0.1 m.
4. **Payphone booth: PASS.** From the card table, walk and sprint reach (299.9 to 300.0, 99.0), inside the booth.
5. **North ruin from the tower: PASS.**
   - My method: the ruin's 37 renderers (SidePath left out) painted flat magenta, rendered from 20 deck eyes. Eyes: x 160.3 to 167.7 on the north wall line z 169.7 and 1 m in, at 1.6 and 2.2 m. The deck is the cab floor (x 160 to 168, z 162 to 170); I walked its edges and found no catwalk.
   - Result: 0 magenta pixels from every eye. The side path also showed 0. Control: painting the 184 tall renderers within 30 m of the ruin gave 2813 pixels, so the test sees what is there.
   - Rook's R-1 (0 of 1024 rays) agrees.
   - Tower from the ruin, same method: 0 px from the warp, doorway, inside and 5 m north. 2 px from 5 m east (180.8, 281.3), 19 px from the loop 12 m south (171.5, 269.3). Cosmetic: a sliver on the loop, not at the ruin.
6. **Climb's closed-in stretches: improved, still FAIL. Hurts.**
   - Sheet counts (steep rock over 35 degrees; open = sky or over 60 m), outside the cleft (222 to 252):
     - Rock 30 percent or less: 15 of 45 frames. FWD 0, 70 to 100, 170, 180, 210, 220; BACK 166, 156, 126, 56, 16, 6.
     - Open 15 percent or more: 30 of 45 (18 of 44 at 8.16a).
   - Still closed, by eye:
     - Leg 3 cwm, FWD 120 to 160: open 2, 0, 0, 0, 0. Rock all round, no sky. The snags read, but it is a dead-end bowl.
     - Leg 4 coming down, BACK 176 to 216: open 0, 0, 1, 9, 5.
     - Chute, FWD 20 to 40: flat head-wall planes with a straight diagonal edge fill the top half.
   - Cleft sky strip (3.2): none in FWD 230, BACK 236 or BACK 226.
   - ClimbFix 3.5 out items:
     - Pale quad FWD 220: gone.
     - Grey plank BACK 206 and 196: gone.
     - Pleated curtain and square pillar: still in at BACK 256 and 246. The curtain now carries boulders that hang on its face with air under them.
     - FWD 230: a dark masonry wall fills the left two thirds.
     - FWD 240: a flat black slab covers the right fifth of the frame.
7. **Trench and hollow.**
   - Pump trench: **still FAIL. Hurts.**
     - Better: FWD 20 and 30 and BACK 30 now read as brush and boulders.
     - Still the smooth poured wall: FWD 40 (folded flat wall across the whole upper frame) and BACK 40 (streaked smooth walls both sides, boulders only at the foot).
     - BACK 70 and 80: a flat untextured wall with a row of puffy boulders on top spans the background.
   - Camp 3 hollow: **PASS on the wall finding.**
     - W1 to Camp 3 FWD 80 to 110 and BACK 0 to 20: no smooth wall left. A ring of stacked boulders.
     - Still dark and closed. Places CAMP 3 CANVASES AT THE BANK shows only boulder wall, no canvases (see 9).
8. **Split snags and fallen trunks: PASS, except the deferred 3.**
   - Ward/Climb/SplitSnag_L and _R and all 30 BurnDeadwood Tree_Dead carry a CapsuleCollider.
   - Rook's trunk check: 0 of 1430 without a collider; the 3 walks into a trunk are Wren's deferred item.

## Also seen

9. **Places.jpg: frames that still show no place. Hurts the gate (2.4), not the game.**
   - OFFICE FROM SOUTH-WEST 19 M: brush fills it.
   - CAMP 3 CANVASES AT THE BANK: boulders only.
   - CAMP 3 AT 20 M: bushes fill it.
   - NORTH RUIN DOORWAY: black.
   - WARD STONES FROM THE LEDGE, EAST: a rock face and plank at arm's length; no stones read.
   - CAVE CHAMBER TO THE SIDE ROOM: dark, with a floating boulder (10).
   - Fixed since 8.17: OFFICE WEST DOOR 6 M, DOCK FROM THE PUMP TRAIL, CAMP 2 FROM THE WEST and WARD STONES FROM THE WEST now read.
10. **Cave: a boulder floats in the chamber. Hurts the look.**
    - Cave/ChamberDressing/Boulder_1 at (88.6, -13.5, 5.5): 2.8 m of air under its bottom, 1.8 m under the ceiling. It shows top right in CAVE CHAMBER TO THE SIDE ROOM.
    - SideRoom/BigBoulders_0 at (91.0, -14.5, 9.5) has 1.2 m under its centre. Not checked by eye.
11. **Grey POI stand-ins on W1 to Camp 3. Cosmetic for this stage.**
    - POI_Footbridge rails and deck: an open grey box beside the trail at FWD 20. POI_Camper_trailer body: grey box at FWD 60 and BACK 80.
    - Not in the 8.17 list (it names only the grey POI boulders). Listed so nobody grades past them.
12. **Hand walk (Gate 2.10): PASS.**
    - Every trail both ways, walk, sprint and sprint-jump, gates as they stand: 84 walks, 78 end to end.
    - The 6 stalls are all at the J to Ward day gate (CairnGate), by design.
    - The Camp 1 spar stalls from 8.17 are gone.

## Done-checks, word for word

13. 8.16b: "the gate passes for every stage line through 8.16". **Not passed.**
    - Open: the climb against ClimbFix 3 (6), the trench wall (7).
    - Closed: the lake, the snag and trunk colliders (8), the hollow wall.
14. 8.17: "the eye-height gate passes and Vesper grades every place C or better". **Not passed** on the gate half.
    - Open: the Camp 2 trap (1).
    - Closed: the dock end, the step, the booth and R-1 (2 to 5).
    - Vesper's grades are hers. Six place frames (9) still show no place.

## Verdict

**FAIL.** Blocks: 1 (Camp 2 trap). Hurts: 6 (climb), 7 (trench), 9 (place frames), 10 (floating cave boulder).

Marlow
