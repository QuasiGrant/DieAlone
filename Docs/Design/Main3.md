# Main3 paper brief

**DRAFT, revision 4, 2026-09-28, Sable. Nothing here is decided.** The loop lives in Docs/Design/DailyLoop.md and is not repeated here. Map: Docs/Design/Main3_map.svg. Numbers provisional until the Milestone 7 simulator.

Coordinates in metres, origin south-west, x east, z north. Heights absolute (metres above map base 0). Walk times at 2.5 m/s, grounded horizontal distance (DailyLoop.md 1.1).

## 1. Purpose

1. A map where every walk is a cost the day has to pay, and no two places cost or feel the same.
2. On day 1 it is a lookout's patch of forest and nothing more. The tower sees every place it must check; the Ward and the cave, the two that take from you, are the two it cannot see.

## 2. Shape

1. **Size: 400 x 300 m.** Fire to the west, forest in the middle, the front zone inside the gate on the east edge.
2. West edge: cliff along x 10, valley floor -40 m below, burning ridge 300 to 500 m beyond.
3. North-west: the spur, high ground to 36 m, the Ward on its cliff edge behind the Tor.
4. South-west: a ravine with the cultist cave below a rock rim.
5. Centre: the keeper's camp on an 8 m knoll with the tower; the lake south; Camp 3 in a hollow west.
6. East: Camp 1 and Camp 2 in the forest, then the front zone.
7. **The fence and gate are the map edge.** Chain-link along x 396, the full height of the map. The gate at (396, 170). The road comes from off-map to the gate and never enters. Inside the gate: a gravel drive, the parking lot, the office and the store.
8. Giant trees: emergents 40 to 50 m tall, trunks 6 to 10 m. Tops never above 50 m absolute; on the knoll at most 42 m tall; on the spur tops below 55 m (under the tower eye, Rook).
9. Routes stay on trails (DailyLoop.md 1.2).

## 3. Places

Six locations with one resident each, plus the keeper's camp and the Ward. Everything reads ordinary on day 1 (DailyLoop.md 6). Sounds heard within 35 m (camp 40 m) unless noted; Hollis owns the sound column.

| Place | Position, ground | Size and feel | Actions (need) | Seen from tower as | Sound |
|---|---|---|---|---|---|
| Keeper's camp | (170, 160), knoll 8 m | 36 m clearing, warm light | tower and lectern; stove (Warmth) | the tower itself | wind vane squeal |
| Lake | water (190, 60), 110 x 55 m; pump on the north dock (190, 96); resident's boathouse on stilts (240, 52) | the one wide open sky | pump (Water); talk; anomaly | open water, boathouse tin roof | lapping, dock chain |
| Camp 1 | (282, 238), ground 5 m | big by kit, one tent: a sprawling workshop camp, 60 m, pale harsh light | cookfire meal (Food, some days); talk; anomaly | a lashed timber spar 24 m with a string of bulbs | pots |
| Camp 2 | (292, 108), ground 4 m | tiny: one tent on top of a 20 m granite stack in a 40 m boulder field, ladder up, dim amber tent lamp | rain barrel (Water, some days); talk; anomaly | the granite stack | wind |
| Camp 3 | (78, 146), hollow floor -4 m | sunk 8 m below its rim, 25 m across, dense, green-glass lantern | sit at the fire (Warmth, some days); talk; anomaly | the Snag: a bleached dead giant on the east rim, top 54 m | creek through the hollow, a low fire |
| Office | (360, 200), front zone, ground 3 m | prefab ranger office on the north side of the lot; store 16 m east at (376, 200) | talk; anomaly; store (Food) | steel lattice mast 30 m with a red lamp | mains hum, radio static with no words |
| Cultist cave | mouth (52, 34), ravine floor -6 m, mouth 4 m tall | the one cold place: grey rock, no sunset colour | placeholder (story session) | hidden (5.5) | chant from night 1, heard only at two trail spots |
| Ward ledge | (32, 258), ground 36 m | stones on the cliff edge facing west, 25 m clearing | night only: the Ward screen | hidden (5.4) | silence from the last bend |

1. Problem states change a landmark, never remove it: bulbs out on the Camp 1 spar, the Camp 2 tent lamp dark, the Snag's lantern missing, the office lamp steady instead of blinking. Details are Vesper's and Pim's.
2. Poles: only two, and they differ: Camp 1's lashed timber spar with bulbs, the office's steel lattice mast with a red lamp. Camp 2 and Camp 3 use rock and a dead tree.
3. Resident hooks and roles: placeholders for the story session. Nothing that leans toward the ending goes in this file.
4. The Tor, the granite stack and the cave mouth are meshes, not terrain (Rook).

### 3.1 Front zone
1. The strip inside the gate, x 340 to 396. The only man-made ground.
2. Road: outside the gate, off-map to the east. Silent: no traffic, ever. The silence is the wrongness (Hollis).
3. Gate (396, 170), chained. Gravel drive 10 m to the lot.
4. Parking lot 30 x 40 m centred (368, 170). Trailhead T at its west side (340, 170).
5. Office (360, 200) and store (376, 200), side by side on the lot's north side, 16 m apart. Store: small, unstaffed, a lit cooler sign.
6. Sound: mains hum here and nowhere else.

## 4. Routes

Shapes differ on purpose: winding trails, a west loop, dead-end spurs, and a straight road the player sees but never walks.

| Leg | Shape | Length m | Walk s | Points of interest (m along leg) |
|---|---|---|---|---|
| Camp to pump | trail | 80 | 32 | 40: water tank on a stand |
| Pump to boathouse | shore path | 84 | 34 | 42: overturned rowboat |
| Boathouse to Camp 2 | trail | 95 | 38 | 48: phone pole with handset box (Early: bell post) |
| Camp 2 to T | trail | 94 | 38 | 47: ring of rusted food lockers |
| Camp to Jg | winding | 125 | 50 | 45: Hollow Giant; 95: forage patch A |
| Jg to T | winding | 105 | 42 | 40: Gate Tree; 85: first sight of the lot |
| Jg to Camp 1 | dead-end spur | 83 | 33 | 42: latrine shed and wash stand |
| T to office / store | across the lot | 40 / 48 | 16 / 19 | office and store 16 m apart |
| Camp to Camp 3 | winding descent | 126 | 50 | 40: forage patch B; 90: log steps down the rim (STAIRS RULE ramp) |
| Pump to W1 | shore path | 80 | 32 | 40: washed-out truck; 80: stepping stones at the creek inlet |
| W1 to Camp 3 | trail | 114 | 46 | 25: footbridge; 70: wrecked camper trailer |
| W1 to cave | dead-end spur, winding down | 109 | 44 | 36: rope handrail, chant spot 1; 72: charms on a dead branch (not visible on day 1); 109: cave mouth |
| Camp to J | trail | 96 | 38 | 48: burn-map board; 90: plank bridge (creek water) |
| J to Ward | switchbacks, night only | 130 | 52 | 0: pale cairn and chain; 50: rune post; 90: Tor base; 110: last bend, silence; 130: stones |

1. Two points of interest are giant trees (Hollow Giant, Gate Tree), plus the Snag landmark. All three are scene objects with mesh colliders (Rook).
2. **The cave spur is unsigned at W1 on purpose.** The cave is hidden and unchecked; finding it is the player's choice. Chant spot 2 is W1 itself.
3. Every leg walked eastward has one bend turning the player west for a glimpse of the glow (Vesper). The fire is loudest at the three west glimpses: W1, the Camp 3 rim, the Camp to J bend (Hollis).
4. Creek: spring by J, through the Camp 3 hollow, to the lake inlet at W1.
5. Loops: east ring (Camp, pump, boathouse, Camp 2, T, Jg, Camp) 583 m; west loop (Camp, pump, W1, Camp 3, Camp) 400 m.

### 4.1 Junctions
Every trail leaves its clearing aimed at the next landmark for its first 30 m. Day markers: spar with bulbs (Camp 1), granite stack (Camp 2), the Snag (Camp 3), red mast lamp (office), water glint (lake), pale cairn over the cut (J). The tower cab as a marker from junctions is unverified; Rook tests it in the blockout.

## 5. Tower sightlines

1. Tower base (164, 166) on the 8 m knoll, deck 56 m, eye 57.6 m (58.2 m at jump height). Every giant is at least 7.6 m below the eye.
2. Cone rule: from the eye to both edges of each landmark, no crown or trunk edge within 3 m of the line.
3. Landmark checks at each clearing's near edge, from the deck centre:

| Place | Bearing | Dist m | Target (abs) | Margin m |
|---|---|---|---|---|
| Lake | SSE | 137 | far water, boathouse roof | 5.7 |
| Camp 1 | ENE | 138 | spar top 29 | 9.2 |
| Camp 2 | ESE | 141 | granite stack top 24, boulder field r 20 | 3.8 |
| Camp 3 | W | 88 | Snag top 54 | 4.0 over every giant cap |
| Office | E | 199 | mast lamp 33 | about 12 |

4. **Ward hidden:** the Tor, a granite dome on the crest at (76, 223), radius 18 m, base 34 m, top 58 m. The line from the deck centre to the stone tops (48 m) crosses it 6.4 m under the top, about 5.4 m at the outer stone edges.
5. **Cave hidden:** the ravine rim, 14 m, north of the mouth (mouth top -2 m). The line from the deck centre crosses the rim 8.3 m under it.
6. **Checks W-1 and C-1** (blockout, rerun after any change): trees off. Eye points on a 1 m grid over the whole deck and walkway, at eye height and jump height. Rays to every stone corner (W-1) and to the cave mouth's corners (C-1). Every ray must hit the Tor, rim or terrain. Repeat with eyes and targets raised 3 m: every ray must still hit. The margins above are from the deck centre only until this runs.
7. Haze: landmarks at 137 to 199 m. First try a distance fog end past 300 m for Main3 and emissive lamps. Height fog is new shader work; only if the blockout shows landmarks lost (Rook).
8. **Fire before the reveal:** the tower sees what any lookout sees: a smoke column and a glow over the far ridge to the west, an ordinary forest fire far away (DailyLoop.md question 5). A band of cliff-edge giants at 46 m or lower hides the valley and the fire's base. Only the Ward shows its scale and the line it cannot cross.

## 6. Break the uniformity

1. Sizes: 60 m (Camp 1), 40 m boulder field (Camp 2), 36 m (camp), 25 m sunken (Camp 3), a 110 m lake, a lot.
2. Distances from camp: 80 m (pump) to 270 m (office).
3. Heights: -6 m (cave), -4 m (hollow), 8 m (knoll), 24 m (stack top), 36 m (Ward).
4. Light: warm (camp), pale harsh (Camp 1), dim amber (Camp 2), green glass (Camp 3), red lamp and sodium (office), grey (cave).
5. Actions: each camp gives a different need (Food, Water, Warmth). The office has the store. The lake has the sure water.

## 7. Daily time budget (provisional)

1. Duty 540 walk-seconds (9 hours, DailyLoop.md 1.1). Fixed: tower 100 s, report 20 s. Free: 420 s.
2. Action costs: DailyLoop.md 3. Anomaly resolve 90 s. Routes start and end at the camp centre; the stove cost includes its walk.

| Day | Route | m | Walk s | Actions | Total s | Needs missed | Spare |
|---|---|---|---|---|---|---|---|
| Quiet, near plan | west loop | 400 | 160 | water, forage B, talk (Camp 3), stove | 328 | 0 if forage finds | 92 |
| Quiet, sure food | east ring and store | 679 | 272 | water, store, stove | 370 | Social (a talk at the office is 13 s over) | 50 |
| Lake anomaly | pump, boathouse and back | 328 | 131 | resolve, talk, water, stove | 349 | Food (forage B is 1 s over) | 71 |
| Camp 3 anomaly | west loop | 400 | 160 | resolve, talk, forage B, water, stove | 418 | 0 if forage finds | 2 |
| Camp 1 anomaly | Jg, Camp 1 and back, patch A | 416 | 166 | resolve, talk, forage, stove | 404 | Water | 16 |
| Camp 2 anomaly | lake side out and back | 518 | 207 | resolve, water, stove | 365 | Food, Social | 55 |
| Office anomaly | T, office, store and back | 564 | 226 | resolve, store, stove | 394 | Water, Social | 26 |
| Two far anomalies | Camp 1 and Camp 2 | 749 | 300 | two resolves | 480 | over by 60: one CHECK stays open | none |

3. All five needs fit only on a quiet day by the near plan, or a Camp 3 anomaly day with 2 s spare, and only if forage finds. Every other anomaly day costs at least one need.
4. Night: camp to the Ward by J, 226 m, 90 s.
5. **Real time per day: about 11.5 min** (9 min of day, 90 s to the Ward, about 60 s at the screen).

## 8. Reviews needed

- Vesper: section 3 feel and light, the two poles, problem states, day 1 fire look.
- Pim: junction markers 4.1, lectern, report box, the bunk-opens-the-screen rule (DailyLoop.md 2.5).
- Hollis: section 3 sound column, the silent road, chant volumes.
- Quill: placeholders only; Camp 1 as one resident with a lot of kit.
- Marlow: the budget in section 7, the trail rule, the 30 s check.
- Rook: W-1 and C-1 grid checks, meshes for the Tor, stack and cave mouth, fog plan, tower cab from junctions.

## 9. Open questions

The map raises no new question. Grant's questions are in DailyLoop.md 10.

Sable
