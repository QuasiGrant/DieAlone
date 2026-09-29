# Fonts

**DRAFT, 2026-09-29, Vesper.** Families and tech are decided (DECISIONS 2026-09-29: Patrick Hand, Overpass, VT323, all SIL OFL; UI text on TextMeshPro). Sizes, weights and colours below are **P** until Grant confirms them. Style.md 7 holds the short rules; this file holds the detail.

## 1. Families

| Family | Designer | Licence | Source | Roles |
|---|---|---|---|---|
| Patrick Hand | Patrick Wagesreiter | SIL OFL 1.1 (verified) | https://fonts.google.com/specimen/Patrick+Hand | Keeper's handwriting only: logbook lines, day list, questions, notes, map labels, pencil fill-in on forms |
| Overpass | Delve Withrington, Dave Bailey, Thomas Jockin | SIL OFL 1.1 (verified) | https://fonts.google.com/specimen/Overpass | Printed forms and the report, rule sheets, store and office paperwork, menus, settings, dialogue and resident lines, objective line, prompts, content warnings, day-end card, Ward screen, tower result line |
| VT323 | Peter Hull | SIL OFL 1.1 (verified) | https://fonts.google.com/specimen/VT323 | Tape overlay only: REC, PLAY >, tape counter, camcorder OSD |

No other face in UI. World signs and labels are textures, judged separately. TMP's default LiberationSans is not used for any shipped text.

## 2. Roles: size, weight, colour

1080-row reference. All **P**.

| Role | Family, weight | Size | Colour | Backing | Case |
|---|---|---|---|---|---|
| Dialogue, resident lines | Overpass Regular | 28 | #D8CCB4 | #1E1916 plate, 80 percent | sentence |
| Prompts, objective line | Overpass Regular | 28 | #D8CCB4 | plate | sentence, one line, two at most |
| Settings values, menu body | Overpass Regular | 28 | #D8CCB4 | plate | sentence |
| Menu items, headings, day-end title, menu title | Overpass SemiBold | 36 | #D8CCB4 | none on menu, plate elsewhere | sentence |
| Small print: credits, footnotes, key hints | Overpass Regular | 22 | #D8CCB4 | plate | sentence |
| Content warnings, body | Overpass Regular | 30 | #D8CCB4 | #1E1916 solid, full screen | sentence |
| Content warnings, heading | Overpass SemiBold | 36 | #D8CCB4 | same | sentence |
| Printed form labels and headings | Overpass SemiBold | 24 | ink #2A2420 | paper #D8CCB4 | caps |
| Printed form body, rule sheets | Overpass Regular | 26 | ink #2A2420 | paper #D8CCB4 | sentence |
| Stamped words (SAFE, CHECK ON FOOT) | Overpass SemiBold | 36 | #8B4A2B, 85 percent | paper | caps |
| Logbook handwriting, pencil fill-in | Patrick Hand Regular | 32 | pencil #4A4440 | paper #D8CCB4 | as the keeper writes |
| Crossed-out handwriting | Patrick Hand Regular, TMP `<s>` | 32 | pencil #4A4440 at 70 percent | paper | as written |
| Tape overlay | VT323 Regular | 36 or larger, whole multiple of its pixel grid (Rook to find the clean size) | #D8CCB4, underlay 1 px #1E1916 | none | caps |
| REC dot | uGUI Image, not text | VT323 cap height | #B0201C | none | n/a |

Rules:
1. Floor 22. Handwriting 32 because it reads smaller than print at the same size.
2. Only the weights listed. No faux bold or italic (TMP `<b>`, `<i>` not used on these faces).
3. No pure white text. Brightest text value #D8CCB4. Saturated colour only on the REC dot and stamps.
4. Handwriting only for what the keeper wrote. Nothing the player reads to make a choice is in handwriting except the logbook's own lines.
5. VT323 never in the world, never for sentences, never for dialogue.
6. Stamps drawn as art (ADMIT, REFUSE, tower stamps) stay uGUI Images as the UI specs say; the stamp row above is for stamped words set as text.
7. Legibility aids are TMP material settings: underlay 1 px #1E1916 on unplated text. No glow, no coloured outline.
8. Sizes may move once Rook answers whether UI draws over or under the tape filter (Style.md 7.7).

## 3. Files to import

| Family | Files **P** | Why |
|---|---|---|
| Patrick Hand | PatrickHand-Regular.ttf | single weight |
| Overpass | Overpass-Regular.ttf, Overpass-SemiBold.ttf (static) | Google Fonts ships Overpass as a variable font with static files in the download; static import is the safe path. TMP handling of variable .ttf is unverified. |
| VT323 | VT323-Regular.ttf | single weight |

TMP font assets **P**, Rook's call on settings: one static SDF asset per file (ASCII plus Latin-1), each with a dynamic fallback from the same .ttf. Render mode for VT323 to be tested for clean pixel edges (Rook). Glyph test before first use, rendered in each font through the tape filter: `Don't. "Camp 2" - 14 C, 3/4 mile... [x] #1 50% O'Neill's`

## 4. Licence files to keep with the fonts

SIL OFL 1.1 (read on Patrick Hand's OFL.txt, 2026-09-29): use, embed and sell with software is allowed; the font may not be sold by itself; the copyright notice and licence must travel with it.

1. Keep each family's `OFL.txt` from its google/fonts folder (ofl/patrickhand, ofl/overpass, ofl/vt323) next to its .ttf in the project, unchanged, one per family.
2. Ship the three OFL.txt files with the build (a licences folder beside the executable **P**; Rook to confirm how). Keep TMP's own LiberationSans OFL too, since the Essential Resources ship it.
3. Credits screen lists each family, designer and "SIL Open Font License 1.1".
4. Assets/SOURCES.md gets one line per family: file names, source URL, licence, date downloaded.
5. Reserved Font Names: whether each family declares one, and whether a generated SDF atlas counts as a modified version under the OFL, is unverified. Check each OFL.txt copyright line on download; if an RFN exists, do not rename or alter the font files.

## 5. Open

1. Grant: confirm the sizes, weights and colours in section 2.
2. Rook: glyph test and VT323 clean size, screenshot through the tape filter.
3. Rook: UI over or under the filter (Style.md 7.7).
4. Rook: how licence files ship in the build.

Vesper
