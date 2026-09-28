# Daily loop

**DRAFT, revision 4, 2026-09-28, Sable. Nothing here is decided.** This is the single source for the loop (DECISIONS 2026-09-28). Main3.md and DESIGN.md point here. Drawing: Docs/Design/DailyLoop_flow.svg. Numbers are provisional until the Milestone 7 simulator runs. Places, routes and walk times are in Main3.md.

Binding: DECISIONS.md, all 2026-09-28 lines. One wake-up is one day. A run ends the first time HP, MIND or WARD reaches 0. No stockpile. Stats on screen only at the Ward. The Ward screen has Give nothing. The player carries the logbook. As WARD drops, the fire gets louder and the world gets weird. Day 1 plays as a very normal job. Feeding at no better than 1 for 1 is a Milestone 7 headline, not a decision; this draft proposes it (question 2).

## 1. The day, step by step

Day is the burning sunset, sun fixed, from waking until the report is filed.

1. **Wake** in the cabin bunk. The objective line shows "Climb the tower" for 3 s.
2. **Climb the tower** to the lectern on the deck (Pim).
3. **Look at each location** except the cultist cave: Lake, Camp 1, Camp 2, Camp 3, Office. Hold the binoculars on one for 2 s and its line in the carried logbook unlocks. The game stamps it SAFE or CHECK ON FOOT from that day's event. The player only looks; spotting is never a skill here. A CHECK shows as a change of state at the landmark, never its removal (Pim), and sometimes as a sound through the binoculars instead of a sight (Hollis).
4. **Objective update.** When all five lines are stamped, the objective line changes for 3 s: "All safe", or "Check the Lake" (one line per CHECK). It shows only when it changes.
5. **Rounds on foot**, in any order: resolve each CHECK (Safety); chores (Food, Water, Warmth, section 3); talk to a resident (Social).
6. **File the report** from the carried logbook, anywhere. The File button stays locked until all five tower lines are stamped. A confirm box: "Filing the report ends your day." It lists every need still unmet and every CHECK still open. "Not yet" has the focus; "File" is second. The day-end tone plays on File (Hollis).
7. **Night falls.** Section 2.

### 1.1 The duty clock
1. The sun does not move, so time is kept by the keeper's watch, on the logbook page and the wrist (question 1).
2. A day holds 9 duty hours. One duty hour is 60 walk-seconds. The watch counts grounded horizontal distance, 1 s per 2.5 m, everywhere including inside camp, plus a fixed cost per action. Sprinting saves real time, not duty time.
3. The tower visit is one fixed cost (100 s, cabin to lectern and back, five looks). Its stairs are not counted again.
4. An action can start only if its whole cost fits in the duty left. If not, its prompt reads "No time" and does nothing.
5. At the end of duty every chore, talk and anomaly prompt closes and the objective becomes "File the report" ("Climb the tower" first if the stamps are not done).

### 1.2 Staying on the trails
Routes are kept to the trails by the ground itself: giant root walls, deadfall, fern thickets and slopes. No off-trail line may save more than 10 percent on any leg; Marlow tests this in the blockout. The watch counts real distance anyway, so any shortcut that slips through is paid for honestly.

## 2. The night

1. After filing, the objective shows "Go to the Ward" for 3 s. The J gate on the Ward path opens (chain down, cairn lamp lit).
2. The only action at night is the Ward. Other prompts are closed. Residents are inside.
3. **The Ward screen** (Pim): the player faces the stones, looking west, the fire behind them. Stats show here and only here: HP, MIND, WARD, tonight's unmet needs and their cost. Choices: give 1, give 2 (HP or MIND in any mix), or Give nothing.
4. **Ward rates** (proposed, question 2):

| Choice | WARD | Total points |
|---|---|---|
| Give nothing | -1 | -1 |
| Give 1 HP or 1 MIND | 0 | -1 |
| Give 2 | +1 | -1 |

5. **The bunk at night:** from night 2, lying in the bunk opens the same Ward screen (the stones in a dream). There is no way to skip the screen, so no night passes without stats shown. Ignoring the Ward is choosing Give nothing. Night 1 is different (section 6).
6. **Sleep.** After the screen, fade out, wake in the cabin. Needs are paid and the game autosaves.
7. WARD at 0: the barrier falls and the world burns. HP or MIND at 0: the run ends. Giving your last HP or MIND is allowed and ends the run; the screen warns once.

## 3. Needs, where they are met, what missing costs

| Need | Missed | Option | Cost | Catch |
|---|---|---|---|---|
| Food | -1 HP | Store by the office, front zone | 30 s | sure, but the far edge of the map |
| | | Forage patch A or B on the trails | 40 s | about 60 percent; a second try is 40 s at the other patch |
| | | Share the cookfire at Camp 1 | 90 s | some days only |
| Water | -1 HP | Hand pump on the lake dock | 20 s | sure |
| | | Creek at the plank bridge below J | 20 s | closed on ash days |
| | | Rain barrel at Camp 2 | 20 s | some days dry |
| Warmth | -1 HP | Split wood and light the cabin stove | 48 s | sure; includes the walk to the chopping block |
| | | Sit at the Camp 3 fire | 60 s | some days only |
| Social | -1 MIND | Talk to a resident who is home | 60 s | who is home when: story session |
| Safety | -1 MIND per open CHECK | Resolve the CHECK on foot | 90 s (60 to 120 by anomaly) | an all-SAFE day counts as met |

1. Each need is yes or no per day and paid at sleep. Meeting a need never restores a stat.
2. **Store price:** no money, no stock carried (DECISIONS 2026-09-27). The price is the walk.
3. **Social with no one reachable:** missed. No fallback.
4. **Safety:** each CHECK left open at sleep costs 1 MIND, so two open cost 2.
5. Resident roles (one who leaves notes, one who visits the tower, one sometimes absent), whether the cave resident talks, and what talking there costs: placeholders for the story session.
6. Borrowed: the nightly ledger of unpaid needs is Papers, Please. Forage chance against the sure, far store is Dredge's risk against a safe haul.

### 3.1 Open CHECKs
1. A CHECK left open carries to the next day at its next stage: still CHECK, 30 s longer to resolve, and 1 MIND each night it stays open.
2. After its third stage an open CHECK ends on its own, badly: WARD -1 once, and its resident's story takes the bad turn (story session). There are no closed locations, so every location can always be stamped and File never locks. (Closures are dropped; question 4.)
3. Carried CHECKs count toward the cap: at most two open at once. No new CHECK is drawn while two are open.
4. Each anomaly belongs to one resident (Quill).

### 3.2 Edge rules
1. **Skipping the tower:** impossible. No stamps, no report; no report, no night.
2. **Filing first thing:** the earliest filing is right after the tower. It costs every chore: -3 HP, -1 MIND for Social, -1 MIND per open CHECK, then the Ward's 1.
3. **HP or MIND at 0 by day:** stats move by day only through events. If one reaches 0, the run ends on the spot.

## 4. The guaranteed loss

1. Start values: HP 12, MIND 12, WARD 12 (provisional).
2. The Ward takes 1 point every night whatever the player does. No chore gives a point back. The 36 points fall by at least 1 a night.
3. Every stat must stay at 1 or more, so the longest possible run ends on night 34 (after 33 nights the best case is 1, 1, 1).
4. The time budget (Main3.md 7) lets all five needs fit only on a quiet day with the near plan and a lucky forage, or with a Camp 3 anomaly (2 s spare). Every other anomaly day costs at least one need (question 3).

| Day type | Needs missed | Ward | Points lost |
|---|---|---|---|
| Quiet, near plan, forage finds | 0 | 1 | 1 |
| Quiet, forage misses | 1 | 1 | 2 |
| One anomaly | 1 to 2 | 1 | 2 to 3 |
| Two anomalies | 2 to 4, one CHECK open | 1 | 4 to 6, and it carries |

5. A typical run ends between night 10 and 18. A day is about 11.5 min of real time (Main3.md 7), so a run is about 2 to 3.5 hours.
6. Recovery (memories, items) is not in this loop. It comes in Milestone 7 and must stay below 1 point a day on average.
7. The player never controls whether they lose, only which stat goes first and when.

## 5. Variety

1. Location state is data, one record per location per day (Rook): SAFE or CHECK, anomaly id and stage, resident state, option flags (cooking, barrel full, creek clean, store stocked).
2. Anomaly odds per checked location per day: 0 on day 1, 10 percent on day 2, plus 2 points a day, cap 50 percent. At most two open CHECKs. Chance of an all-SAFE day: about 59 percent on day 2, 19 percent on day 10, 4 percent on day 20.
3. Each location has its own odds and anomaly pool (tuning asset).
4. **WARD sets the weirdness** (DECISIONS 2026-09-28), proposed bands: WARD 9 to 12 normal; 6 to 8 the fire is louder and ash falls in daylight; 3 to 5 small wrong things in familiar places; 1 to 2 the world leans on you (Vesper and Hollis to fill). The fire is loudest at the three west glimpses (Hollis).
5. The cultist cave has its own event deck and is never checked. Its chant, from night 1 on, carries to two trail spots at two volumes, rising down the spur (Hollis).

## 6. Day 1 and night 1

Day 1 is Firewatch: a very normal job (DECISIONS 2026-09-28).
1. All five locations stamp SAFE. Every resident is home and ordinary. No chant, no weirdness, no anomaly.
2. The tower shows a plain lookout's view: forest, the lake, camp smoke, the office mast, and far to the west an ordinary distant forest fire, a smoke column and a glow over the far ridge (question 5). Nothing magic, nothing held back.
3. The objective lines teach the loop: tower, chores, a talk, the report.
4. **Night 1 is the reveal.** The bunk is not an option; "Go to the Ward" is the only objective. At the ledge the runes wake, the valley opens below, and the fire is shown at its true scale, pressed against a line it cannot cross. The first Ward screen explains the choice once.
5. From day 2 the odds start (5.2).

## 7. What fills a quiet day

1. **Route choice.** All five fit only by the near plan, and the forage roll can miss. The sure store eats most of a day.
2. **Residents' routines** (placeholders): who is home changes the best route.
3. **Option flags.** Creek fouled, barrel dry, no cooking.
4. **Conversation.** Talk moves on only when visited. Quiet days are when the far residents are affordable.
5. **Weirdness by WARD** (5.4): a quiet day at WARD 4 is not a calm day.
6. **Scarcity.** By day 10 a quiet day is a gift, the way a slow night is in Shift at Midnight.

## 8. What breaks the routine

Events (Milestone 14) can block a step (fog on the tower: every location unknown and stamped only on foot), block or add a need option, add a location, replace a step, hijack the night, or drop the player into a minigame, the only place where spotting is a skill.

## 9. What none of the comparables do

Day 1 is Firewatch played straight. Night 1 tells you the lookout's real job. From then on the tower check is Papers, Please's inspection, but the game stamps the verdict and you pay for it in walking, and every day ends at the thing that eats you.

## 10. Open questions for Grant

1. The sun stays fixed, so the day is timed by the keeper's watch: 9 duty hours, moved by walking and chores. When it runs out you must file. OK?
2. Every night the Ward takes 1: give 1 HP or MIND and it holds, give 2 and it rises 1, Give nothing and it drops 1. OK?
3. On a quiet day near home you can meet all five needs, and the Ward still takes its 1. Or should all five never fit?
4. Instead of closing places: an anomaly left open costs 1 MIND a night, and after three days it ends badly on its own and costs 1 WARD. OK?
5. On day 1, before the Ward, should the fire look like an ordinary far-off forest fire (my pick), or be hidden completely?

Sable
