# Map edges

**DRAFT revision 2, 2026-09-29, Vesper. Grant: "Make sure the edges of the map are beautiful." Nothing here is decided until it is a line in DECISIONS.md and Grant has confirmed it. All values P; Rook sets LookTuning and builds. Written for Sable's Valley.md and Valley_map.svg revision 2 (G items not yet approved); section 7 says what changes without the valley.** Palette: Style.md 2. Layer spec: Main3.md 2.11.

## 1. Rules for every edge
1. The skyline is land, never a terrain end. Every terrain or backdrop mesh runs at least 150 m past its crest, so no ray from a walkable eye (floor, tower 57.6 and 58.2, platforms, Ward) ever meets a mesh border, a flat plane or the void. The tower eye (57.6) is above the N and S saddles (50) and the whole E ridge (45 to 60): it looks down their back slopes, so those back slopes must fall to an outer floor that runs out to the far range. Valley_map rev 2 ends the ridge land 20 to 30 m past the N, S and E crests: fails until extended.
2. No crest is a straight line. Each ridge has one saddle, one rock knob and a broken tree line on top; crest height changes at least 10 m every 200 m.
3. Each side reads different at a glance: N forested and high, S bare granite over water, E low and cut by the road, W the dark wall with the fire behind it.
4. Players are stopped by something they can see: rock band, deadfall, thicket, fence, cliff. Any collider sits 2 to 4 m behind that object, never on open ground. No message except the fence during a shift (Main3.md 3.1.4).
5. Value steps back: foreground darkest, near ridge mid, anything through a saddle palest. Backdrop _HazeBlend: near ridge 0.35 to 0.45, far range 0.8.
6. Night: fog (8 to 60 m) swallows the ridges, so the skyline is kept by the sky, not the land. Sky horizon at night #0C1016 on N, S, E, and on W for night one (section 5), one step above #05080D, so crests read as black cutouts. Ridge backdrops use night colour #030406 and are not fogged.
7. Day one: no smoke, glow or ash at any edge, including a thin flat band above the west crest (Style.md 8.13).
8. Check E-1 (proposal for Rook, added to Marlow's walk recipe): from the tower deck grid, every platform and every trail at 10 m steps, rays every 2 degrees of bearing at 0 to 5 degrees above level and 0 to 5 degrees below; each must end on terrain, backdrop or sky above a crest.

## 2. North (Valley.md 2: crest z 350, 100 falling to 50)
- Skyline: the high forested crest. Saddle 50 at x 250, the one view out to the far range from the tower. Rev 2 has a second notch (80 at x 90) next to the knob (90 at x 120), so the knob reads as a bump between two dips. Fix: x 90 to 84, knob to 94, bare rock. Raising only hides more; no sightline depends on x 90.
- Layers: floor forest, the flank climbing in giants and firs, crest tree line against the sky. The far range (150 to 220 at 1.2 to 1.8 km) shows only through the saddle, from the tower and platforms.
- Day one: front-lit by the SSW sun (section 8), the flank the brightest land on the map, crest trees gold (#FFC98A) at the tips, sky above #5E6878. Day two: the flank in rust haze (#9E5C38), in the west ridge's shadow at its foot. Night: black tree-line cutout on #0C1016.
- Closure: flank steepens into a rock band with deadfall (RedwoodHollowLog, BigBoulders) at the foot. The campground loop backs onto it.
- Reference: https://commons.wikimedia.org/wiki/File:Blue_ridge_Mountains_layered.jpg

## 3. South (Valley.md 2: crest z -50, 95 at the west hook falling to 52)
- Skyline: bare granite shoulders (#6E6660) with sparse firs; saddle 50 at x 170, knob 70 at x 240 over the boathouse. Meets rule 1.2.
- Layers: open water, reed-and-rock shore, dark shore trees, the grey ridge face, sky. The lake is the widest sky on the map; this is the postcard view.
- Day one: the sun stands over this ridge (bearing 200), so the face is backlit and in its own shade, the crest rimmed, the lake carrying the glare (reflection unverified; Water shader). The shadow reaches about 90 m north of the crest: the south shore strip, the ravine and the cave corner stay in shade (right for the cave). Day two: ridge flattened by rust haze to one mid value, sky orange at the horizon. Night: ridge a black band, water a faint second horizon if the shader shows the sky; otherwise black.
- Closure: the lake bank and a rock band past the boathouse; the wade limit stays (Main3.md 3.5.1). The ravine side ends against the W ridge foot, never open ground.
- Reference: https://commons.wikimedia.org/wiki/File:Bierstadt_Albert_Sunset_in_the_Yosemite_Valley.jpg (the lit ridge over still water)

## 4. East (Valley.md 2: crest x 445, 49 m past the fence, road cut at z 170)
- Skyline: the lowest crest, broken by the road cut, a clean V. Through the cut, the far range (150 to 200 at 1.5 to 2.5 km): the only way out, framed and out of reach. The tower sees over the whole ridge, so the land behind it (rule 1.1) matters most here.
- Rev 2 south half (z -40 52, z 60 55, z 120 50) is flat: fails rule 1.2. Fix: z -40 45, z 60 57, z 120 47. The knob (60 at z 250) stays the highest point.
- Layers: chain-link, the road dropping into the cut, cut walls in raw rock, crest forest, the pale range in the notch. The mast and red lamp stand in front of it.
- Day one: lit from the SSW on its west face, the range in the notch lightest (#A8A08E). Day two: rust haze fills the cut; the range is a ghost. Night: black crests on #0C1016.
- Headlight (agreed with Sable, Valley.md 6.2): night one only, one car passing in the cut that never turns in, timed for the look-back from P3 and P4. It reads as a warm glow sweeping the cut walls, never the car or its lamps (the cut floor is likely under the giants' tops from P3; Marlow checks). After night one the cut stays dark at night: the missing light is the wrong detail.
- Closure: the fence (Modular Chain Link Fence pack) is the wall; the road leaves through the gate and curves out of sight inside the cut within 150 m, so its end never shows.
- Reference: https://commons.wikimedia.org/wiki/File:A_view_of_the_Blue_Ridge_mountains.jpg

## 5. West from the floor, tower and platforms (Valley.md 2: crest 95 to 108, Ward knob 115)
- Skyline: the tallest, darkest wall; the Ward knob the one feature, a bare rock ridge. The W saddle (95 at z 60) is only 5 m deep and will barely read; accepted, because the knob carries this skyline and the saddle is load-bearing for the sightline.
- Day one: the wall in its own shade (sun at bearing 200 grazes it from behind), a dark mass under a warm pale sky; the brightest sky is SSW, not over this crest. Nothing behind it.
- Night one: the crest a black cutout on #0C1016, no glow, no smoke (agreed with Sable, Valley.md 6.1: the smoke streams west and the reveal is the step out of the cleft). This replaces Style.md 2.2 "first seen at nightfall on day one" for the valley; Style.md edit waits for Grant.
- Day two on: smoke stands over the crest, three to five columns merging into a brown roof (#4A3A32), lit orange below; glow on the sky above the crest, never flame. Width 90 degrees or more from the tower (Style.md 6.3.7). The floor sits in the wall's shadow all day (sun 6 degrees west): intended.
- Night two on: the crest a hard black cutout on #5A2412, lit smoke undersides (#6B2A12) above it.
- Closure: the valley-side face is steep rock; walkable ground ends in scree and boulders at its foot. The crest is reached only by the Ward climb.
- Reference: https://commons.wikimedia.org/wiki/Category:Smoke_from_wildfires

## 6. West from the Ward ledge (night only, ground 98)
- Night one (agreed with Sable, Valley.md 6.1): the burning valley below (floor -40), the far front across 154 degrees, flame tops about 6 degrees above level. Smoke a low lit sheet streaming west, under 100 near the crest rising to about 10 degrees above level far out; underside lit #6B2A12 or brighter along its whole length, or it reads as grey fog. Style.md 6.3.2, 6.3.3 and 6.3.5 are rewritten for night one only: the frame test counts the whole frame at level gaze (fire below, sheet above), not only the sky. Rewrite waits for Grant.
- Night two on: the wind turns; columns stand and lean east over the crest toward the map (Style.md 6.3.3 as written); brighter, wider as WARD drops.
- Closure: the ledge lip and the cliff. The valley floor and far side have no colliders; their meshes run 500 m past the last flame so no edge shows behind the fire.
- Reference: https://commons.wikimedia.org/wiki/File:Bierstadt_Albert_Sunset_in_the_Yosemite_Valley.jpg

## 7. If the valley is not approved (Main3.md rev 16 as built)
- N, S, E: the near layers stay off-map bands (crest 35 to 60, 120 to 300 m out) behind the 60 to 100 m edge forest strip and skirts; the far range shows over them from the floor. Section 2 to 4 skylines and night rules hold; closure is deadfall and thicket at the map line, not a rising flank. Sun as Style.md 5 (west, 28 per 8.9g).
- W: no crest 95. The cliff at x 10 and the cliff-edge giant band close the edge; the far ridge (crest 30) is the skyline. Day two glow and smoke show over the band as now. Section 6 holds, with the Ward at 70 and smoke leaning as Style.md 6.3.3.

## 8. Day-one sun in the valley
- Sun from bearing 200 (SSW), elevation 32. Unity: directional light rotation X 32, Y 20 (points NNE). Colour #FFC98A as Style.md. P; replaces "west, 28" for the valley only.
- Why: the W crest stands about 93 m over the floor (ground 12). A west sun would need about 52 degrees to light Camp 3 (x 78); that is noon, not late afternoon. At bearing 200 and 32 the W ridge shadow reaches about x 57: Camp 3, J and the west trails east of the foot are lit; the ridge's own flank is not. At 28 it reaches about x 66; too tight.
- Cost: the S ridge shades about 90 m north of its crest (shore strip, ravine, cave). The sun is no longer behind the W wall on day one; day two (6 degrees, west) puts it there, which suits the change of day.
- HorizonGlow and SkyGradient day-one warm band follow bearing 200 if they have a direction (unverified).

## 9. Minimum for Grant's gray walk (no dressing)
1. Crest profiles exactly as Valley.md 2 with the N and E fixes above: the silhouette is what Grant will judge.
2. Back slopes on N, S and E to at least 150 m past each crest, falling to an outer floor about 0 to 10; the existing far range backdrop beyond, so the saddles and the whole east show land, not void (rule 1.1).
3. West: terrain to x -40 plus a flat floor at -40 out to a gray far ridge at 30 near x -300 to -500, if the walk reaches the ledge. Otherwise the ledge looks into the void.
4. Visible stops: gray rock blocks or scree at every ridge foot where a collider sits; no invisible stop on open ground.
5. Day-one sun per section 8; no smoke, no fire, no night, no trees on crests.
6. E-1 run on the gray terrain, with its failures listed, before the walk.

## 10. Assets
- Owned, found in Assets: BK Pure Nature 2 Redwood (Sequoia1 to 5, RedFir1 to 8, RedPine1 to 5, BigBoulders_0 to 5, Boulder_0 to 5, RubbleDense and RubbleSparse, RedwoodHollowLog), suffercord PSX Autumn Forest (Pine1 to 6, Bush1 to 4, Aspen and Birch Leafless for snags), Modular Chain Link Fence, NM Fire & Smoke (Prefab_Fire_Huge_Flames_01; the _Blue variant is forbidden), project shaders Backdrop, SkyGradient, HorizonGlow, Smoke, FlameCard.
- Gaps, unverified: no mountain, cliff face or crest tree-line card in any owned pack; ridges are terrain plus Wall-style meshes. A cutout tree-line strip for far crests would be new project shader work. No road surface prefab found in the Parking Lot pack (only Road_Post, Road_Conus). Night sky #0C1016 needs SkyGradient to hold a separate night horizon; unverified.

Vesper
