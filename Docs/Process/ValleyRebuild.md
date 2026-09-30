# Valley rebuild of Main3 (process draft)

2026-09-29, Tully. Grant approved building tonight, walkable gray valley plus the edges that matter for the walk. Inputs: Sable's draft, Docs/Review/2026-09-29-ValleyNumbers.md (Marlow), Docs/Design/Edges.md (Vesper). Draft lines only; Wren writes PLAN.

## 1. Recipes (Tools/Recipes)
- **Change:** main3_8_1_scene_ground.cs (terrain grows west, ridges, knob, ledge; Wall face, plateau, cliff x 10, Wall_Cliff and Wall_West go; DevWarps Ward_Pass and Ward at lines 355 to 356 move). main3_8_3_trails_giants.cs (J to Ward legs and profile; thicket outline shape for the new climb; cliff-edge band x < 32 and the 46 cap go). main3_8_7_ward.cs (stones and lip on the ledge; StandInFire west of the ridge). main3_8_9f_look.cs (line 148: the fire's LookVisibility goes; fire stays on). main3_8_9e_layers.cs (near N, S, E bands give way to the map ridges; far range stays). main3_8_9_sightlines.cs (W-1 against the knob; new F-1). main3_rebuild.sh (F-1 pass string; 8.9g recipe, see risk 1). main3_topdown.cs (tilesX 4 to 5, origin x -40). Checks: main3_8_9c_check.cs (J to ledge time, lip stop), main3_8_9e_edit_check.cs (pass and lip heights), main3_8_9c_rewalk2_edit_check.cs part 2 (stones first seen), main3_8_9e_rewalk_check.cs part d (climb shelves), main3_8_9c_fire_pose.cs (new lip). README.md rows for all of these.
- **Archive:** no recipe is wholly obsolete. Tag `main3-rev16` before the first valley commit: it archives Main3.unity, Assets/Terrain/Main3 (terrain, Thicket, Backdrop meshes) and the rev 16 recipe set, as main-scene-2.0 did. Removed sections leave the live recipes in the same commit; the tag holds them. Nothing deleted.
- **Stay:** 8_2, 8_4, 8_5, 8_6, 8_6_fence_check, 8_8 (rerun, must pass), 8_9a_fix_check, 8_9c_rewalk2_play_check, 8_9d_check, 8_9d_dress_camp, 8_9d_pose, 8_9e_play_check (Wall_100 is the ravine, still valid), 8_9f_pose, main3_walk*, main3_reset. Main 1.0 and 2.0 fire and cliff recipes (build_valley, build_horizon, ward_cliff, fire_cliff_fix, raise_ward) stay history; they target Main and Main2, never reuse.

## 2. Draft PLAN lines (Wren's style)
Gate, not a task: Sable's Main3.md revision 17 and Main3_map.svg answer ValleyNumbers 1.2, 1.7, 1.8, 2.1, 3.2 and 5.1 to 5.6, and DECISIONS carries dated lines for the valley, the 40 m growth and day-one smoke. Rook builds from the frozen revision only.
- [ ] 8.9j Valley ground and Ward ridge. Build Main3.md revision 17: tag main3-rev16 first; the terrain grows 40 m west by placing it at x -40 (no coordinate in any recipe or doc changes); ridges west, north, south and east as terrain with colliders; the Ward knob, ledge and new climb; the Wall, plateau, cliff at x 10 and cliff-edge giant band removed; the stand-in fire west of the ridge, always on. Each change in its owning recipe; F-1 added to the sightline recipe: rays from every place, every trail point at 10 m and the deck grid (eye, jump, +3 m) to every flame top must hit terrain. Done when: no errors, the runner rebuilds cleanly, 8.8's cave check passes, the sightline report passes W-1, C-1 and F-1 with the least margins stated, the J to ledge climb is timed on screen against revision 17 table 4, and the top-down image is committed. Wren ticks after Marlow's re-walk.
- [ ] 8.9k Valley walk checks and walk edges. Retarget the check recipes listed in ValleyRebuild.md 1 to the new climb and ledge; the day-one state check expects the fire on; the Backdrop sits behind the ridges with nothing floating or showing through; E-1 (Edges.md 1.8) as a recipe. Marlow updates WalkChecks.md. Done when: no errors, every WalkChecks check with a recipe passes, E-1 passes, and Marlow's re-walk passes. Wren ticks.
- 8.10 stays as is. Full edge dressing (Edges.md 2 to 6) waits for Milestone 11.
- Safe stop: until 8.9j's runner passes, Main3.unity stays at rev 16 in git; Status.md names the last recipe changed.

## 3. Parallel (no Unity; Rook is the only one in the Editor, after 8.9g)
- Sable: revision 17 and the map (blocks 8.9j). Table 2.1, 3.6, 3.7, 4 J row, 5.4, 5.9, 8.
- Pim: DevWarps list for the new climb (J, ridge foot, knob shoulder, ledge) and the J junction marker (4.1) on paper; checks 8.9h screenshots.
- Hollis: fold Docs/Design/Sound/Valley.md into revision 17's sound column through Sable; nothing built (Milestone 9 postponed).
- Quill: Docs/Private/EventList.md and ScareList.md mention the Wall, plateau or pass (grep hits, unread); bring them to the ridge under Grant. Twist content stays private.
- Marlow (not free): paper check of revision 17 before Rook starts, per DECISIONS 2026-09-29.

## 4. Risks
1. **8.9g reverts on rebuild.** main3_8_9d_dress_camp.cs:449 writes LookTuning_DayOne sun 14 and the old fill; main3_8_9f_look.cs:89 to 91 copy day-one crushBlacks and darkCorners into the night look. look_day_one_8_9g.cs is not in the runner. Rook: put the 8.9g values in 8.9d:449 (or run 8.9g after 8.9f_look) and confirm the night look is unchanged.
2. Renumbering x instead of placing the terrain at x -40 touches every recipe, doc and check coordinate.
3. Heightmap 513 over 440 m coarsens samples from 0.78 to 0.86 m; the cave hole cells (8.8) and trail flattening shift. Going to 1025 costs rebuild time. Rook states which.
4. Ridges as colliderless Backdrop cannot be proven by F-1 or W-1 (ValleyNumbers 1.8). N and S ridges inside z 0 to 300 meet the cave (z 3 to 37), lake south bank and closed loop (z 242 to 282); outside needs a larger terrain.
5. Open blockers from Marlow: fire seen from the tower on SW and NW bearings (1.2); ridge foot over the cave ravine and Camp 3 rim (1.7); saddle at 100 sees the valley (2.1); climb at least 425 m (3.2).
6. DECISIONS 2026-09-29 "the land hides it" vs day-one smoke hidden by wind; WalkChecks check 12 ("StandInFire is inactive") inverts. Needs a dated line first.
7. Grant walked and could regress: tower climb 22.4 s, cabin exit, camp fan, Camp 2 ramp, lake wade and dock, gate and fence, cave passage, dev panel warps. Runner plus WalkChecks 2, 5, 6, 7 and 9 must pass unchanged.
8. The 13 slice captures (8.9f, Grant not yet looked) and LookSlice S3 Cab west show the rev 16 west view. Grant looks before the rebuild, or they are retaken after.
9. New walkable ground not added to 8.3's thicket shape list is either walled off or open off-trail (MAIN3 THICKET).
10. **Assets/Docs/Look/DayOneFix** (Cabin, Giants, S1 PNGs, untracked) is the capture_game_view save_path trap (Rules and Tips line 1): screenshots imported as Assets. Rook: move to Docs/Look/DayOneFix, delete Assets/Docs through the Editor, do not commit Assets/Docs.meta.

Tully
