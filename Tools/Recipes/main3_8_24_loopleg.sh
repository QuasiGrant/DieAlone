#!/usr/bin/env bash
# Main3 8.24 LOOP-LEG measure (Sable 824 gate 3): runs main3_8_24_loopleg.cs in Play mode, part "rest" once, then part "tower" in
# batches of BATCH metres (each job renders only its own metres), then appends the longest unbroken run of metres with 0 tower pixels
# at both eye heights to Docs/Captures/Main3Review_north/LoopLeg.md. Usage: bash Tools/Recipes/main3_8_24_loopleg.sh [from] [to]
# (default 80 132), from the project root with the Editor on Main3 and not in Play mode.
set -u
cd "$(dirname "$0")/../.." || exit 1
FROM="${1:-80}"; TO="${2:-132}"; BATCH=8
OUT="Docs/Captures/Main3Review_north/LoopLeg.md"
unity status 2>/dev/null | grep -q "ready" || { echo "FAIL no Editor in state ready"; exit 1; }
unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' || { echo "FAIL Editor is in Play mode or busy"; exit 1; }
mkdir -p Temp
job() {
  local id; id=$(unity command --detach eval_file --file "$1" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p')
  [ -n "$id" ] || { echo "could not start the job"; return; }
  unity job wait "$id" --timeout 1800 --json 2>/dev/null | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1
}
run_part() {   # $1 part, $2 from, $3 to
  sed -e "s|^float mFrom = [0-9.]*f, mTo = [0-9.]*f; string part = \"[a-z]*\";|float mFrom = $2f, mTo = $3f; string part = \"$1\";|" Tools/Recipes/main3_8_24_loopleg.cs > Temp/loopleg_run.cs
  res=$(job "$(pwd)/Temp/loopleg_run.cs"); echo "$1 $2-$3: $(printf '%s' "$res" | sed 's/\\n.*//')"
}
unity command editor_play >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "playing"' && break; sleep 2; done
sleep 3
run_part rest "$FROM" "$TO"
m=$FROM
while [ "$m" -le "$TO" ]; do e=$((m + BATCH - 1)); [ "$e" -gt "$TO" ] && e=$TO; run_part tower "$m" "$e"; m=$((e + 1)); done
unity command editor_stop >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' && break; sleep 2; done
unity command eval --code 'UnityEngine.Application.runInBackground = false; return "ok";' --result-only >/dev/null 2>&1
rm -f Temp/loopleg_run.cs
# the longest unbroken run with 0 px at both heights (rows "| m | rays | px | rays | px |")
awk -F'|' '/^\| [0-9]/ { m=$2+0; p1=$4+0; p2=$6+0; if (p1==0 && p2==0) { if (run==0) s=m; run++; if (run>best) { best=run; bs=s; be=m } } else run=0 }
  END { if (best > 0) printf "\nLongest unbroken run with 0 tower pixels at 1.6 and 2.2 m: %d m (loop m %s to %s).\n", be - bs, bs, be; else printf "\nNo metre has 0 tower pixels at both heights.\n" }' "$OUT" >> "$OUT"
tail -1 "$OUT"
echo "ProjectSettings changes (should be none): $(git status --porcelain ProjectSettings | tr '\n' ' ')"
