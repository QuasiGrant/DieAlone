# Audio mixer

**DRAFT, 2026-09-28, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md. Mixer features named here (groups, snapshots, exposed parameters, Lowpass Simple, Duck Volume, Send) are from the Unity manual; Rook verifies them in 6000.3.24f1 before building. Sound has no milestone task yet (Milestone 9 is sound groundwork); this is spec only.

## 1. One mixer, five groups **[Grant yes]**

Assets/Audio/DieAlone.mixer

```
Master
  Ambience
  World
  Voices
  UI
  Music
```

No sub-groups until one is needed. Every AudioSource in the game has an output group; none play straight to Master, except the Ward room tone (section 3.6).

| Group | What routes here | 2D or 3D | Examples |
|---|---|---|---|
| Ambience | Beds: continuous loops that are the place and the time, not a thing | 2D beds, plus wide 3D emitters for the fire line | day wind in the giants, night bed, far fire |
| World | Anything with a source in the world: objects, player foley, location signature sounds, anomalies | 3D, mono | footsteps, doors, pump, dock chain, axe beat, generator, store fridge, cave chant |
| Voices | The Voices mechanic and any vocal sound from the residents (no voice acting, per DESIGN.md) | 2D for the Voices, 3D for residents | whispers, breath, murmur under dialogue text |
| UI | Menus, logbook pages, the report confirm, stats screen | 2D | page turn, confirm, stat tick |
| Music | Anything scored, not heard in the world | 2D | day-end tone (if not diegetic, see DayNight.md), rare stingers |

Rule: if the player could walk up to it, it is World. If it is the air, it is Ambience. If it is inside the player's head, it is Voices.

## 2. Effects on groups

Kept to what snapshots need.

1. Ambience: Lowpass Simple (cutoff driven by snapshot). Volume.
2. World: Lowpass Simple. Volume.
3. Voices: Highpass Simple and Echo, set once for the Voices' treatment (tuned later with Vesper). A Send to the Duck Volume on Ambience, so the air thins when a Voice speaks. **[Grant yes]**
4. UI, Music: Volume only.

## 3. Snapshots **[Grant yes]**

| Snapshot | When | Ambience | World | Voices | Music | Lowpass (Amb / World) |
|---|---|---|---|---|---|---|
| Day | From waking until the report is confirmed | 0 dB | 0 dB | 0 dB | 0 dB | off / off |
| Night | From report confirm until sleep | -3 dB | -2 dB | 0 dB | 0 dB | 6000 Hz / off |
| Ward | Player past the last bend of the Ward climb, day or night | -80 dB (room tone survives, 3.6) | -10 dB | 0 dB | -80 dB | 800 Hz / 3000 Hz |
| Paused | Pause menu open | -12 dB | -12 dB | -12 dB | -6 dB | 1200 Hz / 1200 Hz |

1. Day and Night are the base; Ward and Paused are entered from either and return to it.
2. Transition times (AudioTuning): dayToNightSeconds 8, intoWardSeconds 3, outOfWardSeconds 5, pauseSeconds 0.2.
3. The Ward snapshot is entered by a trigger volume at the last bend, not by distance, so the edge is sharp. Out is slower than in: the world comes back reluctantly.
4. Night is lower overall because night is dark and quiet; the fire is what stays. Detail in DayNight.md.
5. No snapshot per location. Location sounds are 3D sources with their own radii; the mixer only knows time of day, the Ward and pause.
6. The Ward room tone must survive Ambience at -80 dB. Option A: it routes to Master directly (the one exception to "none play straight to Master"). Option B: it routes to Ambience, the Ward snapshot keeps Ambience at 0 dB, and code stops every other bed on entry. I prefer A: no code, and the silence cannot be broken by a bed someone forgets to stop.

## 4. Exposed parameters and pause-menu sliders **[Grant yes]**

Four sliders. UI follows Master only.

| Slider | Exposed parameter | Groups it moves |
|---|---|---|
| Master | vol_master | Master |
| Sound | vol_ambience, vol_world | Ambience and World together |
| Voices | vol_voices | Voices |
| Music | vol_music | Music |

1. Stored 0 to 1 in the player settings JSON in persistentDataPath (2026-09-20 rule), default 0.8.
2. Converted to dB as 20 x log10(value), with 0 mapped to -80 dB.
3. Sliders write exposed parameters that sit on top of snapshot volumes. Rook to confirm how exposed parameters and snapshot transitions interact in 6000.3: if a snapshot overrides an exposed parameter, the player slider must live on a separate gain stage (for example a Volume on each group plus an "Attenuation" set by the snapshot). Unverified.
4. The Voices slider goes to 0. It is an accessibility setting; the Voices mechanic must still work as text with it at 0. Pim to confirm with the UI spec.

## 5. AudioTuning fields (mixer part)

Assets/Settings/AudioTuning.asset: dayToNightSeconds, intoWardSeconds, outOfWardSeconds, pauseSeconds, defaultSliderValue, voicesDuckDb (start -6), voicesDuckReleaseSeconds (start 1.5).
