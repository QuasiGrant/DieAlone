# 8.27a Cave rock, Gate step 3 (Vesper)

Built from Docs/Design/CaveRock.md draft 1, commit 59d8399. Frames: AreaFrames F3 and F4; CaveFrames Day_one and Day_two for V9Open_DeadEnd, V9Open_FromSideRoom, V9Shut_FromChamber and BulbsTrail. Layout only. C or better passes.

Correction: my tables list 46 rocks, not the 51 I reported. 46 is right.

**Verdict: FAIL.** Passage, chamber and the V9 doorway are D. The dead end is C and the bulb tree is B.

| Item | Grade | Frames | Why |
|---|---|---|---|
| Entrance passage | **D** | AreaFrames_03 (F3) | The frame matches round 2 by eye. It still stands at (52, 24) heading 180, not the reframe in CaveRock A, (52.0, 27.5) heading 165. None of P1 to P4 shows, although P1 and P4 should sit in this view at the left. I cannot tell whether the rocks are missing or the frame is stale. |
| Chamber | **D**, not from darkness | AreaFrames_04 (F4), CaveFrames_Day_one/two_V9Shut_FromChamber | The ceiling now shows rounded rock masses (F4 top left; V9Shut top). The walls do not: the east wall at about 18 m is light enough to show its tile pattern and seams, and it is still flat. No bulge from L12 to L15 or H8 to H10 breaks its outline, and his rock behind the shelf (L12, H8) is not there. The same goes for the south and north walls. A 0.3 to 1.0 m bulge on a 5 to 7 m rock would break the silhouette at this light. The floor rubble shows only as small dark specks. |
| V9 doorway | **D** | CaveFrames_Day_one/two_V9Open_FromSideRoom | The opening is still a clean rectangle with a pale flat frame. V1 to V3 (jambs and lintel, d 0.4 to 0.5 into the room) do not show. The two room boulders are the ones that were there before. |
| Dead end | C, passes | CaveFrames_Day_one/two_V9Open_DeadEnd | V4 reads: the passage ends at a rounded rock mass, not a slab. Fix: the side walls and lid are still box faces. V5 and V6 do not show; check them under the same rule as below. |
| Bulb tree | B, passes | CaveFrames_Day_one/two_BulbsTrail | Day one: a bare leafless tree by the trail, the right shape for the spot. Day two: about 14 coloured bulbs (red, amber, green, blue) wound on it, all reading clearly at about 17 m in the red-dark frame. Note: the bulbs render as small squares; a round bulb mesh would read better close up. |

## What I think happened (unverified)
The ceiling pieces show and the wall pieces do not. That points to how depth d was applied, not to darkness. If d was measured from the far face, or with the sign reversed, every wall rock sits inside its box collider and out of sight. The ceiling pieces were placed by "bottom", not by d, which would explain why only they appear.

Check before the next round, one line each:
1. For L1, L12, V1 and P1, print the room-facing bounds face and the wall face. The bounds face must lie d m on the room side (CaveRock method 2).
2. Then recapture F3 at the reframe position, plus F4, V9Open_FromSideRoom and Found_06 (niche).

Darkness: not the blocker here. The flat walls show clearly enough to grade. Lighting stays with 11.0.

Vesper
