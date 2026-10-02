# Cave layout: the spur, the ravine, the mouth, the descent, the chamber and the side room (PLAN 8.27)
**DRAFT 1, 2026-10-02, Sable.** Folds in Quill (CaveLayout_Story.md), Pim (CaveLayout_UI.md), Hollis (CaveLayout_Sound.md) and Wren's fixed calls (a battery bank with a cable to a cold, silent generator niche inside the mouth; the chant at the mouth from night 1). Drawing: CaveLayout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Capsule 0.35, step 0.1, jump 0.6, climbable top 0.70, reach 2. **What is measured:** the cave pieces are exact boxes (main3_8_8_cave.cs, main3_8_17_cave.cs); the spur's ground is its trail points read from Main3.unity. **What is not:** I cannot read the terrain asset without a shell, so I draw no line on the ravine floor, walls or rim. Every off-tread point here is a sample request for Marlow (S1 to S8, section 4), and the rim and ravine are drawn only as his samples show them. Props sit at yaw 0, 90, 180 or 270 with boxes from mesh local bounds (FitCollider inflates turned props). Removals (R) go by name; keep-outs (K) go into 8.19 and 8.16.
## 1. A cave day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| W1 to the rise, 75 m | the one unsigned trail, level at -3 then up to +2.3 | the rope rail (on the old spot), then coloured bulbs on a dead branch (from day 2) | you should not be here |
| The descent, 34 m | down 8.3 m into the ravine, rope in hand | grey rock closing in; a rock face ahead that you hear before you see | grey rock, nothing to see |
| The mouth | in at the opening | day 1: boards, CLOSED - UNSAFE. Day 2 on: the boards stacked by the door; water jugs, a drip into a can, a cold generator in a niche | someone carries things in here |
| Three ramps, 46 m | down legs 1 to 3 | a pale taped cable on the wall leading on; colour leaking up the rock from leg 2; chant into bass | the light comes before the room |
| The chamber | out of the level passage at (71, 12) | him on his raised seat to the right, the stack to the left, the side room door dead ahead with one light in it | the best night of your life, for one guest |
| The side room | cross his floor to the table | one table, one light, his chair facing you | the table he sets is for two |
## 2. The asks, answered
1. **The cave works as R7's home: yes, with V1 to V10:** he sleeps, eats, drinks, keeps warm and powers his party here, and everything faces in.
2. **The mouth's found rule (Pim 3.2): keep the tread, clear a strip.** The built last segment runs from (55.96, -3.74, 46.22) to (52.14, -6.0, 37.62), heading 204, 24 degrees off the opening's axis, not 51 (Pim measured the chord from (84, 64)). From it the line to the opening centre crosses z 39.7 at x 52.9, inside the opening and west of the east scree (x 54.7 and more). **Keep clear:** x 52.0 to 54.5, z 38 to 46.5, nothing over 0.3 m. Frames from 30, 10 and 5 m decide; if they fail, the last 10 m comes in along x 52 to 54 on Marlow's samples.
3. **Where R7 stands after the first meeting:** on his raised seat (V5) every day he is not at the table; at the table he is already in his chair when you enter (a cut, never a walk). The meeting happens at the seat. Resident_Cave_Spot moves there.
4. **Event 16's "deeper" passage:** through Wall_E_Future at z 13.2 to 14.4, a 1.2 x 2.1 m opening into a 4 m dead end (V9). It is closed by a rock box on every other day. It never leads out, so M10's "one way in and out" holds.
5. **The deck sees nothing of the cave:** C-1 (land rays) reruns with the boards, the bulbs, the rope rail, the toilet and the mouth as hard must-hide targets. The ravine grove crowns may show as forest.
6. **Hollis's five conflicts, settled:**
   - (24) **Generator:** cold and silent always (Wren's call). The drip stays the cave's one ordinary sound, and the chant at the mouth is clean.
   - (25) **Speaker stack:** goes in the north-east corner, Hollis's footprint, so the stack-to-doorway line is never direct and the side room stays muffled (Quill 20).
   - (26) **Passage split:** the drip keeps z 23 to 33 clear; jugs and the load-down spot sit at z 33 to 37 on the west side, the niche on the east at z 34 to 36. The day-2 boards stand outside, against the west jamb, in sight from the tread (Quill 24).
   - (27) **Rim height:** whatever Marlow samples; Locations 11.2 follows the layout and C-1 passes as built.
   - (28) **Heater:** electric, on the battery bank, silent: the no-hum rule.
## 3. Places
| # | Place, where | Job |
|---|---|---|
| V1 | **Spur** as built (W1 to cave, 109 m), ground from its points: -4.5 at W1, -3.0 at the rope rail (106.8, 57.6), -3.4 at (97.4, 46.2), +0.6 at the bulbs (82.2, 53.0), +2.3 at the rise (77.3, 48.6), -0.5 at (67.9, 46.1), -2.7 at (59.5, 48.6), -6.0 at the mouth. **The built spur has no drop at the rope rail;** its descent is the rise to the mouth, 8.3 m in about 34 m. **The rope rail moves to the descent's steep part,** (77.3, 48.6) to (67.9, 46.1), 1.2 m north of the tread, posts with colliders, rope at 0.95. Chant spot 1 stays at (107, 58) (Hollis). | Quill 29 |
| V2 | **Bulbs on the dead branch** as built (82, 52), 1 m off the tread at +0.6, seen ahead on the way down from (91.3, 47.4). Not on day 1 | Quill 15, 25 |
| V3 | **Mouth and passage** (x 50.5 to 53.5, z 20.5 to 37.4, floor -6, ceiling -2): **drip** from the ceiling onto a can against the west wall (51.2, 28), z 23 to 33 kept clear; **jugs** against the west wall, x 50.6 to 51.2, z 33.5 to 35.0; **load-down spot**, the floor x 50.6 to 52.4, z 35.2 to 37.0. **Generator niche** in the east wall, x 53.5 to 55.5, z 34 to 36, floor -6, ceiling -4, its opening facing west across the passage, the generator cold. Marlow samples the terrain over it (S5); under -3.0 it gets its own rock box like RockAbove. **Cable,** pale taped, no collider: from the niche along the passage's east wall, the inner wall of legs 1 to 3, along the chamber's north wall to the bank. It is the leg 1 marker on the left (Pim 4.2). It is not a light, so nothing glows at the mouth | Quill 6, 10, 11 |
| V4 | **Day-2 boards** stacked against the west jamb's face, x 47.2 to 49.8, z 37.6 to 38.0, 1.2 high, flush (gap 0), hulled. **Toilet:** a pit lid and a shovel against the west side of Boulder_4 (48.9, 41.2), flush, out of sight of the tread (S6 checks the line) | Quill 9, 24 |
| V5 | **Chamber** (x 71 to 89, z 3 to 21, floor -18): **keep clear** z 10.8 to 13.2 from x 71 to 89.25. **His seat:** a rock shelf against the east wall, x 87.8 to 89.0, z 5.0 to 7.0, top 0.6 (leads nowhere: ceiling -10), facing 283 to the passage; it replaces ChamberDressing/Boulder_1 (R). Talk from (86.4, 6.6) facing 80. **Sleep:** mattress on a crate base against the south wall, x 84 to 86, z 3.5 to 4.5, 1.8 m from the shelf; electric heater at (83.4, 4.0), under 0.5 m. **Food:** a cooler and a crate stack on the floor's north-west edge, x 72.5 to 73.5, z 14.0 to 15.5, the first spot a carrier reaches. **Speaker stack** inside x 85 to 88.5, z 17 to 20.5, back to the rock, facing (80, 12); **battery bank** 1.0 x 0.6 flush to its west side at the north wall, (84.5, 20.5) | Quill 5, 7, 12, 13; Hollis 1 |
| V6 | **Side room** as built (x 89.5 to 97.5, z 7.5 to 15.5, 3 m high, doorway z 11.4 to 12.6). **His chair** turns to yaw 180 (built at 205, so FitCollider inflated its box) and the crate goes to yaw 0 at (90.4, 14.6), out of V9's way. Keep clear z 11 to 13 from x 89.25 to 92.4. **Bulb string** lowered to 1.9 m at the doorway (Pim 4.7). **Standing point** after getting up (92.0, 12.0) facing 270 | Quill 16 to 19; Pim 2 |
| V7 | **Side-room rocks** (BigBoulders_1, Boulder_3, BigBoulders_3, BigBoulders_0, BigBoulders_5) are stripped of colliders in 8.17. Each gets a hull **unless** it sits in a keep-clear strip. Marlow reads where each is seated (S8). **R:** Boulder_0 (seated near (97, 13.8)), in V9's opening | |
| V8 | **Chamber rocks** BigBoulders_2, BigBoulders_4 and Boulder_5 stay as ledges, hulled (S8) | |
| V9 | **Event 16's deeper passage:** Wall_E_Future opens at z 13.2 to 14.4, 2.1 high, into a passage x 97.5 to 101.5, then a turn north and a dead end at (101.0, 15.5). Inspect point at its end, facing north, from (101.0, 14.0). A rock box closes the opening on every other day. Terrain over it stays 0 (ceiling -15.9) | event 16 |
| V10 | **Narrow on the descent** (Hollis 13): two rows of owned BigBoulders, 3 to 5 m apart and 4 m tall, along the tread from (72, 46) to (61, 48), continuous (no 0.6 to 1.0 gaps), hulled, placed on Marlow's samples (S3). Only where the ground is open; if the ravine walls already pinch there, nothing is added | sound |
## 4. Samples, colliders, times
**Samples Marlow takes before Rook builds:** S1 the rim north of the mouth (74, 60) and the ridge over the descent; S2 the ravine floor from (62, 48) to (52, 38), every 1 m, 6 m each side; S3 the narrow strip for V10; S4 the mouth face, x 44 to 60 at z 37.4, to its top (Hollis 12 wants 10 m wide and full height); S5 terrain over the niche, x 53.5 to 55.8, z 33.8 to 36.2; S6 the toilet spot and its line from the tread; S7 the new rope-rail post line; S8 every seated cave rock.
**Colliders as built (from the recipes; Marlow confirms):**
- Cave pieces: every Floor, Ceiling and Wall box, 0.5 m skin.
- Mouth: Mouth/RockAbove; the DayOneBoard planks, solid on day 1; the jambs BigBoulders_4 and 5 as convex hulls; Boulder_4 (48.9, 41.2). The overhang BigBoulders_2 is stripped (2.25 m over the floor, out of reach). The scree and ferns have no colliders.
- Side room boxes: Chamber_Wall_E_S, E_N, E_Lintel, Floor, Ceiling, Wall_N, Wall_S, Wall_E_Future.
- Side room props: the table, both chairs and the crate have fitted boxes (two of them turned, V6); the pillar is a Fill.
- No colliders: the side-room and chamber rocks (V7, V8), the chamber lantern, the rubble.
- Spur: the 8.3 rope posts (0.1 m cubes) and the dead branch.
- Forest trunks in the ravine grove (65, 20): unread; Marlow names any within 1.5 m of the tread or of V10.
| Walk | m | s | | Walk | m | s |
|---|---|---|---|---|---|---|
| W1 to the mouth | 109 | 44 | | passage end (71, 12) to the doorway | 18.3 | 7 |
| mouth to the chamber | 73 | 29 | | doorway to the guest chair | 3.6 | 1.5 |
| camp to the table (by the pump and W1) | 402 | 161 | | chamber to the seat (V5) | 16 | 6.4 |
## 5. Trap spots, no regression
| # | Where | Found | Held by | Recheck |
|---|---|---|---|---|
| T1 | pocket by the east flank (54.4, 37) | 8.17 hand walk | mouth rocks as convex hulls, lintel boulders gone | hand walk BOULDER POCKETS |
| T2 | Boulder_1 hung 2.8 m over the chamber floor | 8.17 gate, Marlow 10 | Seat() drops rocks to the floor | V5 replaces it with the shelf |
| T3 | side-room rocks 0.6 to 1 m over the floor; 4.6 to 5.6 m rocks through the ceiling | 8.18a, Marlow | VertY seating, height capped | V7 hulls, then the area flood |
| T4 | RedwoodHollowLog_0 (91.5, 47.3) stall on the spur | Gate_batch Marlow | Marlow's 0.5 m radius passes | V1's rope posts leave the tread clear |
| T5 | Cave_Mouth warp seated under the terrain | Breaks 17 | main3_warp_seat.cs | warp landing check |
## 6. Area check data
1. **Bounds x 40 to 132, z 0 to 72** (W1 is Camp 3's too). **Warps:** Cave_Mouth (58, 44) facing 211; Cave_Chamber (74, 12) facing 90 (dev-only, exempt from the sealed-landing test); **Cave_SideRoom** new (91.0, 12.0) facing 90, row `Cave side room (table)`; **Spur_Descent** new at the rise (77.3, 48.6) facing 250, for the rim checks.
2. **Places:** mouth (52, 37.9), boards, bulbs, rope rail, toilet, drip, jugs, niche, chamber, seat shelf, sleep spot, food, stack, bank, side room, guest chair, his chair, deeper opening. **Interactions:** R7 from (86.4, 6.6) facing 80; guest chair from (91.0, 12.0) facing 90; event 16 from (101.0, 14.0) facing 0.
3. **Found frames:**
   - F1: the tread at (55.96, 46.22) heading 204: the opening, from 30, 10 and 5 m, day 1 and day 2.
   - F2: (91.3, 47.4) heading 300: the bulbs.
   - F3: the passage end (52, 24) heading 180: leg 1's opening on the left, the cable.
   - F4: (71, 12) heading 90: stack left, doorway centre, R7 right.
   - F5: the doorway heading 90: the guest chair.
   - F6: the standing point heading 270, and the doorway heading 270: the ways back.
4. **Deck must-hide, hard (C-1 land rays, plus pixel checks where trees are the cover):** the mouth, boards, bulbs, rope rail, toilet, the ravine floor, the spur below the rise, any colour from the mouth day or night. **Must-see: none.** **Loose:** ravine grove crowns read as forest.
## 7. Borrowed, and what none of them do
Fears to Fathom: a real-scale cave somebody lives in. Inscryption: a host at a table you cross a room to reach. Mouthwashing: the party set for someone. Shift at Midnight: the generator that should hum and does not. None put the loudest room on the map under the quietest walk, with its power source cold and its table set for two.
## 8. Open questions
1. Grant: the rope rail moved to the real descent; event 16's dead-end passage; the generator always cold.
2. Quill: R7 always at the seat or the table; the toilet behind Boulder_4; the day-2 boards outside.
3. Pim: the mouth strip instead of a reroute; F1 to F6; the inspect word for V9.
4. Wren and Grant: Pim's grey rule for the interior.
Sable
