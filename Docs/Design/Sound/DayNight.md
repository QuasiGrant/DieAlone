# Day and night sound

**DRAFT, 2026-09-28, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md. Mixer groups and snapshots: Mixer.md. Import settings and sources: Sourcing.md. Binding inputs: DECISIONS.md 2026-09-28 lines on the daily loop, two states, night at the Ward, the cave. Per-location ambience waits for Sable's Main3 map and DailyLoop.md; nothing here fixes a position.

Tone target: psychological first. The world is quiet, thin and a little too even. A jump is punctuation.

## 1. Two states

| State | From | To | Snapshot |
|---|---|---|---|
| Day | Waking | Report confirmed | Day |
| Night | Report confirmed | Sleep | Night |

The sun is fixed, so sound does not follow a clock. Nothing drifts through the day; the change is one event, the report.

## 2. Day bed (Ambience, 2D stereo, loops)

1. Wind high in the giant trees. Slow, far overhead, little at ground level. 60 s loop or longer so the seam is not learned.
2. Far fire: a low rumble from the fire front. Built as a row of wide 3D emitters along the fire line on Sable's map (mono, Linear rolloff, large min distance), not 2D, so turning your head places the fire.
3. Far fire cracks: one-shots on the same emitters, a giant tree splitting far off. Random interval.
4. Birds: few, far, and fewer as WARD falls (section 8). No close birdsong anywhere. The forest has mostly left.
5. Ash: nothing. Ash is silent. Resist adding a sound for it.

## 3. Night bed (Ambience)

1. The night bed is the day bed with things taken away, not a new bed. The ear should notice what stopped.
2. Wind drops to a near-still layer with tree creak one-shots (World, 3D, on a few giants near trails).
3. No birds. No insects. The fire rumble keeps its level; with everything else gone it reads louder without being louder.
4. Resident signature sounds stop at night (the axe, the canvas chimes, the radio voice), except where an event says otherwise. **[Grant yes]**
5. Front zone machines keep running (section 4).
6. Night allows only the Ward (DECISIONS 2026-09-28), so the night bed is heard on the camp, the trail to the Ward and the climb. Nowhere else needs a night mix.

## 4. Front zone machines and wild sites **[Grant yes]**

Rule: mains electric sound exists only in the front zone (road, fence, parking lot, store, office). Everything past it is hand, animal, water, wind and fire.

Front zone (World, 3D, mono, loops unless noted):
1. Store: chest freezer compressor cycling on and off (on about 40 s, off about 20 s, AudioTuning). The click at the start and end of each cycle is the useful part.
2. Store or office sign: fluorescent buzz with an occasional flicker tick.
3. Office: window AC unit rattle.
4. Road: power line hum on the poles, heard only standing under them.
5. Gate: chain knocking on the post in wind (one-shots).
6. Beyond the fence the road is silent. No traffic, ever. That absence is the front zone's wrongness and it costs nothing to build.

Wild sites (lake, campsites, cave): no hum, no compressor. Battery devices (a resident's radio) are allowed because they hiss and crackle, they do not hum.

Machines run day and night at the same level. They do not care what time it is. At night they are the loudest man-made thing on the map.

Early era: no mains power. Front zone replacement is open until the Early era is designed.

## 5. Anomalies heard, not seen

Serves Safety events and general dread. Hearing an anomaly is never required: the tower marks locations SAFE or not, and spotting anomalies as a skill belongs to a minigame (DECISIONS 2026-09-28). Sound here is atmosphere and a pull, never a gate.

Rules **[Grant yes]**:
1. At most one wrong sound per day, across the whole map. Rarity is what makes it land.
2. Heard, never seen. No anomaly sound has a visible source. It is a 3D World source placed off the trail, behind cover.
3. It stops when the player gets within anomalyStopDistance, or looks straight at its position (within anomalyLookAngle for anomalyLookSeconds). It does not fade; it stops mid-sound.
4. It never uses a jump-scare level. Same loudness as the ordinary sound it imitates.

Four kinds, built from sounds already in the game so they need few new files:
1. Missing: a location's signature sound is absent on a day the site is marked safe.
2. Displaced: a signature sound plays from the wrong place (the dock chain in the forest).
3. Altered: a known sound is off by a little (the axe beat skips one stroke; the day-end tone a semitone flat).
4. Echo: the player's own footsteps repeated a beat late, from behind, for four or five steps.

Which kind fires on which day comes from the event data (Milestone 13 format), not from a random roll in audio code.

## 6. Silence at the Ward **[Grant yes]**

1. From the last bend of the climb (trigger volume) the Ward snapshot takes over: every bed and every distant source goes.
2. Not digital silence. A room tone stays: a very low, close, dead-air bed, mono, 11025 Hz, with the faintest pressure in it, like standing in a closed room. True zero reads as a bug on headphones.
3. What is left is the player: footsteps, clothing, breath. Player foley at the Ward is dry and close; the lowpass on World is set so it does not carry.
4. The fire is not heard at the Ward, though it is the only place that sees its base. That mismatch is the point.
5. Same at day and night. The Ward is the one place time does not change the sound.
6. The Ward's own voice (its hunger, the feeding, what an offering sounds like) is a separate spec after the Ward design. It will be built on top of this silence, sparingly.

## 7. The cave chant **[Grant yes]**

The cultist cave is hidden from the tower and not in the daily check. Sound is how the player learns it is there.

1. A low, wordless chant from inside the cave mouth. World, 3D, mono, loop, 60 s or longer with no obvious seam.
2. Wordless and processed (hum, drone, no language), so it does not break the no-voice-acting rule. Source: CC0 drone or in-house hum layered and pitched down. Vesper hears it before anything else is decided.
3. Heard further than a site signature (caveChantRadius, start 60 m, against 35 m for sites) so it reaches a trail the player already uses, but faint enough to doubt.
4. Day only by default. At night it stops. Open: whether it carries to the Ward climb on some nights. That is story, so Quill and Sable decide.
5. It never stops when looked at. It is not an anomaly; it is a place.

## 8. The day-end tone **[Grant yes]**

1. Plays once when the player confirms the report.
2. A single held tone, about 3 s, soft attack, slight tape wow, then the Day to Night snapshot transition starts under its tail (dayToNightSeconds, 8 s).
3. The same tone every day, so it becomes routine. Routine is what an Altered anomaly (section 5) can break.
4. Diegetic if DailyLoop.md puts a radio or receiver at the report desk: then it is World, 3D, from that object. Otherwise Music, 2D. I prefer diegetic: the day ending because someone received your report is colder than a score cue.

## 9. The fire closing in **[Grant yes]**

1. Fire loudness and brightness follow the WARD stat, not the day count, because runs have no fixed length (DECISIONS 2026-09-27).
2. As WARD falls from 12 to 1: fire rumble gain rises by fireGainRangeDb, the fire emitters' lowpass opens from fireLowpassAtFullWard to fireLowpassAtLowWard, far cracks get more frequent, birds thin to none.
3. Stats are only on screen at the end-of-day phase (DECISIONS 2026-09-28). This makes WARD audible all day without a HUD. Pim and Sable should check it does not undercut that decision.

## 10. AudioTuning fields (this doc)

dayWindGainDb, fireRumbleGainDb, fireGainRangeDb (start 6), fireLowpassAtFullWard (start 800 Hz), fireLowpassAtLowWard (start 3500 Hz), fireCrackIntervalMin / Max at full WARD (start 60 / 180 s) and at low WARD (start 15 / 45 s), birdChanceAtFullWard, nightCreakIntervalMin / Max, freezerOnSeconds (40), freezerOffSeconds (20), anomalyStopDistance (start 15 m), anomalyLookAngle (start 20 degrees), anomalyLookSeconds (start 1), caveChantRadius (start 60 m), siteSignatureRadius (35 m, from Main3 draft), campSignatureRadius (40 m), wardRoomToneGainDb, dayEndToneGainDb.

## 11. Waiting on others

1. Sable: Main3 map and DailyLoop.md. Then per-location ambience, the fire line position, the front zone footprint, the Ward climb trigger point.
2. Quill: whether the cave chant reaches the Ward at night; what the report is sent to.
3. Vesper: tone words for the chant, the room tone and the Voices treatment.
