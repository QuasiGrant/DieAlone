# Walk checks (Main3)

Marlow, 2026-09-29. DECISIONS 2026-09-29: Rook runs these before every scene handback. Every check is scripted and reports PASS or FAIL. Marlow keeps only the checks at the end that need human eyes.

Revised 2026-09-29 for the valley (8.9j, 8.9k; Valley.md rev 7): the Wall, pass, plateau and lip are gone. The climb is J to leg 1, four benched legs on the W ridge face with platforms P1 to P4, leg 5, the cleft (parts A and B, the fin), the ramp and the ledge. The fire is always on. New checks 13 (E-1) and 14 (visible stops); F-1 joins check 10.

Retargeted 2026-09-30 for Valley.md rev 10 as built in 8.14 (3b61464): four legs (chute west, shelf north, cwm north-west, under the wall south), P4, the cleft with its dogleg and fin, the ledge. Rook's deviations are the targets: cleft exit (4, 257.3), leg 3 bend (34, 300), P2 41.5, P3 51.5, leg 2 a 61 m straight. The rev 7 targets (benches, leg 5, parts A and B, the ramp, P1 to P4 at 32 to 92) are gone; a recipe still aimed at them passes by missing.

## Common rules for every play-mode check

- **Editor state:** run `unity status` first, then `editor_status`. Run each check as a detached job: `unity command --detach eval_file`, then `unity job wait`.
- **Scene:** never save the scene. Afterwards, runInBackground must be false and `git diff Assets/Settings ProjectSettings` must be empty.
- **Mover:** use the real CharacterController and its own collision.
  - dt 0.02, gravity 20, jump height 0.6 m, so v0 = sqrt(2 x 20 x 0.6).
  - Modes: walk 2.5 m/s; jump on every landing; sprint-jump 5.5 m/s; crouch 1.5 m/s with a capsule of height 1.0, centre 0.5.
  - A move stalls when the distance to target has not shrunk by 0.05 m in 600 steps.
  - A recipe that moves with a fixed downward push and no gravity or jump (main3_walk_trails.cs, main3_8_9j_climb_check.cs as of 50050ea) proves walk reachability only. It does not pass a check that names jump, sprint-jump, crouch or drops.
- **Drop:** air-top minus landing height, recorded on every landing.
- **Sight rays:** use Physics.DefaultRaycastLayers, so the Thicket walls on layer 2 are ignored. Give every Crown a temporary MeshCollider (play mode only) and destroy it afterwards. A ray that hits the target object itself counts as seen. Keep eye points off structures.
- **Terrain:** the terrain sits at (-40, -200). Any probe that samples heights, alphamaps or holes adds terrain.transform.position (PLAN Rules and Tips, MAIN3 VALLEY).
- **Climb names (rev 10 as built; 8.1's climb array and 8.3's ClimbPts):** J (104, 206) 10; chute mouth and IW2 (86, 213) 12.6, gap x 86, z 211.5 to 214.5; leg 1, the chute, west to P1 (52, 216) 30 (three stair flights with StairRamp colliders); leg 2, the shelf, north, a 61 m straight to P2 (57, 276) 41.5; leg 3, the cwm, via (44, 278) and (34, 300) to P3 (26, 304) 51.5; leg 4, under the wall, via (30, 284) south to P4 (26, 262) 60, the look-back, facing east; the cleft: entry (24.5, 262), dogleg (18, 262) to (14.5, 265.5), slot end (4, 265.5), exit (4, 257.3) 61; the ledge path end (-8.5, 246) 62. Ledge x -10 to 6, z 215 to 285. Lip along x -10, 1.1 m over the ledge; end walls at z 215 and 285, top 66.5; fin at x 3, z 257.5 to 270, top 78. Climb ring: walls 3.5 m over the highest walkable ground within 12 m; rims 1.3 m high, 1.2 m thick. The reveal zone: past the slot exit, x under 4 and z under 257.3.
- **False results to rule out before reporting** (from .claude/agent-memory/playtester/feedback_walk_method.md):
  - straight connectors that hit small colliders
  - doors opened from a spot a player would not stand in
  - teleports into the cave
  - a probe header that disables objects
  - teleport starts no player can reach: check that the start is reachable from a trail before calling something a bypass
  - fixed target points left over from rev 16 (the pass at (112, 262), the lip at x 13 to 14.2, climb point (99.5, 209.9)): a check aimed at ground that is no longer the climb passes by missing
- **Screenshots:** never use capture_game_view with a save path; it writes under Assets. Render Camera.main to a RenderTexture inside the eval and write the PNG outside the project.

## Existing recipes (Tools/Recipes)

| Recipe | Mode | Covers |
|---|---|---|
| main3_walk_trails.cs | play | every leg under Trails, forward and back, day gates off (push mover only; see check 1) |
| main3_walk_legs.cs | play | a chain of legs and waypoints, reachability |
| main3_walk.cs | play | named waypoint routes |
| main3_8_6_fence_check.cs | play | gate and fence pushes |
| main3_8_9_sightlines.cs | edit | places from the deck, W-1, C-1, F-1, cab from junctions, next destination; writes Docs/Layout/Main3/Main3_sightlines.md |
| main3_e1_edges.cs | edit | E-1 edge rays and Marlow's grazing rays; writes Docs/Layout/Main3/Main3_E1.md |
| main3_8_9a_fix_check.cs | play | the 8.9a and 8.9b fix spots: walk, hop, crouch; cairn gate blocker and round the cairn end |
| main3_8_9c_check.cs | play | stair climb time; J to the ledge time; ledge west edge stop at one point |
| main3_8_9j_climb_check.cs | play | J to the ledge path end timed; ledge west edge stop and leg 1 downhill edge stop, one point each (push mover) |
| main3_8_9c_rewalk2_edit_check.cs | edit | camp trail ends against the knoll, camp leg grades, stones seen along the climb before the reveal zone |
| main3_8_9c_rewalk2_play_check.cs | play | camp re-entry, camp to pump timed, stair sprint-jumps |
| main3_8_9d_check.cs | play | cabin exit from the bunk spawn, practical lights |
| main3_8_9e_edit_check.cs | edit | leg lengths, steepest 10 m, ground at P1 to P4, cleft A and B, path end; groves |
| main3_8_9e_play_check.cs | play | Wall_100 (the ravine), Camp 2 ends, rev 15 leftovers |
| main3_8_9e_rewalk_check.cs | play | Wall_100 repro, rim pocket, cairn gate (still aimed at the rev 16 point (99.5, 209.9)), bench pushes 4 m up to the cleft east mouth |
| main3_8_14_climb_check.cs | play | rev 10: camp to J and J to the path end timed (real mover), climb skip and face pushes, ledge lip and end walls, IW2, IW3 |
| main3_day_one_state_check.cs | edit | check 12 |
| main3_edge_shots_8_9k.cs | play | edge shots to Docs/Look/Edges (human eyes, not a pass) |

Marlow's own probes are not committed; they are in the scratchpad.

## Checks

### 1. Every trail walks, both ways
- **Input:** every leg under Trails, centre points in order and reversed. Three modes: walk, jump on every landing, crouch. Turn off the day-one gates (CairnGate, Cave/Mouth/DayOneBoard) for the walk; they get their own check (6).
- **Pass:**
  - 0 stalls.
  - Largest drop 1.5 m or less, not counting jump arcs.
  - Walking time within 3 s, or 5 percent, of Main3.md table 4; J to Ward against Valley.md rev 10 4.1 (265 m, 106 s); camp to the path end 150 s or less (paper 138 s).
- **Recipe:** main3_walk_trails.cs, walk mode only. Replace its push mover with the real mover above; until then check 1 passes for walk reachability only, and jump, crouch, drop and time are open.

### 2. Every place is reachable both ways
- **Input:**
  - Every place's walkable centre: camp clearing, cabin interior, tower deck, pump and dock end, boathouse interior, Camp 1, Camp 2 ramp foot and stack top, Camp 3 floor, office, store, booth, car spot, closed loop, cave chamber (board off), P1 to P4, the cleft (entry, dogleg, exit), the ledge path end and the stone area.
  - Walk from the nearest leg end into the place and back out.
  - The camp gets its own test: a 24-point fan. From each camp leg start, walk to 24 points at r 10 round (170, 160). Then walk from r 12 down to each leg start.
- **Pass:**
  - Every place is reached and left, walking.
  - Fan: every failure is a straight line through the tower or cabin footprint. Anything else is a FAIL.
- **Recipe:** main3_walk_legs.cs and main3_8_9c_rewalk2_play_check.cs (camp re-entry).

### 3. Slope caps
- **Input (edit mode):** sample terrain every 0.5 m along each leg's centre points. For each window 10 m along the path, take the rise over the run.
- **Pass:**
  - Every window 25 percent or less, except the log-step segments on Camp to Camp 3 and W1 to Camp 3, and leg 1's three stair flights (0.25 rise, 0.3 run; the ramps between them stay at 25 percent or less). J to Ward has no other exception, joins included (J to the chute mouth, each platform, the cleft, the ledge).
  - Inside the camp clearing (r 15) the ground is 15 +/- 0.3.
  - Every trail end sits within 0.5 m of the ground height of the place it joins.
  - J to Ward ground within 0.5 m of the built heights: mouth 12.6, P1 30, P2 41.5, P3 51.5, P4 60, cleft exit 61, path end 62.
- **Recipe:** main3_8_9e_edit_check.cs and main3_8_9c_rewalk2_edit_check.cs.

### 4. Side pushes (thicket walls, shortcuts, drops)
- **Input:**
  - Every leg point except the ends, both sides, perpendicular to the trail, 25 m.
  - Three modes: walk, sprint-jump, crouch.
  - For the Ward climb, also sprint-jump 12 m in 12 directions from every point from the chute mouth to the cleft entry, and from every point on P1 to P4. Legs 2 and 4 are 30 m apart in plan, and leg 3 turns back over leg 4 near P3: a push that lands on an earlier or later leg (a skip), or gets over a ring wall or rim, is a FAIL. The player jump-climbs any terrain face (8.14), so only the ring walls and rims stop a push.
- **Pass:** every push ends in one of these ways:
  - within 4.5 m of a trail point (any leg);
  - inside a named clearing or a point-of-interest pocket from which the trail can be walked back to;
  - on the Ward climb, on the same leg: the nearest trail point along the climb less than 15 m away, and no gain over 2.5 m.
- **Fail when any of these happen:**
  - a push ends on a wall top, in the thicket, or off every trail;
  - a push reaches another leg of the climb, uphill or downhill;
  - a push drops more than 2.5 m.
- **Recipe:** none committed that covers every leg. The Ward climb part: main3_8_14_climb_check.cs CLIMB (real mover). main3_8_9e_rewalk_check.cs part d and main3_climb_push_check_8_9k.cs aim at rev 7 benches: retire them.

### 5. Lake and water
- **Input:**
  - Every point of Pump to boathouse, Pump to W1, Boathouse to Camp 2 and Camp to pump that lies within 1.6 lake radii of the lake centre (190, 60), where the lake radii are 54.8 m and 27.6 m.
  - Headings toward (190, 60) and +/- 35 degrees, 30 m, walk and sprint-jump.
  - Also: the dock end and dock sides, the gangway, and west from (247.2, 54.9).
- **Pass:** 0 ends inside the lake ellipse below -5.2. If a player is placed inside, jumping must get them out.
- **Recipe:** main3_8_9a_fix_check.cs covers the dock and wade limits only. The full sweep is not committed. Expected FAIL until 8.15 builds the lake edge stop (Rook, 8.14: the lake can be walked into).

### 6. Gates and fence hold
- **Cairn gate (by day):**
  - Straight on: from 3 m on the J side of CairnGate/GateBlocker, across the blocker's full width plus 1 m past each end, to 3 m on the climb side, along the trail's direction there (bearing 290, J to the chute mouth; IW2 is x 86, z 211.5 to 214.5), in walk, sprint-jump and crouch. Aim from the blocker's own transform, never from fixed points.
  - Round the gate: from 8 points at r 3 round J and the last 12 Camp to J points, toward climb points 6 to 60, in all three modes. Also from the valley side of both rock arms (x 80 to 87, z 198 to 228) toward leg 1, and from the W foot pocket (x 46 to 80, z 165 to 200) toward the chute.
  - Pass: 0 ends within 3 m of a climb point past the gate.
- **Cave board (day one):** from (50.7 / 52 / 53.3, 40) south, in all three modes. Pass: 0 ends south of z 37.3.
- **Gate and fence:**
  - Push east at z 5, 160, 170, 262, 290 and 299, walking and jumping.
  - Pass: x never over 395.7. Shift walls, when on, hold from the booth, drive, lot and spur mouth.
- **Recipe:** main3_8_6_fence_check.cs, main3_8_14_climb_check.cs (IW2, IW3), main3_8_9a_fix_check.cs (b4a, b4b: blocker-relative). main3_8_9e_rewalk_check.cs part c aims at (99.5, 209.9), rev 16: retire it.

### 7. Stairs, tower and hatch
- **Input:**
  - The route up the ten landing centres from the south bay and back down.
  - Sprint-jumps from three points on every flight in six directions, and from every landing outward.
  - A walk round the whole deck walkway.
- **Pass:**
  - Up in 20 to 25 s at 2.5 m/s.
  - 0 head bumps walking.
  - 0 falls (y below start minus 1.2 m).
  - The walkway walks all round.
- **Recipe:** main3_8_9c_check.cs and main3_8_9c_rewalk2_play_check.cs.

### 8. Ward climb, cleft and ledge
- **Input:**
  - J to the ledge path end and back in walk, jump, crouch and sprint.
  - Ledge edges: push off the west edge (toward x -15) at every 1.5 m from z 217 to 283, and off the north and south end walls (toward z 290 and z 210) every 1.5 m from x -9.5 to 5.5, in walk, sprint-jump and crouch; round all four corners.
  - The cleft and the fin: sprint-jump in 12 directions from every 1 m from the slot entry (24.5, 262) to the exit (4, 257.3); the fin and slot walls must hold (no standing on the fin top, no exit over a wall).
  - Ring rims: sprint-jump outward every 1.5 m along P4's east edge, leg 2's outer edge and the cwm rims.
  - Every platform (P1 to P4): push off the downhill edge in all three modes.
  - Stone area: walk to 6 points and back.
- **Pass:**
  - Walked length within 5 percent of 265 m (Valley.md rev 10 4.1).
  - No player gets past x -10.5, north of z 285.5, south of z 214.5, or below 61 on the ledge; nobody stands on the fin, a cleft wall or a ring rim.
  - Every stone point can be reached and left.
- **Recipe:** main3_8_14_climb_check.cs (timed climb with the real mover; LEDGE: lip, both end walls, corners). The cleft and fin sweep, the ring rim sweep and the stone area are not committed. main3_8_9c_check.cs (climb part) and main3_8_9j_climb_check.cs aim at rev 7 points: retire them. The leg side pushes are check 4.

### 9. Earlier repros, run every time
Each must stay fixed.
- **Wall_100:** from (86.6, 51.3) toward (72.2, 30.8), jumping on every landing, at 2.5, 4 and 5.5 m/s. Fail if it ends in the east ravine, on the ground over the cave, or within 2.5 m of (82.2, 47).
- **Boathouse bank pocket:** west from (247.2, 54.9). Fail below -4.8.
- **Camp 2:** both legs reach the ramp foot (298.9, 107.8).
- **Cave passage, board off:** in to (52, 22.2) and out, at x 51, 52 and 53, in all modes.
- **Closed loop:** spur to (387.5, 252) and on to (372, 244.6), in all modes.
- **Cabin:** from the bunk spawn, out the door and back (main3_8_9d_check.cs; run twice, the door swings over frames).
- **Recipe:** main3_8_9e_play_check.cs, main3_8_9e_rewalk_check.cs, main3_8_9a_fix_check.cs. Expected FAIL until 8.15 builds the stops (PLAN Rules and Tips, CHECKS (8.14)): 8.9a fix check parts 2b, 3a, 3d, 7a to 7d, 9a, b3, b4a, b5, b6 and 8.9e Wall_100 relied on the thicket and the lake edge. Rerun after 8.15; a FAIL then is real.

### 10. Sightlines and hidden margins
- **Input:** main3_8_9_sightlines.cs (edit mode, trees on for places, trees off for W-1, C-1 and F-1).
- **Pass:**
  - Every place is seen from the deck.
  - W-1 (every stone corner, behind the knob) and C-1: every ray blocked at eye and jump height, and again with eyes and targets raised 3 m. Least margin 3 m or more.
  - F-1: rays from every place, every trail point at 10 m and the deck grid (eye, jump, +3 m) to every flame top hit terrain. Visible tops (two thirds up each card) must be 0 seen outside the reveal zone. Card tops seen are reported with the count; the card-top exemption holds only while Marlow's human-eyes item on the flame cards passes.
  - F-1 from the cleft every 0.5 m, slot entry (24.5, 262) through the dogleg to the exit (4, 257.3), eye and jump: record where visible tops first show; none before the exit (Valley.md 4.6).
  - Cab seen from every junction.
  - Next destination seen from every junction except the cave spur at W1.
- **Also check:**
  - Stones from the deck: 1 m grid, eye 57.6, jump 58.2, +3 m on the walkway, 18 points per stone. Pass: 0 rays reach a stone.
  - Stones along the climb, every 0.5 m of trail. Pass: 0 seen before the reveal zone. main3_8_9c_rewalk2_edit_check.cs part 2 walks rev 7 points: retarget it to 8.3's ClimbPts.
  - When the smoke sheet is built: the same F-1 rays to its top surface and edges (Valley.md 3.2, table 4 sheet rows).

### 11. Straight view along each trail
- **Input:** from every point of each leg, both directions. The view reaches the farthest later point for which every trail point up to it is visible (eye 1.6, target 0.3 m over the ground, default layers, crowns on).
- **Pass:** 60 m or less. The 2026-09-29 exemptions (Sable: the four benched legs; Wren: J to leg 1 and leg 5) were for rev 7's benched face and do not carry over. Rev 10 leg 2 is a 61 m straight (Rook's deviation): it fails unless Wren rules an exemption. Report every leg of J to Ward separately; the cleft stays at 60 m or less.
- **Recipe:** main3_straight_view_climb_8_9k.cs for J to Ward only; it aims at rev 7 points and exempts rev 7 legs by name: retarget it to 8.3's ClimbPts and drop the old exemptions. None for the other trails.

### 12. Day-one state
- **Input:** the saved scene, edit mode.
- **Pass:**
  - StandInFire is active (the fire is always on; the land hides it, DECISIONS 2026-09-29).
  - CairnGate and DayOneBoard are active.
  - Shift walls are off.
  - Scene not dirty after the checks.
- **Recipe:** main3_day_one_state_check.cs.

### 13. E-1, every edge ends on land
- **Input:** main3_e1_edges.cs: rays every 2 degrees of bearing at -5 to 5 degrees elevation, to 3500 m, from the deck grid, every place, every trail point, P1 to P4 and the ledge path end. Plus Marlow's grazing rays (ValleyNumbers R4.3).
- **Pass:** 0 void, 0 flat plane from every origin set; every grazing ray lands on terrain or a landscape mesh.
- **Recipe:** main3_e1_edges.cs.

### 14. Visible stops
- **Input:** every end point of checks 4, 6 and 8 where a collider stopped the player off open trail.
- **Pass:** each stop has a RidgeStops rock, scree or a visible marker within 1.5 m in front of it, and the camera never ends inside a rock (Edges.md 9.4). On the ledge the lip is the stop: rocks only at the north and south ends, none in the middle 60 degrees of the west view.
- **Recipe:** none committed.

## Kept for Marlow (human eyes)

- The look of each frame against Style.md:
  - the fire frame from the ledge path end (the valley below shows, not a rock band; smoke lit, half the sky);
  - the flame cards: no flame pixel in the upper third of any card in any flipbook frame (the F-1 card-top exemption rests on it);
  - silhouettes and crest profiles against Valley.md 2;
  - fog legibility of trails at night;
  - whether the look-back from P4 reads as "the tower under me" (tower cab, cabin window, lake, lot and highway in one frame, no ring wall across it).
- Whether a place reads as ordinary on day one, and whether a reveal lands: stepping round the fin into the fire, the stones against the glow.
- Whether the climb stays "small and never oppressive" (DECISIONS 2026-09-30) inside the ring walls, and whether the four legs look different at eye height.
- First-time player: can the way in be found without circling (stair entry, the ramp at Camp 2, J to the chute through the band gap, the next destination at each junction as seen, not as ray counts)?
- Anything the numbers pass but that looks wrong: markers in a frame, clipping into collider-free stop rocks, floating slabs, props floating or buried.
- New repros that no script covers yet, before they become a check here.
