# Team meeting input: why the valley failed Grant's walk

2026-09-30, Tully. Read: PLAN 8.9f to 8.9k, DECISIONS 159 to 188, WalkChecks.md, ValleyRebuild.md, ValleyNumbers.md, ValleyRewalk.md, 26 commits since main3-rev16, GrantNotes.md.

## 1. Root causes
1. **We checked geometry and called it testing.** About 15 recipes prove rays hit terrain (W-1, C-1, F-1, E-1), a push mover reaches points, 3444 pushes hold. None asks: is this a path, is this wall fair, is it fun, is it empty. Grant's first note says we overindex on the tower rule; our checks were almost all that rule.
2. **Nobody looked through the player's eyes.** All four reviews were paper. Marlow's final: "Paper only... I cannot walk it by hand this round." Marlow's human-eye list (WalkChecks 189 to 200: way-finding, markers, reads right) was never run. Grant was the first human walker.
3. **Speed over quality.** Tag at 20:09, 8.9k ticked at 21:50. 8.9j built at 21:09, ticked 38 minutes later. 191 files changed. No gate slowed it down because the target was "walkable tonight" (ValleyRebuild.md line 3, my own draft; I carried that target and did not challenge it).
4. **Scope drift.** Grant's asks were menus, brightness, day and night, Ward higher, fire hidden by land. They became a full map rebuild plus 3+ stacked light changes (8.9g sun 14 to 28, then 8.9j sun elevation 32, InteriorFill, cabin +25 percent). No single before/after brightness frame was shown to Grant. Result: "odd and bright".
5. **Ticks without the done-check.** 8.9k: Marlow's final said FAIL on 3 items; 21:47 to 21:50 closed them by recipe and by "Wren's ruling, pending Grant", which is not in DECISIONS. Vesper's check of the retaken edge shots (8f34383) is not on record before the tick. 8.9j was built from rev 5, which Marlow never passed on paper (ValleyRewalk 1.1), breaking DECISIONS line 185 the same day it was made. 8.9i ticked on a script clicking a row; Grant cannot find the switch.
6. **Wren made calls overnight** (J to leg 1 and leg 5 exemptions, slope 23.6, cap rock, accepting the unpassed revision) and ticked on them. Rulings Grant never saw became the basis of "the team checked each one" in Status.md.
7. **Blockout choices leaked into Grant's walk.** Invisible walls with 0.3 m markers (8.9b) and gray stop rocks were a check convenience. Paths the same colour as grass, cut-off legs, empty north, tiny store: all known blockout traits, none flagged to Grant before he walked.

## 2. What "build from Sable's new drawing" authorized
- Authorized (DECISIONS 188): a gray valley far enough for Grant to walk, checked by the team first.
- Did not authorize: stacked lighting changes, closing the east side so the highway is not seen from the lot and office (Valley.md gives it only a road cut; whether the road past the office was removed is unverified), rulings in his name, or ticking on "pending Grant". It did not waive 8.10: "right for now" is Grant's, not ours.
- Read wrong. "The team checks the work first" was read as "machine checks pass". It meant a walk that would catch what Grant caught.

## 3. Process changes (cheap)
1. **Play-feel walk before any Grant walk.** Marlow or Rook plays in the Editor with keyboard and mouse, 10 minutes, and fills a fixed list: can I tell path from wall, does each trail end make sense, is anything empty or tiny, is the light plausible, can I find every dev control Grant asked for. Commit it as Docs/Process/PlayFeel.md. Hard gate on any "Wren ticks after Grant".
2. **Captures, not stills.** Record a 60 s route video per changed area (Unity Recorder is in Unity 6; unverified in this project, ask before adding the package). Grant sees it before he walks.
3. **Overnight cap.** One task per night unattended; no task that removes ground Grant has walked. Any ruling not in DECISIONS blocks the tick. The commit hook rejects a PLAN tick whose message says "pending Grant".
4. **Drawing and one before/after frame to Grant before a build.** Map changes: Grant says yes to the drawing. Light changes: one frame, old vs new, same spot.
5. **One look change at a time.** Brightness numbers change in one owning recipe per task, with one Grant confirm.

## 4. Checks: keep or cut
- Keep: the runner (rebuild from recipes); W-1, C-1 and F-1 (rules Grant confirmed, run once per build); cave check; cabin exit; gate and fence; tower climb time; day-one state; the regression recipes for Grant's walked areas.
- Cut or park: the 56832-ray F-1 detail and the 3444-push climb sweep as a tick gate (cost hours, found nothing Grant cares about); check 11 straight-view (produced a ruling, not a fix); E-1 grazing detail until dressing. Replace the push mover with a real gravity and jump walker (already queued) before adding any more ray checks.

Tully
