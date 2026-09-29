# Events

**DRAFT, revision 2, 2026-09-29, Sable. Nothing here is decided.** Mechanics only. No names, motives or story: Quill fills the slots after Grant's story sessions. Anything that carries the twist goes in Docs/Private. The loop is in DailyLoop.md; places and legs in Main3.md. Numbers are provisional until the Milestone 7 simulator.

Revision 2 fits the grammar and slots to the resident storylines (their design is private). Residents appear here only by neutral place IDs.

Binding: DECISIONS.md 2026-09-28 and 2026-09-29. Each event costs MIND, HP or WARD by what it is, not on a fixed timer. An event may end the day; whether unmet needs are then paid varies by event. Safety is met by resolving that day's flagged events on foot. Spotting is only a skill inside minigames. Day 1 has no events. Warnings before a resident is lost are subtle. Loss timing varies per storyline; there is no universal clock. A completed resident stays. Chases never lead into the cultist cave.

## 1. Resident IDs

Neutral IDs by place. The private design maps them to people.

| ID | Place |
|---|---|
| R1 | Camp 1 |
| R2 | Camp 2 |
| R3 | Camp 3 |
| R4 | Lake |
| R5 | Office |
| R6 | Parking lot |
| R7 | Cave |

Each resident has a **storyline** with its own rhythm (how often they need the player) and **deadlines shown when set** (a plain line in the carried logbook, or a thing at their landmark). Storylines are not events: they run beside the draw. Events touch them only through the verbs and rules below.

## 2. The event record

One record per event (Rook sets the format, Milestone 13; one data format with dialogue).

| Field | Values |
|---|---|
| id | slot number until Quill names it |
| location type | Lake, Camp (any of 1 to 3), Office, Store, Keeper's camp, Tower, Trail leg, Cave, Ward, Map-wide |
| storyline | none, or a resident ID (R1 to R7) the event touches |
| severity | 1 minor, 2 serious, 3 dangerous |
| shown as | tower CHECK (landmark state), tower sound, not shown (found on foot), night |
| effects | one or more verbs from section 3 |
| resolve | the on-foot action that ends it (3.2) |
| if unresolved | what happens when the day ends with it open: its own cost, and whether it carries to tomorrow at its next stage |
| day-end needs | for events that END THE DAY: unmet needs paid or waived |
| fault | for any effect that pushes a storyline toward loss: the visible choice or visible failure it comes from (section 4) |
| stage | 1 to 3 if it is part of a track; the next stage is drawn only after this one ends |

## 3. Grammar

### 3.1 Verbs (what an event can do)
1. **FLAG** a location: the tower stamps it CHECK. Counts for Safety. Only the five checked locations can be flagged; an event on a trail beside one flags that location's line.
2. **BLIND** the tower: some or all lines cannot be stamped from the deck; they must be stamped on foot. Stamping on foot stays open after the day's other prompts close, so File always unlocks.
3. **BLOCK** a need option for the day (pump seized, store empty).
4. **ADD** a need option (a meal left out, a spring running clear).
5. **COST** HP, MIND or WARD: on trigger, on resolve, or if unresolved. The amount is the event's own.
6. **RESTORE** 1 HP or MIND, rare and capped (counts toward the recovery cap, DailyLoop.md 4.6).
7. **CLOSE** a trail leg for the day (deadfall, flood).
8. **OPEN** a place or path that was not there, for the day or for good.
9. **CHASE**: a pursuer on a named leg. Reach a safe point or be caught (3.4).
10. **END THE DAY**: the day stops; the player wakes into night at the Ward (DailyLoop.md 1.1). The record's day-end needs field says whether unmet needs are paid.
11. **HIJACK THE NIGHT**: something on the J to Ward climb or at the stones changes the night.
12. **MINIGAME**: the event plays as a minigame stage; its result sets the cost. Resident minigames are storyline sessions, not events (their design is private).
13. **CHANGE** a landmark or sound the player knows, no mechanical effect. Used by WARD bands (DailyLoop.md 5.3) and by storylines for their subtle warnings.
14. **ABSENT** a resident (by ID) for the day: their Social is blocked there and their storyline's rhythm and deadlines pause that day.
15. **STEP** a storyline (by ID): push it one step forward or one step toward loss. A step toward loss is allowed only with a fault (section 4).

### 3.2 Resolve actions (on foot)
Inspect (look and write it in the logbook), fix (hold interact at a marked spot), carry (bring an object from one place to another), put out (carry water to it), search (find one of several marked spots), talk (a resident conversation), follow (walk a leg behind something), hide or flee (chases), play (minigame).

### 3.3 Rules
1. Safety counts only FLAG events, which are drawn only in the checked-location rolls. Hidden and night events never cost the Safety need; their cost is their own.
2. Each FLAG event left open when the day ends costs 1 MIND for the Safety miss, per open CHECK, plus its own "if unresolved" cost.
3. Unresolved events cost what they say. There is no general aging timer.
4. Carrying: an event may say it carries to tomorrow at its next stage. Carried events still count toward the daily cap.
5. An event that ENDS THE DAY with other events open: each open event takes its own unresolved cost.
6. **Resident losses at dawn** appear as a FLAG on that place's line (CHECK ON FOOT), set by the storyline, not the draw. Inspecting it resolves Safety for that line. It does not count toward the two-FLAG cap. Its MIND cost is set by the storyline.
7. **Lost residents** take their storyline events out of the pool for the run. Place events that need them (ABSENT, Social) are skipped.
8. **Completed residents** stay for the rest of the run. Events may still ABSENT them or CHANGE their place, but never STEP them toward loss.

### 3.4 Chases
1. A chase runs only by day, from day 2, on a named trail leg. The long unlit stretches are Camp to Jg, Jg to T and W1 to Camp 3 (Main3.md 4.6).
2. **Chases never enter the cave spur** (DECISIONS 2026-09-29). A chase that reaches W1 ends there or turns back along the shore path.
3. Safe points: the keeper's camp, any lit camp, the lot.
4. A chase never starts while the player is in a minigame session, at the gate booth, inside the store, or inside the cave.
5. Caught: END THE DAY; the player wakes at the Ward. The record sets the cost and whether unmet needs are paid.
6. Being caught is a visible failure: storyline deadlines that fall on that day still count (section 4.3).

## 4. Events and storylines (the fault rule)

1. **No clock before meeting.** A resident's storyline starts the first time the player talks to them. Before that, no event may STEP, ABSENT or CHANGE that resident's storyline; events at their place use only the place slots (need options, fires, spot fires).
2. **A step toward loss needs a fault.** An event may STEP a storyline toward loss only when the record names the fault: a choice the player made on screen (a reply, a verdict, a choice at a table, leaving something where it should not be) or a failure the player saw happen (caught in a chase, stuck in the store, a failed lock or hand). An event on its own never moves anyone toward loss.
3. **Deadlines shown when set.** Storylines set their own deadlines and show them when set. An event that takes away the player's only way to meet a shown deadline that day (a BLOCK, a CLOSE, an ABSENT, a BLIND that keeps them on the tower, or an END THE DAY that was not the player's visible failure) slides that deadline by one day, shown in the logbook as a plain fact.
4. **Late gates.** While a storyline waits at its late gate, events may CHANGE its place but may not STEP it toward loss.
5. **Warnings are subtle.** Storyline warnings are CHANGE only: a landmark state at the tower check, a plain line in the logbook's tower line, a sound. Never a toast, never a CHECK stamp, never the word "warning".
6. **Variety.** Storyline events are drawn from each resident's own pool, seeded per run, with the same refill rule as place pools (5.6).

## 5. Drawing and escalation

1. **Day 1:** nothing is drawn.
2. **Dawn draw, from day 2:** for each checked location, roll its odds: 10 percent on day 2, plus 2 points a day, cap 50 percent. Then one roll for a hidden event (trail, keeper's camp, cave, map-wide) at 20 percent, and one for a night event at 10 percent, both rising 1 point a day. Storyline events for met residents are drawn from their own pools inside the location roll for their place.
3. **Cap:** at most two FLAG events open at once, one hidden, one night. Carried events count. Resident-loss FLAGs do not (3.3.6).
4. **Severity weights** by week (the Ward's hunger step) and by WARD band:

| Week | Severity 1 | Severity 2 | Severity 3 |
|---|---|---|---|
| 1 (days 2 to 7) | 70 | 25 | 5 |
| 2 (days 8 to 14) | 45 | 40 | 15 |
| 3 on | 25 | 45 | 30 |

   At WARD 5 or less, shift 10 points from severity 1 to severity 3.
5. **Tracks:** a track is three stages of one event line. Stage 2 is drawn no sooner than 2 days after stage 1 ends. Stage 3 is the weirdest (DESIGN.md).
6. **Pools reshuffle:** when a pool runs out (a location's, a resident's, the hidden or the night pool), its used events are reshuffled back in. A track stage returns only after its whole track has ended. So the odds, the severity weights and the all-SAFE figures hold for the whole run.
7. Chance of an all-SAFE tower check: about 59 percent on day 2, 22 percent on day 10, 5 percent on day 20 (before resident-loss FLAGs).

## 6. Starter slots (mechanics only)

Nineteen slots. Costs are proposals. Quill writes what each one is. The storyline column shows where a resident storyline plugs in; "place" means the resident at that place, by ID.

| # | Location type | Sev | Shown as | Effects | Resolve on foot | If unresolved | Storyline |
|---|---|---|---|---|---|---|---|
| 1 | Lake | 1 | CHECK | FLAG; BLOCK pump | fix at the dock | pump blocked tomorrow too; no stat cost | none |
| 2 | Lake | 2 | CHECK | FLAG; pump water costs 1 HP while open | inspect the shore, carry a sample to the office | -1 HP | none |
| 3 | Camp (any) | 1 | CHECK | FLAG; BLOCK that camp's need option | fix at the camp | -1 MIND | place (R1 to R3): may slide a shown deadline (4.3) |
| 4 | Camp (any) | 2 | CHECK | FLAG; ABSENT the place's resident | search 3 spots on the camp's legs | -1 MIND, carries as stage 2 | place (R1 to R3): rhythm paused while absent; only if met |
| 5 | Camp (any) | 2 | CHECK (smoke) | FLAG | put out: carry water from the nearest source | -1 WARD | none |
| 6 | Trail leg beside a checked location | 2 | CHECK on that location's line (smoke rising beside it), with a tower sound | FLAG that location; drawn in that location's dawn roll, from its pool; no sixth line | put out a spot fire on the leg beside it | -2 WARD | none |
| 7 | Office | 1 | CHECK | FLAG | talk, then fix at the office | -1 MIND | R5 if met, otherwise none |
| 8 | Store | 1 | CHECK on the Office line (the store has no line of its own) | FLAG; BLOCK store Food | inspect the store | store blocked tomorrow; no stat cost | R6: a store BLOCK slides R6's shown deadline (4.3) |
| 9 | Office | 2 | tower sound (radio) | FLAG | answer at the office | -1 MIND, carries as stage 2 | none |
| 10 | Tower | 1 | the deck | BLIND the tower | stamp all five on foot | File stays locked until all five are stamped; if another event ends the day first, the missing stamps are waived and no cost | slides any shown deadline the blind keeps the player from (4.3) |
| 11 | Map-wide | 1 | not shown | BLOCK creek; CHANGE the sky | none; ends at sleep | none | none |
| 12 | Keeper's camp | 2 | not shown (at wake) | COST 1 MIND on finding it | search the camp | -1 MIND | none |
| 13 | Keeper's camp | 1 | not shown | ADD a Food option | inspect | none; accepting may have its own cost (story) | none |
| 14 | Trail leg | 1 | not shown | CLOSE the leg for the day | fix (clear it) or go around | none | may slide a shown deadline if the leg was the only way (4.3) |
| 15 | Trail leg | 3 | not shown | CHASE on a long unlit leg, never the cave spur (3.4) | flee to a safe point | caught: END THE DAY, -1 HP; needs paid | caught counts as a visible failure (3.4.6) |
| 16 | Cave | 2 | chant spots louder | OPEN the cave deeper for the day | follow down the spur and inspect | -1 MIND (from night 1 only) | R7 if met: CHANGE only |
| 17 | Ward | 3 | night | HIJACK THE NIGHT: something on the climb | hide, then reach the stones | caught: -1 MIND; the Ward screen still opens | none |
| 18 | Any location | 2 | CHECK | FLAG; MINIGAME (a non-resident minigame stage) | play | the minigame's own cost | none |
| 19 | Camp (any) | 3 | CHECK | FLAG; CHASE starts at the camp once inspected, onto its leg (never the cave spur) | inspect, then flee to a safe point | caught: END THE DAY, -1 HP, -1 MIND, needs paid; left alone: -1 WARD, -1 MIND | place (R1 to R3): never a STEP; the resident is ABSENT that day |

1. Severity spread: 8 at 1, 8 at 2, 3 at 3. More severity 3 slots are needed before the week 3 weights can be met (Milestone 14). Each location type has at least one slot.
2. Every slot can be met on foot, except 11 (weather) and 17 (night), which are not FLAG events.
3. Slot 10 is how the loop tests stamping on foot. It blocks filing until done, unless another event ends the day first.
4. No starter slot STEPs a storyline toward loss. Storyline events that do (each with its named fault) live in the residents' own pools, written after the story sessions.
5. What each slot is, who it belongs to, and what it means: story sessions, then Quill.

## 7. Open questions

For Grant: see DailyLoop.md 9. For the team:
1. Rook: one data format for events, storylines and dialogue (Milestone 13); the storyline record sits beside the location state record (DailyLoop.md 5.1).
2. Marlow: simulate the dawn draw with the cap and the resident pools for 30 days; check the Safety miss rate and that no loss happens without a named fault.
3. Hollis: a sound cue per slot, and per storyline warning (Milestone 14).
4. Quill: the residents' own event pools, each STEP toward loss with its fault named.

Sable
