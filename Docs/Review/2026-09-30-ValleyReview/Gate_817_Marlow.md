# Gate step 2, PLAN 8.17 (places), and the 8.16b lake recheck

Marlow, 2026-09-30. Sheets: Docs/Captures/Main3Review_8_17 (19:22, capture of df3a066): Places.jpg cropped row by row, Checks.md, index.md.
Editor work: Play mode on Main3 as committed at df3a066 (HEAD moved to abddeda during the run, docs only; Main3.unity unmodified; Rook's uncommitted recipe edits were not run). PlayerController.Step, dt 0.02. Walk = no sprint, no jump; sprint-jump = sprint with jump held on every step, so it jumps on every landing. Play stopped after; nothing written into the project.
Scope: 8.17 only. Gate 2.1, 2.3, 2.5 to 2.9 not rerun (no trail, warp, lot, climb or menu change in 8.17 except the ruin warp).

Method per place: a 1 m grid flood from the place's warp or nearest trail point, each move to all 8 neighbours with walk and with sprint-jump, cells keyed by height too (cave and stack top). Trap = a cell reached that cannot get back to the start or out of the box. Every trap was then retested with 16 headings x walk, sprint, sprint-jump for 3 s.

| Place | Box (x, z) | Cells | Traps | Targets (nearest walked) |
|---|---|---|---|---|
| Office and store | 334-376, 184-212 | 726 | 0 | front room 0.6 m, back room 0.6 m, store door 0.4 m |
| Camp 1 | 262-302, 218-258 | 1546 | 0 | centre 0.6 m |
| Camp 2 | 276-314, 88-124 | 1608 | **1** | payphone door 0.6 m, card table seats 0.6 and 0.7 m, stack top 0.3 m |
| Camp 3 | 62-106, 128-162 | 1052 | 0 (4 flagged, all slide down the hollow wall, escape 16 m) | easel stool 0.5 m, rim warp 0.6 m |
| Cave, from Cave_Chamber | 60-100, 0-30 | 449 | 0 | side room table 1.1 m (table in the way), far corner 1.1 m |
| Boathouse | 230-262, 38-68 | 543 | 0 | inside 0.4 m; step, bowl and chair not reached (see 3) |
| Pump and dock | 176-206, 80-112 | 664 | 0 | pump 0.6 m; dock end not reached (see 2) |
| North ruin | 156-190, 262-296 | 1093 | 0 | inside 0.5 m |
| Ward stones | -24-8, 232-260 | 444 | 0 | |
| Trail boulders 1 to 4 | 20 m round each | 378 to 450 | 0 | walked round each |

## Findings

1. **Camp 2: a trap between the boulders and the stack. Blocks.**
   - Where: (290.2, 5.2, 113.5), on Dressing/BigBoulders_0, wedged against BigBoulders_1, BigBoulders_5 and GraniteStack. 9.6 m from the Camp 2 to T trail, 13 m from the Camp 2 warp.
   - Repro: stand at (287, 107) below the stack, sprint toward (290.2, 113.5). No jump needed. 34 of 340 approaches from a 1 m grid round it end there, walk and sprint-jump.
   - Escape: 0.2 m at most, from 48 tries (16 headings x walk, sprint, sprint-jump) and again from 8 sprint-jump headings. The player has to quit.
2. **Dock: an invisible wall across the deck, 1.3 m short of its end. Hurts.**
   - Lake/WadeLimit W23 and W24 run across the dock at z 87.61 to 88.08, from y -8.5 to -2.08 (1.8 m over the deck). The player stops at z 88.44 walking down from the pump. The rail-framed end (z 86.4 to 87.6) cannot be reached.
   - Gate 2.2: no visible reason, and it is not one of the exceptions. The DOCK END LOOKING BACK frame stands where no player can.
   - Repro: warp Lake_Pump, walk south down the dock ramp to the end rail.
3. **Boathouse step, bowl and chair (M8): out of reach. Cosmetic, unless it is meant to be reached.**
   - Step at (240, -3.8, 55.9), outside the north window wall. Its planks have no collider. There is no door on that side, and no path to it round the outside over the water.
   - Valley.md M8 asks only "seen from the deck". M7 puts the fishing spots "within 10 m of the step", so the step may be meant as a standing spot. Wren's call.
4. **North ruin against Valley.md E11 ("not seen from the deck; no tower anywhere in view"). Hurts, if E11 is read as written.**
   - Warp North_Loop_Ruin (165.8, 3.7, 267.7), yaw 25: on the Camp 1 to J trail, on the ground, faces the ruin. The doorway is the first thing its line hits (12.4 m). PASS.
   - Tower view from the ruin: 9 of 9 points on the tower cab are in clear line from each of 5 eyes (warp, 2 m out of the doorway, inside, 5 m east, 5 m north). Tree LOD0 meshes counted as solid, so the result can only overstate cover. At night the lit cab window shows from the doorway (render in my scratchpad, not committed). Sheet: NORTH RUIN FROM THE NORTH has the tower lattice in frame.
   - From the Tower_Deck warp, 15 of 15 points on the cabin are in clear line. Not confirmed by a picture: my render ran at night and showed nothing past the rail.
   - Rook's 4 m east and 3 m north move is the likely cause, but I have not tested the old spot (168, 278).
   - If "no tower anywhere in view" means no tower of its own, the ruin passes that half. "Not seen from the deck" fails either way.
5. **Places.jpg: frames that cannot pass Gate 2.4. Hurts the gate, not the game.**
   - OFFICE FROM SOUTH-WEST 19 M: the eye is inside Ground815/Stops/Hedge_Burn_8 brush; bush fills the frame.
   - OFFICE WEST DOOR 6 M: a bush sits in front of the door. The eye (338, 200) is inside the same hedge.
     - Hedge_Burn_8 stands 2 to 5 m west of the porch, so the RANGER STATION sign faces into a stop hedge. From the deck, the west door is in clear line, 15 of 15 points (M3 holds).
   - CAMP 3 FROM THE RIM: a trunk fills the frame.
   - WARD STONES FROM THE WEST: faces a flat tan plane; no stones in frame.
   - DOCK FROM THE PUMP TRAIL: the pale triangle at right is the camera inside Stops/FaceRock/BigBoulders_2 (191.3 to 194.8, 99.5 to 103.2).
   - CAVE CHAMBER TO THE SIDE ROOM: black. Nothing reads. The three side room frames do read: table, two chairs, lamp.
   - CAMP 2 FROM THE WEST: a boulder fills the left half.
   - With these, 7 of 48 frames show no place. Retake before Vesper's grades for the office, Camp 3, the Ward stones, the dock and the cave chamber can stand.
6. **Camp 1 to J: the trail line runs into furniture. Cosmetic.**
   - The trail passes through Camp_1/Workbench at P7, (270.8 to 271.5, 245.1 to 245.7). The walker slides round it.
   - The trail ends inside Dressing/Spar at P0 (282, 238). Walk and sprint-jump both stall 0.6 m short.
7. **Office west door: opened from inside, it swings out through the porch Bench (at 45 degrees). Cosmetic.**
8. **Payphone: the booth is a solid MeshCollider, (299.33 to 300.67, 98.27 to 99.73). Cosmetic until the minigame's use is specified.**
   - The player reaches the booth door (0.6 m from (298.9, 99)) but cannot step in. Valley.md M5 says "post"; the recipe builds a booth.

## Asked checks

9. Walk in and round each place: **PASS except 1.** Table above.
10. Doors: **PASS.**
    - Both office doors: the interact ray from 1.3 m hits the panel from either side, within 2 m reach.
    - Both swing to 90 degrees and let the walk through. Front room and back room reached.
    - The store door stays shut by design (DECISIONS 2026-09-30).
    - The cave board closes the mouth by day one (DayOneBoard active).
11. Stuck spots: **FAIL**, 1 (finding 1).
12. Minigame spots: **PASS.**
    - Payphone (300, 99): the booth door is reached (finding 8).
    - Card table (297, 98): both chair sides reached.
    - Cave side room: reached from the chamber.
    - Camp 3 easel: stool spot reached, 0.5 m.
    - Office back room: reached through the inner door.
13. North ruin: **warp PASS, no stuck spots PASS, tower view against E11 FAIL** (finding 4).
14. Rook's notes:
    - Office doors swing: checked, yes (10).
    - Ruin offset: logged in the build notes. The new spot is in sight of the deck and of the tower (4).
    - Dock bucket: no collider now, so the player passes through it on the Pump to boathouse trail. Cosmetic. No other place collider sits on a trail line (capsule test every 5 percent of every segment) except the known ones: the Camp 2 ramp rail at P1, the cave DayOneBoard, and 6.
15. Hand walk (Gate 2.10): **PASS.**
    - Every trail both ways, walk and sprint-jump, gates as they stand: 56 walks, 50 end to end.
    - Stalls: 4 at the J to Ward day gate (by design by day), 2 at the Camp 1 spar (6).

## 8.16b recheck: lake wade leak

16. **PASS. The leak is shut.**
    - My 8.16a repro (Pump to W1 P48 (144.4, -4.3, 81.7), toward (147.5, 72.6), headings -20 to +20 degrees, sprint and sprint-jump): lowest feet -5.35; water is at -5.5.
    - My sweep:
      - Starts: 129 trail points within 18 m of the lake, a 1 m grid on the dock, boathouse floor, gangway and both pocket decks, and a 2 m ring of shore ground to 8 m out.
      - Moves: 16 headings x walk, sprint, sprint-jump, 4 s each; 39648 tries.
      - Result: 0 real entries. The 30 flagged all came from one start, (134, 64), which is inside wade boxes W45 and W46 and is not a player spot. The walked flood from Junction_W1 comes no closer than 1.1 m to it.
    - Rook's LAKE JUMP tries two headings and jumps every 0.5 s. It passes, but it is narrower than this sweep.

## Done-check

17. PLAN 8.17: "the eye-height gate passes and Vesper grades every place C or better".
    - The gate does not pass: finding 1 blocks, and 2 and 4 hurt.
    - Vesper's grades are hers. Seven frames that show no place (5) need a retake before a grade can rest on them.

**Verdict: FAIL.**

Marlow
