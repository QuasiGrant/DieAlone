# Main2 team review, 2026-09-28

Grant: "overall just not good... visuals lacking, structures overly simple and boring, layout isnt great, empty and dull, game design hasnt been considered... needs a total rework." All eight agents reviewed the shots in this folder. Shot numbers refer to the PNGs here. Nothing below is decided until it is in DECISIONS.md.

## Verdicts (unanimous: rework)
- Sable: scenic dead ends around one clearing; no place is built around the loop.
- Vesper: a blockout lit in one orange-brown tone; no value structure, no hero landmarks, no lived-in detail.
- Marlow: empty dirt plates joined by identical tree tunnels.
- Quill: set dressing, not a story; nothing says who lives here or what the job is.
- Pim: nothing tells the player where to go or what to use.
- Hollis: silence explains the long walks, not the emptiness.
- Rook: pipeline works, content thin; keep terrain, look and systems code.
- Tully: built one request at a time, nobody signed off the whole.

## Findings by role
- Game design (Sable): every mandatory beat sits in one clearing (02, 03); Food, Water, Warmth have no place. Tower sees only trees (04). Paths are one corridor (05, 18c to 18g). Walks eat about half a 10 to 15 minute week. 12 destinations, most with no job; campsites alike (13 to 15).
- Look (Vesper): one flat value, filter on and off nearly identical (02 vs 20), tape character lost. Hero pieces are primitives: Ward boxes with neon runes read sci-fi (08, 09), ridge is flat cones, cave is a floor and a cube (17). Buildings are boxes with stretched textures, blue roofs off palette. Ordinary pines on a grid, not the giant trees of DESIGN.md. Bare dirt fills every frame.
- Player (Marlow): path A 1 min 52 s with nothing readable (18a, 18b); fallen trunk floats and reads as a block (06); cave unreadable (16, 17); lake reads as mud (12); trees in rows (05). Nothing moves but fire and smoke. No location offers a need action.
- Story (Quill): campsites have no owners (13 to 15); cabin and office have no paper trail (01, 11); board and car unused (18d, 18e); Ward and cave show no human trace (09, 17).
- Wayfinding (Pim): tower view useless (04); board map unreadable (18e); fork sign a speck, cave spur invisible (18c, 18g); no lit logbook desk or radio (01, 11); the Ward shows straight down path A (07), against 2026-09-25.
- Sound (Hollis): path A was kept long on the promise that ambience would carry it; zones would sound alike; no terrain edges for sound to change at.
- Tech (Rook): 9 of 755 Celestia prefabs used, 67 of 423 Campsite, 39 of 159 Cabin; 28k trees and 340k detail instances take the budget; Main2 is Main plus patch scripts, cannot be rebuilt in one pass. Crash likely a GPU timeout on the oversized top-down render (unverified); no risk in normal play.
- Process (Tully): no design intent, 41 commits in one day of one-line requests, no art direction, 5.10 grew into a new scene.

## Keep
Horizon glow and smoke (04), ledge reveal (07, 08), Ward on the cliff hidden from the tower, fire pit cluster (03), lit tent site (13), sunset road (18d), sky ramp, water shader, VHS filter code, notice board as the in-world map, terrain rolls, the crouch trunk as a future event.

## Direction proposed
- Sable: about 200 x 200 m, seven places each with one job (camp: logbook, tower, Warmth; lake: Water; forage ground: Food; three lived-in sites for Social and Safety; Ward ledge). A ring of routes where you cannot meet every need in one week. Tower sees the ring. Cave opens by event.
- Vesper: camp warm and lived in; trails of giant trunks with fire glimpses; carved Ward, dim runes, ash; campsites abandoned mid-routine with one wrong detail; no bare ground within 10 m of camera, 15+ props per clearing; sunset, dusk, night states.
- Quill: each site one person, one reason to stay, one weekly wrongness; a previous keeper's traces; outside world giving up.
- Pim: one landmark per destination seen from the tower, warm light marks points of interest, trail tiers, map board and lit logbook desk in the cabin.
- Hollis: one sound landmark per zone, hard edges between zones, silence at the Ward.
- Marlow's pass checks: no more than 30 s without an identifiable point of interest at 20 m; landmarks nameable cold; no two structures share silhouette and material; next destination visible at every junction; every location has a loop action.
- Tully's gates: space brief (Grant signs), Style.md with reference boards (Vesper, Grant looks), gray blockout (Marlow walk, Tully check, Grant walks), dressing one location per task (Vesper), look pass (Grant). One ordered recipe per location, no patch chains.
- Rook: fresh Main3 from the scene recipe; kitbash the owned packs first; Blender not recommended; new packs need Grant's yes each.

## Timing (unanimous)
Design the map on paper inside Milestone 7, build after it. Tully, Rook, Sable, Vesper: building first risks a third scene.
