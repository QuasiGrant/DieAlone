# Die Alone in a Wildfire Simulator

PS1-style first-person psychological horror. Video game adaptation of *Cinderedge* (Grant Mielke, QuasiReal Publishing, 2025), a solo journaling game. Unity 6, URP.

## Concept
You are a Wardkeeper. You volunteered to have your memory wiped and be stationed alone at a firewatch tower on the edge of the burning Doshairiel forest, ancient elven trees larger than anything in our world, a fire that could last decades. A rune-covered stone Ward near the tower holds the fire back. It feeds on you. Maintain the Ward, survive, scavenge, remember who you were, and try to understand why you asked to forget. There is no winning. There is a way out.

## Pillars
1. Psychological horror first. Inscryption, Mouthwashing, Fears to Fathom. Dread, wrongness, unreliable reality. An actual monster appears occasionally as punctuation, never as the focus.
2. The Ward feeds on you. Every recovery costs something. HP, MIND, memories, inventory, the Ward: all one economy.
3. Routine that gets broken. A fixed daily loop the player learns, then events warp it.
4. Replayable. Random event deck, chained stages, multiple endings, 4-6 hours per full run.

## Setting
Firewatch tower and a fixed, hand-built surrounding area (Fears to Fathom scale, not open world). Nearby: the Ward, forage spots, a creek, forest edge. Fire visible on the horizon, rust-orange haze, falling ash. Some locations only exist once an event track unlocks them (meadow, black lake, cave, grave, shrine, pipes, cliffs). Fantasy world of Ancerra, mid-17th century tech.

## Time
One wake-up = one week. A run is roughly a year (45-60 wake-ups). Each wake-up draws one event from the deck; that event's current stage plays out during the day.

## Core loop (one day)
1. Wake up in the tower. The tower and surrounding area contain physical clues to which event is active today (objects moved, sounds, marks, weather, what is on the table).
2. Forage and do chores. A small variety of chores (wood, water, repairs, traps).
3. Check the fire from the tower.
4. Send in a report: write in the log/journal.
5. Maintain the Ward: feed it HP, MIND, an item, a memory, or nothing.
6. Sleep.
Events interrupt this loop in different ways: block a step, replace a step, add a location, hijack the night, or drop the player into a minigame.

## Events
- 15-20 tracks, 3 stages each (45-60 stages), adapted from the Cinderedge deck. A track's stages fire in order across the run; stage 3 is where things get weirdest.
- Suits carry over as flavor: Hearts = survive/explore/forage, Diamonds = the Ward, Clubs = memories of who you were, Spades = your mind.
- Some stages are self-contained minigames spoofing PSX and horror genres: spot the difference, hiding, survival-horror inventory, fixed-camera, point-and-click, etc. A best-of PSX anthology inside the loop.
- Voices mechanic from Chapter 2 onward (Survivor, Devotee, Stranger, Child, Skeptic, You).

## Stats and resources
- HP, MIND, WARD, each 0-12, never above 12. Weekly drain: -1 HP, -1 WARD.
- Recovery: consume an inventory item for HP, dwell on a memory for MIND (the Ward eats part of it), sacrifice HP or MIND to restore WARD.
- Inventory, memories, scrapbook, map, and Ward runes are real in-game objects the player can pick up, look at, lose, and mark off. Ahmee is in.
- A stat hitting 0 ends the chapter; stats reset, next chapter begins (Wake Up, Voices, Burn). Third bottom-out ends the run.

## Scares
Mix, weighted toward the psychological: the Ward's hunger (meat smell, cocoon, runes on your skin, cracks that scream), voices and hallucinations, things in the forest (the cave thing, the presence at the grave, the shrine, the tracks), the fire closing in, memories going wrong in the scrapbook. Occasional real monster the player can see and must hide from.

## Endings
Mirrors the book. Ending is chosen by which stats bottomed out across the three chapters (Mind, Ward, Health, Everything), each with a neutral and bad variant, plus Ahmee / letter / A-Spades conditions. There are no good endings. The true ending is the "Everything, neutral" ending: you walk away from the fire. You can never go home, but you can't stay here. It still feels like losing, but not really. Losing is shown, not told (the Ward eats you, the fire takes the tower). New run reframed as the memory wipe.

## Presentation
PS1/PSX: low-poly, vertex jitter, affine textures, low-res render, dithering, limited palette. First person. Controller and keyboard/mouse.

## Scope, first playable
- Tower + immediate area, daily loop complete end to end.
- 15-20 event tracks, 3 stages each. A few are minigames.
- Three chapters, voices, all endings.
- Scrapbook, inventory, map, Ward as physical objects.
- Ahmee.
- Controller support.

## Out of scope
No combat. No co-op or multiplayer. No voice acting. No big open map. No mobile.

## Saving
Autosave when the player goes to sleep, before the next event is drawn. No save anywhere. Sleep is the save point so dream sequences can run between sleep and wake-up.
