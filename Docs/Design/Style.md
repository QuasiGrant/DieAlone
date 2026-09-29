# Style guide

**DRAFT, 2026-09-28, updated 2026-09-29, Vesper. Nothing here is decided until it is a line in DECISIONS.md and Grant has confirmed it.** Every later review of Main3 checks against this file. Values marked **P** are proposals for Grant. Values marked **now** are what LookTuning.asset holds today. Rook sets LookTuning; I only propose.

Binding inputs: DECISIONS.md 2026-09-20 (VHS first, Fears to Fathom, PS1 effects off, own shaders only), 2026-09-27 (SSAO off, render scale 0.5), 2026-09-28 (two states day and night, sun fixed, pass checks, packs for hero pieces, day one plays as a normal job), 2026-09-29 (no fire visible on day one until nightfall; bought pack shaders replaced with project shaders; every asset licensed for commercial sale; from day two the fire may show by day as far glow and smoke, never on day one, and it is huge in the distance).

**Pending:** the day one split below (2.0, 6.0) waits on Sable's revision 6 and Grant. Until then, treat it as a proposal. The day two fire lines (2.1, 6.1, 6.3) rest on DECISIONS 2026-09-29; their values stay **P**.

## 1. Tone

1. Words: dread, routine, worn, lived-in, abandoned mid-task, heat, ash, hush, patient, wrong.
2. The camp is the only warm, safe-looking place. Everywhere else looks like someone just stopped what they were doing.
3. Wrongness is one detail, not ten. A site is normal except for one thing.
4. The monster is punctuation. Nothing in the dressing is a monster tease by default.
5. Day one is an ordinary fire lookout job (DECISIONS 2026-09-29). Its unease comes only from isolation and routine: quiet, distance, an empty radio. No fire, smoke, ash or rune in any frame before nightfall. The reveal only works if day one looks safe.
6. Reference: Fears to Fathom, Ironbark Lookout (Rayll, 2023): a fire lookout, same tape look, same everyday-object horror. Read it for density of props and how little light it uses at night.

## 2. Palette

sRGB hex. Day and night only (DECISIONS 2026-09-28: two states). Day one has its own day palette (2.0) because the fire is not visible on day one (DECISIONS 2026-09-29). Whether LookTuning can hold a second day set is unverified; Rook to confirm.

### 2.0 Day one: ordinary late afternoon, no fire (pending Sable rev 6 and Grant)

All **P**. A clean, warm late afternoon turning to sunset. Nothing in it says fire.

| Role | Hex | Change from day two on |
|---|---|---|
| Sky top | #5E6878 | dusty blue-grey, not rust-brown |
| Sky horizon | #E3A968 | gold, not burnt orange |
| Sky below horizon | #5A4A3C | brown, less red |
| Sun glow | #FFC98A | paler, yellower |
| Fire glow on the ridge | none | absent until nightfall |
| Haze (far fog) | #A8A08E | pale dust and distance, not smoke |
| Ambient fill | #6E6658 | slightly cooler, same low value |
| Ash | none | no falling or settled ash |
| Forest floor green | #58583A | a touch less dulled, still olive |
| Surfaces | as 2.1 | wood, rust, granite, canvas, char unchanged |

### 2.1 Day two on: the burning sunset

| Role | Hex | Source |
|---|---|---|
| Sky top | #381C1A | now (skyTop) |
| Sky horizon | #D9662E | now (skyHorizon) |
| Sky below horizon | #4D291A | now (skyGround) |
| Sun glow | #FF8C40 | now (sunGlowColor) |
| Fire glow on the ridge | #FF6B1A | now (fireGlowColor) |
| Haze (far fog) | #9E5C38 | now (sunsetFogColor) |
| Ambient fill | #9E705C now, **#734D42 P** | now is too bright, see 5.3 |
| Ash, falling and settled | #8A8078 | P |
| Char, burnt wood, deepest shadow | #1E1916 | P |
| Rust, metal | #8B4A2B | P |
| Weathered wood | #5C4632 | P |
| Granite (Ward, Tor, rock) | #6E6660 | P |
| Canvas, paper, bone (brightest surface allowed) | #D8CCB4 | P |
| Forest floor green, dulled | #4F4A2C | P |

### 2.2 Night: dark except the fire glow

| Role | Hex | Source |
|---|---|---|
| Night fog and sky | #05080D | now (fogColor) |
| Fire glow on the horizon | #FF6B1A | now, same as day two on; first seen at nightfall on day one |
| Fire core (flame base, hottest tongues) | #FFE8C0 | P, the brightest value in any night frame |
| Sky lit by the fire, above the front | #5A2412 | P, fades to #05080D by 30 degrees above the horizon |
| Smoke underside, lit | #6B2A12 | P |
| Practical light (lantern, cab, stove) | #FFA860 | P |
| Ward runes, lit | #B8481C | P, dim ember, never above the lantern |

### 2.3 Location lights (Main3.md rev 3, section 3)

One light colour per place so no two read alike. All **P**.

| Place | Light | Hex | Rule |
|---|---|---|---|
| Keeper's camp | stove, lantern, cab | #FFA860 | the warmest, softest light on the map |
| Camp 1 | festoon string, tungsten | #E8B840 | paler and harsher than the keeper's camp; two or three bulbs dead |
| Camp 2 | lamp, cold white | #DEDCD4 | neutral cold, never blue |
| Camp 3 | green glass lantern | #6F8436 | olive, never emerald or neon |
| Office | sodium lot lights | #F08A2A | plus one red mast lamp #B0201C so the mast reads against the haze |
| Cave | cold ambient, no sunset | fill #4A5058, light #9AA3AD | grey-blue, desaturated, no glow |

### 2.4 Palette rules

1. Warm owns the frame. Cool allowed only in: the night sky and fog, the cave, the Camp 2 lamp, the day one sky top. All cool stays desaturated.
2. Saturated colour is reserved for light sources: fire, sun, lanterns, one warning object per site (Mast's red rag and lamp). Surfaces stay dull.
3. Nothing brighter than #D8CCB4 on a surface. Pure white exists only in the sun and fire cores.
4. Greens are olive and dulled, never grass green.
5. Day one has no orange above #E3A968 in the sky and no glow on any horizon. The first fire-orange the player sees is at nightfall on day one.
6. Day one exception: every fire, fire glow, smoke, ash or fire glimpse line in this file applies only from nightfall on day one (DECISIONS 2026-09-29). Where a line does not say so, 2.0, 6.0 and 8.13 override it.

## 3. VHS look targets

The brief (DESIGN.md, Presentation): soft low-resolution picture, colour bleed, grain, scan lines. LookTuning has drifted towards a clean HD picture: the Main2 review found filter on and off nearly identical (shots 02 and 20).

| Setting | now | Target **P** | Why |
|---|---|---|---|
| lowResHeight | 875 | 360 | Brief says low-resolution. 875 is near 1080p. See note 1. |
| colorBleed | 0.51 | 0.5 | On brief. Keep. |
| washOut | 0.07 | 0.25 | Tape is washed; 0.07 is almost off. |
| crushBlacks | 0.38 | 0.3 | Keep near; night needs some shadow detail near light. |
| grainStrength | 0.07 | 0.2 | Grain is in the brief; 0.07 is invisible after downsample. |
| grainSpeed | 7.7 | 24 | Slow grain reads as a crawling texture on the world, not tape. |
| noiseBandStrength | 0 (off) | 0.35 | Tape artefact is part of the look. Off entirely is drift. |
| noiseBandInterval | 6 | 20 | Rare band reads as wrongness; frequent reads as broken build. |
| scanLines | 0.3 | 0.3 | Keep. |
| blur | 0.11 | 0.3 | Soft picture is the brief. |
| darkCorners | 0.6 | 0.45 | 0.6 eats the edges where wayfinding cues sit. |

Notes:
1. The filter clamps lowResHeight to the camera target height (LookFilterFeature.cs line 61). With render scale 0.5 (DECISIONS 2026-09-27) a 1080p screen likely renders 540 rows, so 875 does nothing and the real picture is set by render scale. Unverified whether that descriptor is post-scale; Rook to confirm. Either way the look must be set in LookTuning, not by accident of render scale.
2. Test for every look change: the same shot with filter on and off must differ at a glance. If they do not, the filter is too weak.
3. PS1 effects (vertex jitter, affine textures) stay off. SSAO stays off. No bloom halos, lens flare, chromatic aberration beyond colour bleed, or film grain from a second source.

## 4. Materials and texel density

1. Import Max Size 512 for everything. Hero pieces (Ward stones, giant trunks, Tor) may use 1024 **P**. Pack textures at 2K or 4K are capped on import, never shipped raw.
2. Texel density **P**: 128 px per metre for anything the player can pick up or stand within 2 m of; 64 px per metre for buildings, ground and trunks; 32 px per metre allowed on surfaces taller than 10 m. After the 360-row filter, finer detail is invisible; coarser shows as smear.
3. No stretched textures. ProBuilder faces use world-space or per-face auto UVs at the density above. A plank or brick should read at the size it is in the world.
4. One material per surface type across the map (one bark, one granite, one rust metal). No two structures share silhouette and material (DECISIONS 2026-09-28).
5. Bought pack shaders are replaced with the project's own shaders (DECISIONS 2026-09-29). "Own" means shaders written for this project plus Unity's built-in URP shaders (URP Lit, Unlit, Particles) already used across the project; it excludes any shader shipped inside a pack (Wren's reading, pending Grant). Pack materials are rebuilt on those shaders and retinted to the palette. Pack showcase colours (bright blue roofs, clean paint) are repainted or rejected.
6. Emission only on light sources and lit runes. Emission intensity on runes never exceeds the nearest practical light.
7. Ground: no bare flat dirt within 10 m of the camera in any dressed shot **P**. Break it with litter, roots, ash drift (day two on only), needles, grass tufts, stones.
8. Every asset used must be licensed for commercial sale (DECISIONS 2026-09-29). CC0 and Asset Store Standard EULA qualify; anything with non-commercial, editorial-only or unclear terms is rejected before it enters the project. Record the source and licence of each pack or texture when it is judged.

## 5. Structure quality bar

A structure passes a dressing review when all are true:
1. Silhouette identifies it at 20 m with the filter on. Test by squinting at a 360-row capture.
2. It has three height bands: a base that meets the ground (steps, footing, debris), a body, a top (roof overhang, chimney, mast, sheet).
3. No plain box faces larger than 3 x 3 m without trim, frame, openings or props against them.
4. Nothing floats. Every object has ground contact or visible support.
5. It shows use: at least one object mid-task (an axe in the block, a pot on a cold fire, a door ajar).
6. A clearing holds 15 or more props **P**, grouped in two or three clusters, not scattered evenly.
7. No untextured primitive in a dressed location. Primitives are for blockout only.
8. Trees never on a grid. Giants spaced at least 30 m apart **P**, small trees clustered. Giant tops at most 50 m absolute, 42 m tall on the knoll (Main3.md 2.7).

## 6. Lighting

### 6.0 Day one (waking until nightfall; pending Sable rev 6 and Grant)
1. One fixed sun, west, higher than day two on: elevation 14 degrees **P**, colour #FFC98A. Late afternoon, not last light.
2. Shadows medium-long, eastward. Same two cascades.
3. Same value structure rule as 6.1.3: sky brightest, haze mid, foreground darkest.
4. Ambient #6E6658 **P**. Canopy floor still dark; clearings still pools of light.
5. Haze lighter and paler (#A8A08E); far landmarks clear, the ridge a plain blue-grey hill with no glow.
6. No fire glow, no fire glimpses, no ash particles, no smoke column. The cliff-edge gaps from 6.1.7 show only sky.
7. Practical lights on as normal (Pim's rule).
8. Nightfall on day one uses the night rules (6.2). The fire glow appears there for the first time; the full view of the wildfire and the Ward is at the Ward ledge.

### 6.1 Day two on (waking until the report is filed)

What changes from day one: sun lower (6 degrees) and more orange; sky rust-brown to burnt orange; haze becomes smoke; ash falls and settles; fire glow on the ridge; fire glimpses through planned gaps.

1. One fixed sun, low in the west, the fire side. Glow means west (Main3.md 5.7). Elevation 6 degrees **P**, colour #FF8C40.
2. Shadows long and eastward. Two cascades (DECISIONS 2026-09-27).
3. Value structure in every frame: sky brightest, haze mid, foreground darkest. The flat single value of Main2 is the failure to avoid.
4. Ambient low (#734D42 **P**) so the forest floor under canopy goes dark and clearings read as pools of light.
5. Warm practical lights mark points of interest even by day (Pim's rule): lit cabin desk, pump lantern.
6. Haze is heavy enough that far landmarks rise out of it; height fog is Rook's open item (Main3.md 5.6).
7. Fire glimpses on the ground (W1 shore, west bends, Camp 3 rim) need a planned gap in the cliff-edge giant band, not luck. Day two on only; on day one the same gaps show only sky (6.0.6).

### 6.2 Night
1. No sun, no moon. Ambient near black.
2. The fire glow is the only large light: a faint warm fill from the west **P**, enough to show silhouettes against the sky, not surfaces.
3. Practical lights only: cab light, cabin window, lanterns, the cairn lamp. Each is a destination or a marker.
4. Night fog closes in. Fog 8 to 60 m **P** (now 20 to 200 m, which is day-far).
5. Ward runes glow dim ember (#B8481C), readable at 10 m, not at 50 m.

### 6.3 Wildfire scale (Grant, 2026-09-29: "Make sure the fire is BIG in the distance!")

The fire is never small. It is the biggest thing the player ever sees. Distances from Main3.md: ridge 300 to 500 m west of the cliff (x 10), Ward ledge ground 36 m, tower eye 57.6 m, giants 40 to 50 m. Angles are measured from the eye, level gaze; they hold at any FOV. All **P**; Rook checks each with a Game view screenshot.

Night one, from the Ward ledge (first sight):
1. Width: the fire front fills at least 150 degrees of the western horizon, and runs out past both edges of the frame when the player looks west. There is no end to it in view.
2. Flame height: tongues stand 1.5 to 2 times the height of the giants burning in them (70 to 100 m), about 10 to 15 degrees above the horizon at the ridge. The valley below the cliff burns too, so the front reads as a sea, not a line on a hill.
3. Smoke: columns rise at least 5 times giant height (250 m and up), past the top of the frame at level gaze, leaning east over the player. Undersides lit #6B2A12; tops lost in the black.
4. Brightness: the fire core (#FFE8C0) is the brightest value in the frame, brighter than any lantern or rune. The sky above the front glows #5A2412, fading to night black by 30 degrees up. Foreground stones and giants read only as black silhouettes against it.
5. Frame test: looking level west from the Ward, fire and lit smoke cover at least half of the sky between the horizon line and the top of the frame.

Day two on, from the tower (by day):
6. Fire base stays hidden behind the cliff-edge giant band (Main3.md 5.8). What shows is glow along the ridge line and flame tips over it in places.
7. Width: glow and smoke span at least 90 degrees of the western horizon from the tower cab. If growth with WARD is confirmed (DailyLoop.md question 2), 90 degrees is the floor and it widens from there; never smaller.
8. Smoke: three to five columns, each rising at least 4 times giant height above the ridge (200 m and up), about 20 degrees above the horizon or more, merging into a brown sheet that roofs the western sky. Smoke is darker than the sky horizon (#4A3A32 **P**), lit orange from below.
9. Brightness: the ridge glow (#FF6B1A) is brighter than the sky horizon (#D9662E) behind it. Sun and fire are the two brightest things in the frame.
10. The same scale holds at the ground glimpses (W1 shore, west bends, Camp 3 rim): through a gap, the smoke still towers over the nearest giants.

## 7. UI type (Vesper owns type, Pim owns reading and layout)

1. Three families, all SIL OFL, on TextMeshPro (DECISIONS 2026-09-29; replaces the 2026-09-20 built-in-font rule). No other face in UI. Detail, files and licences: Docs/Design/Fonts.md.
   - **Patrick Hand** Regular: only what the keeper wrote (logbook lines, day list, notes, map labels, pencil fill-in on forms).
   - **Overpass** Regular and SemiBold: everything the game or an agency printed (forms, report, rule sheets), menus, settings, dialogue, objective line, prompts, content warnings, day-end card.
   - **VT323** Regular: tape overlay only (REC, PLAY >, counter). Caps, short, never sentences, never in the world.
2. Sizes at a 1080-row reference **P**, per role:

| Role | Family, weight | Size | Colour | Backing |
|---|---|---|---|---|
| Body: dialogue, prompts, settings, objective line | Overpass Regular | 28 | #D8CCB4 | #1E1916 plate, 80 percent |
| Headings, menu items, day-end title | Overpass SemiBold | 36 | #D8CCB4 | plate or none on menu |
| Small print: labels, credits, footnotes | Overpass Regular | 22 | #D8CCB4 | plate |
| Content warnings | Overpass Regular, heading SemiBold | 30, heading 36 | #D8CCB4 | #1E1916 solid, full screen |
| Printed form text on paper | Overpass Regular, labels in caps SemiBold | 24 to 28 | ink #2A2420 | paper #D8CCB4 |
| Logbook handwriting | Patrick Hand Regular | 32 | pencil #4A4440 | paper #D8CCB4 |
| Crossed-out handwriting | Patrick Hand Regular, `<s>` | 32 | pencil #4A4440 at 70 percent | paper |
| Tape overlay | VT323 Regular | 36 or larger, whole multiple of its pixel grid (Rook to find) | #D8CCB4, 1 px shadow #1E1916 | none |
| REC dot | image, not text | matches VT323 cap height | #B0201C | none |

3. Nothing under 22. No faux bold or italic; only the weights listed. No pure white text. Saturated colour in type only on the REC dot.
4. Sentence case, except caps labels on printed forms and the tape overlay. Short. Prompts one line, at most two. No exclamation marks, no emoji, no em dashes.
5. Layout: prompts lower centre, stats screen only in the end-of-day phase (DECISIONS 2026-09-28). Nothing permanent on screen except the crosshair dot.
6. Diegetic first: logbook, notice board, map, labels on objects carry information before any HUD does.
7. Whether UI draws over or under the tape filter is unverified; Rook to confirm. Target **P**: UI over the filter, legible, but in palette. World-space text (labels, notice boards) goes through the filter; judge it at 360 rows.
8. Legibility aids are TMP material settings (outline, underlay), not extra meshes **P**: underlay 1 px #1E1916 on text without a plate. No glow, no coloured outline.

## 8. Forbidden

1. Neon, cyan, teal or blue glow anywhere. The Main2 Ward runes are the example.
2. Saturated blue or bright-painted surfaces (Main2 blue roofs).
3. Untextured primitives or stretched textures in a dressed location.
4. Bare flat ground filling the lower half of the frame.
5. Trees in rows or on a grid; ordinary pines standing in for the giant trees.
6. Flat cone or triangle mountains for the burning ridge.
7. A monster or creature silhouette placed as set dressing.
8. Gore as decoration. Blood only where an event puts it.
9. Jokey, meme or modern-internet text in any in-world writing.
10. PS1 vertex jitter, affine textures, SSAO, lens flare, bloom halos, unless Grant decides otherwise.
11. Pack assets used as-is with their showcase lighting, colours or shaders.
12. Anything that looks new and clean, except one deliberate wrong object.
13. Any fire, fire glow, smoke, ash or lit rune on day one before nightfall.
14. Any asset without a licence for commercial sale.
15. A small fire: a thin strip of flame on the horizon, a single smoke plume, or flames shorter than the giants. Below the 6.3 targets is a fail.

## 9. Reference boards

Per-location boards wait for Main3.md (draft rev 3) to be confirmed. Global boards to assemble for Grant's look, in Docs/Design/Boards when started:
0. Day one: ordinary late afternoon in a lookout forest, clean air, gold sun, no smoke.
1. Sunset and haze (day two on): value structure, fixed low sun, rust haze.
2. Night: fire glow only, practical lights as markers.
3. Giant trees: trunk scale against a person, bark, how light falls between them.
4. The Ward: carved standing stones on a cliff edge, weathered, ember runes.
5. Lived-in camp: props per clearing, abandoned mid-task.

Vesper
