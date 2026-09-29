---
name: build-rewrites-need-grant
description: Release builds rewrite settings and recreate DefaultVolumeProfile; deleting/reverting them from eval is blocked, leave for Grant
metadata:
  type: feedback
---
A release build (for dev-only DLL proofs) rewrites PC_RPAsset, URP global settings, ProjectSettings preloadedAssets and recreates Assets/DefaultVolumeProfile.asset. An eval that deleted the profile and reset preloadedAssets was denied by the permission classifier (irreversible local destruction), 2026-09-29.

**Why:** deleting assets is treated as destructive; the user decides.
**How to apply:** after a proof build, leave those files uncommitted and unstaged, commit only the task files, and report the dirty settings for Grant to decide (commit on their own or revert).
