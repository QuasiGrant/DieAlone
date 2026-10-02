# Front layout: gate, booth, office, store, lot, campground (PLAN 8.22)
**DRAFT 2, 2026-10-02, Sable.** Fixes Marlow's paper check of draft 1 (Docs/Review/2026-10-02-Areas/822_Marlow_paper.md: 2 blocks, 9 hurts) against the built objects he names, with his IW3 call. Folds in Pim (FrontLayout_UI.md), Quill (FrontLayout_Story.md) and Wren's calls of 2026-10-02 (a shift starts on entering the booth; the office door is the built west door; R5-2's search moves to the ice chest). Drawing: FrontLayout.svg. Binding: DECISIONS 2026-09-29 (gate option B; cars until R5's storyline ends; the follow wall), 09-30 (store level; walls only at the front and the Ward path), 10-02. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md; ground 3.00 at every point Marlow sampled. Walk 2.5 m/s. **Where a built object exists, this doc places against it; every gap is under 0.6 m or 1.0 m and over, doors included.**
## 1. A visit: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Arrive at T | off the Jg trail at the trailhead board | office and red mast left, the store's lit sign ahead, one car nosed at the gate; through it the T, the highway, the dead verge tree | the world, 90 m away |
| Store (Food) | 38 m across the lot; walk into the push door | canopy, ice chest, the sign; inside, the same store the windows showed | a lit counter at the end of the world |
| Office (R5, Social; Safety on CHECK) | along the walk to the propped west door | counter, the ranger on his stool or asleep in back; a radio talking to nobody | a post still manned, a job no longer done |
| The car (R6, Social) | 34 m to his bay | a man living in his car, the road in his windscreen | the store 17 m away |
| Gate shift (gate active) | enter the booth | a car at the window, papers, the arm, the road | you hold the exit for other people |
| Campground (after a shift) | up the spur, round the chain | the cars you admitted, parked on their pitches, empty | you let them in |
## 2. Places (real practice)
1. **Booth on the driver's side.** Inbound cars head west; US drivers sit on the left, the **south**. New booth x 390.6 to 393.2, z 164.2 to 166.6: window on the north face (x 391.0 to 392.8, counter 1.0, shutter, drawer), **door 1.0 m** on the south face (x 391.4 to 392.4), stool with no collider, room for a second person behind (Quill 4), roof light. Cars stop with the driver's door 1.2 m off the window. Remove the old booth at (392, 176) and the turning circle (Main3.md 3.2.3 and 3.2.7, main3_reach_check_8_16a.cs and main3_8_9f_look.cs follow; Rook). The booth floor covers the SliceLook grass.
2. **Barrier:** arm 1.0 high, post 0.2 x 0.2 at (389.3, 166.3), arm over the lane to z 172.5, collider when down. Gaps: post to booth 1.2 m, post to a stopped car 1.4 m. On gate days the chain-link gate stands open; cars stop at the arm, inside the fence. ADMIT: the arm lifts, the car turns north into the spur (mouth flared x 381 to 389). REFUSE: the car backs 14 m out through the gate, front to x 403.5, and turns on the apron x 400 to 412, z 162 to 178 (Quill 6). Rook proves both swept paths with a top-down frame.
3. **Campground (block 2).** The spur runs to the chain at z 238 (posts x 387.63, 392.38), then joins the ring at about (385, 250). The ring is the built Loop0 to Loop39 road, r 15 to 20 round (372, 262), **one way, counter-clockwise**, with eight built pitches. **Clear the road band and every pitch:** move Marlow's list (RedwoodHollowLog_0 on Spur3 and at (389, 265), (362.9, 276.5), (354.6, 257.8), (365.1, 248.3); RedwoodHollowLog_1 at (380.2, 247) and on pitch (361.8, 266.2); RedwoodHollowLog_2 at (373, 281.6); trunks at (387.9, 263.2), (389.0, 256.8), (388.8, 253.1), (383.7, 248.6), (384.9, 275.8), (380.2, 278.8), (377.2, 279.5); stumps at (357.4, 253.3), (356.4, 268.1), (387.4, 266.6), (382.2, 257.8)) at least 1 m off the road edge and off the pads. Real practice: a closed loop is chained, not blocked; the deadfall stays between the pitches.
4. **Chain and parked cars.** Outside a car's passage the chain hangs 0.8 high with a collider; the player walks round its east post (3.6 m to the fence). For an admitted car it drops flat as the arm lifts and rises when the car passes z 242. Each car parks nose-in on the next empty pitch from the entry, P1 first, and stays, empty. Past eight, cars park on the ring's outer shoulder in order. Car speed 5 m/s: arm to P1 about 85 m (17 s), to P8 about 185 m (37 s).
5. **IW3 (Marlow's call):** on from the arm lifting for an admitted car until that car is parked, whatever the shift does. The shift itself runs from entering the booth to the day's last car or leaving the booth with no car waiting (Wren).
6. **Office**, local metres from the inside SW corner (344.25, 196.25); front room X 0 to 7.6, back room X 7.9 to 11.5, Z 0 to 7.5, ceiling 2.7.

| # | Thing | Where (X, Z) | Loop job |
|---|---|---|---|
| O1 | West door 1.2 as built, **propped open inward, no prompt**: Door startOpen and fixedSwing to the inside (Rook finds the sign; today both are false and it opens outward); reopened at every wake. The world shuts it only as an Office CHECK state | Z 2.15 to 3.35 | Safety |
| O2 | Window as built with a blind; bench; map board (no collider); chair in the NW corner | window Z 4.15 to 5.35; chair X 0.2 to 0.7, Z 6.9 to 7.5 | none |
| O3 | Counter 1.0 high, stamps, pad; R5's stool; radio on its north end (sound only, Pim) | counter X 2.2 to 2.8, Z 0 to 5.0; stool (3.3, 2.75); radio (2.5, 4.5) | Talk to R5 from (1.5, 2.75), 1.8 m |
| O4 | Built potbelly stove moves 0.55 m west; built filing and drawer cabinets move to the north wall; wall shelf stays | stove X 5.6 to 6.4, Z 0.35 to 1.15; cabinets X 5.4 to 7.4, Z 6.9 to 7.5 | none |
| O5 | Back-room door as built, hinged south | Z 4.15 to 5.35 | none |
| O6 | Built bed as the cot (boots on, Quill), trunk at its foot, desk with drawer, sink and hot plate flush to the locker | bed X 10.6 to 11.5, Z 5.3 to 7.3; desk X 10.8 to 11.5, Z 2.4 to 3.6; sink X 8.9 to 10.7, locker X 10.7 to 11.5, Z 0 to 0.6 | Talk to R5 lying down from (9.8, 6.3) |
7. **The deck reads the west door (Quill 1, Marlow 3).** The deck sees the opening, about 1.2 x 1.4 m at 183 m. Open, it is lit from inside (desk lamp); shut, it is a dark leaf. Porch/SignBoard moves off the opening, onto the wall south of the door. Rook proves it with two rendered frames at Grant's size from the lectern, open and shut, naked eye and binoculars. If they do not differ clearly, a porch lamp over the door is lit only while the door is open.
8. **Store.** Door widened to 1.0, x 364.5 to 365.5. **One store:** Store/Inside as built stays and is the store level's depth 0, so the windows and the level show the same room (panel B). Checkout_Counter moves flush to the front wall (it leaves 0.65 m today). Shelf aisles 1.0 to 2.0. Food: Take at the coolers. Bottle: Take on the east shelf run's east face. Porch 2 m deep, x 360 to 372; **canopy rebuilt to x 360.5 to 371.9, posts at (360.7, 193.8) and (371.9, 193.8)**, so the 367.9 post goes. **Ice chest = the built Ice_Cream_Freezer, moved to x 367.9 to 369.7, z 194.30 to 195.48, against the wall**: 0.4 m to the propane cage, 2.5 m to the door. It holds R5-2's search (Wren). Trash_Can moves against the bench.
9. **Lot** 30 x 40 at x 343 to 373, z 150 to 190, marked: 4 stalls by the office (x 344.3 to 356.0, one 3.6 accessible), 4 by the store, all nose-in z 184.5 to 190; 6 trailhead stalls on the west edge, z 153 to 169.2, clear of LotLights/Post (344.5, 151.5). R6's bay x 367.5 to 373, z 178.0 to 180.8, nose east. Walk z 190 to 193.5 to the porch and the west door. Verge tree SnappedTop is laid along the verge, off the road.
10. **Vault toilet (block 1)** x 340.4 to 342.0, z 185.1 to 186.9, door east onto the lot: 1.1 m east of the Hedge_Burn_8 colliders, east of the bushes (340.3). Marlow's OverlapBox first.
## 3. Times (2.5 m/s; Marlow measured 2 to 7 on the build)
| Leg | m | s | | Leg | m | s |
|---|---|---|---|---|---|---|
| cabin door to T by Jg (Valley) | 236 | 94 | | R6's bay to booth door | 26 | 10 |
| T to store porch | 38 | 15 | | booth door to T | 55 | 22 |
| porch to west door | 31 | 12 | | booth door to the chain | 78 | 31 |
| west door to talk spot | 2 | 1 | | chain, round the ring, back | 118 | 47 |
| west door to R6's driver door | 34 | 14 | | T board to toilet; west door to toilet | 14; 13 | 6; 5 |
Store run: 548 m, 219 s plus the store. All of it, the ring included, back to the cabin by the chain and T (115 m): 914 m, 366 s plus talk, store and shift. Forage is near and can miss; the store is sure and far (Dredge).
## 4. Walls
1. **IW1, exactly:** box x 395.7 to 396.3, z 167.4 to 172.6, y -7 to 53, as built; the fence panels overlap it 0.1 m at both ends. Always on, player only, cars pass. Speaks only in a shift. Rook moves IW1 and IW3 to a layer PlayerInteractor's mask leaves out (today the mask is all layers).
2. IW3: x 385.8 to 396.2 at z 210, as section 2.5. Quill's spur-mouth wall would be walked round east of the store.
3. Every other stop is seen: fence, brush bands, chain, walls.
## 5. Area check data
1. **Bounds x 318 to 448, z 128 to 290**; the cave drops its second rectangle and Closed_Campground.
2. **Warps:** Office (340, 196), Store (366, 193), Trailhead_T (337, 170), Lot_Highway (382, 168); Gate_Booth (392, 162.5) facing north; Closed_Campground (385, 232).
3. **Places, on the objects' own transforms:** office west door (344.1, 199); store door (365, 195.5); booth door (391.9, 164.2); lot centre (358, 170); trailhead board (its transform, z 171.5 to 173.5); first sight of the lot (327, 168); ice chest, R6's car (Resident_Car), toilet door, chain: their transforms.
4. **Deck must-see:** hard: office west door opening (344.0, 4.3, 199), verge tree (419, 25, 139), tested against meshes. Loose: store roof, lot centre (358, 3.1, 170), Resident_Car, the booth roof light, the barrier arm, a highway stretch (430, G, 185), the gate T stop sign moved to the exit's right side (423.1, 166.0). **Must-hide: none.** The ring is neither.
5. **Sightlines (Wren):** S1, the car's hood (372.5, ground + 1.2, 179.4) to the verge tree trunk (418, 20, 136). S2, the Ward lookout (29, ground + 1.6, 261) to the T (428, G, 170), road z 140 to 200 in frame.
## 6. Borrowed, and what none of them do
Papers, Please: the booth. Kiosk and Shift at Midnight: the counter. Exit 8: the store relearned. Fears to Fathom: a real-scale post. Firewatch: the road in view. None let the player walk up afterwards and count the cars they let in, parked and empty, with the exit 30 m away and open.
## 7. Open questions
1. Grant: the booth on the drive's south side, the turning circle gone, refused cars backing onto the apron.
2. Wren: front owns the campground (8.27's wording). Pim: FrontLayout_UI.md and GateBooth.md 2 and 9 follow this draft (booth, barrier, west door with no prompt, IW3 rule).
3. Quill: cars past eight on the shoulder, and the west door shut as an Office CHECK state.
Sable
