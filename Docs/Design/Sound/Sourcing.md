# Sound sourcing and import

**DRAFT, 2026-09-29, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md before they bind. Unity settings named here are from the Unity manual for the AudioImporter; Rook verifies each one exists in 6000.3.24f1 before using it. Binding input: DECISIONS 2026-09-29 on sound and music sources.

## 1. Where sound may come from

Allowed (DECISIONS 2026-09-29):
1. Our own recordings: made in-house (recorded, synthesised or edited by the team).
2. Grant's music from his book. Licence "project". Grant holds the rights; if anyone else co-wrote, performed or published it, Grant confirms he can license it for a commercial game (unverified until he says so). Whether it is committed to the public repo is Grant's call: committed files can be copied by anyone. I would git-ignore it like pack audio.
3. CC0 (public domain). Freesound with the licence filter set to "Creative Commons 0". Other CC0 libraries are allowed if the page states CC0 in plain words; list the site in SOURCES.md the first time it is used.
4. CC-BY, and other licences that allow commercial use with credit. Read the exact licence version on the file's page. CC-BY-SA is not in this group without Grant's yes: share-alike may reach the whole game or its soundtrack (unverified how far for games). **[Grant yes]**
5. Packs Grant owns, under the 2026-09-24 pack rule (git-ignored, listed with store link). Checked 2026-09-28: no audio files under Assets, so the owned packs supplied nothing then. Not rechecked after the 2026-09-29 purchases.
6. Grant's Envato Elements account (section 1a).

Not allowed: anything non-commercial (CC-BY-NC and similar), Sampling+, "free for personal use", ripped game or film audio, AI output with unclear terms.

7. A file edited in-house stays listed under its original source and licence, with the edit noted.
8. Nothing plays in a build that Vesper has not heard (role rule). A file can be committed before that; the Heard column in SOURCES.md records it.

## 1a. Envato Elements

Checked 2026-09-29. The full licence and FAQ pages on help.elements.envato.com returned 403 to my fetch, so I could not read the licence text itself. What follows is from Envato's own summary page (elements.envato.com/learn/how-envato-licensing-works, dated 28 Jan 2026) and search excerpts of the help centre. Grant or Rook reads the full licence while logged in before the first Envato file is used.

1. Registering to a project: Envato's page says every download carries a commercial licence by default, "No project name required". So registration is no longer required. House rule anyway: at each download, name the project "DieAlone" if the form offers it, and save the licence certificate (PDF or text) under Docs/Private/Licences, with the file name matching the SOURCES.md row. Envato's older terms tied one licence to one project; the certificate is the proof either way.
2. Commercial use: allowed, including end products for sale (Envato summary page).
3. After the subscription ends: Envato says items used in projects completed during an active subscription stay licensed. Items cannot be used in new projects after it ends. Unverified: whether a game still in development counts as "completed" if the subscription lapses before release. Until the full text says otherwise, treat it as not safe: keep the subscription active until DieAlone ships, or do not rely on Envato items.
4. Games: Envato's summary does not mention games. A help-centre excerpt says sound effects may be put in a game. Unverified for music in games; to check in the full licence and the music special terms.
5. Music special terms: tracks may not be sold or released as standalone audio, even edited. The end product's value must not come mainly from the music. So no soundtrack release of Envato tracks. Broadcast use has separate terms (not relevant to a game; unverified).
6. Extraction: the item may not be handed over as a reusable source asset. Unity builds pack audio into files players can extract with common tools. Unverified whether Envato treats that as a breach; most stock licences accept it for games if the item is not offered as a separate download. Check in the full licence.
7. Public repo: an Envato file committed to the public GitHub repo is the original item redistributed. Envato audio is git-ignored like pack folders (DECISIONS 2026-09-24), in Assets/Audio/_Envato, listed in SOURCES.md. Restoring it needs a re-download from Grant's account. **[Grant yes]**
8. Credit: Envato's summary lists no attribution requirement. Credit anyway in the in-game list (section 2a), unless the licence forbids naming the source.
9. Content ID: Envato says some tracks are registered with YouTube Content ID. Streamers and trailers using the game may get claims. Envato offers Claim Clear for its subscribers' own videos; unverified whether that covers players' videos. Prefer tracks not registered with Content ID where the item page says so.

## 2. Listing in Assets/SOURCES.md **[Grant yes]**

A new section, "Audio", below Textures. One row per file in the project, not per download.

| File | Site | ID | Page | Author | License | Credit line | Edit | Used for | Heard |
|---|---|---|---|---|---|---|---|---|---|
| Assets/Audio/Ambience/amb_fire_far_loop.wav | Freesound | 123456 | https://freesound.org/s/123456/ | username | CC0 | none required | cut to 40 s loop, mono, low-pass 4 kHz | Day bed, fire layer | 2026-10-xx Vesper |

1. Example row only; the ID is invented.
2. Author is recorded even where the licence does not require it.
3. In-house files: Site "in-house", ID blank, Author the agent or Grant, License "project".
4. Grant's book music: Site "Grant", ID the track title, License "project".
5. Envato files: Site "Envato Elements", ID the item ID, License "Envato Elements", plus the certificate file name under Docs/Private/Licences.
6. License always names the exact version (CC-BY 4.0, not CC-BY).
7. Pack audio (if any is ever used) stays in the pack table and is not repeated here.

## 2a. Credit rule **[Grant yes]**

1. Every audio file has its own SOURCES.md row before it is committed or placed in a scene. No row, no file.
2. Where a licence asks for credit, the Credit line column holds the exact text the licence requires (for CC-BY: title, author, source link, licence and link, and "modified" if edited).
3. The game has a credits list. Every row whose Credit line is not "none required" appears there, word for word. Rook builds it from SOURCES.md rather than typing it twice, so the two cannot drift (how is Rook's call).
4. Credit is also given where not required (CC0, Envato) if Grant wants a complete list; his call.
5. Review check: before any build leaves Grant's machine, the credits list is compared against SOURCES.md. A CC-BY file without its credit is a licence breach.

## 3. Git LFS

Checked .gitattributes on 2026-09-28: .wav, .mp3, .ogg, .flac, .aif, .aiff all go through LFS. Nothing to change.

1. Commit source audio as .wav, 16-bit, 44.1 or 48 kHz. Unity compresses on import, so the repo keeps the clean original and the build size is set by import settings, not by the file.
2. Tracker formats (.xm, .mod, .it, .s3m) are not in LFS. None are planned. If Music ever uses one, add it to .gitattributes first.
3. .m4a and .opus are not imported by Unity as far as I know (unverified for 6000.3); convert to .wav before import.

## 4. Folders

Assets/Audio/Ambience, World, Voices, UI, Music. Matches the mixer groups (Mixer.md), so a file's folder says where it routes. Exception: Envato files (and Grant's music if he keeps it out of the repo) live in git-ignored Assets/Audio/_Envato and Assets/Audio/_Grant, with the same subfolders.

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
