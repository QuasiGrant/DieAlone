# Facts on Grant's valley walk notes (Rook, 2026-09-30)

Read-only. Measured in the live Editor (Main3 open, Play mode, day-one look, Game view 3840 x 1976) and from the recipes and git. No scene, asset, code or setting was changed. Screenshots are in `shots/`. Positions are metres, x east, z north, heights absolute.

## 1. Day/night toggle in F1
- The switch is the LOOK section at the very bottom of the F1 panel: rows `Night`, `Day one`, `Day two`. There is no row called "day/night".
- At 3840 x 1976 the panel list is 3590 px tall inside a 1878 px viewport (canvas scale 2.74). The LOOK header sits at screen y -1339, 1388 px below the viewport's bottom edge. On open, the visible list ends at the `Store` warp (11 of 24 warps).
- Scrolling: no scroll bar is drawn. The mouse wheel cannot reach LOOK. On open the first row (`Graybox`) is selected, and `KeepSelectionVisible` (Assets/Scripts/Dev/DevMenu.cs) pulls the list back every frame so that row stays on screen. Measured: a scroll to the bottom was pulled back to 0.78 (the list moved 381 px, stopping at `Junction Jg`). LOOK is reachable only with arrow keys or the gamepad (27 presses down from the first row), or by first clicking into the list, which picks the clicked row (a warp or scene).
- Shots: `shots/f1_open.png` (what Grant sees on F1), `shots/f1_scrolled.png` (wheel at its limit), `shots/f1_bottom.png` (bottom forced by clearing the selection: LOOK rows, Day one current).

## 2. Warp to the Ward
DevWarps in Main3, panel order: Keepers Camp, Cabin, Tower Deck, Lake Pump, Lake Boathouse, Camp 1, Camp 2, Camp 3, Camp 3 Rim, Office, Store, Gate Booth, Closed Campground, Trailhead T, Junction Jg, Junction J, Junction W 1, Cave Mouth, Ward P 3, Ward P 4, **Ward**, Old Burn, Camp 2 Top, Cave Chamber.
- `Ward` (row 21) is at (-2, 98.2, 258) facing west: the path end on the ledge, stones to the right. It is below the fold (see 1). `Ward P 3` (39, 72, 291) and `Ward P 4` (33, 92, 204) are climb platforms.
- There is no separate Ward scene; the Ward is in Main3. The fire is always on (8.9j) and hidden by the W ridge from everywhere except the ledge.

## 3. Invisible walls
Colliders with no drawn renderer, by type:

| Type | Count | Where | Visible marker | Added |
|---|---|---|---|---|
| Thicket walls (off-trail stops) | 35 tile meshes, 1386 boxes up to 4 m each, 4 m over the highest ground within 3 m | both sides of every trail corridor (2.2 m from centre), round every clearing, lake landing, front zone, climb (1.1 m from centre), cleft (0.8 m), ledge | Gray rocks (RidgeStops, one mesh, 832 rocks, 2 per wall piece, so about 416 pieces) only where the wall stands at a face or drop of 2 m or more, on the climb and ledge, at the W ridge foot (x under 48) and near the N and S edges (z over 296, under 4). The rest, about 970 pieces, have no marker: the 0.3 m Marker_* meshes are still in the scene but their renderers were switched off by main3_8_9f_look.cs (8.9f, commit 51549e0). | 8.9a, raised 8.9b and 8.9e; climb, cleft and ledge walls 8.9j and 8.9k (287 boxes in the climb and ledge tiles); rocks 8.9k |
| Shift walls | 26 (SpurMouth, SpurMouthWest, TurningCircle0 to 23) | turning circle (384, 160) and spur mouth | none | 8.6; inactive (no shift system yet) |
| Edge walls | 2 (Bounds/Wall_North z 300, Wall_South z 0, each 400 m x 150 m) | map edges | rocks only where a thicket line runs near the edge | 8.1. 8.9j removed Wall_Cliff (x 10) and Wall_West (x 0) |
| Climb walls | counted in thicket (above) | benched legs, platforms, leg 5, cleft, ledge lip | rocks | 8.9j, cleft width 8.9k |
| Other: lake WadeLimit | 96 boxes, tops -5.0 or 0.9 m over the bank | 3 percent inside the shore ellipse | the water surface | 8.4 |
| Other: gate PlayerBlocker | 1, 0.6 x 60 x 5.2 m | gate opening (396, 170) | gate and fence visible | 8.6 |
| Other: CairnGate GateBlocker | 1, 0.6 x 3.4 x 6 m | Ward trail start (100, 206) | chain and post (chain has no collider) | 8.7, solid 8.9b |
| Other: boathouse PocketFill/Rail | 4 | boathouse sides | none | 8.9e re-walk |
| Other: tower RailStops | 38 | every flight and landing | tower rails | 8.9c |

## 4. Paths versus grass
- One terrain texture everywhere: Ground054, 3 m tiles. Trail and ground differ only by a tint (diffuse remap max): Layer_Ground (0.58, 0.65, 0.56), Layer_Trail (0.60, 0.59, 0.53), Layer_Burn (0.56, 0.57, 0.67). Recipe colours: floor #58583A, trail #5A4F36 (half floor, half wood), both in main3_8_9f_look.cs.
- Painted 1.2 m, soft to 1.8 m from the centre line (main3_8_3_trails_giants.cs).
- No edge objects and no step: the trail band is flattened to its centre-line profile for 1.5 m each side and blended over 2.5 m, so it meets the ground smoothly. Ground cover (grass, ferns, leaves) is kept 2.5 m off trail points in the open ground east of x 228; elsewhere there is little cover either side, so both read as bare ground.
- Shot: `shots/trail_jg_camp1.png`, standing on Jg to Camp 1 at (267.2, 5.8, 191.3) looking along it (yaw 4). The trail cannot be told from the ground.

## 5. Cut-off path at the lake pump
- The `Lake Pump` warp (190, -2.8, 99) faces south to the dock. Behind it, a straight trench runs north up the bank from the pump: the dock root notch, x 187.5 to 192.5, floor -4.5 at z 96 rising 0.5 per metre north, sides 0.7 per metre. It is made by main3_8_1_scene_ground.cs step 5 (lake bank) so the dock root sits at -4.5. It is not a trail. A thicket wall crosses it at z 98, 2 m from the pump, with a V of gray RidgeStops rocks (8.9k, because the wall stands at a 2 m+ bank there). The real trails leave the pump west (to W1) and east (to the boathouse); Camp to pump arrives from the north-west.
- Shots: `shots/pump_warp_view.png` (the warp view), `shots/pump_from_lake.png` (from over the water: the notch climbing straight up between the two rock lines), `shots/pump_topdown_walls.png` (top-down with the hidden markers switched on for the shot only).
- Other trail-like cuts that stop with no reason (terrain shaped by 8.1, not trails, crossed by thicket walls):
  1. Camp 2 view cut (8.1 step 9b, from 8.9a): a straight trench 6 m wide from (286.5, 102) to (240, 52), about 68 m, kept low so Camp 2 sees the boathouse. Walls cross it at (279, 94), (250, 63) and near (244, 56).
  2. Creek channel (8.1 step 8): J (104, 215) down to the Camp 3 hollow (84, 150), then (84, 128) to (100, 110) and on to the lake by W1 (136.5, 66). A low walkable-looking valley with no trail; walls cross it at (101, 204), (85, 152), (84, 128), (92 to 96, 114 to 119), (129, 75), (133, 71).
  All 13 trail centre lines were probed segment by segment: none is crossed by a wall except the cairn gate on J to Ward (by design, day only).

## 6. The store
- Built: FrontZone/Store at (366, 200), 8 x 5.6 m inside (8.4 x 6.0 m outside), 3.2 m high, gray box, doorway in the south wall. No interior, no sign. Office beside it 12 x 8 m.
- Main3.md: "Store (366, 200), 8 x 5.6 m, doorway at x 366; small, unstaffed, a lit cooler sign" (section 3.2 item 7); the office's Food action is the store.
- DECISIONS 2026-09-29: the store hosts a resident's minigame (buy a bottle or leave on spotting an anomaly); inside it can turn trippy and loop "like the Backrooms". No committed doc or decision says whether the interior is a building in Main3 or a separate scene. Open.

## 7. The road
- There is no road object outside the gate. The only paving is inside the fence: the Drive (x 373.5 to 396.5, z 170), the parking lot (x 343 to 373, z 150 to 190), turning circle, spur and closed campground loop. So from the gate the dark asphalt runs west up to the lot and office and nothing continues east.
- The E ridge (8.1, crest x 445, 45 to 60 m) is cut by a V at z 170: an 8 m flat at 3 m (road level) with 45 degree sides, flat at 3 m straight on east to x 600. The cut floor is plain Layer_Ground terrain, not a road. Road sound is in DECISIONS (ordinary traffic on day 1) but nothing is built.
- From the office front (350, 3.2, 194) looking east (yaw 103): lot, lamp and fence, then the E ridge closing the view on both sides with one V notch at the gate line and a pale far range in it; no highway visible. The E ridge stands 49 m past the fence and rises 16 m by (445, 150). Shot: `shots/office_to_road.png`.
- Main3.md (rev 16) and Valley.md: "the road comes from off-map to the gate and never enters"; "a straight road the player sees but never walks"; Edges.md 4: the road drops into the cut and curves out of sight within 150 m. The seen road is on paper only.

## 8. Trees
Playable area x 0 to 400, z 0 to 300. No terrain trees (TreeInstance count 0) and no terrain grass details; every tree is a scene object.

| Group | Count | Size | Where | Recipe |
|---|---|---|---|---|
| Giants (pack, SliceLook/GiantTrees) | 41: 35 Sequoia, 4 RedPine, 2 Tree_Dead | 15 to 50 m | on the 38 gray giant spots plus 3 heroes (gray stand-ins hidden); 15 south of z 100, 11 between, 15 north of z 200 | spots main3_8_3_trails_giants.cs; pack trees main3_8_9f_look.cs |
| Camp edge trees | 18: 13 RedFir, 5 RedPine | pines | camp clearing edge | main3_8_9d_dress_camp.cs |
| Burn regrowth | 245: 136 RedFir, 83 RedPine, 26 dead snags (plus 174 fallen branches) | 4 to 6 m | old burn only, x 186 to 338, z 145 to 210 | main3_8_9f_look.cs |
| Open ground firs | 277 RedFir | 2 to 4 m | x 228 to 338, z 100 to 297 | main3_8_9f_look.cs |
| Outside the map | 86 East edge forest (x 400 to 430); 35 burning giants west of the ridge | | | main3_8_9f_look.cs, main3_8_7_ward.cs |

- Total in the playable area: 581, of which 41 are big. West of x 186 there are only the giants and the camp edge trees.
- rev16 versus now (from git, not a scene count; main3-rev16 was not rebuilt): the regrowth, open-ground and camp tree code is the same. Giants: BuildNotes records 43 at 8.9e (cliff-edge band plus 7 groves); 8.9j removed the band (Valley.md 1.2), now 38 plus 3 heroes. East edge forest ran x 404 to 446 at rev16, now 404 to 430 (three columns instead of four). The small tree counts at rev16 are not recorded; unverified.

## 9. North third (z 200 to 300)
- Now: flat valley floor at 3 to 10 m from x 80 to 400. On it: Camp 1 (282, 238), the latrine shed (264, 207), the closed campground loop (372, 262), two giant groves (x 236 to 275, z 255 to 277; x 103 to 141, z 265 to 292), two giants near (183, 223), the open-ground firs east of x 228. West of x 80: the W ridge face with the four climb legs (x 33 to 72), the platforms, the Ward knob and ledge. Map edge wall at z 300; the N ridge crest is at z 350 (50 to 103 m), 50 m past the wall, rising from about 20 m at z 320.
- rev16: the Wall (rock escarpment, crest 88, from (55, 190) to (115, 270)), the pass (112, 262) at 78, the Ward plateau at 70 to 72 behind it with the stones (16 to 21, 263 to 270), and the switchback climb on the Wall face (Main3.md rev 16 table 2.1). 8.9j removed the Wall, pass and plateau and did not put anything in their place on the floor, so x 80 to 230 north of z 210 is now open flat ground with one grove.

## 10. Lighting, day one
Current (Assets/Settings/LookTuning_DayOne.asset, applied from the first frame):
- Sun: #FFC98A, intensity 1.1, elevation 32, bearing 200 (from SSW), soft shadows.
- Ambient (the "fill"): flat #998A73.
- Crushed blacks 0.15, dark corners 0.3, wash out 0.25.
- Fog: linear #A8A08E, 40 to 600 m.
- Exposure: none. No Volume in the scene, camera post-processing off; brightness is sun, ambient and the look filter only.
- Cabin: InteriorFill point light, 3, #998A73.

Changes since the evening Grant said day one was "way too dark" (values before 8.9g from commit 5564e8b):

| Value | Then | Now | Commit |
|---|---|---|---|
| Sun elevation | 14 | 32 | a14af63 (8.9g) to 28, 4e1415d (8.9j) to 32 |
| Sun bearing | 270 (west) | 200 | 4e1415d (8.9j) |
| Ambient | #6E6658 | #998A73 | a14af63 |
| Crushed blacks | 0.3 | 0.15 | a14af63 |
| Dark corners | 0.45 | 0.3 | a14af63 |
| Cabin fill | none | 3 | 251c410 |
| Fog, sun intensity, sun colour | 40 to 600, 1.1, #FFC98A | same | |

- Also since then: 8.9j put the map in an open valley and removed the cliff-edge giant band, so most of the floor now has no canopy over it.

## 11. The climb as built
Measured on Trails/J to Ward (249 points, 2 m apart), J (104, 10, 206) to the path end (-2, 98, 258):

| Piece | Length | Rise | Grade | Turn after |
|---|---|---|---|---|
| J west to leg 1 | 32.0 | 2.0 | 6 percent | 90 right, north |
| Leg 1, x 72 north | 86.0 | 20 | 23.2 (max 24) | 180 via P1 |
| P1 (13.4 m level) | 13.4 | 0 | 0 | |
| Leg 2, x 59 south | 86.0 | 20 | 23.2 | 180 via P2 |
| P2 | 13.4 | 0 | 0 | |
| Leg 3, x 46 north | 86.0 | 20 | 23.2 | 180 via P3 |
| P3 | 13.4 | 0 | 0 | |
| Leg 4, x 33 south | 86.0 | 20 | 23.2 | 108 into leg 5 at P4 |
| Leg 5 (dogleg west, then north-west) | 7.8 + 23.7 | 3.0 | 10 (max 15) | 74 left |
| Cleft part A, west | 15.4 | 0 | 0 | 90 right |
| Cleft part B, north | 7.6 | 0 | 0 | 90 left |
| Exit gap, west | 3.6 | 0.6 | 16 | 83 right |
| Ramp to the path end | 18.0 | 2.3 | 13 | |
| Total | 492 | 88 | | |

- The four legs are straight lines of identical length and grade on a 4 m bench, joined by three identical 180 degree hairpins; walls keep the walkway 2.2 m wide. 8.9j timed J to ledge at 195.6 s (commit 4e1415d); Valley.md 5.7 gives 489 m, 196 s, Camp to Ward 585 m, 234 s.

Rook
