# Examine and read

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (diegetic first; the logbook holds notes and questions), 2026-09-29 (UI text is TextMeshPro; Patrick Hand for the keeper's hand only, Overpass for anything printed; no voice acting). Style.md 7.6 and 7.7 (world-space text goes through the filter; judge it at 360 rows; UI over or under the filter unverified). Fonts.md 1 (world signs and labels are textures). No story content in this file: quoted text is placeholder.

## 1. Purpose

Two ways to take in a thing in the world:
1. **Examine**: look at an object and get one plain line about it. The player keeps moving.
2. **Read**: lift a note, form, sign, letter or label into a close view big enough to read through the tape filter at 360 rows.

## 2. Examine

```
+--------------------------------------------------------------+
|                                                              |
|                         (object in view)                     |
|                              .                               |
|                                                              |
|     +------------------------------------------------+       |
|     |  Placeholder examine line.                     |       |
|     +------------------------------------------------+       |
|                        [E] Examine                           |
+--------------------------------------------------------------+
```

1. An examinable object shows the prompt `Examine`, lower centre. `E` / `Y`.
2. The line shows in the Dialogue.md lower band on the dark plate, Overpass Regular 28. One line, 90 characters at most (Dialogue.md 3.5).
3. Not modal. The player keeps moving and looking. The line holds 2 s plus 0.06 s per character, 6 s at most, then fades 0.3 s. Examining another object replaces it.
4. An object examined before shows its line again; content may give a second line on a repeat.
5. An examine can post a logbook line or note (Logbook.md 4.5, 5.6) as content says.
6. A plain status line (an object with nothing to do today) uses the same line, prompt `Examine`. The interact prompt itself stays a verb, never a sentence.

## 3. Read

### 3.1 What can be read

Notes, forms, signs, letters, labels, cards, papers left on a surface. Anything whose words the player is meant to take in. Text that is only set dressing (a faded poster) stays texture with no Read.

### 3.2 The lifted view

```
+--------------------------------------------------------------+
|  (world, dimmed to 40 percent)                               |
|            +--------------------------------+                |
|            |  FORM TITLE                    |                |
|            |                                |                |
|            |  Location: ______              |                |
|            |  Colour:   ______              |                |
|            |                                |                |
|            +--------------------------------+                |
|                          1 / 2                               |
|              [E] put down   [Q] [E] page                     |
+--------------------------------------------------------------+
```

1. Prompt `Read`, lower centre. `E` / `Y`. The object lifts to screen centre over 0.2 s: a screen-space panel, up to 70 percent of screen height, world dimmed behind it.
2. Printed text (forms, signs, typed letters, labels in print): TextMeshPro Overpass on a paper panel, ink #2A2420 on #D8CCB4 (Fonts.md 2). Headings SemiBold caps, body Regular.
3. The keeper's own writing on a found page or form: Patrick Hand, pencil #4A4440.
4. Anyone else's handwriting: texture art, section 5.
5. Pages: `1 / 2` under the panel, only when there is more than one. Left and right turn pages.
6. Movement and look off while lifted. Time does not stop.
7. First time a thing is read, content may post a logbook note (Logbook.md 5.6).

### 3.3 Legibility at 360 rows

The tape filter's low resolution is the test (Style.md 7.7). Sizes at the 1080-row reference, all **P**:

1. If UI draws over the filter (the target): printed body 28, headings 32, handwriting 32 (Fonts.md 2). About 12 lines of body a page.
2. If UI draws under the filter: printed body 36 at least, headings 40, handwriting 40, and about 8 lines a page. Content longer than one page at that size goes to a second page, never smaller type.
3. Texture art (section 5) is authored so its smallest letter stroke is at least 2 px at 360 rows when lifted to 70 percent of screen height. Rook checks every readable texture with a Game view screenshot at 360 rows.
4. Nothing in the world is expected to be read at world distance. If a word must be read, it has a Read.

### 3.4 Forms the keeper fills in

1. A form with blanks shows the blanks as `______`.
2. When content says the keeper fills it in (an event's resolve), the lifted form shows a choosable line under the panel, `Fill in`. Choosing it writes the pencil text into the blanks over 1.0 s with the pencil-scratch sound (Hollis). The words are the game's, never typed by the player.
3. A filled form keeps its pencil text when read again.

### 3.5 Carried readable objects

1. An object that can be both carried and read shows `Pick up`. Picking it up opens the lifted view first; putting the view down leaves it in hand.
2. While carrying, it cannot be read again (no free button: Interact is `Set down`). Set it down and use `Read`.
3. A readable object that cannot be carried shows only `Read`.

## 4. States

| State | What shows |
|---|---|
| Looking at an examinable | `Examine`. |
| Examine line | Lower band line, timed, player free. |
| Looking at a readable | `Read` (or `Pick up` if carriable). |
| Lifted | Panel, dimmed world, page count if more than one. |
| Form with a fill-in | `Fill in` line under the panel until used. |
| Filling | Pencil writes 1.0 s, input locked. |
| Paused | Pause menu over the lifted view; Resume returns to it. |
| Event takes control | Lifted view closes at once, no fade (Logbook.md 8.3). |
| Night | Examine and Read still work; they are not actions (Logbook.md 8.2 reading). |

## 5. Handwriting by other people

Rules, as Collectibles.md keeps its own:
1. Only the keeper's writing is a font (Patrick Hand). Every other person's writing is texture art drawn by Vesper: labels, letters, cards, painted words, names on objects.
2. It is never set in Patrick Hand, and never in Overpass as a stand-in. GateBooth.md 6 (a driver's writing on a paper "printed Overpass, or drawn into the paper art") should drop the Overpass option to match.
3. It is read through the lifted view (3.2) or, mid-talk, through Dialogue.md 3.4, at the size rules of 3.3.
4. Handwriting that changes during play (a name added, a word crossed out) is a texture swap or a layered texture, not text. Vesper and Rook to size the art count per content.
5. A word written in another person's hand that the player must understand to act is also given, word for word, in the keeper's logbook line or an Examine line, so the words never rest on art legibility alone. Quill writes that line.
6. No translation layer or typed transcript on screen.

## 6. Input paths

Keyboard and mouse:
1. `E` to examine, read or pick up.
2. In the lifted view: `A` / `D` or left and right arrows turn pages. `Enter` or click chooses `Fill in`. `E`, `Esc` or right mouse puts it down.
3. `Esc` puts it down; a second `Esc` opens pause.

Gamepad:
1. `Y` to examine, read or pick up.
2. In the lifted view: D-pad or left stick left and right turn pages; `LB` and `RB` also turn pages. `A` chooses `Fill in`. `B` or `Y` puts it down.
3. `Start` puts it down; a second press opens pause.

Requirements for Rook:
1. Player action map off while lifted; the lifted view reads its own put-down bindings (`E`, `Y`, `B`, right mouse), as the binoculars do (TowerCheck.md 8).
2. Sets the modal flag so `Esc` / `Start` put it down first (Logbook.md 10.3).
3. Examine lines use the Dialogue.md band and plate; they do not take the Player map.
4. Forced close for events.

## 7. Open questions

1. Rook: UI over or under the filter decides between 3.3.1 and 3.3.2.
2. Vesper: paper panel art per kind (form, note, card, sign) or one shared paper.
3. Whether lifted panels show the object's own art (a photo of the note) or a clean redraw. Draft: the object's texture, redrawn clean where 3.3.3 fails.

Pim
