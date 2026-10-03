# WardPath: the Ward walk and the ledge, redesigned
**DRAFT 3, 2026-10-02, Sable (PLAN 8.29).** Draft 2 is built. Draft 3 is a change list against that build (section 0), from Marlow's survey of it (Docs/Review/2026-10-02-Areas/829_Marlow_ground.md), which replaces the paper round. What it passed stands:
- the route, 116 to 121 s from camp, under 150;
- the day closure;
- the deck sees neither the Ward nor the fire (0 of 128 on every target);
- nothing north of the fallen giant is reachable except by C6's gap;
- the flights, rails, prow and stones as built.

Sections 1 to 5 are draft 2, with C9's numbers corrected in place.
## 0. Change list against the build (Rook builds from this)
| # | What, where (name and position) | Change | Why |
|---|---|---|---|
| C1 | **Ward/CairnGate/GateBlocker (IW2)**, box at (86.00, 13.54, 213.00), 0.50 x 4.00 x 3.40 | **A script switches it.** It is **on** in every day look (Day one, Day two) and **off** in the Night look: the same switch as the lit cairn lamp, LookVisibility Night. It turns on only at wake, with the player in the cabin. If a dev warp puts the player west of x 86.5 by day, it stays off until the player is east of x 87. | Marlow block 1: nothing turns it off, so J to the prow can never be walked |
| C1a | **The chain** at (86, 13.44, 213) | **By day:** hooked taut across the gap between the two rock arms at 0.9 m, a padlock on the north arm's hook, no sign. IW2 stands behind it and says nothing (Valley 8). **By night:** unhooked, lying slack in a coil at the north arm's foot, nothing across the tread; the cairn lamp lit. Neither state has a collider of its own. | the stop the player sees, both states |
| C2 | **Throat rocks,** no colliders, on the tread (chainage 149.6 to 170.9): FaceRock BigBoulders_2 (16.24, 265.64), Boulder_2 (8.10, 264.05), Boulder_2 (13.53, 263.98), BigBoulders_2 (18.67, 263.23), Boulder_0 (10.12, 266.90), BigBoulders_1 (6.13, 266.69), Boulder_1 (20.68, 259.98), BigBoulders_3 (22.67, 263.71), BigBoulders_2 (6.38, 263.50), Boulder_0 (4.66, 268.45), Boulder_0 (14.84, 266.88) | **Move each** straight away from the tread centreline until no vertex comes within 0.9 m of it (the 0.7 half tread plus 0.2), seated on the cleft wall. Where the slot is too narrow for that, scale the rock down until it fits. No hulls: these stay Stops visuals and the cleft walls hold. | Marlow hurt 2: the body walks through rock up to 1.25 m deep |
| C2a | **Rock/BandScree/BigBoulders_4** (84.20, 13.56, 215.46) at the gate | moves 1.1 m north, to z 216.6 against the north rock arm, clear of the tread by 0.2 m | the same; it enters the tread 0.59 m |
| C3 | **Ward/WardPath/FallenGiant/RunBench/Collider**, capsule x 33.0 to 61.5, r 1.5 at (y 42.17, z 272.0) | **Refit to the visible log:** r 3.8, axis at (y 42.25, z 271.8), the same x run, ends buried as built. Its south face is at z 268.0, 4 m north of landing 1's north rail (z 264). | Marlow hurt 3: the log is r 3.5 to 4, so a walker reached 2 to 2.5 m into the bark |
| C4 | **Landing 2 hide** (new): Ward/WardPath/Landing2_Overhang | One BK BigBoulders at 0.55, the P1_Overhang pattern, against the west face beside landing 2 (32.5, 247, floor 49). Its underside is 2.0 m over the landing and it has a convex hull. Under it, a covered standing spot 1.2 x 1.2 m at about (31.0, 247.0), reached from the landing. Rook fits it to the face. | Marlow hurt 4: the hollow under flight 3 was not built |
| C4a | **Landing 1 hide** (new): Ward/WardPath/Landing1_Overhang | the same, against the west face beside landing 1 (35, 262, floor 44), standing spot about (33.6, 262.0), inside the rails. RedFir_Bent stays as dressing. | landing 1's hide was unclear; this is also Quill's second place above landing 1 |
| C5 | **W foot band at the chute:** Rock/Band_W_N where the draw cuts it, about x 76.5 to 81.5, z 216 to 219 (band top 16 to 17 against the chute floor 15.8 at (79, 214)) | **Close it with a visible stop:** a rock piece in the Rock root on the band's cut face, owned rock texture, top 19.5 (about 3.7 m over the chute floor; the band rule). The same on the south cut face (z 207 to 210) if the band top there is within 2 m of the chute floor. | Marlow hurt 5: the band top is a second way off the walk at night. **Closed, not accepted:** the band is the map's west boundary and must never be a path; a night shortcut off the walk also breaks the "you cannot leave the climb" rule |
| C6 | **Ward/WardPath/FallenGiant/RimTie**, box (58.75, 38.91, 272.00), x 55.5 to 62.0 | **Extend east** from x 62.0 to the first ground over 45 degrees on z 272, about x 64 to 66 (Marlow reads it); same top (43.95) and depth | Marlow hurt 6: a slope at x 62.5 walks past the RimTie's east end, north to the 3 m² pocket at (63.0, 278.8). Closing the gap takes the pocket out of reach |
| C7 | **Ward/WardPath/Flight3/RailE/RailCollider**, a 9.3 m slab | refit to a pitched box following the treads, top 1.1 m over each tread, like the StairRamp | invisible-wall rule (Valley 8) |
| C8 | **J to Ward markers P2 and P4,** 0.36 m over the ground | reseat on the ground | cosmetic |
| C9 | **Numbers** | Camp to the prow 289.5 m in plan, 116 to 121 s. J to the prow 198.3 m in plan. Flight 1 24 degrees. Seep 50, rune post 81.6, landing 1 109.8, landing 2 124.5, lookout 144, fin exit 178.5, prow 197.9. Largest gap 28.1 m (11 s). IW2 3.4 m wide (it fills the built gap). | Marlow 8 |
| C10 | **Beat 0** | The "old giant at the gap" is not built and is dropped. The gate rests on the cairn and the chain. | Marlow 9 |

**Checks after the build:**
- the night flood from every trail reaches the prow and back;
- the day flood stops at the chain;
- the hand walk confirms C5 and C6 closed and the pocket gone;
- the walk-into check counts the C2 rocks (none on the tread);
- the deck checks are unchanged. Folds in Marlow (WardPath_Marlow.md: 3 blocks, 6 hurts), Vesper and Quill (below). Grant, 2026-10-01: "The ward scene and walk up the ward path is the worst." Replaces Valley.md rev 11 1.6 and 4 and ClimbFix.md 1 to 4. Drawing: WardPath.svg. Binding: DECISIONS 2026-09-25, 09-29 (Ward higher than the tower; land hides the fire; night-one reveal), 09-30 (small, never oppressive). Nothing here is decided until it is a dated line in DECISIONS.md and Grant confirms.
## 1. The walk (camp to the stones, every night)
Firewatch's trail to the lookout: forest, timber, a view earned. Five beats, each with its own ground, light and sound. Reference images: Hoh rainforest creek gullies (beat 1), Yosemite Mist Trail steps (3), Friedrich's Wanderer above the Sea of Fog (5).

| Beat (chainage from J) | Do | See | Feel |
|---|---|---|---|
| 0. Gate, 0 to 20 | cairn lamp lit, chain down in a coil | the gap in the rock band, dark beyond | the job is not over |
| 1. The draw, 20 to 54, W | up a fir gully on log steps beside a dry stony bed | banks 3 to 5 m high, firs within 8 m both sides, crowns closing to a strip of sky; a trickle you hear and never see; the seep and tin cup at the head (54) | sheltered; your lamp on ferns |
| 2. The shelf, 54 to 97, N then W (as built to the turn, graded B) | along the rib, the drop on your right; turn west across the bench at the rune post (82) | the whole valley, the tower cab level with you; ahead, a lantern at the stair foot and a fallen giant lying across the old way north | on show; wind |
| 3. The stair, 97 to 144 | three flights cut into the face: N, back S, N | landing 1 (110): lantern, the bent fir across the corner, a rock overhang on the face to hide under (C4a). Landing 2 (124.5): lantern, a rock overhang to hide under (C4), the valley through firs | effort; breath; the lamp swings |
| 4. Lookout and throat, 144 to 179 | the top: a plank floor with a rail, facing east; then through the split snag into the cleft | cab, cabin window, lot lights, highway (one car on night 1); then rock, a sky strip, the insects cut at the dogleg (18, 262) | last look; then silence |
| 5. The ledge, 179 to 199 | round the fin; walk to the prow | the fire (section 2) | the job was never the fire lookout |

Night 1 (WARD 12) is the baseline: nothing changed, the car passes once (trigger: stepping onto the lookout floor). From WARD 11 down, one change per night, my pick by playtest (Exit 8's spot the difference). What none of the inspirations do: the silence walks down the hill as WARD falls; the route you repeat is the gauge.

| WARD | Changes (never an event's beat; Quill) | Silence line (insects cut) and fire sound |
|---|---|---|
| 12 | none (baseline) | cut at the dogleg |
| 9 to 11 | cup gone; rune post doubled | cut at the dogleg |
| 6 to 8 | seep dry; bent fir upright; FIG-CLIMB uses the landings | cut at the lookout; fire first heard there |
| 3 to 5 | ash on the treads; landing 2 lantern unlit | cut at the stair foot; fire heard on the shelf |
| 1 to 2 | trickle silent; highway and the lot's pole lights dark (only lights no storyline uses) | cut at the gate; fire heard from J |
## 2. The ledge, as two shots (night only; no day ledge frames, Vesper 6)
1. **Shot A, the reveal** (WardPath.svg, right): eye at the fin exit (4, 257.3), 63.6, heading 225, level, 91.5 across (bearings 179 to 271). Right 58 percent: the far front (bearings 218 to 271), tops 6 to 10 degrees up, the low lit smoke sheet streaming west over it (Edges 6 night one; columns from night 2). Left fifth: the three stones, 32 to 38 m off, black against the S end wall and the glow above it, not against flame. Right edge: two stunted firs at the lip. The lip hides the valley floor.
2. **Shot B, the drop:** the path ends on a rock prow at (-12, 246), jutting 2 m past the lip, its west face sheer. A timber rail on the prow edge: top bar 1.0 m, open below, collider 1.1 m (the lip height that held). With the eye 0.45 m from the edge, rays from 0 to about 70 degrees down clear it, so the floor fires, bases 25 to 45 degrees down, burn under you. Paper; Rook measures.
3. Ledge floor: BK RubbleSparse and CS_Stone_1-8 in the cracks, moss #4F4A2C; Boulder_0-5 only at the N and S ends, none in the middle 60 degrees (Edges 9.4). Out: the pleated curtain (faced with owned rock), boulders with air under them.
4. Stones: Effigy StoneMenhir carved runes scaled to 8 m, rebuilt on the project shader, imported at 1024; positions unchanged. Proposal: the runes are the WARD gauge (Inscryption's candles), 12 runes, one dark per point lost, relit when WARD rises, not the fire's colour; the rune post stays dark.
5. First fix, before any grading: the night smoke sheet's draw order and height (it hides the fire), and the 0 fire cards (Gate_batch Marlow 8, Vesper 4.1). The sheet stays.
## 3. Layout (x east, z north, ground in m; measured points from Marlow)
| Piece | Path | Length, grade | Ground | Edges and supports |
|---|---|---|---|---|
| Gate | J (104, 206) 10 to (86, 213) 12 | 20 m | dirt | W foot band and IW2 as built |
| Draw | (86, 213), (70, 213), P1 (52, 216) 30 | 34 m, 30 percent | fill both flats (16) so each bank stands 3 to 5 m over the tread within 6 m, laid back 30 degrees or less; bank tops are open forest that drains back to the tread, no rim | log steps, a StairRamp on each |
| Shelf and bench | P1, (55, 244), stair foot (41, 246) 39 | 43 m | as built; bench 35 to 41 | shelf rims as built |
| Flight 1 | to landing 1 (35, 262) 44 | 18.2 m, 24 degrees (as built) | a 2.5 m bench cut and filled into the face along the line | crib logs on the downhill edge |
| Flight 2 | to landing 2 (32.5, 247) 49 | 15 m, 18 degrees | cut bench | crib logs |
| Flight 3 | to the top (29, 262) 60 | 15 m, 36 degrees | stair on stringers set into the face; one notch through the rock band at x 31.5 to 32 (50 to 57), cut faces under 5 m | posts 1.5 m or less |
| Lookout | the top, (27 to 31, 258 to 264), flush with ground | 5 m | planks | rail 1.0 m |
| Throat | split snag (25.5, 262), cleft entry (24, 262), cleft and fin as built | 34.5 m | bare stone | dressing only |
| Ledge | fin exit (4, 257.3) to the prow (-12, 246) 62 | 20 m | stone | lip rim 1.1 m; prow rail (2.2) |
1. J to the prow 198.3 m in plan; camp to the prow 289.5 m in plan, 116 to 121 s (Marlow, as built). Ceiling 150 s. Largest gap between landmarks 28.1 m (P1 lantern to rune post), 11 s.
2. **Closing the old loop:** a fallen giant (BK Sequoia laid down, root plate up) runs continuous at z 272 from the crest face at x 22, across leg 4's tread, down the face and across the whole bench to the east drop at x 59, tied into the shelf's valley rim; collider 1.3 m over ground on both sides, ends buried 2 m into faces over 50 degrees. Everything north of it (old shelf, P2, the cwm, leg 4) is unreachable; the P2 trap is behind it.
3. **Falls:** every flight, landing and the lookout stand on ground or posts of 1.5 m or less, all south of z 266. A fall off any of them slides down the face onto the open bench south of the giant, 20 m or less from the stair foot. No soft lock; rails are for the eye. No collider within 3 m of the face foot (Marlow's slide-pocket pattern).
4. Every join over 0.1 m gets a StairRamp: log steps, flights, landings, the lookout and prow edges (DECISIONS 2026-09-20). The draw bridge is cut (no water; one plank bridge on the map, Quill 6).
5. **F-1 and W-1:** terrain at x 20 or less is unchanged. Ledge work (stones, prow, rail, rim, lip firs under 68, wall facing) is at x -12 to 6, west of the 80 to 86 crest: the deck line to the stone tops passes the knob at 68.2 against 84, and N wall facing up to 85 stays hidden (Marlow 13). Lookout fill at x 27 to 34 is east of x 20. Rook reruns F-1 and W-1, menhirs by lowest vertex.
## 4. Owned assets per beat (AssetCatalogue.md, Vesper 4)
1. Gate: one BK Sequoia (top 50 or less); cairn as built. Draw: CS_Log_* log steps; BK RedFir5-8 and RedPine1-5 every 4 to 6 m on both banks; BK ThinFern1-5, GrassMoss, DeadLeaves; suffercord Bush1-4; dry bed of CS_Stone_1-8; Seep_Rock and cup as built. Shelf: as built; fallen giant BK Sequoia.
2. Stair and lookout: stringers and crib logs CITW_Log, treads CITW_Plank, decks C_Plank, posts RailingPost_Wood, all textured, supports visible; lanterns from the Campsite pack, flame in glass; RedFir1-4 on the cut banks; RedFir_Bent to landing 1; split snag to (25.5, 262). Throat: CS_Rock_1-8 and RedFirBranches; every cleft frame keeps a sky strip or a snag in silhouette, never all wall (Vesper 1); if a frame cannot, Rook reports, no cut.
3. Ledge: Effigy StoneMenhir; CS_Rock rim; prow rail in the stair's timber; RedFir1-4 stunted at the lip; NM Fire & Smoke Huge_Flames_01, FlameCard, Smoke, Backdrop.
## 5. Why this answers "worst"
1. Every D came from geometry: a corridor of walls and a bowl under the 80 m face (Gate_batch Vesper, Marlow). Four patches tuned rock percentages; none moved the path. This moves it: the bowl, leg 4 and the trap are behind one fallen giant, and the stair climbs the face the old loop went around, on ground.
2. Edges are banks, trees and timber; the 12 s throat is the one closed beat, and even it keeps sky. The beats differ by ground, light, sound and what you look at.
3. The ledge is graded as two shots at night with the fire present; each beat gets one hero frame Vesper grades before the full capture; Grant walks it gray before dressing.
## Open questions
1. Runes as the WARD gauge (2.4): Grant. Rune colour, if not ember: Vesper and Quill. Whether 12 runes count at 32 to 38 m: unverified.
2. Ground along the three new flight lines and the prow is unmeasured; cut and fill depths are paper. Plank sizes for treads: unverified.
3. Cabin window, lot lights and highway from the lookout: unverified (Marlow 9).
Sable

## Reviews of draft 1

### Quill review
2026-10-01. Story check of the night scares and events on the walk; details private. Draft until Grant approves.
1. Night one holds: lookout (cab, car), split snag, silence at the dogleg, the fire round the fin. One beat each.
2. Eight scares and events lose their old places (P1 to P4). All have a new place: the lookout takes the look-back ones, landings 1 and 2 take the stair ones, the cleft and ledge stay as they are. None goes in the throat.
3. Needs from the layout: lanterns at the stair foot and both landings (none listed); a second hiding place above landing 1 (only landing 1's overhang is listed; P1's is unstated); the cabin window and cab in the lookout frame.
4. Band table: "prints going up" and "cabin window lit" are already night events; as nightly changes they spend those events. "Valley dark but for camp" would show the resident sites' warning lights; only lights no storyline uses should go dark.
5. Runes as the gauge fit the story, if: they relight when WARD rises; they are not the fire's colour; the rune post stays dark; 12 can be counted at 31 to 36 m (unverified).
6. Two plank bridges now (Camp to J, and the draw at 38). Name the leg in any event that uses one.
Quill

### Vesper review (2026-10-01)
Against Style.md 10, Edges.md 6, UglySpots #1 and #2. Predicted letters are paper; only eye-height frames grade.
1. Grades if built as written: Gate C. Draw C, D risk (creek, ferns and bank ground unnamed). Shelf B as built. Stair D until it is textured meshes on visible supports (Style 5.4, 5.7); C reachable. Lookout C (lot lights, cab, window must read as modest practicals at night). Throat D risk: "exempt" does not exempt hard line 6; every cleft frame needs the sky strip or a snag, never all wall. Shot A C, B reachable. Shot B D today (0 fire cards on the floor); C once the floor burns.
2. UglySpots #2 answered: the bowl and leg 4 are gone. #1 half answered: cards and slab yes, but the plateau and ledge floor (FIN +0 to +14, Shot A lower third "bare ledge") have no dressing named. Fix: BK RubbleSparse and CS_Stone_1-8 in the cracks, moss tinted #4F4A2C (UglySpots #5), Boulder_0-5 only at the N and S ends, none in the middle 60 degrees (Edges 9.4).
3. Conflict: Shot A asks for night-one smoke columns out of the frame top (Style 6.3.3); Edges 6 night one, agreed with Sable, is a low lit sheet streaming west, columns from night two. Keep Edges 6; fix the sheet's draw order and height, do not remove it.
4. Vague assets, named: log steps CS_Log_* (StairRamp rule, LookBoards); ferns BK ThinFern1-5, GrassMoss, DeadLeaves; bank faces over 35 degrees the Rocks_a layer; stair stringers CITW_Log, treads CITW_Plank, deck C_Plank, posts RailingPost_Wood; creek: no owned water asset found, unverified, name it or drop the creek for a dry bed of CS_Stone_1-8.
5. Menhir: Effigy pack ships its own shader; rebuild on the project shader (Style 4.5), import 1024 (4.1), rune emission under the cairn lamp (4.6).
6. Day grade (open 4): no. Valley.md 8 closes the face, cwm and ledge by day on every day, so no player sees it lit. Keep one day frame from the tower and J only as a leak check (no ledge slab, sheet or card visible); drop day ledge frames from the capture set, they produced UglySpots #1. If the climb ever opens by day, Edges 6 "Day" becomes the bar.
Vesper
