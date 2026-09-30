# Gate step 2, PLAN 8.14a plus 8.15 (paths, ground and stops)

Marlow, 2026-09-30. Sheets: Docs/Captures/Main3Review/ (12:17, commit d5360c4). Every sheet opened; Pairs, Warps_NESW, Stops_1, Walk_Views, Markers, Climb and Grey_Trails_Night cropped to full size.
Editor work (Play, PlayerController.Step, dt 0.02): by the time the Editor was free it held 2cf18f4 (Rook's ring-wall back-fill, after d5360c4), so every Editor number below is on 2cf18f4, not d5360c4. Play stopped after, runInBackground false, no files written into the project.
Stage rule (Wren): forest (8.16) and places (8.17) listed, not failed. Night trail reading moved to 8.15a (863ac62), listed, not failed.

## Recheck of the 8.14a FAILs (Gate_8_14a_Marlow.md)

1. Item 5, lot and office, road beyond the gate: **PASS.** Pairs "Lot facing the highway" and W15 E: drive through the gate, highway edge line, white reflector posts, lit T light, poles. Office (Pairs Office, W10 E, Walk_Views OFFICE FACING EAST): the road itself is 1 to 3 px; only the lamp, posts and fence read. Hurts.
2. Item 8, day and night pairs: **PASS.** 10 of 10 comparable. LAKE PUMP DAY ONE now shows pump and lake; OFFICE DAY ONE shows the office wall with brush in front.
3. 11.3, ledge reveal: **PASS.** My count, invisible colliders off, temporary mesh colliders on renderers within 60 m, 9 points per card:
   - Warp eye (-9, 63.6, 246): 120 of 120 cards, tops 120; 592 of 1080 points (the lip hides about half of every card). Eye x -8.5: 120 of 120.
   - Along the path end, x -9: 115 to 120 at z 240 to 252. Two metres back (x -7): 5 to 59 of 120 (Lip_End hides most). Crouched at the lip (eye 1.0 m): 0 of 120.
   - Pushes: 1890 from a 0.8 m by 1 m grid over x -9.6 to -5, z 236 to 256, three modes (sprint-hop, run-up jump, walk-hop), five headings west: 0 off, 0 on the lip. Controller step 0.1, lip top 63.2 over path 62.0.
   - Aside: ClimbRim_Collider stands 1.2 m high inside the path at (-9.6, 235 to 236), 0.4 m in from the Lip mesh. Not reachable (1.2 m over a 0.6 m jump). A player placed on it falls to y -5. Cosmetic.

## Other 8.14a findings, rechecked

4. Warps in bushes: fixed (W4, W10, W16).
5. Stepped rims: fixed (W8 N, W9 N: smooth rim tops).
6. J sign doubled label: still there. W17 E: "NORT" and "RTH LOOP" overlap; the J warp lands at the post. Cosmetic.
7. Pump trench: unchanged. Walk_Views PUMP TRENCH 30 M LEFT and RIGHT, Trail_Camp_to_pump FWD 40 to 60, BACK 20 to 40: 4 to 8 m banks. Hurts.
8. Masonry: W22 E, J TO WARD BACK 0 and 10 M: the back wall is still vertical flutes like a curtain. One horizontal ledge line added. Hurts.
9. Climb oppressive: rock fills 70 percent or more of the frame in 11 of 26 Climb frames (20 to 50, 120, 130, 150, 160, 210 to 230 M); it was 10. CLIMB 220 M: a flat white untextured stone. Hurts (DECISIONS 2026-09-30: "never oppressive").
10. P4 look-back: Walk_Views P4 LOOK EAST, W21 E. The rim boulders now render near-black and cover the lower 30 to 45 percent of the view. Hurts.

## Rook's notes

11. Slide pockets, measured on 2cf18f4:
    - (44.7, 49.4, 290.9): now inside ClimbRing_Rock. Gone.
    - (42.7, 50.6, 294.9): free, 30 of 32 ways out.
    - (48.7, 38.3, 282.0): **still traps: 0 of 16 ways out** (8 walks, 8 sprint-jumps, 45 degrees apart). 41 of 399 drops on a 1 m grid over x 40 to 58, z 272 to 292 slide into it, from the faces at x 45 to 49, z 281 to 285. This contradicts Rook's "1667 places, 0 trapped". Repro: teleport to (48.7, 38.6, 282.0), try all eight ways.
    - Reach: a flood from 61 J to Ward trail points (x under 62, z over 262). Moves: 12 m sprint-walks and 2.2 s sprint-jumps in 16 directions, falls and slides included; 3 rounds, 9632 moves, 545 places. No move ended in the pit. The nearest end was 2.8 m from a pocket, at (42.9, 47.3, 291.1). The pit sits under J to Ward 137 m (44, 283), behind the rim.
    - **Not reachable by walking, jumping, falling or sliding** in this test. Cosmetic. d5360c4 itself not measured.
12. Rubble at (58, 254): on 2cf18f4, rim boulders stand at it: CS_Rock_5 at 0 m, CS_Rock_6 at 0.35 m, CS_Rock_4 at 0.68 m. A sprint from the trail at (55.9, 262.4) heading SE stops at (57.6, 38.3, 259.4) against CS_Rock_3 (0.27 m) and CS_Rock_5. Not a bare stop now; it escapes north.
13. W1 contrast: confirmed from index.md. By day 9 of 14 trails pass. At night 0 of 14 pass (8.15a). By eye at night, the pale edge stones (Valley.md section 6) do not show on Camp to J or J to Ward (Grey_Trails_Night, all frames). 8.15a has to fix that.

## Checklist, 8.15 stage

14. 1. Trails read apart from ground: **FAIL.** By day, 5 of 14 trails fail at 20 m: Camp 2 to T 12, Jg to T -2, Jg to Camp 1 5, W1 to Camp 3 3, J to Ward 9. By eye:
       - J to Ward FWD 20 to 50 and BACK 200 to 230: the tread cannot be told from the floor in the dark chute and cleft.
       - W1 to Camp 3 is dark end to end.
       - Jg to T reads by eye; its numbers fail because brush is the floor.
       - Capture fault: trail frames are every 10 m; Gate.md 2.1 asks for 5 m.
    2. Invisible stops have a visible reason: **PASS.** Stops_1 S1 to S44 all face a bush, rim boulder, stone steps or rock. S3: the bush has a hard vertical seam, cosmetic. Word for word, the 8.15 line says "invisible walls removed except the front and the Ward path by day"; InvisibleColliders.md still lists 561 colliders without a renderer (Hedges 389, WadeLimit 96). They pass Wren's visible-reason rule, not the PLAN wording.
    3. Trail ends: **PASS.** Trail_Ends, 28 frames. Camp 2 and Camp 3 ends face gray placeholders (8.17).
    4. Places without labels: **expected, 8.17.**
    5. Lot and office: **PASS.** See 1.
    6. Climb legs unlike: **PASS.** Leg 1 walled chute (20 to 50 M), leg 2 open bench and knob (60 to 110), leg 3 dark shelf with snags (120 to 160), leg 4 boulders and open sky (170 to 200), cleft (210 to 230).
    7. Top-down and compass views: **PASS.** Compass_Views now from the walkway, clear of the cab. East floor bare (8.16).
    8. Day and night pairs: **PASS.** See 2. Night frames near black (8.15a, Vesper).
    9. F1 by keyboard and pad: **PASS.** dev_panel_8_11_check.cs in Play: 0 failures. Night 2 presses, Ward 3, J at night 5 (at the limit). Virtual devices only.
    10. Hand walk: **PASS.** Every trail, both ways, sprinting and sprint-hopping on every landing, 56 walks, gates off. 0 stalls; 2 stops 0.6 m short of Camp 1 to J's end point inside the Camp 1 pole (282, 238), a route artifact. Max 0.6 m off the line. Run on 2cf18f4, driven by PlayerController.Step, not keys in the Game view.

## New findings, 8.15 content

15. 1. **Gray default slab on the trail at J.** PointsOfInterest/POI_Plank_bridge/Deck: a 4.3 by 4.3 m box, material "Lit" (untextured gray), at (105.3, 9.9, 205.2). No water under it. It fills the foreground of Pairs J DAY ONE and NIGHT, Markers SIGN J, Trail_Ends CAMP 1 TO J START and J TO WARD BACK 240 and 250. 8.15 asks for "every stop a visible thing from owned packs". Repro: F1, warp J, look NW. Hurts.
    2. **No rock on slopes.** Banks show red and green burn stripes, not rock: W4 N, W19 E, Walk_Views PUMP TRENCH 30 M LEFT and RIGHT, Stops S14 and S17. The knob is still a smooth pyramid (CLIMB 100 and 110 M). The Camp 3 hollow walls are smooth planes (W8, W9). Hurts.
    3. **Gray untextured boxes by W1 to Camp 3.** FWD 20 and 60 M, BACK 30 and 80 M. Whether these are 8.17 place stand-ins is unverified. Hurts if not.
    4. **Junction markers.** SIGN JG: the post stands in brush and the LOT arm is cut off (Markers, W16 E). TRAILHEAD BOARD is edge-on from its own trail 5 m back (Markers); it reads only from W14 N. Cosmetic.
    5. **Ward by day, looking west.** W22 W and Pairs WARD PATH END DAY ONE: the lip, then one flat untextured gray plane to the horizon. At night: valley fires on a flat band, and four dark rectangular boards (the smoke columns). Hurts (Vesper to grade).
    6. **Blocky textures at close range.** W9 W, Jg to T FWD 20 and 30 (trunk), S40: large-pixel rock and bark. Cosmetic.
    7. **Unverified, Editor only.** In Camera.main renders during Rook's uncommitted recipe edits after fd33b43, the leg 2 bench and the (58, 259) trail showed a gray and white checker ground. Views: from (56, 40.6, 240) facing north, and from (55.9, 40.6, 262.4) facing SSE. The d5360c4 sheets do not show it. Recheck on the next capture.

## Done-check, word for word

16. PLAN 8.15: "Done when: the eye-height gate passes, with Pim's W1 to W4 counts at zero failures."
    - Not passed: my item 1 fails, and W1 by day is 5 of 14 failures.
    - The task text also asks for "rock on slopes" and "every stop a visible thing from owned packs". 15.1 and 15.2 miss both.
    - PLAN 8.14a: passes for its own FAILs (1 to 3 above).

## Verdict

**FAIL.** Item 1 (5 of 14 trails by day), 15.1 (gray slab at J), 15.2 (no rock on slopes). 8.14a's own FAILs (lot, pairs, ledge) now pass. The slide pit at (48.7, 38.3, 282.0) still traps but was not reached from the trail.

Marlow
