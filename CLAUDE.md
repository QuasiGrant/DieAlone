# DieAlone

PS1/PSX-style horror game. Unity 6000.3.24f1, URP. Solo project; the owner has never written code.

## Working rules
- One task at a time. Touch only the files the task needs. Ask before adding packages, assets, or plugins.
- Anything spanning more than one file or adding a new system: short plan first, wait for my yes.
- Before saying done, run a check that proves it (clean compile, tests pass, or a Game view screenshot) and show the command and its output.
- After each working change, commit with a one-line message. Never leave the project broken at the end of a turn.
- Report each change in one plain-language line: what it does and where to see it in Unity.
- Nothing is decided until it is written in DECISIONS.md (one dated line each) and I have confirmed it.
- Verify a Unity API or package exists for this version before using it. If unverified, say so.
- If something isn't working, say so plainly. Do not build workarounds.
- Be terse. No em dashes. No filler. No unsolicited suggestions.
- When compacting, keep the list of modified files and the current task.

## Unity
- Run `unity status` before touching a scene, prefab, or asset. State "ready" means an Editor is connected: drive it through the unity-editor-mcp tools or `unity command`. Never hand-edit .unity, .prefab, or .asset files while an Editor is reachable.
- If `unity status` shows nothing, check `unity pipeline list` for Safe Mode (compile errors) before assuming the Editor is closed.
- Never commit Library/, Temp/, obj/, Logs/, or Build/.

## Plan
- PLAN.md is the task list. Cowork writes it. You may tick boxes and add lines under Rules and Tips. Do not add, remove, reorder, or reword tasks.
- When I say "next task", do only the first unchecked task in PLAN.md. If it is unclear or blocked by an open decision, stop and say so.
- Read Rules and Tips before starting a task.
- When the task's done-check passes: tick its box, commit the work and PLAN.md together with a one-line message, push, then stop and report.
- If you learned something later tasks need, add one line under Rules and Tips in the same commit.
