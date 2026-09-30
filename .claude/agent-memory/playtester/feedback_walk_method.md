---
name: walk-method-pitfalls
description: Pitfalls in Main3 machine walks (8.10 to 8.9c): false stuck spots, false sightlines, screenshot tool writing into Assets, checks that miss return trips
metadata:
  type: feedback
---

False results I hit in the 8.10 Main3 walk. Check these before reporting a stuck spot or a hidden marker.

- Door.Use swings the door away from the player's position, so opening from far away (cabin) can swing the cab door onto the walkway and block a scripted route. Open doors with the player standing where a real player would.
- Straight-line connector waypoints hit tiny colliders (pump body 0.35 m, gangway side, fire pit, stair posts). Those are route artifacts, not findings; retest with a human-like path.
- Fixed-step "push" tests slide along walls for up to 180 m; judge only the max reach, not the end point.
- Linecasts from an eye inside a collider, or to target points inside a trunk/Tor/stack, read as hidden. Count a hit on the target object itself as seen, and move eye points off structures.
- The play-mode walk ignores crown colliders; add temporary MeshColliders to Crown objects for sightline checks (play mode only).
- The old "hop" (0.08 up for 8 steps) is not the real jump. Use gravity 20, v0 sqrt(2*20*0.6), jump on every landing, dt 0.02; with it the 8.9a hedges and wade boxes were climbable where the old hop and the fix check said they held. Also test crouch (1.0 m capsule) under chains and bars.
- When reusing the first lines of a probe as a header, check they carry no side effects (one disabled the cave board for five probes and faked a board leak).
- Joined-tour STUCK teleports use terrain height; never teleport into the cave (lands on the rim). Use explicit warps.
- capture_game_view save_path must be inside the project and lands under Assets/ (it created Assets/Temp with .meta files). Never use it; render Camera.main to a RenderTexture in an eval and write the PNG to the scratchpad. If it happens, delete through AssetDatabase.DeleteAsset.
- Rook's check recipes walk only the route he changed (8.9c: stair and Ward). Always walk every trail back INTO each place, not just out: the 8.9c knoll left all four camp trails in trenches a walker could not climb out of, and his check never saw it.
- Test sprint-jumps sideways on stairs and landings; 1 m rails on sloped flights are clearable.
- Before accepting Rook's "all pass", read the recipe's mover and aim points: in 8.9k main3_walk_trails.cs still used a no-gravity push (walk only), and a gate check still aimed at a rev 16 point. "Every check with a recipe passes" also passes checks that have no recipe.

**Why:** each of these made a first-pass report wrong until retested.
**How to apply:** any Unity walk or sightline check from eval. Related: [[main3-blockout-state]]
