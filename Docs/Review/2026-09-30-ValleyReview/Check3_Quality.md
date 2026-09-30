# Check 3: look quality of the Main3 valley (Vesper, 2026-09-30)

Judged against an absolute bar (Style.md, LookSlice.md 7, Edges.md 9), not against earlier captures. Evidence: sheet frame labels, shots/, Facts_Rook.md. **Verdict: FAIL. Not ready for Grant to walk.** All values are P; nothing is decided until it is in DECISIONS.md and Grant confirms it.

## 1. Lighting
**Day one.** The frame is flat, not too bright overall: everything sits in one mid-tan band. (a) Shadows are lifted to the tan of lit ground. Ambient #998A73 is 40 percent above Style 6.0.4, and crushBlacks 0.15 plus washOut 0.25 lift it again, so no frame has a dark anchor (Jg_to_Camp_1 all; Pairs camp day; W1_to_cave FWD 0 to 110). (b) Fog #A8A08E from 40 m is the ground's own tan, so ridges and trees past 50 m melt into the floor, with no distance layers (Boathouse_to_Camp_2 BACK 0 to 60). (c) The sun draws as a big white smear, with flat blue-grey sky and no gold #E3A968 band (Climb 140 to 420; Jg_to_T BACK 10). (d) A 32-degree sun gives short, steep early-afternoon shadows and no rim on crests, against the late afternoon in Style 6.0.1. (e) Much of the "odd" look is untextured ridge faces filling half the frame, which no light value fixes (section 3).
**Night.** Overall dark is right. Fails: pure black sky with no #0C1016 horizon, so crests vanish (all night pairs; Edges 1.6). Grass cards speckle white-grey (Pairs camp and S1; cause unverified, Rook to find). Office windows are flat orange slabs. The cairn renders as translucent ghosts (Pairs J). No cab lamp visible from camp. The Ward fire is an even row of cards on black, with no lit smoke or sky glow: fails Style 6.3 and 8.15 (Pairs Ward).

| Day one | Now | P | Why |
|---|---|---|---|
| Sun elevation | 32 | 24 | Raking late-afternoon light, long shadows. W ridge shade to about x 93: the Camp 3 hollow is in shade; the rim, J and W1 are lit (LookBoards 5). Shade lines are my estimate; Rook to confirm top-down. |
| Sun bearing | 200 | 205 | Slightly warmer side-light; S ridge shade stays near z 72, so the pump and most of the lake stay lit. |
| Sun intensity | 1.1, #FFC98A | 1.25, same colour | Key must stay a clear step over the lowered fill. |
| Ambient (fill) | flat #998A73 | flat #6E6658 (Style 6.0.4). If LookTuning can drive Trilight (unverified): sky #6A7080, equator #6E6658, ground #3A3228 | Darker, slightly cool shade against warm sun: the late-afternoon split. |
| crushBlacks / washOut | 0.15 / 0.25 | 0.28 / 0.18 | A real dark under trees; keep the tape wash, lose the milky film. |
| darkCorners | 0.3 | 0.4 | Frames the view; stays under 0.45 (Style 3). |
| Fog | #A8A08E, 40 to 600 | #9A9A94, 110 to 850 | Near ground clear; ridges step paler and cooler than the floor (Style 6.0.5). Style 2.0 haze updated to match if agreed. |
| Exposure / sun glow | none / white smear | none / smaller, dimmer, tinted #FFC98A (field name unverified) | No post Volume; balance with sun and ambient. The smear is the odd sky. |
Night: sky horizon #0C1016 on N, S and E; ambient stays near black; office window emission halved behind blinds; cairn material opaque.
**Test one variable per retake,** in this order: ambient, fog, crushBlacks, washOut, sun elevation, bearing, intensity, darkCorners, glow. Each retake shows the 6 pair spots plus Jg_to_Camp_1 FWD 40, Camp_to_Camp_3 BACK 30 and Boathouse_to_Camp_2 FWD 50, before and after side by side at 1920 x 988.

## 2. Forest
Now (Rook 8): 581 trees on 12 ha; 41 giants; west of x 186 only giants and 18 camp trees. The giants stand in ones and twos with no mid layer, so they read as ordinary pines (Jg_to_Camp_1 FWD 30 to 80). The 277 open-ground firs are 2 to 4 m and evenly sprinkled, which reads as a nursery (Map_TopDown east). Bark is stretched and pixelated up close (Jg_to_T FWD 30, BACK 60; S55; Warps W9 W). No ridge has a tree line. The old burn is the best area.
- My Style 5.8 rule (giants 30 m apart) made islands. Replace it with groves of 4 to 8 giants, 8 to 15 m apart, and 40 to 60 m between groves. Each grove gets 10 to 15 RedFir or RedPine at 8 to 20 m for scale, plus Bush1-4, ThinFern, HollowLog and DeadLeaves at the foot. Bark import raised to 1024 on giants (Style 4.1 hero allowance).
- Targets, owned assets only (Redwood Sequoia1-5, RedFir1-8, RedPine1-5; suffercord Pine1-6, Bush1-4). West trails: from about 15 giants and 0 small to 5 groves, 30 giants, 250 firs, brush on both trail edges. Camp knoll: 18 to 40 trees, max 35 m. Lake shore: a continuous belt, 6 giants and 80 firs. Open east: regroup the 277 into 10 clumps, add 2 groves (12 giants) and 120 firs at 8 to 20 m. North (z 200 to 300): from 1 grove to 3 groves (10 more giants) plus a fir wall at z 270 to 300 (150 trees). Old burn: keep, add 20 snags and 10 fallen trunks. Crests (off-map): N broken line about 300, E about 150, S sparse 40, W none.
- Totals: giants 41 to about 100; playable trees 581 to about 1500. Draw cost unverified; Rook to say whether terrain tree instances with billboards can carry the small trees.

## 3. Ground and paths
- One texture everywhere (Ground054); the trail tint differs by about 2 percent, so the trail is invisible (shots/trail_jg_camp1.png; every trail sheet). Ground cover is held 2.5 m off the trail, which leaves a 5 m bare strip that reads as the path; the real 2.4 m trail does not.
- Ridges, banks and the climb are one untextured tan (Camp_to_pump FWD 20 to 90; W1_to_cave FWD 0 to 60; Climb, every frame). This fails Style 8.3.
- Fix with owned textures. Floor: Redwood GrassPine, darker olive. Trail: SoilPine or Mud, about 25 percent lighter and warmer, 1.4 m band, 0.4 m blend. Slopes over 35 degrees: Redwood Rocks_a retinted #6E6660. Ground cover up to 0.4 m from the trail, and a stone, root or log every 3 to 5 m on one edge.
- The stops' "rocks" (RidgeStops) are gray untextured cubes (Camp_to_pump; Climb; Trail_Ends Camp to Camp 3 start). Swap them for BigBoulders_0-5, Boulder_0-5, RubbleDense and HollowLog. The about 970 unmarked walls get brush or deadfall (Edges 1.4).
- Water is flat blue ellipses with hard rims (W1_to_cave BACK 80, 90). Add reeds and shore rocks, and darken toward #5E6878.

## 4. Structures
- **Reads as a place:** the cabin (Pairs camp day; Trail_Ends Camp to Jg end). The tower reads, but its legs look like red-painted steel (Camp_to_Camp_3 BACK 60 to 100). Retint to wood #5C4632, with rust only on bolts and rails (LookSlice 2).
- **Office, gray box:** one blank 12 m wall with no window, porch, sign or aerial (Pairs office day); fails Style 5.2 and 5.3. Fix: windows on every face, a porch step, a sign, an aerial and siding.
- **Store, gray box, the office's twin** (Pairs lot; Warps W11 N); fails Style 4.4. Whatever the interior decision, the outside needs its own silhouette: a flat roof with a front canopy, a lit sign, an ice chest, a propane cage, and about 10 m of frontage, so it reads as a store at 20 m.
- **Gray boxes:** the Camp 1 spar and tent are gray primitives (Trail_Ends Jg to Camp 1 start). The Camp 2 stack is a brown pillar (Boathouse_to_Camp_2 FWD 30 to 90). Camp 3 has cubes and a capsule (Camp_to_Camp_3 FWD 90; Trail_Ends Camp to Camp 3 start). The boathouse and pump are boxes (Boathouse_to_Camp_2 BACK 80, 90). The cave mouth is a box corridor (Trail_Ends W1 to cave start). The Ward stones are two slabs (Climb 490, 500). Gray domes stand at S6, S7, S21, S22 and Camp_to_Camp_3 FWD 0; unidentified, Rook to name them. Each gets its LookBoards kitbash; the booth needs a sliding window and a barrier arm.

## 5. Composition (A to F)
- Keeper's camp **C+** (Pairs camp and S1 day: tower, cabin and fire read; the lower half is bare tan, Style 8.4). Old burn **C** (Jg_to_T: density and shadows work).
- Front zone **D** (Pairs office and lot: twin boxes on asphalt, no road past the gate, shots/office_to_road.png). Lake and pump **D** (Camp_to_pump FWD 20 to 90: a trench of cubes). Camp 2 and SE **D** (Boathouse_to_Camp_2). Skyline **D** (every sheet: smooth faceted crests, no tree line, N and E crests near level). North and Camp 1 **D-** (Jg_to_Camp_1 FWD 0 to 80: sparse firs, the N ridge one flat wall; Map_TopDown north third empty, no landmark).
- West trails and cave **F** (W1_to_cave FWD 0 to 110). Climb **F** (Climb 0 to 500: cubes both sides, four identical legs; 460 and 470 are a full-frame wall). Ward ledge **F** (Pairs Ward).
- Only the tower and the mast hold a skyline. North fix: a grove wall at the back, one floor landmark near (150, 250) (Sable's call) and a broken N crest tree line. Planting no longer serves the tower sightlines, except the few views Main3 names.

## 6. Ready for Grant to walk (Marlow, from a fresh sheet run; every box ticked)
- [ ] No untextured primitive (cube, capsule, dome, cylinder, slab) in any trail, warp, end or stop frame; no gray-box building.
- [ ] Trail told from ground at a glance in every trail frame; every stop frame shows a boulder, log, brush or fence.
- [ ] No frame over a third one flat untextured face; no frame fully a wall; no bare flat ground over the whole lower half.
- [ ] 4 of 5 trail frames have trees within 30 m on both sides; Map_TopDown: no open ground over 30 m except named clearings, lake and lot.
- [ ] Each day-one pair has a clearly dark area and the horizon sky brightest; ridges step paler with distance; no white sun smear.
- [ ] Night pairs: crests read against the sky, practicals visible, no speckle. Office and store differ at 20 m in the lot frame.
- [ ] North third shows a grove wall and a landmark in Jg_to_Camp_1 and Warps W6. Vesper has passed the sheets before Grant is asked.

## Five highest-impact look fixes, ranked
1. Replace every gray primitive and cube stop with owned boulders, rubble, logs and brush; no bare invisible walls.
2. Terrain layers: rock on steep slopes, a distinct lighter trail, ground cover to the trail edge.
3. Forest: groves with a mid layer, a north grove wall and crest tree lines, to the section 2 counts.
4. Lighting set from section 1, one variable per retake: ambient down, fog out and cooler, crush up, sun 24 from 205, glow down.
5. Office and store as two distinct places; Camp 1 spar, Camp 2 stack and Ward stones from pack meshes.

Vesper
