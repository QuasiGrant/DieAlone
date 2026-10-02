#!/usr/bin/env bash
# Main3 review capture: contact sheets of Main3 as a player sees it (trails, climb, warps, trail ends, invisible stops, compass views,
# hand-walk views, grayscale trails with grey means, day one and night pairs, top-down map, InvisibleColliders.md, 8.18a: the warp landing check, 8.16a: climb both ways
# with rock and sky shares, the night-rule frames) for Marlow, Pim and
# Vesper, then the scripted Play checks (main3_8_14_climb_check.cs, main3_hand_walk_check.cs and, 8.16a, main3_reach_check_8_16a.cs and main3_trunk_check_8_16a.cs) into Checks.md. Rerun before every
# Grant walk.
# Usage: bash Tools/Recipes/main3_review_capture.sh [outDir]   (from the project root, Editor open on Main3, NOT in Play mode)
# Default outDir: Docs/Captures/Main3Review (git-ignored; only verdicts are committed). Enters Play mode, runs main3_review_capture.cs step "day" then
# step "night" and the two checks as detached Editor jobs, leaves Play mode, sets runInBackground back to false and shows ProjectSettings changes.
# It never stops a Play session it did not start: if the Editor is already playing it fails.
# Area mode (PLAN 8.21 to 8.30): bash Tools/Recipes/main3_review_capture.sh --area <id> [outDir]   (ids in Assets/Settings/Main3Areas.asset:
# camp, front, lake, camp1, camp2, camp3, cave, burn, ward, whole). Default outDir Docs/Captures/Main3Review_<id>. Captures only frames
# standing in the area, the compass views and Deck_<id>.jpg, then runs main3_area_check.cs (flood, traps, walk-into, reach and found,
# deck, inventory) and the warp landing check on the area's warps, and prints the area's PASS and FAIL lines last.
set -u
cd "$(dirname "$0")/../.." || exit 1
R="$(pwd)/Tools/Recipes"
AREA=""; AREARES=""; LANDRES=""; PIXRES=""; EXTRARES=""
if [ "${1:-}" = "--area" ]; then AREA="${2:-}"; [ -n "$AREA" ] || { echo "FAIL --area needs an id"; exit 1; }; shift 2; fi
if [ -n "$AREA" ]; then OUT="${1:-Docs/Captures/Main3Review_$AREA}"; else OUT="${1:-Docs/Captures/Main3Review}"; fi
unity status 2>/dev/null | grep -q "ready" || { echo "FAIL no Editor in state ready (unity status)"; exit 1; }
unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' || { echo "FAIL Editor is in Play mode or busy; wait for it to stop"; exit 1; }
mkdir -p Temp
area_copy() {   # $1 = recipe, $2 = copy: the recipe with the area set
  sed -e "s|^string area = \"[a-z0-9]*\";|string area = \"$AREA\";|" "$R/$1" > "$2"
}
# Vesper 2026-10-02: no capture of a build that dropped a place or effect (expected and found count per item; stops on any zero)
area_copy main3_inventory_check.cs Temp/main3_inventory_check_run.cs
INV=$(unity command eval_file --file "$(pwd)/Temp/main3_inventory_check_run.cs" --result-only 2>/dev/null | sed -n 's/.*"result": "\(.*\)".*/\1/p' | head -1)
printf '%s\n' "$INV" | sed 's/\n/\n/g'
case "$INV" in *"ALL PASS"*|*"FAIL "*" differ"*) ;; *) echo "FAIL inventory (main3_inventory_check.cs): fix the build before capturing"; exit 1;; esac
WARPS=""; EXTRA=""
if [ -n "$AREA" ]; then
  EXTRA=$(unity command eval --code "var a = Main3AreaSet.Load()?.Find(\"$AREA\"); return a == null || a.playChecks == null ? \"\" : string.Join(\",\", a.playChecks);" --result-only 2>/dev/null | sed -n 's/.*"result": "\(.*\)".*/\1/p' | head -1)
  WARPS=$(unity command eval --code "var a = Main3AreaSet.Load()?.Find(\"$AREA\"); return a == null ? \"NOAREA\" : string.Join(\",\", a.warps);" --result-only 2>/dev/null | sed -n 's/.*"result": "\(.*\)".*/\1/p' | head -1)
  case "$WARPS" in ""|NOAREA) echo "FAIL no area $AREA in Assets/Settings/Main3Areas.asset"; exit 1;; esac
fi
for step in day night daytwo; do
  sed -e "s|string step = \"day\";|string step = \"$step\";|" -e "s|\"Docs/Captures/Main3Review\"|\"$OUT\"|" -e "s|^string area = \"\";|string area = \"$AREA\";|" "$R/main3_review_capture.cs" > "Temp/main3_review_capture_$step.cs"
done
job() {   # $1 = file; prints the result text
  local id; id=$(unity command --detach eval_file --file "$1" --json 2>/dev/null | sed -n 's/.*"jobId": "\([0-9a-f]*\)".*/\1/p')
  [ -n "$id" ] || { echo "could not start the job"; return; }
  unity job wait "$id" --timeout 1800 --json 2>/dev/null | sed -n 's/^ *"result": "\(.*\)",\{0,1\}$/\1/p' | head -1
}
unity command editor_play >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "playing"' && break; sleep 2; done
# the GPU Resident Drawer off while the capture and the pixel check render (Wren 2026-10-02: Camera.Render into a texture skipped every
# resident object, so the sheets showed bare terrain); in memory only, never saved, restored below
URPA='((UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline)'
GRD=$(unity command eval --code "var a = $URPA; var was = a.gpuResidentDrawerMode; a.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled; return was.ToString();" --result-only 2>/dev/null | sed -n 's/.*"result": "\([A-Za-z]*\)".*/\1/p')
[ -n "$GRD" ] || { echo "FAIL could not turn the GPU Resident Drawer off"; unity command editor_stop >/dev/null 2>&1; exit 1; }
echo "GPU Resident Drawer was $GRD; off for the capture"
rc=0
# the area's own Play checks run first, before the capture moves the player (main3_8_21_camp_check.cs reads the wake spot Play starts at)
EARLYMD=""; erc=0
if [ -n "$AREA" ]; then
  for c in $(printf '%s' "$EXTRA" | tr ',' ' '); do
    # a check may name a look (8.22: "file.cs?look=Night", underscores for spaces): its `string look` line is set in a copy, and a
    # "run again" (the look was just selected and needs a frame to apply) is retried
    file=${c%%\?*}; look=""; case "$c" in *"?look="*) look=$(printf '%s' "${c#*\?look=}" | tr '_' ' ');; esac
    src="$R/$file"; if [ -n "$look" ]; then sed -e "s|^string look = \"[^\"]*\";|string look = \"$look\";|" "$R/$file" > Temp/playcheck_run.cs; src="$(pwd)/Temp/playcheck_run.cs"; fi
    for try in 1 2 3; do res=$(job "$src"); case "$res" in *"run again"*) sleep 3; continue;; esac; break; done
    EXTRARES="$EXTRARES$c: $(printf '%s' "$res" | sed 's/\\n.*//')"$'\n'
    EARLYMD="$EARLYMD"$'\n'"## $c"$'\n\n'"$(printf '%s\n' "$res" | sed 's/\\n/\n/g' | sed 's/^/    /')"$'\n'
    echo "$c: $(printf '%s' "$res" | sed 's/\n/ | /g')"
    case "$res" in *"ALL PASS"*) ;; *) erc=1;; esac
  done
  # the area check next, also before the capture: it writes Temp/area_found_<id>.txt, from which the capture draws Found_<id>.jpg
  area_copy main3_area_check.cs Temp/main3_area_check_run.cs
  AREARES=$(job "$(pwd)/Temp/main3_area_check_run.cs")
  EARLYMD="$EARLYMD"$'\n'"## main3_area_check.cs"$'\n\n'"$(printf '%s\n' "$AREARES" | sed 's/\\n/\n/g' | sed 's/^/    /')"$'\n'
  case "$AREARES" in *"ALL PASS"*) ;; *) erc=1;; esac
fi
for step in day night daytwo; do
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
  printf '%s' "$EARLYMD" >> "$OUT/Checks.md"
  if [ -n "$AREA" ]; then   # area mode: the area's hard checks only
    sed -e "s|^string onlyWarps = \"\";|string onlyWarps = \"$WARPS\";|" "$R/main3_warp_landing_check.cs" > Temp/main3_warp_landing_check_run.cs
    CHECKS="$(pwd)/Temp/main3_warp_landing_check_run.cs"   # the area check ran before the capture
  else
    CHECKS=""; for c in main3_8_19_forest_check.cs main3_warp_landing_check.cs main3_walk_into_check.cs main3_tower_stairs_check.cs main3_breaks_recheck.cs main3_8_20_closure_check.cs main3_8_14_climb_check.cs main3_hand_walk_check.cs main3_reach_check_8_16a.cs main3_trunk_check_8_16a.cs; do CHECKS="$CHECKS $R/$c"; done
  fi
  LANDRES=""
  for path in $CHECKS; do
    check=$(basename "$path" | sed 's/_run\.cs$/.cs/')
    res=$(job "$path")
    case "$check" in main3_area_check.cs) AREARES="$res";; main3_warp_landing_check.cs) LANDRES="$res";; *) [ -n "$AREA" ] && EXTRARES="$EXTRARES$check: $(printf '%s' "$res" | sed 's/\\n.*//')"$'\n';; esac
    { echo; echo "## $check"; echo; printf '%s\n' "$res" | sed 's/\n/\n/g' | sed 's/^/    /'; } >> "$OUT/Checks.md"
    echo "$check: $(printf '%s' "$res" | sed 's/\n/ | /g')"
    case "$res" in *"ALL PASS"*) ;; *) rc=1;; esac
  done
  # the deck pixel check (CampLayout.md 5): each tree- or rock-covered must-hide target, built and raised 20 m (the control), in batches
  # of 16 eyes per job (main3_deck_pixel_check.cs: one job of every render ran the GPU out of memory)
  PIXRES=""
  if [ -n "$AREA" ]; then
    PIX=$(unity command eval --code "var s = Main3AreaSet.Load(); var a = s.Find(\"$AREA\"); int n = 0; if (a.deckListWritten) foreach (var t in a.deckHide) if (!string.IsNullOrEmpty(t.treeCoverPath)) n++; int side = UnityEngine.Mathf.FloorToInt(2f * s.deckHalf / s.deckGrid + 0.01f) + 1; return n + \" \" + (side * side * 2);" --result-only 2>/dev/null | sed -n 's/.*"result": "\(.*\)".*/\1/p' | head -1)
    NPIX=${PIX% *}; NEYES=${PIX#* }
    for t in $(seq 0 $((NPIX - 1))); do
      built=0; ctrl=0; noise=0; worst=0; label=""
      for raised in 0 1; do
        for from in $(seq 0 16 $((NEYES - 1))); do
          sed -e "s|^string area = \"[a-z0-9]*\"; int target = [0-9]*; int eyeFrom = [0-9]*; int raised = [0-9]*;|string area = \"$AREA\"; int target = $t; int eyeFrom = $from; int raised = $raised;|" "$R/main3_deck_pixel_check.cs" > Temp/main3_deck_pixel_check_run.cs
          res=$(job "$(pwd)/Temp/main3_deck_pixel_check_run.cs")
          case "$res" in PIXEL*seen*) ;; *) echo "FAIL main3_deck_pixel_check.cs target $t: $res"; rc=1; continue;; esac
          label=$(printf '%s' "$res" | sed 's/^PIXEL \(.*\) raised .*/\1/')
          n=$(printf '%s' "$res" | sed 's/.*: seen \([0-9]*\) .*/\1/'); w=$(printf '%s' "$res" | sed 's/.* worst \([0-9]*\) .*/\1/'); z=$(printf '%s' "$res" | sed 's/.* noise \([0-9]*\) .*/\1/')
          if [ $raised -eq 0 ]; then built=$((built + n)); [ $w -gt $worst ] && worst=$w; noise=$((noise + z)); else ctrl=$((ctrl + n)); [ $ctrl -gt 0 ] && break; fi
        done
      done
      if [ $ctrl -eq 0 ] || [ $noise -gt 0 ]; then v="VOID"; rc=1; elif [ $built -eq 0 ]; then v="PASS"; else v="FAIL"; rc=1; fi
      line="$v PIXEL hide $label: seen from $built of $NEYES deck eyes (worst $worst px); control raised 20 m seen from $ctrl eyes; render noise $noise px"
      PIXRES="$PIXRES$line"$'\n'
      echo "$line"; { echo; echo "## main3_deck_pixel_check.cs"; echo; echo "    $line"; } >> "$OUT/Checks.md"
    done
    rm -f Temp/main3_deck_pixel_check_run.cs
  fi
fi
unity command editor_stop >/dev/null 2>&1
for i in $(seq 1 60); do unity command editor_status --result-only 2>/dev/null | grep -q '"playMode": "stopped"' && break; sleep 2; done
unity command eval --code "var a = $URPA; a.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.$GRD; return \"GPU Resident Drawer restored to \" + a.gpuResidentDrawerMode;" --result-only 2>/dev/null | sed -n 's/.*"result": "\(.*\)".*/\1/p'
echo "Render pipeline asset changes (should be none): $(git status --porcelain Assets/Settings/PC_RPAsset.asset | tr '\n' ' ')"
unity command eval --code 'UnityEngine.Application.runInBackground = false; return "runInBackground " + UnityEngine.Application.runInBackground;' --result-only 2>/dev/null
rm -f Temp/main3_review_capture_day.cs Temp/main3_review_capture_night.cs Temp/main3_review_capture_daytwo.cs
echo "ProjectSettings changes (should be none): $(git status --porcelain ProjectSettings | tr '\n' ' ')"
if [ -n "$AREA" ]; then
  echo; echo "AREA $AREA: hard checks (main3_area_check.cs, main3_warp_landing_check.cs on $WARPS)"
  printf '%s\n' "$AREARES" | sed 's/\\n/\n/g' | grep -E "^(ALL PASS|FAILS|INCOMPLETE|PASS|FAIL|----)"
  printf '%s\n' "$LANDRES" | sed 's/\\n/\n/g' | head -1 | sed 's/^/WARP LANDING: /'
  printf '%s' "$PIXRES"
  [ -n "$AREA" ] && printf '%s' "$EXTRARES"
  rm -f Temp/main3_area_check_run.cs Temp/main3_warp_landing_check_run.cs Temp/main3_inventory_check_run.cs Temp/playcheck_run.cs
fi
[ $erc -eq 0 ] || rc=1
exit $rc
