---
name: runner-overwrites-standalone
description: Standalone recipes that edit shared assets (LookTuning, prefabs) get silently reverted by the Main3 runner; check every new recipe is in main3_rebuild.sh or its values live in the owning recipe
metadata:
  type: feedback
---
When a task adds a standalone recipe that writes a shared asset, check whether a runner recipe writes the same asset later. Found 2026-09-29: look_day_one_8_9g.cs sets LookTuning_DayOne, but main3_8_9d_dress_camp.cs (SetLook for DayOne) and main3_8_9f_look.cs (copies day-one values into the night look) run in the runner and revert it.

**Why:** the project's rule is "every generated thing regenerates from a committed recipe"; a value outside the runner is lost on the next rebuild and nobody notices until Grant sees the old look.
**How to apply:** at every commit review, grep the runner's recipes for the asset path the new recipe writes. Also check new untracked folders under Assets (capture_game_view save_path puts PNGs in Assets/Docs). See [[valley-rebuild]].
