# Valley draft: paper number check

2026-09-29, Marlow. Sable's valley draft of Main3 (not built), checked against Main3.md revision 16, DECISIONS.md and the Player prefab. Paper only; nothing walked.

Inputs used: tower eye (164, 166) at 57.6 (58.2 jump), W-1 rule +3 m on eye and target; S1 eye (156, 148) 16.6 (LookSlice.md); Camp 2 stack top eye (292, 108) 25.6; flame tops 130; draft stones at about (-2, 262) tops 110; knob (12, 258) 115; saddle (15, 275) 100; west crest 95 along x 0, z 0 to 300. Straight-line interpolation between eye and target.

## 1. Sightline claims

1. **Tower to fire due west: pass.** Flames at x -300: line crosses x 0 at 83.2 (11.8 under 95); at +3, 86.2 (8.8 under). Draft figure confirmed. With the stand-in as built (Main3.md 3.7.8, nearest flames 200 m past the old cliff, x -190): 91.1, and **94.1 at +3, 0.9 m under the crest.** The draft silently moves the fire out to x -300; say so or the margin is under 1 m. Hurts.
2. **Tower sees the fire past both ends of the west ridge: fail. Blocks.** The west ridge is 95 only from z 0 to 300. The fire must span 150 degrees from the ledge (3.6.8), so flames run z -870 to 1370. Lines from the eye to those flames leave the map over the south and north ridges already high and rising:
   - to (-300, -870), bearing 204: exits z 0 at x 90, height 69.2, then climbs over the south ridge (crest 50 to 70).
   - to (-300, -400), bearing 219: exits z 0 at x 28, height 78.8.
   - to (-300, 800), bearing 324: exits z 300 at x 66, height 72.9, over the north ridge (crest 60 to 80).
   - to (-300, 1370), bearing 339: exits z 300 at x 112, height 65.7.
   Every such line is at or above the stated crests before the +3 rule. The draft only checked due west. Day 1 fire visible from the tower on south-west and north-west bearings.
3. **Ward hidden behind the knob: pass on the summit line only.** Tower to (-2, 262): 191.8 m; passes 3.5 m from the summit at 177.6 m out, line 106.1 (draft 106 confirmed), 8.9 under 115; +3: 109.1, 5.9 under. Lines to stones anywhere from z 256 to 270 cross x 12 between z 247 and 260 at 105 to 106 (+3: 108 to 109.2). Holds only if the knob stays at 110 or more across about z 246 to 262 at x 12, not just at the summit point. A cone-shaped knob fails this. Toward the saddle (100) the knob falls; it must not fall below 110 south of z 262. Unverified until shape is drawn. Hurts.
4. **Tower sees all six places: pass.** Lake, Camp 1, Camp 2, Camp 3 (Snag top 54) and the office all lie east of the ridge foot or on lines that never cross it. Cave (C-1) is hidden by the ravine rim as before; the ridge behind the mouth changes nothing.
5. **Cabin, camp, S1 (east, lower): pass due west.** S1 line crosses x 0 at 55.4 (39.6 under). Camp and cabin similar. South-west bearings cross the south ridge at 33 to 45 m within 100 m of the map edge; hidden only if the south ridge crest (50 to 70) sits within about 100 m of z 0. Distance not given. Unverified.
6. **West-side low places: pass.** Camp 3 floor: line at x 0 about 25 (70 under). Cave ravine floor -6: far lower. Steep angle works for them.
7. **Geometry conflicts with "unchanged":** the ridge foot at x 60 (15 m) rising to 95 at x 0 runs from z 0 to 300, which covers the cave ravine and mouth (x 44 to 80, z 34 to 60; ridge ground at x 52 would be about 26) and the Camp 3 west rim (x 58 to 66 at 4 m becomes 15 to 18). The draft must bend the foot round both or they are not unchanged. Hurts.
8. **Checks cannot prove it as specced:** Main3.md 2.11 off-map hills have no colliders. If the north, south and east ridges now hide anything, W-1 style raycasts pass straight through them. Hurts.

## 2. High points that see the fire

1. **Saddle (15, 275) at 100: fail. Blocks the ledge-only claim.** Eye 101.6; the line to flame tops crosses x 0 at 103.0, 8 m over the 95 crest. The whole valley shows: from 101.6 over a 95 crest 15 m away, anything above about -37 at 300 m out is visible, so the valley floor fires show too. A "saddle" at 100 between a 115 knob and a 95 crest is a shoulder; ground falls west from it.
2. **Upper east-face switchbacks:** any trail point within 15 m of x 0 above about 93 sees over the crest. Route not drawn; unverified.
3. **Tower upper stair: pass.** Below the deck; the deck case is item 1.
4. **Camp 2 stack top: pass due west** (77.1, +3 80.1, 14.9 under). South-west over the south ridge: line at z 0 is 37.1, at z -200 about 58.5; hidden only if the south ridge is 50 or more within about 150 m. Unverified.

## 3. Climb distance and time

1. **Speed: pass.** walkSpeed 2.5 in Assets/Settings/PlayerTuning.asset. 430 m / 2.5 = 172 s; 526 m / 2.5 = 210.4 s. Arithmetic correct.
2. **Length is the floor, not an estimate. Hurts.** J (104, 206) to ridge foot at x 60: about 50 m, 10 to 15. Foot 15 to saddle 100 at exactly 24 percent: 354 m. Saddle round the knob to the ledge: 21 m straight, more round. Minimum about 425 to 440 m with every metre at exactly 24 percent and no level turn platforms. The as-built Wall climb overran paper by 5.5 percent (380 to 401). Expect about 455 m, 182 s; Camp to Ward about 551 m, 220 s.

## 4. Slope limit

1. **Pass, verified.** Assets/Prefabs/Player.prefab CharacterController m_SlopeLimit 45, stepOffset 0.1; no override in Main3.unity; no script sets slopeLimit. 24 percent is 13.5 degrees. The 8.3 trail recipe clamps grade at 0.3, also above 24 percent.
2. Note: any ridge, knob or saddle face under 45 degrees is walkable off-trail wherever the thicket does not line it. East face average from x 60 to 0 is 53 degrees, but the north, south and knob faces are not specified.

## 5. DECISIONS conflicts

1. **2026-09-29 "fire not visible at all on day one" (line 99) and "never visible on day one" (line 104): broken** by section 1 item 2.
2. **2026-09-29 "never switched off; the land hides it" (line 187):** the day 1 east wind hides the smoke by weather, not land. Grant's call. Also laid-flat smoke must stay under 155 m at 464 m from the tower, 176 at 564, 218 at 764 (west, +3 rule), and under about 70 to 90 anywhere north or south of the map. Not specified. Day 1 wind versus night 1 is unstated: if columns and glow stand on night 1, the walk to the Ward sees the fire before the ledge (Main3.md 5.9).
3. **2026-09-29 Ward on a plateau at about 70 behind a wall crest 88, climb turns back at a pass to look down on the tower (line 167), and revision 15 approval (line 171):** replaced by this draft; needs a new dated line. The look-back at a pass is not in the draft.
4. **2026-09-25 stones on the cliff edge to one side of the approach (line 37):** the draft places the stones "facing the fire" on the ledge but not to one side of the lip. Unstated.
5. **2026-09-29 Ward never visible from the tower (line 159):** holds only under section 1 item 3's knob condition.
6. **2026-09-29 layers first, scaling held back (line 168):** the map grows 40 m west. Small, but a size change; needs Grant.
7. Lines 69, 85, 94 (tower sees every place but the Ward and the cave): hold.

Marlow

## Revision 2

2026-09-29, Marlow. Sable's Valley.md and Valley_map.svg revision 2. Paper only. Map scale read from the SVG: px = 276 + 1.8 x, py = 756 - 1.8 z (tower (571.2, 457.2), knob summit (297.6, 291.6) both check). W crest from the SVG: x 0 at z -60 to 60, x 4 at z 150, x 8 at z 200, x 10 at z 230, x 12 at z 242 to 268, x 8 at z 300. Knob top rect x 8 to 16, z 242 to 268. Stones: 1 (-3.9, 268), 2 (1.1, 271.1) spanning x -0.6 to 2.8, z 269.2 to 273.1, 3 (-6.9, 272.5). Cleft: (20, 230), bend (11, 230), west mouth (2, 233). +3 rule on eye and target throughout: tower 60.6, flame tops 133, stone tops 113.

### Earlier items
1. 1.1 fire at x -300: pass, stated in 3.1.
2. 1.2 fire ends: pass. SW end (-300, -250) crosses x 0 at z 19, line 86.2 under crest 98.4 (12.2). NW end (-300, 750) crosses the N crest at (10.8, 358.8), line 84.5 under 98.5 (14). Through the W saddle to (-300, -134): 86.2 under 95, 8.8 confirmed. Every tower-to-fire line crosses the W ridge (z -60 to 360) or a hook; no gap at either corner.
3. 1.5, 2.4, camp, S1, Camp 2 SW and NW: pass. Camp (170, 160): 60.6 under 99 SW, 56.1 under 96 NW. S1: 58.4 under 99, 57.6 under 99. Camp 2: 74.7 under 92.4 (S hook x 31), 68.0 under 85.6 (N x 69). Camp 1, office, gate: 36.6 to 46.5 under 69 to 77 NW; 59 to 73 under 92 to 99 SW.
4. 1.7 foot bend: pass. SVG floor edge x 38 (z 0 to 85), x 46 (z 100 to 180), x 72 (z 195 to 300).
5. 1.8 colliders: pass as specced (W, N, S real terrain).
6. 2.1 saddle: pass, removed.
7. 3.2 length: pass. 32 + 340 + 42 + 29 + 20 + 26 = 489; /2.5 = 195.6 s; +96 = 585, 234 s.
8. 1.3 and 5.5 knob: **fail, new geometry. Blocks.** See R2.1.
9. 5.2, 5.3, 5.4, 5.6: flagged G. Pending Grant, not a pass.

### New findings
1. **Stone 2 not hidden by 3 m. Blocks** (DECISIONS 2026-09-29, Ward never visible from the tower). Tower to stone 2 centre: at knob x 8, z 266.5, line 110.8 under 113: 2.2. SE top corner (2.8, 269.2): 111.3 at x 8, 1.7. NE top corner (2.8, 273.1): the line leaves the knob's 113 zone at (10.5, 268) at 110.5, then runs to 113 over ground the doc does not give, which falls to the ledge at 98 by x 4. On paper that corner shows. Without the +3 rule it still leaves the knob at 107.5 over the same falling ground. At jump height every margin drops 0.6. Stones 1 and 3 pass (3.2, 4.1). The "3 or more" in table 4 and on the map is wrong for stone 2.
2. **Knob width stated three ways.** Table 2: x 8 to 14 (6 m) and "12 m wide at 113"; SVG rect: x 8 to 16 (8 m), rounded corners rx 6 m, which cut the 113 zone further at z 262 to 268 where stone lines cross. Hurts; R2.1 used 8 to 16 and still fails.
3. **"Every climb point 6 m or more under the crest": false; hidden by the rule, thin.** P4 (33, 204) due west over crest (8, 204) 104.1: line 99.3, 4.8. Leg 5 middle (26.5, 217) to (-300, -250): 101.1 under 104.9, 3.8. Cleft east mouth (20, 230) to (-300, -250): crosses the crest at (8.9, 213) 102.7 under 104.4, **1.7**; to (-500, -250) 2.5. No platform or climb point sees the valley floor or valley fires. Hurts.
4. **The cleft has a straight view through. Hurts.** The bend offsets the west half 3 m over 9 m (18 degrees) in a 2.5 m slot. The line z = 230.75 + 0.159 (11 - x) clears every wall by 0.5 m from (20, 229.3) to (2, 232.2) and meets the far front at about (-300, 280). Flames at 130 rise 1.7 m over the 18 m slot, inside the 97 to 105 walls. From the east mouth the fire shows down the slot 20 m before the step out. Repro: plot the slot walls from the SVG polyline at 2.5 m width.
5. **Day 1 smoke through the W saddle: 1.7 m.** Smoke tops at 3.2 (150 at x -300, 170 at x -400, 210 at x -600) all give 93.3 at the saddle (0, 60) against 95. Valley smoke at 100 at (-50, 28): 93.1, 1.9. Cleft east mouth to smoke at x -600, 213: 103.6 under 105, 1.4. The "least margin 8.8" is flames only. Hidden, but a particle sheet needs a hard top. Hurts.
6. Three far saddles: pass. N saddle (250, 350) shows the fire only to eyes above about 24 m (+3) in the wedge x 300 to 396, z 244 to 300; ground there is 3 to 5, nothing climbable. S saddle (170, -50): wedge x 300 to 440, z -50 to 65, same cut-off, ground low. E cut faces away. From the tower all three sit 50 degrees or more off the fire (bearings 25, 178, 89 against 228 to 322: confirmed). From the climb every saddle line points east or south, away from the fire.
7. **Ground west of x -40 unstated.** Terrain ends at x -40 at -40; valley fires sit at x -50 to -150 and the far ridge at x -300 to -500. From the ledge at 98 the terrain edge at x -40 is in the view down. Hurts until a backdrop or terrain is named.
8. **Leg 1 on the foot line.** The foot is x 72 at ground 12 from z 195 to 300, and leg 1 runs up x 72 to 32 at z 290: a 20 m bank on the valley side of the foot. Shape it or move the foot east. Cosmetic on paper, hurts in build.
9. 5.3 calls P4 "the widest"; 5.2 makes P4 3 x 3 m and P1 to P3 13 m across. Cosmetic.
10. Headlight glow from P3 and P4 (7.4): P4 to the cut (445, 170) runs mostly over the old burn, line 60 at x 185, likely seen. P3's line passes north of the burn at 48 at x 185 against giant tops 50: likely blocked by any giant there. Unverified until groves are placed.
11. Stone 3 footprint x -8.6 to -5.2 runs past the ledge's west edge (x -6). Cosmetic, fits "cliff edge".

Result: **FAIL.** Blocking: R2.1 (stone 2 on the knob's north end).

Marlow
