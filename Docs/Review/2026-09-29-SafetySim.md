# Safety miss simulation, dawn draw, 30 days

2026-09-29, Marlow. Answers Events.md 5 item 2. Monte Carlo, 100,000 runs per case, awk, seed 12345.

## Assumptions
1. Draw per Events.md 3.2: five checked lines (Lake, Camp 1 to 3, Office), each rolls p = 10 percent on day 2, +2 points a day, cap 50 percent (reached day 22). Rolled in fixed order; once 2 FLAG events are open (cap 3.3, carried ones included) the rest stamp SAFE.
2. Slot 6 (trail leg FLAG, tower sound) is taken as 1 in 7 of the hidden roll (20 percent, +1 a day). Events.md does not say how slot 6 is drawn; see finding 4.
3. Safety missed = at least one FLAG open at File (DailyLoop 3, -1 MIND flat, whatever the count). Player resolves each FLAG with probability R, independently. R = 1, 0.8, 0.5, 0.
4. Unresolved slot 4 or 9 carries one day and holds a cap place. Infinite pool case: 20 percent of unresolved FLAGs carry.
5. Two pool cases, because Events.md 3.6 leaves refill to Milestone 14. Infinite: every roll that hits draws an event. Finite: the 11 FLAG slots (1 to 9, 18, 19; slot 8 on the Office line, slot 18 once at any line, Camp slots shared by the three camps) are each drawn once, then the line stamps SAFE.
6. Events that end the day, BLIND, and severity are ignored; they do not change whether Safety is at stake.
7. Days 2 to 30 = 29 drawn days. DailyLoop 4 says no run lives past night 26 (19 without recovery), so days 27 to 30 are unreachable and shown only because Events.md asked for 30.

## Per day: chance Safety is at stake (at least one FLAG), and chance it is missed

Infinite pool:

| Day | At stake | Miss R=0.8 | Miss R=0.5 | Miss R=0 | Mean FLAGs |
|---|---|---|---|---|---|
| 2 | 0.43 | 0.10 | 0.24 | 0.43 | 0.52 |
| 6 | 0.64 | 0.17 | 0.40 | 0.69 | 0.88 |
| 10 | 0.79 | 0.23 | 0.52 | 0.83 | 1.19 |
| 14 | 0.88 | 0.27 | 0.60 | 0.91 | 1.45 |
| 18 | 0.94 | 0.30 | 0.66 | 0.95 | 1.65 |
| 22 to 30 | 0.97 | 0.33 | 0.70 | 0.98 | 1.79 |

Finite pool, no refill:

| Day | At stake | Miss R=0.5 | Miss R=0 |
|---|---|---|---|
| 2 | 0.43 | 0.24 | 0.43 |
| 6 | 0.63 | 0.39 | 0.68 |
| 10 | 0.60 | 0.38 | 0.68 |
| 14 | 0.36 | 0.21 | 0.44 |
| 18 | 0.16 | 0.10 | 0.21 |
| 22 | 0.06 | 0.03 | 0.07 |
| 30 | 0.02 | 0.01 | 0.02 |

R = 1 misses 0 in every case.

## Expected total Safety misses (each is -1 MIND)

| Case | by day 19 | by day 26 | by day 30 |
|---|---|---|---|
| Infinite, R=0.8 | 4.0 | 6.3 | 7.6 |
| Infinite, R=0.5 | 9.0 | 13.9 | 16.7 |
| Infinite, R=0 | 14.3 | 21.1 | 25.0 |
| Finite, R=0.8 | 2.0 | 2.1 | 2.1 |
| Finite, R=0.5 | 5.0 | 5.2 | 5.2 |
| Finite, R=0 | 9.2 | 9.6 | 9.7 |

## Findings
1. Safety has no chance in it. A player who resolves every CHECK never misses; the draw only sets how much walking is owed. Difficulty is entirely in the events' own costs and walk length, not in the Safety need. Hurts.
2. Infinite pool: from day 18 Safety is at stake 94 to 97 percent of days, 1.8 FLAGs a day. An ignoring player loses about 14 MIND to Safety alone by day 19, more than the 12 MIND start, before any event cost or feeding. Ignoring Safety is fatal by itself; partial effort (R=0.8) costs about 4 MIND by day 19. Punishing but fair, provided walks are doable; not verified, needs the Milestone 7 simulator with walk times.
3. Finite pool (no refill): FLAG slots run out; at-stake falls below 40 percent by day 14 and below 7 percent by day 22. Safety becomes free late game, opposite to DailyLoop 7.6 ("by day 10 a quiet day is a gift") and Events.md 3.7 (5 percent all-SAFE on day 20). Those Events.md 3.7 figures hold only with an infinite pool. Blocks the escalation design until Milestone 14 decides refill.
4. Slot 6 is a trail leg FLAG shown by tower sound that counts for Safety, but 3.2 puts trail events in the hidden roll and 2.3.1 says hidden events never cost Safety. How slot 6 is drawn is undefined. Hurts.
5. Safety cost is -1 MIND flat whether 1 or 2 CHECKs are open. Once one CHECK is skipped, the second costs only its own unresolved cost, so skipping both is no worse for Safety than skipping one. Cosmetic to hurts, depends on event costs.
6. The cap binds from about day 10 (mean FLAGs 1.2 and rising to 1.8); with p at 50 percent, 5 rolls give 2 or more hits 81 percent of the time, so late days are almost always exactly two CHECKs. Variety in FLAG count disappears after day 20. Cosmetic.
7. 30 days exceeds the longest possible run (26 nights). Days 27 to 30 cannot be played.

Marlow
