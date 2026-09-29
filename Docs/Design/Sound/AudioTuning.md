# AudioTuning fields and footstep surfaces

**DRAFT, 2026-09-29, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md. Sources: Mixer.md, DayNight.md, Sourcing.md in this folder; Main3.md revision 7 for places and surfaces; PLAN.md Rules and Tips (Tunables line) for the pattern. Sound has no milestone task yet (Milestone 9 is sound groundwork); this is spec only. Unity APIs named here are unverified for 6000.3.24f1 until Rook checks them.

## 1. The asset **[Grant yes]**

1. Class `AudioTuning : ScriptableObject`, `[CreateAssetMenu(menuName = "DieAlone/Audio Tuning")]`, asset at Assets/Settings/AudioTuning.asset. Same pattern as PlayerTuning and LookTuning: public fields grouped by `[Header]`, a `[Tooltip]` with the unit on each, scripts take a `[SerializeField] AudioTuning tuning` reference, editable in Play mode and kept after Play stops.
2. Levels are stored in dB and converted in code with volume = 10^(dB / 20). AudioSource.volume is 0 to 1, so every source level is 0 dB or lower; no field may push a source above 0 dB. Offsets (sprint, per surface) are added and the sum clamped to 0 dB.
3. Snapshot levels and lowpass cutoffs (Mixer.md section 3) live in the mixer asset, not here. Only transition times and runtime numbers are here.
4. WARD-driven fields come in pairs, "at full WARD" and "at low WARD". Code blends them with t = clamp01((startWard - WARD) / (startWard - 1)), startWard read from LoopTuning (12 now), never copied here.
5. Clips are asset references, not numbers, but the footstep and zone-bed clip lists live in this asset beside their gain so one Inspector page tunes a surface by ear. Every other clip sits on its own AudioSource in the scene.
6. Starting values marked "ear" have no basis yet beyond a guess; they get set in Milestone 9 by listening.

## 2. Field list

### 2.1 Mixer (from Mixer.md)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| dayToNightSeconds | float | s | 8 | Day to Night snapshot transition, starts under the day-end tone tail |
| intoWardSeconds | float | s | 3 | into Ward snapshot at the last bend trigger |
| outOfWardSeconds | float | s | 5 | slower out than in |
| pauseSeconds | float | s | 0.2 | into and out of Paused |
| defaultSliderValue | float | 0 to 1 | 0.8 | first-run value of the four pause sliders; saved values live in the player settings JSON, not here |
| sliderFloorDb | float | dB | -80 | slider value 0 maps here |
| voicesDuckDb | float | dB | -6 | Ambience duck when a Voice speaks; code writes it to an exposed parameter on the Duck Volume (Rook verifies Duck Volume threshold is exposable) |
| voicesDuckReleaseSeconds | float | s | 1.5 | |

### 2.2 Footsteps: movement

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| walkStepMeters | float | m | 1.1 | horizontal distance per step while grounded; 2.5 m/s gives about 2.3 steps/s |
| sprintStepMeters | float | m | 1.8 | 5.5 m/s gives about 3 steps/s |
| crouchStepMeters | float | m | 0.6 | 1.5 m/s gives 2.5 steps/s, quiet |
| footstepBaseGainDb | float | dB | -8 | ear; every step starts here |
| sprintGainDb | float | dB | 3 | offset, added |
| crouchGainDb | float | dB | -8 | offset, added |
| gainJitterDb | float | dB | 2 | random plus or minus per step |
| pitchJitter | float | ratio | 0.06 | random plus or minus on pitch 1; keep small, large jitter reads as a toy |
| landMinFallSpeed | float | m/s | 3 | landing one-shot only above this; the 0.6 m hop lands near 4.9 m/s, a 0.4 m step near 4 m/s (gravity 20) |
| landGainDb | float | dB | 0 | offset on the land clip |
| jumpTakeoffGainDb | float | dB | -4 | offset; takeoff plays one walk clip |
| footstepProbeLength | float | m | 0.4 | downward ray from 0.1 m above the capsule bottom |
| zoneProbeRadius | float | m | 0.15 | sphere at the ray hit for sound zones (section 3.2 step 2) |
| footstepSpatialBlend | float | 0 to 1 | 1 | World group, 3D mono source at the feet (Mixer.md); drop toward 0.7 only if it pans oddly on stairs |
| noRepeat | bool | | true | never the same clip twice in a row on one surface |

### 2.3 Footsteps: per surface

One list entry per surface (section 3). Serializable struct `SurfaceSound`:

| Field | Type | Unit | Notes |
|---|---|---|---|
| surface | enum FootstepSurface | | DirtTrail, ForestFloor, GravelLot, AsphaltRoad, WoodDeck, CabinFloor, Rock, WaterEdge, CaveStone |
| walkClips | AudioClip[] | | 6 to 8 variants |
| landClips | AudioClip[] | | 2 to 3 variants; empty falls back to the loudest walk clip at landGainDb |
| gainDb | float | dB | per-surface offset, starts below |
| pitch | float | ratio | per-surface centre pitch, start 1 |

| Additional field | Type | Start | Notes |
|---|---|---|---|
| defaultSurface | FootstepSurface | ForestFloor | used when nothing else resolves (section 3.4) |
| terrainLayerSurfaces | list of (TerrainLayer, FootstepSurface) | see 3.2 | maps each Main3 terrain layer asset to a surface |

### 2.4 Echo anomaly (DayNight.md 10.4)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| echoDelaySeconds | float | s | 0.4 | a beat late |
| echoStepCount | int | steps | 5 | four or five |
| echoDistanceBehind | float | m | 5 | source placed behind the player on the path walked |
| echoGainDb | float | dB | -3 | offset on the player's own step; same surface clips, never louder |

### 2.5 Day bed and fire (DayNight.md 4, 12)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| dayWindGainDb | float | dB | -14 | ear |
| fireRumbleGainDb | float | dB | -22 | at full WARD, ear |
| fireGainRangeDb | float | dB | 6 | added as WARD falls to 1 |
| fireLowpassAtFullWard | float | Hz | 800 | on the fire emitters |
| fireLowpassAtLowWard | float | Hz | 3500 | |
| fireCrackIntervalMinFull / MaxFull | float | s | 60 / 180 | at full WARD |
| fireCrackIntervalMinLow / MaxLow | float | s | 15 / 45 | at low WARD |
| birdIntervalMin / Max | float | s | 20 / 60 | added here: DayNight.md gives a chance but no interval |
| birdChanceAtFullWard | float | 0 to 1 | 0.5 | chance a scheduled call plays; 0 at WARD 1 |

### 2.6 Day one (DayNight.md 2)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| day1BirdDensity | float | calls/min | 6 | ear; replaces the interval above on day one |
| day1InsectGainDb | float | dB | -18 | lake and forage ground |
| roadPassIntervalMin / Max | float | s | 90 / 240 | day one only |
| radioChatterIntervalMin / Max | float | s | 20 / 60 | office radio, day one |
| radioFragmentChanceAtFullWard | float | 0 to 1 | 0.3 | day two on; 0 at WARD 1 |

### 2.7 Night (DayNight.md 5)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| nightCreakIntervalMin / Max | float | s | 15 / 45 | tree creak one-shots |

### 2.8 Front zone (DayNight.md 6)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| freezerOnSeconds | float | s | 40 | |
| freezerOffSeconds | float | s | 20 | |

### 2.9 Night one reveal (DayNight.md 3)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| revealCutSeconds | float | s | 0.1 | night bed cut |
| revealHoldSeconds | float | s | 1.5 | room tone and breath only |
| revealRiseSeconds | float | s | 4 | roar rise |
| revealRoarGainDb | float | dB | 0 | the loudest thing so far; everything else sits lower so 0 dB is room enough |
| barrierLowpassHz | float | Hz | 1500 | ear; the roar held back at the Ward line |
| wardScreenDuckDb | float | dB | -6 | roar under the Ward screen |

### 2.10 An event ends the day (DayNight.md 11)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| eventCutSeconds | float | s | 0 | hard cut |
| eventBlackSeconds | float | s | 2 | |
| eventBreathSeconds | float | s | 6 | breath slowing at the Ward |

### 2.11 Anomalies (DayNight.md 10)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| anomalyStopDistance | float | m | 15 | |
| anomalyLookAngle | float | degrees | 20 | |
| anomalyLookSeconds | float | s | 1 | |

### 2.12 Places, radii and levels

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| siteSignatureRadius | float | m | 35 | AudioSource max distance, Linear rolloff (Main3.md) |
| campSignatureRadius | float | m | 40 | |
| caveChantRadius | float | m | 60 | |
| caveChantGainDb | float | dB | -16 | added here, ear |
| worldMinDistance | float | m | 1 | default min distance for 3D World sources |
| firePitGainDb | float | dB | -6 | added here: Milestone 9 names the fire pit loop |
| firePitMaxDistance | float | m | 20 | Linear rolloff |
| wardRoomToneGainDb | float | dB | -30 | ear; must never read as digital silence |
| dayEndToneGainDb | float | dB | -10 | ear |

### 2.13 Zone beds (Milestone 9: one bed per zone, an edge where it changes)

| Field | Type | Unit | Start | Notes |
|---|---|---|---|---|
| zoneBedCrossfadeSeconds | float | s | 3 | when the player crosses a zone edge (trigger volume) |
| zoneBeds | list of (zone name, AudioClip, gainDb) | dB | -14 each | ear; zones follow Sable's map: camp, trails, Ward climb, lake, front zone, cave ravine |

## 3. Footstep surfaces

### 3.1 The nine surfaces and where they are in Main3

| Surface | Where in Main3 | Sound, in words | Gain start |
|---|---|---|---|
| DirtTrail | every painted trail, clearings, the Ward climb below the last bend | packed dry earth, short, a little grit | 0 dB |
| ForestFloor | ground off the trails (only reachable at trail edges and clearings), the old burn | soft duff, needles, a dry twig now and then | -2 dB |
| GravelLot | gravel drive from the gate, the parking lot (if dressed as gravel) | loose crunch, longest tail of the set | 0 dB |
| AsphaltRoad | none walkable today: Main3 puts the road off-map outside the gate. Kept for the lot if Vesper dresses it as asphalt | flat hard scuff | 0 dB |
| WoodDeck | tower stairs and deck, pump dock, boathouse deck and stilts walk, footbridge, plank bridge, log steps down the Camp 3 rim | open planks, hollow knock, some give | 0 dB |
| CabinFloor | indoor wooden floors: tower cab, cabin if it exists, boathouse interior, office and store unless they get their own floor | enclosed boards, duller knock, a creak on some variants | -1 dB |
| Rock | the Tor, the granite stack top at Camp 2 and the boulder field, stepping stones at the creek inlet, steep terrain painted rock, the Ward ledge from the last bend | hard, dry, close; at the Ward it is most of what the player hears | 0 dB |
| WaterEdge | lake shallows along the shore path, the creek where it is walkable | splash in shin-deep water, slower attack | 0 dB |
| CaveStone | from the cave mouth inward | wet stone, a short tight reflection baked into the clip, colder than Rock | -1 dB |

1. The old burn uses ForestFloor for now. A tenth surface (ash and char) would be one more clip set; open for Vesper.
2. Ladders (Camp 2) are not a surface: climbing is not a player feature. If the ladder is built as a ramp, it is WoodDeck.

### 3.2 How each surface is detected

Resolution runs once per step, in this order; the first that answers wins. **[Grant yes]**

1. **Component on the hit collider.** A downward ray (footstepProbeLength, triggers ignored) from the capsule bottom. If the hit collider or a parent has a `SurfaceSound` component, use its surface. Used for every mesh surface: WoodDeck, CabinFloor, Rock on the Tor, stack, boulders and stepping stones, GravelLot or AsphaltRoad if the lot is a mesh. Every StairRamp collider needs one (WoodDeck), since the ray hits the ramp, not the treads.
2. **Sound zone.** If no component answered, a small overlap sphere (zoneProbeRadius, triggers only, on a dedicated SoundZone layer) at the hit point. A `SoundZone` trigger volume holds one surface. Used for WaterEdge (a volume over the wading band and the walkable creek; optional water-surface height so it answers only when the feet are below it) and CaveStone (one volume from the mouth inward, overriding whatever terrain or mesh is under it). Stepping stones and docks keep their own sound because step 1 wins.
3. **Terrain layer.** If the hit collider is a TerrainCollider, read the alphamap at the hit point (TerrainData.GetAlphamaps for one cell, the layer with the largest weight, TerrainData.terrainLayers for the asset) and look it up in terrainLayerSurfaces. No blending between layers; the strongest layer plays. Expected Main3 layers and their surfaces:

| Terrain layer (Main3, names not fixed yet) | Surface |
|---|---|
| Trail | DirtTrail |
| Forest floor / moss | ForestFloor |
| Old burn | ForestFloor (see 3.1 note 1) |
| Rock (steep) | Rock |
| Gravel (front zone, if painted) | GravelLot |
| Shore mud (if painted) | DirtTrail |

4. **Fallback.** defaultSurface (ForestFloor). In the Editor and development builds, log a warning once per collider name so Marlow's machine walk lists every unmarked surface. Compiled out of release builds (same rule as the dev menu).

Why a component and not tags or physics materials: a GameObject has one tag and tags are shared with gameplay; physics materials change friction and are easy to swap by accident. A component says only what it sounds like and costs nothing to add in a recipe. Rook may prefer physics materials; if so, a lookup from PhysicsMaterial to surface in this asset is the same shape.

### 3.3 Unity parts to verify (Rook)

TerrainData.GetAlphamaps, TerrainData.alphamapWidth / alphamapHeight, TerrainData.terrainLayers, Physics.Raycast and Physics.OverlapSphere with QueryTriggerInteraction, Component.GetComponentInParent, AudioMixerSnapshot.TransitionTo, AudioMixer.SetFloat. All believed present in 6000.3; unverified.

## 4. Waiting on others

1. Sable: final Main3 terrain layer names, whether the lot is gravel or asphalt, whether the keeper's camp has a cabin, zone edges for the beds.
2. Vesper: burn surface yes or no; lot material.
3. Rook: section 3.3; component versus physics material.

Hollis
