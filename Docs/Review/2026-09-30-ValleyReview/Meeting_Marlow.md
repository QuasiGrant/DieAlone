# Meeting input: Marlow (playtest)

2026-09-30. Read: WalkChecks.md, ValleyNumbers R1 to R4, ValleyRewalk, GrantNotes.md, DevMenu.cs, LookPreview.cs.

## 1. Grant's findings against my checks
1. Paths look like grass: not covered. No check compares trail paint to grass from eye height. Every trail check is a centre-line walk; it passes on unpainted ground.
2. Pointless invisible walls: partly. Check 14 (visible stops) was written, has no recipe, and I did not block 8.9k on it. I flagged the cleft walls 0.8 m off the path and told Grant to watch for them instead of walking them myself.
3. Cut-off paths, lake pump warp: not covered. Check 2 walks from a leg end into a place; a path whose paint stops, or that only a warp reaches, still passes. I have not reproduced the pump case; unverified.
4. Tiny store: not covered. Check 2 only asks "reached and left". Main3.md 7 specifies it 8 x 5.6 m, "small"; I never asked if that reads as a building worth a trip.
5. No highway view from lot and office: not covered. I checked the road cut only for void (R3.6, E-1). I helped close the east with a ridge and never checked what the lot must see.
6. Long, uniform climb: covered as numbers only. I checked 489 m, 196 s, slopes, bench pushes. Four identical 85 m legs was the design; I never judged three minutes of it as a player.
7. Empty north: not covered. E-1 proves no void, not that anything is there.
8. Day/night toggle: not covered. Code has it in F1 under LOOK as Night, Day one, Day two (DevMenu.cs 269). Nothing says "day/night"; I never opened the panel in Main3. Unverified that it shows there.
9. Few trees, bright lighting: not covered. The look list in WalkChecks ("Kept for Marlow") was never run.

## 2. What 8.9j and 8.9k measured
Collision, slope, drop, reachability, ray margins, void. All proxies: a capsule that never looks, rays that never render. I passed paper and Rook's printed numbers; I walked nothing and took no screenshot. I listed "watch for" items to Grant, which handed him my job. The done-check was met; the task was bad. I should have said both.

## 3. Pass must include before Grant walks (human eyes)
1. Eye-height frames every 5 m on every trail, both ways: path distinguishable from ground in each frame.
2. Every invisible stop: a frame from 2 m back. A reason to stop is visible, or the wall goes.
3. Every trail end and every warp point: path continues or ends at a place, in frame.
4. Every place: exterior and interior frame; size and purpose read without labels.
5. Lot and office: the road beyond the gate is in frame.
6. Each climb leg: first, middle, last frame; if frames look alike, that is a finding.
7. Top-down plus four compass views per map quarter: no empty quarter, tree density even with Style.md.
8. Day and night frames of the same 10 spots; lighting against Style.md.
9. F1 panel in Play in the scene under test: every section reachable by keyboard and gamepad, labels a first-timer understands.
10. My own hand walk of every new trail in the Editor, not paper.

## 4. What I need
- Rook: a recipe that renders Camera.main along each trail at eye height every 5 m and at each stop, PNGs outside Assets, plus a contact sheet per trail; a list of every invisible collider with position and the trail point nearest it.
- Rook: a first-person video or frame-sequence walk of the whole climb at walk speed.
- Wren: Editor time reserved for my hand walk; done-checks that name this checklist and cannot pass on recipes alone.

Marlow
