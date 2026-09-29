# Day end confirm: "This ends your day"

**DRAFT, 2026-09-28, revised 2026-09-29 (second pass), Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (filing the report asks to confirm that it ends the day; stats on screen only at the Ward; the player carries the logbook), 2026-09-29 (the day ends by filing the report or by an event; no time budget for the day; routine in Docs/Design/DailyLoop.md revision 6 approved for now). Terms (Wren, 2026-09-29): stamps are `SAFE` and `CHECK ON FOOT`; locations are Lake, Camp 1, Camp 2, Camp 3, Office. Day one rule (Quill): nothing names the Ward before the night 1 reveal. The logbook is Docs/Design/UI/Logbook.md. Type per Style.md 7 and Fonts.md; warning colour **[GAP: Style]**. Revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Overpass for forms, menus and warnings): section 3 type note.

## 1. Purpose

Filing the report ends the day and starts the night. It cannot be undone. The confirm makes the player see what is still open before they commit.

## 2. Where it opens

1. In the carried logbook, on the Today page, the player chooses `File the report`. Filing works anywhere (DailyLoop.md 1.6).
2. The confirm opens as a small card over the dimmed logbook page. The logbook stays behind it.
3. `File the report` is only choosable once all five locations are stamped (DailyLoop.md 1.6). Before that the logbook line reads `Climb the tower first.` and cannot be chosen.
4. Event-ended day: when an event ends the day (DECISIONS 2026-09-29, for example a chase that catches the player), this card never opens and the open lines are never shown. If the card or the logbook is open when the event fires, both close at once with no choice made. The event's sequence ends in black and the player comes to at the Ward, on the Ward screen (WardNight.md section 2.4).
5. The card shows no clock, time left or hour. The day has no time budget.

## 3. Wireframe

```
+--------------------------------------------------------+
|  (logbook page, dimmed to 40 percent)                  |
|                                                        |
|      +------------------------------------------+      |
|      |  File today's report?                    |      |
|      |  Filing the report ends your day.        |      |
|      |                                          |      |
|      |  Still open:                             |      |
|      |    Camp 2 not checked.                   |      |
|      |    Food not met.                         |      |
|      |    Water not met.                        |      |
|      |                                          |      |
|      |     [ Not yet ]       [ File it ]        |      |
|      |      ^ focus                             |      |
|      +------------------------------------------+      |
|                                                        |
+--------------------------------------------------------+
```

With nothing open, the list is replaced by one line: `Nothing left open.`

Type: TextMeshPro, Overpass. The card is the game asking, not the keeper writing, so no handwriting on it even though it sits over the logbook page. Heading SemiBold **P**; lines and buttons Regular. The open lines are the warning and must be the most legible text on screen (Fonts.md 3.3).

## 4. Open lines

1. One line per thing not done today. No numbers and no stat names: stats show only at the Ward (DECISIONS 2026-09-28). The words are the warning.
2. Order: open CHECK ON FOOT locations first, in tower-sheet order, then needs in the fixed order Food, Water, Warmth, Social.
3. Wording. The need words are the same in every UI spec (Objective.md uses them too): `Food`, `Water`, `Warmth`, `Social`. Quill may change the words, but a change applies to all specs at once.

| Open item | Line |
|---|---|
| CHECK ON FOOT location not resolved | `<Location> not checked.` |
| Food not met | `Food not met.` |
| Water not met | `Water not met.` |
| Warmth not met | `Warmth not met.` |
| Social not met | `Social not met.` |

The needs are Food, Water, Warmth, Social, with Safety carried by the location lines (DailyLoop.md).

4. At most 8 lines. If more, the eighth reads `And more.` (Nine is only possible with all five locations CHECK ON FOOT and no need met.)
5. Safety has no line of its own. It is met by resolving each CHECK ON FOOT location, so the location line is the Safety line (DECISIONS 2026-09-28).
6. A need blocked by an event that day still gets its line; events may add their own wording later (Milestone 14).

## 5. Buttons and default focus

1. Two buttons: `Not yet` (left) and `File it` (right).
2. Default focus: `Not yet`. The irreversible choice is never the default.
3. Input lock: for 0.4 s after the card opens, Submit and click are ignored, so the press that opened it cannot carry through.
4. `Not yet` closes the card and returns to the logbook page, focus back on `File the report`.
5. `File it` closes the card and the logbook, plays the day-end tone (Hollis) and starts the transition to night (DailyLoop.md 2). The on-screen line then reads `Logbook: go to the Ward.`, or on day 1 `Logbook: the cairn path, at dark.` (Objective.md 4, Logbook.md 4.4).
6. Day 1 card: no line on it names the Ward.

## 6. States

| State | What shows |
|---|---|
| Opening | Card fades in over 0.2 s. Input locked 0.4 s. |
| Open items listed | Heading, list, two buttons, focus on `Not yet`. |
| Nothing open | Heading, `Nothing left open.`, two buttons, focus on `Not yet`. |
| Focus on `File it` | Button highlighted in the warning colour **[GAP: Style]**. |
| Confirmed | Card and logbook close. Night transition. |
| Cancelled | Card closes. Logbook page, focus on `File the report`. |
| Event ends the day while open | Card and logbook close, no choice made. The modal flag clears. Event sequence, then WardNight.md section 2.4. |
| Pause pressed | Pause is suppressed. `Esc` or `Start` acts as `Not yet` (section 8). The pause menu is one more press away, from the logbook page. |

## 7. Input paths

Keyboard and mouse:
1. Arrow keys or `A` / `D` move focus between the buttons.
2. `Enter` activates the focused button. Whether `Space` also counts under the `*/{Submit}` binding is unverified; Rook to check.
3. `Esc` acts as `Not yet`.
4. Mouse: click a button. Hovering a button moves focus to it, so there is never a second highlight.
5. `E` does nothing on the card.

Gamepad:
1. D-pad or left stick left and right move focus.
2. `A` (south) activates the focused button.
3. `B` (east) acts as `Not yet`.
4. `Start` acts as `Not yet`.

## 8. Requirements for Rook

1. **Pause suppressed (needs a small pause code change).** `Esc` is bound to both Player/Pause and UI/Cancel; `Start` is bound to Pause. PauseMenu.cs subscribes to Pause.performed on its own and calls GamePause.Toggle, so a card cannot swallow the press today. Required: while this card is open, pause is suppressed (the Ward screen lets Pause through, see WardNight.md), and `Esc` or `Start` goes to the card's cancel instead. On this card cancel is `Not yet`. One way: GamePause (or PauseMenu) checks a "modal open" flag that the card sets on open and clears on close, and ignores Pause while it is set. The method is Rook's call; the behaviour is the requirement.
2. **Player action map off.** While the card is open the Player map is disabled, then re-enabled when the card closes (either button). Otherwise the card's keys fire gameplay actions underneath: `Enter` is Attack, `A` (south) is Jump, `B` (east) is Crouch, D-pad left and right are Previous and Next, `WASD` is Move. Only the UI map (Navigate, Submit, Cancel, Point, Click) is live. If the logbook page behind already turns the Player map off, the card keeps it off; it must not turn it back on when it closes over the logbook.
3. **Event close.** An event that ends the day must be able to close this card and the logbook with no button pressed, clear the modal flag, and leave the Player map off for the event sequence and the Ward screen (WardNight.md section 8.2).
4. Reason UI cancel wins over pause here: the card is a yes or no about an irreversible step, and backing out is what `Esc` should mean on it.

## 9. Open questions

1. Closed 2026-09-29: yes, File is locked until all five are stamped (DailyLoop.md 1.6).
2. Closed 2026-09-29 (DECISIONS 2026-09-29): the day ends by filing or by an event, never by time. An event-ended day skips this card (section 2.4).
3. Should the card name a cost in words (for example `Water not met. It will cost you.`) without numbers? Draft says no: the line alone.

Pim
