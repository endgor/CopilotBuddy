# CopilotBuddy — Instructions

## What is this project?

CopilotBuddy is a **WoW 3.3.5a (WotLK, build 12340)** bot written in C# (.NET 10, WPF, x86). It is a clean rewrite that **ports Honorbuddy's API surface** from three decompiled HB versions (see Reference Sources below). It uses **no original HB DLLs** and has **no authentication system** — it is a private, standalone project.

The bot injects into the WoW client via an EndScene hook, reads game memory through offsets, executes Lua in the game thread, and drives character behavior using **synchronous behavior trees**.

---

## Reference Sources (read-only, never modify)

Three decompiled HB versions sit alongside the project. Each serves ONE specific purpose:

| Purpose | Source folder | Why |
|---------|--------------|-----|
| **Offsets, memory & Lua** | `.hb 3.3.5a\` | Only WotLK version — sole source for memory addresses, offsets, and Lua API calls |
| **API surface & architecture** | `.hb 4.3.4\Honorbuddy\Honorbuddy\` | Clean synchronous code (Cata) — we port class signatures, method names, and code flow from here |
| **Navigation & UI** | `.hb 6.2.3\Honorbuddy\` | Navigation system (Tripper, mesh) + modern WPF layout (WoD) |
| **FrameLock** | `.hb 6.2.3\Honorbuddy\` | FrameLock implementation — we port the design and pattern from here and .hb 5.4.8 |
> **Rule:** When porting a feature, get the *offsets/memory* from 3.3.5a, the *API design* from 4.3.4, and *navigation/UI* from 6.2.3. Write clean, readable code — never copy-paste raw decompiled output.

> **Bug-fix precision:** When a bug exists in HB 4.3.4, check HB 5.4.8 and 6.2.3 to see if a later version fixed it. If so, port the fix pattern from the newer version (e.g. adding a POI guard, a frame visibility check, or a timing adjustment). The base API design stays 4.3.4, but bug fixes from any HB version are valid sources.

### Obfuscated files in HB decompiled sources

Files starting with `-`, `.`, or `⌂` (e.g. `--.XXX.cs`, `⌂-.cs`) are obfuscation artifacts. **Always ignore them.** Only files starting with a letter are real source.

---

## Project Structure

```
CopilotBuddy/                      # Root — C# WinExe, net10.0-windows7.0, x86
├── GreenMagic/                     # EndScene hook injection, memory read/write, ASM
├── Styx/                           # Core bot engine
│   ├── WoWInternals/               # ObjectManager, WoWObjects, Lua, LuaEvents, WoWMovement
│   ├── Offsets/                    # Memory addresses for 3.3.5a (GlobalOffsets, descriptors)
│   ├── Logic/
│   │   ├── Combat/                 # SpellManager, RoutineManager, WoWAura
│   │   ├── Pathing/                # Navigator, Flightor, AvoidanceManager, WoWPoint
│   │   ├── Profiles/               # XML profile loading, quest nodes, vendors, trainers
│   │   ├── Inventory/              # InventoryManager, Frames (Gossip, Merchant, Quest, Taxi…)
│   │   ├── Questing/               # Quest, QuestLog, CustomForcedBehavior
│   │   └── AreaManagement/         # Hotspot, GrindArea, QuestArea
│   ├── Helpers/                    # Logging, Settings, WaitTimer, CircularQueue
│   ├── CommonBot/                  # GameStats, CharacterManagement
│   ├── Database/                   # SQLite queries for creature/NPC spawns
│   ├── Loaders/                    # DynamicLoader, CustomClassLoader, AssemblyVerifier
│   └── Plugins/                    # PluginManager, HBPlugin interface
├── TreeSharp/                      # Behavior tree library (19 files) — namespace: TreeSharp
├── Tripper/                        # Navigation (ported from HB WoD + XNAMath)
├── CommonBehaviors/                # Reusable bot Actions (17) and Decorators (5)
├── Bots/
│   ├── Grind/                      # LevelBot / GrindBot
│   └── Quest/                      # QuestBot, QuestManager, ForcedBehaviors, Objectives
├── UI/                             # WPF windows (Main, Settings, Plugins, DevTools)
├── Lib/                            # Native/managed DLLs (see below)
├── Tools/                          # Python scripts for profile generation
├── docs/                           # MkDocs documentation (api/, compatibility/)
├── PLAN_DUNGEONBUDDY.md            # Implementation plan — DungeonBuddy bot (audited)
├── PLAN_GATHERBUDDY.md             # Implementation plan — GatherBuddy bot (audited)
├── Offsets.txt                     # Extracted 3.3.5a offsets
└── 335offsetsall.txt               # Full offset dump
```

**~542 .cs files. Compiles without errors.**

### External DLLs (Lib/)

| DLL | Role |
|-----|------|
| `fasmdll_managed.dll` | FASM assembler for ASM injection |
| `BlackMagic.dll` | Memory read/write helper |
| `Navigation.dll` | C++ navmesh DLL (ported from HB WoD navigation system) |
| `System.Data.SQLite.dll` | SQLite with SEE encryption |
| `RecastManaged.dll` | Recast navigation |
| `Tripper.Tools.dll` / `Tripper.XNAMath.dll` | Navigation math |

### Dungeon Scripts (outside main project)

66 pre-written dungeon scripts live in `c:\Users\Texy\Desktop\.test\Dungeon Scripts\` covering Classic (2), Burning Crusade (32), and Wrath of the Lich King (32). These are the target scripts that DungeonBuddy must load and execute. They use `#if USE_DUNGEONBUDDY_DLL` conditional compilation, namespace `Bots.DungeonBuddy.Profiles`, and call extension methods like `StyxWoW.Me.IsTank()`, `.IsDps()`, `.IsFollower()`, `.IsRange()`.

---

## Critical Rules

### 1. Synchronous code only — no async/await in bot logic

All bot behavior (BotBase, CommonBehaviors, TreeSharp composites) is **synchronous**. The pulse loop calls `Root.Tick()` on the main thread. No `Task`, no `async`, no `await` in bot code. This matches HB 4.3.4's architecture.

### 2. WotLK 3.3.5a limitations

Features that **do not exist** in WoW 3.3.5a:
- Mastery, Reforging, Transmogrification
- Looking for Raid (LFR) — only LFD (Dungeon Finder, patch 3.3)
- Currency system (Emblems are items, not currencies)
- Scenarios, Challenge modes, Proving Grounds
- Any MoP/WoD/Legion+ system

When HB 4.3.4 references a Cata feature, create a **stub** with the same signature returning neutral values (`null`, `0`, `false`, empty list).

### 3. NEVER invent code

**Every class, method, enum, and property must come from one of the three HB references.** If it doesn't exist in HB, it doesn't exist in CopilotBuddy. Do not create "helpful" enums, utility classes, or convenience wrappers that have no HB counterpart.

Before adding any new type or member, search all three HB references to confirm it exists. If you cannot find it, do not add it.

### 4. Deobfuscation requirements

Code ported from HB 3.3.5a (obfuscated) must be cleaned:

```csharp
// BAD (obfuscated)                // GOOD (clean)
smethod_0()                  →    CheckBehavior()
field_0                      →    _targetGuid
num, num2                    →    index, count
goto IL_0045;                →    restructure control flow (no goto)
```

### 5. TreeSharp behavior tree lifecycle

TreeSharp composites follow this lifecycle — violating it causes subtle bugs:

```
Start(context)  →  called ONCE when composite begins
Tick(context)*  →  called every pulse until Success/Failure
Stop(context)   →  called ONCE when composite ends
```

**Never** call `Start()` every pulse. Store the active composite in a field and only call `Start()` when switching to a different composite.

---

## Code Conventions

| Aspect | Convention |
|--------|-----------|
| Language | Comments, logs, variable names in **English** |
| Naming | Descriptive names — no single-letter variables |
| Logging | `Logging.Write()` with format `[HH:mm:ss.fff] Message` |
| Errors | Log them — never use `MessageBox` |
| Namespace | `TreeSharp` (not `Styx.TreeSharp`) |
| WoWGuid | `ulong` (64-bit) — never 128-bit |
| Null safety | Project uses `<Nullable>enable</Nullable>` |

---

## Key APIs (already implemented in CopilotBuddy)

These are the APIs that exist in the codebase. Use them — do not reinvent them:

| API | Location | Notes |
|-----|----------|-------|
| `BotBase` (abstract) | `Styx/BotBase.cs` | Override `Name`, `Root`, `PulseFlags`; virtual `Start()`, `Stop()`, `Pulse()` |
| `ObjectManager` | `Styx/WoWInternals/ObjectManager.cs` | `.Me`, `.GetObjectsOfType<T>()` |
| `Lua.DoString()` / `Lua.GetReturnVal<T>()` | `Styx/WoWInternals/Lua.cs` | Execute Lua in game thread |
| `Lua.Events.AttachEvent()` / `DetachEvent()` | `Styx/WoWInternals/LuaEvents.cs` | Subscribe to WoW client events |
| `WoWMovement.Face(WoWPoint)` | `Styx/WoWInternals/WoWMovement.cs` | Static method — face a point |
| `Navigator.MoveTo(WoWPoint)` | `Styx/Logic/Pathing/Navigator.cs` | Pathfind and move |
| `SpellManager.Cast()` | `Styx/Logic/Combat/SpellManager.cs` | Cast spells by name or ID |
| `WoWPlayer.IsTank` / `.IsHealer` | `Styx/WoWInternals/WoWObjects/WoWPlayer.cs` | Properties (class-based, not role-based) |
| `WoWGameObject.CanUse()` | `Styx/WoWInternals/WoWObjects/WoWGameObject.cs` | `!Locked && !InUse` |
| `WaitTimer` / `CircularQueue<T>` | `Styx/Helpers/` | Timing and data utilities |

> **Note:** `WoWPlayer.IsTank` is a class-based property (Warrior/Paladin/DK = tank). For role-based checks (actual LFD role), the DungeonBuddy plan defines extension methods using `UnitGroupRolesAssigned()` Lua. See `PLAN_DUNGEONBUDDY.md`.

---

## Active Development Plans

Two implementation plans have been written and audited:

| Plan | Purpose | Status |
|------|---------|--------|
| `PLAN_DUNGEONBUDDY.md` | Automated Dungeon Finder bot (~50 files) | Audited, corrections applied |
| `PLAN_GATHERBUDDY.md` | Herb/Mining gathering bot (5 files) | Audited, corrections applied |

When implementing code from these plans, follow them closely — they have been verified against the actual codebase, dungeon scripts, HB references, and WoW APIs.

---

## Tools (Python)

Profile generation scripts in `Tools/`:

| Tool | Purpose |
|------|---------|
| `ProfileGenerator/zygor_parser.py` | Converts Zygor guides → XML quest profiles |
| `ProfileGenerator/gameobject_exporter.py` | Exports spawns from WotLK MariaDB |
| `ProfileGeneratorV2/` | V2 parser with zone extraction, transport profiles, validation |

**Local WotLK database (SPP):** `127.0.0.1:3310`, user `root`, password `123456`, database `world`

Full documentation: `docs/profile-generator.md`

---

## Prohibitions

- **No HB DLLs** — this project has zero HB binaries
- **No authentication system** — no login, no license
- **No HB trimesh** — we use our own `Navigation.dll`
- **No automated tests** — testing is manual in-game

---

## QC (Quality Control) — Verified Files

A list of files that have already been manually QC'd against all three HB references lives at **`docs/QC-VERIFIED-FILES.md`**.

**Before analyzing or modifying any file, check that list first.** If the file appears there, it has already been verified — do not re-review it unless explicitly asked.

### QC Workflow (when asked to QC files)

1. Run `git diff -- <file>` to see only the uncommitted changes
2. For each change, find the matching code in the HB reference (3.3.5a for offsets, 4.3.4 for API, 6.2.3 for nav/UI)
3. Compare line-by-line — the ported code must match the HB logic
4. Verdict: **PASS** (commit) or **FAIL** (explain what's wrong and fix it)
5. Commit each file individually: `git add <file>; git commit -m "<descriptive message>"`
6. Update `docs/QC-VERIFIED-FILES.md` with the new entry

---

## Git Workflow

- **One file per commit** — each logical change gets its own commit
- **Descriptive messages** — e.g. `"WoWSpell: fix IsMeleeSpell ==2, CanCast via Lua (HB 4.3.4 port)"`
- **Never force push** — ask before any destructive git operation
- **Never commit invented code** — see Rule #3
- **No obfuscated files** — never port files starting with `-`, `.`, `⌂`
- **No `.resx` resource files** — not used
- **No 128-bit WoWGuid** — WotLK uses 64-bit `ulong`
- **No async/await in bot logic** — synchronous only
