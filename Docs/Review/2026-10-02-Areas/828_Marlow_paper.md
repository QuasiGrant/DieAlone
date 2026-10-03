# 8.28 paper check, round 1: BurnLayout.md and BurnLayout.svg (Sable, draft 1)
2026-10-02, Marlow. **FAIL** (1 block, 7 hurts, 9 cosmetic). No Editor calls (Rook has it).

**Read from files at 85e0134:**
- Main3.unity: world transforms and every scene collider; prefab colliders were read from the prefabs.
- Main3_TerrainData.asset, decoded (check (392, 165) = 3.000).
- FBX vertices for Sequoia3 (the Hollow Giant), Tree_Dead (the Gate Tree stub and every burn snag) and every CS_Stone near the trails.

**Method:**
- Body: radius 0.35, climb 0.70, slope 45, reach 2.
- Deck eyes: 128, at 57.6 and 58.2 m, over x 160.5 to 167.5, z 162.5 to 169.5.
- Crowns are cones from FBX bounds, which is unverified.
- Snag and stub shapes are taken from their real vertices.
- "m" means metres along the scene markers, as in 828_Marlow_ground.md.

**Correction to my 828 survey, table 2.** The pocket at (188.2, 137.1) is not a pocket. My survey flood only counted ground within 8 m of the four burn trails. The cells walk out south, 4 m, to Camp to pump P60 (186.69, 7.46, 133.44), and Camp to pump was outside that mask. I reran the flood over x 150 to 245, z 100 to 182, with every trail in the region as an exit and 0.35 m body clearance on 0.25 m cells. It finds no pocket there, with or without B9, and the way out has 3.40 m of collider clearance. The flood found two other small sets of trapped cells, on the region's edges at x 150 and z 100 (4.4 m² and 0.7 m²), where cells outside the region were not counted. Sable's B9 and section 5 T3 rest on my error.

## Block
1. **The Hollow Giant opening (B2) is drawn on the invisible collider, 3 m in front of the tree.**
   - **The two surfaces.** The trunk collider is a Unity cylinder of r 4.5. The visible tree is SliceLook/GiantTrees/[Sequoia3] at (202, 6.0, 140), scale 1.17. Its bark radius is 1.1 to 3.2 at y 6 to 8 and 0.9 to 3.5 at y 10 to 12; toward bearing 330 it is 1.17 to 1.52 at sill height.
   - **Where the opening lands.** B2 puts the opening "on the trunk surface" at r 4.5, (199.75, 143.90). That point is 2.3 to 3.3 m of air in front of the bark.
   - **Where the bark actually is.** The bark toward 330 is at about (201.25, 141.30). The ground there is 6.00, the floor of the drop. The tread at the nearest point, (198.19, 146.59), is at 10.91.
   - **What a sill at tread height means.** It is 4.9 m up the visible trunk and 6.1 m from the centreline.
   - **The landing grows.** It is about 5.3 m long, not 2.5, over ground that falls from 10.90 to 6.00, so its skirts would be up to 4.9 m tall.
   - **No room for the interior.** A room "2.5 m across at most", closed but for the opening, will not fit inside a visible trunk about 3 m across at that height.
   - **The opening cannot be cut as things stand.** The collider is a primitive cylinder mesh, which cannot be cut, and the doc does not say what replaces it. It also does not say how the opening is made in the Sequoia3 mesh.
   - **Step.** A sill at 11.2 (B2) would be 0.29 over the tread (10.91). The step limit is 0.1, so the landing would have to slope (STAIRS RULE).
   - **Roof.** The bark roof hides the opening only if it is about 3.2 m wide. Lines from the opening to the deck (bearing 301.7, 47.1 degrees down) reach roof height within 0.28 to 2.33 m of the opening, 28 degrees off the roof's axis. B2 gives the roof's depth but not its width.
   - **Repro:** draw Sequoia3's vertex hull at y 10 to 12 over panel A beside the r 4.5 circle.

## Hurts
2. **B9 fills a pocket that does not exist, and it adds a 3.5 m box beside Camp to pump.**
   - The fill (x 185 to 191.5, z 136 to 138.5, top 11.0) stands 2.6 m from the Camp to pump centreline at P60. The trail is at 7.46 there, so the fill's face rises 3.5 m beside it. That ground belongs to 8.21b.
   - Its height measures fine: CampStep's top is 13.80 (2.8 over the fill) and the hedge tops are 16.2 to 17.0. But the reason for it is my survey error (see the correction above).
3. **The verge frame keeps about 20 burn snags; B8 removes the wrong one.**
   - B8's rule is no snag within 15 m in plan of the deck-to-verge line from x 250 to 345. The one it removes, Forest/BurnDeadwood/Tree_Dead (315.77, 134.50), is 15.35 m from that line, outside the rule.
   - About 20 snags are inside it. The nearest few, with their tops:

     | Snag | Distance from the line | Top |
     |---|---|---|
     | BurnDeadwood (305.61, 150.59) | 0.41 m | 16.16 |
     | BurnDeadwood (257.81, 157.82) | 1.74 m | 9.39 |
     | BurnDeadwood (264.37, 157.91) | 2.52 m | 9.38 |
     | BurnRegrowth (303.45, 148.45) | 2.77 m | 13.04 |
     | BurnRegrowth (278.18, 157.48) | 3.55 m | 19.28 |
     | BurnRegrowth (255.48, 160.08) | 3.75 m | 18.45 |
     | BurnDeadwood (271.66, 158.76) | 4.14 m | 19.92 |
     | BurnDeadwood (292.23, 157.12) | 4.67 m | 9.90 |

   - The rest are 4.6 to 13.5 m off and listed in the run log.
   - They stand 9 to 21 m tall under a line that is 31 to 47 m above the ground there. So none of them blocks the verge tree, but they sit in its frame (Quill 22).
4. **The office line's height caps are wrong, and five snags reach the line.**
   - **The line itself** (lowest of the 128 eyes): 37.4 m at x 230 (30.6 m above the ground), 20.0 m at x 290 (14.6 m above) and 8.4 m at x 330 (4.1 m above).
   - **The caps should be** the line minus 2 m: 35.4, 18.0 and 6.4. B8 says about 33, 21 and 12. The caps at x 290 and x 330 are above the line, so they would allow trees that block it.
   - **Snags whose real vertices come within 3.5 m of the central eye's line:**

     | Snag | Top | Line height there | Closest vertex to the line |
     |---|---|---|---|
     | SliceLook/BurnRegrowth/Tree_Dead (313.35, 191.96) | 13.30 | 13.4 | 0.38 m |
     | Forest/BurnDeadwood/Tree_Dead (308.17, 191.32) | 17.45 | 14.9 | 0.77 m |
     | BurnRegrowth Tree_Dead (300.77, 191.62) | 15.05 | 17.1 | 1.95 m |
     | BurnDeadwood (294.31, 190.29) | 15.10 | 19.0 | 3.17 m |
     | BurnDeadwood (287.66, 189.99) | 17.64 | 21.0 | 3.50 m |

     The first two touch or nearly touch the line. A correct cap would remove all five. My collider-and-cone rays (31 of 128 eyes clear, the rest lost to the tower's own rails) do not model snag branches.
5. **Forage A's stand misses with the ray as given.**
   - From (241.6, 160.6) facing 330:
     - bush (240.98, 162.83) is at bearing 344.5, 2.06 m from the eye to its surface;
     - bush (239.82, 161.66) is at bearing 300.8, 1.85 m to its surface.
   - At yaw 330 the ray passes 0.58 m and 1.01 m from those sphere centres (r 0.55), so it hits neither within reach.
   - Facing about 301, pitched about 36 degrees down, reaches the second bush at 1.85 m.
   - Section 6.2's surface distances of 1.35 and 1.56 do not match these: in plan I get 1.52 and 1.76.
6. **Forage B's crop shows from the deck.** This is hard must-hide, and section 6.4 says B is checked here.
   - Four of B's five bush tops are clear to 17 to 27 of 128 eyes, on terrain, colliders and crown cones: (142.12, 166.85) 17, (141.42, 168.34) 18, (139.48, 166.52) 25 and (140.92, 165.73) 27.
   - Forage C is hidden: 0 of 128 with crown cones, and 5 to 7 of 128 on terrain and colliders alone.
7. **Forage A's screen is given as a point, but the lines it must stop are spread out.**
   - Today the tops of three A bushes are clear to 27 to 33 of 128 eyes: (240.22, 164.29), (239.82, 161.66) and (240.98, 162.83). The other two are hidden by Hedge_Burn_3's collider.
   - At x 236, the deck lines to the five bush tops cross z 161.7 to 164.6, between 7.4 and 9.4 m high: 2.4 to 4.4 m over the ground (5.00).
   - So the screen must be foliage across z 161.7 to 164.6, from 2.4 to 4.4 m up. Three firs "at (236.0, 163.0)" do not say that.

## Cosmetic
8. **Found frames at 30 m fail, but the found rule passes.** This was measured on terrain, colliders and crown cones, at 45 degrees and 1 degree:
   - **Forage A:** first found at 17 m from the camp side, (233.4, 148.3), 27 degrees off. From the Jg side it is first found at 14 m, (247.8, 174.3), 37 degrees off.
     - At 30 and 25 m from camp the trail heads 65 to 68 degrees away from the patch, and Hedge_Burn_3 is in the line.
     - From Jg there is no 30 m frame (Jg is 28.7 m away), and at 15 m the patch is 63 degrees off.
   - **Forage B:** found at 12 m and 11 m.
   - **Gate Tree:** found at 24 m from Jg, (267.0, 180.7), 38 degrees off. From T it is found at 17 m, but only on its upper trunk (y 12 and 18); at y 6, Hedge_Burn_7 hides it all the way.
   - **What to change:** Pim's "30 and 15 m" frames should be set from these points.
9. **Times, from the camp signpost (165, 158), which is 19.9 m from Camp to Jg P16 (doc 16):**

   | Walk | Measured | Doc |
   |---|---|---|
   | Camp centre to Jg | 126.4 m, 51 s | 122.5 m |
   | Camp centre to forage A | 97.7 m, 39 s | 93 m |
   | Forage A to forage B through camp | 129.9 m, 52 s | 133 m |
   | Camp to the lot centre | 249.0 m, 100 s | 242 m |

   - Camp to forage B is 13.9 + 18.3 m (forage B is at m 18.3 of Camp to Camp 3).
   - Camp to the lot is 126.4 + 104.5 + 18.1 m.
   - Forage A to Jg is 28.7 m (doc 29).
10. **Look-back straight.** m 62 to 88 has a 25.2 m chord with a 2.59 m wander (doc 26 m and 1.3). The longest stretch within 1.5 m is 28 m from m 13; within 1.0 m it is 20 m from m 39.
11. **Tread flatness at ±2 m.** The doc says within 0.2 m. Measured: Camp to Jg 0.22 (m 23), Jg to T 0.28 (m 95), Camp 2 to T 0.35 (m 92), Jg to Camp 1 0.04.
12. **Junction bearings.**
    - At Jg, the legs leave at T 35.2, camp 262.5 and Camp 1 351.6 (doc 37 and 265).
    - At T, the Jg mouth leaves at 260.4 and the Camp 2 mouth at 184.4. They are 76 degrees apart, not Pim's 228 and 201. B4's arms should follow those bearings.
13. **Forage A's colliders** are spheres, r 0.55, centred at y 5.41. Their tops are 0.92 to 0.96 over the ground, not 0.77 to 0.81 (that is the mesh). At 0.92 to 0.96 they are over the 0.70 climb, so they cannot be climbed.
14. **Jg post** (263.8, 170.2): Hedge_Burn_7's collider centred at (266.83, 170.50) is 2.44 m from the post. Its centre is 5.06 m from the junction. Check it against B3's "no brush within r 4 on the post's side".
15. **CS_Stone_3 (261.40, 173.30)** is 0.15 m tall. Its edge is 0.23 m from the Jg to Camp 1 centreline (its centre 0.40). B7 moving it to 1.4 m holds.
16. **Gate Tree stub visual** (Tree_Dead at scale 13.33, 3.45, 13.33): the bark is 1.08 m from the Jg to T centreline at y 4 to 6 and 1.95 m at y 6 to 9. The collider (r 4.0) stands up to 3.25 m outside the visible trunk at y 6 to 9. The branches above y 9 hang over the tread, 5 to 15 m up.

## Passes
- **B7 stones:**
  - All 11 convex-hull stones are 0.12 to 0.43 m tall. Each one's edge is 0.90 to 1.38 m from its centreline.
  - Every other trail-edge stone within 3 m of a trail is 0.09 to 0.37 m tall and has no collider; Campsite stone prefabs carry none.
  - Removing the colliders leaves nothing over 0.5 m.
  - It is unverified which recipe gave those 11 their hulls. Walk-in checks skip anything under 0.5 m, so 8.18a should not add them back.
- **B3 Jg post:**
  - It is 2.71 m from the Camp to Jg centreline, 2.55 m from Jg to T and 2.55 m from Jg to Camp 1.
  - It sits 2.55 m from the junction at bearing 135, 2.84 m from the Junction_Jg warp, on ground 5.00.
  - The nearest regrowth is RedFir5 (266.60, 169.31), 2.91 m from the post.
- **B5 Gate Tree:**
  - Its collider is 1.51 m off the centreline at m 34.2, and its visible base 1.08 m off.
  - No other trunk of r 2 or more is within 20 m of Jg to T there. The nearest giant is Forest/Grove_C1Ring/Sequoia2 (296.5, 205.8), about 25 m away.
- **Hollow Giant deck angle:** the opening at (199.75, 143.9) is 42.0 m from the deck centre and 47.1 degrees down (doc 48). Today 17 or 18 of 128 eyes see that point; the rest are lost to the tower.
- **B1 removal:** SliceLook/ShotGround/CS_Log_Firewood (240.21, 161.01) is 1.94 m from the centreline, between the trail and bush (239.82, 161.66), as Sable says.
- **Deck sees the trails:** terrain alone, 128 of 128 at every 10th marker (828 table 5).
- **Office and verge lines:** no collider in the burn blocks either one, with or without crown cones. Every lost eye is lost to Camp/Tower parts (Flight10 RailStop, Cab E_Sill, DeckRails RailE, W_Head, Lectern).
- **Warps:** Junction_Jg is built at facing 88.6 (Pim proposes 40). Old_Burn (239.6, 157.8) is built at facing 331. It is 5.24 m from A's centre at bearing 2.5, so facing 5 puts the patch dead ahead.
- **Not checked:** the B6 firebreak ground (x 205 to 211, north of the trail to z 200); Quill 2's 70 m and 30 m widths; Hollis's sound items, beyond the Gate Tree's face distance.

Marlow
