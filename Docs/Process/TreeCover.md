# Tree cover for the crest belt (8.14 prep; Rook, 2026-09-30)
Read-only. Measured with Tools/Recipes/tree_cover_8_14_measure.cs in an unsaved preview scene; Main3 untouched. Rev 7 is tagged main3-rev7.

## What the owned trees are
- BK Redwood firs, pines and sequoias: four LODs, fade mode None (hard pops), LOD3 is an impostor card. Screen heights 0.25 / 0.125 / 0.063 / 0.010 with lodBias 2, so a 20 m fir is LOD1 at 150 m, LOD2 at 300 m and the impostor at 600 m. No culling inside the map.
- Foliage is URP Lit, alpha cutout 0.5, texture mip "preserve coverage" off, so crowns thin with distance. No colliders on any tree (Main3's are GameObjects under SliceLook; the terrain holds no tree instances).
- The deck line crosses the W crest 7 m above the land (Valley.md rev 8 section 7), so the belt must block the lower crowns and trunks. The tower deck looks up at the crest, into the belt's thinnest part.

## Measured gaps (belt 25 m deep, band 2 to 20 m over its ground, 3840 x 1976, no filter)
| Spacing | Trees per 1000 m2 | 150 m | 300 m | 600 m |
|---|---|---|---|---|
| 4 m | 72 | 0.4 pct, widest 2.4 m | 0.6 pct, 2.3 m | 0.3 pct, 1.8 m |
| 6 m | 35 | 8.6 pct, 5.9 m | 8.5 pct, 5.6 m | 2.1 pct, 2.8 m |
| 9 m | 14 | 27 pct, 9.8 m | 29 pct, 10.1 m | 17 pct, 9.8 m |
- Rev 8's belt (about 262 trees over roughly 19,000 m2 of crest and hooks, my estimate) is about 14 per 1000 m2: the 9 m row. It fails the 2 m gap rule by about 5 times.
- Even 4 m spacing (about 1,400 trees) leaves 2.4 m gaps at 150 m. With the look filter on, the gaps count higher (the colour bleed spreads the background); treat those numbers as unverified.
- F-1 cannot count these trees. They have no colliders, and a MeshCollider on LOD0 would treat the alpha-cut cards as solid, so it would pass rays the player can see through. The only honest tree test is a render test (pixels on the flame).

## Options, best first
1. Raise the land under the W crest and hook belt (8.1 recipe) about 15 m where the deck lines cross it. Rev 8 puts the flame line at 72.8 and the smoke line at 78.3 over land at 66, so this clears the smoke line by about 3 m. F-1 then passes on land, the belt stays for the look, and the check stays a ray test. It costs ridge height the 2026-09-30 decision wanted to keep low.
2. A solid backing berm or rock fin, 15 m tall, inside the belt with a collider, in ridge colours. F-1 counts it as land. The trees hide it by day; at night check it against the lower crest cutouts.
3. Dense planting at 4 m or closer. That is about 1,400 trees and still 2 m gaps, and F-1 needs a render test instead of rays. Not recommended.
4. A colour-matched card behind the belt. It is flat, so it shows from the side and at night against the fire glow. Not recommended.
My pick: option 1 on the stretch the deck lines cross; option 2 only where raising the land changes a view the design needs.
