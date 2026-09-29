# Objective: logbook day list and the on-screen line

**DRAFT, 2026-09-28, revised 2026-09-29 (second pass), Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (daily loop; a location that is not safe goes into the logbook and the objective; stats in the logbook only outside the Ward; the player carries the logbook, which holds notes, questions and settings), 2026-09-29 (the day ends by filing the report or by an event; no time budget; routine in Docs/Design/DailyLoop.md revision 6 approved for now). Terms (Wren, 2026-09-29): the tower stamps `SAFE` or `CHECK ON FOOT`; locations are Lake, Camp 1, Camp 2, Camp 3, Office (DailyLoop.md, Main3.md). Day one rule (Quill): nothing names the Ward before the night 1 reveal. The full logbook is Docs/Design/UI/Logbook.md. Colours per Style.md 7. Type per Fonts.md; revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Overpass for the objective line; Patrick Hand for logbook handwriting): sections 2.7 and 3.1.

## 1. Purpose

1. The logbook day list is the record of what today asks of the player. It is the only full list.
2. The on-screen line is a short, temporary note that the list just changed. It never stays on screen.

## 2. The day list (logbook, Today tab)

Layout, past days, night and day 1 wording: Logbook.md section 4. Summary of the rules this spec owns:

1. Duties in the order they happen: `Climb the tower`, the five location lines under it (`___`, `SAFE` or `CHECK`), `File the report`, then `Then the Ward.` (day 1: the keeper's note, Logbook.md 4.4).
2. A `CHECK` line has its own tick box and ticks when resolved on foot. What resolves it is per event (Events.md).
3. Needs show `met` or `-` in the fixed order Food, Water, Warmth, Social. Safety is the CHECK lines. Need words are the same in every UI spec.
4. No clock, time-left line or duty timer anywhere (DECISIONS 2026-09-29).
5. At night the list is read-only (Logbook.md 8.2).
6. If an event ends the day early, today's page stays as the event left it: open lines stay open. The day counts as filed (DailyLoop.md 1.1.2).
7. The day list is the keeper's hand: Patrick Hand (Logbook.md 2.1).

## 3. The on-screen line

```
+--------------------------------------------------------------+
|  Logbook: check Camp 2.                                      |
|                                                              |
|                                                              |
|                              .                               |
|                                                              |
+--------------------------------------------------------------+
```

1. Top left, inside a 5 percent safe margin. TextMeshPro, Overpass Regular, same size and colour as the interact prompt, with a 1 px dark shadow for the VHS filter (TMP underlay or shadow in the material, Rook's call). Printed, not handwritten, although it quotes the logbook. No VT323.
2. Starts with `Logbook:` so the player knows where the full list lives.
3. Fades in 0.3 s, holds 3 s (DailyLoop.md 1.1, 1.4, 2.1), fades out 0.5 s. A pencil-scratch sound on fade in (Hollis).
4. One line at a time. Lines that fire together queue with 0.5 s between.
5. Held (queued, not shown) while any of these is open: binoculars, logbook, report confirm, Ward screen, pause menu. Shown when it closes.
6. No line for a tick the player made at the logbook itself.

## 4. Triggers

| When | Line |
|---|---|
| Wake | `Logbook: climb the tower.` |
| All five stamped, none CHECK ON FOOT | `Logbook: all safe.` |
| All five stamped, some CHECK ON FOOT (after the binoculars lower) | `Logbook: check <Location>.` one line per CHECK, in tower-sheet order |
| A CHECK resolved | `Logbook: <Location> checked.` |
| A need met | `Logbook: Water met.` (same words: `Food`, `Water`, `Warmth`, `Social`) |
| New question or note (Logbook.md 5.6) | `Logbook: new note.` |
| Report filed, day 2 on | `Logbook: go to the Ward.` |
| Report filed, day 1 | `Logbook: the cairn path, at dark.` (Logbook.md 4.4; Quill owns the words) |
| Event ends the day | No line. The player is already at the Ward (WardNight.md 2.4). |
| Event adds or blocks a need (Milestone 14) | Written per event. |

Stamps do not post a line one by one; the line waits until all five are stamped (DailyLoop.md 1.4). Wording is draft; Quill may rewrite, but need words change in all specs at once.

## 5. States

| State | Day list | On-screen line |
|---|---|---|
| Waking | `Climb the tower`, five blank location lines, `File the report` locked, the Ward line or the day 1 note. | Wake line. |
| Tower done | Stamps filled. | All safe, or one line per CHECK. |
| Rounds | CHECKs and needs tick. | One line per CHECK resolved, per need met. |
| Report filed, night | Read-only. | Go to the Ward, or the day 1 cairn line. |
| Event ended the day | Frozen as the event left it. | None. Held lines are dropped. |
| Overlay open | Unchanged. | Queued. |

## 6. Input paths

Opening the book, tabs and focus: Logbook.md section 10. On the Today page the only choosable line is `File the report` (and `<` `>` for past days).

The on-screen line takes no input.

## 7. Open questions

1. Closed 2026-09-29: the logbook is carried; opening it is Logbook.md.
2. Closed 2026-09-29 (draft in Logbook.md 8.2): the logbook opens read-only at night. Wren to confirm with Grant.
3. Closed 2026-09-29: what resolves a CHECK is per event (Events.md); the list shows the CHECK line from the stamp on.

Pim
