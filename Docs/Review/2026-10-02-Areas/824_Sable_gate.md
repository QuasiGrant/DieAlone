# 8.24 gate, design (Sable), 2026-10-02
Against NorthLayout.md draft 2 and PLAN 8.24 ("its layout serves the loop, minigame and event sites it holds, with times stated"). Capture Docs/Captures/Main3Review_north (20:27) with Checks.md and index.md; Map_TopDown.jpg and AreaFrames_north.jpg looked at; BurnLayout_Sound.md 21 and Status 2026-10-02 read for LOOP-LEG. Read only; no Play run by me. Talk, Share the meal and the events' scripts are later milestones; this gate judges the places they need.

**Design items: PASS. Two lines UNVERIFIED and owed to Rook (4.1, 4.2). LOOP-LEG: UNVERIFIED; Rook's deck data does not cover that stretch (section 3).**

## 1. A north round, as built
| # | Item | Built | Verdict |
|---|---|---|---|
| 1 | The loop as a walk | Jg to Camp 1 81.2 m, 32.3 s; Camp 1 to J 230.9 m, 92.1 s; flood 14327 places, 0 leaks, 0 traps, 0 walk-into; 16 of 16 places reached and found. Every stretch has a next place on the found list (table, blaze, forage C, giant, ruin, J). | PASS. Firewatch's walk: each stretch ends at something, and the way home is the trail you are on. |
| 2 | Camp 1 as a family camp | Spar, kid's table, R1's spot, tent, cookfire, latrine, blaze all reached and found from their trails (table 29.5 m, 5.3 degrees off; spar 40.4 degrees tall). Flood and walk-into 0 inside the camp, so the table, chair, stool, box row and jug gaps hold. | PASS on places. R1 `Talk` and the pot's `Share the meal` are not in the build's interaction list (2 points: report box, forage C), so their rays are unproven (4.1). |
| 3 | Forage C, the minigame | Stand (229.2, 270.6): ray meets Bush_ForageC_1 at 1.94 m, prompt "Forage". 5 shrub colliders, least gap 0.58, 0 in the 0.6 to 1.0 band. Found from the loop at 25.4 m, 43.9 degrees off. | PASS. Dredge's rotating spot, one press at the end of a 64 m walk from Camp 1. Hurt, not a fail: 1.94 m is 0.06 inside reach from the stand, so a player stopping a step short gets no prompt until they close in. Whether a bare patch reads bare from 15 m is Vesper's eye-height call, not mine. |
| 4 | The ruin and its events | Room 219 of 504 floor cells clear for the capsule; doorway step 0.06 m; report box ray 1.44 m from its stand; walk in to the bunk, trunk and table and out 37.0 m, 14.1 s; stovepipe line from (192.7, 276.6) clear of every mesh at 22.5 m; inventory 47 of 47. | PASS. Enterable, every event spot inside stands on open floor, and the pipe pulls you off the loop from 21 m. Firewatch's dead keeper, with a report box like yours. |
| 5 | Search spots | SS1 found 28.2 m, 8.2 degrees off; SS2 25.4 m, 37.0 off; SS3 16.2 m, 14.0 off; each 1.0 degree tall. Deck pixel hide 0 of 128 for all three, each control seen. | PASS. Found by a player who looks, never from the deck. |
| 6 | Deck must-see and must-hide | Spar top 128 of 128 by every mesh (hard); kid's table and tripod 128 of 128; ruin pixel 0 of 128 (control seen from 14). Tent loose 0 of 128: the first block is the tent's own mesh, since the target sits inside it, so the tent is what the deck sees. | PASS. Papers, Please's inspection: the deck checks his spar and table and nothing of the cabin. |
| 7 | No tower from the ruin (draft 2 5.5) | Not rerun after N13 to N15. The deck pixel hide is the other direction, to the ruin's surfaces, not from an eye at the doorway or inside. | UNVERIFIED (4.2). |
| 8 | Warps | Camp_1 and North_Loop_Ruin land, fall 0.17 and 0.16 m, way out to 10 m. | PASS |

## 2. Times, on the build (2.5 m/s, PlayerController.Step)
| Leg | m | s | Doc |
|---|---|---|---|
| Jg to Camp 1 | 81.2 | 32.3 | 83 / 33 |
| Camp 1 to J, the loop (forage C and the side-path mouth on it) | 230.9 | 92.1 | 231 / 92 |
| Side path in at the doorway, round the room, out | 37.0 | 14.1 | 24 / 10 |
| Side path to the report box stand | 10.4 | 4.1 | |
| Door to Jg, J to door | not walked | | 125 / 50, 96 / 38 |

A full round is about 226 s plus stops (paper 224). The side path runs 4 s over paper because the check walks to the bunk, trunk and table; immaterial. Forage C pays only on a Camp 1 day, and on that day the round costs nearly four minutes of a day you cannot spare: right.

## 3. LOOP-LEG between forage C and the ruin (BurnLayout_Sound.md 21)
Where it can go. Forage C sits at loop m 64 from Camp 1, the side-path mouth (168.1, 267.1) at m 132. E66 (forage C insects, max 20 m) is proposed, not built; if it stands and is not replayed, the stretch starts 20 m past C. Leave 5 m before the side-path mouth. Window: **loop m 86 to 127, 41 m**, room for a 25 m stretch (10 s) with margin both ends.

| Need (21) | Rook's data | Verdict |
|---|---|---|
| 20 to 30 m with no tower view | Deck rays cover 8 targets only. None is on this stretch: SS1 is east of C, the ruin 14 m off the mouth. No trail point between m 64 and 132 is measured. | UNVERIFIED |
| One footstep surface | Walk-into and flood say nothing about colliders or SurfaceSound meshes beside the tread. | UNVERIFIED |
| No hop | The walk arrives; it does not log air time or step heights along the stretch. | UNVERIFIED |
| No sound volumes | No SoundZone, reverb or trigger list for the area. | UNVERIFIED |
| Silent marker post beside the tread | Not built (it moved here from the Hollow Giant stretch). | Not placed; I place it once 4.3 names the stretch. |

What Rook must measure, every 1 m along the Camp 1 to J trail centre from m 80 to m 132:
1. **Tower view, both ways.** Mesh rays from eyes at 1.6 m and 2.2 m (jump) over the ground to every Camp/Tower renderer, and a pixel check rendering from each eye with only Camp/Tower drawn against the full scene depth: 0 px is the bar. Report the longest unbroken run with 0 px.
2. **Surface.** Every collider within 0.5 m of the tread edge (name, position, gap), every mesh with a SurfaceSound on or over the tread, and the terrain layer under the centre line (one layer the whole run).
3. **Grade.** Ground height at 0.5 m steps; the largest single rise or drop; air time on a 2.5 m/s walk and a sprint, both directions. Bar: no frame ungrounded.
4. **Volumes.** Every SoundZone, reverb zone, audio trigger or bed-zone edge whose bounds touch the stretch, with the emitters (and max distance) in range of it.

## 4. Owed (not blocking the design item)
1. **R1 and the pot:** add R1 from (284.3, 242.1) and the cookfire pot from (274.5, 231.6) to the north check's interaction rays when those prompts exist, so Camp 1's two events are proven like the report box.
2. **No tower from the ruin:** rerun the 8.16b sightline from the North_Loop_Ruin warp, the doorway (169.88, 278.88) and the report box stand, eye 1.6 m: 0 px. Until then the deck line of 8.24's done-check is not fully met.
3. **LOOP-LEG:** section 3, items 1 to 4. With them I name the stretch and place the post.

## 5. Noted for other owners
1. Camp 1 to J fails Day one trail grey at 20 m (diff 16), and Jg to Camp 1 too (17). Pim, Vesper.
2. Forage C stand to shrub 1.94 m against reach 2 (row 3). Pim, if it hurts the prompt on the walk.

## 6. Open questions
1. Grant (carried from draft 2, open 1 to 3): the customer chair with the bear in it, SS1 to SS3 moved and hidden from the deck, the removals and the sky gap over the ruin. All built; judge on the walk.
2. Hollis: is E66 kept, and can ScareSound replay it? If yes, the LOOP-LEG window opens back toward forage C.
3. Quill: the chase's safe point (carried).

Sable
