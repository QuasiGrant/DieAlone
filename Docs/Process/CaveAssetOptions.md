# Cave and cliff rock packs: option 1 for 8.27a (Vesper, 2026-10-03)
Looked at only; nothing bought; Grant buys. Facts are from each Asset Store page on 2026-10-03. Every Asset Store pack is under the Standard EULA, which passes Style.md 4.8. Where a page does not state something, it says **unverified**. No page I read lists a poly count, and most list no texture size, so those are from secondary listings where I name them, or unverified. Grant: check the "Package Content" tab before buying.

## What we need
- Cave walls, ceilings and openings that join, so the 8.8 boxes can be skinned or replaced.
- Cliff faces for the ravine walls and the Ward cliff.
- It must sit beside BK PureNature boulders, which are realistic PBR and get our cave tint, under the VHS filter (Style.md 3, 4). Low-poly is fine: the filter eats detail. Flat-shaded or bright stylized is not: it clashes with BK at 2 m.
- Pack shaders get replaced with URP Lit (Style.md 4.5), and textures are capped at 512 (4.1). Any pack's look is judged after that.

## Candidates

| # | Pack | Publisher | Price | URP | Unity 6 | Poly / texture | Fit |
|---|---|---|---|---|---|---|---|
| 1 | [Realistic Cliffs and Rocks](https://assetstore.unity.com/packages/3d/environments/landscapes/realistic-cliffs-and-rocks-251490) | Highpoly Forehead | $30 | listed compatible | **unverified** (built on 2022.3.62f3; updated 2026-06-02, v1.2) | tagged "lowpoly", PBR; counts and texture sizes **unverified**; 362 MB | Best match to BK: realistic PBR cliffs and boulders with mossy variants. Cliff faces cover the ravine and the Ward cliff. Cave interiors only by placing cliff pieces inward, as with BK boulders but much larger. That fixes the box read; no purpose-built ceiling or opening pieces |
| 2 | [Modular cave kit](https://assetstore.unity.com/packages/package/id/146549) | Lukebox | $8.99 ($4.99 on sale per gameassetdeals) | "standard shaders", URP **unverified** | "all versions" per the third-party listing, **unverified** on the store page | under 300 tris per piece, 1024 textures, colliders and LODs included (third-party listing) | The only one built for what failed: interlocking walls, ceilings, branches and openings with no gaps. Low poly suits the filter. Texture style **unverified** from the page; if it is hand-painted it will need retexturing with our rock material. No cliffs |
| 3 | [Low Poly Cave Pack - Polyworks](https://assetstore.unity.com/packages/3d/environments/polyworks-cave-pack-54613) | Off Axis Studios | $25 | listed compatible | **unverified** (built on 2020.3; updated 2023-01-25) | about 765 verts and 362 tris average over 403 meshes (search summary of the page); texture **unverified** | Many cave pieces, but flat-shaded, stylized low poly with crystals and ores. Clashes with BK and reads as a fantasy mine. Reject on look |

Looked at and rejected:
- [High-Poly Cliffs and Rocks Pack](https://assetstore.unity.com/packages/3d/environments/high-poly-cliffs-and-rocks-pack-229866) (Ravibio, $20). Unity 6000.0.23f1 and URP listed, but 3.5 GB of high-poly assets: wrong budget for the filter.
- [Cave System](https://assetstore.unity.com/packages/3d/environments/landscapes/cave-system-32024) (MotuProprio, $40). Last updated 2017; URP and Unity 6 unverified.
- [Low Poly Rocks & Cliffs Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-rocks-cliffs-pack-397808) ($15.99). Unity 6 and URP listed, but it is a 1.3 MB stylized, mobile-friendly kit.
- [Retro PSX Modular Survival House](https://assetstore.unity.com/packages/3d/environments/retro-psx-assets-modular-survival-house-263892) ($4.99). Its "cave tunnel" contents are not described, and PSX vertex-lit style is off-brand while jitter stays off.

## Ranking and pick
1. **Realistic Cliffs and Rocks (pick).** It solves both needs with one material family that matches the BK rocks we already use. Large cliff pieces placed inward hide the boxes in the cave, the ravine and the Ward cliff. Risk: Unity 6 unverified, so buy, import into a scratch project and check for pink materials before it enters DieAlone.
2. **Modular cave kit**, only if Grant wants true modular tunnels rather than skinning the 8.8 boxes. It is cheap and the right shapes, but no cliffs, and its look is unverified until imported.
3. Polyworks: rejected on look.

Before either goes in, I judge the imported pack in a test frame (filter on, tinted, URP Lit, 512 cap) against Style.md 10, as with any pack asset.

Vesper
