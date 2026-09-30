#!/usr/bin/env bash
# Main3 review capture: contact sheets of Main3 as a player sees it (trails, climb, warps, trail ends, invisible stops, compass views,
# hand-walk views, grayscale trails with grey means, day one and night pairs, top-down map, InvisibleColliders.md, 8.16a: climb both ways
# with rock and sky shares, the night-rule frames) for Marlow, Pim and
# Vesper, then the scripted Play checks (main3_8_14_climb_check.cs, main3_hand_walk_check.cs and, 8.16a, main3_reach_check_8_16a.cs and main3_trunk_check_8_16a.cs) into Checks.md. Rerun before every
# Grant walk.
# Usage: bash Tools/Recipes/main3_review_capture.sh [outDir]   (from the project root, Editor open on Main3, NOT in Play mode)
# Default outDir: Docs/Captures/Main3Review (git-ignored; only verdicts are committed). Enters Play mode, runs main3_review_capture.cs step "day" then
# step "night" and the two checks as detached Editor jobs, leaves Play mode, sets runInBackground back to false and shows ProjectSettings changes.
# It never stops a Play session it did not start: if the Editor is already playing it fails.
set -u
cd "$(dirname "$0")/../.." || exit 1
R="$(pwd)/Tools/Recipes"
OUT="${1:-Docs/Captures/Main3Review}"
unity status 2>/dev/null | grep -q "ready" || { echo "FAIL no Editor in state ready (unity status)"; exit 1; }
unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' || { echo "FAIL Editor is in Play mode or busy; wait for it to stop"; exit 1; }
mkdir -p Temp
for step in day night; do
  sed -e "s|string step = \"day\";|string step = \"$step\";|" -e "s|\"Docs/Captures/Main3Review\"|\"$OUT\"|" "$R/main3_review_capture.cs" > "Temp/main3_review_capture_$step.cs"
done
job() {   # $1 = file; prints the result text
  local id; id=$(unity command --detach eval_file --file "$1" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p')
  [ -n "$id" ] || { echo "could not start the job"; return; }
  unity job wait "$id" --timeout 1800 --json 2>/dev/null | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1
}
unity command editor_play >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "playing"' && break; sleep 2; done
rc=0
for step in day night; do
  for try in 1 2 3; do
    res=$(job "$(pwd)/Temp/main3_review_capture_$step.cs")
    case "$res" in *"run again"*) sleep 3; continue;; esac
    break
  done
  echo "$step: $(printf '%s' "$res" | sed 's/\n/ | /g')"
  case "$res" in *"$step done"*) ;; *) rc=1; break;; esac
done
if [ $rc -eq 0 ]; then
  { echo "# Main3 scripted Play checks"; echo; echo "From Tools/Recipes/main3_review_capture.sh, $(date '+%Y-%m-%d %H:%M'). Every move is PlayerController.Step; see each recipe's header for the method."; }  > "$OUT/Checks.md"
  for check in main3_8_14_climb_check.cs main3_hand_walk_check.cs main3_reach_check_8_16a.cs main3_trunk_check_8_16a.cs; do
    res=$(job "$R/$check")
    { echo; echo "## $check"; echo; printf '%s\n' "$res" | sed 's/\n/\n/g' | sed 's/^/    /'; } >> "$OUT/Checks.md"
    echo "$check: $(printf '%s' "$res" | sed 's/\n/ | /g')"
    case "$res" in *"ALL PASS"*) ;; *) rc=1;; esac
  done
fi
unity command editor_stop >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' && break; sleep 2; done
unity command eval --code 'UnityEngine.Application.runInBackground = false; return "runInBackground " + UnityEngine.Application.runInBackground;' --result-only 2>/dev/null
rm -f Temp/main3_review_capture_day.cs Temp/main3_review_capture_night.cs
echo "ProjectSettings changes (should be none): $(git status --porcelain ProjectSettings | tr '\n' ' ')"
exit $rc
