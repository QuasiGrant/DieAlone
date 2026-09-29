# Collectibles page

**DRAFT, 2026-09-29, revised 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (the player carries the logbook; later collectibles live in it), 2026-09-29 (the last page of the logbook is a collectible page of achievements that reference other games, for example the lamppost from The Beginner's Guide and the phone booth from Disco Elysium; a camp has a payphone as a Disco Elysium homage; unlocking the best ending is an achievement; the game saves only when the player sleeps; many more homages are wanted, in their own milestone later; the collectibles name the games they shout out, the team's favourite games and inspirations; a find counts straight away; collectibles are also Steam achievements; the page shows only inspiration collectibles, never other game achievements). Host: Logbook.md. Type per Style.md 7 (Vesper). The homage list is Vesper's (section 2); where each stands in Main3 is Vesper and Sable's, not this spec.

## 1. Purpose

A quiet page at the back of the logbook that keeps the homages the player has found. It rewards looking around. It is never needed to progress and never hints at the story.

## 2. What counts as a homage

1. A world object that nods to another game. Each one is an ordinary object in the world first. It fits the place and passes Style.md; the nod is for players who know.
2. Each has a stable ID, a page drawing, a title, one line of text, and the name of the game it shouts out. Quill writes the words, Vesper draws. The game names are the team's favourite games and inspirations (DECISIONS 2026-09-29).
3. Only inspiration collectibles appear here. No other game achievement (the best ending or any later one) is ever a frame on this page.
4. The current list, Vesper's five (placements and reasons are in a private note, Vesper and Sable own them):

| Frame | ID | Object | Game named | Find kind (draft) |
|---|---|---|---|---|
| 1 | `homage_lamppost` | A lit lamppost in the woods | The Beginner's Guide | Stand, in its light |
| 2 | `homage_payphone` | The camp payphone | Disco Elysium | Use, lift the receiver |
| 3 | `homage_pony` | A small plastic pony | Pony Island | Use, pick up and look |
| 4 | `homage_squirrel` | A hand-drawn squirrel card | Inscryption | Seen, turned face up during an activity |
| 5 | `homage_redshell` | One fired red shell | Buckshot Roulette | Use, pick up and look |

5. Many more are wanted (DECISIONS 2026-09-29, own milestone later); the page and the count grow with the list, no layout change. At 3 per row the left page holds 12 frames; past that the grid pages with `<` `>` at the page foot, as Logbook.md 4.3 **[GAP: final count]**.

## 3. Finding one in the world

Three kinds of find. Each homage uses one.

| Kind | What the player does | Prompt |
|---|---|---|
| Use | Interacts with it (for example lifts the payphone receiver). | The object's normal prompt, lower centre, for example `Lift the receiver`. Nothing says it is special. |
| Stand | Stands in a spot (for example under the lamppost's light) for 2 s. | None. |
| Seen | The object comes up during an activity (for example a card turned face up in a card game). It counts when it is face up on screen. | None; the activity's own input. |

1. Keyboard and mouse: `E` for a Use find. Gamepad: `Y` (Interact, as today). Stand finds need no button. Seen finds use whatever the activity uses; the activity spec owns that path.
2. On the first find of that homage, ever: one page sound (Hollis, UI group), and one line through the Objective.md queue: `Logbook: a page kept.` (Quill may change the words). Nothing else: no pop-up, no icon, no pause.
3. Finding it again, in this run or a later one, does nothing extra. The object still does its normal thing.
4. The first find ever also reveals the page (section 4.4).
5. Finds are not blocked by time of day, but at night the player can only go to the Ward (DECISIONS 2026-09-28), so in practice finds happen by day. A homage on the Ward path could be found at night; Vesper to avoid that unless wanted.
6. A find never costs HP, MIND or WARD, never counts as a need, and never starts an event.

## 4. The page

### 4.1 Where it sits

The last tab of the logbook, after Settings. This follows DECISIONS 2026-09-29 ("the last page") and changes Logbook.md section 3, which put reserved tabs before Settings. Logbook.md is revised to match in its next pass.

### 4.2 Layout

```
+-----------------------------+-----------------------------+
|  KEPT                3 of 5 |  The receiver               |
|                             |                             |
|  +-----+ +-----+ +-----+    |   +---------------------+   |
|  | (1) | | (2) | |  3  |    |   |                     |   |
|  |draw.| |draw.| |     |    |   |   (ink drawing of   |   |
|  +-----+ +-----+ +-----+    |   |    the payphone)    |   |
|  +-----+ +-----+            |   |                     |   |
|  |  4  | | (5) |            |   +---------------------+   |
|  |     | |draw.|            |   Disco Elysium             |
|  +-----+ +-----+            |   (one line, Quill). Day 6. |
|                             |                             |
|                             |                             |
|                             |                             |
+-----------------------------+-----------------------------+
```

1. Left page: a grid of frames, one per homage, in a fixed order (Vesper's list order). Three per row at Style.md 7.2 sizes; Rook to check fit at 1080 rows.
2. Top line: `KEPT` and `n of total`. The total is the real count of homages in the build.
3. Found frame: a small ink drawing (uGUI Image).
4. Unfound frame: an empty ruled frame with its number only. No silhouette, no title, no hint, no place name. The number shows the gap without saying what fills it.
5. Right page: the focused frame. Found: the title, a large drawing, the name of the game it shouts out, one line of text, and the day and run it was first found (`Day 6`). Unfound: the frame number and `Not yet.`
6. The game name is shown on the page only after the find. In the world the object stays an ordinary object with its normal prompt; the name never appears on it or in its prompt. The name is plain text, the game's title as its makers write it, no logo. Whether naming the games needs any trademark check before release is unverified (MullinsNotes 4.4); Grant's call.
7. Legacy Text only. No colour-only states: found and unfound differ by drawing and text.

### 4.3 Across runs

1. The page keeps every homage ever found, across all runs. A new run does not clear it.
2. It is saved in its own file, not in run.json (section 7).

### 4.4 Hidden until the first find

1. Before any homage has been found, the tab is not shown (Logbook.md rule: a tab with no content is hidden, not greyed).
2. After the first find it shows in every run from then on, including a new run's day 1.

## 5. Opening and reading

1. Opens as any logbook tab: by day fully, at night read-only (Logbook.md 8.1, 8.2). There is nothing to choose on this page, so night and day look the same.
2. Not reachable from the pause menu or the main menu. Not a new screen: it is a logbook tab only.
3. Focus opens on frame 1, or the last focused frame if the page was closed on this tab.

## 6. Input paths

Keyboard and mouse:
1. `Tab` opens the logbook. `Q` and `E` change tab; `E` on Settings moves to this page. Clicking the tab opens it.
2. Arrow keys or `W` `A` `S` `D` move focus between frames, in grid order; left at a row start wraps to the row above.
3. Hover moves focus. Clicking a frame focuses it. `Enter` does nothing more. With more than 12 frames: right from the last column of the grid, or clicking `>` at the page foot, turns to the next grid page; left from the first column, or `<`, turns back.
4. `Esc` or `Tab` closes the book.

Gamepad:
1. `View` opens the logbook. `LB` and `RB` change tab; `RB` on Settings moves to this page.
2. D-pad or left stick move focus between frames. With more than 12 frames, right from the last column turns to the next grid page, left from the first column turns back.
3. `A` does nothing more.
4. `B`, `Start` or `View` closes the book.

Tab wrap: `RB` / `E` on this last tab does nothing (tabs do not wrap), matching Logbook.md.

## 7. Saving

1. A find is written at once to a profile file in persistentDataPath (draft name profile.json), separate from run.json and settings.json. This is not the run save: DECISIONS 2026-09-29 (save only at sleep) covers the run, and a homage is not part of the run. So a find counts straight away, even if the player quits before sleeping (DECISIONS 2026-09-29).
2. The file holds found IDs, and the day and run of each first find. Nothing about the story.
3. Unreadable profile file: the page shows as if nothing is found, and the file is renamed to profile.old.json before a new one is written, never overwritten silently (MainMenu.md 4.1 pattern).
4. New run and Quit do not touch it.

## 8. Steam achievements

Collectibles are also Steam achievements (DECISIONS 2026-09-29). The Steamworks API, and which Unity wrapper would call it from 6000.3.24f1, is unverified. Nothing here adds a package; the SDK or wrapper is a separate ask to Grant.

1. Each homage ID maps to one Steam achievement with the same ID. The page is the source of truth; Steam is a copy.
2. On a find, the game unlocks the matching Steam achievement at the same moment it writes the profile file.
3. On every launch, the game sends every found ID again, so a find made offline, or before the Steam layer existed, is unlocked later. Whether re-sending an unlocked achievement is harmless on Steam is unverified.
4. Steam names follow the page: the title, and the game it names in the description. Unfound ones are set hidden in the Steam achievement settings, if Steam supports hidden achievements (unverified), so the list does not spoil what to look for.
5. Other achievements (the best ending, DECISIONS 2026-09-29, and any later) are Steam achievements only. They never appear on this page (DECISIONS 2026-09-29: the page shows only inspiration collectibles). This spec does not cover them.

Requirements for Rook:
1. A homage list asset: ID, frame number, find kind (Use, Stand or Seen), drawing, title key, text key. One component on each world object points to its ID.
2. Find records go through one call that writes the profile file, posts the Objective line and (later) calls the Steam layer. The Steam call must never block or fail the find: no Steam, no network, or an SDK error, and the page still records it.
3. The Stand kind uses a trigger volume and a 2 s dwell on scaled time, so pause stops it; it counts only while the Player map is live (not in the logbook, not in a minigame).
4. The page is a logbook tab component; it reads the profile file when the book opens.

## 9. States

| State | What shows |
|---|---|
| Nothing found ever | No tab. |
| Some found | Tab last; grid with drawings and numbered empty frames. |
| All found | Every frame drawn; top line `5 of 5`. No extra reward on the page. |
| Profile unreadable | As nothing found; file kept as profile.old.json. |
| Night | Same page, read-only (nothing to choose anyway). |

## 10. Open questions

1. Closed 2026-09-29: the page names the games (section 4.2).
2. Closed 2026-09-29: a find counts straight away (section 7).
3. Closed 2026-09-29: the best ending is not on this page (section 8.5).
4. Closed 2026-09-29: Steam. Open: the SDK or wrapper, and when it is added. Grant, after Rook verifies.
5. Trademark check on naming the games before release: unverified. Grant.
6. Placements in Main3 dressing (Milestone 11), and whether a homage inside an activity that can be lost for good stays findable in a later run. Vesper and Sable.

Pim
