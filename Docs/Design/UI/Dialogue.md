# Dialogue and captions

**DRAFT, 2026-09-29, Pim. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-28 (one data format for dialogue and events; characters before the format), 2026-09-29 (no voice acting, all speech is text; each minigame session and storyline has its own dialogue; UI text is TextMeshPro; Overpass for dialogue). Type per Style.md 7 and Fonts.md (Vesper). Line kinds follow Quill's draft legend (speaker line, reply set, ACT, form-typeface line, call during play, overheard speech). No story content in this file: every quoted line below is a placeholder.

## 1. Purpose

1. Talk: a resident speaks, the player reads, and sometimes picks one of up to three replies.
2. Calls: short lines a resident says during a minigame, while the player keeps playing.
3. Captions: speech heard in the world from a source the player is not talking to (a radio, a phone, a voice across a camp). There is no voice acting, so every word a voice says is a caption.

All three use the same plate and type, in a band that never covers the play area.

## 2. Bands

```
+--------------------------------------------------------------+
|  Logbook: ...  (objective line, top left)                    |
|  +--------------------------------------------------------+  |
|  |  UPPER BAND  (only when the host screen asks for it)   |  |
|  +--------------------------------------------------------+  |
|                                                              |
|                      (play area / world)                     |
|                              .                               |
|                                                              |
|  +--------------------------------------------------------+  |
|  |  LOWER BAND  (default)                                 |  |
|  +--------------------------------------------------------+  |
|                   [interact prompt row]                      |
+--------------------------------------------------------------+
```

1. Lower band is the default: 60 percent of screen width, centred, bottom edge just above the interact prompt row, inside the 5 percent safe margin.
2. Upper band: same width, under the objective line. Used only when a host screen (a minigame spec) says its play area is in the lower half. The host spec names the band; this spec never overrides it.
3. The band never covers the host's play area, the crosshair dot, the objective line or the interact prompt. If a host has no free band, the host spec is wrong, not this one.
4. The plate is #1E1916 at 80 percent (Style.md 7.2), sized to its text, not to the band. Empty band: nothing drawn.

## 3. Talk

### 3.1 Opening and closing

1. A resident with talk has the prompt `Talk`, lower centre. `E` / `Y` (Interact).
2. The Player action map goes off. Over 0.5 s the camera turns to face the speaker (look off, no movement). The first line fades in 0.15 s.
3. The talk ends at the last line or reply answer: the plate fades 0.2 s, control returns, the camera stays where it is.
4. Talks started by a host (a minigame opening, a talk at a car hood, a talk at the stones before `Kneel`) open the same way without the prompt. The host decides the band.

### 3.2 Wireframe, lower band

```
+--------------------------------------------------------+
|  Line the resident says, up to two lines at most,      |
|  whole, no typing effect.                          [A] |
+--------------------------------------------------------+
```

With replies:

```
+--------------------------------------------------------+
|  The line the replies answer.                          |
|                                                        |
|  > First reply, up to two lines.        <- focus       |
|    Second reply.                                       |
|    Third reply.                                        |
+--------------------------------------------------------+
```

1. One line on screen at a time. A new line replaces the old one; there is no scrolling history. The logbook keeps what matters (Logbook.md 4.5).
2. No speaker name. The speaker is whoever the camera faces. Residents are not named on screen.
3. `[A]` / `[Enter]` continue glyph, bottom right of the plate, by last device (TowerCheck.md 3.5 glyph rules). It shows 0.3 s after the line, when input unlocks.
4. Replies sit under the line they answer, in one plate. The line stays visible while choosing.
5. Focus: `>` marker plus full brightness; unfocused replies at 70 percent. Never colour alone.

### 3.3 Replies

1. One to three replies. Never more. Content with more than three is a content error (Rook's import check).
2. Order is set by content (shuffled per Quill's rule). The UI shows them in the order given. Default focus is the first.
3. No reply is marked as better, safer or costly. No stat names, icons or numbers on a reply.
4. A single reply is still shown as a reply (the player chooses to say it).
5. Input lock 0.5 s when replies appear, so the press that ended the last line cannot choose.
6. No cancel. Once replies show, one must be chosen. `B` and UI Cancel do nothing. `Esc` / `Start` open the pause menu; Resume returns to the same replies with the lock reset.

### 3.4 Line kinds

| Kind | How it shows |
|---|---|
| Speaker line | Overpass Regular 28, #D8CCB4, on the dark plate. |
| ACT (stage direction) | Same plate, Overpass Regular 28 at 70 percent opacity, set in square brackets: `[Placeholder direction.]`. Advances like a line. Content may mark an ACT `silent` when the scene animates it; a silent ACT is not shown and does not wait for input. |
| Form-typeface line (an adult's printed words in a resident's mouth) | A paper plate, #D8CCB4 paper with ink #2A2420, Overpass Regular 28, sentence case. Same position and size as the dark plate, so the swap itself is the signal. Not handwriting. |
| Object text shown mid-talk (words on a card, a label) | Examine.md lifted view, opened by the talk for 2.5 s or until Submit, then back to the talk. |
| Reply | Overpass Regular 28, #D8CCB4, on the dark plate under the line. |

No italics, no bold, no colour-only marking (Fonts.md 2.2 and 2.3).

### 3.5 Length budgets (for Quill and Rook's import check)

Measured at the 1080-row reference, Overpass 28, 60 percent width: about 80 characters per line (estimate, to be checked on the first build).

| Text | Budget |
|---|---|
| Speaker line, ACT, form line | 90 characters, two lines at most |
| Reply | 80 characters, two lines at most |
| Call during play | 45 characters, one line |
| Caption | 90 characters, two lines at most |

Over budget: the import check warns; the UI never shrinks text to fit.

## 4. Calls during play

1. A minigame host posts calls in its band while the player keeps control of the minigame. No input to advance.
2. A call that the play depends on (the player must act on its words) stays until the next call replaces it or the host clears it. Content marks these `held`.
3. Other calls (table talk) show for 2 s plus 0.06 s per character, 6 s at most, then fade 0.3 s.
4. A held call and a table-talk line never share the plate: table talk waits until the held call clears, or is dropped if the host clears the band first.
5. When the host ends play and moves to talk (replies), the band switches to Talk (section 3) in the same place.

## 5. Captions: voices heard in the world

Speech from a source in the world while the player moves freely: a radio calling, a phone voice, a recording, a resident talking on a phone with their back turned.

```
+--------------------------------------------------------+
|  Radio: Line the voice says.                        >  |
+--------------------------------------------------------+
```

1. Lower band, unless a talk or host band is in use; then captions go to the other band. Talk and calls always win the band they are in.
2. Each caption starts with a source word and a colon: `Radio:`, `Phone:`, `Voice:`. The source word is the object, never a character name. Content supplies it.
3. Shown only while the player is inside the source's hearing radius (the sound's audible range, Hollis's number). Leaving the radius mid-line fades it 0.3 s; the rest of that speech is missed. Content that must be read (a caller who waits for an answer) repeats until answered, as content says.
4. Off-screen source: a small arrow Image (`<` or `>`) at the plate's edge on the side the source is. No arrow when it is in view.
5. Hold: 2 s plus 0.06 s per character, 6 s at most. Consecutive lines from one source queue with 0.3 s between.
6. A pause written into the speech shows as `Radio: ...` for its length.
7. Captions take no input and do not stop the player. They hold while the logbook, binoculars, Ward screen or pause menu is open, and resume the queue after, dropping any line whose source is now out of range.
8. Always on. There is no voice audio to turn them off in favour of, so no setting.
9. Non-speech sounds (a phone ringing, a horn) are not captioned in this draft. Open question 2.

## 6. States

| State | What shows |
|---|---|
| Prompt | `Talk`, lower centre. |
| Opening | Camera turn 0.5 s, first line fades in. |
| Line | Line, continue glyph after 0.3 s. |
| Replies | Line plus 1 to 3 replies, lock 0.5 s. |
| ACT | Dimmed bracketed line; silent ACTs skipped. |
| Form line | Paper plate. |
| Object text | Examine.md lifted view, then back. |
| Calls | Host band, held or timed. |
| Caption | Source word, line, arrow if off-screen. |
| Paused | Pause menu over everything; talk waits, calls and captions freeze. |
| Event takes control | Talk closes at once, no reply chosen, no effects applied for an unchosen reply (Logbook.md 8.3 forced close). |
| Talk over | Plate fades 0.2 s, control back. |

## 7. Input paths

Keyboard and mouse:
1. `E` on `Talk`.
2. `Enter`, `Space` (unverified as Submit) or left click: continue.
3. Arrow keys or `W` / `S`: move reply focus. Hover moves focus. `Enter` or click chooses.
4. `Esc`: pause menu.

Gamepad:
1. `Y` on `Talk`.
2. `A`: continue.
3. D-pad or left stick up and down: reply focus. `A` chooses.
4. `B`: nothing. `Start`: pause menu.

Calls and captions take no input.

Requirements for Rook:
1. Player action map off from the `Talk` press (or the host's start) to the fade-out; only the UI map live. Otherwise `A` is Jump, `B` Crouch, `Enter` Attack.
2. Pause goes through (no modal flag), as the Ward screen. The dialogue component goes in GamePause's gameplay list.
3. Forced close API for events, shared with Logbook.md 10.6.
4. Import check: reply count 1 to 3, the budgets in 3.5, `held` and `silent` marks.

## 8. Open questions

1. Whether the camera should turn to face the speaker or leave look free during talk. Draft turns it: the plate is read, not the face, and a fixed view stops the player looking away mid-choice.
2. Captions for non-speech cues that carry meaning (a phone ringing, a knock). Hollis and Grant.
3. Budgets in 3.5 are estimates; Rook to check on the first build at 1080 and through the filter.

Pim
