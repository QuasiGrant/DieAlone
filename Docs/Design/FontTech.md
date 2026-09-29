# Font tech: custom fonts on legacy Text vs TextMeshPro

**DRAFT, 2026-09-29, Rook. Research only. Nothing here is decided.** Binding inputs: DECISIONS.md 2026-09-20 (uGUI, built-in legacy Text font, no TextMeshPro because Essential Resources would be imported into Assets), 2026-09-27 (PC render scale 0.5, VHS filter downsamples). Style.md 7 (one font, sizes 22 to 36 at 1080 rows, 1 px shadow). DayOneSamples.md 4 and 111 (no strikethrough on legacy Text; pencil lines without a second face).

Tags: **[V]** verified this session (source named), **[U]** unverified.

## 1. What the project has today

1. `Packages/manifest.json` lists `com.unity.ugui` 2.0.0. No separate `com.unity.textmeshpro`. **[V]**
2. uGUI 2.0.0 CHANGELOG: "Merge of the com.unity.textmeshpro package." The TMP runtime is already compiled: `Library/ScriptAssemblies/Unity.TextMeshPro.dll` exists. So TextMeshPro needs no package added. **[V]**
3. Three scripts use legacy Text: `Assets/Scripts/Dev/DevMenu.cs`, `Assets/Scripts/Interaction/InteractPromptUI.cs`, `Assets/Scripts/UI/PauseMenu.cs`. DevMenu loads `LegacyRuntime.ttf` and builds a Screen Space Overlay canvas. **[V]** (grep)

## 2. Path A: custom TTF/OTF on legacy Text

Importer: `TrueTypeFontImporter` in 6000.3 has `fontSize`, `fontRenderingMode`, `fontTextureCase`, `includeFontData`, `fontNames`, `fontReferences`, `characterPadding`, `characterSpacing`, `customCharacters`, `ascentCalculationMode`, `shouldRoundAdvanceValue`. None marked obsolete. **[V]** (6000.3 Scripting API)

1. Character (`FontTextureCase`): Dynamic, Unicode, ASCII, ASCIIUpperCase, ASCIILowerCase, CustomSet. **[V]**
2. Dynamic: glyphs rasterised at runtime at the size actually drawn. With Include Font Data on, the TTF ships in the build. Use Dynamic for UI; static sets bake one size and go blocky when scaled. **[V]** for the enum and Include Font Data; blockiness from the 6000.3 "Create meshes for text strings" page.
3. Rendering Mode: Smooth, Hinted Smooth, Hinted Raster, OS Default. Hinted Raster is aliased, for pixel fonts. **[V]** enum exists; per-mode look **[U]** for this project.
4. Fallback: `fontNames` (OS font names, only when font data is not included) and `fontReferences` (other font assets in the project searched for missing glyphs). **[V]**
5. OTF: Unity imports .otf through the same importer. **[U]** for CFF-outline OTFs on legacy Text; TTF is the safe choice.
6. Rich text: `b`, `i`, `size`, `color` only for UI Text (`material` and `quad` are text-mesh only). No strikethrough, underline, outline or letter spacing. **[V]** (uGUI StyledText.md)
7. Bold and italic: synthesized if the TTF has no bold or italic face, unless extra font files are added. **[U]**
8. Shadow and outline: the uGUI `Shadow` and `Outline` components duplicate the mesh. They work but thicken badly past 1 to 2 px. **[U]** on look.

## 3. Path B: TextMeshPro (inside uGUI 2.0)

1. Setup: Window > TextMeshPro > Import TMP Essential Resources adds a `TextMesh Pro` folder to Assets. **[V]** (uGUI 2.0 TMP manual)
2. Contents, read from `Library/PackageCache/com.unity.ugui@b996e7548785/Package Resources/TMP Essential Resources.unitypackage`: 45 entries, about 4.0 MB. **[V]**
   - `Fonts/`: LiberationSans.ttf (350 KB) plus its OFL licence.
   - `Resources/`: TMP Settings.asset, LiberationSans SDF.asset (2.2 MB) plus Fallback, Outline and Drop Shadow materials, Default Style Sheet, EmojiOne sprite asset, two line-breaking text files.
   - `Shaders/`: about 20 TMP shaders and cgincs, plus URP Lit/Unlit and HDRP Lit/Unlit shader graphs (about 1.1 MB of graphs).
   - `Sprites/`: EmojiOne.png, .json and an attribution note pointing to EmojiOne's own licensing terms.
3. Whether the HDRP shader graphs log errors in a URP-only project: **[U]**. Whether EmojiOne can be deleted without TMP warnings: **[U]**.
4. Custom font: TTF/OTF goes into Font Asset Creator and comes out as an SDF font asset. Static (baked atlas, source font can be removed) or Dynamic (atlas grows at runtime, source font ships). Render modes include SDFAA, SDFAA_HINTED, SDF8, SDF16, SDF32. **[V]** (uGUI 2.0 TMP docs, local copy)
5. Sharpness: SDF stays sharp at any size and scale from one atlas; outline, soft shadow, underlay and dilate are shader settings, not extra meshes. **[V]** in docs; look under the VHS filter **[U]**.
6. Rich text adds `<s>`, `<u>`, `<mark>`, `<cspace>`, `<font>`, `<sprite>`, `<rotate>`, `<voffset>`, style sheets and more. Strikethrough fixes DayOneSamples.md 4. **[V]** (tag list in local docs)
7. Fallback: per-font-asset fallback lists plus a global list in TMP Settings; dynamic fallback atlases for glyphs not baked. **[V]** (FontAssetsFallback.md, FontAssetsDynamicFonts.md)

## 4. Sharpness at render scale 0.5 with the VHS filter

1. URP asset docs: "This only scales the game rendering. UI rendering is left at the native resolution for the device." **[V]**
2. So a Screen Space Overlay canvas (what DevMenu uses) draws at native resolution, after the camera. Both paths are sharp there. Whether it sits over or under the VHS filter depends on how the filter is injected; Style.md 7.7 still has this open **[U]**.
3. World-space text (labels on objects, notice boards, a 3D logbook, Style.md 7.6 "diegetic first") renders at 0.5 scale and through the filter. Legacy Text in world space rasterises at font size then gets scaled, so it goes soft or blocky; SDF holds its edge. Both get softened by the filter anyway. **[U]** how much difference survives the filter; needs a side-by-side capture.

## 5. Handwriting-style fonts

1. Both paths load any TTF, so a handwriting face works on both. **[V]** in principle.
2. Connected scripts that rely on OpenType ligatures or contextual alternates: legacy Text does no shaping. TMP in uGUI 2.0 handles kerning and some font features; full contextual shaping **[U]**. Pick a handwriting font whose letters do not need to join.
3. A pencil feel (grey, slight softness, uneven weight) is shader material work in TMP (softness, dilate, face texture) and needs a second font or colour only on legacy Text. **[U]** on the look.
4. A second face conflicts with Style.md 7.1 (one font). Vesper's call.

## 6. Localisation later

1. Unity Localization (a package, needs Grant's yes) drives any string setter through events, so either path works. **[U]** for 6000.3 specifics.
2. CJK or Cyrillic: legacy uses `fontReferences` fallbacks with dynamic fonts. TMP uses fallback font assets with dynamic atlases; this is the path Unity's own localisation guidance targets. **[U]** which one handles CJK line breaking better; TMP ships line-breaking rules files (seen in 3.2).

## 7. Recommendation

TextMeshPro, with one custom font baked as a static SDF asset (ASCII plus Latin-1) and a dynamic fallback asset from the same TTF.

- Why: strikethrough and underline for the logbook, shader outline and shadow for legibility through the tape filter, one asset sharp at every size including world-space labels, and a proper fallback chain for localisation. The switch is cheapest now: three scripts.
- Cost: no package (already in uGUI 2.0). About 4 MB, 45 files added under `Assets/TextMesh Pro`. One font file plus its licence, and generated font assets. Rewrite text in three scripts. One side-by-side capture to confirm the gain through the filter.
- Needs Grant's yes: yes. It adds assets to Assets and reverses DECISIONS 2026-09-20 and Style.md 7.1; a new dated DECISIONS line is required. Font choice and licence are Vesper's and Grant's.
- If Grant keeps legacy Text: custom TTF works. Import as Dynamic, Include Font Data on, Smooth rendering, overlay canvas. Cost is one font file and a yes for the asset; accept no strikethrough and weaker world-space text.
