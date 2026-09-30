# Frame rate after 8.16 (Rook, 2026-09-30)

Development player, 3840 x 1976, vSync off, GPU Resident Drawer on, PC lodBias 1.25. Tool: Assets/Scripts/Dev/PerfSpots.cs (dev builds only),
run as `Build/DieAlone.exe -screen-width 3840 -screen-height 1976 -screen-fullscreen 0 -perfspots <file> -perfframes 3000 [-perfdetail]`.
Two passes over Camp, S1 and the office; 3000 frames per spot after 60 warm-up frames. Floor: 1% low 60 fps.

## Results (last profiled run)

| Spot | Pass 1 avg / 1% low | Pass 2 avg / 1% low | Slow-frame mean / median frame |
|---|---|---|---|
| Camp | 115.0 / 45.3 | 123.8 / 66.4 | pass 1 22.1 ms / 8.4 ms; pass 2 15.1 / 7.8 |
| S1 | 110.1 / 56.7 | 114.3 / 80.8 | pass 1 17.6 / 8.9; pass 2 12.4 / 8.4 |
| Office | 199.5 / 68.2 | 206.2 / 102.4 | pass 1 14.7 / 4.7; pass 2 9.8 / 4.3 |

Earlier runs (600 and 3000 frames) gave Camp 52 to 67 and S1 51 to 75: the 1% low swings by about 15 fps run to run.

## Cause of the Camp and S1 slow frames

- **Mostly first view.** The same view measured a second time (pass 2) clears the floor at every spot: Camp 66, S1 81. The first pass carries
  hitches from the first time each view is drawn: shader and pipeline-state compiles for the BatchRendererGroup variants (Keep All) and
  the first upload of the trees, terrain layers and detail grass. The Office sees little forest, and its pass 1 still sits over the floor.
- **The rest is scattered, not periodic.** The 30 slowest frames of each 3000 fall across the whole run with no fixed spacing, so they are
  not a timed script or a regular garbage collection. The one cluster, at the office in pass 1 (frames 890 to 950), is again first-view work.
- **Not measured: which thread.** The -perfdetail counters (main thread, render thread, GC, present wait) read 0 in the first run: they
  were created but never started. The fixed build (recorders started) ran without writing its file, so per-thread numbers are still missing.

## Cheap fixes, not done

1. Warm the shaders at load: a ShaderVariantCollection of the BRG, terrain and foliage variants, prewarmed on the loading screen.
2. Or show Main3 for a few frames behind the loading screen before play starts, so the first-view hitches happen there.
Either needs the next Rook to confirm with the per-thread counters before building.
