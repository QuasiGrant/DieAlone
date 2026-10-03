# 8.27 paper check, round 1: CaveLayout.md and CaveLayout.svg (Sable, draft 1), with the S1 to S8 samples
2026-10-02, Marlow. **FAIL** (1 block, 10 hurts, 9 cosmetic). No Editor calls (Rook has it).

**Sources, read at a4dcf39:**
- Main3.unity, parsed for world transforms and every scene collider. Prefab-internal colliders were read from the prefabs.
- Main3_TerrainData.asset, decoded (check (392, 165) = 3.000). Terrain holes were not decoded; they are taken from main3_8_8_cave.cs: x 50.3 to 53.7, z 33.6 to 37.4.
- FBX LOD0 vertices for every BK rock and CS stone in x 38 to 115, z -2 to 75 (64 rocks), and for Chair, Table and C_Crate_Small_1.

**Assumptions:**
- Controller: radius 0.35, jump 0.6, climb top 0.70, slope 45, reach 2 m.
- Deck eyes: 64 at 57.6 and 64 at 58.2, over x 160.5 to 167.5, z 162.5 to 169.5.
- Crowns are modelled as cones from FBX bounds, which is unverified. Every deck line here is already under the terrain, so crowns do not change any result.

**The ravine, as sampled:**
- **Floor:** an east-west trench at -6.0, from x 40 to about x 80, between z 36 and 44.
- **North wall:** rises from about -2 at z 50 to the rim at 17 to 18 (z 58 to 64, x 54 to 80). The steepest part is z 52 to 56, 10 m in 4 m.
- **South wall:** rises from -5.9 at z 37 to 0 at z 33 (about 60 degrees, 6 m), up to the flat plate over the cave (0 from z 32 south, x 44 to 92).
- **The spur stays on the bank.** It does not go down the ravine floor. From the rise (77.3, 48.6, +2.1) it runs west along the north bank, 4.5 to 6 m north of a 71 to 78 degree drop to the floor, and reaches the floor only at the mouth.
- **Access:** a flood fill (44 degree cells, terrain only) finds no way from the ravine floor to the lowland, the rim or the plate, except within 3 m of the spur.
- **Terrain over the cave:** 0 over every cave piece, and -1 to -2 over the side room and V9.

## Samples (ground heights, m; terrain only)
**S1. The rim north of the mouth and the ridge over the descent.** (74, 60) = **18.00**.

| z \ x | 50 | 52 | 54 | 56 | 58 | 60 | 62 | 64 | 66 | 68 | 70 | 72 | 74 | 76 | 78 | 80 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 66 | 11.8 | 13.9 | 15.0 | 15.4 | 15.7 | 16.0 | 16.2 | 16.5 | 16.7 | 17.0 | 17.1 | 16.8 | 16.0 | 15.1 | 14.1 | 12.9 |
| 64 | 13.1 | 15.3 | 16.8 | 17.0 | 17.2 | 17.4 | 17.6 | 17.7 | 17.8 | 17.9 | 17.9 | 17.8 | 17.4 | 16.8 | 16.0 | 15.1 |
| 62 | 13.9 | 16.0 | 17.7 | 17.9 | 18.0 | 18.0 | 18.0 | 18.0 | 18.0 | 18.0 | 18.0 | 18.0 | 18.0 | 17.8 | 17.4 | 16.8 |
| 60 | 14.1 | 15.9 | 17.8 | 18.0 | 18.0 | 18.0 | 18.0 | 17.9 | 17.8 | 17.7 | 17.9 | 18.0 | 18.0 | 18.0 | 18.0 | 17.8 |
| 58 | 14.0 | 15.5 | 16.7 | 17.2 | 17.2 | 17.0 | 16.7 | 16.3 | 15.9 | 15.7 | 16.0 | 17.2 | 17.9 | 18.0 | 18.0 | 18.0 |
| 56 | 13.5 | 14.0 | 14.7 | 14.9 | 14.7 | 14.3 | 13.6 | 13.0 | 12.3 | 11.5 | 12.1 | 14.1 | 16.0 | 17.2 | 12.6 | 7.0 |
| 54 | 10.8 | 10.7 | 11.2 | 11.2 | 10.6 | 9.8 | 8.8 | 7.8 | 6.8 | 5.8 | 7.0 | 9.6 | 12.0 | 8.8 | 4.0 | 1.1 |
| 52 | 6.5 | 5.9 | 6.2 | 2.4 | 0.4 | -0.3 | -0.1 | 0.3 | 0.9 | 0.2 | 1.8 | 4.3 | 6.6 | 3.5 | 1.5 | 1.2 |
| 50 | 1.6 | 0.9 | -1.5 | -2.7 | -2.9 | -2.5 | -2.1 | -1.8 | -1.7 | -2.1 | -2.0 | -0.5 | 1.9 | 2.1 | 1.9 | 1.6 |
| 48 | -2.8 | -3.5 | -3.7 | -3.4 | -3.1 | -2.6 | -2.0 | -1.6 | -1.1 | -0.8 | -0.9 | -0.9 | 1.2 | 1.9 | 2.1 | 2.2 |
| 46 | -5.5 | -4.7 | -4.0 | -3.8 | -3.4 | -3.0 | -2.2 | -1.4 | -0.9 | -0.5 | 0.0 | 0.6 | 1.2 | 1.6 | 1.7 | 0.8 |
| 44 | -5.9 | -4.8 | -4.4 | -4.2 | -4.4 | -5.0 | -4.1 | -2.8 | -1.4 | -0.4 | 0.2 | 0.6 | 1.0 | 1.2 | -0.5 | -3.7 |

**S2. Ravine floor, (62, 48) to (52, 38), every 1 m.** Offsets are perpendicular to the line: + to the south-east (the ravine floor side), - to the north-west (the bank). This line is the spur's own corridor; the floor proper is the -6.0 cells.

| s | point | -6 | -5 | -4 | -3 | -2 | -1 | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 0 | (62.0,48.0) | 1.4 | -1.2 | -2.4 | -2.6 | -2.4 | -2.2 | -2.0 | -1.8 | -1.6 | -1.4 | -1.3 | -1.4 | -1.6 |
| 1 | (61.3,47.3) | -0.5 | -2.3 | -2.8 | -2.7 | -2.6 | -2.4 | -2.2 | -2.0 | -2.0 | -2.1 | -2.3 | -2.6 | -2.9 |
| 2 | (60.6,46.6) | -1.7 | -2.9 | -3.0 | -2.9 | -2.8 | -2.6 | -2.5 | -2.6 | -2.8 | -3.1 | -3.5 | -4.1 | -4.9 |
| 3 | (59.9,45.9) | -2.4 | -3.2 | -3.2 | -3.1 | -3.0 | -3.0 | -3.2 | -3.5 | -3.9 | -4.8 | -5.1 | -6.0 | -6.0 |
| 4 | (59.2,45.2) | -2.9 | -3.4 | -3.4 | -3.4 | -3.3 | -3.4 | -3.9 | -4.5 | -5.2 | -6.0 | -6.0 | -6.0 | -6.0 |
| 5 | (58.5,44.5) | -3.3 | -3.6 | -3.6 | -3.6 | -3.6 | -3.8 | -4.3 | -4.9 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 |
| 6 | (57.8,43.8) | -3.7 | -3.8 | -3.8 | -3.8 | -3.9 | -4.0 | -4.4 | -4.9 | -5.4 | -6.0 | -6.0 | -6.0 | -6.0 |
| 7 | (57.1,43.1) | -4.1 | -4.0 | -4.0 | -4.1 | -4.2 | -4.3 | -4.5 | -4.9 | -5.3 | -6.0 | -6.0 | -6.0 | -6.0 |
| 8 | (56.3,42.3) | -4.5 | -4.4 | -4.2 | -4.3 | -4.4 | -4.5 | -4.6 | -4.9 | -5.3 | -5.7 | -6.0 | -6.0 | -6.0 |
| 9 | (55.6,41.6) | -5.0 | -4.7 | -4.5 | -4.5 | -4.6 | -4.7 | -4.8 | -5.0 | -5.3 | -5.6 | -5.9 | -6.0 | -6.0 |
| 10 | (54.9,40.9) | -5.6 | -5.1 | -4.8 | -4.7 | -4.8 | -4.9 | -5.1 | -5.2 | -5.4 | -5.6 | -5.8 | -6.0 | -5.8 |
| 11 | (54.2,40.2) | -5.9 | -5.4 | -5.1 | -5.0 | -5.1 | -5.2 | -5.3 | -5.4 | -5.5 | -5.6 | -5.8 | -5.8 | -5.1 |
| 12 | (53.5,39.5) | -6.0 | -5.6 | -5.4 | -5.3 | -5.3 | -5.4 | -5.5 | -5.6 | -5.6 | -5.7 | -5.8 | -5.0 | -3.8 |
| 13 | (52.8,38.8) | -6.0 | -6.0 | -5.7 | -5.6 | -5.6 | -5.7 | -5.7 | -5.8 | -5.8 | -5.8 | -5.5 | -4.2 | -2.3 |
| 14 | (52.1,38.1) | -6.0 | -6.0 | -5.9 | -5.9 | -5.8 | -5.8 | -5.9 | -5.9 | -5.9 | -5.8 | -5.0 | -3.3 | -1.0 |

**S3. The narrow strip (V10), along the tread from x 72 to 61, every 1 m.** Offsets are from the tread centreline: + south (toward the ravine), - north.

| tread point | -6 | -5 | -4 | -3 | -2 | -1 | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| (72.0,44.8) | 2.2 | 0.4 | -0.6 | -0.3 | 0.4 | 0.6 | 0.6 | 0.6 | 0.3 | -0.8 | -2.6 | -6.0 | -6.0 |
| (71.2,44.9) | 1.5 | -0.6 | -1.4 | -0.8 | 0.1 | 0.4 | 0.4 | 0.4 | 0.2 | -0.9 | -2.6 | -5.8 | -6.0 |
| (70.5,45.1) | 1.6 | -0.4 | -1.4 | -0.8 | 0.0 | 0.2 | 0.2 | 0.2 | -0.1 | -1.2 | -2.9 | -5.5 | -6.0 |
| (69.6,45.4) | 1.4 | -1.0 | -1.7 | -1.1 | -0.3 | 0.0 | 0.0 | 0.0 | -0.3 | -1.3 | -3.0 | -5.6 | -6.0 |
| (68.7,45.7) | 1.2 | -1.4 | -1.9 | -1.4 | -0.5 | -0.3 | -0.3 | -0.3 | -0.5 | -1.5 | -3.1 | -5.2 | -6.0 |
| (67.9,46.1) | 1.0 | -1.3 | -2.0 | -1.5 | -0.7 | -0.5 | -0.5 | -0.5 | -0.7 | -1.7 | -3.3 | -5.5 | -6.0 |
| (67.1,46.4) | 0.9 | -1.6 | -2.2 | -1.7 | -0.9 | -0.7 | -0.7 | -0.7 | -0.9 | -1.9 | -3.4 | -6.0 | -6.0 |
| (66.4,46.7) | 0.8 | -1.5 | -2.2 | -1.8 | -1.1 | -0.9 | -0.9 | -0.9 | -1.1 | -2.0 | -3.5 | -5.6 | -6.0 |
| (65.7,47.0) | 1.5 | -0.7 | -1.8 | -1.7 | -1.2 | -1.1 | -1.1 | -1.1 | -1.3 | -2.2 | -3.6 | -5.7 | -6.0 |
| (65.0,47.3) | 3.6 | 0.9 | -1.2 | -1.7 | -1.5 | -1.3 | -1.3 | -1.3 | -1.4 | -2.3 | -3.6 | -6.0 | -6.0 |
| (64.2,47.6) | 4.6 | 2.1 | -0.9 | -1.7 | -1.6 | -1.5 | -1.5 | -1.5 | -1.7 | -2.5 | -3.8 | -5.6 | -6.0 |
| (63.5,47.8) | 5.7 | 2.4 | -0.5 | -1.6 | -1.7 | -1.7 | -1.7 | -1.7 | -1.9 | -2.7 | -3.9 | -6.0 | -6.0 |
| (62.8,48.1) | 6.8 | 3.8 | -0.1 | -1.6 | -1.9 | -1.8 | -1.9 | -1.9 | -2.0 | -2.9 | -4.0 | -6.0 | -6.0 |
| (62.1,48.3) | 9.3 | 5.2 | 0.8 | -1.4 | -2.1 | -2.1 | -2.0 | -2.0 | -2.1 | -2.8 | -3.9 | -6.0 | -6.0 |
| (61.2,48.4) | 9.9 | 6.0 | 0.9 | -1.5 | -2.2 | -2.3 | -2.3 | -2.2 | -2.4 | -3.1 | -4.2 | -5.9 | -6.0 |

**S4. Mouth face, x 44 to 60, z 40 to 30.** The hole cells (x 50.3 to 53.7, z 33.6 to 37.4) are not ground. Three rocks stand on this ground:
- **West jamb BigBoulders_4:** x 43.49 to 49.36, z 34.88 to 40.83, y -6.23 to -0.41.
- **East jamb BigBoulders_5:** x 54.64 to 60.74, z 34.91 to 40.83, y -6.11 to -0.12.
- **Overhang BigBoulders_2:** x 48.48 to 55.51, z 31.12 to 38.16, y -3.74 to 1.40.

| z \ x | 44 | 45 | 46 | 47 | 48 | 49 | 50 | 51 | 52 | 53 | 54 | 55 | 56 | 57 | 58 | 59 | 60 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 40.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -5.9 | -5.9 | -5.7 | -5.6 | -5.4 | -5.3 | -5.3 | -5.2 | -5.3 | -5.5 | -6.0 | -6.0 |
| 39.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -5.9 | -5.9 | -5.8 | -5.7 | -5.6 | -5.5 | -5.4 | -5.5 | -5.7 | -6.0 | -6.0 |
| 38.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -5.9 | -5.9 | -5.8 | -5.7 | -5.6 | -5.6 | -5.7 | -6.0 | -6.0 | -6.0 |
| 37.4 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -6.0 | -5.9 | -5.9 | -5.8 | -5.7 | -5.7 | -5.8 | -6.0 | -6.0 | -6.0 |
| 37.0 | -5.9 | -5.9 | -5.9 | -5.9 | -6.0 | -6.0 | -6.0 | -6.0 | -5.9 | -5.9 | -5.8 | -5.8 | -5.8 | -5.8 | -5.9 | -5.9 | -5.9 |
| 36.0 | -5.0 | -5.1 | -5.1 | -5.1 | -5.4 | -5.6 | -5.8 | -5.9 | -6.0 | -5.9 | -5.8 | -5.7 | -5.4 | -5.1 | -5.1 | -5.1 | -5.1 |
| 35.0 | -3.1 | -3.1 | -3.1 | -3.1 | -3.1 | -4.2 | -5.0 | -5.4 | -5.6 | -5.5 | -5.2 | -4.6 | -4.0 | -3.2 | -3.2 | -3.2 | -3.2 |
| 34.0 | -1.2 | -1.2 | -1.2 | -1.2 | -0.7 | -1.2 | -2.9 | -3.5 | -3.8 | -3.7 | -3.1 | -1.1 | -1.1 | -1.3 | -1.3 | -1.3 | -1.3 |
| 33.0 | -0.2 | -0.2 | -0.2 | -0.3 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | -0.3 | -0.3 | -0.3 | -0.4 | -0.4 |
| 32.0 | -0.2 | -0.2 | -0.2 | -0.2 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | -0.2 | -0.3 | -0.3 | -0.3 | -0.3 |
| 31.0 | -0.2 | -0.2 | -0.2 | -0.2 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | -0.2 | -0.2 | -0.2 | -0.3 | -0.3 |
| 30.0 | -0.2 | -0.2 | -0.2 | -0.2 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | 0.0 | -0.2 | -0.2 | -0.2 | -0.2 | -0.2 |

**S5. Terrain over the niche, x 53.5 to 55.8, z 33.8 to 36.2.**

| z \ x | 53.5 | 54.0 | 54.4 | 54.9 | 55.3 | 55.8 |
|---|---|---|---|---|---|---|
| 36.2 | -5.90 | -5.86 | -5.80 | -5.72 | -5.67 | -5.61 |
| 35.6 | -5.77 | -5.67 | -5.54 | -5.38 | -5.24 | -5.08 |
| 35.0 | -5.35 | -5.17 | -4.95 | -4.69 | -4.44 | -4.16 |
| 34.4 | -4.48 | -4.23 | -3.93 | -3.41 | -3.04 | -2.37 |
| 33.8 | -2.89 | -2.57 | -1.77 | -0.06 | -0.05 | -0.46 |

**S6. Toilet spot.**
- **Boulder_4** (48.9, 41.2), ground -5.94: footprint x 48.14 to 49.64, z 40.30 to 42.06. Its top is -4.72, 1.22 m over the ground. It has the prefab's exact MeshCollider.
- **The spot** west of it: (47.64, 41.20), ground -6.00, in the corner it makes with the west jamb (jamb north edge z 40.83).
- **Sight lines** from every tread eye, P40 to P84 (eye 1.6), to a lid 0.1 m up and a shovel top 1.1 m up:
  - the lid is hidden from all of them;
  - the shovel top is seen from P54, P56, P58, P60, P62, P64, P66, P68, P70, P72, P74, P76, P78, P80 and P84.

**S7. Rope-rail post line.**

(a) As drawn: the straight line (77.3, 48.6) to (67.9, 46.1), moved 1.2 m north. It runs 0.92 to 3.65 m from the tread centreline. Ground every 1 m along it:

| m along | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
|---|---|---|---|---|---|---|---|---|---|---|
| ground | 2.02 | 2.11 | 1.95 | 1.38 | 0.14 | -1.13 | -1.19 | -0.89 | -0.59 | -0.54 |

(b) 1.2 m north of the tread itself:

| Point | Post position | Ground | Tread |
|---|---|---|---|
| P54 | (77.70, 51.29) | 1.68 | 1.66 |
| P56 | (76.31, 49.26) | 2.08 | 2.27 |
| P58 | (75.14, 47.50) | 1.71 | 1.69 |
| P60 | (74.15, 46.37) | 1.27 | 1.19 |
| P62 | (72.91, 45.86) | 0.84 | 0.73 |
| P64 | (70.94, 46.17) | 0.22 | 0.18 |
| P66 | (68.34, 47.18) | -0.51 | -0.51 |
| P68 | (65.35, 48.48) | -1.31 | -1.29 |

The drop is on the south side. From P58 to P70 the steepest ground is 4.5 to 6.0 m south of the tread, at 71 to 78 degrees, with the floor at -6.00 below. At P54, P56 and P72 it is 46 to 60 degrees.

**S8. Every seated cave rock.** Vertices at the instance transform; the floor is -18 in the chamber and the side room.

| Rock | Centre | Footprint x | Footprint z | y | Collider |
|---|---|---|---|---|---|
| ChamberDressing/BigBoulders_2 | (70.99, 5.01) | 69.16 to 72.87 | 3.28 to 7.07 | -18.30 to -14.72 | convex hull |
| ChamberDressing/BigBoulders_4 | (79.96, 21.01) | 78.18 to 81.79 | 19.12 to 22.68 | -18.30 to -14.77 | convex hull |
| ChamberDressing/Boulder_1 (R) | (89.00, 5.51) | 87.78 to 90.22 | 3.78 to 7.23 | -18.30 to -15.83 | convex hull |
| ChamberDressing/Boulder_5 | (79.00, 3.01) | 77.92 to 80.36 | 1.05 to 4.68 | -18.30 to -15.43 | convex hull |
| ChamberDressing/RubbleSparse_2 x3 | (82.86, 18.68), (74.34, 17.91), (76.28, 5.79) | 81.47 to 84.56 for the first | 17.16 to 20.74 for the first | 0.48 tall | none |
| SideRoom/BigBoulders_0 | (89.50, 9.51) | 88.52 to 90.66 | 8.49 to 10.74 | -18.31 to -16.30 | convex hull |
| SideRoom/BigBoulders_1 | (92.01, 15.50) | 91.09 to 92.52 | 14.58 to 16.08 | -18.29 to -16.71 | convex hull |
| SideRoom/BigBoulders_3 | (97.52, 9.50) | 96.61 to 98.41 | 8.74 to 10.53 | -18.31 to -16.60 | convex hull |
| SideRoom/BigBoulders_5 | (95.00, 15.50) | 94.18 to 95.76 | 14.52 to 16.18 | -18.29 to -16.52 | convex hull |
| SideRoom/Boulder_0 (R) | (97.50, 13.79) | 97.14 to 98.10 | 13.20 to 14.38 | -18.29 to -17.10 | none |
| SideRoom/Boulder_3 | (95.50, 15.49) | 95.05 to 95.82 | 14.99 to 16.03 | -18.31 to -17.00 | convex hull |
| Pockets/CaveMouthPit/Boulder_4 | (55.99, 36.11) | 54.46 to 57.56 | 34.17 to 38.03 | -5.81 to -2.65 | none (the pit's box collider: x 54 to 58, z 34.35 to 37.85, y -6.5 to -2.5) |

## Block
1. **The generator niche (V3) cannot go where it is drawn.**
   - **What is there:** S5 puts the ground over x 53.5 to 55.8, z 34 to 36 at -5.9 to -2.4. A room with floor -6 and ceiling -4 is therefore mostly above the terrain, in the open air east of the opening.
   - **What fills that air:** the east jamb BigBoulders_5 (exact mesh collider from z 34.91), and Cave/Pockets/CaveMouthPit, a solid box at x 54 to 58, y -6.5 to -2.5, z 34.35 to 37.85, with its Boulder_4 drawn inside it. At (55.2, -4.5, 35.0) the niche's east half is inside that rock and that box.
   - **Why it cannot be cut:** the terrain holes stop at x 53.7. Cutting the niche here means cutting the box that fills T1's pocket (54.4, 37).
   - **Missing from the doc:** section 4 does not list CaveMouthPit.
   - **Repro:** draw the niche over panel B with the pit box and the jamb footprint from S4.

## Hurts
2. **The day-2 boards are drawn inside the west jamb.**
   - Test points at x 47.2, 48.0 and 48.8 (y -5.8 and -5.2, z 37.8) are inside BigBoulders_4.
   - Only x 49.4 and up is open, and the Entrance Wall_N stub starts at x 50.0. That leaves 0.6 m of room for 2.6 m of boards.
   - Repro: S4 and the jamb footprint.
3. **The toilet's shovel shows on the walk down.** In S6, Boulder_4 is 1.22 m tall, and a 1.1 m shovel beside it is seen from 15 tread points, P54 to P84. Quill 9 says "not seen on the walk down". Only a lid under about 0.3 m stays hidden.
4. **The mouth is seen from the north rim.**
   - Eyes on the rim at (58 to 78, 58 to 60), eye height 19.4 to 19.6, see all four mouth points: board centre, both void edges and the floor. The range is about 26 to 30 m, across the ravine.
   - From (52 to 54, 60 to 62), the rim's own edge hides it.
   - A player can get up there. The rim plateau slopes 27 to 37 degrees down to the east, toward W1 (at z 66: from 10 at x 84 to 0 at x 96, terrain only).
   - Quill 30 ("from the rim the mouth is not seen") fails. The deck still cannot see the mouth (see Passes).
5. **The rope rail goes on the uphill side, and the drawn line leaves the tread.**
   - In S7, the drop is south of the tread: 71 to 78 degrees, starting 4.5 to 6 m out, from P58 to P70. V1 puts the rail 1.2 m north.
   - The drawn straight line runs 0.9 to 3.65 m from the tread and dips to -1.19 at (72.16, 48.47), 1.9 m below the tread.
   - Line (b) sits on ground within 0.2 m of the tread.
6. **V10's south row would sit on the lip.**
   - In S3, from 3 m south the ground breaks away: -0.8 to -2.9 at +3, -2.6 to -4.2 at +4, -5.5 to -6.0 at +5.
   - A row 1.5 to 2.5 m south is on that break for x 66 to 72 (-0.1 to -1.1 at +2). A 4 m hulled rock there overhangs the 5 m drop, making a perch over the fall.
   - The north row has open ground: -0.3 to -2.1 between -1.5 and -2.5 m, then the wall from -5 m. So V10 can go on that side.
7. **The bulbs are not where V2 says, and F2 cannot frame them.**
   - **As built:** POI_Coloured_bulbs is at (82.64, 4.84, 48.98), 3.86 m off the tread, on ground 4.84. Its DeadBranch has a capsule collider from y 4.97 to 7.72, and the BulbString hangs at y 7.44.
   - **As drawn:** V2 says 1 m off the tread at +0.6, and the SVG draws them at (82, 52).
   - **Framing:** from F2 (91.3, 47.4) they bear 280, 20 degrees off the heading of 300, but sit 38 to 42 degrees up. From P44 they are 44 to 48 degrees up, and from P48 49 to 55 degrees up. A level frame (half-height 30 degrees) misses them. The line is clear of rock and terrain.
8. **R7's talk stand misses him.** From (86.4, 6.6) facing 80, the shelf centre (88.4, 6.0) is 2.09 m away at bearing 106.7. That is past the 2 m reach to his head, and 27 degrees off the ray.
9. **Section 4's collider list is wrong in seven places, and Rook builds from it.**
   - **(a) Table and chairs:** the table and both chairs have the prefab's convex MeshCollider (Chair.prefab, Table.prefab), not FitCollider boxes. So "built at 205, so FitCollider inflated its box" does not apply.
     - His chair is at local yaw 205 under the RouletteTable group's 90, which is world 295.
     - "Turns to yaw 180" is ambiguous. Local 180 is world 270, facing the guest and the doorway (Quill 17). World 180 faces the south wall.
   - **(b) Crate:** the crate does have an inflated FitCollider box: 0.82 x 0.82 for a 0.5 x 0.5 crate at yaw 20.
   - **(c) Side-room rocks:** all five V7 rocks already have convex hulls. Only Boulder_0, which goes, has none. So "stripped in 8.17" and "no colliders" are both wrong.
   - **(d) Chamber rocks:** all four have convex hulls. Section 4 says none; V8 says hulled.
   - **(e) Jambs:** BigBoulders_4, BigBoulders_5 and Boulder_4 keep the prefab's exact (non-convex) MeshCollider, not convex hulls.
   - **(f) Overhang:** BigBoulders_2 has a convex hull; it is not stripped. Its underside is at -3.74 (2.26 m over the floor) and runs x 48.5 to 55.5, z 31.1 to 38.2. Inside the passage, at z 33.6 to 37.4, it hangs 1.74 m below the -2 ceiling, over the load-down spot.
   - **(g) Missing or wrong entries:** CaveMouthPit (a box collider) is missing. The spur's rope rail is 1.0 m posts with a rope box at 0.6 m, not "0.1 m cubes".
10. **The moved crate leaves gaps in the 0.6 to 1.0 band.** C_Crate_Small_1 at yaw 0, at (90.4, 14.6), 0.5 m across:
    - 0.65 m to the room's west wall face (x 89.5);
    - 0.65 m to the north wall (z 15.5);
    - 0.44 m to BigBoulders_1 (x 91.09).

    The corner is closed and too narrow to enter (0.65 against a 0.70 body).
11. **The cable has nowhere to start.** V3 runs it from the niche (finding 1). Until the niche is placed, the leg 1 marker on the left (Pim 4.2) has no source.

## Cosmetic
12. **Mouth strip** (x 52 to 54.5, z 38 to 46.5, nothing over 0.3 m):
    - Ground815/TrailEdges/[CS_Stone_8] (52.84, -4.96, 41.94) is 0.40 m tall and has a hull.
    - CS_Stone_8 (53.69, 44.54) is 0.17 m and RubbleSparse_1 (54.70, 38.90) is 0.01 m.
    - The overhang reaches z 38.16, overhead.
13. **Walk times.** Camp to the table is 92.3 (camp to pump) + 78.3 + 108.6 + 73 + 18.3 + 3.6 = 374 m, 150 s (doc 402 m, 161 s). W1 to the mouth is 108.6 m, 43 s (doc 109, 44).
14. **Sound doc against the built ground.**
    - **Hollis 12:** there is no flat face at z 37.4. The ground there is -5.7 to -6.0 the whole way. The face is a 60 degree slope from z 37 to z 33, 6 m high, mostly behind the jambs.
    - **Hollis 13:** spur metres 85 to 100 run from (67.9, 46.1) to (55.5, 45.0). V10 covers about metres 81 to 93.
    - **Hollis 3:** he asks for at least 1 m of rock between the chamber and the side room. The wall is 0.5 m (x 89.0 to 89.5), and side-room BigBoulders_0 pokes 0.48 m through it into the chamber at z 8.49 to 10.74, 0.66 m south of the doorway.
15. **Pim 4.7:** the bulbs at the doorway hang 2.07 to 2.12 m over the floor (Pim says 2.45), level with the 2.1 m lintel. Lowering them to 1.9 still works.
16. **Battery bank** (x 84 to 85, z 20.2 to 20.8) stands on RubbleSparse_2 (82.86, 18.68), whose footprint reaches x 84.56, z 20.74. The stack is 0.44 m from the rubble. The rubble has no collider.
17. **V9:** the terrain over it is -1 to -2, not 0. Clearance is still about 13 m.
18. **The guest chair stand** (91.0, 12.0) facing 90 has the chair at bearing 105, 1.92 m away, with its back 0.5 m under the eye. It needs about 15 degrees down (Pim notes the pitch is kept).
19. **In the passage:** Ground815/TrailEdges/[CS_Stone_5] (53.22, -5.88, 36.83) sits by the east wall, 0.5 m east of the load-down spot. It is 0.21 m tall with no collider. Separately, spur point P56 sits 0.18 m over its ground (2.27 against 2.09).
20. **SVG:**
    - the bulbs are drawn at (82, 52); they are built at (82.64, 48.98);
    - the rope rail is drawn as the straight chord;
    - panel B has no CaveMouthPit, overhang or jamb footprints.

## Passes
- **V1 spur heights.** They match the scene to 0.1 m from P0 to P84, and the trail is cut flush (ground equals tread everywhere except P56). The descent is 8.27 m over 34.0 m (P56 to P84). The old rope rail is on level ground at -3.0 with no drop, as Sable says.
- **Ravine closed.** There is no walkable route from the floor to the lowland, plateau or rim except along the spur (finding 4 is about sight, not access). Falling off the north bank lands on the floor, which leads on foot to the mouth: no trap.
- **Deck must-hide (hard), terrain alone, 128 eyes.** All of these are hidden, with the best line at least 8.6 m under the ground:
  - the mouth board and the east jamb top;
  - the bulbs and the toilet shovel;
  - both new rail lines and the spur from P56 to P84;
  - 24 points on the ravine floor.

  Nothing depends on trees.
- **Mouth found rule** (rocks, terrain and trunks; ferns and bushes not modelled):

  | Distance back | Eye position | Result |
  |---|---|---|
  | 30 m | (74.80, 45.51) | east jamb blocks all three targets |
  | 20 m | (65.44, 47.14) | tread heads 294; mouth is 57 to 62 degrees off travel |
  | 10 m | (56.31, 46.60), heading 222 | board and both void edges clear, 12 to 23 degrees off travel, 9.7 to 10.6 m |
  | 5 m | | clear, 1.5 to 20 degrees off travel |

  So it is found at 10 m. Sable's line through (52.9, 39.7) stays west of the east scree (54.7).
- **Keep-clear strips.**
  - **Chamber** (z 10.8 to 13.2): no rock. The nearest is side-room BigBoulders_0, at z 10.74.
  - **Side room** (z 11 to 13, x 89.25 to 92.4): no rock or prop. BigBoulders_0 is 0.26 m south; the guest chair's west edge is at about 92.55.
- **Trap spots T2 to T5.**
  - **T2 and T3 hold.** Every chamber and side-room rock bottoms out at -18.29 to -18.31, and the side-room rock tops are 1.2 to 2.0 m under a 3 m ceiling. The shelf covers Boulder_1's footprint.
  - **T4:** there is no RedwoodHollowLog_0 near (91.5, 47.3).
  - **T5:** Cave_Mouth (58, -4.16, 44) is 0.24 m over the ground (-4.40). The opening bears 223, 12 degrees left of the warp's facing of 211.
- **Chamber.**
  - The seat shelf is 1.8 m from the mattress.
  - The stack centre is 7.1 m from the doorway. From the table, the doorway shows the chamber only at z 13.6 and below, so the stack front (z 17 and up) stays out of view.
  - The food is 1.5 m off the west wall and 0.5 m outside the passage line.
  - From the passage end (71, 12) it is 18.25 m to the doorway and 16.3 m to the seat stand.
- **Side room.**
  - The standing point (92.0, 12.0) clears the guest chair by about 0.2 m, with the doorway 2.75 m ahead.
  - Cave_SideRoom (91.0, 12.0) is 1.6 m from the chair.
  - V9's opening (z 13.2 to 14.4) is clear once Boulder_0 goes; BigBoulders_5 is 0.12 m north of it.
  - The inspect point is 1.5 m from the dead end.
- **Passage contents.** The drip can (51.2, 28), the jugs (50.9, 34.2) and the load-down spot (51.5, 36.1 to 37.0) are clear of rock from y -5.8 to -3.9.
- **Trees.**
  - No Forest trunk is within 1.5 m of the tread west of x 100, or of V10. The nearest is RedFir5 (97.92, 42.45), 3.65 m away.
  - Nothing grows over the cave at (65, 20); that is the plate at 0.
  - The trees on the ravine floor are RedPine1 (49.68, 43.58), 4.95 m from the tread, RedFir6 (64.44, 39.07), RedPine1 (81.32, 38.87) and RedFir8 (41.76, 39.75).
- **Rim at (74, 60): 18.00** (Quill 28, C-1).

Marlow
