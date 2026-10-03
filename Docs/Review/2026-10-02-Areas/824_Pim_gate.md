# 8.24 gate step 4: Camp 1 and the north loop (Pim, 2026-10-02)
Sources: Docs/Captures/Main3Review_north (index.md, Checks.md, Found_north.jpg, AreaFrames_north.jpg, Warps_NESW.jpg, capture 2026-10-02 20:27); NorthLayout_UI.md; DailyLoop.md 1.6; DayEndConfirm.md; DevMenu.cs, DevWarpLabels.cs, PlayerInteractor.cs, InputSystem_Actions.inputactions; Main3.unity (saved file, warp transforms); main3_8_24_north.cs. No Editor run. Grant's Game view 3840 x 1976; no frame in this capture shows the prompt or the dev panel at that size.
Input facts: Interact is E / pad Y (buttonNorth). Look is mouse or right stick; there is no keyboard look binding, so "keyboard only" aiming at a world point uses the mouse. Presses below count buttons, not aiming.

## 1. Filing the report at the report box: wrong task, see a to c
a. **Premise.** Filing is never done at a box. DailyLoop.md 1.6: the report is filed from the carried logbook, anywhere. The ruin box is the dead keeper's box: `Examine`, Quill's line, files nothing (NorthLayout_UI.md row 11).
b. **Box reach: PASS.** Checks.md: the interactor ray from the stand (170.16, 277.32) first hits ReportPost/ReportBox at 1.44 m (reach 2.0); the level eye ray from the approach first hits the box at 1.6 m. Found: report post from the side path 11.7 m, 6.9 deg off, 6.3 deg tall (passes the found rule); the magenta ball shows in the last Found_north frame; AreaFrames F5 shows the post by the doorway. 0.5 m off the opening edge: UNVERIFIED (doorway width not in the sheets).
c. **Box prompt: UNVERIFIED.** The recipe adds no Interactable to ReportBox (main3_8_24_north.cs line 234; the check tests the ray only, no prompt). No Examine code exists. Today the player sees no prompt there. Spec path when built: look, E / Y, 1 press. Forage C got a stand-in prompt; the box did not, so its 1 press cannot be tested in Play.
d. **Filing (logbook), on paper: 4 presses, UNVERIFIED (not built).** Pad: open book (proposed View), A on `File the report` (only choosable line), Right to `File it`, A = 4; 5 if the book opens on another tab. Keyboard: Tab (proposed), Enter, Right or D, Enter = 4 or 5. Clash to rule before Logbook is built: DevMenu reads pad View directly, so in Editor and dev builds View would open the dev panel and the book together.

## 2. Foraging at C: PASS on presses, 1 hurt
1. **Presses: PASS.** Look at a shrub, E / Y: 1 press, pad and keyboard. Prompt word `Forage` matches NorthLayout_UI.md and BurnLayout_UI.md. Checks.md: ray from the stand (229.2, 270.6) meets Bush_ForageC_1 at 1.94 m, prompt "Forage"; 5 solid capsules, least gap 0.58 m.
2. **Found: PASS by numbers, UNVERIFIED by eye.** Forage C from Camp 1 to J 25.4 m out, 43.9 deg off travel (bar 45), 1.8 deg tall. Found_north.jpg reads only at 1/8 on the sheet; I cannot find the ball or the shrubs in AreaFrames F3 at this scale. Same ask as 822: single frames at 1920 x 988.
3. **Hurt: reach margin 0.06 m.** 1.94 of 2.0 from the one stand, pitched about 35 deg down at the south shrub. The area check's level eye ray from the same stand hits nothing within 2 m. A player who stops half a step short gets no prompt. Ask Rook: from how many points on the trail edge within 1 m of the stand does the prompt fire, looking at any shrub.
4. **UNVERIFIED (no code):** bearing vs bare reads apart at 10 m; prompt hidden when bare or Food met. The stand-in always shows `Forage`.

## 3. Warps in the area: PASS on name, landing and facing; presses over the bar
| Warp | Panel name (DevWarpLabels.cs) | Lands (Checks.md) | Faces (Main3.unity) | Pad | Keyboard |
|---|---|---|---|---|---|
| Camp_1 | `Camp 1`, CAMPS | (268.0, 5.2, 226.0), fell 0.17 m, terrain 5.00: PASS | yaw 49.0, at the Camp 1 root (282, 238): PASS | View, RB, Down x10, A = 13 | F1, PgDn, Down x10, Enter = 13 |
| North_Loop_Ruin | `North loop: ruin`, CAMPS | (165.8, 3.7, 267.7), fell 0.16 m, terrain 3.54: PASS | yaw 25.0, at the ruin (172, 281): PASS | View, RB, Down x12, A = 15 | F1, PgDn, Down x12, Enter = 15 |

1. Facing by frame: Warps_NESW N and E bracket 49 (tent N, tripod and clearing E); ruin N frame shows the ruin and stovepipe right of centre, where 25 puts it. No frame at the landing yaw itself: UNVERIFIED by eye.
2. Presses: 13 and 15 exceed 5. Gate.md 4 sets the bar for day/night, warp to the Ward and tasks Grant names; area warps are not named, so this is not a gate fail. Wren to rule if it should be.
3. UNVERIFIED: the dev panel was not run at 3840 x 1976 after these rows; Warp() sets yaw only, so camera pitch carries over from before the warp (DevMenu.cs line 156).

## Not judged here
Trail grey (Jg to Camp 1 diff 20 m 17, Camp 1 to J 16, both FAIL by numbers; by-eye rule not applied) and stops: outside Wren's three items.

Pim
