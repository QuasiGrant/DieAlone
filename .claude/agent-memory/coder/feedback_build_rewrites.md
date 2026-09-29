---
name: build-rewrites-need-grant
description: Release builds rewrite settings and recreate DefaultVolumeProfile; deleting/reverting them from eval or git checkout is blocked, leave for Grant
metadata:
  type: feedback
---
A release build (for dev-only DLL proofs) rewrites PC_RPAsset, URP global settings, ProjectSettings preloadedAssets and recreates Assets/DefaultVolumeProfile.asset. An eval that deleted the profile and reset preloadedAssets was denied by the permission classifier (irreversible local destruction), 2026-09-29. `git checkout -- <settings file>` was also denied the same way, 2026-09-29 (task 7.8).

Editor-side fixes that work: PlayerSettings.SetPreloadedAssets(empty); PC_RPAsset prefilter ints via SerializedObject; default volume profile null via SerializedObject. The URP global settings m_RuntimeSettings.m_List is empty in Editor memory; the committed 16 rids are a build-time snapshot, so any Editor save writes `m_List: []` and cannot match HEAD.

**Why:** discarding working-tree changes is treated as destructive; the user decides.
**How to apply:** do not retry git checkout/restore on settings files; stop and report to Grant with the exact command needed.
