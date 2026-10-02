# Team meeting, 2026-10-02: Milestone 8

Compiled by Wren from Sable, Vesper, Tully, Marlow, Pim, Hollis and Quill (notes in this folder). Rook was fixing the failed rebuild.

## Where we are

- Built: the valley, the Ward path redesign (8.20), the dense forest (8.19, 2,846 of about 3,040 canopy trees), every place, the break fixes, solid rocks.
- Not proven: nobody outside Rook has checked 8.19 or 8.20. Grant's rebuild this morning stopped at the smoke step (the fire object is missing), so the scene is half rebuilt and uncommitted. Rook is on it.
- PLAN 8.14 to 8.20 are all open. 8 gate rounds, none passed in full.

## What the team agrees on

1. The loop is too long: build, three reviews, a fix list, again. Each round finds new items, so Milestone 8 never closes (Sable, Tully).
2. Our checks have repeatedly lied to us: the noise band hid built objects, the fire was checked only by day, traps were retested only on the old spot, warps were never dropped into. Each time, reviews failed real work or passed broken work (Vesper, Marlow, Tully).
3. Rebuilds wait on Grant, because the safety check blocks agents from the delete step. Fixes sat unproven for 15 hours (Tully).

## Changes, Wren's calls (in effect now)

1. Every number in the gate becomes a hard PASS or FAIL inside the capture script, including Marlow's flood and trap test and Vesper's "count every place and effect, stop on any zero". Rook does not hand back until it is clean.
2. Reviewers then judge by eye only. A re-check covers only the failed items and the area round them.
3. At most two gate rounds per stage. After that Wren rules, or the item moves to a later milestone.
4. One build, one gate, one fix list. No fixes land while a gate is running. Gates run on a committed scene.
5. Any design that changes what the player hears goes to Hollis on paper first. The proof frames include story close-ups for Quill.
6. Rook makes the rebuild move the old scene into a backup folder instead of deleting it, so agents can rebuild without Grant.

## Needs Grant

1. **A frozen exit list for Milestone 8.** Sable and Tully both say the milestone has no fixed finish line. Proposed list:
   - The layout works: no falls, no traps, every warp lands, every place reachable and findable, the Ward path closed by day, the fire and Ward hidden, every check passing.
   - The forest is dense and feels like a forest (Vesper C or better).
   - Every place and the Ward path read clearly (Vesper C or better).
   - Grant walks it and says it is right.
   - The ledge fire look, the night look, the remaining ugly spots and fine dressing move to Milestone 11.
   [Yes]
2. **If Rook's backup-folder rebuild still trips the safety check:** one allow rule for `Tools/Recipes/main3_rebuild.sh` in Claude Code's permission settings, so agents can rebuild. [Yes]
3. **Story drafts, when you next work with Quill:** the Ward runes as the WARD gauge, and four home details.
