# Collectibles page

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (the player carries the logbook; later collectibles live in it), 2026-09-29 (the last page of the logbook is a collectible page of achievements that reference other games, for example the lamppost from The Beginner's Guide and the phone booth from Disco Elysium; a camp has a payphone as a Disco Elysium homage; unlocking the best ending is an achievement; the game saves only when the player sleeps). Host: Logbook.md. Type per Style.md 7 (Vesper). Which homages exist beyond the two named, and where they stand in Main3, is Vesper's list, not this spec.

## 1. Purpose

A quiet page at the back of the logbook that keeps the homages the player has found. It rewards looking around. It is never needed to progress and never hints at the story.

## 2. What counts as a homage

1. A world object that nods to another game: the payphone at a camp (Disco Elysium), a lamppost (The Beginner's Guide), more from Vesper.
2. Each one is an ordinary object in the world first. It fits the place and passes Style.md; the nod is for players who know.
3. Each has a stable ID (for example `homage_payphone`, `homage_lamppost`), a page drawing, a title and one line of text. Quill writes the words, Vesper draws.
4. Placement: Vesper and Sable, in Main3 dressing (Milestone 11). The lamppost has no place in Main3.md yet **[GAP: placement]**.

## 3. Finding one in the world

Two kinds of find. Each homage uses one.

| Kind | What the player does | Prompt |
|---|---|---|
| Use | Interacts with it (for example lifts the payphone receiver). | The object's normal prompt, lower centre, for example `Lift the receiver`. Nothing says it is special. |
| Stand | Stands in a spot (for example under the lamppost's light) for 2 s. | None. |

1. Keyboard and mouse: `E` for a Use find. Gamepad: `Y` (Interact, as today). Stand finds need no button.
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
|  KEPT                3 of 8 |  The receiver               |
|                             |                             |
|  +-----+ +-----+ +-----+    |   +---------------------+   |
|  | (1) | | (2) | |  3  |    |   |                     |   |
|  |draw.| |draw.| |     |    |   |   (ink drawing of   |   |
|  +-----+ +-----+ +-----+    |   |    the payphone)    |   |
|  +-----+ +-----+ +-----+    |   |                     |   |
|  |  4  | | (5) | |  6  |    |   +---------------------+   |
|  |     | |draw.| |     |    |                             |
|  +-----+ +-----+ +-----+    |   Someone is still on the   |
|  +-----+ +-----+            |   line. Day 6.              |
|  |  7  | |  8  |            |                             |
|  +-----+ +-----+            |                             |
+-----------------------------+-----------------------------+
```

1. Left page: a grid of frames, one per homage, in a fixed order (Vesper's list order). Three per row at Style.md 7.2 sizes; Rook to check fit at 1080 rows.
2. Top line: `KEPT` and `n of total`. The total is the real count of homages in the build.
3. Found frame: a small ink drawing (uGUI Image).
4. Unfound frame: an empty ruled frame with its number only. No silhouette, no title, no hint, no place name. The number shows the gap without saying what fills it.
5. Right page: the focused frame. Found: the title, a large drawing, one line of text, and the day and run it was first found (`Day 6`). Unfound: the frame number and `Not yet.`
6. The page does not name the source games **[Grant]**. The nod is the object, not a credit line.
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
3. Hover moves focus. Clicking a frame focuses it. `Enter` does nothing more.
4. `Esc` or `Tab` closes the book.

Gamepad:
1. `View` opens the logbook. `LB` and `RB` change tab; `RB` on Settings moves to this page.
2. D-pad or left stick move focus between frames.
3. `A` does nothing more.
4. `B`, `Start` or `View` closes the book.

Tab wrap: `RB` / `E` on this last tab does nothing (tabs do not wrap), matching Logbook.md.

## 7. Saving

1. A find is written at once to a profile file in persistentDataPath (draft name profile.json), separate from run.json and settings.json. This is not the run save: DECISIONS 2026-09-29 (save only at sleep) covers the run, and a homage is not part of the run. So a find counts even if the player quits before sleeping **[Grant]**.
2. The file holds found IDs, and the day and run of each first find. Nothing about the story.
3. Unreadable profile file: the page shows as if nothing is found, and the file is renamed to profile.old.json before a new one is written, never overwritten silently (MainMenu.md 4.1 pattern).
4. New run and Quit do not touch it.

## 8. Platform achievements (later)

The platform is not chosen. Which store, and whether its achievement API works from this Unity version, is unverified. Nothing here adds a package; any SDK is a separate ask to Grant.

1. Each homage ID maps to one platform achievement of the same name. The page is the source of truth; the platform is a copy.
2. On a find, the game unlocks the matching achievement at the same moment.
3. On every launch, the game sends every found ID again, so a find made offline or before the platform layer existed is unlocked later. Whether the platform allows that without side effects is unverified.
4. Platform achievement names and descriptions follow section 4.2 rules: unfound ones are hidden on the platform where it supports hidden achievements (unverified per platform).
5. The best-ending achievement (DECISIONS 2026-09-29) is a platform achievement. Draft: it is not a frame on this page, because this page is homages only and a frame for it would hint at the ending **[Grant]**.

Requirements for Rook:
1. A homage list asset: ID, frame number, find kind (Use or Stand), drawing, title key, text key. One component on each world object points to its ID.
2. Find records go through one call that writes the profile file, posts the Objective line and (later) calls the platform layer.
3. The Stand kind uses a trigger volume and a 2 s dwell on scaled time, so pause stops it; it counts only while the Player map is live (not in the logbook, not in a minigame).
4. The page is a logbook tab component; it reads the profile file when the book opens.

## 9. States

| State | What shows |
|---|---|
| Nothing found ever | No tab. |
| Some found | Tab last; grid with drawings and numbered empty frames. |
| All found | Every frame drawn; top line `8 of 8`. No extra reward on the page. |
| Profile unreadable | As nothing found; file kept as profile.old.json. |
| Night | Same page, read-only (nothing to choose anyway). |

## 10. Open questions

1. Does the page name the source games, or leave the nod unnamed (draft)? Grant.
2. Should a find count before the player sleeps (draft: yes, profile file written at once)? Grant.
3. Is the best-ending achievement a frame on this page, or platform only (draft)? Grant.
4. Which platform, and when. Grant.
5. The full homage list, placements and the lamppost's place in Main3. Vesper and Sable.

Pim
