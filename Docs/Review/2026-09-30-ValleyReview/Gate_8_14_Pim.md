# Gate step 4, PLAN 8.14 (Valley rev 10 ground and road), Pim, 2026-09-30

Source: Docs/Captures/Main3Review (Rook, 2026-09-30 10:12), DevMenu.cs and DevWarpLabels.cs at HEAD 3b61464, Valley.md rev 10.
Stage rule (Wren): trail contrast (W1) and visible stops off the climb belong to 8.15. W1 is recorded as a baseline, not failed.
Method limits: I have no shell and no Editor. The dev panel test is read from code at Grant's Game view (3840 x 1976, Check2_UI.md), not run in Play. Rook's recipe Tools/Recipes/dev_panel_8_11_check.cs is the Play proof. Frame calls are by eye from the sheets.

**Verdict: FAIL.** The two Gate tasks pass. Fails: next place hidden on 19 frames (item 3); junction markers mostly missing and owned by no build (item 2); dev panel does not match Valley.md 14 (item 1b). "Only IW1 to IW3" is unverified (item 4).

## 1. Dev panel task test

a. **PASS, Gate tasks.** All 25 warps are listed in DevWarpLabels.Rows (24) plus the Ward row; none fall to OTHER. At 1976 high the canvas scale is 2.74, and TIME and the Ward row sit in the top third of the column with no scrolling.

| Task | Keyboard | Pad | Presses |
|---|---|---|---|
| Day one to night | F1, Left | View, d-pad Left | 2 |
| Night to day one | F1, Right | View, d-pad Right | 2 |
| Warp to the Ward | F1, Down, Enter | View, d-pad Down, A | 3 |

Note, not a fail: the TIME row steps through every look. The 8.12 tip adds 14 lighting options after Day two. From a middle option (for example V5), night takes about 8 presses. From the start look it takes 2.

b. **FAIL against Valley.md 14:**
- Renames done: Ward_P3 is "Ward climb, burned cwm" and Ward_P4 is "Ward climb, look-back". "Lot, facing the highway" (Lot_Highway) is added.
- Missing: "North loop: ruin". There is no warp, no ruin, and no north loop trail (item 2).
- Not done: WARD CLIMB still comes after TOWER AND CAMP. J at night is F1, Left, Down x5, Enter = 8 presses (pad: View, Left, Down x5, A). The spec says 5. Fix: move the WARD CLIMB group to sit directly under the Ward row. That makes J the 2nd warp: F1, Left, Down, Down, Enter.
- Not done: Junction_Jg is still last in FRONT. The spec says it follows Camp 1 in CAMPS.
- New places: P3, P4, the Ward ledge and the highway view (Lot_Highway) have warps. P1, P2 and the cleft do not, and the spec does not ask for them.

## 2. Junction markers (W4)

Valley.md 15 hands 8.15 sections 8, 9 and 12, and 8.17 section 6. **Section 11, the junction markers, has no build.** Wren to assign.

| Junction | Spec | Seen in capture |
|---|---|---|
| Camp (165, 158) | four-arm signpost | absent (Camp to pump fwd 0, Camp to J back 70, Camp to Camp 3 back 100) |
| Pump (193, 99) | signpost | grey post only (Pump to boathouse back 70 and 80); no arms |
| Jg (262, 172) | signpost | absent (Camp to Jg fwd 100, Jg to T back 100, Jg to Camp 1 back 80) |
| T (338, 170) | trailhead board | unclear; a pale post at Jg to T fwd 80 |
| W1 (128, 70) | blaze post, Camp 3 branch | absent (Pump to W1 fwd 70, W1 to Camp 3 fwd 0) |
| J (104, 206) | cairn; signpost for Camp and north loop | cairn PASS; signpost absent |
| Camp 1 (272, 246) | blaze where the loop leaves | no loop leaves; north loop trail not built (Map_TopDown: Camp 1 trail ends at W6) |
| Ruin | stovepipe | not built |
| Gate T | stop sign, entrance sign, mailbox | not in any frame; unverified |
| Climb | none, landmarks | legs 1 and 2 have the knob and shelf; seep, rune post, split snag and bent fir are not built (8.17 props, not in 8.14 scope) |

The new terrain adds no unmarked fork I can see on the map. The cave spur's rope rail (W1 to cave fwd 30) is in the spec.
Baseline W4: 9 of 9 junctions incomplete. The J cairn is the only built marker; the pump is partial and T is unclear.

## 3. Next destination in view (W3)

PASS legs: Boathouse to Camp 2 (the stack in every frame), Camp 2 to T (red mast), Jg to T (mast, lot), Jg to Camp 1 (spar from fwd 20), Camp to Jg (mast through the burn at fwd 40 to 50), Camp to J (cairn and the rock band gap), climb 0 to 110 (chute, then the knob over the shelf). Every back frame shows the tower or the lake.

FAIL, 19 frames:

| Leg | Frames | What fills the view |
|---|---|---|
| Camp to pump | fwd 40, 50, 60 | switchback trench banks; no water, tank or pump |
| Pump to boathouse | fwd 0, 10 | shore bank; boathouse first at fwd 30 |
| Pump to W1 | fwd 0 | knoll bank |
| Camp to Camp 3 | fwd 10 to 50 | rolling ground and ridge; no Snag or hollow until fwd 80 |
| W1 to Camp 3 | fwd 60 to 90 | creek banks; Camp 3 first at fwd 100 |
| Climb | 210, 220, 230 | rock in the whole frame, no sky strip; the cleft does not read as a way through. Marlow's walk (2.10) confirms or clears it |
| J at night (Pairs) | 1 | the cairn is not lit and the chute gap cannot be seen. The one night walk has no lead-on |

Accepted by design: W1 to cave fwd 40 to 70 (the spur is meant to show nothing; the mouth shows at fwd 80).
Fix owner is Wren's call. Terrain frames (trench, banks) are 8.14 ground. The cairn lamp is Vesper and Rook.

## 4. Invisible walls and stops

- **S1** CairnGate/GateBlocker at (86, 213) is IW2, allowed. It lies across the chute mouth, 0.2 m from the centre line. The chain is faint in the S1 frame. Marlow judges it from 2 m (2.2).
- **S2 to S4** ChuteSteps/StairRamp are the walkable ramps under the visible cut steps. They lift the player and do not stop him. PASS. Rook: exclude walkable ramps from the stop scan.
- **Climb stops have a visible cause: PASS from frames.** From the chute (20 to 50) through the shelf (60 to 110), the cwm (130 to 160) and under the wall (170 to 200), rock meshes stand on both sides. The shelf drop has a visible rim (60 to 110, right).
- **Only IW1, IW2 and IW3 exist: UNVERIFIED.** The scan covers only 5 m either side of 13 trails, and it leaves out walls along the trail sides. No trail runs through the lot, the drive or the spur, so IW1 and IW3 never appear. A scene-wide list of renderer-less colliders, with positions, is needed (Rook).
- Full trail width: no trail exists yet (8.15), so it cannot be measured. The IW2 gap is 3 m per spec.

## 5. W1 baseline (not a fail, 8.15)

No numbers. The sheets are colour and day one only. There are no grayscale or night trail frames, and I cannot read pixel values without a shell. By eye, no trail band shows in any day frame on any of the 13 trails, so the trail-minus-floor difference is about 0. Exceptions: the chute's stone steps and the lot asphalt, which are not trail.
Baseline: 13 of 13 trails fail at 5 m and 20 m, day. Night is not captured.
For 8.15's done-check, Rook's recipe must add grayscale day and night trail frames and print the mean grey of the trail and of the floor beside it, 5 m and 20 m ahead.

Baselines for 8.15 (W1 to W4): W1 13 of 13 trails; W2 0 invisible stops found near trails (scan limits above); W3 19 frames; W4 9 of 9 junctions incomplete.

Pim
