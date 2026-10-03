# 8.25 Camp 2, Gate step 3 (Vesper)

Sheets: Docs/Captures/Main3Review_camp2, capture 2026-10-03 00:55 (commit ad2fad6). Layout only, not dressing. Bar: Style.md 10. C or better passes.

**Verdict: FAIL.** Ramps and landings D, PS3 D. Everything else C or better.

## Grades

| Place | Grade | Frames | Why |
|---|---|---|---|
| Granite stack | C | Found_02, Trail_Boathouse FWD 30 to 80, F2 | Tall landmark from the boathouse leg at 30 to 80 m. Fix: it reads as a built tower (flat faces, coursed blocks), not granite. Dressing task. |
| Ramps and landings | **D** | Found_06, 07, 18, 19, 04; Trail_Camp_2_to_T BACK 50 to 90 | The 1.5 m screens do not read as part of the place. See Screens. |
| Top floor | B | F5, Found_06, 07, 18, 19 | Chair at the east rail, tent, lamp pole, rope rail, open view to the ridge. Reads as his lookout. |
| Tent | B | F5, Found_06 | Clear at 10 m, door east, lamp beside it. |
| Booth and table | C | F1, F2, F3, Found_09, 10 | Booth reads at 30 m (hood light, pale frame). Fix 1: two phones. The booth mesh's own phone shows on the back wall; the hook is at the south jamb with no handset seen (Found_09). Keep one phone, at the hook. Fix 2: the table group reads only in F2; F3 is grass at the lens, Found_10 hides it behind the stair skirt. |
| Barrel | C | Found_20, F2 | Reads at 8 m. At 12 m (F2) it is a dark smudge at the stack foot. |
| PS1, PS2 | C | Found_18, 19 | Marks sit on the top rail; a place a paper can sit. Paper itself is dressing. |
| PS3 | **D** | Found_20 | Mark on bare grass at the stack's west foot. No talus top or rock reads there. Checks.md puts it at (283.7, 4.5, 107.9), not the doc's talus top (285.7, 109.9). Put it on a rock you can see. |
| Boathouse to Camp 2 | B | Trail_Boathouse_to_Camp_2 | Trail clear both ways. Stack and stair show at 30 to 50 m; the booth at 70. |
| Camp 2 to T | C | Trail_Camp_2_to_T, Found_01 | Forward leg clear to the lot. Fix: coming back (BACK 50 to 90, Found_01), the dark stair skirt fills a third to a half of the frame, and BACK 90 is close to all wall. |

## Screens (Rook's 1.5 m planks)

They do not read as part of the place:
- They are solid walls at eye height. The climb loses its promised payoff ("treetops dropping, then the highway"). The stair becomes a corridor (Found_06, 07, 18).
- The east screens carry a stretched, blocky texture (Found_06, 07, 18, right edge). That breaks hard line 6.
- From below, the toothed tops read as battlements (Found_19, top left). With the coursed stack, that gives a castle read.
- From the T leg, the screens and skirts make a dark 3 m block (Found_01, 10; Trail_Camp_2_to_T BACK 50 to 90).

**Single change:** swap each solid screen for open post-and-rail like the top rail: posts every 2 m to 1.5 m, timber rails at 0.5, 1.05 and 1.5. Keep the current 1.5 m box collider unchanged. The stop against a 1.05 m-plus sprint jump stays the same. The top rail gives the visible reason, and the view comes back.

## Deck view

- Lamp core: 128 of 128 rays clear (Checks.md). By eye in Deck_camp2_01 (day), the cold core is not visible at about 144 m. The stack top shows only as a faint pale column (Deck_camp2_08, about 935, 430). **Unconfirmed by eye.** I need a night deck frame with the lamp lit before I can say it is seen.
- Booth hood light: 0 of 128, blocked by Sequoia3. The doc only asks for it loose.

## Outside this area (note only, not graded)

On the far ridge, behind the trees east of the tower, there are pale see-through giant shapes (F5, Found_06, 07, 18). Something solid is rendering translucent in haze. Wren to route.

Vesper

## Round 2 (final), 2026-10-03

Rechecked only my round-1 fails plus the phones, from Rook's new single frames. Layout only.

**Verdict: FAIL.** PS3 and the deck lamp are still D.

| Item | Grade | Frames | Why, and the one fix |
|---|---|---|---|
| Ramps and landings | C, passes | Found_04, 06, 18, 19 | The open post-and-rail reads as part of the stair. The view comes back (treetops, lake, ridge), and the battlement read is gone. Fix: the outer posts are heavy, with a coarse, blocky texture at 1 m (Found_06, 18, right edge). Use thinner posts with a finer texture. Dressing task. |
| PS3 | **D** | Found_20 (16.2 m) | The mark still reads as grass at the stack's west foot. No boulder top shows under it, and the talus humps behind it sit higher than the mark. I can't confirm it is on BigBoulders_1. Fix: show it in a frame where the rock's top is under the mark, or move the spot to a rock that shows from the trail. |
| Deck lamp by eye | **D** | DeckLamp_Night_Eye, DeckLamp_Night_Binoculars | By eye: no warm point where Camp 2 should be. Only the red mast and the lot lights show. Through binoculars: a 1 to 2 px dim grey speck at the centre, not a lamp. Fix (one lighting change): raise the lamp's emissive core until it sits at least 40 grey over the frame mean in the binocular frame (the Gate.md night N1 bar). Keep the light's range as it is, so it stays a point and does not flood the top. The coder sets the value in LookTuning. |
| Two phones | C, passes | AreaFrames_08, Found_09 | Now one phone, on the back wall, and the jamb handset is gone. Fix: the dark receiver on a dark body cannot be seen (AreaFrames_08), and the two pale rings are left over from the old mesh. Give the receiver a lighter value or a metal cradle, and cover the rings. |

Vesper
