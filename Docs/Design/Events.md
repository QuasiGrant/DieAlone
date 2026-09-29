# Events

**DRAFT, 2026-09-29, Sable. Nothing here is decided.** Mechanics only. No names, motives or story: Quill fills the slots after Grant's story session. Anything that carries the twist goes in Docs/Private. The loop is in DailyLoop.md; places and legs in Main3.md. Numbers are provisional until the Milestone 7 simulator.

Binding: DECISIONS.md 2026-09-28 and 2026-09-29. Each event costs MIND, HP or WARD by what it is, not on a fixed timer. An event may end the day. Safety is met by resolving that day's flagged events on foot. Spotting is only a skill inside minigames. Day 1 has no events.

## 1. The event record

One record per event (Rook sets the format, Milestone 13).

| Field | Values |
|---|---|
| id | slot number until Quill names it |
| location type | Lake, Camp (any of 1 to 3), Office, Store, Keeper's camp, Tower, Trail leg, Cave, Ward, Map-wide |
| severity | 1 minor, 2 serious, 3 dangerous |
| shown as | tower CHECK (landmark state), tower sound, not shown (found on foot), night |
| effects | one or more verbs from section 2 |
| resolve | the on-foot action that ends it (section 2.2) |
| if unresolved | what happens when the day ends with it open: its own cost, and whether it carries to tomorrow |
| day-end needs | for events that END THE DAY: unmet needs paid or waived |
| stage | 1 to 3 if it is part of a track; the next stage is drawn only after this one ends |
| residents | which resident it belongs to (placeholder) |

## 2. Grammar

### 2.1 Verbs (what an event can do)
1. **FLAG** a location: the tower stamps it CHECK. Counts for Safety.
2. **BLIND** the tower: some or all lines cannot be stamped from the deck; they must be stamped on foot. File unlocks when all five are stamped.
3. **BLOCK** a need option for the day (pump seized, store empty).
4. **ADD** a need option (a meal left out, a spring running clear).
5. **COST** HP, MIND or WARD: on trigger, on resolve, or if unresolved. The amount is the event's own.
6. **RESTORE** 1 HP or MIND, rare and capped (counts toward the recovery cap in DailyLoop.md 4.6).
7. **CLOSE** a trail leg for the day (deadfall, flood).
8. **OPEN** a place or path that was not there, for the day or for good.
9. **CHASE**: a pursuer on a named leg. Reach a safe point (keeper's camp, a lit camp, the lot) or be caught.
10. **END THE DAY**: the day stops; the player wakes into night at the Ward (DailyLoop.md 1.1). Whether needs still unmet are paid varies by event (DECISIONS 2026-09-29): the record's "day-end needs" field says paid or waived.
11. **HIJACK THE NIGHT**: something on the J to Ward climb or at the stones changes the night.
12. **MINIGAME**: the event plays as a minigame stage; its result sets the cost. Placeholder hook (DECISIONS 2026-09-29): some minigames progress one of the six residents' dialogue and affect the multiple endings. Not designed yet; they may be events, visits or their own track.
13. **CHANGE** a landmark or sound the player knows (a wrong thing in a familiar place), no mechanical effect. Used by WARD bands (DailyLoop.md 5.3).

### 2.2 Resolve actions (on foot)
Inspect (look and write it in the logbook), fix (hold interact at a marked spot), carry (bring an object from one place to another), put out (carry water to it), search (find one of several marked spots), talk (a resident conversation), follow (walk a leg behind something), hide or flee (chases), play (minigame).

### 2.3 Rules
1. Safety counts only FLAG events. Hidden and night events never cost the Safety need; their cost is their own.
2. Unresolved events cost what they say. There is no general aging timer.
3. Carrying: an event may say it carries to tomorrow at its next stage. Carried events still count toward the daily cap.
4. An event that ENDS THE DAY with other events open: each open event takes its own unresolved cost.

## 3. Drawing and escalation

1. **Day 1:** nothing is drawn.
2. **Dawn draw, from day 2:** for each checked location, roll its odds: 10 percent on day 2, plus 2 points a day, cap 50 percent. Then one roll for a hidden event (trail, keeper's camp, cave, map-wide) at 20 percent, and one for a night event at 10 percent, both rising 1 point a day.
3. **Cap:** at most two FLAG events open at once, one hidden, one night. Carried events count.
4. **Severity weights** by week (the Ward's hunger step) and by WARD band:

| Week | Severity 1 | Severity 2 | Severity 3 |
|---|---|---|---|
| 1 (days 2 to 7) | 70 | 25 | 5 |
| 2 (days 8 to 14) | 45 | 40 | 15 |
| 3 on | 25 | 45 | 30 |

   At WARD 5 or less, shift 10 points from severity 1 to severity 3.
5. **Tracks:** a track is three stages of one event line. Stage 2 is drawn no sooner than 2 days after stage 1 ends. Stage 3 is the weirdest (DESIGN.md).
6. **Empty pool:** if a location has no event left to draw, it stamps SAFE. Milestone 14 decides whether pools refill.
7. Chance of an all-SAFE tower check: about 59 percent on day 2, 22 percent on day 10, 5 percent on day 20.

## 4. Starter slots (mechanics only)

Nineteen slots. Costs are proposals. Quill writes what each one is.

| # | Location type | Sev | Shown as | Effects | Resolve on foot | If unresolved |
|---|---|---|---|---|---|---|
| 1 | Lake | 1 | CHECK | FLAG; BLOCK pump | fix at the dock | pump blocked tomorrow too; no stat cost |
| 2 | Lake | 2 | CHECK | FLAG; pump water costs 1 HP while open | inspect the shore, carry a sample to the office | -1 HP |
| 3 | Camp (any) | 1 | CHECK | FLAG; BLOCK that camp's need option | fix at the camp | -1 MIND |
| 4 | Camp (any) | 2 | CHECK | FLAG; resident absent, Social there blocked | search 3 spots on the camp's legs | -1 MIND, carries as stage 2 |
| 5 | Camp (any) | 2 | CHECK (smoke) | FLAG | put out: carry water from the nearest source | -1 WARD |
| 6 | Trail leg | 2 | tower sound | FLAG a sixth line for the day; not needed to File (File needs only the five location lines), counts for Safety | put out a spot fire on the leg | -2 WARD |
| 7 | Office | 1 | CHECK | FLAG | talk, then fix at the office | -1 MIND |
| 8 | Store | 1 | CHECK on the Office line (the store has no line of its own) | FLAG; BLOCK store Food | inspect the store | store blocked tomorrow; no stat cost |
| 9 | Office | 2 | tower sound (radio) | FLAG | answer at the office | -1 MIND, carries as stage 2 |
| 10 | Tower | 1 | the deck | BLIND the tower | stamp all five on foot | File stays locked until all five are stamped; if another event ends the day first, the missing stamps are waived and no cost |
| 11 | Map-wide | 1 | not shown | BLOCK creek; CHANGE the sky | none; ends at sleep | none |
| 12 | Keeper's camp | 2 | not shown (at wake) | COST 1 MIND on finding it | search the camp | -1 MIND |
| 13 | Keeper's camp | 1 | not shown | ADD a Food option | inspect | none; accepting may have its own cost (story) |
| 14 | Trail leg | 1 | not shown | CLOSE the leg for the day | fix (clear it) or go around | none |
| 15 | Trail leg | 3 | not shown | CHASE on a long unlit leg (Main3.md 4.6) | flee to a safe point | caught: END THE DAY, -1 HP |
| 16 | Cave | 2 | chant spots louder | OPEN the cave deeper for the day | follow down the spur and inspect | -1 MIND (from night 1 only) |
| 17 | Ward | 3 | night | HIJACK THE NIGHT: something on the climb | hide, then reach the stones | caught: -1 MIND; the Ward screen still opens |
| 18 | Any location | 2 | CHECK | FLAG; MINIGAME | play | the minigame's own cost |
| 19 | Camp (any) | 3 | CHECK | FLAG; CHASE starts at the camp once inspected | inspect, then flee to a safe point | caught: END THE DAY, -1 HP, -1 MIND; left alone: -1 WARD, -1 MIND |

1. Severity spread: 8 at 1, 8 at 2, 3 at 3 (slot 19 is the severity 3 FLAG event). More severity 3 slots are needed before the week 3 weights can be met (Milestone 14). Each location type has at least one slot.
2. Every slot can be met on foot, except 11 (weather) and 17 (night), which are not FLAG events.
3. Slot 10 is how the loop tests stamping on foot. It blocks filing until done, unless another event ends the day first.
4. What each slot is, who it belongs to, and what it means: story session, then Quill.

## 5. Open questions

For Grant: see DailyLoop.md 9. Slots 15 and 19 (chases) are proposed as needs paid; Quill may waive per event. For the team:
1. Rook: one data format for events and dialogue (Milestone 13).
2. Marlow: simulate the dawn draw with the cap for 30 days and check the Safety miss rate.
3. Hollis: a sound cue per slot (Milestone 14).

Sable
