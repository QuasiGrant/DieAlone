# Fonts

**DRAFT, 2026-09-29, Vesper. Nothing here is decided.** Answer to Grant: fonts are picked now, before Milestone 10 builds the main menu, logbook and day-end card. Binding inputs: DECISIONS.md 2026-09-20 (uGUI, legacy Text, no TextMeshPro), 2026-09-29 (every asset licensed for commercial sale; no share-alike), Style.md 7 (type rules), Docs/Design/UI specs, Text/DayOneSamples.md.

## 1. What changes if this is approved

1. Style.md 7.1 ("one font: the built-in legacy Text font, no second face") is replaced by the three families in section 4. Needs one DECISIONS.md line and Grant's yes. Style.md is not edited until then.
2. Still legacy Text, still no TextMeshPro. Legacy Text takes any imported Font asset in its Font field. Unity 6000.3 manual (create-meshes-text-strings) confirms `.ttf` import; `.otf` is not confirmed there, so use `.ttf` only. Variable-font `.ttf` in a legacy Font asset is unverified: import the static weight files. Rook to prove with one import and a Game view screenshot before the menu is built.
3. Adding the font files is adding assets: Grant's yes first (CLAUDE.md). Each file goes in Assets/SOURCES.md with its licence.
4. SIL OFL 1.1 terms (checked on Patrick Hand's OFL.txt, 2026-09-29): use, embed and sell with software is allowed; the font may not be sold by itself; the copyright notice and licence must travel with it. So the build ships each family's OFL.txt and the credits list the fonts. Apache 2.0 (Special Elite) also allows commercial use and embedding, with its licence notice shipped.

## 2. Test for every candidate

1. Licence: commercial sale, embedding in a game, not share-alike. Source checked: google/fonts repository METADATA.pb for each family, 2026-09-29.
2. Legibility through the tape filter: the picture is soft, low resolution and bleeds colour. Thin strokes, hairline serifs, tight counters and small x-heights fail. Floor is Style.md 7.2: nothing under 22 at the 1080-row reference.
3. English coverage: all candidates carry the Google Fonts `latin` subset (Basic Latin and Latin-1). Curly quotes, apostrophe, en dash, ellipsis and the degree sign are expected in that subset but not checked glyph by glyph. Rook to render this line in each font before approval: `Don't. "Camp 2" - 14 C, 3/4 mile... [x] #1 50% O'Neill's`
4. Tone: late-90s working paperwork, a lookout's pencil, a camcorder's on-screen display. Nothing modern-app, nothing Halloween, no grunge distressing.

## 3. Roles and candidates

### 3.1 Logbook handwriting (keeper's notes, day list, questions, notes, map labels, pencil fill-in on forms)

| Font | Designer | Licence | Link | Fit | Through the filter | Coverage |
|---|---|---|---|---|---|---|
| **Patrick Hand** | Patrick Wagesreiter | SIL OFL (verified) | https://fonts.google.com/specimen/Patrick+Hand | Plain, tired print-hand; reads as a person, not a font gimmick. | Even stroke weight, open counters, decent x-height. Holds at 28. | latin, latin-ext (verified) |
| Kalam | Indian Type Foundry | SIL OFL (verified) | https://fonts.google.com/specimen/Kalam | Looser, faster pen hand; more hurried. | Three weights; Regular holds at 28, Light fails. Slight slant softens more in the blur. | latin, latin-ext (verified) |
| Reenie Beanie | James Grieshaber | SIL OFL (verified) | https://fonts.google.com/specimen/Reenie+Beanie | Best pencil scrawl of the three; most human. | Thin strokes and small x-height: fails under 36 through the filter. Reject for body text. | latin only (verified); punctuation coverage unverified |

Recommend **Patrick Hand**. Body at 32 **P** (handwriting reads smaller than print at the same size). No faux bold.

### 3.2 Printed forms and the report (daily report form, tower sheet headings, rule sheet in the booth, SAFE / CHECK ON FOOT words, store and office paperwork)

| Font | Designer | Licence | Link | Fit | Through the filter | Coverage |
|---|---|---|---|---|---|---|
| **Overpass** (same family as 3.3) | Delve Withrington, Dave Bailey, Thomas Jockin | SIL OFL (verified) | https://fonts.google.com/specimen/Overpass | Inspired by Highway Gothic (overpassfont.org): the face of US road and park signs, so a forest agency form set in it reads right. | See 3.3. Caps headings and ruled lines hold. | see 3.3 |
| Courier Prime | Alan Dague-Greene | SIL OFL (verified) | https://fonts.google.com/specimen/Courier+Prime | Typewritten form, office-issue. | Heavier than Courier New; holds at 24. | latin, latin-ext (verified) |
| Special Elite | Astigmatic | Apache 2.0 (verified) | https://fonts.google.com/specimen/Special+Elite | Worn typewriter, strongest period feel. | Built-in ink noise stacks on the tape grain and turns to mush under 32. | latin only (verified) |

Recommend **Overpass**, caps for printed labels, with Patrick Hand in the blanks. A typewriter face would be a fourth family; not worth it. If Grant wants typewritten paper later, Courier Prime replaces nothing and costs the family cap, so it needs its own decision.

### 3.3 Menus, settings, dialogue and resident lines, objective line, prompts, content warnings, day-end card, Ward screen, tower result line

One interface face for every non-handwritten, non-tape word. Dialogue and content warnings must be the most legible text in the game, so they share it.

| Font | Designer | Licence | Link | Fit | Through the filter | Coverage |
|---|---|---|---|---|---|---|
| **Overpass** | Delve Withrington, Dave Bailey, Thomas Jockin | SIL OFL (verified) | https://fonts.google.com/specimen/Overpass | Highway Gothic lineage: municipal, outdoors, trail signs, a job with a uniform. Not a modern app sans. | Wide, open letters, generous spacing; Regular and SemiBold hold at 22. Avoid Thin to Light. | latin, latin-ext, cyrillic, vietnamese (verified) |
| Libre Franklin | Impallari Type | SIL OFL (verified) | https://fonts.google.com/specimen/Libre+Franklin | Franklin Gothic revival: 90s newspaper and government print. | Sturdy, slightly condensed; tighter than Overpass, a little less clear at 22. | latin, latin-ext, cyrillic, vietnamese (verified) |
| Public Sans | USWDS, Dan Williams, Pablo Impallari, Rodrigo Fuenzalida | SIL OFL (verified) | https://fonts.google.com/specimen/Public+Sans | US government web face; very legible. | Excellent. | latin, latin-ext, vietnamese (verified) |

Recommend **Overpass**. Public Sans is the legibility fallback but reads 2020s web, off-brand. Weights: Regular for body, SemiBold for headings and the menu title. Sizes and colour stay Style.md 7.2 and 7.3.

### 3.4 VHS overlay text (REC, tape counter, PLAY > on the main menu, any camcorder OSD)

| Font | Designer | Licence | Link | Fit | Through the filter | Coverage |
|---|---|---|---|---|---|---|
| **VT323** | Peter Hull | SIL OFL (verified) | https://fonts.google.com/specimen/VT323 | Blocky terminal pixel face; close to a VCR on-screen display. | Pixel shapes survive the blur as blocks. Size at whole multiples of its grid **P** (Rook to find the clean size), 36 or larger. | latin, latin-ext, vietnamese (verified) |
| Share Tech Mono | Carrois Apostrophe | SIL OFL (verified) | https://fonts.google.com/specimen/Share+Tech+Mono | Squared technical mono; reads as late-90s equipment readout. | Clean at 28; less VCR, more instrument panel. | latin only (verified) |
| VCR OSD Mono | Riciery Leal | Unverified. dafont says "100% Free"; no licence text found. | https://www.dafont.com/vcr-osd-mono.font | The exact look. | Good. | unverified |

Recommend **VT323**. VCR OSD Mono is rejected until a written licence from the author allowing commercial embedding is found (Style.md 8.14). VT323 is used only for tape OSD words, caps, short: `REC`, `PLAY >`, the counter. Never for sentences.

## 4. Families: three

| Family | Files to import **P** | Roles |
|---|---|---|
| Patrick Hand | Regular | 3.1 |
| Overpass | Regular, SemiBold (static .ttf) | 3.2, 3.3 |
| VT323 | Regular | 3.4 |

## 5. Rules that go into Style.md 7 if approved

1. Handwriting only for what the keeper wrote. Anything the game or an agency printed is Overpass. Nothing the player reads to make a choice is in handwriting except the logbook's own lines.
2. VT323 only for tape OSD. Never in the world, never for dialogue.
3. No other face in UI. World signs and labels are textures, judged separately.
4. Colour, sizes, sentence case and no-pure-white stay as Style.md 7.2 to 7.4; handwriting body 32 **P**.
5. Stamps (ADMIT, REFUSE, tower stamps as art) stay uGUI Images as the UI specs say.

## 6. Open

1. Grant: approve the three families and the change to Style.md 7.1.
2. Rook: one-import proof (legacy Text with an imported .ttf, static weight) and the glyph test line in section 2.3, screenshot through the tape filter.
3. Whether UI draws over or under the filter (Style.md 7.7) changes the sizes; sizes stay **P** until Rook answers.

Vesper
