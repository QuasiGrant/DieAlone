# Old burn, forage patches, Jg and T trails: interaction points for 8.28 (Pim, DRAFT 1 2026-10-02, nothing decided)
Inputs: Valley.md 1.2, 1.3, 6 (E3, E4, E13, 6.1), 8, 11, 14; Main3.md 4 (Camp to Jg, Jg to T, Jg to Camp 1); ForestPlan.md 8.2, 8.4; DailyLoop.md 3; LookBoards.md (berries dull red, the burn's one saturated note); Objective.md; Examine.md; FrontLayout_UI.md (T board); NorthLayout_UI.md and NorthLayout.md (forage C); Gate.md 4. Code (verified): reach 2 m (PlayerTuning.interactReach); one eye ray, first non-trigger collider wins (QueryTriggerInteraction.Ignore); Interact E / pad Y; while carrying, Interact sets down and the prompt reads `Set down`; one prompt label, InteractPromptUI, word only. No forage code: nothing in the burn has an Interactable. Built (recipes, read; scene not opened): trails Camp to Jg (170,160) (200,136) (232,152) (262,172), 125 m; Jg to T (262,172) (288,196) (316,148) (340,170), 105 m; Jg to Camp 1 (262,172) (264,208) (282,238), 83 m; Camp 2 to T ends at the same point (340,170) from (324,128). POI_Forage_patch_A: five grey spheres 1.1 x 0.8 m on r 1.4 round (240, 162.8), sphere colliders, pushed until 3.8 m or more from every trail centre point (final position not read). POI_Forage_patch_B the same at (142.4, 163.6), built near (140.7, 167.8). Gate_Tree stub (290, 176), 15 m. Sign_Jg (265, 169): post 2.6 m, arms CAMP, LOT, CAMP 1 at 2.4, 2.0, 1.6 m, each pointing at its leg 8 m out. Trailhead_Board (338, 172.5), face east, `VALLEY TRAILS`. Burn hedges: open corridor 3.5 m each side of every trail centre, r 4 round Jg and T, r 4 round each side piece, r 5.5 round each warp; brush 2.0 to 2.5 m on the corridor edge. By day only: no prompt in the burn at night (the night walk is camp to the Ward).

## 1. Interactables
| # | Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|---|
| 1.1 | Forage A, Camp to Jg at 96 m from camp (29 from Jg), north side | 2 m from the eye to a shrub collider | the near shrub, 0.6 to 1.0 high, from the trail edge on the patch side: about 1.2 m out, 35 degrees down | Bearing today, Food not met, nothing carried: `Forage`. Otherwise none. Same word at A, B and C |
| 1.2 | Forage B, Camp to Camp 3 at 40 m (camp area, rule shared) | 2 m | near shrub, as 1.1 | as 1.1 |
| 1.3 | Forage C (8.24) | 2 m | as NorthLayout_UI | as 1.1 |
| 1.4 | Jg signpost (265, 169) | none | arms | none; read by eye. On T7 days [GAP: Events, private] |
| 1.5 | Trailhead board, T (338, 172.5) | 2 m | map face, 1.5 high | `Examine` (FrontLayout_UI; Examine.md read view). Diegetic map |
| 1.6 | Gate Tree stub, snags, fallen trunks, regrowth | none | | none, off the mask |
| 1.7 | Hollow Giant opening (202, 140), Camp to Jg at 39 | none | | none. Event days only: the event's inspect word [GAP: Events, private] |
| 1.8 | First-sight-of-the-lot stake (327.3, 168.2) | none | | none |

### Forage rules (1.1 to 1.3)
1. **Bearing reads by eye before the prompt.** Bearing: dull red berries on the shrubs. Bare: the same shrubs, no berries; nothing is removed. Both states must read apart at 15 m in a day-one and a day-two frame (Vesper grades). A bare patch shows no prompt, so a bare day costs the walk and no press.
2. **One press meets Food.** On `Forage`: Food met, the shrubs go bare at once, the objective line shows `Logbook: Food met.` for 3 s (Objective.md). Nothing goes in the hand; no inventory.
3. **Food already met:** no prompt at any patch (the second patch is a second try only on a miss, DailyLoop 3).
4. **Carrying:** the code shows `Set down` instead. Set down first, then forage.
5. **Collider (Rook):** the ray ignores triggers, so each shrub needs a solid collider, top 0.6 m or more. It also stops the walk, which is fine: the shrub is the visible reason. Forage C's built shrubs have none (NorthLayout.md N8): same fix there.
6. **Reach from the trail:** the recipe keeps the patch centre 3.8 m from the trail centre, so the near shrub surface is about 1.9 m off centre. From the centre the ray to its top is about 2.06 m: out of reach. From the patch-side trail edge it is about 1.44 m: in reach. **Ask (Sable):** near shrub surface 1.0 to 1.5 m off the trail edge at A and B, as C. Then one sidestep, never off the band.

### Spacing (camp rules: 1.2 m centre to centre, 0.5 m edge to edge, none in front of another within 2 m along the approach)
7. Nothing with a prompt within 10 m of A or B. Old_Burn warp 5 m south of A (no prompt). Jg sign and T board have no neighbour within 2 m. Pass on paper.

## 2. Press counts (Gate 4, from standing at the point, both devices)
| Task | Keyboard | Pad | Bar 5 |
|---|---|---|---|
| Forage at A, B or C | `E` (1) | `Y` (1) | pass |
| Forage while carrying | `E` set down, `E` forage (2) | `Y`, `Y` (2) | pass |
| Read the Jg sign | 0 | 0 | pass |
| T board map, open and put down | `E`, then `E` or `Esc` (2) | `Y`, then `B` or `Y` (2) | pass |
| Warp to forage A from Play | F1 panel, measured on the built panel | same | not a gate task |
Pass on paper.

## 3. Found rule (clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
1. **Forage A from camp,** Camp to Jg heading about 56. On a straight the near shrub sits about 2.5 m left of the centre line: at 30 m out 5 degrees off travel, 0.8 m tall is 1.5 degrees. The line stays inside the 3.5 m corridor all the way. Pass on a straight; the leg is built 125 m on a 101 m map line, so its meanders may put the brush in the line. The 30 m and 15 m frames decide.
2. **Forage A from Jg,** heading about 236, same figures, patch on the right. Same frames.
3. **Keep clear (Sable), A, both approaches:** nothing over 0.5 m (brush, snag, fallen trunk, regrowth) between the trail centre and the near shrubs over the last 30 m. The hedge bulge round the patch stands behind it, never on the approach side of it. Burn snags every 8 m on the hedge (ForestPlan 8.4) stay out of the last 15 m on the patch side.
4. **Forage B from camp,** Camp to Camp 3 descending, heading about 313 on the map line; **from Camp 3,** heading about 133. The patch is 14 m off the map line, so the built meander sets both headings: unverified until the trail is read. Same keep-clear as 3.3, on the knoll's west flank bushes and the ForageB pocket fill.
5. **The burn from Camp to Jg** (heading 56): the burn starts at forage A (x 230); its found target is the open light and the first snags past A. Frame at A heading 56.
6. **The burn from T** (Jg to T walked west, the leg (316,148) to (288,196) heading 330): the Gate Tree stub (290, 176), 8.4 m left of the line, 16 degrees off at 30 m. Its top clears the 2.5 m brush (the line is 7 m up where it crosses the corridor edge). Pass on paper. **Keep clear:** no regrowth fir or snag over 6 m within 2 m of the line from 30 m out to the stub's top half.
7. **The burn from Jg toward T** (heading 47 then 330): the Gate Tree at 34 m chainage; frame at 30 m out.
8. **The burn from Camp 1** (Jg to Camp 1 walked south, heading 183): the Jg signpost and the open burn light at the grove ring's edge. Frame at 30 m out.
9. **Jg from each of its three trails:** the signpost. Built post (265, 169) stands 4.2 m off the camp and T lines and 3.1 m off the Camp 1 line, on the corridor edge, 2.6 m tall against 2.0 to 2.5 m brush: likely only its top shows. **Ask (Sable):** post to (263.8, 170.2), 2.5 m from the junction centre at bearing 135, the empty sector between the T leg (47) and the camp leg (236); that puts it 1.9 to 2.6 m off all three lines, inside the corridor. And no brush in the junction's r 4 open ground on the post's side.
10. **T from the Jg trail** (heading about 47 on its last stretch): the lot itself, then the board. **T from the lot** (lot centre (358, 170), heading 270): the board at 20 m, 7 degrees. Pass on paper.
11. Frames for Rook: A at 30 and 15 m from both sides, bearing and bare; B the same on the built trail; the Gate Tree at 30 m on both Jg to T approaches; Jg sign at 30 m from all three trails and from the junction centre facing 135; T board from the lot centre facing 270.

## 4. Junction markers on the Jg and T trails
| Junction | Needs one | Marker | Faces |
|---|---|---|---|
| Jg (262, 172), three ways | yes: from T the Camp 1 mouth is a 136 degree turn behind you on the right; from Camp 1 the T mouth is the same on the left | signpost, arms `CAMP`, `LOT`, `CAMP 1` (built) | each arm points along its own leg (camp about 236, T about 47, Camp 1 about 3). Read from the junction centre facing 135: arm faces 11, 2 and 42 degrees off square, all legible. Arms in line with your travel are edge-on from the trail; that is expected, the post is the found target and the junction centre is the read point |
| T (340, 170): the Jg trail and the Camp 2 trail end at one point, mouths about 228 and 201, 27 degrees apart | yes, from the lot side: two mouths that look like one | trailhead board (built) plus **two arms on its north post (new, proposed):** `CAMP` along the Jg trail mouth, `CAMP 2` along the Camp 2 mouth | arms point along their mouths; from the lot (viewer due east) the faces are 48 and 21 degrees off square, legible. Mouth headings are from the map; read from the built Trails before building |
| Camp to Jg at camp, Camp 2 to T at Camp 2, Jg to Camp 1 at Camp 1 | covered by 8.21, 8.25, 8.24 | | |
| Side bulges (forage A, Gate Tree, Old_Burn warp) | no | none; the dirt band carries the line, the bulges have none | |

## 5. Warps (F1)
| Plain name | Warp object | Lands | Facing | State |
|---|---|---|---|---|
| Burn fork (to Camp 1 and the lot) | Junction_Jg | (262, 168), 4 m south of the junction centre | 88: sign 17 degrees right; T mouth about 41 left; Camp 1 mouth about 85 left, likely out of frame | built. **Proposed:** facing 40, so the Camp 1 mouth (38 left), the T mouth (about 0) and the sign (32 right) are all in frame |
| Old burn | Old_Burn | (239.6, 157.8) on Camp to Jg, about 5 m south of forage A | 331: forage A about 34 degrees right of centre; in frame only if half the horizontal view is over 34 degrees (camera FOV not read) | built. **Proposed:** label `Old burn, forage A`, facing about 5 (the patch dead ahead), row moved from FRONT to just under `Burn fork` in DevWarpLabels.cs. Pitch is kept from before the warp |
| Parking lot trailhead | Trailhead_T | (337, 170) | 90, the lot | built, front area (8.22); both trail mouths are behind it |

No new warp. Forage B has none; Keepers_Camp is 40 m of trail away.

## Open
1. Forage collider and reach from the trail edge (1.4 to 1.6; Rook, Sable).
2. Bearing against bare at 15 m (Vesper).
3. Jg post moved into the corridor; T arms (Sable, Wren).
4. B's approach headings on the built trail (Rook reads the Trails points).
5. Warp facings and the Old burn row (Wren).
6. T7 and Hollow Giant event prompts (Events, private).
Figures on paper; built frames decide. Pim
