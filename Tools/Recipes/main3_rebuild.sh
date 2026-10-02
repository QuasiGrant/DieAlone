#!/usr/bin/env bash
# Main3 runner (8.9a; 8.9e distance layers, 8.9d dressing, 8.9f look fixes and the 8.9g day-one start added before the sightlines): rebuilds Main3 from 8.1 in task order through the connected Editor and stops at the first failure.
# Usage: bash Tools/Recipes/main3_rebuild.sh   (from the project root, Editor open, not in Play mode, nothing unsaved)
# Each recipe runs as a detached Editor job (long recipes outlive the bridge's 5 s request limit) and must return its
# success text; 8.8 and 8.9 must also pass their own checks. Output: one line per step.
set -u
cd "$(dirname "$0")/../.." || exit 1
R="$(pwd)/Tools/Recipes"
unity status 2>/dev/null | grep -q "ready" || { echo "FAIL no Editor in state ready (unity status)"; exit 1; }
unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' || { echo "FAIL Editor is in Play mode or busy"; exit 1; }
run() {   # $1 = recipe file, then any number of required substrings
  local file="$1"; shift
  local id; id=$(unity command --detach eval_file --file "$R/$file" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p')
  [ -n "$id" ] || { echo "FAIL $file: could not start the job"; exit 1; }
  local out; out=$(unity job wait "$id" --timeout 1200 --json 2>/dev/null)
  local res; res=$(printf '%s' "$out" | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1)
  printf '%s' "$out" | grep -q '"state": "completed"' || { echo "FAIL $file: job did not complete"; printf '%s\n' "$out" | head -20; exit 1; }
  for need in "$@"; do
    case "$res" in *"$need"*) ;; *) echo "FAIL $file: missing \"$need\""; printf '%s\n' "$res" | sed 's/\n/\n/g'; exit 1;; esac
  done
  echo "ok   $file: $(printf '%s' "$res" | sed 's/\n/ | /g' | cut -c1-400)"
}
run main3_reset.cs "saved=True"
run main3_8_1_scene_ground.cs "saved=True"
run main3_8_2_camp_tower.cs "saved=True"
run main3_8_3_trails_giants.cs "saved=True" "owned pieces missing: none"
run main3_8_4_lake.cs "saved=True"
run main3_8_5_campsites.cs "saved=True"
run main3_8_6_front_zone.cs "saved=True"
run main3_8_7_ward.cs "saved=True"
run main3_8_8_cave.cs "saved=True" "terrain never enters the passage or chamber: YES" "only at the mouth: YES"
run main3_8_9e_layers.cs "saved=True" "missing: none"
run main3_8_9d_dress_camp.cs "saved=True" "missing: none"
run main3_8_9f_look.cs "saved=True" "missing: none"
run main3_8_15_ground.cs "saved=True" "missing: none"
run main3_8_16_forest.cs "saved=True" "missing: none"
run main3_8_17_front.cs "saved=True" "missing: none"
run main3_8_17_camp1.cs "saved=True" "missing: none"
run main3_8_17_camp2.cs "saved=True" "missing: none"
run main3_8_17_camp3.cs "saved=True" "missing: none"
run main3_8_17_cave.cs "saved=True" "missing: none" "clear True"
run main3_8_17_lake.cs "saved=True" "missing: none"
run main3_8_17_ruin.cs "saved=True" "missing: none"
run main3_8_17_stones.cs "saved=True" "missing: none" "menhirs 3"
run main3_8_20_ward_path.cs "saved=True" "missing: none"
run main3_8_19_forest_dense.cs "saved=True" "missing: none"
run main3_8_21_camp.cs "saved=True" "missing: none"
run main3_8_18_look.cs "night sky" "chain-link #"
run main3_8_18a_smoke.cs "saved=True" "SheetTop points 72"
run look_day_one_8_9g.cs "day one sun 20 bearing 205 crush 0.28 corners 0.4 fill #6E6658" "night crush 0.3 corners 0.45" "open-scene overrides: none"
run main3_8_18a_pockets.cs "saved=True" "missing: none"
run main3_8_18a_solid.cs "saved=True" "walk-ins left with no fix 0"
run main3_walk_into_check.cs "ALL PASS"
run main3_warp_seat.cs "saved=True" "no ground under: none"
run main3_8_9_sightlines.cs "all seen: True" "Ward hidden: True, cave hidden: True" "Ruin hidden: True" "ok True | next:" "all True" "ok True | cab from" "F-1 hidden: True"
run main3_e1_edges.cs "E-1 pass: True" "grazing rays ok True"
run main3_topdown.cs "wrote"
run main3_inventory_check.cs "ALL PASS"   # Vesper 2026-10-02: expected and found count per place and effect; stops on any zero
# the Play checks: the warp landing check (Grant 2026-10-01: warps that fell through the map), the tower stairs on foot and Marlow's
# breaks recheck (8.18a)
unity command editor_play >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "playing"' && break; sleep 2; done
prc=0
for check in main3_warp_landing_check.cs main3_tower_stairs_check.cs main3_breaks_recheck.cs main3_8_20_closure_check.cs main3_8_14_climb_check.cs; do
  wid=$(unity command --detach eval_file --file "$R/$check" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p')
  wres=$(unity job wait "$wid" --timeout 1200 --json 2>/dev/null | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1)
  case "$wres" in *"ALL PASS"*) echo "ok   $check: $(printf '%s' "$wres" | cut -c1-160)";; *) echo "FAIL $check"; printf '%s\n' "$wres" | sed 's/\\n/\n/g'; prc=1;; esac
done
unity command editor_stop >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' && break; sleep 2; done
[ $prc -eq 0 ] || exit 1
echo "Main3 rebuilt. Commit Main3.unity.meta with ProjectSettings/EditorBuildSettings.asset (the scene GUID changes on every rebuild)."
