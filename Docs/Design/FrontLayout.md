# Front layout: gate, booth, office, store, lot (PLAN 8.22)
**DRAFT 1, 2026-10-02, Sable.** Folds in Pim (FrontLayout_UI.md), Quill (FrontLayout_Story.md) and Wren's calls of 2026-10-02 (a shift starts when the player enters the booth, Main3 3.2.6; the office door is the built west door; event R5-2's search moves to the store's ice chest). Drawing: FrontLayout.svg (office and store level 1:50, booth 1:50 and section 1:100, zone 1:500). Binding: DECISIONS 2026-09-29 (gate option B, cars until R5's storyline ends, the wall's line), 09-30 (store is its own level; invisible walls only at the front and the Ward path), 10-02. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md; ground about 3 (Marlow samples before the build). Walk 2.5 m/s. **As built** (InvisibleColliders.md): office shell x 344.1 to 355.9, z 196.1 to 203.9, partition at x 352 with a door at z 200.4 to 201.6, west door z 198.4 to 199.6; gate blocker x 395.7 to 396.3, z 167.4 to 172.6; IW3 x 385.8 to 396.2 at z 210, off.
## 1. A visit: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Arrive at T | off the Jg trail at the trailhead board (338, 170) | the lot opens: office and red mast left, the store's lit sign ahead, one car nosed at the gate; through the gate the T, a stop sign, the highway, the dead verge tree | the world, 90 m away |
| Store (Food) | 38 m across the lot to the porch; walk into the push door | canopy, ice chest, propane cage, the sign; inside, the store level (DECISIONS 09-30) | a lit counter at the end of the world |
| Office (R5, Social; Safety on CHECK) | along the walk to the open west door | counter, the ranger on his stool or asleep in back; a radio talking to nobody | a post still manned, a job no longer done |
| The car (R6, Social) | 36 m to his bay | a man living in his car, the road in his windscreen | the store 17 m away and he never goes in |
| Gate shift (gate active) | enter the booth: the shift starts | a car at the window, papers, the barrier, the road beyond | you hold the exit for other people |
| Leave | back to T | the road at your back | you can see out and never go |
## 2. Places (real practice)
1. **Booth moves to the driver's side.** US cars are left-hand drive and an inbound car heads west, so the driver sits on the **south**; the built booth (392, 176) is on the passenger side. New booth x 390.6 to 393.2, z 164.2 to 166.6, inside the fence: window on its north face (x 391.0 to 392.8, counter 1.0, shutter), door 0.9 m on its south face, stool at the window and room for a second person standing behind (Quill 4), a drawer under the counter (Quill 5), a roof light the deck sees. Cars stop with the driver's door 1.2 m off the window (section E).
2. **Barrier, not the gate, is the check.** A lift arm 1.0 high at x 389.5 across the lane, post at (389.5, 166.9), down by default. On gate-active days the chain-link gate stands open; cars stop at the arm, inside the fence. ADMIT: the arm lifts, the car goes 5 m west and up the spur (mouth flared to 8 m, x 381 to 389, for the turn). REFUSE: the car reverses 8 m out through the gate and turns on a gravel apron outside, x 400 to 412, z 162 to 178 (Quill 6). **The turning circle at (384, 160) goes.**
3. **Office, local metres from the inside SW corner (344.25, 196.25).** Front room X 0 to 7.6, back room X 7.9 to 11.5, Z 0 to 7.5, ceiling 2.7 (Quill 2: a real ceiling).

| # | Thing | Where (X, Z) | Practice | Loop job |
|---|---|---|---|---|
| O1 | West door, 1.2 m, swings in, hinged south, **open at every visit** (Door startOpen) | west wall Z 2.15 to 3.35; sweep X 0 to 1.2 empty | public door of a ranger station; the deck's line (Valley 7.6) | Safety on CHECK |
| O2 | Public side: bench, map board (no collider), loose chair, window with a blind | X 0 to 2.2; window west wall Z 5.0 to 6.2 | visitor counter lobby | none |
| O3 | Counter 1.0 high, stamps and pad; R5's stool behind it | counter X 2.2 to 2.8, Z 0 to 5.0; stool (3.3, 2.75) | permit counter | Talk to R5 from (1.5, 2.75): 1.8 m, in reach (Pim 1) |
| O4 | Radio base station on the counter's north end | (2.5, 4.5) | ranger stations log by radio | sound only (Pim) |
| O5 | Files, first aid, key board | north wall X 5.4 to 7.4 | | none |
| O6 | Back-room door, swings in, hinged north | partition Z 4.15 to 5.35 | | none |
| O7 | Cot (boots on, Quill), desk with drawer, sink and hot plate, locker | cot X 10.6 to 11.5, Z 5.3 to 7.3; desk X 10.8 to 11.5, Z 2.4 to 3.6; sink X 8.2 to 10.0, Z 0 to 0.6 | quarters behind the station | Talk to R5 lying down from (9.8, 6.3) |
4. **Store, Main3 side:** shell 12 x 9 at (366, 200), flat roof, lit sign, windows on the lot, porch light. **Porch** 2 m deep under the canopy, x 360 to 372, z 193.5 to 195.5. **Ice chest** x 368.0 to 369.6, z 194.0 to 194.8, lid opens: R5-2's search spot (Wren); 1.5 m clear of the doorway (Pim 2), 0.5 m to the propane cage (x 370.1 to 371.7). Nothing inside is built in Main3.
5. **Store level, depth 0, local from (360.3, 195.8), 11.4 x 8.4 inside, matching the shell:** push door X 5.2 to 6.2; till counter on the south wall X 7.0 to 10.4 under the east window (StoreUI: counter view faces the window); coolers along the north wall (Take: Food at (3.0, 7.2)); three gondolas 1.8 high at X 1.8, 3.9, 6.0, each 0.9 wide, Z 2.4 to 6.0; west wall shelf; freezer X 8.2 to 9.6, Z 4.0 to 4.8; bottle shelf on the east wall (Take: his brand at (10.4, 4.4)). Aisles 1.2 (ADA route 0.91). Unstaffed, honour box. What it becomes is private (StoreUI).
6. **Lot** stays 30 x 40 at x 343 to 373, z 150 to 190, gravel, marked so it reads as a lot: 8 stalls 2.7 x 5.5 nose-in on the north edge (4 by the office, one 3.6 accessible; 4 by the store), 6 trailhead stalls on the west edge z 150 to 166, wheel stops. A gravel walk z 190 to 193.5 joins the stalls to the porch and round the office's south-west corner to the west door. **R6's bay** x 367.5 to 373, z 178.0 to 180.8, nose east to the gate (Quill 10), 1 m clear all round; his drive line runs bay, drive, gate, T, south (Quill 11). Lot light on its pole as built.
7. **Vault toilet** (338, 186), 1.6 x 1.8, door east: every trailhead has one, and R5 has no other (privy practice, CampLayout 2.4).
8. **The closed campground and spur belong to this area, not 8.27:** they are R5's gate game, IW3 is its wall, and the cave is 300 m west. The loop (372, 262), the chain and the fee booth phone (390, 238) are checked here.
## 3. The loop here (2.5 m/s; Valley 1.2 legs, lot legs on paper)
| Step | From, to | m | s |
|---|---|---|---|
| 1 | cabin door to T by Jg | 236 | 94 |
| 2 | T to the store porch | 38 | 15 |
| 3 | store porch to the office west door, by the walk | 30 | 12 |
| 4 | west door to the counter talk spot | 2 | 1 |
| 5 | office west door to R6's driver door | 36 | 14 |
| 6 | R6's bay to the booth door | 25 | 10 |
| 7 | booth to T | 56 | 22 |
| 8 | T to the cabin | 236 | 94 |
Store run only (1, 2, back): 548 m, 219 s plus the store. Everything here: 659 m, 264 s plus talk, store and shift. The store is the sure Food and the far edge; forage is near and can miss (Dredge). The world takes the walk.
## 4. Walls
1. **IW1, the front invisible wall, exactly:** a box x 395.7 to 396.3, z 167.4 to 172.6, ground to 53 (as built, 0.6 x 60 x 5.2 centred (396, 23, 170)), its ends flush with both gate posts' colliders. Always on; player only, cars pass; off the interactor mask (Pim). Speaks `You can't abandon your post.` only in a shift.
2. **IW3** stays at the brush gap, x 385.8 to 396.2, z 210, shift only (Valley 8). Quill 7 asks for the spur mouth: a wall there is walked round across the open ground east of the store; the gap is the only way north. The spur mouth stays open-looking.
3. A shift runs from entering the booth to the day's last car or leaving the booth between cars (Wren). GateBooth.md 2 and 9.3 follow.
4. Every other stop is seen: the fence along x 396, the brush bands (Valley 8), building walls.
## 5. Walkability
1. Gaps under 0.6 m or 1.0 m and over, in and round every building, the bay, porch and booth.
2. Office: 1 m clear path from the west door behind the counter's north end (Z 5.0 to 7.5) to the back door, the cot side and the desk.
3. Nothing drawn crosses a built collider; Marlow's OverlapBox runs at the booth, barrier post, apron edge, ice chest, toilet and bay before Rook builds.
4. The fence has no gap a 0.35 m capsule passes, from the S band to the N band.
## 6. Area check data
1. **Bounds x 318 to 448, z 128 to 290** (Rook's z 140 to 270 misses the verge tree and the loop's north half). Cave drops its second rectangle and the Closed_Campground warp.
2. **Warps:** Office (340, 196), Store (366, 193), Trailhead_T (337, 170), Lot_Highway (382, 168) confirmed; **Gate_Booth moves to (392, 162.5) facing north**; Closed_Campground (385, 232) joins.
3. **Places:** office west door (341, 199), store door (366, 194), lot (358, 170), the T (337, 170), first sight of the lot (327, 168) confirmed; booth becomes (392, 165.4); add ice chest (368.8, 194.4), R6's bay (370, 179.4), vault toilet (338, 186), chain (390, 238).
4. **Deck must-see:** hard: office west door (341, 4.2, 199), verge tree (419, 25, 139). Loose (trees move): store roof (366, roof top + 0.1, 200), lot centre (358, 3.1, 170), R6's bay (370, G, 179.4), booth and barrier (391, G, 166), a highway stretch (430, G, 185), gate T stop sign (424.5, G, 166.5). **Must-hide: none.** The campground loop is neither.
5. **Sightlines for Rook (Wren):** S1, the car's hood (372.5, ground + 1.2, 179.4) to the verge tree trunk (418, 20, 136). S2, the Ward lookout (29, ground + 1.6, 261) to the T (428, G, 170) with the road from z 140 to 200 in frame, for the night-one car.
## 7. Borrowed, and what none of them do
Papers, Please: the booth. Kiosk and Shift at Midnight: the counter job. Exit 8: the store relearned. Fears to Fathom: a real-scale post. Firewatch: the road in view. None put the exit 30 m from the player, open, and make the only wall that speaks the one you are paid to keep.
## 8. Open questions
1. Grant: the booth on the drive's south side and the barrier inside the fence (2.1, 2.2). DECISIONS says "just inside the gate": it still is.
2. Grant: the turning circle goes; refused cars reverse onto an apron outside.
3. Wren: front owns the campground and spur (2.8); PLAN 8.27's wording follows.
4. Rook: the store and booth heights as built; push door and level load (unverified).
Sable
