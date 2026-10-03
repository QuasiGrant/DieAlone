# 8.27 Cave and ravine: gate step 4 (Pim, 2026-10-03)
Sources: Docs/Captures/Main3Review_cave (run 2026-10-03 07:56): Checks.md, AreaFrames_cave_01 to 08, Found_cave_01 to 16 of that run (older numbered frames in the folder ignored); CaveLayout.md draft 2; CaveLayout_UI.md draft 1; main3_8_27_cave.cs (warps); DevWarpLabels.cs. Frames only; Grant is in Play, the Editor was not touched. Bar: found rule (30 m, 45 deg of travel, 1 deg tall, clear by meshes) plus a read by eye; each task 5 presses or fewer. Wren's calls applied: the chair judged at its centre; the toilet exempt from trail-find.

**Verdict: PASS** (no blocking item; unverified lines listed under "To close").

## Input facts (verified)
- Interact E / pad Y; one eye ray, reach 2 m, first hit wins. No keyboard Look binding, so counts are pad and keyboard plus mouse (open from 8.24).
- Prompts are stand-ins that show whenever the ray meets them; the day and state conditions in CaveLayout_UI.md 1 (from day 2, table open, not carrying, by day only) wait on the loop code.

## Interactables
| Point | Word (UI doc) | Ray (Checks PROMPTS) | Found | Presses, pad / kb+mouse | Result |
|---|---|---|---|---|---|
| Guest chair | "Sit" (match, 1.5) | from (92.0, 12.0), the standing point, at the chair's centre: GuestChair at 0.9 m. SIDE ROOM: from the warp stand (91.0, 12.0) at its centre, 1.83 m | 28.4 m from the descent walk, 1.0 deg off, 2.0 deg tall; by eye AreaFrames_05 (from the doorway: chair back, table, lamp spill, the most readable thing in the room, as 4.5 asks) and AreaFrames_07 | walk, Y / E: 1. Get up (1 press, 2.3): no table code yet, unverified | PASS |
| R7 | "Talk" (match, 1.3) | from the talk stand (86.9, 6.2) facing 98, 18 down: StandInBody at 1.4 m; TALK: head 1.52 m, bearing 97.6 | seat shelf found 23.0 m from the descent walk, 15.1 deg off. No visible body: AreaFrames_04 shows the shelf, no figure (unverified by eye until a body exists) | 1 | PASS on paper |
| Event 16, open day | "Examine" (UI doc left the word open; Examine is the Examine.md word, accepted) | from (101.0, 14.0) facing 0, level: DeadEnd at 1.8 m | deeper opening 12.3 m from the chamber-to-side-room walk, 8.4 deg off, 9.8 deg tall. In Found_cave_16 the target sits on the doorway's north jamb edge: the line grazes it. That frame shows the shut state; no open-day frame from inside the side room | walk through the opening, 1 | PASS (by eye unverified) |
| Event 16, shut rock | none (match: no prompt on other days) | from (96.5, 13.8) facing 90: none on DeeperClosed at 1.00 m; open, at the opening: none | n/a | 0 | PASS |
| Day-one board, bulbs, rope rail, lantern, table props, his chair | none (match) | off the mask (SPACING lists 3 points only) | boards 15.6 m, bulbs 16.8 m, rail 17.8 m | 0 | PASS |
| Toilet | none | n/a | exempt (Wren 2026-10-03); lid unseen from P40 to P84 | 0 | PASS |
Spacing: 3 points, each ray meets itself first from its approach. PASS.

## Side room in and out
- **In:** walks PASS: mouth to the chamber 76.0 m, passage end to the doorway and the guest chair stand 20.0 m. Keep-clear strips PASS (chamber z 10.8 to 13.2; side room). AreaFrames_04 from the passage end (71, 12) heading 90: the chamber is near black; the doorway shows only as the bulb string and a faint gap, not the "brightest rectangle on the east wall" of UI doc 4.4. The line is straight and the bulbs lead to it, so the way in holds; the readability is Vesper's to grade. AreaFrames_05 from the doorway: chair, table and lamp read. PASS.
- **Out:** AreaFrames_06, standing point (92.0, 12.0) heading 270: the doorway fills the centre, framed, with the bulbs over it, as UI doc 2.4 asks. Forward only to leave: PASS. The exit passage at (71, 12) from the doorway heading 270 (UI doc 3.5) has no frame; through the doorway in AreaFrames_06 the chamber beyond is dark and the passage does not read. Walk passes; by eye unverified.

## Mouth found rule from the spur (UI doc 3.1 and 3.2)
- The strip ask was built: MOUTH STRIP, nothing over 0.3 m in x 52 to 54.5, z 38 to 46.5 (CS_Stone_8 removed).
- AreaFrames_01, (56.31, 46.60) heading 222, 10 m out: the boarded opening and the red `CLOSED - UNSAFE` sign read clearly between the jambs, dead ahead. PASS on day 1.
- Note: Checks' Mouth line ("from walk: the descent 15.9 m") and Found_cave_01 are taken from inside the passage looking out at the boards. That is not evidence from the spur. The checker seems to keep the farthest walk that finds a place, so the spur line is masked. Ask Rook to print the spur (W1 to cave) line for the mouth on its own. Day 2 (boards down, the void) has no frame: unverified.

## Warps
| Warp | Name (DevWarpLabels.cs) | Landing (Checks) | Facing | Result |
|---|---|---|---|---|
| Cave_Mouth (58, 44) | "Cave mouth" | PASS, fell 0.12 m, terrain -4.36 | 223 (recipe); the opening's centre (52, 37.9) bears 224.5, so dead ahead at 8.6 m. No frame at the warp spot; AreaFrames_01 (2.7 m north-west, heading 222) shows what it faces | PASS by geometry |
| Cave_Chamber (74, 12) | "Cave chamber" | PASS, fell 0.17 m | 90: the doorway 15 m ahead on the line. Proxy AreaFrames_04 (from 3 m behind): dark, bulbs mark the doorway | PASS by geometry |
| Cave_SideRoom (91.0, 12.0) | "Cave side room (table)" (UI doc 5 row, as asked) | PASS, fell 0.17 m, blocked ahead 2.0 m (the chair) | 90, AreaFrames_07: guest chair back in the foreground, table with candle and glasses, his chair empty beyond, lamp over. Reads as the table on landing. Pitch is kept from before the warp (DevMenu): a level arrival shows the chair at the frame's foot | PASS |
| Spur_Descent (77.3, 2.3, 48.6) | "Cave spur, the descent" | PASS, fell 0.16 m, blocked ahead right 2.1 m | 250, AreaFrames_08: the trail curving down to the left with the rope rail on the drop side and a boulder on the right. Reads as the way on; the cave is out of sight by design | PASS |
Warp press counts in the dev panel: not gate tasks; not rerun (Grant in Play). Gate menu tasks (day/night, warp to Ward): DevWarpLabels.cs gained two rows, both at the end of the list (CAVE group last); the Ward row is FirstWarp, above every group, so its press path does not change. PASS by code read, not rerun.

## To close (none blocking)
1. Rook: print the mouth's found line from the spur (W1 to cave) separately; one day-2 frame of the open void from AreaFrames_01's spot.
2. One open-day frame of the deeper opening from inside the side room, and one from the doorway heading 270 toward the exit passage.
3. Get-up press (UI doc 2.3): measure when the table code exists.

Pim
