# Dialogue and Event Formats (DRAFT)

Rook, 2026-09-28. Research only. Nothing here is decided.

Context: DECISIONS.md 2026-09-28 says one data format serves both dialogue and events, and characters come before the format is chosen. So this is a comparison for Milestone 13, not a choice. UI is uGUI with legacy Text, no TextMeshPro (2026-09-20). Any package needs Grant's yes first (CLAUDE.md).

The sample in every section is the same: one campsite exchange with a placeholder resident, one choice that sets a flag and costs 1 MIND, and the other choice that just leaves. Names and lines are placeholders until Quill and Grant write the characters.

What every format must carry, for dialogue and events alike:
- Lines with a speaker.
- Choices.
- Conditions (flags, stats, day, location).
- Effects: set a flag, change HP, MIND or WARD, meet or block a need, mark a location not safe, open a location.

## 1. ScriptableObjects authored in the Inspector

How a line is written: Grant or Quill opens an asset in the Inspector and fills in fields. One asset per conversation or per event, holding a list of nodes. Choices point to other nodes by id.

Sample (what the Inspector holds, shown as field values):

```
Conversation: Campsite2_Humming
  Node start
    Speaker: Resident
    Text: "You hear it too, don't you. At night. Under the fire."
    Choices:
      - Text: "Tell me what you hear."
        Effects: SetFlag heard_humming = true ; Stat MIND -1 ; MeetNeed Social
        Next: listen
      - Text: "I have to get back to the tower."
        Next: end
  Node listen
    Speaker: Resident
    Text: "It says your name. Not the one you use now."
    Next: end
```

Events and dialogue sharing it: both are ScriptableObject types built on the same node and effect classes. An event asset is nodes plus a trigger (day, location, condition). Effects are small serializable classes, so one effect list serves both.

Localization: none built in. Either each Text field becomes a key into Unity's Localization package (another package, needs a yes) or a CSV we load ourselves.

Git diffs: poor. The .asset file is Unity YAML with GUIDs and fileIDs. A changed line shows in a diff, but a reordered list or a moved node makes noisy diffs, and merge conflicts are hard to read. Nobody should edit these by hand while the Editor is open (CLAUDE.md).

Package: none. Built into Unity 6000.3. Needs `[SerializeReference]` for polymorphic effect lists, which exists in 6000.3 (Unity manual, unverified for this exact version by me in a live compile).

License: n/a.

Effort: medium to high. We write the data classes, a runner, and ideally a custom Inspector or graph editor so writing is not painful. Writing hundreds of lines in Inspector fields is slow for a writer. Quill cannot write these directly (no Unity access); Rook would have to import Quill's text.

## 2. JSON or YAML text files

How a line is written: a text file per conversation or event in `Assets/Content`, edited in any text editor. Quill can write them directly.

Sample (JSON):

```json
{
  "id": "Campsite2_Humming",
  "nodes": {
    "start": {
      "speaker": "Resident",
      "text": "You hear it too, don't you. At night. Under the fire.",
      "choices": [
        { "text": "Tell me what you hear.",
          "effects": [ { "setFlag": "heard_humming" },
                       { "stat": "MIND", "add": -1 },
                       { "meetNeed": "Social" } ],
          "next": "listen" },
        { "text": "I have to get back to the tower.", "next": "end" }
      ]
    },
    "listen": {
      "speaker": "Resident",
      "text": "It says your name. Not the one you use now.",
      "next": "end"
    }
  }
}
```

The same in YAML is shorter and easier to read, but see Package below.

Events and dialogue sharing it: same schema. An event file adds a `trigger` block and top-level `effects` (block a need, mark a location not safe). Dialogue is just an event whose body is nodes.

Localization: none built in. Pattern: every text becomes a key, text lives in one file per language. We would build that.

Git diffs: good for JSON if one field per line; very good for YAML. Merges are readable.

Package: JSON parses with Unity's built-in `JsonUtility` (module `com.unity.modules.jsonserialize` is already in the manifest). `JsonUtility` cannot read dictionaries or polymorphic lists, so the effect list needs a flat shape (every effect has the same fields) or a second parser such as Newtonsoft (`com.unity.nuget.newtonsoft-json`, a Unity package; not checked for 6000.3 here, unverified). YAML has no Unity parser; it needs a third-party library such as YamlDotNet (MIT). Not checked for 6000.3, unverified.

License: n/a for JSON with JsonUtility. YamlDotNet is MIT.

Effort: medium. We write the schema, the loader, validation (typos in flag names and node ids are only caught by our own checker), and the runner. JSON with nested braces is error-prone for a non-coder to write by hand; one missing comma breaks the file.

## 3. ink (inkle) with ink-unity-integration

How a line is written: a plain `.ink` text file. Prose first; choices are `*` or `+` lines; flags are variables; game effects are external functions or tags that our code reads. inkle's editor Inky gives a live preview while writing.

Sample:

```ink
VAR heard_humming = false

=== campsite2_humming ===
Resident: You hear it too, don't you. At night. Under the fire.
* [Tell me what you hear.]
    ~ heard_humming = true
    ~ stat("MIND", -1)
    ~ meet_need("Social")
    Resident: It says your name. Not the one you use now.
    -> END
* [I have to get back to the tower.]
    -> END
```

`stat` and `meet_need` are external functions we bind in C#. Speaker names are plain text by convention; we split on the colon or use a `# speaker:Resident` tag.

Events and dialogue sharing it: yes. An event is a knot (`=== event_smoke_at_lake ===`) whose body calls `block_need("Water")` or `mark_unsafe("Lake")` and may contain choices. Trigger rules (which day, which location) either live in ink as conditions the game asks for, or in a small C# table that names the knot to run. ink holds all state (flags) and can save it as JSON, which fits the versioned save.

Localization: none built in. Known approaches are one ink file per language or pulling lines out to a string table ourselves. This is ink's weak spot.

Git diffs: very good. Plain text, one line per line of dialogue. The compiled output is generated on import in 2.0 (no committed JSON needed; see release notes).

Package: `https://github.com/inkle/ink-unity-integration.git#upm` (UPM git URL) or OpenUPM `com.inkle.ink-unity-integration`. Verified on the package's own pages: 2.0.0 requires Unity 2022.3 or newer; the 1.3.0 release notes say "Updated for Unity 6" (obsolete API and serialization warnings fixed). package.json on the upm branch lists no dependencies, so no TextMeshPro. 6000.3 specifically is not named; unverified until a spike compiles it. The 2.0.0 date on the releases page shows "July 7" without a year I could confirm.

License: MIT (stated on the repo).

Effort: low to medium. The runtime (`Story`, `Continue`, `currentChoices`, `ChooseChoiceIndex`) is small. We write the uGUI presenter with legacy Text, the external function bindings, and the trigger table. No stock UI to strip out.

## 4. Yarn Spinner for Unity

How a line is written: a plain `.yarn` text file. Nodes with a title, lines as `Speaker: text`, choices as `->`, variables with `<<set>>`, game effects as commands `<<stat MIND -1>>` bound in C#.

Sample:

```yarn
title: Campsite2_Humming
---
Resident: You hear it too, don't you. At night. Under the fire.
-> Tell me what you hear.
    <<set $heard_humming to true>>
    <<stat MIND -1>>
    <<meet_need Social>>
    Resident: It says your name. Not the one you use now.
-> I have to get back to the tower.
===
```

Events and dialogue sharing it: yes. An event is a node whose body runs commands such as `<<mark_unsafe Lake>>` and `<<block_need Water>>`. Yarn 3 has node groups and "when" conditions for picking among nodes, which is close to an event deck (seen in docs, not tested). Speaker is part of the line syntax, not a convention.

Localization: built in. Yarn generates line IDs and string tables (CSV) per language, and can integrate with Unity Localization. Strongest of the four.

Git diffs: very good for `.yarn` text. Line ID tags (`#line:abc123`) get added to lines when localizing, which adds some noise.

Package: `dev.yarnspinner.unity` 3.2.8 (package.json on the main branch), `"unity": "2022.3"`, dependencies empty. Install from the Asset Store, itch.io, GitHub or OpenUPM (docs). The changelog mentions Unity 6.4 changes (EntityId), so Unity 6 is being tracked. 6000.3 specifically is not named; unverified until a spike compiles it. TextMeshPro is optional for the package, but search results say the stock Line Presenter and samples rely on TextMeshPro. With our no-TMP rule we would write our own presenter, same as ink. Not verified on a live install.

License: MIT (repo). Free from GitHub; paid Asset Store and itch versions add the Text Animator add-on and support.

Effort: low to medium. Similar to ink. More built-in pieces (runner, variable storage, localization), some of which assume TMP UI we would not use.

## Side by side

| | ScriptableObject | JSON / YAML | ink | Yarn Spinner |
|---|---|---|---|---|
| Quill can write it directly | No | Yes (error-prone) | Yes | Yes |
| Live preview while writing | No | No | Inky | Try Yarn Spinner (web) |
| Events share it | Yes | Yes | Yes | Yes |
| Localization | Build it | Build it | Build it | Built in |
| Git diffs | Poor | Good | Very good | Very good |
| New package | None | None (JSON) / YAML lib | Yes, MIT | Yes, MIT |
| Unity 6000.3 | Built in | Built in | Unity 6 noted, 6000.3 unverified | Unity 6 noted, 6000.3 unverified |
| Works without TMP | Yes | Yes | Yes (no stock UI) | Package yes, stock UI no |
| Integration effort | Medium to high | Medium | Low to medium | Low to medium |

## What I would test first

ink first, Yarn Spinner second. Both let Quill write plain text that diffs well, and both carry flags and choices without us writing a parser or validator. ink has no dependencies and no stock UI, so it fits the uGUI and legacy Text rule with nothing to strip out, and its whole state saves to JSON for the versioned save. Yarn's built-in localization is its real advantage; if localization becomes a goal, Yarn moves ahead. ScriptableObjects and hand-written JSON cost us the tooling the two packages already have.

## What the M13 spike should prove

For each tested package, in a throwaway branch, with Grant's yes to add it:
1. It imports into Unity 6000.3.24f1 with zero console errors and zero warnings from the package.
2. Quill's three samples (a logbook day, a notice board sheet, one campsite exchange) run in Play mode on a uGUI canvas with legacy Text.
3. A choice sets a flag, costs 1 MIND through the plain C# game state, and meets Social; the logbook or a debug readout shows it.
4. One event (for example: the lake is not safe today, Water blocked) runs from the same format and reaches the daily loop.
5. Flags survive a save and load through the versioned save.
6. A one-line text change shows as a one-line git diff.
7. A typo in a flag or node name is caught at import, not in Play mode.
8. Removing the package leaves the project compiling (clean exit if it loses).

Sources checked 2026-09-28: github.com/inkle/ink-unity-integration (README, releases, upm package.json), docs.yarnspinner.dev (installation), github.com/YarnSpinnerTool/YarnSpinner-Unity (README, main package.json), web search on Yarn Line Presenter and TextMeshPro.
