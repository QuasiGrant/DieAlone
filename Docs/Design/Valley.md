# Valley: Main3 in a ring of ridges

**DRAFT revision 2, 2026-09-29, Sable. Build source for Rook (DECISIONS 2026-09-29). Folds in Marlow's check (Docs/Review/2026-09-29-ValleyNumbers.md), Vesper's Edges.md rules 1 to 8, and Quill's platforms. Items marked G need a DECISIONS line and Grant's yes.** Drawing: Valley_map.svg (wins for shapes; this file wins for heights). Coordinates as Main3.md: metres, origin south-west, x east, z north, heights absolute. Everything in Main3.md rev 16 not named here stays.

## 1. Shape
1. The walkable map does not grow. The terrain grows 40 m west (x -40) for the west ridge's far face, and a ring of ridge terrain is added round the map (G: replaces "layers first, no scaling", DECISIONS 2026-09-29).
2. The Wall, the pass, the plateau, the low crest, the rock lip and the cliff-edge giant band are removed (G: replaces the plateau line, DECISIONS 2026-09-29). J, the cairn gate and the Camp to J trail stay.
3. Load-bearing land (W, N and S ridges) is real terrain with its collider, so W-1 and E-1 rays hit it (Marlow 1.8). The E ridge hides nothing and may stay a collider-free backdrop.
4. The player is stopped by rock bands, scree and deadfall at each ridge foot; any invisible stop sits 2 to 4 m behind them (Edges 1.4). Ridge faces within reach are steeper than 45 degrees or lined.

## 2. Ridges (Edges 1.2, 1.3: no straight crest, one saddle and one knob each, each side different)

| Ridge | Crest line (distance) | Crest heights along it | Saddle | Knob | Reads as |
|---|---|---|---|---|---|
| W | x 0 to 12, z -60 to 360; east foot bends round places (1.5 below) | z -60: 95; z 0: 100; **z 60: 95**; z 150: 108; z 200: 104; z 230: 105; x 8 to 14, z 242 to 268: **113 or more**; z 300: 105; z 360: 100 | **95 at (0, 60)**, the lowest west point | **the Ward knob**: a north-south rock ridge, not a cone, 26 m long, 12 m wide at 113, summit 115 at (12, 258) | dark bare rock wall, fire behind; burned snags on top from day 2 |
| N | z 350 (50 m past the map edge), x 0 to 440 | x 0: 100; x 60: 88; x 90: 80; x 120: 90; x 170: 72; **x 250: 50**; x 320: 70; x 400: 62 | 50 at x 250 | 90 at x 120, bare rock | high forested flank |
| S | z -50 (50 m past the map edge), x 0 to 440 | x 0: 95; x 60: 90; x 110: 75; **x 170: 50**; x 240: 70; x 320: 58; x 400: 52 | 50 at x 170 | 70 at x 240, over the boathouse | bare granite over the lake |
| E | x 445 (49 m past the fence), z -50 to 350 | z -40: 52; z 60: 55; z 120: 50; road cut at z 170; z 250: 60; z 340: 55 | the road cut, a V to road level | 60 at z 250 | low, cut by the road; far range in the notch |

5. **W ridge east foot** (ground about 12): x 38 from z -60 to 85 (the ravine and cave keep their west end against it, x 44 to 80 unchanged); x 46 from z 100 to 180 (the Camp 3 rim and its blend to r 30 unchanged); x 72 from z 195 to 300 (J at x 104 unchanged). West face: crest to the valley floor -40 at x -40, a cliff.
6. The N and S ridges' west ends (x 0 to 120) are the hooks that wrap the fire: they are load-bearing (3.2).

## 3. The fire and the smoke
1. **The fire moves out and narrows.** Far front: x -300 to -500 (300 to 500 m past the crest; the stand-in at x -190 moves out, Marlow 1.1), z -250 to 750, flame tops 130 on a far ridge at 30. Valley fires: floor -40, x -50 to -150, z 0 to 500, flame tops 60. From the ledge the valley fires carry the edges: 154 degrees in all (Style 6.3.1 holds).
2. **Day 1 and night 1: east wind** (G, listed open). Smoke streams due west over the fire as a flat sheet: top under 100 within 100 m of the crest, 150 at x -300, 170 at x -400, 210 at x -600; none north of z 750 or south of z -250. No glow on the sky east of the crest. The reveal is at the ledge (Quill).
3. **Night 2 on and day 2 on:** columns stand and lean east over the crest; glow above the crest, never flame (Style 6.3.3, 6.3.6 to 6.3.9; Edges 5).

## 4. Sightlines (paper; eye plus 3 m and target plus 3 m; Rook reruns W-1, C-1, E-1 on terrain)

| From | To | Crosses | Line / crest | Result |
|---|---|---|---|---|
| Tower eye 57.6 | far front (-300, -134), through the W saddle | (0, 60) | 86.2 / 95 | hidden, 8.8 |
| Tower | far front SW end (-300, -250) | (0, 19) | 86.2 / 98 | hidden, 11.8 |
| Tower | far front NW end (-300, 750) | N hook (18, 350) | 83.4 / 96 | hidden, 12.6 |
| Camp 2 stack 25.6 | (-300, 750) / (-300, -250) | N (69, 350) / S (31, -50) | 68 / 86; 75 / 92 | hidden |
| Camp 1, lot, gate | fire ends | N ridge x 155 to 175; S hook x 38 or less | 37 to 42 / 70+; 82 / 91 | hidden |
| Tower | Ward stones, tops 110, every corner | knob at x 12, z 256 to 266 | 107 to 110 / 113 | hidden, 3 or more |
| Tower | knob summit 115 | nothing | seen | the tower sees the hill every day, never what is on it |
| Tower | N saddle (250, 350), S saddle (170, -50), E cut (445, 170) | | bearings 25, 178, 89 | far ranges only; the fire (bearings 228 to 322) and the Ward (301) are 50 degrees or more away |
| Every trail point on the climb | the fire | W crest due west | 6 m or more below it | hidden until the ledge |

5. The tower still sees all six places (Marlow 1.4); the cave stays hidden by the rim (C-1).

## 5. The climb (J to the ledge, night only)
1. J (104, 206) west 32 m to the foot (72, 205) at 12.
2. Four legs on the east face, north-south between z 205 and 290, each 85 m rising 20 m (23.5 percent): leg 1 x 72 north, leg 2 x 59 south, leg 3 x 46 north, leg 4 x 33 south. **Four level platforms** at the turns, 3 m deep, P1 to P3 13 m across to join the legs: P1 (65, 291) 32, P2 (52, 204) 52, P3 (39, 291) 72, P4 (33, 204) 92, 3 x 3 m. Events and scares use them (Quill). The face is shaped so each leg sits on a bench at its own height. Stops on the downhill edges as on the tower stair.
3. **Look-back (replaces the pass, G):** every platform faces east across the valley; P3 is the first above the tower deck (16 m over it), P4 the widest, 36 m over it.
4. Leg 5: P4 north-west to the cleft mouth (20, 230) at 97, 29 m.
5. **The cleft:** a rock slot 2.5 m wide through the crest at z 228 to 232, floor 97, walls to the crest (105) or higher, 20 m long, bending north-west halfway so no straight view passes through. Silence starts inside (Hollis).
6. **The ledge:** a shelf on the west face, ground 98, x -6 to 4, from the cleft's west mouth (2, 232) north to the end of the path at (-2, 258), facing west. **The reveal is the step out of the cleft.** Stones to the right of the path end, to one side of the approach (G, DECISIONS 2026-09-25): (-4, 268), (1, 271), (-7, 272), 3.6 x 4 m, tops 110.
7. Length on paper: 32 + 340 legs + 42 platforms + 29 + 20 cleft + 26 ledge = **489 m, 196 s**; Camp to the Ward **585 m, 234 s**. Marlow's 455 m, 182 s (551 m, 220 s from camp) is the floor with no platforms; the build may run 5 percent over paper. Walk times are information, not a budget.

## 6. Answers to Edges.md
1. **East wind on night 1 (Vesper Q1):** keep it, change Style 6.3 for night 1 only. Anything hidden from the tower sits at most about 10 degrees above level from the ledge, so columns leaning over the player would show from the floor before the ledge. Night 1: a lit sheet streaming west, low; the burning valley below fills the half-frame test (6.3.5); flame tops about 6 degrees up, not 10 to 15 (6.3.2). 6.3.3 holds from night 2, when the wind turns and the smoke comes toward you. Edges 5 night: glow on the crest from night 2, not night 1.
2. **Headlight in the road cut on night 1 (Vesper Q2):** keep. One sweep, a car passing in the cut that never turns in, timed for the platforms' look-back. After night 1 the cut stays dark at night (gate cars still come by day, DECISIONS 2026-09-29). The last ordinary light, seen as the player turns toward the fire; its absence later is the wrong detail (Exit 8). Hollis aligns the road sound.

## 7. Open
1. G: the four lines marked G, and the east wind (3.2).
2. Sun: a west sun under about 30 degrees puts Camp 3, W1 and the west trails in the ridge's shadow all day 1. Vesper sets azimuth and elevation.
3. Vesper: Style 6.3.1 to 6.3.5 night-1 rewrite (6.1); the ledge frame from 98.
4. Marlow: headlight glow visible from P3 and P4 over the giants (unverified); every climb point 6 m under the crest; the 3 m rule at the knob.
5. Night walk: 234 s each night is 35 s over the as-built climb. Playtest whether the nightly walk drags; the platforms' events are the answer, not a shortcut.

Sable
