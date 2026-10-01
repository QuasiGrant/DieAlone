# WardPath draft 1: paper check (Marlow)
2026-10-01. Docs/Design/WardPath.md and WardPath.svg at 3345b2e (draft plus Vesper and Quill notes). Ground and colliders read from Main3 in the Editor on 3345b2e (Play was running; read-only raycasts and Terrain.SampleHeight, nothing moved or edited). Mover from PlayerController.cs and PlayerTuning.asset: walk 2.5, sprint 5.5, hop 0.6 m, step offset 0.1, slope limit 45, eye 1.6, radius 0.35, no fall damage.

**Result: FAIL.** 3 blocks, 6 hurts. Walk time, day closure, F-1 and W-1 pass on paper.

## Blocks

1. **The stair stands on a flat bench, not on the step.**
   - Where: flights 1 to 3, x 34 to 55, z 250 to 262.
   - Measured ground under the drawn flights: 36.5 to 38.6 from x 55 to x 43 (one flat bench, x 38 to 56, z 244 to 280, 35 to 41). The step is x 29 to 41: 39 at x 41, 44 at x 35, 50 at x 32, 57 at x 31.5, 60 at x 29, about 60 degrees.
   - So landing 1 (45.5) stands 8.4 m over ground, landing 2 (53) 14.9 m, flight 3 up to 17 m; the lookout (58) is 6 m over ground at x 34. As drawn it is a free-standing timber tower on the bench, with 40 of its 47 m of flight over flat ground. Open question 3 is answered: the flights cannot "fit to" this ground in this plan.
   - What I would do instead: flights run north-south along the face (x 29 to 41), cut into it, so the treads sit on ground; that needs about 40 m of 30 degree run inside an 18 m strip (z 244 to 262), so a redraw.
   - Repro: Terrain.SampleHeight along (55, 250), (40, 253), (52, 258), (34, 262).

2. **The z 256 deadfall rim does not close the bench.**
   - Where: SVG rim z 256 runs x 48 to 62. Ground at z 256: 38 from x 38 to 56, then the east drop (34 at x 59, 22 at x 65); the face foot is at x 37.
   - x 38 to 48 is 10 m of flat open bench. Walk north there and you are on the old shelf, then old P2 and the cwm. The Gate batch P2 climb trap (63.8, 29.2, 279.0) is reached from that bench. "The P2 climb trap is behind the rim" is false as drawn.
   - Repro: from the stair foot (55, 250), walk west to (43, 252), then north to (43, 266). Flat all the way (37 to 40).

3. **The upper stair hangs over the closed side.**
   - Where: landing 2 (52, 258), flight 3 (z 258 to 262), the lookout's north and east edges, and flight 2 east of x 47. All are north of z 256, over the bench that the z 256 and z 266 rims close.
   - The edges there are "rail" with no height given. A 1 m rail on a sloped flight is cleared by a diagonal downhill sprint-jump. Numbers: 5.5 m/s for 0.49 s is 2.7 m, so 1.9 m along a 30 degree flight, where the ramp drops 1.1 m. The 8.9c tower rails were cleared this way. Falls do no damage.
   - Landing below: a fall puts the player on the closed bench, 15 m down. Both 1.3 m rims hold from either side by design, so the player cannot get back: soft lock.
   - Repro, once built: on flight 3 at (44, 260), sprint-jump north-east over the rail.

## Hurts

4. **Shot B does not show the fire "under you".**
   - Where: path end (-8.5, 246).
   - Lip_End collider: inner face at x -10.0, top 63.1. Eye 63.6.
   - Rays west from the eye clear at 15 degrees down and hit the lip from 18 degrees (z 240 and 246; from 15 at z 252). The doc's 12 to 45 degrees below needs the floor fire bases (25 to 45 degrees) to show. They cannot from the path end: you see 12 to about 16 degrees, tops only.
   - Valley 4.7's "28 degrees" was already wrong (Gate_8_14a_Marlow 12). Pressed against the lip (eye at x -9.65) is unmeasured: it depends on rim width.
   - Collider only. The render mesh may differ.

5. **Shot A's left half has no fire.**
   - From the fin exit (4, 257.3), heading 215, 91.5 across, the frame spans bearings 169 to 261. The far front (x -240, z -60 to 650) spans 217.6 to 328. So the far front fills only the right 43 degrees (from 2.6 right of centre). The valley fires are under the lip.
   - The stones sit at bearings 185 to 197, against the S end wall (top 66, about 3 degrees up) and sky, not against flame. The SVG sketch draws flames edge to edge behind the stones, and that is wrong.
   - Stone distances: 32.3, 34.4, 37.9 m (doc: 31 to 36).

6. **The draw is not a gully.**
   - Where: x 52 to 86, z 210 to 216.
   - The tread runs on a spine. To the south it drops 3 to 11 m to a flat at 16 (x 48 to 76, z 188 to 208). To the north it drops to a flat at 16 (x 60 to 80, z 219 to 236).
   - "Banks laid back to 35 degrees; rim on each bank top" has no rising bank to lay back. Firs "on both banks within 8 m, crowns closing" would stand on those flats, 3 to 11 m below the tread.
   - The flats are ward-side and sealed: Band_S_W on the east; to the south-east a 4 m terrain step (11.9 to 15.9 in about 1 m) under ClimbRim_Collider, x 67 to 79, z 190 to 195. So laying the banks back opens no day route.

7. **Open question 1 is stale, and the doc's closures rest on it.**
   - The mover was fixed on 2026-09-30 (a23ff06, 8.14a; PLAN Rules "STEEP GROUND"). Ground over 45 degrees cannot be jumped from and slides the player off. Valley 4.4 and its rev 11 header, which the doc cites, predate it.
   - What is left of "hop on steep slopes":
     - (a) Slide pockets, where the slide wedges the player against a collider at a face foot. 6 were found in 8.14a, and the P2 trap in the Gate batch. The z 266 rim sits on a 67 degree face (61 at x 29, 54 at x 32), which is that pattern.
     - (b) Reach is 0.7 m on level ground (0.6 hop plus 0.1 step). A 1.1 m lip held 118 pushes (8.14a), but rails on sloped flights do not hold (3).
   - "Then the rims can shrink" does not follow: 1.1 to 1.3 m is already what this mover needs. The 3.5 m ring walls have been obsolete since 8.14a.

8. **Joins with steps over 0.1 m stall the player.**
   - Where: draw log steps, flight 3's log edges, both ends of the plank bridge at 38, and the deck-to-ground join at the lookout (deck 58; ground 57 at x 31.5, 60 at x 29).
   - The doc names StairRamp only for flight 1. Every one of these needs a StairRamp or a lip of 0.1 m or less (DECISIONS 2026-09-20).
   - The ramp grades themselves pass: flight 1 30.5 degrees, flight 2 30.0, flight 3 15.

9. **Night-one baseline and beats.**
   - Night 1 is WARD 12 (LoopTuning startWard 12), which is High. The High row lists "cup gone; rune post doubled". Night 1 is the first walk (the path is closed by day), so it must carry no change, or the baseline is wrong. "Car on night 1 only" is a night beat, not a WARD change.
   - The lookout to tower cab line is clear: 0 colliders and 0 tree bounds on the ray (34, 59.6, 262) to (164, 58, 166). The 8.19 forest is not in yet. Cabin window, lot lights and highway from the lookout: unverified.
   - The night-one car was timed for P4 at 216 m. The lookout is at about 140 m, and the doc names no trigger.
   - Agree with Quill 3: only landing 1 is a named hide, and N1 needs platform hides. The insect-cut trigger in the private sound doc still uses a pre-rev-11 coordinate; the dogleg is now (18, 262) to (14.5, 265.5).

## Passes, with notes

10. **Walk time: PASS.**
    - The doc's coordinates sum to 201 m from J to the path end, not 207. Landing 2 is at 120 (doc 127), the lookout 138 to 144 (doc 145), the throat 144 to 184, the ledge 184 to 201.
    - Camp to path end is 281 m. That is 104 s at the timed rate (348 m in 128.6 s). The timed rate is 2.71 m/s, faster than walk (2.5), so it is not a walk speed. At walk it is about 112 s, plus about 5 m of slope length on the flights.
    - Ceiling 150 s holds. The largest landmark gap is 34 m (seep 54 to rune post 88).
11. **Day closure and crest by day: PASS.**
    - IW2 (GateBlocker, x 86, z 211 to 215, y 12 to 16) is as built.
    - The draw flats are sealed (6). The bench faces are about 60 degrees.
    - No new route reaches the crest by day.
12. **Leg skips at night: none found on paper, if 2 and 3 are fixed.**
    - The flights are 7.5 m apart vertically over a 60 degree face, so "cutting a corner by hopping" cannot reach the next flight up.
    - Hops down from flight 2 south of z 256 land on the open bench, which is fine.
13. **F-1 and W-1: PASS on paper. The wording is wrong.**
    - "Nothing at x 20 or less changes" is false by the doc's own text: menhirs at x -7 to 1, the lip rim, lip firs, N and S wall facing, the curtain, boulders.
    - None of them opens a day line:
      - All sit west of the 80 to 86 crest.
      - The deck line to the stone tops still passes the knob at 68.2 against 84.
      - The deck line to the N end wall crosses x 20 at z 270, where ground is 86, so facing up to 85 stays hidden.
    - The lookout join needs fill at x 29 to 34, against "terrain work at x 34 or more". That is harmless for F-1.
    - Rook's rerun should include the menhirs (bounds sit under the mesh on rotated pack meshes; use the lowest vertex).

Marlow
