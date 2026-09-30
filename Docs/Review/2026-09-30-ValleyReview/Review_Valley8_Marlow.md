# Review: Valley.md rev 8 and Valley_map.svg, play and numbers (Marlow, 2026-09-30)

Paper only; nothing walked. SVG read at 1.5 px per m, x = 150 + 1.5X, y = 600 - 1.5Z. Controller: slopeLimit 45, stepOffset 0.1, jump 0.6 (Player.prefab, PlayerTuning.cs).

## Blocks
1. **Arm ends leak out of the park.** Fence rocks cover only about x 392 to 404 at z 301 to 311 and z -9 to 1. No stop is named on the N foot from x 300 to 392 or the S foot east of the lake (x 245 to 392); arm crests there are 20 to 34, gentle. Repro: from (380, 290) walk north up the foothill to the arm crest near z 332, then down the outer slope to the terrain end (150 m past), or east round the fence rock to the highway curve at z 430. From the arm crest, west along the N arm to the N hook (66 to 70) by day puts the player over the fire valley on day one. Breaks DECISIONS 2026-09-29 (fence; fire not visible on day one).
2. **Ledge not closed at its N and S ends.** Ledge x -10 to 6, z 238 to 300 at 62 meets crest land 64 to 70 at z 228 to 232 and north of the cleft; the lip is only "at the edge". Repro: path end (0, 272), walk south past the stones, step up onto the crest at about (12, 225), follow it to the S hook and the S arm (item 1); or walk off the west face to the fire floor at -40 (no fall damage, PLAN 2.2).
3. **IW2 walks round, and the face is walkable.** SVG draws IW2 9 m long at x 100, z 204 to 213, with J in open forest and the ridge foot at x 80. Repro: from (104, 215) walk west to the chute foot (84, 205) by day. The W face at z 195 to 300 averages 40 degrees (foot x 80 at 12 to crest x 12 at 64 to 70), under slopeLimit 45: unless it is cut into rock steps over 0.7 m, the whole face is a daytime route up. Agrees with Pim 4. I would put IW2 in the chute between its boulder walls.
4. **Belt cover is measured at the wrong height.** Deck rays cross the W crest at 72.8 (flame) and 78.3 (smoke), 3 to 14 m above crest ground: trunks and lower crown, not the tops. "Clears by 13" is irrelevant. Pack crowns are alpha cards with holes; F-1's solid crown proxy passes what the render leaks; binocular zoom enlarges the holes; LOD billboards or culling at 150 to 350 m are unverified (terrain trees billboard past Billboard Start). Firs 18 to 24 on ground 64 top out at 82 to 88, so "tops 86 or more" fails at the saddle with short firs. Agrees with Vesper 1 and 2: land must hide flame tops from every daytime eye with the +3 rule; the belt carries smoke and skyline only. F-1 must be a render test: flame and smoke in a key colour, zero key pixels from the deck grid at normal and binocular FOV, shipped LODs.
5. **Daytime eyes the doc never checks.** Camp 2 stack top (292, 108), eye 25.6: flame line crosses the saddle (z 60) at 63.2 against land 64, 0.8 m (0.2 at jump); smoke-top line at 69.6, 5.6 m over land. Lot at eye 4.6: smoke-top line crosses the saddle at 63.4, 0.6 m. Both fail +3 on land and lean on belt trees 280 to 350 m away, where LOD culling is likeliest.
6. **Belt gaps in the drawing.** Belt ends z 228, knob starts z 232; knob ends z 278, belt restarts z 281. Deck rays cross there at about 73 over land 64 to 70. The rock between knob and cleft (x 6 to 20, z 278 to 292) has no height.

## Hurts
7. **Reveal spoiled from leg 4.** From (25, 285), eye 60.6, a flame line at z 285 passes the ledge edge (x 6) at 63.2, 1.2 m over ledge ground: night-1 fire before the slot unless item 6's rock is 66 or more. Belt extent: text says x 5 to 30 (sits on leg 4 and in front of P4's east view), SVG draws about x 3 to 21 (leg 4 open east, sees the cab before P4). Pick one.
8. **W-1 margin depends on the knob's profile.** Stone-top line is 68.2 at the knob's east edge (x 20), 69.1 at x 8; with +3, 71.2 and 72.1. The knob must hold 74 across x 8 to 20 at z 240 to 256, not only at a peak.
9. **Legs 2, 3, 4 are stacked.** At z 272 leg 2 (x 50, 38) and leg 4 (x 26, 56.7) are 24 m apart, 18.7 m up: 38 degrees, leg 3 between at about 40 each side, all under slopeLimit 45. Without rock steps players walk straight up and skip leg 3.
10. **Variety.** Legs 2 and 4 both run north (358 and 354 degrees): three bearings, not four. P2 and P3 are 150-degree hairpins inside 24 m, the zigzag Grant called uniform. Not oppressive: grades 10 to 27 percent, three 4 m flights, 117 s.
11. **Invisible walls.** IW3 breaks DECISIONS 2026-09-30 until Grant answers open 1, and walks round from the office 60 m through open forest (agrees with Pim 5). Tower landing outer-edge stops (Main3.md section 3 note 4) have no rail and are not in 8.3's kept list: they break it too.
12. **Deck sees over the arms.** Arm east halves (20 to 45) sit 13 to 38 m under the deck eye; the deck looks down their back slopes, and the doc names no outer floor N or S. E-1 must rerun on rev 8.
13. **Day-one smoke.** Valley 5.2 puts a smoke sheet up on day 1; Edges 1.7 says no smoke on day one. Its z extent and whether it keeps rising west of x -500 are not given; items 5 and 6 depend on both. Only the ledge on night 1 needs it: spawn it at nightfall, or it is a daytime leak with 0.6 m margins. Grant's call against "never switched off".
14. **Off trail at night:** fog 8 to 60 m, the only guide the tower cab, hidden under floor-giant crowns (tops to 52, cab 56). By day see Pim 9.
15. **Ward over tower:** ledge 62 against a cab roof of about 60 (unverified). P4 at 60 is level with the cab; 1.6 says both "level" and "higher". Grant's 2026-09-29 complaint can repeat (open 3).

## Numbers (cosmetic)
16. Legs sum to 292 m, 116.8 s; camp to end 372 m, 148.8 s: correct. SVG: leg 3 is 44.7 m (grade 27, not 24 percent), leg 4 60.3, cleft 23.2, leg 1 32.8, but three 4 m flights at 0.25/0.3 plus 20 percent ramps need 34.4 m of run. Points of interest after the split snag are 17 m short (bent fir 206, P4 240, cleft 246, fire 266); gaps stay under 75 m. The 40-degree stairs likely slow the capsule (unverified): expect 150 to 152 s on screen, inside 3 percent, over "under 150".
17. PASS: dead ends are only the cave spur and the ledge; the night ends by fade to the cabin (DailyLoop 7), so there is no walk back. Every other leg has a point of interest inside 75 m.

Marlow

## Revision 9 (recheck, 2026-09-30)

Paper only; nothing walked. Same SVG scale. Rays: C# over the crest polylines and heights in Valley.md 2 (W crest x 12, N and S crests as drawn, heights interpolated between the named points), +3 on eye and target, jump +0.6. Controller: PlayerController.Move has no slope slide; isGrounded alone gates the jump.

### Blocks
1. **Fire seen by day from the north-east and south-east corners.** Lines to the far front's ends cross the arms at their low saddles, not the W crest. Closed campground (372, 262), ground 8, jump eye 10.2: flame top at (-240, 643) clears the N arm saddle (250, 338, 28) by 1.1 m with no +3 (fails +3 by 4.1). (390, 250): 3.2 m clear (-6.2 with +3). (385, 295): -5.6 with +3. (360, 230): 0.0 with +3. South-east, open forest west of the SE thicket, (330, 5), ground 3: flame at (-240, -150) grazes the S arm at (159, -42) by 0.2 m, fails +3 by 2.8; (340, 15) fails +3 by 1.3. Day-1 sheet from (385, 295) fails +3 by 0.8. The closed campground is walkable outside shifts (E14) and off-trail walking is allowed, so these are daytime eyes. Section 7 lists neither corner. Unless the arms' valley-side slopes are shaped to block, this breaks DECISIONS 2026-09-29 (fire not visible on day one). Repro: stand at (390, 250) by day, face bearing 285, look over the N arm saddle.
2. **IW2 walks round in the drawing.** The band gap is at x 80, z 209.3 to 216.7 (7.4 m); IW2 is drawn 6 m east of it at x 86, z 210 to 216, with boulders r 2.7 at z 207.3 and 218.7. Between the band (edge x 80.7) and the north boulder (edge x 83.3) is a 2.6 m lane. Repro: from J walk to (84, 224), south along x 82 to (82, 213), west up the chute by day. Text says "3 m between boulders" and "the chain hangs where the wall is"; the drawing wins for shapes and shows neither.
3. **Ledge ends open.** Ledge x -10 to 6, z 215 to 285; the lip is drawn along x -10 only. The shoulder (x 6 to 20) and knob (x 8 to 20) sit east of the ledge, parallel to it, not across its ends. With the west face falling 80 to -40 over x 12 to -40, the ground at z 286 is under 62 west of about x 4. Repro: from the path end walk north to (-2, 284), keep going: drop down the west face to the fire floor at -40, off the map. Same at the S end, (-2, 214). The claim "N end: the shoulder's rock wall; S end: the knob's west face" needs rock at 63 or more across x -10 to 6 at both ends, and it is not in the heights or the drawing.
4. **IW3 walks round.** IW3 is drawn at z 176, x 381 to 389. The booth M2 (392, 180) is already north of it and east of its end, and the lot (to z 190) is north of it too. The only way into the campground is the 10 m gap x 386 to 396 at z 206 to 215 where the spur passes the brush. Repro: during a shift, from the booth window walk north along x 393 to (393, 212), then to (385, 240). Second gap: the x 340 to 348 brush ends at z 300, the N foot band edge is at about z 302.2 there: a 2.2 m lane covered only by NE grove trees. Repro: (330, 296) east to (360, 290). Breaks DECISIONS 2026-09-29 (following a car hits the wall). I would put IW3 across the spur gap at z 206 to 215 and close the lane.

### Hurts
5. **50-degree faces are not a stop by themselves.** No slope slide in PlayerController; isGrounded is true on any below contact, so jump-spamming up a face over slopeLimit is likely (unverified on this controller; the 8.9a hedges fell the same way). The rock bands (3 to 4 m) and 2 m bench steps hold; the cwm rims (24 to 28 m of 50-degree face, no band) and the face over P4 do not rely on anything else. Test at build with the real jump. Crest reached at night leads down the arm back slopes to the unbounded outer ground.
6. **Ledge lip 0.8 m against a 0.7 m reach** (jump 0.6 plus stepOffset 0.1). 0.1 m margin; a rounded or sloped rock top may let the capsule ride up. Test at build.
7. **Cleft fin not specified.** No slot width, fin position or fin length in the doc; the drawing has no fin. From the dogleg looking west, a ray out the mouth past the fin's south tip toward bearing 250 crosses the lip into the valley. Cannot pass on paper; camera test at build as the doc says.
8. **Deck margin depends on where the crest top sits.** Sable measures at x 15. At x 12: 1.6 (1.2 at jump). If the top wanders to x 10 as 2.4 allows: 1.0 at jump. Still hidden; Rook's F-1 must test the built top line, not a single x.

### Rechecks of rev 8 items
- R8-1 arms: PASS on paper. N band x 80 to 396 and S band x 38 to 396 are continuous; both outcrops span x 391 to 404 over the band ends and the fence.
- R8-2 ledge: FAIL (block 3).
- R8-3 IW2 and face: face PASS on paper with the bands and steps (see 5); IW2 FAIL (block 2).
- R8-4 and R8-6 belt: moot, land now does the work.
- R8-5 stack and lot: PASS. Stack worst is through the saddle, not the SW corner: flame (-240, -115) at 3.3 (3.0 at jump), not 6.1. Lot 8.0 (7.8 at jump), not 8.8. The stack's margin means the saddle may not drop under 74.
- R8-7 leg 4: PASS. Shoulder 80 stands 6 to 10 m west, 20 m or more over the eye; P4 sees east only.
- R8-9 stacked legs: PASS on paper (2 m steps, 30 m apart).
- R8-10 bearings: PASS. 5, 279 then 325, 169 then 190: four bearings. P3 turn is 156 degrees, not 140.
- R8-11 landing rails: PASS (rails round every landing, stops inside). IW3: FAIL (block 4); still also waits on Grant (open 1).
- Climb numbers: legs match the SVG within 0.5 m; J to path end 265.5 m, 106 s; chainage correct.

### Cosmetic
9. 4.2 "largest gap 58 m": the largest is 34 m (chain to seep, seep to rune post).
10. 5.3 "every daytime line passes over the W crest at least 10 m above it": means under.
11. F-1 table: deck to (-240, 650) is 1.8 (1.4 at jump), not 2.4; it crosses the N hook at (19, 340), ground about 79.4.

### Crest at 80 from the valley floor (player view)
Not oppressive from most of the valley: from camp the crest is 23 degrees up, from Camp 1 15. It fills the top of a level view only within about 110 m of the W foot: J 37 degrees, Camp 3 about 49 (55 with the belt tops). At J that suits a gate you dread; at Camp 3 the hollow reads as a pit. The climb is fine: 106 s, grades 18 to 25 percent, a stop every 34 m or less. The wall does make the west third feel closed and the valley read smaller, which works against Grant's "map feels a little small" note.

FAIL: blocks 1 to 4.

Marlow
