# Shader swap

**DRAFT, 2026-09-29, Vesper. Nothing here is decided until it is a line in DECISIONS.md and Grant has confirmed it.** Rook builds; I review the result with a Game view screenshot per material group.

Binding inputs: DECISIONS 2026-09-20 (own shaders only), 2026-09-29 (bought pack shaders replaced with the project's own); Style.md 4.5 ("own" = project shaders plus Unity's built-in URP Lit, Unlit, Particles; pack shaders excluded; Wren's reading, pending Grant); PLAN.md Rules and Tips, PACK SHADERS TO REPLACE.

Project shaders available today (Assets/Shaders): DieAlone/SkyGradient (skybox), DieAlone/HorizonGlow (additive fog-proof glow strip), DieAlone/Water (dark water, glint, fresnel), LookFilter (post, not a material shader).

## 1. Summary

Material counts are from a scan of the pack .mat files on 2026-09-29.

| Pack shader | Materials using it | What it does | Replace with | Must survive | Accepted loss |
|---|---|---|---|---|---|
| BK/Standard Layered | 18 (6 hollow logs, 12 boulders) | Lit, albedo/normal/mask, second layer (moss) blended top-down or by vertex colour | URP Lit | Albedo, normal, silhouette; granite #6E6660 and wood/char tints | Moss layer |
| BK/Vegetation Trunk | 3 (Sequoia branches, RedFir trunk, RedPine trunk) | Lit bark, second layer by vertex colour, trunk wind | URP Lit | Bark normal and albedo at giant scale; one bark material (4.4) | Wind, second layer |
| BK/Vegetation Leaves | 3 (Sequoia, RedFir, RedPine leaves) | Alpha-cut foliage, translucency, height gradient, colour-variation noise, two wind layers | URP Lit, Alpha Clipping on, Render Face Both | Cut-out silhouette, olive #4F4A2C range, dark underside | Translucency, wind, per-tree colour noise |
| BK/Impostor | 18 (\*_Imp, far LOD cards) | Cross-plane tree cards, fades planes seen edge-on (Hide Sides), same leaf tint and wind | URP Lit, Alpha Clipping on, Render Face Both | Tree silhouette at distance, same tint as the near LOD | Edge-on fade, wind (see 2.4) |
| BK/Grass | 11 (grass, ferns, clovers, moss, fungi, dead leaves, branches) | Alpha-cut ground cover, two-colour noise tint, wind, depth fade | URP Lit, Alpha Clipping on, Render Face Both | Cut-out shape, dulled olive, never grass green (2.4.4) | Wind, two-colour noise, depth fade |
| BK/Sky | 1 (Sky_Redwoood) | Procedural day/night sky with stars | DieAlone/SkyGradient | Nothing; the project sky owns this | All; never used |
| BK/Clouds | 2 (Clouds_Swirly, Clouds_Overcast) | Scrolling noise cloud layer | None | Nothing | All; the smoke sheet (6.3.8) roofs the sky, not clouds |
| BK/Water | 1 (Water) | Tessellated waves, refraction, caustics, foam, teal/blue | DieAlone/Water | Nothing from the pack | All; its teal and blue are forbidden (8.1) |
| BK/Standard Layered Masked | 0 | Layered lit, mask mode | None | Nothing | Not in the PLAN list of 8; no material uses it |
| NM Fire Unlit | 6 (fire flipbooks big, small, torch, blue; embers; ashes) | Additive HDR flipbook flame, optional wind bend, soft edge at intersections | URP Particles Unlit, Blend Additive, flipbook from the Texture Sheet Animation module | Flipbook motion; additive glow; core #FFE8C0 as the brightest value (6.3.4) | Shader wind (use the particle Noise and Velocity modules) |
| NM Smoke Lightmap Lit | 28 | Six-way lightmapped smoke: R/G/B/A = right/left/top/bottom light, second map front/back/emission/alpha | DieAlone/Smoke (new, section 3) | Underside lit by fire #6B2A12, dark body #4A3A32, soft alpha | Full six-way response to moving lights |
| NM Smoke Lightmap Unlit | 10 | Same, unlit | DieAlone/Smoke | As above | As above |
| NM Smoke Normalmap Lit | 10 | Smoke lit through a normal map; colour mask R, emission B, alpha A | DieAlone/Smoke | As above | Normal-map lighting |
| NM Smoke Normalmap Unlit | 10 | Same, unlit | DieAlone/Smoke | As above | As above |
| NM Lava Emissive | 0 | Lit emissive lava particle; file names it NatureManufacture/HDRP/Particles/Lava Emissive | None | Nothing | No material uses it |
| Catacombs C_Candle_Fire, C_Torch_Fire | 2 | Legacy Particles/~Additive-Multiply, pink in URP | URP Particles Unlit, Blend Premultiply | Flame texture, flicker, warm #FFA860 | Exact legacy blend (see 2.7) |

Totals: 9 BK shader files (8 in use), 6 NM shader files (5 in use), 2 Catacombs materials. Fourteen shaders in use need an answer; two unused ones need none.

## 2. Notes per group

1. **Swap in place.** Change the shader on the existing pack material and remap its textures, so every pack prefab keeps working. Do not duplicate materials into committed folders with pack textures inside; pack content stays git-ignored (Assets/SOURCES.md).
2. **Lit remap (Standard Layered, Trunk, Leaves, Impostor, Grass).** Albedo to _BaseMap, main colour to _BaseColor retinted to Style.md 2.1, normal to _BumpMap. The BK mask texture is Metallic (R) Occlusion (G) Smoothness (A); URP Lit reads metallic R and smoothness A from _MetallicGlossMap and occlusion G from _OcclusionMap, so the same texture goes in both slots. Metallic 0 on everything organic. Smoothness at most 0.2; nothing in this forest is wet or glossy.
3. **Wind is gone from all vegetation.** I accept it. Still air under a smoke roof reads as hush, which is on tone (1.1). If Grant wants sway, it is a new project shader, not a pack one.
4. **Impostors.** Without Hide Sides, a cross-plane card shows a thin sliver when seen edge-on. At 360 rows and in haze this may not read. Test: tower cab view west and east, filter on. If slivers show, either raise the impostor LOD distance until fog hides it, or write DieAlone/TreeCard (cut-out lit card with view-angle fade). Try LOD distance first.
5. **Colour variation lost.** Leaves and grass lose per-instance tint noise. Fix with two or three material variants per species, placed in clusters, not one tint map-wide (5.8).
6. **Fire.** The blue flipbook (M_fire_flipbook_big_01_blue) is rejected outright (8.1). Soft edges need URP soft particles, which need the depth texture on the URP asset.
7. **Catacombs flames.** Legacy Additive-Multiply adds colour and darkens behind by alpha; URP Particles Premultiply is the nearest built-in blend. Style.md 2.3 gives the cave no glow; these flames ship only where Grant places a practical light.
8. **Sky and clouds.** Nothing from BK/Sky or BK/Clouds enters any scene. Stars are forbidden by 6.2.1 (no moon, near-black sky).
9. **Emission.** Only fire and smoke underside carry emission (4.6). No emission on bark, rock or foliage.

## 3. New project shader: DieAlone/Smoke

The one case that needs a new shader. URP Particles cannot read the NM smoke textures: their RGB channels are lighting directions or masks, not colour, so the smoke comes out tinted wrong. Deriving greyscale copies would make new pack-derived files; the shader avoids that.

- Unlit, alpha blend, particle vertex colour, same pattern and size as HorizonGlow.
- Reads the pack smoke texture: one light channel as shade, A as alpha, B (emission) as the fire-lit mask.
- Colour = body colour (#4A3A32 **P**) lit toward the top by the sun, plus fire colour (#6B2A12 **P**) through the emission mask from below.
- One fog amount per material: far columns 300 to 500 m out must still read through haze (6.3.8), same reason HorizonGlow ignores fog.
- Serves all four NM smoke shaders. Values come from LookTuning, not hard-coded.
- Not visible on day one before nightfall (8.13): scene logic, not the shader, turns smoke off.

## 4. Done-check I will review

1. No material in any scene or prefab in use references a BK/*, NatureManufacture/* or Legacy Shaders/* shader. Judge by shader name, not the converter's return value (PLAN Rules and Tips).
2. No pink material in the Game view.
3. One Game view capture per group, filter on: a boulder and hollow log, a giant near and far (LOD and impostor), a grass patch, fire, smoke column from the tower. I check palette, silhouette and brightness order against Style.md.

## 5. Unverified

1. Which Editor API sets URP Lit keywords (normal map, alpha clip) after a scripted shader swap; Rook to confirm for 6000.3.
2. Whether the URP asset has the depth texture on (soft particles).
3. Whether the BK trees go in as Terrain trees or placed prefabs, and whether Terrain draws their LOD impostors correctly on URP Lit.
4. The exact blend of Legacy Particles/~Additive-Multiply in 6000.3.

## 6. Open for Grant

1. Confirm Style.md 4.5 reading: Unity built-in URP shaders count as the project's own.
2. Accept static vegetation (no wind), or ask for a sway shader.
3. Approve DieAlone/Smoke as the one new shader.

Vesper
