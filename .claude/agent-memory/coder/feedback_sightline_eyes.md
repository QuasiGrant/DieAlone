---
name: sightline-eyes
description: Sightline searches and frame eyes must reject eyes inside drawn meshes and test drawn meshes, not gameplay colliders
metadata:
  type: feedback
---

Sightline searches must reject an eye that stands inside a drawn mesh. Test lines against temporary exact colliders on the drawn meshes, not against gameplay colliders.

**Why:** On 2026-10-03 (8.26 round 2) a viewpoint "saw" the Snag line from inside the drawn Snag. A ray that starts inside a mesh misses its back faces. The drawn tree is SliceLook/GiantTrees/Tree_Dead. The Giants/Heroes/Snag/Trunk renderer is disabled, and a trunk's capsule is thinner than its bark. The frame showed only bark.

**How to apply:** In any viewpoint or frame-eye search:
- reject eyes where a level ray with Physics.queriesHitBackfaces on meets a back face first;
- count only temp mesh colliders and the terrain as blockers;
- look at the frame image before reporting.
