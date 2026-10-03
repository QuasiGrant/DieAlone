# Burn layout: the old burn, the forage patches, Jg, and the Jg and T trails (PLAN 8.28)
**DRAFT 2, 2026-10-02, Sable.** Fixes Marlow's paper check of draft 1 (Docs/Review/2026-10-02-Areas/828_Marlow_paper.md: 1 block, 7 hurts, 9 cosmetic) and takes his correction: there is no pocket at (188.2, 137.1), so B9 is gone. Everything he passed stands. Drawn from his ground survey (828_Marlow_ground.md). Folds in Quill (BurnLayout_Story.md), Pim (BurnLayout_UI.md) and Hollis (BurnLayout_Sound.md). Wren's calls apply: LOOP-LEG goes to the north loop; ash footsteps go to M11; Pim's warp names and facings stand; forage C stays as built. Drawing: BurnLayout.svg. Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
World metres as Valley.md. Capsule 0.35, climb 0.70, slope 45, reach 2. "m" is metres along Marlow's scene markers.
- **Tread:** within 0.35 m of the ground 2 m either side, all four trails.
- **Camp to Jg, m 10 to 28:** a drop south of the trail, 8.3 m at up to 79 degrees, starting 2.5 m from the centreline, down to the Hollow Giant stand at 6.0. A fall walks out south to Camp to pump.
- **Deck:** sees every trail marker from all 128 eyes, on terrain alone.
## 1. A burn trip: do, see, feel
| Leg | Do | See | Feel |
|---|---|---|---|
| Camp to Jg, 126 m | down off the knoll along the drop | below on the right, the Hollow Giant's burned-out base facing you across the drop; the live giants end at m 58 and the sky opens | leaving the saved ground |
| Forage A, m 74 to 80 | one sidestep from the tread, `Forage` | dark berries or stripped twigs | near and uncertain |
| Jg | the post with three arms | Camp, Lot, Camp 1, each readable from its own leg | the fork you use every supply day |
| Jg to T, 105 m | east through short regrowth, past the Gate Tree stub 1.1 m off the tread | the stub's slap of your own steps; open sky east; the lot lights at m 89 | the ordinary world getting close |
| Jg to Camp 1, 81 m | north out of the burn | a live edge by m 37, then the latrine at 42 | the doorway to Camp 1 |
## 2. The asks, answered
1. **The burn works as the supply scar: yes, with B1 to B8.** The outline stays ValleyShapes.BurnOutline.
2. **Forage A, Pim's reach:** passes as built. The nearest bush surfaces are 1.52 and 1.76 m in plan from the stand. The bushes are spheres of r 0.55, tops 0.92 to 0.96 over the ground, so they cannot be climbed.
3. **Forage C:** as built at (229.5, 273.5), 8.5 m from Valley E13; Wren updates Valley. It gets solid sphere colliders on a r 1.0 ring, gaps 0.3 or less. It is hidden from the deck (Marlow: 0 of 128).
4. **The Hollow Giant (Marlow's block): I drop the inside room and keep a shallow burned hollow at the base.** Reasons:
   - The visible trunk is 1.2 to 1.5 m in radius toward the trail at tread height, so no 2.5 m room fits.
   - The bark stands on the drop floor at 6.0, 4.9 m under the tread, so any opening at trail height needs a 5.3 m landing with 4.9 m skirts.
   - A hollow at the base, read from the trail across the drop and reached on foot from below, keeps the place, the event and the deck rule without new structure.
   - Quill 17's "floor the player steps onto from the trail" becomes "a hollow you look down into from the trail and reach from below". Hollis 16's interior zone goes.
5. **Settled clashes, kept from draft 1:**
   - **Jg post:** Pim's spot; the knock follows the post.
   - **Hollow Giant stretch:** cannot hide the tower; its marker post goes with LOOP-LEG.
   - **Firebreak:** visual only.
   - **Regrowth:** 6 m or less beside Jg to T, and no tree over 8 m within 40 m east from m 50.
   - **Figures on the trails:** the events' rule, never shown while the player is on the deck.
   - **Ash:** M11.
## 3. Places
| # | Place, where | Job |
|---|---|---|
| B1 | **Forage A** as built, centre (239.59, 5.01, 163.04). **Stand:** (241.6, 160.6) facing 301, pitch -36, reaching bush (239.82, 161.66) at 1.85 m (Marlow). **Keep clear,** both approaches: nothing over 0.5 m between the centreline and the near bushes. **R:** SliceLook/ShotGround CS_Log_Firewood (240.21, 161.01). **Deck screen:** three RedFir5 (about 9 m) at x 235.8, z 161.9, 163.15 and 164.4. Their foliage is continuous across z 161.7 to 164.6 from 2.4 to 4.4 m over the ground (7.4 to 9.4), where the deck lines to the bush tops cross x 236. They stand inside Hedge_Burn_3's collider (235.77, 163.25; z 161.85 to 164.65), so the gaps between their trunks are closed. Pixel check: the bush tops at 0 of 128 | `Forage` |
| B2 | **Hollow Giant** (202, 140), Sequoia3 at scale 1.17. **The r 4.5 cylinder collider is replaced** by PlaceKit.PackTrunkCapsule fitted to the bark (8.16a), so a body can stand at the trunk. **The hollow:** a burned-out base, 1.2 m wide, 2.0 m tall, 0.6 m deep, made with a kitbash shell of burnt-bark pieces round a dark recess against the trunk. It faces bearing 28, centred on the bark at about (203.2, 142.3) on ground near 6.0 (Marlow samples). It is read from Camp to Jg m 28 to 34, 8 to 13 m away and below. **Reached on foot** from Camp to pump through the stand (Marlow's flood shows the way out runs there; he confirms the way in). Inspect from (203.9, 143.6) facing 208. **The deck** bears 302, 86 degrees off the hollow's facing, so it sees the hollow edge-on; pixel check on its back face at 0 of 128 | events |
| B3 | **Jg signpost** moves to (263.8, 170.2): 2.55 m from the junction at bearing 135, 2.55 to 2.71 m off the three lines (Marlow). Legs leave Jg at T 35.2, camp 262.5 and Camp 1 351.6. Hedge_Burn_7's collider (centre (266.83, 170.50), 5.06 m from the junction) stays only if its box keeps outside r 4 of the junction; Marlow reads its extent | marker |
| B4 | **Trailhead board at T** (338, 172.5): two arms on its north post, CAMP along the Jg mouth (260.4) and CAMP 2 along the Camp 2 mouth (184.4), 76 degrees apart | marker |
| B5 | **Gate Tree stub** (290, 176): its r 4.0 collider stands up to 3.25 m outside the visible trunk, so it is refitted to the bark (PackTrunkCapsule). The bark is 1.08 m off the Jg to T centreline at y 4 to 6 and 1.95 at y 6 to 9, so the trail stays within 2.5 m of the face (Hollis 15). Nothing between the trail and the stub; no other trunk of r 2 or more within 20 m | landmark |
| B6 | **Firebreak:** saw-cut stumps 4 to 6 m wide, x 205 to 211, from the trail north to z 200. Visual only (Marlow samples its ground) | Quill 2 |
| B7 | **Trail-edge stones:** the 11 convex-hull stones (0.12 to 0.43 m tall) lose their colliders (passed). CS_Stone_3 (261.40, 173.30), on the Jg to Camp 1 tread, moves out to 1.4 m | chase legs |
| B8 | **Deck lines through the burn.** **Verge frame:** every Forest/BurnDeadwood and BurnRegrowth snag within 15 m in plan of the deck-to-verge line from x 250 to 345 moves out to 15 m or more (about 20; Marlow's run log names them). The nearest are BurnDeadwood (305.61, 150.59), (257.81, 157.82), (264.37, 157.91), (271.66, 158.76) and (292.23, 157.12), and BurnRegrowth (303.45, 148.45), (278.18, 157.48) and (255.48, 160.08). (315.77, 134.50), 15.35 m off, stays. **Office line:** caps are the lowest eye's line minus 2 m: 35.4 at x 230, 18.0 at x 290, 6.4 at x 330, absolute. Nothing within 3.5 m of the line in plan rises above the cap. The five snags near it each move 5 m or more off the line, perpendicular: SliceLook/BurnRegrowth/Tree_Dead (313.35, 191.96), Forest/BurnDeadwood/Tree_Dead (308.17, 191.32), BurnRegrowth (300.77, 191.62), BurnDeadwood (294.31, 190.29) and BurnDeadwood (287.66, 189.99) | deck |
**Forage B** (8.26's, checked here): four bush tops are clear to 17 to 27 of 128 eyes. **Two RedFir8 (9 to 11 m)** go 2.3 m east of the patch, toward the deck, at (143.0, 166.0) and (143.0, 168.3). Their crowns stand over the 64 degree deck lines, and they give Quill 10's shade. Pixel check at 0 of 128. Marlow checks them against the Camp to Camp 3 tread.
## 4. Times (2.5 m/s, from the camp signpost (165, 158), Marlow)
| Walk | m | s | | Walk | m | s |
|---|---|---|---|---|---|---|
| camp to Jg | 126.4 | 51 | | camp to forage A | 97.7 | 39 |
| Jg to T | 104.5 | 42 | | forage A to Jg | 28.7 | 11 |
| Jg to Camp 1 | 81.2 | 32 | | forage A to B through camp | 129.9 | 52 |
| Camp 2 to T | 92.8 | 37 | | camp to the lot centre | 249.0 | 100 |
**Look-back straight** (Quill 15): m 13 to 41, 28 m within 1.5 m of a straight chord, along the drop; Quill asked for 30. m 62 to 88 wanders 2.59 m.
## 5. Trap spots, no regression
| # | Where | Found | Held by | Recheck |
|---|---|---|---|---|
| T1 | Hedge_Burn_1 stood on from the CampStep collider | 8.21b | hedges 2 m over walkable ground within 2.5 m | untouched (no B9) |
| T2 | Hedge_Burn_0 jumped onto from TrenchWest | 8.21b | the same | area flood |
| T3 | burn fallen trunks at (190, 174) and (276, 149) | Gate_816b_817 | known, deferred | trunk check |
| T4 | a fall off the Camp to Jg drop | 828 survey and paper | walks out to Camp to pump P60 (186.69, 133.44), 3.40 m clear | area flood over x 150 to 245, z 100 to 182, every trail an exit |
| T5 | trapped cells on that flood's edges at x 150 and z 100 (4.4 and 0.7 m²) | 828 paper | cells outside the region not counted | the whole-map flood (8.30) |
## 6. Area check data
1. **Bounds x 180 to 345, z 100 to 240.** **Warps:** Junction_Jg (262, 168) facing 40 (built 88.6); Old_Burn (239.6, 157.8), label `Old burn, forage A`, facing 5 (built 331; the patch is at bearing 2.5).
2. **Places:** forage A, Hollow Giant hollow, Jg signpost, Gate Tree, trailhead board and arms, firebreak, first-sight stake (327.3, 168.2). **Interactions:** forage A as B1; the hollow as B2.
3. **Found frames, set from Marlow's measured first-found points:**
   - forage A from the camp side at (233.4, 148.3) and from the Jg side at (247.8, 174.3);
   - forage B at 12 and 11 m;
   - the Gate Tree from Jg at (267.0, 180.7), and from T at 17 m on its upper trunk (Hedge_Burn_7 hides the base);
   - the Jg sign from all three legs and from the junction facing 135;
   - the T board from the lot facing 270;
   - the hollow from m 30 heading 200.
4. **Deck must-see, hard:** the office west door and the verge tree (no burn collider blocks either; B8 keeps the frames). **May see:** the scar, snags, stub, trails, lot lights. **Must-hide, hard (pixel checks):** whether A, B or C bears (B1 and B's firs; C passes), and the inside of the Hollow Giant hollow (B2).
## 7. Borrowed, and what none of them do
Firewatch: an old burn you walk under a lookout. Dredge: a rotating forage spot read by eye. Exit 8: a junction sign that can turn. Papers, Please: the deck checks the verge tree through the scar. None put your food on the store road, so a good dawn means you never see the people you walk toward.
## 8. Open questions
1. Grant: the Hollow Giant as a shallow base hollow, reached from below, with no room; the burn year (Quill 6).
2. Quill: the look-back straight at 28 m; about 25 snags moved for the verge and office frames; firs over forage B.
3. Marlow: the way in to the hollow from Camp to pump; Hedge_Burn_7's extent; the hollow's ground and B6's.
Sable
