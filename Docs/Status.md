# Status

Where DieAlone stands between sessions. Wren (chief of staff) updates this at the end of every session. Every session starts by reading it.

Last updated 2026-09-29.

## Next step

As of 2026-09-29, night:
1. Done: Milestones 5 to 7; Main3 blockout and dressed slice 8.1 to 8.9e. 8.9f captures passed Vesper, wait for Grant.
2. Grant's walk found: too dark, dev panel off screen, the fire visible from the cabin. Running: Rook on 8.9g (day-one brightness and start look), 8.9h (responsive menus), 8.9i (dev panel day and night switch).
3. Valley draft (Sable, paper only): ridges wrap the map, the Ward on a knob at 115 m above the tower's 56 m, the fire hidden by the land from every place but the Ward ledge. Marlow is checking its numbers; Vesper is writing Docs/Design/Edges.md (beautiful map edges, Grant 2026-09-29). Grant approves a drawing before any rebuild.
4. Then 8.10: Grant's walk and "right for now".
5. Plan shape: 10 one playable week, 11 dressing and look, 12 characters, 13 content format and dialogue, 14 events, 15 resident minigames, 16 homage references, 17 release; sound (9) after 11.
6. Open for Grant: Docs/Private/OpenForGrant.md.

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
