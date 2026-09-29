# Main3 build notes

Rook, Milestone 8 blockout, 2026-09-29. Source: Main3.md revision 11 and Main3_map.svg. Rule (Wren): the map drawing wins for positions and shapes, table 2.1 wins for heights; a conflict that would break a DECISIONS.md line stops the build. One line per resolution, old value then new value, for Sable to fold back. Map pixels convert as x = (px - 60) / 2.5, z = (820 - py) / 2.5. Differences under 1.5 m are drawing precision and are not listed.

## Audit before 8.1

- Spur crest: 2.1 (35, 140) to (62, 195) at 34 -> map line (72, 212) to (108, 284) at 34, rising to 36 at the Ward ledge, J (104, 206) at the foot (Wren's call).
- Plank bridge: 2.1 (110, 200) -> map point (104.8, 203.2), which lies on the creek line; the creek runs through it.
- Creek spring: 2.1 (100, 215) -> map start (104, 215.2).
- Creek mouth: map line stops at (128, 78), 12 m short of the water -> extended to the shore at (136.5, 66), beside W1.
- Ravine extent: map polygon reaches z 20, but 2.1 and 3.3.8 put ground 0 over the cave (z 3 to 22) and the entrance passage (z 22 to 34) -> the -6 floor starts north of the mouth face (z 37 and up); the polygon strip south of z 34 stays at 0.
- Ravine rim 18 m zone: 5.5 "20 m either side of (74, 60)" -> 20 m west, 15 m east, because the map rim line ends 23 m east of the vertex and tapers to the W1 to cave trail entry over its last 8 m. 8.9 C-1 checks the result.
- Dock root: 2.1 ground -4.5 at (190, 96) against "banks rise to 0 within 15 m" -> a 5 m wide notch at -4.5 from the water to z 96 under the map dock (x 188.4 to 191.6, z 86.4 to 96).
- Camp 3 hollow profile: 2.1 "rim ring 12 to 20 m out" -> floor -4 inside r 8 (the map's dashed circle), wall r 8 to 12.4, rim 4 from r 12.4 to 20, blending to the ground by r 30. The creek cuts the rim north and south.
- Ward stones: 3 "stones on the cliff edge" at the ledge (32, 258) -> map stones at x 21.8, z 252, 258 and 264, 11.8 m in from the cliff at x 10.
- Route lengths: every map curve is shorter than table 4. The map curve is the centre line, with the ends and points of interest at their map positions; each trail meanders about it to reach the table length within 5 percent. Camp to pump 67.2 -> 80; pump to boathouse 72.3 -> 84; boathouse to Camp 2 80.2 -> 95; Camp 2 to T 79.8 -> 94; Camp to Jg 100.5 -> 125; Jg to T 84.1 -> 105; Jg to Camp 1 69.7 -> 83; Camp to Camp 3 99.2 -> 126; pump to W1 68.1 -> 80; W1 to Camp 3 91.4 -> 114; W1 to cave 85.7 -> 109; Camp to J 81.3 -> 96; Ward climb polyline 114.1 -> 130.
- Positions the doc leaves to the map: mast at the office north-east corner (356, 204); Camp 2 granite stack 12 m across (map r 6); office 12 x 8 m; store 8 x 5.6 m; boathouse 6.4 x 5.6 m.
- Keeper's camp layout (not in the doc or the map): cabin centred (178, 168) in the clearing's north-east quarter, the only quarter no trail leaves through; player spawn inside it.
- Creek through the hollow: map line (84, 150) -> (100, 110) passes 4 m from the Snag, which 2.1 puts at ground 4 on the rim -> creek rerouted (84, 150), (78, 146), (84, 128), (100, 110), 11 m clear of the Snag; the W1 to Camp 3 trail enters the hollow along this cut.

## 8.2 Camp and tower

- Camp items the doc places only by name -> fire pit (172, 163), generator (181.5, 171.5) behind the cabin, stove inside the cabin, lectern inside the cab facing north, report box on the cabin desk.
- Tower frame: Vesper's timber legs built (square posts, beams every 8 m, X braces), gray. Deck 8 x 8 m, cab 4.4 x 4.4 m with a 1.8 m walkway, stairs a south switchback of 12 flights, arriving at the deck's south-west corner.
