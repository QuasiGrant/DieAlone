---
name: inline-comment-edits
description: When adding a // comment to a C# line by sed or Edit, put it at the very end of the line, never before code on the same line
metadata:
  type: feedback
---
Never insert a `// ...` comment in the middle of a one-line C# statement chain. Everything after `//` becomes comment, and the code still
compiles when the swallowed part was optional.

**Why:** 2026-10-03, this happened four times in one session (a Q() helper, a StairRamp's localScale, a hit filter, a frames string). Once
the compile failed. Once it compiled, and a ramp stayed 1 m long until a check caught it.

**How to apply:** append comments with `s|$|   // ...|` on the target line, or put them on their own line above. After any sed edit to
code, grep the line to confirm nothing follows the comment. Related: [[editor-one-job-at-a-time]].
