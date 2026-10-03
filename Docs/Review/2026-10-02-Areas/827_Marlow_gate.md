# 8.27 gate, Marlow: checklist from the sheets and hand walk (cave and ravine)
2026-10-03, Marlow. **0 blocks, 4 hurts, 6 cosmetic. 0 traps.**
- The two hurts that matter: the rim band leaves both ends open, so you can get down the north wall to the mouth; and the rope rail can be sprint-jumped.
- The 242 s walk time is an artifact of the check; the real walk is 43 s.

**Setup:**
- Sheets: Docs/Captures/Main3Review_cave (capture 07:56).
- Scene: as saved at 0ed3625; I changed nothing.
- **Play:** Play was already running when I arrived, with no job in progress. I took it to be left from Grant's walk, stopped it, and entered a clean session at 08:37. I stopped at 08:59.
- Mover: every move is PlayerController.Step (dt 0.02).
- **Runtime toggles:** DeeperClosed was switched off only inside two test runs. I switched it back each time and confirmed it was on afterwards.
- Frames are in the scratchpad (g826b 01 to 20) and are not committed.

## Gate step 2 checklist (from the sheets)
| # | Item | Result | Frames |
|---|---|---|---|
| 1 | Every trail both ways: path reads apart from ground | PASS. The tread reads in every frame. FWD 110 is pressed against the boards. Frames are every 10 m (the gate asks for 5 m) | Trail_W1_to_cave |
| 2 | Every invisible stop, from 2 m back, has a visible reason | PASS. S54 is the rope rail; S55 (CaveMouthPit) is a rock face | Stops_1 |
| 3 | Every trail end and warp: the path goes on, or ends at a place | PASS. The W1 end faces the junction and the sign. The cave end faces the passage. Cave_Mouth S and W show the boarded mouth. Spur_Descent shows the rail and tread | Trail_Ends, Warps_NESW |
| 4 | Every place, outside and in: size and purpose read | PASS. Mouth and board read, with CLOSED - UNSAFE legible from P76. The side room reads: a table for two, his chair, a light. The chamber reads as a big dark stone room; R7 and the stack are hard to pick out in F4 (Vesper's call) | Places, AreaFrames F1, F4 to F6 |
| 5 to 6 | Lot road; climb legs | not this area | |
| 7 | Top-down plus compass views | PASS for this quarter | Compass_Views |
| 8 | Day and night pairs | no cave pair spots on this sheet | Pairs_DayOne_Night |
| 9 | F1 by keyboard and pad | Pim's | |
| 10 | Hand walk | below | |

**Area frames:**
- F2 (bulbs, heading 300) shows no bulbs. That is right for the day-one look; they appear from day 2.
- F3 (leg 1 opening and the cable) is almost black.

## Hand walk, asked items
1. **Spur descent and rope rail. Walk PASS; hurt on the rail.**
   - **Walk:** W1 to the mouth by the markers is 107.5 m, 43.1 s, no stall. The mouth to W1 is 107.1 m, 42.8 s.
   - **Over the rail:** from the tread P52 to P76, every 0.5 m at -0.6, 0 and +0.6 m off the centre line, I ran 24 headings in four modes: 20,064 runs.
     - 1,771 of them reach the ravine floor (y under -4.3), directly or after up to two more sprints the same way.
     - **At P52 to P56** (x 77 to 81), east of the rail's end post (78.3, 47.9), you simply walk or sprint off the bank.
     - **At P58 to P68**, a sprint-jump clears the rail. Its rope box stands only about 0.5 m over the tread there. Example: from (75.61, 1.59, 46.28), heading 180, sprint-jump lands at (75.42, -0.53, 42.04) beyond the rail, then (75.23, -5.84, 36.74) on the floor.
     - 49 runs end standing on a RailBox top. Every one walks off.
     - P72 to P76 counts include plain walking down the trail.
   - **No trap:** the floor leads to the mouth (layout T4).
2. **The narrow. PASS.** The NarrowRow rocks (BigBoulders_1 and _2) can be sprint-jumped onto from the tread, 5 ends, and each walks off. The frame from P68 reads as rock on the right and the drop on the left (frame 12).
3. **Mouth strip. PASS.**
   - The strip to the boards is 7.9 m, 3.2 s.
   - The strip to the toilet lid and back is 9.6 m, 4.0 s.
   - The lid (0.1 m) can be stood on: 6 ends.
   - **Toilet hidden:** from the strip the lid is hidden behind brush (frame 14).
4. **Passage, niche and chamber. PASS.**
   - The passage down legs 1 to 3 to the chamber (71, 12) is 73.8 m, 29.5 s, no stall, and the same back up.
   - **Niche:** the body gets in to x 54.1, as far as the generator and cans allow. In 5,376 runs from the passage and the niche mouth, none ended deeper than x 54.5, and none trapped.
   - Round the chamber (food, sleep, stack, bank): 40.9 m, no stall.
   - Passage end to the seat stand: 16.6 m, 6.6 s.
5. **Side room in and out. PASS.**
   - Passage end to the doorway, the guest-chair stand and back: 39.0 m, 15.6 s, no stall.
   - Going round the table's south side stalls at (94.57, 11.88), on the table and chair. Go round the north side.
6. **Dead end, shut and open. PASS.**
   - **Shut:** the walk to the inspect point stops 0.61 m in, at the rock. 100 runs from the side room's east wall: none get past the rock or fall.
   - **Open:** the standing point to the dead end and back is 19.7 m, 7.9 s, no stall.
   - **Flood (open):** a flood from the side room with the passage open, 937 places in four modes, gives 0 falls. The furthest it gets is x 101.23, z 15.43.
   - **Cosmetic, unreachable void:** x 98 to 99.9, z 14.9 to 16.6 is a sealed space behind Wall_N_A and Wall_W_B. It has no north wall and no floor past z 16.6. A body placed inside it falls out of the world. It cannot be reached shut or open.
7. **Cave interior sweep. PASS.**
   - Every 0.5 m over entrance, niche, legs, turns, passage, chamber, side room and the deeper passage: 1,915 starts, 91,920 runs.
   - Ends on props, all of which walk off: cooler, crates, sleep base, heater, battery bank, seat shelf, table, chairs, the pistol and the side-room rocks.
   - The only "traps" were starts placed inside the sealed void (item 6).
8. **The rim band. Hurt: open at both ends.**
   - **Over the band:** from rim ground over x 52 to 82, 0 sprint-jumps cross it (it matches Rook's RIM line).
   - **West of the band's west end (x 51.95):** from the rim at x 40 to 50, z 58 to 61 (ground 14), sprinting south runs down the north wall. Four more sprints reach the ravine floor at the mouth.
     - Example: (47.00, 14.02, 58.00), heading 180, sprint ends at (47.16, 5.01, 50.67), then (47.78, -5.87, 40.86), beside the toilet. 181 such runs in the 1 m sweep.
     - From the slope, a collider line to the mouth (52, -4.5, 37.9) is clear, and frame 18 at (47.16, 6.6, 50.67) shows the mouth's jamb.
   - **East of the band's east end (x 82.05):** from (83 to 87, 17.3 to 17.9, 58 to 59), headings 210 and 240, the same: down to the floor at (76 to 77, 36.7).
   - **What it breaks:** layout section 3 V11 ("no walkable cell is left south of it") and "nothing on foot reaches the floor except along the spur". No trap: the floor walks out by the spur.
   - **Not proved:** I did not trace a walk from W1 to those rim starts. They are open rim ground in the sweep.
9. **Mouth found line from the spur (Pim), by eye.** Frames 01 to 09 follow the tread P70 to P80 along travel, and P72 to P76 straight at the mouth.

   | Tread point | Distance out | What shows |
   |---|---|---|
   | P70 | about 18 m | the board and jambs on the left frame edge, about 35 degrees off travel |
   | P72 | about 15 m | the boarded mouth, framed by the two jamb boulders |
   | P74 | about 12 m | sits in the upper middle ahead, on the tread's line |
   | P76 | | CLOSED - UNSAFE is legible |

   **PASS from P72 on. It reads as a doorway in rock, not as a far shape.**
   - **From inside the passage** (frame 10, z 30 looking out): the boards show as a backlit grille at the end, plainly the way out.
10. **The 242 s walk (Sable). The check's artifact, not a stall.**
    - Rook's walk with his arrive rule (break inside 0.25 m) takes 42.35 s to P82.
    - It then spends 200.0 s at P84 (52.14, -6.00, 37.62). The body stops 0.39 m short, at (52.15, -5.84, 38.01), because P84 lies inside Cave/Mouth/DayOneBoard/Plank (z 37.57 to 37.63).
    - It walks in place until its 200 s cap. The line passes because 0.39 is within arrive (0.5). 242.35 s = 42.35 + 200.
    - **Real walk time:** 43 s.

## Hurts
1. Rim band open at both ends: a second way down to the mouth (item 8).
2. The rope rail can be sprint-jumped at P58 to P68, and the bank east of the rail at P52 to P56 is open (item 1).
3. The WALK line for W1 to the mouth prints 242.3 s: the P84 marker sits inside the day-one board, and the walk idles there for 200 s (item 10).
4. F3 is nearly black; F2 has nothing to find on day one. Both frames check nothing as shot.

## Cosmetic
1. The sealed void behind the deeper passage has no floor or north wall past z 16.6 (item 6).
2. Side room: the table's south side is closed to a walk-round (item 5).
3. The side room's bulb string reads as floating orange blocks, and the strip light as a white bar (Places, AreaFrames F5 to F7).
4. The chamber floor and walls read as one tiled grey box (F4, frame 15).
5. The niche reads as a dark cupboard with an orange can and a green box (frame 13).
6. Trail sheet frames are every 10 m, not 5.

## Done-check (my lines)
| Line | Result |
|---|---|
| Marlow's flood, trap and walk-into checks find 0 problems in it | PASS on traps (0 in 91,920 cave runs and 44,352 spur and rim runs). Not clean: the rim and the rail both let a player onto the ravine floor (hurts 1 and 2) |
| every warp in it lands | PASS (Rook; the Spur_Descent and Cave_SideRoom frames read) |

Marlow
