# Front zone: interaction points for 8.22 (Pim, DRAFT 2 2026-10-02, nothing decided)
Inputs: FrontLayout.md draft 2 (positions below follow it), Main3.md 3.1 and 3.2, Valley.md 6, 8 and 11, UI/GateBooth, Dialogue, Examine, Docs/Private/StoreUI. Code (verified): reach 2 m, one eye ray, first collider hit wins, Interact E / pad Y. No booth, store-door, radio or barrier code exists yet.

| Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|
| Barrier arm, post (389.3, 166.3) | none | arm across the lane, 1.0 high | none. Solid when down. IW1 (gate opening, x 396) always solid; touching it in a shift shows `You can't abandon your post.`, outside a shift nothing. IW3 (z 210) on while an admitted car is on the spur; touching it shows the line (GateBooth 9.3) |
| Booth window (391.0 to 392.8, north face of the booth at x 390.6 to 393.2, z 164.2 to 166.6) | 2 m | counter top under the lane window, from inside (door 1.0 m on the south face) | `Work the window` when the gate is active and by day; shutter down otherwise, no prompt. Walk plus 1 press to the counter view |
| Shift | none | car at the barrier, driver's door 1.2 m off the window | none. Starts when the player enters the booth (Wren 2026-10-02); ends at the day's last car, or on leaving the booth with no car waiting. No press |
| Office west door | none | open doorway | none. Propped open inward (Wren 2026-10-02), no prompt. Shut only as an Office CHECK state |
| R5's spot, front room | 2 m | R5 at the counter, head and shoulders | `Talk` (Dialogue.md; Social, starts his minigame). Counter itself: no prompt |
| Radio, front room | none | none | none. Sound only (DailyLoop 6.1). `Examine` only if Quill writes a line |
| Door to back room | 2 m | door leaf | `Open` / `Close` |
| Back room cot, desk | 2 m | cot blanket, desk top | `Examine` only where Quill writes a line. No `Sleep` (the keeper sleeps in the bunk) |
| R6 at the car (370, 179.4) | 2 m | driver's window | `Talk` |
| Store door | none | door and lit sign | none. Spring push door, walk into it (StoreUI 3.1); crossing the threshold loads the store level. After filing: locked, handle rattles, no line. Walk plus 0 presses. Rook: push door and level load unverified |
| Trailhead board (338, 170) | 2 m | map face at eye height | `Examine` (Examine.md read view of the map). Diegetic map |
| Gate T: stop sign, entrance sign, mailbox (428, 170) | none | seen through the gate, outside the fence | none. Not reachable |
| Payphone; fee booth phone (390, 238) | n/a | n/a | not here: payphone is Camp 2 (8.25); the fee booth phone is past IW3 (8.27) |
### Spacing (camp rules: interactable colliders 1.2 m apart centre to centre, 0.5 m edge to edge; none in front of another within 2 m along the approach)
1. Office front room: R5, radio and back-room door 1.2 m or more apart; R5 faces the outer door, reached in a straight look from 1.5 m inside it. Booth: the window counter is the only interactable in the 2.6 x 2.4 m hut.
2. IW1 and IW3 sit off the interactor mask, so they never take the eye ray. Store doorway: ice chest and propane cage 1.5 m clear of it. Car: nothing interactable within 2 m of the driver's window.
### Found targets (rule: clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
3. From T (340, 170) heading east off the Jg trail: trailhead board, R6's car. From the lot centre (358, 170) heading north: office door, store door. Heading east: the car.
4. From the lot's east edge (373, 170) heading east: booth (19 m, 14 degrees), barrier arm (16 m, 0), spur mouth. Gate T stop sign: one frame from there through the gate (55 m, about 2 degrees), seen, never reached. Inside the office: R5 and the back-room door found from the outer doorway, heading in. Figures are on paper from Main3 coordinates; the built frames decide. Pim
