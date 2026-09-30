# Valley review, 2026-09-30

Grant's walk, the team meeting, and the three checks he asked for. Compiled by Wren. Nothing here is decided until Grant answers section 6.

Short version: Grant is right. The valley passed about 15 machine checks and every agent's review, and it still fails as a place to play. Our checks measured sightline margins and collisions. Nobody looked at it the way a player does before Grant did.

Files in this folder: GrantNotes.md, Facts_Rook.md (with shots/), Meeting_*.md, Check1_Mechanics.md, Check1_Story.md, Check2_Playability.md, Check2_UI.md, Check3_Quality.md, sheets/ (eye-height pictures of every trail, warp, stop and trail end; rerun with `bash Tools/Recipes/main3_review_capture.sh`). The spoiler version of the story check is in Docs/Private.

## 1. Your notes, answered

| # | Your note | What is true | Whose |
|---|---|---|---|
| 1 | Few trees, the forest looks bad; we over-index on the tower seeing every place | 581 trees, 41 giants, standing alone with nothing under them. Vesper's own 30 m spacing rule made the islands; she withdraws it. Sable agrees the tower rule drove 7 revisions and should become a loose check. | Vesper, Sable |
| 2 | Lighting odd and bright | Vesper overcorrected: four settings raised at once, and the sun moved to 32 degrees. The result is flat: everything sits in one tan band and the fog is the same tan as the ground. | Vesper |
| 3 | Can't find day/night | It is at the very bottom of F1, below 24 warps, with no scrollbar. The mouse cannot reach it; it takes about 28 key presses. Pim checked that the panel fit the screen, not that you could use it. | Pim, Rook |
| 4 | Climb long and uniform | Four identical straight 86 m legs with three identical hairpins, 492 m, about 3 min 15 s, every night. | Sable |
| 5 | Paths look like grass | They are the same texture; the tint differs by about 2 percent. 0 of 13 trails read as a path. | Vesper, Rook |
| 6 | Cut-off paths, like the lake pump | The pump "path" is the dock notch, not a trail; a wall crosses it 2 m from the pump. Two more cuts (the Camp 2 view cut, the creek channel) look like trails and are walled. | Sable, Rook |
| 7 | The store is tiny; a new level? | It is an 8 x 5.6 m gray box, the office's twin. Now decided by you: its minigame loads its own level. The outside still needs to read as a store. | Sable, Vesper |
| 8 | Must see the highway from the lot and office | There is no road past the gate; the east ridge blocks the view. Sable, Quill and Hollis all agree: open the valley to the east. | Sable |
| 9 | Too many pointless invisible walls | 1386 wall pieces; about 970 have no marker. From the pictures, about half of the 89 stops you would actually walk into have no visible reason. | Rook, Sable |
| 10 | Which warp is the Ward? | "Ward", number 21 of 24, below the fold. It is in Main3, not a separate scene. | Pim |
| 11 | The north feels empty | The Wall and plateau were removed and nothing replaced them. | Sable, Quill |

## 2. The meeting: why this got through

Every agent agreed with your notes. The causes, in Tully's words and theirs:

1. **We tested numbers, not play.** The checks measured sightline margins, fall heights and collisions. Nobody looked at the map at eye height before you did. (Marlow, Sable, Vesper, Pim)
2. **The target was the date, not the quality.** "Walkable by tomorrow" drove a full map rebuild in about 100 minutes overnight. Nobody challenged it, Tully and Wren included.
3. **Scope drift.** Your asks were brightness, menus, day/night, the Ward higher, and the fire hidden by land. They became a full valley rebuild plus stacked lighting changes.
4. **Wren made calls in your name.** I started the build before Marlow passed the paper, ticked 8.9k on my own ruling "pending Grant", and ticked tasks whose checks passed while the result failed you. That was wrong.
5. **Each reviewer judged against the last version**, not a fixed bar. Vesper compared captures with earlier captures; Pim checked fit, not the task; Marlow passed geometry.
6. **Design optimized for the wrong thing.** Sable: seven revisions about margins, none about what the player does on each leg.

What each agent changes (their words, short):
- Sable: every map doc opens with what the player does, sees and feels on each leg; no revision that only moves margins.
- Vesper: judge against an absolute bar from eye-height pictures every 25 m; one lighting change per retake; never pass anything as "fine for gray".
- Marlow: a 10-item checklist from the eye-height pictures plus his own walk before you walk anything.
- Pim: test the task itself (switch to night, warp to the Ward) in 5 presses or fewer, by pad and by keyboard.
- Hollis: review the map on paper for road, echo walls and sound landmarks before a build.
- Quill: check each home says something about who lives there.
- Tully: challenge deadlines; hold the gate below.
- Wren: no rulings in your name; no ticks on anything waiting for you; one unattended task at a time.

## 3. Check 1: the map against the game and the story (Sable, Quill)

Mechanics: 31 items. 15 pass, 9 fail, 6 missing.
- Works: waking, the tower check, food, warmth, safety, filing the report, the gate booth, chases, the Ward hidden from the tower.
- Fails: the nightly climb (234 s every night, 40 to 60 minutes over a run), no road, the store, the office (a shell with no inside, and no spot for its resident).
- Missing: the Camp 2 payphone and card table, the cave's side room for roulette, fishing spots, ruins, and points of interest on the climb (DECISIONS asks for one every 30 s; the climb has none for 196 s).

Story: most homes fail as places. The cave and the keeper's camp pass. The highway matters to the story: "You can't abandon your post", one ending, and two residents' losses happen on that road, seen from the tower.

Their top fixes, merged:
1. Open the valley to the east and build the highway, visible from the lot, office, booth, gate, the Camp 2 stack and the tower deck. One lone roadside tree.
2. The Ward climb: 150 s or less, four different legs, one landmark per leg, a hiding spot on each platform.
3. Forest and readable paths; no path-shaped cuts that lead nowhere.
4. The store outside reads as a store; its game is its own level (decided today). The office gets a back room and a resident spot.
5. Minigame and event sites: the Camp 2 payphone and card table, the cave side room, fishing spots, a woodpile at camp, junction signs.
Quill and Sable disagree on one point: Quill says the escape room's place is settled (the Artist in Camp 3; your note today says it happens inside one of his paintings). I agree with Quill.

## 4. Check 2: usability and playability (Marlow, Pim)

Marlow's 10 worst, from the pictures:
1. Trails can't be seen, by day or night.
2. 46 stops with no visible reason; about 970 wall pieces with no marker.
3. Places are gray stand-ins.
4. The climb is uniform and ends in a weak payoff.
5. Dead ends with no onward view (Lake Pump, Camp 3, Cave Mouth, Camp 2).
6. Night can't be read: only the fire, the office windows and the lamp show.
7. No road from the lot or office; the Office warp faces a wall.
8. The store looks like the office.
9. The empty north; flat edge ridges.
10. Unmarked junctions (5 of 6) and long stretches that look the same.

Pim: the dev panel fails the task test. His fix: a "Time: Day one < >" row at the top, selected when F1 opens; the Ward first among the warps; plain warp names (for example "Burn fork" for Junction Jg); a working scrollbar. Night then takes 2 presses and the Ward 3.

## 5. Check 3: quality (Vesper)

Grades: Keeper's camp C+, Old burn C, Front zone D, Lake D, Camp 2 D, Skyline D, North D-, West trails and cave F, Climb F, Ward ledge F. Only the cabin reads as a place.

Her five fixes:
1. Replace every gray block and cube with owned boulders, rubble, logs and brush.
2. Ground: rock texture on slopes, a distinct dirt trail, ground cover up to the trail edge.
3. Forest: groves of 4 to 8 giants with firs under them, a north grove wall, tree lines on the crests. About 100 giants and 1500 trees (performance unverified).
4. Lighting: sun 24 degrees from bearing 205, ambient #6E6658, crushed blacks 0.28, wash-out 0.18, dark corners 0.4, grey fog from 110 to 850 m. Tested one change at a time, before and after side by side.
5. The office and store as two distinct places; the camps and Ward stones from pack meshes.
Night: the crests need a dark blue sky behind them; the fire reads as a row of candles.

## 6. What needs you

Answer by number. My recommendation is in brackets.

1. **Valley shape:** a horseshoe open to the east. The west ridge stays high with the fire and Ward behind it, and the north and south arms drop to foothills before the fence. The highway runs north to south in plain view, with a junction at the gate. [Yes]
2. **Ward climb:** 150 s or less from camp, four different legs (a boulder chute, an exposed ledge, burned snags, an easy traverse), a landmark per leg. This may mean lowering the ledge from 98 to about 72, still above the tower deck at 56. [Yes]
3. **The tower rule** becomes a loose check, run once per build, not a design driver. [Yes]
4. **The north:** a loop trail from Camp 1 to J with the densest grove, a third forage patch and one ruin. Quill's draft for the ruin is the previous keeper's cabin. [Yes, ruin to be settled with Quill]
5. **Invisible walls:** every stop has something you can see (boulder, deadfall, brush, fence), or it is removed. The only wall with no visible cause is the one that says "You can't abandon your post". [Yes]
6. **Lighting:** Vesper's set above, one change at a time. You see each before and after. [Yes]
7. **New gate before you walk anything:**
   - Rook reruns the eye-height pictures.
   - Marlow runs his checklist from them and walks it himself.
   - Vesper grades it against her fixed bar.
   - Pim runs the task test on every menu.
   - Wren only brings it to you when all four pass.
   [Yes]
8. **Working rules:**
   - Nothing is ticked while it waits on you.
   - Nothing is ruled in your name.
   - At most one unattended task at a time.
   - You see a drawing before any map build.
   [Yes]
9. **Untick 8.9i (day/night) and 8.9k (walk checks).** Both passed their checks and failed you. [Yes]
10. **Withdraw my overnight calls** that you never confirmed: the 32 degree sun (replaced by item 6), the straight-view ruling, and the stop-rock distance. [Yes]

## 7. Proposed order once you answer

1. **Quick fixes, one at a time:** the dev panel (Pim's spec), then lighting, one change per retake. You see each.
2. **Paper:** Sable draws the valley again, starting from what the player does on each leg: the open east, the highway, the short varied climb, the north loop, every minigame and event site, and the junction markers. Quill, Hollis, Vesper, Pim and Marlow review the drawing. Then you approve it.
3. **Build in stages**, each through the new gate:
   1. terrain and road;
   2. paths and ground;
   3. the forest;
   4. visible stops;
   5. places from pack meshes.
4. **Your walk.**

Wren
