---
name: forest-calls-816
description: 2026-09-30 LightingOptions 7 calls after 8.16 forest: trail material not ambient, sky blend 4.5, lodBias 1.25
metadata:
  type: project
---

Calls in LightingOptions.md section 7 (sheets 0bb60f8), proposed, not in DECISIONS.md yet:
- Shade trails failing W1 at 20 m: fix the trail material (paler, wider dirt at distance), keep shade ambient as the bar 2 dark anchor, accept no legs. Trail drops 60 to 23 grey from 5 m to 20 m, so the loss is in the trail's far render; Rook finds why first.
- Sky: skyBlendRate field replacing hardcoded 1.8 in SkyGradient.shader, P 4.5, band 6. Supersedes my section 6 band-to-30.
- lodBias PC quality 2 to 1.25, floor 1.0. GPU Resident Drawer fine if frames match.

**Why:** 1% low 55 to 66 vs 60 floor; flat ochre sky at gaze; Pim's 20-grey rule.
**How to apply:** Re-judge from the next capture against these; related [[lighting-options]], [[forest-plan]].
