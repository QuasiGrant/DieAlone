---
name: walk-method-pitfalls
description: Pitfalls found running Main3 machine walks (8.10) that produce false stuck spots or false sightline results
metadata:
  type: feedback
---

False results I hit in the 8.10 Main3 walk. Check these before reporting a stuck spot or a hidden marker.

- Door.Use swings the door away from the player's position, so opening from far away (cabin) can swing the cab door onto the walkway and block a scripted route. Open doors with the player standing where a real player would.
- Straight-line connector waypoints hit tiny colliders (pump body 0.35 m, gangway side, fire pit, stair posts). Those are route artifacts, not findings; retest with a human-like path.
- Fixed-step "push" tests slide along walls for up to 180 m; judge only the max reach, not the end point.
- Linecasts from an eye inside a collider, or to target points inside a trunk/Tor/stack, read as hidden. Count a hit on the target object itself as seen, and move eye points off structures.
- The play-mode walk ignores crown colliders; add temporary MeshColliders to Crown objects for sightline checks (play mode only).

**Why:** each of these made a first-pass report wrong until retested.
**How to apply:** any Unity walk or sightline check from eval. Related: [[main3-blockout-state]]
