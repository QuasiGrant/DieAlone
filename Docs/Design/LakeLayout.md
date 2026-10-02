# Lake layout: pump, dock, shore, boathouse, step and the wade limit (PLAN 8.23)
**DRAFT 1, 2026-10-02, Sable.** Folds in Quill (LakeLayout_Story.md), Pim (LakeLayout_UI.md), Wren's calls of 2026-10-02 (eat point = the step chair; no window lamp; a wading strip inside the wade limit), Minigames.md 3.4 and FishingUI (private; nothing private is repeated here). Drawing: LakeLayout.svg. Built positions are from Tools/Recipes/main3_8_4_lake.cs, main3_8_17_lake.cs, main3_8_15_ground.cs and Marlow's 8.16b walk. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Water -5.5, boathouse floor -3.8, dock deck -4.8. Walk 2.5 m/s, capsule 0.35 x 1.8, step 0.1, jump 0.6, reach 2. **Owned assets only; no dock pack: the dock is the built C_Plank_A_Thick planks and CS_Log posts and rails.**
## 1. A lake day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Down to water, 120 m | two switchbacks to the trail end | water through the trunks, the tank, then the dock and pump | going down to water |
| Pump | one press at the handle | the end rail, open water, the boathouse small to the left along the shore | the sure thing |
| Shore, 84 m | the north bank path | widest sky, the rowboat at 43 m, the house on stilts, reeds, a chair and a bowl facing the tower | the one place you breathe out |
| The house | up the gangway, through the empty room | an empty boat slip, dark water out the boat door, light in the north doorway | nobody lives here |
| The step | out the north doorway | her on the blanket, her bowl, one chair, the tower over the trees | someone left her a bowl |
| Fish (from day 2) | the slip (dark, fast, some wrong) or the reeds (slow, sure) | the rod tip against water, the far shore, the low sun | where you walk is the choice |
| One fish | bowl or chair | she eats, or you do | Food or her, never both |
## 2. The asks, answered
1. **The pump serves the Water chore: yes, as built.** (190, 94.8) on the dock root; the trail ends at (190, 96) facing south, the handle 1.2 m ahead, the spout and bucket south of it. Door to pump and back 240 m, 96 s (Marlow). Body to each dock edge 1.43 m.
2. **The step serves the cat: yes, as built** (Marlow 8.16b: walk and sprint reach the chair and bowl). Reached through the house: gangway, east door, north doorway.
3. **The dock does not serve fishing.** It is 1.2 m past the water line (shallow, not dark), 62 m from her bowl, and it is the Water place; Pim agrees. **"The dock" means the pump dock only** (x 188.4 to 191.6, z 86.4 to 96). It holds the pump, the bucket, the intake pipe and event 1 and 2 points; no chair, no rod. FishingUI 9.2's "dock chair" reads "step chair"; LookBoards' dock chair goes.
4. **The shore walk works and the wade limit holds as built** (8.16b: dock end and step PASS; the ring at the water line, 1.5 m over the bank). The ring changes only at the shallows (L9, L10).
5. **The deck sees** the step, roof, pump, dock and shore path; it must hide nothing here (section 5).
## 3. Places (L = layout)
| # | Place, where | Job |
|---|---|---|
| L1 | Pump and the bucket under its spout, as built | `Pump water`, 1 press, by day |
| L2 | Intake pipe (owned green pipe, no collider) down the dock's east edge, elbow into the water at the end post; fix point (191.3, -4.6, 86.7), stand (190.8, 87.2), 1.6 m | event 1 (Pim's `Fix the pump` moves here) |
| L3 | Dark band at the notch's west water line; target (188.1, -4.7, 88.6), stand on the deck at (188.75, 88.9) where the side rail ends, 1.7 m | event 2 sample (Quill 11) |
| L4 | Overturned rowboat as built (223.6, 83.2); oar (carry, 2.7 m) on its trail side (224.6, 84.6). GAP: no owned boat or oar, kitbash planks | Quill 12 |
| L5 | Boathouse as built, x 236.8 to 243.2, z 49.6 to 55.2. East doorway (z 51.8 to 53.0) and north doorway (x 239.36 to 240.64) **both open, no leaf, no prompt**. Lamp practical and hanging lamp removed (Wren); an empty hook on the north wall at x 238.9. Resident_Lake_Spot and Pim's `Talk` row go: R4 is the cat; met = first crossing of the north doorway by day | entry and passage |
| L6 | **Stilts rest = the empty slip** (Quill 6). Floor cut x 236.95 to 240.45, z 51.47 to 53.33; rails 1.05 on N, S, E; the west wall's middle module opens as the boat door. Rest in the east rail (240.85, 52.4), stand (241.3, 52.4) facing 270: dark water, far shore and sky through the door. Bed under it -7.5 | Fish (fast, some wrong) |
| L7 | Inside: crates (237.6, 50.5) stay, 0.51 to the slip rail; barrel moves to (237.25, 54.75), 1.06 to the rail; rope to (238.2, 54.75); bucket stays. Walkways 1.66 N and S, floor east of the slip 2.54 | none |
| L8 | Step as built, x 238.2 to 241.8, z 55.2 to 57.0, rails W, E, N 1.05. Chair (238.8, 56.4) facing the tower = the eat point; bowl (241.2, 56.5); blanket (240.7, 56.65), her place. Chair, bowl, blanket, cat: no colliders; blanket and cat off the mask | `Put it in the bowl`, `Eat it` |
| L9 | Roof: the eave over the step's west end 2.5 m or less above the step floor (about 2.4 if the 6 degree slope falls west; Rook confirms); the oar props there from (238.4, 56.9) | Quill 4 |
| L10 | Skirt: planks from the floor into the bed on all four sides, colliders, and under the gangway's north edge. One gap 0.5 x 0.4 at the water line, north face, x 237.25 to 237.75. Only she goes under | Quill 5 |
| L11 | **Shallows (Wren's wading strip):** bed flat -6.0 (0.5 m of water), x 237.0 to 243.2, z 55.2 to 60.4, 1.9 m clear under the step. Entry: PocketDeck_N, its skirt, rails and posts go; the bank there becomes a beach from the trail edge (x 245.3) to the bed, no face over 25 degrees. In and out only by the beach | the three wading events |
| L12 | **Stake line = the wade limit there:** CITW_Log stakes 0.8 m out of the water every 2 m, a sagging rope, from where it meets the ring on the bank near (243.5, 60.4) west to (236.8, 60.4), south to the house NW corner. A wade box on it, 0.4 thick, top -4.0, 0.1 into the ring and the skirt. Bed drops to -7.5 outside. Ring boxes inside the outline go | visible stop |
| L13 | Reed bed moves from the west side to outside the stake line, x 239 to 244, z 60.6 to 64. **Reeds rest** on the bank (243.9, 61.2), stand (244.3, 61.0) facing 290 over the reeds | Fish (slow, sure) |
| L14 | Event points, 2.4 m or more apart: under the stilts, stand (237.5, -6.0, 55.95) at the gap; off the step, (240.0, 57.4) on the bed; drifts ashore, caught on the rope (238.5, 60.0) | Quill 14 |
| L15 | Soft ground 1.2 x 0.7 at (246.8, 58.5), 1.8 m east of the trail | Quill 15 |
Rests from the step centre: slip 4.3 m, reeds 6.4 m (Quill: within 10). The old Stilts rest (242, 46) lies behind Hedge_LakeSouth_East, in the closed south belt.
## 4. Times and walkability (2.5 m/s; Marlow measures on the build)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| door to pump | 120 | 48 | | step to slip rest | 4 | 2 |
| pump to gangway foot | 84 | 34 | | step to reeds rest, round by the trail | 19 | 8 |
| gangway foot to step | 9.5 | 4 | | gangway foot to the gap by the beach | 12 | 5 |
A lake day (pump, step, fish the slip, bowl, home by the pump): 435 m, 174 s, plus 2 s at the pump and 6 to 40 s of waiting for a bite.
1. Every gap is under 0.6 m or 1.0 m and over: the numbers in L1, L7, and the step legs 1.19 to the stake box, 1.51 to the skirt, 3.18 apart.
2. **Measure first** (Marlow, before Rook builds): ground on a 0.5 m grid at x 236 to 248, z 50 to 65 (beach, strip, reed rest, soft ground), the notch both sides of the dock (a strip beside the dock of 0.6 to 1.0 m is filled with rock), the rowboat 3 m round, the roof's low eave, the CharacterController slope limit. Positions not sampled are unverified.
3. No trap: rails 1.05 on the step and slip (jump 0.6); nothing in the strip under 1.9 m clear; the gangway's underside skirted.
## 5. Area check data
1. **Bounds x 133 to 262, z 25 to 130** (the whole water and ring; shares the W1 shore path with 8.26).
2. **Warps:** Lake_Pump (190, 99) facing south; Lake_Boathouse (248.5, 52.4) facing west.
3. **Places, on their transforms:** pump, dock end rail, rowboat, water tank (182, 128), east doorway (243.2, 52.4), step (Lake/Boathouse/Dressing/Step), slip rest, reeds rest, beach (244.3, 57.0), stake line, soft ground. Rook's (233, 62) "cat step" goes.
4. **Deck must-see:** hard, every mesh: the step (240, -3.3, 56.5). Loose: boathouse roof (240, -1.0, 52.4), pump (190, -3.6, 94.8), dock end (190, -4.3, 86.6), mid water (190, -5.4, 60), reed bed (241.5, -4.6, 62.3), stake line (240, -4.8, 60.4), rowboat, shore path east (255, G, 55). **Must-hide: none.** At night no window at the lake is lit (practicals count drops by one).
5. **Sightlines:** S1, seated at the chair (238.8, -2.6, 56.4) to the tower cab: clear by meshes. S2, reeds stance heading 290, and S3, slip stance heading 270: far shore and sky in frame.
## 6. Borrowed, and what none of them do
Dredge: the line and the home port. Firewatch: the lake you are sent to look at. Fears to Fathom: a real-scale boathouse you wade under. Papers, Please: the deck checks her step. None hand you one fish a day and make you choose whose need it meets.
## 7. Open questions
1. Grant: the slip inside the boathouse as the Stilts spot, with a boat door west; the shallows north of the house behind a stake and rope line, PocketDeck_N becoming the beach.
2. Pim: LakeLayout_UI.md follows: no `Talk`, no door prompt, `Fix the pump` at the intake, the sample at the band, met at the north doorway.
3. Quill: the drift point on the rope, the empty hook, the soft ground spot.
Sable
