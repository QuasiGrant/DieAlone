# Gate step 3, PLAN 8.14 (valley rev 10 ground and road): Vesper

2026-09-30. Sheets: Docs/Captures/Main3Review (10:12), every sheet read. Bar: Style.md 10, hard lines 1 to 7 plus the 8.14 stage line. Look: D1 (day one) and Night.

**Verdict: FAIL.** Five zones at D or F. The terrain reads as blockout: no floor, bank or ridge texture anywhere, and the west face and climb are box geometry.

## Grades

| Zone | Grade | Why (frames) |
|---|---|---|
| Valley floor and banks | **F** | Untextured tan everywhere; the stage line says missing textures are graded. Trench banks are one flat face over a third of the frame: Camp_to_pump FWD 40, 50, 60, BACK 10 to 40; Pump_to_W1 BACK 60; Camp_to_Camp_3 FWD 90, 100. Bare flat ground over the lower half: Jg_to_Camp_1 FWD 0 to 60, Jg_to_T FWD 40 to 60 (hard line 7). |
| W ridge, knob and saddle | **F** | Flat planes with V-shaped cuts and a crenellated top, a fortress rather than a ridge: Camp_to_J FWD 0 to 70, pair J day, W1_to_Camp_3 FWD 0 to 40, Trail_Ends Camp to J start. The knob reads as a smooth cone (forbidden 8.6): Climb 60 to 110. From the floor the crest is one straight line: W1_to_cave FWD 0, 10; Pump_to_W1 FWD 50 to 70. No saddle reads in any frame. |
| N and S arms | **D** | Smooth domes, crests straight sweeps, no knob or saddle visible: N arm, Jg_to_Camp_1 FWD 0 to 70; S arm, Boathouse_to_Camp_2 BACK 50, 60, Pump_to_boathouse FWD 50 to 80. The granite knob 45 over the boathouse does not read as rock. Plus: they layer blue-grey and paler than the floor at Jg_to_T BACK 10 to 90 (hard line 3 met there). |
| Rock bands | **F** | Vertical stacked slabs with stepped, battlement tops and one stamped texture. They read as masonry, not strata; they meet the ground at a hard line with no scree: Climb 0, 10, pair J day, Camp_to_J FWD 60, 70. Slopes over 35 degrees stay tan, not rock: Camp_to_pump FWD 50, 60, Pump_to_W1 FWD 0, 10. |
| Highway and open east | **D** | The east opens well: low layered far ground, lot light, mast (Camp_2_to_T FWD 20 to 90, Jg_to_T FWD 80, 90, Boathouse_to_Camp_2 FWD 20 to 40). But the stage line fails: no road, ditch or T reads past the gate in the lot frames (Trail_Ends Jg to T start, Jg_to_T FWD 100, pair lot). The lot is a flat black untextured slab over the lower half (same frames, Camp_2_to_T FWD 70, 80). |
| The climb | **F** | Untextured grey stairs and slabs: Climb 20 to 50, Stops S2 to S4. All-wall frames (hard line 6): Climb 210, 220, 230; Trail_J_to_Ward BACK 20, 30. Legs 1, 3 and 4 all read as the same brown corridor (Climb 20 to 40, 130 to 150, 170 to 200). |
| Ledge reveal | **D** | Width is right: the front runs past both frame edges (pair Ward night). It fails the rest of 6.3: one even row of identical flame cards; no smoke columns; no #5A2412 sky glow, black straight above; no valley fires in frame; frame test 5 (half the sky) fails; forbidden 8.15 (thin strip). By day the far ridge is a pure black band with flame flecks on it (pair Ward day, Climb 240, 250, Trail_Ends J to Ward start): hard line 5, and no haze on it. The ledge floor is flat untextured. |
| Night pairs | **D** | Hard line 4: no crest reads against the horizon in camp, S1 or J night. Grass renders as white speckle (S1 night lower half, camp night left). Camp fire is a modest practical (pass). |
| Lighting (D1) | **D** | Pass: warm key, long shadows falling east (Jg_to_T FWD 0 to 60), far ridges pale and cool at Jg_to_T BACK. Fail: no near-black anchor in open frames, ground and haze one value (Jg_to_Camp_1 FWD 0 to 80); shade warm, not cool; no gold horizon band; white sun smear (Jg_to_T BACK 10, Camp_to_Jg BACK 20, 40). Hard lines 1 and 2. |

## A Tri

Yes, make A Tri day one now. It is the one lighting change for the next retake. Before and after go side by side at the six LightingOptions spots. sunsetTrilight exists in LookTuning.cs (line 62); Rook sets it and copies A's values. Hold sunElevation 20 for the retake after it (one change per retake). A Tri fixes the anchor and cool shade (hard lines 1, 2). It does not fix the gold band; the cause of that is still Rook's open item.

## Rook's rock walls (3.5 m round the climb)

Not acceptable as built. A 3.5 m wall on both sides for most of 256 m blinds legs 2 and 4, gives all-wall frames, and breaks DECISIONS 2026-09-30 ("the climb is small and never oppressive"). Masonry tops make it worse. What I would do instead:
1. Full height only on the uphill side and at the few skip points. On the valley side, a broken rim of boulders at about 1.2 m, so the valley and the tower show (legs 2, P4 look-back).
2. Tops broken into blocks of 2.5 to 5 m; faces leaning 70 to 80 degrees, not vertical; scree at the foot.
3. Ask Rook whether the mover can stop jump-climbing faces over its slope limit, so the walls can come down to rims. Unverified; his call.

## Noted for later stages (not graded here)

- 8.15: stops are untextured stairs (S2 to S4); the trail does not read apart from the ground anywhere.
- 8.16: trees stand in islands; the W wall shows no crest belt.
- 8.17: grey domes and cubes as boulders and props (Camp_to_J, Camp_2_to_T FWD 20 to 40); the office is a plain box face over a third of the frame (pair office day); the night windows are flat salmon panels, too bright; the cave mouth is a grey box (W1_to_cave FWD 100, 110); the boathouse is a box; the Ward stones are grey slabs (Climb 240); the tents and pole at Camp 1 are primitives (Jg_to_Camp_1 FWD 80).

## Fixes, ranked (each becomes a task)

1. Owned terrain layers now: floor, bank and rock by slope over 35 degrees. They lift floor, banks, arms and climb off F.
2. Rebuild the W face as broken, leaning rock with an uneven crest. Make the knob a rock mass, not a cone, and let the saddle read from camp and J.
3. Rock bands as sloped strata with scree feet, not stacked vertical slabs.
4. Highway and T in the lot frame; texture the lot.
5. Climb walls as above; texture the stairs.
6. Ledge fire to 6.3: varied flame heights, 3 to 5 lit columns, sky glow, valley fires in frame. By day, haze on the far ridge and no flame tips.
7. Night: crest against the #0C1016 horizon; fix the grass speckle.
8. A Tri as the one lighting change.

Vesper
