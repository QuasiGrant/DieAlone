---
name: project-sound-docs
description: Hollis's sound spec set in Docs/Design/Sound and Docs/Private, and the non-obvious conventions it established (as of 2026-09-29)
metadata:
  type: project
---

Sound specs live in Docs/Design/Sound: Mixer.md, DayNight.md (DN), Sourcing.md, AudioTuning.md, Locations.md, Shortlist.md (Milestone 9 downloads for Grant). Private (twist material): Docs/Private/StoreSound.md, RouletteSound.md, ScareSound.md (sound per ScareList tag; proposes a fifth mixer snapshot "Cut", tape sounds on UI, Child humming carried by pots because humming is voice). All drafts until Grant confirms lines in DECISIONS.md.

Conventions set in Locations.md:
- 35 m site / 40 m camp radius is horizontal; elevated sources use slant max distance (vane 60 m on the 15 m knoll of Main3 rev 13, stack 41 m). Recheck when Main3 heights change.
- Fire is a proxy emitter row at the cliff (x 0), reach stepped by WARD band (140/190/300/420 m); per-zone fire offsets; three west glimpse volumes.
- Cave: chant outside only; inside, descent progress from floor height drives chant to bass (lowpassed music) to full mix.

Night-one reveal on the built valley (Design/Sound/Valley.md item 7, draft 2026-09-29): insect cut at the turn into cleft part B; untimed hold; roar starts on a camera-tested first-sight trigger for flame tops, builds by ramp progress to the path end; runes after the roar. ScareSound ENV-CUT points there.

Roulette (RouletteSound.md): the rave is his pulse; music cuts on the beat when he is shot and the chant is under it; his foley uses fixed variants per run so each day replays identically; tempo stages need five music files.

Valley rev 10 (2026-09-30) folded in: Sound/Valley.md rev 2 (items 1, 3, 4, 6, 7 rewritten, item 8 = climb landmarks per leg) and Locations 2, 9.1, 10, 12 to 15 updated. Road row x 428 (12 emitters), fire row on the W crest x 12 y 82, ledge roar = 10-emitter fan on the fire gated to the Ward zone, Ward snapshot at the cleft dogleg every night, DN 7.5 night-2 check PASS (18 m+ under crest). Written 2026-09-30 without a shell; commit status unknown, check git.

Keeper's kerosene lamp (DECISIONS 2026-09-30): DN 16 owns the lamp set (world_lamp_*), World 3D on the hand, 12 dB under footsteps. Ordinary nightfall light has a match strike; scare relights have none (the tell). JUMP-LANTERN = gutter + blowout (air at arm's length); CHASE-LIGHT = gutter only, no blowout.

**Why:** Locations.md flagged DN 8, DN 3.7 and Mixer Ward snapshot conflicts; Main3 rev 13 also moved the Ward approach (climb 138 m), not yet re-checked against LOC 12 and 14.
**How to apply:** Before new sound work, check whether Grant approved these docs and whether DN 8 / DN 3.7 / AudioTuning 2.12 / LOC 12 were updated to match.
