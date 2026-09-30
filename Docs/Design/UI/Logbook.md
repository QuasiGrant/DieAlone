# Logbook

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (the player carries the logbook; its main job is tracking questions and notes; settings live in it, later inventory, memories and collectibles; stats show on screen only at the Ward, the rest of the time in the logbook only; at night the only action is the Ward), 2026-09-29 (the day ends by filing or by an event; no time budget; nothing forces the player to act; routine in Docs/Design/DailyLoop.md revision 6 approved for now). Written after the four loop specs, per Tully's order. Revised 2026-09-29 to add the Kept tab after Settings (Collectibles.md). Colours and sizes follow Style.md section 7 and Fonts.md (Vesper owns type). Settings content is Settings.md. Revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Patrick Hand for logbook handwriting, Overpass for forms, menus and settings, VT323 only for the tape overlay): section 2.1. Revised 2026-09-29 (third pass) for event text: tower lines, log lines, missed lines, night lines, off-row places, length budgets, pages before day 1 and the unread state (sections 4.5 to 4.7). Examine and read: Examine.md. Talk and captions: Dialogue.md.

Terms (Wren, 2026-09-29: the approved DailyLoop.md wins): the tower stamps `SAFE` or `CHECK ON FOOT`; locations are Lake, Camp 1, Camp 2, Camp 3, Office, as in DailyLoop.md and Main3.md. On a narrow logbook column `CHECK ON FOOT` may be cut to `CHECK`; the full words show on the tower sheet, the binoculars and the map foot line.

Day one rule (Quill): before the night 1 reveal, no objective line, logbook page or map label names the Ward. The night 1 climb is led by the keeper's hand and the lit cairn (section 4.4).

## 1. Purpose

The one place the player keeps track of the job: today's duties, the questions the place raises and what they have learned, where things are, and the settings. It is carried, so it opens anywhere by day. It is the only permanent information screen; nothing else stays on screen but the crosshair dot.

## 2. What it looks like

A screen-space uGUI overlay drawn as an open notebook, about 80 percent of screen width, centred. The world stays visible at the edges, dimmed to 40 percent. Plate and text colours from Style.md 7.3. Tabs are paper tabs along the top edge. The book is not a 3D object in the hand; that is an art pass question, not this spec.

```
+----------------------------------------------------------------+
|  (world, dimmed)                                               |
|   [LB]  [ Today ]  Questions   Map   Settings  [RB]            |
|  +-----------------------------+-----------------------------+ |
|  |  DAY 14                     |  At the Ward, night 13      | |
|  |                             |    HP     7                 | |
|  |  [x] Climb the tower        |    MIND   9                 | |
|  |      Haze. (general line)   |    WARD   4                 | |
|  |      Lake       SAFE        |                             | |
|  |      Camp 1     SAFE        |  Today                      | |
|  |      Camp 2     CHECK   [ ] |    Food -  Water met        | |
|  |        (tower line)         |    Warmth -  Social -       | |
|  |      Camp 3     SAFE        |  Log                        | |
|  |      Office     ___         |  . (log line, up to three   | |
|  |                             |    lines)                   | |
|  |  [ File the report ]        |    (log line)               | |
|  |    Climb the tower first.   |                       more  | |
|  |  Then the Ward.             |                             | |
|  |                         < > |                    Day 14   | |
|  +-----------------------------+-----------------------------+ |
|        [Tab] close   [Q] [E] tabs                              |
+----------------------------------------------------------------+
```

1. Tab glyphs `[LB]` `[RB]` or `[Q]` `[E]` follow the last device used, as in TowerCheck.md 3.5. The hint line under the book shows for the first 3 s of each opening on days 1 to 3, then never.
2. TextMeshPro only (section 2.1). Tick boxes, stamps and pins are uGUI Images, not glyphs.
3. No clock, hour or time-left anywhere in the book (DECISIONS 2026-09-29).
4. Fit: same rule as MainMenu.md 2.6, fit target the open book plus tabs and hint line. The book keeps its page ratio and shrinks to fit narrow screens; the world fills the rest. The pause menu beside it (section 9) uses the 8.9h pause fit.

### 2.1 Type

Rule (Fonts.md 5.1): what the keeper wrote is Patrick Hand; what the game or an agency printed is Overpass. No VT323 anywhere in the book.

| Text | Font |
|---|---|
| Page content on Today, Questions, Map: `DAY n`, duties, `File the report` and its locked line, location names, `At the Ward` reading and numbers, needs, past-day lines, `Back to today`, questions, notes, map labels, the map foot line, the day 1 note, night top lines | Patrick Hand Regular, body 32 **P** |
| `SAFE`, `CHECK`, `___` beside a location | Stamp Images (art in Overpass caps, Vesper), not text |
| Tab labels on the paper tabs, the hint line under the book | Overpass Regular |
| Settings page | Overpass (Settings.md) |
| Kept page | Collectibles.md |
| Pause menu and its quit confirm (section 9) | Overpass; heading `Paused` SemiBold **P** |

1. Handwriting gets no faux bold and no italics. Emphasis is by position, not weight.
2. Done lines: dim to 50 percent and struck through with TMP `<s>` **P** (strikethrough was not possible on legacy Text; Vesper to confirm the look).
3. Page headings in the book (`QUESTIONS`, `Answered`) are handwriting too: the keeper wrote them.

## 3. Tabs

Order is fixed. A tab that has no content yet is hidden, not greyed.

| Tab | Shown | Job |
|---|---|---|
| Today | always | duties, stamps, File, needs, last Ward reading, past days |
| Questions | always | open and answered questions, notes under each |
| Map | always | hand-drawn map, pins from the stamps |
| Inventory | reserved, hidden | later (DECISIONS 2026-09-28) |
| Memories | reserved, hidden | later |
| Settings | always | Settings.md |
| Kept | hidden until the first homage is found, ever | Collectibles.md |

Reserved tabs get their own spec when their feature is planned. Their slots are between Map and Settings. The collectibles tab (`Kept`) is the last page of the book (DECISIONS 2026-09-29), after Settings. Tabs do not wrap.

The book always opens on the tab it was closed on, except: after waking it opens on Today once.

## 4. Today tab

### 4.1 Left page: today

1. `DAY n`, then duties in the order they happen.
2. `Climb the tower` with a tick box. Under it one line per location in tower-sheet order: name, then `___` (not looked at), `SAFE`, or `CHECK` with its own tick box. A `CHECK` box ticks when resolved on foot. The word, never colour alone.
3. `File the report`: the one choosable line on the page. Locked until all five locations are stamped (DailyLoop.md 1.6); while locked, the line under it reads `Climb the tower first.` and focus skips it. Choosing it opens DayEndConfirm.md.
4. `Then the Ward.` plain line, no box. Day 1 only, section 4.4 replaces it.
5. Done lines dim to 50 percent and are struck through (section 2.1).

### 4.2 Right page

1. `At the Ward, night n`: HP, MIND and WARD as numbers, exactly as they stood when the player last left the Ward screen. Not live. Day 1 shows `Nothing written yet.` Draft reading of DECISIONS 2026-09-28 line "the rest of the time they are in the logbook only"; see question 1.
2. `Today`: needs with `met` or `-`, fixed order Food, Water, Warmth, Social, two to a line. Safety is the CHECK lines.
3. `Log`: the day's log lines (section 4.5). Hidden while empty.

### 4.3 Past days

1. `<` and `>` at the page foot turn to earlier days. Past pages are read-only: what was stamped, what was met, the Ward reading that night.
2. On a past day the right page foot shows `Day n`; the left page foot shows `Back to today` as a choosable line.
3. Oldest page is day 1, unless content adds a page before it (section 4.7). Past pages are kept for the whole run and saved with it (GameState, Rook to size).

### 4.4 Day 1 and night 1: the Ward is not named

1. Day 1 left page: `Then the Ward.` is replaced by a note in the keeper's hand, written on the page from waking: `At dark: the cairn path, up past the chain.` Main3.md places a pale cairn and chain at the foot of the Ward path (J). Quill owns the words.
2. Day 1 right page: `Nothing written yet.` No `At the Ward` heading.
3. Report filed on day 1: the on-screen line is `Logbook: the cairn path, at dark.` instead of `go to the Ward.`, and the night page top line is `Night. The cairn path.` The chain is down and the cairn lamp lit (DailyLoop.md 2.1), so the lamp is the thing to walk to.
4. Map on day 1 and night 1: the cairn is drawn with no label; the ledge is an unnamed cliff.
5. After the night 1 Ward screen closes, every later page, line and label may name the Ward. The day 1 page, read back later, keeps its original wording.

### 4.5 Event lines

The game writes four kinds of line from event and storyline content (Events.md; words by Quill). All Patrick Hand 32, pencil, as the rest of the page. The player never writes.

| Kind | Written when | Where |
|---|---|---|
| Tower line | The location's row is stamped (binoculars or on foot) | Left page, indented under its row, one line |
| Log line | An event is dealt with, a thing is found, a talk or examine says so, or on waking | Right page, `Log` block, newest last |
| Missed line | At sleep, for an event left open that has one | The day just ended, `Log` block, after its log lines |
| Night line | At night (the climb, the stones) | The day just filed, `Log` block, under a handwritten `Night` subheading |

1. **Tower line.** The fact the tower saw, SAFE rows included (Events.md 4.5: warnings are a plain line, never a stamp). One per row at most. The event record names its row. The location prefix is stripped when the line sits under its own row (`Camp 2: smoke.` shows as `smoke.` under Camp 2).
2. **Off-row places.** Places without a row hang under the row their event belongs to (Events.md 3.1.1 and 6: a trail beside a location flags that location; the store is on the Office line). They keep their prefix, since it is not the row name. Draft mapping, Sable to confirm per event:

| Place in the text | Row |
|---|---|
| Store, lot, booth, barrier, closed loop, the road | Office |
| Trail leg, trail pole, crossing | the location whose roll drew it |
| Sky, far trees, the tower itself, map-wide | General line |

3. **General line.** One line directly under `Climb the tower`, for tower facts that belong to no row. Hidden when empty. At most one; a second map-wide fact replaces it.
4. **Log block.** Lines in the order written. A log line that resolves a CHECK sits in the block; the CHECK box on the left ticks as before. The same content line is never written twice on one page.
5. **Missed lines** are written at sleep, so the player reads them on the past page (`<`) or when a past day is shown. The unread state (4.7) marks them.
6. **Night lines** go on the day that was filed, so night 3 is read on day 3's page. Lines written on waking go on the new day's Log block.
7. Past pages keep all four kinds exactly as written.

### 4.6 Length budgets and overflow

Estimates at the 1080-row reference, Patrick Hand 32, about 45 characters per line on one page. Rook to check on the first build and through the filter.

| Line | Budget |
|---|---|
| Tower line, with any prefix | 45 characters, one line |
| General line | 45 characters, one line |
| Log line | 120 characters, three lines at most |
| Missed line | 120 characters, three lines at most |
| Night line | 120 characters, three lines at most |

1. Over budget: Rook's import check warns. The page never shrinks text.
2. Left page worst case (general line, five rows, five tower lines, File with its locked line, the Ward line) is 15 lines. It fits one page; no scroll on the left.
3. The Log block scrolls when it outgrows the right page. A handwritten `more` at its foot shows when lines are below; `back up` at its head when lines are above. Right stick or mouse wheel scrolls it. Focus never enters it (it has nothing to choose).
4. Line spacing: 0.5 line gap between log lines so a three-line entry reads as one.

### 4.7 Pages before day 1, and unread

1. Content may add a page dated before day 1. It sits before day 1 in the `<` order and reads like any past page. Nothing in the UI announces it beyond the unread mark.
2. **Unread.** A line or page the player has not yet had on screen for 1 s carries the new-note dot (Image, as Questions 5.1): beside the line, on the `<` arrow when an earlier page holds one, and on the `Today` tab label.
3. A dot clears when its line or page has been on screen 1 s. Reading a page clears every dot on it.
4. Content may test whether a page is still unread (for a line written at each sleep while it stays unread). The UI only keeps the flag; the rule is content's.
5. Unread state is saved with the run.

## 5. Questions tab

The main job of the book (DECISIONS 2026-09-28).

```
+-----------------------------+-----------------------------+
|  QUESTIONS                  |  Who parked at the office?  |
|                             |                             |
|  > Who parked at the office?|  Day 2  Blue car, no one    |
|  . Why is the road so quiet?|         inside.             |
|    What is the cairn lamp?  |  Day 4  The store clerk     |
|                             |         says it came in     |
|  Answered                   |         before me.          |
|    Where is the water pump? |                             |
|                             |                             |
+-----------------------------+-----------------------------+
```

1. Left page: open questions first, newest first, then `Answered` in the order they were answered. `.` is a new-note dot (Image) until the question has been focused once.
2. Right page: the focused question and its notes, oldest first, each with its day.
3. The game writes every question and note. The player cannot type: free text is unusable on a gamepad and nothing in the loop needs it.
4. A question is answered when its content says so. Answered questions stay, dimmed, with their notes.
5. Writing and triggers are content (Quill, Milestone 13 format). Twist-bearing text follows the twist rule (DECISIONS 2026-09-28).
6. A new question or note posts `Logbook: new note.` through the Objective.md queue. Objective.md adds that trigger in its next revision.
7. Empty state (should not happen after waking on day 1): `Nothing to ask yet.`

## 6. Map tab

```
+-----------------------------+-----------------------------+
|        (hand-drawn map, both pages)                       |
|     Camp 3 (o)                                            |
|                    Tower [#]        Camp 2 (!)            |
|   Lake (o)            Cabin                               |
|                                  Office (_)  Store        |
|  (cliff)                Camp 1 (o)                        |
|                                                           |
|  Camp 2: check on foot. Stamped day 14.                   |
+-----------------------------+-----------------------------+
```

1. One drawing across both pages, from the Milestone 8 top-down map image, redrawn in the book's hand **[GAP: Main3 map art]**.
2. Pins only on the five checked locations. Pin states match today's stamps: `(_)` not looked at, `(o)` SAFE, `(!)` CHECK, `(o)` with a tick once a CHECK is resolved. Pins are Images, shape differs per state, not colour alone.
3. Tower, cabin, store, road and trails are drawn, not pinned.
4. The cultist cave is not on the map until the player has stood at it once; then it is drawn, never pinned (it is never checked).
5. The Ward ledge: drawn as an unnamed cliff until night 1; after the night 1 reveal it is labelled `The stones`. Quill may change the word.
6. No you-are-here marker. The player finds the way by landmarks (DECISIONS 2026-09-28 pass checks).
7. Focus moves between the five pins; the line at the foot describes the focused pin. Pins are the only choosable things on the map; choosing does nothing more.

## 7. Settings tab

Content, controls and saving: Settings.md. In the book it is drawn as a ruled page with the same controls. It is the same panel the pause menu and main menu use.

## 7a. Kept tab

The homage collectibles page: content, finds, the cross-run save (profile.json) and platform achievements are Collectibles.md. Read-only by day and night; nothing on it is choosable beyond focus.

## 8. Opening and closing

### 8.1 By day

1. Opens anywhere the player is walking, standing, crouching or carrying. A carried object stays held.
2. Does not open while: binoculars are up, the Ward screen is open, a dialogue or minigame is open, the pause menu is open, an event sequence has taken control.
3. Opening: 0.2 s fade and page sound (Hollis, UI group). Time does not stop. The world runs; an event can fire while the book is open.
4. Movement and look are off while it is open (section 10). The player stands still.
5. On the first wake of a run, 1 s after the wake line (Objective.md), a lower-centre prompt shows once for 4 s: `[Tab] Logbook` or `(View) Logbook`, by last device.

### 8.2 At night

1. Opens, read-only. DECISIONS 2026-09-28: at night the only action is the Ward; reading and settings are not actions in the world.
2. Today shows the day as filed, plus one line at the top: `Night. Go to the Ward.` No choosable line on the page.
3. Questions, Map and Kept read as by day. Settings work fully.
4. Night 1: the same, with the top line `Night. The cairn path.` (section 4.4), so settings stay reachable on the reveal night.

### 8.3 Forced close

1. An event that ends the day closes the book at once, with the day-end card if open (DayEndConfirm.md 2.4). No fade.
2. Anything that takes control of the camera (event sequence, scripted scare) closes it the same way. The tab and focus are kept for next time.

## 9. Pause menu beside the book

The book does not replace the pause menu. The pause menu stays the non-diegetic way out, reachable from everywhere except the day-end card, where it is one press away (DayEndConfirm.md 6).

1. The pause menu shrinks to four rows. Look sensitivity and invert move out of it into Settings.

```
+------------------------------+
|  Paused                      |
|                              |
|  [ Resume ]    <- focus      |
|  [ Settings ]                |
|  [ Quit to menu ]            |
|  [ Quit game ]               |
+------------------------------+
```

2. `Settings` opens the book's Settings page alone (no other tabs), over the paused game. Time stays frozen. `Back`, `Esc` or `B` returns to the pause menu with focus on `Settings`. This is how settings are reached on the Ward screen, in binoculars and anywhere the book cannot open.
3. `Quit to menu` exists once MainMenu.md is built; until then it is not shown. Both quit rows open a confirm card: `Quit? You wake where you last slept. Today is lost.` Buttons `Not yet` (default focus) and `Quit`. Reason: GameSave writes only at sleep. On day 1 before the first sleep: `Quit? The run is lost.`
4. `Esc` or `Start` while the book is open: closes the book (it does not open pause). A second press opens the pause menu. So Resume and Quit are always at most two presses away.

## 10. Input paths

Keyboard and mouse:
1. `Tab` opens and closes the book.
2. `Q` and `E` change tab. Clicking a tab opens it.
3. Arrow keys or `W` / `S` move focus up and down. On Today, `A` / `D` or left and right arrows turn days; on Map they move between pins; on Settings they change the focused control; on Kept they move between frames.
4. `Enter` chooses. Mouse click chooses; hover moves focus. Mouse wheel scrolls a long Questions list and the Today `Log` block (4.6.3).
5. `Esc` closes the book.

Gamepad:
1. `View` (the small left centre button) opens and closes the book.
2. `LB` and `RB` change tab.
3. D-pad or left stick move focus. Left and right turn days on Today, move between pins on Map, change the control on Settings, move between frames on Kept.
4. `A` chooses.
5. `B` or `Start` closes the book.
6. Right stick up and down scrolls the Today `Log` block and a long Questions list.

Requirements for Rook:
1. New Player action `Logbook`: `<Keyboard>/tab` and the gamepad View/Select button. The exact Input System path for View (`<Gamepad>/select` expected) is unverified; Rook to check. Tab, View, LB and RB are unbound today (checked in Assets/InputSystem_Actions.inputactions).
2. **Player map off while the book is open**, except the `Logbook` action, which the book listens to itself so the same button closes it. Otherwise `E` is Interact, `A` (south) is Jump, `B` is Crouch, D-pad left and right are Previous and Next, `Enter` is Attack, `WASD` is Move. Only the UI map plus the book's own tab bindings (`Q`, `E`, `LB`, `RB`) are live.
3. **Pause while the book is open.** `Esc` is bound to both Player/Pause and UI/Cancel, `Start` to Pause. The book sets the same "modal open" flag as the day-end card (DayEndConfirm.md 8.1), so Pause is ignored and `Esc` / `Start` close the book instead. Pause-hosted Settings uses the same flag so `Esc` goes Back, not Resume.
4. Book UI runs on unscaled time so it works when opened from the paused Settings row.
5. The book component goes in GamePause's gameplay list.
6. Forced close API for events (section 8.3), clearing the modal flag.
7. Right stick scroll in the book: the UI map has no gamepad scroll today (unverified; Rook to check and add a `ScrollWheel`-like binding for `<Gamepad>/rightStick`).
8. Event line records: row (Lake, Camp 1 to 3, Office, or general), kind (tower, log, missed, night), unread flag, day. Import check for the 4.6 budgets.

## 11. States

| State | What shows |
|---|---|
| Closed | Nothing. |
| Opening | 0.2 s fade. Input ignored for 0.2 s so the open press does not choose. |
| Day, tower not done | Today with `___` stamps, File locked. |
| Day, all stamped | File choosable. |
| Day-end card open | DayEndConfirm.md over the dimmed page. |
| Night | Read-only Today (8.2). |
| Past day | Read-only page, `Back to today`. |
| Unread lines | New-note dots on the lines, the `<` arrow and the Today tab (4.7). |
| Log block full | Scrolls; `more` / `back up` marks (4.6.3). |
| Page before day 1 | Reached with `<` from day 1, read-only (4.7). |
| Opened from pause | Settings page only, Back returns to pause. |
| Forced close | Gone at once, no fade. |

## 12. Open questions

1. Stats in the book: the reading from the last Ward visit (draft), or live numbers? Live numbers would show event costs as they happen by day.
2. Does quitting mid-day have to lose the day (draft, save at sleep only), or should quit also save?
3. Map art and the cliff label wait on Main3 blockout and Quill.

Pim
