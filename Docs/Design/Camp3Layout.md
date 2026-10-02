# Camp 3 layout: the hollow, the studio, the Snag line, the creek, W1 and the west trails (PLAN 8.26)
**DRAFT 1, 2026-10-02, Sable.** Folds in Quill (Camp3Layout_Story.md), Pim (Camp3Layout_UI.md, his scene read) and Wren's calls of 2026-10-02 (the easel canvas is the escape-room entry; the Snag line starts hung with his old sketches; the job form is a camp find, exempt from the trail-find rule). Drawing: Camp3Layout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Capsule 0.35, step 0.1, jump 0.6, climbable top 0.70, reach 2. Hollow floor -4 inside r 8 of (78, 146), wall to r 12.4, rim 4 (Main3.md 2); trail heights read from Main3.unity. **Every floor position here waits on Marlow's ground samples and his read of the FaceRock boulder ring (Breaks 88), which may cut the floor's edge; my read misses Forest/Dense trunks, so Marlow names any on K1 to K4.** Every prop sits at yaw 0, 90, 180 or 270, or gets a box from its mesh's local bounds (Camp2 lesson: FitCollider inflates turned props). Removals (R) go by name after 8.19; keep-outs (K) go into 8.19's spot test and 8.16's planting.
## 1. A west day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Camp to Camp 3, 126 m | west and down; forage B at 40 | the Snag growing ahead; sketches hung on a line off its side; water heard before seen | going down into something private |
| The rim and the steps, at 107 | log steps down the wall; planks over the creek at the foot | the hollow opening below: a fire, a tent, an easel facing away from you, pale canvas backs against the rock | you are let in, not invited |
| The floor | sit by his fire; talk; look at the one painting he shows | the canvas on the easel; the job form on a plank table, the cleanest thing here | a man who hides his work, even from himself |
| The rim spur, 30 m | along the line to the north rim | his sketches, faces to the tower, backs to you | you see what the valley sees, from behind |
| Camp 3 to W1, 114 m | down the creek cut, water on your right | stones in the water; the camper trailer at 71; the lake ahead; the W1 sign | following water to water |
| W1 to pump, 80 m | shore path, over the stepping stones | the washed-out truck at 41; the pump's signpost | back to the sure thing |
## 2. The asks, answered
1. **Camp 3 works as the Artist's camp: yes, once the floor is re-laid (C2) so the creek runs through it (C5) and the steps are on the wall (C4).** Today the easel and stool stand on the creek line, the dam point is 0.6 m from the paint box, the form lies 8.5 m from the stool, both log-step sets sit on the floor and there is no water.
2. **The hollow floor is seen from the deck: yes, and that stays.** The cab shows from the Camp 3 centre from 18 of 18 eyes (Main3_sightlines), so the deck sees down onto the floor, 88 m off at about 35 degrees down. What must not be seen is his work: **every painted face is turned away from the deck** (the deck bears 77 from the hollow; the easel's face looks 284, 153 degrees off; the old canvases face the rock). The one thing of his the deck reads is the Snag line, faces out.
3. **W1 gets a turnable sign (C9):** a fingerpost with two arms, Camp 3 and Pump, each arm on its own yaw pivot so an event can turn it; the cave spur stays unsigned. W1 keeps the sign-turning event.
4. **Quill's porch item:** Camp 3 has no porch. **His porch is a ground sheet at the tent door (C2);** whatever Quill wrote for a porch lies there.
5. **The job form:** on a plank table 1.9 m from the easel stool, 2.6 m from the fire, found inside the camp (Wren's exemption).
6. **The trails work:** Camp to Camp 3 walks down the steps at 107 (C4); W1 to Camp 3 runs level beside the creek and comes onto the floor through the south notch. The W1 leg's log steps go: that leg never climbs (-4.2 at their spot). The footbridge goes: it spans dry ground 6.7 m from the creek (Quill 14, Valley 6.2).
## 3. Places
| # | Place, where | Job |
|---|---|---|
| C1 | **Arrival:** both trails end at the floor's south-east, (79.06, 145.39) and (78.57, 144.67). Camp_3 warp (74, 142) facing 45 stays | |
| C2 | **Floor, all at -4 (Marlow samples):** **fire** moves to (76.0, 150.5); **seat log** west of it, yaw 0, (73.8, 149.3 to 151.7), 1.15 m off the pit edge; **R3** sits on its south end (73.8, 149.6), facing the fire. **Tent** as built (80.5, 153), yaw 215; **ground sheet** 1.5 x 1.5 at its door (79.3, 151.6), flat, no collider (the porch). **Easel** moves to (80.3, 148.8), face yaw 284 toward the fire; **stool** 1.6 m in front (78.75, 149.2); paint box at the easel foot (80.6, 147.6). **Plank table** 1.2 x 0.6, top 0.75 (not climbable), yaw 0, x 77.9 to 79.1, z 147.0 to 147.6; the **job form** under a mug on it. **Faced canvases** stay on bare rock, x 70, z 144.5 to 147.4, nothing in front. Gaps: table to easel 1.17, to stool 1.4; easel to tent 2.2; fire to table 2.6; tent to the creek 1.8 | `Talk`, `Sit by the fire`, `Study the painting`, `Examine` |
| C3 | **Snag line** (E9): rope from the Snag at 3.5 m over the rim to a stake on the north rim (80, 163) at 2.4 m; four pegs at (92.8, 149.8), (89.6, 153.1), (86.4, 156.4), (83.2, 159.7), pieces 0.5 to 1.0 under the rope, faces east to the tower, hung with his old sketches (Wren). **Hoist:** a pulley on the Snag's tower face 12 m up, rope down to a cleat 1.2 m up, 1 m off the trunk | deck; events |
| C4 | **Log steps** move to the wall: Camp to Camp 3 from the rim at chainage 107 (96.5, 4.0, 142.8) to the floor (82.7, -3.7, 143.8), 13.8 m, 7.7 m down (29 degrees): 19 logs, a collider ramp on the profile (STAIRS RULE). 8.3 must place them by chainage, not by the leg's end. **R:** the W1 leg's POI_Log_steps | the way in |
| C5 | **Creek water,** this area's whole run: plank bridge on Camp to J (104.8, 203.2), (96.5, 185), north notch (88.7, 163.2), (86.0, 155.6), **dam point** at the inlet (83.8, 151.6), (83.3, 147.0), **plank span** under the steps' foot (83.0, 143.6), 2.2 m, at tread height, (84.6, 138.5), (86.8, 132.0), (87.0, 126.0), (89.5, 121.0), (93.5, 118.5), (98.5, 113.5), (100, 110), (110, 98), (120, 86), the **stepping stones** on Pump to W1 (131.7, 72.8), mouth (136.5, 66). Water strip 1.2 m wide, 0.15 under the banks, bed cut 0.4, banks 30 degrees or less, stones every 3 to 5 m, no collider, no slowdown. Always 3 m or more from the W1 leg's centreline. Dam stand (82.6, 151.2) facing 70: 3.3 m from the easel, 4.0 from the table. **K1:** 1.5 m each side | the creek; `Clear the dam`; the flood |
| C6 | **Rim spur,** 30 m, a trail: from Camp to Camp 3 at (98.6, 142.7), round the Snag's east side past the Camp_3_Rim warp (100, 148) (1.5 m off the trunk's surface; Marlow reads it), then 1.5 m outside the line: (94.0, 151.4), (90.6, 154.8), a plank over the creek (87.6, 157.6), (84.6, 160.8), to the **rim spot**, flat 4 x 4 at (81.5, 164.5) (Quill 8). All r 15 or more from the centre: no edge stop needed, a fall slides to the floor. **K2:** tread plus 1.5 m | Quill 6, 8 |
| C7 | **Lamppost spot** (58, 150), kept free to the ridge foot (E15). **K3:** r 3 | homage |
| C8 | **W1 leg:** **R:** POI_Footbridge (114.8, 85.2). Camper trailer (89, 112) stays, 4.5 m or more from the creek | |
| C9 | **W1 sign:** fingerpost at (127.0, 72.8), 3 m from W1 (128, 70) in the wedge between the Camp 3 and Pump arms, 0.9 m or more off both treads. Post 0.15 box; two arms at 1.7 and 1.9 m, each on its own yaw pivot, no colliders. Blaze_W1_Camp3 (120.4, 72.9) stays. **K4:** r 2 | Quill 15 |
## 4. Times and walkability (2.5 m/s)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| camp to Camp 3; W1 to Camp 3 | 126; 114 | 50; 46 | | rim spur, out and back | 60 | 24 |
| pump to W1; west loop | 80; 430 | 32; 172 | | arrival to fire; to easel; to dam | 5.9; 3.6; 7.8 | 2.4; 1.4; 3.1 |
1. Gaps in section 3 are 0 or 1.0 and over; Marlow checks them against real boxes. 2. Nothing climbable leads anywhere: table 0.75, stool and paint box lead to nothing; a fall off the rim slides 8 m to the floor and walks out by either trail. 3. Before Rook builds, Marlow samples the floor at every C2 position, the notches, C4's line, C6's points and C9, and reads the FaceRock ring, the Snag mesh and the trunks on K1 to K4.
## 5. Trap spots, no regression
| # | Where | Found | Held by | Recheck |
|---|---|---|---|---|
| T1 | FaceRock boulders round the hollow: BigBoulders_2 (88.2, 2.6, 133.9), 8 spots; BigBoulders_3 (73.6, 3.1, 156.0) | Breaks 88 | 8.18a hulls | walk-into after the creek cut and the moves |
| T2 | faced canvas backs (70, 144 to 147), the easel canvas, campfire and pot hanger | Breaks 99 | 8.18a hulls | rerun solid after C2 moves them |
| T3 | hollow wall: 4 flagged slides, escape 16 m | Gate_817 Marlow | the slide | the creek bed and plank span leave no pocket |
| T4 | wade-limit jump from Pump to W1 P48 (144.4, -4.3, 81.7) | Gate_816a | 8.16b wade boxes | LAKE JUMP with the creek mouth cut |
| T5 | footbridge rails caught the player | 8.3 (Marlow 10) | rails colliderless | gone (C8) |
## 6. Area check data
1. **Bounds x 50 to 160, z 62 to 205** (the creek from the plank bridge; the cave spur is 8.27's). **Warps:** Camp_3 (74, 142) facing 45; Camp_3_Rim (100, 4.3, 148) facing 265; Junction_W1.
2. **Places:** Camp_3, fire, seat log, tent, easel, table, faced canvases, Snag, Snag line pegs, hoist, stake, rim spot, steps top (96.5, 142.8), dam point, plank span, W1 sign, Blaze_W1_Camp3, camper trailer, stepping stones, lamppost spot. **Interactions:** R3 from (75.2, 148.6) facing 305; fire from (74.6, 151.2) facing 90; easel from (79.0, 149.9) facing 130 (1.7 m); form from the stool or (78.5, 146.4) facing 0; dam from (82.6, 151.2) facing 70.
3. **Found frames:** F1 steps top heading 270: fire, tent, canvases. F2 W1 leg arrival (84, 132) heading 342: fire, tent, easel back. F3 rim spur (90.6, 154.8) heading 315: pieces' backs. F4 pump leg at the stepping stones heading 250: W1 sign. F5 W1 heading 300: blaze.
4. **Deck must-see, hard:** the four Snag line pieces. **Loose:** Snag lantern at night, hoist, rim spot, steps top, fire, tent. **Must-hide, pixel check with the 20 m control:** the easel's painted face and every faced canvas's face (painted magenta, 0 px).
5. **Sightline:** the cab from the Camp 3 centre stays 18 of 18 after C2.
## 7. Borrowed, and what none of them do
Firewatch: a camp in a hollow you look down on. Inscryption: a painting that is a door. Exit 8: W1's sign that turns. Papers, Please: the deck checks his line. None let the tower see a man's whole camp and never his work, then walk you down his steps to look at the one painting he lets you see.
## 8. Open questions
1. Grant: the floor seen from the deck, the painted faces never; the floor re-laid round a running creek.
2. Grant: the footbridge and the W1 leg's log steps removed; a turnable two-arm sign at W1.
3. Quill: the porch is the ground sheet at the tent door; R3's seat on the log's south end; the rim spot at the line's end.
4. Pim: `Clear the dam` and the sign's fix prompt wording; F1 to F5.
Sable
