# DieAlone

Rewritten 2026-09-29 by Sable for PLAN.md task 6.6, pending Marlow's check. Where this file and DECISIONS.md disagree, DECISIONS.md wins. Detail lives in Docs/Design: the loop in DailyLoop.md, events in Events.md, the map in Main3.md.

First-person psychological horror with a VHS tape look. Video game adaptation of *Cinderedge* (Grant Mielke, QuasiReal Publishing, 2025), a solo journaling game. Unity 6000.3.24f1, URP.

## Concept
You are a Wardkeeper. You volunteered to have your memory wiped and be posted to a firewatch tower on the edge of the Doshairiel forest, ancient trees larger than anything in our world. You are not alone: six people live around the posting, one at each location. On the first day it is an ordinary lookout job. On the first night you find the real one: a rune-covered stone Ward on a cliff edge holds back a vast wildfire, and it feeds on you. There is no winning.

## Pillars
1. Psychological horror first. Inscryption, Mouthwashing, Fears to Fathom. Dread, wrongness, unreliable reality. A real monster appears now and then as punctuation (a chase), never as the focus.
2. The Ward feeds on you. HP, MIND and WARD are one economy; every night it takes, and it gets hungrier.
3. Routine that gets broken. A daily loop the player learns on a very normal first day, then events warp it.
4. Replayable. Events drawn each day, six residents, multiple endings.

## Setting
1. A fixed, hand-built area (Fears to Fathom scale, not open world), about 400 x 300 m in the current Main3 draft (Docs/Design/Main3.md).
2. The keeper's camp with the tower, raised above giant trees. Six locations, each with one resident: the lake, three campsites, the cultist cave and the office. The front zone inside the gate holds the office, parking lot and a small store; the road stays outside the gate.
3. The tower sees every location except the Ward and the cultist cave. The cave is not part of the daily check.
4. The fire lies west, beyond a cliff. It is not visible at all on day 1. From day 2 it may show by day as far glow and smoke, huge in the distance.
5. Fantasy world of Ancerra. Two eras, Early and Modern, as variants of the same area; the Modern era is built first.

## Time and the loop
1. One wake-up is one day. There is no time budget for the day and no fixed run length.
2. The loop is written in one place: Docs/Design/DailyLoop.md, with its drawing DailyLoop_flow.svg. This file does not repeat it.
3. Two lighting states: day is a burning sunset with the sun fixed, from waking until the report is filed; night is dark except for the fire glow.
4. The player wakes in the camp cabin.

## Stats and needs
1. HP, MIND and WARD, never above 12. A run ends the first time any of them reaches 0. WARD at 0 means the barrier falls and the world burns.
2. Needs: Food, Water, Warmth, Safety and Social, each met or missed per day. Details and costs: DailyLoop.md.
3. No stockpile. Nothing in the cabin is counted or depleted.
4. The Ward's hunger rises each week, so every run ends. Feeding may raise WARD. Give nothing is a valid choice.
5. Stats show on screen only during the night Ward screen. The rest of the time they live in the logbook, which the player carries. The logbook also holds questions, notes and settings, and later inventory, memories and collectibles if those are added.
6. As WARD gets low, the fire gets louder and the world gets weird.

## Events
1. Events are drawn each day from day 2 and change the loop: flag a location, block or add a need, cost HP, MIND or WARD by what they are, chase, end the day, open a place, or cause trouble on the climb to the Ward (the night still ends at the Ward screen). Spec: Docs/Design/Events.md.
2. Tracks run in stages; the last stage is the weirdest. Suits from the book carry over as flavour: Hearts survive and forage, Diamonds the Ward, Clubs memories, Spades the mind.
3. Minigames: spotting anomalies as a skill lives only in minigames. There will be minigames that progress each of the six residents' dialogue and affect the endings. Some spoof PSX and horror genres. Not designed yet.
4. Voices (Survivor, Devotee, Stranger, Child, Skeptic, You): kept from the book; when they start is open until chapters are redesigned.

## Chapters and endings
1. The book's three strikes are gone: a run ends at the first bottom-out. What chapters mean now is redesigned in Milestone 7.
2. There are no good endings. Ending conditions stay open until the characters exist (Milestone 12). Losing is shown, not told.

## Presentation
VHS tape look first (Fears to Fathom): soft low-resolution picture, colour bleed, grain, scan lines. PS1 effects (vertex jitter, affine textures) stay off unless added later. Low-poly assets. First person. Controller and keyboard and mouse.

## Later
Main menu, settings, the physical objects (scrapbook, inventory, map, memories), Ahmee, a pet the player must also feed, scares, the art pass. Order is in PLAN.md.

## Out of scope
No combat. No co-op or multiplayer. No voice acting. No big open map. No mobile.

## Saving
Autosave when the player goes to sleep, before the next day's events are drawn. No save anywhere. Sleep is the save point so dream sequences can run between sleep and waking.
