# Rebuild specs for Rook's batch (Vesper, 2026-10-01)

All values **P**. Recapture with the noise band off; grade per item, before and after frames side by side. Lighting items in section 4 each get their own A/B pair so one change is judged at a time.

## 1. Forest read (Style 5.8)
1. Emergent, height: keep the top at 48 to 50 m; drop the rest of its grove to 28 to 34 m tops (a 16 to 20 m step, up from 12 to 18).
2. Emergent, crown: 1.5 times the widest crown in its grove (14 to 18 m across), ragged; dead spike 4 to 6 m above the live crown, bleached #8A8070. The pale spike against dark canopy is what reads from 57.6 m.
3. Emergent, value: live crown tinted darker than the grove (#3A3826 against #4F4A2C), so it reads as a dark mass with a pale tip.
4. Emergent, ring: inside 15 m of its trunk, no crown within 20 m of its top. Inside 10 m, no fir (as now).
5. Emergent, siting: on the tower-facing edge of the grove or on a rise; one per grove, never two in one view line from the tower.
6. Gaps: no open olive ground over 30 m across outside the lake, named clearings, camps and a 3 m trail buffer. Fill with an irregular grove (Style 5.8) or, where it must stay open, brush, scree, snags and logs at 1 per 25 m2 in 2 or 3 clusters.
7. Burn: no straight edge over 20 m. Edge band 20 m wide: 100 percent snags inside, 50/50 snags and live at the band, 20 percent snags 20 m out. Ash layer blended over 10 m, not cut.
8. Crests: clumps of 3 to 7, heights mixed by plus or minus 30 percent, 15 to 40 m between clumps, set 5 to 15 m off the crest line on both sides. Never more than 3 trees in a line. One snag per two clumps. North bench: clumps on ledges plus brush; no bare face over 40 m wide.
9. Past the highway: two bands. Near band 80 to 150 m from the road, real trees 25 to 45 m tall in clumps, no more than 4 equal heights in a row. Back band 250 to 400 m, silhouette only, paler and cooler (backdropNearHaze 0.55 to 0.7).

## 2. Ground on the open lot
Pick: a different owned texture. SoilPine_a (BK) as the open-lot base at a 6 m tile; it is one even needle-brown with no green blotches, so there is nothing to checker. GrassPine_a only as painted patches, 20 to 40 m irregular blobs, 30 percent weight at most. Pull SoilPine toward dull olive-brown with the layer tint (target on screen near #5E5440); unverified that the URP terrain shader honours the layer remap, Rook checks. GrassMud_a rejected: same red and green blotches. Tufts and stones per Style 4.7 within 10 m of the road.

## 3. Cave (Style 2.3: fill #4A5058, light #9AA3AD, no glow)
1. Chamber: one roof crack with a cold shaft. Spot light #9AA3AD, 25 degree cone, range 14 m, aimed at the floor 2 m off the table. Ambient fill #4A5058 inside the cave volume only. Target in greyscale: lit floor patch 90 to 110, walls 25 to 45, corners under 15. No shadows needed from it.
2. Mouth: two BK BigBoulders as leaning jambs, one BigBoulder as an overhang 1.5 m proud of the face. Opening 3 to 4 m wide at the floor, 2 to 2.5 m high, narrower at the top. No straight edge over 1 m. The box wall set 2 m behind the mouth so the void hides it. Scree and brush at the foot.
3. At 20 m the overhang must read as the top band; the mouth as a dark irregular hole, not a doorway.
4. Side room: boulders sit on the floor or walls; ceiling is rock resting on the walls (Style 5.4).

## 4. PLAN 8.18 fire and night (Edges 6, Style 6.2 and 6.3)
1. Ledge fire, night one: remove the red backing rectangles. Flame cards in clusters of 3 to 9, cluster widths and gaps irregular, heights plus or minus 40 percent, tops about 6 degrees above level, front across 154 degrees. Colours base #FFE8C0, body #FF6B1A, tips fade through #B8481C. Core is the brightest value in the frame.
2. Ledge fire, day: flame on no more than a third of the width at once; emission half the night value; smoke carries the view (Edges 6).
3. Smoke: night one a low sheet streaming west, underside #6B2A12 along its whole length (smokeFireStrength 1.0 to 1.6) or it reads as fog. Day two on: 3 to 5 columns 200 m and up, merging into a #4A3A32 roof, lit below.
4. Sky glow: #5A2412 at the western horizon, fading to night black by 30 degrees up, over the full front width.
5. Lit valley floor: under each flame cluster a soft unlit glow card on the ground, 2 to 3 times the cluster width, #8A3A14, no hard edge. The floor reads as a burning sea between clusters, never a flat orange field.
6. Night horizon: sky band #0C1016 from 0 to 8 degrees on every side but the west, #05080D above. Crests and far trees at or under #05080D. Pass: greyscale step of 6 or more between crest and sky after the filter. If crushBlacks eats it, raise the band to #141A24; never lighten the fog.
7. Lamp (PlayerTuning): lampIntensity 20 to 6, lampRange 8 to 9. Unverified that this alone softens the disc edge; if the edge stays hard, Rook reports what the falloff is.
8. No blowouts: pale rock, plank and boulder albedo capped at #A89C88. Pass: nothing above 230 greyscale at 2 m from the lamp except the lamp glass.
9. Gold halo (day one asset): sunGlowColor #C8A070 to #E0A848, sunGlowSize 80 to 40. Day two stays #FF8C40, 24. No bloom.
10. Carried night items: J cairn lamp a small flame in glass, not a disc; Camp 3 capsule solid; chain-link mid grey, not bright panels.
11. Frames: pair Ward night and day, ledge west level and 10 degrees down, J_to_Ward night 200 to 240, Grey_Trails_Night, Compass night, J_to_Ward FWD 240 and 250 day.

Vesper
