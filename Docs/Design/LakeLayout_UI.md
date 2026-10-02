# Lake: interaction points for 8.23 (Pim, DRAFT 2026-10-02, nothing decided)
Inputs: Main3.md 2.1, 3.5.1 and 4, Valley.md 2 (M7, M8), DailyLoop.md 3, Events.md (Lake 1, 2), Dialogue, Examine, Docs/Private/FishingUI. Built positions from Marlow (Gate_816b_817). Code (verified): reach 2 m, one eye ray, first hit wins, Interact E / pad Y. No pump, bowl, rod or fishing code exists (grep of Assets).

| Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|
| Pump (190, 94.8), dock root | 2 m | handle and spout, 1.0 to 1.3 high, facing south off the trail end | By day: `Pump water`, 1 press, stroke sound about 2 s, Water ticks in the Logbook. Water met today or night: none. Lake event 1 open: `Fix the pump` (resolves it; Water blocked). Walk plus 1 press |
| Dock and end rail (z 86.4 to 96) | none | end rail, open water | none. Bucket and chain are dressing, off the interactor mask. No fishing from the pump dock |
| Boathouse east door (z 52.4), gangway | 2 m | door leaf | `Open` / `Close` (Door.cs), open by day. Built state unverified |
| Resident, inside by the west wall | 2 m | head and shoulders | `Talk` (Dialogue.md). The first talk counts as met, which gates the rods |
| North doorway to the step | none | doorway | none. Always open |
| Bowl, step (241.0, 56.3) | 2 m | bowl rim, about 40 degrees down from the doorway | Carrying a fish: `Put it in the bowl`. Otherwise `Examine` (Quill's line). Walk plus 1 press |
| Chair, step (239.1, 56.0) | 2 m | seat | Carrying a fish: `Eat it` (open 1). Otherwise none |
| The cat | none | on the step | none. Its collider stays off the mask so it never takes the bowl's ray |
| Rod rests: Reeds (232, 55), Stilts (242, 46) | 2 m | rod in the rest, butt at hip height | From day 2, by day, once met, session not spent: `Fish` (enters the stance). Otherwise rests empty, no prompt. Walk plus 1 press |
| Shore and wade limit | none | the water edge | none. Invisible wall at 0.97 of the lake radii (tops -5.0 under the dock deck); no message. Never on the mask |
| Signs, markers | none | pump lantern (lit), boathouse tin roof, rowboat | none. No signs at the lake; none called for |
| Lake event 2 sample | 2 m | a point at the water edge (position: Sable) | While open: `Take a sample`, carried to the office. Carry blocks other prompts |
### Spacing (camp rules: 1.2 m centre to centre, 0.5 m edge to edge, none in front of another within 2 m along the approach)
1. Bowl to chair 1.9 m centre to centre: passes; edges unmeasured. From the doorway the bowl is down-right, the chair down-left; the chair back must not cross the bowl's ray. Rests are 8 m (Reeds) and 10 m (Stilts) from the step: no conflict.
2. Pump: the only interactable on the dock. Resident, east door and north doorway 1.2 m or more apart; the resident is not on the east door to north doorway line.
### Found targets (rule as FrontLayout_UI: clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
3. Camp to pump, last bend heading south: pump, lantern, dock end rail. Pump to boathouse path, 30 m before the gangway foot (247.6, 52.4), heading east: boathouse, gangway, Reeds rest. Gangway foot heading south-west: Stilts rest (8.5 m).
4. East doorway heading west: resident, north doorway. North doorway heading north: bowl and chair (0.2 m bowl at 2 m is about 6 degrees). Figures on paper; the built frames decide.
Open: (1) eat point: FishingUI 9.2 puts `Eat it` on a pump dock chair; Valley M8 and the build put the chair on the step. My pick the step chair: the choice sits beside the bowl, and the dock gets no fish prompt. (2) Stilts rest stands 2.8 m off the boathouse south wall; no standing deck is built or walked. (3) Main3 east door vs Marlow's north doorway: both, as above, unless Sable says otherwise. Pim
