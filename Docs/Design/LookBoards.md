# Look boards for Main3

**DRAFT, 2026-09-29, Vesper. Nothing here is decided until it is a line in DECISIONS.md and Grant has confirmed it.** One board per place in Docs/Design/Main3.md (revision 10, approved for blockout 2026-09-29). Palette hexes and rules are from Docs/Design/Style.md; every value is **P** unless Style.md marks it **now**. Packs are the ones in Assets/SOURCES.md and the PLAN.md Rules and Tips inventory lines (PACK PREFABS, MENHIR, REDWOOD, FIRE, CATACOMBS, FRONT ZONE, PROP PACK). Residents are placeholders by place only.

Shared rules for every board:
1. Three states: **day one** (Style.md 2.0, 6.0: gold late afternoon, no fire, smoke, ash or rune anywhere), **day two on** (2.1, 6.1: rust sunset, ash, far glow), **night** (2.2, 6.2: dark, practicals only). At night the player's only errand is the Ward (DECISIONS 2026-09-28), so most places are seen at night only as a far light.
2. Wrongness is one detail per place, never ten (Style.md 1.3). Day one has none.
3. All pack materials run on project or URP built-in shaders and are retinted to the palette (Style.md 4.5, 8.11). Import Max Size 512; hero pieces 1024.
4. Reference links: each was picked from web search results on 2026-09-29. I have not viewed the images; the link is a mood pointer, not a spec. Licences not checked; nothing is copied into the project.
5. "Gap" means no owned pack piece is known to cover it; ProBuilder with a CC0 texture first, then ask Grant.

---

## 1. Keeper's camp and tower (170, 160), knoll 8 m

- **Mood:** home, kept, warm, small against the sky, the one safe-looking place (Style.md 1.2).
- **Palette:** weathered wood #5C4632, canvas #D8CCB4 (brightest surface), rust #8B4A2B on stove and tower hardware, dulled olive floor (#58583A day one, #4F4A2C day two on).
- **Day one:** gold sun #FFC98A from the west at 14 degrees; long warm pools on the clearing; stove, cabin window and cab lamp on at #FFA860 but modest (Pim's rule, 6.0.7).
- **Day two on:** sun #FF8C40 at 6 degrees; ash settling on roofs, rails and the logbook lectern; the cab faces the glow. Practicals unchanged: the camp is the constant.
- **Night:** the warmest, softest light on the map (#FFA860): stove glow through the door, cabin window, cab light at the top as the marker seen from the Ward trail.
- **Structure and silhouette:** cabin low and horizontal, tower the single tallest vertical, cab a box glazed on all four sides with an overhanging roof. Tower legs timber-braced, not steel lattice, so it never shares silhouette and material with the office mast (Style.md 4.4). Three height bands on the cabin: porch and steps, log body, roof with stovepipe.
- **Density:** highest on the map. 25 or more props in three clusters: fire ring and seats, cabin porch and woodpile, tower foot and lectern. Everything mid-task and tidy: kettle on, axe in the block, logbook open.
- **Kitbash:** Cabin In The Woods (CITW log walls, doors, CITW_Wood_Stove, CITW_Oil_Lamp_Flame_FX); Campsite (CS_Campfire_1, CS_Log_*_Seat, CS_Firewood_Logs, CS_Lantern_*); Farm Tools (Axe, KeroseneLamp, WateringCan, Bucket); Supplies (CanedFood, Water, MatchesV1-3). Tower: ProBuilder with Planks023A and PaintedMetal006 only on bolts, rails and stair treads.
- **Reference:** [Mountain Fire Lookout Tower above forest (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Mountain_Fire_Lookout_Tower_above_forest.jpg)

## 2. Lake: water (190, 60), pump dock (190, 96), boathouse (240, 52)

- **Mood:** open, exposed, still, the one wide sky, a held breath.
- **Palette:** dark water (Water.shader) catching the sky horizon colour; grey-bleached dock wood (#5C4632 lifted toward #6E6660); tin roof rust #8B4A2B in streaks over dull grey; reeds olive.
- **Day one:** the brightest wide frame on the map: gold sky #E3A968 on the water, clear far shore, boathouse roof reads as a tin glint from the tower. Pump lantern lit as a marker.
- **Day two on:** the water turns rust-orange from #D9662E sky; ash film on the surface near the shore (a darker band, not particles on the water); boathouse window lamp on.
- **Night:** off the Ward route. Seen from camp only as the boathouse window, one small warm point over black water.
- **Structure and silhouette:** boathouse on stilts, long and low, pitched tin roof, stilts visible under it: the only building standing in water. Pump a short cast-iron post with a lever on the dock end. Dock long and thin.
- **Density:** sparse on purpose, the counterweight to camp. 15 props in two clusters: pump dock (bucket, rope, cleats, a chair) and boathouse deck (nets, oars, crates, a cold stove pipe). Shore path points of interest carry the rest: overturned rowboat, washed-out truck.
- **Kitbash:** CITW plank modules for boathouse walls and dock; Celestia Roof parts for the tin roof (retint), or a CC0 corrugated steel texture (gap, needs judging); Campsite CS_Rock_* for shore; Farm Tools Bucket, Rope, Hook1-2, OilCanister; Supplies Water. Pump, rowboat: gap. Washed-out truck: Celestia Vehicle_Body (used in Main 1.0), stripped and rusted.
- **Reference:** [Boathouse on Lake Namekagon at Forest Lodge (Wikipedia file page)](https://en.wikipedia.org/wiki/File:Panorama_of_Boathouse_on_Lake_Namekagon_at_Forest_Lodge.jpg)

## 3. Camp 1 (282, 238), ground 5 m: the sprawling workshop camp

- **Mood:** busy, sprawling, practical, over-equipped, a job site nobody is working.
- **Palette:** tarp and canvas #D8CCB4 gone grubby, wood #5C4632, tool steel and rust #8B4A2B, one red fuel can as the site's single saturated object.
- **Day one:** gold sun across a messy yard; festoon string lit #E8B840, pale and harsher than camp, two or three bulbs dead as bought, not as omen.
- **Day two on:** ash on tarps and the workbench; the string reads stronger against the rust haze. Event state: bulbs out along the spar (Main3.md 3.1).
- **Night:** off the Ward route. From the tower, the bulb string on the spar is the only thing that shows.
- **Structure and silhouette:** one big tent, many tarps and work surfaces; the lashed timber spar 24 m, tapering, guyed, with the bulb string swagged down to the tent. The spar is rough timber with rope lashings; it must never read as steel or a telegraph pole.
- **Density:** 30 or more props across 60 m in three clusters: workbench and tools, cookfire and table, stacked kit and fuel. Paths between clusters stay clear (props 2 m off trail centres, Rules and Tips CAMPSITES).
- **Kitbash:** Campsite CS_Tent_Large_Old_*, CS_Campfire_2, CS_Log_* seats, CS_Lantern_*; Farm Tools almost whole set (Saw, Chainsaw, Sledgehammer, ToolBoxOpened, Rope, Canister, AxeLong, Pitchfork, Worn variants); Supplies (GasTank, Toolbox, Backpack, CanedFood Opened and Empty). Spar: ProBuilder cylinders on a bark material from Redwood (retinted) with Farm Tools Rope. Bulbs: small ProBuilder spheres, emission per Style.md 4.6. Latrine shed and wash stand on the spur: CITW planks.
- **Reference:** [Category: Logging camps (Wikimedia Commons)](https://commons.wikimedia.org/wiki/Category:Logging_camps)

## 4. Camp 2 (292, 108), ground 4 m: one tent on a 20 m granite stack

- **Mood:** perched, lonely, wind, refusal, the smallest camp on the biggest rock.
- **Palette:** granite #6E6660, lichen dulled olive, tent canvas #D8CCB4 small against grey, one rope ladder in wood and rust.
- **Day one:** the stack catches the gold sun on its west face; boulder field in shade below. Tent lamp on, tiny.
- **Day two on:** ash collects in the boulder gaps, not on the vertical faces. Event state: tent lamp dark (Main3.md 3.1).
- **Night:** off the Ward route. One small point high on a black shape, seen from the tower.
- **Light conflict, open:** Main3.md rev 10 says "dim amber tent lamp"; Style.md 2.3 says cold white #DEDCD4. With camp, Camp 1 and the office all warm, amber makes this the fourth warm point. I would keep cold white, dim, so Camp 2 is the only neutral light outdoors. If Grant keeps amber: #C07A38 at low intensity, and Style.md 2.3 changes to match.
- **Structure and silhouette:** a single blunt granite tower with a flat-ish top and one tent on it, ladder up the south face. Boulder field of 10 to 15 rocks around it, none above a third of its height, so the stack stays the one landmark.
- **Density:** low. 15 props: 8 on the top (tent, bedroll, lamp, stove, pot, water jug, rope coil, pack), 7 at the foot (rain barrel, ladder base, tarp over stores, crate, bucket). Nothing between.
- **Kitbash:** stack: Rook's mesh (Main3.md 3.4), dressed with Redwood BigBoulders_0-5 and Boulder_0-5 at the base; field from Redwood Boulder and Campsite CS_Rock_*; small CS_Tent_*, CS_Lantern_*; Catacombs ladder pieces (retint) or the Main 1.0 ladder; Supplies (Water, Backpack, CanedFood); Farm Tools Bucket, Rope. Rain barrel: gap.
- **Reference:** [Rock outcrop, Berghamn, Finland (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Rock_outcrop_Berghamn_Finland.JPG)

## 5. Camp 3 (78, 146), hollow floor -4 m: sunk, dense, the Snag

- **Mood:** sunk, damp, close, secretive, hushed by the creek.
- **Palette:** darker olive than anywhere else, wet rock #6E6660 darkened, moss, bleached bone-grey Snag near #D8CCB4 (never brighter), green-glass lantern #6F8436 (olive, never emerald or neon).
- **Day one:** the hollow sits in shade while the rim and the Snag top catch gold sun; the value jump from rim to floor is the look. Lantern on, green, low.
- **Day two on:** the Snag top lit rust from the west; from the rim the smoke towers over the giants (Style.md 6.3.10, fire glimpse point). Event state: the Snag's lantern missing (Main3.md 3.1).
- **Night:** off the Ward route. The Snag is a pale shape against the fire-lit west sky from the tower.
- **Structure and silhouette:** a bowl 25 m across, 8 m deep, log steps down the rim; a low shelter under an overhang or tarp, not a tent (camp and Camp 1 and 2 already have tents); the Snag on the east rim, bare, branch stubs only, top 54 m.
- **Density:** dense and close. 20 props in two clusters: fire and seats by the creek, shelter and stores under cover. Ferns, fungi and fallen wood fill every gap; no bare ground in frame.
- **Kitbash:** Redwood HollowLogs, RubbleDense, ThinFern1-5, FungiCoral, DeadLeaves, GrassMoss; Campsite CS_Log_* steps (StairRamp rule), CS_Campfire_1, CS_Lantern_* with a green glass material; CITW planks for a lean shelter. Snag: Celestia Tree_Dead or a suffercord leafless tree scaled up; texel density and silhouette at 54 m are the risk, check at 20 m and from the tower before dressing around it.
- **Reference:** [Ithaca Hemlock Gorge (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Ithaca_Hemlock_Gorge.JPG)

## 6. Front zone and gate booth: lot (358, 170), office (350, 200), store (366, 200), booth (392, 176)

- **Mood:** municipal, humming, fluorescent-tired, the edge of the world, a job with a uniform.
- **Palette:** asphalt and gravel grey-brown, painted lines worn to #D8CCB4 fragments, prefab siding dull (no clean paint, Style.md 8.2), steel #8B4A2B rust at bolts. Lights: sodium #F08A2A on the lot, red mast lamp #B0201C, store cooler sign as the one cool-white glow inside a window (small, desaturated).
- **Day one:** gold sun low across the lot, long pole shadows east; sodium heads already on but weak by day; red mast lamp blinking; road beyond the gate audible, cars pass.
- **Day two on:** ash on car roofs, the booth ledge and the lot lines; the mast lamp is the landmark through haze from the tower. Event state: mast lamp steady instead of blinking (Main3.md 3.1).
- **Night:** off the Ward route. From the tower, a sodium pool and a red point at 200 m.
- **Structure and silhouette:** office a low prefab box with a porch step and a radio aerial; store smaller, flat roof, lit cooler sign; booth a 2 x 2 m hut with a sliding window and a lift barrier, the smallest building on the map; mast a 30 m steel lattice with the red lamp, the only steel lattice anywhere. Fence 2.1 m chain-link along x 396; gate with a lift barrier.
- **Density:** medium, man-made: 25 props across three clusters: office porch (bench, bin, notice board, ashtray), store front (ice chest, cooler sign, propane cage), booth (clipboard, stool, thermos, radio). The lot stays mostly empty tarmac with the resident's car in the north-east corner.
- **Kitbash:** Parking Lot pack (Ground, Building_1-3, Boom_Gate, Barrier_1-2, Divider_1-4, Bumper, Light_Head, Road_Post, TrashCan_1-2); Modular Chain Link Fence (Fence_Frame_F, Door_Frame, Door_Latch); Celestia Building_Parts, Wall_Parts, Roof, Marketplace_Assets (store counter, shelves, freezers, packaged food); Celestia Vehicle_Body for cars (retint); Supplies (SodaDrink, EnergyDrink, Cigarettes, Flashlight). Mast: ProBuilder lattice on PaintedMetal006. Chain-link may shimmer at 360 rows (PackShortlist 5.2); judge in the blockout capture.
- **Reference:** [Glacier Basin Campground Ranger Station (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Glacier_Basin_Campground_Ranger_Station.jpg)

## 7. Closed campground loop (372, 262) behind the chain at (390, 238)

- **Mood:** off-season, empty, orderly, waiting; cars parked where no one should be.
- **Palette:** pale gravel, faded site-number posts #5C4632, one rusted chain #8B4A2B, dulled olive between pitches.
- **Day one:** the same gold light as the lot; empty pitches in neat order; if cars were admitted, they sit parked and empty, doors shut, nothing else wrong.
- **Day two on:** ash on the cars and the picnic tables, undisturbed: no footprints leading away. That is the one wrong detail, and it is a proposal only (event content is Pim's and Quill's).
- **Night:** no light of its own. Not seen from the tower (Main3.md 3.2.5).
- **Structure and silhouette:** a one-way gravel ring with numbered posts, 8 to 10 pitches, each with a fire ring and a picnic table; no buildings, so it never competes with the front zone.
- **Density:** repeat, not clutter: 8 to 10 identical pitch kits (post, table, cold fire ring) plus the parked cars. Repetition is the look here and the only place it is allowed.
- **Kitbash:** Parking Lot Road_Post, Road_Conus, Barrier_1-2; Campsite CS_Stone_* fire rings and CS_Log_* seats; Celestia benches for tables if no picnic table exists (gap, unverified); chain: ProBuilder links or Farm Tools Rope retinted as chain (gap).
- **Reference:** [Pull-in campsite, Big Meadows Campground (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Pull_In_Campsite_at_Big_Meadows_Campground_Shenandoah_2022-06-19_12-30-54_1.jpg)

## 8. Ward climb and ledge: J to Ward, stones at (32, 258), ground 36 m

- **Mood:** climb, silence, ancient, watched; at night, awe and the size of the fire.
- **Palette:** granite #6E6660 for the Tor and stones; pale cairn near #D8CCB4; runes #B8481C, dim ember, never above the lantern (never teal, Style.md 8.1). Fire: glow #FF6B1A, core #FFE8C0, lit sky #5A2412, smoke underside #6B2A12.
- **Day one:** closed by day (trail is night only); the cairn and chain at J read as an ordinary closed path. No rune lit (Style.md 8.13).
- **Day two on:** same by day; from J the Tor is only a grey dome over the trees.
- **Night:** the one lit route. Cairn lamp at J, rune post a faint ember at 10 m, Tor base black, silence from the last bend, then the ledge: the fire front across 150 degrees or more, stones as black silhouettes against it (Style.md 6.3.1 to 6.3.5).
- **Structure and silhouette:** switchbacks on a bare spur; the Tor a smooth granite dome, radius 18 m, top 58 m; three to six tall carved stones on the cliff edge, leaning, tops up to about 12 m above the shelf. Nothing built by hand except the cairn, chain and post.
- **Density:** the emptiest place on the map. Under 10 objects on the ledge: stones, a few fallen offerings or none (Quill's call), loose rubble. The fire is the dressing.
- **Kitbash:** Menhir Stone Circle, Rock or DarkRock _CarvedRunes variants only (reject Jade, Obsidian, BloodRunes as off palette or gore; Sandstone only if it can be retinted to granite); Tor: Rook's mesh with the granite material, base dressed with Redwood BigBoulders and RubbleSparse; cairn: Campsite CS_Stone_*; rune post: CITW plank; fire: NatureManufacture Prefab_Fire_Huge_Flames_01 (never _Blue), Fire_Embers_01, smoke on DieAlone/Smoke; existing HorizonFire glow kept (PackShortlist 3).
- **Reference:** [Standing Stones of Stenness (Wikimedia Commons)](https://commons.wikimedia.org/wiki/File:Standing_Stones_of_Stenness_062015.jpg)

## 9. Cultist cave as a rave: mouth (52, 34), chamber (80, 12) at -18 m

- **Mood:** outside: grey, closed, unremarkable. Inside: loud, warm, cheerful, sweaty, too much fun for where it is. The wrongness is the joy (DECISIONS 2026-09-29: busting the grim cult cave).
- **Palette:** ravine and entrance rock grey #6E6660 under cold fill #4A5058 and light #9AA3AD. Chamber rave lights, saturated because they are light sources (Style.md 2.4.2): magenta #C0207A, red #D02818, amber #F0A020, sodium orange #F08A2A. No cyan, teal or blue (Style.md 8.1). UV violet is the rave cliche; it needs an 8.1 exception from Grant, and I would leave it out so the cave stays in the warm family.
- **Style.md conflict, open:** 2.3 still says the cave is cold with no glow. Main3.md rev 10 and DECISIONS 2026-09-29 overrule it for the chamber; 2.3 needs a new cave row (cold grey mouth and entrance, warm rave chamber) once Grant confirms the colours.
- **Day one:** mouth boarded with a plain CLOSED, UNSAFE board; grey rock, daylight to the passage end, silence. No coloured bulbs on the spur.
- **Day two on:** boards down. Entrance grey; leg 2 shows coloured light leaking up the rock; leg 3 glows; chamber full colour with moving beams and a haze. Coloured bulb string on a dead branch at 72 m on the spur.
- **Night:** the chamber looks the same day or night (underground); outside, a faint colour on the ravine rock at the mouth, seen only if the player is there.
- **Structure and silhouette:** mouth 4 m tall, rough and natural, not masonry; descent a plain rock ramp in three legs; chamber 18 m across, 8 m tall, rough rock with a made floor (boards, rugs, cables). Beams cut through haze to make shape; the room reads by light, not by props.
- **Density:** entrance and ramp near empty (cable run, a taped arrow, a dropped cup). Chamber 25 or more props in three clusters: sound rig and decks, seating (crates, sofas, cushions), drinks and litter. No skulls, candles, altars or robes as dressing (Style.md 8.7, 8.8, and the brief).
- **Kitbash:** mouth and ramp: Redwood BigBoulders_0-5, Boulder_0-5, RubbleDense; Campsite CS_Rock_*; board: CITW planks. Chamber: Catacombs only for plain walls, pillars, planks, crates, ladders (reject Skull variants, Coffins, Sarcophagus, Bones, Body_Wrap, Urns); Supplies (SodaDrink, EnergyDrink, Cigarettes, Flashlight, Backpack); Farm Tools Canister for a generator's fuel. Speakers, decks, lights rigs, sofas: gap; check Celestia interior folders first. Haze: DieAlone/Smoke on a pack smoke mesh, tinted by the lights.
- **Reference:** [Inside a secret rave in a cave at Sutro Baths (SF Standard)](https://sfstandard.com/2024/07/09/secret-sutro-baths-rave-cave-san-francisco-is-back/)

## 10. Trails and giants (all legs in Main3.md 4)

- **Mood:** long, quiet, dwarfed, walked alone, the routine between chores.
- **Palette:** redwood bark retinted toward #5C4632 (no bright red-orange), floor #58583A day one and #4F4A2C day two on, trail dirt a shade lighter, understory dulled olive (suffercord autumn colours pulled toward olive and rust, never yellow-bright).
- **Day one:** shafts of gold between trunks, canopy floor dark, clearings as pools; west bends show a clear far ridge and sky only.
- **Day two on:** ash drifts on the trail edges; west bends frame the smoke towering over the nearest giants (Style.md 6.3.10); light shafts go rust.
- **Night:** only the Camp to J and J to Ward legs are walked; black trunks, a lantern radius, no ground detail beyond it.
- **Structure and silhouette:** giants at least 30 m apart, trunks 6 to 10 m, never on a grid; small trees clustered between them. Hollow Giant with a walk-in hollow; Gate Tree a burned stub cut to 15 m. Points of interest each a different silhouette: water tank on legs, phone pole, food locker ring, truck, footbridge, camper, rope handrail, burn-map board.
- **Density:** one point of interest per 30 s walked (DECISIONS 2026-09-28); between them, trail-edge litter only (roots, stones, needles, a dropped glove). No props in the middle of the trail.
- **Kitbash:** Redwood Sequoia1-5, RedPine1-5, RedFir1-8, HollowLogs, Boulders, Plants; suffercord trees, Bush1-4, Fern1-4 for mid layer; Celestia Tree_Dead for the burnt Gate Tree top; Celestia Vehicle_Body for the truck; CITW planks for bridges, board and lockers; Campsite CS_Stone_* for stepping stones. Water tank, phone pole with handset box, camper trailer: gap. Pack vegetation stays still (DECISIONS 2026-09-29); Redwood Fx (Butterflies, TreeLeaves, Rain, LightShafts) not used unless I review them.
- **Reference:** [Redwood trunk with a person for scale, Jedediah Smith Redwoods (Wikipedia file page)](https://en.wikipedia.org/wiki/File:Redwood_M_D_Vaden.jpg)

## 11. Old burn: knoll foot (185, 166) to the front zone (340, 178)

- **Mood:** healed-over, bright, scrubby, ordinary; a past fire nobody talks about.
- **Palette:** char #1E1916 only on old stumps and snags, grey-silver dead wood, young regrowth a lighter olive than the forest, berries a dull red as forage patch A's one saturated note.
- **Day one:** the brightest ground in the forest: no canopy, gold light on the regrowth; from the tower a clear lane to the lot. It must read as an old scar, never a recent fire (Style.md 8.13): no ash, no smoke, no glowing ends.
- **Day two on:** fresh ash falls on the old burn like everywhere; the rhyme with the far fire is left to the player, not dressed in.
- **Night:** not walked. From the tower, a darker gap in the canopy between camp and the sodium pool.
- **Structure and silhouette:** a flat thicket of young trees 4 to 6 m (4 m in the last 40 m), no giants, no mid canopy; scattered grey snags up to 10 m and blackened stumps. Trails cut through as narrow lanes with walls of regrowth either side.
- **Density:** dense in growth, sparse in objects: stumps, fallen grey trunks, the Gate Tree stub, the forage bush patch. No man-made props except trail markers.
- **Kitbash:** suffercord small trees and Bush1-4 for regrowth; Celestia Tree_Dead and Grass_Cluster; Redwood DeadLeaves, Branchs, RubbleSparse; stumps from Redwood trunks cut by the terrain or Campsite CS_Log_* (retint char). Check that thousands of young trees stay inside the draw call budget (Rules and Tips PERFORMANCE).
- **Reference:** [Yellowstone fires of 1988, regrowth (Wikimedia Commons)](https://commons.wikimedia.org/wiki/Yellowstone_fires_of_1988)

---

## Open for Grant

1. Camp 2 lamp: cold white (Style.md) or dim amber (Main3.md rev 10). I would keep cold white.
2. Cave chamber rave colours as above, and whether UV violet gets an exception to Style.md 8.1. I would leave it out.
3. Tower legs in timber so the office mast stays the only steel lattice. Rook to say whether a 56 m timber-braced tower is buildable from ProBuilder and CITW planks.

Vesper
