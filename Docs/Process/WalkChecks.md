# Walk checks (Main3)

Marlow, 2026-09-29. DECISIONS 2026-09-29: Rook runs these before every scene handback. Every check is scripted and reports PASS or FAIL. Marlow keeps only the checks at the end that need human eyes.

Revised 2026-09-29 for the valley (8.9j, 8.9k; Valley.md rev 7): the Wall, pass, plateau and lip are gone. The climb is J to leg 1, four benched legs on the W ridge face with platforms P1 to P4, leg 5, the cleft (parts A and B, the fin), the ramp and the ledge. The fire is always on. New checks 13 (E-1) and 14 (visible stops); F-1 joins check 10.

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
- **Climb names:** J (104, 206); leg 1 x 72 north, P1 (65, 291) 32; leg 2 x 59 south, P2 (52, 204) 52; leg 3 x 46 north, P3 (39, 291) 72; leg 4 x 33 south, P4 (33, 204) 92; leg 5 to the cleft east mouth (20, 230) 95; part A west to (4.25, 230); part B north to the fin's north end (z 237.2); the ramp; the ledge path end (-2, 258) 98. Ledge x -9 to 4, z 232 to 275. The reveal zone: x under 5.5 and z over 237.2.
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
| main3_day_one_state_check.cs | edit | check 12 |
| main3_edge_shots_8_9k.cs | play | edge shots to Docs/Look/Edges (human eyes, not a pass) |

Marlow's own probes are not committed; they are in the scratchpad.

## Checks

### 1. Every trail walks, both ways
- **Input:** every leg under Trails, centre points in order and reversed. Three modes: walk, jump on every landing, crouch. Turn off the day-one gates (CairnGate, Cave/Mouth/DayOneBoard) for the walk; they get their own check (6).
- **Pass:**
  - 0 stalls.
  - Largest drop 1.5 m or less, not counting jump arcs.
  - Walking time within 3 s, or 5 percent, of Main3.md table 4; J to Ward against Valley.md 5.7 (489 m, 196 s as built).
- **Recipe:** main3_walk_trails.cs, walk mode only. Replace its push mover with the real mover above; until then check 1 passes for walk reachability only, and jump, crouch, drop and time are open.

### 2. Every place is reachable both ways
- **Input:**
  - Every place's walkable centre: camp clearing, cabin interior, tower deck, pump and dock end, boathouse interior, Camp 1, Camp 2 ramp foot and stack top, Camp 3 floor, office, store, booth, car spot, closed loop, cave chamber (board off), P1 to P4, the cleft (A and B), the ledge path end and the stone area.
  - Walk from the nearest leg end into the place and back out.
  - The camp gets its own test: a 24-point fan. From each camp leg start, walk to 24 points at r 10 round (170, 160). Then walk from r 12 down to each leg start.
- **Pass:**
  - Every place is reached and left, walking.
  - Fan: every failure is a straight line through the tower or cabin footprint. Anything else is a FAIL.
- **Recipe:** main3_walk_legs.cs and main3_8_9c_rewalk2_play_check.cs (camp re-entry).

### 3. Slope caps
- **Input (edit mode):** sample terrain every 0.5 m along each leg's centre points. For each window 10 m along the path, take the rise over the run.
- **Pass:**
  - Every window 25 percent or less, except the log-step segments on Camp to Camp 3 and W1 to Camp 3. J to Ward has no exception, joins included (J to leg 1, each platform, leg 5, the ramp).
  - Inside the camp clearing (r 15) the ground is 15 +/- 0.3.
  - Every trail end sits within 0.5 m of the ground height of the place it joins.
  - J to Ward ground within 0.5 m of Valley.md 5.2: P1 32, P2 52, P3 72, P4 92, cleft 95, path end 98.
- **Recipe:** main3_8_9e_edit_check.cs and main3_8_9c_rewalk2_edit_check.cs.

### 4. Side pushes (thicket walls, shortcuts, drops)
- **Input:**
  - Every leg point except the ends, both sides, perpendicular to the trail, 25 m.
  - Three modes: walk, sprint-jump, crouch.
  - For the Ward climb, also sprint-jump 12 m in 12 directions from every point from J to the cleft east mouth, and from every point on P1 to P4. The benches are stacked 13 m apart across and 20 m up: a push off a downhill edge that lands on the leg below is a FAIL, and so is one that climbs onto the leg above.
- **Pass:** every push ends in one of these ways:
  - within 4.5 m of a trail point (any leg);
  - inside a named clearing or a point-of-interest pocket from which the trail can be walked back to;
  - on the Ward climb, on the same bench: the nearest trail point along the climb less than 15 m away, and no gain over 2.5 m.
- **Fail when any of these happen:**
  - a push ends on a wall top, in the thicket, or off every trail;
  - a push reaches another bench of the climb, uphill or downhill;
  - a push drops more than 2.5 m.
- **Recipe:** none committed that covers every leg. main3_8_9e_rewalk_check.cs part d covers the benches with 4 m pushes only; that is not this check.

### 5. Lake and water
- **Input:**
  - Every point of Pump to boathouse, Pump to W1, Boathouse to Camp 2 and Camp to pump that lies within 1.6 lake radii of the lake centre (190, 60), where the lake radii are 54.8 m and 27.6 m.
  - Headings toward (190, 60) and +/- 35 degrees, 30 m, walk and sprint-jump.
  - Also: the dock end and dock sides, the gangway, and west from (247.2, 54.9).
- **Pass:** 0 ends inside the lake ellipse below -5.2. If a player is placed inside, jumping must get them out.
- **Recipe:** main3_8_9a_fix_check.cs covers the dock and wade limits only. The full sweep is not committed. The terrain was resampled in 8.9j (1025 heights, new origin), so the lake bed and banks are rebuilt ground Grant walked on rev 16.

### 6. Gates and fence hold
- **Cairn gate (by day):**
  - Straight on: from 3 m on the J side of CairnGate/GateBlocker, across the blocker's full width plus 1 m past each end, to 3 m on the climb side, along the trail's direction there (west, about z 205.5), in walk, sprint-jump and crouch. Aim from the blocker's own transform, never from fixed rev 16 points.
  - Round the gate: from 8 points at r 3 round J and the last 12 Camp to J points, toward climb points 6 to 60, in all three modes. Also from the ridge foot north and south of the gate (x 78 to 82, z 195 to 215) toward leg 1.
  - Pass: 0 ends within 3 m of a climb point past the gate.
- **Cave board (day one):** from (50.7 / 52 / 53.3, 40) south, in all three modes. Pass: 0 ends south of z 37.3.
- **Gate and fence:**
  - Push east at z 5, 160, 170, 262, 290 and 299, walking and jumping.
  - Pass: x never over 395.7. Shift walls, when on, hold from the booth, drive, lot and spur mouth.
- **Recipe:** main3_8_6_fence_check.cs, main3_8_9a_fix_check.cs (b4a, b4b: blocker-relative), main3_8_9e_rewalk_check.cs part c (retarget: its aim point (99.5, 209.9) is rev 16).

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
  - Ledge edges: push off the west edge (toward x -15) at every 1.5 m from z 232 to 275, and off the north edge (toward z 282) every 1.5 m from x -9 to 4, in walk, sprint-jump and crouch; round both corners.
  - Ramp and part B: sprint-jump in 12 directions from every 1 m; the fin and cleft walls must hold (no standing on the fin top, no exit over a wall).
  - Every platform (P1 to P4): push off the downhill edge in all three modes.
  - Stone area: walk to 6 points and back.
- **Pass:**
  - Walked length within 5 percent of Valley.md 5.7 (489 m as built).
  - No player gets past x -9.5, north of z 275.5, or below 97 on the ledge; nobody stands on the fin or a cleft wall.
  - Every stone point can be reached and left.
- **Recipe:** main3_8_9c_check.cs and main3_8_9j_climb_check.cs (timing; one west-edge point and one leg 1 point, push mover). The edge sweep, ramp, part B and platforms are not committed. The bench side pushes are check 4.

### 9. Earlier repros, run every time
Each must stay fixed.
- **Wall_100:** from (86.6, 51.3) toward (72.2, 30.8), jumping on every landing, at 2.5, 4 and 5.5 m/s. Fail if it ends in the east ravine, on the ground over the cave, or within 2.5 m of (82.2, 47).
- **Boathouse bank pocket:** west from (247.2, 54.9). Fail below -4.8.
- **Camp 2:** both legs reach the ramp foot (298.9, 107.8).
- **Cave passage, board off:** in to (52, 22.2) and out, at x 51, 52 and 53, in all modes.
- **Closed loop:** spur to (387.5, 252) and on to (372, 244.6), in all modes.
- **Cabin:** from the bunk spawn, out the door and back (main3_8_9d_check.cs; run twice, the door swings over frames).
- **Recipe:** main3_8_9e_play_check.cs, main3_8_9e_rewalk_check.cs, main3_8_9a_fix_check.cs.

### 10. Sightlines and hidden margins
- **Input:** main3_8_9_sightlines.cs (edit mode, trees on for places, trees off for W-1, C-1 and F-1).
- **Pass:**
  - Every place is seen from the deck.
  - W-1 (every stone corner, behind the knob) and C-1: every ray blocked at eye and jump height, and again with eyes and targets raised 3 m. Least margin 3 m or more.
  - F-1: rays from every place, every trail point at 10 m and the deck grid (eye, jump, +3 m) to every flame top hit terrain. Visible tops (two thirds up each card) must be 0 seen outside the reveal zone. Card tops seen are reported with the count; the card-top exemption holds only while Marlow's human-eyes item on the flame cards passes.
  - F-1 from part B and the ramp every 0.5 m, eye and jump: record where visible tops first show; none before the fin's north end.
  - Cab seen from every junction.
  - Next destination seen from every junction except the cave spur at W1.
- **Also check:**
  - Stones from the deck: 1 m grid, eye 57.6, jump 58.2, +3 m on the walkway, 18 points per stone. Pass: 0 rays reach a stone.
  - Stones along the climb, every 0.5 m of trail. Pass: 0 seen before the reveal zone (main3_8_9c_rewalk2_edit_check.cs part 2).
  - When the smoke sheet is built: the same F-1 rays to its top surface and edges (Valley.md 3.2, table 4 sheet rows).

### 11. Straight view along each trail
- **Input:** from every point of each leg, both directions. The view reaches the farthest later point for which every trail point up to it is visible (eye 1.6, target 0.3 m over the ground, default layers, crowns on).
- **Pass:** 60 m or less, except the switchback legs on the W ridge face (Sable's ruling, 2026-09-29: covers the four benched legs). Report every leg of J to Ward separately; J to leg 1, leg 5, the cleft and the ramp are not switchback legs.
- **Recipe:** none committed.

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
  - whether the look-back from P3 and P4 reads as "the tower under me".
- Whether a place reads as ordinary on day one, and whether a reveal lands: the ramp crest into the fire, the stones against the glow.
- First-time player: can the way in be found without circling (stair entry, the ramp at Camp 2, J to leg 1 through the ridge foot, the next destination at each junction as seen, not as ray counts)?
- Anything the numbers pass but that looks wrong: markers in a frame, clipping into collider-free stop rocks, floating slabs, props floating or buried.
- New repros that no script covers yet, before they become a check here.
