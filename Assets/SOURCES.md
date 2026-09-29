# Sources

## Asset Store packs

Bought on the owner's Unity account. The repo is public and the Asset Store EULA forbids redistribution, so these folders are git-ignored and never committed. To restore the project: clone the repo, then in Unity open Package Manager, My Assets, and import each pack below.

| Pack | Publisher | Store page | Folder in project | Licence |
|---|---|---|---|---|
| PSX Modular Complete Pack | Celestia Studio | https://assetstore.unity.com/packages/3d/environments/psx-modular-complete-pack-270446 | Assets/Celestia_Studio/PSX_Modular_Complete_Pack | Standard Unity Asset Store EULA, commercial use allowed |
| Campsite - Retro Style Modular Environment Pack | Revolving Pizza Games | https://assetstore.unity.com/packages/3d/environments/campsite-retro-style-modular-environment-pack-395914 | Assets/Revolving Pizza Games/Campsite | Standard Unity Asset Store EULA, commercial use allowed |
| Cabin In The Woods - Retro Style Modular Environment Pack | Revolving Pizza Games | https://assetstore.unity.com/packages/3d/environments/cabin-in-the-woods-retro-style-modular-environment-pack-345902 | Assets/Revolving Pizza Games/Cabin In The Woods | Standard Unity Asset Store EULA, commercial use allowed |
| PSX Autumn Forest Pack | suffercord | https://assetstore.unity.com/packages/3d/vegetation/psx-autumn-forest-pack-378758 | Assets/suffercord | Standard Unity Asset Store EULA, commercial use allowed |
| Menhir Stone Circle | Effigy GameWorks | https://assetstore.unity.com/packages/3d/props/menhir-stone-circle-315132 | Assets/Effigy GameWorks | Standard Unity Asset Store EULA, commercial use allowed |
| Pure Nature 2 : Redwood | BK | https://assetstore.unity.com/packages/3d/environments/pure-nature-2-redwood-313097 | Assets/BK | Standard Unity Asset Store EULA, commercial use allowed |
| Fire & Smoke - Dynamic Nature | NatureManufacture | https://assetstore.unity.com/packages/vfx/particles/fire-explosions/fire-smoke-dynamic-nature-217775 | Assets/NatureManufacture Assets | Standard Unity Asset Store EULA, commercial use allowed |
| Catacombs - Retro Style Modular Environment Pack | Revolving Pizza Games | https://assetstore.unity.com/packages/3d/environments/catacombs-retro-style-modular-environment-pack-388272 | Assets/Revolving Pizza Games/Catacombs | Standard Unity Asset Store EULA, commercial use allowed |
| PSX Edition - Modular Parking Lot | FANNEC | https://assetstore.unity.com/packages/3d/environments/urban/psx-edition-modular-parking-lot-326617 | Assets/PSX Edition - Modular Parking Lot | Standard Unity Asset Store EULA, commercial use allowed |
| Modular Chain Link Fence | PolyPlex | https://assetstore.unity.com/packages/3d/props/exterior/modular-chain-link-fence-252145 | Assets/Modular Chain Link Fence | Standard Unity Asset Store EULA, commercial use allowed |
| PSX Rural Farm Tools Pack - 32 Lowpoly Tool Props | Rosemary3d | https://assetstore.unity.com/packages/3d/props/tools/psx-rural-farm-tools-pack-32-lowpoly-tool-props-365026 | Assets/PSX Farm Tools Pack | Standard Unity Asset Store EULA; the pack README adds its own note that allows commercial use and forbids resale or redistribution as-is |
| PSX Supplies Pack - 46 Lowpoly Supplies Props | Rosemary3d | https://assetstore.unity.com/packages/3d/props/tools/psx-supplies-pack-46-lowpoly-supplies-props-349098 | Assets/PSX Supplies Pack | Standard Unity Asset Store EULA; the pack README adds its own note that allows commercial use and forbids resale or redistribution as-is |

Materials in the packs are converted to URP after import through Unity's Render Pipeline Converter.

Licences checked on each store page on 2026-09-29: every pack above lists "Standard Unity Asset Store EULA", licence type Extension Asset, Single Entity seat; that EULA allows use in commercial games. Packs bought for task 7.4 are imported with Tools/Recipes/import_packs_7_4.cs and converted with Tools/Recipes/urp_convert_packs_7_4.cs; Fire & Smoke also needs Tools/Recipes/import_nm_fire_urp.cs (its own URP 17.3 support package).

## Textures

CC0 from ambientCG (https://ambientcg.com). Only the 1K JPG color map is used. Import Max Size is 512. These are committed.

| Folder | Site | Asset ID | Page | Used for |
|---|---|---|---|---|
| Ground054 | ambientCG | Ground054 | https://ambientcg.com/view?id=Ground054 | Ground |
| PaintedMetal006 | ambientCG | PaintedMetal006 | https://ambientcg.com/view?id=PaintedMetal006 | Tower legs, deck, stairs, rails |
| Concrete034 | ambientCG | Concrete034 | https://ambientcg.com/view?id=Concrete034 | Test room and tower room walls, roof, platform |
| Planks023A | ambientCG | Planks023A | https://ambientcg.com/view?id=Planks023A | Props: table, shelf, loose objects, desk, bunk, test box, doors |
