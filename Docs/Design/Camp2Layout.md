# Camp 2 layout: his knob, the payphone, the card table, the trails to T and the boathouse (PLAN 8.25a)
**DRAFT 3, 2026-10-03, Sable.** Grant walked it: the octagonal column with a tent on top is too weird. Draft 3 replaces the column, its stair and everything on it with a **granite knob he found, not built** (Quill's 8.25a pick A, Camp2Layout_Story.md). Every height and line is drawn from Marlow's survey (Docs/Review/2026-10-02-Areas/825a_Marlow_ground.md):
- the camp is a flat terrace at 4.0, with no natural rise;
- from the north part of the camp a standing eye at (292, 122) sees the highway, the gate T and the barrier, and at +6 all three plainly.

The booth, the table and its three places, the barrel, the phone wire and the trails stand as built (draft 2 C2, C5, C8, C9; Wren's 8.25 calls). Drawing: Camp2Layout.svg, for Grant before it is built. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Capsule 0.35, step 0.1, jump 0.6, climbable top 0.70, reach 2, slope 45. Props sit at yaw 0, 90, 180 or 270, or carry a box from mesh local bounds.
## 1. A Camp 2 day: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Boathouse to Camp 2, 94 m | into the south-east grove | the phone pole at 41, its wire over your head to a lit booth; grey rock showing through the trunks ahead | a line somebody is waiting on |
| The foot | the trail passes the card table | three places by the booth: his hand fanned, one hand face down by a clean upturned mug | you are a stand-in, and not even for that seat |
| A hand (hand days) | sit across from him; play | the booth's open door at his shoulder | the phone is the third player |
| Up the knob, 28 m | across the camp floor, then a worn scramble up the back of a granite knob, no ladder | rock steps polished by one man's boots; a sheet of paper wedged in a crack | he carried everything up here himself |
| The top | stand by his chair at the east edge | the highway, the gate T and the barrier through the trees; his tent; the cold lamp; a 5 m drop to the trail | as high as he could get, and still only just |
| Camp 2 to T, 89 m | north-east past the knob's cliff | the food lockers, the red mast lamp, road noise | the ordinary world close |
## 2. The asks, answered
1. **A lookout a real person would camp on: yes, a bare granite knob** in the north part of the camp, where the ground already sees the road.
   - Its top is **5.0 m over the ground (9.0)**: the lowest that does the job. Marlow's eye at (292, 122) sees all three targets standing (gaps between trunks) and plainly at +6. His seated eye on the top is 6.2 m over the ground, so it sees them; at +11 the gate T and barrier drop out behind branches.
   - **Grant's "over the trees" is read as "through them":** no knob under 21 m clears the trees from anywhere in the camp (Marlow 6). A 5 m rock with the road in view between trunks is what a real lookout here is.
2. **The deck sees the lamp:** the lamp stands at (296.6, 122.6). The cell (296, 124) at +5 is seen from the E rail (98 px), and the line passes north of where the column stood. A hard target: Rook's machine check decides, with a fallback of the pole to 2.5 m or the lamp up to 1.5 m along the top.
3. **The scramble is walked, never climbed:** a collider ramp of 23 to 31 degrees under rock steps (the STAIRS RULE, as the log steps).
4. **His hours, the payphone, the cord, the barrel and `Take water` stand as draft 2.** "On top" now means the knob top.
5. **The ring box:** in the tent on the top, reached from the tent door (Wren). Docs/Private/Minigames.md 3.2.4 is corrected to match: the ring box is a clue in his tent, not at the table.
## 3. Places
| # | Place, where | Job |
|---|---|---|
| K1 | **The knob:** footprint x 288 to 299, z 116 to 127, ground 4.0 (Marlow 1). A solid core collider (one convex rock mesh, top 9.0) skinned with owned BK BigBoulders_0 to 5 at scale 1.0 to 1.3, each hulled and overlapping the core (gap 0, no slots). **East and north-east faces:** a near-vertical cliff on the road side, rock to within 0.5 m of the top edge. **South face:** steep rock. **West face:** the scramble (K2). **Flat top** x 291.0 to 297.5, z 119.5 to 125.5, at 9.0, a flat rock slab collider. No rail and nothing built (Quill 4); a fall off the top is 5 m onto open ground that walks back | lookout |
| K2 | **Scramble,** a collider ramp 1.6 m wide with owned Boulder_0 to 5 steps (scale 0.3 to 0.4) set along it as the visible treads, flush to the core (gap 0). **Leg 1:** foot (288.0, 114.5) at 4.0, north along x 287.2 to 288.8 to (288.0, 121.5) at 7.0 (3 m over 7 m, 23 degrees). **Leg 2:** from there north-east to the top's west edge (291.0, 123.0) at 9.0 (2 m over 3.4 m, 31 degrees). 10.5 m. Its open west side drops 3 m or less onto open ground; no rail | the way up |
| K3 | **The top:** **tent** CS_Tent_Modern_2, mesh box 2.03 x 2.25, x 291.3 to 293.55, z 121.5 to 123.53, door east. The **ring box** sits just inside the door, reached from (294.2, 122.5) facing 270, as built in the 8.25 follow-up. **His chair** CS_Chair_2, a box (0.74 x 0.78, back 1.20), at (296.3, 124.6) facing 65 (the T). **Stand** (295.4, 123.6) facing 50. **Lamp** on a 1.2 m pole at (296.6, 122.6), cold core, 1.6 m from the chair. Letters under stones by the tent; two mugs; sleeping bag (D) | `Talk`, `Examine`; deck |
| K4 | **Rockfall** (Quill 7: the boulder field reads as rock off the cliff): BoulderField Boulder (287.8, 122.3), scale 0.37, moves to (300.0, 126.5); Boulder (285.9, 115.6), scale 0.44, moves to (300.5, 123.5). Both go to the cliff foot, touching the face. They clear the scramble and stand 4 m or more from the T tread | dressing, stop |
| K5 | **Paper spots:** PS1 under a stone at the top's south-west edge (291.5, 119.9); PS2 wedged in the scramble's rocks at (288.0, 119.0), 0.9 m off the ramp's centre; PS3 on the moved rockfall boulder (300.0, 126.5) | Quill 10 |
| K6 | **Trail ends:** both legs still end at (298.9, 107.8), now the foot of the camp. From there to the scramble foot is 12.5 m across open floor. Camp_2_Top warp moves to (294.0, 9.2, 120.5) facing 60 | |
**Removals, by name (Rook strips the column cleanly):**
- **Campsites/Camp_2/GraniteStack**, its MeshCollider and the Places_StackRock UVs.
- **Campsites/Camp_2/StackPath**, all of it: Ramp1 to Ramp4 and their _Rail pieces, LandingN1, LandingS1, LandingN2, LandingTop and every rail on them (RailN, RailS, RailE, RailW, LandingTop_RailOverLaneA), and the four Posts.
- **Campsites/Camp_2/StackTop:** its pieces move to K3 (CS_Tent_Modern_2, CS_Lantern_Modern and LampCore, CS_Chair_2, thermos, mugs, papers and stones, the ring box). Resident_Camp2_Spot moves to the chair; the group is removed.
- **Campsites/Camp_2/Layout825, every piece except the keep list below:**
  - the octagon top rail and its posts and rope;
  - the landing rail (draft 2 C3);
  - the under-stair skirts and way-in planks (C6);
  - the 8.25 round 2 open post-and-rail screens;
  - the wedge-notch fill block.
- **The keep list:** the booth handset and cord, the CardTable cluster and its three chairs, the barrel, the PhoneWire and its standoff, the moved Boulder_1 (303.4, 97.9), StartBlaze.
- **Campsites/Camp_2/Dressing:** the 8.17 talus BigBoulders round the stack foot (r 6.6), with PS3's old boulder.
- **Ground815/Pockets:** the 8.18a pocket fill at the old talus pocket (289.6, 113.3).
## 4. Times (2.5 m/s, Marlow's markers)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| boathouse to Camp 2; Camp 2 to T | 93.7; 88.7 | 37; 35 | | trail end to his chair (floor 12.5, scramble 10.5, top 5.5) | 28.5 | 12 |
| your seat to the booth door; table to the trail end | 3; 10 | 1.2; 4 | | door to the barrel, by the pump; by Jg and T | 299; 324 | 120; 130 |
1. Gaps: the core and its skin 0; the scramble to the core 0; the chair 0.8 to 1.2 m off the open east edge (open air, not a slot); the rockfall boulders touching the cliff (0).
2. Nothing climbable leads anywhere new: skin boulders over 0.70 where they stand free; the chair and lamp pole are boxes; the barrel stays at 1.19.
## 5. Trap spots, no regression
| # | Where | Found | Held by | Recheck |
|---|---|---|---|---|
| T1, T2 | the old talus pocket (290.2, 113.5) and (289.6, 113.3) | Gate_817, Gate_816b_817 | gone with the talus and the column | the area flood over x 284 to 302, z 110 to 130 |
| T3 | a top edge with a drop | Breaks 14 (20 m then) | now 5 m onto open ground, no rail by design | sprint-jump off every edge: lands, walks back |
| T4 | booth hull | Gate_817 8 | open-front boxes | unchanged |
| T9 (new) | pockets between skin boulders, the core and the ground | this draft | gap 0 by build | Marlow's 1 m grid of direct approaches round the whole knob foot, 2 to 9 m |
## 6. Area check data
1. **Bounds x 262 to 345, z 56 to 172.** **Warps:** Camp_2 (294.3, 95.0) facing 55; Camp_2_Top (294.0, 9.2, 120.5) facing 60.
2. **Places:** the knob, the scramble foot (288.0, 114.5), the top, his chair, the tent, the lamp, the ring box, the payphone, the hook, the CardTable and its three seats, the barrel, POI_Phone_pole, POI_Food_lockers, StartBlaze, PS1 to PS3. **Interactions:**
   - R2 on top from (295.4, 123.6) facing 50;
   - the ring box from (294.2, 122.5) facing 270;
   - the hook, the table and the barrel as draft 2.
3. **Found frames:** the knob from both legs (grey rock through the trunks); the scramble foot from the trail end heading 300; the top from the scramble head heading 80; the rest as draft 2.
4. **Deck must-see, hard:** the lamp core (296.6, about 10.3, 122.6). **Loose:** his chair, the tent, PS1 to PS3, the hood light at night. **Must-hide: none.**
5. **Sightline S1, hard:** seated at his chair (296.3, 10.2, 124.6), with a 2 m cube seen (over 24 px) at each of three targets: the highway (430, 4, 185), the gate T stop sign (423.05, 5.3, 166) and the barrier arm (389.3, 4.5, 169.5). Marlow's (292, 122) +6 figures are the evidence. 8.19 refuses trees on these three lines.
6. **Valley 7 F-1:** the eye drops from 25.6 to 10.2, so the fire line can only be more hidden. Rook reruns it.
7. **Sound (Hollis):** the stack wind E13 moves to the knob top (294, 10, 122).
## 7. Borrowed, and what none of them do
Firewatch: a lookout facing a road, on rock someone found. Inscryption: cards across a table from a man who watches something past you. Mouthwashing: a seat kept for someone. Papers, Please: the deck checks his lamp. None put the man's watch post on a rock only just high enough to see the road through the trees, so even his vigil is a near miss.
## 8. Open questions
1. Grant: the knob at 5 m, the road seen between trunks rather than over the treetops; no rail on top.
2. Quill: K5's new paper spots; the rockfall moves.
3. Rook: the lamp fallback; the S1 cube test from the chair before dressing.
Sable
