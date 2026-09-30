# Lighting options to see (PLAN 8.12; Vesper, 2026-09-30)
Source: Check3_Quality.md 1, Style.md 3 and 6.0. All values **P**. Each look is a copy of LookTuning_DayOne.asset ("D1 now"), listed on LookPreview with daylight on, labelled as below. Rook builds after 8.11.
## 1. Single changes (each from D1 now, one thing only, not cumulative; in test order)
| Label | Change | Now |
|---|---|---|
| V1 Ambient | sunsetAmbient #6E6658 | #998A73 |
| V2 Fog colour | sunsetFogColor #9A9A94 | #A8A08E |
| V3 Fog range | sunsetFogStart/End 110 / 850 | 40 / 600 |
| V4 Crush | crushBlacks 0.28 | 0.15 |
| V5 Wash | washOut 0.18 | 0.25 |
| V6 Sun height | sunElevation 24 | 32 |
| V7 Sun bearing | sunBearing 205 | 200 |
| V8 Sun strength | sunIntensity 1.25 | 1.1 |
| V9 Corners | darkCorners 0.4 | 0.3 |
| V10 Glow | sunGlowSize 80, sunGlowColor #C8A070 | 24, #FFC98A |
## 2. Full candidates (what Grant chooses between)
| Label | Sun elev / bearing / int / colour | Ambient | Fog colour, range | crush / wash / corners | Sky horizon; glow |
|---|---|---|---|---|---|
| A Vesper | 24 / 205 / 1.25 / #FFC98A | #6E6658 | #9A9A94, 110-850 | 0.28 / 0.18 / 0.4 | #E3A968; 80, #C8A070 |
| B Late gold | 16 / 212 / 1.35 / #FFB878 | #66584A | #A89A86, 90-750 | 0.30 / 0.16 / 0.4 | #E39A58; 60, #D09060 |
| C Overcast | 30 / 205 / 0.7 / #E8DCC8 | #6A6C6C | #8E9296, 70-600 | 0.32 / 0.22 / 0.4 | #A8A8A4; 200, #A8A49C |

A is my pick going in; I may fold V results into A before Grant sees the sheet. B must not read as day two (no rust sky, no fire, elevation over 14); its lower sun widens the west-ridge shade, so Rook confirms top-down that camp, J and W1 stay lit.
## 3. Sheet: six fixed spots, same camera, height and heading in every look
Camp (pair-sheet camp frame); S1 (pair-sheet S1); under the giants (Jg_to_Camp_1 FWD 40); the office (pair-sheet office); J (pair-sheet J); cabin interior (doorway, looking in to desk and stove). Filter on, 1920 x 988, no dev panel. Sheet 1 for me: D1 now plus V1 to V10 (six rows, eleven columns). Sheet 2 for Grant: D1 now, A, B, C, large (six rows, four columns). Spots saved in the capture recipe so retakes after 8.14 to 8.16 match.
## 4. Absolute bar (every frame of every candidate; not "better than now")
1. Value order: horizon sky brightest, haze mid, foreground darkest (Style 6.0.3).
2. Dark anchor: every outdoor frame has a near-black area (under the giants, cabin shade side) that still shows shape; in a greyscale thumbnail lit ground and shade separate at a glance.
3. Warm key, cool shade: sunlit faces warm, shade cooler; a rim on crests where the sun rakes.
4. Distance: at least two ridge layers at camp and J, each paler and cooler than the floor; nothing past 50 m melts into the ground tan.
5. Sun: shadows at least twice object height, falling east; no white smear; gold horizon band where sky shows.
6. Office: lot lights and windows read as modest warm practicals. Cabin interior: darker than outside, window brightest, lamp and stove pools, corners dark, desk readable.
7. Tape and day: filter on and off differ at a glance (Style 3.2); no clipped white but the sun; no fire, smoke, ash or rust sky (Style 6.0.6).
## 5. LookTuning fields Rook verifies (may not exist)
- Trilight ambient: missing; daylight sets flat sunsetAmbient (LookEnvironment.cs 71). Needs sky / equator / ground (#6A7080 / #6E6658 / #3A3228) and a mode switch; if added, add "A Tri" beside A.
- Fog range: sunsetFogEnd is capped at 600 by its Range; V3, A and B need 850 and 750.
- Glow strength: no field, only colour and size; V10 dims by colour. Rook confirms the smear is this glow, not bloom.
- Exposure: no field and no post Volume. Not requested; if no look meets bar 5 without it, ask me first.
- skyHorizon is already #E3A968 in D1 yet no gold band shows; Rook finds why before bar 5 is judged.
