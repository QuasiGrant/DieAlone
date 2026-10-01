# Archived with PLAN 8.20 (the Ward path rebuild, 2026-10-01)

WardPath.md draft 2 replaced the four-leg climb (P1 to P4, the cwm, leg 4) with the stair on the face, closed by the fallen giant at
z 272. Main3 is still built by the old recipes first; main3_8_20_ward_path.cs then reshapes the stair region, rewrites the J to Ward
trail and adds the new pieces, so the land behind the giant (the old shelf north, P2, the cwm, P3, leg 4 past the lookout) stays as
scenery.

Moved here, because they walk or push the old four-leg route and no longer test what the player can reach:
- main3_8_9j_climb_check.cs (8.9j timed climb with the push mover)
- main3_climb_push_check_8_9k.cs (8.9k pushes aimed at the old legs)
- main3_straight_view_climb_8_9k.cs (8.9k straight views on the old legs)

Kept in place, overridden by 8.20 where they touch the new route:
- main3_8_1_scene_ground.cs: the climb carving (8.20 restores and reshapes x 20 to 82, z 196 to 276 from its own base copy).
- main3_8_3_trails_giants.cs: ClimbPts (8.20 rewrites the J to Ward points between the shelf and the lookout and ends them at the prow).
- main3_8_7_ward.cs: P2_RockRoof and P3_RootPlate (removed), the P2 and P3 lanterns (removed; the stair foot and landings get copies of
  P4's), the bent fir and split snag (moved).
- main3_8_14_climb_check.cs: still the timed walk and the pushes; its ledge now counts the prow.
The checks for the new route: main3_8_20_closure_check.cs (giant, corner and edges, prow), main3_8_14_climb_check.cs, main3_breaks_recheck.cs.
