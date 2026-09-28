# Daily loop

**DRAFT, 2026-09-28, Sable. Nothing here is decided.** This is the single source for the loop (DECISIONS 2026-09-28). Main3.md and DESIGN.md point here. Drawing: Docs/Design/DailyLoop_flow.svg. Numbers are provisional until the Milestone 7 simulator runs. Places, routes and walk times are in Main3.md.

Binding: DECISIONS.md, all 2026-09-28 lines. One wake-up is one day. A run ends the first time HP, MIND or WARD reaches 0. No stockpile. Stats on screen only at the Ward. Feeding at no better than 1 for 1 is a Milestone 7 headline, not a decision; this draft proposes it (question 2).

## 1. The day, step by step

Day is the burning sunset, sun fixed, from waking until the report is filed.

1. **Wake** in the cabin bunk. The objective line shows "Climb the tower" for 3 s.
2. **Climb the tower.** Stairs, then the lectern on the deck: the logbook page and the binoculars (Pim).
3. **Look at each location** except the cultist cave: Lake, Camp 1, Camp 2, Camp 3, Office. Hold the binoculars on a location for 2 s and its line in the log unlocks. The game stamps it SAFE or CHECK ON FOOT from that day's event. The player only looks; spotting is never a skill here. A CHECK shows as a change of state at the landmark, never its removal (Pim), and sometimes as a sound heard through the binoculars instead of a sight (Hollis).
4. **Objective update.** When all five lines are stamped, the objective line changes for 3 s: "All safe", or "Check the Lake" (one line per CHECK). It shows only when it changes.
5. **Rounds on foot**, in any order:
   - go to each CHECK location and resolve its anomaly (Safety);
   - survival chores (Food, Water, Warmth) from the options in section 3;
   - talk to a resident (Social).
6. **File the report** at the cabin desk. The File button stays locked until all five tower lines are stamped. A confirm box: "Filing the report ends your day." It lists every need still unmet and every CHECK still open. "Not yet" has the focus; "File" is second. The day-end tone plays on File (Hollis).
7. **Night falls.** Dark, only the fire glow. Section 2.

### 1.1 The duty clock
1. The sun does not move, so time is kept by the keeper's watch, on the logbook page and the wrist (question 1).
2. A day holds 9 duty hours. One duty hour is 60 walk-seconds. The watch moves by distance (1 s per 2.5 m, stairs included) and by a fixed cost per action. Sprinting saves real time, not duty time.
3. At the end of duty every chore, talk and anomaly prompt closes and the objective becomes "File the report". If the tower is not done yet, it becomes "Climb the tower" first.

## 2. The night

1. After filing, the objective shows "Go to the Ward" for 3 s. The J gate on the Ward path opens (chain down, cairn lamp lit).
2. The only action at night is the Ward. Other prompts are closed. Residents are inside.
3. **The Ward screen** (Pim): the player faces the stones, looking west, the fire behind them. Stats show here and only here: HP, MIND, WARD, tonight's unmet needs and their cost. The player offers HP, MIND, or nothing.
4. **Ward rates** (proposed, question 2):

| Offered tonight | WARD change | Total points change |
|---|---|---|
| nothing | -1 | -1 |
| 1 HP or 1 MIND | 0 | -1 |
| 2 (any mix) | +1 | -1 |

   The Ward is hungry 1 every night. Each point offered buys 1 WARD. At most 2 a night.
5. **Ignoring it:** going to the bunk instead counts as offering nothing. WARD -1, no stats screen that night.
6. **Sleep.** After the screen, fade out at the ledge, wake in the cabin. Needs are paid and the game autosaves at sleep.
7. WARD at 0: the barrier falls and the world burns. HP or MIND at 0: the run ends. Offering your last HP or MIND is allowed and ends the run there; the screen warns once.

## 3. Needs, where they are met, and what missing costs

| Need | Missed | Option | Cost | Catch |
|---|---|---|---|---|
| Food | -1 HP | Store in the front zone | 30 s | sure, but the far edge of the map |
| | | Forage patch A or B on the trails | 40 s | about 60 percent; a second try is 40 s at the other patch |
| | | Share the cookfire at Camp 1 | 90 s | only on days the resident cooks |
| Water | -1 HP | Hand pump on the lake dock | 20 s | sure |
| | | Creek at the plank bridge below J | 20 s | closed on ash days |
| | | Rain barrel at Camp 2 | 20 s | resident home and barrel not dry |
| Warmth | -1 HP | Split wood and light the cabin stove | 40 s | sure |
| | | Sit at the Camp 3 fire | 60 s | resident home |
| Social | -1 MIND | Talk to any resident at home, the cave's included | 60 s | one resident only leaves notes: a reply counts, 30 s |
| | | The resident who sometimes climbs the tower in the morning | 0 s | some days only |
| Safety | -1 MIND | Resolve every CHECK anomaly on foot | 60 to 120 s each | an all-SAFE day counts as met |

1. Each need is yes or no per day and paid at sleep. Meeting a need never restores a stat.
2. **Store price:** there is no money and no stock to carry (DECISIONS 2026-09-27). The price is the walk. Some days the shelves are empty (a flag, section 5).
3. **Social with no one reachable:** missed. No fallback. Absences are an event lever.
4. **The cave resident:** talking counts as Social. The cave is on foot by day, on a dead-end spur, never checked. Any further cost belongs to the events design.
5. **Safety on an all-SAFE day: met.** Quiet days are the relief the rising odds take away (5.2).
6. Borrowed: the nightly ledger of unpaid needs is Papers, Please. Forage chance against the sure, far store is Dredge's risk against a safe haul.

### 3.1 Unresolved anomalies
1. Each day unresolved: Safety missed, -1 MIND. The anomaly moves to its next stage, stays CHECK, and takes longer to resolve.
2. Third day unresolved: the location closes for 3 days. Its resident is absent and its option is lost (Camp 1 meal, Camp 2 barrel, Camp 3 fire, the store with the Office, Social there). WARD -1 once (question 4).
3. Each anomaly belongs to one resident (Quill). Resolving it is how the player meets that person's story.

### 3.2 Edge rules
1. **Skipping the tower:** impossible. The report cannot be filed without the five stamps, and the day cannot end without the report.
2. **Filing first thing:** the earliest filing is right after the tower (about 120 s). It costs every chore: Food, Water, Warmth -3 HP, Social -1 MIND, Safety -1 MIND if any CHECK. Up to 5 points plus the Ward's 1.
3. **Filing with open CHECKs:** allowed, listed in the confirm box, paid as 3.1.
4. **HP or MIND reaching 0 by day:** stats move by day only through events. If one reaches 0, the run ends on the spot.

## 4. The guaranteed loss

1. The Ward takes 1 point every night whatever the player does (table 2.4). No chore gives a point back. The 36 points fall by at least 1 a day.
2. The time budget (Main3.md 7) lets all five needs fit only on a quiet day with the near plan and a lucky forage, or with a near anomaly. Far or double anomalies always cost at least one need (question 3).

| Day type | Needs missed | Ward | Points lost |
|---|---|---|---|
| Quiet, near plan, forage finds | 0 | 1 | 1 |
| Quiet, forage misses | 1 | 1 | 2 |
| One far anomaly (Office, Camp 1, Camp 2) | 1 to 2 | 1 | 2 to 3 |
| Two anomalies | 2 to 3, one CHECK left open | 1 | 3 to 4, plus escalation |

3. With 36 points the longest possible run is 35 days. With anomaly odds rising (5.2) a typical run ends between day 10 and 18. Real time per day is about 11 min (Main3.md 7), so a run is about 2 to 3.5 hours.
4. Recovery (memories, items) is not in this loop. It comes in Milestone 7 and must stay below 1 point a day on average, or the run can become endless.
5. The player never controls whether they lose, only which stat goes first and when.

## 5. Variety

1. Location state is data, one record per location per day (Rook): state (SAFE, CHECK, CLOSED), anomaly id and stage, resident (home, absent, notes only, at the tower), option flags (cooking, barrel full, creek clean, store stocked).
2. Anomaly odds per checked location per day: 10 percent on day 1, plus 2 points a day, cap 50 percent. At most two CHECKs a day. Chance of an all-SAFE day: about 59 percent on day 1, 19 percent on day 10, 4 percent on day 20.
3. Each location has its own odds and anomaly pool (tuning asset), so far and near places do not feel alike.
4. The cultist cave has its own event deck and is never checked. Its chant carries to two trail spots (Hollis).

## 6. What fills a quiet day

A day with every location SAFE must still be worth playing on day 20 (Marlow).

1. **Route choice.** All five fit only by the near plan, and the forage roll can miss. The sure store eats most of a day.
2. **Residents' routines.** One is sometimes absent, one only leaves notes, one sometimes climbs the tower (Quill). Who is where changes the best route.
3. **Option flags.** Creek fouled, barrel dry, no cooking: a quiet day can still close a sure option.
4. **Conversation.** Each resident's talk moves on only when visited. Quiet days are when the far residents are affordable.
5. **Scarcity.** By day 10 a quiet day is a gift, the way a slow night is in Shift at Midnight.

## 7. What breaks the routine

Events (Milestone 14) can:
1. block a step (fog on the tower: no stamps, every location unknown until visited on foot, and the File lock opens);
2. block or add a need option (pump seized, the store stocked with something wrong);
3. add a location (a trail that was not there);
4. replace a step (the tower visitor waiting at the lectern);
5. hijack the night (something between the gate and the Ward);
6. drop the player into a minigame, the only place where spotting is a skill.

## 8. What none of the comparables do

The tower check is Firewatch's lookout and Papers, Please's inspection, but the game stamps the verdict and the player pays for it in walking. The report box is Kiosk's end of shift, except filing it hands you to the thing that eats you. Every day ends at the Ward, and the only choice left is which part of you it takes.

## 9. Open questions for Grant

1. The sun stays fixed, so the day is timed by the keeper's watch: 9 duty hours, moved by walking and by chores, not by real seconds. When it runs out you must file. OK?
2. Every night the Ward takes 1: offer 1 HP or MIND and it holds, offer 2 and it rises 1, offer nothing and it drops 1. Going to bed instead counts as nothing. OK?
3. On a quiet day near home you can meet all five needs, and the Ward still takes its 1. Or should the day be shorter so all five never fit?
4. An anomaly left alone gets worse each day. After three days that place closes for three days (resident gone, their help lost) and WARD drops 1. OK?

Sable
