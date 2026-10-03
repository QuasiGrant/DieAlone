# 8.26 gate, design check: Camp 3 and the west trails as built
2026-10-03, Sable. Against Camp3Layout.md draft 2.
**Sources:**
- Docs/Captures/Main3Review_camp3/Checks.md (Rook, 2026-10-03 03:21: all scripted checks pass);
- the AreaFrames, Found, Places and Deck sheets;
- Wren's measured walks and the three changes he accepted.

**Verdict: FAIL on two items (the escape-room entry and the dam), both one-collider fixes. Everything else passes.**

| # | Item | Verdict | Evidence and reason |
|---|---|---|---|
| 1 | **The loop** | PASS | **Times:** camp to Camp 3 41.3 s, Camp 3 to W1 44.6 s, W1 to the pump 30.9 s, the pump to camp 48 s (LakeLayout). The west loop runs about 165 s, under Valley's 172. **Every leg has its next place in view:** the Snag ahead on the way down (found 28.8 m); the spring from the trench foot (F6, 29.6 m); the W1 sign (F4) and blaze (F5); the stepping stones (16.6 m). **The creek** falls the whole way (0.00 m rise on both runs) and keeps its 1.0 m edge (least 1.09 m). Area flood: 0 traps, 0 leaks; T1's FaceRock is hulled (41 of 41) |
| 2 | **Escape-room entry (the easel)** | **FAIL** | The layout holds: the easel is reached and found (28.8 m from W1 to Camp 3), and from the arrival it is 2.3 s round the table. Its painted face turns from the deck (pixel 0 of 128) and shows from the floor (sink frame: the canvas faces the camp). **But the interaction ray from its stand (79.0, 149.9) facing 130 meets no collider within 2.0 m** (Checks, SPACING): TheCanvas/Painting has no collider, so the entry can never be pressed. **Fix:** a thin box collider on TheCanvas's face, 0.9 x 1.1 x 0.05, non-trigger, on the interactor's layer. Then Rook re-fires the stand's ray, facing 130 and about 8 degrees down (eye 1.6 m, canvas centre 1.3 m) |
| 3 | **The job form** | PASS | Its ray hits the table at 1.2 m from (78.6, 148.4) facing 180. The table is 0.76 m edge to edge from the easel (2.1 m between centres), so it is in reach of the easel seat and a few steps from the fire (2.6 m); found from W1 to Camp 3 at 21.5 m. It is a camp find (Wren's exemption), on the cleanest surface in the camp |
| 4 | **The Snag line** | PASS | Pieces 1 to 3 seen from 128 of 128 deck eyes (hard), piece 4 from 68 (loose). The least clearance is 2.87 m with the stake at 3.60, so nobody walks into a piece. F3 from the floor shows the rope and the backs against the sky. The pieces are out of found and reach by design: the line is read, not handled. The Snag lantern reads 0 of 128 (the trunk stands in front); it is loose and the line is the deck's read of Camp 3. Wren's rope end at 3.56 m stands |
| 5 | **The flood event at the dam** | **FAIL** | The layout holds: the pool sits in the floor's low point, the sink at its south lip; the dam stand is 1.5 s from the arrival, found at 6.6 m, 3.3 m from the easel. **But the stand's ray (82.6, 147.2) facing 45 meets nothing within 2.0 m.** The sink's collider is 0.2 x 0.1 m with its top at -4.3, under the floor at -4.1, so `Clear the dam` can never be pressed. **Fix:** a debris collider at the sink, 0.8 x 0.4 x 0.6, top 0.4 over the floor, centred (83.6, 148.45), non-trigger (under the 0.70 climb, leads nowhere). Its ray faces 45 at about 40 degrees down (eye 1.6, target 0.2, 1.7 m) |
| 6 | Tent door east (Wren's change) | PASS, no objection | North mattered only to face the fire; east faces the arrival and the W1 leg, which is as good for a door. The tent's box matches its mesh exactly (2.13 x 1.64 x 4.07), and the studio is 2.7 m or more from it. **The porch ground sheet moves to the east door** (x 77.04 to 78.54, flat, no collider), keeping 1.0 m from the W1 tread edge |
| 7 | Three plain pocket fills by the tent and canvases | HURT (layout) | In Places, CAMP 3 CANVASES AT THE BANK, a plain grey box stands at the bank beside the canvas row and reads as a box. Quill 2 asks for "nothing in front" of the canvases' backs. **Fix:** each fill stays where the flood needs it, but none stands between the canvas row and the floor. It is skinned with the owned rock material like the other cave and hollow boxes. The canvases are still found at 29.7 m, so this is not a FAIL |
| 8 | Times as measured | PASS | Arrival to the fire 5.0 s, to the easel 2.3 s, to the dam stand 1.5 s; the steps' top to the east rim spot 1.9 s. The whole floor is within 5 s of the arrival |

**Not graded here:**
- the look: Vesper (the pool reads as flat blue sheets, and a fern fills F6's foreground, both dressing);
- the warps: all 3 land.

**Done when:** the two colliders above are added and their rays re-fired from the stands; the fills are moved or skinned.

Sable
