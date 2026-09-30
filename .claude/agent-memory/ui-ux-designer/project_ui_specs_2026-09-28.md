---
name: ui-specs-daily-loop-drafts
description: State of the Docs/Design/UI specs (loop four plus Logbook, MainMenu, Settings) and the term rules they follow
metadata:
  type: project
---

2026-09-29: seven drafts in Docs/Design/UI: TowerCheck, WardNight, Objective, DayEndConfirm, Logbook, MainMenu, Settings. Uncommitted drafts.
Terms (Wren 2026-09-29): approved DailyLoop.md wins. Stamps SAFE / CHECK ON FOOT; locations Lake, Camp 1-3, Office. Quill: nothing names the Ward before the night 1 reveal; night 1 is led by the keeper's note "the cairn path, at dark" and the lit cairn.
2026-09-29 later: Collectibles.md (last logbook tab AFTER Settings, cross-run profile.json) and ContentWarning.md (adds main menu row, Reduce flashing toggle in Settings Display). Logbook.md 3, MainMenu.md and Settings.md need revising to match.
2026-09-29 later: minigame UI specs go in Docs/Private (git-ignored, twist): StoreUI, RouletteUI, FishingUI. Shared patterns: GateBooth counter view reused; carry-aware prompts on targets (till, bowl, chair); no HUD numbers; results only in the Today right-page block. Proposed Logbook `Notes` block for LOG lines, pending Grant.
WardNight still has [GAP: DailyLoop] items answered by DailyLoop.md 2 and 6 (bunk, night 1 explainer, last-point warning); 0 to 3 points and WARD cap folded in 2026-09-29.
2026-09-29 type pass: all ten specs now TextMeshPro. Rule: keeper's writing = Patrick Hand (logbook pages, Kept page, pencil fill-in); anything printed or game-spoken = Overpass (menus, settings, cards, prompts, objective line, forms, tab labels, Ward screen, binocular readout); VT323 only for PLAY > tape mark. Stamps stay Images. MainMenu + ContentWarning are the Milestone 10 build.
2026-09-29 event-text pass (Wren's order, drafts, no commit): new Dialogue.md (talk, max 3 replies, ACT, form-plate lines, minigame calls, captions with source word; lower band default, host picks upper) and Examine.md (examine line, lifted read view, 360-row sizes, others' handwriting = texture only; GateBooth 6 Overpass option to drop). Logbook 4.5-4.7: tower/log/missed/night lines, off-row mapping to Office/general line, budgets TOWER 45 LOG 120, Log block scrolls (right stick), pre-day-1 page, unread dots. WardNight 6a: event WARD gift / smaller hunger lines.
2026-09-29 8.9h review: CanvasFit.cs (ref height, floor, fit target). Fit lines added: MainMenu 2.6 (the rule: ref 1080, floor 0.667), ContentWarning 5.6, Settings 3.7 (scrolls, no shrink), Logbook 2.4. Dev panel uses ref 720 floor 1.
2026-09-30 Gate 8.14 (Gate_8_14_Pim.md): FAIL. Grant's Game view 3840 x 1976. Capture sheets are colour, day only: W1 grey numbers need Rook's recipe to print means. Valley.md 11 junction markers belong to no build. Stop scan misses IW1/IW3 (no trail there); ask for scene-wide renderer-less collider list.

**Why:** loop doc landed after the first drafts; terms changed under them.
**How to apply:** grep UI specs for "[GAP:" and old words (NOT SAFE, Campsite) before any revision. Input facts: Esc is bound to Player/Pause and UI/Cancel; Interact E / pad Y; Crouch pad B; Tab, View, LB, RB unbound (proposed Logbook action).
