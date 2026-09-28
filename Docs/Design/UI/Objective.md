# Objective: logbook day list and the on-screen line

**DRAFT, 2026-09-28, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (daily loop; a NOT SAFE location goes into the logbook and the objective; stats in the logbook only outside the Ward). Gaps marked **[GAP: DailyLoop]** wait on Sable's Docs/Design/DailyLoop.md. The full logbook is Docs/Design/UI/Logbook.md (not yet written) **[GAP: Logbook]**. Colours **[GAP: Style]**.

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
7. The logbook does not open at night. DECISIONS 2026-09-28: at night the only thing the player can do is go to the Ward. After the report is filed, the desk has no prompt until the next day.

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
| Event adds or blocks a need (Milestone 14) | Written per event. |

Wording is draft; Quill may rewrite, but need words change in all specs at once.

## 5. States

| State | Day list | On-screen line |
|---|---|---|
| Waking | Only `Climb the tower`, `File the report`, `Then the Ward.` | Wake line. |
| Tower done | NOT SAFE lines added. | One line per location. |
| Chores | Needs tick. | One line per need met. |
| Report filed, night | Not reachable: the logbook does not open at night. | `go to the Ward.` |
| Overlay open | Unchanged. | Queued. |

## 6. Input paths

The day list is a page of the logbook; opening it and turning pages belong to Logbook.md **[GAP: Logbook]**. On the page itself:

Keyboard and mouse: arrow keys or `W` / `S` move between lines that can be chosen (only `File the report`); `Enter` or click chooses it.

Gamepad: D-pad or left stick up and down move; `A` chooses; `B` closes the logbook.

Requirement for Rook: the Player action map is off while the logbook is open, then back on when it closes. Otherwise the page keys fire gameplay underneath: `Enter` is Attack, `A` (south) is Jump, `B` (east) is Crouch, D-pad left and right are Previous and Next, `WASD` is Move. Only the UI map is live. Pause behaviour on the logbook page itself belongs to Logbook.md **[GAP: Logbook]**.

The on-screen line takes no input.

## 7. Open questions

1. Is the logbook fixed to the cabin desk, or carried? If fixed, the player cannot check the list in the field, and the only reminder is the on-screen line. Proposal if fixed: `Tab` / D-pad up re-shows the current top open line for 4 s. Both are free in the Player map. Not a new screen.
2. What resolves a NOT SAFE location, and can the day list show it before the player arrives? **[GAP: DailyLoop]**

Pim
