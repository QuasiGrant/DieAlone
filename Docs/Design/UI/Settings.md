# Settings

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-20 (player settings persist in a JSON file in persistentDataPath, separate from PlayerTuning; input is controller plus keyboard and mouse), 2026-09-28 (settings live in the carried logbook), 2026-09-29 (UI text is TextMeshPro; Overpass for settings; the main menu, whose Settings row hosts this panel, is built in Milestone 10). Sources read: Assets/Scripts/Settings/PlayerSettings.cs and Assets/Scripts/UI/PauseMenu.cs (read only; today: look sensitivity 0.2 to 3, default 1, and invert look, both in the pause menu, file settings.json), Docs/Design/Sound/Mixer.md section 4 (four volume sliders, Hollis, **[Grant yes]** pending), PLAN.md Later (volume, graphics and control rebinding). Type per Style.md 7 and Fonts.md (Vesper). Revised 2026-09-29: Reduce flashing toggle (ContentWarning.md). Revised 2026-09-29: TextMeshPro and Overpass (section 3 note 6).

## 1. Purpose

One settings panel, built once, shown in three hosts:
1. **Logbook**: the Settings tab (Logbook.md 7), by day and at night.
2. **Pause**: the pause menu's `Settings` row opens the same page alone, time frozen (Logbook.md 9.2). Reaches settings where the book cannot open: binoculars, the Ward screen, dialogue.
3. **Main menu**: `Settings` row (MainMenu.md 3).

"Logbook" in the table below means hosts 1 and 2.

## 2. Every setting

| Section | Setting | Control | Range, default | Lives in | Source |
|---|---|---|---|---|---|
| Controls | Look sensitivity | slider + value | 0.2 to 3.0, step 0.05, default 1.00 | both | exists (PlayerSettings) |
| Controls | Invert look | toggle | off | both | exists (PlayerSettings) |
| Sound | Master | slider | 0 to 100, step 5, default 80 | both | Mixer.md 4 |
| Sound | Sound (ambience and world) | slider | same | both | Mixer.md 4 |
| Sound | Voices | slider | same, goes to 0 | both | Mixer.md 4.4 |
| Sound | Music | slider | same | both | Mixer.md 4 |
| Display | Display mode | stepper | Fullscreen, Borderless, Windowed; default Borderless | both | PLAN Later |
| Display | Resolution | stepper | the monitor's list; default native | both | PLAN Later |
| Display | V-sync | toggle | on | both | PLAN Later |
| Display | Brightness | slider | proposal only, see 2.5 | both | **[Vesper]** |
| Display | Reduce flashing | toggle | off | both, and the content warning screen | ContentWarning.md 4 |
| Controls | Rebind keys and buttons | list, later | defaults from InputSystem_Actions | main menu only | PLAN Later |
| All | Reset section to defaults | button per section | | both (rebind: main menu) | |

1. Sound sliders show 0 to 100 on screen and store 0 to 1 (Mixer.md 4.1). UI sounds follow Master only (Mixer.md 4).
2. Voices at 0: the Voices mechanic must still work as text (Mixer.md 4.4). That text belongs to Milestone 13 (Dialogue.md); this panel only says under the slider, at 0: `Voices will show as text.` **[GAP: Dialogue]**.
3. Invert look inverts vertical only (current code has one bool; Rook to confirm it is vertical).
4. One look sensitivity for mouse and stick, as today. A separate stick value is not in scope.
5. Brightness: a horror game usually offers it, but it moves the picture Vesper tunes. Draft: not built until Vesper says whether it exists and what it scales (exposure offset within a small range, never touching the filter). Listed so it is not forgotten.
6. Rebinding is main menu only: rebinding mid-run can strand the player (for example unbinding the logbook button while the book is open). It gets its own spec when planned. Pause and the logbook show no rebinding row until then; rows that do not exist yet are hidden, not greyed.
7. Display mode and Resolution apply at once and open a keep card (section 5).
8. Reduce flashing applies at once, no keep card. What it changes is ContentWarning.md 4. One stored value, shared with the content warning screen.

## 3. Layout (same in all hosts)

```
+-------------------------------------------------+
|  SETTINGS                                       |
|                                                 |
|  Controls                                       |
|    Look sensitivity   [=====|------]  1.00      |
|    Invert look        [ ]                       |
|  Sound                                          |
|    Master             [=======|----]  80        |
|    Sound              [=======|----]  80        |
|    Voices             [=======|----]  80        |
|    Music              [=======|----]  80        |
|  Display                                        |
|    Display mode       < Borderless >            |
|    Resolution         < 1920 x 1080 >           |
|    V-sync             [x]                       |
|    Reduce flashing    [ ]                       |
|    Reset to defaults                            |
|                                                 |
|  [ Back ]      (pause and main menu hosts only) |
+-------------------------------------------------+
```

1. One column, one row per setting, section headings not choosable.
2. Longer than the page in the logbook: the page scrolls to keep the focused row in view. Two logbook pages side by side (Controls and Sound left, Display right) if it fits at Style.md 7.2 sizes; Rook to check at 1080 rows.
3. In the logbook host there is no Back row: the book's close does it. In pause and main menu hosts, `Back` is last.
4. Values change live: sound as the slider moves, look sensitivity on the next frame.
5. uGUI: Slider and Toggle exist. The stepper `< value >` is a Selectable that takes left and right (Rook builds it); no Dropdown (legacy or TMP_Dropdown), because a dropdown list is awkward on a pad.
6. Type: all text TextMeshPro in Overpass, in every host, including the logbook host. The panel is printed, not handwritten, even on the ruled page. `SETTINGS` and section headings SemiBold **P**; rows, values, notes, `Back` and the keep card Regular. No Patrick Hand, no VT323.
7. Fit: same rule as MainMenu.md 2.6 in the pause and main menu hosts. The list does not shrink to fit; it scrolls with the focused row kept in view (as in 3.2), headings and `Back` never off screen.

## 4. Input paths

Keyboard and mouse:
1. Arrow keys or `W` / `S` move between rows.
2. Left and right arrows or `A` / `D`: slider one step, stepper one value. `Enter` flips a toggle or presses a button.
3. Mouse: drag or click a slider, click a toggle, click the stepper arrows. Hover moves focus. Wheel scrolls the page.
4. `Esc`: in the logbook, closes the book; in pause and main menu hosts, Back.

Gamepad:
1. D-pad or left stick up and down move between rows.
2. D-pad or left stick left and right change the focused slider or stepper; holding repeats after 0.4 s, every 0.08 s.
3. `A` flips a toggle or presses a button.
4. `B`: in the logbook, closes the book; in pause and main menu hosts, Back. `Start` does the same as `B` here.

Requirements for Rook:
1. One panel prefab, three hosts. Adding a setting follows the pattern in PauseMenu.cs: a field in PlayerSettingsData with a default, one Bind method.
2. Look sensitivity and invert move from PauseMenu to the panel; PauseMenu keeps Resume, Settings, Quit rows (Logbook.md 9).
3. Save when the panel closes or the host changes, not on every slider tick. Today BindLookSensitivity calls PlayerSettings.Save on every value change, which writes the file on each step of a drag.
4. Sound sliders write the mixer's exposed parameters through the gain stage Mixer.md 4.3 asks Rook to confirm.
5. Display: Screen.fullScreenMode, Screen.resolutions, Screen.SetResolution, QualitySettings.vSyncCount. Rook confirms each in 6000.3.24f1 before use (not verified here). Display settings are also stored in the settings JSON.
6. While the panel is open in the pause host, `Esc` and `Start` go to Back, not Resume (modal flag, Logbook.md 10.3).
7. Panel animations on unscaled time (pause host runs at timeScale 0).
8. `reduceFlashing` and `warningSeen` fields in PlayerSettingsData (ContentWarning.md 7). Display Reset to defaults resets `reduceFlashing` to off; it never resets `warningSeen`.
9. Homage finds are not settings: they live in profile.json (Collectibles.md 7), never in settings.json, and no reset here touches them.

## 5. Keep display card

After Display mode or Resolution changes:

```
+----------------------------------------+
|  Keep this display?                    |
|  Going back in 10                      |
|                                        |
|     [ Go back ]       [ Keep ]         |
|      ^ focus                           |
+----------------------------------------+
```

1. Counts down 10 s on unscaled time; at 0 it reverts and closes.
2. Default focus `Go back`, so a player who cannot see the screen gets it back by pressing nothing.
3. `Esc` / `B` act as `Go back`.

## 6. States

| State | What shows |
|---|---|
| Open, logbook host | Settings page, no Back row. |
| Open, pause host | Settings page alone over the paused game, Back last. |
| Open, main menu host | Full panel over the dimmed menu, Back last. |
| Voices at 0 | Note under the slider (2.2). |
| Keep card | Section 5. |
| Settings file unreadable | Defaults load silently (PlayerSettings.Load does this today). No error shown. |

## 7. Open questions

1. Brightness: exists or not, and what it scales (Vesper).
2. Mixer.md section 4 still waits on Grant's yes; the Sound rows follow it.

Pim
