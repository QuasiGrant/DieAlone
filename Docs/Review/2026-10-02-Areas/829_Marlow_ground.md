# 8.29 ground survey: the Ward path and ledge as built, against WardPath.md draft 2
2026-10-02, Marlow. Measured as built, read-only; no Editor calls.

**Sources, read at 7f2c893:**
- Main3.unity, parsed for world transforms and every scene collider. Prefab colliders were read from the prefabs.
- Main3_TerrainData.asset, decoded (check (392, 165) = 3.000).
- The rock and lip collider meshes, decoded from their YAML assets in Assets/Terrain/Main3: Rock/BackWall_N and _S, Band_S_W, Band_W_N, EndWall_N and _S, Fin, FinCap, Lip, Lip_N, ClimbRing; ClimbRim_WardPath, LipEnd_WardPath and Rims815.
- FBX vertices for every BK rock, CS stone and CS rock near the path, and for the fallen giant's Sequoia4.

**Walking surface.** It is terrain everywhere except flight 3 and the prow. Every J to Ward marker sits within 0.05 m of the terrain, with three exceptions:
- P2 and P4 are 0.36 m over the ground.
- Flight 3 (P138 to P155) is 0.35 m over the ground, on its StairRamp.
- The prow (P210 and P211) is at 62.0 on ProwTop, over terrain at 35.6 to 38.2.

**Method:**
- Cells are 0.5 m. "Ground" is the walking surface: the terrain, raised to the top of any box collider or walkable collider mesh under the cell. Box tops (posts, rails, the gate blocker) therefore show up as small drops in table 2.
- A cell is wall if any collider comes within 0.3 m of a body standing there, between 0.3 and 1.8 m over the ground. Prefab collider shapes (rocks and stones) come from their FBX vertices.
- Moves allowed: walk up at most 44 degrees per cell; drop down any height.
- Flood seeds: every trail in the region (Camp 1 to J, Camp to J, J to Ward), per the 828 lesson. Night is the gate blocker off; day is the blocker on.
- Speed: 2.5 m/s.
- Deck: 128 eyes, at 57.6 and 58.2 m, over x 160.5 to 167.5, z 162.5 to 169.5.

## 1. Ground along the path, every 2 m, and 2, 4 and 6 m to each side
How to read it:
- "m" is metres along the J to Ward markers from J (104, 206).
- Offsets are perpendicular to travel: + is left, - is right.
- "w" marks a cell a standing body could not occupy (wall).
- No collider sits on the tread centreline anywhere along the path.

| m | point | tread y | -6 | -4 | -2 | 0 | +2 | +4 | +6 |
|---|---|---|---|---|---|---|---|---|---|
| 0 | (104.0, 206.0) | 10.01 | 9.33 | 9.40 | 9.59 | 9.89 | 9.84 | 9.36 | 8.61 |
| 2 | (102.1, 206.7) | 10.27 | 9.30 | 9.39 | 9.56w | 9.92 | 10.02 | 10.00 | 9.97 |
| 4 | (100.3, 207.4) | 10.54 | 9.36 | 9.59 | 9.90 | 10.12 | 10.48 | 10.09 | 10.09 |
| 6 | (98.4, 208.2) | 10.81 | 9.76 | 9.93 | 10.81 | 10.79 | 10.80 | 10.26 | 10.29 |
| 8 | (96.5, 208.9) | 11.08 | 9.78w | 9.99 | 10.90 | 11.09 | 11.01 | 10.49 | 10.53 |
| 10 | (94.7, 209.6) | 11.35 | 9.92 | 10.17 | 11.31 | 11.36 | 11.34 | 10.84 | 10.91 |
| 12 | (92.8, 210.4) | 11.62 | 10.25w | 10.44 | 11.60 | 11.60 | 11.58 | 11.16w | 11.37w |
| 14 | (91.0, 211.1) | 11.89 | 10.76w | 10.90 | 11.86 | 11.88 | 11.83 | 12.21 | 12.21 |
| 16 | (89.1, 211.8) | 12.15 | 11.28 | 11.40w | 12.77 | 12.17 | 12.12 | 12.21 | 12.21 |
| 18 | (87.2, 212.5) | 12.42 | 11.61 | 11.79w | 12.77 | 12.45 | 12.39w | 15.23w | 16.86 |
| 20 | (85.3, 213.1) | 12.78 | 15.22 | 16.48 | 14.62w | 12.72 | 15.21w | 16.80 | 16.86 |
| 22 | (83.3, 213.2) | 13.27 | 16.38 | 16.48 | 14.10w | 13.21 | 13.95w | 15.06w | 16.89 |
| 24 | (81.3, 213.4) | 14.17 | 15.96 | 15.78 | 14.30 | 14.14 | 14.31 | 15.50 | 15.95 |
| 26 | (79.3, 213.6) | 15.85 | 15.96 | 16.17 | 15.87 | 15.74 | 16.03 | 15.97 | 15.96 |
| 28 | (77.3, 213.8) | 17.50 | 15.96 | 17.37w | 17.75 | 17.38 | 17.72 | 17.15w | 15.96 |
| 30 | (75.3, 213.9) | 18.39 | 15.96 | 17.53w | 18.55 | 18.36 | 18.54 | 17.68w | 15.96 |
| 32 | (73.3, 214.1) | 18.89 | 16.87 | 21.28 | 19.13 | 18.85 | 19.39 | 21.26 | 21.54 |
| 34 | (71.4, 214.3) | 20.17 | 22.88 | 21.93w | 20.63 | 20.18 | 20.53 | 21.53w | 22.57 |
| 36 | (69.4, 214.5) | 21.84 | 24.41 | 24.43w | 22.18 | 21.73 | 22.27 | 24.03w | 24.30 |
| 38 | (67.4, 214.6) | 23.48 | 25.95 | 25.64 | 23.63 | 23.38 | 23.89 | 25.64 | 26.02 |
| 40 | (65.4, 214.8) | 24.02 | 26.75 | 26.05 | 24.44 | 24.00 | 24.31 | 26.06 | 26.51 |
| 42 | (63.4, 215.0) | 24.58 | 27.25 | 27.02 | 24.92 | 24.56 | 25.06 | 26.70w | 27.16 |
| 44 | (61.4, 215.2) | 26.16 | 28.63 | 27.32 | 26.39 | 26.11 | 26.73 | 27.74 | 28.77 |
| 46 | (59.4, 215.4) | 27.82 | 30.49 | 29.54 | 28.24 | 27.76 | 28.19 | 29.22w | 30.25 |
| 48 | (57.4, 215.5) | 29.16 | 31.75 | 31.04 | 29.43 | 29.14 | 29.52 | 31.18 | 31.69 |
| 50 | (55.4, 215.7) | 29.65 | 32.15 | 31.57 | 29.87 | 29.63 | 30.11 | 31.53 | 32.37 |
| 52 | (53.4, 215.9) | 30.00 | 32.15 | 31.76 | 30.03 | 30.00 | 29.68w | 27.00 | 23.16 |
| 54 | (52.1, 217.0) | 30.03 | 29.03 | 29.53 | 29.98 | 30.00 | 30.00 | 30.00 | 32.00 |
| 56 | (52.2, 219.0) | 30.21 | 29.83 | 30.35 | 30.61 | 30.20 | 30.18 | 30.31 | 31.00 |
| 58 | (52.4, 221.0) | 30.61 | 30.80 | 31.46 | 32.07 | 30.61 | 31.00 | 30.72 | 31.20 |
| 60 | (52.6, 223.0) | 31.02 | 32.06 | 32.66 | 33.24 | 31.02 | 31.01 | 31.12 | 31.40 |
| 62 | (52.7, 225.0) | 31.42 | 31.07 | 31.85 | 31.94 | 31.42 | 31.40 | 31.53 | 31.53 |
| 64 | (52.9, 227.0) | 31.83 | 29.03 | 29.94 | 31.61w | 31.83 | 31.81w | 31.94w | 31.94w |
| 66 | (53.1, 229.0) | 32.23 | 27.35 | 28.44 | 32.09w | 32.24 | 32.81 | 32.81 | 32.34 |
| 68 | (53.2, 231.0) | 32.64 | 25.67 | 28.96 | 32.56 | 32.64 | 32.81 | 32.81 | 32.75w |
| 70 | (53.4, 233.0) | 33.04 | 24.70 | 28.70 | 32.89w | 33.05 | 33.04 | 33.15w | 33.15 |
| 72 | (53.6, 235.0) | 33.45 | 25.44 | 29.44 | 33.46 | 33.46 | 33.44 | 33.56 | 33.56 |
| 74 | (53.7, 237.0) | 33.86 | 26.18 | 30.18 | 33.86 | 33.86 | 33.84w | 34.66 | 33.97 |
| 76 | (53.9, 238.9) | 34.26 | 25.92 | 29.92 | 33.95w | 34.27 | 34.66 | 34.66 | 34.38 |
| 78 | (54.1, 240.9) | 34.67 | 26.66 | 30.66 | 34.62 | 34.68 | 34.67 | 34.78 | 34.78w |
| 80 | (54.2, 242.9) | 35.07 | 27.40 | 31.40 | 35.09 | 35.08 | 35.06 | 35.09 | 35.19 |
| 82 | (54.0, 244.6) | 35.41 | 36.62 | 36.20 | 35.80 | 35.39 | 34.97 | 34.57 | 34.66 |
| 84 | (52.0, 244.9) | 35.49 | 36.69 | 36.48 | 36.48 | 35.49 | 35.09 | 34.68 | 34.66 |
| 86 | (50.0, 245.2) | 35.58 | 36.72 | 36.48 | 36.48 | 35.50 | 35.09 | 34.68 | 34.27 |
| 88 | (48.1, 245.5) | 35.65 | 36.82 | 36.41 | 36.00 | 35.60 | 35.19 | 34.78w | 34.38 |
| 90 | (46.1, 245.8) | 35.73 | 36.82 | 36.41 | 36.00 | 35.75 | 35.29 | 34.89 | 34.48 |
| 92 | (44.3, 246.4) | 36.00 | 36.31 | 36.11 | 35.91 | 36.02 | 37.86 | 42.99 | 46.00 |
| 94 | (43.5, 248.2) | 36.98 | 36.72 | 36.51 | 36.35 | 36.90 | 38.61 | 41.22 | 47.00 |
| 96 | (42.6, 250.0) | 37.96 | 37.02 | 36.82 | 36.77 | 38.00 | 39.28 | 42.94 | 46.40 |
| 98 | (41.7, 251.8) | 38.94 | 37.43 | 37.22 | 37.68 | 39.10 | 40.64 | 44.56 | 47.27 |
| 100 | (40.8, 253.6) | 39.93 | 37.74 | 37.63 | 38.82 | 39.87 | 41.33 | 44.99 | 47.79 |
| 102 | (39.9, 255.4) | 40.92 | 38.14 | 37.94 | 40.12 | 40.97 | 42.99 | 45.46 | 47.47 |
| 104 | (39.1, 257.2) | 41.90 | 38.55 | 38.34 | 40.38 | 41.85 | 43.18 | 46.14 | 46.65 |
| 106 | (38.2, 258.9) | 42.88 | 38.85 | 39.04 | 42.29 | 42.94 | 44.73 | 45.53 | 46.70 |
| 108 | (37.3, 260.7) | 43.86 | 39.26 | 41.27 | 44.54 | 43.72 | 44.04 | 45.13 | 49.15 |
| 110 | (35.9, 261.9) | 44.00 | 53.44 | 48.70 | 44.00 | 44.00 | 43.72 | 43.01 | 41.09 |
| 112 | (34.9, 260.2) | 44.13 | 59.84 | 56.35 | 45.87 | 44.04 | 43.60 | 43.31 | 40.21 |
| 114 | (34.7, 258.2) | 44.99 | 58.12 | 58.55w | 47.66 | 45.09 | 44.73 | 42.39 | 41.31 |
| 116 | (34.6, 256.2) | 45.87 | 55.50 | 57.05 | 48.09 | 45.96 | 46.25 | 41.64 | 40.86 |
| 118 | (34.4, 254.2) | 46.73 | 55.50 | 55.55 | 48.53 | 46.82 | 45.45 | 41.78 | 40.20 |
| 120 | (34.3, 252.2) | 47.59 | 55.43 | 54.05 | 48.96 | 47.69 | 46.75 | 43.23 | 39.94 |
| 122 | (34.1, 250.2) | 48.47 | 57.00 | 52.60 | 61.10 | 48.57 | 47.72 | 44.56 | 41.14 |
| 124 | (34.0, 248.2) | 49.00 | 56.51 | 50.86 | 50.50 | 49.00 | 48.13 | 44.67 | 41.21 |
| 126 | (32.9, 246.7) | 49.00 | 54.07 | 51.85 | 50.55 | 48.97 | 49.00 | 50.72 | 50.00 |
| 128 | (31.5, 245.4) | 49.00 | 48.00 | 50.05 | 49.00 | 49.00 | 51.47 | 56.97 | 62.46 |
| 130 | (31.3, 247.4) | 49.97 | 45.54 | 50.05 | 49.00 | 50.15 | 51.86 | 57.29 | 62.51 |
| 132 | (31.1, 249.4) | 51.51 | 46.23 | 48.54 | 48.85 | 51.75 | 54.09 | 59.74 | 65.07 |
| 134 | (30.8, 251.4) | 53.06 | 45.87 | 47.67 | 48.23 | 53.25 | 54.07 | 58.00 | 61.20 |
| 136 | (30.5, 253.4) | 54.60 | 45.45 | 46.82 | 48.74 | 54.80 | 55.50 | 55.50 | 58.28 |
| 138 | (30.3, 255.4) | 56.15 | 46.44 | 45.97 | 49.70 | 56.30 | 55.50 | 55.50 | 58.24 |
| 140 | (30.0, 257.3) | 57.69 | 45.21 | 45.11 | 49.26 | 57.85 | 55.50 | 55.50 | 59.62 |
| 142 | (29.7, 259.3) | 59.24 | 44.04 | 44.82 | 50.21 | 59.40 | 59.04 | 59.33 | 60.46w |
| 144 | (29.2, 261.2) | 60.00 | 55.87 | 58.89w | 59.99 | 60.00 | 59.05 | 55.86 | 55.50 |
| 146 | (27.2, 261.6) | 60.00 | 59.39 | 59.67 | 60.00 | 60.00 | 60.00 | 55.86w | 55.50 |
| 148 | (25.2, 261.9) | 60.00 | 59.31 | 59.76 | 60.00w | 60.00 | 60.00w | 59.25w | 59.62 |
| 150 | (23.2, 262.0) | 60.04 | 60.89 | 61.07 | 60.92 | 60.05 | 61.13 | 62.01 | 62.37 |
| 152 | (21.2, 262.0) | 60.11 | 74.93 | 62.50 | 62.29 | 60.12 | 62.37 | 67.94 | 67.86 |
| 154 | (19.2, 262.0) | 60.17 | 85.42 | 85.55 | 78.52 | 60.18 | 77.77 | 85.76 | 85.82 |
| 156 | (17.4, 262.7) | 60.24 | 83.79 | 84.18 | 78.88 | 60.24 | 73.65 | 87.63 | 87.57 |
| 158 | (16.0, 264.0) | 60.31 | 85.60 | 85.53 | 82.09 | 60.31 | 80.39 | 86.60 | 86.27 |
| 160 | (14.4, 265.3) | 60.38 | 85.41 | 84.98 | 73.72 | 60.38 | 79.18 | 86.01 | 85.93 |
| 162 | (12.5, 265.5) | 60.45 | 84.82 | 84.77 | 79.20 | 60.44 | 79.20 | 86.29 | 86.82 |
| 164 | (10.5, 265.5) | 60.52 | 85.33 | 85.58 | 79.26 | 60.51 | 79.26 | 85.56 | 86.03 |
| 166 | (8.5, 265.5) | 60.58 | 85.77 | 86.17 | 79.33 | 60.58 | 79.33 | 85.17 | 85.60 |
| 168 | (6.5, 265.5) | 60.65 | 85.42 | 85.85 | 79.36 | 60.64 | 79.13 | 84.81 | 85.25 |
| 170 | (4.5, 265.5) | 60.71 | 61.97 | 61.86 | 61.70 | 60.71 | 60.79 | 60.86 | 60.93 |
| 172 | (4.0, 263.8) | 60.78 | 61.59 | 61.48 | 61.41 | 60.78 | 64.82 | 79.35 | 79.28 |
| 174 | (4.0, 261.8) | 60.84 | 61.47 | 61.33 | 61.24 | 60.84 | 65.53 | 85.17 | 85.43 |
| 176 | (4.0, 259.8) | 60.91 | 61.37 | 61.22 | 61.12 | 60.91 | 65.47 | 85.40 | 85.72 |
| 178 | (4.0, 257.8) | 60.98 | 61.32 | 61.16 | 61.05 | 60.98 | 65.58 | 86.29 | 86.48 |
| 180 | (2.8, 256.3) | 61.03 | 61.33 | 61.18 | 61.07w | 61.02 | 61.06 | 66.07 | 89.06 |
| 182 | (1.4, 254.9) | 61.13 | 61.39 | 61.28 | 61.16 | 61.12 | 61.16 | 61.26 | 61.40 |
| 184 | (-0.1, 253.6) | 61.28 | 61.50 | 61.41 | 61.31 | 61.27 | 61.33 | 61.40 | 61.53 |
| 186 | (-1.6, 252.2) | 61.46 | 61.64 | 61.58 | 61.50 | 61.47 | 61.48 | 61.53 | 61.63 |
| 188 | (-3.1, 250.9) | 61.65 | 61.78 | 61.73 | 61.66 | 61.63 | 61.64 | 61.71 | 61.78 |
| 190 | (-4.6, 249.6) | 61.81 | 61.90 | 61.85 | 61.83 | 61.81 | 61.82 | 61.87 | 61.91 |
| 192 | (-6.1, 248.2) | 61.93 | 47.53w | 61.96 | 61.95 | 61.94 | 61.94 | 61.97 | 61.98 |
| 194 | (-7.5, 246.9) | 61.98 | 35.43 | 47.58w | 62.00 | 62.00 | 62.00 | 62.00 | 62.00 |
| 196 | (-9.2, 246.0) | 62.00 | 61.96 | 61.99 | 62.00 | 62.00 | 62.00 | 62.00 | 62.00 |
| 198 | (-11.2, 246.0) | 62.00 | 36.74 | 36.79 | 62.00 | 62.00 | 62.00 | 34.92 | 34.92 |

## 2. Slopes over 30 degrees, drops over 1 m, pockets
**Ground over 30 degrees within 8 m of the path** (connected cells of 4 or more):

| Centre | Extent | Area m2 | Max deg | Ground range | Nearest to path m | Nearest cell |
|---|---|---|---|---|---|---|
| (-13.8, 246.1) | x -19.5 to -9.5, z 238.0 to 255.5 | 128.75 | 89 | 12.96 to 63.05 | 0.0 | (-11.5, 246.0) |
| (15.5, 266.9) | x 2.5 to 24.5, z 262.5 to 273.5 | 94.25 | 88 | 58.75 to 86.27 | 0.3 | (18.5, 262.5) |
| (30.3, 253.3) | x 5.0 to 45.0, z 237.5 to 270.0 | 581.75 | 88 | 33.98 to 89.78 | 0.0 | (31.0, 250.0) |
| (63.6, 220.7) | x 44.5 to 90.5, z 204.5 to 252.0 | 611.25 | 85 | 11.56 to 37.02 | 0.0 | (69.0, 214.5) |
| (49.8, 219.6) | x 48.5 to 51.5, z 219.0 to 220.0 | 3.50 | 47 | 30.08 to 31.00 | 0.8 | (51.5, 220.0) |
| (50.6, 228.4) | x 48.5 to 52.5, z 228.0 to 229.5 | 4.75 | 46 | 31.91 to 32.81 | 0.5 | (52.5, 228.5) |
| (51.0, 237.6) | x 49.0 to 53.5, z 237.0 to 238.5 | 5.50 | 47 | 33.74 to 34.66 | 0.3 | (53.5, 238.0) |
| (51.7, 246.5) | x 49.5 to 54.0, z 246.0 to 247.5 | 5.25 | 47 | 35.57 to 36.48 | 0.8 | (50.0, 246.0) |
| (89.8, 206.4) | x 89.5 to 90.5, z 204.0 to 208.5 | 4.75 | 47 | 11.32 to 12.21 | 2.6 | (90.5, 208.5) |
| (95.9, 212.0) | x 91.0 to 100.0, z 208.5 to 214.0 | 9.50 | 40 | 9.89 to 12.03 | 0.9 | (100.0, 208.5) |
| (102.7, 200.0) | x 100.5 to 105.5, z 198.0 to 202.5 | 14.00 | 49 | 7.08 to 9.99 | 3.6 | (103.0, 202.5) |

**Drops over 1 m within 8 m of the path** (connected cells over 45 degrees whose ground range, neighbours included, exceeds 1 m):

| Centre | Extent | Area m2 | Max deg | Ground range | Nearest to path m | Nearest cell |
|---|---|---|---|---|---|---|
| (-13.8, 246.1) | x -19.5 to -9.5, z 238.0 to 255.5 | 128.75 | 89 | 12.96 to 63.05 | 0.0 | (-11.5, 246.0) |
| (28.1, 249.7) | x 5.5 to 44.5, z 237.5 to 265.0 | 365.75 | 88 | 34.08 to 89.67 | 0.5 | (14.0, 265.0) |
| (13.9, 267.1) | x 5.5 to 22.5, z 262.5 to 273.5 | 68.25 | 88 | 60.05 to 86.27 | 0.3 | (18.5, 262.5) |
| (24.0, 268.3) | x 24.0 to 24.0, z 266.5 to 270.0 | 2.00 | 56 | 58.75 to 60.71 | 4.5 | (24.0, 266.5) |
| (35.0, 261.8) | x 28.0 to 44.0, z 247.0 to 270.0 | 137.50 | 86 | 36.77 to 61.10 | 0.3 | (30.5, 255.5) |
| (49.9, 214.7) | x 44.5 to 61.0, z 208.0 to 232.0 | 84.00 | 85 | 18.03 to 35.00 | 1.9 | (53.5, 214.0) |
| (52.5, 236.0) | x 52.5 to 52.5, z 236.0 to 236.0 | 0.25 | 48 | 33.54 to 34.66 | 1.2 | (52.5, 236.0) |
| (55.5, 221.3) | x 53.5 to 59.5, z 218.5 to 225.5 | 11.25 | 66 | 29.19 to 33.24 | 0.9 | (53.5, 223.5) |
| (58.8, 237.9) | x 55.0 to 62.0, z 223.5 to 252.0 | 145.00 | 70 | 20.44 to 37.02 | 2.0 | (56.0, 240.0) |
| (57.5, 207.5) | x 57.5 to 57.5, z 207.5 to 207.5 | 0.25 | 45 | 30.92 to 32.36 | 8.0 | (57.5, 207.5) |
| (61.0, 219.5) | x 60.5 to 61.5, z 217.5 to 221.5 | 6.75 | 46 | 26.02 to 30.81 | 2.2 | (60.5, 217.5) |
| (62.5, 223.0) | x 61.5 to 63.5, z 223.0 to 223.0 | 1.25 | 48 | 27.27 to 30.35 | 7.8 | (61.5, 223.0) |
| (65.0, 211.4) | x 62.0 to 68.0, z 210.0 to 212.0 | 6.25 | 66 | 23.66 to 27.59 | 2.8 | (65.0, 212.0) |
| (67.8, 218.1) | x 62.5 to 72.0, z 217.0 to 219.0 | 13.25 | 66 | 20.63 to 27.65 | 2.4 | (68.5, 217.0) |
| (69.0, 208.5) | x 68.0 to 70.0, z 208.0 to 209.0 | 3.75 | 45 | 22.73 to 26.13 | 5.4 | (70.0, 209.0) |
| (73.0, 210.0) | x 68.0 to 78.0, z 206.5 to 212.0 | 20.25 | 80 | 15.96 to 24.46 | 2.4 | (70.5, 212.0) |
| (69.8, 220.5) | x 68.5 to 71.0, z 220.0 to 221.0 | 4.25 | 46 | 22.58 to 26.26 | 5.4 | (68.5, 220.0) |
| (69.3, 206.5) | x 69.0 to 69.5, z 206.5 to 206.5 | 0.50 | 46 | 23.39 to 25.25 | 7.9 | (69.5, 206.5) |
| (69.0, 222.5) | x 69.0 to 69.0, z 222.5 to 222.5 | 0.25 | 47 | 24.93 to 26.35 | 8.0 | (69.0, 222.5) |
| (72.0, 209.5) | x 72.0 to 72.0, z 209.5 to 209.5 | 0.25 | 45 | 21.29 to 22.54 | 4.7 | (72.0, 209.5) |
| (75.0, 218.8) | x 72.5 to 79.0, z 217.0 to 222.0 | 14.00 | 81 | 15.96 to 23.15 | 3.2 | (77.5, 217.0) |
| (80.9, 210.6) | x 80.5 to 81.0, z 210.0 to 211.0 | 1.00 | 47 | 13.90 to 15.83 | 2.4 | (81.0, 211.0) |
| (81.2, 216.3) | x 80.5 to 81.5, z 216.0 to 217.0 | 1.50 | 48 | 13.92 to 16.07 | 2.5 | (80.5, 216.0) |
| (82.0, 217.5) | x 82.0 to 82.0, z 217.5 to 217.5 | 0.25 | 45 | 14.76 to 15.93 | 4.1 | (82.0, 217.5) |
| (85.1, 208.7) | x 82.5 to 86.5, z 204.5 to 211.5 | 17.25 | 78 | 11.96 to 16.96 | 1.3 | (86.0, 211.5) |
| (85.7, 216.6) | x 82.5 to 90.0, z 213.5 to 221.0 | 19.50 | 77 | 11.56 to 16.63 | 0.9 | (84.5, 214.0) |
| (101.7, 199.6) | x 101.5 to 102.0, z 198.5 to 201.0 | 2.75 | 45 | 7.31 to 9.86 | 5.6 | (101.5, 201.0) |
| (104.2, 198.7) | x 104.0 to 104.5, z 198.5 to 199.0 | 0.75 | 49 | 7.15 to 8.66 | 7.0 | (104.0, 199.0) |
| (106.5, 203.0) | x 106.5 to 106.5, z 203.0 to 203.0 | 0.25 | 69 | 9.55 to 12.16 | 3.9 | (106.5, 203.0) |
| (107.0, 202.5) | x 107.0 to 107.0, z 202.5 to 202.5 | 0.25 | 69 | 9.51 to 12.16 | 4.6 | (107.0, 202.5) |
| (107.0, 203.5) | x 107.0 to 107.0, z 203.5 to 203.5 | 0.25 | 69 | 9.56 to 12.16 | 3.9 | (107.0, 203.5) |
| (107.5, 203.0) | x 107.5 to 107.5, z 203.0 to 203.0 | 0.25 | 69 | 9.52 to 12.16 | 4.6 | (107.5, 203.0) |

**Pockets.** The flood was run at night, seeded from every trail, with the gate blocker off.
- **Main result:** from J alone it reaches the chute top, both landings, the lookout and the prow, and walks back from the prow. Nothing north of the fallen giant (z 272) is reachable from any trail; I tested (30, 290), (40, 280), (45, 285) and (55, 290).
- **Trapped cells:**

  | Pocket | Size | Ground | Distance from the path | Way in |
  |---|---|---|---|---|
  | (63.0, 278.8), x 62.5 to 63.5, z 278.0 to 279.5 | 3.0 m² | 29.37 to 32.50 | 30.5 m | A 0.66 m drop from (62.5, 277.5, 33.16). The way there runs north from the draw at (62, 215.5) along x 62.5 up the slope east of the shelf rim, to z 277.5, past the east end of the giant's RimTie. |
  | (78.3, 235.0) | 1.0 m² | 15.95 | 21.2 m | A 1 m drop from the W foot rock band top (77.5, 234.5, 16.95). |
  | (84.0, 226.5) | 0.75 m² | 12.32 | 13.2 m | A 2.8 m drop off the north rock arm of the gate (83.5, 226.0, 15.16). |
  | (100.0, 295.0) | 0.25 m² | 7.90 | 68.0 m | A 7.8 m drop off the W foot band's north end (99.5, 295.0, 15.68). |

  - The last three are single rows of 0.5 m cells, too narrow for a 0.7 m body. They are cell artifacts at the rock mesh edges.
  - (63.0, 278.8) is big enough to stand in. It is a candidate for the Play flood.
- **The W foot band top is a second way out at night.** It can be walked from inside the chute, because at (79, 214) the chute floor is 15.8 and the band top 16 to 17. From there it runs north along x 79 to z 289 and down near (94, 294).

## 3. Colliders as built (within 8 m of the path, plus everything under Ward)
| Collider | Kind | Centre (x, y, z) | Size x by y by z | Visible mesh | Flag |
|---|---|---|---|---|---|
| Ward/CairnGate/GateBlocker | box | (86.00, 13.54, 213.00) | 0.50 x 4.00 x 3.40, z 211.30 to 214.70, y 11.54 to 15.54 | none (IW2) | invisible by design; the chain is the reason. No script switches it off at night (finding 1) |
| Ward/CairnGate/Stone x5 | sphere | (102.93, 9.74 to 11.01, 208.78) | r 0.70 to 0.20 | the cairn | fits |
| Ward/Climb/ChuteSteps/StairRamp x3 | box | (79.43, 15.69, 213.58), (70.08, 21.18, 214.40), (60.74, 26.65, 215.23) | 3.00 x 0.20 x 7.3 | log steps | ramps under the steps, fits the STAIRS RULE |
| Ground815/Pockets/P1Hide | box | (47.60, 27.80, 213.40) | 2.00 x 4.00 x 2.00 | none | a pocket fill, 5.45 m off the tread |
| Ward/Climb/P1_Overhang | convex mesh | (50.91, 29.35, 214.22) | scale 0.55 | rock overhang | P1's hiding place, 2.59 m off the tread |
| Ward/Climb/RunePost, RuneScreen | convex mesh | (55.12, 35.79, 246.09), (56.89, 36.24, 244.79) | small | post, screen | fit |
| Ward/WardPath/Flight3/StairRamp | box | (30.56, 54.42, 253.16) | 2.00 x 0.20 x 18.22, pitched | stringers and treads | 0.35 m over the terrain the whole flight: no space under it |
| Ward/WardPath/Flight3/RailE/RailCollider | box | (31.30, 56.45, 255.18) | 0.16 x 9.30 x 10.60, y 51.81 to 61.10 | a 1.0 m rail | **much bigger than the rail:** a vertical slab up to about 9 m over the treads at the flight's foot |
| Landing1 Rail0, Rail1; Landing2 Rail0; Landing2S Rail0; Lookout RailE | box | landing and lookout edges | 0.16 x 1.10 x 1.46 to 4.00 (Lookout 0.16 x 1.43 x 2.50) | log rails on RailingPost_Wood | fit, 1.1 m |
| Ward/WardPath/Prow/ProwTop | box | (-11.10, 60.00, 246.00) | 2.60 x 4.00 x 5.20, top 62.0 | BigBoulders_1 scaled to the prow | inside the rock |
| Prow RailN, RailS, RailW | box | z 248.6, 243.4; x -12.2 | 0.16 x 1.10 x 2.20 and 5.20 | log rails | fit, 1.1 m (WardPath 2.2) |
| Ward/WardPath/FallenGiant/RunBench/Collider | capsule | x 33.0 to 61.5, y 42.17, z 272.0 | r 1.5 | Sequoia4 laid down, x 29.9 to 63.2 | **much smaller than the log:** at x 40 to 58 the visible log spans y 37.9 to 46.6 and z 267.97 to 275.65 (r about 3.5 to 4); the capsule spans y 40.67 to 43.67, z 270.5 to 273.5 |
| Ward/WardPath/FallenGiant/RunLeg4/Collider | capsule | (22.0, 62.79) to (29.0, 59.50), z 272 | r 1.5 | Sequoia2 laid down | its bottom is 0.07 to 0.53 m over the ground at x 27 to 28; nothing passes under |
| Ward/WardPath/FallenGiant/RimTie | box | (58.75, 38.91, 272.00) | 6.50 x 10.07 x 2.40, y 33.88 to 43.95 | inside the log's east end, below ground | fits |
| Ward/Stones/Stone_1 to 3 | box | (-4, 65.5, 226), (1, 65.5, 223), (-7, 65.5, 221) | 3.60 x 9.00 x 4.00, y 61 to 70 | menhirs (StonesDressing, Effigy) | the boxes' own renderers are off. The menhir meshes did not decode (unverified), so whether the boxes are bigger than the stones is not measured |
| Ward/Climb/SplitSnag_L, _R; RedFir_Bent | capsule | (25.4, 260.2), (25.2, 263.8); (35.4, 264.4) | r 0.54, 0.50; r 0.61 | snag halves; bent fir | fit the trunks |
| Ground815/TrailEdges | convex mesh x6 | along the draw and shelf | under 0.5 m | stones | off the centreline |
| Ground815/ClimbGrounds | capsule x14, convex mesh x4 | the draw | | logs and rubble | off the centreline |
| Forest/Dense/Canopy | capsule x12 | within 8 m | | trunks | off the centreline |

**Meshes without colliders that the body enters on the tread.** These are rocks drawn over the path that a walker passes through. 8.18a strips hulls from Ground815/Stops and Rock/BandScree, and the walk-into check lists those groups apart, so it does not fail them.

| Rock | Position | Tread length the body enters | Deepest | From chainage |
|---|---|---|---|---|
| Ground815/Stops/FaceRock/[BigBoulders_2] | (16.24, 60.75, 265.64) | 4.00 m | 1.25 m | 157.0 |
| Ground815/Stops/FaceRock/[Boulder_2] | (8.10, 61.01, 264.05) | 3.75 m | 0.64 m | 165.6 |
| Ground815/Stops/FaceRock/[Boulder_2] | (13.53, 60.78, 263.98) | 3.25 m | 1.00 m | 158.7 |
| Rock/BandScree/[BigBoulders_4] | (84.20, 13.56, 215.46) | 3.25 m | 0.59 m | 20.0 (the gate) |
| Ground815/Stops/FaceRock/[BigBoulders_2] | (18.67, 60.73, 263.23) | 3.00 m | 0.72 m | 153.3 |
| Ground815/Stops/FaceRock/[Boulder_0] | (10.12, 61.00, 266.90) | 2.75 m | 0.60 m | 163.3 |
| Ground815/Stops/FaceRock/[BigBoulders_1] | (6.13, 61.21, 266.69) | 2.75 m | 0.79 m | 167.2 |
| Ground815/Stops/FaceRock/[Boulder_1] | (20.68, 62.57, 259.98) | 2.50 m | 0.34 m | 151.6 |
| Ground815/Stops/FaceRock/[BigBoulders_3] | (22.67, 60.33, 263.71) | 2.00 m | 0.40 m | 149.6 |
| Ground815/Stops/FaceRock/[BigBoulders_2] | (6.38, 61.16, 263.50) | 1.50 m | 0.16 m | 167.5 |
| Ground815/Stops/FaceRock/[Boulder_0] | (4.66, 61.70, 268.45) | 1.25 m | 0.23 m | 169.6 |
| Ground815/Stops/FaceRock/[Boulder_0] | (14.84, 62.84, 266.88) | 0.75 m | 0.14 m | 160.1 |

## 4. Times, landmarks, hiding places
**Lengths and time:**
- J to the prow on the markers: 198.3 m in plan (doc 199) and 210.9 m along the slope.
- Camp signpost (165, 158) to the first Camp to J marker: 14.8 m. Camp to J: 76.4 m.
- Camp to the prow: 289.5 m in plan (about 302 m along the slope), 116 to 121 s at 2.5 m/s, under the 150 s cap. Doc: 279 m, about 115 s.

**Chainage of each landmark, and its distance off the tread centre:**

| Beat (doc chainage) | Landmark | Chainage | Off the tread |
|---|---|---|---|
| Gate (0 to 20) | Cairn | 2.0 | 2.21 |
| | Chain and GateBlocker | 19.3 | 0.13 |
| | Doc's "old giant at the gap" | none | the nearest giant is SliceLook/GiantTrees/Sequoia1 (88.2, 250.5), 34.3 m off the tread |
| Draw (20 to 54) | ChuteFootLantern | 23.0 | 1.80 |
| | ChuteMidLantern | 47.5 | 1.80 |
| | Seep_Cup | 50.0 (doc 54) | 1.50 |
| | P1Lantern and P1_Overhang | 53.5 | 1.80 and 2.59 |
| Shelf (54 to 97) | RunePost | 81.6 (doc 82) | 1.75 |
| | StairFootLantern | 89.9 | 1.26 |
| | Flight 1 foot | 91.6 | 0.00 |
| | Fallen giant across the old way north | z 272 | in view from the bench |
| Stair (97 to 144) | Landing 1 | 109.8 (doc 114) | 0.00 |
| | Landing1Lantern | 109.6 | 2.25 |
| | RedFir_Bent | 109.8 | 2.45 |
| | Landing 2 | 124.5 (doc 129) | 0.00 |
| | Landing2Lantern | 124.6 | 1.70 |
| | Flight 3 foot | 128.8 | 0.00 |
| Lookout and throat (144 to 179) | Lookout | 144.0 (doc 144) | 0.00 |
| | P4Lantern | 145.5 | 0.89 |
| | SplitSnag_L | 147.5 | 1.74 |
| | Dogleg | 155.1 | 0.34 |
| Ledge (179 to 199) | Fin exit | 178.5 (doc 179) | 0.21 |
| | Prow | 197.9 (doc 199) | 0.00 |

- The largest gap between landmarks is 28.1 m (11 s), from P1Lantern (53.5) to the RunePost (81.6).

**Flight grades:**

| Flight | Measured | Doc |
|---|---|---|
| Flight 1, m 91.6 (35.8) to m 109.8 (44.0) | 8.2 m rise over 18.2 m, 24 degrees | 17 degrees |
| Flight 2, to m 124.5 (49.0) | 19 degrees | 18 degrees |
| Flight 3, to m 144.0 (60.0) | 36 degrees, on its StairRamp | 36 degrees |

**Hiding places:**
- **P1:** P1_Overhang, 2.59 m off the tread at chainage 53.5. Its size was not measured.
- **Landing 1:** RedFir_Bent's trunk capsule runs from (36.15, 47.14, 264.24) to (34.71, 49.24, 264.51). That is 3.1 to 5.2 m over the landing (44.0), and outside its north rail (Rail1 at z 264, x 34 to 38). No covered standing room inside the rails was measured; the crown is not modelled.
- **Landing 2:** the "hollow under flight 3" is not built. The flight's ramp is 0.35 m over the terrain from P138 to P155.
- **A second place above landing 1 (Quill review 3):** none built.

## 5. The day closure
**It stops you at the right place, for a visible reason.**
- IW2 is Ward/CairnGate/GateBlocker at x 86, z 211.30 to 214.70. That is 3.4 m wide; Valley 8 says 3.0 m, z 211.5 to 214.5.
- It sits at chainage 19.3, under the chain at (86, 13.44, 213).

**Day flood.**
- From J alone it stops at P20 (85.32, 213.06).
- I also seeded every trail except the climb west of x 86.5. Neither run reaches the chute (79, 214), P1, either landing, the lookout, the prow or the W foot band top. So with the blocker on, the closure holds on paper.

**"You can't abandon your post."** IW2 speaks "no" in Valley 8, so nothing is required here. That message belongs to IW1 and IW3, which wait for the gate minigame (Main3_BuildNotes 53).

**No night switch.** No script and no component switches the blocker off at night. GateBlocker has only a Transform and a BoxCollider. The build notes (85, 94) say the climb is closed by day and dev warps reach it. Today nobody can walk J to the prow at all (finding 1).

## 6. The deck view (terrain alone, then crown cones)
| Target | Eyes that see it | Margin |
|---|---|---|
| Ward stone tops (y 70) | 0 of 128 | best line 23.8 to 24.9 m under the ground |
| Prow rail top | 0 of 128 | 36.0 m |
| Prow floor +1.7 | 0 of 128 | 35.9 m |
| Ledge floor (0, 250) +1.7 | 0 of 128 | 37.7 m |
| Fin exit +1.7 | 0 of 128 | 26.5 m |
| Valley fires, flame tops at -20 (x -120 to -200) | 0 of 128 | 64.6 to 77.2 m |
| Far front flame tops at 105 (x -250 to -350) | 0 of 128 | 11.1 to 24.3 m |
| Lookout floor +1.7 | 128 of 128 | |
| Split snag top | 128 of 128 | |

The Ward and the fire stay hidden. The lookout is seen, as its own frame of the cab implies.

## 7. Where the build departs from WardPath.md
**Block**
1. **The night walk is closed.**
   - IW2 (GateBlocker) is always on; nothing switches it at night.
   - WardPath beat 0 says "chain down" at night, and Valley 8 says "off at night".
   - 8.29's done-check ("J to the prow at night", "every place in it can be reached ... from its trails") cannot pass in Play.
   - My night flood shows the walk works once the blocker is off.

**Hurt**
2. **Twelve rocks with no collider sit on the tread, eleven of them in the throat (chainage 149.6 to 170.9) and one at the gate (20.0).** The body enters them for 0.75 to 4.0 m of tread, up to 1.25 m deep (table in 3). The walk-into check cannot fail them, because it lists Ground815/Stops and Rock/BandScree apart.
3. **The fallen giant's collider is much smaller than the visible log.** The capsule is r 1.5; the log is r about 3.5 to 4 at x 40 to 58, so a walker reaches 2 to 2.5 m into the bark from either side. It still holds the line: its top is 2.68 m over the bench (doc 1.3 m), and nothing north of z 272 is reachable.
4. **Landing 2 has no hiding place (no hollow under flight 3), and landing 1's is unclear.** The bent fir is outside the north rail, 3 to 5 m up.
5. **The W foot band top is walkable from inside the chute at night,** a second way off the Ward walk (section 2). This is paper only; the hand walk confirms or clears it.
6. **A candidate 3 m² pocket at (63.0, 278.8),** reached by walking the slope east of the shelf rim north from the draw (section 2). This is paper only; the hand walk confirms or clears it.

**Cosmetic**
7. **Flight 3's east rail collider** is a 9.3 m slab behind a 1.0 m rail (invisible wall rule, Valley 8).
8. **Numbers:**
   - Flight 1 is 24 degrees (doc 17).
   - Landmark chainages differ from the beat table by up to 7 m: seep 50 (doc 54), stair-foot lantern 89.9 (doc 97 for the stair), landing 1 109.8 (114), landing 2 124.5 (129).
   - Camp to the prow is 289.5 m (doc 279) and 116 to 121 s (doc 115); still under 150 s.
   - IW2 is 3.4 m wide (Valley 3.0).
9. **The gate has no "old giant at the gap".** Beat 0 rests on the cairn and the chain.
10. **J to Ward markers P2 and P4** float 0.36 m over the ground at J.

Marlow
