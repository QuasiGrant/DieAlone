# Comparables, the warden's day, and the design milestones


**Superseded 2026-09-29.** Kept as a record of how the design got here. The loop now lives in Docs/Design/DailyLoop.md and the plan order in PLAN.md; where this file disagrees with DECISIONS.md, DECISIONS.md wins. Parts 1 and 2 (comparables, duty pool) are still useful background.
Started 2026-09-27 for the Milestone 6 rework. Moved into the repo 2026-09-28 so every agent can read it. This is the standing reference for Milestones 6 to 10. Nothing here is decided until it is written to DECISIONS.md and Grant has confirmed it; Part 4 tracks what has been. Add to Part 4 at the end of every design session.

## Part 1. Ten comparable games and their loops

Sources are web pages read on 2026-09-27; night and day counts are as reported there. Kiosk's night count was not confirmed.

### 1. Fears to Fathom (series, Rayll)
Anthology of short first-person episodes, each a different ordinary person in an ordinary place. The loop is the same in every episode: arrive, learn the space, do small real tasks (cook, shower, check the phone, answer texts, lock up), sleep, wake, repeat, while one wrong detail per cycle accumulates into a chase or escape on the last night. Episode 4, Ironbark Lookout, is the direct model for this game: a fire lookout who each day reads the thermometer and anemometer, files a weather report at the computer, fetches firewood, lights the stove, and checks in by radio with the next tower. About four to five days. Day 3 a campfire on the trail must be put out; night 3 a robed figure leaves a skull; day 4 a lost hiker's call goes silent and a maintenance man turns out to be lying; night 4 the generator fails, the cult is seen, and the episode ends in a hide-and-run to the truck. Takeaway: the chores are real and repeated until they are habit, then the same chores become the horror's delivery mechanism. VHS look and low-res picture are our look reference already.

### 2. Kiosk (Vivi)
One cook, one late-night burger kiosk, one night per level. Loop: a customer arrives, orders from a menu that grows night by night (burgers, coffee, then pancakes, sausage, eggs), the player grills and assembles by recipe, serves, repeat until the clock says the night is survived. Horror arrives through the customers: what they say about the murdered previous cook, occasional jump scares, a slow burn rather than the FNAF spike. Takeaway: the job itself is forgiving and pleasant; menace lives entirely in who shows up and what they say. The work is the cover for the story.

### 3. The Boba Teashop (miketendev)
Seven days as Risa running a boba shop, about an hour total. Loop: open, take orders, mix drinks from a small set that widens as customers ask for new ones, close, sleep. Each day the customers' attitude shifts and Risa's sanity visibly slips; scares are both jump and blink-and-miss. The ending is a single dark twist; two playthroughs cover everything. Takeaway: a fixed short count of days with one visible variable (the owner's state) drifting day to day is enough to carry a short horror game. Customer behaviour is the escalation dial.

### 4. Shift at Midnight (Ozk)
Night shift at a gas station where some customers are doppelgangers. Two jobs compete for attention: retail (stock shelves, take deliveries, clean, sell, meet a quota, later fuel cars and search vehicles) and verification (five questions, ID inspection, terminal records, watching behaviour before and after paperwork). A wrongly released doppelganger comes back later in the shift and the game turns into barricades, traps and hiding. Story mode is 13 nights with a randomised customer pool after the first. Co-op splits counter, floor and observer roles. Takeaway: the strongest model here for "routine plus judgement call": every mundane task is a timer running while the player decides about a person. That structure fits our other characters and their events.

### 5. Firewatch (Campo Santo)
A summer as Henry in a Wyoming lookout, days numbered on screen with growing skips (1, 2, 3, then 9, 33, 64). Loop: wake in the tower, get a task over the radio (put out a campfire, check a cut phone line, investigate), walk the trails while talking to Delilah, return, sleep. Conversations are timed to walking. No stats, no failure. Takeaway: walking is the game, talk fills the walk, and the day count is a narrative device more than a system. The tower and a radio voice make one person feel like company. Our Voices mechanic can do the same job without a second character.

### 6. Dredge (Black Salt Games)
Day and night loop at sea. Fish and sail by day, sell and repair at a dock before dusk, sleep to reset. Staying out in the dark raises Panic, which distorts the screen and spawns hallucinated hazards that damage the boat and cargo; Aberrations (corrupted fish) sell high and lure the player to risk it. Takeaway: one visible fear meter tied to a clock is a clean, readable design for MIND. The temptation loop (risk the night for the rare catch) is the same shape as feeding the Ward.

### 7. Papers, Please (Lucas Pope)
31 days at a border booth. Loop: process entrants against a rulebook that changes daily, earn per correct stamp, take citations for mistakes, then at night spend the pay on rent, heat, food and medicine for a family whose health drops when you cannot. Twenty numbered endings plus deaths, branched by money, family survival, a resistance faction and the day 31 choice. Takeaway: a job with a rulebook, a nightly ledger, and an ending matrix decided by which resource you let fail. This is the closest published model to our HP, MIND, WARD economy and the "which stat bottomed out" ending selection.

### 8. The Convenience Store (Chilla's Art)
Several nights on a Japanese konbini night shift, escape on night 4. Loop: register, restock, deal with rats, take out the trash, close, walk home, sleep; each night the store is a little more wrong (a prank, then the ghost on the cameras, then the shed). Two endings decided by one choice about a tape. Takeaway: the nightly walk home and the apartment are part of the loop, so the horror follows you out of work. Cheap, repeated tasks with one new wrong thing per night.

### 9. Mouthwashing (Wrong Organ)
Five crew on a cargo ship, told out of order before and after the crash, with glitch transitions between times. Play is walking, talking to the crew, and small item puzzles; no stats or combat. The horror is entirely the people: what they hide, how they change, what the routine of a dead-end job does to them. Takeaway: a small cast under routine is enough; the same rooms reused across a broken timeline become uncanny. Our campsites and their occupants can work this way, and a late reveal about who they are is Mouthwashing-shaped.

### 10. The Exit 8 (Kotake Create)
One corridor, walked over and over. Rule: if nothing is wrong, go forward; if anything is wrong, turn back; eight right calls in a row reach the exit, one wrong call resets to zero. Anomalies range from a changed poster to a flood or a thing walking toward you. Fifteen to thirty minutes. Takeaway: the purest spot-the-difference loop. It is the template for one of our minigame stages and for "clues at wake-up": the tower each morning is a corridor the player has walked before, and one thing is different.

### What the ten have in common
- A fixed count of shifts or days the player can feel (4, 7, 13, 31).
- The job is real, simple, repeated, and satisfying on its own.
- One new wrong thing per cycle; escalation is by content, not by difficulty.
- The people who show up are the horror's carrier.
- Where there are stats (Dredge, Papers), one visible meter is tied to the clock and one to the ledger; endings fall out of which meter failed.
- The walk between stations is where the story is told.

### Where we break from them (the novel part)
All ten keep the job and the horror separate until the last night. Ours does not. The Ward is a chore that eats you. The logbook is where your stats live. The rounds visit people who are not what they seem. One wake-up is a week, so the world jumps between days. And there is no fixed count of days: the run always ends by a stat bottoming out, and the measure is how long you lasted. The loop itself is the monster.

## Part 2. The warden's day to day

Scope: what a Wardkeeper actually does, week in week out, with no event running. This is the routine the events later break. Minigames, scares and event content are out of scope here. Written for the Modern era first because that is the scene we have; the Early era loses the generator, radio and vehicles and gains bells, lamps and a horse.

### The job in one sentence
Watch the fire, keep the Ward, keep yourself, and tell someone you are still here.

### Duties, grouped (the full pool; see Part 4 for what is mandatory)
Watching
- Climb the tower and read the fire line: where the glow is, how far, which way the smoke leans, how thick the ash fall is. Mark it on the tower map against fixed landmarks (the far ridge, the lake, the gate road).
- Weather: wind direction and strength, temperature, air quality. Read from instruments on the deck, write the numbers down.
- Spot fires: any smoke inside the Ward line is wrong. Walk to it, put it out, log it.

Keeping the Ward
- Daily maintenance that is not feeding: brush ash off the runes, clear roots and growth from the stone bases, check the runes are all still lit and note which are dim. This is the "check the Ward" chore; feeding is the design's own step and stays separate.
- Walk the Ward line: the boundary markers (posts, cairns, the fence in the Modern era) between the stones and the fire. Note any that have fallen or burned.

Keeping the camp
- Water: carry from the lake or creek to the cabin barrels. In the Modern era the lake and a hand pump; the dock is where the bucket goes in.
- Wood: split at the chopping block, stack the pile, keep the cabin stove and the fire pit going.
- Generator: fuel it from the drum, start it, note the fuel level. When it runs, the cabin has light and the radio has power.
- Repairs: a loose ladder rung, a shutter, a cracked barrel, the tower rail. Small fix-it jobs from a short rotating list.
- Cooking and eating: one meal a day matters to the game; the rest is flavour.

Foraging and food
- Berry and mushroom spots off the trails, a snare line in the forest, fishing from the dock. Each spot gives a little, some days nothing. Food is inventory; the design says it is HP.

Reporting
- The logbook on the cabin table: the week's weather numbers, the fire line, the Ward reading, and a line about yourself. This is the "report" step and the place the design says stats are shown.
- Outgoing: in the Modern era a radio check-in at a set hour, answered or not. In the Early era a signal lamp to a tower that may or may not answer. A written report left in the box at the gate for a pickup that comes or does not.
- Incoming: the notice board and the gate box. Supplies, mail, orders, nothing.

Rounds
- Walk to each campsite and back. Officially to check on the other campers; in play it is how the player meets them and their events. On a no-event day the sites are quiet and the walk is the ambience.
- The gate: check it is locked, check the box, look down the road.
- Off-limits: the cave and the Ward path are closed by day. Walking them is a choice, not a chore.

Self
- Wash, sleep, tend a wound, mend clothes, look at what is in your pack. Cheap actions that give the player a reason to be in the cabin at dusk.

### A no-event week, as the player would play it
1. Wake in the cabin. Light, stove, look around. (Later: the clue check.)
2. Cabin chores: water level, wood, generator. Ten minutes of small work in the clearing.
3. Tower: climb, read the fire and weather, mark the map. First long look at the horizon.
4. Rounds: pick a route. Lake for water and a line in the water, then the campsites, or the gate first. Forage on the way. This is the bulk of the day and the walk where voices and ambience live.
5. Back at camp: cook, eat, repairs.
6. Logbook: fill in the week. Stats visible here and only here.
7. Dusk: the Ward path opens. Walk up, brush the runes, check the line, feed or do not feed.
8. Sleep. Autosave. Next wake-up is next week.

### Open questions for Grant
- Is the radio ever answered?
- How long should a no-event week take in real minutes? Ironbark and Boba run 10 to 15 minutes per day.
- Which needs does each route cover (see Part 4, route pick)?
- The Ward feeding rates (see Part 4, the math).

## Part 3. Proposed design milestones

These replace the old Milestone 6 headline and sit before any more building. They are design milestones: the output of each is a document in Docs/Design and, where it applies, a tuning asset with the fields named, not a playable feature. Headlines only; tasks get written when each becomes current. Not yet in PLAN.md; Grant has not confirmed them.

Milestone 6: The warden's day and the daily loop
Settle the duty pool above, which duties are mandatory, the order of a no-event week, what each duty costs and gives, and how long a week takes. Output: Docs/Design/DailyLoop.md, and DESIGN.md's Core loop section rewritten from it.

Milestone 7: Stats, ends and endings
HP, MIND, WARD: range, weekly drain, every source of loss and gain, the feeding rates, what each stat is shown as in the logbook and what it looks like in the world at 12, 6, 3 and 0. Chapter end: which stat at 0 ends the chapter, what resets, what carries. Run end: third bottom-out. Endings: the full matrix (which stat bottomed in which chapter, neutral or bad, Ahmee, the letter, A-Spades, the walk-away true ending) as a table with the exact conditions. Output: Docs/Design/Systems.md and a GameTuning asset field list.

Milestone 8: Dialogue system
How the game talks: text on screen or diegetic (radio, notes, the logbook, the Voices), who speaks, choices or none, how a line is authored and stored, and how a conversation can change a stat or a flag. Output: Docs/Design/Dialogue.md and a decision on the authoring format before Code builds anything.

Milestone 9: The other characters
Who is at each campsite, what they want from the warden, what they know, how they change across the three chapters, and how the twist is protected. Output: Docs/Design/Characters.md, with anything that carries the twist under Docs/Private.

Milestone 10: Their events and minigames
For each character, the event track (three stages) and any minigame stage, mapped to a suit and to a location. Which locations open when. Output: Docs/Design/Events.md as the deck spec.

After these, the build milestones from the 27 September audit resume in this order: the daily loop built, sound and ambience, physical objects, events, chapters and endings, scares, minigames, menus, art pass. Sound and ambience can move earlier on Grant's call.

## Part 4. Working notes

Running record of context, reasoning and what Grant has said, so later sessions can see why things are the way they are. Newest at the bottom. "Decided" means it is in DECISIONS.md.

### 2026-09-27, session one

Too many duties. Grant asked whether the Part 2 list was too long. Cowork: yes as a fixed loop. Every comparable runs three to five actions per cycle (Ironbark: thermometer, wind, report, wood, stove). Twenty duties is a chore list and chore lists are not scary. Proposal: four mandatory beats (tower, logbook, Ward at dusk, sleep) and a pool the week draws two or three from, so the shape repeats and the content does not. The "one new wrong thing" per week goes into a familiar chore, slightly off.

Grant's response. He likes climbing the tower to look for problems every day and filing some sort of report. Ward upkeep and the feed-or-let-it-crumble choice at the end of the day are their own system with HP, MIND, WARD and will be returned to. He wanted more: Food, Water, Warmth, Safety (checking the other people).

Three routes offered for the needs. (1) Needs ledger, Papers shape: each need met or not by bedtime, unmet costs a stat point at sleep. (2) Stockpile: cabin stocks that deplete weekly. (3) Route pick: one long walk a week, the map is already a loop, each route covers different needs, events sit on routes. Cowork recommended 3 for the day and 1 for the accounting. Safety flagged as the odd one: the other three are physical, Safety is social and is where the twist lives.

Grant decided (all in DECISIONS.md, 2026-09-27): no stockpile. Food, Water, Warmth are yes or no per week, each unmet costs 1 HP at sleep. Safety (a problem found and written in the log) and Social (talked to one of the six other people, maybe through a minigame) are yes or no per week, each unmet costs 1 MIND at sleep. Events can affect any of these. A pet to feed comes later to raise difficulty. No fixed number of days: HP, MIND or WARD must always bottom out; it should feel helpless and hopeless and be about how long you survive.

The math for guaranteed loss (Cowork, not yet decided in numbers). 36 points across three stats. Base drain HP 1, WARD 1 per week from DESIGN.md, plus the need costs. Nothing the player does raises a stat from chores: meeting a need only stops a loss. Recovery comes only from finite pools: memories are a deck that is spent when dwelt on, items are scarce and do not respawn faster than the drain. Feeding the Ward converts HP or MIND to WARD and must be lossy or even. The pending owner rate of 1 HP = +2 WARD creates points and breaks hopelessness; 1 for 1 or 2 for 1 keeps the total falling. Result: total points fall by at least 2 a week, usually 3 to 5, something hits 0 between week 8 and 18, chapter ends, stats reset, drain steps up. Three chapters give the 45 to 60 wake-ups in DESIGN.md.

Escalation lever. WARD drain grows per chapter (1, 2, 3 a week) because the fire is closing in. Diegetic: closer glow, thicker ash. The player never sees a day counter; the logbook shows the week number as the score.

What the player controls. Not whether they lose. Which stat goes first and in which chapter. That is the ending matrix.

Still open after this session: Ward feeding rates; whether the radio is answered; minutes per week; which route covers which needs; Milestones 6 to 10 headlines into PLAN.md (Grant has not said yes).

### 2026-09-28, session two

Process overhaul. Grant moved the project to a team of named agents inside Claude Code: Wren (chief of staff), Sable (game designer), Vesper (creative director), Tully (process manager), Rook (coder), Marlow (playtester), Quill (writer, under Grant), Pim (UI/UX), Hollis (sound). Role files in .claude/agents. Every Code session in DieAlone opens as Wren. Cowork's role ends; this doc and Docs/Status.md move into the repo so the team can read them.
