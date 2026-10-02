# 8.22 gate, Marlow's hand walk: front zone
2026-10-02, Marlow. **FAIL.** Main3 at 0a963df (later commits 8a9604f, 193deb0 are docs only). Sheets: Docs/Captures/Main3Review_front. One Play session I entered and stopped; every move is PlayerController.Step (dt 0.02, walk 2.5, sprint 5.5, jump 0.6), detached jobs; frames by ScreenCapture to the scratchpad (not committed). Nothing saved; git status unchanged. Severity: blocks, hurts, cosmetic. Positions are world metres.

## Findings
1. **The closed campground leaks west over the brush band.** Blocks. RedwoodHollowLog_0 (x 349.98 to 351.01, z 257.3 to 261.8, top 3.91) lies 2 m from FrontZone/BrushBands/Brush (x 340 to 348, z 206 to 310, top 4.40). A sprint-jump from (353, 3.0, 260) heading 292.5 lands on the band top at (346.95, 4.44, 263.40); from there the player walks along the top anywhere from z 206 to 310 and steps off west to the forest floor at (326.8, 4.85, 263.4), then on to (277, 4.8, 300.5) in 20 s. Valley.md rev 10: "the closed campground is reached only by the spur"; FrontLayout 4.3 lists the brush bands as stops. Rook's FLOOD passes because it fails only on closed-zone places and fell-through; an exit past a stop inside the bounds is not a fail. 46 more grid spots stand on that log; 1 of 2,072 ring starts reached the band. Repro: stand at (353, 260), sprint and jump toward WNW over the log.
2. **Both brush bands are invisible walls on open ground.** Hurts; Gate 2.2 FAIL. Both FrontZone/BrushBands/Brush renderers are disabled (material Slice_Brush). Walking north anywhere on x 349 to 386 stops at z 205.62; from 2 m back at (380, 203.6) the frame shows bare ground to the tree line, nothing at the stop. West band: walking west stops at x 348.38 for z 225 to 280; from its top (finding 1) the ground under it is forest floor with scattered ferns and two bushes, no brush line. FrontLayout 4.3: "every other stop is seen: fence, brush bands". Stops_1.jpg only lists stops within 5 m of a trail, so neither band is on the sheets. Repro: walk north from (380, 200).
3. **The store cannot be entered.** Hurts. FrontZone/Store/Shell/Door_GlassPanel is a solid BoxCollider (x 364.5 to 365.5, z 195.57 to 195.63) with no Door, trigger or Interactable within 4 m. Walk and sprint-jump north from (365, 193) stop at z 195.08. FrontLayout 1: "walk into the push door ... inside, the same store the windows showed"; Food is "Take at the coolers". FrontLayout_UI marks the push door and level load "Rook: unverified". The inside (35 colliders) is reachable only by teleport.
4. **Closed_Campground warp shows no place.** Hurts; Gate 2.3 FAIL. It lands at (385, 3.04, 232) on the spur's west verge, south of the chain, facing 336.6: the frame is trunks, fir fronds and a hollow log; no road, no chain (bearing about 40, off frame), no ring. 19 renderers lie within 1 m of the point 1 m ahead (RedPine2 LODs); the sheet index already lists RedPine2 at 0.0 m in N and W. Repro: dev menu, Closed campground.
5. **Porch bench puts the eye inside the porch roof.** Cosmetic. A sprint-jump onto FrontZone/Office/Porch/Bench (top 4.04) from (340, 198) heading 67.5 stands the player at (343.63, 4.0 to 4.07, 201.1); eye 5.6 is inside Office/Porch/PorchRoof (no collider). The frame is the roof's inside.
6. **Ice chest and propane cage can be stood on; the canopy has no collider.** Cosmetic. Sprint-jumps reach the Ice_Cream_Freezer lids (3.94) and PropaneCage/CageBlock (4.62). From the cage the eye is 6.22, above Store/Canopy/Deck: the body stands through the canopy and the view looks over its top. On the bench under the canopy (3.78) the head is in the Deck mesh. Repro: from (364.5, 193) sprint and jump heading 67.5.
7. **Store inside: standing on the counter puts the head through the roof.** Cosmetic, matters once finding 3 is fixed. Checkout_Counter/Cash_Drawer_Cover top 4.17; the head (5.97) is in Store/Roof/Roof_Tile_Flat (no collider, about 5.7).
8. **Other furniture you can stand on.** Cosmetic. Office front chair (4.17), back-room table and lamp (4.37), bed (4.21), toilet bench (3.59), dumpster lids (4.26), cardboard boxes, the campground logs and stumps. All escape; no traps.
9. **Doc numbers against the build.** Cosmetic, Sable's. Barrier post to booth 1.00 m (doc 2.2: 1.2). Rook's timed walks against doc 3: chain, ring and back 152.4 m, 60.5 s (118, 47); R6's bay to booth door 32.1, 13.8 (26, 10); west door to R6's driver door 38.0 (34); T board to toilet 17.6 (14); west door to toilet 16.0 (13).
10. **Lot_Highway warp is not in the lot.** Cosmetic. (382, 168) is on the drive 9 m east of the lot edge (x 373); the label reads "Lot, facing the highway".

## Passes
- **Leaving the map:** east, fence and IW1, 349 starts from z 128 to 302 at 5 headings, walk and sprint-jump: max x 395.60. North, x 349 to 395.5 at 8 headings for 25 s: max z 304.21 (Rock/Bands/Band_W_N, Outcrop_N). Nothing climbable stands within reach of the fence: the car (body 0.95), barrier arm (1.05) and posts (1.15 to 1.2) cannot be mounted; 462 starts round the booth and gate gave 0 raised ends. The T and highway cannot be reached on foot.
- **IW3 by script:** off at start, a walk north passes z 215; after SpurWall.CarAdmitted it stops walk and sprint-jump at z 209.42 for x 386.2 to 395.7, and from the north at 210.59; after CarParked it passes again. Placed inside the box while on, the player is pushed out north and can walk on; only possible if a car is admitted while the player stands at z 210, which the booth rule prevents. Brush band overlap and the fence seal both ends.
- **Booth:** in through the 1.0 m south door to (391.9, 165.1) and out; nothing in the booth can be climbed.
- **Office:** west door open 90, leaf inside, CanUse false (no prompt). Porch to talk spot, round the counter end, through the back door (opens with Use, swings into the back room), to the cot and desk and back: 37.8 m, 15.3 s, no stall. Behind the counter to the stool and stove: clear.
- **Toilet:** in through the east door to (341.5, 186) and out.
- **Campground:** booth door, round the chain's east post, the whole ring counter-clockwise: 206 m, no stall; ring to each of P1 to P8 and back, about 10.4 m each, no stall.
- **Traps:** 0 in 8,722 grid starts (0.5 m round every built piece, 1 m over the lot, west edge, spur and ring), 139,552 walk and sprint-jump moves; every raised end escapes. 0 fell through.
- **Warps:** Office, Store, Gate_Booth, Trailhead_T, Lot_Highway and Closed_Campground land on terrain within 0.04 m and walk 3 m in 8 of 8 directions (Store 2.1 and 2.6 toward the wall). Faults in what they show are items 4 and 10.
- **Deck door frames:** in the binocular pair the change is the porch lamp; the opening itself barely changes. Under Wren's two 2026-10-02 lines in Status.md (the deck check counts from anywhere on the deck; the lamp is on only while the door is open) this passes.

## Gate step 2, front zone
| Item | Result | Frames |
|---|---|---|
| 2.1 trails | not checked this round | |
| 2.2 invisible stops | FAIL, item 2 | not on Stops_1.jpg; my frame stopS_380 |
| 2.3 trail ends and warps | FAIL, item 4 | Warps_NESW Closed_Campground |
| 2.4 places | PASS outside; store inside not reachable (item 3) | AreaFrames_front |
| 2.5 lot and office: road in frame | PASS | Lot_Road_Full, GateT_Zoom |
| 2.6 to 2.9 | not this area's | |
| 2.10 hand walk | FAIL, items 1 to 4 | |

Done-check, word for word: "Marlow's flood, trap and walk-into checks find 0 problems in it": FAIL (items 1, 2). "every warp in it lands": PASS (all six land; item 4 is Gate 2.3). The rest are Pim's, Sable's, the deck's and Vesper's.

Marlow
