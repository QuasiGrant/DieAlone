# 8.25a Camp 2 granite knob, Gate step 3 (Vesper)

Frames: Docs/Captures/Main3Review_camp2 (AreaFrames F1, F4, F5, F8, F9; Found 18 to 20). Spec: Camp2Layout.md draft 4 and section 0. Layout only. C or better passes.

**Verdict: FAIL.** The knob is D, and so is the rockfall. The question that matters: it does **not** read as a natural granite knob. It reads as a black box with boulders piled at its foot. It is less strange than the column, but still built, not found.

| Place | Grade | Frames | Why |
|---|---|---|---|
| The knob | **D** | F4, F5, F8, Found_20 | **F5 and Found_20:** a near-black rectangular block, straight level top edge, vertical flat sides. The BK boulders cover only its lower third. **F4:** the same block with sharp corners. **F8:** the flat face Rook named shows high on the right, and a level top edge runs the frame's width. A knob has a rounded, broken skyline and grades into talus; this has neither. |
| Scramble | C | Found_19, F8 | Rock steps up the west side, with pale boulders along the open edge. It reads as a way up. Fix: the steps are dark slabs that read as stair treads. Give them the boulder tint. |
| Top: tent, chair, lamp | **D** | F9, Found_18 | **The view works:** F9 shows the T, the road, the mast lamp and the dead trees past the chair. **The top does not:** a perfectly flat, smooth plane with a straight edge, and the gully head cut as a square notch with stair edges (Found_18). **The lamp core is a white clipped rectangle** in F4, F5, F9 and Found_20, which breaks hard line 5 (no clipped white but the sun). |
| Booth and table | C | F1 | The booth, table and boulder read at the foot. Fix: behind them the knob is a dark block between the trunks. |
| PS1 to PS3 | C | Found_18, 19, 20 | Each mark sits on a real thing: the top's edge, a step block, the rockfall boulder. |
| Rockfall | **D** | F5, Found_20 | The two boulders are pale grey eggs against the black face. With that contrast they look placed, not fallen. |

## What makes it read as a box, and the fix
1. **The core is visible.**
   - The near-black faces with a straight top line are the core collider's own surface, or a skin that stops at about a third of the height. Either way, no boulder breaks the top edge.
   - **Fix:** turn the core's renderer off, and skin it so boulders form the skyline. Use BigBoulders at scale 1.3 to 1.6, set high on the faces, with tops rising 0.3 to 1.0 m over the 9.0 top at the edges (not over the chair's view fan, bearings 64 to 72). The knob's outline then rises and falls; no straight top line longer than 2 m shows from F4, F5 or F8.
2. **One tint for all of the knob's rock.**
   - The knob skin, the scramble steps and the rockfall must share one tint and material, so they read as the same granite. Today: black faces, pale rockfall, dark-slab steps.
   - Use the BK boulder material at its pack value, retinted only toward #6E6862. The rockfall stays as it is.
3. **The top.**
   - Keep the flat walk collider, but cover its drawn surface with Boulder_0 to 5 at low scale, laid flat and sunk, plus RubbleSparse at the edges, so the top reads as weathered rock with a lip, not a slab.
   - The gully head gets two boulders on its edges, so the cut is not square.
4. **Lamp.**
   - Lower the lamp's emissive until its core is the brightest thing in the frame but not clipped: under 230 grey in F5 and F9, filter on.
   - The coder sets the value in LookTuning. The deck must still see the core (the 8.25 rule).

Rook's south-west flat face (F8): it is the same issue as point 1. Cover it with a boulder on the face above leg 1's walk height, with d 0 to the face, so leg 1's clear width is untouched. If no fit clears leg 1, a smaller Boulder (scale 0.6) set high, from 2.5 m up, does.

Vesper

## Round 2 (final), 2026-10-03

Frames: AreaFrames F4, F5, F8, F9, F10, F11, F12 (6952b9f). Layout only.

**Verdict: PASS.** Knob, top and rockfall are all C or better. On the question that matters: **yes, it now reads as a natural granite knob.** From both trail ends it is a rounded pile of weathered boulders with a broken skyline, no box. A man could have found it and climbed it.

| Place | Grade | Frames | Why, and the fix |
|---|---|---|---|
| The knob | C, passes | F4, F5, F8 | F4 and F8: rounded masses and a broken skyline; the core is gone. **F5 fix:** at the right of the skyline there is still a dark block with a level top and a square corner, about a fifth of the frame wide (Rook's flat edge). One BigBoulders at scale 1.3 on that corner, its top 0.5 m over the edge, ends it. |
| Top: tent, chair, lamp | B, ships | F9 | The top is now lumpy rock with boulders at its lip, and the tent, chair and lamp sit on it. The view to the T, the road and the mast is intact. Note: the lamp's card reads near-white in F5 and F9. At Rook's 207 grey it passes hard line 5; I can't confirm the value from a JPEG. |
| Rockfall | C, passes | F5 | With one tint, the pale eggs are gone and the rockfall sits as part of the knob's foot. Fix: it is now hard to pick out at all. That is fine for dressing, but PS3 sits on it, so keep PS3's paper the palest thing there. |

Rook's three flags:
1. **F12, the west boulder over leg 1:** it reads as a perched boulder on a ledge. That is natural on granite and I keep it. What reads wrong is the face under it: a flat vertical plane with a straight edge from frame top to bottom. One boulder on that face, set from 2.5 m up, clear of leg 1, fixes it. Logged; not a fail.
2. **F11, the cairn:** it is lost. It stands pale among the pale boulder field behind it, so at 15 m it reads as one more rock. Fix: move it 1 to 2 m east so the dark knob face is its background, or darken the boulders behind it. A cairn needs a dark ground to read.
3. **F5, the flat edge:** see the knob row above.

Vesper
