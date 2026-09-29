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

## 8.3 Trails and giants

- Trails inside the keeper's camp clearing (15 m) are not built: the tower stands on the Camp to J line 8.5 m from the camp centre.
- Footbridge: the map creek passes 8 m north of the map footbridge (114.8, 85.2), and the W1 to Camp 3 trail runs on the creek's south-west bank without crossing it -> the footbridge stands on the trail at its map point over dry ground.
- W1 to cave: the trail ends on the ravine floor at (52, 37.5), in front of the mouth face; the last 3.5 m to the mouth (52, 34) is 8.8's passage.
- Points of interest along the leg (m, table 4 -> built): water tank 40 -> 42; rowboat 42 -> 43; phone pole 48 -> 41; food lockers 47 -> 45; Hollow Giant 45 -> 39; forage A 95 -> 96; Gate Tree 40 -> 34; first sight of the lot 85 -> 89; latrine 42 -> 42; forage B 40 -> 40; log steps 90 -> 107; truck 40 -> 39; stepping stones 80 -> 74; footbridge 25 -> 27; camper 70 -> 71; rope handrail 36 -> 35; bulbs 72 -> 68; burn-map board 48 -> 53; plank bridge 90 -> 95; rune post 50 -> 57. Positions follow the map; the log steps and plank bridge differ most because the map places them near the leg ends.
- Giant field: the doc places no giants except the three heroes. Built by rule: 46 giants 30 m or more apart, 40 to 50 m tall, tops at most 50 (46 west of x 32), none on the spur (its ground leaves no 40 m giant under the caps), none in clearings, trails, the lake, ravine, hollow, old burn, front zone, or within 3 m plus crown of the five tower cones on the map.
- Lot walks T to office / store / booth are not trails (8.6 gravels the lot): straight segments 36.4 / 41.0 / 53.4 m against 35 / 42 / 52.

## 8.4 Lake

- Dock: map dock (z 86.4 to 96) reaches only 1.2 m past the water line; built as drawn: deck -4.8 from z 86.4 to 90, then a gentle ramp to the -4.5 root at z 96, pump at (190, 94.8).
- Boathouse door and gangway (not in the doc): door in the east wall at z 52.4 facing the shore, a 3.8 m gangway to the ground; the lake resident's spot is inside by the west wall.
- Wade limit: invisible boxes at 0.97 of the lake radii, tops at -5.0 under the dock deck, so the player wades the edge and never walks into deep water.
- Trail ends at named points are pinned to their table 2.1 heights (J 10, pump -4.5, W1 -4.5 and the rest).

## 8.5 Campsites

- Camp 1: the spar stands at the camp centre where the map draws it, with the bulb string running 14 m west to a stake; one tent, three workbenches, lumber, cookfire (Food), resident spot, all placed by me (the doc gives only "sprawling workshop, 60 m").
- Camp 2: granite stack 12 m across at the base (map), tapering to 9.6 m, top 24; trails that end at (292, 108) now end at the stack foot. Ladder on the west-south-west face toward the arriving trail is looks only: the game has no ladder climb, so the tent and resident spot on top cannot be reached on foot yet.
- Camp 3: tent, fire (Warmth) and seat log in the hollow west of the creek; the green-glass lantern hangs on the Snag's hollow side 3 m above the rim (the doc's event state "the Snag's lantern missing" implies it hangs there).

## 8.6 Front zone and gate

- Office doorway on the lot side at x 348, store doorway at x 366, booth doorway on its west wall (toward the spur), booth window on the lane side; none of these are in the doc.
- Mast: map base at the office north-east corner, built at (357, 205) just outside the walls; three-leg lattice stand-in with the red lamp at 30 m.
- Resident's car at the map point (370, 179.4), doc (370, 178).
- Shift walls (3.2.7) are built but off: nothing starts a shift until the gate minigame exists; the "You can't abandon your post." message also waits for it. The gate PlayerBlocker is always on; it blocks everything for now, so cars will need their own layer when they exist.
- Closed campground: loop road ring (5 m wide, 40 m outside) with eight empty pitches; the spur runs on to the loop at z 242.

## 8.7 Ward climb and ledge

- Cairn gate: pale cairn 2.2 m beside the trail start at J with a chain across the trail to a post. The chain has no collider: nothing makes the climb night only yet, and a solid chain would stop the climb.
- The Tor: dome of radius 18 m, top 58, built as an ellipsoid (vertical radius 28, centre y 30) so it meets the lower ground on its south-east side (its base circle runs 7 m past the spur edge) without floating. Closest point of the climb trail is 22 m from its centre.
- Ward stones at the map positions, 3.6 x 4 m, tops 48 (5.4); they stand 11.8 m in from the cliff, as in the 8.1 note.

## 8.8 Cultist cave

- Ramp legs: 3.3.2 "16 m legs at about 14 degrees" -> each leg slopes over 13 m (17 degrees) between level 3 m turns centred on x 52 and x 68, so the legs keep their 16 m spacing, the turns are level, and the east turn stays 1.5 m clear of the chamber (x 71). Walked mouth to chamber: 73 m of floor as in 3.3.4.
- Entrance passage 3 m wide (the doc gives the legs' width; the entrance width is not stated), 4 m tall, running from the mouth face at z 37.4 to z 20.5.
- Mouth: terrain hole of 35 cells, x 50.0 to 53.9, z 33.4 to 37.5; a rock block fills it above the passage ceiling. The day-one board (CLOSED, UNSAFE) has no collider until a day system exists.

## 8.9 Sightline check

- Report: Docs/Layout/Main3/Main3_sightlines.md. Every place is seen from the deck; the Ward (Tor) and the cave (rim and terrain) are hidden from all 64 eye points, also with eyes and targets raised 3 m.
- Hidden margins (least-hidden line): W-1 4.2 m at eye height (5.0 from the deck centre, doc 6.4), 0.9 m at +3 m jump; C-1 9.2 m (9.3 from the centre, doc 8.3), 6.0 m at +3 m jump (doc about 5.3).
- For Sable: the Hollow Giant (map (202, 140), a 40 to 50 m giant, top 50 here) stands 1.8 m outside the map's Camp 2 cone and its crown reaches into it: the stack top is seen from 37 of 64 eye points with 0.5 m margin from the deck centre (doc 3.8, cone rule 3), and the boulder field is not seen. The same crown hides the cab from the Camp 2 stack foot.

## 8.9a Fix batch (after Marlow's walk and Tully's check)

- Camp 2: switchback path up the stack's east face (four 26.6 degree ramps on two lanes, z 108 to 118, rails, a top landing over the stack top). Boulders keep off it and off the Camp 2 view line.
- Hollow Giant crown: radius 5.5, centred 2.5 m south of the trunk -> 3.5 m clear of the deck-centre lines to the Camp 2 stack.
- Ward stones: map x 21.8 -> x 12.5 on the cliff edge (DECISIONS 2026-09-25 wins over the map).
- Tor: top 58 -> 63.5 (raised 5.5 m) so W-1 keeps 3.8 m with eyes and targets raised 3 m.
- Snag: map (90, 146) -> (96, 146.5), 6 m east on the rim, so the log-steps trail clears its trunk and the cab shows from the Camp 3 centre.
- Camp 2 view: ground under the line from the Camp 2 junction (286.5, 102.2) to the boathouse roof lowered to 1.3 m under the line; giants and hedges kept under it.
- Off-trail: hedges line the outline of the walkable ground (trail corridors 2.2 m each side, clearings, lake landings, front-zone surfaces, ledge, cave approach): 1.2 m thicket, 5 m regrowth in the old burn, 4 m in its last 40 m. The regrowth near T hides the lot's west edge from the tower (5.3 claims a 2.6 m margin over it); the lot centre, office, store, car and mast stay seen.
- Lake: wade boxes stand 0.9 m over the ground where the ring runs up the bank (buried where a shore trail crosses it); dock rails over the water, so there is no stepping off and no way back needed; shore trails keep 6 percent of the lake radii off the water line.
- Shift walls: a U across the spur mouth and a closed ring round the turning circle; still off until shifts exist.
- Cairn gate 4 m up the J to Ward trail with the cairn on its west side; the chain and the cave's day-one board are solid, so the Ward climb and the cave are closed by day (dev warps reach both).
- Boathouse trail end moved from (246, 57) to (247.6, 52.4), the gangway foot. Footbridge rails have no colliders. 8.1 builds no gate wall.

## 8.9b Fix batch 2 (after Marlow's re-walk)

- Off-trail blocking: the gray hedges became invisible walls 4 m over the highest ground under them (Ignore Raycast layer, so no sight line sees them) with 0.3 m gray markers; real vegetation is Milestone 11. The lot's west edge is seen from the tower again.
- Cave: the walkable ground runs 7 m into the mouth, so no wall crosses the passage.
- Closed campground: the spur's walkable ground and gravel run on to the loop ring at (387.5, 252).
- Lake: the 4 m walls hold every shore trail, the dock and the boathouse against jumping.
- Ward gate: an invisible block 3 m tall across the whole trail corridor under the chain, so it cannot be crouched under, jumped or passed round the cairn.
- W1 to the Snag kept clear of giants.

## 8.9c Gray-box feedback (revision 13)

- Knoll 15 with a linear flank to the ground by 63 m (45 m blend, under 25 percent); the knoll no longer feeds the smooth ground field, so the ground round the lake stays low and the pump still sees the Snag.
- Tower: legs at 3.75 m (7.5 m square) to fit the spiral inside; steps have no colliders (a step edge caught the capsule where a ramp met a landing); flight rails stop short of the landings. Climb walked: 56.0 m, 22.4 s at 2.5 m/s.
- Ward climb keeps 21 m from the Tor's centre (the meander ran it into the Tor's foot); J to the lip walked 147 m, 59 s.
- Stand-in fire is built switched off. With the scene's night fog (end 200 m) the Game view from the lip shows none of it; 5.8's fog end past 300 m for Main3 is not done yet (look work). The frame check shot is taken with fog off.
