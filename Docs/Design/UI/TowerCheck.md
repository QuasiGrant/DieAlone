# Tower check: lectern and binoculars

**DRAFT, 2026-09-28, revised 2026-09-29 (second pass), Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (daily loop, cave not checked, day and night states, night allows only the Ward), 2026-09-29 (the day ends by filing the report or by an event; no time budget, so no clock in the binocular view; routine in Docs/Design/DailyLoop.md revision 6 approved for now). Terms (Wren, 2026-09-29): stamps are `SAFE` and `CHECK ON FOOT`; locations are Lake, Camp 1, Camp 2, Camp 3, Office (DailyLoop.md, Main3.md). Colours and type per Style.md 7 and Fonts.md (Vesper); the warning colour is still **[GAP: Style]**. Revised 2026-09-29 to DECISIONS 2026-09-29 (UI text is TextMeshPro; Overpass for forms, Patrick Hand for handwriting, VT323 only for the tape overlay): section 3.1.

## 1. Purpose

Every day the player climbs the tower and looks at each location except the cultist cave. The game stamps each one SAFE or CHECK ON FOOT from that day's events. The player only has to look; spotting is not a skill here (DECISIONS 2026-09-28).

## 2. Where it lives

1. A lectern in the tower cab, facing the widest window. On it: a pair of binoculars and a clipped sheet, the tower sheet.
2. The tower sheet is a world-space uGUI canvas on the lectern, readable by walking up to it. It is the diegetic summary: one row per location, a pencil mark appears when checked.
3. Locations checked (DailyLoop.md 1.3): Lake, Camp 1, Camp 2, Camp 3, Office. The cave is never on the sheet.

## 3. Wireframes

Tower sheet on the lectern (world space, seen at arm's length):

```
+----------------------------------+
|  TOWER SHEET          DAY 14     |
|                                  |
|  LAKE ...... SW   SAFE           |
|  CAMP 1 .... S    ___            |
|  CAMP 2 .... ESE  CHECK ON FOOT  |
|  CAMP 3 .... NNW  ___            |
|  OFFICE .... E    ___            |
|                                  |
|  (blank line until all checked)  |
+----------------------------------+
```

The bearing next to each name tells the player which way to face. Bearings follow the Main3 layout **[GAP: Main3 revision]**.

Binocular view (screen space overlay, full screen):

```
+--------------------------------------------------------------+
|                           SW 222                             |
|        .-------------.          .-------------.              |
|      /                 \      /                 \            |
|     |                   |    |                   |           |
|     |            ( o )  |    |  ( o )            |           |
|     |                   |    |                   |           |
|      \                 /      \                 /            |
|        '-------------'          '-------------'              |
|                                                              |
|                       LAKE  . . .                            |
|                                                              |
|                 [E] [RMB] lower   or   (Y) (B) lower         |
+--------------------------------------------------------------+
```

1. Two-circle binocular mask, black outside. The world view is inside it at a narrow field of view.
2. `( o )` is the inner ring: a thin circle at the centre of the view, radius about 15 percent of screen height. It is the look target.
3. Top centre: bearing readout, compass letters and degrees, updated as the player turns. Matches the letters on the tower sheet.
4. Lower centre: the result line. Empty until a location is in the inner ring.
5. Bottom: the exit hint, shown for the first 3 s of each use, then hidden. It shows the glyphs for the device used last: `[E] [RMB] lower` on keyboard and mouse, `(Y) (B) lower` on a gamepad, switching live if the player changes device. Glyphs are small uGUI Images: a rounded key cap with a letter for keys, a circle with a letter for pad buttons, drawn in code like the tent icon (no new asset pack). Face-button letters follow the Xbox layout; PlayStation glyphs are out of scope until Grant says otherwise.

### 3.1 Type

All text TextMeshPro. The tower sheet is a world-space canvas, so its text renders through the render scale and the tape filter; SDF keeps its edge better than legacy Text there (FontTech.md 4.3, look through the filter unverified).

| Text | Font |
|---|---|
| Tower sheet printed parts: `TOWER SHEET`, `DAY`, location names, bearings, dot leaders | Overpass, caps; heading SemiBold, rows Regular |
| Tower sheet stamps `SAFE`, `CHECK ON FOOT`, footer `Checked.` | Overpass caps, stamped (Fonts.md 3.2) |
| Tower sheet pencil fill-in: the day number, the tick beside a stamped row | Patrick Hand |
| Binocular bearing readout, result line (`LAKE . . .`, `CAMP 2    CHECK ON FOOT`, `All checked.`), exit hint letters | Overpass; the result line SemiBold **P** |
| Lectern prompt `Use binoculars` | Overpass (interact prompt) |

No VT323: the binocular readout is part of the binoculars, not the tape overlay.

## 4. How a location is checked (look-to-unlock)

1. Each location has one designated view: a fixed direction (bearing and elevation) from the deck eye to its landmark, set by the scene recipe. Every designated view must be clear from the deck on an ordinary day: through a window opening, not a wall or post, and over the trees (Main3 sightline rules). Marlow checks each one on the blockout.
2. The look counts on the bearing alone. The check is the angle between the view centre and the designated direction; there is no line-of-sight ray. If smoke, fog, trees or an event hide the landmark that day, the look still counts. An obscured view can itself be that day's anomaly.
3. When the designated direction is inside the inner ring, the result line shows the name and three dots fill in over 2.0 s (DailyLoop.md 1.3): `LAKE .`, `LAKE . .`, `LAKE . . .`.
4. If the direction leaves the ring before the dots fill, the line clears and the timer resets.
5. When the dots fill, the game stamps the location from that day's events. The line becomes `LAKE    SAFE` or `CAMP 2    CHECK ON FOOT`. A pencil-scratch sound plays (Hollis). The tower sheet row and the logbook line get the same stamp.
6. A location already marked today shows its result at once when it enters the ring. No dwell.
7. Only one direction can be in the ring at a time. If two overlap, the one nearest the ring centre wins. Designated views are at least 20 degrees apart, so this is rare.
8. When the last location is marked, the line reads `All checked.` for 2 s. The view stays up until the player lowers it.
9. The tower check can always be completed on any day. Nothing in the world can block a designated view from counting, so filing the report can never be blocked forever by the tower. If an event blinds the tower, the line can also be stamped on foot (DailyLoop.md 1.6); that is the event's content, not this screen.
10. When all five are stamped, the objective line posts after the binoculars are lowered: all safe, or one line per CHECK ON FOOT (Objective.md 4). It never interrupts the view.
11. Day 1: all five stamp SAFE (DailyLoop.md 6.1). Nothing on the sheet or in the view names the Ward.

## 5. How SAFE or CHECK ON FOOT is shown

1. The word, never colour alone. SAFE and CHECK ON FOOT differ in length and shape so they read through the VHS filter.
2. SAFE in the text colour of Style.md 7.3. CHECK ON FOOT in the warm warning colour **[GAP: Style]**.
3. The world carries the event (thick smoke, torn sheet). That is scene content, not UI. The mark only confirms.

## 6. States

| State | What shows |
|---|---|
| Day, tower not checked | Lectern prompt `Use binoculars`. Sheet rows blank. |
| Binoculars up, nothing in ring | Mask, bearing, empty result line. |
| Target in ring, dwelling | Name plus filling dots. |
| Stamped SAFE / CHECK ON FOOT | Name plus stamp. Sheet row and logbook line updated. |
| All marked | `All checked.` for 2 s. Sheet footer: `Checked.` |
| Day, already checked | Prompt `Use binoculars` still works. Every target shows its result at once. |
| Night | No prompt at the lectern. Binoculars cannot be raised (night allows only the Ward). |
| Paused | Pause menu over the binocular view. Resume returns to the same view. |
| Event ends the day while binoculars up | Binoculars lower at once (0.3 s fade), no marks added. Event sequence, then WardNight.md section 2.4. No day-end card. |
| Carrying an object | No prompt (PlayerInteractor only offers Set down while carrying). |

## 7. Camera and feel

1. Raise and lower: 0.3 s fade through black, then the mask. No zoom animation.
2. Field of view in binoculars: 15 degrees vertical (tuning number).
3. Look speed scales down with the field of view, so a stick push turns the same screen distance as normal.
4. Movement, jump, crouch, sprint and throw are off while the binoculars are up.
5. Standing position is fixed at the lectern. Turning is free through 360 degrees. The cab walls block the picture where there is no window, but every designated view falls in a window (section 4.1).

## 8. Input paths

Keyboard and mouse:
1. Walk to the lectern. `E` to raise.
2. Mouse to look. Hold the landmark in the inner ring.
3. `E` or right mouse to lower.
4. `Esc` opens the pause menu, as everywhere.

Gamepad:
1. Walk to the lectern. `Y` (north) to raise.
2. Right stick to look.
3. `Y` or `B` (east) to lower.
4. `Start` opens the pause menu.

Requirements for Rook:
1. The Player action map is off from the moment the binoculars go up until they come down: Move (WASD, left stick), Jump (Space, A), Crouch (C, B), Sprint, Attack (Enter, left mouse, X), Previous and Next (1, 2, D-pad left and right), Interact and Throw. Look is read by the binocular component itself. The map comes back on when the binoculars are lowered.
2. Because Interact (`E`, `Y`), Throw (right mouse) and Crouch (`B`) are in the Player map, the binocular component reads its own Lower bindings for `E`, right mouse, `Y` and `B` while the map is off.
3. Pause stays live here: `Esc` and `Start` open the pause menu, and Resume returns to the binocular view. The binocular component goes in GamePause's gameplay list so it sleeps while paused.

## 9. Open questions

1. Bearings **[GAP: Main3 revision]**. The list is closed: DailyLoop.md 1.3.
2. Closed 2026-09-29: File stays locked until all five are stamped (DailyLoop.md 1.6); section 4.9 keeps that gate from ever locking.
3. Closed 2026-09-29 (Wren): stamps are SAFE and CHECK ON FOOT.
4. Dwell 2.0 s (DailyLoop.md) and ring 15 percent: Marlow to time them on the blockout.

Pim
