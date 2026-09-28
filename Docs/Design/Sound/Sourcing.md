# Sound sourcing and import

**DRAFT, 2026-09-28, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md before they bind. Unity settings named here are from the Unity manual for the AudioImporter; Rook verifies each one exists in 6000.3.24f1 before using it.

## 1. Where sound may come from **[Grant yes]**

1. Made in-house (recorded, synthesised or edited by the team), or
2. CC0 (public domain). Freesound only with the license filter set to "Creative Commons 0". Other CC0 libraries are allowed if the page states CC0 in plain words; list the site in SOURCES.md the first time it is used.
3. Asset Store packs Grant already owns, under the 2026-09-24 pack rule (git-ignored, listed with store link). Checked 2026-09-28: no audio files found under Assets, so the owned packs currently supply nothing.
4. Not allowed: CC-BY, CC-BY-NC, Sampling+, "free for personal use", ripped game or film audio, AI output with unclear terms. CC-BY is refused even though it is legal to use, because one missed credit is a license breach in a public repo.
5. A CC0 file edited in-house stays listed under its original source, with the edit noted.
6. Nothing plays in a build that Vesper has not heard (role rule). A file can be committed before that; the Heard column in SOURCES.md records it.

## 2. Listing in Assets/SOURCES.md **[Grant yes]**

A new section, "Audio", below Textures. One row per file in the project, not per download.

| File | Site | ID | Page | Author | License | Edit | Used for | Heard |
|---|---|---|---|---|---|---|---|---|
| Assets/Audio/Ambience/amb_fire_far_loop.wav | Freesound | 123456 | https://freesound.org/s/123456/ | username | CC0 | cut to 40 s loop, mono, low-pass 4 kHz | Day bed, fire layer | 2026-10-xx Vesper |

1. Example row only; the ID is invented.
2. Author is credited even though CC0 does not require it.
3. In-house files: Site "in-house", ID blank, Author the agent or Grant, License "project".
4. Pack audio (if any is ever used) stays in the pack table and is not repeated here.

## 3. Git LFS

Checked .gitattributes on 2026-09-28: .wav, .mp3, .ogg, .flac, .aif, .aiff all go through LFS. Nothing to change.

1. Commit source audio as .wav, 16-bit, 44.1 or 48 kHz. Unity compresses on import, so the repo keeps the clean original and the build size is set by import settings, not by the file.
2. Tracker formats (.xm, .mod, .it, .s3m) are not in LFS. None are planned. If Music ever uses one, add it to .gitattributes first.
3. .m4a and .opus are not imported by Unity as far as I know (unverified for 6000.3); convert to .wav before import.

## 4. Folders

Assets/Audio/Ambience, World, Voices, UI, Music. Matches the mixer groups (Mixer.md), so a file's folder says where it routes.

Names: `<group>_<thing>_<variant>_<loop|os>`, lowercase, e.g. `world_dockchain_02_os`, `amb_night_wind_loop`.

## 5. Import settings **[Grant yes]**

Aim: sounds like a late-90s disc game. Low sample rates and mono are part of the look, not only a saving. Tune by ear against the VHS filter; the numbers below are starting points.

| Kind | Examples | Force To Mono | Load Type | Compression | Quality | Sample rate |
|---|---|---|---|---|---|---|
| 3D point sounds | dock chain, axe, generator, radio | on | Compressed In Memory | Vorbis | 50 | Override 22050 |
| Short frequent one-shots | footsteps, door, pickup | on | Decompress On Load | ADPCM | n/a | Override 22050 |
| 2D beds (loops over 20 s) | day wind, night bed, fire far | off (stereo) | Streaming | Vorbis | 40 | Override 22050 |
| Low beds | fire rumble, Ward room tone | on | Streaming | Vorbis | 40 | Override 11025 |
| Voices treatment | whispers, breath | on | Compressed In Memory | Vorbis | 60 | Override 22050 |
| UI | page turn, confirm | on | Decompress On Load | PCM | n/a | Override 22050 |
| Music | stingers, day-end tone | off | Streaming | Vorbis | 60 | Preserve |

1. Everything spatial (3D) is mono. A stereo clip on a 3D source is folded down anyway and wastes memory.
2. 11025 Hz cuts everything above about 5.5 kHz: fine for rumble and room tone, wrong for anything with hiss or crackle.
3. Load In Background on for Streaming beds. Preload Audio Data off for Streaming.
4. Normalize off. Levels are set in the mixer and the AudioTuning asset, not baked into files.
5. One Preset per row in Assets/Audio/Presets, applied by folder, so a new file lands with the right settings. Rook to confirm Preset Manager folder filters work for AudioImporter in 6000.3.

## 6. Tunable numbers

All volumes, radii, fades and chances live in one AudioTuning ScriptableObject (Assets/Settings/AudioTuning.asset), editable in Play mode like PlayerTuning. Fields are listed in Mixer.md and DayNight.md.
