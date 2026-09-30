# Status

Where DieAlone stands between sessions. Wren (chief of staff) updates this at the end of every session. Every session starts by reading it.

Last updated 2026-09-29.

## Next step

As of 2026-09-29, late night:
1. Done: Milestones 5 to 7; Main3 8.1 to 8.9e; 8.9h responsive menus; 8.9i dev panel day and night (F1, LOOK, Night / Day one / Day two).
2. The valley is built (8.9j, commit 4e1415d, unticked until Marlow's re-walk): Docs/Design/Valley.md rev 6. Ridges wrap the map, the Ward sits on a knob at 115 above the tower's 56, and the fire burns behind the west ridge, always on, hidden by the land. F-1 (fire hidden), W-1, C-1 and E-1 (no map edge shows) all pass. The rev 16 scene is kept at tag main3-rev16.
3. Running: Rook on 8.9k (walk checks retargeted, visible stops at ridge feet, edge screenshots) and the cabin roof light leak (8.9g). Then Marlow re-walks, and Vesper checks the edges and the cabin.
4. Then 8.10: Grant walks the valley.
5. Open for Grant: Docs/Private/OpenForGrant.md, Docs/Private/ValleyTextFixes.md (story line changes for the valley).
6. Plan shape: 10 one playable week, 11 dressing and look, 12 characters, 13 content format and dialogue, 14 events, 15 resident minigames, 16 homage references, 17 release; sound (9) after 11.
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
