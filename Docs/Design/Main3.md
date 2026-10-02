# Main3 paper brief

**DRAFT, revision 16, 2026-09-29, Sable. Revision 16 folds in Rook's 8.9e build (the Ward climb, pass, plateau and W-1 as built; groves; night fog; the five leftovers fixed). Revision 15 moved the Ward onto a high plateau at about 70 m behind a rock wall with its crest at about 88 m (DECISIONS 2026-09-29), replaces the Tor, adds distance layers (DECISIONS 2026-09-29; Vesper's layer spec, 2.11), and lists the small leftovers for Rook's next batch (8). Revision 14 folded in Rook's 8.9c build facts. Revision 13 changed the knoll and tower stairs, the cabin, the Ward approach and ledge, and added the stand-in fire (Grant's gray-box walk). Revision 7 approved pending resident placement (DECISIONS 2026-09-29). Revision 12 folds in Rook's gray blockout resolutions (Docs/Design/Main3_BuildNotes.md) so this file matches what was built; three items stay only in the build notes (trail ends pinned, shift walls off, the sightline report line). Rook's fix batch after Marlow's walk may change more. Nothing else here is decided.** The loop lives in Docs/Design/DailyLoop.md and events in Docs/Design/Events.md; neither is repeated here. Map: Docs/Design/Main3_map.svg.

Coordinates in metres, origin south-west, x east, z north. Heights absolute (metres above map base 0). Walk times at 2.5 m/s are information for pacing and events; there is no day time budget (DECISIONS 2026-09-29). Where the map drawing and this file differ, the map wins for positions and shapes and table 2.1 wins for heights (Wren's build rule).

## 1. Purpose

1. A map where near and far feel different, and no two places look, sound or work the same.
2. On day 1 it is a lookout's patch of forest and nothing more. The tower sees every place it must check; the Ward and the cave, the two that take from you, are the two it cannot see.

## 2. Shape

1. **Size: 400 x 300 m.** Fire to the west, forest in the middle, the front zone inside the gate on the east edge.
2. West edge: cliff along x 10, valley floor -40 m below, burning ridge 300 to 500 m beyond.
3. North-west: **the Wall**, a bare rock escarpment with its crest at about 88 m, and behind it **the Ward plateau** at about 70 m, above the tower deck (56 m) and hidden from it (DECISIONS 2026-09-29). The Ward stones stand at the plateau's cliff edge facing the fire (3.6).
4. South-west: a ravine with the cultist cave below a rock rim.
5. Centre: the keeper's camp on a 15 m knoll with the tower; the lake south; Camp 3 in a hollow west.
6. East: Camp 1 and Camp 2 in the forest, then the front zone.
7. **The fence and gate are the map edge.** Chain-link along x 396, the full height of the map. The gate at (396, 170). The road comes from off-map to the gate and never enters. Inside the gate: a gravel drive, the parking lot, the office and the store.
8. Giant trees: emergents 40 to 50 m tall, trunks 6 to 10 m. Tops never above 50 m absolute (46 west of x 32); on the knoll (ground up to 15) at most 35 m tall, re-capped to the absolute 50; none on the Wall or the plateau (their ground leaves no room for a giant under the caps; the plateau carries low wind-bent trees only, under 8 m). One stated exemption: the Snag at Camp 3, top 54 m, 3.6 m under the tower eye. **As built (8.9e):** 43 giants in 7 grove clusters plus the cliff-edge band (2.11), by rule, none in clearings, trails, the lake, ravine, hollow, old burn or front zone, and none within 3 m plus crown of the five tower cones (see 5.10 for one exception found).
9. Routes stay on trails (DailyLoop.md 1.2).
10. **The old burn** (DECISIONS 2026-09-29: the tower sees more of the office and lot). A years-old burn scar runs from the foot of the knoll east to the front zone: 30 m wide at the knoll (x 185, z 151 to 181), widening to 70 m at the front zone (x 340, z 143 to 213). No giants and no mid canopy in it: dense young regrowth 4 to 6 m tall, 4 m in the last 40 m before the front zone. The thicket is impassable off the trails, so it opens the view, not a shortcut. An old burn under a lookout is ordinary on day 1.
11. **Distance layers** (DECISIONS 2026-09-29: make the map feel bigger with layers first; scaling the whole map is held back). Vesper's layer spec, all proposals:
    - **Off-map hills,** no colliders, rolling irregular bands, never cones:

| Side | Near layer | Far range |
|---|---|---|
| North | ridge 120 to 250 m out, crest 45 to 60 m, forested | 1.2 to 1.8 km out, crest 150 to 220 m |
| South, beyond the lake | low ridge 150 to 300 m out, crest 35 to 50 m | 1.5 to 2 km out, crest 180 to 250 m |
| East, beyond the fence | rolling forest 20 to 35 m for 100 to 400 m, the road in a gap | 1.5 to 2.5 km out, crest 150 to 200 m |

      The near layers crest below the Ward plateau (70). An edge forest strip 60 to 100 m deep runs outside the north and south map edges.
    - **Fog that starts near** (linear, LookTuning): day one #A8A08E from 40 to 600 m; day two on #9E5C38 from 25 to 420 m; night #05080D from 8 to 60 m. The far emissive markers (cab lamp, lot lights, mast lamp) are not fogged at night. Each backdrop layer also carries its own fixed blend to haze in its material (near ridge 55 percent, far range 80 percent, the sky horizon lightest), so each layer reads lighter than the one in front on the day looks. Day fog now reaches past the checked landmarks (137 to 212 m), which settles 5.8's fog end.
    - **Giant groves with gaps:** the giants regrouped into 6 to 8 groves of 4 to 8 trees, 12 to 20 m apart inside a grove, with 60 to 100 m gaps between groves holding only mid canopy. One grove sits in each tower distance band (50 to 80, 120 to 160, 200 m and out), framing the view lanes. The caps, the cone clearances and the cliff-edge band stay; rerun the sightlines after regrouping.
    - **Curving trails:** trail centre lines bend so no trail shows more than about 60 m straight ahead (the road is the one straight line), and the next landmark comes and goes through the gaps between groves. **As built (8.9e): not re-curved;** trails keep their centre lines and only meander about them. That matters only where a leg's centre line runs straight for more than about 60 m, since a meander within 5 percent of length does not break a long straight view. Marlow measures the longest straight view along each leg; any leg over 60 m gets a re-curve, and its length in table 4 changes with it.
    - **As built (8.9e):** the giants stand in 7 grove clusters plus the cliff-edge band, 43 in all; night fog runs 8 to 60 m.

### 2.1 Ground heights (absolute, metres, as built)

Base ground is 0 in the south-west, rolling up to 5 in the north-east. Between named points the blockout interpolates smoothly.

| Point | Position | Ground m | Notes |
|---|---|---|---|
| Keeper's camp, knoll top | (170, 160) | 15 | raised from 8 in revision 13; flat 15 across the 36 m clearing, tower base (164, 166) at 15; a linear flank reaches the surrounding ground by 63 m from the centre (45 m blend, under 25 percent). The knoll does not raise the ground round the lake; the pump still sees the Snag |
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
| The Snag | east rim (96, 146.5) | 4 | 50 m dead giant, top 54; moved 6 m east so the log-steps trail clears it and the cab shows from the Camp 3 centre |
| Creek line | (104, 215.2), J, (84, 150), (78, 146), (84, 128), (100, 110), mouth (136.5, 66) | 10 falling to -5.5 | rerouted to pass 11 m clear of the Snag; reaches the shore beside W1 |
| J, Ward junction | (104, 206) | 10 | at the foot of the Wall's east face |
| **The Wall**, crest | from (55, 190) to (115, 270) | **88** | bare rock escarpment, faced east toward the tower; falls to the ground (10 to 20) on the tower side, to the plateau (70) on the west side. Replaces the spur and the Tor |
| The pass | (112, 262), a notch at the Wall's north-east end | **78.3** as built | 10 m below the crest, well off the tower-to-stones line (bearing 332 against 307) |
| **Ward plateau** | north-west of the Wall, to the cliff at x 10 and the map edge at z 300 | **70 to 72** as built | falls gently from 72 under the Wall to 70 at the cliff side |
| Ward low crest | x 25, z 250 to 266 | 72 | 2 m above the plateau, 15 m from the edge; hides the drop until topped |
| Ward rock lip | (13, 256) | 70, lip 0.8 m high | 3 m from the cliff edge at x 10; the path ends here facing west; walkable ground stops at the lip line |
| Ledge past the lip | x 10 to 13 | falls to 67.5 at the cliff edge | so the near valley shows 30 degrees down from the lip |
| Ward stones | (16, 266), (21, 263), (18, 270) | 70, tops 82 | the as-built stone layout moved 4 m north onto the plateau |
| Cliff below the plateau | x 10 | 70 down to the valley floor at -40 | 110 m drop |
| Ravine floor | north of the mouth face (z 37.4 and up) | -6 | the strip south of z 34 stays at 0 |
| Cave mouth | face at z 37.4, x 50 to 53.9 | floor -6 | 4 m tall, top -2; terrain hole x 50.0 to 53.9, z 33.4 to 37.5, a rock block above the passage ceiling |
| Ravine rim, north side | along the rim line | 14 | 18 for 20 m west and 15 m east of (74, 60) (the rim line ends 23 m east of the vertex and tapers to the cave trail entry over its last 8 m) |
| Surface over the cave interior | x 44 to 89, z 3 to 37.4 | 0 | |
| Hollow Giant | (202, 140) | 6 | top 50 |
| Forage patch A / B | on Camp to Jg at 96 m / Camp to Camp 3 at 40 m | 5 / 6 | |
| Jg | (262, 172) | 5 | in the old burn |
| Old burn | knoll flank to front zone | about 12 at x 185 falling to 3 | |
| Gate Tree | (290, 176) | 4 | stub 15 m |
| Camp 1 | (282, 238) | 5 | |
| Camp 2 boulder field / stack top | (292, 108) | 4 / 24 | |
| Front zone (lot, office, store, T, drive, booth, turning circle, spur, loop) | x 340 to 396 | 3 | flat |

## 3. Places

Six locations with one resident each, plus the keeper's camp and the Ward. Everything reads ordinary on day 1 (DailyLoop.md 6). Sounds heard within 35 m (camp 40 m) unless noted; Hollis owns the sound column.

| Place | Position, ground | Size and feel | Actions (need) | Seen from tower as | Sound |
|---|---|---|---|---|---|
| Keeper's camp | (170, 160), knoll 15 m | 36 m clearing, warm light; layout 3.4 | tower and lectern; stove (Warmth) | the tower itself | wind vane squeal |
| Lake | water (190, 60), 110 x 55 m, surface -5.5, bed -8, shore -4.5 (2.1); pump on the north dock (190, 94.8); resident's boathouse on stilts (240, 52), 6.4 x 5.6 m | the one wide open sky | pump (Water); talk; events | open water, boathouse tin roof | lapping, dock chain |
| Camp 1 | (282, 238), ground 5 m | big by kit, one tent: a sprawling workshop camp, 60 m, pale harsh light | cookfire meal (Food, some days); talk; events | a lashed timber spar 24 m with a string of bulbs | pots |
| Camp 2 | (292, 108), ground 4 m | tiny: one tent on top of a 20 m granite stack (12 m across at the base, 9.6 m at the top) in a 40 m boulder field, ladder up, cold white tent lamp (Vesper) | rain barrel (Water, some days); talk; events | the granite stack | wind |
| Camp 3 | (78, 146), hollow floor -4 m | sunk 8 m below its rim, 25 m across, dense, green-glass lantern | sit at the fire (Warmth, some days); talk; events | the Snag: a bleached dead giant on the east rim (ground 4, 50 m tall), top 54 m | creek through the hollow, a low fire |
| Office | (350, 200), front zone, ground 3 m, 12 x 8 m | prefab ranger office on the north side of the lot; store 16 m east at (366, 200), 8 x 5.6 m. This location's resident lives around the lot where the drive starts: a car in the lot's north-east corner at (370, 179.4). Gate booth and closed campground: 3.2 | talk (Social, starts his gate minigame); events; store (Food) | the whole parking lot, office, store and mast (30 m, red lamp), in a wide view across the old burn | mains hum; radio with ordinary chatter on day 1, going to static after the reveal |
| Cultist cave | mouth face (52, 37.4), ravine floor -6 m, mouth 4 m tall | weird on purpose (DECISIONS 2026-09-29): rave lights and music inside, busting the grim cult cave trope; the rock outside stays grey, so the light spilling up the descent is the surprise. Interior: 3.3 | talk (Social); its own events; story session | hidden (5.5), never checked | chant from night 1, heard at two trail spots; inside it slowly turns into bass on the way down (DECISIONS 2026-09-29) |
| Ward plateau | rock lip (13, 256), ground 70 m, behind the Wall (crest 88); approach 3.6 | stones at the cliff edge facing the fire, to the player's right from the lip: (16, 266), (21, 263), (18, 270), each 3.6 x 4 m, tops 82 | night only: the Ward screen | hidden behind the Wall (5.4) | silence from the rim |

1. Event states change a landmark, never remove it: bulbs out on the Camp 1 spar, the Camp 2 tent lamp dark, the Snag's lantern missing, the office lot lights out. The mast lamp burning steady instead of blinking is reserved for one storyline warning and is never used as an event state. Details are Vesper's and Pim's.
2. Poles: only two, and they differ: Camp 1's lashed timber spar with bulbs, the office's steel lattice mast with a red lamp. Camp 2 and Camp 3 use rock and a dead tree.
3. **Tower legs are timber** (Vesper's proposal, built): square posts, beams every 8 m, X braces. The office mast is the only steel lattice on the map.
4. Resident hooks and roles: placeholders for the story session. Nothing that leans toward the ending goes in this file.
5. The Wall's rock face, the granite stack and the cave mouth are meshes, not terrain (Rook). The Tor (revisions 7 to 14) is removed: the Wall does its job (5.4).

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
Order along the drive from the gate, heading west: gate (x 396), barrier arm (x 389.3), booth window (x 391 to 392.8, south side), spur turn-off (x 385), lot (x 373). As built in 8.22 to FrontLayout.md draft 2 (Wren's call 2026-10-02, open for Grant); items 1, 3 and 7 below follow it.
1. **Booth** on the driver's side, south of the drive: x 390.6 to 393.2, z 164.2 to 166.6, window on its north face over the lane, 1.0 m doorway on its south face. A lift barrier at x 389.3 does the check; cars stop at the arm with the driver's door 1.2 m off the window (FrontLayout.md 2.1 and 2.2).
2. **Gravel spur** for admitted cars, joined to the drive: it leaves the drive's north edge at (385, 172.5), runs north to (385, 186), bends east to x 390 by (390, 196), then north along the inside of the fence to the chain at (390, 238), and on to the loop at z 242. About 72 m long, 4 m wide, single lane. Closest to the store (366, 200): 24 m.
3. **No turning circle** (removed in 8.22): refused cars back 14 m out through the gate and turn on the apron outside it, x 400 to 412, z 162 to 178 (FrontLayout.md 2.2).
4. **Chain** across the spur at (390, 238), lowered for admitted cars.
5. **Closed campground loop** behind the chain: a one-way gravel loop road 5 m wide, 40 m across on its outer edge, centred (372, 262), x 352 to 392, z 242 to 282, eight empty pitches. It sits north of the old burn, so the tower sees at most tree gaps; it has no line in the tower check and is never stamped.
6. **Shifts.** A shift starts when the player enters the booth. It ends when that day's cars are done, or when the player leaves the booth between cars. If the player leaves while a car waits, that car waits at the barrier until the player returns or the day ends. Re-entering the booth the same day resumes that day's remaining cars; there is no fresh count. A shift never ends the day and never traps the player. Cars per day is a tuning value set by playtest (Grant).
7. **Shift wall (IW3)** across the spur at the brush gap, z 210, x 385.8 to 396.2: on whenever an admitted car is on the spur, from the barrier lifting until the car is parked, whatever the shift does (Wren's call 2026-10-02; SpurWall). Touching it or the gate collider (3.1.4) during a shift shows "You can't abandon your post."
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
1. **Cabin** centred (178, 168) in the clearing's north-east quarter, the only quarter no trail leaves through. Inside 6 m east-west by 4.5 m north-south, ceiling 2.7 m.
   - Door in the south wall, centred. The player spawns inside, in the bunk.
   - Bunk along the north wall, west half.
   - Stove in the north-east corner, 1 m clear on every open side.
   - Desk under the west window, the player facing west toward the tower when seated. The report box sits on it (a prop: the report is filed from the carried logbook, DailyLoop.md 1).
   - A 1.5 m clear aisle from the door north to the bunk, stove and desk.
2. Fire pit (172, 163). Generator (181.5, 171.5) behind the cabin.
3. **Tower** base (164, 166) on the knoll at 15 m, 8.5 m from the camp centre on the Camp to J line. Deck stays at 56 m absolute, so every sightline check in section 5 holds; the legs rise 41 m. Timber legs (section 3, note 3), set 7.5 m square to fit the spiral inside. Deck 8 x 8 m; cab 4.4 x 4.4 m with a 1.8 m walkway; lectern inside the cab facing north.
4. **Stairs: a square spiral inside the tower legs** (Vesper: the legs stay a clean silhouette; Sable: each flight faces a new bearing, so the climb turns the player through the whole horizon). Ten flights of about 4.1 m rise each (16 to 17 steps of 0.25 m rise, 0.3 m run, so 4.9 m of run per flight), one 1 m corner landing between flights, two and a half turns, arriving through a hatch in the deck. StairRamp colliders per the STAIRS RULE; the steps themselves have no colliders (a step edge caught the capsule where a ramp met a landing); flight rails stop short of the landings. Invisible stops on both lane edges of every flight and the landings' outer edges keep the player on the stair (207 sprint-jumps in test, none left it).
5. **Climb time as built:** 56.0 m walked, **22.4 s up** at 2.5 m/s, inside the 20 to 25 s target. The walk clock counts grounded horizontal distance (DailyLoop.md), so the tower check's fixed cost is unchanged by design.
6. Trails stop at the clearing edge; there are no built trails inside the 15 m clearing.
7. **Walkable grades with the higher knoll (as built):** every trail holds 25 percent (14 degrees) or less over any straight 10 m. Camp trail ends meet the clearing at 14.6 to 14.9 m and walk back in. **Camp to pump** could not drop 15 to -4.5 in 80 m, so it is rebuilt with **two switchbacks**: 109.7 m from the camp centre, 95.7 m walked from the clearing edge to the pump, about 38 s each way, 24.8 percent at its steepest. Of the rest, Camp to Jg is the steepest at 23.2 percent. The only steeper walked slopes on the map are the log steps, the cave ramp legs (17 degrees, 30 percent) and stairs with their ramps.
8. **Water still fits the day.** The pump is the sure Water option; the longer leg makes the round trip about 76 s of walking plus the fill. There is no daily time budget (DECISIONS 2026-09-29), so a longer leg costs only the walk; Water stays met on any day the player goes.

### 3.5 Other places as built
1. **Lake:** boathouse door in the east wall at z 52.4 facing the shore, with a 3.8 m gangway to the ground; the resident's spot is inside by the west wall. A wade limit (invisible, at 0.97 of the lake radii, tops -5.0 under the dock deck) lets the player wade the edge and never walk into deep water.
2. **Camp 1:** the spar at the camp centre with the bulb string running 14 m west to a stake; one tent, three workbenches, lumber, the cookfire (Food), the resident's spot.
3. **Camp 2:** a switchback path climbs the stack's east face to the top (four 26.6 degree ramps on two lanes, z 108 to 118, rails, a top landing); the ladder on the west-south-west face is looks only. Both Camp 2 legs end at the ramp foot (298.9, 107.8) (fixed in 8.9e).
4. **Camp 3:** tent, fire (Warmth) and a seat log in the hollow west of the creek. The green-glass lantern hangs on the Snag's hollow side, 3 m above the rim.
5. **Ward climb:** a cairn gate 4 m up the J to Ward trail, the pale cairn 2.2 m on its west side, a chain across the trail to a post. By day it is solid (an invisible block 3 m tall across the whole trail corridor under the chain), so the climb is closed until night. Route and ledge: 3.6.

### 3.6 The Ward on the high plateau (revision 15, DECISIONS 2026-09-29)
The Ward now sits above the tower: plateau 70 m against the deck's 56, stone tops at 82. The Wall hides it. The climb turns back at the pass to look down on the tower, then crosses the rim to the stones facing the fire.

1. **Up the Wall's east face.** From J (104, 206) at 10, switchbacks climb the east face, centre line (104, 206), (118, 218), (106, 232), (122, 244), (110, 252), to **the pass** at (112, 262), 78.3 m as built. All on the tower side of the Wall's crest line, at 24 percent or less; the switchbacks turn on level 3 m platforms. The Wall fills the view west the whole way; the tower is behind the player.
2. **The pass: look back.** The trail turns at the notch. The player faces back east and sees the tower below: the deck 22 m lower, the camp, the lake, the old burn to the front zone. The first time the tower is under them. A short level stretch (about 10 m) holds the view.
3. **Over the rim.** Through the notch onto the plateau (78 down to 72 under the Wall's west side). Silence starts here (Hollis). The plateau runs west about 95 m along z 262 to 270, centre line (112, 262), (100, 270), (60, 268), (30, 260), to the lip, falling gently to 70. Low wind-bent trees only. From the rim the stones stand far ahead as black shapes against the glow of the fire on the sky; the drop and the valley stay hidden.
4. **The low crest** at x 25, 2 m above the plateau (72), 15 m from the edge, hides the drop until the player tops it. Topping it opens the cliff and the burning valley.
5. **Rock lip** at (13, 256), 3 m from the cliff edge at x 10, 0.8 m high: the path ends here facing west. Past the lip the ground falls to 67.5 at the edge so the near valley shows 30 degrees down.
6. **Stones** to the player's right from the lip, at the cliff edge facing the fire: (16, 266), (21, 263), (18, 270), each 3.6 x 4 m, tops 82 (the as-built layout moved 4 m north). The stone screen outcrop is no longer needed: the reveal is the fire over the crest with the stones already in view as silhouettes.
7. **Climb total as built (8.9e):** J to the lip **401 m, 160 s** (paper said about 380 m, 152 s). Camp to the Ward by J: 96 + 401 = **497 m, about 199 s**, one way (sleep fades at the ledge).
8. **Vesper's frame check from the lip** (eye 71.6 m, standing behind the lip, looking level west), against the stand-in as built (3.7.8, fires at 200 to 300 m):
   - Fire across at least 150 degrees: as built, it covers 150 degrees inside the far clip.
   - Drop and burning valley in the bottom third (about 10 to 30 degrees below level): the valley floor at -40 sits about 112 m below the eye, so that band is the valley from about 195 m to 630 m out. The stand-in's valley fires at 200 to 300 m sit in it.
   - Ridge crest (30) at 300 m sits about 8 degrees below level, flame tops (100 to 130) 6 to 11 degrees above. Smoke past the top of the frame. Rook rechecks with a Game view shot from the new lip.
9. The Tor, the stone screen and the old ledge at 36 are removed. The cairn gate (3.5.5) stays at J.

### 3.7 Stand-in burning ridge and fire (gray blockout, beyond the west map edge)
Marlow: the ledge shows no fire yet. A gray stand-in, sized to Style.md 6.3, so the frame can be checked before the art pass. Gray shapes with plain orange emissive for flame and glow; no final look.
1. **Ridge:** a rolling band (not cones, Style.md), 300 to 500 m west of the cliff (x -290 to -490), from z -870 to z +1,370 (2.24 km), crest about 30 m absolute. It spans at least 150 degrees from the lip.
2. **Burning giants and flames on the ridge:** stand-in trunks 40 to 50 m with flame shapes to 70 to 100 m above the ridge (tops about 100 to 130 absolute), about 10 to 15 degrees above level from the lip.
3. **Valley floor** at -40 between the cliff and the ridge, burning in patches from 135 m to 440 m west of the lip (x about -125 to -430), so the bottom third of the frame burns.
4. **Smoke:** three to five columns rising 250 m and more, leaning east over the map, with a sheet roofing the west. Stand-in: tall gray cylinders or cards.
5. **Glow:** an orange emissive band along the ridge line and the valley fires, brightest thing in the frame.
6. **Switched by state:** hidden entirely on day 1 by day (DECISIONS 2026-09-29), shown from nightfall on day 1. From day 2 by day as far glow and smoke (5.9). The cliff-edge giant band keeps the base hidden from the tower.
7. Rook checks the frame from the lip with a Game view screenshot against 3.6.8.
8. **As built (8.9c):** the stand-in is built switched off. It was brought in to 200 to 300 m so it covers 150 degrees inside the camera's far clip: fires across the valley floor and the lower ridge face, smoke columns (460 to 560 m tall) and a smoke bank. Flames, glow band, valley fires and smoke use the project shader DieAlone/FireStandIn (unlit, ignores fog, colour from LookTuning fireGlowColor, fireGlowIntensity and smokeFireColor, no shadows); ridge and trunks stay gray and fogged. With the current night fog (end 200 m) the lip view shows little of it; the fog end past 300 m for Main3 (5.8) is look work still to do. The frame check shot is taken with fog off.

## 4. Routes

Shapes differ on purpose: winding trails, a west loop, dead-end spurs, and a straight road the player sees but never walks.

The map curves are the trails' centre lines. Every built trail meanders about its curve to reach the table length within 5 percent (map curve lengths: Camp to pump rebuilt with two switchbacks in revision 14, pump to boathouse 72, boathouse to Camp 2 80, Camp 2 to T 80, Camp to Jg 101, Jg to T 84, Jg to Camp 1 70, Camp to Camp 3 99, pump to W1 68, W1 to Camp 3 91, W1 to cave 86, Camp to J 81; the Ward climb is redrawn in revision 15, 3.6). Points of interest are at their built distances.

| Leg | Shape | Length m | Walk s | Points of interest (m along leg, as built) |
|---|---|---|---|---|
| Camp to pump | trail with two switchbacks down the knoll flank (24.8 percent max) | 110 from the camp centre (96 walked from the clearing edge) | 44 (38 from the edge) | water tank on a stand, about halfway |
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
| J to Ward | switchbacks up the Wall's east face (turning on 3 m platforms) to the pass, then west across the plateau, night only | 401 | 160 | 4: cairn gate and chain; a rune post on a switchback corner; the pass, look back on the tower; over the rim, silence, the stones far ahead against the glow; the low crest, the reveal; 401: rock lip, stones to the right (positions along the leg as built; Rook's build notes) |

1. Two points of interest are giant trees (Hollow Giant, Gate Tree), plus the Snag landmark. All three are scene objects with mesh colliders (Rook).
2. **The cave spur is unsigned at W1**, the one exception to the junction pass check (DECISIONS 2026-09-29). Chant spot 2 is W1 itself, from night 1 on; on day 1 the cave is not heard.
3. Every leg walked eastward has one bend turning the player west. From day 2 these are glimpses of the glow (DECISIONS 2026-09-29); on day 1 they show only the far ridge. The fire is loudest at the three west glimpses: W1, the Camp 3 rim, the Camp to J bend (Hollis).
4. Creek: spring (104, 215.2), past J, through the Camp 3 hollow, to the lake shore beside W1 (2.1). Crossings: Camp to J (plank bridge on the creek) and pump to W1 (stepping stones at the mouth). The W1 to Camp 3 trail runs on the creek's south-west bank; its footbridge spans dry ground.
5. Loops (camp centre, revision 14 pump leg): east ring (Camp, pump, boathouse, Camp 2, T, Jg, Camp) about 613 m, 245 s; west loop (Camp, pump, W1, Camp 3, Camp) about 430 m, 172 s. Camp to the office about 266 m. Camp to the Ward by J 497 m, about 199 s (revision 16, as built).
6. Chase events (Events.md) use these legs: the longest unlit stretches are Camp to Jg, Jg to T and W1 to Camp 3. Chases never route into the cave spur (3.3.7). Safe points for a chase: the keeper's camp, any lit camp, the lot.

### 4.1 Junctions
Every trail leaves its clearing aimed at the next landmark for its first 30 m. Day markers: spar with bulbs (Camp 1), granite stack (Camp 2), the Snag (Camp 3), red mast lamp (office), water glint (lake), pale cairn over the cut (J). The tower cab as a marker from junctions is unverified; Rook's sightline report: the cab shows from every junction (from the Camp 2 stack foot at 10 of 18 eye points).

## 5. Tower sightlines

1. Tower base (164, 166) on the 15 m knoll (revision 13), deck 56 m, eye 57.6 m (58.2 m at jump height), all unchanged. Every giant is at least 7.6 m below the eye. The raised knoll's crowns (15 + 35 at most) stay under every sightline: the lowest line near the tower (to the lake's far water) is at about 50 m where it passes the knoll edge.
2. Cone rule: from the eye to both edges of each landmark, no crown or trunk edge within 3 m of the line.
3. Landmark checks at each clearing's near edge, from the deck centre (paper figures; Camp 2 as built in 5.10):


| Place | Bearing | Dist m | Target (abs) | Margin m |
|---|---|---|---|---|
| Lake | SSE | 137 | far water at -5.5, over bank crowns at about 16 (ground -2 plus 18); boathouse roof | 5.9 |
| Camp 1 | ENE | 138 | spar top 29 | 9.2 |
| Camp 2 | ESE | 141 | granite stack top 24, boulder field r 20 | as built after the fix batch: stack top seen from 64 of 64 eye points, Hollow Giant crown 5.4 m clear (5.10) |
| Camp 3 | W | 88 | Snag top 54 | 4.0 over every giant cap |
| Office and lot | E, 75 to 95 | 189 to 212 | lot surface from its west edge, cars, office, store, mast | 2.6 over the 4 m regrowth at the lot edge; buildings and mast far more |

4. **Ward hidden: the Wall** (revision 15). The tower-to-stones line (bearing about 307) crosses the Wall's crest line (from (55, 190) to (115, 270), crest 88) about 55 percent of the way out, near (82, 228). There the line from the eye (57.6) to the stone tops (82) is at about 71 m: about 17 m under the crest on paper from the deck centre. **As built (8.9e, W-1 over the deck grid): 8.1 m with eyes and targets raised 3 m**, well over the 3 m rule. The plateau (70 to 72) lies below the crest, so no part of it shows. The pass notch (78.3) at the Wall's north-east end is at bearing about 332, 25 degrees off the stones' line; what shows through it is plateau ground with nothing on it. Rerun W-1 after any change. The Tor (revisions 7 to 14) is removed.
5. **Cave hidden:** the ravine rim north of the mouth (mouth floor -6, top -2), raised to 18 m near (74, 60) (2.1). **As built (C-1 over 64 deck eye points):** least margin 9.2 m (9.3 m from the deck centre), 6.0 m with eyes and targets raised 3 m.
6. The old burn lies east of the tower (bearings 75 to 95) and crosses neither hiding line.
7. **Checks W-1 and C-1** (rerun after any change): trees off. Eye points on a 1 m grid over the whole deck and walkway, at eye height and jump height. Rays to every stone corner (W-1) and to the cave mouth's corners (C-1). Every ray must hit the Wall, rim or terrain. Repeat with eyes and targets raised 3 m: every ray must still hit.
8. Haze: landmarks at 137 to 212 m. Fog per Vesper's layer spec (2.11): day fog ends at 600 m (day one) and 420 m (day two on), past every landmark; night fog as built 8 to 60 m, with emissive markers not fogged. Height fog is new shader work; only if the blockout shows landmarks lost (Rook).
9. **The fire and the tower:**
   - Day 1: the fire is not visible at all (DECISIONS 2026-09-29). The west view is a clear far ridge over a band of cliff-edge giants at 46 m or lower. No glow, no smoke, no haze colour from the fire.
   - Night 1: the first sight of the roaring wildfire is at the Ward ledge, with the Ward holding it off.
   - Day 2 on (DECISIONS 2026-09-29): far glow and smoke over the ridge, seen from the tower and the west glimpses, huge in the distance (a smoke column filling a wide slice of the western sky, glow spanning the ridge), growing as WARD drops. The cliff-edge band still hides the valley and the fire's base; only the Ward shows the line it cannot cross.
   - Build note: the fire needs a day 1 off state for the whole scene (glow, smoke, ash, sound, sky tint). Rook.
10. **Fixed in the build:** the Hollow Giant's crown (radius 5.5, centred 2.5 m south of its trunk) is now 5.4 m clear of the deck-centre lines to the Camp 2 stack (Docs/Layout/Main3/Main3_sightlines.md).
11. **The Wall and the fire view.** The Wall spans bearings about 282 to 335 from the tower. The tower's fire view (W to SW, about 225 to 282) stays open; the Wall is the dark cliff at its northern end, the one thing the tower cannot see past.

## 6. Break the uniformity

1. Sizes: 60 m (Camp 1), 40 m boulder field (Camp 2), 36 m (camp), 25 m sunken (Camp 3), a 110 m lake, a lot.
2. Distances from camp: about 110 m (pump) to about 266 m (office).
3. Heights: -18 m (cave chamber), -6 m (ravine), -4 m (hollow), 15 m (knoll), 24 m (stack top), 56 m (tower deck), 70 to 72 m (Ward plateau), 78.3 m (the pass), 88 m (the Wall's crest).
4. Light: warm (camp), pale harsh (Camp 1), cold white (Camp 2, Vesper's call, so the keeper's camp, Camp 1 and the office are not all warm), green glass (Camp 3), red lamp and sodium (office), rave colour spilling from grey rock (cave).
5. Actions: each camp gives a different need (Food, Water, Warmth). The office has the store. The lake has the sure water.

## 7. Reviews needed

- Vesper: the distance layers as folded (2.11), the Wall and the plateau look, the pass view back to the tower, the frame from the new lip.
- Pim: junction markers 4.1, lectern, report box, the bunk-counts-as-Give-nothing rule (DailyLoop.md 2.5).
- Hollis: silence from the rim, the wind on the pass, the far layers' sound.
- Quill: placeholders only.
- Marlow: walk the new Ward climb (length, grade, the pass look-back); measure the longest straight view along each leg (2.11 curving trails); recheck the leftovers.
- Rook: the lip frame shot from the new lip.

## 8. Open

Grant's loop questions are in DailyLoop.md 9. No new question for Grant.

**Leftovers, status after 8.9e:**
1. **Wall_100 ravine jump (Hurts):** not reported fixed in 8.9e; still open until Marlow's recheck. Design intent: the ravine east of the mouth and the ground over the cave are off the walkable map.
2. **Camp 2 legs:** fixed; both end at the ramp foot (298.9, 107.8).
3. **W1 to Camp 3 grade:** fixed with log steps at the hollow wall (exempt, as on Camp to Camp 3).
4. **Boathouse bank pockets:** fixed; walled.
5. **Tower deck east walkway:** fixed; the walkway goes all the way round.
6. **Curving trails:** not re-curved (2.11); open until Marlow's straight-view measure.

Residents (DECISIONS 2026-09-29): one at the lake, one at each of the three campsites, one in the cultist cave, and one around the parking lot where the drive starts. No names or story in this file.

Sable
