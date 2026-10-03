# Cave layout: the spur, the ravine, the mouth, the descent, the chamber and the side room (PLAN 8.27)
**DRAFT 2, 2026-10-02, Sable.** Fixes Marlow's paper check of draft 1 (Docs/Review/2026-10-02-Areas/827_Marlow_paper.md: 1 block, 10 hurts, 9 cosmetic). His S1 to S8 tables are the ground truth; everything he passed stands. Folds in Quill (CaveLayout_Story.md), Pim (CaveLayout_UI.md), Hollis (CaveLayout_Sound.md) and Wren's fixed calls (battery bank, cable, cold silent generator; the chant at the mouth from night 1). Drawing: CaveLayout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Capsule 0.35, step 0.1, jump 0.6, climbable top 0.70, reach 2. **The ravine as sampled (Marlow):**
- **Floor:** an east-west trench at -6.0, x 40 to 80, z 36 to 44.
- **North wall:** from about -2 at z 50 to the rim, 17 to 18 at z 58 to 64; steepest at z 52 to 56.
- **South wall:** a 60 degree slope from -5.9 at z 37 to 0 at z 33, then the flat plate at 0 over the cave.
- **The spur runs on the north bank,** 4.5 to 6 m north of a 71 to 78 degree drop, and meets the floor only at the mouth.
- **No other way down:** nothing on foot reaches the floor except along the spur.
Props sit at yaw 0, 90, 180 or 270, or carry a box from the mesh's local bounds. Removals (R) go by name.
## 1. A cave day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| W1 to the rise, 75 m | the one unsigned trail, level at -3, then up the bank to +2.3 | the old rope rail on flat ground; coloured bulbs on a dead branch beside the tread (from day 2) | you should not be here |
| Along the bank, 34 m | down 8.3 m along the ravine's north bank, rope on the drop side | a 6 m fall on your left; grey rock across the trench | grey rock, nothing to see |
| The mouth | in at the opening | day 1: boards, CLOSED - UNSAFE. From day 2: boards stacked flat inside, a drip into a can, a cold generator and jugs in a niche | someone carries things in here |
| Three ramps, 46 m | down legs 1 to 3 | a pale taped cable leading on the left; colour leaking up the rock from leg 2; chant into bass | the light comes before the room |
| The chamber | out of the level passage at (71, 12) | him on his raised seat to the right, the stack to the left, the side room door dead ahead with one light in it | the best night of your life, for one guest |
| The side room | cross his floor to the table | one table, one light, his chair facing you | the table he sets is for two |
## 2. The asks, answered
1. **The cave works as R7's home: yes, with V1 to V10.**
2. **Mouth found rule:** found at 10 m (Marlow: board and both void edges clear, 12 to 23 degrees off travel); the strip x 52 to 54.5, z 38 to 46.5 stays under 0.3 m (R below).
3. **R7 after the first meeting:** on his seat (V5) every day he is not at the table; at the table he is already in his chair (a cut). The meeting happens at the seat.
4. **Event 16's deeper passage:** V9, a dead end closed on other days; M10's one way in and out holds.
5. **The deck sees nothing of the cave:** terrain alone hides the mouth, board, jamb top, bulbs, toilet, both rail lines, the spur below the rise and the ravine floor from all 128 eyes, by 8.6 m or more (Marlow). Nothing depends on trees.
6. **The north rim (Quill 30): hidden, not stopped.** Eyes on the rim at 18 see the mouth across the ravine from 26 to 30 m. A brush band (V11) runs along the top of the north wall's steep face, so no walkable cell is left south of it. The rim plateau stays open forest; it slopes down to W1.
7. **Hollis's five, settled with reasons:**
   - (24) **Generator:** cold and silent always (Wren); the drip is the one ordinary sound.
   - (25) **Stack:** north-east corner, so the doorway is never on its line.
   - (26) **Passage:** the drip keeps z 23 to 33 free of anything that makes a sound. The niche at z 31.6 to 33.4 is silent and holds the jugs.
   - (27) **Rim height:** sampled 18.0 at (74, 60).
   - (28) **Heater:** electric and silent.
   - **Two more from the sampled ground:**
     - Hollis 12: there is no flat face at z 37.4. The echo comes off the jambs and the 60 degree slope behind them as built.
     - Hollis 13: the narrow becomes one rock row on the bank side (V10), because a south row would perch on the drop.
## 3. Places
| # | Place, where | Job |
|---|---|---|
| V1 | **Spur** as built, 108.6 m; ground equals tread (Marlow). **Rope rail moves to the drop side:** posts 1.0 m with the rope box at 0.6 (the built kind), 1.2 m south of the tread centreline, perpendicular, every 2 m, from P56 to P70, on the tread's own ground (S3's +1 m column, within 0.2 m of the tread). The 71 to 78 degree drop starts 4.5 to 6 m south of the tread. The old rail at (106.8, 57.6) goes (R); chant spot 1 stays at (107, 58) | Quill 29 |
| V2 | **Bulbs:** POI_Coloured_bulbs moves from (82.64, 4.84, 48.98), which is 3.86 m off the tread with its string 6 to 7 m over it, to 1.5 m north of the tread point (82.15, 52.96), on the tread's ground (Marlow samples). The DeadBranch capsule keeps its own height, with the string 2.6 m over the tread. From F2 (91.3, 47.4) heading 300 it then sits about 9 degrees off travel and 19 degrees up. Not on day 1 | Quill 15 |
| V3 | **Passage** (x 50.5 to 53.5, z 20.5 to 37.4, floor -6, ceiling -2; the overhang hangs at -3.74, 2.26 m over the floor, from z 33.6 to 37.4). **Drip** onto a can at (51.2, 28) against the west wall. **Generator niche,** on sampled rock: in the east wall at x 53.5 to 55.5, z 31.6 to 33.4, floor -6, ceiling -4. Terrain over it is 0.0 to -0.4 (S4 rows z 32 and 33), so the rock cover is 3.5 m or more; it is 0.95 m south of CaveMouthPit's box and clear of the east jamb (z 34.91 and north) and of the terrain holes. The opening faces west, the generator cold, the jugs beside it. **This moves Wren's z 34 to 36:** that place is open air inside the east jamb and the T1 pit box (Marlow block 1). **Cable,** pale, taped, no collider and no light: from the niche south along the east wall, round the inner wall of legs 1 to 3, then along the chamber's north wall to the bank (the leg 1 marker on the left, Pim 4.2). **Day-2 boards** lie flat in a 0.3 m stack against the west wall, x 50.55 to 51.35, z 34.0 to 37.2 (under 0.5 m, no hull). **Load-down spot:** the floor on the east side, x 52.0 to 53.4, z 35.0 to 37.0. **R:** Ground815/TrailEdges/CS_Stone_5 (53.22, 36.83) | Quill 6, 10, 11, 24 |
| V4 | **Toilet:** a pit lid 0.1 m high, with the shovel lying flat beside it, at (47.64, -6.0, 41.20), in the corner the west jamb makes with Boulder_4 (48.9, 41.2). The lid is hidden from every tread eye (S6). Nothing stands up | Quill 9 |
| V5 | **Chamber,** keep clear z 10.8 to 13.2. **Seat shelf** x 87.8 to 89.0, z 5.0 to 7.0, top 0.6, replaces ChamberDressing/Boulder_1 (R); **talk stand (86.9, 6.2) facing 98, 1.5 m from him.** **Sleep:** mattress on a crate base, x 84 to 86, z 3.5 to 4.5; electric heater at (83.4, 4.0). **Food:** cooler and crates at x 72.5 to 73.5, z 14.0 to 15.5. **Stack** inside x 85 to 88.5, z 17 to 20.5, facing (80, 12); **battery bank** 1.0 x 0.6 flush to its west side at the north wall (84.5, 20.5). **R:** ChamberDressing/RubbleSparse_2 (82.86, 18.68), which lies under the bank | Quill 5, 7, 12, 13 |
| V6 | **Side room** as built. **His chair** goes to world yaw 270 (local 180 under the RouletteTable group's 90), facing the guest and the doorway; it keeps its convex mesh collider. **Crate** C_Crate_Small_1 at yaw 0, flush in the north-west corner at (89.75, 15.25): 0 to both walls, 1.09 m to BigBoulders_1. **SideRoom/BigBoulders_0** moves 1.0 m east to (90.50, 9.51), so it no longer pokes 0.48 m into the chamber; its z 8.49 to 10.74 stays outside z 11 to 13. Bulb string at 1.9 m at the doorway. Standing point (92.0, 12.0) facing 270. Guest-chair warp stand (91.0, 12.0) facing 90, pitch -15 | Quill 16 to 19 |
| V7 | **R:** SideRoom/Boulder_0 (97.50, 13.79), which has no collider and sits in V9's opening. The other side-room rocks keep their hulls | |
| V8 | **Chamber rocks** BigBoulders_2, BigBoulders_4 and Boulder_5 keep their hulls (T2 holds) | |
| V9 | **Deeper passage:** Wall_E_Future opens at z 13.2 to 14.4, 2.1 high, into x 97.5 to 101.5, then turns north to a dead end at (101.0, 15.5). Inspect from (101.0, 14.0) facing 0. A rock box closes it on other days. The terrain over it is -1 to -2, about 13 m of cover | event 16 |
| V10 | **Narrow:** one row of owned BigBoulders on the bank side, 1.5 to 2.5 m north of the tread from x 72 to 61 (ground -0.3 to -2.1, S3), continuous, hulled, 4 m tall, so the tread runs between rock and the drop. No south row | sound |
| V11 | **Rim band:** brush 1.5 m tall over a 2 m hedge collider (the Ground815 kind), continuous along the top of the north wall's steep face from x 54 to 80, about z 58 to 59 (ground 15.7 to 18.0), tied into the slope at each end. No walkable cell is left south of it. Marlow reruns the rim eyes (58 to 78, 58 to 60) and the flood | Quill 30 |
## 4. Colliders as built (Marlow 9, corrected)
- **Cave pieces:** every Floor, Ceiling and Wall box (0.5 m skin); Mouth/RockAbove; the DayOneBoard planks, solid on day 1.
- **Mouth rocks:** jambs BigBoulders_4 (x 43.49 to 49.36, z 34.88 to 40.83) and BigBoulders_5 (x 54.64 to 60.74, z 34.91 to 40.83), and Boulder_4 (48.9, 41.2), each with the prefab's exact mesh collider. The overhang BigBoulders_2 has a convex hull.
- **Cave/Pockets/CaveMouthPit (T1):** a box at x 54 to 58, z 34.35 to 37.85, y -6.5 to -2.5, with a colliderless Boulder_4 drawn inside it.
- **Side room:** Chamber_Wall_E_S, E_N, E_Lintel, Floor, Ceiling, Wall_N, Wall_S, Wall_E_Future. The table and both chairs have the prefabs' convex mesh colliders. The crate's FitCollider box is inflated today (0.82 for 0.5, at yaw 20) and true at yaw 0. The pillar is a Fill.
- **Rocks:** the five kept side-room rocks and the four chamber rocks have convex hulls; Boulder_0 has none (R). The rubble and lantern have no colliders.
- **Spur:** rope rail posts 1.0 m with a rope box at 0.6; the DeadBranch is a capsule.
- **Trees:** no trunk within 1.5 m of the tread west of x 100, or of V10. Trees on the ravine floor: RedPine1 (49.68, 43.58), RedFir6 (64.44, 39.07), RedPine1 (81.32, 38.87), RedFir8 (41.76, 39.75).
- **R in the mouth strip:** Ground815/TrailEdges/CS_Stone_8 (52.84, 41.94), 0.40 m tall and hulled.
| Walk | m | s | | Walk | m | s |
|---|---|---|---|---|---|---|
| W1 to the mouth | 108.6 | 43 | | passage end (71, 12) to the doorway | 18.3 | 7 |
| mouth to the chamber | 73 | 29 | | doorway to the guest chair | 3.6 | 1.5 |
| camp to the table (pump 92.3, W1 78.3) | 374 | 150 | | passage end to the seat stand | 16 | 6.4 |
## 5. Trap spots, no regression
| # | Where | Found | Held by | Recheck |
|---|---|---|---|---|
| T1 | pocket by the east flank (54.4, 37) | 8.17 hand walk | CaveMouthPit box; mouth rocks' mesh colliders | untouched: V3's niche stays 0.95 m south of the box |
| T2 | Boulder_1 hung over the chamber floor | 8.17 gate | seated rocks at -18.3 | the shelf replaces it |
| T3 | side-room rocks floating or through the ceiling | 8.18a | seated, tops 1.2 to 2.0 m under the ceiling | BigBoulders_0's move keeps its bottom at -18.3 |
| T4 | ravine bank: a fall off the north bank | 827 Marlow | the floor leads on foot to the mouth: no trap | V1's rail and V10 leave no perch over the drop |
| T5 | Cave_Mouth warp under the terrain | Breaks 17 | warp seat: 0.24 m over ground | landing check |
## 6. Area check data
1. **Bounds x 40 to 132, z 0 to 72.** **Warps:** Cave_Mouth (58, 44) facing 223 (the opening's bearing); Cave_Chamber (74, 12) facing 90; **Cave_SideRoom** new (91.0, 12.0) facing 90; **Spur_Descent** new (77.3, 48.6) facing 250.
2. **Places:** mouth (52, 37.9), boards, bulbs, rope rail, toilet, drip, niche, chamber, seat shelf, sleep, food, stack, bank, side room, guest chair, his chair, deeper opening. **Interactions:** R7 from (86.9, 6.2) facing 98; guest chair from (91.0, 12.0) facing 90, pitch -15; event 16 from (101.0, 14.0) facing 0.
3. **Found frames:**
   - F1: (56.31, 46.60) heading 222: the opening (found at 10 m).
   - F2: (91.3, 47.4) heading 300: the bulbs.
   - F3: (52, 24) heading 180: the leg 1 opening and the cable.
   - F4: (71, 12) heading 90: stack, doorway, R7.
   - F5: the doorway heading 90: the guest chair.
   - F6: the standing point heading 270.
4. **Deck must-hide, hard (C-1 land rays):** mouth, boards, jamb top, bulbs, toilet, rail, spur below the rise, ravine floor. **Rim must-hide:** the mouth from the rim eyes behind V11. **Loose:** the rim line and V11 read as forest.
## 7. Borrowed, and what none of them do
Fears to Fathom: a real-scale cave somebody lives in. Inscryption: a host at a table you cross a room to reach. Mouthwashing: the party set for someone. Shift at Midnight: the generator that should hum and does not. None put the loudest room on the map under the quietest walk, with its power source cold and its table set for two.
## 8. Open questions
1. Grant: the niche at z 31.6 to 33.4, not Wren's z 34 to 36 (open air there); the rim band; the rope rail on the drop.
2. Quill: the day-2 boards lie flat inside; the shovel lies flat; R7 at the seat or the table.
3. Pim: F1 to F6; the inspect word for V9.
Sable
