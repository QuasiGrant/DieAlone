# Check 2, UI half: dev panel and wayfinding in Main3 (Pim, 2026-09-30)

Sources: sheets/ (all 20), shots/f1_*.png, Facts_Rook.md, Meeting_Pim.md, DevMenu.cs, recipe main3_8_2_camp_tower.cs. Day one frames only, except the 6 night pairs. Stop grades are by eye on 1/3-size frames, so counts are "about".

## 1. Dev panel: FAIL
- What Grant sees: F1 opens on SCENES with `Graybox` selected, and the visible rows end at `Store` (f1_open). There is no scrollbar and no "more below" mark. KeepSelectionVisible pulls the wheel back every frame, so it stops at `Junction Jg` (f1_scrolled). The mouse cannot reach `LOOK` (Night, Day one, Day two) or `Ward` at all. The header says LOOK, not day or night. Navigation is Vertical with no wrap, so Up from the top does nothing.
- Night: F1, 26 Down, Enter = 28 presses (Rook measured 27 Down). Pad: View, 26 d-pad Down, A = 28. The limit is 5. FAIL.
- Ward (row 23): F1, 22 Down, Enter = 24. Pad: View, 22 Down, A = 24. FAIL.
- Fix spec for the coder:
  1. Order: title and one hint line, then TIME, WARPS, and SCENES last.
  2. TIME is one row, `Time: Day one  < >`, focused on open. Left/Right (arrows, d-pad, stick) steps Day one, Day two, Night and wraps. The change applies at once, with no Enter.
  3. The first warp row is `Ward: stones on the ledge`. The rest follow in route-order groups (Meeting_Pim 2), with the section 3 labels.
  4. The wheel scrolls freely, and selection moves only on key or pad input. Draw a scrollbar and a "more below" mark whenever rows are off screen.
  5. LB/RB and Page Up/Down jump between section heads.
  6. Optional: F2 cycles time without opening the panel. This needs a DECISIONS line (it reverses 7.9).
- After the fix: Night takes 2 presses (F1, Left; or View, d-pad Left). Ward takes 3 (F1, Down, Enter; or View, Down, A). Pass test: TIME and the Ward row are on screen at open, with no scrolling, at 3840 x 1976 and at every 8.9h size.

## 2. Wayfinding against W1 to W4
- W1 (the path is brighter or darker than the ground): 0 of 13 trails pass. Trail and ground are the same bare dirt (Rook 4).
  - Where a route reads at all, it is from the shape around it: rock-lined trenches (CAMP TO PUMP FWD 20 to 60, PUMP TO BOATHOUSE FWD 20 to 40, J TO WARD legs) or asphalt at the lot.
  - Worst: JG TO T FWD 40 and BACK 30; JG TO CAMP 1 FWD 0 to 20; CAMP 2 TO T FWD 20; CAMP TO JG FWD 50 and 90; W1 TO CAVE FWD 0 to 70 (dark brown, no cue at all).
  - Night: the ground is black in the J and Camp pairs. No night frames were taken on the trails, so night per trail is unverified.
- W2 (every stop has a visible cause): about 34 of 89 pass and about 55 fail.
  - The passes are rock or steep face: S1, S11, S16, S17, S31 to S34, S36 to S47 (the climb), S61 to S64, S69, S71, S73, S81, S85 to S87, S89.
  - Worst are open ground facing nothing: S2 to S5 (meadow), S18 to S22 (Camp to J), S74 to S80, S84, S88. In the burn, all of S25 to S29 and S48 to S60 fail.
  - This counts only stops facing the walker. About 970 more unmarked wall pieces run along the trail sides (Rook 3).
- W3 (no path fades out): all 13 trails end at a place or a junction (Trail_Ends, Rook 5). 3 path-shaped cuts fail: the pump dock notch (W4 LAKE_PUMP N), the Camp 2 view cut, and the creek channel. Walls cross all three, with no visible end.
- W4 (every junction has a marker): 1 of 6 passes. J has its cairn (CAMP TO J FWD 70). Camp, Lake pump, W1, Jg and T have nothing (CAMP TO JG START, PUMP TO W1 START, JG TO T START). W1's cave spur stays unsigned by design (Check1 row 14), but its Camp 3 branch needs a mark.

## 3. Warp names Grant cannot read, with plain labels
- Junction Jg: Burn fork (to Camp 1 and the lot)
- Junction J: Ward trail start (cairn)
- Junction W 1: Lake west fork (Camp 3, cave)
- Trailhead T: Parking lot trailhead
- Ward P 3: Ward climb, third bend
- Ward P 4: Ward climb, top of the legs
- Ward: Ward: stones on the ledge
- Camp 3 Rim: Above Camp 3
- Camp 2 Top: Camp 2, top of the stack
- Keepers Camp: Keeper's camp (tower foot)
- Cabin: Cabin (wake spot)
- Also, "W1" names both a warp number and a junction on the sheets.

## 4. On waking
- The player spawns in the bunk facing the closed south door (yaw 180, recipe 8.2). The tower is 17 m west, out of view. Whether it shows through the west window is unverified.
- The `Climb the tower` objective line (Objective.md) is not built: the string is nowhere in Assets.
- Outside, the tower dominates the clearing (Camp pair). So the first destination is not clear on waking, and clear about 5 s later. Fix: build the wake line.

## Five worst, ranked
1. **The dev panel's time and Ward rows take 24 to 28 presses and the mouse cannot reach them.** Fix: the section 1 spec and the section 3 labels.
2. **No trail reads as a path (W1: 0 of 13).** Fix: ground cover (grass, needles, litter) up to a clear edge, a lighter bare trail, and rock, root or log edge breakers every few metres. Check a grayscale eye-height frame per trail in day one, day two and night; the edge must show 20 m ahead.
3. **About 55 of 89 stops have no visible cause, worst in the burn and meadow.** Fix: give each one a cause at eye height (deadfall, boulder, brush 1 m or taller) or remove the wall. Re-run Stops_1 and Stops_2 until every frame shows its cause.
4. **5 of 6 junctions are unmarked.** Fix: signposts at Camp, Lake pump, Jg and the lot trailhead, a cairn or blaze on W1's Camp 3 branch only, and J keeps its cairn.
5. **3 path-shaped cuts have no end: the pump notch, the Camp 2 view cut and the creek.** Fix: fill each, or give it a visible end (reeds and a rod rest at the pump, deadfall across the view cut, water in the creek).

Pim
