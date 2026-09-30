# Check 2: playability of Main3 as a player sees it (Marlow, 2026-09-30)

Judged from sheets/ (made 07:39 today) and shots/. Frame names are sheet + label. No hand walk done (checklist item 10 still open). Severity: blocks, hurts, cosmetic.

## Checklist (Meeting_Marlow.md 3)
1. **Path readable from ground (fails, blocks).** On open ground no frame shows a path: Boathouse to Camp 2 FWD 20 to 60, Jg to T FWD 0 to 100 and BACK 0 to 100, Jg to Camp 1 FWD 0 to 70, Camp to Jg FWD 40 to 100, Camp 2 to T FWD 10 to 50. Only trenches read as routes (Camp to pump FWD 20 to 60, Camp to Camp 3 FWD 10), and non-trails read the same: the pump notch (shots/pump_from_lake.png, Warps W4 N) looks like a path climbing between two rock lines.
2. **Every stop has a reason (fails, blocks).** Of 89 stops, 46 face open walkable ground with nothing drawn: S2 to S5, S7 to S10, S12 to S15, S18 to S23, S25 to S30, S35, S48 to S60, S67, S68, S74 to S76, S83 to S85. S36 (cairn gate) shows a flat slab and no readable chain. The 43 with a reason mostly get it from untextured gray cubes (S1, S31, S62, S69), which read as placeholders, not rock.
3. **Every trail end reaches a place (fails, hurts).** Trail_Ends: Camp 2 to T END (299,108) and Boathouse to Camp 2 START (299,106) end nose to a gray slab, the Camp 2 tower; Jg to Camp 1 START (281,236) ends against a pole; W1 to cave START (52,38) ends in a gray box tunnel; Camp to Camp 3 START and W1 to Camp 3 START (79,145) end in a pit ringed by floating cubes and one capsule; Camp to pump START (190,96) is a mound that hides the lake. Cut-offs: pump notch (W4 N, Stops S34), Camp 2 view cut and creek channel (Facts_Rook 5) look walkable and are walled.
4. **Every place reads without labels (fails, hurts).** Camp 2 is a gray slab with a ladder (Warps W7 N). Boathouse is a gray box (Pump to boathouse FWD 80, W5 W). Camp 3 is a hole (W8 N/E/S/W). Cave mouth and chamber are gray boxes in murk (W18 S, W24 all). Camp 1 is two gray tents and a pole (Jg to Camp 1 FWD 80). Ward stones are two flat slabs (Climb 490, 500). Large gray domes near camp (Camp to J FWD 0 to 50, S6) read as nothing; unverified what they stand in for.
5. **Lot and office see the road (fails, blocks per Grant).** Pairs Office DAY ONE and Warps W10 E: the Office warp spawns facing a blank side wall. shots/office_to_road.png and W12 S: lot, fence, booth, ridge with a notch, no road. Lot DAY ONE: two identical brick sheds. Store from outside (W11 N): brick box, dark doorway, no sign, no lit cooler, same material and scale as the office; a player cannot tell which is the store.
6. **Climb legs differ (fails, hurts).** Climb 50 to 110, 150 to 210, 250 to 310, 350 to 420 are the same frame: cliff left, gray cube row right, haze. The only change is the tower shrinking. Hairpins face a cube wall (Climb 30, 130, 330). Cleft is full-frame brown with no sky (Climb 460, 470; J to Ward BACK 20, 30, 40). Payoff is two gray slabs and a flame strip (Climb 500, Pairs Ward DAY ONE). 196 s for that.
7. **No empty quarter (fails, hurts).** Map_TopDown: x 80 to 230, z 210 to 300 is bare tan with one grove and no trail in; the closed campground loop is empty. Jg to Camp 1 FWD 20 to 80 and W6 N: flat floor up to a flat, untextured N ridge with a hard polygon crest. West of x 186 only giants (Facts_Rook 8).
8. **Day and night read (fails, blocks at night).** Night pairs: ground, trail, walls and stops are black; only the fire, office windows, lamp and a ghost cairn show (Pairs Camp, S1, Lot, J NIGHT). A night walk cannot follow any trail. Day: beige haze flattens every frame; backlit frames wash out (Camp to Jg BACK 40, Jg to Camp 1 BACK 0 to 50); the south ridge shadow makes W1 to cave FWD 0 to 90 near-black murk.
9. **F1 reachable (Pim's; Rook's facts only).** LOOK is 27 presses down and unreachable by wheel (Facts_Rook 1, shots/f1_scrolled.png). Not retested by me.
10. **My hand walk (not done).** Pictures only; collision, the cleft width and camera clipping at 0.8 m walls are unverified by hand.

## Getting lost
Next destination visible: yes at Camp (tower, cabin), Boathouse, Camp 1, T, Jg, J, W1 (all see the tower). No at Lake Pump (W4 N/E/W are banks and cubes, S is the dock only), Camp 3 (all four W8 frames are walls), Cave Mouth (W18 all murk), Camp 2 (W7 N a slab). Junctions carry no marker: W15 N/E/S/W are four near-identical burn frames. The tower is the only landmark; lose it and the player is lost.

## Also found (Grant's next walk)
- Floating cubes: Pump to W1 FWD 10 (cube hangs clear of the slope), W1 to Camp 3 FWD 110, W8 N. Cosmetic.
- Close-range giant bark is pixel mush: Jg to T FWD 20, 30, BACK 60; W9 W; S55. Cosmetic.
- Pond at W1 is a flat blue polygon with hard corners: W17 E, W1 to Camp 3 BACK 110. Cosmetic.
- Ward fire is a straight row of billboard flames on flat ground, reads as candles: W21 W, Pairs Ward. Hurts (it is the game's key sight).
- Boredom: Jg to T is 22 identical frames; W1 to cave FWD 0 to 60 stares at a blank ridge. Hurts.
- Far view seam: W20 E shows a dark band between the valley floor and the far hills. Cosmetic.

## Rook's 0.92 m point
Found by read-only eval of the same probe: W1 to cave 110 m (52.1, -6.0, 37.6). The probe hit Plank/DayOneBoard (the cave board), not ground; the trail point is 0.10 m under the terrain. Second worst 0.90 m is the cairn chain at J to Ward 4 m. Real trail-to-ground worst is 0.17 m. In frames: W1 to cave FWD 110 is a gray slab filling the view and Trail_Ends W1 to cave START is a gray box tunnel. Not a floating trail; it looks broken because the cave mouth is a gray box.

## Ten worst, ranked (fix at paper level; whose job)
1. Trails invisible by day and night. Own trail material, edge stones or logs, ground cover up to the edge; pale edge markers for night. Vesper look, Sable rule, Rook build.
2. 46 pointless invisible stops, about 970 unmarked wall pieces. Rule: a wall exists only where something drawn stops you; else delete it. Sable rule, Rook build, Marlow recheck on Stops sheets.
3. Places are gray stand-ins (Camp 2 tower, boathouse, cubes, Ward stones, cave, domes). Swap to pack meshes before the next Grant walk. Vesper list, Rook build.
4. Climb: four equal legs, blind hairpins, brown cleft, slab payoff. Vary lengths and grades, one view or object per hairpin, sky in the cleft, real stones. Sable design, Rook build.
5. Dead ends with no onward view: Pump, Camp 3, Cave Mouth, Camp 2 end. Fill or legitimise the pump notch, open Camp 3 on one side, frame and light the cave mouth, end Camp 2 trails beside the tower. Sable.
6. Night unreadable. Ground and trail legible at 10 m by moonlight. Vesper.
7. No road from lot and office; Office warp faces a wall. Build the road in the E cut per Edges.md 4; re-aim the warp. Sable, Rook.
8. Store looks like the office. Sign, lit cooler, distinct shape; interior is its own level (DECISIONS 2026-09-30). Sable, Vesper.
9. Empty north and flat edge ridges. One reason to go north; ridge material and silhouette. Sable content, Vesper look.
10. Unmarked junctions and long same-looking stretches. Sign at T, Jg, J, W1; one landmark per 50 m. Sable.

Marlow
