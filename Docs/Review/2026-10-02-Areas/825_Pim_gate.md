# 8.25 Camp 2: gate step 4 (Pim, 2026-10-03)
Sources: Docs/Captures/Main3Review_camp2 (run 2026-10-03 00:55; frames 01 to 20 of that run, older numbered frames in the folder ignored), Checks.md, index.md, Camp2Layout.md draft 2, Camp2Layout_UI.md draft 1, main3_8_25_camp2.cs, PlayerInteractor.cs, InputSystem_Actions.inputactions. Bar: found rule (Gate.md 4 and Checks.md: 30 m, 45 deg of travel, 1 deg tall, clear by meshes) plus a read by eye; each task 5 presses or fewer.

**Verdict: FAIL** (2 items: the handset reads wrong, the ring box cannot be reached).

## Input facts (verified)
- Interact: E / pad Y (buttonNorth). One eye ray, reach 2 m, first hit wins, triggers ignored (PlayerInteractor.cs).
- Look: pad right stick or mouse delta only. **No keyboard Look binding.** Keyboard-only cannot turn or pitch, so no world point that needs a turn or a pitch is usable keyboard-only. Not an 8.25 fault (carried from 8.24); counts below are pad and keyboard plus mouse.
- No Interactable code exists for any Camp 2 point (no phone, barrel, talk or ring box script). Every prompt below is on paper; a build shows none today.

## Interactables
| Point | Found (rule) | By eye | Ray | Presses, pad / kb+mouse | Result |
|---|---|---|---|---|---|
| Payphone (booth) | 29.4 m from the boathouse leg, 1.6 deg off, 6.2 deg tall | PASS: lit booth centred (Found_camp2_08) | n/a | walk, no press | PASS |
| Handset and hook | 2.1 m from the table-to-booth walk (Found_camp2_09) | **FAIL.** The booth mesh carries its own payphone on the back wall: body, dial and a hanging handset, clearly a phone. The live receiver (Layout825/Handset) is a 7 cm black slab on the south jamb, black on black, unreadable. A player aims at the back-wall phone; that ray meets the booth wall and shows no prompt. Camp2Layout C2 said Rook covers the mesh's phone; the recipe does not | PASS: from (299.6, 99.2) facing 180, pitch -17, meets Receiver at 0.75 m (Checks HOOK). Needs pitch: not keyboard-only | Ringing: walk, Y / E, 1 press. During a hand, from your seat: leave the card game (Minigames, private, count unverified), 3 m, then 1 press | FAIL |
| Barrel `Take water` | 18.6 m from the boathouse leg, 36.1 deg off, 3.7 deg tall | PASS. Found_camp2_14 (18.6 m) hides it under the ring on dark rock, but it reads clearly in Found_camp2_20 (16 m, same leg) and Warp W7 N | PASS: stand (292.3, 100.4) facing 318 meets Barrel at 0.87 m. Barrel top 1.19, eye 1.6: needs pitch down about 25 deg (check's pitch not printed, unverified) | walk, Y / E, 1 press | PASS (wording `Take water` still Grant's, Camp2Layout 8.3) |
| Ring box in the tent | not in the area check's places list | not seen: no frame shows the tent's inside | **FAIL.** RingBox is a 0.07 m Slab with no collider (PlaceKit.Slab, collide false) at (289.35, 108.0), inside the tent's FitExact box (about x 288.10 to 289.62): the ray hits the tent first and the box has nothing to hit | none possible. No doc gives its prompt word or what a press does (my UI doc omits it; Camp2Layout C1 says only "reached from the door", Wren) | FAIL |
| R2 at the table `Talk`, `Deal me in` | his seat 16.4 m, 36.8 deg off | his chair reads in W7 E | PASS: from (298.5, 98.7) facing 136, first hit his chair at 1.0 m | Y, then ACT reply: 2 presses | PASS on paper (no NPC built) |
| R2 on top `Talk` | his chair 6.2 m from the PS1 walk | PASS (AreaFrames F5, W24 N) | PASS: from (293.8, 109.5) facing 50, first hit at 1.2 m | Y: 1 press | PASS on paper |
| Hook to R2-at-table spacing | | | edge 0.29 m, Wren's exception (C2); ray from the booth mouth hits the receiver first | | PASS |
| Guest chair, third place, table props, lantern, tent lamp, letters | | | off the mask; R2's ray is not taken by his own chair's neighbours (first hit itself) | none | PASS |
| Start blaze, phone pole, food lockers | 27.1, 30.0, 25.8 m (Checks) | frames 17, 15, 16 | none | none | PASS |
| PS1 to PS3 (not interactables) | 11.0, 12.6, 16.0 m | PS3: nothing reads at the ring on the talus top (Found_camp2_20); paper is 1.1 deg tall | none | none | rule PASS; by eye unverified (dressing, 11.0) |

## Warps
| Warp | Name (DevWarpLabels.cs) | Landing (Checks) | Facing | Result |
|---|---|---|---|---|
| Camp_2 (294.3, 95.0) | "Camp 2" | PASS, fell 0.17 m, terrain 4.00 | 55 (recipe). Booth bears 55 and the table 57 from the spot, 7 m: both centred. W7 E (90) shows booth, table and chairs clear; W7 N shows stack and barrel. The facing-55 frame itself was not captured: unverified by frame. index.md lists RedPine1 0.0 m in the E direction; W7 E shows no trunk in the way | PASS by geometry |
| Camp_2_Top (294.5, 24.2, 107.2) | "Camp 2, top of the stack" | PASS, fell 0.17 m, on GraniteStack 24.00; railed, flood reaches the ramp foot | 20 (recipe). His chair bears about 8 deg at 3.1 m, highway beyond: W24 N shows chair, rail and road. Facing-20 frame not captured: unverified by frame | PASS by geometry |
Warp press counts in the dev panel: not rerun for 8.25 (not gate tasks; Camp_2_Top was 15 presses at 8.24). Gate menu tasks (day/night, warp to Ward): no menu changed in the 8.25 recipe, so not retested.

## To close
1. Handset: put the live handset on the booth mesh's own phone (its collider on that handset), or remove or cover the mesh phone so the kitbash one is the only phone. Then one frame from the booth mouth at eye height, no ball.
2. Ring box: Wren or Quill states its prompt and what a press does (private if it touches the story); Rook gives it a collider reachable through the tent door (tent box cut back or the box moved outside its fabric) and a frame from the door. I add it to Camp2Layout_UI.md once the word exists.
3. Facing frames for both warps (Camp_2 at 55, Camp_2_Top at 20) to close the unverified lines.
4. Open, not 8.25: keyboard-only has no Look binding (Grant decides whether keyboard-only must play the world, or the bar is keyboard plus mouse).

Pim

## Round 2 (2026-10-03)
Sources: Checks.md lines 8 to 13 and 71 to 77; AreaFrames_camp2_08 to 11; Found_camp2_09.

**Verdict: PASS** (all round-1 fails closed; notes below are not blocking).

| Item | Evidence | Result |
|---|---|---|
| 1. Phone | Checks: ray from (299.7, 99.1) meets Payphone/Telephone_Booth/Handset at 0.59 m, prompt "Lift the receiver"; 121 of 121 looks within 10 deg from the stand meet it first. Only one phone now: the back-wall phone is the live one. 1 press (Y / E), pad and kb+mouse. Note: in AreaFrames_08 (0.6 m) and Found_09 (2.1 m) the cover reads as a plain black panel with the mesh's two grey rings; no handset shape. The prompt carries the task; the look goes to Vesper / dressing 11.0. Note: Found_09's ball still sits on the south jamb where the kitbash handset was, so the Hook place in Main3Areas.asset may still point at Layout825/Handset; if that slab still exists, Rook removes it and retargets the place (unverified, scene not read) | PASS |
| 2. Ring box | Checks: RingBox collider x 290.0 to 290.1, top 24.1, in the doorway; ray from (290.3, 108.0) meets it at 1.55 m, first hit itself, prompt "Examine". AreaFrames_09 shows it by eye as a small dark red block at the door sill, below the reticle; that frame's own ray (level) meets the tent at 1.0 m, so the player must look down at it (about 75 deg, inside the 85 deg pitch limit). 1 press. Note: it now sits in the doorway, not inside the tent; whether that still meets Wren's "stays in the tent" is Wren's call. What Examine shows is still unwritten (Quill, private) | PASS |
| 3. Prompts | Checks: "Take water" barrel 0.87 m, "Talk" R2 table 1.00 m, "Talk" R2 top 1.23 m, plus the two above: five of five show from their stands. "Deal me in" is right as a reply, not a prompt (Camp2Layout_UI.md row 2, Dialogue.md): Y, then pick the ACT reply, at most 3 presses with 3 replies on screen; 5-press bar met | PASS |
| 4. Warp facing | AreaFrames_10, Camp_2 facing 55: lit booth centred, table and three chairs in front, path on the left; reads as the place on landing. AreaFrames_11, Camp_2_Top facing 20: his chair at the rail, the highway and lot lights beyond, rail across the view | PASS |

Still open, not 8.25: no keyboard Look binding (round 1, Input facts).

Pim
