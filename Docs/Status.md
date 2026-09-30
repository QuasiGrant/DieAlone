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
