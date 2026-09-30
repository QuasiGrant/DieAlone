# Valley re-walk review (8.9j, 8.9k)

2026-09-29, Marlow. Paper only: Rook holds the Editor, nothing walked by me. Read: commits 4e1415d and 50050ea, Docs/Layout/Main3 (Main3_sightlines.md, Main3_E1.md, Main3_top.png), Docs/Look/Edges, Valley.md rev 6, ValleyNumbers R2 to R4, Rook's reported results, Wren's rulings (leg exception, slope 23.6 percent, cap rock at the cleft exit, Vesper's edge fixes). WalkChecks.md updated for the valley in the same pass.

## 1. Findings

1. **8.9j built from a revision I never passed. Hurts (process).** PLAN 8.9j: "latest revision that Marlow has passed on paper". ValleyNumbers ends at R4: FAIL (R4.3). No R5 check exists. Rev 5 changed only what R4.3 asked for, and E-1 now proves it on meshes (item 2), so the gap is closed by the build, not by a paper pass. Wren's call whether to record it.
2. **Committed F-1 report is stale. Hurts.** Main3_sightlines.md (4e1415d) says F-1 least 5.9 m. Rook reports 1.2 m at part B beside the fin. The report in git does not show the number the pass rests on. Repro: compare Main3_sightlines.md F-1 line with Rook's result.
3. **F-1 passes on visible tops, not card tops. Hurts until checked.** Deck sees 3807 of 56832 card-top rays, places 27. The pass counts tops two thirds up each card on the recipe's claim that the flipbook fills only the lower two thirds (main3_8_9_sightlines.cs:133, 207). Unverified. Ledge_west_from_the_path_end.png shows flame wisps well above the flame bodies. If any frame puts a flame pixel in the upper third, the tower sees fire on day one (DECISIONS: the land hides it). Repro: step the FlameCard flipbook frame by frame, check the top third is clear.
4. **Check 1 not run as written. Hurts.** main3_walk_trails.cs still uses a fixed downward push: no gravity, no jump, no crouch, no drop record, no time. "All pass" on check 1 is walk reachability only. Same mover in main3_8_9j_climb_check.cs.
5. **The climb's side pushes are not checked. Blocks the re-walk.** Benches are stacked 13 m apart and 20 m up. Check 4 (25 m pushes, 12-direction sprint-jumps) has no recipe. 8.9e_rewalk part d pushes 4 m only. Ledge edge and platform edges are checked at one point each (x -15 at the path-end z; leg 1 at z 250). A drop off a downhill bench edge onto the leg below is not ruled out anywhere.
6. **Cairn gate check aimed at rev 16 ground. Hurts.** main3_8_9e_rewalk_check.cs part c pushes toward (99.5, 209.9), a rev 16 climb point. The new trail runs west along about z 205.5. 8_9a b4a and b4b are blocker-relative and still valid. WalkChecks 6 now says to aim from the GateBlocker.
7. **Stop rocks sit on the wall line with no collider. Cosmetic, unverified.** Rules and Tips: rocks laid on the thicket wall line; Valley.md 1.4 puts the invisible stop 2 to 4 m behind them. A walker can step into the half of each rock in front of the wall. Vesper's stagger may cover it; check 14 tests it.
8. **Main3_E1.md says "Places stand in for the platforms until they exist".** P1 to P4 exist since 4e1415d. State whether they and the ledge path end are origins. Cosmetic.
9. **Main3_sightlines.md W-1 text still says "the Wall for the Ward".** Cosmetic.
10. **Check 11:** each of the four benched legs is 85 m straight by design, but only leg 1 (91 m) was reported. Sable's ruling covers them. J to leg 1, leg 5, the cleft and the ramp are not covered and need their numbers. Hurts until reported.

## 2. Mesh-check list (ValleyNumbers R3 and R4 "For Rook")

| Item | Status | Evidence |
|---|---|---|
| R3.1 knob, W-1 to 12 stone corners, least 4.9 | proven | W-1 +3 jump 4.9 (1536 rays = 64 eyes x 24 targets). Knob heights over x 8 to 18, z 242 to 280 not printed; W-1 governs |
| R3.2 ledge 98 to x -9; F-1 part B every 0.5 m; first show | proven for flames | first show eye (1.71, 246.11), jump (2.91, 242.30), both past z 237.2. Ledge height not in any report: open |
| R3.3 P4, leg 4 top, leg 5 through the W saddle; east face profile | flames proven (F-1 trail, least over 1.2); smoke open | no sheet built. As-built saddle 94 (Valley 2.9) takes 1 m off R4.1's sheet margins (4.7 to 3.7 at leg 5 top) |
| R3.4 sheet bounds and hard top | open | no sheet built |
| R3.5 E-1 on meshes | proven | 0 void, 0 flat, grazing rays on land. The two P4 rays hit the E ridge itself at (455, 44, -46), so the SE quadrant past it is proven only from the deck |
| R3.6 rev 16 ground planes | proven | Ground_North, Ground_South, Ground_East deleted in 4e1415d |
| R3.7 NW and SW corners from the ledge | no void proven (E-1); look open | Vesper's edge shots, fixes in progress |
| R3.8 J to leg 1 through the foot; leg 1 downhill stops | slope proven after the fix (23.6 percent, rerun to confirm); stops open | one downhill point tested (item 5) |
| R4.1 outer floor spans; E-1 deck and P4 | proven | as R3.5 |
| R4.2 far ranges: no gap, no floating lip | no gap proven; lip open | Vesper removing floating slabs |
| R4.3 ground planes gone | proven | as R3.6 |
| R4.4 NW corner 98.8 at (6, 350); W saddle; east face south of P4 | open | built heights not reported except saddle 94, z 0 98.8 |
| R4.5 part B and the lip | open | stones seen from part B (the cap rock fixes it); F-1 1.2 at part B, thin |
| Valley.md 7.3 P4 headlight view | open | no groves placed |

## 3. Regressions in areas Grant walked

None shown. Every failure Rook reported is on new ground (J to leg 1, leg 1, part B). Not proven for:
- the lake: check 5 has no sweep, and the terrain was resampled (1025 heights, origin (-40, -200));
- trail jump, crouch, drops and times: item 4;
- the cabin: 8.9k changed the ceiling (35.1 to 35.0) and the ridge joint, so check 9's cabin exit must be in the rerun.

Rook gave no tower climb time. The rerun must print it (Grant walked 22.4 s).

## 4. Pass once the cap rock and slope fixes land?

No, not on the current reruns. 8.9j: yes, if the reruns below pass and items 2 and 3 are closed. 8.9k: no, until item 5 is covered and I walk the climb and ledge myself. 8.9k's done-check needs Marlow's re-walk; "every check with a recipe passes" is met by leaving checks 4, 5 and 11 without recipes, which proves nothing about the benches.

Reruns, in order, after the cap rock (rev 7), the slope fix and Vesper's edge fixes:
1. main3_rebuild.sh (full runner; includes main3_8_9_sightlines.cs and main3_e1_edges.cs). Commit the regenerated Main3_sightlines.md and Main3_E1.md.
2. main3_day_one_state_check.cs
3. main3_8_9e_edit_check.cs (J to Ward steepest 10 m, 25 or less; P1 to P4, cleft, path end heights)
4. main3_8_9c_rewalk2_edit_check.cs (stones: 0 seen before the reveal zone)
5. main3_walk_trails.cs
6. main3_walk_legs.cs and main3_8_9c_rewalk2_play_check.cs (places, camp fan)
7. main3_8_9c_check.cs (tower up time printed; J to ledge)
8. main3_8_9j_climb_check.cs (J to ledge time and length)
9. main3_8_9a_fix_check.cs, main3_8_6_fence_check.cs (gate, fence, dock, wade)
10. main3_8_9e_play_check.cs and main3_8_9e_rewalk_check.cs (Wall_100, rim pocket, bench pushes; part c retargeted to the blocker)
11. main3_8_9d_check.cs, twice (cabin exit)
12. main3_edge_shots_8_9k.cs (to Vesper)
13. Rook's check 11 probe: every J to Ward segment, not only leg 1

Still needed, no recipe: check 4 on the climb (every bench, P1 to P4), check 8's ledge edge sweep and part B sprint-jumps, check 14. Without these I walk them by hand before I pass 8.9k.

Marlow
