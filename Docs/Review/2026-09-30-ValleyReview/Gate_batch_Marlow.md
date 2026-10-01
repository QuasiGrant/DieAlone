# Gate step 2, batched build (PLAN 8.16b, 8.17, 8.18): recheck of Gate_816b_817_Marlow.md

Marlow, 2026-10-01.
- Sheets: Docs/Captures/Main3Review_batch (capture 14:28, noise band off): Climb up and back, Trail_Camp_to_pump, Walk_Views, Places, Look_Day_one, Look_Night, Look_Day_two, Night_Rule_Frames, index.md, Checks.md.
- Editor: Play mode on Main3 at scene commit 05a50b8 (HEAD e8b0337; the scene and scripts are unchanged since 05a50b8). PlayerController.Step, dt 0.02. Walk = no sprint, no jump; sprint-jump = sprint with jump held. Frames rendered from Camera.main into the scratchpad. Play stopped, runInBackground false, scene not dirty. Nothing written into the project.
- Known and not counted: the 2 log stalls (RedwoodHollowLog_0 at (91.5, 47.3), RedwoodHollowLog_2 at (179.5, 270)).

## Rechecks

1. **Climb: still FAIL. Hurts.**
   - Sheet count: 21 of 43 frames pass Wren's bars (Status.md, index.md).
   - Fixed: the shelf and leg 2 now read open. FWD 60 to 100 and BACK 36 to 166 show sky, valley floor and the tower, with fir screens and rims beside the tread.
   - Still closed:
     - The cwm, FWD 120 to 160: open 2, 0, 0, 0, 0. It is a rock bowl with no sky.
     - Leg 4 coming down, BACK 176 to 206: open 0, 0, 3, 10.
     - The chute, FWD 10 to 40: a flat head-wall plane with a straight diagonal edge fills the top half.
     - FWD 50: a flat plank-coloured face fills the lower half, with open 1.
   - The cwm cut and the terraces did not open FWD 120 to 160.
   - New in the look:
     - FWD 230 (cleft exit): a boulder hangs in the air top centre with sky under it.
     - FWD 160 to 200 and BACK 166 to 206: saturated dark-red smears on the ground and lower faces. I could not tell from the frames which layer or mesh it is (unverified).
2. **Climb trap: NEW. Blocks.**
   - Where: (63.8, 29.2, 279.0), the steep slope east of leg 3 at P2. The player slides down to an invisible Ground815/Stops "Rims" MeshCollider and stops there.
   - Escape: 0.09 m at best from 48 tries (16 headings x walk, sprint, sprint-jump, 3 s each).
   - Reached from the tread: start at (54.7, 41.7, 276.4) on J to Ward, about 118 m.
     - Sprint-jump north-west to (52.3, 42.4, 278.4).
     - Walk north to (52.1, 42.4, 280.4).
     - Sprint-jump north to (52.2, 43.2, 283.7).
     - Sprint-jump north-east to (54.5, 42.2, 283.4).
     - Sprint-jump east off the edge to (58.8, 35.5, 280.6).
     - Sprint-jump north-east into the pocket.
   - From (57 to 64, z 279 to 287) a plain walk toward the pocket ends in it: 36 of 138 approaches from rings at 4, 7 and 10 m.
   - Rook's own TRAPS drop check reports it as FAIL in Checks.md.
3. **Camp 2 trap: PASS.**
   - A 1 m grid of direct approaches round (289.6, 113.3), from 2 to 9 m: 244 starts x walk, sprint and sprint-jump, 361 distinct end places.
   - Every end place was tested with 48 escape tries, and none held the player.
4. **BOULDER POCKETS 0: agree for Camp 2 (3).** I did not rerun the other 4 clusters. The climb trap (2) is not at a boulder cluster.
5. **Trunks: PASS.** The 4090 walks put 0 into a trunk. Of 1425 trunk, log, snag and dead-tree renderers, 23 have no collider. All 23 are Ward/StandInFire/BurningGiants trunks out past the ledge, where no player can reach.
6. **Pump trench: still FAIL. Hurts.**
   - FWD 40: the streaked, smooth, poured-looking wall fills the upper half. BACK 30 and 40, right side: the same wall.
   - The batch's "trench face rock" put a vertical streak on it, but it still reads as a cast panel, not rock.
   - BACK 30 also shows a grey cylinder above the right wall that I did not identify.
7. **Cave.**
   - Boulder mouth: **PASS**. CAVE MOUTH FROM THE TRAIL and FROM THE EAST read as a boarded door in a boulder pile.
   - Seated rocks: **still FAIL. Hurts.**
     - Bounds put every chamber and side room boulder's bottom at -18.3, under the floor (-18.05), so a bounds check passes.
     - The meshes' lowest vertices are higher than that:
       - Chamber: -17.8 to -17.97.
       - Side room: BigBoulders_5 -17.28, BigBoulders_1 -17.46, BigBoulders_0 -17.51, BigBoulders_3 -17.66, Boulder_3 -17.72.
     - The side room is 3.5 m high (floor -18, ceiling -14.5), and its 4.6 to 5.6 m boulders run up through the ceiling.
     - On screen they float. From an eye 0.45 m off the chamber floor, BigBoulders_4 (80, 21), Boulder_1 (89, 5.5) and BigBoulders_2 (71, 5) all show floor under them. SIDE ROOM TABLE and SIDE ROOM FROM THE EAST show boulders hanging from the ceiling over the table.
   - Roof-crack light: **FAIL. Hurts.**
     - CrackShaft (spot, intensity 3, range 14) and the RoofCrack mesh exist at (91.8, -15, 11.5).
     - No shaft or bright crack shows in any of the 4 SIDE ROOM frames or in my chamber renders. The room reads lit by the bulb line only.
   - Also: from the Cave_Chamber warp (74, -16.2, 12) facing west, a white sky-coloured rectangle shows top left, a light leak toward the passage. Its position is unverified. Cosmetic.

## 8.18 at night, as a player

8. **Ledge reveal at night: FAIL. Blocks.**
   - What the player sees: from the Ward ledge eye (-8.5, 63.6, 246) facing west, level and 10 degrees down, almost the whole view is a flat tan-brown plane. Only a few flame tips show on its top edge. Look_Night LEDGE WEST LEVEL and 10 DOWN show the same.
   - Cause: Ward/SmokeSheet/Body (LookVisibility "DayOneAndNight"; bounds x -870 to -110, y -40 to 110) is drawn over the valley fire at night.
   - Repro: in Play, Night look, at the ledge eye, facing west. Set Ward/SmokeSheet inactive and render again: two rows of flames across the valley and the ridge appear.
   - Regression: the 818_before capture showed a wall of flames in the same frames.
   - With the sheet off the fire is there, but the flames read dull red on black. Vesper grades that.
   - The capture's flame check now runs in the day one look, where the fire is hidden by design (DECISIONS 2026-09-29), so it reports NO FLAME. Nothing checks the fire from the ledge at night.
9. **Follow Camp to J and J to Ward with the lamp (intensity 6, range 9): PASS by eye. The numbers are Pim's.**
   - My frames: Camp to J 40 and 60, J to Ward 0, 120, 130, 140, 160 and 180.
   - The lamp pool shows the bare tread as a pale band 6 to 8 m ahead, edged by brush and boulders. Past that it is black.
   - Camp to J has lanterns and the J sign in sight. At J the cairn lamp marks the Ward path.
   - J to Ward 120 to 150: no light ahead (N1 NO). I could follow the tread by its edges and the rims, not by its own brightness.
   - The capture's N2 count: 1 of 8 frames on Camp to J and 3 of 20 on J to Ward.
   - Lantern glows draw as solid flat yellow discs (J to Ward 50, 120 and 160; Look_Night compass). Cosmetic.
   - J to Ward 200 m: 638 px over 230 grey (index.md blowouts). Cosmetic.

## Hand walk (Gate 2.10)

10. **PASS except the known log.**
    - Every trail both ways at walk, sprint and sprint-jump, with the gates off: 80 of 84 end to end.
    - The 4 stalls are all at RedwoodHollowLog_2 on Camp 1 to J, (179.7, 270.8) forward and (179.2, 269.0) back, at walk and sprint. That is the known item.
    - W1 to cave passed in all 6 of my walks. Rook's 0.3 m arrival radius stalls at RedwoodHollowLog_0; my 0.5 m radius does not.

## Done-checks, word for word

11. 8.16b, "the gate passes for every stage line through 8.16": **not passed.** Open: the climb (1), the new climb trap (2), the trench (6).
12. 8.17, "the eye-height gate passes and Vesper grades every place C or better": **not passed** on the gate half. Open: cave seated rocks and crack light (7). The Camp 2 trap is closed (3).
13. 8.18, "the gate passes and Vesper grades the ledge and night C or better": **not passed.** The ledge at night shows no fire (8). The night trails are followable by eye (9).

## Verdict

**FAIL.** Blocks: 2 (climb trap), 8 (smoke sheet hides the fire at night). Hurts: 1 (climb), 6 (trench), 7 (cave rocks and crack light).

Marlow
