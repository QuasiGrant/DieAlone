# Sound by location (Main3)

**DRAFT, 2026-09-29, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md. Map and positions: Main3.md revision 10 and Main3_map.svg (approved for blockout, DECISIONS 2026-09-29). Time of day, ramp and replacements: DayNight.md (sections cited as DN). Mixer groups and snapshots: Mixer.md. Import and sources: Sourcing.md. Tuning fields: AudioTuning.md. Binding inputs: DECISIONS 2026-09-29 lines on day one sounds (fire at the Ward on night one, ordinary road on day one, chant from night one), the fire never on day one, the slow-burn ramp with creepy replacements, the rave cave, the chant turning into bass on the descent, the gate (option B) and cars arriving until the office resident's storyline ends, and chases never entering the cave. Unity features named here are unverified for 6000.3.24f1 until Rook checks them. Nothing plays in a build that Vesper has not heard.

**Update 2026-09-30, Hollis:** sections 2 (Ward and Ward climb rows), 9.1, 10, 12, 13, 14 and 15 follow Valley.md revision 10 (draft, being built): highway at x 428, fire row on the W crest, the four-leg climb, the cleft fin. Detail in Sound/Valley.md revision 2. Everything else still reads Main3.

Coordinates are Main3's: metres, origin south-west, x east, z north, y absolute height. Positions read off the map sheet (2.5 px per m) are marked "map"; they are within about 2 m and snap to the blockout as built. Sound has no milestone task yet (Milestone 9 is sound groundwork); this is spec only.

## 1. Rules for every place

1. **Hearing radius.** A place's signature sound is heard within 35 m of its source, the keeper's camp within 40 m (Main3.md 3). Radius is horizontal, measured on the map. Where the source is high above the trail, AudioSource max distance is the slant distance, sqrt(radius squared plus height difference squared), so the ring on the ground stays 35 or 40 m. Linear rolloff, min distance worldMinDistance (1 m) unless stated.
2. **Heard before seen.** Landmarks read at 20 m (DECISIONS 2026-09-28 pass check). A 35 m radius gives 15 m, about 6 s at walk, of hearing a place before seeing it; the camp's 40 m gives 8 s. Tall landmarks seen from far off (tower, spar, stack, Snag, mast) are seen first; for them the rule is heard before the clearing opens. Section 14 lists the check per approach.
3. **One sound, one place.** No two places share a signature (Main3.md 1.1). Three chains are on the map (dock, gate, cairn at J). Dock chain: heavy, wet, knocking wood. Gate chain: light, bright, on a steel post. The cairn chain at J is silent.
4. **Beds by zone.** One Ambience bed per zone (Milestone 9), 2D, crossfade zoneBedCrossfadeSeconds at the edge. Edges are trigger volumes placed so trails cross them, not run along them. Exit is zoneExitMarginMeters (start 4 m) further out than entry so the bed does not flutter on the line.
5. **Mains hum only in the front zone** (DN 6). Wild sites have hand, water, wind, fire and battery sounds only.
6. **Day one is full and ordinary; from day two nothing drops out without a replacement** (DN 15, DECISIONS 2026-09-29). Each place lists which DN 15 rows play there.
7. **Night.** Night allows only the Ward (DECISIONS 2026-09-28). Night beds are built for the night route: camp, the camp to J trail, the Ward climb. Other zones at night play their day bed under the Night snapshot (-3 dB, 6000 Hz lowpass) with day-only layers off; no extra files until Pim says whether the player can reach them at night.
8. **Anomaly material.** Each place's signature is what the Missing, Displaced and Altered anomalies use (DN 10). The cave chant and music are never anomalies (DN 8.5).

## 2. Zones and edges

| Zone | Shape and edge (map) | Bed day | Bed night | Fire offset | Priority |
|---|---|---|---|---|---|
| Ward | Ward snapshot from the cleft dogleg trigger (14.5, 265.5), through the last straight, round the fin and the whole ledge (x -10 to 6, z 215 to 285) | room tone (DN 7) | room tone | own roar fan (10.4) | 1 |
| Cave interior | from the mouth (52, 34) inward, same volume as the CaveStone footstep zone | cave room | cave room | -80 dB | 2 |
| Ward climb | from the chute mouth (86, 213) along the four legs, P4 and the cleft to the dogleg, corridor 20 m wide; the approach J to the chute mouth is the fire row's fade (12) | none (closed by day) | climb night, per leg (12) | -80 dB | 3 |
| Ravine | the ravine below the rim: (20, 20), (80, 25), (95, 50), (74, 60), (30, 55) map, extended east along the spur to the rope rail, chant spot 1 (107, 58) map. Edge: the rope rail | ravine | ravine day bed, Night snapshot | -8 dB | 4 |
| Hollow | circle r 16 m on Camp 3 (78, 146). Edge: the rim, top of the log steps (96, 145) map. Crossfade hollowCrossfadeSeconds (start 5 s), the length of the steps | hollow | hollow day bed, Night snapshot | -6 dB | 5 |
| Knoll | circle r 25 m on the camp (170, 160): the 36 m clearing plus 7 m | knoll day | knoll night | 0 dB | 6 |
| Lake | x 120 to 260, z 20 to 105: the water (x 135 to 245, z 32 to 88) plus the shore paths, the pump dock (190, 96), the boathouse (240, 52) and W1 (128, 70) map. Camp to pump crosses the edge about 60 m along | lake | lake day bed, Night snapshot | 0 dB | 7 |
| Front | x 320 to 396, all z. Edge x 320: 8 m before the first sight of the lot (328, 164) map on Jg to T, and on Camp 2 to T just past the food lockers (320, 134) map | front | front day bed, Night snapshot | 0 dB | 8 |
| Burn | the old burn (Main3.md 2.10) from x 230 to x 320. The neck x 185 to 230 is left to Forest, because Camp to Jg runs along its south edge there. Entry on Camp to Jg at forage A (240, 163) | burn | burn day bed, Night snapshot | +2 dB (open) | 9 |
| Forest | everything else, no volume | forest day | forest night | 0 dB | 10 |

1. Where zones overlap, the lower priority number wins.
2. Fire offset is added to the fire row gain inside that zone (section 10). Sheltered places are quieter; the three west glimpses are louder (10.3).

## 3. Keeper's camp (170, 160), knoll 15 m (Main3.md revision 13)

- **Bed (Knoll):** open air on a knoll. Wind across the clearing rather than high in the canopy, a slow low groan from the tower frame in gusts. Different from Forest by what is missing (canopy hiss) and what is added (the structure).
- **Signature, heard before seen:** wind vane squeal on the tower roof, (164, 166) y about 60 (roof height from Rook's blockout). One-shots driven by the wind, vaneIntervalMin / Max (start 8 / 25 s by day). Source is 45 m above the knoll ground (roof about 60, knoll 15, revision 13), so max distance is sqrt(40 squared plus 45 squared) = 60 m (vaneMaxDistance), min 10 m. Without this, a 40 m max distance on a source 45 m up is never heard on the ground.
- **Close layer:** the fire pit loop (Milestone 9), firePitGainDb, firePitMaxDistance 20 m. Position: the pit as Vesper places it in the clearing. Dry wood, steady, a pop now and then. Warm.
- **Edge:** Knoll circle r 25 m. Leaving camp on any leg, the vane stays audible for the first 40 m.
- **Day one:** knoll day bed, the day-one leaves layer, full birds, vane, fire pit.
- **Day two on:** leaves layer off, birds thin, fire row off at High, heard at camp from Mid (10.2).
- **Night:** knoll night bed (near-still air, tower frame tick as it cools). Vane interval longer, vaneIntervalNightMin / Max (start 30 / 90 s). Fire pit burns on (the camp is a chase safe point, lit). Night one: night-one insects layer until the reveal cut; not heard again after.
- **Ramp and replacements:** DN 15 rows 1 and 2 (birds, day), 5 and 6 (crickets and creaks, night), 8 (a far resident signature once, early in the night), 9 Last (the Ward room tone leaks into camp at night).

## 4. Trails: Forest and Burn

### 4.1 Forest (default zone)
- **Bed:** wind high in the giant trees, slow, far overhead (DN 4.1). Loop 60 s or longer.
- **Birds:** one-shots spawned around the listener at 15 to 60 m on random bearings by code, not fixed emitters. Day one day1BirdDensity; later birdChanceAtFullWard thinning to 0 at WARD 1 (DN 12.3). None in the Cave interior, Ward or Ward climb zones.
- **Night:** forest night bed (near-still air). Tree creak one-shots on six giants within 15 m of the night route (camp to J), chosen in the blockout by Rook and me, nightCreakIntervalMin / Max.
- **Points of interest with a sound** (optional, second priority, each 3D World):
  - Hollow Giant (202, 140): wind moaning in the hollow trunk, loop, max 25 m. Ordinary; the obvious DN 15 row 6 Mid source later.
  - Water tank on a stand (182, 128) map: slow drip into a tin tray, one-shots, max 12 m.
  - Phone pole with handset box (273, 72) map: the wire singing in wind, loop, max 15 m. Wind on a wire, not mains, so it keeps rule 1.5.
  - The Gate Tree stub, rowboat, truck, lockers, trailer, latrine: silent. Not everything speaks.
- **Ramp:** rows 1 and 2 by day; 5 and 6 at night.

### 4.2 Burn (x 230 to 320)
- **Bed:** open sky over 4 to 6 m regrowth. Brush rustle close at head height, no canopy hiss, no giants creaking. Brighter and drier than Forest. The change from Forest is audible where Camp to Jg enters at forage A and where Jg to T starts.
- **Day one:** forage A insects (world, 3D, section 5 file shared with the lake, one emitter at (240, 163), max 20 m). Birds full.
- **Day two on:** insects off, replaced by DN 15 row 3 (one fly). Fire offset +2 dB: the burn is open, the fire carries.
- **Ramp:** rows 1, 2, 3.

## 5. Lake: water (190, 60), pump dock (190, 96), boathouse (240, 52)

- **Bed (Lake):** the one wide open sky. Lapping along the shore, wider stereo than any other bed, a far bird over water on day one.
- **Signature, heard before seen:** the dock chain at the pump dock (190, 96), one-shots knocked by the swell, dockChainIntervalMin / Max (start 10 / 30 s), max 35 m. Heavy chain against wood and water. On Camp to pump it is first heard at the water tank, 40 m along (32 m from the dock).
- **Second source:** water slapping the boathouse stilts (240, 52), loop, max 20 m, and a lighter slap under the pump dock, max 15 m. The boathouse tin roof ticks in the sun on day one, one-shots, max 15 m.
- **Edge:** Lake box (section 2). On Pump to W1 the bed holds to W1, where the Ravine edge is the rope rail further on.
- **Day one:** insects at the pump dock, the boathouse and the inlet at W1 (128, 70), each max 20 m. Full birds.
- **Day two on:** insects off, DN 15 row 3 (the fly) at the shore. Tin roof ticks stay (sun is fixed, the roof is always warm).
- **Night:** lake day bed under the Night snapshot; chain interval doubled (dockChainNightScale, start 2).
- **Ramp:** rows 1, 2, 3.
- **Anomaly material:** the dock chain (Displaced: the chain in the forest, DN 10).

## 6. Camp 1 (282, 238), ground 5 m

- **Bed:** Forest. The camp is in the forest; its sounds carry the place.
- **Signature, heard before seen:** pots. The resident's cooking and a hanging rack of pots and tools knocking together as he works, one-shots, potsIntervalMin / Max (start 4 / 15 s), max 35 m, emitter at the camp centre (282, 238) y 6. On the dead-end spur from Jg it is first heard at the latrine shed, 42 m along (34 m from the centre).
- **Also:** the lashed spar creaks under its bulb string in wind, one-shots, max 15 m. The bulbs run on batteries: silent. No generator (rule 1.5).
- **Day one:** pots on time, birds full.
- **Day two on:** pots on the days the site is safe; missing or altered only by event data (DN 10).
- **Night:** pots stop (DN 5.4). Heard from camp at most as DN 15 row 8.
- **Ramp:** rows 1, 2 by day; row 8 at night from camp.

## 7. Camp 2 (292, 108), ground 4 m, stack top 24 m

- **Bed:** Forest, with the boulder field's stone slightly drier in the footsteps (Rock, AudioTuning 3.1).
- **Signature, heard before seen:** wind at the top of the stack. Guy lines whistling and the tent canvas flapping, 3D at the stack top (292, 108) y 25. Loop plus canvas flap one-shots. Height difference to the ground about 21 m, so max distance sqrt(35 squared plus 21 squared) = 41 m (stackWindMaxDistance). On Boathouse to Camp 2 first heard about 55 m along, past the phone pole; on Camp 2 to T heard for the first 40 m.
- **Why wind is a signature here:** the stack top is the only point on the map above the ground wind. It sounds like height from the ground.
- **Day one:** full, flapping.
- **Day two on:** same.
- **Night:** ground wind drops (DN 5.2), so the guy lines go quiet; one slow canvas creak, stackCreakIntervalMin / Max (start 20 / 60 s). The amber tent lamp is battery: silent.
- **Ramp:** rows 1, 2; row 8 at night from camp.

## 8. Camp 3 (78, 146), hollow floor -4 m, rim 4 m

- **Bed (Hollow):** sunk 8 m. The high wind drops away over the log steps and the creek is suddenly close and bright. The one bed that feels enclosed outdoors.
- **Signature, heard before seen:** the creek. Two creek emitters in the hollow (C3, C4 in section 12), max 35 m. The hollow cannot be seen into until the rim, so the creek is always heard first. On Camp to Camp 3 first heard about 70 m along; on W1 to Camp 3 at the camper trailer, 70 m along.
- **Close layer:** a low fire at the hollow centre, damp wood hiss and a slow pop, max 20 m. Smaller and wetter than the keeper's fire pit so the two never sound alike. Heard from the rim, not before.
- **Edge:** the rim at the log steps, 5 s crossfade.
- **Day one:** creek, low fire, birds full.
- **Day two on:** fire offset -6 dB (sheltered) except on the rim, which is a west glimpse (10.3).
- **Night:** the low fire burns down to embers (a lit camp, chase safe point): embers loop, same emitter.
- **Ramp:** rows 1, 2; row 8 at night.
- **Anomaly material:** the low fire (Missing on a safe day; the Snag's lantern event is visual).

## 9. Front zone (x 320 to 396)

### 9.1 Bed and road
- **Bed (Front):** open, flat, man-made. Air over gravel, a wire fence ticking in wind, a far wash of open country beyond the fence. No traffic in the bed: traffic is one-shots.
- **Through-traffic (Valley rev 10, 3.2):** a row of twelve road emitters on the highway centre line x 428, z -130 to 420 every 50 m, y 3.5 (verge 2 to 3); the end emitters sit where the road curves away behind the arm ends (z -130 and about z 430), so passes come round the curve and go out of hearing behind the arms. One-shot passes, roadPassIntervalMin / Max on day one, stretched by roadGapScaleWeek2 in week two, last car at WARD 3 to 5 (DN 6.6.1). roadPassMaxDistance 270 m (was 260; the road is 264 m from camp): clear in the front zone, faint at Jg (262, 172), just audible at camp on a still moment (DN 2.4). Lowpass falls with distance. Open east, flat verge, nothing between: heard from its true bearing.
- **Night:** night one, one car timed for P4 on its own moving emitter (Sound/Valley 1). Nights 2 on, the road is empty and dark all night: no passes.
- **Heard before seen:** the road. It is the only front-zone sound that reaches the trails. The mains hum does not: from the first sight of the lot (328, 164) the transformer is 42 m away. The lot is seen first; the bed edge at x 320 makes the change audible 8 m before the lot shows.

### 9.2 Office (350, 200), ground 3 m
- **Mains hum (signature):** a transformer on the office pole, near (350, 200) y 8, loop, max 35 m. Heard at T (340, 170), 32 m away. Mains hum here and nowhere else.
- **Window AC unit** on the office wall: rattle loop, max 15 m.
- **Office radio** inside: max 15 m, so it is heard at the door and inside, not across the lot. Day one: ordinary garbled chatter (DN 2.5). Day two on: static with rare fragments (DN 6.7). Night: off (DN 5.4).
- **Mast** (30 m, red lamp) beside the office: a relay click in the base box in time with the lamp blink, max 10 m. Event state "lamp steady" (Main3.md 3.1) stops the click: the Missing anomaly for free.

### 9.3 Store (366, 200)
- Chest freezer compressor cycling, freezerOnSeconds / freezerOffSeconds, click on and off, max 20 m.
- Cooler sign: fluorescent buzz with flicker ticks, max 12 m.
- Shop bell on the door, one-shot on open.

### 9.4 Gate (396, 170), booth (392, 176), spur, closed campground
- Gate chain knocking on the steel post, one-shots, max 25 m (light and bright, rule 1.3).
- Barrier lift and lower, one-shots, max 35 m (electric motor; front zone).
- Gate arrivals as DN 6a: approach, idle at the barrier (reserved cue), admitted (gravel up the spur to the chain at (390, 238), chain drop and raise) or refused (turn on the gravel circle at (384, 160), pull away). Exempt from the ramp.
- Booth interior: no own bed. Its walls and window come from the World lowpass only if Rook adds a booth volume later; not in this spec.
- Closed campground loop (372, 262): Front bed, very still. Admitted cars parked there tick as their engines cool for carTickSeconds (start 90 s) after arrival, then nothing. Parked and empty, silent.
- Shift wall message: no sound (UI owns it, Pim).

### 9.5 Front zone over the run
- Day one: road ordinary, radio chatter, machines, birds full.
- Day two on: DN 15 rows 4 (road), 7 (radio), 10 (after the office storyline ends). Machines run day and night at one level (DN 6); at night they are the loudest man-made thing on the map.
- Fire: offset 0 dB, reaches here only at Last (10.2).

## 10. The fire

### 10.1 Fire row
- Nine emitters on the W crest line (Valley rev 10, 2.4), x 12, z -50 to 350 every 50 m, y 82 (2 m over the crest at 80), so the sound comes over the ridge, not through it. The fire is 120 to 400 m further west; a proxy row on the crest keeps the falloff across the map usable and the direction west. Ambience group, 3D, mono, Linear rolloff, fireMinDistance (start 60 m), max distance by band (10.2). Rumble loop plus far crack one-shots (DN 4.2, 4.3).
- Day one: row off, whole scene (Main3.md 5.9 build note). Night one: off until the reveal; after it, on at the current band.

### 10.2 How far it reaches, by WARD band
Slant distances from the nearest row emitter (y 82), map estimates: Camp 3 floor 108 m, J 117, W1 142, camp 172, pump 197, Camp 1 281, Camp 2 291, T 337, office 347, gate 392.

| Band (DN 12.4) | fireMaxDistance | Reaches |
|---|---|---|
| High, WARD 9 to 12 | 150 m (was 140) | Camp 3, J, W1 (faint). Not camp. |
| Mid, 6 to 8 | 200 m (was 190) | the camp by day (DN 12.4), the pump |
| Low, 3 to 5 | 300 m | Camp 1, Camp 2 |
| Last, 1 to 2 | 420 m | the whole map, the front zone under its machines |

WARD changes only at the Ward and at sleep, so the band steps at waking; no mid-day jumps. Gain and lowpass also follow WARD (DN 12.3).

### 10.3 West glimpses (DN, Main3.md 4.3)
Three volumes where the fire is loudest: W1 (128, 70) r 12 m; the Camp 3 rim at the log steps (96, 145) r 10 m; the Camp to J bend, placed near the burn-map board (136, 190) r 10 m until Marlow confirms the bend's position (unverified). Inside: fireGlimpseGainDb (start +6) and the row lowpass opened by fireGlimpseLowpassHz (start +1500 Hz). Glimpses sit on top of the zone offsets; the Camp 3 rim is inside the Hollow edge, so the glimpse wins there. Valley rev 10: the land hides every flame from the floor, so these are no longer sight glimpses. Held as the three spots where the crest columns show best from day two, until Vesper checks what each shows on the build; any that shows nothing goes.

### 10.4 The roar at the Ward ledge
- A fan of ten emitters on the fire itself, seven on the valley fires and three on the far front, gated to the Ward zone (Sound/Valley 6; positions in 13, E49). World group, not Ambience, so it survives the Ward snapshot (Ambience at -80 dB, Mixer.md 3). roarMinDistance 160, roarMaxDistance 450. Night only. Barrier lowpass barrierLowpassHz. Reveal and nightly build as Sound/Valley 7.
- Conflict to settle: DN 3.4 opens the roar to full bandwidth at the reveal, but the Ward snapshot puts a 3000 Hz lowpass on World. I would drop full bandwidth: at the Ward the roar is always held back, which is the image. If Grant wants the open moment, the roar needs its own group or a snapshot change. **[Grant yes]**
- Second conflict: DN 3.7 keeps the roar down the climb on night one; DN 7.5 keeps the climb silent both ways. This doc follows 7.5: the climb zone mutes the fire row, and it returns at J. Walking down from the roar into near-silence and then meeting the fire again at J is stronger than a slow fade. DN 3.7 changes when this is approved. Rechecked on Valley rev 10 from night 2: PASS, every climb eye is 18 m or more under the crest (Sound/Valley 4).

## 11. Cultist cave: spur, ravine, mouth (52, 34), chamber (80, 12)

### 11.1 Outside: the chant **[Grant yes]**
- DECISIONS 2026-09-29 (chant turns to bass on the descent) settles DN 8: outside the cave only the chant carries. The music, and DN 8.2's kick thumping through the forest, stay inside. DN 8 is rewritten to match once this is approved.
- One chant emitter at the mouth (52, 34) y -4, World, 3D, mono, loop. Wordless voices, low, many at once.
- Two trail spots at two volumes (Main3.md 4.2): W1 (128, 70), 84 m from the mouth, faint enough to doubt; chant spot 1, the rope rail (107, 58), 60 m, clearer. A custom rolloff curve with three keys: chantW1GainDb at 84 m (start -30 dB, ear), chantSpot1GainDb at 60 m (start -20 dB), chantMouthGainDb at 0 m (start -10 dB). Max distance 90 m.
- Audibility gate: the chant plays only inside the chant zone (W1 clearing r 12 m, the spur corridor 15 m wide, the Ravine and the Cave interior). Outside it, gain 0 with a 2 s fade. Without the gate, 90 m from the mouth also reaches the footbridge (115, 85) at 81 m, a third spot Main3 does not want.
- Heard before seen: the chant is heard at W1, 109 m before the mouth. Longest lead on the map.
- Timeline: off on day one (DN 2.9). From night one, day and night, steady. Not on the ramp: it is a place, not an ordinary sound that drops out (DN 8.7).
- The coloured bulbs on the dead branch at (82, 52) map, from day two: battery, silent.
- Chases: no pursuer sound enters the chant zone (Main3.md 3.3.7); a chase that reaches W1 ends there.
- The chant uses human voices. DESIGN.md says no voice acting. A wordless chant is not acting, but it is voice, so it needs Grant's yes, the same as any vocal in the track (DN 8.6).

### 11.2 Ravine bed
- Sheltered gully under a 14 m rim: wind passing over the rim above, stone ticking, a far trickle. Fire offset -8 dB (rule: sheltered, even though it is the closest place to the cliff after the Ward). Day one: this bed, birds, the boarded mouth, nothing else.

### 11.3 Inside: chant to bass to rave
Descent progress p comes from the listener's floor height, not path maths: p = clamp01((-6 - y) / 12). The entrance passage is level at -6 (p 0), the ramp falls steadily to -18 (p 1), the chamber is -18. It works the same both ways, so the climb out reverses it for free (Main3.md 3.3.4). Rook confirms which height to read (feet or camera minus eye height).

| Stretch (Main3.md 3.3) | p | Chant | Music (bass) | Music lowpass |
|---|---|---|---|---|
| Entrance passage (52, 34) to (52, 22), floor -6 | 0 | clear, full | off | |
| Leg 1, z 22, -6 to -10: a bass note starts under the chant | 0 to 0.33 | full | rises from off to caveBassLeg1GainDb (start -12 dB) | caveMusicLowpassFar, 150 Hz |
| Leg 2, z 17, -10 to -14: chant thins, bass leads | 0.33 to 0.67 | falls to -12 dB | rises to -3 dB | 150 to 250 Hz |
| Leg 3, z 12, -14 to -18: chant gone, bass only, light growing | 0.67 to 1 | off by p 0.7 | 0 dB | 250 to 400 Hz (caveMusicLowpassLeg3) |
| Level passage (68, 12) to (71, 12), 3 m | 1 | off | 0 dB | opens 400 to caveMusicLowpassNear (8000 Hz) over the 3 m |
| Chamber (80, 12), 18 m across, floor -18, ceiling -10 | 1 | off | full mix | open |

1. The bass is the chamber music itself through a lowpass on its source, not a separate bass file. One music file, one chant file.
2. Music source: the speaker stack in the chamber (position Vesper's), World group (diegetic, not Music), loop 60 s or longer, gated to the Cave interior zone. It is stereo; spatial blend chamberSpatialBlend (start 0.5) so it has a direction and still sounds like a party. Unverified how Unity 6000.3 spatialises a stereo clip on a partly 3D source; Rook checks, and if it folds to mono we use two mono emitters (left and right stacks).
3. Ideally the chant is a stem of the same track, in the same key and tempo, so the bass lands under it and the joke is that the chant was always part of the rave. Depends on the source (11.5).
4. Cave room bed (Ambience, 2D) under all of it: stone air, a far drip. Drip one-shots in the entrance passage at (52, 28) y -4, max 10 m.
5. Talk in the chamber: dialogue is text; while a dialogue is open the music ducks by chamberTalkDuckDb (start -6 dB) so the text reads calm. Music keeps playing.
6. No generator. A full sound system and rave lights 10 m under the forest with no hum and no engine. The rule (1.5) makes that wrong on its own; nobody needs to comment on it.
7. Day one: mouth boarded, music emitter disabled. Day two on: music runs day and night whether or not the player is inside. It never changes with WARD (DN 8.7, proposed).

## 12. Ward climb and ledge (Valley rev 10, 1.6 and 4)

- **Climb bed (night only):** sparse wind, stone, no birds, no insects after night one. J (104, 206) to the path end (-8.5, 246), 265 m: approach, four legs (chute west, shelf north, cwm north-west, under the wall south), P4, the cleft, the fin, the ledge. One sound change per leg; gains, landmarks and positions in Sound/Valley 8.
- **J:** the spring that feeds the creek, a small gurgle at (104, 206), max 15 m. The cairn chain and lamp are silent (1.3).
- **Fire row:** fades out over the approach, J to the chute mouth (fireClimbFadeMeters, 20 m); muted in the climb zone; returns the same way down (10.4, Sound/Valley 4).
- **Thinning to silence:** the climb bed fades over wardClimbFadeMeters (now 6 m), cleft entry (220) to the dogleg (226), where the slot walls explain it, so the Ward snapshot at the dogleg lands on an already thin bed. P4's wind stays full for the look-back.
- **Heard before seen:** the silence. It starts at the dogleg, about 19 m before the fire and the stones show round the fin.
- **Night one:** the ordinary night on the climb (insects, one owl, soft wind, DN 3), the car at P4, the insect cut at the dogleg, the roar from first sight round the fin (Sound/Valley 7).
- **Ledge:** room tone (DN 7.2), the roar fan built by ledge position every night (10.4), the Ward's own voice later (separate spec).
- **Rune post (88):** silent in this spec. It is the natural first place for the Ward's voice; that waits for the Ward design.
- **Ramp:** DN 15 row 9 at the ledge. The climb's WARD changes (cup, seep, ember) follow the visual flags (Sound/Valley 8).

## 13. Emitter positions

All World, 3D, mono unless stated. y where it matters; otherwise ground plus 1 m.

| ID | What | Position (x, z, y) | Max m | Days |
|---|---|---|---|---|
| E1 | vane squeal | (164, 166, about 60) | 60 | all |
| E2 | fire pit | camp clearing, Vesper places | 20 | all |
| E3 | dock chain | (190, 96) | 35 | all |
| E4 | pilings, pump dock | (190, 96) | 15 | all |
| E5 | pilings and tin roof, boathouse | (240, 52) | 20 / 15 | all |
| E6 to E10 | day-one insects | forage A (240, 163), forage B (142, 164), pump dock (190, 96), boathouse (240, 52), inlet (128, 70) | 20 | day one |
| E11 | pots | (282, 238, 6) | 35 | day |
| E12 | spar creak | Camp 1 spar, Vesper places | 15 | all |
| E13 | stack wind and canvas | (292, 108, 25) | 41 | all |
| E14 to E20 | creek C1 to C7 | C1 (100, 195), C2 (88, 172), C3 (80, 152), C4 (84, 138), C5 (100, 112), C6 footbridge (115, 85), C7 inlet (126, 72); map estimates, snap to the creek as built | C3, C4: 35; others 15 | all |
| E21 | spring | J (104, 206) | 15 | all |
| E22 | low fire / embers | (78, 146, -3.5) | 20 | all |
| E23 | transformer hum | office pole near (350, 200, 8) | 35 | all |
| E24 | AC rattle | office wall | 15 | all |
| E25 | radio | inside the office | 15 | day |
| E26 | mast relay | mast base by the office | 10 | all |
| E27 | freezer | store (366, 200) | 20 | all |
| E28 | sign buzz | store front | 12 | all |
| E29 | shop bell | store door | 15 | on open |
| E30 | gate chain | gate post (396, 170) | 25 | all |
| E31 | barrier | (396, 170) | 35 | on use |
| E32 | gate car (moves) | road, barrier, spur, circle | 60 | shifts |
| E33.01 to E33.12 | road row | x 428, z -130 to 420 every 50, y 3.5; ends on the curves | 270 | by day, by ramp; never at night |
| E33.13 | night-one car (moves) | on the car, highway x 428 | 450 | night one, timed for P4 |
| E40 to E48 | fire row (Ambience) | x 12, z -50 to 350 every 50, y 82 | by band | day two on |
| E49.01 to E49.10 | ledge roar fan, L1 to L10 | L1 to L7: x -130, z 36, 144, 202, 246, 290, 348, 456, y -30; L8 to L10: x -300, z 20, 170, 320, y 60 | 450, min 160, gated to the Ward zone | night, from the reveal |
| E49.11 | roar sub, 2D World | on the listener | gated | night, from revealSubAtT |
| E53 | chant | mouth (52, 34, -4) | 90, gated | night one on |
| E54 | rave music (stereo, partly 3D) | chamber stack near (80, 12, -16) | chamber, gated | day two on |
| E55 | cave drip | (52, 28, -4) | 10 | all |
| E56 | Hollow Giant moan (optional) | (202, 140) | 25 | all |
| E57 | tank drip (optional) | (182, 128) | 12 | all |
| E58 | phone wire (optional) | (273, 72) | 15 | all |
| E59 | tree creaks | six giants on the night route, chosen in blockout | 30 | night |
| E60 | seep trickle | head of the chute (54, 216, 31) map | 12 | night; gone when the seep is dry |
| E61 | seep drip, cup or stone | same | 6 | night; cup drips until the cup is gone, then stone |
| E62 | cwm snag creaks | three dead snags round the cwm, chosen in the build | 20 | night |
| E63 | split snag creak | (39, 285, 48) map | 8 | night, once per pass |
| E64 | split snag ember | inside the split snag | 3 | night, when the ember flag is set |

Birds (day) and replacement crickets (night) spawn around the listener by code.

## 14. Approach check (for the Milestone 9 walk)

| Place | Leg | First heard (m along) | Landmark reads at 20 m (m along) | Lead |
|---|---|---|---|---|
| Camp | any leg in | 40 m from the tower | 20 m from the clearing | 8 s |
| Lake | Camp to pump | 40 (water tank) | about 60 | 8 s |
| Camp 1 | Jg to Camp 1 | 42 (latrine) | about 63 | 8 s |
| Camp 2 | Boathouse to Camp 2 | about 55 | about 75 | 8 s |
| Camp 3 | Camp to Camp 3 | about 70 (creek) | 90 (rim) | 8 s |
| Camp 3 | W1 to Camp 3 | about 70 (creek, trailer) | about 95 | 10 s |
| Front zone | Jg to T | 0 (road, far) and 75 (bed edge) | 85 (first sight) | 4 s from the edge |
| Cave | W1 to cave | 0 (chant at W1) | 109 (mouth) | 44 s |
| Ward | J to Ward | 226 (silence, dogleg) | about 245 (fire and stones, round the fin) | 7 s |

"Landmark reads at" is the leg length minus 20 m until Marlow's walk measures it. Pass rule proposed: every place is heard at least 4 s before it reads. Front zone is the tightest; if Grant hears it as late, the edge moves to x 310.

## 15. Tuning fields added by this doc (AudioTuning.md to add on approval)

zoneExitMarginMeters (4), hollowCrossfadeSeconds (5), zoneFireOffsetDb per zone (section 2), vaneMaxDistance (60), vaneIntervalMin / Max (8 / 25 s), vaneIntervalNightMin / Max (30 / 90 s), dockChainIntervalMin / Max (10 / 30 s), dockChainNightScale (2), potsIntervalMin / Max (4 / 15 s), stackWindMaxDistance (41), stackCreakIntervalMin / Max (20 / 60 s), roadPassMaxDistance (270), carTickSeconds (90), fireMinDistance (60), fireMaxDistanceHigh / Mid / Low / Last (150 / 200 / 300 / 420), plus the Sound/Valley 7.6 and 8 fields, fireGlimpseGainDb (6), fireGlimpseLowpassHz (1500), chantW1GainDb (-30), chantSpot1GainDb (-20), chantMouthGainDb (-10), chantZoneFadeSeconds (2), caveBassLeg1GainDb (-12), caveMusicLowpassLeg3 (400 Hz), chamberSpatialBlend (0.5), chamberTalkDuckDb (-6), wardClimbFadeMeters (6, was 40). caveMusicLowpassFar (150) and caveMusicLowpassNear (8000) already exist in DN 13; caveChantRadius (60) in AudioTuning 2.12 is replaced by the three chant keys.

## 16. Files

Naming and import rows from Sourcing.md 4 and 5. Checked 2026-09-29: a search of Assets found no audio files; unverified whether that search saw the git-ignored pack folders, so the owned packs are rechecked before any row below is sourced elsewhere.

Source codes: **IH** in-house (recorded, synthesised or edited by the team; who records and on what is open). **FS0** Freesound, licence filter Creative Commons 0 (search terms given; availability unverified). **BY** CC-BY 4.0 fallback, credited. **ENV** Envato Elements (git-ignored in Assets/Audio/_Envato; subscription kept active until ship, Sourcing.md 1a.3). **GM** Grant's book music (Sourcing.md 1.2). Every file edited in-house to length, mono and level. Order is the plan: first code tried first.

### 16.1 Beds (Ambience, 2D, loop 60 s or longer)

| File | Import row | Plan |
|---|---|---|
| amb_forest_day_loop | 2D bed | FS0 "wind tall trees", "wind canopy", keep only the high layer; IH loop edit |
| amb_forest_night_loop | 2D bed | FS0 "forest night still"; strip any insects (they are separate layers) |
| amb_knoll_day_loop | 2D bed | FS0 "wind open field", plus IH low tower-frame groan layered in |
| amb_knoll_night_loop | 2D bed | IH from the knoll day loop, gusts removed, frame ticks added |
| amb_burn_day_loop | 2D bed | FS0 "brush rustle wind", "shrubs wind" |
| amb_lake_day_loop | 2D bed | FS0 "lake lapping shore", "lake ambience"; no boats, no people |
| amb_hollow_day_loop | 2D bed | FS0 "creek close", "stream small" with IH lowpassed wind above |
| amb_front_day_loop | 2D bed | FS0 "open field wind fence", "chain link fence wind"; no traffic |
| amb_ravine_day_loop | 2D bed | FS0 "canyon wind", "gully ambience"; IH stone ticks |
| amb_climb_night_loop | 2D bed | FS0 "mountain wind night light"; IH thin |
| amb_cave_room_loop | 2D bed, mono | FS0 "cave ambience", "cave room tone"; BY fallback |
| amb_leaves_day1_loop | 2D bed | FS0 "leaves rustle wind" |
| amb_insects_night1_loop | 2D bed | FS0 "crickets night summer" (must cut cleanly: no reverb tail baked in) |
| amb_ward_roomtone_loop | low bed | IH: synthesised or a recorded closed room, lowpassed (DN 7.2) |
| amb_fire_rumble_loop | low bed, mono, on the fire row | FS0 "wildfire", "forest fire rumble", "bonfire roar" pitched down; ENV fallback |
| amb_fire_crack_01..04_os | 3D point | FS0 "tree falling crack far", "wood split"; IH lowpass |

### 16.2 Place sounds (World, 3D, mono)

| File | Import row | Plan |
|---|---|---|
| world_vane_squeal_01..04_os | 3D point | IH: a rusty hinge or gate, recorded; FS0 "weathervane squeak", "rusty hinge" |
| world_firepit_loop | 3D point | FS0 "campfire crackle" (dry, no voices) |
| world_dockchain_01..03_os | 3D point | FS0 "chain dock boat", "chain rattle wood" |
| world_pilings_loop | 3D point | FS0 "water slapping pier" |
| world_tinroof_tick_01..03_os | 3D point | FS0 "metal roof ticking heat"; IH |
| world_insects_day1_loop | 3D point | FS0 "insects summer meadow", "flies buzzing field" |
| world_pots_01..06_os | short one-shot | IH: pots and utensils recorded; FS0 "pots pans clank" |
| world_sparcreak_01..03_os | 3D point | FS0 "wood creak rope", "mast creak" |
| world_stackwind_loop | 3D point | FS0 "wind whistle rope", "guy wire wind" |
| world_canvasflap_01..03_os | 3D point | FS0 "tent flap wind" |
| world_canvascreak_01..02_os | 3D point | IH from the canvas flaps, slowed |
| world_creek_01_loop, world_creek_02_loop | 3D point | FS0 "stream", "brook"; two different recordings so neighbouring emitters do not phase |
| world_spring_loop | 3D point | FS0 "small spring trickle", "water trickle rocks" |
| world_lowfire_loop | 3D point | FS0 "small fire damp hiss", "smouldering fire" |
| world_embers_loop | 3D point | FS0 "embers crackle quiet" |
| world_hum_transformer_loop | 3D point | FS0 "transformer hum", "power pole hum" |
| world_ac_rattle_loop | 3D point | FS0 "window air conditioner" |
| world_radio_chatter_01..08_os | 3D point | IH: band-limited garbled mouth noise and hums, no words (DN 2.5) |
| world_radio_static_loop | 3D point | FS0 "radio static", "walkie talkie static"; IH |
| world_radio_squelch_01..03_os | 3D point | FS0 "squelch walkie talkie"; IH |
| world_mastrelay_click_os | short one-shot | IH: a relay or switch recorded; FS0 "relay click" |
| world_freezer_run_loop, world_freezer_click_on_os, world_freezer_click_off_os | 3D point / short | FS0 "chest freezer compressor", "fridge compressor start stop" |
| world_signbuzz_loop, world_signflicker_01..03_os | 3D point | FS0 "fluorescent light buzz flicker" |
| world_storebell_os | short one-shot | FS0 "shop door bell" |
| world_gatechain_01..04_os | 3D point | IH: light chain on a metal post; FS0 "chain metal pole" |
| world_barrier_lift_os, world_barrier_lower_os | 3D point | FS0 "boom barrier", "parking gate motor" |
| world_car_approach_01..02_os, world_car_idle_loop, world_car_pullaway_os, world_car_spur_gravel_os, world_car_turn_gravel_os, world_car_tick_loop | 3D point | FS0 "car pass by gravel", "car idle", "engine cooling ticking"; IH sweeps (DN 6a) |
| world_chain_drop_os, world_chain_raise_os | 3D point | IH from the gate chain recording, heavier |
| world_roadpass_car_01..04_os, world_roadpass_truck_01..02_os | 3D point | FS0 "car pass by distant", "truck pass far"; IH gain and lowpass sweep, no Doppler |
| world_fire_roar_loop, world_fire_roar_sub_loop | 3D point (roar), low bed (sub) | FS0 "wildfire roar", "large fire"; ENV fallback. The loudest sound in the game: worth the best source |
| world_chant_loop | 3D point | GM (a stem of the cave track, 11.3.3), else IH (team voices, wordless, layered and pitched), else ENV. No words. **[Grant yes]** on voice |
| world_cave_music_loop | Music row (stereo, Preserve rate) routed to World | GM first; ENV second (check music-in-games terms, Sourcing.md 1a.4; prefer no Content ID); BY third. No vocals with words |
| world_cave_drip_01..04_os | short one-shot | FS0 "cave water drip" |
| world_hollowgiant_moan_loop (optional) | 3D point | FS0 "wind hollow tree", "wind bottle"; IH pitch |
| world_tank_drip_01..03_os (optional) | short one-shot | FS0 "drip metal tray" |
| world_phonewire_loop (optional) | 3D point | FS0 "wind telephone wire singing" |
| world_treecreak_01..06_os | 3D point | FS0 "tree creak wind", "large tree creaking" |
| world_bird_day_01..12_os | 3D point | FS0, four common forest species, three calls each, far and near versions by lowpass in IH. Species must not be ones that call at night |

### 16.3 Replacement files (DN 15), per place above

Most rows reuse the files above. New files: world_bird_farcall_os (one sample, never varied), world_wingbeats_01..03_os, world_caw_long_os, world_fly_loop, world_buzz_dense_loop, world_cricket_chirp_os (one chirp, triggered in unison by code), world_truck_gears_far_os, world_gravel_stop_os, world_horn_held_os, world_radio_carrier_loop, world_radio_breath_loop (Voices-adjacent: breath on a channel, IH only), amb_ward_pressure_loop, amb_ward_pulse_loop, world_booth_window_os, world_barrier_creak_os. Plan: FS0 first for the natural ones (fly, caw, crickets, wingbeats, horn, gravel), IH for the radio carrier and breath and the Ward layers.

### 16.4 Not in this doc
Footsteps (AudioTuning.md 3), interaction foley (pump, stove, ladder, lectern, doors, logbook, carry), the Voices, the monster and chase, the Ward's voice, the day-end tone (DN 9). Each gets its own spec.

## 17. Conflicts and open items

1. DN 8 (music carries outside as a thump) against DECISIONS 2026-09-29 (chant outside, turns to bass inside). This doc follows the decision; DN 8 to be rewritten on approval.
2. Ledge roar bandwidth under the Ward snapshot (10.4). **[Grant yes]**
3. Roar on the climb after the reveal: DN 3.7 against DN 7.5; this doc takes 7.5 (10.4).
4. Wordless chant as voice under "no voice acting" (11.1). **[Grant yes]**
5. Vane height: max distance 60 m assumes a roof near y 60 on the 15 m knoll (Main3.md revision 13, deck 56 m absolute); Rook's blockout sets it.
6. Camp to J bend position for the third glimpse: unverified (10.3). Marlow.
7. Night reachability of the lake, burn, hollow, front zone and ravine (rule 1.7). Pim.
8. Stereo clip on a partly 3D source in 6000.3 (11.3.2). Rook. Also AudioSource custom rolloff curves set from code for the chant (11.1), believed to be AudioSource.SetCustomCurve with AudioSourceCurveType.CustomRolloff; unverified.
9. DN 6.4's power line hum under road poles is dropped: the poles are outside the fence and the player never stands under them. The transformer at the office replaces it.
10. Road line: settled on paper by Valley rev 10 at x 428 (9.1); follows the build.
11. Tone words from Vesper for every bed in 16.1 before any source is chosen.

Hollis
