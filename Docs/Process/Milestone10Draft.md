# Milestone 10 task draft (Tully, 2026-09-29)

Draft task lines for Milestone 10, to be written into PLAN.md by Wren when Milestone 8 closes. [G] means Grant plays it and Wren ticks it. Placeholder text is fine until Quill's story content lands.

- 10.1 TextMeshPro and fonts. Import TMP Essential Resources; add Patrick Hand, Overpass and VT323 .ttf files with their OFL.txt, listed in Assets/SOURCES.md; bake static SDF assets with dynamic fallbacks. Done when: a screenshot through the filter shows the Fonts.md test line in all three fonts. Stop before touching existing UI.
- 10.2 Move the prompt, pause menu and dev menu text to TMP. Done when: clean compile, screenshots of each.
- 10.3 RunDirector in Main3: holds GameState, wakes the player in the cabin bunk, saves with GameSave at sleep only; numbers in LoopTuning.asset. Done when: a Play eval advances two days and run.json shows day 3. No UI.
- 10.4 Objective line (Objective.md). Done when: the wake line shows in Play.
- 10.5 Tower check (TowerCheck.md): placeholder CHECK draw with odds in tuning, day 1 all SAFE. Done when: all five locations stamp in Play.
- 10.6 Carried logbook, Today and Settings tabs only (Logbook.md). Done when: stamps and the last Ward reading show.
- 10.7 Chores: pump, wood and stove, store, forage, resolving a CHECK. Done when: each need ticks in the logbook.
- 10.8 One placeholder conversation that counts as Social. Done when: talking ticks Social.
- 10.9 File report and day-end confirm (DayEndConfirm.md). Done when: File is locked until all five are stamped, and Not yet backs out.
- 10.10 Night: night lighting state, the Ward climb opens, bunk counts as Give nothing from night 2. Done when: filing brings night.
- 10.11 Ward screen (WardNight.md): give 0 to 3, stats, sleep, autosave. Done when: WARD changes by points given minus hunger.
- 10.12 Run-end screen returning to the menu. Done when: forcing a stat to 0 shows it.
- 10.13 Settings panel: controls, display, reduce flashing; no sound sliders yet.
- 10.14 Content warning screen (ContentWarning.md). Done when: it shows on first launch only.
- 10.15 Main menu scene from the recipe (MainMenu.md). Done when: every load result case is shown.
- 10.16 [G] Grant plays several weeks until a run ends; he also judges each screen.

Wren's notes: the UI specs are approved by being built and judged in 10.16. The Ward screen follows DailyLoop.md (give 0 to 3 a night). Sound cues wait for the postponed sound milestone.
