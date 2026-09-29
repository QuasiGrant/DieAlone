# Daily loop

**DRAFT, revision 6, 2026-09-29, Sable. Nothing here is decided.** This is the single source for the loop. Main3.md and DESIGN.md point here. Drawing: Docs/Design/DailyLoop_flow.svg. Events: Docs/Design/Events.md. Numbers are provisional until the Milestone 7 simulator runs. Places and walk times are in Main3.md.

Binding: DECISIONS.md, all 2026-09-28 and 2026-09-29 lines. One wake-up is one day. The day ends when the player files the report or an event ends it; there is no time budget. A run ends the first time HP, MIND or WARD reaches 0, and every run must end. Feeding may raise WARD. All five needs may sometimes be met. Events set their own costs. The fire is not visible at all on day 1. No stockpile. Stats on screen only at the Ward. Give nothing is a valid choice. The player carries the logbook.

## 1. The day, step by step

Day is the burning sunset, sun fixed, from waking until the day ends.

1. **Wake** in the cabin bunk. The objective line shows "Climb the tower" for 3 s.
2. **Climb the tower** to the lectern on the deck (Pim).
3. **Look at each location** except the cultist cave: Lake, Camp 1, Camp 2, Camp 3, Office. Hold the binoculars on one for 2 s and its line in the carried logbook unlocks. The game stamps it SAFE or CHECK ON FOOT from that day's events (Events.md). The player only looks; spotting is never a skill here. A CHECK shows as a change of state at the landmark, never its removal (Pim), and sometimes as a sound through the binoculars instead of a sight (Hollis).
4. **Objective update.** When all five lines are stamped, the objective line changes for 3 s: "All safe", or "Check the Lake" (one line per CHECK). It shows only when it changes.
5. **Rounds on foot**, in any order, as long as the player likes: resolve each CHECK (Safety); chores (Food, Water, Warmth, section 3); talk to a resident (Social).
6. **File the report** from the carried logbook, anywhere. The File button stays locked until all five lines are stamped (from the tower, or on foot when an event blinds the tower). A confirm box: "Filing the report ends your day." It lists every need still unmet and every CHECK still open. "Not yet" has the focus; "File" is second. The day-end tone plays on File (Hollis).
7. **Night falls.** Section 2.

### 1.1 When an event ends the day
1. Some events end the day early (a chase that catches the player, a collapse). Events.md sets which.
2. The player wakes into night already at the Ward. The report counts as filed. Whether needs still unmet are paid varies by event (DECISIONS 2026-09-29); each event says so in its record (Events.md 1). The event adds its own cost.

### 1.2 Staying on the trails
Routes are kept to the trails by the ground itself: giant root walls, deadfall, fern thickets and slopes. Walk times in Main3.md are information for pacing and events, not a budget.

## 2. The night

1. After filing, the objective shows "Go to the Ward" for 3 s. The J gate on the Ward path opens (chain down, cairn lamp lit).
2. The only action at night is the Ward. Other prompts are closed. Residents are inside.
3. **The Ward screen** (Pim): the player faces the stones, looking west, the fire behind them. Stats show here and only here: HP, MIND, WARD, tonight's hunger, tonight's unmet needs and their cost. The player gives 0 to 3 points of HP or MIND in any mix; 0 is Give nothing.
4. **Hunger and feeding** (weekly hunger 1, 2, 3 approved, DECISIONS 2026-09-29; the feeding numbers are proposed):
   - The Ward's hunger H is taken from WARD every night: H = 1 on days 1 to 7, 2 on days 8 to 14, 3 on days 15 to 21, and so on.
   - Each point given buys 1 WARD. At most 3 a night.
   - WARD, HP and MIND never go above 12. The screen does not accept a point that would push WARD past 12.
   - WARD change tonight = points given minus H. Give nothing: WARD falls by H.
   - Diegetic: the fire is closer each week; its roar at the Ward grows with H (Hollis).
5. **The bunk at night:** from night 2, lying in the bunk counts as Give nothing: WARD falls by H, no Ward screen, no stats that night. The only night action is going to the Ward (DECISIONS 2026-09-28). Night 1 is different (section 6).
6. **Nothing forces the player to act** (DECISIONS 2026-09-29). A player who never files the report or never goes to the Ward simply does not progress. No pause, no penalty: their choice. It is not a surviving run.
7. **Sleep.** After the screen, fade out, wake in the cabin. Needs are paid and the game autosaves.
8. WARD at 0: the barrier falls and the world burns. HP or MIND at 0: the run ends. Giving your last HP or MIND is allowed and ends the run; the screen warns once.

## 3. Needs, where they are met, what missing costs

| Need | Missed | Option | Catch |
|---|---|---|---|
| Food | -1 HP | Store by the office, front zone | sure, but the far edge of the map |
| | | Forage patch A or B on the trails | about 60 percent; the other patch is a second try |
| | | Share the cookfire at Camp 1 | some days only |
| Water | -1 HP | Hand pump on the lake dock | sure |
| | | Creek at the plank bridge below J | closed on ash days |
| | | Rain barrel at Camp 2 | some days dry |
| Warmth | -1 HP | Split wood and light the cabin stove | sure |
| | | Sit at the Camp 3 fire | some days only |
| Social | -1 MIND | Talk to a resident who is home | who is home when: story session |
| Safety | -1 MIND | Resolve every CHECK flagged today, on foot | an all-SAFE day counts as met |

1. Each need is yes or no per day and paid at sleep. Meeting a need never restores a stat.
2. All five can be met on a good day. The cost of a good day is the Ward's hunger, which rises (section 4).
3. **Store price:** no money, no stock carried. The price is the walk.
4. **Social with no one reachable:** missed. No fallback.
5. **An event left unresolved** costs what that event says (Events.md), on top of the Safety miss. There is no fixed aging timer.
6. Resident roles (notes only, visits the tower, sometimes absent), whether the cave resident talks, and what that costs: placeholders for the story session.
7. **Placeholder hook, character minigames** (DECISIONS 2026-09-29): there will be minigames that progress each of the six residents' dialogue and affect the multiple endings. Where they sit in the day, whether they count as Social, and how they feed the endings: not designed yet.
7. Borrowed: the nightly ledger of unpaid needs is Papers, Please. Forage chance against the sure, far store is Dredge's risk against a safe haul.

### 3.1 Edge rules
1. **Skipping the tower:** impossible. No stamps, no report; no report, no night. Nothing closes the tower or File, so this never soft-locks.
2. **Filing first thing:** allowed. Every need not met is paid at sleep.
3. **HP or MIND at 0 by day:** stats move by day only through events. If one reaches 0, the run ends on the spot.

## 4. Why every run ends (the timer)

1. Start: HP 12, MIND 12, WARD 12 (provisional). 36 points.
2. Every night the Ward takes H from WARD. Feeding only moves points from HP or MIND into WARD, 1 for 1. So the three stats together lose at least H every night, whatever the player does.
3. Minimum total loss, a perfect player meeting all five needs every day with no event costs:

| Nights | H each night | Total lost by the end |
|---|---|---|
| 1 to 7 | 1 | 7 |
| 8 to 14 | 2 | 21 |
| 15 to 18 | 3 | 33 |
| 19 | 3 | 36 |

4. All three stats at 1 or more needs 3 points left, so without recovery the longest possible run ends on night 19 (26 with the recovery cap, 4.6).
5. Every play style bottoms out:
   - **Feed everything:** WARD stays up, HP and MIND carry the whole hunger and hit 0.
   - **Give nothing:** WARD falls by H a night; 12 WARD is gone on night 10 at the latest.
   - **Balance:** all three fall together; the total still falls by H.
   - **Meet all five every day:** saves only the need costs, never the hunger.
   - **Hoard WARD high early:** the hunger doubles and triples later; the hoard is spent faster.
6. **Recovery cap:** all recovery together (RESTORE events, and later memories and items) gives back at most 1 point a day, and never lifts a stat past 12. With the full 1 a day, the net minimum loss is H minus 1: 0 in week 1, 1 in week 2, 2 in week 3, 3 in week 4. Total lost: 0 by night 7, 7 by night 14, 21 by night 21, 33 by night 25. **The longest possible run with recovery ends on night 26** (19 without). Any recovery with a fixed daily cap is overtaken once H passes it, so the run always ends.
7. Typical runs, with needs missed and event costs, end between night 10 and 16. At 10 to 15 real minutes a day, a run is about 2 to 4 hours.
8. Rising hunger is the timer (approved 2026-09-29). Events escalate on top of it (Events.md 3).

## 5. Variety

1. Location state is data, one record per location per day (Rook): SAFE or CHECK, event id and stage, resident state, option flags.
2. Events are drawn at dawn (Events.md 3).
3. **WARD sets the weirdness** from day 2 on, proposed bands: WARD 9 to 12 normal; 6 to 8 the fire louder and ash in daylight; 3 to 5 small wrong things in familiar places; 1 to 2 the world leans on you (Vesper and Hollis to fill). The fire is loudest at the three west glimpses (Hollis).
4. The cultist cave has its own events and is never checked. Its chant, from night 1 on, carries to two trail spots at two volumes, rising down the spur (Hollis).

## 6. Day 1 and night 1

Day 1 is an ordinary fire lookout job (DECISIONS 2026-09-29).
1. All five locations stamp SAFE. Every resident is home and ordinary. No event, no weirdness. The road carries ordinary distant traffic. The office radio has ordinary chatter. The cave is not visited or heard.
2. **The fire is not visible at all:** no glow, no smoke, no fire view, from the tower or anywhere. The tower shows a plain lookout's view: forest, the lake, camp smoke, the office mast, a clear western ridge.
3. The objective lines teach the loop: tower, chores, a talk, the report.
4. **Night 1 is the reveal.** The bunk is not an option; "Go to the Ward" is the only objective. At the ledge the runes wake, the valley opens below, and the player first sees the roaring wildfire and the Ward holding it off. The first Ward screen explains the choice once.
5. From day 2 events are drawn and WARD bands apply.
6. The wrongness cues (the silent road, the radio gone to static, the chant) begin only after the night 1 reveal and grow as WARD drops.
7. **Day 2 on** (DECISIONS 2026-09-29): the fire shows by day as far glow and smoke over the ridge, seen from the tower and the west glimpses, and it reads as huge in the distance: the smoke column fills a wide slice of the western sky and the glow spans the ridge. It grows louder and closer as WARD drops. Never on day 1. The Ward stays the only place that shows its base and the line it cannot cross.

## 7. What fills a quiet day

1. **Route choice.** Near options against sure ones: the store is certain and far, forage is near and can miss.
2. **Residents' routines** (placeholders): who is home changes the best route.
3. **Option flags.** Creek fouled, barrel dry, no cooking.
4. **Conversation.** Talk moves on only when visited.
5. **Weirdness by WARD** (5.3): a quiet day at WARD 4 is not a calm day.
6. **Scarcity.** Event odds rise (Events.md 3); by day 10 a quiet day is a gift, the way a slow night is in Shift at Midnight.

## 8. What none of the comparables do

Day 1 is Firewatch played straight. Night 1 tells you the lookout's real job. From then on the tower check is Papers, Please's inspection, the game stamps the verdict and you walk to pay it, and every day ends at the thing that eats you, hungrier each week.

## 9. Open questions for Grant

1. Feeding buys 1 WARD per HP or MIND point, up to 3 a night, and recovery is capped at 1 point a day. With the approved hunger that makes the longest run 19 nights, 26 with the most recovery. OK?

Sable
