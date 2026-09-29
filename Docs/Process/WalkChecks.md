# Walk checks (Main3)

Marlow, 2026-09-29. DECISIONS 2026-09-29: Rook runs these before every scene handback. Every check is scripted and reports PASS or FAIL. Marlow keeps only the checks at the end that need human eyes.

## Common rules for every play-mode check

- **Editor state:** run `unity status` first, then `editor_status`. Run each check as a detached job: `unity command --detach eval_file`, then `unity job wait`.
- **Scene:** never save the scene. Afterwards, runInBackground must be false and `git diff Assets/Settings ProjectSettings` must be empty.
- **Mover:** use the real CharacterController and its own collision.
  - dt 0.02, gravity 20, jump height 0.6 m, so v0 = sqrt(2 x 20 x 0.6).
  - Modes: walk 2.5 m/s; jump on every landing; sprint-jump 5.5 m/s; crouch 1.5 m/s with a capsule of height 1.0, centre 0.5.
  - A move stalls when the distance to target has not shrunk by 0.05 m in 600 steps.
- **Drop:** air-top minus landing height, recorded on every landing.
- **Sight rays:** use Physics.DefaultRaycastLayers, so the Thicket walls on layer 2 are ignored. Give every Crown a temporary MeshCollider (play mode only) and destroy it afterwards. A ray that hits the target object itself counts as seen. Keep eye points off structures.
- **False results to rule out before reporting** (from .claude/agent-memory/playtester/feedback_walk_method.md):
  - straight connectors that hit small colliders
  - doors opened from a spot a player would not stand in
  - teleports into the cave
  - a probe header that disables objects
  - teleport starts no player can reach: check that the start is reachable from a trail before calling something a bypass
- **Screenshots:** never use capture_game_view with a save path; it writes under Assets. Render Camera.main to a RenderTexture inside the eval and write the PNG outside the project.

## Existing recipes (Tools/Recipes)

| Recipe | Mode | Covers |
|---|---|---|
| main3_walk_trails.cs | play | every leg under Trails, forward and back (hop mover only; see check 1) |
| main3_walk_legs.cs | play | a chain of legs and waypoints, reachability |
| main3_walk.cs | play | named waypoint routes |
| main3_8_6_fence_check.cs | play | gate and fence pushes |
| main3_8_9_sightlines.cs | edit | places from the deck, W-1, C-1, cab from junctions, next destination; writes Docs/Layout/Main3/Main3_sightlines.md |
| main3_8_9a_fix_check.cs | play | the 8.9a and 8.9b fix spots: walk, hop, crouch |
| main3_8_9c_check.cs | play | stair climb time, Ward climb time, lip stop |
| main3_8_9c_rewalk2_edit_check.cs | edit | camp trail ends against the knoll, camp leg grades, stones seen along the climb |
| main3_8_9c_rewalk2_play_check.cs | play | camp re-entry, camp to pump timed, stair sprint-jumps |
| main3_8_9d_check.cs | play | cabin exit from the bunk spawn, practical lights |
| main3_8_9e_edit_check.cs | edit | leg lengths, steepest 10 m, pass and lip heights, groves |
| main3_8_9e_play_check.cs | play | Wall_100, Camp 2 ends, rev 15 leftovers |
| main3_8_9e_rewalk_check.cs | play | Wall_100 repro, rim pocket, cairn gate, switchbacks |

Marlow's own probes are not committed; they are in the scratchpad.

## Checks

### 1. Every trail walks, both ways
- **Input:** every leg under Trails, centre points in order and reversed. Three modes: walk, jump on every landing, crouch. For the walk, turn off the day-one gates (CairnGate, Cave/Mouth/DayOneBoard); they get their own check (6).
- **Pass:**
  - 0 stalls.
  - Largest drop 1.5 m or less, not counting jump arcs.
  - Walking time within 3 s, or 5 percent, of Main3.md table 4.
- **Recipe:** main3_walk_trails.cs. Replace its hop mover with the real mover above.

### 2. Every place is reachable both ways
- **Input:**
  - Every place's walkable centre: camp clearing, cabin interior, tower deck, pump and dock end, boathouse interior, Camp 1, Camp 2 ramp foot and stack top, Camp 3 floor, office, store, booth, car spot, closed loop, cave chamber (board off), Ward lip and stone area.
  - Walk from the nearest leg end into the place and back out.
  - The camp gets its own test: a 24-point fan. From each camp leg start, walk to 24 points at r 10 round (170, 160). Then walk from r 12 down to each leg start.
- **Pass:**
  - Every place is reached and left, walking.
  - Fan: every failure is a straight line through the tower or cabin footprint. Anything else is a FAIL.
- **Recipe:** main3_walk_legs.cs and main3_8_9c_rewalk2_play_check.cs (camp re-entry).

### 3. Slope caps
- **Input (edit mode):** sample terrain every 0.5 m along each leg's centre points. For each window 10 m along the path, take the rise over the run.
- **Pass:**
  - Every window 25 percent or less, except the log-step segments on Camp to Camp 3 and W1 to Camp 3.
  - Inside the camp clearing (r 15) the ground is 15 +/- 0.3.
  - Every trail end sits within 0.5 m of the ground height of the place it joins.
- **Recipe:** main3_8_9e_edit_check.cs and main3_8_9c_rewalk2_edit_check.cs.

### 4. Side pushes (thicket walls, shortcuts, drops)
- **Input:**
  - Every leg point except the ends, both sides, perpendicular to the trail, 25 m.
  - Three modes: walk, sprint-jump, crouch.
  - For the Ward climb, also sprint-jump 12 m in 12 directions from every point up to the pass.
- **Pass:** every push ends in one of these ways:
  - within 4.5 m of a trail point (any leg);
  - inside a named clearing or a point-of-interest pocket from which the trail can be walked back to;
  - on the Ward climb, on the same shelf: the nearest trail point along the climb less than 15 m away, and no gain over 2.5 m.
- **Fail when any of these happen:**
  - a push ends on a wall top, in the thicket, or off every trail;
  - a push reaches another shelf of the climb, uphill or downhill;
  - a push drops more than 2.5 m.
- **Recipe:** none committed that covers every leg. main3_8_9e_rewalk_check.cs covers the Ward climb.

### 5. Lake and water
- **Input:**
  - Every point of Pump to boathouse, Pump to W1, Boathouse to Camp 2 and Camp to pump that lies within 1.6 lake radii of the lake centre (190, 60), where the lake radii are 54.8 m and 27.6 m.
  - Headings toward (190, 60) and +/- 35 degrees, 30 m, walk and sprint-jump.
  - Also: the dock end and dock sides, the gangway, and west from (247.2, 54.9).
- **Pass:** 0 ends inside the lake ellipse below -5.2. If a player is placed inside, jumping must get them out.
- **Recipe:** main3_8_9a_fix_check.cs covers the dock and wade limits only. The full sweep is not committed.

### 6. Gates and fence hold
- **Cairn gate (by day):**
  - Straight on: from (104, 206.5) to x 97.5 to 105 at z 204.5, then 3 m past, in walk, sprint-jump and crouch.
  - Round the gate: from 8 points at r 3 round J and the last 12 Camp to J points, toward climb points 6 to 60, in all three modes.
  - Pass: 0 ends within 3 m of a climb point past the gate.
- **Cave board (day one):** from (50.7 / 52 / 53.3, 40) south, in all three modes. Pass: 0 ends south of z 37.3.
- **Gate and fence:**
  - Push east at z 5, 160, 170, 262, 290 and 299, walking and jumping.
  - Pass: x never over 395.7. Shift walls, when on, hold from the booth, drive, lot and spur mouth.
- **Recipe:** main3_8_6_fence_check.cs, main3_8_9a_fix_check.cs, main3_8_9e_rewalk_check.cs.

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

### 8. Ward climb and lip
- **Input:**
  - J to the lip and back in walk, jump, crouch and sprint.
  - At the lip, push west toward x 8 at z 250 to 262, every 1.5 m, in three modes, and round both lip ends.
  - Stone area: walk to 6 points and back.
- **Pass:**
  - Walked length within 5 percent of table 4.
  - No player gets past x 13.0 or below the ledge.
  - Every stone point can be reached and left.
- **Recipe:** main3_8_9c_check.cs (timing, lip stop). The side pushes are check 4.

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
- **Input:** main3_8_9_sightlines.cs (edit mode, trees on for places, trees off for W-1 and C-1).
- **Pass:**
  - Every place is seen from the deck.
  - W-1 and C-1: every ray blocked at eye and jump height, and again with eyes and targets raised 3 m. Least margin 3 m or more.
  - Cab seen from every junction.
  - Next destination seen from every junction except the cave spur at W1.
- **Also check:**
  - Stones from the deck: 1 m grid, eye 57.6, jump 58.2, +3 m on the walkway, 18 points per stone. Pass: 0 rays reach a stone.
  - Stones along the climb, every 2 m of trail. Pass: first seen over the rim, never below the pass.

### 11. Straight view along each trail
- **Input:** from every point of each leg, both directions. The view reaches the farthest later point for which every trail point up to it is visible (eye 1.6, target 0.3 m over the ground, default layers, crowns on).
- **Pass:** 60 m or less, except the switchback legs on the Wall face (Wren's call). The J to Ward plateau run (89 m) is open for Wren's confirmation.
- **Recipe:** none committed.

### 12. Day-one state
- **Input:** the saved scene, edit mode.
- **Pass:**
  - StandInFire is inactive.
  - CairnGate and DayOneBoard are active.
  - Shift walls are off.
  - Scene not dirty after the checks.

## Kept for Marlow (human eyes)

- The look of each frame against Style.md:
  - the fire frame from the lip (sea not dots, smoke lit, half the sky);
  - silhouettes;
  - fog legibility of trails at night;
  - whether the pass look-back reads as "the tower under me".
- Whether a place reads as ordinary on day one, and whether a reveal lands: the crest hiding the drop, the stones against the glow.
- First-time player: can the way in be found without circling (stair entry, the ramp at Camp 2, the next destination at each junction as seen, not as ray counts)?
- Anything the numbers pass but that looks wrong: markers in a frame, clipping, props floating or buried.
- New repros that no script covers yet, before they become a check here.
