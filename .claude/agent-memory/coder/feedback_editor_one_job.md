---
name: editor-one-job-at-a-time
description: Never start a Play-mode script or editor_play/stop while a background capture or area run holds the Editor
metadata:
  type: feedback
---
One Editor job at a time: while main3_review_capture.sh (or any background run) is in Play, do not enter or stop Play, and do not run evals that need Play.

**Why:** 2026-10-03, during the 8.25 round, a side Play script I started stopped Play mid-capture and broke --area camp's night step; the whole area had to be rerun.

**How to apply:** when a capture runs in the background, do code edits only; queue Editor work after its completion notice. Related: [[feedback_build_rewrites]].
