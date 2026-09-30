# Gate step 2, PLAN 8.14 (Valley rev 10 ground and road)

Marlow, 2026-09-30. Commit 3b61464. Sheets: Docs/Captures/Main3Review/ (2026-09-30 10:12, after 9f9e12f). Judged from the pictures; every sheet opened. No Editor this session: items 9 and 10 not run.
Stage rule (Wren): paths, ground textures, forest, visible stops off the climb and places are 8.15 to 8.17 and are listed, not failed.

## Checklist

1. Trails read apart from ground: **expected, 8.15+.** No trail band on any trail sheet. Not failed.
2. Invisible stops have a visible reason: **PASS.** Stops_1: S1 is IW2 (CairnGate/GateBlocker, the allowed day exception); S2 to S4 are ChuteSteps/StairRamp colliders inside the visible stone steps. IW1 and IW3 are not framed (IW1 front exit exception; IW3 shift only).
3. Trail ends go on or end at a place: **PASS.** Trail_Ends, all 26 frames face a place, a junction or the next trail. No terrain dead end found.
4. Places read without labels: **expected, 8.17.** Gray boxes throughout. Terrain around places is failed under item 11 below.
5. Lot and office, road beyond the gate in frame: **FAIL.** Warps W15 LOT_HIGHWAY E, W11 STORE E, W12 GATE_BOOTH E, Pairs "Office day one": the fence reads as a solid gray wall the full width of the frame; the drive past the gate, the T, the lanes and the reflector posts are not visible. Only pole tops and the verge tree (a black cylinder that reads as a chimney) show over it.
6. Climb legs do not look alike: **FAIL.** Climb_J_to_Ward: leg 1 (CLIMB 20 to 40 M), leg 3 (120 to 160 M), leg 4 (170 to 200 M) and the cleft (210 to 230 M) are all a brown walled trench with a strip of sky. Only leg 2 (60 to 110 M) differs.
7. Top-down plus four compass views, no empty quarter: **FAIL.** The capture has no compass views (Map_TopDown only). From the top-down and warps, the empty NE (x 250 to 396, z 250 to 330) and SE floors are forest work, 8.16+.
8. Day and night of the same 10 spots: **FAIL.** Pairs_DayOne_Night has 6 spots, not 10. Of the 6, WARD PATH END NIGHT is 8.14's (item 11.3).
9. F1 by keyboard and pad: **NOT RUN** (no Editor). Stays open until Wren books it.
10. Hand walk of new or changed trails: **NOT RUN** (no Editor). Plan below.

## 8.14 findings (fail on these)

11. Things 8.14 owns that fail:
   1. **P4 look-back is walled in.** Warps W21 WARD_P4 E; J TO WARD FWD 210 M. From P4 (26, 262) facing east, a ring wall about 1.5 m away fills the lower half of the frame and a second crenellated wall fills the rest. No tower cab, cabin, lake, lot or highway. Valley.md 1.6 and E8 (night-1 car) depend on this frame. Repro: warp Ward_P4, look east. Blocks.
   2. **Climb is oppressive.** Climb_J_to_Ward frames 20 to 50 M and 120 to 230 M: rock wall fills 70 percent or more of the frame in 16 of 26 frames. DECISIONS 2026-09-30: "small and never oppressive." Cause: the 3.5 m ring walls on every leg (Rook's jump-climb fix). Blocks.
   3. **Ledge reveal reads as a black wall.** Pairs WARD PATH END DAY ONE and NIGHT; Warps W22 WARD W; J TO WARD FWD 240 and 250 M; Trail_Ends J TO WARD START. Day: a flat black slab across the frame with a few flame specks on its top edge; no valley below, no valley fires. Night: one row of identical flame cards like a fence, no columns, no depth. Valley.md 1.6: "the burning valley below, the far front across the frame, smoke columns rising." Paper cause, unverified in play: the lip went from 0.8 to 1.1 m, which cuts the steepest ray down past it (Valley.md 4.7 relied on 0.8 m for 28 degrees; valley fire bases sit 25 to 45 degrees down). Blocks.
   4. **W ridge reads as a fortress, not a ridge.** Trail_Camp_to_J FWD 0 to 70 M; Pairs J DAY ONE; Warps W17 JUNCTION_J W; CLIMB 0 and 10 M. Flat crest at 80 along its whole length, a crenellated rock band like battlements at the foot, a pale V-shaped slab (the cwm) and the knob as a smooth cone like a pyramid (CLIMB 60 to 110 M). Hurts.
   5. **Arms read as wedges.** Warps W13 N, W14 N, W15 N; Trail_Jg_to_Camp_1 FWD 0 to 80 M; W18 JUNCTION_W1 S. The N and S arms are smooth planes with a straight crest edge. Hurts.
   6. **Rock bands and ring walls read as masonry.** Warps W21 WARD_P4 N and E, W22 WARD E; J TO WARD BACK 0 and 10 M. Regular vertical stripes and flat tops like stacked planks or a castle wall. Shape is 8.14's; texture is 8.15's. Hurts.
   7. **Camp to pump runs in a trench.** Trail_Camp_to_pump FWD 20 to 70 M, BACK 10 to 60 M: dirt banks above eye height on both sides; no water through trunks, only dirt. Same fault as the 8.9c knoll. Repro: walk Camp to pump from (158, 150). Hurts.
   8. **Lake reads as a pit.** Trail_Pump_to_boathouse FWD 0 to 80 M (steep bank above eye on the left, lake a canal); BACK 0 M; Warps W4 LAKE_PUMP N and W (pump in a notch, sheer cut face west of it); W5 LAKE_BOATHOUSE N, E, S (boathouse in a bowl of domes); Trail_W1_to_Camp_3 BACK 90 to 110 M (a pond in a crater). Valley.md 1.2: "the widest sky; the postcard." Hurts.
   9. **Terrain spikes.** Sharp dirt cones: east of Camp 3 (Trail_Camp_to_Camp_3 BACK 0 and 10 M, Warps W8 CAMP_3 E; about (85 to 95, 143), unverified); above the cave spur (Trail_W1_to_cave BACK 10 and 20 M; about (55 to 65, 45 to 55), unverified); west of W1 (Warps W18 W; Trail_Pump_to_W1 FWD 50 to 70 M). Cosmetic, or hurts if a player can jump-climb onto one (check in the hand walk).
   10. **Open east closed by the fence.** Warps W13 CLOSED_CAMPGROUND E, W14 TRAILHEAD_T E, W12 GATE_BOOTH E; Trail_Camp_2_to_T FWD 50 to 90 M. The fence along x 396 renders as an opaque gray wall the whole way; the verge is the only thing past it. DECISIONS 2026-09-30: "the highway in view." Blocks (with item 5).
   11. **Verge tree reads as a chimney.** Warps W15 LOT_HIGHWAY E, W14 E, W10 OFFICE E. A 30 m black cylinder. Gray placeholder allowed, but at this silhouette it reads as industry and draws the eye off the road. Cosmetic.
   12. **Leg 2 straight is 61 m.** Rook's deviation. WalkChecks 11 caps straight views at 60 m, and the old exemption was for rev 7's benched switchbacks. Needs a ruling from Wren. Paper, hurts.
   13. **PLAN wording.** The 8.14 line says "Valley revision 8"; the build is rev 10. The done-check passes only when the gate passes; it does not, on 5, 6, 7, 8 and 11.1 to 11.3.

## Expected, 8.15+ (not failed)

12. Trail bands and ground textures (all trail sheets); rock texture on slopes; gray placeholder stones, steps, P2 roof, split snag and root plate (CLIMB 50, 120 to 160 M); gray places (W10 to W13, W19, W25); the missing N fir wall and crest belt (W3 TOWER_DECK W, W6 N); the empty NE and SE floors (Map_TopDown); night frames almost black (Vesper's call); the lake can be walked into (Rook).

## Hand walk (2.10), for Wren to book

13. What I will walk in the Editor, real CharacterController, walk, sprint, jump on landing, crouch:
   1. J to the path end and back. Sprint-jump the lip every 1.5 m (1.1 m high; a 1.2 m, 0.5 m thick rim was cleared in Rook's check) and both end walls. Sprint-jump the ring rims at P4's east edge, leg 2's outer edge and the cwm rims.
   2. From P4 look east, then stand at the slot exit (4, 257.3) and step round the fin: note the first frame with flame, and whether the valley fires show from the path end.
   3. Try jump-climbs from the valley onto the W face north and south of the band arms (x 80 to 86, z 195 to 230) and in the W foot pocket (x 46 to 80, z 165 to 200).
   4. Camp to pump trench and pump notch: climb out sideways; the sheer face west of the pump.
   5. The three spikes (11.9): can a player stand on top?
   6. Lot, office porch, store and booth: face east; is any part of the road visible; walk the fence line z 160 to 300.
   7. Every trail both ways at eye height, including walking back into Camp 3, the cave mouth and the boathouse.
   8. F1: every section by keyboard only and pad only, counting presses.

Marlow
