# Pack shortlist for Main3 hero pieces

**DRAFT, 2026-09-28, updated 2026-09-29, Vesper.** Grant bought the six picks in section 6 plus a few more prop packs (DECISIONS 2026-09-29); the extra prop packs are not listed here and still need judging against Style.md. Alts and rejects below are not bought. Rook checks URP and 6000.3 fit before import. Every pack is judged against Docs/Design/Style.md.

How this was checked: each store page was fetched on 2026-09-28. Price, publisher, pipeline table, Unity version and file size are from the page. The store pages did not return their description or image text, so **contents, poly counts, scale and look are unverified** unless marked otherwise. Prices change with sales.

"Unity version" is the version the publisher lists, not a promise it works in 6000.3.24f1.

"Commercial use" (DECISIONS 2026-09-29: every asset licensed for commercial sale): **yes** means the store page, fetched 2026-09-29, states Standard Unity Asset Store EULA, which permits use in a commercial game; all six bought packs are licence type "Extension Asset" under that EULA. **unclear** means the licence was not checked; check before any purchase.

## 1. The Ward: carved standing stones

Need: three stones, tops up to about 12 m above the shelf, weathered granite, carved runes that can take a dim ember emission. Any pack stone will be scaled up 3 to 6 times; texel density (Style.md 4.2) is the main risk for all of them.

| | Pack | Publisher | Link | Price | Pipelines on page | Unity on page | Size | Commercial use |
|---|---|---|---|---|---|---|---|---|
| **Bought** | Menhir Stone Circle | Effigy GameWorks | https://assetstore.unity.com/packages/3d/props/menhir-stone-circle-315132 | $9.99 | Built-in, URP | 2021.3.45f1 | 1.1 GB | yes |
| Alt | Runestone Pack | Gamertose | https://assetstore.unity.com/packages/3d/props/runestone-pack-by-gamertose-152468 | $8.79 | Built-in, URP, HDRP | 2019.4.2f1, last update 2020 | 445 MB | unclear |
| Alt | Totem & Rune Stones Pack | Red Blue Pixel Studio | https://assetstore.unity.com/packages/3d/props/totem-rune-stones-pack-325812 | $29.99 | Built-in, URP, HDRP | 2021.3.43f1 | 3.1 GB | unclear |

1. Menhir Stone Circle: why: tall standing-stone shapes are the right silhouette, and "runes" is a page keyword. Risk: whether stones are carved is unverified; 1.1 GB suggests high-res textures to cap at 1024; if runes are baked into the albedo they cannot glow, and the rune emission becomes Rook's own shader work.
2. Runestone Pack: why: cheapest, rune-carved by name. Risk: old (2020), Norse runestones are usually short slabs, not towers; may read as a museum prop.
3. Totem & Rune Stones Pack: why: widest variety, carved stone. Risk: "totem" and "tribal" keywords point at a different culture; 3.1 GB; price.
4. Rejected: Rune Stones Package / 19+ Assets (PackDev, $9.99): page says Built-in only. Mossy Megaliths (Boom Fluff CG, $27.99): page says made with AI, and moss green is off palette.

## 2. Giant trees: 40 to 50 m, walk-around trunks 6 to 10 m, mesh colliders

Need: two hero trunks (Hollow Giant, Gate Tree; Main3.md rev 3, 4.1) and giants that read from the tower. Owned tree packs are ordinary pines (Main2 review). No store page found states a tree height; all scale is unverified.

| | Pack | Publisher | Link | Price | Pipelines on page | Unity on page | Size | Commercial use |
|---|---|---|---|---|---|---|---|---|
| **Bought** | Pure Nature 2 : Redwood | BK | https://assetstore.unity.com/packages/3d/environments/pure-nature-2-redwood-313097 | $25 (sale, list $50) | not shown on page | 2022.3.10 or later | 1.5 GB | yes |
| Alt | Big Tree Bundle | ALIyerEdon | https://assetstore.unity.com/packages/3d/vegetation/trees/big-tree-bundle-337336 | $39 | Built-in, URP, HDRP | 2022.3.62f1 | 1.6 GB | unclear |
| Alt | Dead Tree Pack | Jesse Mario | https://assetstore.unity.com/packages/3d/vegetation/trees/dead-tree-pack-239578 | $26.99 | Built-in, URP, HDRP | 2021.3.15f1, update Feb 2026 | 955 MB | unclear |

1. Pure Nature 2 : Redwood: why: redwood is the right species for a giant conifer trunk; 110+ assets per a search summary (unverified) would also give roots, logs and stumps. Risk: URP support is claimed only in a third-party search snippet, not on the page; described as stylized, which may clash with the tape look; the publisher recommends an instancing plugin for its demo scene.
2. Big Tree Bundle: why: pipelines stated, recent. Risk: keywords say "realistic", "Speedtree", "mobile"; species unknown and may be broadleaf, not conifer; most expensive.
3. Dead Tree Pack: why: the Gate Tree and the cliff-edge band can be dead or burnt giants, and a dead trunk scales up with less visible texture stretch than foliage. Risk: covers only the dead heroes, not living giants; "stylized" keyword.
4. Also seen: Old Massive Tree (App Mechanic, $7, pipeline not shown, 2020.3); Burned Trees Package (Rispat Momit, $25, 2017, pipeline not shown); Giant Redwood Tree is deprecated and cannot be bought. Commercial use unclear for all three.

## 3. Burning ridge and fire backdrop

Need: fire and smoke that read at 300 to 500 m through haze, plus a ridge profile that is not cones. The current HorizonFire glow and smoke are on the keep list; a pack adds, it does not replace. None of it shows on day one before nightfall (Style.md 6.0).

| | Pack | Publisher | Link | Price | Pipelines on page | Unity on page | Size | Commercial use |
|---|---|---|---|---|---|---|---|---|
| **Bought** | Fire & Smoke - Dynamic Nature | NatureManufacture | https://assetstore.unity.com/packages/vfx/particles/fire-explosions/fire-smoke-dynamic-nature-217775 | $16.50 | Built-in, URP, HDRP | 2021.2 to 6000.3.9f1 | 284 MB | yes |
| Alt | Free Fire VFX - URP | Vefects | https://assetstore.unity.com/packages/vfx/particles/fire-explosions/free-fire-vfx-urp-266226 | Free | URP | up to 6000.0.23f1 | 8.9 MB | unclear |
| Alt | Environment Fire VFX | MalkoVFX | https://assetstore.unity.com/packages/vfx/particles/environment/environment-fire-vfx-266411 | $19 | URP | 2022.3.62f2 | 32 MB | unclear |

1. Fire & Smoke - Dynamic Nature: why: the only one listing a 6000.3 version; publisher makes nature environments, so large-area fire and smoke is its likely use. Risk: whether it has large-scale wildfire effects or only local fires is unverified. It ships its own shaders; those are replaced (Style.md 4.5), so Rook keeps its meshes, textures and particle settings and puts them on the project's shaders or Unity's built-in URP Particles shader.
2. Free Fire VFX - URP: why: free, cheap test of whether a pack beats our HorizonFire. Risk: effects are probably close-range; bright stylised flames need retinting.
3. Environment Fire VFX: why: page lists campfire, torch, flames and smoke; useful for the camp fire pit and site fires too. Risk: small-scale by description.
4. Ridge profile: Background Mountains Pack - 14 Mountains (MetaStudio, $13, https://assetstore.unity.com/packages/3d/environments/landscapes/background-mountains-pack-14-mountains-151579, pipeline not shown, 2017.2, 1.3 GB, commercial use unclear) or Free Background Mountain (MetaStudio, free, Built-in only, commercial use unclear). A backdrop only needs the mesh; its material is replaced with our unlit haze shader, so Built-in only is acceptable here. My view: shape the ridge in terrain first and buy only if it still reads as cones.

## 4. The cultist cave

Need: a natural cave mouth, and an inner chamber that shows human use: carving, candles, a made place. Owned Campsite pack has a Rocks and Stones folder; kitbash the mouth from that first.

| | Pack | Publisher | Link | Price | Pipelines on page | Unity on page | Size | Commercial use |
|---|---|---|---|---|---|---|---|---|
| **Bought** | Catacombs - Retro Style Modular Environment Pack | Revolving Pizza Games | https://assetstore.unity.com/packages/3d/environments/catacombs-retro-style-modular-environment-pack-388272 | $17.99 | Built-in, URP, HDRP | 2022.3.62f2 | 5.7 MB | yes |
| Alt | Low Poly Cave Pack - Polyworks | Off Axis Studios | https://assetstore.unity.com/packages/3d/environments/polyworks-cave-pack-54613 | $25 | Built-in, URP | 2020.3.34f1, latest listed 6.0 | 36.5 MB | unclear |
| Alt | Cave Environment / 68+ Assets | PackDev | https://assetstore.unity.com/packages/3d/props/cave-environment-68-assets-271214 | $39.99 | Built-in, URP, HDRP | 2022.3.7f1 | 4.1 GB | unclear |

1. Catacombs: why: same publisher and retro texture style as the owned Campsite and Cabin packs, so it matches; carved underground rooms suit the cultists' chamber behind a natural mouth. Risk: it is built masonry, not rock; whether it has altars or ritual props is unverified.
2. Polyworks Cave: why: natural cave shells, updated to Unity 6. Risk: Polyworks is flat-shaded bright low poly; likely clashes with the owned textured packs.
3. PackDev Cave: why: page lists stalagmites, stalactites, a fireplace and animal bones, which fits a lived-in cave. Risk: 4.1 GB, realistic PBR; texture cap needed; price.
4. Also seen: Abandoned Ruins - Retro Style Modular Environment Pack (Revolving Pizza Games, $29.99, all three pipelines, 2022.3.62f1, https://assetstore.unity.com/packages/3d/environments/abandoned-ruins-retro-style-modular-environment-pack-372742, commercial use unclear). Contents unverified; if it holds standing stones it could serve both the cave and the Ward in the owned style.

## 5. Front zone: office, small store, parking lot, road, fence

Owned first: the PSX Modular Complete Pack (Celestia Studio) has Building_Parts, Wall_Parts, Roof and a Marketplace_Assets folder with checkout counters, retail shelves, freezers and packaged food (checked in the project). That covers the store interior and both building shells. Gaps: parking lot, fence, road, vehicles.

| | Pack | Publisher | Link | Price | Pipelines on page | Unity on page | Size | Commercial use |
|---|---|---|---|---|---|---|---|---|
| **Bought** | PSX Edition - Modular Parking Lot | FANNΞC | https://assetstore.unity.com/packages/3d/environments/urban/psx-edition-modular-parking-lot-326617 | $9.99 | Built-in, URP, HDRP | 2022.3.21f1 | 13 MB | yes |
| **Bought** | Modular Chain Link Fence | PolyPlex | https://assetstore.unity.com/packages/3d/props/exterior/modular-chain-link-fence-252145 | $5 | URP | 6000.0.0f1 | 6.6 MB | yes |
| Alt | Low Poly Streets and Cars Pack | AndreyPopU | https://assetstore.unity.com/packages/3d/vehicles/low-poly-streets-and-cars-pack-292375 | $10 | Built-in, URP, HDRP | 6000.0.27f1 | 2.2 MB | unclear |
| Alt | Low Poly Retro Cars | Retropolia | https://assetstore.unity.com/packages/3d/vehicles/low-poly-retro-cars-251164 | $6.43 | Built-in, URP, HDRP | 2021.3.19f1 | 7.6 MB | unclear |

1. Parking Lot: why: PSX-labelled, small, matches the owned PSX look. Risk: contents unverified (lines, kerbs, lamps, a booth?).
2. Chain Link Fence: why: the map edge needs a fence that reads at distance; modular; lists Unity 6. Risk: "realistic" keyword; wire mesh may alias or shimmer badly at 360 rows, so it may need a solid-backed variant.
3. Streets and Cars: why: road pieces and cars in one cheap pack, lists Unity 6. Risk: likely flat-colour city style, off palette until retinted.
4. Retro Cars: why: one or two parked cars at the store or lot. Risk: "racing car" keyword; may be too sporty.
5. Road: no pack needed. A ProBuilder or terrain-painted strip with a CC0 asphalt texture from ambientCG fits the existing texture rule (DECISIONS 2026-09-22) at no cost. CC0 counts as commercial use yes.
6. Rejected: Vintage Gas Station (Monkey punk, $29): page says Built-in only. Old Fuel Gas Station (MartinFernandez, $24.99): page says HDRP only. PSX PROPS v1 (Leksii, $6.50): Built-in only.

## 6. Bought set

| Need | Pack | Price paid | Commercial use |
|---|---|---|---|
| Ward | Menhir Stone Circle | $9.99 listed | yes |
| Giant trees | Pure Nature 2 : Redwood | $25.00 listed | yes |
| Fire | Fire & Smoke - Dynamic Nature | $16.50 listed | yes |
| Cave | Catacombs - Retro Style | $17.99 listed | yes |
| Front zone | PSX Edition - Modular Parking Lot | $9.99 listed | yes |
| Front zone | Modular Chain Link Fence | $5.00 listed | yes |
| **Total** | | **$84.47** listed before tax; price actually paid not recorded | |

Plus a few more prop packs Grant bought (DECISIONS 2026-09-29), names not yet given to me; each needs a fit and licence check against Style.md before use.

## 7. Answered and open

1. Answered (DECISIONS 2026-09-29): shaders shipped in bought packs are replaced. Unity's built-in URP shaders already used across the project count as allowed; only shaders shipped in packs are excluded (Style.md 4.5). The fire pack is used for meshes, textures and particle settings; Rook puts them on project or built-in URP shaders.
2. Open: names of the extra prop packs, so I can judge them.

Vesper
