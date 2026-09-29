# The Ward at night: offering screen

**DRAFT, 2026-09-28, revised 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (stats on screen only at the end-of-day Ward phase; at night the only action is going to the Ward and offering HP or MIND; ignoring it lowers WARD; WARD at 0 means the barrier falls; a run ends the first time HP, MIND or WARD reaches 0; the Ward screen has `Give nothing`), 2026-09-29 (the day ends by filing the report or by an event, for example a chase that catches the player, who then finds themselves at the Ward; no time budget for the day). DESIGN.md: stats 0 to 12, never above 12. Gaps marked **[GAP: DailyLoop]** wait on Sable's Docs/Design/DailyLoop.md. Colours **[GAP: Style]**. Revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Overpass for menus and warnings; VT323 only for the tape overlay): section 3 notes 3 and 6.

Terms check, 2026-09-29 (Wren: the approved DailyLoop.md wins): this spec uses no stamp or location names. Day one rule (Quill): the night 1 climb is led by the keeper's note and the lit cairn, never by naming the Ward (Logbook.md 4.4); the screen itself may name it, being the reveal. 0 to 3 points in any mix and the WARD cap are folded in (section 4.4 and 4.5, 2026-09-29). Event changes to tonight's Ward added in section 6a (2026-09-29, third pass). The other **[GAP: DailyLoop]** items below (bunk from night 2, the night 1 explainer, the last-point warning) are answered by DailyLoop.md 2 and 6 and get folded in at the next revision.

## 1. Purpose

The one place the player sees HP, MIND and WARD as numbers, and the one choice of the night: give the Ward HP, give it MIND, or give it nothing. Once the player kneels, there is no way back out without choosing.

## 2. How it opens

1. Night only. At the stones the prompt is `Kneel`. By day the stones have no prompt.
2. `E` / `Y` on the prompt. The camera eases over 1.0 s to a fixed framing: facing the stones, looking west, the fire behind them (Main3.md 2.7.3). Mouse and stick look are off; the cursor shows.
3. The stat rows fill in left to right over 0.8 s. Input is locked until they finish.
4. Event-ended day: when an event ends the day (for example a chase that catches the player), the event's own sequence ends in black. No day-end card (DayEndConfirm.md section 2.4), no `go to the Ward.` line, no walk and no `Kneel` prompt. The picture fades up from black over 1.5 s already in the fixed framing of step 2, kneeling at the stones, and the rows fill as in step 3. From there the screen is the same as a knelt-at night: same choices, same rules, same input lock (about 2.3 s in total). The event's effect on HP, MIND or WARD (DECISIONS 2026-09-29) is already in the rows when they fill; whether it plays on the pips as a drop is **[GAP: DailyLoop]**.

## 3. Wireframe

```
+--------------------------------------------------------------+
|                                                              |
|                 |     ||      |                              |
|                 |     ||      |      (stones, fire behind)   |
|                 |     ||      |                              |
|                                                              |
|   HP    # # # # # # # . . . . .    7                         |
|   MIND  # # # # # # # # # . . .    9                         |
|   WARD  # # # # x . . . . . . .    4                         |
|                                                              |
|    [ Give HP ]      [ Give MIND ]      [ Give nothing ]      |
|     ^ focus                                                  |
|    [====------]  hold to give                                |
|                                                              |
+--------------------------------------------------------------+
```

1. Lower half of the screen only. The stones stay visible above.
2. Each row: label, 12 pips, the number. `#` lit pip, `.` dark pip, `x` cracked pip (section 6.4).
3. Pips are uGUI Images, not font glyphs. Overpass's coverage of block characters is unverified, and Images survive the VHS filter better.
4. The number is kept beside the pips because 12 pips are hard to count at a glance through the filter.
5. The hold bar sits under the focused button and fills while the button is held.
6. Type: all text TextMeshPro in Overpass (Fonts.md 3.3): labels `HP` `MIND` `WARD` and numbers SemiBold **P**; buttons, `hold to give` and the result lines (`The stones go quiet.`, `The stones are fed.`) Regular. No handwriting, no VT323. The `Kneel` prompt is the interact prompt, also Overpass.

## 4. The choice

1. Three buttons: `Give HP`, `Give MIND`, `Give nothing`.
2. Each needs a 1.0 s hold. Release early and the bar drains; nothing happens. No button is safe, so no default can be safe; the hold is the guard.
3. Default focus: `Give HP` (leftmost). Not a recommendation, just the reading order.
4. Rate and amount (DailyLoop.md 2.3 and 2.4, approved for now): the player gives 0 to 3 points a night, HP or MIND in any mix. Each hold gives one point, and each point buys 1 WARD. After each gift the screen asks again, with `Enough` in place of `Give nothing`, until the player holds `Enough` or 3 points are given; after the third, the screen goes straight to the end of the night (section 5). Holding `Give nothing` before any gift gives 0.
5. WARD cap: WARD never goes above 12. A Give button is disabled, with `The stones are fed.` under the rows, when one more point would push WARD past 12 (DailyLoop.md 2.4). Only `Give nothing` or `Enough` then remains. Whether tonight's hunger is taken before the cap is checked is **[GAP: DailyLoop]**.
6. No back-out. This screen has no cancel. `B` and UI Cancel do nothing, and the player cannot walk away. `Esc` and `Start` open the pause menu; the Ward screen waits underneath and comes back as it was on Resume (section 8).
7. Focus change mid-hold: moving focus (D-pad, stick, arrow keys, `A` / `D`, or the pointer leaving the button) while a hold is running cancels the hold. The bar drains in 0.2 s and nothing is given. The newly focused button does not start filling until Submit is released and pressed again, so a hold can never slide from one gift onto another.
8. Holding with two devices at once (for example `Enter` held while clicking) counts as one hold on the focused button; releasing either input cancels it.

## 5. What happens on each choice

| Choice | Sequence (about 2.5 s) |
|---|---|
| Give HP | The last lit HP pip flickers and goes dark. 0.4 s later the next WARD pip lights. A rune on the stones brightens in the world. |
| Give MIND | Same, from the MIND row. |
| Give nothing | WARD drops (section 6). |
| Enough | No sequence; ends the choosing. |

After a gift, if fewer than 3 points are given, the buttons return with `Enough` (section 4.4). When choosing ends, the rows hold for 1.5 s, then fade to black and the night ends **[GAP: DailyLoop]** (Main3.md 3.1.4 has the player sleep at the ledge and wake in the cabin).

## 6. What the player sees when WARD drops

1. The last lit WARD pip cracks (`x`), flickers twice and goes dark.
2. In the world, one rune on the stones goes dark and stays dark.
3. The look filter rolls one noise band up the screen (LookFilter already has a band; Rook to add a scripted trigger).
4. A line under the rows for 2 s: `The stones go quiet.`
5. Sound: a low crack, then the silence of the ledge drops further (Hollis).
6. If the player never came to the Ward that night, WARD drops at sleep with no screen. On the next visit that pip shows cracked (`x`) instead of plain dark, once, so the loss is seen late.
7. The drop sequence plays through without input, except Pause. Pausing freezes the sequence where it is; Resume continues it.

## 6a. Events that change tonight before the player gives

Added 2026-09-29 (third pass). Some events change the Ward before the screen opens: a point of WARD given tonight by something other than the player, or a smaller hunger tonight. Words are Quill's; the lines below are placeholders.

1. **A talk at the stones** before kneeling plays in the Dialogue.md format, lower band, before the `Kneel` prompt shows. The Ward screen opens only after it ends.
2. **WARD given tonight by an event.** During the fill (section 2.3), the WARD row fills to its value before the event, then, 0.4 s after the fill ends, the next pip lights with the same rune brighten as a gift (section 5) and one line shows under the rows for 2.5 s: `The stones have been fed tonight.` Input stays locked until the line has shown 1 s.
3. The WARD cap holds (section 4.5): if WARD is already 12, no pip lights and the line is `The stones are fed.` as usual.
4. Whether an event's point counts toward the player's 3 a night is **[GAP: Sable]**. If it counts, the screen asks as if one point was already given (`Enough` in place of `Give nothing`).
5. **A smaller hunger tonight.** Once the hunger row is folded in (DailyLoop.md 2.3, next revision of this spec), a reduced hunger shows as fewer hollow pips on that row, and one line under the rows for 2.5 s: `The stones want less tonight.` No numbers in the line.
6. **Paid back later.** An event whose cost lands the next night (a debt) shows as a normal hunger or stat change on that night; no extra line unless content gives one.
7. One event line per night at most on this screen. A second waits for the logbook (Logbook.md 4.5, night line).
8. Line budget: 45 characters, one line, Overpass Regular 28, under the rows where `The stones go quiet.` sits. Never both at once: a drop line waits until the event line has faded.

## 7. States

| State | What shows |
|---|---|
| Day at the stones | No prompt. No screen. |
| Night, at the stones | Prompt `Kneel`. |
| Arrived by event | Fade up already kneeling (section 2.4), then Opening. |
| Talk at the stones | Dialogue.md talk before `Kneel` (section 6a.1). |
| Event gift or smaller hunger | Extra pip or fewer hollow pips after the fill, one line 2.5 s (section 6a). |
| Opening (input lock, about 1.8 s) | Camera eases, rows fill. Every press is ignored except Pause, which freezes the easing and fill until Resume. A Submit held down when the lock ends does not count; it must be released and pressed again. |
| Choosing | Rows, three buttons, focus on one. |
| Holding | Bar fills under the focused button. Focus change cancels it (section 4.7). |
| Result: gift | Section 5. |
| Result: WARD drop | Section 6. |
| A stat at 1 | Its last pip pulses slowly. No warning text. Giving it is allowed. |
| A stat reaches 0 | Run ends. The screen hands to the ending sequence **[GAP: Milestone 7 endings]**. |
| WARD cap reached | Give buttons disabled with `The stones are fed.`; only `Give nothing` or `Enough` remains (section 4.5). |
| 3 points given | Choosing ends; no fourth ask. |
| Owed tonight (proposal) | If need costs are taken at sleep after this screen **[GAP: DailyLoop]**, pips owed tonight show hollow and flickering so the player can decide knowing them. |
| Pause pressed (any phase) | Pause menu opens over the Ward screen, which waits underneath, frozen. Resume returns to the same phase. A running hold is reset; a Submit still held at Resume must be released and pressed again. |
| Window loses focus mid-hold | The hold cancels, as a focus change. |

## 8. Input paths

Keyboard and mouse:
1. At the stones, `E` to kneel.
2. Arrow keys or `A` / `D` move focus.
3. Hold `Enter` 1.0 s to give. (`Space` as Submit unverified.)
4. Mouse: press and hold on a button 1.0 s. Hover moves focus.
5. `Esc` opens the pause menu. It never cancels the screen.

Gamepad:
1. At the stones, `Y` to kneel.
2. D-pad or left stick left and right move focus.
3. Hold `A` 1.0 s to give.
4. `B` does nothing.
5. `Start` opens the pause menu.

Requirements for Rook:
1. **Pause goes through here.** Unlike the day-end confirm (DayEndConfirm.md section 8, the only card that suppresses pause), this screen does not set the "modal open" flag. `Esc` and `Start` reach GamePause as usual. `Esc` is also UI Cancel; the screen ignores Cancel so only the pause fires. While paused, the Ward screen does not take input and its sequences stop: easing, fill and result sequences run on scaled time so timeScale 0 freezes them. The hold resets on pause.
2. **Player action map off** from the `Kneel` press (or, on an event-ended day, from the event's fade to black) until the fade to black ends, then back on (or handed to the next scene state **[GAP: DailyLoop]**). Otherwise the screen's keys fire gameplay underneath: `Enter` is Attack, `A` (south) is Jump, `B` (east) is Crouch, D-pad left and right are Previous and Next, `WASD` is Move. Only the UI map is live.
3. The Ward screen component goes in GamePause's gameplay list so it sleeps while paused.

## 9. Open questions

1. Closed 2026-09-29 (DECISIONS 2026-09-28): `Give nothing` stays. It is a valid, hopeless choice.
2. Closed 2026-09-29: 0 to 3 points a night, 1 WARD each, WARD capped at 12 (DailyLoop.md 2.4; section 4.4 and 4.5). Open: whether hunger is taken before the cap check.
3. Night clock or blackout before reaching the Ward (Main3.md 3.1.5) **[GAP: DailyLoop]**.
4. Order of costs: are the day's unmet needs taken before this screen (so the rows already show them) or at sleep after it?
5. Closed 2026-09-28 (Wren accepted): Pause goes through on this screen; only the day-end confirm suppresses it.

Pim
