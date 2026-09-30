---
name: player-build-perf
description: How the 8.16 frame-rate floor is measured in a development player, and what a build costs and dirties
metadata:
  type: project
---
The frame-rate floor (1% low 60 fps) is judged on a development player build (Wren, 2026-09-30), not Editor Play mode.
Build with BuildPipeline to Build/DieAlone.exe (Development), then run
`DieAlone.exe -screen-width 3840 -screen-height 1976 -screen-fullscreen 0 -perfspots <file> [-perfframes 3000]`
(Assets/Scripts/Dev/PerfSpots.cs, dev builds only; two passes over Camp, S1, Office).

**Why:** Editor spikes made the 1% low swing by 10 fps between runs.
**How to apply:** the first build after turning on the GPU Resident Drawer (BatchRendererGroup variants Keep All) took about 60 min of shader
compiling; later builds about 30 s. A build dirties URP global settings (m_List), PC_RPAsset prefilter ints, preloadedAssets, the ProBuilder
Settings.json and adds Assets/DefaultVolumeProfile.asset; see [[build-rewrites-need-grant]] for what can be restored from the Editor.
The perf player pauses when its window loses focus (runInBackground off), so an unwatched run stalls and never quits; a stalled one
(2026-09-30, PID 69588) could not be stopped by an agent (auto-mode denial): only Grant can close it. Keep the window in front until "done".
