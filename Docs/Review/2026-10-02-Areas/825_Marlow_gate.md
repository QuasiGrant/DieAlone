# 8.25 gate, Marlow: checklist from the sheets and hand walk (Camp 2)
2026-10-03, Marlow. **PASS on everything I walked** (0 blocks, 1 hurt, 4 cosmetic). The hurt is item 4: the hook and the table chairs have no prompts yet.

**Setup:**
- Sheets: Docs/Captures/Main3Review_camp2 (capture 00:55).
- Scene: the working tree at ad2fad6, unchanged by me.
- Play: one session, which I entered at 00:59 (the Editor was stopped and not compiling) and stopped at 01:02.
- Mover: every move is PlayerController.Step (dt 0.02; walk 2.5, sprint 5.5, jump 0.6, crouch 1.0).
- Frames: rendered from a copy of Camera.main into the scratchpad, folder g825 (not committed).

## Gate step 2 checklist (from the sheets)
| # | Item | Result | Frames |
|---|---|---|---|
| 1 | Every trail both ways, path reads apart from ground | PASS. The tread reads in every frame. Frames are every 10 m (the gate asks for 5 m). Boathouse to Camp 2 shows FWD 20 to 90 and BACK 0 to 70; Camp 2 to T shows FWD 0 to 90 and BACK 0 to 90 | Trail_Boathouse_to_Camp_2, Trail_Camp_2_to_T |
| 2 | Every invisible stop, from 2 m back, has a visible reason | PASS. S1 is the booth; S6 and S7 are plank skirts (a board wall and vertical boards); S8 is the boulder; S9 to S48 are brush hedges | Stops_1 |
| 3 | Every trail end and warp: the path goes on, or ends at a place | PASS. Both stair-foot ends face up into the plank-screened stair. The T ends face the lot and the VALLEY TRAIL board. Camp_2 N and E show the stack, the booth and the table. Camp_2_Top shows his chair (N), the stair head between screens (E) and the tent (W) | Trail_Ends, Warps_NESW |
| 4 | Every place, outside and in: size and purpose read | PASS. The stack, booth, table and chairs read from the south, the west and 20 m. The stack top reads as a camp: tent, lamp, rail | Places, AreaFrames_camp2 F1 to F7 |
| 5 to 6 | Lot road; climb legs | not this area | |
| 7 | Top-down plus compass views | PASS for this quarter | Compass_Views, Map_TopDown |
| 8 | Day and night pairs | no pair spots on this sheet (0 frames) | Pairs_DayOne_Night |
| 9 | F1 by keyboard and pad | Pim's | |
| 10 | Hand walk | below | |

## Hand walk, asked items
1. **The 1.5 m plank screens on every ramp and landing. PASS; they read as a reason to stop.**
   - **Off the stair.** I started from every ramp and landing (all eight pieces), every 0.4 m across and 0.8 m along, and ran 16 headings each way: walk, sprint, walk-jump and sprint-jump. That is 8,512 runs: 0 left the stair's footprint, and 0 ended standing on a screen, rail, board, gap skirt or post.
   - **How they look** (frames 01 to 06):
     - From the ramps the screens read as a solid dark plank fence about shoulder high. Their tops step up the ramp in a saw-tooth.
     - On the landings they show narrow vertical slits between boards, with the forest and sky through them.
     - Nothing reads as a gap you could step through.
2. **Wedge fill and the corner behind the way-in planks. PASS.**
   - **Onto the fill.** From the ground every 0.25 m over x 295.5 to 299, z 104.5 to 113.5, I made 6,656 sprint and sprint-jump runs on 16 headings. 0 ended standing on WedgeFill, WedgeFillCorner, a screen, rail, board or gap skirt.
   - **The corner itself** (x 296 to 298.2, z 105.6 to 107.9): every ground spot can walk 1 m out on some heading, so there is no pocket.
   - Frame 07 shows the corner dark, between the stack and the planks. The frame from x 295 (08) is inside the stack, so I made no read from there.
3. **Ramp1/Ramp2 void. PASS.** From Ramp1, every 0.4 m along and at 3 points across, I ran 16 headings each of walk, sprint, walk-jump, sprint-jump and crouch-walk: 5,760 runs, 0 ending under Ramp2 or any ramp.
4. **Hook and table-chair pair. PASS on the ray; hurt on prompts.**
   - **The hook ray.** From the booth mouth (299.6, 99.2), facing 180 and 17 degrees down, the first hit is Layout825/Handset/Receiver at 0.75 m, as Rook found.
   - **Hitting the receiver is hard.** Over 1,224 looks (yaw every 5 degrees, pitch -10 to 70) from 4 stands round the mouth and the table, the receiver is the first hit within 2 m in only 6, 3, 0 and 2 looks. His chair takes 15 to 53, the table 27 to 110, your chair 2 to 68 and the third place up to 23.
   - **No prompts at all.** There is no Interactable anywhere in Camp 2, so the interactor shows no prompt for the hook, the barrel or R2. The prompts are not wired yet.
   - **Hurt:** once prompts exist, the hook is a 0.07 x 0.22 m target beside a chair that takes most nearby looks.
   - **Walk:** your seat round the table's north side to the booth door is 3.1 m, 1.3 s, no stall (Rook 2.2 m, 0.8 s, on a straighter line).
5. **His chair at its new spot. PASS.**
   - **Toward the T signpost** (frame 09, seated eye (294.94, 25.20, 110.30)): the view runs over the top rail and the stair head into the forest, with the lot's light line on the far edge. The rail crosses the frame just under the horizon. At 75 m the signpost itself is not readable by eye; Rook's line is clear.
   - **The highway** (frames 10 and 11, seated and standing at the east rail): a row of lights shows above the rail and the trees.
   - The Camp_2_Top warp at heading 20 (frame 16) shows the chair, the rail and the lot lights.
6. **PS1 to PS3. PASS (reach); cosmetic (PS2 and PS3 read).**

   | Spot | Position | Nearest stand | Eye to paper | Read |
   |---|---|---|---|---|
   | PS1, paper on the top rail | (288.14, 25.05, 109.60) | 0.50 m off, (288.64, 24.02, 109.60) | 0.76 m | a white diamond on the rail (frame 12) |
   | PS2, on the top floor | (288.80, 24.00, 111.20) | 0.56 m off | 1.71 m | a small pale scrap by a rail post (frame 13) |
   | PS3, on the ground at the stack's west foot | (283.70, 4.00, 107.90) | stand on it | 1.63 m | from 3 m east, the dressing boulders hide it (frame 14); Rook finds it at 16 m from the trail |

7. **Camp_2_Top warp and the walk down. PASS.** The warp settles at (294.50, 24.04, 107.20) on the GraniteStack, a 0.17 m fall. From it, down LandingTop, Ramp4, LandingN2, Ramp3, LandingS1, Ramp2, LandingN1 and Ramp1 to the foot (298.9, 105.5): 56.0 m, 22.4 s, no stall.
8. **Walk times. PASS, no stall on any:**

   | Walk | Distance | Time | Rook |
   |---|---|---|---|
   | Boathouse to Camp 2, markers | 93.0 m | 37.2 s | 93.7 m, 37.2 s |
   | Camp 2 to T, markers | 93.4 m | 37.3 s | 94.6 m, 37.4 s |
   | Ramp foot up the stair to his chair stand (293.8, 109.5) | 58.2 m | 23.3 s | 57.4 m, 22.2 s |
   | Warp down to the foot | 56.0 m | 22.4 s | |
   | Seat to booth door | 3.1 m | 1.3 s | |
   | Table to the ramp foot | 8.4 m | 3.3 s | 9.1 m, 3.6 s |

## Cosmetic
9. The ramp screens' tops step in a saw-tooth up each ramp, and the landing screens show sky slits between boards (frames 01 to 06). They still read as a fence.
10. PS2 is a small pale scrap and PS3 sits behind boulders from the east (item 6).
11. Trail sheets every 10 m, not 5, and the Boathouse to Camp 2 frames are cut at FWD 20 and BACK 70.
12. Booth hood light: 0 of 128 deck rays (loose target), blocked by SliceLook Sequoia3 (Checks.md).

## Done-check (my lines)
| Line | Result |
|---|---|
| Marlow's flood, trap and walk-into checks find 0 problems in it | PASS for the stair, screens, wedge, corner and void as walked (20,928 runs, 0 off or onto a stop) |
| every warp in it lands | PASS (Camp_2_Top settled on the stack; Camp_2 by Rook) |

Marlow
