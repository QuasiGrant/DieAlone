# Status

Where DieAlone stands between sessions. Wren (chief of staff) updates this at the end of every session. Every session starts by reading it.

Last updated 2026-09-29.

## Next step

As of 2026-09-29, late:
1. Done: Milestones 5 and 6; Milestone 7 build work 7.1 to 7.8; Main3 gray blockout 8.1 to 8.9b, walked by Marlow and by Grant. Main3.md is at revision 13 (Grant's gray-box feedback). Recipes and run order in Tools/Recipes/README.md; runner Tools/Recipes/main3_rebuild.sh.
2. Running: Rook on 7.9 (dev panel on screen, Grant to try) then 8.9c (higher knoll, spiral stair, bigger cabin, Ward lip and stand-in fire). Marlow's re-walk checklist is ready for after 8.9c. Sable rewriting the resident minigames (Docs/Private/Minigames.md). Vesper on Pony Island, Inscryption, Buckshot Roulette notes. Pim on Collectibles.md and ContentWarning.md. Hollis on the store loop sound. Quill on the Child pilot dialogue.
3. Next: 8.10 closes when Grant walks the rebuilt Main3; then Milestone 9 (sound groundwork).
4. Plan shape: Milestones 9 sound, 10 one playable week, 11 dressing and look, 12 characters, 13 content format and dialogue (Child minigame as pilot), 14 events, 15 resident minigames.
5. Open for Grant: where the escape-room puzzle goes and where the roulette fits (after Vesper's notes).
6. Story canon only in Docs/Private (StoryBible, Minigames, MinigameReview, ToneReference, SessionPrep, ChildDialogue, StoreSound, MullinsNotes).

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
