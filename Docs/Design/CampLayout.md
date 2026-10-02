# Camp layout: the keeper's camp, cabin and tower (PLAN 8.21)
**DRAFT 2, 2026-10-02, Sable.** Fixes Marlow's paper check of draft 1 (Docs/Review/2026-10-02-Areas/821_Marlow_paper.md: 5 blocks, 8 hurts) against the scene objects he names, with Wren's deck call. Folds in Quill (CampLayout_Story.md) and Pim (CampLayout_UI.md). Drawing: CampLayout.svg (cabin 1:50 plan and section, camp 1:500). Binding: DECISIONS 2026-09-27 (no stockpile), 09-28 (loop, carried logbook), 09-30 (lamp), 10-02 (a real home). Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
Cabin local metres: origin the inside south-west corner (175, 165.75), X east, Z north. Walk 2.5 m/s. Capsule 0.35 radius, 1.8 tall, step 0.1. **The east side of the camp is the burn hedge** (Hedge_Burn_3 boxes from x 183.26, CS_Rock_1 and _4): nothing of ours goes east of x 183.
## 1. A day at home: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Wake | stand at the bunk edge, facing 245 | in one look: the door standing open on daylight (left), the desk and west window with the tower legs in it (centre), the stove and kettle (right) | an issued room; the bed never made |
| Out to the tower, 20 m | through the door, off the porch, along the tower's south side | the legs fill the view; the stair going up through them | an ordinary job |
| Deck | binoculars at the lectern | section 5 | Papers, Please at a window |
| Warmth, 33 m | out of the tower, past the porch to the block, split, carry the armload in, light the stove | the axe in the stump; the kettle starting to tick | the one thing home can give |
| Evening | read or file at the desk, or anywhere | the tower dark in the window | the forms are the only neat thing |
| Night, 111 m to J | out the door, round the tower's south-west leg, the lamp lit in your hand | the desk light in the window behind you; from the Ward lookout you see that window again | leaving the only safe room |
## 2. Living: where everything is (real practice)
1. **Size: 6 x 4.5 m inside, ceiling 2.7, is right.** A USFS L-4 lookout cab, 14 x 14 ft (4.3 m square), held a keeper's whole life; a ground guard cabin of about 14 x 20 ft (unverified) is our 27 m2. Keep the shell; move the inside. Home and post are split: Firewatch's Henry lives in his cab; ours cannot, or the daily climb stops being a beat.
2. **Collider sizes in this doc win over mesh sizes.** Rook sets box colliders to them: CITW_Bed (mesh 2.48 x 1.48) gets 2.0 x 0.9; CITW_Wood_Stove (1.49 x 0.64) gets 0.7 x 0.7. Mesh overhang above 0.6 m is allowed; the player spawns on the floor at (4.9, 3.2), never on the mattress.

| # | Thing | Where (local X, Z) | Practice, reference | Loop job |
|---|---|---|---|---|
| C1 | Bunk, drawers under for clothes | north wall east half, X 3.9 to 5.9, Z 3.6 to 4.5, pillow east | lookout cot, one blanket | wake; night 2 on `Sleep` = Give nothing |
| C2 | Wood stove, cooks and heats | **north-west corner**, X 0.45 to 1.15, Z 3.35 to 4.05, hearth; sheet-metal shield both walls | NFPA 211: 0.9 m to combustibles unshielded | Warmth (`Light the stove`) |
| C3 | Drying rack, kettle, pot on a hook | rack over the stove at 2.2 m, no collider | clothes dry over the stove | none |
| C4 | Wood box | north wall X 2.15 to 2.6 | beside the stove, 1.0 m off it | none |
| C5 | Supply shelf over a counter: tins, coffee, matches, a water jug; two water cans under | north wall **X 2.6 to 3.4**, Z 3.9 to 4.5; 0.5 m to the bunk (Pim 1) | lookout water was packed in by can | `Examine` only; near bare, never counted |
| C6 | Desk, one chair (static); report box, desk lamp, handheld radio on charge | west wall Z 1.0 to 2.2; window centred Z 1.6, 1 m wide | one table to eat, read and write | logbook; report filed from the carried book, anywhere |
| C7 | Washstand, basin, towel, no mirror | south wall X 4.0 to 4.6, Z 0 to 0.5 | basin wash, water heated on the stove | `Examine` if Quill writes a line |
| C8 | Door, 1.0 m clear, swings **in**, hinged west, **open at every wake**; lamp hook and extinguisher left of it | south wall X 2.5 to 3.5; sweep X 2.5 to 3.5, Z 0 to 1 empty | inward doors in snow country (Quill 8) | section 4.5 |
| C9 | Coat pegs | east wall Z 1.2 to 3.2, no collider | | none |
3. **Outside (world).** Porch 6.4 x 2 m on the south, flush (built 0.03); **both porch end railings go** (Porch/CITW_Railing). **Woodpile** stacked against the east wall, x 181.2 to 182.2, z 166 to 170.45, roof on wall brackets at 2.2 m, **no posts**; the strip to the hedge (x 182.2 to 183.26) is 1.06 m and open at both ends. **Chopping block = the built stump (182.8, 165.4)**, axe in it; 1.35 m to the porch edge (a floor, not a wall), 2.0 m or more to the hedge. E1 moves 2.7 m from (184, 163). **Generator (181, 172.8)**, a 1.2 x 0.7 box shed behind the north wall, which has no openings: 8.1 m to the west window, 7.8 m to the door, 2.0 m off the wall (CPSC: 20 ft, 6.1 m, from openings); 1.66 m to the hedge box. Powers the cab radio and lamp. Tool rack on the north end of the woodpile: Pulaski, shovel, bow saw, maul, fire bucket (the lookout's issue).
4. **Privy (176.5, 178)**, pit outhouse, outside 1.6 x 1.8 m, inside 1.2 x 1.5 with a 0.45 m seat bench, free floor 1.2 x 1.05, door 1.0 m clear facing **west** (away from the cabin, across the slope). Floor set to the ground at the door; threshold 0.1 m or less, else a StairRamp. At the clearing's north edge (Quill 12), 6.7 m north of the cabin, downhill, draining north; 77 m from J, about 83 m from the lake (Leave No Trace: 61 m). 4 m south of RedwoodHollowLog_0 and clear of RedFir5, RedFir7 and RedPine1 on paper. Path: porch west end, up the cabin's west side at x 173.5 (1.3 m off the wall, 3.8 m clear of the FootCluster barrel), to the door: 17 m, 7 s.
5. **Water and food stay walks.** No barrel at camp: a camp barrel would make Water free (DailyLoop 3.3). **Fire pit (169, 156)**, cold, no prompt: real lookouts light no open fire in fire season (open 2).
## 3. The loop, in walking order (Marlow's measured routes; 2.5 m/s)
| Step | From, to | m | s |
|---|---|---|---|
| 1 | wake spot to the door | 3.7 | 1.5 |
| 2 | door, porch, along the tower's south side, in by the 1.1 m gap at the SW leg, stair foot | 20.2 | 8.1 |
| 3 | stair up, as built | 56 | 22.4 |
| 4 | hatch north on the east walkway, round to the cab door, lectern; five looks; back to the hatch | 36 | 14.4 + 20 looks |
| 5 | stair down | 56 | 22.4 |
| 6 | out by the SW gap, south of the porch, to the stump | 24 | 9.6 |
| 7 | stump to the stove with the armload, along the porch | 9 | 3.6 |
| 8 | Water: door to pump and back | 240 | 96 |
| 9 | Night: door, south of the tower, round the SW leg, north to the clearing's WNW edge (31 m), then 80 m to J | 111 | 44 |
A camp-only day (tower plus Warmth) is 82 s of walking plus 20 s of looks, the split and the light. Interactables: Pim's 1.2 m centre and 0.5 m edge spacing holds for bunk, stove, desk, door, shelf, washstand, stump.
## 4. Walkability rules
1. One clear path 1 m wide from the door to the bunk front, stove front, desk chair and shelf; the floor X 1.3 to 5.4, Z 0.5 to 3.3 holds nothing. Thresholds flush; rugs and wall-mounted things have no collider.
2. Every gap is under 0.6 m or 1.0 m and over, in and around the cabin and privy.
3. No drawn route crosses a built collider: tower RailStops and legs, the porch, the FootCluster barrel.
4. Before Rook builds, Marlow's OverlapBox and ground samples pass at the stump, generator, privy and its path (trees there are unverified by me).
5. **Door.cs, for Rook:** (a) `startOpen`: Awake keeps the scene pose as closed, then opens; the prompt reads `Close`. (b) `fixedSwing`: the leaf opens to one set side (the cabin door: into the room) whoever uses it. (c) The cabin door is reset open at each wake, not only at scene load. (d) WouldHitPlayer stays as built.
## 5. The view from the deck (Wren's call: the path may read as a trail; the rune post and stones may not)
| Thing | From the deck | Check |
|---|---|---|
| Cabin roof, stovepipe (smoke once lit), woodpile, porch, fire pit, trail mouths | seen, below | capture |
| Privy roof | seen or under crowns; either is fine | none |
| Lake, Camp 1 spar, Camp 2 stack, Camp 3 Snag line, office west door; verge tree, lot, highway | seen (hard: verge tree, west door, cat step, Snag line) | Valley 7.6 frames |
| Ward stones and ledge | hidden by the knob, W-1 12.8 m | W-1 and pixel check |
| Fire, day-1 sheet | hidden by land, F-1 1.9 m (1.5 at jump) | F-1 |
| Cave | hidden | C-1 |
| Ruin, as built (172, 281) | hidden by Grove N1 crowns and Forest/RuinScreen | pixel check |
| Ward stair, lanterns (unlit by day), lookout | may read as a plain trail | none |
| **Rune post** | **hidden by rock**: it moves to the shelf's uphill side, and an owned BK BigBoulders rock with its collider stands on the outer rim between it and the deck, top 1 m over the post top | pixel check |
**Pixel check (Rook):** the target is painted one flat unlit colour, everything else as built; render from every deck-grid eye at Grant's view size; pass is 0 target pixels. Control: the same render with the target raised 20 m must count it, or the check is void.
## 6. Borrowed, and what none of them do
Firewatch: the lookout's ordinary life. Fears to Fathom: a real-scale home and the walk to the outhouse at night. Exit 8: one room relearned (the alternate camp, DECISIONS 2026-09-29). Papers, Please: the desk. Dredge: the home port. None make the home this poor on purpose: it gives one need, Warmth, and every other comfort is a walk the Ward is timing.
## 7. Open questions
1. Grant: bunk north-east, stove north-west, so the wake look holds door, stove and desk (Quill 1; Pim's heading becomes 245). And the fire pit cold with no prompt (fire-season ban), or FirePit.cs stays lit as in the slice.
2. Grant: a dated line that "the Ward" hidden from the tower means the stones and the rune post, not the path.
3. Quill: the lamp on its hook by day, gone at night with no pickup; E1 at the built stump keeps K4 and T6; privy door west, not north.
Sable
