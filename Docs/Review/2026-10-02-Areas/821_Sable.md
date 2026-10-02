# 8.21 gate, design (Sable), 2026-10-02
Against CampLayout.md draft 2, CampLayout_Story.md (Quill) and PLAN 8.21 ("its layout serves the loop ... with times stated"). Capture Docs/Captures/Main3Review_camp (11:02, at 6d06013) and its Checks.md; recipe Tools/Recipes/main3_8_21_camp.cs read for intent. Read only; no Play run by me. Marlow, Pim and Vesper reports read.

**FAIL.** Two blocks, three fixes. The room, the wake and the privy are right. The home still burns a free fire outside, and nobody has shown the camp from the deck.

## 1. The day at home, as built
| Leg | Built | Verdict |
|---|---|---|
| Wake | Spawn 0.00 m off (4.9, 3.2), facing 245; door open at -90, into the room (Checks). W2 frames: door on daylight (S), desk, lamp, window, stove pipe (W), shelf and bunk (N). Tower in the window from the bunk: the window at Z 2.25 still holds it (line from the wake eye through Z 1.75 to 2.75 lands at world z 164.5 to 167.5 at the tower, legs z 162 to 170). | PASS. The 245 first frame is not captured (Pim 1); I pass on the math and the spawn check. |
| Tower | Wake to stair foot is walk only, 0 presses. Stair walked to the deck in 24 s (doc 22.4). Lectern not built (Milestone 10). | PASS on layout. Step count not stated (3.3). |
| Warmth | Woodpile 6 items on the east wall; stump with axe kept at (182.8, 165.4); both porch end railings gone (recipe), so stump to stove runs along the porch. Stove collider 0.7, path to its front arrives. Flood: 0 traps, so the hedge strip by the woodpile no longer dead-ends a player. No frame shows woodpile or stump (Vesper). Stump not in the reach list (Pim). | PASS on layout, with 3.2 and 3.4. |
| Evening | Desk under the window: partly. See 3.1. | FIX |
| Night to J | Porch west end open (privy walk passes it); flood clean; Camp to J reads by day (grey diff 23). Night N2 0 of 8 and no desk light in the window from outside: both Milestone 11 (DECISIONS 2026-10-02). | PASS for 8.21. The night leg's feel ("the desk light behind you") is owed in 11. |
| Privy | (176.5, 178), door west, 1.0 m opening, floor at the door; walked in and stood on. | PASS |

## 2. Blocks
1. **The fire pit burns.** Position is right: in the Pairs frame from (172, 150) facing (154, 186), the flame sits dead centre, which is the bearing to (169, 156); at (172, 163) it would sit 27 degrees right. Pim 3 and Vesper 3 read the position off the area check's place marker, which is stale. But the flame and its night light are on in both Day one and Night, at 6d06013, though the recipe's step 3 removes FirePit and turns off FX_Flames and FirePitLight. Something missed (other flame objects, or the cold step not run before this capture; unverified which). Why it blocks: a fire burning at home every morning says Warmth is here for free. Draft 2's home gives Warmth only by the chore (stump, armload, stove). Fix: no flame, no light, no FirePit on the pit; recapture W1 N and the pair. If Grant answers open 1 "lit", this becomes a pit with a prompt 3 m or more off the door to tower line (Pim), and Warmth math must say what it gives.
2. **Camp seen from the deck: unproven.** Section 5 wants the cabin roof, stovepipe, woodpile, porch and fire pit seen below. Rays say 128 of 128; both frames show only tower timbers; no compass view pitches that low. I agree with Pim 2 and Vesper 8. It matters to the loop: the deck is where the keeper sees his home port (Dredge), and from there the night walk later reads as leaving it. Fix: frames from the east and south rail at eye height, pitched down to each target, and a renderer-based count (the rune post pixel check, inverted: pass is target pixels over 0). If every rail eye truly fails, I redraw: a rail spot on the east walkway that overhangs the camp side, not a dropped requirement.

## 3. Fixes (cheap, before the tick)
1. **Desk on the window.** Rook's window stays at Z 2.25: accepted. The desk does not follow it: recipe puts the desk and chair at Z 1.0 to 2.2, chair centre Z 1.6. Seated, the window is all north of the eye; a line through it lands north of the tower at the tower's east legs (doc Z 2.9 to 11.7 at X -7, legs at Z -3.75 to 4.25). Quill 3 ("the tower in the window when seated") fails on paper. Fix: desk, chair, report box, lamp and the water jug under it to **Z 1.65 to 2.85**, centred on the window. Desk end to the stove hearth (Z 3.35) is 0.5 m, under the 0.6 band; the walking floor X 1.3 to 5.4 is untouched. Proof: one seated-eye frame, west, tower in the glass. Seat eye X unverified by me.
2. **Place markers.** main3_areas_setup.cs camp places: Fire pit (172, 163) to (169, 156); Generator (183, 173) to (181, 172.8), as the recipe builds them. Add Stump (182.8, 165.4) (Pim). Two reviewers were misled by the old marks.
3. **Tower step count.** Quill 14 asks a fixed, countable number. Rook states it in this gate (treads per flight, total); I write it into CampLayout. No build change unless the count is not constant per flight.
4. **Woodpile and stump frame.** One eye-height frame from the porch east end showing the stump, axe and the stacked wall (Vesper). Layout proof, not dressing.

## 4. Rook's deviations
| Deviation | Call | Why |
|---|---|---|
| Window stays at Z 2.25 | Accept, with 3.1 | The wake view keeps the tower; moving the desk is cheaper than rebuilding wall modules. |
| Privy open doorway, no leaf | Accept for Milestone 8 | Opening faces west, away from the cabin, so the cabin never sees in. No 0.6 to 1.0 gap. A leaf is Quill's to ask for in 11 if an event needs a door that bangs (Fears to Fathom's outhouse). |
| Fire pit goes cold | Accept; not done as captured | Block 2.1. It is draft 2's design; the capture shows it lit. |

## 5. Quill's layout items
| # | Item | Built | Verdict |
|---|---|---|---|
| 1 | Bunk sightline: door, stove and kettle, desk in one look | spawn 245; frame math (Marlow) | PASS, frame owed (Pim 1) |
| 2 | Stove, room to crouch, firebox to the room | NW corner, 0.7 collider, path arrives | PASS |
| 3 | Desk under the west window, tower in it seated | desk Z 1.0 to 2.2, window Z 2.25 | FIX 3.1 |
| 5 | Supply shelf | counter and shelf X 2.6 to 3.4, 0.5 m to the bunk | PASS |
| 6 | Washstand by the door, no mirror | X 4.0 to 4.6, Z 0 to 0.5, 0.5 m off the jamb | PASS |
| 7 | Lamp hook inside the door | not built; 0.5 m of wall free between jamb and washstand | Open (draft; lamp's day place is open 3) |
| 8 | Door in, stands open, can slam | startOpen, fixedSwing into the room | PASS (slam is an event, 10) |
| 12 | Privy at the edge, door away | 4 | PASS |
| 14 | Tower foot seen from the door; stair seen through the legs; hatch; step count | legs and stair read from the clearing (Vesper B); count not stated | FIX 3.3 |

## 6. Walk times (2.5 m/s)
Built and timed: only the stair (24 s up). The rest is Marlow's paper measure on the built colliders. No loop step needs a timer; these are stated for PLAN.

| Step | m | s | Source |
|---|---|---|---|
| Wake to door | 3.7 | 1.5 | paper |
| Door to stair foot, by the SW gap | 20.2 | 8.1 | Marlow |
| Stair up | 56 | 24 | timed |
| Deck: hatch, cab, lectern, back | 36 | 14.4 plus 20 of looks | paper |
| Stair down | 56 | 24 | assumed as up |
| Stair foot to stump, south of the porch | 28 | 11.3 | Marlow |
| Stump to stove with the armload | 9 | 3.6 | paper |
| Water, door to pump and back | 240 | 96 | Marlow |
| Night, door to the J mouth (31) plus Camp to J (77) | 108 | 43 | Marlow, capture index |

Camp-only day: 87 s walking plus 20 s of looks, the split and the light. The tower and Warmth together cost under two minutes; Water alone costs one and a half. That is the right order: home is close, everything else is a walk. Marlow times steps 2, 6, 7 and 9 with PlayerController.Step on the next run.

## 7. Open questions
1. Grant: fire pit cold, no prompt (draft 2, Status 2026-10-02), as a dated line in DECISIONS. Until then 2.1 stands as written.
2. Grant: "the Ward" hidden from the tower means the stones and rune post, not the path (CampLayout open 2). The stair zigzag reads from the deck.
3. Wren: is the night look (N2, desk light in the window) in 8.21 or wholly 11? I say 11.
4. Quill: privy leaf in 11 or never; lamp hook place.

Sable
