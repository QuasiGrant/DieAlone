# Lake layout: pump, dock, shore, boathouse, step and the wade limit (PLAN 8.23)
**DRAFT 2, 2026-10-02, Sable.** Fixes Marlow's paper check of draft 1 (Docs/Review/2026-10-02-Areas/823_Marlow_paper.md: 0 blocks, 11 hurts) against the built objects and ground he read. Folds in Quill (LakeLayout_Story.md), Pim (LakeLayout_UI.md), Wren's calls of 2026-10-02 (eat point = the step chair; no window lamp; a wading strip inside the wade limit), Minigames.md 3.4 and FishingUI (private; nothing private is repeated here). Drawing: LakeLayout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Water -5.5, boathouse floor -3.8, dock deck -4.8. Controller 0.35 radius, 1.87 with skin, step 0.1, jump 0.6, slope limit 45, reach 2, no water slowdown. **Owned assets only; no dock pack: the dock is the built planks and CS_Log posts and rails.** The trail's last 10 m runs at x 247.8 to 248.1 (P70, P72) to the gangway foot (247.6, 52.4).
## 1. A lake day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Down to water, 120 m | two switchbacks to the trail end | water through the trunks, the tank, then the dock and pump | going down to water |
| Pump | one press at the handle | the end rail, open water, the boathouse small to the left along the shore | the sure thing |
| Shore, 84 m | the north bank path | widest sky, the rowboat at 43 m, the house on stilts, reeds, a chair and a bowl facing the tower | the one place you breathe out |
| The house | up the gangway, through the empty room | an empty boat slip, black water under the far ridge through the boat door, light in the north doorway | nobody lives here |
| The step | out the north doorway | her on the blanket, her bowl, one chair, the open edge, the tower over the trees | someone left her a bowl |
| Fish (from day 2) | the slip (dark, fast, some wrong) or the reeds (slow, sure, sky) | the rod tip against water | where you walk is the choice |
| One fish | bowl or chair | she eats, or you do | Food or her, never both |
## 2. The asks, answered
1. **The pump serves the Water chore: yes, as built.** (190, 94.8) on the dock root; the trail ends at (190, 96) facing south, the handle 1.2 m ahead. Door to pump and back 240 m, 96 s. Body 1.43 m to each dock edge.
2. **The step serves the cat: yes** (8.16b walk), reached through the house: gangway, east doorway, north doorway.
3. **The dock does not serve fishing:** 1.2 m past the water line, 62 m from her bowl, and the Water place. **"The dock" is the pump dock only** (x 188.4 to 191.6, z 86.4 to 96): pump, bucket, intake pipe, events 1 and 2; no chair, no rod. FishingUI 9.2's "dock chair" reads "step chair"; LookBoards' dock chair goes. The notch beside it is open bank joined to the deck (Marlow 11): **nothing is filled there.**
4. **The shore walk works and the wade limit holds as built.** It changes only at the shallows (L11, L12).
5. **The deck sees** the step, bowl and blanket (hard), roof, pump, dock and shore path; it hides nothing here (section 5).
## 3. Places
| # | Place, where | Job |
|---|---|---|
| L1 | Pump and the bucket under its spout, as built | `Pump water`, 1 press, by day |
| L2 | Intake pipe (owned green pipe, no collider) down the dock's east edge, elbow into the water at the end post; fix point (191.3, -4.6, 86.7), stand (190.8, 87.2), 1.57 m | event 1 (`Fix the pump` moves here) |
| L3 | Dark band on the notch bank's lake face, west of the dock, just landward of ring box W24_W's end (z 88.05): target (188.0, -5.25, 88.2), stand on the bank (187.9, 88.7), about 1.9 m. Ring boxes stay off the interactor mask (Pim's rule). Marlow confirms the reach on the ground | event 2 sample (Quill 11) |
| L4 | Rowboat: POI_Overturned_rowboat as built (226.47, 87.28). Oar (carry, 2.6 m) laid along the hull's north-west side, centre (225.65, 87.76), 3.9 m off the trail. GAP: no owned boat or oar; kitbash planks | Quill 12 |
| L5 | Boathouse as built, x 236.8 to 243.2, z 49.6 to 55.2. East doorway (z 51.8 to 53.0) and north doorway (x 239.36 to 240.64) open, no leaf, no prompt. Lamp practical and hanging lamp removed (Wren); an empty hook on the north wall at x 238.9. Resident_Lake_Spot and Pim's `Talk` row go: R4 is the cat; met = first crossing of the north doorway by day. Rook seats the roof on the walls (Marlow 13) | entry and passage |
| L6 | **Stilts rest = the empty slip** (Quill 6). Floor cut x 236.95 to 240.45, z 51.47 to 53.33; rails 1.05 on N, S, E. **Boat door: the west wall's whole middle module removed, 1.86 m wide (z 51.47 to 53.33), open to the wall top, 2.6 m over the floor**, no lintel. Rest clamped to the east rail at (240.85, 52.4): its collider sits 0.9 to 1.3 m over the floor, nothing below 0.9, so it gives no step over the rail. Stand (241.3, 52.4) facing 270. Bed under the slip -7.5. Frame: black water, the far bank and the ridge, **no sky** (the ridge stands 19 to 21 degrees up) | Fish (fast, some wrong) |
| L7 | Inside: crates as built (AABB to z 51.12), 0.29 to the slip rail; **barrel moves to the NE corner (242.65, 54.65)**, collider 0.8, flush to the east and north walls, 1.25 to the east doorway's jamb, 1.61 to the north doorway's, 1.15 off the door to doorway line; rope to (238.2, 54.75), no collider; bucket stays. Walkways 1.66 N and S, floor east of the slip 2.54 | none |
| L8 | Step as built, x 238.2 to 241.8, z 55.2 to 57.0. **The north rail goes** (it hid the bowl and her from the deck); W and E rails stay. Off the open edge is the shallows (L11), walked out by the beach, so the edge is safe. Chair (238.8, 56.4) facing the tower = the eat point; bowl (241.2, 56.5); blanket (240.7, 56.65), her place. Chair, bowl, blanket, cat: no colliders; blanket and cat off the mask | `Put it in the bowl`, `Eat it` |
| L9 | Roof as built: underside 2.41 m over the step floor at x 238.2, eave at z 55.5. The oar (2.6 m) props from (238.4, 56.0) to that eave. **The oar has no player collision** (a layer the controller ignores; the eye ray still hits it), carried or dropped | Quill 4 |
| L10 | Skirts, planks from the deck into the bed, colliders: the house on all four sides, with one gap 0.5 x 0.4 at the water line in the north face, x 237.25 to 237.75; **the step on W, N and E, so nothing walks under it** (it had 3 cm spare); the gangway's north edge, which also gets a 1.05 rail now PocketDeck_N is gone | Quill 5 |
| L11 | **Shallows:** bed flat -6.0 (0.5 m of water) x 237.0 to 241.9, z 55.2 to 60.4 outside the step skirt. **Beach** x 241.9 to 245.3, z 53.1 to 60.4, from the bank (-4.42 to -4.57) down 1.5 m over 3.4 m, 24 degrees or less; PocketDeck_N, its skirt, rails and posts go. In and out only by the beach. West pocket between the stake box and the step skirt: 1.2 m | the three wading events |
| L12 | **Stake line = the wade limit there:** CITW_Log stakes every 2 m, tops -4.7, a sagging rope, from ring W0 (overlap as Marlow found) west along z 60.4 to x 236.8, south to the house NW corner. Wade box on it, 0.4 thick, top -4.0. Bed drops to -7.5 outside it **west of x 240 only**; ring boxes W93 to W95 go | visible stop |
| L13 | **Reed bed in the water only:** x 240.5 to 242.6, z 60.8 to 63.5, outside the stake line, bed kept at -6.0 or shallower. **Reeds rest** (243.6, 62.0) on the bank by ring W1; stand (244.1, 62.0), ground about -5.0, 1.5 m from the nearest reed, facing 290 over them; ridge trees unverified for the sky | Fish (slow, sure) |
| L14 | Event points, inside the shallows: under the stilts, stand (237.5, -6.0, 55.95) at the gap; off the step, (240.0, 57.6) on the bed below the open edge; drifts ashore, caught on the rope (238.5, 60.0) | Quill 14 |
| L15 | Soft ground 1.2 x 0.7, x 249.4 to 250.6, z 58.0 to 58.7, east of the trail, 1.3 m clear of its edge | Quill 15 |
Rests from the step centre: slip 4.3 m, reeds 6.9 m. The old Stilts rest (242, 46) lies behind Hedge_LakeSouth_East, in the closed south belt.
## 4. Times and walkability (2.5 m/s; Marlow's paper figures)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| door to pump | 120 | 48 | | step to slip stand | 4.1 | 2 |
| pump to gangway foot | 83.6 | 33 | | step to reeds stand, by the trail; off the edge and the beach | 20; 12 | 8; 5 |
| gangway foot to step | 9.4 | 4 | | gangway foot to E-a, round the step skirt | 13 | 5 |
1. Every gap is under 0.6 m or 1.0 m and over: crate 0.29, barrel as L7, west pocket 1.2, step skirt to the stake line 3.2, walkways 1.66.
2. No step into deep water: slip and step side rails 1.05, rest collider 0.9 or higher, oar no collision, crate top 0.80 and barrel 0.99 not climbable. Every way off the step or gangway lands in the shallows or on the beach.
3. Before Rook builds, Marlow samples L3, L4's oar spot, L13's stand and L15 on the ground.
## 5. Area check data
1. **Bounds x 133 to 262, z 25 to 130.** **Warps:** Lake_Pump (190, 99) facing south; Lake_Boathouse (248.5, 52.4) facing west.
2. **Places, on their transforms:** pump, dock end rail, POI_Overturned_rowboat, POI_Water_tank (186.34, 124.53), east doorway (243.2, 52.4), step (Lake/Boathouse/Dressing/Step), slip rest, reeds rest, beach (243.6, 57.0), stake line, soft ground. Rook's (233, 62) "cat step" goes.
3. **Deck must-see, hard, every mesh from the 364 deck eyes:** blanket (240.7, -3.6, 56.65) and bowl (241.2, -3.7, 56.5). Loose: roof (240, -1.0, 52.4), pump, dock end, mid water (190, -5.4, 60), reed bed (241.5, -4.6, 62.2), stake line (240, -4.8, 60.4), rowboat, shore path east (255, G, 55). **Must-hide: none.** At night no window at the lake is lit (practicals drop by one: Lake/Boathouse/Dressing/Lamp).
4. **Sightlines:** S1, seated at the chair (238.8, -2.6, 56.4) to the tower cab: clear by meshes. S2, reeds stance heading 290: far shore and sky in frame. S3, slip stance heading 270: far shore and ridge, no sky (by design).
## 6. Borrowed, and what none of them do
Dredge: the line and the home port. Firewatch: the lake you are sent to look at. Fears to Fathom: a real-scale boathouse you wade beside. Papers, Please: the deck checks her step. None hand you one fish a day and make you choose whose need it meets.
## 7. Open questions
1. Grant: the slip inside the boathouse as the Stilts spot with a 1.86 m boat door; the shallows behind a stake and rope line; the step's north rail removed.
2. Quill: the slip frame has no sky (ridge 19 to 21 degrees); only the reeds keep Quill 8's sky. The drift point on the rope, the empty hook, the soft ground spot.
3. Pim: LakeLayout_UI.md follows: no `Talk`, no door prompt, `Fix the pump` at the intake, the sample at the band, met at the north doorway.
Sable
