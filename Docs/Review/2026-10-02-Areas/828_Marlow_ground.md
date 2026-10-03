# 8.28 ground survey: the old burn, the forage patches, the Jg and T trails
2026-10-02, Marlow. Measurements only, for Sable to draw from. No Editor calls.

Read from files at 265253d: Main3.unity (world transforms and every scene collider; prefab colliders checked in the prefabs) and Main3_TerrainData.asset (check (392, 165) = 3.000).

Scope:
- the burn area as set in main3_areas_setup.cs (x 205 to 335, z 140 to 200), plus 8 m either side of the four burn trails (Camp to Jg, Jg to T, Jg to Camp 1, Camp 2 to T; ForestPlan 71);
- the forage spots: A POI_Forage_patch_A (239.59, 5.01, 163.04), B POI_Forage_patch_B (140.75, 8.29, 167.12), C Places/ForageC (229.50, 4.36, 273.50).

Warps in the area: Old_Burn (239.60, 5.35, 157.80) and Junction_Jg (262.00, 5.20, 168.00). Jg signpost post: (265.00, 169.00).

Method:
- Ground is terrain only. Grid cells are 0.5 m. Slopes are central differences.
- Body: radius 0.35, climb 0.70, slope 45.
- Deck eyes: 64 at 57.6 m and 64 at 58.2 m, over x 160.5 to 167.5, z 162.5 to 169.5.

## 1. Ground along each trail, every 2 m, and 2, 4 and 6 m to each side
Sampling:
- Points are at 2 m steps along the scene trail markers.
- "tread y" is the marker height.
- Columns are offsets perpendicular to travel: + is left of the direction the trail is listed in, - is right.
- Camp to Jg starts at marker P16, the first marker in the scene.

**Camp to Jg**, 106.5 m on the scene markers, from P16 (184.39, 153.51) to P100 (261.60, 171.78).

| m | point | tread y | -6 | -4 | -2 | 0 | +2 | +4 | +6 | 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | 
| 0 | (184.4, 153.5) | 14.93 | 15.00 | 14.96 | 14.93 | 14.92 | 14.93 | 14.88 | 14.56 | 
| 2 | (184.9, 151.6) | 14.82 | 15.00 | 14.92 | 14.84 | 14.83 | 14.83 | 14.73 | 14.39 | 
| 4 | (185.5, 149.7) | 14.66 | 15.00 | 14.83 | 14.69 | 14.68 | 14.69 | 14.58 | 14.25 | 
| 6 | (186.3, 147.8) | 14.41 | 14.99 | 14.69 | 14.45 | 14.41 | 14.38 | 14.27 | 13.88 | 
| 8 | (187.3, 146.1) | 13.97 | 14.43 | 14.18 | 13.99 | 13.96 | 13.95 | 13.88 | 13.48 | 
| 10 | (188.6, 144.7) | 13.49 | 13.65 | 13.59 | 13.46 | 13.48 | 13.53 | 13.67 | 13.78 | 
| 12 | (190.4, 143.8) | 13.00 | 11.55 | 12.25 | 12.92 | 13.00 | 13.12 | 13.67 | 14.15 | 
| 14 | (192.4, 143.7) | 12.51 | 7.33 | 10.15 | 12.35 | 12.51 | 12.60 | 13.21 | 14.19 | 
| 16 | (194.3, 144.4) | 12.01 | 6.16 | 9.32 | 11.92 | 12.02 | 11.92 | 12.10 | 13.72 | 
| 18 | (196.0, 145.3) | 11.53 | 6.00 | 8.80 | 11.36 | 11.53 | 11.45 | 11.70 | 13.58 | 
| 20 | (197.7, 146.3) | 11.04 | 6.00 | 8.48 | 10.86 | 11.03 | 10.98 | 11.26 | 13.28 | 
| 22 | (199.5, 147.3) | 10.55 | 6.00 | 8.27 | 10.41 | 10.54 | 10.48 | 10.91 | 12.99 | 
| 24 | (201.4, 147.9) | 10.06 | 6.00 | 8.10 | 9.99 | 10.05 | 10.00 | 10.62 | 12.61 | 
| 26 | (203.4, 148.2) | 9.57 | 6.00 | 7.82 | 9.42 | 9.57 | 9.66 | 10.42 | 12.49 | 
| 28 | (205.2, 149.0) | 9.13 | 7.17 | 8.21 | 9.14 | 9.22 | 9.28 | 10.28 | 12.40 | 
| 30 | (206.8, 150.2) | 9.47 | 8.49 | 9.02 | 9.41 | 9.45 | 9.54 | 10.72 | 12.52 | 
| 32 | (208.2, 151.6) | 9.93 | 10.02 | 10.07 | 9.95 | 9.94 | 10.01 | 11.03 | 12.41 | 
| 34 | (209.5, 153.1) | 10.43 | 10.33 | 10.56 | 10.46 | 10.44 | 10.47 | 11.18 | 12.22 | 
| 36 | (211.0, 154.5) | 10.78 | 10.22 | 10.65 | 10.82 | 10.83 | 10.86 | 11.24 | 11.88 | 
| 38 | (212.7, 155.5) | 10.75 | 10.24 | 10.51 | 10.72 | 10.74 | 10.75 | 10.92 | 11.25 | 
| 40 | (214.6, 155.8) | 10.37 | 10.39 | 10.47 | 10.41 | 10.38 | 10.38 | 10.37 | 10.33 | 
| 42 | (216.5, 155.2) | 9.96 | 10.38 | 10.19 | 10.01 | 9.98 | 9.97 | 9.84 | 9.54 | 
| 44 | (218.3, 154.3) | 9.58 | 10.16 | 9.82 | 9.62 | 9.59 | 9.57 | 9.39 | 8.98 | 
| 46 | (219.9, 153.2) | 9.21 | 9.79 | 9.45 | 9.22 | 9.22 | 9.22 | 9.03 | 8.61 | 
| 48 | (221.6, 152.0) | 8.83 | 9.44 | 9.06 | 8.85 | 8.84 | 8.83 | 8.63 | 8.18 | 
| 50 | (223.2, 150.9) | 8.46 | 9.00 | 8.64 | 8.46 | 8.46 | 8.46 | 8.27 | 7.84 | 
| 52 | (224.9, 149.8) | 8.06 | 8.60 | 8.28 | 8.08 | 8.07 | 8.05 | 7.84 | 7.42 | 
| 54 | (226.6, 148.9) | 7.66 | 8.01 | 7.77 | 7.65 | 7.65 | 7.65 | 7.49 | 7.14 | 
| 56 | (228.5, 148.0) | 7.24 | 7.23 | 7.20 | 7.20 | 7.24 | 7.28 | 7.23 | 7.01 | 
| 58 | (230.4, 147.6) | 6.84 | 6.73 | 6.82 | 6.87 | 6.84 | 6.75 | 6.54 | 6.44 | 
| 60 | (232.4, 147.9) | 6.49 | 6.24 | 6.40 | 6.51 | 6.49 | 6.43 | 6.37 | 6.51 | 
| 62 | (234.1, 148.8) | 6.21 | 5.96 | 6.14 | 6.22 | 6.22 | 6.20 | 6.32 | 6.72 | 
| 64 | (235.5, 150.3) | 5.99 | 5.83 | 5.96 | 6.00 | 5.99 | 6.00 | 6.16 | 6.73 | 
| 66 | (236.5, 152.0) | 5.78 | 5.80 | 5.81 | 5.79 | 5.79 | 5.79 | 5.92 | 6.48 | 
| 68 | (237.4, 153.7) | 5.58 | 5.71 | 5.62 | 5.58 | 5.58 | 5.58 | 5.65 | 6.01 | 
| 70 | (238.3, 155.5) | 5.38 | 5.58 | 5.41 | 5.37 | 5.38 | 5.38 | 5.37 | 5.54 | 
| 72 | (239.3, 157.3) | 5.20 | 5.49 | 5.24 | 5.19 | 5.20 | 5.20 | 5.14 | 5.17 | 
| 74 | (240.6, 158.8) | 5.08 | 5.47 | 5.17 | 5.07 | 5.07 | 5.07 | 5.04 | 5.00 | 
| 76 | (242.1, 160.1) | 5.02 | 5.46 | 5.12 | 5.02 | 5.02 | 5.01 | 5.01 | 5.00 | 
| 78 | (243.2, 161.7) | 5.03 | 5.47 | 5.12 | 5.02 | 5.02 | 5.02 | 5.01 | 5.00 | 
| 80 | (243.9, 163.6) | 5.08 | 5.54 | 5.18 | 5.08 | 5.07 | 5.07 | 5.03 | 5.00 | 
| 82 | (244.2, 165.5) | 5.17 | 5.57 | 5.26 | 5.16 | 5.16 | 5.16 | 5.09 | 5.00 | 
| 84 | (244.5, 167.5) | 5.28 | 5.59 | 5.39 | 5.28 | 5.28 | 5.27 | 5.16 | 5.08 | 
| 86 | (244.8, 169.5) | 5.41 | 5.60 | 5.50 | 5.41 | 5.42 | 5.42 | 5.37 | 5.39 | 
| 88 | (245.2, 171.4) | 5.57 | 5.51 | 5.66 | 5.60 | 5.57 | 5.56 | 5.59 | 5.72 | 
| 90 | (246.1, 173.2) | 5.71 | 5.49 | 5.65 | 5.73 | 5.71 | 5.71 | 5.86 | 6.14 | 
| 92 | (247.6, 174.3) | 5.74 | 5.62 | 5.66 | 5.73 | 5.75 | 5.76 | 5.89 | 6.08 | 
| 94 | (249.6, 174.3) | 5.65 | 5.50 | 5.65 | 5.66 | 5.66 | 5.67 | 5.76 | 5.94 | 
| 96 | (251.5, 173.9) | 5.51 | 5.61 | 5.58 | 5.52 | 5.52 | 5.51 | 5.54 | 5.62 | 
| 98 | (253.4, 173.3) | 5.37 | 5.57 | 5.42 | 5.37 | 5.37 | 5.37 | 5.35 | 5.37 | 
| 100 | (255.3, 172.6) | 5.25 | 5.47 | 5.28 | 5.25 | 5.24 | 5.24 | 5.18 | 5.25 | 
| 102 | (257.2, 172.0) | 5.15 | 5.29 | 5.14 | 5.14 | 5.14 | 5.14 | 5.12 | 5.20 | 
| 104 | (259.2, 171.5) | 5.07 | 5.20 | 5.07 | 5.07 | 5.07 | 5.04 | 5.07 | 5.13 | 
| 106 | (261.2, 171.7) | 5.01 | 5.08 | 5.01 | 5.01 | 5.01 | 5.02 | 5.07 | 5.14 | 

**Jg to T**, 104.5 m on the scene markers, from P0 (262.00, 172.00) to P84 (339.93, 169.94).

| m | point | tread y | -6 | -4 | -2 | 0 | +2 | +4 | +6 | 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | 
| 0 | (262.0, 172.0) | 5.00 | 5.06 | 5.00 | 5.00 | 5.00 | 5.02 | 5.08 | 5.15 | 
| 2 | (263.2, 173.6) | 5.02 | 5.08 | 5.01 | 5.02 | 5.02 | 5.03 | 5.09 | 5.17 | 
| 4 | (263.9, 175.5) | 5.06 | 5.15 | 5.06 | 5.06 | 5.06 | 5.06 | 5.12 | 5.20 | 
| 6 | (264.6, 177.4) | 5.12 | 5.23 | 5.13 | 5.12 | 5.12 | 5.12 | 5.18 | 5.28 | 
| 8 | (265.5, 179.1) | 5.20 | 5.25 | 5.19 | 5.19 | 5.20 | 5.20 | 5.29 | 5.54 | 
| 10 | (266.8, 180.6) | 5.28 | 5.19 | 5.20 | 5.26 | 5.28 | 5.30 | 5.43 | 5.78 | 
| 12 | (268.6, 181.5) | 5.37 | 5.34 | 5.41 | 5.39 | 5.37 | 5.36 | 5.48 | 5.77 | 
| 14 | (270.6, 181.4) | 5.43 | 5.22 | 5.37 | 5.44 | 5.43 | 5.43 | 5.49 | 5.58 | 
| 16 | (272.4, 180.7) | 5.47 | 5.19 | 5.37 | 5.46 | 5.47 | 5.47 | 5.49 | 5.50 | 
| 18 | (274.1, 179.7) | 5.49 | 5.22 | 5.41 | 5.48 | 5.48 | 5.48 | 5.48 | 5.46 | 
| 20 | (275.6, 178.4) | 5.46 | 5.31 | 5.44 | 5.47 | 5.47 | 5.47 | 5.47 | 5.44 | 
| 22 | (277.1, 177.1) | 5.40 | 5.43 | 5.45 | 5.41 | 5.40 | 5.40 | 5.23 | 5.09 | 
| 24 | (278.8, 176.0) | 5.25 | 5.66 | 5.41 | 5.25 | 5.24 | 5.25 | 5.16 | 5.34 | 
| 26 | (280.7, 176.0) | 4.99 | 4.97 | 4.94 | 4.98 | 5.00 | 5.09 | 5.39 | 5.47 | 
| 28 | (282.4, 177.0) | 4.74 | 4.20 | 4.48 | 4.72 | 4.74 | 4.77 | 5.14 | 5.46 | 
| 30 | (283.9, 178.3) | 4.55 | 4.01 | 4.29 | 4.55 | 4.56 | 4.57 | 4.86 | 5.41 | 
| 32 | (285.5, 179.5) | 4.45 | 4.00 | 4.21 | 4.42 | 4.44 | 4.45 | 4.74 | 5.36 | 
| 34 | (287.1, 180.7) | 4.36 | 4.00 | 4.19 | 4.35 | 4.36 | 4.36 | 4.67 | 5.34 | 
| 36 | (288.9, 181.7) | 4.33 | 4.00 | 4.17 | 4.31 | 4.32 | 4.33 | 4.67 | 5.36 | 
| 38 | (290.8, 182.1) | 4.34 | 4.00 | 4.18 | 4.33 | 4.34 | 4.34 | 4.67 | 5.36 | 
| 40 | (292.7, 182.0) | 4.41 | 4.00 | 4.20 | 4.39 | 4.41 | 4.43 | 4.75 | 5.28 | 
| 42 | (294.6, 181.4) | 4.52 | 4.01 | 4.28 | 4.50 | 4.51 | 4.52 | 4.80 | 5.23 | 
| 44 | (296.6, 181.0) | 4.64 | 4.19 | 4.43 | 4.62 | 4.63 | 4.64 | 4.87 | 5.17 | 
| 46 | (298.6, 180.6) | 4.75 | 4.44 | 4.62 | 4.74 | 4.75 | 4.76 | 4.91 | 5.08 | 
| 48 | (300.5, 180.0) | 4.84 | 4.62 | 4.76 | 4.83 | 4.84 | 4.85 | 4.90 | 4.96 | 
| 50 | (302.3, 179.3) | 4.88 | 4.73 | 4.84 | 4.87 | 4.87 | 4.88 | 4.86 | 4.82 | 
| 52 | (304.0, 178.1) | 4.87 | 4.81 | 4.89 | 4.87 | 4.87 | 4.86 | 4.80 | 4.69 | 
| 54 | (305.3, 176.7) | 4.83 | 4.89 | 4.89 | 4.84 | 4.83 | 4.83 | 4.74 | 4.60 | 
| 56 | (306.4, 175.0) | 4.80 | 4.99 | 4.88 | 4.81 | 4.81 | 4.80 | 4.72 | 4.57 | 
| 58 | (307.2, 173.2) | 4.79 | 5.01 | 4.87 | 4.79 | 4.79 | 4.79 | 4.71 | 4.54 | 
| 60 | (307.9, 171.3) | 4.79 | 5.00 | 4.86 | 4.79 | 4.78 | 4.78 | 4.70 | 4.55 | 
| 62 | (308.4, 169.4) | 4.78 | 5.00 | 4.86 | 4.79 | 4.78 | 4.78 | 4.71 | 4.57 | 
| 64 | (308.9, 167.4) | 4.78 | 4.99 | 4.85 | 4.78 | 4.78 | 4.78 | 4.72 | 4.59 | 
| 66 | (309.6, 165.5) | 4.78 | 4.96 | 4.84 | 4.78 | 4.77 | 4.77 | 4.72 | 4.60 | 
| 68 | (310.3, 163.7) | 4.76 | 4.94 | 4.83 | 4.77 | 4.76 | 4.76 | 4.71 | 4.62 | 
| 70 | (311.4, 162.0) | 4.75 | 4.86 | 4.78 | 4.75 | 4.74 | 4.74 | 4.70 | 4.61 | 
| 72 | (312.8, 160.6) | 4.72 | 4.74 | 4.73 | 4.72 | 4.72 | 4.72 | 4.68 | 4.61 | 
| 74 | (314.6, 159.9) | 4.68 | 4.62 | 4.67 | 4.68 | 4.69 | 4.69 | 4.68 | 4.63 | 
| 76 | (316.6, 159.8) | 4.65 | 4.56 | 4.63 | 4.65 | 4.65 | 4.66 | 4.66 | 4.63 | 
| 78 | (318.6, 160.1) | 4.62 | 4.55 | 4.60 | 4.62 | 4.62 | 4.63 | 4.62 | 4.61 | 
| 80 | (320.4, 160.9) | 4.60 | 4.56 | 4.59 | 4.60 | 4.60 | 4.60 | 4.59 | 4.56 | 
| 82 | (322.2, 161.8) | 4.57 | 4.57 | 4.58 | 4.58 | 4.58 | 4.57 | 4.56 | 4.52 | 
| 84 | (324.0, 162.7) | 4.55 | 4.57 | 4.56 | 4.55 | 4.55 | 4.55 | 4.53 | 4.47 | 
| 86 | (325.8, 163.6) | 4.52 | 4.57 | 4.55 | 4.53 | 4.53 | 4.52 | 4.48 | 4.39 | 
| 88 | (327.7, 164.2) | 4.48 | 4.56 | 4.51 | 4.48 | 4.49 | 4.49 | 4.45 | 4.37 | 
| 90 | (329.4, 165.2) | 4.43 | 3.79 | 4.29 | 4.42 | 4.43 | 4.46 | 4.49 | 4.47 | 
| 92 | (330.3, 167.0) | 4.36 | 3.40 | 4.18 | 4.36 | 4.36 | 4.36 | 4.37 | 4.37 | 
| 94 | (331.0, 168.8) | 4.26 | 3.31 | 3.61 | 4.17 | 4.26 | 4.27 | 4.30 | 4.33 | 
| 96 | (331.9, 170.5) | 4.07 | 3.42 | 3.74 | 3.97 | 4.05 | 4.06 | 4.12 | 4.15 | 
| 98 | (333.7, 170.7) | 3.78 | 4.39 | 4.32 | 3.99 | 3.80 | 3.77 | 3.69 | 3.48 | 
| 100 | (335.6, 170.2) | 3.57 | 4.15 | 3.82 | 3.59 | 3.59 | 3.58 | 3.42 | 3.19 | 
| 102 | (337.5, 169.6) | 3.46 | 3.35 | 3.17 | 3.41 | 3.43 | 3.33 | 3.12 | 3.02 | 
| 104 | (339.5, 169.9) | 3.09 | 3.00 | 3.01 | 3.02 | 3.10 | 3.11 | 3.08 | 3.07 | 

**Jg to Camp 1**, 81.2 m on the scene markers, from P0 (262.00, 172.00) to P68 (281.41, 236.42).

| m | point | tread y | -6 | -4 | -2 | 0 | +2 | +4 | +6 | 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | 
| 0 | (262.0, 172.0) | 5.00 | 5.05 | 5.02 | 5.01 | 5.00 | 5.04 | 5.11 | 5.18 | 
| 2 | (261.7, 174.0) | 5.02 | 5.07 | 5.05 | 5.03 | 5.02 | 5.04 | 5.14 | 5.23 | 
| 4 | (260.9, 175.8) | 5.06 | 5.19 | 5.12 | 5.08 | 5.07 | 5.07 | 5.16 | 5.25 | 
| 6 | (259.9, 177.5) | 5.15 | 5.24 | 5.17 | 5.15 | 5.15 | 5.14 | 5.20 | 5.32 | 
| 8 | (258.9, 179.3) | 5.27 | 5.27 | 5.25 | 5.27 | 5.27 | 5.27 | 5.29 | 5.43 | 
| 10 | (258.1, 181.1) | 5.42 | 5.30 | 5.39 | 5.41 | 5.42 | 5.43 | 5.52 | 5.75 | 
| 12 | (257.9, 183.1) | 5.58 | 5.26 | 5.43 | 5.55 | 5.58 | 5.60 | 5.75 | 6.00 | 
| 14 | (258.5, 184.9) | 5.71 | 5.24 | 5.49 | 5.67 | 5.71 | 5.72 | 5.85 | 6.03 | 
| 16 | (259.7, 186.5) | 5.78 | 5.26 | 5.62 | 5.76 | 5.77 | 5.78 | 5.88 | 6.03 | 
| 18 | (261.3, 187.7) | 5.79 | 5.32 | 5.68 | 5.80 | 5.80 | 5.80 | 5.87 | 6.02 | 
| 20 | (263.0, 188.8) | 5.80 | 5.36 | 5.70 | 5.80 | 5.80 | 5.80 | 5.87 | 5.99 | 
| 22 | (264.7, 189.8) | 5.79 | 5.43 | 5.72 | 5.78 | 5.78 | 5.79 | 5.85 | 5.97 | 
| 24 | (266.4, 190.9) | 5.77 | 5.50 | 5.70 | 5.77 | 5.77 | 5.77 | 5.82 | 5.94 | 
| 26 | (268.1, 192.0) | 5.75 | 5.57 | 5.69 | 5.75 | 5.75 | 5.75 | 5.81 | 5.92 | 
| 28 | (269.5, 193.4) | 5.73 | 5.57 | 5.67 | 5.73 | 5.73 | 5.74 | 5.79 | 5.92 | 
| 30 | (270.3, 195.2) | 5.71 | 5.63 | 5.68 | 5.71 | 5.71 | 5.72 | 5.78 | 5.91 | 
| 32 | (270.2, 197.2) | 5.67 | 5.50 | 5.62 | 5.67 | 5.68 | 5.69 | 5.78 | 5.90 | 
| 34 | (269.6, 199.1) | 5.62 | 5.34 | 5.52 | 5.61 | 5.62 | 5.63 | 5.74 | 5.93 | 
| 36 | (268.8, 200.9) | 5.53 | 5.23 | 5.43 | 5.53 | 5.53 | 5.54 | 5.69 | 5.95 | 
| 38 | (268.2, 202.8) | 5.43 | 5.13 | 5.31 | 5.42 | 5.43 | 5.44 | 5.59 | 5.90 | 
| 40 | (267.9, 204.8) | 5.31 | 5.10 | 5.21 | 5.29 | 5.31 | 5.32 | 5.43 | 5.69 | 
| 42 | (268.0, 206.7) | 5.19 | 5.09 | 5.15 | 5.19 | 5.19 | 5.18 | 5.22 | 5.34 | 
| 44 | (268.1, 208.7) | 5.10 | 5.00 | 5.05 | 5.10 | 5.10 | 5.10 | 5.15 | 5.30 | 
| 46 | (267.7, 210.7) | 5.05 | 5.00 | 5.02 | 5.04 | 5.04 | 5.04 | 5.09 | 5.25 | 
| 48 | (267.1, 212.6) | 5.01 | 5.00 | 5.01 | 5.01 | 5.01 | 5.01 | 5.04 | 5.16 | 
| 50 | (266.6, 214.5) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.01 | 5.05 | 
| 52 | (266.3, 216.5) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.01 | 
| 54 | (266.8, 218.5) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 56 | (268.0, 220.0) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 58 | (269.7, 221.1) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 60 | (271.5, 221.8) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 62 | (273.4, 222.5) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 64 | (275.4, 223.0) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 66 | (277.3, 223.6) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 68 | (279.1, 224.4) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 70 | (280.8, 225.5) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 72 | (281.8, 227.2) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 74 | (281.9, 229.2) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 76 | (281.7, 231.2) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 78 | (281.4, 233.2) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 
| 80 | (281.4, 235.2) | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 5.00 | 

**Camp 2 to T**, 92.8 m on the scene markers, from P0 (298.90, 107.80) to P78 (339.80, 169.35).

| m | point | tread y | -6 | -4 | -2 | 0 | +2 | +4 | +6 | 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | 
| 0 | (298.9, 107.8) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 2 | (300.4, 107.5) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 4 | (300.7, 109.5) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 6 | (300.4, 111.4) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 8 | (300.0, 113.4) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 10 | (299.8, 115.4) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 12 | (300.6, 117.0) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 14 | (302.4, 117.8) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 16 | (304.4, 118.1) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 18 | (306.3, 118.5) | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 4.00 | 
| 20 | (308.0, 119.6) | 4.01 | 4.05 | 4.01 | 4.01 | 4.01 | 4.01 | 4.00 | 4.00 | 
| 22 | (308.6, 121.5) | 4.02 | 4.15 | 4.06 | 4.02 | 4.02 | 4.01 | 4.00 | 4.00 | 
| 24 | (308.6, 123.5) | 4.04 | 4.17 | 4.08 | 4.04 | 4.04 | 4.04 | 4.02 | 4.00 | 
| 26 | (308.4, 125.5) | 4.06 | 4.18 | 4.12 | 4.07 | 4.06 | 4.06 | 4.04 | 4.00 | 
| 28 | (308.6, 127.5) | 4.10 | 4.17 | 4.15 | 4.11 | 4.10 | 4.10 | 4.07 | 4.04 | 
| 30 | (309.7, 129.0) | 4.13 | 4.16 | 4.13 | 4.13 | 4.13 | 4.13 | 4.15 | 4.20 | 
| 32 | (311.5, 129.7) | 4.15 | 4.09 | 4.12 | 4.15 | 4.15 | 4.15 | 4.17 | 4.22 | 
| 34 | (313.5, 130.1) | 4.16 | 4.16 | 4.17 | 4.16 | 4.16 | 4.16 | 4.17 | 4.19 | 
| 36 | (315.4, 130.2) | 4.17 | 4.21 | 4.19 | 4.17 | 4.17 | 4.17 | 4.16 | 4.17 | 
| 38 | (317.4, 130.4) | 4.17 | 4.24 | 4.19 | 4.16 | 4.16 | 4.16 | 4.16 | 4.15 | 
| 40 | (319.4, 130.7) | 4.16 | 4.22 | 4.18 | 4.15 | 4.16 | 4.16 | 4.15 | 4.15 | 
| 42 | (321.3, 131.5) | 4.14 | 4.19 | 4.16 | 4.15 | 4.14 | 4.14 | 4.13 | 4.14 | 
| 44 | (322.4, 133.0) | 4.13 | 4.09 | 4.12 | 4.13 | 4.13 | 4.13 | 4.14 | 4.15 | 
| 46 | (322.9, 135.0) | 4.13 | 4.07 | 4.11 | 4.13 | 4.13 | 4.13 | 4.13 | 4.15 | 
| 48 | (322.9, 137.0) | 4.14 | 4.10 | 4.13 | 4.14 | 4.14 | 4.14 | 4.14 | 4.15 | 
| 50 | (322.6, 138.9) | 4.16 | 4.15 | 4.16 | 4.16 | 4.16 | 4.16 | 4.16 | 4.16 | 
| 52 | (322.3, 140.9) | 4.19 | 4.19 | 4.19 | 4.19 | 4.19 | 4.19 | 4.19 | 4.20 | 
| 54 | (322.1, 142.9) | 4.23 | 4.22 | 4.22 | 4.23 | 4.23 | 4.23 | 4.24 | 4.26 | 
| 56 | (322.1, 144.9) | 4.27 | 4.22 | 4.24 | 4.27 | 4.28 | 4.28 | 4.31 | 4.36 | 
| 58 | (322.8, 146.8) | 4.32 | 4.22 | 4.26 | 4.31 | 4.32 | 4.33 | 4.38 | 4.48 | 
| 60 | (324.2, 148.2) | 4.37 | 4.22 | 4.29 | 4.35 | 4.36 | 4.37 | 4.43 | 4.53 | 
| 62 | (326.0, 149.1) | 4.39 | 4.24 | 4.34 | 4.38 | 4.38 | 4.39 | 4.44 | 4.54 | 
| 64 | (327.9, 149.8) | 4.33 | 4.25 | 4.32 | 4.34 | 4.34 | 4.35 | 4.43 | 4.56 | 
| 66 | (329.8, 150.3) | 4.23 | 4.21 | 4.27 | 4.24 | 4.24 | 4.24 | 4.38 | 4.56 | 
| 68 | (331.7, 150.8) | 4.07 | 3.97 | 4.08 | 4.08 | 4.08 | 4.09 | 4.30 | 4.56 | 
| 70 | (333.7, 151.3) | 3.88 | 3.59 | 3.81 | 3.88 | 3.88 | 3.89 | 4.08 | 4.38 | 
| 72 | (335.6, 151.8) | 3.67 | 3.17 | 3.47 | 3.65 | 3.66 | 3.68 | 3.82 | 4.12 | 
| 74 | (337.5, 152.6) | 3.44 | 3.01 | 3.25 | 3.44 | 3.43 | 3.41 | 3.45 | 3.69 | 
| 76 | (339.2, 153.6) | 3.22 | 3.00 | 3.11 | 3.21 | 3.21 | 3.19 | 3.25 | 3.57 | 
| 78 | (340.5, 155.0) | 3.07 | 3.00 | 3.03 | 3.06 | 3.07 | 3.06 | 3.18 | 3.62 | 
| 80 | (341.3, 156.8) | 3.01 | 3.00 | 3.00 | 3.01 | 3.01 | 3.01 | 3.16 | 3.67 | 
| 82 | (341.4, 158.8) | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.14 | 3.67 | 
| 84 | (341.1, 160.7) | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.15 | 3.70 | 
| 86 | (340.6, 162.7) | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.00 | 3.19 | 3.80 | 
| 88 | (340.1, 164.6) | 3.00 | 3.00 | 3.00 | 3.01 | 3.01 | 3.01 | 3.24 | 3.90 | 
| 90 | (339.8, 166.6) | 3.00 | 3.00 | 3.00 | 3.01 | 3.01 | 3.01 | 3.37 | 4.19 | 
| 92 | (339.7, 168.6) | 3.01 | 3.00 | 3.00 | 3.01 | 3.05 | 3.36 | 3.55 | 3.78 | 

## 2. Slopes, drops and pockets
**Ground over 30 degrees** (connected 0.5 m cells inside the scope):

| Centre | Extent | Area m2 | Max deg | Cells over 45 (0.25 m2 each) | Ground range | Nearest trail m | Trail |
|---|---|---|---|---|---|---|---|
| (197.8, 141.5) | x 181.5 to 211.5, z 136.0 to 147.0 | 104.75 | 79 | 228 | 6.00 to 14.16 | 2.0 | Camp to Jg |
| (198.6, 151.6) | x 190.5 to 207.5, z 146.5 to 157.5 | 43.25 | 62 | 60 | 9.65 to 13.79 | 2.7 | Camp to Jg |
| (284.0, 184.0) | x 284.0 to 284.0, z 184.0 to 184.0 | 0.25 | 30 | 0 | 4.90 to 4.90 | 4.4 | Jg to T |
| (285.0, 184.5) | x 285.0 to 285.0, z 184.5 to 184.5 | 0.25 | 30 | 0 | 4.88 to 4.88 | 4.3 | Jg to T |
| (285.5, 185.0) | x 285.5 to 285.5, z 185.0 to 185.0 | 0.25 | 33 | 0 | 4.90 to 4.90 | 4.5 | Jg to T |
| (286.0, 185.5) | x 286.0 to 286.0, z 185.5 to 185.5 | 0.25 | 31 | 0 | 4.95 to 4.95 | 4.7 | Jg to T |
| (287.5, 186.0) | x 287.5 to 287.5, z 186.0 to 186.0 | 0.25 | 31 | 0 | 4.99 to 4.99 | 4.4 | Jg to T |
| (290.8, 186.5) | x 290.5 to 291.0, z 186.5 to 186.5 | 0.50 | 30 | 0 | 4.90 to 4.90 | 4.3 | Jg to T |

**Drops over 1 m** (connected cells over 45 degrees, with the ground range taken across the cells and their neighbours):

| Centre | Extent | Ground range | Drop m | Max deg | Nearest trail m | Trail |
|---|---|---|---|---|---|---|
| (194.9, 140.9) | x 181.5 to 208.5, z 136.0 to 145.5 | 6.00 to 14.28 | 8.28 | 79 | 2.5 | Camp to Jg |
| (190.0, 139.5) | x 190.0 to 190.0, z 139.5 to 139.5 | 11.53 to 12.62 | 1.09 | 45 | 4.3 | Camp to Jg |
| (191.5, 148.0) | x 191.5 to 191.5, z 148.0 to 148.0 | 12.48 to 13.74 | 1.26 | 46 | 4.3 | Camp to Jg |
| (198.8, 151.7) | x 192.0 to 204.5, z 148.5 to 154.5 | 9.88 to 13.68 | 3.80 | 62 | 4.0 | Camp to Jg |
| (205.0, 155.0) | x 205.0 to 205.0, z 155.0 to 155.0 | 11.00 to 12.29 | 1.29 | 54 | 4.6 | Camp to Jg |
| (205.5, 155.5) | x 205.5 to 205.5, z 155.5 to 155.5 | 11.06 to 12.22 | 1.16 | 50 | 4.6 | Camp to Jg |
| (206.0, 156.0) | x 206.0 to 206.0, z 156.0 to 156.0 | 11.10 to 12.14 | 1.05 | 45 | 4.6 | Camp to Jg |

**Pockets.** A pocket is a set of cells you can reach from a trail but cannot walk back out of.
- **Reach:** you can walk up at most 44 degrees per cell, and you can drop down any height.
- **Walls:** a cell counts as wall if any scene collider fills its 0.75 to 1.8 m band. That covers boxes, capsules (with the Gate Tree and Hollow Giant cylinders, r 4.0 and 4.5) and mesh colliders, which are taken by their bounding box.

One pocket was found:

| Centre | Extent | Area m2 | Ground range | Nearest trail m | Trail | Colliders round it |
|---|---|---|---|---|---|---|
| (188.2, 137.1) | x 185.0 to 191.5, z 136.0 to 138.5 | 12.00 | 7.54 to 11.04 | 6.0 | Camp to Jg | Ground815/Stops/Hedge_Burn_4/HedgeCollider, Ground815/Stops/Hedge_Burn_1/HedgeCollider, Ground815/Pockets/CampStep |

## 3. Colliders in the scope, as built (trunk capsules are in table 4)
How the columns were measured:
- **Size** is the world box (full size). Mesh colliders show their bounding box; the GraniteStack's real shape is in 825_Marlow_paper.md.
- **Inflated** applies to a prop box made by PlaceKit.FitCollider on a prop turned off 0, 90, 180 or 270. The true footprint is solved from W = w + d sin 2a.
- **Top over ground** is the collider top minus the ground at its centre.

| Collider | Kind | Centre (x, y, z) | Size x by y by z | Yaw | Inflated by a turned prop | Top over ground |
|---|---|---|---|---|---|---|
| Camp/Dressing/Rocks/[CS_Rock_1] | box | (188.26, 14.91, 154.64) | 3.24 x 0.90 x 3.24 | 39 | yes, yaw 39: mesh about 1.64 x 1.64 | 0.49 |
| Campsites/Camp_1/Dressing/Cookfire/[CS_Chair_1] | box | (274.40, 5.60, 233.40) | 1.50 x 1.20 x 1.50 | 130 | yes, yaw 130: mesh about 0.76 x 0.76 | 1.20 |
| Campsites/Camp_1/Dressing/Cookfire/[CS_Chair_3] | box | (277.70, 5.48, 233.50) | 1.20 x 0.96 x 1.20 | 220 | yes, yaw 220: mesh about 0.60 x 0.60 | 0.96 |
| Campsites/Camp_1/Dressing/KidTable/[CS_Log_Stool_1] | box | (285.97, 5.20, 236.25) | 0.52 x 0.40 x 0.52 | 295 | yes, yaw 295: mesh about 0.29 x 0.29 | 0.40 |
| Campsites/Camp_1/Dressing/KidTable/[CS_Log_Stool_2] | box | (284.48, 5.20, 235.67) | 0.52 x 0.40 x 0.52 | 25 | yes, yaw 25: mesh about 0.29 x 0.29 | 0.40 |
| Campsites/Camp_1/Dressing/KidTable/[CS_Table_Small_Modern_1] | box | (285.20, 5.39, 236.50) | 1.60 x 0.78 x 1.60 | 25 | yes, yaw 25: mesh about 0.91 x 0.91 | 0.78 |
| Campsites/Camp_1/Dressing/KidTable/Layout824/[CS_Chair_3] | box | (283.93, 5.48, 237.09) | 0.60 x 0.96 x 0.62 | 115 | yes, yaw 115: mesh about 0.30 x 0.39 | 0.96 |
| Campsites/Camp_1/Dressing/Spar/[CITW_Log_1] | mesh | (284.00, 5.61, 241.20) | aabb 1.40 x 1.39 x 1.00 | 0 | convex hull | 1.30 |
| Campsites/Camp_1/Dressing/Spar/[CITW_Log_1] | mesh | (282.96, 5.61, 239.40) | aabb 1.56 x 1.39 x 1.70 | 240 | convex hull | 1.30 |
| Campsites/Camp_1/Dressing/Spar/[CITW_Log_1] | mesh | (285.04, 5.61, 239.40) | aabb 1.56 x 1.39 x 1.70 | 120 | convex hull | 1.30 |
| Campsites/Camp_2/Dressing/Payphone/BoothSide | box | (300.00, 5.40, 99.67) | 0.12 x 2.80 x 1.34 | 270 | no (not a fitted prop box) | 2.81 |
| Campsites/Camp_2/Dressing/StartBlaze/PostCollider | box | (301.98, 4.90, 112.60) | 0.16 x 1.80 x 0.16 | 0 | no (not a fitted prop box) | 1.80 |
| Campsites/Camp_2/GraniteStack | mesh | (292.00, 4.00, 108.00) | aabb 1.00 x 1.00 x 1.00 | 0 | exact mesh | 0.50 |
| Campsites/Camp_2/StackPath/LandingN1 | box | (299.65, 8.90, 118.75) | 2.90 x 0.20 x 1.50 | 0 | no (not a fitted prop box) | 5.00 |
| Campsites/Camp_2/StackPath/LandingN1_RailE | box | (301.07, 9.50, 118.75) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 6.00 |
| Campsites/Camp_2/StackPath/LandingN1_RailN | box | (299.65, 9.50, 119.47) | 2.90 x 1.00 x 0.06 | 0 | no (not a fitted prop box) | 6.00 |
| Campsites/Camp_2/StackPath/LandingN1_RailW | box | (298.23, 9.50, 118.75) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 6.00 |
| Campsites/Camp_2/StackPath/LandingN2 | box | (299.65, 18.90, 118.75) | 2.90 x 0.20 x 1.50 | 0 | no (not a fitted prop box) | 15.00 |
| Campsites/Camp_2/StackPath/LandingN2_RailE | box | (301.07, 19.50, 118.75) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 16.00 |
| Campsites/Camp_2/StackPath/LandingN2_RailN | box | (299.65, 19.50, 119.47) | 2.90 x 1.00 x 0.06 | 0 | no (not a fitted prop box) | 16.00 |
| Campsites/Camp_2/StackPath/LandingN2_RailW | box | (298.23, 19.50, 118.75) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 16.00 |
| Campsites/Camp_2/StackPath/LandingS1 | box | (299.65, 13.90, 107.25) | 2.90 x 0.20 x 1.50 | 0 | no (not a fitted prop box) | 10.00 |
| Campsites/Camp_2/StackPath/LandingS1_RailE | box | (301.07, 14.50, 107.25) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 11.00 |
| Campsites/Camp_2/StackPath/LandingS1_RailS | box | (299.65, 14.50, 106.53) | 2.90 x 1.00 x 0.06 | 0 | no (not a fitted prop box) | 11.00 |
| Campsites/Camp_2/StackPath/LandingS1_RailW | box | (298.23, 14.50, 107.25) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 11.00 |
| Campsites/Camp_2/StackPath/LandingTop | box | (298.30, 23.90, 107.25) | 5.60 x 0.20 x 1.50 | 0 | no (not a fitted prop box) | 20.00 |
| Campsites/Camp_2/StackPath/LandingTop_RailE | box | (301.07, 24.50, 107.25) | 0.06 x 1.00 x 1.50 | 0 | no (not a fitted prop box) | 21.00 |
| Campsites/Camp_2/StackPath/LandingTop_RailOverLaneA | box | (298.90, 24.50, 107.97) | 1.40 x 1.00 x 0.06 | 0 | no (not a fitted prop box) | 21.00 |
| Campsites/Camp_2/StackPath/LandingTop_RailS | box | (298.30, 24.50, 106.53) | 5.60 x 1.00 x 0.06 | 0 | no (not a fitted prop box) | 21.00 |
| Campsites/Camp_2/StackPath/Post | box | (298.20, 13.50, 106.50) | 0.20 x 21.00 x 0.20 | 0 | no (not a fitted prop box) | 20.00 |
| Campsites/Camp_2/StackPath/Post | box | (301.10, 13.50, 106.50) | 0.20 x 21.00 x 0.20 | 0 | no (not a fitted prop box) | 20.00 |
| Campsites/Camp_2/StackPath/Post | box | (298.20, 13.50, 119.50) | 0.20 x 21.00 x 0.20 | 0 | no (not a fitted prop box) | 20.00 |
| Campsites/Camp_2/StackPath/Post | box | (301.10, 13.50, 119.50) | 0.20 x 21.00 x 0.20 | 0 | no (not a fitted prop box) | 20.00 |
| Campsites/Camp_2/StackPath/Ramp1 | box | (298.90, 6.41, 113.04) | 1.40 x 0.20 x 11.48 | 0 | no (not a fitted prop box) | 5.07 |
| Campsites/Camp_2/StackPath/Ramp1_Rail | box | (298.23, 7.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 5.95 |
| Campsites/Camp_2/StackPath/Ramp1_Rail | box | (299.57, 7.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 5.95 |
| Campsites/Camp_2/StackPath/Ramp2 | box | (300.40, 11.41, 112.96) | 1.40 x 0.20 x 11.48 | 0 | no (not a fitted prop box) | 10.07 |
| Campsites/Camp_2/StackPath/Ramp2_Rail | box | (299.73, 12.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 10.95 |
| Campsites/Camp_2/StackPath/Ramp2_Rail | box | (301.07, 12.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 10.95 |
| Campsites/Camp_2/StackPath/Ramp3 | box | (298.90, 16.41, 113.04) | 1.40 x 0.20 x 11.48 | 0 | no (not a fitted prop box) | 15.07 |
| Campsites/Camp_2/StackPath/Ramp3_Rail | box | (299.57, 17.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 15.95 |
| Campsites/Camp_2/StackPath/Ramp3_Rail | box | (298.23, 17.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 15.95 |
| Campsites/Camp_2/StackPath/Ramp4 | box | (300.40, 21.41, 112.96) | 1.40 x 0.20 x 11.48 | 0 | no (not a fitted prop box) | 20.07 |
| Campsites/Camp_2/StackPath/Ramp4_Rail | box | (299.73, 22.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 20.95 |
| Campsites/Camp_2/StackPath/Ramp4_Rail | box | (301.07, 22.00, 113.00) | 0.06 x 1.00 x 11.18 | 0 | no (not a fitted prop box) | 20.95 |
| Campsites/Camp_2/StackTop/Dressing/[CS_Tent_Modern_2] | box | (292.50, 24.76, 108.80) | 3.48 x 1.52 x 3.56 | 200 | yes, yaw 200: mesh about 2.03 x 2.25 | 21.52 |
| Forest/Dense/Floor/[CITW_Tree_Stump] | mesh | (212.26, 10.43, 193.15) | aabb 1.41 x 1.00 x 1.41 | 309 | convex hull | 1.43 |
| Forest/Dense/Floor/[CITW_Tree_Stump] | mesh | (286.58, 5.76, 220.01) | aabb 1.15 x 0.83 x 1.14 | 32 | convex hull | 1.18 |
| Forest/Dense/Floor/[CITW_Tree_Stump] | mesh | (272.96, 6.09, 213.77) | aabb 1.21 x 1.17 x 1.20 | 2 | convex hull | 1.68 |
| Ground815/JunctionMarkers/Sign_Jg/Post | box | (265.00, 6.00, 169.00) | 0.14 x 3.20 x 0.14 | 0 | no (not a fitted prop box) | 2.60 |
| Ground815/JunctionMarkers/Trailhead_Board/Board | box | (338.00, 4.81, 172.50) | 1.90 x 1.10 x 0.06 | 90 | no (not a fitted prop box) | 2.05 |
| Ground815/JunctionMarkers/Trailhead_Board/Post | box | (338.00, 4.31, 173.40) | 0.14 x 2.00 x 0.14 | 0 | no (not a fitted prop box) | 2.06 |
| Ground815/JunctionMarkers/Trailhead_Board/Post | box | (338.00, 4.31, 171.60) | 0.14 x 2.00 x 0.14 | 0 | no (not a fitted prop box) | 1.96 |
| Ground815/Stops/Hedge_Burn_1/HedgeCollider | box | (182.08, 12.69, 141.58) | 1.00 x 8.62 x 7.72 | 146 | no (not a fitted prop box) | 2.73 |
| Ground815/Stops/Hedge_Burn_1/HedgeCollider | box | (184.08, 12.28, 138.26) | 1.00 x 8.10 x 0.80 | 198 | no (not a fitted prop box) | 7.53 |
| Ground815/Stops/Hedge_Burn_10/HedgeCollider | box | (336.80, 4.18, 148.46) | 1.00 x 2.36 x 0.72 | 101 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_10/HedgeCollider | box | (336.80, 4.18, 148.46) | 1.00 x 2.36 x 0.72 | 281 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (254.69, 6.58, 184.00) | 1.00 x 2.96 x 4.36 | 190 | no (not a fitted prop box) | 2.38 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (236.00, 7.43, 191.75) | 1.00 x 2.76 x 2.30 | 90 | no (not a fitted prop box) | 2.21 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (231.50, 7.62, 152.31) | 1.00 x 4.00 x 2.30 | 266 | no (not a fitted prop box) | 3.20 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (212.00, 11.98, 158.75) | 1.00 x 3.94 x 4.48 | 253 | no (not a fitted prop box) | 3.18 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (218.00, 10.87, 158.34) | 1.00 x 3.54 x 3.60 | 294 | no (not a fitted prop box) | 2.75 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (256.64, 6.07, 175.84) | 1.00 x 2.82 x 0.72 | 221 | no (not a fitted prop box) | 2.31 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (226.94, 8.90, 152.42) | 1.00 x 3.56 x 2.36 | 294 | no (not a fitted prop box) | 2.81 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (206.75, 12.03, 184.50) | 1.00 x 3.06 x 1.00 | 135 | no (not a fitted prop box) | 2.55 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (264.25, 6.66, 197.74) | 1.00 x 2.84 x 2.98 | 69 | no (not a fitted prop box) | 2.14 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (256.00, 6.89, 195.25) | 1.00 x 2.60 x 1.30 | 90 | no (not a fitted prop box) | 2.09 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (198.00, 13.34, 152.50) | 1.00 x 5.28 x 2.80 | 307 | no (not a fitted prop box) | 3.41 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (266.53, 6.57, 197.50) | 1.00 x 2.78 x 1.38 | 159 | no (not a fitted prop box) | 2.25 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (235.77, 6.23, 163.25) | 1.00 x 3.44 x 2.80 | 182 | no (not a fitted prop box) | 2.96 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (187.78, 15.90, 157.50) | 1.00 x 2.92 x 5.32 | 5 | no (not a fitted prop box) | 2.38 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (239.50, 6.23, 166.88) | 1.00 x 3.44 x 3.32 | 264 | no (not a fitted prop box) | 2.95 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (244.25, 6.77, 176.35) | 1.00 x 2.84 x 2.22 | 231 | no (not a fitted prop box) | 2.34 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (259.00, 6.84, 196.25) | 1.00 x 2.66 x 5.68 | 68 | no (not a fitted prop box) | 2.12 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (261.75, 6.67, 191.91) | 1.00 x 2.78 x 10.38 | 237 | no (not a fitted prop box) | 2.23 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (246.50, 6.75, 177.47) | 1.00 x 2.84 x 3.48 | 251 | no (not a fitted prop box) | 2.33 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (252.75, 6.93, 194.00) | 1.00 x 2.62 x 2.84 | 79 | no (not a fitted prop box) | 2.10 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (256.27, 6.67, 187.60) | 1.00 x 2.76 x 4.34 | 218 | no (not a fitted prop box) | 2.24 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (188.28, 15.79, 154.88) | 1.00 x 3.14 x 1.76 | 280 | no (not a fitted prop box) | 2.48 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (189.25, 15.77, 155.00) | 1.00 x 3.20 x 1.00 | 225 | no (not a fitted prop box) | 2.66 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (247.25, 7.03, 192.25) | 1.00 x 2.72 x 5.90 | 80 | no (not a fitted prop box) | 2.17 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (221.50, 9.32, 187.75) | 1.00 x 4.52 x 6.70 | 51 | no (not a fitted prop box) | 3.34 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (262.25, 6.78, 197.25) | 1.00 x 2.62 x 1.80 | 90 | no (not a fitted prop box) | 2.10 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (254.75, 6.90, 194.75) | 1.00 x 2.58 x 2.10 | 56 | no (not a fitted prop box) | 2.09 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (266.39, 6.61, 195.06) | 1.00 x 2.70 x 1.46 | 222 | no (not a fitted prop box) | 2.18 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (237.25, 7.40, 191.50) | 1.00 x 2.74 x 1.00 | 135 | no (not a fitted prop box) | 2.22 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (242.72, 6.66, 174.38) | 1.00 x 3.02 x 3.46 | 209 | no (not a fitted prop box) | 2.41 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (235.39, 6.77, 157.00) | 1.00 x 4.34 x 7.06 | 207 | no (not a fitted prop box) | 3.58 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (208.25, 11.86, 184.25) | 1.00 x 3.44 x 2.80 | 90 | no (not a fitted prop box) | 2.76 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (233.17, 7.36, 153.19) | 1.00 x 4.14 x 2.40 | 219 | no (not a fitted prop box) | 3.49 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (200.50, 12.53, 151.56) | 1.00 x 5.98 x 3.32 | 277 | no (not a fitted prop box) | 5.01 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (209.75, 11.66, 184.00) | 1.00 x 3.12 x 1.00 | 135 | no (not a fitted prop box) | 2.60 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (244.25, 7.10, 191.50) | 1.00 x 2.64 x 1.00 | 45 | no (not a fitted prop box) | 2.14 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (250.75, 6.96, 193.25) | 1.00 x 2.60 x 2.10 | 56 | no (not a fitted prop box) | 2.10 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (266.75, 6.58, 196.25) | 1.00 x 2.72 x 1.80 | 178 | no (not a fitted prop box) | 2.20 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (192.25, 15.49, 154.75) | 1.00 x 3.74 x 1.80 | 270 | no (not a fitted prop box) | 3.13 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (215.25, 11.36, 159.18) | 1.00 x 3.42 x 2.82 | 278 | no (not a fitted prop box) | 2.73 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (228.94, 8.37, 151.89) | 1.00 x 3.74 x 2.42 | 276 | no (not a fitted prop box) | 2.91 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (265.92, 6.58, 198.11) | 1.00 x 2.80 x 1.16 | 105 | no (not a fitted prop box) | 2.27 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (212.50, 11.22, 183.75) | 1.00 x 3.92 x 5.30 | 90 | no (not a fitted prop box) | 3.00 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (241.47, 6.42, 170.02) | 1.00 x 3.52 x 6.32 | 189 | no (not a fitted prop box) | 2.81 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (230.25, 8.20, 152.02) | 1.00 x 3.72 x 0.98 | 227 | no (not a fitted prop box) | 3.14 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (207.00, 12.04, 155.21) | 1.00 x 5.20 x 8.70 | 226 | no (not a fitted prop box) | 3.81 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (203.00, 12.10, 151.82) | 1.00 x 5.32 x 2.50 | 246 | no (not a fitted prop box) | 4.80 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (190.25, 15.65, 155.25) | 1.00 x 3.42 x 1.80 | 270 | no (not a fitted prop box) | 2.81 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (226.50, 8.36, 191.00) | 1.00 x 3.70 x 5.90 | 63 | no (not a fitted prop box) | 3.05 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (234.75, 7.53, 192.00) | 1.00 x 2.76 x 1.00 | 135 | no (not a fitted prop box) | 2.24 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (191.25, 15.60, 155.00) | 1.00 x 3.52 x 1.00 | 315 | no (not a fitted prop box) | 2.97 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (222.75, 9.92, 155.25) | 1.00 x 4.58 x 8.38 | 307 | no (not a fitted prop box) | 3.30 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (236.34, 6.23, 161.00) | 1.00 x 3.44 x 2.64 | 149 | no (not a fitted prop box) | 2.92 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (254.00, 6.34, 176.63) | 1.00 x 3.28 x 5.64 | 291 | no (not a fitted prop box) | 2.58 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (255.94, 6.26, 177.50) | 1.00 x 3.20 x 3.72 | 151 | no (not a fitted prop box) | 2.60 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (240.75, 7.23, 191.25) | 1.00 x 2.88 x 6.80 | 90 | no (not a fitted prop box) | 2.28 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (217.00, 10.34, 184.75) | 1.00 x 3.92 x 4.78 | 63 | no (not a fitted prop box) | 2.96 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (231.75, 7.68, 192.25) | 1.00 x 3.00 x 5.80 | 90 | no (not a fitted prop box) | 2.34 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (236.91, 6.23, 165.61) | 1.00 x 3.44 x 3.42 | 225 | no (not a fitted prop box) | 2.94 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (249.75, 6.61, 177.78) | 1.00 x 2.98 x 3.82 | 277 | no (not a fitted prop box) | 2.36 |
| Ground815/Stops/Hedge_Burn_3/HedgeCollider | box | (254.73, 6.45, 180.50) | 1.00 x 3.14 x 3.40 | 166 | no (not a fitted prop box) | 2.53 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (211.00, 11.13, 149.15) | 1.00 x 3.38 x 1.38 | 247 | no (not a fitted prop box) | 2.72 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (215.50, 10.97, 151.79) | 1.00 x 3.78 x 3.52 | 111 | no (not a fitted prop box) | 2.89 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (214.00, 11.23, 149.75) | 1.00 x 3.14 x 2.54 | 243 | no (not a fitted prop box) | 2.43 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (212.25, 11.27, 149.31) | 1.00 x 3.00 x 1.80 | 274 | no (not a fitted prop box) | 2.38 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (216.29, 10.89, 150.38) | 1.00 x 3.88 x 2.90 | 264 | no (not a fitted prop box) | 3.00 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (212.25, 11.15, 150.65) | 1.00 x 3.42 x 5.20 | 46 | no (not a fitted prop box) | 2.29 |
| Ground815/Stops/Hedge_Burn_5/HedgeCollider | box | (217.29, 10.82, 150.85) | 1.00 x 3.76 x 1.22 | 141 | no (not a fitted prop box) | 3.18 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (229.25, 7.36, 160.70) | 1.00 x 3.40 x 2.82 | 278 | no (not a fitted prop box) | 2.37 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (231.50, 6.45, 160.81) | 1.00 x 3.02 x 2.38 | 254 | no (not a fitted prop box) | 2.21 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (232.50, 6.78, 170.77) | 1.00 x 2.86 x 2.56 | 118 | no (not a fitted prop box) | 2.10 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (234.20, 6.26, 169.37) | 1.00 x 2.76 x 2.54 | 141 | no (not a fitted prop box) | 2.05 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (224.75, 8.90, 164.86) | 1.00 x 2.40 x 2.64 | 348 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (233.25, 5.93, 161.67) | 1.00 x 2.60 x 2.18 | 233 | no (not a fitted prop box) | 1.98 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (235.19, 5.89, 167.50) | 1.00 x 2.58 x 2.38 | 164 | no (not a fitted prop box) | 1.96 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (235.33, 5.72, 165.25) | 1.00 x 2.38 x 2.82 | 187 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (228.50, 7.93, 171.19) | 1.00 x 2.80 x 2.38 | 74 | no (not a fitted prop box) | 2.04 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (225.44, 8.70, 168.89) | 1.00 x 2.58 x 2.40 | 32 | no (not a fitted prop box) | 1.94 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (230.50, 7.37, 171.38) | 1.00 x 2.94 x 2.30 | 95 | no (not a fitted prop box) | 2.11 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (224.69, 8.89, 167.00) | 1.00 x 2.40 x 2.34 | 11 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (225.61, 8.70, 162.86) | 1.00 x 2.60 x 2.42 | 324 | no (not a fitted prop box) | 1.94 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (234.59, 5.73, 163.11) | 1.00 x 2.40 x 2.42 | 214 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (227.11, 8.24, 161.44) | 1.00 x 2.94 x 2.40 | 302 | no (not a fitted prop box) | 2.08 |
| Ground815/Stops/Hedge_Burn_6/HedgeCollider | box | (226.75, 8.37, 170.33) | 1.00 x 2.66 x 2.18 | 53 | no (not a fitted prop box) | 1.98 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (255.25, 6.00, 142.25) | 1.00 x 2.32 x 2.80 | 270 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (299.00, 5.29, 143.25) | 1.00 x 2.38 x 2.30 | 270 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (257.50, 6.01, 142.75) | 1.00 x 2.34 x 2.54 | 243 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (264.89, 5.99, 163.44) | 1.00 x 2.36 x 2.40 | 58 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (319.19, 4.96, 147.00) | 1.00 x 2.36 x 3.50 | 200 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (251.75, 6.07, 143.25) | 1.00 x 2.42 x 5.22 | 294 | no (not a fitted prop box) | 1.87 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (316.50, 4.94, 144.28) | 1.00 x 2.38 x 3.88 | 303 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (314.00, 5.00, 145.25) | 1.00 x 2.34 x 2.30 | 270 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (307.00, 5.15, 145.75) | 1.00 x 2.38 x 4.30 | 270 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (297.75, 5.33, 143.00) | 1.00 x 2.30 x 1.00 | 225 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (267.25, 6.14, 147.00) | 1.00 x 2.54 x 16.62 | 243 | no (not a fitted prop box) | 1.91 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (248.20, 6.14, 169.00) | 1.00 x 2.68 x 4.48 | 17 | no (not a fitted prop box) | 1.99 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (267.27, 5.79, 167.50) | 1.00 x 2.44 x 3.32 | 5 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (247.67, 5.93, 166.25) | 1.00 x 2.36 x 1.82 | 353 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (311.25, 5.37, 157.67) | 1.00 x 2.36 x 4.40 | 121 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (337.92, 3.73, 157.75) | 1.00 x 2.36 x 1.82 | 188 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (320.87, 5.02, 149.71) | 1.00 x 2.36 x 3.62 | 223 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (318.33, 4.89, 143.40) | 1.00 x 2.30 x 0.98 | 253 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (249.91, 6.24, 170.86) | 1.00 x 2.48 x 2.50 | 98 | no (not a fitted prop box) | 1.87 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (323.50, 5.06, 151.72) | 1.00 x 2.32 x 3.70 | 242 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (240.83, 6.15, 153.00) | 1.00 x 2.78 x 4.78 | 27 | no (not a fitted prop box) | 2.03 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (304.96, 5.48, 168.75) | 1.00 x 2.32 x 8.08 | 164 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (295.50, 5.19, 177.71) | 1.00 x 2.90 x 9.58 | 104 | no (not a fitted prop box) | 2.16 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (318.65, 4.91, 144.50) | 1.00 x 2.34 x 2.30 | 179 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (289.25, 4.93, 177.95) | 1.00 x 2.38 x 4.22 | 63 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (285.52, 5.17, 175.28) | 1.00 x 2.70 x 5.62 | 48 | no (not a fitted prop box) | 2.07 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (337.28, 3.75, 161.50) | 1.00 x 2.40 x 6.48 | 166 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (246.24, 5.74, 159.95) | 1.00 x 2.32 x 3.74 | 26 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (314.75, 5.32, 156.39) | 1.00 x 2.34 x 3.82 | 97 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (328.75, 5.15, 161.15) | 1.00 x 2.42 x 6.26 | 67 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (247.37, 5.82, 163.50) | 1.00 x 2.44 x 4.38 | 11 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (296.50, 5.36, 142.75) | 1.00 x 2.38 x 2.30 | 270 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (253.75, 5.97, 169.62) | 1.00 x 2.68 x 6.22 | 112 | no (not a fitted prop box) | 2.02 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (309.25, 5.11, 146.00) | 1.00 x 2.30 x 1.00 | 225 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (306.74, 5.46, 163.25) | 1.00 x 2.32 x 4.10 | 157 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (278.25, 5.96, 172.67) | 1.00 x 2.50 x 3.92 | 105 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (321.00, 5.25, 157.36) | 1.00 x 2.32 x 3.54 | 68 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (311.00, 5.07, 146.00) | 1.00 x 2.38 x 3.34 | 279 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (281.77, 5.62, 172.86) | 1.00 x 2.78 x 4.08 | 70 | no (not a fitted prop box) | 2.02 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (283.75, 5.99, 147.75) | 1.00 x 2.68 x 9.32 | 304 | no (not a fitted prop box) | 1.98 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (268.05, 5.82, 176.79) | 1.00 x 2.42 x 2.12 | 30 | no (not a fitted prop box) | 1.87 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (303.09, 5.51, 173.75) | 1.00 x 2.34 x 3.30 | 147 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (301.13, 5.51, 175.79) | 1.00 x 2.34 x 3.06 | 125 | no (not a fitted prop box) | 1.79 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (318.00, 5.28, 156.47) | 1.00 x 2.34 x 3.36 | 79 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (259.25, 6.02, 143.25) | 1.00 x 2.30 x 1.80 | 270 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (269.00, 5.93, 177.88) | 1.00 x 2.42 x 1.48 | 58 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (295.25, 5.40, 142.50) | 1.00 x 2.30 x 1.00 | 225 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (266.83, 5.69, 170.50) | 1.00 x 2.36 x 3.52 | 339 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (291.25, 5.60, 143.75) | 1.00 x 2.70 x 8.38 | 292 | no (not a fitted prop box) | 1.99 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (260.50, 6.04, 162.81) | 1.00 x 2.32 x 2.38 | 106 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (302.50, 5.23, 144.50) | 1.00 x 2.36 x 5.90 | 243 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (243.67, 5.82, 156.70) | 1.00 x 2.50 x 5.30 | 47 | no (not a fitted prop box) | 1.98 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (308.49, 5.42, 160.12) | 1.00 x 2.34 x 3.72 | 144 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (247.25, 6.23, 146.00) | 1.00 x 2.48 x 6.00 | 308 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (332.50, 4.99, 163.98) | 1.00 x 2.48 x 4.22 | 31 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (330.25, 4.64, 153.92) | 1.00 x 3.16 x 11.16 | 255 | no (not a fitted prop box) | 1.96 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (274.75, 6.09, 174.82) | 1.00 x 2.36 x 5.18 | 134 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (312.75, 5.03, 145.50) | 1.00 x 2.32 x 1.00 | 315 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (258.30, 5.97, 164.30) | 1.00 x 2.42 x 3.70 | 135 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (336.65, 3.98, 156.16) | 1.00 x 2.74 x 3.16 | 234 | no (not a fitted prop box) | 2.03 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (266.45, 5.91, 165.00) | 1.00 x 2.40 x 2.72 | 34 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (335.01, 4.35, 165.09) | 1.00 x 3.40 x 3.54 | 111 | no (not a fitted prop box) | 2.30 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (256.80, 5.84, 167.01) | 1.00 x 2.44 x 3.38 | 169 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (266.92, 5.71, 174.00) | 1.00 x 2.40 x 4.52 | 18 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (277.25, 6.22, 150.50) | 1.00 x 2.38 x 5.82 | 275 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (262.75, 6.04, 162.70) | 1.00 x 2.32 x 2.82 | 82 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (239.91, 6.42, 150.67) | 1.00 x 2.36 x 0.98 | 345 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (242.50, 6.38, 149.04) | 1.00 x 2.42 x 5.94 | 297 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (272.00, 6.10, 177.18) | 1.00 x 2.32 x 2.70 | 123 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (270.25, 6.04, 178.01) | 1.00 x 2.38 x 1.84 | 103 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_7/HedgeCollider | box | (324.25, 5.22, 158.99) | 1.00 x 2.34 x 4.34 | 60 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (261.97, 6.31, 183.75) | 1.00 x 2.44 x 2.24 | 40 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (261.75, 5.97, 181.24) | 1.00 x 2.44 x 1.60 | 337 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (261.43, 6.14, 182.42) | 1.00 x 2.50 x 1.46 | 353 | no (not a fitted prop box) | 1.90 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (270.50, 6.35, 189.60) | 1.00 x 2.32 x 4.24 | 50 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (296.84, 5.40, 184.58) | 1.00 x 2.58 x 9.24 | 284 | no (not a fitted prop box) | 1.92 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (321.07, 5.21, 165.06) | 1.00 x 2.40 x 6.20 | 241 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (314.75, 5.35, 163.60) | 1.00 x 2.36 x 2.10 | 304 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (274.99, 6.00, 202.38) | 1.00 x 2.70 x 6.02 | 61 | no (not a fitted prop box) | 2.06 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (280.25, 5.68, 179.70) | 1.00 x 2.78 x 1.90 | 250 | no (not a fitted prop box) | 2.09 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (277.75, 6.02, 181.07) | 1.00 x 2.52 x 5.10 | 313 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (323.54, 5.11, 167.75) | 1.00 x 2.40 x 2.80 | 175 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (312.96, 5.39, 166.30) | 1.00 x 2.34 x 5.16 | 335 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (317.00, 5.29, 163.37) | 1.00 x 2.36 x 3.34 | 260 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (273.17, 6.26, 198.75) | 1.00 x 2.44 x 5.00 | 343 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (265.79, 6.37, 186.41) | 1.00 x 2.32 x 7.76 | 59 | no (not a fitted prop box) | 1.78 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (325.75, 4.94, 171.73) | 1.00 x 2.34 x 1.94 | 246 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (273.70, 6.34, 195.00) | 1.00 x 2.32 x 3.32 | 6 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (332.50, 4.58, 174.40) | 1.00 x 2.46 x 2.30 | 268 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (263.50, 5.94, 182.08) | 1.00 x 2.44 x 3.52 | 218 | no (not a fitted prop box) | 1.88 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (338.88, 3.71, 176.70) | 1.00 x 2.42 x 3.42 | 214 | no (not a fitted prop box) | 1.87 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (262.25, 5.89, 180.73) | 1.00 x 2.32 x 0.82 | 251 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (301.50, 5.96, 199.50) | 1.00 x 2.42 x 4.58 | 69 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (310.86, 5.39, 173.00) | 1.00 x 2.30 x 6.62 | 342 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (282.98, 5.33, 181.74) | 1.00 x 2.54 x 5.60 | 228 | no (not a fitted prop box) | 1.98 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (274.00, 6.13, 183.74) | 1.00 x 2.30 x 4.80 | 297 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (290.50, 5.23, 185.51) | 1.00 x 2.34 x 4.32 | 266 | no (not a fitted prop box) | 1.90 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (324.21, 5.01, 170.20) | 1.00 x 2.40 x 3.16 | 213 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (337.00, 3.90, 175.41) | 1.00 x 2.56 x 2.30 | 270 | no (not a fitted prop box) | 1.95 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (330.00, 4.74, 173.23) | 1.00 x 2.46 x 4.08 | 233 | no (not a fitted prop box) | 1.89 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (307.15, 5.46, 179.75) | 1.00 x 2.36 x 3.72 | 317 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (266.20, 6.06, 184.17) | 1.00 x 2.40 x 4.08 | 244 | no (not a fitted prop box) | 1.86 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (327.50, 4.87, 172.07) | 1.00 x 2.40 x 2.30 | 270 | no (not a fitted prop box) | 1.84 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (272.77, 6.35, 192.19) | 1.00 x 2.30 x 3.34 | 30 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (298.75, 6.03, 198.75) | 1.00 x 2.34 x 1.80 | 90 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (286.73, 5.21, 184.43) | 1.00 x 2.30 x 4.30 | 242 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (335.00, 4.23, 194.42) | 1.00 x 2.52 x 3.62 | 162 | no (not a fitted prop box) | 1.92 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (294.75, 6.02, 200.00) | 1.00 x 2.36 x 7.26 | 111 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (303.58, 5.51, 182.25) | 1.00 x 2.34 x 5.72 | 297 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (334.75, 4.26, 174.92) | 1.00 x 2.76 x 2.98 | 249 | no (not a fitted prop box) | 1.99 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (311.89, 5.40, 169.25) | 1.00 x 2.32 x 1.80 | 357 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (309.09, 5.41, 177.25) | 1.00 x 2.34 x 3.24 | 328 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (269.95, 6.12, 184.89) | 1.00 x 2.32 x 4.40 | 273 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_8/HedgeCollider | box | (334.69, 4.32, 197.00) | 1.00 x 2.34 x 2.34 | 191 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (339.38, 3.67, 145.63) | 1.00 x 2.32 x 1.36 | 225 | no (not a fitted prop box) | 1.82 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (327.25, 4.98, 145.87) | 1.00 x 2.32 x 3.14 | 61 | no (not a fitted prop box) | 1.79 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (330.00, 4.92, 147.00) | 1.00 x 2.46 x 3.42 | 74 | no (not a fitted prop box) | 1.85 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (334.00, 4.53, 147.94) | 1.00 x 2.92 x 5.40 | 79 | no (not a fitted prop box) | 2.08 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (337.75, 3.84, 145.00) | 1.00 x 2.60 x 2.84 | 259 | no (not a fitted prop box) | 1.99 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (333.25, 4.42, 143.25) | 1.00 x 3.14 x 7.46 | 245 | no (not a fitted prop box) | 2.08 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (338.13, 3.94, 147.23) | 1.00 x 2.86 x 4.38 | 127 | no (not a fitted prop box) | 2.25 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (328.25, 4.83, 141.50) | 1.00 x 2.30 x 3.84 | 262 | no (not a fitted prop box) | 1.80 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (326.00, 4.84, 141.61) | 1.00 x 2.32 x 1.52 | 306 | no (not a fitted prop box) | 1.81 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (325.74, 4.94, 144.60) | 1.00 x 2.36 x 1.60 | 24 | no (not a fitted prop box) | 1.83 |
| Ground815/Stops/Hedge_Burn_9/HedgeCollider | box | (325.49, 4.88, 142.98) | 1.00 x 2.36 x 2.34 | 359 | no (not a fitted prop box) | 1.83 |
| Ground815/TrailEdges/[CS_Stone_1] | mesh | (265.36, 5.08, 213.94) | aabb 3.52 x 2.89 x 3.77 | 342 | convex hull | 1.52 |
| Ground815/TrailEdges/[CS_Stone_2] | mesh | (321.49, 4.59, 159.88) | aabb 2.70 x 3.24 x 3.57 | 85 | convex hull | 1.62 |
| Ground815/TrailEdges/[CS_Stone_3] | mesh | (197.67, 11.32, 144.46) | aabb 3.11 x 2.70 x 3.19 | 97 | convex hull | 1.41 |
| Ground815/TrailEdges/[CS_Stone_3] | mesh | (326.08, 4.57, 162.26) | aabb 2.68 x 2.82 x 2.94 | 16 | convex hull | 1.45 |
| Ground815/TrailEdges/[CS_Stone_4] | mesh | (323.33, 4.24, 142.81) | aabb 4.84 x 4.08 x 5.85 | 117 | convex hull | 2.05 |
| Ground815/TrailEdges/[CS_Stone_5] | mesh | (238.55, 5.67, 152.74) | aabb 2.54 x 2.42 x 2.12 | 282 | convex hull | 1.26 |
| Ground815/TrailEdges/[CS_Stone_7] | mesh | (271.17, 5.56, 180.03) | aabb 2.02 x 2.00 x 2.31 | 68 | convex hull | 1.10 |
| Ground815/TrailEdges/[CS_Stone_7] | mesh | (256.15, 5.17, 170.84) | aabb 3.13 x 2.32 x 3.11 | 55 | convex hull | 1.16 |
| Ground815/TrailEdges/[CS_Stone_8] | mesh | (240.79, 5.18, 157.08) | aabb 1.86 x 2.01 x 2.21 | 230 | convex hull | 1.04 |
| Ground815/TrailEdges/[CS_Stone_8] | mesh | (282.84, 5.05, 236.36) | aabb 2.50 x 2.13 x 2.03 | 280 | convex hull | 1.11 |
| PointsOfInterest/POI_First_sight_of_the_lot/Post | box | (327.34, 4.97, 168.16) | 0.20 x 1.20 x 0.20 | 81 | no (not a fitted prop box) | 1.20 |
| PointsOfInterest/POI_Forage_patch_A/Bush | sphere | (240.22, 5.41, 164.29) | aabb 1.48 x 0.80 x 1.47 | 27 | exact mesh | 0.77 |
| PointsOfInterest/POI_Forage_patch_A/Bush | sphere | (239.82, 5.41, 161.66) | aabb 1.48 x 0.80 x 1.48 | 27 | exact mesh | 0.78 |
| PointsOfInterest/POI_Forage_patch_A/Bush | sphere | (238.35, 5.41, 162.40) | aabb 1.48 x 0.80 x 1.48 | 27 | exact mesh | 0.79 |
| PointsOfInterest/POI_Forage_patch_A/Bush | sphere | (238.60, 5.41, 164.03) | aabb 1.47 x 0.80 x 1.47 | 27 | exact mesh | 0.81 |
| PointsOfInterest/POI_Forage_patch_A/Bush | sphere | (240.98, 5.41, 162.83) | aabb 1.47 x 0.80 x 1.47 | 27 | exact mesh | 0.79 |
| PointsOfInterest/POI_Latrine_shed/Shed | box | (263.70, 6.35, 207.46) | 1.30 x 2.20 x 1.30 | 14 | no (not a fitted prop box) | 2.20 |
| PointsOfInterest/POI_Latrine_shed/WashStand | box | (265.25, 5.70, 207.08) | 0.80 x 0.90 x 0.50 | 14 | no (not a fitted prop box) | 0.95 |

## 4. Trees and rocks on or near the trails (within 3 m of a centreline)
How it was measured:
- Distance is from the centreline to a trunk's surface (capsule radius taken off), or to an object's centre.
- **Trees:** inside 3 m there is only the Gate Tree (290, 176), r 4.0, 1.51 m from Jg to T, and one Forest/BurnDeadwood snag. The Hollow Giant trunk (202, 140), r 4.5, stands about 3.2 m from Camp to Jg markers P30 and P32.
- **Colliders:** Campsite CS_Stone and log prefabs have no collider of their own, so "none in scene file" means none.

| Off trail m | Object | Position | Collider | Trail |
|---|---|---|---|---|
| 1.28 | Ground815/TrailEdges/[CS_Stone_4] | (323.33, 142.81) | object centre; collider mesh convex | Camp 2 to T |
| 1.33 | Ground815/TrailEdges/[CS_Stone_2] | (325.69, 150.45) | object centre; collider none in scene file | Camp 2 to T |
| 1.34 | Ground815/TrailEdges/[CS_Stone_7] | (325.16, 147.17) | object centre; collider none in scene file | Camp 2 to T |
| 1.40 | Ground815/TrailEdges/[CS_Stone_4] | (331.78, 152.24) | object centre; collider none in scene file | Camp 2 to T |
| 1.40 | Ground815/TrailEdges/[CS_Stone_1] | (321.40, 137.65) | object centre; collider none in scene file | Camp 2 to T |
| 1.42 | Ground815/TrailEdges/[CS_Stone_8] | (316.19, 128.83) | object centre; collider none in scene file | Camp 2 to T |
| 1.52 | Ground815/TrailEdges/[CS_Stone_2] | (313.12, 128.52) | object centre; collider none in scene file | Camp 2 to T |
| 1.52 | Ground815/TrailEdges/[CS_Stone_4] | (320.55, 144.84) | object centre; collider mesh convex | Camp 2 to T |
| 1.53 | Ground815/TrailEdges/[CS_Stone_6] | (300.71, 118.92) | object centre; collider none in scene file | Camp 2 to T |
| 1.56 | Ground815/TrailEdges/[CS_Stone_5] | (319.41, 129.11) | object centre; collider none in scene file | Camp 2 to T |
| 1.57 | Ground815/TrailEdges/[CS_Stone_1] | (323.92, 132.44) | object centre; collider none in scene file | Camp 2 to T |
| 2.61 | SliceLook/OpenGround/[DeadLeaves1] | (341.66, 152.15) | object centre; collider none in scene file | Camp 2 to T |
| 2.79 | SliceLook/OpenGround/[DeadLeaves2] | (296.99, 114.32) | object centre; collider none in scene file | Camp 2 to T |
| 2.92 | Forest/BurnDeadwood/[Tree_Dead] | (315.77, 134.50) | trunk r 1.34 | Camp 2 to T |
| 2.96 | SliceLook/OpenGround/[DeadLeaves1] | (297.73, 118.57) | object centre; collider none in scene file | Camp 2 to T |
| 1.22 | Ground815/TrailEdges/[CS_Stone_8] | (192.27, 145.00) | object centre; collider none in scene file | Camp to Jg |
| 1.31 | Ground815/TrailEdges/[CS_Stone_8] | (240.79, 157.08) | object centre; collider mesh convex | Camp to Jg |
| 1.36 | Ground815/TrailEdges/[CS_Stone_7] | (256.15, 170.84) | object centre; collider mesh convex | Camp to Jg |
| 1.38 | Ground815/TrailEdges/[CS_Stone_8] | (183.94, 150.00) | object centre; collider none in scene file | Camp to Jg |
| 1.39 | Ground815/TrailEdges/[CS_Stone_6] | (219.43, 151.85) | object centre; collider none in scene file | Camp to Jg |
| 1.43 | Ground815/TrailEdges/[CS_Stone_5] | (238.55, 152.74) | object centre; collider mesh convex | Camp to Jg |
| 1.43 | Ground815/TrailEdges/[CS_Stone_1] | (228.90, 149.42) | object centre; collider none in scene file | Camp to Jg |
| 1.48 | Ground815/TrailEdges/[CS_Stone_2] | (200.64, 149.26) | object centre; collider none in scene file | Camp to Jg |
| 1.49 | Ground815/TrailEdges/[CS_Stone_8] | (218.39, 155.97) | object centre; collider none in scene file | Camp to Jg |
| 1.51 | Ground815/TrailEdges/[CS_Stone_6] | (240.05, 160.33) | object centre; collider none in scene file | Camp to Jg |
| 1.54 | SliceLook/ShotGround/[CS_Stone_3] | (243.12, 169.13) | object centre; collider none in scene file | Camp to Jg |
| 1.54 | Ground815/TrailEdges/[CS_Stone_3] | (243.13, 169.22) | object centre; collider none in scene file | Camp to Jg |
| 1.55 | Ground815/TrailEdges/[CS_Log_Firewood_Short] | (253.06, 171.75) | object centre; collider none in scene file | Camp to Jg |
| 1.58 | Ground815/TrailEdges/[CS_Stone_3] | (197.67, 144.46) | object centre; collider mesh convex | Camp to Jg |
| 1.73 | SliceLook/ShotGround/[DeadLeaves1] | (245.59, 163.35) | object centre; collider none in scene file | Camp to Jg |
| 1.85 | SliceLook/ShotGround/[CS_Stone_1] | (237.21, 157.33) | object centre; collider none in scene file | Camp to Jg |
| 1.88 | SliceLook/ShotGround/[CS_Stone_3] | (246.64, 169.34) | object centre; collider none in scene file | Camp to Jg |
| 1.94 | SliceLook/ShotGround/[CS_Log_Firewood] | (240.21, 161.01) | object centre; collider none in scene file | Camp to Jg |
| 2.05 | SliceLook/ShotGround/[CS_Log_Firewood] | (242.95, 170.89) | object centre; collider none in scene file | Camp to Jg |
| 2.32 | SliceLook/ShotGround/[CS_Log_Firewood] | (244.42, 175.02) | object centre; collider none in scene file | Camp to Jg |
| 2.34 | SliceLook/ShotGround/[DeadLeaves1] | (241.03, 162.48) | object centre; collider none in scene file | Camp to Jg |
| 2.47 | SliceLook/ShotGround/[DeadLeaves1] | (242.55, 171.02) | object centre; collider none in scene file | Camp to Jg |
| 2.47 | SliceLook/ShotGround/[CS_Stone_3] | (241.03, 162.80) | object centre; collider none in scene file | Camp to Jg |
| 2.49 | SliceLook/ShotGround/[DeadLeaves2] | (242.42, 170.47) | object centre; collider none in scene file | Camp to Jg |
| 2.67 | SliceLook/ShotGround/[CS_Stone_3] | (243.11, 173.48) | object centre; collider none in scene file | Camp to Jg |
| 2.89 | SliceLook/OpenGround/[DeadLeaves2] | (230.07, 150.66) | object centre; collider none in scene file | Camp to Jg |
| 2.92 | SliceLook/ShotGround/[DeadLeaves2] | (244.11, 175.60) | object centre; collider none in scene file | Camp to Jg |
| 3.00 | SliceLook/ShotGround/[DeadLeaves2] | (238.43, 160.81) | object centre; collider none in scene file | Camp to Jg |
| 0.40 | Ground815/TrailEdges/[CS_Stone_3] | (261.40, 173.30) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.31 | Ground815/TrailEdges/[CS_Stone_1] | (265.30, 188.65) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.32 | Ground815/TrailEdges/[CS_Stone_4] | (261.93, 176.68) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.32 | Ground815/TrailEdges/[CS_Stone_5] | (276.11, 224.61) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.33 | Ground815/TrailEdges/[CS_Stone_1] | (265.36, 213.94) | object centre; collider mesh convex | Jg to Camp 1 |
| 1.34 | Ground815/TrailEdges/[CS_Stone_6] | (269.32, 203.73) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.35 | Ground815/TrailEdges/[CS_Log_Firewood_Short] | (279.99, 226.81) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.36 | Ground815/TrailEdges/[CS_Stone_3] | (267.78, 216.81) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.37 | Ground815/TrailEdges/[CS_Stone_2] | (282.70, 234.36) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.38 | Ground815/TrailEdges/[CS_Log_Firewood_Short] | (269.22, 205.57) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.43 | Ground815/TrailEdges/[CS_Stone_8] | (282.84, 236.36) | object centre; collider mesh convex | Jg to Camp 1 |
| 1.47 | Ground815/TrailEdges/[CS_Stone_5] | (267.19, 221.27) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.48 | Ground815/TrailEdges/[CS_Stone_7] | (256.79, 184.91) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.48 | Ground815/TrailEdges/[CS_Stone_3] | (273.97, 221.09) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.49 | Ground815/TrailEdges/[CS_Stone_4] | (283.42, 229.27) | object centre; collider none in scene file | Jg to Camp 1 |
| 2.47 | SliceLook/OpenGround/[DeadLeaves2] | (265.13, 210.65) | object centre; collider none in scene file | Jg to Camp 1 |
| 2.90 | SliceLook/OpenGround/[DeadLeaves1] | (270.09, 224.42) | object centre; collider none in scene file | Jg to Camp 1 |
| 1.22 | Ground815/TrailEdges/[CS_Stone_7] | (271.17, 180.03) | object centre; collider mesh convex | Jg to T |
| 1.23 | Ground815/TrailEdges/[CS_Stone_2] | (269.36, 180.30) | object centre; collider none in scene file | Jg to T |
| 1.29 | Ground815/TrailEdges/[CS_Stone_5] | (329.40, 168.16) | object centre; collider none in scene file | Jg to T |
| 1.34 | Ground815/TrailEdges/[CS_Stone_2] | (321.49, 159.88) | object centre; collider mesh convex | Jg to T |
| 1.34 | Ground815/TrailEdges/[CS_Stone_3] | (326.08, 162.26) | object centre; collider mesh convex | Jg to T |
| 1.37 | Ground815/TrailEdges/[CS_Stone_7] | (293.48, 180.30) | object centre; collider none in scene file | Jg to T |
| 1.40 | Ground815/TrailEdges/[CS_Stone_2] | (301.59, 178.05) | object centre; collider none in scene file | Jg to T |
| 1.41 | Ground815/TrailEdges/[CS_Stone_8] | (309.80, 169.61) | object centre; collider none in scene file | Jg to T |
| 1.42 | Ground815/TrailEdges/[CS_Stone_8] | (306.28, 171.67) | object centre; collider none in scene file | Jg to T |
| 1.43 | Ground815/TrailEdges/[CS_Stone_3] | (310.70, 160.48) | object centre; collider none in scene file | Jg to T |
| 1.44 | Ground815/TrailEdges/[CS_Log_Firewood_Short] | (291.95, 180.69) | object centre; collider none in scene file | Jg to T |
| 1.46 | Ground815/TrailEdges/[CS_Stone_6] | (281.26, 177.95) | object centre; collider none in scene file | Jg to T |
| 1.51 | Giants/Heroes/Gate_Tree/Trunk | (290.00, 176.00) | trunk r 4.00 | Jg to T |
| 1.52 | Ground815/TrailEdges/[CS_Stone_6] | (309.00, 162.86) | object centre; collider none in scene file | Jg to T |
| 1.53 | Ground815/TrailEdges/[CS_Stone_5] | (304.21, 175.60) | object centre; collider none in scene file | Jg to T |
| 1.59 | Ground815/TrailEdges/[CS_Stone_4] | (335.26, 171.95) | object centre; collider none in scene file | Jg to T |
| 1.60 | Ground815/TrailEdges/[CS_Stone_1] | (263.59, 179.05) | object centre; collider none in scene file | Jg to T |

## 5. What the tower deck sees, terrain alone
Grid every 5 m over the scope (cells outside it are blank). Each cell shows how many of the 128 deck eyes see it:
- "A": all 128 eyes see both the ground and a point 1.7 m above it;
- "a/b": a eyes see the ground and b eyes see the point 1.7 m up;
- "-": no eye sees the point 1.7 m up.

Trees, crowns, brush and hedges are not counted.

| z \ x | 180 | 185 | 190 | 195 | 200 | 205 | 210 | 215 | 220 | 225 | 230 | 235 | 240 | 245 | 250 | 255 | 260 | 265 | 270 | 275 | 280 | 285 | 290 | 295 | 300 | 305 | 310 | 315 | 320 | 325 | 330 | 335 | 340 | 345 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 245 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| 240 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |
| 235 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |
| 230 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |
| 225 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |
| 220 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |
| 215 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| 210 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| 205 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| 200 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 195 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 190 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 185 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 180 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 175 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |
| 170 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |
| 165 |  |  |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | 0/128 | A | A |
| 160 | A | A |  |  |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |
| 155 | A | A | A |  | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |
| 150 | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |
| 145 | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |
| 140 |  | A | A | 0/128 | 85/128 | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A | A |  |  |
| 135 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A | A |  |  |  |
| 130 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A |  |  |  |  |
| 125 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A | A |  |  |  |  |
| 120 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A | A |  |  |  |  |  |  |
| 115 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A | A |  |  |  |  |  |  |  |
| 110 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A |  |  |  |  |  |  |  |  |
| 105 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A | A | A |  |  |  |  |  |  |  |  |
| 100 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  | A |  |  |  |  |  |  |  |  |  |

Trail markers every 10th, and the named spots (spot rows: forage A, forage B, forage C, the Old_Burn warp, Junction_Jg):

| Point | Position | Eyes seeing the ground | Eyes seeing 1.7 m up |
|---|---|---|---|
| Camp to Jg P20 | (186.4, 147.4) | 128 | 128 |
| Camp to Jg P30 | (196.9, 145.8) | 128 | 128 |
| Camp to Jg P40 | (207.0, 150.4) | 128 | 128 |
| Camp to Jg P50 | (215.6, 155.6) | 128 | 128 |
| Camp to Jg P60 | (228.3, 148.1) | 128 | 128 |
| Camp to Jg P70 | (237.3, 153.4) | 128 | 128 |
| Camp to Jg P80 | (243.7, 162.8) | 128 | 128 |
| Camp to Jg P90 | (248.1, 174.5) | 128 | 128 |
| Camp to Jg P100 | (261.6, 171.8) | 128 | 128 |
| Jg to T P0 | (262.0, 172.0) | 128 | 128 |
| Jg to T P10 | (268.7, 181.6) | 128 | 128 |
| Jg to T P20 | (280.0, 175.7) | 128 | 128 |
| Jg to T P30 | (291.8, 182.2) | 128 | 128 |
| Jg to T P40 | (302.1, 179.4) | 128 | 128 |
| Jg to T P50 | (308.4, 169.2) | 128 | 128 |
| Jg to T P60 | (316.0, 159.7) | 128 | 128 |
| Jg to T P70 | (327.5, 164.1) | 128 | 128 |
| Jg to T P80 | (334.8, 170.4) | 128 | 128 |
| Jg to Camp 1 P0 | (262.0, 172.0) | 128 | 128 |
| Jg to Camp 1 P10 | (257.8, 182.6) | 128 | 128 |
| Jg to Camp 1 P20 | (267.2, 191.3) | 128 | 128 |
| Jg to Camp 1 P30 | (268.8, 201.0) | 128 | 128 |
| Jg to Camp 1 P40 | (267.3, 211.9) | 128 | 128 |
| Jg to Camp 1 P50 | (270.6, 221.5) | 128 | 128 |
| Jg to Camp 1 P60 | (281.8, 227.3) | 128 | 128 |
| Camp 2 to T P0 | (298.9, 107.8) | 128 | 128 |
| Camp 2 to T P10 | (300.1, 116.7) | 128 | 128 |
| Camp 2 to T P20 | (308.6, 121.3) | 128 | 128 |
| Camp 2 to T P30 | (312.8, 130.0) | 128 | 128 |
| Camp 2 to T P40 | (323.0, 135.4) | 128 | 128 |
| Camp 2 to T P50 | (322.7, 146.7) | 128 | 128 |
| Camp 2 to T P60 | (335.2, 151.7) | 128 | 128 |
| Camp 2 to T P70 | (341.3, 160.1) | 128 | 128 |
| spot | (239.6, 163.0) | 128 | 128 |
| spot | (140.8, 167.1) | 128 | 128 |
| spot | (229.5, 273.5) | 128 | 128 |
| spot | (239.6, 157.8) | 128 | 128 |
| spot | (262.0, 168.0) | 128 | 128 |

Marlow
