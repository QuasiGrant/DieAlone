# Status

Where DieAlone stands between sessions. Wren (chief of staff) updates this at the end of every session. Every session starts by reading it.

Last updated 2026-09-30.

## Next step

As of 2026-09-30, early morning:
1. Done tonight: 8.9h responsive menus, 8.9i dev panel day and night, 8.9j valley build, 8.9k valley walk checks. The team checked each one.
2. Waiting on Grant: 8.9f (the 13 slice captures, now pre-valley), 8.9g (look at Docs/Look/DayOneFix, Vesper passed), then 8.10 (walk the valley and say "right for now").
3. Valley: Docs/Design/Valley.md rev 7 and Valley_map.svg. Ridges wrap the map; the Ward sits on a knob at 115, above the tower's 56; the fire burns behind the west ridge, always on, seen only from the ledge. The rev 16 scene is kept at tag main3-rev16.
4. Not built yet: the day-one smoke sheet (spec in Valley.md 3.2), the flame and ledge dressing (Edges.md 6, Milestone 11), and a lock that keeps the Ward climb night-only.
5. Queued after Milestone 8: WalkChecks 4 beyond the climb, the lake sweep (5), a trail walker with gravity and jump; Milestone 10 tasks from Docs/Process/Milestone10Draft.md.
6. Open for Grant: Docs/Private/OpenForGrant.md.
7. Plan shape: 10 one playable week, 11 dressing and look, 12 characters, 13 content format and dialogue, 14 events, 15 resident minigames, 16 homage references, 17 release; sound (9) after 11.

## How we work now

- Nine named agents in .claude/agents. Wren runs the team, settles disagreements, reports to Grant. Nothing is decided until it is a dated line in DECISIONS.md and Grant has said yes.
- Tully checks tasks and commits; Marlow checks work before Grant looks. Walk checks become a recipe (Docs/Process/WalkChecks.md).
- Quill works under Grant on story. Drafts live in Docs/Private (git-ignored, own local git history).
- Grant answers numbered lists by number. Wren keeps every agent busy and lists who is working in every report.
- The commit hook (Tools/Hooks/commit-msg) guards em dashes, tick and Rules and Tips counts, task lines and DECISIONS lines.

## Where the project is

- Main3.unity is the working scene, rebuilt by Tools/Recipes/main3_rebuild.sh. Main2 archived at tag main-scene-2.0.
- Daily loop designed (Docs/Design/DailyLoop.md): one wake-up is one day; tower check, chores, file the report; at night only the Ward. Game state, tuning, simulator and save v2 in Assets/Scripts/Core.
- Look: VHS filter with LookTuning (current, day one, day two, night). F1 dev panel: scenes, warps, looks.
- Second builder waits for Milestone 13.

## Private

Docs/Private holds the story bible, minigames, events, scares and dialogue drafts. Break backup: committed locally and zipped to C:\Users\grant\Documents\DieAlone Private Backups. The Drive upload does not work for a file this size yet (see OpenForGrant).

## Setup

- Once per clone: `git config core.hooksPath Tools/Hooks` turns on the plan check hook (Docs/Process/PreCommitHook.md).

## Wren's calls (for Grant to review after)

- 2026-09-30: The north gets Sable's loop trail, densest grove, a third forage patch and one ruin (Quill's draft: the previous keeper's cabin).
- 2026-09-30: Gate capture sheets go to git-ignored Docs/Captures/; only verdicts are committed (Tully: about 22 MB a run).
- 2026-09-30: Process lessons live where Tully's Docs/Process/Improvements.md says; agent habits applied to .claude/agents.
- 2026-09-30: Grant's "other than the other camp" for invisible walls is unclear; Sable states a reading in Valley.md rev 8 and the team goes with it.
- 2026-09-30: Vesper's eye-height bar (Style.md 10) and groves rule (5.8) adopted as the gate standard.
- 2026-09-30: Valley rev 8 (Sable) goes to review as drawn: ridges 64 to 70 with a crest tree belt as cover, the ledge at 62 (still above the deck at 56), the highway at x 428, and a climb of 117 s from J.
- 2026-09-30: Grant's "other camp" read as the closed campground: one shift-only wall at its spur mouth stays (DECISIONS 2026-09-29).
- 2026-09-30: Off-trail walking in open forest: yes. Each dawn, two of the three forage patches bear: yes, to be playtested.
- 2026-09-30: Camp knoll trees are 35 m (tops 50), not 42, so they don't block the deck's view.
- 2026-09-30: The fire is hidden by land, not trees (Rook measured a planted belt at about 27 percent gaps). The W crest rises to about 78 to 80 where the tower's lines cross it, and the knob to 84; still 15 to 25 m under rev 7. Trees stay on top for the look.
- 2026-09-30: Map boundaries come from visible land (cliffs over the 45 degree slope limit, rock bands, fence, water), not invisible walls; only the front and the Ward path by day keep one.
- 2026-09-30: Gate step 4 adds Pim's path rule: trail at least 20 grey from the floor at 5 m and 20 m, day and night; every stop across the full trail width.
- 2026-09-30: Asset gaps use owned fallbacks first (BK rocks and rubble for scree, young firs plus large bushes for tall brush, owned sign boards and cairns for markers). Buying the dock pack waits for Grant.
- 2026-09-30: Day one shows only a low smoke sheet streaming west; tall smoke columns appear from nightfall on night one.
- 2026-09-30: Rook starts 8.14 on Valley rev 10 while Marlow rechecks the four fixed items on paper; the built-mesh checks (F-1, walk, push) are the final word.
- 2026-09-30: Frame-rate floor for the forest: 1% low at 60 fps or better, thinned in Vesper's order if it drops.
- 2026-09-30: The far fire's north end stays at z 650 so it never visibly stops in the ledge reveal; the raised N arm hides it (Marlow).
- 2026-09-30: A Tri becomes the day-one look now (Vesper's pick); Grant sees it on his walk and can overturn.
- 2026-09-30: The player may no longer jump-climb slopes steeper than the slope limit; with that, the climb's ring walls drop to low rims except where needed.
- 2026-09-30: Leg 2's 61 m straight is exempt from the 60 m straight-view cap.
- 2026-09-30: Junction markers (Valley.md section 11) and the north loop trail go into 8.14a; the ruin stays in 8.17.
- 2026-09-30: The "North loop: ruin" warp comes with the ruin in 8.17.
- 2026-09-30: The rest of 8.14a is fixed alongside 8.15, and one gate covers both; rock textures are the main fix for the bands and climb. Ledge: Marlow's curb-and-catch-shelf plus Vesper's taller valley flames and lit floor.
- 2026-09-30: Ledge built as Rook's thin 1.1 m lip falling outward (120 of 120 valley fires seen, 0 pushes off), not the curb and shelf a sprint-jump cleared.
- 2026-09-30: CHASE-LIGHT: the lamp relights when the footsteps stop (Quill), not at the Ward zone (Hollis); Hollis aligns ScareSound on his next pass.
- 2026-09-30: Lanterns with small glows at the chute foot and on P1 to P4 stay (Rook's addition for the night rule).
- 2026-09-30: The cleft is a slot, so all-wall frames inside it are allowed; the chute must still open up.
- 2026-09-30: Rook moves on to the forest (8.16) now; the remaining day-path contrast, hedge cover and next-place frames are remeasured after it, since groves change the floor light. One gate covers 8.14a, 8.15, 8.15a and 8.16.
- 2026-09-30: Vesper's forest look calls adopted: trail material fixed for distance (no ambient change), sky blend rate 4.5, lodBias 2 to 1.25 on PC (never below 1.0).
- 2026-09-30: No dock pack for now; Rook builds the dock from owned logs and planks in 8.17. Buy only if Vesper grades it below C or a doc needs a visible boat (Tully).
- 2026-09-30: No shell for Vesper, Pim or Hollis; Wren makes one path-limited commit per review round instead (Tully).
- 2026-09-30: W1 at 20 m may pass by eye if the leg passes at 5 m, the trail shows to the next bend in every frame, and brush, walls or markers hold the line (Pim). The night rule applies only where the line can be lost.
- 2026-09-30: The 60 fps 1% low floor is a working target until the frame-rate cause is known; Grant can overturn.
- 2026-09-30: Frame rate passes: the Camp and S1 slow frames are first-view hitches (1% lows 66 and 81 on a second pass). A shader prewarm behind the loading screen goes with Milestone 10's loading and menu work.
- 2026-09-30: Climb rock count excludes walkable ground: count rock steeper than 35 degrees, and count as open any sky or hit over 60 m. Bar outside the cleft: rock 30 percent or less, open 15 percent or more (Vesper).
- 2026-09-30: Front-zone roads count as paths for the 50 m rule.
- 2026-09-30: The boathouse step (cat feeding) and the payphone booth must be reachable; the north ruin stays hidden from the tower.
- 2026-09-30: Giants keep the 50 m cap; one emergent giant per grove near the cap, the rest 12 to 18 m lower, no fir within 10 m of it (Vesper).
- 2026-09-30: W3 exempts the cleft frames (climb 210 to 230); the ledge is hidden there on purpose.
- 2026-10-01: Changes that need the reset are batched with 8.18 into one rebuild Grant runs once.
- 2026-10-01: Climb bars: the cwm bowl frames (FWD 50, FWD 120 to 160, BACK 176 and 186) pass at rock 40 percent or less with open not counted, Vesper grading the top half by eye; elsewhere rock 30 or less with open 15 or more, or rock 40 or less with open 25 or more.
- 2026-10-01: Lamp at intensity 16, range 10 (Vesper); trail stretches that still fail N2 get a trail material fix.
- 2026-10-01: Vesper's forest density (ForestPlan section 8) replaces the old open-ground rule in Style.md 10 (Wren's call, after Grant: the forest is far too sparse).
- 2026-10-01: Night one keeps the low lit smoke sheet (Edges 6); columns from night two. The Ward ledge gets no day grade; capture keeps only the day leak checks from the tower and J.
- 2026-10-01: WardPath draft 2 goes to build with Marlow's draft-2 conditions folded in (rail on all three open sides of the prow, two collider runs on the fallen giant, the flight 3 support height and the Band_S_W exit closed), checked on the built mesh rather than another paper round.
- 2026-10-01: Walk-into colliders come from one late pass (main3_8_18a_solid.cs) plus the walk-into check, not each owning recipe.
- 2026-10-01: The Cave_Chamber dev warp stays inside the cave and is exempt from the sealed-landing test; it is a dev-only warp and F1 is the way out.
- 2026-10-02: Warmth comes from the cabin stove: split wood at the woodpile, then light the stove. The outdoor fire pit loses its prompt and stays as camp dressing.
- 2026-10-02: Cabin interior swaps bunk and stove (bunk north-east sees door, desk and stove); the fire pit moves off the door-to-tower line and stays cold.
- 2026-10-02: The Ward path may be seen from the deck as a trail; the rune post and Ward stones may not. The ruin and rune post get a render pixel check, since tree crowns have no colliders.
- 2026-10-02: CampLayout draft 2 goes to build without a third paper round (two-round cap); the 8.21 area check verifies overlaps on the built scene.
- 2026-10-02: Captures turn the GPU Resident Drawer off in memory while rendering and restore it after, because Camera.Render skips its objects (cabin, tower, forest); every capture since 8.16 missed them.
- 2026-10-02: The woodpile need not be seen from the deck. The Ward stones are proven hidden by land rays (W-1); the pixel check applies only where trees are the cover.
- 2026-10-02: "Found" means seen by meshes, within 45 degrees of the direction of travel, at least 1 degree tall, with a labelled frame (Pim); it replaces the 30 m line rule.
- 2026-10-02: The deck need not see the camp under its own tower; camp items come off the deck must-see list. Must-hide still applies.
- 2026-10-02: 8.21 ruling after round 2 (cap): layout passes; three small items (fire pit wedge, chair to stove gap, target marks on found frames) are fixed by Rook and proven by the area check, then Wren ticks without a third review round.
- 2026-10-02: A gate shift starts when the player enters the booth (Main3 3.2.6), not when a car stops. The office door is the built west door (Valley.md).
- 2026-10-02: The gate booth moves to the driver's side (south of the drive, x 390.6 to 393.2); a lift barrier does the check; the turning circle goes and refused cars reverse out. The front zone owns the closed campground.
- 2026-10-02: IW3 is on whenever an admitted car is on the campground spur, not tied to the gate shift (DECISIONS 2026-09-29).
- 2026-10-02: IW3 always shows "You can't abandon your post." when it stops the player, shift or not, so no invisible wall is silent. The booth is an open doorway.
- 2026-10-02: The tower check is done from anywhere on the deck, so the office door counts as seen from the deck's east side; it need not be seen from the lectern.
- 2026-10-02: The fishing eat point is the boathouse step chair (Valley and the build), not a pump dock chair. Stilts rest and the step door are Sable's to settle in LakeLayout.
- 2026-10-02: The tower lectern moves to the cab's east side so the binoculars see the office door (Sable). The office porch lamp is fully off when the door is shut and near white when open.
- 2026-10-02: Lake: no window lamp in the boathouse (nobody lives there); a shallow wading strip past the shore for the three wading events, inside the wade limit (Sable designs it).
- 2026-10-02: Lake: fishing from the empty boat slip inside the boathouse through a new boat door; the shallows behind a stake-and-rope line north of the house for the wading events; the cat (R4) has no Talk point.
- 2026-10-02: The bowl and chair answer only from the step floor, so the shallows event point can stay where it is.
- 2026-10-02: The tower lectern stands on the walkway's north-east corner just outside the cab, where the binoculars see the office door.
- 2026-10-02: The lake's boat slip view has no sky (the ridge stands above it); accepted for layout.
- 2026-10-02: 8.22 ruling after round 2: Rook fixes the chain visibility, Found_06 and Found_07, and the store counter wedge, proven by the area check; then Wren ticks without a third review round.
- 2026-10-02: Breaks recheck trap 3a is exempt; its start now sits inside the raised hedge, and the lake flood finds 0 traps in that trench.
- 2026-10-02: North: search spot SS3 is found from the fallen giant's end; the ruin gets a report box like the keeper's (Quill's draft; Grant can overturn); the loop passes within about 21 m of SS1.
- 2026-10-02: The bowl and chair use a must-stand-on-the-step-floor rule when they become usable (Milestone 10); the lake layout passes without it.
- 2026-10-02: 8.23 ruling after round 2: from the deck the step need only show a warm spot there or gone; the full read (the cat itself on the step) is checked when the cat is built (Milestone 11/12).
- 2026-10-02: Camp 2: the empty setting is a third place at the table nobody sits in; the ring box stays in the tent; the phone cord reaches the table (Sable sets the distance).
- 2026-10-02: Camp 2: the payphone stays in its booth with a 1.37 m armored cord to the table; the barrel offers water on about half the days (never beats the pump); the Lover sits on the stack top by day and at the table on hand days.
- 2026-10-02: Camp 3: the easel canvas is the escape-room entry; the Snag line starts hung with his old sketches; the job form sits in camp, outside the painting. Whether the hollow floor is seen from the deck is Sable's to settle with the tower lines.
- 2026-10-02: The Artist's job form is found inside the camp, not from the trail; it is exempt from the trail-find rule. The entry prompt reads "Study the painting" for now.
- 2026-10-02: Camp 3 after paper round 2: Sable over Marlow on two points. The drop at (89.0, 163.9) runs downhill, so it is a cascade, not a fault. Creek to trail is now water edge 1.0 m or more from the tread edge (her own 3 m number withdrawn). Both are proven at build by the flood and walk-into checks; no third paper round.
- 2026-10-02: Burn: Quill's story calls (road start, the year, forage eaten at the patch) stay Draft for Grant's next session with Quill; Sable lays out the burn on the draft, which none of the three changes. LOOP-LEG needs a leg with no tower view: Sable checks the 25 m under the Hollow Giant first, else the scare moves leg.
- 2026-10-02: Burn warps (Pim): "Burn fork (to Camp 1 and the lot)" faces 40 so all three trail mouths show; "Old burn" becomes "Old burn, forage A", faces the patch, and sits under Burn fork in the list. Forage shrubs get solid colliders (the interact ray skips triggers).
- 2026-10-02: LOOP-LEG: Marlow's survey shows the deck sees the whole Hollow Giant stretch, so the scare moves to the north loop between forage C and the ruin (Hollis); Marlow checks that stretch for tower view in the 8.24 gate. Ash footsteps (Hollis: a footstep zone from day 2, not a repaint) go to Milestone 11 with Vesper; not layout.
- 2026-10-02: Burn after paper round 2: the Hollow Giant has no inside room, only a shallow burned hollow at its base (trunk too thin; a trail-height opening needed a 5.3 m landing). Hollis's small-room sound becomes the hollow's. Sable's three open checks (walk-in from Camp to pump, Hedge_Burn_7 near the Jg post, ground at the hollow and firebreak) are proven by the area check at build; no third paper round.
- 2026-10-02: Cave power: a battery bank by the speakers, cabled to a generator niche in sampled rock in the passage east wall (z 31.6 to 33.4; moved by Sable from z 34 to 36, which was open air, Wren accepted); the generator is always cold and silent whenever the player can be there (Hollis over Quill: a running one would drown the drip and the chant and break the no-hum rule). "Nothing heard at the mouth before day 2" covers the generator and music only; the chant is at the mouth from night 1. Quill's two story questions (his seat or the table the day after he loses; whether the keeper's-door bulbs are the branch bulbs) wait for Grant's next session with Quill; both spots are kept in the layout either way.
