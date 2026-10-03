# 8.26 Camp 3 and the west trails: gate step 4 (Pim, 2026-10-03)
Sources: Docs/Captures/Main3Review_camp3 (run 2026-10-03 03:21): Checks.md, AreaFrames_camp3_01 to 08, Found_camp3_01 to 19; Camp3Layout.md draft 2 (places, stands 6.2); Camp3Layout_UI.md draft 1; main3_8_26_camp3.cs; DevWarpLabels.cs; PlayerInteractor.cs. Bar: found rule (30 m, 45 deg of travel, 1 deg tall, clear by meshes) plus a read by eye; each task 5 presses or fewer. Wren's calls applied: Snag pieces and the lamppost spot out of the found set; job form exempt from the trail-find rule.

**Verdict: FAIL** (2 items: the easel ray meets nothing from its stand; the dam point is out of reach from its stand).

## Input facts (verified)
- Interact E / pad Y; one eye ray, reach 2 m, first hit wins, triggers ignored. No keyboard Look binding (pad right stick or mouse only), so counts are pad and keyboard plus mouse (open from 8.24, not an 8.26 fault).
- No prompt stand-ins at Camp 3: main3_8_26_camp3.cs adds no Interactable, Checks.md has no PROMPT lines (8.25 round 2 had them), and Dressing/Fire has no FirePit. No prompt shows in a build today; every count below is on paper.

## Interactables
| Point | Found (rule) | By eye | Ray from its stand (Checks, SPACING) | Presses, pad / kb+mouse | Result |
|---|---|---|---|---|---|
| Easel canvas, `Study the painting` | 28.8 m from W1 to Camp 3, 31.1 deg off, 3.6 deg tall | Found_05 at 28.8 m: the ball sits in a rock gap, easel not readable. F2 (W1 arrival) and AreaFrames_07 show it clearly | **FAIL.** From (79.0, 149.9) facing 130 (Camp3Layout 6.2, no pitch given): "first hit none within 2.0 m". The stand is 1.7 m from the easel centre and on its bearing, so the canvas has no collider the level ray meets, or the canvas sits below the eye line (cause unverified) | walk, Y / E: 1 press once the ray meets it | FAIL |
| Job form, `Examine` | exempt (Wren) | on the plank table under its mug (Found_14, foreground) | PASS: from (78.6, 148.4) facing 180, pitch -35, first hit itself at 1.2 m | 1 press | PASS on paper |
| Fire, `Sit by the fire` | 23.9 m from W1 to Camp 3, 36.7 deg off, 1.4 deg tall | Found_02 at 23.9 m: not readable under the ball. F2 and AreaFrames_07 show the pot hanger and pit. Lit or cold at Camp 3: unverified | PASS: from (74.6, 151.2) facing 117, first hit itself at 0.2 m | 1 press | PASS on paper |
| Dam point (sink), `Clear the dam` | 6.6 m from a floor walk, 42.9 deg off, 1.5 deg tall (not a trail find; it is a camp point) | Found_14: pool reads; the stone is under the ball, not readable on its own | **FAIL.** From the dam stand (82.6, 147.2) facing 45: "first hit none within 2.0 m". By the numbers: sink box x 83.5 to 83.7, z 148.4 to 148.5, top -4.3 (0.3 under the floor); eye at -2.4; nearest corner 0.9 m east, 1.2 m north, 1.9 m down = 2.41 m, past reach 2.0. Reachable only by standing about 0.5 m from the lip and looking almost straight down (unverified) | 1 press once reachable | FAIL |
| R3 by the fire, `Talk` | | no body built | not in the check (only 4 points: fire, easel, form and table, sink). On paper from (75.2, 148.6) facing 305 the ray passes 0.9 m clear of the fire box's west face (x 74.8): unverified | 1 press | unverified |
| Faced canvases, log steps, W1 blaze, W1 sign | 29.7, 26.6, 29.8, 30.0 m (Checks) | F4, F5 frames | none, off the mask | none | PASS |
| Spacing | easel to form and table 2.1 m centres, 0.76 m edges | | PASS. My draft-1 FAIL (dam point 1.1 m from the easel) is closed by the move: sink to easel box 2.4 m | | PASS |

## Warps
| Warp | Name (DevWarpLabels.cs) | Landing (Checks) | Facing (frame) | Result |
|---|---|---|---|---|
| Camp_3 (75.0, 143.6) | "Camp 3" | PASS, fell 0.16 m, terrain -4.05 | 30, AreaFrames_07: pot hanger and pit, the easel (painted face side-on), the plank table, the paint box, the pool. Reads as the camp | PASS |
| Camp_3_Rim (97.8, 142.0) | "Above Camp 3" | PASS, fell 0.16 m, terrain 4.00, blocked right 2.6 m | 268, AreaFrames_08: the log steps going down, boulders. The floor and its camp are not in frame; the name says "above", the frame shows the way down. Acceptable | PASS |
| Junction_W1 (130.0, 72.0) | "Lake west fork (Camp 3, cave)" | PASS, fell 0.16 m | not moved by 8.26; no facing frame. F5 (W1 heading 300) shows the blaze | PASS (landing); facing unverified |
Gate menu tasks (day/night, warp to Ward): no menu changed by 8.26; not retested.

## To close
1. Easel: give the canvas a collider the eye ray from (79.0, 149.9) facing 130 meets within 2 m (or state the stand's pitch and prove it), then a PROMPT line in the check.
2. Dam point: raise the sink's box to the floor (top at -4.0 or higher) or move the dam stand to within reach (about 0.5 m from the lip), then a PROMPT line from the stated stand.
3. Prompt stand-ins as at 8.25: `Study the painting`, `Examine` (form), `Sit by the fire`, `Clear the dam`, `Talk` (R3), each with a PROMPT line from its stand. Wording still open: `Study the painting` (Grant, M6), `Clear the dam` (Camp3Layout 8.4).
4. R3 added to the spacing check (his ray against the fire box).

Pim
