#!/usr/bin/env bash
# Main3 proof frames (8.18a): renders every shot in a shots file, in each look it names, with main3_proof_frames.cs.
# Usage: bash Tools/Recipes/main3_proof_frames.sh <shots file> [outDir]   (project root, Editor on Main3, not in Play mode)
# Enters Play mode, runs the recipe once per look named in the file (selecting it first), leaves Play mode, resets runInBackground.
set -u
cd "$(dirname "$0")/../.." || exit 1
SHOTS="$(cygpath -m "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")")"; OUT="${2:-Docs/Captures/Main3Review_818a/Proof}"
unity status 2>/dev/null | grep -q "ready" || { echo "FAIL no Editor in state ready"; exit 1; }
unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' || { echo "FAIL Editor is in Play mode or busy"; exit 1; }
mkdir -p Temp
job() { local id; id=$(unity command --detach eval_file --file "$1" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p'); [ -n "$id" ] || { echo "could not start the job"; return; }
  unity job wait "$id" --timeout 1800 --json 2>/dev/null | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1; }
unity command editor_play >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "playing"' && break; sleep 2; done
LOOKS=$(grep -v '^#' "$SHOTS" | cut -d'|' -f2 | sort -u | tr ' ' '_')
for look in $LOOKS; do
  look="${look//_/ }"
  sed -e "s|string wantLook = \"\";|string wantLook = \"$look\";|" -e "s|\"Tools/Recipes/main3_proof_shots.txt\"|\"$SHOTS\"|" -e "s|\"Docs/Captures/Main3Review_818a/Proof\"|\"$OUT\"|" Tools/Recipes/main3_proof_frames.cs > Temp/main3_proof_frames_run.cs
  for try in 1 2 3; do res=$(job "$(pwd)/Temp/main3_proof_frames_run.cs"); case "$res" in *"run again"*) sleep 3; continue;; esac; break; done
  echo "$res"
done
unity command editor_stop >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' && break; sleep 2; done
unity command eval --code 'UnityEngine.Application.runInBackground = false; return "runInBackground " + UnityEngine.Application.runInBackground;' --result-only >/dev/null 2>&1
rm -f Temp/main3_proof_frames_run.cs
