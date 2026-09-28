# Status

Where DieAlone stands between sessions. Wren (chief of staff) updates this at the end of every session. Every session starts by reading it.

Last updated 2026-09-28.

## Next step

As of the end of 2026-09-28:
1. Done today: Milestone 5 closed (Main2 archived, tag main-scene-2.0). Milestone 6 tasks 6.1 to 6.5 done (plan check hook, carry tuning, test setup, Cowork retired in CLAUDE.md, scene facts in Docs/Scenes/Main2.md). DailyLoop.md and Main3.md at revision 5 (DRAFT, committed).
2. Done: 7.1 game state (Assets/Scripts/Core, LoopTuning.asset, 23 tests), 7.2 run simulator (DieAlone > Simulate Runs), 7.3 versioned save (31 tests pass). Simulator on draft numbers, median and max day a run ends: gives nothing 12 and 12 (WARD ends it); idle 4 and 4 (HP); gives 1 12 and 16 (HP mostly); gives 2 9 and 11 (HP); balanced 16 and 20 (WARD). Every run ends. A save from another version is refused; no migrations yet.
3. Grant, next: comment on the revision 5 drawings (Docs/Design/Main3_map.svg, DailyLoop_flow.svg) and answer: day timed by the keeper's watch; Ward feeding (1 holds, 2 raises); all five needs sometimes met on a quiet day; an open anomaly costs 1 MIND a night and after three days 1 WARD; the day-one fire as an ordinary far-off forest fire; the unsigned cave trail as the one exception to the junction check.
4. Grant, any time: buy packs from Docs/Design/PackShortlist.md after checking screenshots; confirm bought-pack shaders get swapped for ours.
5. 2026-09-29: story session, Grant and Quill, prep in Docs/Private/SessionPrep.md.
6. After Grant approves DailyLoop.md: task 6.6 (DESIGN.md in line), then Milestone 7 design lines into DECISIONS.md, then Milestone 8 blockout tasks.

Drafts to review: Docs/Design (Style, PackShortlist, DialogueFormats, Sound, UI), Docs/Process/PreCommitHook.md.

## How we work now (2026-09-28)

- Nine named agents in .claude/agents. Wren runs the team, settles disagreements, reports to Grant. Grant confirms every decision; nothing is decided until it is a dated line in DECISIONS.md and he has said yes.
- Tully checks every task before it is written and every commit before it lands.
- Marlow checks every task after it is marked done and before Grant is asked to look.
- Quill works under Grant on story. Drafts live in Docs/Private (git-ignored). Story text the game needs may be committed (2026-09-28).
- Grant batches yeses against numbered lists. Routine commits report hash and clean status only.
- Rules for editing PLAN.md and DECISIONS.md: start from head, edit by script, check no em dashes and unchanged tick and Rules and Tips counts, read back after saving, diff against head before committing.

## Where the project is

- Milestones 1 to 4 complete: player, interaction, doors, carry, VHS look, prefabs, scene recipe, dev menu, pause menu with saved settings, controller.
- Milestone 5 built, 5.10 unwalked. Main.unity is 1.0, frozen at tag main-scene-1.0. Main2.unity is 2.0, the working scene, nine locations: Ward, camp with tower, tent site, two cabin campsites, lake with dock, entrance with gate and office, cave, trail beats.
- Game systems: none. Of the DESIGN.md systems, settings menu and controller work; check the fire, Ward touch and the map board are stubs; everything else does not exist. Full audit in Docs/Audit/2026-09-27-project-audit.md.
- Milestone 6 is flagged for rework. The old daily loop is superseded. Loop decisions so far (DECISIONS.md 2026-09-27): tower and logbook mandatory every week; Food, Water, Warmth yes or no per week, 1 HP each if unmet; Safety and Social yes or no per week, 1 MIND each if unmet; no stockpile; no fixed run length, a run always ends by bottoming out; events touch needs; pet later.
- Housekeeping done 2026-09-27: recipes in Tools/Recipes, SSAO off, companyName QuasiReal Publishing, template packages and profiles removed, DESIGN.md eras and VHS presentation, CLAUDE.md line one.
- Four Asset Store packs in the project, git-ignored, listed in Assets/SOURCES.md.
- Old Milestone 6 owner notes from 2026-09-23 (stats in the logbook not on screen; sleep any time; Ward rates 1 HP = +2 WARD, 1 MIND = +2 WARD, 1 food = +1 WARD; MIND only goes down) are superseded pending the Milestone 7 design. The +2 rates create points and were flagged.

## Private

Docs/Private holds Main3 story placeholders and Quill's session prep. There is no story bible yet. Grant and Quill hold a story session during Milestone 7.

## Setup

- Once per clone: `git config core.hooksPath Tools/Hooks` turns on the plan check hook (Docs/Process/PreCommitHook.md).
