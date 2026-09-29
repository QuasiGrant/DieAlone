# Main menu

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: PLAN.md Later ("Main menu with New run and Continue"), DECISIONS.md 2026-09-20 (settings JSON in persistentDataPath), 2026-09-28 (a run ends the first time HP, MIND or WARD reaches 0), 2026-09-29 (day one is an ordinary lookout job; no fire visible before nightfall on day one; UI text is TextMeshPro; fonts per role; the main menu is built in Milestone 10). Save facts from Assets/Scripts/Core/GameSave.cs (read only): one file, run.json, version 1, written at sleep; Load returns Loaded, Missing, Corrupt, UnsupportedVersion or TuningMismatch and never throws. Type per Style.md 7 and Fonts.md (Vesper). Revised 2026-09-29: Content warnings row and first-launch screen (ContentWarning.md, DECISIONS 2026-09-29 content warnings line). Revised 2026-09-29: TextMeshPro and fonts per role.

**Build: Milestone 10** (DECISIONS 2026-09-29). The decision names New run, Continue, Settings and Quit; the Content warnings row comes from the content warning line and ContentWarning.md, also Milestone 10. Row order stays as section 2.

## 0. Type

All text is TextMeshPro (the TextMeshProUGUI component on a uGUI canvas). No legacy Text.

| Text | Font | Weight **P** |
|---|---|---|
| Title `DieAlone` | Overpass | SemiBold |
| Rows, `Day n`, sublines, confirm card | Overpass | Regular |
| `PLAY >  SP` | VT323 | Regular, caps |

Sizes and colour: Style.md 7. VT323 appears only in the tape mark, never in a row or line.

## 1. Purpose

Start a run, go back to the saved run, change settings before playing, quit. Nothing else.

## 2. Layout

```
+--------------------------------------------------------------+
|                                                              |
|  (still shot: the camp at late afternoon, day-one palette,   |
|   no fire, tape filter on, slow grain)                       |
|                                                              |
|  DieAlone                                                    |
|                                                              |
|  [ Continue ]      Day 14                                    |
|  [ New run ]                                                 |
|  [ Settings ]                                                |
|  [ Content warnings ]                                        |
|  [ Quit ]                                                    |
|                                                              |
|                                               PLAY >  SP     |
+--------------------------------------------------------------+
```

1. Background: a fixed camera in a menu scene or in Main3, day-one look (Style.md 2.0), never fire or runes, so the menu does not spoil night 1. The tape filter runs over it like the game.
2. Title and list lower left, inside a 5 percent safe margin. Overpass (section 0), Style.md 7 sizes and colours.
3. Bottom right: a tape OSD mark (`PLAY >`), static, VT323. Vesper may drop it.
4. `Day n` beside Continue is the day the saved run wakes into. Nothing else about the run is shown here (no stats: DECISIONS 2026-09-28).
5. Rows, top to bottom: Continue, New run, Settings, Content warnings, Quit. Continue is not shown when there is nothing to continue (section 4).

## 3. Rows

| Row | Does |
|---|---|
| Continue | Loads run.json and wakes the player in the cabin on the saved day. |
| New run | Starts day 1. If a save exists, confirm first (section 5). |
| Settings | The settings panel (Settings.md), full screen over the dimmed menu. Back returns here, focus on Settings. |
| Content warnings | The content warning screen (ContentWarning.md) over the dimmed menu, with `Back` and the `Reduce flashing` toggle. Back returns here, focus on Content warnings. |
| Quit | Quits. No confirm: nothing is lost at the menu. In the Editor it stops Play mode (PauseMenu.cs pattern). |

Default focus: Continue if it is shown and loads, else New run.

## 4. Continue and the save file

GameSave.Load runs once when the menu opens. The result sets the row:

| LoadResult | Continue row | On choosing it |
|---|---|---|
| Loaded, run live | `Continue` + `Day n` | Loads the run. |
| Loaded, run ended | Not shown | A run that ended cannot be continued. **[GAP: Milestone 7 endings]** for what an ended run leaves in the file. |
| Missing | Not shown | |
| Corrupt | `Continue` dimmed, subline `This save cannot be read.` | Not choosable; focus skips it. |
| UnsupportedVersion | `Continue` dimmed, subline `This save is from an older version.` | Not choosable; focus skips it. |
| TuningMismatch | Same as UnsupportedVersion. | Not choosable. |

1. An unreadable save is never deleted silently and never overwritten without the New run confirm.
2. The dimmed row stays so the player sees their run is known about, not lost without a word.
3. The Continue load happens again on choosing, in case the file changed; if it now fails, the row updates in place and focus moves to New run. No error box.

## 5. New run confirm

Shown only when run.json exists (any result except Missing).

```
+------------------------------------------+
|  Start a new run?                        |
|  Your saved run will be replaced.        |
|                                          |
|     [ Not yet ]       [ New run ]        |
|      ^ focus                             |
+------------------------------------------+
```

1. Unreadable save: second line reads `The old save will be kept as run.old.json.` and the file is renamed, not deleted.
2. Readable live save: the file is replaced by the new run's first save at the first sleep. Until then it stays on disk; quitting before the first sleep leaves it in place **[Rook to confirm this fits GameSave]**.
3. New run never touches profile.json (homage finds, Collectibles.md 7) or settings.json.
4. `Not yet` is default focus. Input locked 0.4 s after opening (DayEndConfirm.md 5.3 pattern).

## 6. States

| State | What shows |
|---|---|
| First launch | The content warning screen first (ContentWarning.md 3); the menu and its background load after Continue. Then New run (focus), Settings, Content warnings, Quit. |
| Live save | Continue (focus), New run, Settings, Content warnings, Quit. |
| Unreadable save | Continue dimmed with subline, New run (focus), Settings, Content warnings, Quit. |
| Content warnings | ContentWarning.md screen over the dimmed menu. |
| New run confirm | Card over the dimmed menu. |
| Settings | Settings.md panel. |
| Loading | Menu fades to black 0.5 s; tape noise band on the fade (look filter). |
| Returned from a game (Quit to menu) | Same as launch; the load check runs again. |

## 7. Input paths

Keyboard and mouse: arrow keys or `W` / `S` move focus; `Enter` chooses; click chooses, hover moves focus; `Esc` on the list does nothing, on the confirm card acts as `Not yet`, in Settings and Content warnings acts as Back.

Gamepad: D-pad or left stick up and down; `A` chooses; `B` on the list does nothing, on the confirm card acts as `Not yet`, in Settings and Content warnings acts as Back. `Start` does nothing.

Requirements for Rook:
1. The menu uses only the UI map. The Player map is off in the menu scene.
2. The cursor is visible and unlocked in the menu; hidden and locked on entering the game.
3. When the mouse moves, hover focus takes over; when a pad or key is used, the selected row returns (one highlight only).
4. Quit to menu from the pause menu (Logbook.md 9.3) lands here.

## 8. Open questions

1. Menu background: its own small scene, or a camera in Main3? Main3 loads slower and risks showing night state; a small scene is cheaper. Vesper and Rook.
2. What an ended run leaves in run.json **[GAP: Milestone 7 endings]**.

Pim
