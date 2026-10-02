# Lake: interaction points for 8.23 (Pim, DRAFT 2, 2026-10-02, nothing decided)
Inputs: LakeLayout.md draft 1 (Sable, L1 to L15), LakeLayout_Story.md, Main3.md 2.1, 3.5.1 and 4, Valley.md 2 (M7, M8), DailyLoop.md 3, Events.md (Lake 1, 2), Dialogue, Examine, Docs/Private/FishingUI. Code (verified): reach 2 m (PlayerTuning.interactReach), one eye ray on the interactor mask, first hit wins, Interact E / pad Y, pitch limit 85 degrees. No pump, bowl, rod, carry or fishing code exists (grep of Assets).
Draft 2 changes: no `Talk` (the cat is R4; no resident); no door or doorway prompt; the Stilts rest is the slip inside the house, out the boat door; `Fix the pump` moves to the intake; event 2's sample is at the notch band; the reed bed and Reeds rest move north of the stake line; the shallows added.

| Point | Reach | Player looks at | Prompt (E / Y) |
|---|---|---|---|
| Pump (190, 94.8), dock root (L1) | 2 m | handle and spout, 1.0 to 1.3 high, facing south off the trail end | By day: `Pump water`, 1 press, stroke sound about 2 s, Water ticks in the Logbook. Water met today, night, or Lake event 1 open: none. Walk plus 1 press |
| Intake fix point (191.3, -4.6, 86.7), stand (190.8, 87.2) (L2) | 2 m | pipe elbow at the end post, 1.57 m from the eye, 63 degrees down | Lake event 1 open: `Fix the pump` (resolves it; Water then open). Otherwise none. Pipe has no collider; the point is its own collider on the mask. Walk plus 1 press |
| Notch band sample, target (188.1, -4.7, 88.6), stand (188.75, 88.9) (L3) | 2 m | dark band at the west water line, 1.66 m from the eye, 64 degrees down | Lake event 2 open: `Take a sample`, carried to the office. Otherwise none. Carry blocks other prompts |
| Dock rails, bucket, chain, end rail | none | open water | none. Dressing, off the mask. No fishing from the pump dock |
| Rowboat (223.6, 83.2) and oar (224.6, 84.6) (L4) | 2 m | oar shaft on the trail side | Oar only while its event wants it: prompt from Events.md [GAP: Events]. Rowboat none |
| East doorway (z 51.8 to 53.0), north doorway, boat door (L5, L6) | none | openings | none. Open, no leaf, no prompt |
| Slip rest, east rail (240.85, 52.4), stand (241.3, 52.4) facing 270 (L6) | 2 m | rod in the rest, butt at hip height; slip water, boat door, far shore | From day 2, by day, once met, session not spent, not carrying: `Fish` (enters the stance, FishingUI). Otherwise rest empty, no prompt. Walk plus 1 press |
| Crates, barrel, rope, bucket, empty hook (L7) | none | | none. Off the mask. Hook: Quill's lamp event may add one [GAP: Events] |
| Met | none | | First crossing of the north doorway by day. No prompt, no on-screen line; `Fish` shows from then on |
| Bowl, step (241.2, 56.5) (L8) | 2 m | bowl rim | Carrying a fish: `Put it in the bowl`. Otherwise `Examine` (Quill's line). Walk plus 1 press |
| Chair, step (238.8, 56.4), facing the tower (L8) | 2 m | seat | Carrying a fish: `Eat it`. Otherwise none |
| The cat, blanket (240.7, 56.65) | none | | none. No `Talk`. Both off the mask, so neither takes the bowl's ray |
| Reeds rest (243.9, 61.2), stand (244.3, 61.0) facing 290 (L13) | 2 m | rod in the rest; reed bed and open water | As the slip rest |
| Beach (244.3, 57.0) and shallows, bed -6.0 (L11) | none | | none. Walk in; no wade prompt, no message |
| Wading event points (L14): gap (237.5, 55.95); off the step (240.0, 57.4); rope drift (238.5, 60.0) | 2 m | at water level, eye -4.4 | Only while their event is open: prompts from Events.md [GAP: Events]. Otherwise none, off the mask |
| Stake line, wade box, ring (L12) | none | stakes and rope | none. Visible stop; no message. Never on the mask |
| Soft ground (246.8, 58.5) (L15) | 2 m | the ground patch | Only while its event is open [GAP: Events]. Otherwise none |

### Spacing (camp rules: 1.2 m centre to centre, 0.5 m edge to edge, none in front of another within 2 m along the approach)
1. Dock: pump to intake 8.2 m, pump to band 6.3 m, intake to band 3.7 m. From the intake stand the band target is 3.0 m off; from the band stand the intake is 3.4 m off: neither reaches the other. Pass.
2. House: slip rest is 2.4 m in from the east doorway on z 52.4; the doorway to north doorway line passes 2.1 m north of it. No other point inside. Pass.
3. Step: bowl to chair 2.4 m. Pass. Blanket and cat 0.5 m from the bowl, off the mask.
4. Shallows: the bowl is 1.65 m from a wader's eye at the off-step point (1.5 m out, 0.7 m up, through the north rail). FAIL unless fixed: the bowl and chair take rays only from the step floor (step-side check, coder), or the off-step point moves 0.5 m or more north. Off-step point to bowl 1.5 m, to chair 1.56 m centre to centre (pass, with the fix). Gap to off-step 2.9 m, off-step to drift 3.0 m. Pass.
5. Reeds rest: 0.8 m north of the stake line's bank end (no prompt there), 4.2 m from the beach, 5.5 m from the drift point. Pass.
### Found targets (rule as FrontLayout_UI: clear line by meshes, eye 1.6 m, within 30 m, within 45 degrees of travel, 1 degree tall or more, one labelled frame each)
6. Camp to pump, last bend heading south: pump, lantern, dock end rail. Pump heading south down the dock: intake elbow at 8 m (9 degrees off travel; needs 0.14 m or more of visible pipe or elbow), band at 6.5 m (17 degrees off; must read over the west deck edge, unverified).
7. Shore path heading east, 20 m short of the gangway foot (247.6, 52.4): boathouse, gangway, Reeds rest (about 18 degrees left; rod 0.35 m or more tall at 20 m). Gangway foot heading north-west: beach.
8. East doorway heading west: slip rest 2.4 m ahead, boat door and water beyond, north doorway. North doorway heading north: chair (45 degrees left, 1.7 m) and bowl (43 degrees right, 1.8 m), both on the limit; frame one step out. Beach heading west: off-step point and gap under the step; heading north: Reeds rest.
9. Figures on paper; the built frames decide.
Open: (1) wading, oar, hook and soft ground prompts wait on Events.md. (2) bowl reach from the shallows (spacing 4). (3) slip stance facing 270 sees only the boat door's frame width; Sable's S3 frame decides. Pim
