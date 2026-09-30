# Valley: Main3 in a ring of ridges

**DRAFT revision 3, 2026-09-29, Sable. Build source for Rook (DECISIONS 2026-09-29). Folds in Marlow's checks (Docs/Review/2026-09-29-ValleyNumbers.md, both rounds), Vesper's Edges.md revision 2, and Quill's platforms. Items marked G need a DECISIONS line and Grant's yes.** Drawing: Valley_map.svg (wins for shapes; this file wins for heights). Coordinates as Main3.md: metres, origin south-west, x east, z north, heights absolute. Everything in Main3.md rev 16 not named here stays.

## 1. Shape
1. The walkable map does not grow. The terrain grows 40 m west (x -40) for the west ridge's far face, and a ring of ridge terrain is added round the map (G: replaces "layers first, no scaling", DECISIONS 2026-09-29).
2. The Wall, the pass, the plateau, the low crest, the rock lip and the cliff-edge giant band are removed (G: replaces the plateau line, DECISIONS 2026-09-29). J, the cairn gate and the Camp to J trail stay.
3. Load-bearing land (W, N and S ridges up to their crests) is real terrain with its collider, so W-1 and E-1 rays hit it (Marlow 1.8). The E ridge, all back slopes and the outer floors hide nothing and may be collider-free backdrop meshes.
4. The player is stopped by rock bands, scree and deadfall at each ridge foot; any invisible stop sits 2 to 4 m behind them (Edges 1.4). Ridge faces within reach are steeper than 45 degrees or lined.

## 2. Ridges (Edges 1.2, 1.3: no straight crest, one saddle and one knob each, each side different)

| Ridge | Crest line (distance) | Crest heights along it | Saddle | Knob | Reads as |
|---|---|---|---|---|---|
| W | x 0 to 12, z -60 to 360; east foot bends round places (2.5) | z -60: 95; z 0: 100; **z 60: 95**; z 150: 108; z 200 to 235: 106; knob; z 300: 105; z 360: 100 | **95 at (0, 60)**, the lowest west point | **the Ward knob**: a flat-topped rock ridge, not a cone, **x 8 to 18 (10 m wide), z 242 to 280 (38 m long), 113 or more across all of it**, square ends; summit 115 at (12, 258); falls to 105 by z 300 | dark bare rock wall, fire behind; burned snags on top from day 2 |
| N | z 350 (50 m past the map edge), x 0 to 440 | x 0: 100; x 60: 88; x 90: 84; x 120: 94; x 170: 72; **x 250: 50**; x 320: 70; x 400: 62 | 50 at x 250 | 94 at x 120, bare rock | high forested flank |
| S | z -50 (50 m past the map edge), x 0 to 440 | x 0: 95; x 60: 90; x 110: 75; **x 170: 50**; x 240: 70; x 320: 58; x 400: 52 | 50 at x 170 | 70 at x 240, over the boathouse | bare granite over the lake |
| E | x 445 (49 m past the fence), z -50 to 350 | z -40: 45; z 60: 57; z 120: 47; road cut at z 170; z 250: 60; z 340: 55 | the road cut, a V to road level | 60 at z 250 | low, cut by the road; far range in the notch |

5. **W ridge east foot** (ground about 12): x 38 from z -60 to 85 (the ravine and cave keep their west end against it, x 44 to 80 unchanged); x 46 from z 100 to 180 (the Camp 3 rim and its blend to r 30 unchanged); x 80 from z 195 to 300 (J at x 104 unchanged; leg 1 at x 72 sits on the face, not the floor). West face: crest to x -40 at -40, a cliff.
6. The N and S ridges' west ends (x 0 to 120) are the hooks that wrap the fire: load-bearing (1.3).
7. **Back slopes** (Edges 1.1, 9.2): N, S and E fall from the crest to an outer floor at 0 to 10 that runs at least 150 m past the crest (N to z 500, S to z -200, E to x 595); the far range backdrop stands beyond. The tower looks over the N and S saddles and the whole E ridge, so no border or void may show.
8. **West of x -40** (Edges 9.3): a flat gray floor at -40 out to a gray far ridge, crest 30, at x -300 to -500, z -250 to 750; both meshes run 500 m past the last flame, no colliders.

## 3. The fire and the smoke
1. **The fire moves out and narrows.** Far front: on the far ridge, x -300 to -500 (the stand-in at x -190 moves out, Marlow 1.1), z -250 to 750, flame tops 130. Valley fires: floor -40, x -50 to -150, z 0 to 500, flame tops 60. From the ledge the valley fires carry the edges: 154 degrees in all (Style 6.3.1 holds).
2. **Day 1 and night 1: east wind** (G, listed open). Smoke streams due west over the fire as a flat sheet with a **hard top**: 95 within 50 m of the crest, 105 at x -100, 145 at x -300, 165 at x -400, 200 at x -600; none north of z 750 or south of z -250. That keeps 3 m at the W saddle. No glow on the sky east of the crest. The reveal is at the ledge (Quill).
3. **Night 2 on and day 2 on:** columns stand and lean east over the crest; glow above the crest, never flame (Style 6.3.3, 6.3.6 to 6.3.9; Edges 5).
4. **Day-1 sun** (Edges 8, P): bearing 200, elevation 32. The W ridge shadow reaches about x 57; Camp 3, J and the west trails are lit.

## 4. Sightlines (paper; eye plus 3 m and target plus 3 m; Rook reruns W-1, C-1, E-1 on terrain)

| From | To | Crosses | Line / crest | Result |
|---|---|---|---|---|
| Tower eye 57.6 | flames through the W saddle, (-300, -134) | (0, 60) | 86.2 / 95 | hidden, 8.8 |
| Tower | day-1 smoke tops (3.2) through the W saddle | (0, 60) | 92 / 95 | hidden, 3 |
| Tower | far front SW end (-300, -250) | (0, 19) | 86.2 / 98.4 | hidden, 12.2 |
| Tower | far front NW end (-300, 750) | N hook (10.8, 358.8) | 84.5 / 98.5 | hidden, 14 |
| Camp 2 stack, Camp 1, lot, gate, camp, S1 | fire ends | N hook, N ridge, S hook | Marlow R2 item 3 | hidden, 17 or more |
| Tower | every Ward stone corner, tops 110 | knob east edge x 18, z 253 to 264 | 105.8 to 108.1 / 113 | hidden, **4.9 or more (4.3 at jump height)** |
| Tower | knob summit 115 | nothing | seen | the tower sees the hill every day, never what is on it |
| Tower | N saddle, S saddle, E cut | | bearings 25, 178, 89 | far ranges only; the fire (228 to 322) and the Ward (301) are 50 degrees or more away |
| Climb: P4, leg 5, cleft east mouth | flames and day-1 smoke | W crest (106 at z 200 to 235) | P4 6.7; leg 5 6.4; cleft mouth 5.3 (smoke 4.4) | hidden until the step past the fin (5.5) |

5. The tower still sees all six places (Marlow 1.4); the cave stays hidden by the rim (C-1).

## 5. The climb (J to the ledge, night only)
1. J (104, 206) west 32 m to leg 1's start (72, 205) at 12, crossing the ridge foot at x 80.
2. Four legs on the east face, north-south between z 205 and 290, each 85 m rising 20 m (23.5 percent): leg 1 x 72 north, leg 2 x 59 south, leg 3 x 46 north, leg 4 x 33 south. **Four level platforms** at the turns, 3 m deep, P1 to P3 13 m across to join the legs: P1 (65, 291) 32, P2 (52, 204) 52, P3 (39, 291) 72, P4 (33, 204) 92, 3 x 3 m. Events and scares use them (Quill). The face is shaped so each leg sits on a bench at its own height. Stops on the downhill edges as on the tower stair.
3. **Look-back (replaces the pass, G):** every platform faces east across the valley; P3 is the first above the tower deck (16 m over it), P4 the highest, 36 m over it.
4. Leg 5: P4 north-west to the cleft's east mouth (20, 230) at 95, 29 m.
5. **The cleft:** a slot 2.5 m wide, floor 95, walls 106 or higher. Part A runs west 15 m along z 230 to (5, 230). Part B turns right, 90 degrees, and runs north 8 m to the west mouth (4, 238), behind a rock fin 2 m thick on its west side (top 106). Any straight view out of part B points north of bearing 340, past the fire's north end, and every view down meets the ledge, which stands 3 m above the slot floor. The fire shows only at the step past the fin's end. Silence starts inside (Hollis).
6. **The ledge:** a shelf on the west face, ground 98, x -8 to 4. The path rises from the west mouth (4, 238) at 95 north to the end of the path at (-2, 258), facing west. **The reveal is the step out of the cleft.** Stones to the right of the path end, to one side of the approach (G, DECISIONS 2026-09-25): (-4, 268), (1, 271), (-7, 272), 3.6 x 4 m, tops 110, all behind the knob from the tower.
7. Length on paper: 32 + 340 legs + 42 platforms + 29 + 23 cleft + 21 ledge = **487 m, 195 s**; Camp to the Ward **583 m, 233 s**. Marlow's 455 m is the floor with no platforms; the build may run 5 percent over paper. Walk times are information, not a budget.

## 6. Answers to Edges.md
1. **East wind on night 1 (Vesper Q1):** keep it, change Style 6.3 for night 1 only. Anything hidden from the tower sits at most about 10 degrees above level from the ledge, so columns leaning over the player would show from the floor before the ledge. Night 1: a lit sheet streaming west, low; the burning valley below fills the frame test; flame tops about 6 degrees up. 6.3.3 holds from night 2, when the wind turns and the smoke comes toward you. Agreed with Vesper (Edges 5, 6).
2. **Headlight in the road cut on night 1 (Vesper Q2):** keep. One car passing in the cut that never turns in, timed for P4's look-back (P3's view is likely blocked by giants, Marlow R2 item 10). After night 1 the cut stays dark at night (gate cars still come by day). The last ordinary light, seen as the player turns toward the fire; its absence later is the wrong detail (Exit 8). Hollis aligns the road sound.

## 7. Open
1. G: the four lines marked G, and the east wind (3.2).
2. Vesper: Style 6.3.1 to 6.3.5 night-1 rewrite; the ledge frame from 98.
3. Marlow: the cleft's views out of part B; the knob's 3 m rule on the built mesh; P4's headlight view once groves are placed.
4. Night walk: 233 s each night is 34 s over the as-built climb. Playtest whether the nightly walk drags; the platforms' events are the answer, not a shortcut.

Sable
