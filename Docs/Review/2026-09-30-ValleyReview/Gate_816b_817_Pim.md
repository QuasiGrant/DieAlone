# Gate step 4, PLAN 8.16b and 8.17, Pim, 2026-10-01

Source: Docs/Captures/Main3Review_816b (capture 2026-10-01 11:47): index.md, Checks.md, Trail_* sheets, Pump_FWD50_Full, Climb_J_to_Ward, Trail_Ends, Places, Markers, GateT_Zoom, Pairs_DayOne_Night. Code read: Assets/Scripts/Dev/DevWarpLabels.cs, Tools/Recipes/main3_8_17_camp2.cs, main3_8_17_camp3.cs, main3_8_15_ground.cs.
Rules: Gate.md step 4 as written; W3 with Wren's call that the Snag on Camp to Camp 3 is Camp 3's landmark.
Method limits: no shell, no Editor. Sheets read shrunk; frame calls by eye. The night rule is 8.18 and is not judged here.

**Verdict: FAIL.** 8.16b: Camp 2 to T fwd 0 unverified, W3 at 7 (4 if the cleft is exempt). 8.17: the ruin and Camp 3 cannot be found from their trails; the dev panel test was not rerun after its warp list changed.

## 1. 8.16b rechecks (open items of Gate_816a_Pim.md)

| Item | Was | Now |
|---|---|---|
| W1 day, Camp 2 to T fwd 0 | FAIL, no band leaves in view | **OPEN, unverified.** Fwd 0 frame unchanged: red duff, grey domes, dark wedge left, no band; the new StartBlaze (camp2.cs line 105: post 6 m along, 1.8 m right) is not readable in it at 1/3. Trail_Ends "Camp 2 to T end (299, 108)" shows the band leaving left and a dark post right of it, likely the blaze; its pale band is not readable either. The two frames stand at the same point and face different ways. Rook: say why, add a full-size fwd 0 and the blaze to Markers.jpg |
| W3, Camp to pump fwd 50 | check at full size | **PASS.** Pump_FWD50_Full: at the trail's vanishing point a pale water glint at the trench mouth and a small dark shape on a lit flat (the pump side). Small, but on the line, and fwd 60 confirms water |
| W3, Camp to pump fwd 40 | FAIL | **FAIL unchanged.** Trench wall and a boulder |
| W3, Camp to Camp 3 fwd 80 | FAIL | **PASS** by Wren's call: the Snag's trunk fills frame centre |
| W3, Camp to Camp 3 fwd 50 | FAIL | **PASS by position.** The Snag (96, 146.5) is about 25 m ahead on the line; a grey trunk sits left of centre. Not confirmed by name |
| W3, W1 to Camp 3 fwd 80, 90 | FAIL | **FAIL unchanged.** Creek banks and rock; no Camp 3 sign of life (see 2.1, Camp 3) |
| W3, Climb fwd 50 | FAIL | **FAIL unchanged.** Open 1; landing step fills the frame, lantern at left |
| W3, Climb fwd 210, 220, 230 | FAIL | **FAIL by the rule, proposed exempt.** These are the cleft (Valley.md 1.6: cleft 29.5 m then ledge 17 m before the path end, so about 209 to 239 m). The design hides the ledge there for the reveal (F-1 exempts only past the fin end); the capture already exempts cleft frames from the rock bar. I would exempt them from W3 the same way. Wren and Grant decide |

W3: **7** (pump 40; W1 to Camp 3 80, 90; climb 50, 210, 220, 230). **4** if the cleft is exempt.

## 2. 8.17 wayfinding

### 2.1 Can each place be found from its trail

| Place | Trail and frames | Call |
|---|---|---|
| Pump and dock | Camp to pump fwd 60 to 90: water, then dock rail and signpost | PASS |
| Boathouse | Pump to boathouse fwd 0 (small, right), 50 to 80 | PASS |
| Camp 2 | Boathouse to Camp 2 fwd 30 to 80: stack, payphone booth and lamp | PASS |
| Office, store, front | Camp 2 to T fwd 80, 90; Trail_Ends Camp 2 to T start: office row and lot | PASS |
| Camp 1 | Jg to Camp 1 fwd 60 to 80: tables, chairs, stump; Camp 1 to J back 200 to 230. The spar bulbs (Valley.md 1.3) are not seen | PASS |
| Cave mouth | W1 to cave fwd 100: the board between boulders. Spur unsigned by design | PASS |
| Ward stones | Climb fwd 240, 250 | PASS |
| **North ruin** | Camp 1 to J fwd 50 to 140 and back 90 to 180: trunks only. Places "from the loop warp" and "at 20 m": not readable | **FAIL.** Valley.md 1.3 asks for the pale roof slab and leaning stovepipe 30 m ahead under a gap of light, and a short side path to the doorway; Valley.md 11 makes the stovepipe the ruin's only marker. None reads. Whether the side path is built: unverified (index says R-1 leaves it out). The RuinScreen is south of the loop, so it should not be what hides it |
| **Camp 3** | Camp to Camp 3 fwd 90, 100; W1 to Camp 3 fwd 100, 110; Pairs "Camp 3 warp" day and night: rock hollow, an A-frame, a red sign, a plank | **FAIL.** No tent, fire or seat log in any frame. camp3.cs line 29 places CS_Tent_Old_2 at (80.5, 153), 8 to 13 m in front of the warp, which faces (90, 158); the warp pair should show it. Either the capture predates the rebuild or the tent is hidden. Rook to say. At night no fire glow in the pair either |

### 2.2 "North loop: ruin" warp and the dev panel task test

- Warp: **PASS.** North_Loop_Ruin is in Main3; DevWarpLabels.cs line 33 lists "North loop: ruin" in CAMPS after the burn fork, per Valley.md 14. Marlow's 8.17 walk: the doorway is the first hit from the warp, 12.4 m. That the ruin does not read from it is the place fail above.
- Dev panel test: **OPEN, not rerun.** The menu was touched (one row added), so Gate.md 4 needs the run. Only Docs/Captures/Main3Review/DevPanel_Presses.txt exists (8.16a). By code the new row sits below TIME, the Ward row and WARD CLIMB, so night 2, Ward 3 and J at night 5 presses should hold. Rook: rerun Tools/Recipes/dev_panel_8_11_check.cs at 3840 x 1976 into Main3Review_816b.
- For the record, not a gate task: the ruin warp is 15 presses (F1, 13 Down, Enter; Page Down saves one Down and costs one press). Grant has not named it.

### 2.3 Signs and markers at the places

| Marker | Call |
|---|---|
| Sign Camp | PASS. CAMP 3, LAKE, SPRING, LOT (ground.cs line 414). Valley.md 11 says "Spring and north loop", "Burn and lot"; the short words read |
| Sign Pump, Sign Jg, Sign J and cairn | PASS |
| Blaze Camp 1 stump | PASS |
| Gate T | PASS. GateT_Zoom: stop sign red face across the road; booth and gate from the lot |
| Trailhead board | Unverified. Post seen from 5 m; board face not readable at 1/3 |
| Blaze W1 Camp 3 | Unverified. Post seen; pale band not readable at 1/3 |
| Camp 2 start blaze | Not in Markers.jpg (see 1) |
| Office RANGER STATION, store ICE and lit sign, cave CLOSED - UNSAFE | PASS |
| Ruin | None by design; the stovepipe is the marker and does not read (2.1) |

## Counts for Wren

- W1 day by eye: 1 open (Camp 2 to T fwd 0), needs Rook's full-size frame.
- W3: 7, or 4 with the cleft exemption (decision for Wren and Grant).
- 8.17 found from trail: 2 FAIL (north ruin, Camp 3).
- Warp: PASS. Dev panel: rerun needed.
- Markers: 2 unverified (trailhead board, W1 blaze), 1 missing frame (Camp 2 blaze).

Pim
