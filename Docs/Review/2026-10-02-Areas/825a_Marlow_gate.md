# 8.25a gate, Marlow: checklist from the sheets and hand walk (Camp 2, the granite knob)
2026-10-03, Marlow. **0 blocks, 3 hurts, 5 cosmetic. 0 traps.**
- The scramble walks both ways without jumping.
- Every top edge drops to the floor and walks out.
- The chair sees all three targets.
- The hurts: the ring box is a 3 x 3 degree target; by day the deck can't make out the lamp at 960 px; the knob reads as a black box from the trails.

**Setup:**
- Sheets: Docs/Captures/Main3Review_camp2 (capture 13:40).
- Scene: at 394a732; I changed nothing.
- Play: one session, entered at 13:43 (the Editor was stopped and not compiling) and stopped at 13:52.
- Mover: every move is PlayerController.Step (dt 0.02).
- Frames: rendered from a copy of the game camera into the scratchpad (k825a); not committed.
- **Test note:** a teleported body can catch its own interactor ray. Looks pitched 20 degrees or more down then hit "Player" at 0.03 m. After walking in with Step that never happens, so every prompt figure below is taken after walking to the stand.

## Gate step 2 checklist (from the sheets)
| # | Item | Result | Frames |
|---|---|---|---|
| 1 | Every trail both ways: path reads apart from ground | PASS. The tread reads in every frame. Frames are every 10 m (the gate asks for 5 m) | Trail_Camp_2_to_T, Trail_Boathouse_to_Camp_2 |
| 2 | Every invisible stop has a visible reason | PASS for this area | Stops_1 |
| 3 | Every trail end and warp: the path goes on, or ends at a place | PASS. Camp_2 N and E show the knob, booth and table. Camp_2_Top shows the lamp, the tent and the trees toward the road | Warps_NESW, Trail_Ends |
| 4 | Every place, outside and in: size and purpose read | **Hurt (look).** The booth, table and chairs read. **The knob does not read as a granite knob.** From both legs (F4, F5; Camp 2 to T FWD 10, BACK 50 and 60) it is a flat-topped black box with rounded boulders at its foot. **Stale frame:** Places "Camp 2 stack top" still points the camera at the old stack top's position and shows only grey | AreaFrames F4, F5, Places |
| 5 to 6 | Lot road; climb legs | not this area | |
| 7 | Top-down plus compass views | PASS for this quarter | Compass_Views |
| 8 | Day and night pairs | no Camp 2 pair spot on this sheet; the deck lamp pair is item 7 below | DeckLamp_* |
| 9 | F1 by keyboard and pad | Pim's | |
| 10 | Hand walk | below | |

## Hand walk, asked items
1. **The scramble up and down, no jumps. PASS, with two side-lane stalls (cosmetic).**
   - **Lanes:** I walked from the foot (288, 113.6), up leg 1, onto the landing, up leg 2 to the head (291.2, 120.7), across the top to the talk stand (291.3, 123.2), and back. Three lanes, 0.5 m left of centre, centre and 0.5 m right; walk and sprint, both ways.

     | Lane | Up, walk | Down, walk | Stalls |
     |---|---|---|---|
     | -0.5 | 16.2 m, 6.6 s | 6.5 s | 0 |
     | centre | 15.7 m, 6.4 s | 6.3 s | 0 |
     | +0.5 (east) | 6.9 s | | 1 walking up, 2 sprinting up; 0 down |

   - **Stall 1:** at (288.38, 6.82, 120.23), the top of leg 1 into the landing. The leg 2 ramp box (x from 288.57) and the core's 0.25 m cells at x 288.80 to 289.80, z 120.00 to 120.50, pinch the lane there.
   - **Stall 2** (sprint only): on leg 2's north edge at (289.22, 7.42, 121.57), against the gully wall cell x 289.55, z 121.75.
   - **Leg 2 alone:** from the landing to the head at five lateral offsets (-0.6 to +0.6), 1.1 to 1.5 s each, no stall, ending on the top at y 9.0.
2. **Getting stuck: none.**
   - **Sweep:** every 0.5 m over x 282 to 306, z 108 to 134, 12 headings in walk, sprint, walk-jump and sprint-jump. That is 107,808 runs from 2,246 starts, covering the knob foot, the north side where Sequoia1 stood, both rockfall wedges, the gully and its head.
   - **Off-ground ends** (87), every one escaping on a 16-heading retry in four modes. **0 trapped.**
     - Leg1Ramp: 54 (walking onto the ramp from the ground).
     - BoulderField Boulder_0: 9.
     - RedwoodHollowLog_0: 24.
3. **Sprinting off every edge. PASS.**
   - **Runs:** 58 points every 0.5 m round the top's outline, sprint and sprint-jump at 0 and ±35 degrees out: 300 runs.
     - 283 land on the floor.
     - 2 stay on the top.
     - 15 land on rock part-way down: 8 on the north skin BigBoulders_1 (y 5.2 to 8.4) and 4 on the rockfall Boulder_2s at (301, 5.3 to 5.7, 122.7 to 126.2). The other 3 I did not list.
   - **Escapes:** from each of those ledges, 30 to 46 of 48 moves reach the floor. Nothing perches.
4. **Prompts.**
   - **Talk from the stand facing 150. PASS.**
     - I walked in to (291.36, 9.04, 123.23).
     - 1,737 of 4,941 looks on a 1 degree grid (yaw 120 to 180, pitch -10 to 70 down) show `Talk` on HisChair.
     - The window is yaw 132 to 172 and pitch 14 to 58 down: easy.
   - **Ring box from the tent door. PASS on reach; hurt on how hard it is to hit.**
     - I walked in to (295.60, 9.04, 120.37).
     - Only 9 of 4,941 looks show `Examine`: yaw 179 to 181, pitch 30 to 32 down.
     - The ring box collider is 0.07 m across at 1.79 m. A player has to find a 3 x 3 degree spot with nothing on screen to aim at.
5. **The view from his chair. PASS.**
   - Seated eye (292.0, 10.2, 122.0), a 2 m magenta cube on each target, 960 x 494 render:

     | Target | Distance | Pixels seen |
     |---|---|---|
     | highway | 152 m | 167 px |
     | gate T stop sign | 138 m | 116 px |
     | barrier arm | 108 m | 114 px |

   - **In the frames** (chair_to_* and chair_facing_65), the lot, the building and the highway lights show through gaps in the trunks.
   - **Standing at the talk stand** loses the highway (0 px), and so does the top's east edge. The view is the chair's only.
6. **Trail times. PASS, no stall either way.**

   | Walk | Distance | Time |
   |---|---|---|
   | Boathouse to Camp 2, by the markers | 93.0 m | 37.2 s, both ways |
   | Camp 2 to T, by the markers | 87.9 m | 35.1 s, both ways. Rook prints 94.6 m by his own route |
   | Trail end (298.9, 107.8) to the talk stand, by the scramble | 27.9 m | 11.2 s |
   | Talk stand back to the trail end | 27.8 m | 11.1 s |

7. **Can I see his lamp from the deck rails? By day, no. At night, Rook's frame only.**
   - **The core:** LampCore at (297.00, 11.13, 121.20) is 0.35 x 0.45 x 0.35 m.
   - **Magenta copies at 960 x 494 (day look),** 20 standing rail eyes, five per side: a magenta copy of the lamp, and of the core alone, shows 0 px from every eye.
   - **Control:** at the same point a 1 m cube shows 53 to 67 px from every S and E rail eye, and from two N and one W eye. The line is clear, but at 145 m a 0.45 m core falls under a pixel after the look filter's downsample.
   - **Night:** Rook's DeckLamp_Night_Eye (3840 x 1976, from (167.5, 57.6, 162.5)) gives the core 73 grey over a frame mean of 12. In that frame I count several separate lights on the far side and cannot tell which dot is his lamp.
   - **What I did not test:** the night look, or Grant's Game view size.

## Hurts
1. The ring box is a 3 x 3 degree target from the tent door (item 4).
2. By day the lamp is under a pixel from every rail at 960 px (item 7). At night it rests on Rook's 4K frame.
3. The knob reads as a black flat-topped box from both trails (checklist item 4).

## Cosmetic
1. Two side-lane stalls on the scramble, at the leg 1 to landing joint (east lane) and leg 2's north edge when sprinting (item 1).
2. The lamp reads as a white square on a post from the chair and from the top warp (frames chair_*, Warps_NESW Camp_2_Top E).
3. The Places sheet's "Camp 2 stack top" frame is stale: grey sky from the old stack's height.
4. F8 (trail end heading 300, the scramble foot) shows the knob's west face; the scramble itself does not stand out.
5. Trail sheets are every 10 m, not 5.

## Done-check (my lines)
| Line | Result |
|---|---|
| Marlow's flood, trap and walk-into checks find 0 problems in it | PASS: 0 traps in 107,808 sweep runs and 300 edge runs; the scramble walks both ways |
| every warp in it lands | PASS (Rook; Camp_2 and Camp_2_Top frames read) |

Marlow
