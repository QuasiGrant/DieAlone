# North layout: Camp 1, the north loop, forage C, the ruin (PLAN 8.24)
**DRAFT 1, 2026-10-02, Sable.** Folds in Quill (NorthLayout_Story.md), Pim (NorthLayout_UI.md) and Wren's calls of 2026-10-02 (SS1 found from the loop; SS3 found from the fallen giant's end; the ruin keeps a report box like the keeper's; forage C gets a real position; the ruin framed clear of N1's trunks from its approach). Drawing: NorthLayout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. **Positions are read from Main3.unity at a4dbf57:** trail points, Camp_1 children, warps, and prefab instances by their position overrides (trees, bushes, plants, edge stones). Nested or unpacked objects and pack-internal colliders were not read; ground heights come from the plant and trail points beside each spot, not a heightmap sample. Marlow samples before Rook builds (4.3). Walk 2.5 m/s, capsule 0.35, step 0.1, jump 0.6, reach 2.
## 1. A north round: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Jg to Camp 1, 83 m | north out of the burn | latrine at 42; the clearing; the kid's table, a spar strung with bulbs | a big camp for one small person |
| Camp 1 | talk, or share the meal | his table set for a customer who is a bear; the grown-up chair empty at the cold fire | someone should be here |
| Camp 1 to forage C, 66 m | west at the blaze, the fir wall on the right | from 25 m, whether the patch is bearing | the walk pays, or it did not |
| Forage C to the ruin, 68 m | into N1 | at 21 m, straight ahead between two giants, a pale roof slab and a leaning stovepipe | a cabin nobody mentions |
| The ruin, 12 m in | the stones to the doorway | a bunk under the roof, a table under the window, a report box like yours, lid hanging | another keeper's job ended here |
| Ruin to J, 96 m | out of the grove, along a fallen giant | a stump in the open; the giant's root plate; the cairn at J | the Ward's hill is close |
## 2. The asks, answered
1. **Camp 1 works as a family camp: yes, with five changes** (N1, N4, N5, N7). A real family pitch is a tent pad, a fire ring, a table and food storage, with water carried in. Camp 1 has all of them except water and a customer: a jug and bucket go at the fire, a chair faces the kid. It gives Social only; it never feeds or waters the keeper.
2. **The loop works as a walk: yes.** Built 231 m, Camp 1 to J. Each middle stretch now has a next place: forage C, the ruin, the stump, the giant, the cairn. The way home is plain: at J the CAMP arm and the tower over the creek; from Camp 1, back the way you came to Jg.
3. **Forage C works as a chore: yes, at (229.5, 273.5).** Valley's (225, 266) is a trail point, so Rook found no patch there. C is the far patch: 274 m from the door by Jg, 261 m by J. It pays only on a Camp 1 day, and you see from 25 m whether it bears, so a bare day costs the walk. That is the hurt.
4. **The ruin serves its events: yes, with fixes** (N14, N15). Doorway faces the knob; side path; cache trunk; the report box is built. It needs a bunk frame, a table under the window, two bushes gone and a clear approach lane.
5. **The deck** sees the spar (hard) and the kid's table, tent and tripod (loose). It must not see the ruin or SS1 to SS3: pixel check with control. The ruin passed it in 8.21: 0 of 128 eyes.
## 3. Places
| # | Place, where | Job |
|---|---|---|
| N1 | **Kid's table** as built (285.2, 236.5), yaw 25. Pots in a back row, 8 ingredients in a front row (top about 1.2 x 0.8, unverified). The bear's stool becomes the **customer chair**: CS_Chair_3 at table local (-1.0, 0), facing his stool, with the bear in it. The lid stool moves to local (0.3, -1.7): 1.1 m to the table, 1.3 m to the chair | M9, R1's sessions |
| N2 | Spar as built (284, 240): nothing but its three braces within 2 m. Draft: a jacket on it at 1.6 m, no collider | deck landmark |
| N3 | R1's spot (285, 244) facing 200 | `Talk` |
| N4 | **Tent** as built (271, 234.5), yaw 200, door facing 20. Shoes, and draft work boots, at the step. **Taped-box row:** 4 boxes of 0.5 m, 0.2 m apart, 1.0 m off the tent's WNW side, running along it. **Chase lane:** door to the loop at (272, 244.7), 8 m and 3 m wide, holds nothing with a collider; the blaze stump is 2.6 m past the join | Quill 3, 11 |
| N5 | Cookfire as built (276, 232). Bucket at fire local (0.9, -1.0), 20 L jug on a crate at (1.7, -1.4). Neither has a collider or a prompt, and neither ever counts as the keeper's Water | `Share the meal` at the pot |
| N6 | Workbenches (266.5, 243), (275, 251), (293, 232), lumber (300, 240), latrine (263.7, 207.5), blaze stump (271.7, 247.3), all as built | none |
| N7 | **Drag lane:** 4 m wide from (292, 247) to (304, 252), no colliders. RedFir5 (303.1, 256.7) stands 4.7 m north of its line and marks the edge; the lumber is 9.5 m off | Quill 6 |
| N8 | **Forage C:** 5 low shrubs, 0.6 to 1.0 high, on r 1.4 round (229.5, 273.5), **no colliders**. North of the loop: tread edge 1.6 m away, fir wall 12 m behind. Ground 4.3 to 4.5, level with the trail. ThinFern3 (228.6, 273.0) and Grass1 (228.2, 274.7) go. The nearest object kept is an edge firewood piece (227.3, 270.7), 3.6 m off. Stand (229.2, 270.6) facing 0; target the south shrub (229.3, 272.1), 1.7 m from the eye. 28 m to SS1 | `Forage` (bearing days) |
| N9 | **SS1:** at the fir wall foot, under RedFir5 (255.1, 283.4), spot (255.0, 282.4). RedFir1 (253.9, 280.3) 2.4 m and RedFir8 (256.8, 286.4) 4.4 m from it. 24 m off the loop | C2 |
| N10 | **SS2 moves off the trail** (Valley's (150, 270) lies on it): an owned stump with a collider, top 0.5, at (152, 280). 10.2 m off the loop; nearest object Bush3 (149.6, 282.3), 3.3 m | C2 |
| N11 | **Fallen giant:** a Sequoia laid down as in 8.20. Axis (145.8, 271.1) to (131.5, 257.3), 19.9 m. Capsule r 1.5, sunk 0.3, so its top is 2.7 m up; tilted to the ground, which rises about 2 m to the SW. Root plate at the NE end. Trail centre 5.0 to 6.4 m off the axis, so the tread edge stays 2.0 to 3.4 m clear. Edge stones are 2.4 m or more off it | Valley 1.3; hides SS3 |
| N12 | **SS3 moves behind the giant** (Valley's (135, 255) lies on the trail): (132.3, 262.3), 3.0 m NW of the axis, 4 m in from the SW end, 1.5 m off the collider. Hidden from the tread | C2 |
| N13 | **Ruin** as built (172, 281), yaw 225. Bush3 (173.6, 282.3) on the back wall line goes. Bush3 (171.2, 273.7), 2.1 m off the stones between the loop and the doorway, goes. Inside, in ruin local metres: a **bunk frame** at x -1.6 to 0.3, z -1.95 to -1.15, under the slab, no collider. A **table** 0.6 x 0.8 under the left half wall (the window) at x -2.9 to -2.3, z 0.4 to 1.2, with a collider (top 0.75, not climbable). Stove, pipe, cache trunk and chair stay as built; an empty hook and boot nail by the door, no collider. **Report box** on its stump as built (169.1, 280.1), 0.65 m off the opening edge | events (private) |
| N14 | **Approach lane:** eye (192.7, G+1.6, 276.6) to the roof slab (172.3, 5, 280.6), 21 m, 12 degrees off travel. No trunk or bush over 1 m within 1 m of the line. The giants stay: Sequoia2 (177.6, 284.3) 4.1 m off and RedPine5 (179.2, 276.4) 3.1 m off frame it | Pim 5, Wren |
| N15 | **Sky gap:** no fir crown under 25 m within r 8 of the ruin; giants stay. **No sun shaft:** the day-one sun (200, 32) runs 16 degrees off the deck line at the same 14 to 18 m heights, so a shaft would cut the R-1 screen. The ruin's light is Milestone 11 | Valley "gap of light" |
## 4. Times and walkability (2.5 m/s; measured along the built trail points)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| door to Jg; Jg to Camp 1 | 125; 83 | 50; 33 | | ruin side path, in and out | 24 | 10 |
| Camp 1 to forage C | 66 | 26 | | side-path mouth to the giant's SW end | 40 | 16 |
| forage C to side-path mouth | 68 | 27 | | SW end to J; J to door | 57; 96 | 23; 38 |
A full north round is 559 m and 224 s, plus talk, forage and search. Chase: from the tent door to the loop is 8 m, then forage C at 66 m (Quill picks the safe point).
1. Every gap is under 0.6 m or 1.0 m and over: customer chair 1.3, lid stool 1.1, box row 0.2, bunk frame to trunk 0.3, giant to tread 2.0 or more. Forage shrubs, stones, plants, the slab, the jug and bucket have no colliders.
2. Nothing new can be climbed onto: the giant's top 2.7, the table 0.75. The stump (0.5) stands 3.3 m clear of everything. You pass the giant round either end.
3. Before Rook builds, Marlow samples ground and colliders at N4's lane, N7, N8 to N12, and N14's line. Trunk colliders off the trail are unverified (8.19 gives capsules only near trails).
## 5. Area check data
1. **Bounds x 116 to 310, z 200 to 305.** The loop's last 12 m and J belong to 8.29. **Warps:** Camp_1 (268, 226) facing 49; North_Loop_Ruin (165.8, 267.7) facing 25. No new warps.
2. **Places, on their transforms:** Camp 1 (282, 238), spar, kid's table, R1's spot, tent, cookfire, latrine shed, blaze stump, forage C, SS1 to SS3, the giant, NorthRuin, ruin doorway (170.6, 279.6), report box. **Interactions:** R1 from (284.3, 242.1); forage C from (229.2, 270.6); report box from (168.0, 279.0); cookfire pot from (274.5, 231.6).
3. **Found frames (Pim's rule):** F1 Jg trail heading 31 into the clearing: table, cookfire, spar bulbs. F2 table heading NW: blaze. F3 (251.3, 260.4) heading 287: forage C, 25 m, 14 degrees off. F4 = N14. F5 side-path mouth (168.1, 267.1) heading 9: doorway and report box. F6 (260.6, 256.2) heading 304: SS1, 26.8 m, 44 degrees off (thin margin: if it fails, SS1 moves to (258, 282.5)). F7 (161.2, 269.4) heading 289: SS2, 14 m, 30 degrees off. F8 (140.3, 257.0) heading 236: the giant's SW end, 8.8 m, 36 degrees off. F9 from the end, (130.5, 259.5): SS3.
4. **Deck must-see:** hard: spar top (284, 29, 240). Loose: kid's table (285.2, 5.8, 236.5), tent (271, 6.5, 234.5), tripod (276, 6.3, 232). **Must-hide, pixel check with the 20 m control:** north ruin (172, 4, 281, Places/NorthRuin); SS1 (255.0, G+0.3, 282.4); SS2 (152, G+0.5, 280); SS3 (132.3, G+0.3, 262.3). The loop may read as a trail.
5. **Sightline:** no tower from the ruin's warp, doorway or inside (Marlow 8.16b: 0 px). Rerun after N13 to N15. The "North ruin" inventory count changes with N13.
## 6. Borrowed, and what none of them do
Firewatch: an empty camp and another keeper's ruin in the woods. Boba Teashop and Kiosk: a counter with one customer, here a child's table and a bear. Dredge: the rotating forage spot. Fears to Fathom: a real-scale family pitch. Papers, Please: the deck checks his spar. None of them put your far food patch between a child's camp and a dead keeper's cabin, so feeding yourself means passing both.
## 7. Open questions
1. Grant: Quill 5's drafts (adult chair, unopened pack, boots, jacket on the spar) and the customer chair with the bear in it.
2. Grant: SS1 to SS3 hidden from the deck (Quill 10) by pixel check. SS2 and SS3 move because Valley E12's points lie on the built loop; Wren updates Valley E12 and E13.
3. Grant: a sky gap over the ruin, not a sun shaft; the ruin's light waits for Milestone 11.
4. Quill: the chase's safe point (forage C at 66 m, or Jg by the clearing's south side). Camp 1's water is a jug and bucket. The spring (Valley 1.3) is 8.29's and has no prompt, so the pump keeps Water.
Sable
