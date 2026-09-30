---
name: paper-check-method
description: How to run paper sightline checks on design docs (Valley.md etc.): no Python here, margin metric, views agents miss
metadata:
  type: feedback
---

- No Python on this machine (python3 is the Store stub). Compute with C# via PowerShell: `powershell -NoProfile -Command "Add-Type -TypeDefinition (Get-Content -Raw 'x.cs') -Language CSharp; [Cls]::Run()"` from the scratchpad.
- "Hidden by N m" under the +3 rule = ground minus line at the deepest point of the blocker (line lowest), not at the far edge. R2 of the valley review used the far edge and was stricter than the rule; I had to correct it in R3.
- Designers check lines straight at the fire. Also check lines that run along a ridge face to a low saddle (P4 at eye level with the W saddle saw the smoke corner), the sheet or mesh edges nearest the crest, and saddles on other ridges (S saddle from the gate).
- For floors/backdrops, check both axes of every extent and the corner quadrants between them; grazing rays over low corners land there. In main3_8_9e_layers.cs Band(), frontD (700/900/800) is the range front at ground level; crestD (1200/1500/1500) is where the crest starts. I misquoted the crest start as the front in R3; Sable copied it.
- Check a sheet's far edges too (north edge from the climb ran along the face over the NW corner), and taller targets from the cleft part B.
- For edge/void checks, grazing rays over low crests from the tower descend 2 degrees; compare where they reach ground level with the far range's front distance in Tools/Recipes/main3_8_9e_layers.cs.

**Why:** each of these changed a result in the 2026-09-29 valley review.
**How to apply:** any paper review of sightlines or edges. Related: [[walk-method-pitfalls]]
