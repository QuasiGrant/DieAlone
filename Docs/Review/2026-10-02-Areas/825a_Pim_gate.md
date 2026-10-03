# 8.25a Camp 2, the knob: gate step 4 (Pim, 2026-10-03)
Sources: Docs/Captures/Main3Review_camp2 (run 2026-10-03 13:40): Checks.md, AreaFrames_camp2_01 to 13, Found_camp2_01 to 20; main3_8_25a_knob.cs and main3_8_25a_camp2_check.cs headers; DevWarpLabels.cs; Camp2Layout_UI.md draft 1 for the words. Frames only, Unity not touched. Bar: found rule (30 m, 45 deg of travel, 1 deg tall, clear by meshes) plus a read by eye; each task 5 presses or fewer.

**Verdict: FAIL** (1 item: the way up the knob does not read by eye from any frame; the rule passes).

## Input facts (verified)
- Interact E / pad Y; one eye ray, reach 2 m, first hit wins. No keyboard Look binding: counts are pad and keyboard plus mouse (open from 8.24).
- Prompts are stand-ins on chair boxes and props; R2 has no body.

## Interactables
| Point | Word (UI doc) | Ray (PROMPT line) | Found | Presses, pad / kb+mouse | Result |
|---|---|---|---|---|---|
| Hook | "Lift the receiver" (match) | from (299.7, 99.1), bearing 94, 2.3 down: Telephone_Booth/Handset at 0.58 m; 121 of 121 looks within 10 deg meet it first | payphone 29.4 m from Boathouse to Camp 2, 1.6 deg off; hook 2.9 m from the table walk. AreaFrames_12: the booth reads from the warp | walk, 1 | PASS |
| Barrel | "Take water" (match) | from (292.3, 100.4), bearing 318, 37.3 down: Barrel at 0.87 m | 30.0 m from Camp 2 to T, 33.3 deg off, 2.3 deg tall (the checker keeps the farthest line; it was also found at 18.6 m from the boathouse leg in 8.25); by eye in Found_04 (left of the trail) | 1 | PASS |
| R2 at the table | "Talk" (match) | from (298.5, 98.7), bearing 136.2, 37.5 down: CardTable/HisChair at 1.00 m | his seat 16.4 m from Boathouse to Camp 2 | 1; `Deal me in` is a reply after it (UI doc), at most 3 more | PASS |
| R2 on top | "Talk" (match) | from the talk stand (291.3, 123.2), bearing 149.7, 35.7 down: Layout825a/Top/HisChair at 1.25 m. Facing 150 is right: the chair (292.0, 122.0) bears 150.3 from the stand. main3_8_25a_knob.cs line 16 still says the stand faces 120; if the marker was set to 120, it disagrees with the check (scene not read) | his chair found only from the walk on the top (4.7 m), "from the trails: none". The knob is found from Boathouse to Camp 2 (29.9 m) and the scramble head (29.3 m), so the top is found; the chair is a find on the top, like the hook in the booth. AreaFrames_09 shows chair, tent and lamp from the head | climb, then 1 | PASS on the press; see the scramble below |
| Ring box | "Examine" (the 8.25 round 2 stand-in word; UI doc has no line for it yet) | from (295.6, 120.3), facing 180, 31.1 down: Layout825a/Top/RingBox at 1.73 m | hidden from the trails by design (inside the tent, seen from its door, Camp2Layout K3). AreaFrames_11, the tent door facing north: through the door, a crate just inside with the box on it; the crate reads, the 7 cm box is a dark spot on its top (2.3 deg at 1.73 m) | 1 | PASS |

Spacing: 5 points, each ray meets itself first from its approach; hook to R2 at the table 1.6 m centres, 0.57 m edges (the 8.25 exception no longer needed). PASS.

Note for when R2 has a body: both Talk lines meet his chair box at about 36 down. His chair box tops out 1.2 m over the top, so a look at a seated man's head (about 1.3 m) would pass over the box today. The body's own collider has to carry `Talk`.

## The way up (the item that fails)
- Reach: WALK, the trail end across the floor and up the scramble to the talk stand, 25.5 m, PASS; T12 gully up and down, PASS.
- Rule: scramble foot found from Boathouse to Camp 2 at 30.0 m, 13.2 deg off, 3.8 deg tall; head at 29.3 m. PASS on paper.
- By eye: **FAIL.** In Found_03 (foot, 30 m) the ring sits on grass at the knob's base with nothing that reads as a way up. AreaFrames_04 (knob from the boathouse leg) and AreaFrames_05 (from the T leg) show a dark block with boulders at its foot, the tent peak and the lamp on top, and no route. AreaFrames_08 (trail end heading 300, "the scramble foot") shows grass, boulders and the knob's dark face; no scramble reads. The top says "go up"; nothing says where. Ask: one frame from the scramble foot looking up the line; and something at the foot that reads as the start of a climb from the trail end (lighter worn rock, his steps, a rope, a cairn: Sable's or Vesper's pick, not mine).

## Paper spots (not interactables)
PS1 6.1 m and PS2 6.0 m, found only from the walk up to his chair ("from the trails: none"); PS3 16.5 m from Camp 2 to T at 1.0 deg tall (at the bar). They are Quill 10's deck spots, not trail finds. Deck lines for PS2 and PS3 are "low" (0 rays); that is the deck check's call, not mine. No result from me.

## Warps
| Warp | Name (DevWarpLabels.cs) | Landing (Checks) | Facing (frame) | Result |
|---|---|---|---|---|
| Camp_2 (294.3, 95.0) | "Camp 2" | PASS, fell 0.17 m, terrain 4.00 | 55, AreaFrames_12: the lit booth centred, the table and chairs before it, the barrel right of them | PASS |
| Camp_2_Top (293.0, 9.2, 123.6) | "Camp 2, top of the stack" | PASS, fell 0.17 m, on Core at 9.00 | 65 (his own facing, toward the T), AreaFrames_13: over the knob's edge to the lot, the mast lamp and the highway. His chair and tent are behind the landing, not in frame; AreaFrames_09 shows them from the head | PASS. The label still says "stack"; the place is now a knob. Suggest "Camp 2, top of the knob" (Wren's word) |
Gate menu tasks: no new rows; Ward is FirstWarp. Not rerun.

## To close
1. The way up reads from the trail end and from the boathouse leg (above), with a frame from the foot looking up.
2. Confirm the talk stand marker faces 150, not the recipe comment's 120.
3. Warp label word for the knob.

Pim

## Round 2 (2026-10-03)
Sources: Checks.md (run 15:20) PROMPT lines and REACH AND FOUND; AreaFrames_camp2_10 (F10), 11 (F11), 12 (F12), 14 (ring box).

**Verdict: FAIL** (the way up still does not read by eye; everything else PASS).

| Item | Evidence | Result |
|---|---|---|
| The way up, F10 (boathouse leg's end) | The ring sits on grass at the horizon line. Behind it is a row of five grey rounded stones of one tone and size class (the talus). If the cairn is the shape just above the ring, it does not stand apart from them: same grey, no lighter face, no height that breaks the row at this range. The cleared grass does not show at eye height: the foreground grass (about 0.5 m) hides the ground to the stones. The knob is a dark mass on the right, and no step line shows on it | FAIL |
| The way up, F11 (T leg's start) | Same row. A faint stacked shape sits above and left of the ring against a dark trunk; it is the only thing that could be the cairn, and it is as grey as the boulders beside it. No line from it up the rock | FAIL |
| The way up, F12 (scramble foot, up the line) | A dark rock face fills the right half; one boulder overhangs at the top; the steps are dark on dark, and no tread edge or lighter step face shows a route. From the foot itself the climb does not read as steps | FAIL |
| Found rule | Knob 29.9 m and scramble foot 30.0 m from Boathouse to Camp 2 (unchanged). Scramble head now "from the trails: none" (round 1: 29.3 m) and tent "from the trails: none"; the knob and foot carry the find. Rule PASS; by eye, above | rule PASS |
| Ring box | PROMPT: from (295.6, 120.3), facing 180, 31.1 down, the ray meets RingBox/RingBoxReach at 1.61 m, "Examine", target 9.8 deg (5.0 or more). Confirmed. AreaFrames_14: the crate inside the door reads; the reach box is larger than the drawn box, so a look anywhere on the crate top finds it | PASS |
| Angular size on every PROMPT line | All five print it, all 5.0 or more: hook 73.3, barrel 54.4, R2 at the table 45.6, R2 on top 54.2, ring box 9.8. Confirmed | PASS |

What would pass, for whoever builds it (Sable or Vesper choose the look): the cairn is lighter or a different hue than the talus, and taller than every boulder in that row, from both F10 and F11. The first two step faces are lighter than the knob's face, reading as a stair from the foot (F12). Or there is a line (a worn path or a rope) from the trail end to the first step that shows above the grass.

Pim
