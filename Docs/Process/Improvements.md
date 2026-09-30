# Where process lessons live

Tully, 2026-09-30. From the valley review (Docs/Review/2026-09-30-ValleyReview/README.md section 2) and DECISIONS 2026-09-30.

| Kind of lesson | Home | Why |
|---|---|---|
| A habit one agent keeps every task | One line under "Your job" in .claude/agents/<agent>.md | Loaded on every run of that agent, costs nobody else context. Wren applies; Tully checks the diff. |
| A check several agents run in order | Docs/Process/Gate.md | Loaded only when a done-check names it; one source, no copies. |
| A rule every agent and the main session obey | CLAUDE.md | Always loaded, so only rules with no other home; none needed now. |
| A technical fact the coder needs for later tasks | PLAN.md Rules and Tips | Rook reads it before each task. Already 125 lines: no process lessons here. |
| One agent's method trap (false results, tool quirks) | That agent's memory | Private to the agent that repeats the mistake. |
| A mistake not yet fixed at its home | Docs/Process/Lessons.md | One line; deleted when the fix lands in one of the homes above. |

## Lines for Wren to apply in .claude/agents (under "Your job")
- game-designer.md: `- Every map doc opens with what the player does, sees and feels on each leg. No revision that only moves margins.`
- creative-director.md: `- Judge against the fixed bar in Style.md from eye-height frames every 25 m, never against the last capture. One lighting change per retake. Never pass anything as "fine for gray".`
- playtester.md: `- Run Docs/Process/Gate.md step 2: the 10 items from the sheets, then your own hand walk, before Grant walks anything.`
- ui-ux-designer.md: `- Test the task, not the fit: each task in 5 presses or fewer, pad only and keyboard only, at Grant's Game view size (Gate.md step 4).`
- sound-designer.md: `- Review every map drawing on paper for the road, echo walls and sound landmarks before it is built.`
- writer.md: `- Check that each home says something about who lives there.`
- coder.md: `- Before any handback Grant will walk, run Gate.md step 1 and hand the sheets path to Wren.`
- process-manager.md: `- Challenge any task whose target is a date rather than a quality. Hold Gate.md: refuse a Grant walk until steps 2 to 4 pass.`
- chief-of-staff.md, add: `- Make judgement calls and move work forward; log each call as yours, one line in Docs/Status.md, for Grant to review after. Never write a call as Grant's.`
- chief-of-staff.md, add: `- Bring a build to Grant only after Gate.md passes. Fixes become new tasks; finished tasks stay ticked.`
- chief-of-staff.md, replace `Never expand a task's scope silently. If the work outgrows the task, stop and tell Grant.` with `Never expand a task's scope silently. If the work outgrows the task, split it into new tasks and say so in Status.md.`

CLAUDE.md: no change. Its rule "if the done-check needs another agent's check, do not tick" already covers Gate.md.

## Open, for Wren
- Vesper's eye-height bar is not yet in Style.md; Gate step 3 cannot pass until she writes it.
- Each gate run makes about 22 MB of sheets. Committed, five build stages plus retakes add 200 MB or more to git. Proposal: write gate sheets to a git-ignored folder and commit only the verdicts; the recipe regenerates them.
