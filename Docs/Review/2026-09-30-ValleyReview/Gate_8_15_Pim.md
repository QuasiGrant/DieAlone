# Gate step 4, PLAN 8.14a plus 8.15 (Rook d5360c4), Pim, 2026-09-30

Source: Docs/Captures/Main3Review (capture 12:17): index.md, Checks.md, DevPanel_Presses.txt, InvisibleColliders.md, Markers, Stops_1, Warps_NESW, Walk_Views, Pairs, trail and climb sheets, grey sheets and their tables. Code read: Tools/Recipes/main3_8_15_ground.cs (sign text).
Method limits: no shell, no Editor. Sheets are read shrunk, so frame calls are by eye; sign text is checked in code.

**Verdict: FAIL.** 8.15's done-check needs W1 to W4 at zero. W1 day 5 fail, W1 night 14 fail (no night rule yet), W2 off-trail unproven, W3 7 frames, W4 1 fail and 1 unverified. Dev panel, IW3 size, near-trail stops, junction sign text: pass.

## 1. Rechecks of Gate_8_14a_Pim.md FAILs

| Item | Was | Now |
|---|---|---|
| Dev panel task test | PASS by code | PASS by Play test (DevPanel_Presses.txt, 3840 x 1976): night 2 presses keyboard and pad; Ward 3; J at night 5 keyboard and pad; 0 failures |
| "North loop: ruin" warp | FAIL | Closed for this gate: moved to 8.17 with the ruin (Status.md 2026-09-30). Not a DECISIONS line; Wren to confirm it needs none |
| IW3 size | unverified, listed 1 x 1 x 1 | PASS. Bounds x 386 to 396 (index.md group table) |
| Hedge boxes S3, S12, S13 | face open ground | S12, S13 PASS: bush at frame centre. S3 PASS narrow: the bush's left edge sits at frame centre. Marlow's hand push of S3 stands |
| Hedge boxes off trail | 363 unverified | **Still unverified**, now 389 (Ground815/Stops). No footprint-under-brush report. Hedge_Burn_0 still has a 29.1 x 8.3 x 10.5 m bounds box (line 91) |
| Markers not framed (Camp, W1, Camp 1, Gate T) | 4 unverified | See section 5 |
| Next place, 9 frames | FAIL | See section 4: 7 |

## 2. W1, trail grey by day (index.md, Trail grey, Day one)

9 of 14 pass at 5 m and 20 m. The five fails all pass at 5 m (J to Ward at exactly 20) and fail at 20 m.

| Trail | Trail 20 m | Floor 20 m | Diff | Cause (by eye from the colour and grey sheets) | Fix |
|---|---|---|---|---|---|
| Camp 2 to T | 60 | 48 | 12 | open burn: the floor beside is sunlit pale dry grass, near the band's value at distance; the band drops from 99 at 5 m to 60 at 20 m | darken the burn floor cover along this leg (grass cards and base layer) by about 15 grey, or raise the band's value on the burn legs |
| Jg to T | 35 | 38 | -2 | the band runs between regrowth brush and snags; at 20 m it lies in their shade and the band goes darker than the floor | pull brush back 1 m from the band edge so the band takes sun, and a paler dirt on the burn legs. Rook: confirm the 20 m floor sample lands on ground, not on brush leaves |
| Jg to Camp 1 | 61 | 56 | 5 | the Camp 1 clearing: pale grey-green grass both sides reads as light as the dirt (fwd 30 to 80) | darken the clearing grass value by about 20, or a darker duff strip 0.4 to 1 m each side of the band |
| W1 to Camp 3 | 26 | 23 | 3 | the whole leg sits in the west face's shade; dirt and floor both go dark | a paler dirt layer on shaded legs (target band 45 or more at 20 m in shade). Edge stones do not count toward this rule |
| J to Ward | 40 | 31 | 9 | the tread is rock and scree beside rock; on the ledge the floor is pale sand like the tread. 5 m reads exactly 20 | a paler worn tread (d5360c4's is +9 short), darker scree beside it; on the ledge, darker ground cover beside the sand path |

Every trail still loses contrast from 5 m to 20 m (drop 5 to 42). Cause unverified: day fog (20 to 200 m) lifting the floor, or the terrain texture blurring at grazing angle. Rook: one frame per failing leg with fog off at 20 m splits the two. If it is fog, one band value change fixes all five.

## 3. W1 at night: proposed rule (Vesper proposes in parallel)

Night trail and floor both read 2 to 22 (index.md, Trail grey, Night). Ambient near black (Style.md 6.2.1) means no texture can pass. A texture rule at night tests the wrong thing.

What the player needs at night, from the design:
1. Only one walk exists: camp to J to the Ward (DECISIONS 2026-09-28; Valley.md 9.6; LookBoards 10). The other 12 trails are not night routes and are not measured at night.
2. **Where to go:** a light to walk toward in view from every point. Camp: the fire and the J sign are behind; the cairn lamp at J is the goal (Logbook.md 4.4, "the cairn path, at dark"). Climb: the rune post ember at 10 m, then the fire glow over the crest.
3. **Where the edge is, near:** in 8 to 60 m night fog the player steers by the next few metres. Pale edge stones (Valley.md 12.1) do this only if something lights them. At 2 to 22 grey they are as black as the floor.
4. On the climb the ring walls hold the player (Checks.md CLIMB, LEDGE: 0 fail), so the edge can be lost only on Camp to J and the chute approach.

Proposed rule, for Camp to J and J to Ward, night look, filter on, frames every 10 m facing along the trail:
- **N1, the next light:** a practical light or lit marker (cairn lamp, rune post, fire glow over the crest) in frame, its patch 40 grey or more above the frame mean. The cairn lamp counts from camp to J; the climb counts from the chute on.
- **N2, the edge at 5 m:** the trail edge (band or edge stones) 20 grey or more from the floor beside it, 5 m ahead. No 20 m test at night; the fog owns that distance.
- N2 needs a light near the player. Two ways: a carried lantern (LookBoards 10 already says "a lantern radius"; a new system, Grant decides) or lit markers along the 80 m to J and the chute approach (practicals, Style 6.2.3). My pick: the carried lantern, about 6 m radius. It makes the day band count again near the feet, it matches the lookout job, and it needs no new UI.

Seen, for Vesper: sign arm labels read white at night (Grey_Trails_Night, Pump to boathouse 0 m). If the label material is unlit, it breaks "practical lights only". The J night pair shows the cairn lamp lit and nothing else of the trail.

Until Grant rules, W1 night stands at 14 fails, not measurable.

## 4. W3, next place in view

| Leg | Frame | Now | Fix |
|---|---|---|---|
| Camp to pump | fwd 40 | FAIL, trench walls both sides, no lake | a notch in the east bank at the bend, or the pump lantern (lit by day, Style 6.1.5) on a post that tops the bank |
| Camp to pump | fwd 50 | PASS, water glint | |
| Camp to Camp 3 | fwd 50 | FAIL, trunks and the ridge face | clear the trunk left of the line so the Snag top (54) frames. Fwd 10 to 30 bare snag still UNVERIFIED as the Snag |
| W1 to Camp 3 | fwd 60, 70 | PASS, the camper; the hollow rim with the log steps (by eye) | |
| W1 to Camp 3 | fwd 80, 90 | FAIL, creek banks only | lower the Camp 3 side bank so the rim or the Snag top shows |
| Climb | 50 | **New** FAIL, rock step fills the frame, no sky. The seep (Valley.md 1.6 leg 1 head) is not seen | the seep at frame centre, or a sky strip over the chute head |
| Climb | 210, 220, 230 | FAIL, unchanged: the cleft is rock on all sides | Valley.md 1.6 says "a strip of sky": open the cleft top so sky shows at level gaze |

W3: **7 frames** (was 9; 2 fixed, 1 new). Seen, not scored: climb 160 has a fir filling the frame at arm's length.

## 5. W4, junction markers (Markers.jpg, 5 m back; text from main3_8_15_ground.cs)

| Marker | In frame | Labels |
|---|---|---|
| Camp four-arm | PASS, all four arms | CAMP 3, LAKE, SPRING, LOT. Spec (Valley.md 11) says "Spring and north loop", "Burn and lot". One word fits the 1.5 m arm; I accept SPRING and LOT. Wren: amend Valley.md 11 |
| Pump | PASS | CAMP, BOATHOUSE, WEST SHORE, match |
| Jg | PASS; fir foliage covers the post's lower half | CAMP, LOT, CAMP 1 in code (was CMP1), match. LOT points away from this side, as expected |
| T trailhead board | PASS, at the right edge | VALLEY TRAILS faces the lot (Jg to T fwd 100) |
| J | PASS | NORTH LOOP, CAMP, match; cairn beside |
| W1 blaze | PASS narrow, small pale band on a post left of centre | none by design |
| Camp 1 stump blaze | **FAIL**: a grey slab stands between the trail and the stump at frame centre; the blaze cannot be read | move the slab or frame from the loop side |
| Gate T set | **UNVERIFIED**: the frame is from the lot through the gate; stop sign, entrance sign and mailbox (about x 425) are not identifiable at sheet scale | Rook: a full-size crop of that frame |

W4: 6 of 8 pass, 1 fail, 1 unverified. Ruin is 8.17.

## 6. W2, stops

Rule (Gate.md 4): every stop across the line of travel covers the full trail width.

| Group | Call |
|---|---|
| Hedge stops near trails, S1 to S17, S35 to S44 | PASS. All 1.6 to 4.2 m from the centre line; the band edge is 1.1 m (0.7 plus 0.4 blend), so no hedge box sits in the trail and none half-blocks it. Brush in every frame |
| S18 CairnGate/GateBlocker | PASS. Fills the 3 m chute gap (z 211.3 to 214.7), full width; the Ward path by day is an allowed wall |
| S19 to S21 ChuteSteps ramps | walkable, not stops |
| S22 to S34 climb rim | PASS. 1.5 to 4.8 m beside the tread, rock in every frame |
| 389 Ground815/Stops boxes off trail | **UNVERIFIED.** Needs Rook's footprint-under-brush percent per box, or a top-down with the boxes drawn over the brush |

W2: 0 fails near trails; off-trail boxes unproven, so W2 cannot read zero yet.

## 7. Warps clear of bushes (Warps_NESW, by eye, sheet shrunk)

W4, W10, W16 (named in 8.14a) now PASS: no foliage fills a frame. W23 Old_Burn N and W: regrowth brush fills the lower half at arm's length, suspect. Rook: print the nearest renderer distance per warp and direction; under 1.5 m fails.

## Counts for Wren

W1 day 5 fail (Camp 2 to T, Jg to T, Jg to Camp 1, W1 to Camp 3, J to Ward). W1 night 14, blocked on the night rule. W2 0 near trails, 389 unverified. W3 7. W4 1 fail (Camp 1 stump), 1 unverified (Gate T). Warps: W23 suspect.

Pim
