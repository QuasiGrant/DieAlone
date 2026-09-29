# Content warning

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-29 (the game opens with content warnings; this is an adult game, resident deaths can be bloody and scary; the cultist cave has rave lights), 2026-09-29 (UI text is TextMeshPro; Overpass for warnings, menus and settings; the content warning screen is built in Milestone 10), 2026-09-20 (settings JSON in persistentDataPath). Warning list checked against Docs/Private/StoryBible.md; nothing in this file names who, where or why. Photosensitivity: Vesper's flag (reported by Wren; not found in a committed file) and the moving beams in LookBoards.md section 9. Type per Style.md 7 and Fonts.md. Hosts: game start, and MainMenu.md. Revised 2026-09-29: TextMeshPro and Overpass.

**Build: Milestone 10** (DECISIONS 2026-09-29), with the main menu.

Twist rule (DECISIONS 2026-09-28): every line here is committed text. It names subjects, never story.

## 1. Purpose

Tell the player, before anything plays, what the game contains, and let a player who is sensitive to flashing turn it down before any flashing can happen.

## 2. The list

Plain terms, one per line, no story context:

1. Suicide
2. Blood and violent death
3. Alcohol addiction and drink driving
4. Death of a pet
5. Depression **[Grant]**
6. Flashing lights (with the option in section 4)

Notes:
1. Lines 1 to 4 are Wren's brief. Line 3 widens "drink driving" to include addiction, which the story carries on its own (StoryBible section 15 draft says the same).
2. Line 5 comes from the premise in the story bible. It names a theme, not a person or an event, and does not touch the twist. Grant decides whether it goes on.
3. Considered and left off, for Grant: a child in danger (the story keeps it subtle and undecided; naming it would point at one storyline); memory loss (twist-adjacent); a named mental illness (not ruled on, and would point at one character); jump scares and sudden loud sounds (no scare devices are adopted yet, DECISIONS 2026-09-29).
4. The list is content. Quill owns final wording; it is revised when storylines are written. Adding a line never needs a layout change.

## 3. When it shows

1. **First launch**: before the main menu, after the Unity splash (if any). The main menu does not load its background until the screen is dismissed, so nothing moves behind it.
2. Shows once. A flag `warningSeen` in settings.json records it. If settings.json is missing or unreadable, defaults load (Settings.md 6) and the screen shows again. That is acceptable: showing it twice is safe, skipping it is not.
3. **From the main menu**: a `Content warnings` row, after Settings and before Quit. It opens the same screen over the dimmed menu. This adds a row to MainMenu.md; it is revised to match in its next pass.
4. Not in the pause menu or the logbook.

## 4. Photosensitivity

A single toggle: `Reduce flashing`. Default off. Shown on this screen and in Settings.md under Display, the same stored value (Settings.md revised to match).

When on:
1. No light, and no area of the screen, changes brightness sharply more than 3 times in any second. Draft threshold from the common three-flashes guideline; Vesper and Rook to confirm the rule we test against.
2. Cave rave lights: beams sweep slowly with steady brightness; no strobe, no on and off pulses.
3. Tape filter: noise bands off (Style.md 3 noiseBandStrength to 0). Grain and scan lines stay; they do not flash.
4. Any full-screen flash (event, scare, lightning, flare) is replaced by a short fade.
5. Fire flicker stays: it is slow and small. Vesper to confirm it passes rule 1.

When off, the game should still avoid full-screen strobing; the toggle removes what remains. Vesper owns what flashes in the look; Rook owns the check.

## 5. Layout

```
+--------------------------------------------------------------+
|                                                              |
|  (black. Tape filter off on this screen. No noise bands.)    |
|                                                              |
|     This game contains:                                      |
|                                                              |
|       Suicide                                                |
|       Blood and violent death                                |
|       Alcohol addiction and drink driving                    |
|       Death of a pet                                         |
|       Depression                                             |
|       Flashing lights                                        |
|                                                              |
|     [ ] Reduce flashing                                      |
|                                                              |
|     [ Continue ]    <- focus                                 |
|                                                              |
+--------------------------------------------------------------+
```

1. Black background, text left-aligned inside a 5 percent safe margin, Style.md 7 sizes (heading 36, list 28). No plate needed on black. All text TextMeshPro in Overpass: heading SemiBold, list, toggle label and button Regular **P** weights. No handwriting, no VT323 on this screen.
2. The tape filter is off on this screen so the text is at its most legible and nothing can flash before the player has chosen.
3. Heading line: `This game contains:` (Quill may change). No title card, no logo, no sound but the UI press sound.
4. From the main menu, the button reads `Back` instead of `Continue`.
5. A player who cannot read the screen is not covered by this draft; no screen reader in uGUI with TextMeshPro (unverified that one exists for it).

## 6. Dismissing it

1. Only `Continue` dismisses it. The toggle does not.
2. Input is locked for 1.5 s after it appears, so a button held through the splash does not skip it. `Continue` shows dimmed until the lock ends.
3. On first launch, `Esc` and `B` do nothing: there is nothing to go back to.
4. From the main menu, `Esc` and `B` act as `Back`, focus returns to the `Content warnings` row. No lock from the menu.
5. The toggle value saves when the screen closes (Settings.md requirement 3 pattern).

## 7. Input paths

Keyboard and mouse:
1. Arrow keys or `W` / `S` move focus between `Reduce flashing` and `Continue`.
2. `Enter` flips the toggle or presses `Continue`.
3. Mouse: click the toggle or the button; hover moves focus. The cursor is visible (MainMenu.md requirement 2).
4. `Esc`: section 6.

Gamepad:
1. D-pad or left stick up and down move focus.
2. `A` flips the toggle or presses `Continue`.
3. `B`: section 6. `Start` does nothing.

Default focus: `Continue`. The toggle is one press up.

Requirements for Rook:
1. UI map only; Player map off (MainMenu.md requirement 1).
2. `warningSeen` and `reduceFlashing` fields in PlayerSettingsData with defaults false (Settings.md requirement 1 pattern).
3. A single read of `reduceFlashing` that the look filter, cave lights and any flash effect check. Look code reads it; this screen only writes it.
4. The tape filter must be off for this screen. How (a camera without the renderer feature, or the feature skipped on a flag) is Rook's call; confirm it is possible before building.
5. Lock timer on unscaled time.

## 8. States

| State | What shows |
|---|---|
| First launch | Screen, Continue locked 1.5 s, then main menu on Continue. |
| Later launches | Skipped; main menu first. |
| From main menu | Same screen over the dimmed menu, `Back`, no lock. |
| Reduce flashing on | Toggle ticked here and in Settings. |
| Settings file unreadable | Shows again as on first launch. |

## 9. Open questions

1. Depression on the list: yes or no. Grant.
2. The four items left off (section 2 note 3). Grant.
3. The flash threshold we test against, and which effects flash today. Vesper and Rook.
4. Should the screen also show once after an update that adds a new line to the list? Draft: no. Grant.

Pim
