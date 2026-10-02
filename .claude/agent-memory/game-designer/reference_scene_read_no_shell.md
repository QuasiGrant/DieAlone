---
name: reference-scene-read-no-shell
description: How Sable places items against real Main3 objects without a shell: multiline Grep of Main3.unity prefab overrides, guid to pack prefab via .meta files
metadata:
  type: reference
---
Without a shell, real object positions come from Assets/Scenes/Main3.unity with Grep (read only, never edit):
- Prefab instances: multiline grep `propertyPath: m_LocalPosition\.x\n\s+value: (range)...m_LocalPosition\.z\n\s+value: (range)`; parents are at the origin for Forest, Trails and Places groups, so local = world (nested and unpacked objects are missed; say so).
- Trail points: transforms whose m_Father is the trail's Transform fileID (e.g. "Camp 1 to J").
- guid to prefab: pack folders are gitignored, so Grep skips them unless `path` points inside the pack folder (BK/PureNature_Redwood, suffercord, Revolving Pizza Games).
Limits found in Marlow's 824 check: this read misses Forest/Dense trunks (unpacked), and PlaceKit boxes are often far larger than meshes (ruin walls 2.3 thick, stump 3.98, tent 7.32, table 1.6). Ask for collider sizes, never assume mesh size; name every tree removed and give 8.19 keep-out zones.
Main3.unity line numbers shift while Rook saves: Grep with -A context, never Read by an older line number. Place recipes (Tools/Recipes/main3_8_5_campsites.cs, main3_8_17_*.cs) give built geometry faster than the scene.
Used for NorthLayout.md draft 1 and Camp2Layout.md draft 1 (2026-10-02). Marlow still samples ground and colliders; see [[feedback-measure-ground-first]].
