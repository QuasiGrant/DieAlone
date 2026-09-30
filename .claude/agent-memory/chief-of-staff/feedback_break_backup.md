---
name: feedback-break-backup
description: When Grant says he is taking a break, back up Docs/Private to his Google Drive before anything else
metadata:
  type: feedback
---

When Grant says he is taking a break (or similar: stepping away, done for now, heading out), do this first:
1. Snapshot the private history: `git add -A` and commit in Docs/Private.
2. Zip Docs/Private/*.md with a dated name.
3. Upload the zip to his Google Drive with the Drive connector's create_file (base64Content, contentMimeType application/zip, disableConversionToGoogleType true) into a folder named "DieAlone Private Backups". Create the folder the first time and find it with search_files after that.
4. Confirm in one line: the file name and the Drive folder.

**Why:** On 2026-09-29 Grant chose this over Google Drive for desktop. Docs/Private (story bible, minigames, all dialogue and event text) is git-ignored and exists nowhere else.

**Known limit (2026-09-29, first attempt):** the Drive connector works (folder created, id 1MnE2VsV17m2X4X7Ow9nazq2yqr2xgHaC), but the zip is 181 KB (242 KB as base64). create_file needs the whole content inline in one call, which is impractical and risks corruption. The small-business Drive plugin failed auth; the working one is the 44f2326d connector. Chrome was not connected. Fallback used: a copy in C:\Users\grant\Documents\DieAlone Private Backups (same disk, not off-machine). Proposed fix to Grant: Google Drive for desktop syncing that Documents folder.

**How to apply:** Do it every time, without asking. If the Drive connector is unavailable, say so and give him the zip path. Also run the usual end-of-session status update.

Related: [[user-grant]].
