# Plan check hook (DRAFT)

Status: draft by Tully, 2026-09-28. Source: DECISIONS.md 2026-09-28, "A git pre-commit script runs the plan checks". Builder: Rook.

## Where it lives
- One script: `Tools/Hooks/commit-msg` (bash). It runs as git's commit-msg hook, not pre-commit, because a pre-commit hook cannot see the commit message and the task-line check needs it. It runs before the commit is written, so it still blocks.
- Install once per clone: `git config core.hooksPath Tools/Hooks`. Rook adds this line to Docs/Status.md under setup, and one line under Rules and Tips.
- Add `.gitattributes` line `Tools/Hooks/* text eol=lf` (core.autocrlf is true here; a CRLF script fails in bash). Mark it executable: `git update-index --chmod=+x Tools/Hooks/commit-msg`.

## Tools allowed
bash, git, grep, sed, wc, sort only. No Python (only the Windows Store stub exists). GNU grep 3.0 in Git Bash. Match the em dash as bytes: `LC_ALL=C grep -F $'\xe2\x80\x94'`. Strip `\r` from every line read.

## Head vs staged
Head file: `git show HEAD:PLAN.md`. Staged file: `git show :PLAN.md`. Staged list: `git diff --cached --name-only --diff-filter=ACMR`. If there is no HEAD (first commit), skip the compare checks. Only run a compare check if that file is staged.

## Checks (any failure blocks the commit)
1. Em dash: in staged PLAN.md, DECISIONS.md, DESIGN.md, CLAUDE.md, `.claude/agents/*`, and any staged `.md` or `.svg` under `Docs/`. Whole staged content. Out of scope: `.claude/skills/` (vendored, has em dashes) and code.
2. Ticks: count of `^- \[x\] ` in PLAN.md, staged must be >= head.
3. Rules and Tips: count of `^- ` lines from `## Rules and Tips` to the next `## ` or end of file, staged must be >= head.
4. Task lines: lines matching `^- \[[ x]\] [0-9]+\.[0-9a-z]+ ` with the box normalised to `[ ]`. Every head task line must appear unchanged in staged. Blocked unless the message has a line starting `Plan-edit:`.
5. DECISIONS.md: every head line must still exist in staged. Blocked unless the message has a line starting `Decisions-edit:`.
6. Forbidden paths staged: `Docs/Private/`, `Library/`, `Temp/`, `Logs/`, `Build/`, `obj/`, and every pack folder listed in the `# Asset Store packs` block of `.gitignore` (read the block, do not hardcode, so a new pack is covered by its ignore line).

## Messages
One line per failure, then a summary. No colour codes.
- `plan-check BLOCKED em-dash: Docs/Status.md line 12`
- `plan-check BLOCKED ticks: 37 at head, 36 staged`
- `plan-check BLOCKED tips: 60 at head, 59 staged`
- `plan-check BLOCKED task-line: "6.2 ..." removed or reworded (add Plan-edit: to the message if intended)`
- `plan-check BLOCKED decisions: line 58 removed or changed (add Decisions-edit: if intended)`
- `plan-check BLOCKED path: Docs/Private/Twist.md`
- Pass: `plan-check ok (ticks 37 -> 38, tips 60 -> 61)`

## Bypass
`git commit --no-verify`. Only Grant decides a bypass, in his own message. No agent bypasses on another agent's word. The commit message must carry `Bypass: <reason>`; Tully reviews it next session.

## Tests Rook runs (on a throwaway branch, deleted after)
1. Clean doc edit commits and prints ok.
2. Em dash in Docs/Status.md blocks; em dash in a `.claude/skills` file passes.
3. Unticking a box blocks.
4. Deleting a Rules and Tips line blocks.
5. Rewording task text blocks; same change with `Plan-edit:` passes.
6. Ticking a box (only change) passes.
7. Removing a DECISIONS.md line blocks; with `Decisions-edit:` passes.
8. `git add -f Docs/Private/x.md` blocks; same for a file under a pack folder.
9. PLAN.md saved with CRLF endings gives the same counts as LF.
10. Commit not touching PLAN.md or DECISIONS.md skips checks 2 to 5.
11. `--no-verify` commits (proves the bypass path).
Paste each command and its output in the build report.
