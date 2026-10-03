# Old burn, forage and the Jg and T trails: what sound needs from the layout
Hollis, 2026-10-02. **Draft** for 8.28; Sable places, Grant decides. Layout needs only. Positions are scene coordinates (x, z) from Marlow's 828_Marlow_ground.md; "m" is metres along his scene markers (Camp to Jg m 0 is P16 (184.4, 153.5), about 16 m from the camp centre). Sound behaviour is in Sound/Locations.md 2, 4, 9 and 13 (draft); this file adds emitters E65 and E66, the E67 interior zone and the Gate Tree reflection (rewrite due when sound resumes after Milestone 11). Unity features named are unverified for 6000.3.24f1 until Rook checks.

## Telling the legs apart by ear
Each leg has its own order of sounds. The layout need is that each one sits where stated and nothing else speaks between them.

1. **Camp to Jg** (home leg). Vane behind, fading out at m 15 (60 m slant from the roof). Knoll bed edge at m 11. Then the Hollow Giant moan (E56, max 25 m) from below on the right, m 0 to 52, loudest at m 24 to 30. Then the bed turns to Burn where the live giants on the south side end (item 3). Then forage A (item 12). Road faint ahead the whole way (item 9).
2. **Hollow Giant moan, E56:** emitter at the centre of the opening, at whatever height Sable builds it, not at the trunk centre. The opening faces the trail (Quill 17), so the moan comes from the trail side. No other emitter within 30 m of it.
3. **Burn bed edge on Camp to Jg:** one place where the giants stop overhanging the trail from the south and regrowth stands on both sides. Expected near x 230 (m 58); Sable places it from the layout. It must be a single crossing, not a fringe that comes and goes, so the canopy hiss drops out once. It must sit at least 5 m past the end of the LOOP-LEG stretch if that stays on this leg (item 21).
4. **Jg to T** (supply leg). Burn bed from Jg. No emitter until the Gate Tree. The Gate Tree stub gives a close slap of the player's own steps from the right, m 30 to 40 (item 15). The road grows ahead the whole leg, 166 m at Jg to 88 m at T. Front bed edge at x 320 (m 80): fence tick, open wash. Office hum from m 96. Nothing on this leg makes a sound of its own except the front: it is the quiet open leg.
5. **Jg signpost knock, E65 (optional, second priority):** one loose arm knocking in the wind, one-shots, signKnockIntervalMin / Max (start 20 / 60 s), max 15 m, at the post (265, 169), y about 2.2. Tells Jg by ear from 15 m on all three legs, at a chase pace too. Layout: the post stays where it is; no other wooden prop within 15 m of it. The knock does not change when the arms are turned (T7): the sign stays ordinary to the ear.
6. **Jg to Camp 1.** Burn bed from Jg, road behind and fading. The live edge going north (Quill 19) is the bed edge back to Forest: canopy hiss returns. It must cross the trail once, by m 37 (z 200 now, about m 36), at least 5 m before the latrine (m 42) where the pots first sound (E11). Bed change, then pots: two beats, not one. The Burn zone's north edge follows that live edge.
7. **Camp 2 to T.** Forest bed. Stack wind behind (E13) to m 39. Front bed edge at x 320 at m 41, by the food lockers. Road grows from 130 m to 88 m. Told from Jg to T by the canopy hiss, the stack behind you, and reaching the front at half way instead of near the end. No layout change needed.
8. **No sound on the burn legs from snags.** The burn snags near Camp to Jg (Quill 15) and the Gate Tree have no emitters by day or night. Silent props stay silent, so nothing marks one snag out from the others.

## The road heard from the burn
9. Road row E33 on x 428, by day only, never at night. Distances from the trails (horizontal): camp 258 m, Hollow Giant stretch 225 m, forage A 184 m, Jg 166 m, Gate Tree 138 m, front edge (m 80 Jg to T) 108 m, T 88 m. roadPassMaxDistance 270 m: faint on all of Camp to Jg, clear by T.
10. The burn is where the road is heard best before the front. Proposed behaviour: roadZoneOffsetDb, Burn +2 (matches its fire offset: open sky), Forest 0. Walking home, the road drops a step at the burn edge (item 3); walking out, it lifts there.
11. Layout need: on Jg to T from m 50 to the front edge, open sky above the regrowth to the east, toward the highway. No open-east clump (8 to 20 m trees, Valley 6) within 40 m east of the trail there. The sound comes from a bearing the eye reads as open.

## Forage patches
12. **A (239.6, 163.0):** in the Burn bed, open sky. Day-one insects E6 snap to the bushes' centre, 1 m up, max 20 m; heard on Camp to Jg m 52 to 102. Nearest bush within 1.5 m of the tread edge (today the centre is 4.3 m off the centreline), so the pick sound plays from the bush in front of the hand, not from behind the regrowth.
13. **B (140.75, 167.12):** forest, shade, a rotting log (Quill 10). E7 snaps to the log as placed (Locations has (142, 164)). The log sits off the walking line, 1 to 1.5 m from the tread edge, so it is never stepped on or hopped (no landing sound at a food stop).
14. **C (229.5, 273.5) as built;** Valley E13 says (225, 266). Sable or 8.24 settles it. C has no insect emitter today; add **E66**, day-one insects at C, max 20 m, same file as A and B, in the light gap at the fir wall foot. All three patches carry the same insect layer whether they bear or not: sound never tells the player if a patch bears. The visual read at 10 m stays the only tell (Quill 8).

## Echo and dead spots
15. **Gate Tree stub, the one reflection on Jg to T.** A close slap of player foley only, one early reflection: gateTreeEchoDelayMs (start 12; about 2 m to the face and back), gateTreeEchoGainDb (start -14), zone along the trail m 30 to 40 where the trail runs 1.5 to 2.5 m from the stub's face. Layout: trail stays within 2.5 m of the face for at least 8 m (it does today); no regrowth or brush between trail and stub on that stretch; no other trunk of radius 2 m or more within 20 m of the trail there, so this is the only wall on the leg. Same unverified mechanism as the cleft echo (Sound/Valley 8).
16. **Hollow Giant interior.** A small enclosed room (Quill 17): close, dry, one short reflection when the player stands inside, nothing outside. Layout: the trunk closed all round but the opening; inner space no wider than 2.5 m; the floor a separate collider so it can take its own surface (soft rot and ash: ForestFloor) and a SoundZone volume inside the trunk only. The moan (E56) is the wind over the opening, so it cuts while the player stands inside.
17. **The open burn is the dead place.** No reflections anywhere in x 230 to 320 but the stub (Sound/Valley 3: Burn off). Layout: no rock face, wall or structure over 3 m tall within 20 m of Jg to T or the burn half of Camp to Jg. Snags are thin and do not count.
18. **The drop south of Camp to Jg** (x 181.5 to 208.5, z 136 to 145.5, up to 8.3 m, 2.5 m from the trail): no emitter in it, no echo from it. The player stands on its lip; open air below, nothing comes back.

## Footsteps
19. **Ash on Camp to Jg from day 2 (Quill 18).** I would add a tenth surface, AshTrail (soft, dry, a faint hiss of fine dust), switched on as a SoundZone over the tread from day 2 (AudioTuning 3.2 step 2). Layout: one continuous painted trail tread of steady width from the knoll edge to Jg, so the zone can follow it; no mesh with a SurfaceSound on the tread (it would win over the zone). Needs Vesper's yes on a burn surface (AudioTuning 4.2) and Grant.
20. **Chase legs (Quill 13):** no hop obstacles on either leg, so a sprint never throws a landing sound. Trail-edge stones stay off the tread; any stone with a collider within 0.5 m of the tread edge either loses its collider or moves out.

## What LOOP-LEG needs from the ground
21. One stretch of 20 to 30 m (8 to 12 s walking) on one leg. Inside it, all of these hold:
    1. **One bed zone the whole way.** No zone edge, no SoundZone, no reverb or trigger volume starts or ends inside it. On Camp to Jg: start at m 16 or later (past the knoll edge, m 11, plus margin, and past the vane at m 15); end at least 5 m before the Burn bed edge (item 3).
    2. **One footstep surface the whole way.** Painted trail only. No stone, log, root or plank with a collider on or within 0.5 m of the tread. Today Camp to Jg has CS_Stone_3 at (197.67, 144.46) with a convex collider, 1.58 m from the centreline: check it against the tread edge.
    3. **Steady grade, no hop.** No step or edge that makes a landing sound. Camp to Jg m 16 to 40 falls 12.0 to 9.1 and rises to 10.8: smooth, fine.
    4. **The marker post (Quill 16) is silent:** no emitter, no SurfaceSound, beside the tread, not on it.
    5. **Every emitter in range is a fixed point or replayed.** On Camp to Jg the stretch sits inside E56's 25 m. That is fine if E56 joins the bed in the replay (ScareSound, private). If Rook cannot replay a 3D loop's position, the stretch must sit wholly outside E56 (m 53 or later), and the burn edge margin (item 3, about m 53) leaves no room on this leg.
    6. Sound needs nothing else from the layout.

## Conflicts with Quill's list
22. **Quill 16, the tower over the Hollow Giant stretch.** Marlow's table 5 (terrain alone): all 128 deck eyes see every Camp to Jg marker. The tower is west-north-west of the stretch (about (164, 166)); the Hollow Giant stand (208, 130) is south-south-east. The stand cannot be what hides the tower there. Sable checks, but I expect (a) fails. Of Quill's (b), I would take the north loop between forage C and the ruin: one Forest bed, no creek, no signature in range (pots 63 m away). W1 to Camp 3 has creek loops C1 to C7 along it, each one more thing to replay.
23. **Quill 2, the firebreak as the burn's hard west line.** Locations 2 starts the Burn bed at x 230, not the firebreak, because Camp to Jg runs the burn's south edge under live giants. Keep it that way: the firebreak gets no sound edge. If the bed changed at the firebreak (knoll foot, about m 26), it would fall inside the vane's tail and inside any LOOP-LEG stretch on this leg.
24. **Quill 18, ash footsteps.** AudioTuning 3.1 note 1 has the burn on ForestFloor and no ash surface. Item 19 is what I would do instead of a terrain repaint: a day-state SoundZone on the tread.
25. **Locations 4.2 says the Burn bed starts "at forage A (240, 163)";** the zone itself starts at x 230, 20 m earlier (m 58). Item 3 replaces both: the edge is where the south giants end, as built.

## Tuning fields (AudioTuning, on approval)
26. signKnockIntervalMin / Max (20 / 60 s), signKnockMaxDistance (15), roadZoneOffsetDb per zone (Burn +2, Forest 0), gateTreeEchoDelayMs (12), gateTreeEchoGainDb (-14), surface AshTrail. New emitters: E65 Jg sign knock, E66 forage C insects, E67 Hollow Giant interior zone (SoundZone, not an emitter).

Hollis
