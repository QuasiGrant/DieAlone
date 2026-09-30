---
name: lighting-options
description: 2026-09-30 LightingOptions.md for PLAN 8.12: V1-V10 single changes, candidates A/B/C, six sheet spots, absolute bar, missing LookTuning fields
metadata:
  type: project
---
Docs/Design/LightingOptions.md specifies looks Rook builds after 8.11 as LookTuning_DayOne copies on LookPreview. Candidates: A Vesper (Check3 set), B Late gold (16/212), C Overcast. Grant judges from Sheet 2 (D1 now, A, B, C at six spots).
Missing fields found: no Trilight ambient (flat sunsetAmbient), sunsetFogEnd Range cap 600, no glow strength, no exposure. skyHorizon already #E3A968 but no gold band renders.

2026-09-30 verdict (section 6): no look passes (no gold band anywhere). Pick A Tri; alternatives A, and B (re-judge on rev 8). C rejected. Fix: sunElevation 20 if the rev 8 ray check keeps camp lit. New field skyBandHeight P 30 deg needed. Cairn at J renders see-through (material fix, Rook).
**Why:** Grant cannot judge lighting from numbers; he must see them ([[check3-quality]]).
**How to apply:** when Rook delivers sheets, judge against section 4 bar, pick best, then Grant chooses; record in DECISIONS only after Grant confirms.
