# Objective: logbook day list and the on-screen line

**DRAFT, 2026-09-28, revised 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (daily loop; a NOT SAFE location goes into the logbook and the objective; stats in the logbook only outside the Ward; the player carries the logbook, which holds notes, questions and settings), 2026-09-29 (the day ends by filing the report or by an event; no time budget for the day). Gaps marked **[GAP: DailyLoop]** wait on Sable's Docs/Design/DailyLoop.md. The full logbook is Docs/Design/UI/Logbook.md (not yet written) **[GAP: Logbook]**. Colours **[GAP: Style]**.

## 1. Purpose

1. The logbook day list is the record of what today asks of the player. It is the only full list.
2. The on-screen line is a short, temporary note that the list just changed. It never stays on screen.

## 2. The day list (logbook, today's page)

```
+-------------------------------+-------------------------------+
|  DAY 14                       |                               |
|                               |   (right page: report,        |
|  [x] Climb the tower          |    past days, stats           |
|  [ ] Campsite 2: see to it    |    [GAP: Logbook])            |
|  [ ] File the report          |                               |
|      Then the Ward.           |                               |
|                               |                               |
|  Today                        |                               |
|    Food     -                 |                               |
|    Water    met               |                               |
|    Warmth   -                 |                               |
|    Social   -                 |                               |
|                               |                               |
+-------------------------------+-------------------------------+
```

1. Top block: duties, in the order they happen. `[x]` done, `[ ]` open. Box marks are uGUI Images, not glyphs.
2. Done lines also dim to 50 percent. Legacy Text rich text has no strikethrough tag, so a strike, if Vesper wants one, is an Image line over the text.
3. A NOT SAFE location is inserted after `Climb the tower` when the tower check marks it, one line per location, in tower-sheet order. It ticks when resolved on foot **[GAP: DailyLoop]** for what resolves it.
4. `Then the Ward.` is a plain line with no box, shown from waking. It is a reminder, not a duty.
5. Lower block: the needs with `met` or `-`. Need words are the same in every UI spec (DayEndConfirm.md uses them too): `Food`, `Water`, `Warmth`, `Social`. Safety is not listed; the location lines are Safety. The list of needs follows DECISIONS 2026-09-28 and stays a **[GAP: DailyLoop]** until Sable's DailyLoop.md sets it.
6. Numbers never appear on this page. Stats have their own page **[GAP: Logbook]**.
7. The logbook is carried, so the day list can be read anywhere by day. No clock, time-left line or duty timer appears on this page or anywhere else: the day has no time budget (DECISIONS 2026-09-29).
8. At night the day list is read-only: `File the report` cannot be chosen, and nothing ticks. DECISIONS 2026-09-28: at night the only thing the player can do is go to the Ward. Whether the logbook opens at night at all is open (section 7), because settings live in it.
9. If an event ends the day early, today's page stays as it was when the event hit: open lines stay open. What the unfiled report costs **[GAP: DailyLoop]**.

## 3. The on-screen line

```
+--------------------------------------------------------------+
|  Logbook: Campsite 2, see to it.                             |
|                                                              |
|                                                              |
|                              .                               |
|                                                              |
|                                                              |
+--------------------------------------------------------------+
```

1. Top left, inside a 5 percent safe margin. Same font and off-white as the interact prompt, with a 1 px dark shadow for the VHS filter.
2. Starts with `Logbook:` so the player knows where the full list lives.
3. Fades in 0.3 s, holds 4 s, fades out 0.5 s. A pencil-scratch sound on fade in (Hollis).
4. One line at a time. If two changes happen together, they queue with 0.5 s between.
5. Held (queued, not shown) while any of these is open: binoculars, logbook, report confirm, Ward screen, pause menu. Shown when it closes.
6. No line for a tick the player made at the logbook itself; they are already looking at it.

## 4. Triggers

| When | Line |
|---|---|
| Wake **[GAP: DailyLoop]** | `Logbook: climb the tower.` |
| Tower check marks a NOT SAFE location (shown after binoculars lower) | `Logbook: <Location>, see to it.` |
| All five checked, none NOT SAFE | `Logbook: all quiet. File the report when ready.` |
| A NOT SAFE location resolved | `Logbook: <Location> seen to.` |
| A need met | `Logbook: Water met.` (same words: `Food`, `Water`, `Warmth`, `Social`) |
| Report filed | `Logbook: go to the Ward.` |
| Event ends the day | No line. The player is already at the Ward (WardNight.md section 2). |
| Event adds or blocks a need (Milestone 14) | Written per event. |

Wording is draft; Quill may rewrite, but need words change in all specs at once.

## 5. States

| State | Day list | On-screen line |
|---|---|---|
| Waking | Only `Climb the tower`, `File the report`, `Then the Ward.` | Wake line. |
| Tower done | NOT SAFE lines added. | One line per location. |
| Chores | Needs tick. | One line per need met. |
| Report filed, night | Read-only (section 2.8). | `go to the Ward.` |
| Event ended the day | Frozen as the event left it (section 2.9). | None. Held lines are dropped. |
| Overlay open | Unchanged. | Queued. |

## 6. Input paths

The day list is a page of the logbook; opening it and turning pages belong to Logbook.md **[GAP: Logbook]**. On the page itself:

Keyboard and mouse: arrow keys or `W` / `S` move between lines that can be chosen (only `File the report`); `Enter` or click chooses it.

Gamepad: D-pad or left stick up and down move; `A` chooses; `B` closes the logbook.

Requirement for Rook: the Player action map is off while the logbook is open, then back on when it closes. Otherwise the page keys fire gameplay underneath: `Enter` is Attack, `A` (south) is Jump, `B` (east) is Crouch, D-pad left and right are Previous and Next, `WASD` is Move. Only the UI map is live. Pause behaviour on the logbook page itself belongs to Logbook.md **[GAP: Logbook]**.

The on-screen line takes no input.

## 7. Open questions

1. Closed 2026-09-29 (DECISIONS 2026-09-28): the logbook is carried. The re-show proposal is dropped; the player opens the logbook instead. The open button belongs to Logbook.md **[GAP: Logbook]**.
2. Does the logbook open at night? Settings live in it (DECISIONS 2026-09-28), so a closed logbook at night leaves settings only in the pause menu. Draft: it opens read-only. Wren to confirm with Grant.
3. What resolves a NOT SAFE location, and can the day list show it before the player arrives? **[GAP: DailyLoop]**

Pim
