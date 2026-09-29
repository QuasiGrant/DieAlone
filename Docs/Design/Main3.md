# Main3 paper brief

**DRAFT, revision 12, 2026-09-29, Sable. Revision 7 approved pending resident placement (DECISIONS 2026-09-29). Revision 12 folds in Rook's gray blockout resolutions (Docs/Design/Main3_BuildNotes.md) so this file matches what was built; three items stay only in the build notes (trail ends pinned, shift walls off, the sightline report line). Rook's fix batch after Marlow's walk may change more. Nothing else here is decided.** The loop lives in Docs/Design/DailyLoop.md and events in Docs/Design/Events.md; neither is repeated here. Map: Docs/Design/Main3_map.svg.

Coordinates in metres, origin south-west, x east, z north. Heights absolute (metres above map base 0). Walk times at 2.5 m/s are information for pacing and events; there is no day time budget (DECISIONS 2026-09-29). Where the map drawing and this file differ, the map wins for positions and shapes and table 2.1 wins for heights (Wren's build rule).

## 1. Purpose

1. A map where near and far feel different, and no two places look, sound or work the same.
2. On day 1 it is a lookout's patch of forest and nothing more. The tower sees every place it must check; the Ward and the cave, the two that take from you, are the two it cannot see.

## 2. Shape

1. **Size: 400 x 300 m.** Fire to the west, forest in the middle, the front zone inside the gate on the east edge.
2. West edge: cliff along x 10, valley floor -40 m below, burning ridge 300 to 500 m beyond.
3. North-west: the spur, high ground to 36 m, the Ward on its cliff edge behind the Tor.
4. South-west: a ravine with the cultist cave below a rock rim.
5. Centre: the keeper's camp on an 8 m knoll with the tower; the lake south; Camp 3 in a hollow west.
6. East: Camp 1 and Camp 2 in the forest, then the front zone.
7. **The fence and gate are the map edge.** Chain-link along x 396, the full height of the map. The gate at (396, 170). The road comes from off-map to the gate and never enters. Inside the gate: a gravel drive, the parking lot, the office and the store.
8. Giant trees: emergents 40 to 50 m tall, trunks 6 to 10 m. Tops never above 50 m absolute (46 west of x 32); on the knoll at most 42 m tall; none on the spur (its ground leaves no room for a 40 m giant under the caps). One stated exemption: the Snag at Camp 3, top 54 m, 3.6 m under the tower eye. **As built:** 46 giants by rule, 30 m or more apart, none in clearings, trails, the lake, ravine, hollow, old burn or front zone, and none within 3 m plus crown of the five tower cones (see 5.10 for one exception found).
9. Routes stay on trails (DailyLoop.md 1.2).
10. **The old burn** (DECISIONS 2026-09-29: the tower sees more of the office and lot). A years-old burn scar runs from the foot of the knoll east to the front zone: 30 m wide at the knoll (x 185, z 151 to 181), widening to 70 m at the front zone (x 340, z 143 to 213). No giants and no mid canopy in it: dense young regrowth 4 to 6 m tall, 4 m in the last 40 m before the front zone. The thicket is impassable off the trails, so it opens the view, not a shortcut. An old burn under a lookout is ordinary on day 1.

### 2.1 Ground heights (absolute, metres, as built)

Base ground is 0 in the south-west, rolling up to 5 in the north-east. Between named points the blockout interpolates smoothly.

| Point | Position | Ground m | Notes |
|---|---|---|---|
| Keeper's camp, knoll top | (170, 160) | 8 | tower base (164, 166) at 8 |
| Lake water surface | (190, 60) | -5.5 | 110 x 55 m |
| Lake bed | deepest, centre | -8 | 2.5 m deep, shelving from the shore |
| Lake shore line | all round | -4.5 | banks rise to 0 within 15 m of the water |
| Dock notch | x 188.4 to 191.6, z 86.4 to 96 | -4.5 | a 5 m wide cut through the bank from the water to the dock root |
| Pump and dock root | pump (190, 94.8), root (190, 96) | -4.5 | dock deck -4.8 from z 86.4 to 90, then a gentle ramp to the root |
| Boathouse | (240, 52) | shore -4.5 | on stilts over the water, floor -3.8 |
| W1, west shore junction | (128, 70) | -4.5 | creek mouth beside it; stepping stones -5.3 |
| Sill between lake and ravine | x 100 to 120, z 45 to 65 | -3 or higher | keeps the lake from draining into the ravine |
| Creek spring | (104, 215.2) | 10 | |
| Plank bridge on Camp to J | (104.8, 203.2) | 9 | on the creek line; creek bed 8 |
| Camp 3 hollow floor | (78, 146), inside r 8 | -4 | wall from r 8 to 12.4 |
| Camp 3 rim | r 12.4 to 20 | 4 | blends to ground by r 30; the creek cuts it north and south |
| The Snag | east rim (90, 146) | 4 | 50 m dead giant, top 54 |
| Creek line | (104, 215.2), J, (84, 150), (78, 146), (84, 128), (100, 110), mouth (136.5, 66) | 10 falling to -5.5 | rerouted to pass 11 m clear of the Snag; reaches the shore beside W1 |
| J, Ward junction | (104, 206) | 10 | at the foot of the spur |
| Spur crest | (72, 212) to (108, 284) | 34 | rising to 36 at the Ward ledge |
| The Tor (rock outcrop) | (76, 223) | base 34, top 58 | |
| Ward ledge | (32, 258) | 36 | |
| Ravine floor | north of the mouth face (z 37.4 and up) | -6 | the strip south of z 34 stays at 0 |
| Cave mouth | face at z 37.4, x 50 to 53.9 | floor -6 | 4 m tall, top -2; terrain hole x 50.0 to 53.9, z 33.4 to 37.5, a rock block above the passage ceiling |
| Ravine rim, north side | along the rim line | 14 | 18 for 20 m west and 15 m east of (74, 60) (the rim line ends 23 m east of the vertex and tapers to the cave trail entry over its last 8 m) |
| Surface over the cave interior | x 44 to 89, z 3 to 37.4 | 0 | |
| Hollow Giant | (202, 140) | 6 | top 50 |
| Forage patch A / B | on Camp to Jg at 96 m / Camp to Camp 3 at 40 m | 5 / 6 | |
| Jg | (262, 172) | 5 | in the old burn |
| Old burn | knoll foot to front zone | 8 falling to 3 | |
| Gate Tree | (290, 176) | 4 | stub 15 m |
| Camp 1 | (282, 238) | 5 | |
| Camp 2 boulder field / stack top | (292, 108) | 4 / 24 | |
| Front zone (lot, office, store, T, drive, booth, turning circle, spur, loop) | x 340 to 396 | 3 | flat |

## 3. Places

Six locations with one resident each, plus the keeper's camp and the Ward. Everything reads ordinary on day 1 (DailyLoop.md 6). Sounds heard within 35 m (camp 40 m) unless noted; Hollis owns the sound column.

| Place | Position, ground | Size and feel | Actions (need) | Seen from tower as | Sound |
|---|---|---|---|---|---|
| Keeper's camp | (170, 160), knoll 8 m | 36 m clearing, warm light; layout 3.4 | tower and lectern; stove (Warmth) | the tower itself | wind vane squeal |
| Lake | water (190, 60), 110 x 55 m, surface -5.5, bed -8, shore -4.5 (2.1); pump on the north dock (190, 94.8); resident's boathouse on stilts (240, 52), 6.4 x 5.6 m | the one wide open sky | pump (Water); talk; events | open water, boathouse tin roof | lapping, dock chain |
| Camp 1 | (282, 238), ground 5 m | big by kit, one tent: a sprawling workshop camp, 60 m, pale harsh light | cookfire meal (Food, some days); talk; events | a lashed timber spar 24 m with a string of bulbs | pots |
| Camp 2 | (292, 108), ground 4 m | tiny: one tent on top of a 20 m granite stack (12 m across at the base, 9.6 m at the top) in a 40 m boulder field, ladder up, cold white tent lamp (Vesper) | rain barrel (Water, some days); talk; events | the granite stack | wind |
| Camp 3 | (78, 146), hollow floor -4 m | sunk 8 m below its rim, 25 m across, dense, green-glass lantern | sit at the fire (Warmth, some days); talk; events | the Snag: a bleached dead giant on the east rim (ground 4, 50 m tall), top 54 m | creek through the hollow, a low fire |
| Office | (350, 200), front zone, ground 3 m, 12 x 8 m | prefab ranger office on the north side of the lot; store 16 m east at (366, 200), 8 x 5.6 m. This location's resident lives around the lot where the drive starts: a car in the lot's north-east corner at (370, 179.4). Gate booth and closed campground: 3.2 | talk (Social, starts his gate minigame); events; store (Food) | the whole parking lot, office, store and mast (30 m, red lamp), in a wide view across the old burn | mains hum; radio with ordinary chatter on day 1, going to static after the reveal |
| Cultist cave | mouth face (52, 37.4), ravine floor -6 m, mouth 4 m tall | weird on purpose (DECISIONS 2026-09-29): rave lights and music inside, busting the grim cult cave trope; the rock outside stays grey, so the light spilling up the descent is the surprise. Interior: 3.3 | talk (Social); its own events; story session | hidden (5.5), never checked | chant from night 1, heard at two trail spots; inside it slowly turns into bass on the way down (DECISIONS 2026-09-29) |
| Ward ledge | (32, 258), ground 36 m | stones at x 21.8, z 252, 258 and 264, 11.8 m in from the cliff at x 10, facing west; each 3.6 x 4 m, tops 48; 25 m clearing | night only: the Ward screen | hidden (5.4) | silence from the last bend |

1. Event states change a landmark, never remove it: bulbs out on the Camp 1 spar, the Camp 2 tent lamp dark, the Snag's lantern missing, the office lamp steady instead of blinking. Details are Vesper's and Pim's.
2. Poles: only two, and they differ: Camp 1's lashed timber spar with bulbs, the office's steel lattice mast with a red lamp. Camp 2 and Camp 3 use rock and a dead tree.
3. **Tower legs are timber** (Vesper's proposal, built): square posts, beams every 8 m, X braces. The office mast is the only steel lattice on the map.
4. Resident hooks and roles: placeholders for the story session. Nothing that leans toward the ending goes in this file.
5. The Tor, the granite stack and the cave mouth are meshes, not terrain (Rook). The Tor is built as an ellipsoid (horizontal radius 18, vertical radius 28, centre y 30) so it meets the lower ground on its south-east side without floating.

### 3.1 Front zone
1. The strip inside the gate, x 340 to 396. The only man-made ground. The Wardkeeper never goes beyond the fence (DECISIONS 2026-09-29).
2. Road: outside the gate, off-map to the east. Day 1: ordinary distant traffic. After the night 1 reveal it goes quiet as WARD drops, and creepy sounds replace what drops out (DailyLoop.md 6).
3. Gate (396, 170) with a lift barrier. Gravel drive west from the gate to the lot's east edge at x 373, 23 m; one lane, 5 m wide (z 167.5 to 172.5).
4. **The gate and fence are always solid to the player.** Cars pass the lifted barrier; a player-only collider fills the gate opening at all times, like the rest of the fence. During a shift, touching it shows "You can't abandon your post."; outside a shift it is a plain fence, no message.
5. Parking lot 30 x 40 m centred (358, 170), x 343 to 373, z 150 to 190, gravelled. Trailhead T at its west side (340, 170).
6. The front-zone resident's car stands in the lot's north-east corner at (370, 179.4), where the drive starts, clear of the drive and the turning circle. Where he sleeps (car, office back room, a trailer) is for the story session.
7. Office (350, 200), 12 x 8 m, doorway on the lot side at x 348. Store (366, 200), 8 x 5.6 m, doorway at x 366; small, unstaffed, a lit cooler sign. 16 m apart.
8. Mast at the office's north-east corner, just outside the walls at (357, 205): lattice, red lamp at 30 m.
9. Sound: mains hum here and nowhere else.

### 3.2 Gate booth and closed campground (option B, DECISIONS 2026-09-29)
Order along the drive from the gate, heading west: gate (x 396), booth window (x 392), spur turn-off (x 385), turning circle (x 376 to 392, south side), lot (x 373).
1. **Booth** beside the lane, on the drive's north side at (392, 176), footprint 2 x 2 m (x 391 to 393, z 175 to 177): 2.5 m from the lane edge, 4.5 m east of the spur. Window on the lane side, doorway on its west wall toward the spur. Cars stop in the lane at its window; nothing drives through it.
2. **Gravel spur** for admitted cars, joined to the drive: it leaves the drive's north edge at (385, 172.5), runs north to (385, 186), bends east to x 390 by (390, 196), then north along the inside of the fence to the chain at (390, 238), and on to the loop at z 242. About 72 m long, 4 m wide, single lane. Closest to the store (366, 200): 24 m.
3. **Turning circle** for refused cars: gravel, 16 m across, centred (384, 160), joined to the drive's south edge. Clearances: 3 m to the lot, 4 m to the fence, about 15 m to the resident's car. Refused cars loop it and leave through the gate.
4. **Chain** across the spur at (390, 238), lowered for admitted cars.
5. **Closed campground loop** behind the chain: a one-way gravel loop road 5 m wide, 40 m across on its outer edge, centred (372, 262), x 352 to 392, z 242 to 282, eight empty pitches. It sits north of the old burn, so the tower sees at most tree gaps; it has no line in the tower check and is never stamped.
6. **Shifts.** A shift starts when the player enters the booth. It ends when that day's cars are done, or when the player leaves the booth between cars. If the player leaves while a car waits, that car waits at the barrier until the player returns or the day ends. Re-entering the booth the same day resumes that day's remaining cars; there is no fresh count. A shift never ends the day and never traps the player. Cars per day is a tuning value set by playtest (Grant).
7. **Shift wall**, only while in a shift: invisible walls across the spur mouth at (385, 176) and at the turning circle's edge stop the player following a car: "You can't abandon your post." The gate collider (3.1.4) shows the same message during a shift.
8. **Outside shifts** the player may walk up the spur and around the chain into the loop. That is where the admitted cars are found, parked and empty.
9. Cars keep arriving until the office resident's storyline ends (completed, or he dies). They do not thin out as WARD falls. How many days the minigame runs is set by playtesting, never the whole game.
10. Talking to the office resident counts as Social and starts his minigame. Working the booth does not count as Social.
11. The minigame's rules and documents: not designed yet (story session and Milestone 13).

### 3.3 Cultist cave interior (walked step by step, as built)
The cave is a dead end with one way in and one way out. All interior pieces are meshes below the terrain; the mouth is a terrain hole with a rock block filling it above the passage ceiling.
1. **Entrance.** The W1 to cave trail ends on the ravine floor at (52, 37.5) in front of the mouth face (z 37.4). A level passage 3 m wide and 4 m tall runs south from the face to z 20.5, floor -6, ceiling -2, grey rock, lit only from outside. The chant from the trail spots is clear here. On day 1 the mouth is boarded with an ordinary CLOSED, UNSAFE board; from day 2 the boards are down.
2. **Descent.** Three ramp legs side by side in plan, 5 m apart (3 m passage, 2 m rock between), so nothing is stacked. Each leg slopes over 13 m at about 17 degrees between level 3 m turns centred on x 52 and x 68 (a ramp, no steps, so the STAIRS RULE is not needed). The east turn stays 1.5 m clear of the chamber (x 71). Drop from -6 m to -18 m. Passage 3.5 m tall.
   - Leg 1, east along z 22, floor -6 to -10. A bass note starts under the chant.
   - Leg 2, west along z 17, floor -10 to -14. Coloured light leaks up the rock; the chant thins, the bass leads.
   - Leg 3, east along z 12, floor -14 to -18. Chant gone, bass only, the rave light growing.
3. **Rave chamber** east of the ramp, reached by a short level passage: centred (80, 12), 18 m across (x 71 to 89, z 3 to 21), floor -18 m, ceiling -10 m. It lies beside the ramp, never under it. Surface above it is forest floor at 0 m, so 10 m of rock over the ceiling. Rave lights, music, the resident. Talk (Social) and the cave's own events. The one warm colour burst on a grey map.
4. **Way out.** Back up the same descent; the bass turns back into chant as the player climbs, and the entrance passage returns to grey and daylight. Mouth to chamber 73 m of floor walked, 29 s each way.
5. **Section drawing:** the map sheet carries a side view of the cave unfolded along the path, with every floor and ceiling height (Main3_map.svg, bottom).
6. Nothing inside is seen from the tower.
7. **Chases never enter the cave spur.** The W1 to cave spur and the cave are off limits to chase events; a chase that reaches W1 ends there or turns back along the shore path.
8. Rock cover over the entrance passage is only 2 m (ceiling -2 under surface 0), over leg 1 from 2.5 m to 6.5 m. Keep the terrain from dipping into these ceilings in any later change.

### 3.4 Keeper's camp layout (as built)
1. Cabin centred (178, 168) in the clearing's north-east quarter, the only quarter no trail leaves through. The player spawns inside it. Stove inside; report box on the cabin desk (a prop: the report is filed from the carried logbook, DailyLoop.md 1).
2. Fire pit (172, 163). Generator (181.5, 171.5) behind the cabin.
3. Tower base (164, 166), 8.5 m from the camp centre on the Camp to J line. Timber legs (section 3, note 3). Deck 8 x 8 m at 56 m; cab 4.4 x 4.4 m with a 1.8 m walkway; lectern inside the cab facing north. Stairs are a south switchback of 12 flights arriving at the deck's south-west corner.
4. Trails stop at the clearing edge; there are no built trails inside the 15 m clearing.

### 3.5 Other places as built
1. **Lake:** boathouse door in the east wall at z 52.4 facing the shore, with a 3.8 m gangway to the ground; the resident's spot is inside by the west wall. A wade limit (invisible, at 0.97 of the lake radii, tops -5.0 under the dock deck) lets the player wade the edge and never walk into deep water.
2. **Camp 1:** the spar at the camp centre with the bulb string running 14 m west to a stake; one tent, three workbenches, lumber, the cookfire (Food), the resident's spot.
3. **Camp 2:** trails end at the stack foot. The ladder on the west-south-west face is looks only for now: the game has no ladder climb, so the tent and the resident's spot on top cannot be reached on foot yet (open, 8).
4. **Camp 3:** tent, fire (Warmth) and a seat log in the hollow west of the creek. The green-glass lantern hangs on the Snag's hollow side, 3 m above the rim.
5. **Ward climb:** pale cairn 2.2 m beside the trail start at J, with a chain across the trail to a post. The climb passes the Tor 22 m from its centre at the closest.

## 4. Routes

Shapes differ on purpose: winding trails, a west loop, dead-end spurs, and a straight road the player sees but never walks.

The map curves are the trails' centre lines. Every built trail meanders about its curve to reach the table length within 5 percent (map curve lengths: Camp to pump 67, pump to boathouse 72, boathouse to Camp 2 80, Camp 2 to T 80, Camp to Jg 101, Jg to T 84, Jg to Camp 1 70, Camp to Camp 3 99, pump to W1 68, W1 to Camp 3 91, W1 to cave 86, Camp to J 81, Ward climb 114). Points of interest are at their built distances.

| Leg | Shape | Length m | Walk s | Points of interest (m along leg, as built) |
|---|---|---|---|---|
| Camp to pump | trail | 80 | 32 | 42: water tank on a stand |
| Pump to boathouse | shore path | 84 | 34 | 43: overturned rowboat |
| Boathouse to Camp 2 | trail | 95 | 38 | 41: phone pole with handset box (Early: bell post) |
| Camp 2 to T | trail | 94 | 38 | 45: ring of rusted food lockers |
| Camp to Jg | winding, along the south edge of the old burn | 125 | 50 | 39: Hollow Giant at (202, 140); 96: forage patch A, berries in the burn |
| Jg to T | winding, through the burn regrowth | 105 | 42 | 34: Gate Tree, a burned-out giant stub cut to 15 m; 89: first sight of the lot |
| Jg to Camp 1 | dead-end spur | 83 | 33 | 42: latrine shed and wash stand |
| T to office / store / booth | straight walks across the gravelled lot and drive (not trails) | 36 / 41 / 53 | 15 / 16 / 21 | office and store 16 m apart; booth at the gate |
| Camp to Camp 3 | winding descent | 126 | 50 | 40: forage patch B; 107: log steps down the rim (STAIRS RULE ramp) |
| Pump to W1 | shore path | 80 | 32 | 39: washed-out truck; 74: stepping stones at the creek mouth |
| W1 to Camp 3 | trail, entering the hollow along the creek cut | 114 | 46 | 27: footbridge (over dry ground; the creek runs 8 m north); 71: wrecked camper trailer |
| W1 to cave | dead-end spur, winding down | 109 | 44 | 35: rope handrail, chant spot 1; 68: a string of coloured bulbs on a dead branch (not on day 1); 109: ravine floor at (52, 37.5), boarded mouth on day 1 (3.3) |
| Camp to J | trail | 96 | 38 | 53: burn-map board; 95: plank bridge (creek water) |
| J to Ward | switchbacks, night only | 130 | 52 | 0: pale cairn and chain; 57: rune post; 90: Tor base; 110: last bend, silence; 130: stones |

1. Two points of interest are giant trees (Hollow Giant, Gate Tree), plus the Snag landmark. All three are scene objects with mesh colliders (Rook).
2. **The cave spur is unsigned at W1**, the one exception to the junction pass check (DECISIONS 2026-09-29). Chant spot 2 is W1 itself, from night 1 on; on day 1 the cave is not heard.
3. Every leg walked eastward has one bend turning the player west. From day 2 these are glimpses of the glow (DECISIONS 2026-09-29); on day 1 they show only the far ridge. The fire is loudest at the three west glimpses: W1, the Camp 3 rim, the Camp to J bend (Hollis).
4. Creek: spring (104, 215.2), past J, through the Camp 3 hollow, to the lake shore beside W1 (2.1). Crossings: Camp to J (plank bridge on the creek) and pump to W1 (stepping stones at the mouth). The W1 to Camp 3 trail runs on the creek's south-west bank; its footbridge spans dry ground.
5. Loops: east ring (Camp, pump, boathouse, Camp 2, T, Jg, Camp) 583 m, 233 s; west loop (Camp, pump, W1, Camp 3, Camp) 400 m, 160 s. Camp to the office about 266 m. Camp to the Ward by J 226 m, 90 s.
6. Chase events (Events.md) use these legs: the longest unlit stretches are Camp to Jg, Jg to T and W1 to Camp 3. Chases never route into the cave spur (3.3.7). Safe points for a chase: the keeper's camp, any lit camp, the lot.

### 4.1 Junctions
Every trail leaves its clearing aimed at the next landmark for its first 30 m. Day markers: spar with bulbs (Camp 1), granite stack (Camp 2), the Snag (Camp 3), red mast lamp (office), water glint (lake), pale cairn over the cut (J). The tower cab as a marker from junctions is unverified; Rook tests it in the blockout (from the Camp 2 stack foot it is hidden by the Hollow Giant's crown, 5.10).

## 5. Tower sightlines

1. Tower base (164, 166) on the 8 m knoll, deck 56 m, eye 57.6 m (58.2 m at jump height). Every giant is at least 7.6 m below the eye.
2. Cone rule: from the eye to both edges of each landmark, no crown or trunk edge within 3 m of the line.
3. Landmark checks at each clearing's near edge, from the deck centre (paper figures; Camp 2 as built in 5.10):

| Place | Bearing | Dist m | Target (abs) | Margin m |
|---|---|---|---|---|
| Lake | SSE | 137 | far water at -5.5, over bank crowns at about 16 (ground -2 plus 18); boathouse roof | 5.9 |
| Camp 1 | ENE | 138 | spar top 29 | 9.2 |
| Camp 2 | ESE | 141 | granite stack top 24, boulder field r 20 | 3.8 on paper; 0.5 as built (5.10) |
| Camp 3 | W | 88 | Snag top 54 | 4.0 over every giant cap |
| Office and lot | E, 75 to 95 | 189 to 212 | lot surface from its west edge, cars, office, store, mast | 2.6 over the 4 m regrowth at the lot edge; buildings and mast far more |

4. **Ward hidden:** the Tor (a rock outcrop; "tor" is the plain word for a bare rock hill), on the crest at (76, 223), radius 18 m, base 34 m, top 58 m. **As built (W-1 over 64 deck eye points):** least margin 4.2 m at eye height (5.0 m from the deck centre), 0.9 m with eyes and targets raised 3 m. Every ray still hits.
5. **Cave hidden:** the ravine rim north of the mouth (mouth floor -6, top -2), raised to 18 m near (74, 60) (2.1). **As built (C-1 over 64 deck eye points):** least margin 9.2 m (9.3 m from the deck centre), 6.0 m with eyes and targets raised 3 m.
6. The old burn lies east of the tower (bearings 75 to 95) and crosses neither hiding line.
7. **Checks W-1 and C-1** (rerun after any change): trees off. Eye points on a 1 m grid over the whole deck and walkway, at eye height and jump height. Rays to every stone corner (W-1) and to the cave mouth's corners (C-1). Every ray must hit the Tor, rim or terrain. Repeat with eyes and targets raised 3 m: every ray must still hit.
8. Haze: landmarks at 137 to 212 m. First try a distance fog end past 300 m for Main3 and emissive lamps. Height fog is new shader work; only if the blockout shows landmarks lost (Rook).
9. **The fire and the tower:**
   - Day 1: the fire is not visible at all (DECISIONS 2026-09-29). The west view is a clear far ridge over a band of cliff-edge giants at 46 m or lower. No glow, no smoke, no haze colour from the fire.
   - Night 1: the first sight of the roaring wildfire is at the Ward ledge, with the Ward holding it off.
   - Day 2 on (DECISIONS 2026-09-29): far glow and smoke over the ridge, seen from the tower and the west glimpses, huge in the distance (a smoke column filling a wide slice of the western sky, glow spanning the ridge), growing as WARD drops. The cliff-edge band still hides the valley and the fire's base; only the Ward shows the line it cannot cross.
   - Build note: the fire needs a day 1 off state for the whole scene (glow, smoke, ash, sound, sky tint). Rook.
10. **Found in the build (open for Rook's fix batch):** the Hollow Giant (202, 140), top 50, stands 1.8 m outside the Camp 2 cone and its crown reaches into it. The stack top is seen from 37 of 64 eye points with 0.5 m margin from the deck centre, and the boulder field is not seen, against the cone rule's 3 m. The same crown hides the tower cab from the Camp 2 stack foot. Geometry is not changed here.

## 6. Break the uniformity

1. Sizes: 60 m (Camp 1), 40 m boulder field (Camp 2), 36 m (camp), 25 m sunken (Camp 3), a 110 m lake, a lot.
2. Distances from camp: 80 m (pump) to about 266 m (office).
3. Heights: -18 m (cave chamber), -6 m (ravine), -4 m (hollow), 8 m (knoll), 24 m (stack top), 36 m (Ward).
4. Light: warm (camp), pale harsh (Camp 1), cold white (Camp 2, Vesper's call, so the keeper's camp, Camp 1 and the office are not all warm), green glass (Camp 3), red lamp and sodium (office), rave colour spilling from grey rock (cave).
5. Actions: each camp gives a different need (Food, Water, Warmth). The office has the store. The lake has the sure water.

## 7. Reviews needed

- Vesper: section 3 feel and light, the two poles, event states, the day 1 fire-off look.
- Pim: junction markers 4.1, lectern, report box, the bunk-counts-as-Give-nothing rule (DailyLoop.md 2.5).
- Hollis: section 3 sound column, the road over the run, the cave chant and bass, the fire's day 1 silence.
- Quill: placeholders only; Camp 1 as one resident with a lot of kit.
- Marlow: the walk of the gray blockout against this revision.
- Rook: the fix batch after Marlow's walk, including 5.10 and the Camp 2 ladder.

## 8. Open questions

Grant's loop questions are in DailyLoop.md 9. The map raises no new question for Grant. Open for the team, from the build:
1. Camp 2 has no way up the stack on foot (no ladder climb in the game). Needs a ramp path, a climb mechanic, or the resident and tent moved to the stack foot. Sable's lean: a narrow switchback path cut into the stack's east face, so the tent stays on top.
2. The Hollow Giant crowds the Camp 2 cone (5.10).

Residents (DECISIONS 2026-09-29): one at the lake, one at each of the three campsites, one in the cultist cave, and one around the parking lot where the drive starts. No names or story in this file.

Sable
