# Day and night sound

**DRAFT, 2026-09-29, Hollis. Nothing here is decided.** Lines marked **[Grant yes]** need Grant's confirmation and a dated line in DECISIONS.md. Mixer groups and snapshots: Mixer.md. Import settings and sources: Sourcing.md. Binding inputs: DECISIONS.md 2026-09-28 lines on the daily loop, two states, night at the Ward, the cave, the first day as a normal job; 2026-09-29 lines on the day ending by report or event, the fire not visible on day one, the rave cave, and the slow-burn ramp where every sound that drops out is replaced by a creepy one; 2026-09-29 lines on the gate minigame (option B) and gate cars arriving until the office resident's storyline ends. DailyLoop.md section 6 (day 1 and night 1). Per-location ambience waits for Sable's Main3 map; nothing here fixes a position.

Tone target: psychological first. Day one is ordinary and full. From day two the world is never quiet (DECISIONS 2026-09-29): each ordinary sound that leaves is replaced by a wrong one, subtle while WARD is high, stronger as it falls (section 15). A jump is punctuation.

## 1. Two states, two ways into night

| State | From | To | Snapshot |
|---|---|---|---|
| Day | Waking | Report confirmed, or an event ends the day | Day (Day1 on day one) |
| Night | Report confirmed, or event end | Sleep | Night (Night1 on night one) |

1. The sun is fixed, so sound does not follow a clock. Nothing drifts through the day; the change is one event.
2. Report path: the day-end tone and a slow transition (section 9).
3. Event path: a hard cut, no tone (section 11).

## 2. Day one: an ordinary job **[Grant yes]**

Day one is Firewatch (DECISIONS 2026-09-28). No fire is seen (2026-09-29), so none is heard. The ear should believe this is a normal summer posting. Everything below is what later days take away.

Own snapshot, Day1. Beds (Ambience, 2D stereo, loops) and sources (World, 3D, mono):
1. Wind in the giant trees, same as later days but with more movement at ground level: leaves, branches, a warmer layer.
2. Birds: full and close. Several species, near and far, including close birdsong on the trails and at camp. This is the one day the forest is fully alive.
3. Insects: a daytime layer at the lake and forage ground (flies, a bee pass as a one-shot).
4. Distant road traffic: a car or truck passing on the far road, one-shots on a 3D emitter row along the road, long random interval (roadPassIntervalMin / Max, start 90 / 240 s). Doppler off; a slow gain and lowpass sweep inside the clip does the pass. Heard from the front zone and faintly from camp. Ordinary, never close.
5. Office radio: ordinary dispatch chatter. World, 3D, from the radio object, battery device rules (hiss, squelch, tone bursts). No voice acting (DESIGN.md), so the voices are band-limited, pitched and garbled past understanding: the rhythm of talk, not words. Built in-house from processed hums and mouth noise, or CC0. Vesper hears it first.
6. Residents' signature sounds all play, all on time. No anomaly (DailyLoop.md 6.1).
7. Front zone machines run (section 5).
8. No fire rumble, no fire cracks, no smoke hiss. Nothing from the west.
9. The cave is not heard. The cave music emitter is disabled on day one (section 8).
10. Not decided here: DailyLoop.md 6.2 still describes an ordinary distant fire seen from the tower on day one. That line predates DECISIONS 2026-09-29 and conflicts with it. This doc follows 2026-09-29: no fire sound at all on day one. If Grant keeps a far smoke column, it stays silent.

## 3. Night one: the reveal **[Grant yes]**

The one sound moment the whole run is built on. The player walks up in an ordinary night and arrives at the fire.

Climb (Night1 snapshot, from the report to the last bend):
1. Ordinary night. Crickets and night insects, one owl, a soft wind. Tree creak one-shots. The office radio is off. A single far car on the road, once, early in the night.
2. Nothing hints. No rumble under it, no low end. The climb must sound safe.

Ledge (triggered when the view opens, one timeline, numbers in AudioTuning):
1. The ordinary night bed cuts out, not fades (revealCutSeconds, start 0.1 s). The insects stopping all at once is the first wrong sound in the game.
2. A beat of Ward room tone and the player's breath (revealHoldSeconds, start 1.5 s). Nothing else.
3. The runes wake: the Ward's own voice, one low swell (spec waits for the Ward design, section 7.6).
4. The fire arrives: the full roar, 3D on a wide emitter row along the fire line below, opened up to full bandwidth, with the far cracks and a deep sub layer. Rise over revealRiseSeconds (start 4 s) to revealRoarGainDb. It is the loudest sound the player has heard so far.
5. The roar presses against the barrier: a heavy lowpass and a slight flutter on the fire bus where it meets the Ward line, so it reads as held back, not just far.
6. The first Ward screen opens over the roar. The roar ducks under it (wardScreenDuckDb, start -6 dB) and stays.
7. Leaving the ledge: the roar stays audible down the climb and into camp, lower. The ordinary night bed does not come back.

## 4. Day bed from day two (Ambience, 2D stereo, loops)

1. Wind high in the giant trees. Slow, far overhead, little at ground level. 60 s loop or longer so the seam is not learned.
2. Far fire: a low rumble from the fire front. A row of wide 3D emitters along the fire line on Sable's map (mono, Linear rolloff, large min distance), not 2D, so turning your head places the fire. Loudest at the three west glimpses (DailyLoop.md 5.4).
3. Far fire cracks: one-shots on the same emitters, a giant tree splitting far off. Random interval.
4. Birds: few, far, and fewer as WARD falls (section 12). No close birdsong. The forest has mostly left since night one. The gap is filled by rows 1 and 2 of section 15.
5. Insects: none by day. The gap is filled by row 3 of section 15.
6. Ash: nothing. Ash is silent. Resist adding a sound for it.

## 5. Night bed from night two (Ambience)

1. The night bed is the day bed with things swapped, not a new bed. The ear should notice what stopped and what took its place (section 15).
2. Wind drops to a near-still layer with tree creak one-shots (World, 3D, on a few giants near trails). The creaks are the replacement for the wind (section 15 row 6).
3. No birds. The night-one insects do not return; a replacement insect layer does (section 15 row 5). The fire rumble keeps its level.
4. Resident signature sounds stop at night (the axe, the canvas chimes, the radio), except where an event says otherwise. **[Grant yes]**
5. Front zone machines keep running (section 6).
6. Night allows only the Ward (DECISIONS 2026-09-28), so the night bed is heard on the camp, the trail to the Ward and the climb.

## 6. Front zone machines and wild sites **[Grant yes]**

Rule: mains electric sound exists only in the front zone (road, fence, parking lot, store, office). Everything past it is hand, animal, water, wind and fire.

Front zone (World, 3D, mono, loops unless noted):
1. Store: chest freezer compressor cycling on and off (on about 40 s, off about 20 s, AudioTuning). The click at the start and end of each cycle is the useful part.
2. Store or office sign: fluorescent buzz with an occasional flicker tick.
3. Office: window AC unit rattle.
4. Road: power line hum on the poles, heard only standing under them.
5. Gate: chain knocking on the post in wind (one-shots).
6. Road traffic, two separate things:
   1. Through-traffic on the main road beyond the fence follows the slow-burn ramp: cars pass normally through the first week (section 2.4 numbers), gaps lengthen in week two (roadPassIntervalMin / Max scale up by roadGapScaleWeek2), and the last car passes when WARD first enters 3 to 5. The road is never silent: section 15 row 4 fills the lengthening gaps and replaces the passes after the last car.
   2. Gate arrivals (section 6a): not road ambience. They keep coming until the office resident's storyline ends, and do not thin as WARD falls (DECISIONS 2026-09-29).
7. Office radio: ordinary chatter on day one only. From day two, static with a rare fragment of chatter, fewer fragments as WARD falls, replaced as in section 15 row 7.

Wild sites (lake, campsites, cave): no hum, no compressor. Battery devices are allowed because they hiss and crackle, they do not hum.

Machines run day and night at the same level. They do not care what time it is. At night they are the loudest man-made thing on the map.

Early era: no mains power. Front zone replacement is open until the Early era is designed.

## 6a. Gate arrivals (gate minigame) **[Grant yes]**

DECISIONS 2026-09-29: gate minigame option B; cars keep arriving until the office resident's storyline ends (completed, or he dies); they do not thin out as WARD falls.

1. The engine idling at the barrier is the signal that a car has arrived (Pim). That cue is reserved: while the minigame runs, an idle at the barrier always means a real car. No replacement, anomaly or ambience in this doc may use an engine idle at or near the gate.
2. Sequence, all World, 3D, mono, one emitter that moves with the car: approach on the road (one-shot, gain and lowpass sweep, no Doppler), idle at the barrier (loop, until the player acts), then either admitted (pull away, tyres up the gravel spur, fading toward the campground loop behind the chain) or refused (turn on the gravel, pull away down the road).
3. Arrival sounds stay ordinary at every WARD band. They are exempt from the section 15 ramp. The contrast is the point: the only normal cars left come to the booth, while the road beyond turns wrong around them.
4. Arrival timing comes from the minigame's event data, not from roadPassInterval.
5. When the office resident's storyline ends, arrivals stop for good: no approach, no idle, no gravel spur, from the next day on (the car in progress, if any, finishes). The gap is filled by section 15 row 10, at the band WARD is in at that moment. Completed and died use the same sound rule; whether they should differ is Quill's call.

AudioTuning fields: gateIdleGainDb, gateApproachGainDb, gateSpurGainDb.

## 7. Silence at the Ward **[Grant yes]**

1. From the last bend of the climb (trigger volume) the Ward snapshot takes over: every bed and every distant source goes.
2. Not digital silence. A room tone stays: a very low, close, dead-air bed, mono, 11025 Hz, with the faintest pressure in it, like standing in a closed room. True zero reads as a bug on headphones.
3. What is left is the player: footsteps, clothing, breath. Player foley at the Ward is dry and close; the lowpass on World is set so it does not carry.
4. At the Ward the fire is heard only on the ledge at night, held back behind the barrier (section 3.5). This replaces the earlier rule that the fire is never heard at the Ward: 2026-09-29 puts the first sound of the wildfire there.
5. By day the Ward is silent apart from room tone, even though it sees the fire's base. At night the ledge carries the held-back roar. The climb stays silent both ways.
6. The Ward's own voice (its hunger, the feeding, what an offering sounds like) is a separate spec after the Ward design. It will be built on top of this silence, sparingly.
7. The Ward is the one place built on near-silence. It is a place, not a dropout along the ramp, so I read DECISIONS 2026-09-29 as allowing it. It still follows the ramp: section 15 row 9 adds a layer under the room tone as WARD falls. If Grant wants no near-silence anywhere, this section changes.

## 8. The rave cave music **[Grant yes]**

The cultist cave is hidden from the tower and not in the daily check. Sound is how the player learns it is there. DECISIONS 2026-09-29 makes it a rave cave with lights and music. Quill suggests music replaces the chant outright; pending Grant. The 2026-09-29 line "the cave chant starts on night one" would then read "the cave music starts on night one".

1. Music playing inside the cave. World (diegetic, not the Music group), 3D, mono, loop, 60 s or longer with no obvious seam.
2. Distance does the work: far off, only the kick and bass carry, a thump through rock and trees (lowpass on the emitter falls with distance, caveMusicLowpassFar, start 150 Hz, to caveMusicLowpassNear, start 8000 Hz at the mouth). The player hears a beat in the forest before they know it is music. That is the wrongness, not a grim drone.
3. Off on day one. From night one on (DailyLoop.md 5.5) it carries to two trail spots at two volumes, rising down the spur (caveMusicRadius, start 60 m, against 35 m for sites), faint enough to doubt.
4. Day and night from night one. Whether it reaches the Ward climb at night is story, so Quill and Sable decide.
5. It never stops when looked at. It is not an anomaly; it is a place.
6. Source: Grant's music from the book, an Envato Elements track, or CC-BY (Sourcing.md section 1). No vocals with words unless Grant waives the no-voice-acting rule for music; a vocal track would need its own line in DECISIONS.md.
7. Whether the music changes with WARD (slows, detunes) is open for Vesper and Quill. I would keep it steady: the party does not care about the fire, and that is the joke.

## 9. The day-end tone **[Grant yes]**

1. Plays once when the player confirms the report. Not played when an event ends the day (section 11).
2. A single held tone, about 3 s, soft attack, slight tape wow, then the Day to Night snapshot transition starts under its tail (dayToNightSeconds, 8 s).
3. The same tone every day, so it becomes routine. Routine is what an Altered anomaly (section 10) and an event end can break.
4. Diegetic if the report desk has a radio or receiver: World, 3D, from that object. Otherwise Music, 2D. I prefer diegetic: the day ending because someone received your report is colder than a score cue.

## 10. Anomalies heard, not seen

None on day one (DailyLoop.md 5.2). From day two, serves Safety events and general dread. Hearing an anomaly is never required: the tower marks locations SAFE or not, and spotting as a skill belongs to a minigame (DECISIONS 2026-09-28). Sound here is atmosphere and a pull, never a gate.

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

Day one sounds are the reference the player compares against. A Displaced car pass on the main road (after the last car, 6.6.1) is available as a late, low-WARD anomaly. No anomaly uses an engine idle at the gate (6a.1).

Which kind fires on which day comes from the event data (Milestone 13 format), not from a random roll in audio code.

## 11. An event ends the day **[Grant yes]**

DECISIONS 2026-09-29: an event can end the day, for example a monster that catches the player, who then finds themselves at the Ward early. The monster and chase sound are a separate spec. This is the transition only.

1. The chase runs at its peak: pursuer, the player's breath and footsteps, the day bed ducked.
2. At the catch: one short impact or grab one-shot (World, 2D, close), then a hard cut of every bus to nothing (eventCutSeconds, start 0 s). A true zero here is correct, unlike the Ward: it is short and it is meant to read as something taken.
3. Black and silence for eventBlackSeconds (start 2 s). This is punctuation, not a dropout; it lasts seconds. Flag for Grant against DECISIONS 2026-09-29; if he wants no zero at all, a low ringing tail replaces it.
4. Wake at the Ward: room tone first, then the player's breath, ragged and slowing over eventBreathSeconds (start 6 s). Then the Ward's own voice or, at night, the held-back roar (section 7.4), faded in rather than revealed.
5. No day-end tone. The routine sound is missing, and that is the tell that the day was taken, not ended.
6. State goes straight to Night; no Day to Night transition plays.
7. Day one cannot end this way, since day one has no events that threaten. If Quill writes one, it would collide with the reveal; section 3 wins.

## 12. From day two: the fire closing in **[Grant yes]**

1. Everything in sections 4, 5, 6.7, 8.3, 10 and 15 switches on from day two. Day one's birds, insects and chatter do not come back; their replacements (section 15) take their places. Through-traffic thins on its own ramp (6.6.1). Gate arrivals (6a) are not part of any ramp.
2. Fire loudness and brightness follow the WARD stat, not the day count, because runs have no fixed length (DECISIONS 2026-09-27).
3. As WARD falls from 12 to 1: fire rumble gain rises by fireGainRangeDb, the fire emitters' lowpass opens from fireLowpassAtFullWard to fireLowpassAtLowWard, far cracks get more frequent, birds thin to none, radio fragments thin to pure static. Each thinning is matched by its replacement rising (section 15), so the total bed never gets emptier.
4. Weirdness bands (DailyLoop.md 5.4) for sound, proposed: WARD 9 to 12 as section 4; 6 to 8 the fire rumble reaches camp by day; 3 to 5 the one wrong sound a day becomes likely and the Altered kind is unlocked; 1 to 2 the room tone of the Ward leaks into camp at night. Vesper to check.
5. Stats are only on screen at the end-of-day phase (DECISIONS 2026-09-28). This makes WARD audible all day without a HUD. Pim and Sable should check it does not undercut that decision.

## 13. AudioTuning fields (this doc)

dayWindGainDb, fireRumbleGainDb, fireGainRangeDb (start 6), fireLowpassAtFullWard (start 800 Hz), fireLowpassAtLowWard (start 3500 Hz), fireCrackIntervalMin / Max at full WARD (start 60 / 180 s) and at low WARD (start 15 / 45 s), birdChanceAtFullWard, day1BirdDensity, day1InsectGainDb, roadPassIntervalMin / Max (start 90 / 240 s), roadGapScaleWeek2 (start 2.5), radioChatterIntervalMin / Max, radioFragmentChanceAtFullWard, nightCreakIntervalMin / Max, revealCutSeconds (start 0.1), revealHoldSeconds (start 1.5), revealRiseSeconds (start 4), revealRoarGainDb, barrierLowpassHz, wardScreenDuckDb (start -6), eventCutSeconds (start 0), eventBlackSeconds (start 2), eventBreathSeconds (start 6), freezerOnSeconds (40), freezerOffSeconds (20), anomalyStopDistance (start 15 m), anomalyLookAngle (start 20 degrees), anomalyLookSeconds (start 1), caveMusicRadius (start 60 m), caveMusicLowpassFar (start 150 Hz), caveMusicLowpassNear (start 8000 Hz), replacement fields in section 15.5, siteSignatureRadius (35 m, from Main3 draft), campSignatureRadius (40 m), wardRoomToneGainDb, dayEndToneGainDb.

## 14. Waiting on others

1. Sable: Main3 map. Then per-location ambience, the fire line position, the road emitter row, the front zone footprint, the Ward climb trigger and the ledge reveal trigger.
2. Wren or Grant: DailyLoop.md 6.2 (distant fire seen on day one) against DECISIONS 2026-09-29.
3. Quill: whether the cave music reaches the Ward at night; what the report is sent to; whether any day one event can end the day; whether the cave music changes with WARD.
4. Vesper: tone words for the day one radio chatter, the cave music, the room tone, the reveal, the Voices treatment and each replacement in section 15; tone of the week-two road gaps.
5. Grant: music replacing the chant in the cave (Quill's suggestion); whether the Ward near-silence (7.7) and the 2 s event zero (11.3) are allowed under the no-quiet rule.
6. Pim: confirm the barrier idle is the only arrival cue and that reserving it (6a.1) fits the minigame. Quill: whether the completed and died ends of the office storyline should sound different at the gate (6a.5).

## 15. Replacements: nothing drops out without something wrong in its place **[Grant yes]**

DECISIONS 2026-09-29: the slow-burn ramp stands, but the world does not go quiet. Each ordinary sound that leaves is replaced by a creepy one. Early replacements must pass as ordinary on a first listen; they get stronger as WARD falls. All are built from sounds already in the game where possible, so they cost few new files.

Bands follow section 12.4: High is WARD 9 to 12, Mid 6 to 8, Low 3 to 5, Last 1 to 2.

| # | What drops out (when) | Replaced by: High | Mid | Low | Last |
|---|---|---|---|---|---|
| 1 | Close birdsong (day two) | One far bird call, the same sample at the same interval, never varied. Reads as a bird until the third repeat. | The call comes from two places at once, a beat apart. | The call is pitched down and cut off before its end. | No call. Wingbeats overhead with no voice. |
| 2 | Far birds thinning (as WARD falls) | Nothing extra; row 1 covers it. | A single crow-like caw held a little too long. | Caw answered from the direction of the fire. | Rows 1 and 2 go to wingbeats only. |
| 3 | Day insects at lake and forage ground (day two) | One fly circling the player that never lands. Ordinary. | The fly stops when the player stops. | A dense buzz off trail, behind cover, from something unseen. Never visited, never found. | The buzz follows at a fixed distance. |
| 4 | Through-traffic on the main road (thins from week two, last car at WARD 3 to 5, 6.6.1). Not gate arrivals (6a). | Week one: normal passes, nothing replaced. Week two, in the longer gaps: a far truck on the main road climbing through its gears. It never gets closer. | In the gaps: a pass sweep that stops at its loudest point, holds, then cuts. | The last car passes, once. After it: tyres on gravel along the fence far from the gate, stopping. No door, no engine. | A single car horn from the road, held, then cut. Once per run at most. |
| 5 | Night insects (night two, after night one's cut) | One cricket, near. Ordinary. | Several crickets chirping in perfect unison. | The unison chirp stops when the player moves and resumes when they stop. | The chirp tempo matches the player's footsteps. |
| 6 | Ground-level wind at night (night two) | Tree creak one-shots (section 5.2). | Creaks fall into a slow rhythm, like weight shifting. | Two creaks answer each other across the trail. | A creak directly above the player, then nothing for a count, then again. |
| 7 | Office radio chatter (day two) | Static with rare garbled fragments (6.7). | Fragments repeat the same garbled rhythm each time. | A carrier tone under the static, and something breathing on the channel. | The radio plays the day-end tone (section 9), a semitone flat, unprompted. |
| 8 | Resident signature sounds at night (5.4) | Night is Ward only, so the player is at camp or on the Ward trail. From camp: one far signature sound (the axe, the chimes) once, early, where the site is out of range by distance. | Same, later in the night. | Same, closer than it could be. | Nothing from the sites; see row 9. |
| 9 | Every bed at the Ward (section 7) | Room tone only. | Room tone with a faint slow pressure swell, like breath held in a closed room. | The swell has a pulse under it. | The Ward room tone leaks into camp at night (12.4); at the Ward the pulse is audible without headphones. |
| 10 | Gate arrivals (the day after the office resident's storyline ends, 6a.5) | The chain on the gate post knocks as if brushed, with no wind in the bed. | A car approach starts far down the road, as it used to, then stops before it arrives. No idle. | The booth's window slides or the barrier creaks with nobody there. | Nothing from the gate itself; the road (row 4) carries on. |

Rules:
1. A replacement plays at the same loudness as what it replaced, never louder. Stronger means stranger, not louder.
2. High-band replacements must be deniable. If Vesper hears one as wrong on first listen, it moves to Mid.
3. Rows 1 to 8 and 10 are World, 3D, mono. Row 9 is Ambience, 2D, mono.
3a. No replacement uses an engine idle at or near the gate (6a.1). Row 10 Mid is an approach only and must stop well short of the barrier.
4. These are the bed, not anomalies. They do not count against the one wrong sound a day (section 10) and do not stop when looked at. Row 4 Last and row 7 Last are rare one-shots and are the exception: once per run each, driven by event data like anomalies.
5. AudioTuning fields: replaceBirdIntervalSeconds, replaceFlyGainDb, replaceRoadGainDb, replaceGateGainDb, replaceCricketGainDb, replaceCreakIntervalMin / Max per band, radioCarrierGainDb, wardPressureGainDb per band, wardBandThresholds (9, 6, 3, 1).
6. Night one keeps its reveal cut (3.1): the insects stop and the fire roar replaces them. That is the first replacement and the model for the rest.
