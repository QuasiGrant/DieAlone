# Cave rock: placement spec (PLAN 8.27a)
**DRAFT 1, 2026-10-03, Vesper.** Replaces Layout827/RockBreakup (main3_8_27_cave.cs, the rockSet list). Built from the geometry in main3_8_8_cave.cs, main3_8_17_cave.cs and main3_8_27_cave.cs, and the strips and props in CaveLayout.md draft 2. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.

## Why round 2 failed
Rook's pieces were 1.3 to 4.5 m across, in an 18 x 18 x 8 m box, and stood clear of the walls. They read as boulders in a room. The walls stayed flat, tiled rectangles. Here the rock *is* the wall: owned BK boulders at or above their native 6 m, sunk mostly behind the wall faces, overlapping, so no flat box face wider than 2 m shows from any walk line.

## Owned meshes used (AssetCatalogue.md, paths checked 2026-10-03)
- **BK/PureNature_Redwood/Prefabs/Rocks/BigBoulders_0-5** (native about 6 x 6 m) and **Boulder_0-5** (native about 6 x 4 m, flatter). Every rock below is one of these.
- **BK Prefabs/Rocks/RubbleSparse_1-3** (5 to 7 m wide, 0.8 high, no collider): floor breakup only.
- **Assets/Prefabs/Forest/Valley_Aspen1Leafless** (5 to 6 m, already on the map): the bulb tree.
- **Not used:** Effigy GameWorks. The catalogue's "Rock and DarkRock 30 x 9 outcrops" are only found on disk as StoneMenhir standing stones and stone circles. Unverified as outcrops; they read as ritual stones. Celestia Tree_Dead: PSX style, and today it is squashed non-uniform (1 x 3 x 1). Campsite CS_Rock: 256 px texture, too soft next to BK.
- **Gap:** no owned cave-wall or cliff-face mesh. Boulders are the only rock, so the method below makes them do the job.

## Method (every piece)
1. Spawn the prefab, turn it to **yaw** (world, degrees), and scale it uniformly so the largest side of its mesh bounds equals **S** (m).
2. Position it by its mesh bounds, not its pivot:
   - Along the wall: the bounds centre sits at the given **along** coordinate.
   - Height: the bounds centre sits at **y**. For a ceiling piece, the bounds bottom sits at **bottom** instead.
   - Depth: the bounds face nearest the room sits **d** m inside the room from the wall face. A small d means the rest of the rock is behind the wall, inside the box collider.
3. **Material:** Places_CaveBoulder (boulderHex #6E6862), as in round 2.
4. **Collider:** if the bounds bottom is less than 2.5 m above the local floor, the piece gets a convex hull from LOD0, as in round 2. Otherwise it has none.
5. **NARROWS** marks a piece that takes width off a walk line or an opening. Rook's flood, walk and reach checks decide: if one fails, cut d to 0.1, and if that still fails, drop the piece. Do not move it elsewhere.
6. Nothing goes into a keep-clear strip, onto the cable, or over a prop footprint (listed per area).

## A. Entrance passage
Box x 50.5 to 53.5, z 20.5 to 37.4, floor -6, ceiling -2. Leg 1 opens in the east wall at z 20.5 to 23.5 (floor falling east, ceiling 3.5 m over it).
Keep clear:
- the drip can (51.2, 28) and 1 m round it;
- the day-2 boards x 50.55 to 51.35, z 34.0 to 37.2;
- the load-down spot x 52.0 to 53.4, z 35.0 to 37.0;
- the niche opening, east wall z 31.6 to 33.4, floor to -4;
- the cable along the east wall's foot.

| # | Prefab | S | yaw | Wall (face) | along | y / bottom | d | Masks |
|---|---|---|---|---|---|---|---|---|
| P1 | BigBoulders_1 | 4.0 | 20 | SW corner: W face x 50.5 and S face z 20.5 | x 51.0, z 21.0 (bounds centre) | y -4.2 | 0.8 off both faces | rounds the dead-end corner so the space visibly bends east. **NARROWS** the turn |
| P2 | BigBoulders_5 | 2.6 | 330 | E face x 53.5, north jamb of leg 1 | z 24.6 | y -4.4 | 0.4 into the passage; its south face no further south than z 23.2 | a rock lip on the opening's north edge, catching light from the passage. **NARROWS** leg 1's mouth (3.0 to 2.7) |
| P3 | Boulder_2 | 3.5 | 0 | over leg 1's mouth, x 53.5 to 55.0 | z 22.0 | bottom -2.8 | (ceiling) | breaks the opening's flat top edge. No collider |
| P4 | BigBoulders_2 | 3.0 | 70 | S face z 20.5, x 53.0 to 56.0 | x 54.5 | y -5.0 | 0.4 | the south jamb, so the opening has two rock edges, not a slot. **NARROWS** leg 1 |
| P5 | BigBoulders_3 | 4.0 | 110 | W face x 50.5 | z 25.0 | y -4.0 | 0.4 | west wall, first 5 m from the turn |
| P6 | BigBoulders_4 | 4.0 | 250 | W face x 50.5 | z 31.0 (bounds z 29.0 to 33.0) | y -4.0 | 0.4 | west wall between the drip and the boards |
| P7 | BigBoulders_0 | 3.0 | 200 | E face x 53.5 | z 30.0 (bounds z 28.5 to 31.5) | y -4.5 | 0.4 | the niche's **south jamb**: a projecting rock edge just before the recess, so the recess reads as a hole behind it. Do not cover the cable: its bottom at -5.6 or higher, so the cable at the foot shows |
| P8 | Boulder_4 | 3.5 | 160 | ceiling | x 52.0, z 24.5 | bottom -2.5 | (ceiling) | ceiling, over the turn |
| P9 | Boulder_1 | 3.5 | 300 | ceiling | x 52.0, z 29.5 | bottom -2.5 | (ceiling) | ceiling, middle |
| P10 | Boulder_2 | 3.0 | 40 | ceiling | x 52.0, z 32.5 | bottom -2.4 | (ceiling) | ceiling, over the niche |

**Niche (material, not placement):**
- Every niche face (Niche/Wall_E_S, Wall_E_N, Wall_E_Over and the niche's back, floor and ceiling) takes the cave rock material, never Places_CaveVoid. Today Niche/Wall_E_N is void (main3_8_27_cave.cs line 360), which is why the recess reads as flat dark wall.
- The void stays only on the mouth's first 4 m, z 33.4 to 37.4, outside the niche.

**F3 reframe:** from (52.0, 24.0) heading 180, the opening is beside the eye, not ahead of it. Move F3 to (52.0, 27.5) heading 165, so the turn and P1 to P4 sit in the left half of the frame.

## B. Chamber
Box x 71 to 89, z 3 to 21, floor -18, ceiling -10. The west opening is at z 10.5 to 13.5, 3.5 m high (top -14.5). The east doorway is at z 11.4 to 12.6, 2.1 m high (top -15.9).
Keep clear:
- the strip z 10.8 to 13.2, wall to wall;
- the seat shelf x 87.8 to 89.0, z 5 to 7, with R7 (capsule x 88.2 to 88.6, z 5.8 to 6.2) and the talk stand (86.9, 6.2);
- sleep x 84 to 86, z 3.5 to 4.5, and the heater (83.4, 4.0);
- food x 72.5 to 73.5, z 14.0 to 15.5;
- the stack x 85 to 88.5, z 17 to 20.5, and the battery bank x 84 to 85, z 20.2 to 20.8;
- the cable at the north wall's foot;
- the bulb string (86.5, -15.1, 12.3) to the doorway.

**Remove** all 17 round-2 chamber pieces in RockBreakup.

### B1. Low band (reachable; all hulled)
| # | Prefab | S | yaw | Wall (face) | along | y | d | Masks |
|---|---|---|---|---|---|---|---|---|
| L1 | BigBoulders_1 | 6.0 | 15 | S, z 3 | x 74.0 | -15.8 | 1.0 | south-west corner, wraps onto the west wall |
| L2 | BigBoulders_3 | 5.5 | 95 | S, z 3 | x 78.5 | -16.0 | 0.9 | south wall |
| L3 | Boulder_2 | 5.0 | 200 | S, z 3 | x 82.8 | -16.2 | 0.3 | south wall behind the heater (heater 1.0 off the face; 0.7 left) |
| L4 | BigBoulders_4 | 6.0 | 140 | S, z 3 | x 87.6 | -15.8 | 0.3 | south-east corner, wraps onto the east wall; stays south of z 4.6 on the east face, clear of the shelf |
| L5 | BigBoulders_2 | 6.0 | 220 | W, x 71 | z 6.0 | -15.8 | 1.0 | west wall south of the opening |
| L6 | BigBoulders_5 | 4.5 | 300 | W, x 71 | z 8.0 (bounds z 5.75 to 10.25) | -16.3 | 0.8 | the opening's south jamb: a rock edge, not a box edge |
| L7 | BigBoulders_0 | 4.5 | 30 | W, x 71 | z 15.8 (bounds z 13.55 to 18.05) | -16.3 | 0.4 | the opening's north jamb, behind the food (food 1.5 off the face) |
| L8 | BigBoulders_3 | 6.0 | 175 | W, x 71 | z 19.2 | -15.8 | 0.9 | north-west corner, wraps onto the north wall |
| L9 | BigBoulders_1 | 6.0 | 250 | N, z 21 | x 75.5 | -15.6 | 0.2 | north wall. Bottom at -17.4 or higher, so the cable at the foot stays in sight |
| L10 | BigBoulders_4 | 5.5 | 35 | N, z 21 | x 80.5 | -15.8 | 0.2 | north wall, same cable rule |
| L11 | BigBoulders_3 | 6.0 | 175 | NE corner: N face z 21 and E face x 89 | x 87.8, z 20.0 (bounds centre) | -14.8 | 0.2 off both faces | behind and above the stack. Bottom at -15.0 or higher (stack top -15.0), so it sits on the stack's line, not in it |
| L12 | BigBoulders_0 | 5.0 | 60 | E, x 89 | z 6.0 | -14.9 (bottom at -17.4, the shelf top) | 0.3 | **his rock**: the shelf becomes the foot of a rock mass rising behind R7. Rook checks R7's capsule clears it by 0.1 or more |
| L13 | BigBoulders_5 | 4.5 | 280 | E, x 89 | z 8.4 (bounds z 6.15 to 10.65) | -16.3 | 0.6 | east wall between the shelf and the doorway; 1.5 m from the talk stand |
| L14 | BigBoulders_2 | 4.5 | 45 | E, x 89 | z 15.6 (bounds z 13.35 to 17.85) | -16.3 | 0.4 | the doorway's north jamb. Face at x 88.6, clear of the stack (x 88.5) by 0.1. **NARROWS** the way to the stack |
| L15 | BigBoulders_1 | 3.0 | 10 | E, x 89 | z 9.3 (bounds z 7.8 to 10.8) | -16.6 | 0.3 | south of the doorway, stopping at the strip's edge (10.8). The 0.6 m of flat wall between it and the door is left. The strip rules |

### B2. High band and lintels (over reach; no colliders)
| # | Prefab | S | yaw | Wall | along | y / bottom | d | Masks |
|---|---|---|---|---|---|---|---|---|
| H1 | Boulder_0 | 7.0 | 0 | S | x 75.5 | y -12.0 | 0.8 | upper south wall |
| H2 | Boulder_3 | 7.0 | 180 | S | x 83.5 | y -11.8 | 0.8 | upper south wall |
| H3 | Boulder_1 | 7.0 | 90 | W | z 5.5 | y -11.5 | 0.8 | upper west wall |
| H4 | Boulder_4 | 5.0 | 90 | W | z 12.0 | bottom -14.3 | 0.5 | over the west opening: its top edge becomes rock. Over the strip, but 3.7 m above its floor; if the strip is meant as a full-height volume, drop H4 |
| H5 | Boulder_5 | 7.0 | 270 | W | z 18.5 | y -11.5 | 0.8 | upper west wall |
| H6 | Boulder_2 | 7.0 | 0 | N | x 76.0 | y -11.5 | 0.9 | upper north wall |
| H7 | Boulder_0 | 7.0 | 180 | N | x 84.0 | bottom -13.8 or higher | 0.9 | upper north wall, over the stack |
| H8 | Boulder_5 | 7.0 | 90 | E | z 6.0 | bottom -14.2 or higher | 0.8 | over his rock (L12): one mass from shelf to ceiling |
| H9 | Boulder_3 | 4.5 | 90 | E | z 12.0 | bottom -15.5 | 0.4 | over the east doorway. 0.4 m of the lintel box still shows under it. The bulb string passes about -15.8 at the rock's face (x 88.6): Rook checks 0.2 or more of clearance |
| H10 | Boulder_1 | 7.0 | 270 | E | z 18.0 | y -11.5 | 0.8 | upper east wall |

### B3. Ceiling (no colliders)
Ceiling face -10; bottom is the lowest point of the rock.

| # | Prefab | S | yaw | x, z | bottom |
|---|---|---|---|---|---|
| C1 | Boulder_0 | 8.0 | 75 | 75.5, 7.0 | -11.0 |
| C2 | Boulder_2 | 8.0 | 10 | 82.5, 7.5 | -11.2 |
| C3 | Boulder_4 | 7.0 | 130 | 76.5, 16.5 | -10.8 |
| C4 | Boulder_1 | 8.0 | 200 | 84.0, 16.0 | -11.0 |
| C5 | Boulder_3 | 6.0 | 45 | 80.0, 12.0 | -10.6 |

### B4. Floor (breaks the flat plane)
- RubbleSparse_1, 2, 3, 1, 2, each scaled to 3.0 m wide and flattened to 0.25 m high (y scale only).
- Placed at (73.2, 5.0), (79.0, 4.4), (72.4, 8.6), (77.5, 18.9), (82.0, 18.6), yaws 0, 70, 140, 210, 280.
- All at the wall feet, outside the strip and every footprint, and 1.2 m or more from the north wall so the cable stays clear. No collider; Rook's walk-into check decides. If they fail, drop to 0.15 m high.
- With L1 to L15 bulging 0.2 to 1.0 m in, the floor's edge is no longer a straight line at any wall.

## C. V9 doorway and dead end
- **Side room:** x 89.5 to 97.5, z 7.5 to 15.5, floor -18, 3 m high. V9 opens in its east wall (face x 97.5) at z 13.2 to 14.4, 2.1 m high.
- **Deeper passage:** east to x 101.6, then north to z 16.1, 1.2 m wide. The DeadEnd slab is at z 15.8 to 16.1, x 100.4 to 101.6. Inspect point (101.0, 14.0) facing 0.

| # | Prefab | S | yaw | Wall | along | y / bottom | d | Masks |
|---|---|---|---|---|---|---|---|---|
| V1 | BigBoulders_2 | 2.4 | 30 | side room E, x 97.5 | z 12.4 (bounds 11.2 to 13.4) | y -16.8 | 0.5 | south jamb; overlaps the opening 0.2. **NARROWS** V9 (1.2 to 1.0 with V2) |
| V2 | BigBoulders_4 | 2.4 | 200 | side room E, x 97.5 | z 15.2 (bounds 14.0 to 16.4) | y -16.8 | 0.4 | north jamb; overlaps the opening 0.2. **NARROWS** |
| V3 | Boulder_3 | 3.0 | 90 | side room E, x 97.5 | z 13.8 | bottom -16.0 | 0.4 | lintel: the opening's top edge becomes rock. Headroom 2.0. **NARROWS** height; hulled |
| V4 | Boulder_5 | 2.5 | 0 | dead end, end face z 15.8 | x 101.0 (bounds wider than the passage, sunk into both side walls) | floor to ceiling | 0.15 in front of the DeadEnd slab | the end face is rock. **No collider.** The DeadEnd slab keeps its box and `Examine`, its renderer off, its face moved to 0.05 m in front of V4's nearest point (the rock is its visible reason). The inspect ray must still meet DeadEnd within 2.0 m |
| V5 | BigBoulders_0 | 2.0 | 120 | outer corner of the turn: E face x 101.6 and S face z 13.2 | x 101.2, z 13.6 (bounds centre) | y -17.0 | 0.2 off both faces | rounds the turn. **NARROWS** |
| V6 | Boulder_1 | 3.0 | 0 | deeper passage ceiling | x 99.5, z 13.8 | bottom -16.05 (0.15 hang) | (ceiling) | breaks the flat lid. **NARROWS** height; Rook confirms body height |

**Shut days:**
- Today DeeperClosed is a flat panel inside the new rock frame. Give it a child, Boulder_2 at S 1.6, yaw 0, filling the opening with its face 0.1 m behind the wall face (x 97.6), so it toggles with DeeperClosed.
- DeeperClosed's renderer goes off; its box stays as the stop, and the boulder is its visible reason.

## D. Bulb tree (V2)
- **Remove** POI_Coloured_bulbs/DeadTree (Tree_Dead at 1 x 3 x 1) and the hidden DeadBranch.
- **Tree:** Assets/Prefabs/Forest/Valley_Aspen1Leafless, unscaled, yaw 40, foot on the ground 1.8 m north of the tread point (82.15, 52.96), at the bulbs' position today. Its collider is the prefab's own.
- **NARROWS:** check against "no trunk within 1.5 m of the tread west of x 100" (CaveLayout 4). If it fails, move the foot north in 0.25 m steps up to 2.5 m, never onto the drop side.
- **String:** 14 bulbs on 4 runs: red, amber, green, blue, repeating.
  - Run 1 winds the trunk from 1.2 to 3.0 m (three turns).
  - Runs 2 to 4 swag from the first fork to the three lowest branch tips, sagging 0.3.
  - No bulb lower than 2.3 m over the tread. None over the tread centre line.
- **Bulb:** the same mesh and Places_BulbGlow-family emissive as the side-room string, scaled to 0.14 m, with no collider.
- **Day-two read:** in CaveFrames_Day_two_BulbsTrail (F2, (91.3, 47.4) heading 300), at least six bulbs show, each 40 grey or more over the frame mean (the Gate.md night N1 bar). The look values are set by the coder in LookTuning; I propose the emissive only, not a light. Day one: the tree stands bare, with no string.

## E. Checks Rook runs after the build
1. Area flood, traps, walk-into and collider size (main3_area_check), plus the 8.27 cave check: strips, talk reach, side room, deeper, narrow, prompts.
2. The NARROWS list above:
   - Entrance: P1, P2, P4.
   - Chamber: L14; clearances for L3 and L4 (0.2 from sleep), L12 (0.1 from R7) and H9 (0.2 from the bulb string).
   - V9: V1, V2, V3, V5, V6.
   - Bulb tree: the trunk rule.
3. Frames for my round: F3 reframed; Found_06 (niche); F4 and Found_08 (chamber); CaveFrames V9 open, V9 shut and the dead end; CaveFrames_Day_two_BulbsTrail.
   - Pass bar: no flat box face over 2 m wide at eye height in any of them, and the niche, his rock, both doorways and the bulbs read without labels.

Vesper
