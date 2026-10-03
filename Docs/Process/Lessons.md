# Lessons log

Anyone appends one line when a mistake reaches Grant or a review: `- YYYY-MM-DD who: what went wrong / the fix / where the fix now lives`.
Tully reviews at each commit review. A line whose fix now lives in an agent file, Gate.md or a recipe is deleted. Keep under 20 lines.

- 2026-09-30 all: valley passed 15 number checks and failed at eye height / Gate.md / Gate.md
- 2026-09-30 Wren, Tully: deadline beat quality, nobody challenged it / Tully challenges any date-driven task / process-manager.md
- 2026-10-01 Rook, Wren: fix checks retested the exact old repro, so a moved trap still passed / sweep a ring of approaches around each found problem / pending: coder.md line
- 2026-10-01 Rook: recipe fixes reported done did not show in the rebuilt scene / verify each fix in the scene after a rebuild before reporting / pending: diagnosis
- 2026-10-01 Rook, Vesper, Pim: the VHS noise band sat on the eye line in every capture frame and hid built objects, so reviews failed real work / capture with the band off, noted on each sheet / pending: main3_review_capture.cs
- 2026-10-01 Rook, Wren: the capture checked the ledge fire only in the day-one look, where it is hidden, so a smoke sheet covering it at night went unseen / every look-dependent check runs in each look it matters in / pending: main3_review_capture.cs
- 2026-10-01 Wren, Rook, Marlow: several warps dropped Grant through the map; captures framed warps but nothing dropped a player there in Play / a warp landing check in the capture / pending: main3_review_capture.cs
- 2026-10-01 Marlow, Rook, Wren: two agents shared the Editor and one stopped the other's Play session / Marlow enters Play only when Wren says Rook is idle / pending: playtester.md line
- 2026-10-01 Rook: 465 walk-into meshes had no collider and no check caught it / a collider check on walk-into meshes over 0.5 m / pending: capture
- 2026-10-02 Rook: a pixel check rendered about 1,100 full-size frames in one Editor frame and crashed the GPU (second D3D12 crash from batch rendering) / render in small batches across jobs / main3_deck_pixel_check.cs
- 2026-10-02 Rook, Wren: since the GPU Resident Drawer went on, every capture sheet left out the cabin, tower and forest, so look and forest grades rested on frames missing them / capture with the drawer off in memory / pending: main3_review_capture.cs
- 2026-10-02 Rook: 8.21a passed its own check while hedges could still be stood on; stop heights measured terrain only and the 2 m flood never reached the stop edges / measure all walkable colliders and use finer flood cells along stops / pending: main3_8_21a_stops.cs, main3_area_check.cs
- 2026-10-02 Rook: the lake area flood passed Marlow's dock rail trap; the lake bed was no closed zone and the saved area asset never got the lake stops (a changed class default does not reach a saved asset) / closed water fill, and stops set in the setup recipe / main3_area_check.cs, main3_areas_setup.cs
- 2026-10-02 Marlow, Sable: Sable drew to colliders from recipes; the Hollow Giant and Camp 3 tent colliders were inflated boxes far larger than the mesh, and the 828 survey flood left out a trail, so it reported a false pocket that Sable then walled / every ground survey floods from all trails in the area, and every drawn opening or landing is checked against the visible mesh as well as the collider / 828_Marlow_ground.md, 826 and 828 paper checks
