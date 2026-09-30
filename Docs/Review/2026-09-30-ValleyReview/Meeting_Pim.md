# Meeting input: Pim (UI/UX), 2026-09-30

## 1. Why the day/night toggle is unfindable
- It exists (8.9i): the LOOK section, rows "Night", "Day one", "Day two". It is the last section, under SCENES and about 24 warps (main3_8_1_scene_ground.cs 410-429 plus Tower_Deck and Cabin). At the dev panel's 720 reference, the list is about 1100 px tall; the viewport is about 684. LOOK starts off the bottom edge.
- Nothing shows there is more below: no scrollbar, no "more" mark. Mouse wheel or about 30 presses of down reach it. The header says LOOK, not DAY / NIGHT.
- "Ward" is a warp row near the bottom of WARPS, also below the fold, next to "Ward P3" and "Ward P4". There is no separate Ward scene; the Ward is on the ledge in Main3.
- How I missed it: my 8.9h check asked "is the panel on screen and readable at each size", not "can Grant do what he opens it for". I treated "the list scrolls" as a pass. 8.9i's done-check was two day/night screenshots; nobody opened the panel at Grant's size (3840 x 1976) and timed finding the toggle.

## 2. Dev panel layout I would want (paper only)
- Order top to bottom: title and one hint line; DAY / NIGHT; WARPS; SCENES last (rarely used).
- DAY / NIGHT: one row, "Time: Day one  < >". Left/right (d-pad, stick, arrow keys) steps Day one, Day two, Night. Also a dev key to cycle it without opening the panel: F2 on keyboard. F2 was folded into F1 in 7.9, so this reverses that; needs a DECISIONS line. Gamepad has no spare free button outside the panel; pad uses the row.
- WARPS grouped under small subheads, in route order: TOWER AND CAMP (Keeper's camp, Cabin, Tower deck); WARD (J junction, Ward climb low, Ward climb high, "Ward: stones on the ledge"); LAKE; CAMPS; FRONT (Office, Store, Gate booth, Trailhead, Closed campground, Old burn); CAVE. Labels come from a name-to-label table like SceneLabel, not raw names ("Junction Jg" means nothing to Grant).
- Rule: DAY / NIGHT and the Ward warp are on screen when the panel opens, at every 8.9h size, no scrolling. Anything below the fold shows a "more below" mark.
- Section jump: LB / RB on pad, Page Up / Page Down on keyboard, move focus to the next or previous section head.

## 3. Wayfinding rules for the map
- W1 Path reads as path: every trail differs from grass in value, not only hue, under the VHS filter in day one, day two and night. Check: a grayscale screenshot at eye height on each trail; the trail edge must be visible 20 m ahead. Worn dirt, edges broken by rock, root, log (Firewatch: bare dirt, clear edges).
- W2 No invisible walls: every collider that stops the player has a visible cause at eye height (rock, scree, deadfall, fence, steep face). The only exception is DECISIONS 2026-09-29 ("You can't abandon your post"), and that one speaks. Edges.md 9 already asks for this; Grant says it is not met.
- W3 No path fades out: every trail ends at a place or a visible stop (gate, chain, deadfall, sign). Lake Pump fails this per Grant.
- W4 Junctions have a marker (sign, cairn, blaze); the Ward branch keeps its rule (no sign, cairn only, closed by day).

## 4. How I check UI from now on
- Task check, not fit check: at Grant's Game view size, from open, do the job Grant asked for (switch to night; warp to the Ward) with pad only and with keyboard only. Count presses; fail over 5.
- I open the panel myself from the running Editor, not only from the coder's screenshots.
- For map readability I review W1 to W4 screenshots at eye height before any walk is handed to Grant.

Pim
