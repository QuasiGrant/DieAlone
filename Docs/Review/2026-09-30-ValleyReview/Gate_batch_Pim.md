# Gate step 4, batched build (PLAN 8.16b, 8.17, 8.18), Pim, 2026-10-01

Source: Docs/Captures/Main3Review_batch (capture 2026-10-01 14:28, noise band off): index.md, Checks.md, Markers, Trail_Ends, Trail_Camp_2_to_T, Trail_Camp_to_pump, Trail_Camp_to_Camp_3, Trail_W1_to_Camp_3, Trail_Camp_1_to_J, Climb_J_to_Ward, Places, Pairs_DayOne_Night, Night_Rule_Frames. Code read: main3_8_17_camp2.cs (StartBlaze), main3_8_17_camp3.cs, main3_8_15_ground.cs (Blaze, Trailhead_Board), main3_8_7_ward.cs (cairn flame card), main3_8_18_look.cs (lamp 6, range 9).
Rules: Gate.md step 4 as written; N1/N2 counted only where the line can be lost (ring walls and cleft exempt, as in Gate_816a_Pim.md).
Method limits: no shell, no Editor. Sheets read shrunk; frame calls by eye.

**Verdict: FAIL.** 8.16b: Camp 2 to T fwd 0 still open, W3 at 7 (4 with the cleft exempt). 8.17: ruin and Camp 3 still not found from their trails; dev panel not rerun. 8.18: night N2 fails 11 counted frames.

## 1. Open items from Gate_816b_817_Pim.md

| Item | Was | Now |
|---|---|---|
| W1 day, Camp 2 to T fwd 0 | OPEN, blaze unreadable | **FAIL unchanged.** Same frame: boulders, red duff, dark wedge left, no band and no blaze post in view. StartBlaze (camp2.cs 121, 6 m along, 1.8 m right) is either behind the eye's facing or out of frame. Trail_Ends "Camp 2 to T end (299, 108)" faces the stack wall with a post at left; the two frames still disagree on which way the start faces. Rook: one full-size fwd 0 and the blaze from 5 m |
| Camp 2 blaze in Markers | missing | **Still missing.** Markers.jpg has 8 frames, none for StartBlaze |
| W3 Camp to pump fwd 40 | FAIL | **FAIL unchanged.** Rock bank and a mossy boulder. Fwd 50 now shows water at sheet scale: PASS |
| W3 Camp to Camp 3 fwd 50, 80 | PASS | PASS (the Snag trunk on the line) |
| W3 W1 to Camp 3 fwd 80, 90 | FAIL | **FAIL unchanged.** Rock wall, then boulders in the slot; Camp 3 first shows at fwd 110 (tripod and easel) |
| W3 Climb fwd 50 | FAIL | **FAIL unchanged.** Landing step fills the frame, open 1 |
| W3 Climb fwd 210, 220, 230 | proposed exempt | **Exempt only if Wren and Grant agree.** The capture now labels 220 and 230 "cleft, exempt" (rock bar only) but 210 FAIL. By Valley.md 1.6 the cleft starts about 209 m, so 210 is cleft too. Rook to say where the capture's cleft starts |
| Trailhead board | unverified | **PASS.** Trail_Ends "Camp 2 to T start (340, 169)": the board face reads at sheet scale. It prints "ALLEY TRAIL"; the first letter looks cut by the board edge. Check at full size. Markers frames the board edge-on: the aim is wrong, not the board |
| W1 Camp 3 blaze | unverified | **Unverified.** Post seen in Markers and in W1 to Camp 3 back 100 with a pale top; the band does not read at 1/3. Needs a full-size frame |

W3: **7** (pump 40; W1 to Camp 3 80, 90; climb 50, 210, 220, 230). **4** if the cleft is exempt.

Seen, not mine to grade: W1 to Camp 3 fwd 0, 20, 60, back 30, 80 show untextured grey boxes by the trail. Marlow and Vesper.

## 2. 8.17 found from trail

| Place | Call |
|---|---|
| North ruin | **FAIL unchanged.** Places now shows stone walls, a doorway and the stovepipe (from the north). From the loop: Camp 1 to J fwd 50 to 140 and back 90 to 180 show trunks only; "North ruin from the loop warp" and "at 20 m" do not read. Valley.md 1.3 and 11 need the roof slab or stovepipe in a loop frame |
| Camp 3 | **FAIL, narrower.** The fire is in now: tripod and pot in Camp to Camp 3 start, W1 to Camp 3 fwd 110, Places. No tent in any frame. camp3.cs 29 places CS_Tent_Old_2 at (80.5, 153), about 13 m from the warp (74, 142) and about 14 degrees off its facing to (90, 158), so the Camp 3 pair should show it. Rook: is it in the runner's Missing list, or under the ground? Night pair: one small glow high up (Snag lantern); the fire is cold by design |

## 3. Dev panel task test

**OPEN, not rerun.** No DevPanel_Presses.txt in Main3Review_batch and nothing in Checks.md; the only file is Main3Review (8.16a), before the ruin row. Rook: run Tools/Recipes/dev_panel_8_11_check.cs at 3840 x 1976, keyboard and pad, into Main3Review_batch.

## 4. Night rule (8.18)

Counting rule: J to Ward 60 to 200 m sit inside the ring walls (ClimbRim S28 at 59 m to S41 at 215 m), 210 to 230 in the cleft; the line cannot be lost there. The chute (10 to 50) and the ledge (240, 250) count.

| | N1 counted fails | N2 counted fails |
|---|---|---|
| Camp to J (8) | 0 | **7**: 0 (11), 10 (15), 20 (13), 30 (12), 40 (10), 50 (15), 70 (17). Only 60 passes (22) |
| J to Ward (26) | 0 (60, 120 to 150 are inside the ring) | **4**: 10 (13), 40 (14), 240 (17), 250 (11). 20 unmeasured |

N2: **11 FAIL.**

### What N2 needs from the lamp

- The gap is set by the lamp, not the ground. Grey_Trails_Night, Camp to J at 5 m: trail 21, floor 6. Day is 93 and 32, the same ratio (about 3:1). Light scales both, so the gap scales with the light.
- Target: the worst counted frame (Camp to J 40, gap 10) must double; the mean (14) must rise about 1.5 times. That is trail about 30 or more with floor about 10, at 5 m. How much lamp that takes depends on the filter's tone curve in the dark; Vesper measures it. I cannot.
- The 5 m point and the floor 3 m beside it (about 5.8 m from the lamp) must sit in the full pool, not the fade. At range 9 the fade likely already bites at 5 to 6 m (URP's range fade, unverified for this version). Raise range before intensity.
- Three limits on the same change:
  1. N1 is 40 over the frame mean. A brighter pool raises the mean; Camp to J 10 to 50 pass N1 only on far small dots. Re-run N1 with any lamp change.
  2. Blowouts: 200 m already has 638 px over 230 at intensity 6, and the cleft frames (210 to 230) are pale rock at arm's length. More intensity blows these out; more range does not, as much.
  3. Confirm the trail material is not in main3_8_18_look.cs's albedo-cap lowered list. If it is, the cap cut the gap.
- One change per retake (Gate.md 3). Read Camp to J 40 and J to Ward 10 first; they are the worst counted frames.

### The cairn's flame card and N1 at J

- Count: no harm now. N1 is 8 of 8 on Camp to J and J to Ward 0 passes with the card in this build.
- By eye at J: helps. The pair "J night" and Camp to J 70 show the cairn as the lit thing on the line, and a flame reads as the keeper's "lit cairn" where a disc did not.
- From far: it hurts. The card is 0.14 by 0.24 m (main3_8_7_ward.cs 60), replacing a 0.6 m glow that ward.cs line 51 notes was the size that reads from 60 m (0.25 m was sub-pixel). At 60 m on a 1920 x 988 frame it is a few pixels (my estimate; FOV unverified). If the cairn carries N1 on Camp to J 10 to 50, a brighter lamp pool raising the mean will drop those frames first. Fix to ask for: keep a soft glow behind the flame for distance, or show which light carries N1 in each Camp to J frame.

## Counts for Wren

- W1 day: 1 (Camp 2 to T fwd 0). Markers: Camp 2 blaze missing, W1 blaze unverified, trailhead board PASS.
- W3: 7, or 4 with the cleft exempt (Wren and Grant).
- 8.17 found from trail: 2 FAIL (north ruin, Camp 3 tent).
- Dev panel: not rerun.
- Night: N1 0; N2 11 (Camp to J 7, J to Ward 4).

Pim
