# Tower check: lectern and binoculars

**DRAFT, 2026-09-28, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (daily loop, cave not checked, day and night states, night allows only the Ward). Gaps marked **[GAP: DailyLoop]** wait on Sable's Docs/Design/DailyLoop.md. Colours and type sizes wait on Vesper's Style.md **[GAP: Style]**.

## 1. Purpose

Every day the player climbs the tower and looks at each location except the cultist cave. The game marks each one SAFE or NOT SAFE from that day's event. The player only has to look; spotting is not a skill here (DECISIONS 2026-09-28).

## 2. Where it lives

1. A lectern in the tower cab, facing the widest window. On it: a pair of binoculars and a clipped sheet, the tower sheet.
2. The tower sheet is a world-space uGUI canvas on the lectern, readable by walking up to it. It is the diegetic summary: one row per location, a pencil mark appears when checked.
3. Locations checked **[GAP: DailyLoop]** for the final list. Draft uses the five from DECISIONS 2026-09-28: Lake, Campsite 1, Campsite 2, Campsite 3, Office. The cave is never on the sheet.

## 3. Wireframes

Tower sheet on the lectern (world space, seen at arm's length):

```
+----------------------------------+
|  TOWER SHEET          DAY 14     |
|                                  |
|  LAKE ........ SW    SAFE        |
|  CAMPSITE 1 .. S     ___         |
|  CAMPSITE 2 .. ESE   NOT SAFE    |
|  CAMPSITE 3 .. NNW   ___         |
|  OFFICE ...... E     ___         |
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

## 4. How a location is checked (look-to-unlock)

1. Each location has one designated view: a fixed direction (bearing and elevation) from the deck eye to its landmark, set by the scene recipe. Every designated view must be clear from the deck on an ordinary day: through a window opening, not a wall or post, and over the trees (Main3 sightline rules). Marlow checks each one on the blockout.
2. The look counts on the bearing alone. The check is the angle between the view centre and the designated direction; there is no line-of-sight ray. If smoke, fog, trees or an event hide the landmark that day, the look still counts. An obscured view can itself be that day's anomaly.
3. When the designated direction is inside the inner ring, the result line shows the name and three dots fill in over 1.0 s: `LAKE .`, `LAKE . .`, `LAKE . . .`.
4. If the direction leaves the ring before the dots fill, the line clears and the timer resets.
5. When the dots fill, the game marks the location from that day's event. The line becomes `LAKE    SAFE` or `CAMPSITE 2    NOT SAFE`. A pencil-scratch sound plays (Hollis). The tower sheet row gets the same word.
6. A location already marked today shows its result at once when it enters the ring. No dwell.
7. Only one direction can be in the ring at a time. If two overlap, the one nearest the ring centre wins. Designated views are at least 20 degrees apart, so this is rare.
8. When the last location is marked, the line reads `All checked.` for 2 s. The view stays up until the player lowers it.
9. The tower check can always be completed on any day. Nothing in the world can block a designated view from counting, so filing the report can never be blocked forever by the tower.
10. A NOT SAFE mark adds the location to the logbook day list and posts an objective line after the binoculars are lowered (Objective.md). It never interrupts the view.

## 5. How SAFE or NOT SAFE is shown

1. The word, never colour alone. SAFE and NOT SAFE differ in length and shape so they read through the VHS filter.
2. SAFE in the off-white of the interact prompt. NOT SAFE in the warm warning colour **[GAP: Style]**.
3. The world carries the event (thick smoke, torn sheet). That is scene content, not UI. The mark only confirms.

## 6. States

| State | What shows |
|---|---|
| Day, tower not checked | Lectern prompt `Use binoculars`. Sheet rows blank. |
| Binoculars up, nothing in ring | Mask, bearing, empty result line. |
| Target in ring, dwelling | Name plus filling dots. |
| Marked SAFE / NOT SAFE | Name plus word. Sheet row updated. |
| All marked | `All checked.` for 2 s. Sheet footer: `Checked.` |
| Day, already checked | Prompt `Use binoculars` still works. Every target shows its result at once. |
| Night | No prompt at the lectern. Binoculars cannot be raised (night allows only the Ward). |
| Paused | Pause menu over the binocular view. Resume returns to the same view. |
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

1. Final location list and bearings **[GAP: DailyLoop, Main3 revision]**.
2. Does the tower check have to be complete before the report can be filed? **[GAP: DailyLoop]** DayEndConfirm.md assumes yes; section 4.9 keeps that gate from ever locking.
3. Wording: SAFE and NOT SAFE, or in-world words such as `CLEAR` and `SEE TO IT`? Draft keeps the words in DECISIONS.
4. Dwell 1.0 s and ring 15 percent are guesses. Marlow to time them on the blockout.

Pim
