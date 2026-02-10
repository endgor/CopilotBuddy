# Deep Porting Gap Analysis — CopilotBuddy vs Honorbuddy (3 versions)

**Generated:** 2026-02-09  
**Scope:** CopilotBuddy (~542 .cs files) vs HB 4.3.4 (~930 .cs), HB 5.4.8 (~800+ .cs), HB 6.2.3 (~1638 .cs)  
**Method:** File-by-file source code analysis across all four codebases

---

## Table of Contents

1. [HB API Evolution Summary](#1-hb-api-evolution-summary)
2. [Obfuscated Code Findings](#2-obfuscated-code-findings)
3. [Missing Classes](#3-missing-classes)
4. [Incomplete Classes](#4-incomplete-classes)
5. [Stub Methods](#5-stub-methods)
6. [Bugs / Incorrect Ports](#6-bugs--incorrect-ports)
7. [Recommended Porting Order](#7-recommended-porting-order)

---

## 1. HB API Evolution Summary

### 4.3.4 → 5.4.8 (Cata → MoP): Major Restructure

| Change | Detail |
|--------|--------|
| **Namespace reorganization** | `Styx.Logic.*` → `Styx.CommonBot.*`. SpellManager, Targeting, TreeRoot, BotBase, BotManager, Frames, Profiles, AreaManagement, Rest all moved. |
| **New `Styx.Common` namespace** | TreeHooks, HookExecutor, Logging, MathEx, Vector2/3, Quaternion, ScriptManager, TypeLoader, WPF controls, Compiler utilities. |
| **New `Styx.Pathing` namespace** | Navigator, MeshNavigator, Flightor, AvoidanceManager moved from `Styx.Logic.Pathing`. Full avoidance subsystem added (17 files). |
| **Memory library swap** | `BlueMagic` → `GreyMagic`. BaseAddress type changed from `uint` to `IntPtr`. |
| **Coroutine system added** | `Styx.CommonBot.Coroutines` namespace with `CommonCoroutines`, `CoroutineTask<T>`, bridge from TreeSharp → async. |
| **TreeHooks system** | Plugin-injectable named hook points in behavior tree. `AddHook()`, `ReplaceHook()`, `InsertHook()`. |
| **PerFrameCachedValue** | Lazy-evaluated per-frame caching helper for expensive property reads. |
| **CombatRoutine evolution** | `ICombatRoutine` marked `[Obsolete]`. Behavior composite properties (`RestBehavior`, `CombatBehavior`, etc.) added alongside legacy void methods. |
| **WoWSpell/WoWAura moved** | From `Styx.Logic.Combat` → `Styx.WoWInternals`. |
| **New classes** | `WoWMissile`, `WoWGroupInfo`, `SpellCooldownInfo`, `GameInput`, `HealTargeting`, `Chat`, `GameStats`, `ActionBar`/`ActionButton`, `CharacterManagement` subsystem. |
| **DB access** | New `Styx.WoWInternals.DB` namespace for raw client DB reading. |

### 5.4.8 → 6.2.3 (MoP → WoD): Incremental + WoD Features

| Change | Detail |
|--------|--------|
| **128-bit GUIDs** | `ulong` → `WoWGuid` struct (4×uint). All ObjectManager keys, method signatures changed. |
| **Garrison system** | 15 new files under `WoWInternals/Garrison/`, 18 new `DB/Garr*.cs` files. |
| **TreeRootState enum** | New: `Stopped`, `Starting`, `Running`, `Paused`, `Stopping`. |
| **CapabilityManager** | Dynamic capability toggling for combat routines with handles, conditions, events. |
| **INavigationProvider removed** | Replaced by concrete `NavigationProvider` class. `IStuckHandler` also removed. |
| **ICombatRoutine/IBehaviors removed** | Only abstract `CombatRoutine` class remains. |
| **AvoidanceManager restructured** | `Avoidance/` (17 files) replaced by `FlightorAnnotation/IndoorEntrance`. |
| **ConfigurationWindow** | BotBase gains WPF `Window ConfigurationWindow` alongside WinForms `Form ConfigurationForm`. |
| **PerFrameCachedValue heavy use** | WoWUnit properties like `Auras`, `CanInterruptCurrentSpellCast` now cached per-frame. |
| **New: TradeSkills** | `Recipe`, `Ingredient`, `TradeSkill` classes. |
| **New: SpellChargeInfo** | WoD spell charge system. |
| **New: WoWAreaTrigger** | Area trigger object with `AreaTriggerShapes/` (Box, Cylinder, Polygon, Sphere). |

### Impact on CopilotBuddy

CopilotBuddy uses HB 4.3.4's namespace layout (`Styx.Logic.*`), which is correct per the instructions. Key considerations:
- Plugins/CRs written for 5.4.8+ use `Styx.CommonBot.*` namespaces — would need `using` aliases for compatibility
- Coroutine-based plugins are incompatible (CopilotBuddy is sync-only) — by design
- 128-bit `WoWGuid` not needed (WotLK uses 64-bit `ulong`) — correct
- TreeHooks could be added for modern plugin compatibility if desired

---

## 2. Obfuscated Code Findings

### Overview

| HB Version | ns* Folders | Obfuscated Files | Key Finding |
|-------------|-------------|-------------------|-------------|
| 4.3.4 | 44 (ns0–ns43) | ~430 | Mostly utility code, x86 emulation, compression, delegates |
| 5.4.8 | 52 (ns0–ns51) | ~400 | Similar pattern, more structured |
| 6.2.3 | 105 (ns0–ns104) | ~390 | Largest but most heavily obfuscated |

### What the Obfuscated Classes Actually Do

Core bot internals (ObjectManager, WoWMovement, Lua, SpellManager, EndScene) are **NOT** in the ns* folders — they're in the non-obfuscated `Styx/` hierarchy. The obfuscator primarily processed:

#### Category 1: Compiler / Assembly Loading (Class455, Class456)
- **Class455** (ns30): Source code compiler. Takes `.cs` files, resolves references, compiles to in-memory assemblies. Uses `System.CodeDom.Compiler.CodeDomProvider`. 
- **CopilotBuddy equivalent:** `SourceCompiler.cs` — fully ported as clean Roslyn-based compiler.

#### Category 2: Bot Initialization Sequence (Class448)
- **Class448** (ns28): Main initialization orchestrator. Calls ObjectManager init, Navigator init, ProfileManager load, RoutineManager init, BotManager load, PluginManager start in sequence.
- **CopilotBuddy equivalent:** Initialization is spread across `MainWindow.xaml.cs` startup and `TreeRoot.Start()`. Same functionality, different organization.

#### Category 3: Stuck Handler (Class485)
- **Class485** (ns33): Full stuck detection and recovery. Detects stuck via position delta tracking, implements recovery strategies (jump, strafe, backtrack, hearth).
- **CopilotBuddy equivalent:** `StuckHandler.cs` — ported from HB 6.2.3's version. Functionally equivalent.

#### Category 4: x86 FPU / Opcode Handlers (~20 classes)
- Classes implementing individual x86 instruction handlers (FADD, FMUL, FCOM, FSTP, etc.) for remote code execution.
- **CopilotBuddy equivalent:** Not needed. GreenMagic uses FASM for direct ASM injection instead of interpreted execution.

#### Category 5: Compression / Encryption (~8 classes)
- LZMA decompression, DES encryption, ZIP stream handling.
- **CopilotBuddy equivalent:** Not needed. Uses .NET standard compression libraries.

#### Category 6: Dungeon-Specific Logic (Class72, Class81)
- **Class72** (ns8): Dungeon targeting logic — filters targets in dungeon context, prioritizes by role/threat.
- **Class81** (ns9): Dungeon navigator — handles instance-specific pathfinding, boss room navigation.
- **CopilotBuddy equivalent:** Not yet implemented. Required for planned DungeonBuddy.

#### Category 7: Closure / Delegate Wrappers (~50+ classes)
- Generated by the decompiler for lambda expressions and LINQ queries. `Class491`, `Class492`, etc.
- **CopilotBuddy equivalent:** N/A — these are decompiler artifacts, not real classes.

#### Category 8: Assembly Verification (Class457)
- Verifies loaded assemblies via hash/signature checks (anti-tamper).
- **CopilotBuddy equivalent:** `AssemblyVerifier.cs` — simplified version for safety checks only.

### Cross-Reference Between Versions

Some classes obfuscated in 4.3.4 have clearer names in later versions:
- 4.3.4 `Class546.smethod_1()` → appears to be a frame state sync, called during pulsation
- 4.3.4 `Struct46` (WoWAura backing) → 5.4.8 has `AuraApplicationInfo` struct
- 4.3.4 `Struct50` (WoWSkill backing) → 5.4.8 has named `SkillInfo` 
- 4.3.4 `Struct77` (WoWDb header) → 6.2.3 has named `DbFileHeader`

---

## 3. Missing Classes

### Priority 0 — Critical (blocks core functionality)

| Class | HB Location | What It Does | WotLK 3.3.5a? | Action |
|-------|-------------|--------------|----------------|--------|
| `GameState` enum | `Styx/GameState.cs` | Game state detection (Idling, Zoning, LoggingIn, etc.) | Yes | **Port** — needed for `StyxWoW.IsInWorld` |
| `GlueScreen` enum | `Styx/GlueScreen.cs` | Login screen state (Login, CharSelect, etc.) | Yes | **Port** — needed for login automation |
| `AreaTable` DBC wrapper | `Styx/WoWInternals/DBC/AreaTable.cs` | Zone ID/name/level/parent lookups | Yes | **Port** — needed for zone identification |
| `BattlegroundStatus` enum | `Styx/Logic/Battlegrounds/` | BG queue state (Queued, Confirm, Active) | Yes | **Port** — dependency for BG system |
| `BattlefieldWinner` enum | `Styx/Logic/Battlegrounds/` | BG outcome (None, Horde, Alliance) | Yes | **Port** |
| `QueuedBattlegroundInfo` struct | `Styx/Logic/Battlegrounds/` | BG queue info (type, wait time, status) | Yes | **Port** |

### Priority 1 — High (blocks subsystems or common API usage)

| Class | HB Location | What It Does | WotLK? | Action |
|-------|-------------|--------------|--------|--------|
| `BagType` enum | `Styx/WoWInternals/WoWObjects/BagType.cs` | Container type (Normal, Herb, Enchant, etc.) | Yes | **Port** |
| `WoWGlyphInfo` class | `Styx/WoWInternals/WoWObjects/WoWGlyphInfo.cs` | Glyph slot data (SpellId, GlyphType, etc.) | Yes (6 glyphs in WotLK) | **Port** |
| `SpecType` enum | `Styx/WoWInternals/WoWObjects/SpecType.cs` | Role type (None, RangedDps, MeleeDps, Healer, Tank) | Yes | **Port** |
| `LfgDungeons` DBC wrapper | `Styx/WoWInternals/DBC/LfgDungeons.cs` | Dungeon Finder dungeon list | Yes (LFD added in 3.3) | **Port** — needed for DungeonBuddy |
| `MapDifficulty` DBC wrapper | `Styx/WoWInternals/DBC/MapDifficulty.cs` | Instance difficulty/max players | Yes | **Port** — needed for dungeon/raid detection |
| `SpellCooldownInfo` class | `Styx/WoWInternals/SpellCooldownInfo.cs` | Cooldown tracking struct | Yes | **Port** |
| `ForceMailManager` class | `Styx/Logic/Profiles/ForceMailManager.cs` | Profile force-mail directives | Yes | **Port** — profiles use `<ForceMail>` tags |
| `WoWArenaTeamInfo` struct | `Styx/WoWInternals/WoWObjects/WoWArenaTeamInfo.cs` | Arena team data | Yes | **Stub** (WotLK has arenas but bots rarely need this) |

### Priority 2 — Medium (nice to have, edge cases)

| Class | Action | Notes |
|-------|--------|-------|
| `ReputationFlags` enum | **Port** | Faction standing flags |
| `WoWInebriationLevel` enum | **Stub** | Sober/Tipsy/Drunk/Smashed — low priority |
| `DurabilityCostEntry` struct | **Port** | Required for `GetEstimatedRepairCost()` |
| `DurabilityQualityEntry` struct | **Port** | Required for `GetEstimatedRepairCost()` |
| `ProfileUnknownElementEventArgs` | **Stub** | Unknown XML elements in profiles |
| `LfgDungeonExpansion` DBC wrapper | **Port** | Dungeon Finder expansion filtering |
| `LocationRetriever` delegate | **Port** | Used by `Mount.MountUp()` overload |
| `GraphicsApi` enum | **Stub** | Needed for `StyxWoW.GameGraphicsApi` |

### Priority 3 — Low / Not Needed

| Class | Action | Reason |
|-------|--------|--------|
| BG Landmark files (13 maps) | Skip for now | Only needed if BGBuddy is implemented |
| `RaFHelper` | Skip | Niche feature |
| `OnDemandDownloading/` | Skip | Not needed — local meshes only |
| `FlagCheckedListBox` | Skip | WinForms — CB uses WPF |
| `EncryptedAttribute` | Skip | Auth/license system — CB has no auth |
| Garrison classes (15 files) | Skip | WoD-only |
| `WoWAreaTrigger` + shapes | Skip | WoD-only |
| `SpellChargeInfo` | Skip | WoD-only |
| `WoWGuid` 128-bit struct | Skip | WotLK uses 64-bit `ulong` |
| `CapabilityManager` | **Stub** | 6.2.3 only — provide empty implementation |

---

## 4. Incomplete Classes

### 4.1 WoWUnit — 54% of HB size (1686 vs 3690 lines)

#### Missing Combat Stats (13 properties)

| Property | Type | Descriptor Source |
|----------|------|-------------------|
| `AttackPower` | `uint` | `UnitField.UNIT_FIELD_ATTACK_POWER` |
| `AttackPowerMultiplier` | `float` | `UnitField.UNIT_FIELD_ATTACK_POWER_MULTIPLIER` |
| `RangedAttackPower` | `uint` | `UnitField.UNIT_FIELD_RANGED_ATTACK_POWER` |
| `RangedAttackPowerMultiplier` | `float` | `UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER` |
| `MinDamage` | `float` | `UnitField.UNIT_FIELD_MINDAMAGE` |
| `MaxDamage` | `float` | `UnitField.UNIT_FIELD_MAXDAMAGE` |
| `MinOffHandDamage` | `float` | `UnitField.UNIT_FIELD_MINOFFHANDDAMAGE` |
| `MaxOffHandDamage` | `float` | `UnitField.UNIT_FIELD_MAXOFFHANDDAMAGE` |
| `MinRangedDamage` | `float` | `UnitField.UNIT_FIELD_MINRANGEDDAMAGE` |
| `MaxRangedDamage` | `float` | `UnitField.UNIT_FIELD_MAXRANGEDDAMAGE` |
| `BaseAttackTime` | `uint` | `UnitField.UNIT_FIELD_BASEATTACKTIME` |
| `BaseOffHandAttackTime` | `uint` | `UnitField.UNIT_FIELD_BASEATTACKTIME + 1` |
| `BaseRangedAttackTime` | `uint` | `UnitField.UNIT_FIELD_RANGEDATTACKTIME` |

#### Missing Negative Stat Modifiers (5 properties)

| Property | Type |
|----------|------|
| `StrengthNegativeModifier` | `uint` — from `UnitField.UNIT_FIELD_NEGSTAT0` |
| `AgilityNegativeModifier` | `uint` — from `UnitField.UNIT_FIELD_NEGSTAT1` |
| `StaminaNegativeModifier` | `uint` — from `UnitField.UNIT_FIELD_NEGSTAT2` |
| `IntellectNegativeModifier` | `uint` — from `UnitField.UNIT_FIELD_NEGSTAT3` |
| `SpiritNegativeModifier` | `uint` — from `UnitField.UNIT_FIELD_NEGSTAT4` |

#### Missing Power System Infrastructure

| Member | Type |
|--------|------|
| `PowerPercent` | `float` — generic power percentage |
| `GetPowerRegenFlat(WoWPowerType)` | `float` — base regen rate |
| `GetPowerRegenInterrupted(WoWPowerType)` | `float` — regen while casting |
| `GetPowerCostModifier(WoWPowerType)` | `uint` — flat cost modifier |
| `GetPowerCostMultiplier(WoWPowerType)` | `float` — percent cost modifier |
| `PowerInfo` struct | Full struct with `Current`, `Max`, `RegenFlat`, `RegenInterrupted`, `CostModifier`, `CostMultiplier` |
| `ManaInfo`, `RageInfo`, `EnergyInfo`, etc. | `PowerInfo` convenience properties |

#### Missing Unit Properties

| Property | Type | Notes |
|----------|------|-------|
| `BaseHealth` | `uint` | Base health before modifiers |
| `MaxHealthModifier` | `float` | Health multiplier |
| `HoverHeight` | `float` | Hover offset |
| `CastSpeedModifier` | `float` | Cast speed multiplier |
| `NativeDisplayId` | `uint` | Original model ID |
| `PetNumber` | `uint` | Pet tracking number |
| `PetExperience` | `uint` | Pet XP |
| `PetNextLevelExperience` | `uint` | Pet XP to next level |
| `AuraState` | `uint` | Aura state flags |
| `NpcEmoteState` | `EmoteState` | Current NPC emote |
| `SubName` | `string` | Creature subtitle/guild |
| `CreatureFamilyInfo` | `CreatureFamily` | Pet family (Wolf, Cat, etc.) |
| `VirtualItemSlotIds` | `uint[3]` | Visual weapon slots |
| `PvPState` | `PvPState` enum | PvP flags |

#### Missing Methods

| Method | Signature |
|--------|-----------|
| `IsTargetingAnyMinion` | `bool` |
| `IsGasCloud` | `bool` — creature type check |
| `RenderFacing` | `float` — raw facing from position struct |

### 4.2 WoWPlayer — 71% smaller (346 vs 1207 lines)

#### Missing Combat Stats (~50 properties)

| Category | Properties |
|----------|-----------|
| **Defense** | `BlockPercent`, `DodgePercent`, `ParryPercent`, `ShieldBlock`, `ShieldBlockCritPercent` |
| **Offense** | `CritPercent`, `RangedCritPercent`, `OffHandCritPercent`, `Expertise`, `ExpertiseOffHand` |
| **Spell schools** (7 each) | `PhysicalCritPercent`..`ArcaneCritPercent`, `PhysicalBonusPositive`..`ArcaneBonusPositive`, `PhysicalBonusNegative`..`ArcaneBonusNegative`, `PhysicalBonusPercent`..`ArcaneBonusPercent` |
| **Healing** | `HealingBonusPositive`, `HealingModifierPercent`, `HealingBonusPercent` |
| **Misc** | `SpellPowerModifierPercent`, `TargetResistanceModifier`, `TargetArmorModifier` |

#### Missing Player Properties

| Property | Type | Priority |
|----------|------|----------|
| `CharacterPoints` | `uint` (talent points remaining) | Medium |
| `RestedExperience` | `uint` | Low |
| `HasRestedXp` | `bool` | Low |
| `GuildRank` | `uint` | Low |
| `BankBagSlotCount` | `byte` | Medium |
| `PetSpellPower` | `uint` | Medium (hunter/warlock) |
| `RuneRegen` | `float[4]` | Medium (DK) |
| `GlyphsEnabled` | `uint` | Medium |
| `Glyphs` | `List<WoWGlyphInfo>` | Medium |
| `MaxLevel` | `uint` | Low |
| `SelfResurrectSpellId` | `uint` | Low |
| `HonorableKills` | `uint` | Low |
| `WatchedFactionIndex` | `uint` | Low |
| Appearance bytes | `SkinType`, `FaceType`, `HairStyle`, `HairColor`, `FacialHair` | Low |

### 4.3 WoWItem — 57% smaller (706 vs 1623 lines)

#### Missing Types

| Type | Notes |
|------|-------|
| `ScalingStatValuesEntry` struct | Item scaling for heirlooms |
| `ScalingStatDistributionEntry` struct | Stat distribution for scaled items |
| `SpellItemEnchantmentRecord` struct | Enchantment data from DBC |
| `WoWItemRandomSuffix` class | Random suffix data (e.g., "of the Bear") |

### 4.4 WoWGameObject — 42% smaller (480 vs 834 lines)

| Missing | Type | Notes |
|---------|------|-------|
| `AnimationState` | `byte` | GO animation state |
| `Model` property | Fix needed | Returns `Name` instead of actual model file path |

### 4.5 WoWContainer

| Missing | Notes |
|---------|-------|
| `BagType` property + enum | Container type classification |
| `UsedSlots` | Number of occupied slots |
| `implicit operator WoWBag` | Conversion to WoWBag |

### 4.6 SpellManager — Critical Overload Gap

`SpellManagerEx` contains 40+ methods that should be on `SpellManager`:

| Missing from SpellManager | Count |
|---------------------------|-------|
| `CanCast` convenience overloads (1-3 params) | 9 |
| `CanBuff` overloads | 12 |
| `Cast(int)`, `Cast(int, WoWUnit)`, `Cast(WoWSpell, WoWUnit)` | 3 |
| `Buff` overloads | 6 |
| `CastRandom` overloads | 9 |
| `BuffRandom` overloads | 9 |

**Impact:** Combat routines call `SpellManager.CanBuff(...)` — these won't compile because the methods are on `SpellManagerEx` instead.

### 4.7 WoWSpell — Missing Properties

| Property | Signature | Priority |
|----------|-----------|----------|
| `IsMeleeSpell` | `bool` — `SpellRangeId == 1 or 2` | **High** |
| `IsSelfOnlySpell` | `bool` — `TargetType & 0x200000 != 0` | **High** |
| `FromId(int)` | `static WoWSpell` — factory method | **High** |
| `Cast()` | `void` — instance cast method | Medium |
| `Tooltip` | `string` | Low |
| `Description` | `string` | Low |
| `DurationPerLevel` | `float` | Low |
| `MaxDuration` | `int` | Low |

### 4.8 LocalPlayer — Missing Critical Members

| Member | Signature | Priority |
|--------|-----------|----------|
| `SpecType` | `SpecType { get; }` — determines role | **Critical** |
| `IsIndoors` override | `bool` — Lua `IsIndoors()` | **High** |
| `IsOutdoors` override | `bool` — Lua `IsOutdoors()` | **High** |
| `GetReputationWith(uint)` | `int` — raw rep value | **High** |
| `GetReputationLevelWith(uint)` | `WoWUnitReaction` — rep level | **High** |
| `HearthstoneAreaId` | `uint` — hearthstone location | **High** |
| `GetTotemBarSpells(int)` | `List<WoWSpell>` | Medium |
| `GetEstimatedRepairCost()` | `WoWPrice` | Medium |
| `CanUseItem(uint, out GameError)` | `bool` | Medium |

### 4.9 Battlegrounds — Nearly Empty Stub

CopilotBuddy has ~55 lines vs HB's 394. Missing 15+ methods/properties:
- `JoinBattlefield()`, `LeaveBattlefield()`, `AcceptBattlegroundConfirmation()`
- `IsQueuedForBattleground()`, `WaitingForConfirmation`, `Finished`, `Winner`
- `GetStatus()`, `GetQueuedBattlegroundWaitTime()`, `GetQueuedBattlegroundInfo()`
- `BattlefieldInstanceRunTime`, `BattlefieldStartTime`, `BattlegroundStatuses`

### 4.10 StyxWoW Main Class

| Missing | Signature | Priority |
|---------|-----------|----------|
| `IsInWorld` | `bool` — `IsInGame && GameState != Zoning` | **Critical** |
| `GameState` | `GameState { get; }` — reads memory | **Critical** |
| `GlueState` | `GlueScreen { get; }` — login state | **High** |
| `Camera` property | `WoWCamera { get; }` — camera access | Medium |
| `Landmarks` property | `Landmarks { get; }` — exposed ref | Low |
| `GameGraphicsApi` | `GraphicsApi { get; }` | Low |

### 4.11 GatherBuddy — Minimal Implementation (497 vs 5658 lines, 9%)

| Missing Feature | Priority | Notes |
|-----------------|----------|-------|
| Flying/Flightor gathering | **Critical** | Northrend herb/mining routes require flying |
| Vendor/repair integration | **High** | No bag management during gathering |
| Mining pick validation | Medium | Won't warn if no pick equipped |
| Stats tracking | Low | No XP/hr, nodes/hr display |
| Node blacklist persistence | Medium | Unreachable nodes not saved between sessions |
| `Initialize()` override | Medium | Blacklisted nodes not loaded |
| Configuration form | Low | No settings UI |

### 4.12 Targeting — Missing Filters

| Missing from DefaultRemoveTargetsFilter | Impact |
|----------------------------------------|--------|
| Profile `AvoidMobs` check | Attacks mobs the profile says to avoid |
| Profile `Blackspot` check | Pulls mobs in restricted areas |
| `IsCritter` / `IsNonCombatPet` check | Attacks critters and pets |
| `IsNotWithinHotspotRange` check | Attacks mobs far from hotspots |
| Party/raid minion awareness | Removes party member pets from targets |

---

## 5. Stub Methods

Methods that exist but return placeholder values:

### 5.1 Battlegrounds

| Method | Current Return | Should Do |
|--------|---------------|-----------|
| `GetCurrentBattleground()` | `BattlegroundType.None` | Read BG type from game memory |
| `IsInsideBattleground` (getter) | Auto-property (default false) | Check current map vs BG map IDs |

### 5.2 WoWMovement

| Member | Current | Should Do |
|--------|---------|-----------|
| `ActiveMover` | Returns `WoWPoint` | Return `WoWUnit` (the actual active mover) |
| `ActiveMoverGuid` | **Missing** | Return ulong GUID of active mover |
| `Pulse()` | **Missing** | Process timed movement queue entries |

### 5.3 CombatRoutine Behavior Properties

| Property | Current | Should Do |
|----------|---------|-----------|
| `RestBehavior` | Returns `null` | Return `Decorator(NeedRest, Action(Rest))` |
| `PreCombatBuffBehavior` | Returns `null` | Return decorated composite wrapping `PreCombatBuff()` |
| `PullBuffBehavior` | Returns `null` | Return decorated composite |
| `PullBehavior` | Returns `null` | Return `Action` that calls `Pull()` |
| `CombatBuffBehavior` | Returns `null` | Return decorated composite |
| `CombatBehavior` | Returns `null` | Return `Action(Combat)` |
| `HealBehavior` | Returns `null` | Return `Decorator(NeedHeal, Action(Heal))` |

**Impact:** Legacy combat routines that don't override the Behavior properties will get null composites — their Rest/Heal/Buff phases will silently do nothing.

### 5.4 Cata-Only Stubs Needed

These should exist with neutral returns per the WotLK instructions:

| Property | Stub Return | Reason |
|----------|-------------|--------|
| `WoWUnit.CurrentSoulShards` | `0` | Cata warlock power |
| `WoWUnit.CurrentEclipse` | `0` | Cata druid power |
| `WoWUnit.CurrentHolyPower` | `0` | Cata paladin power |
| `WoWUnit.MaxSoulShards/Eclipse/HolyPower` | `0` | Matching max |
| `WoWPlayer.Mastery` | `0f` | Cata stat |
| `WoWUnit.FocusPercent` | `0f` | Hunter power percent |

---

## 6. Bugs / Incorrect Ports

### P0 — Critical Bugs

| # | Location | Bug | HB Behavior | CB Behavior | Fix |
|---|----------|-----|-------------|-------------|-----|
| 1 | `WoWUnit.CurrentCastTimeLeft` | Reads spell's **base cast time** instead of actual cast **end timestamp** from memory | Reads performance counter offset, converts via `PerformanceCounterToDateTime`, computes `EndTime - Now` | Uses `CastingSpell.CastTime` (static value) | Read cast end time from memory offset, subtract current time |
| 2 | `CombatRoutine` Behavior defaults | All return `null` | Return decorated composites wrapping the virtual void methods | Legacy CRs get no rest/heal/buff behavior | Implement default wrapped composites as in HB 4.3.4 |
| 3 | `WoWMovement.ActiveMover` | Returns `WoWPoint` instead of `WoWUnit` | Returns `WoWUnit` — the controlled unit | Returns position point, breaking `.Guid`, `.IsValid`, `.Transport` access | Change return type to `WoWUnit` |
| 4 | `TreeRoot.Start()` | Calls `Current.Initialize()` directly | Calls `Current.DoInitialize()` which guards against double-init | May initialize bot twice | Use `DoInitialize()` |
| 5 | `ICombatRoutine` | Missing `ShutDown()` method | Interface requires `ShutDown()` | Combat routines implementing the interface won't compile | Add `ShutDown()` to interface |

### P1 — High-Priority Bugs

| # | Location | Bug | Fix |
|---|----------|-----|-----|
| 6 | `WoWMovement.MovementDirection` enum | Values wrong: `Forward=1, Backwards=2` | HB uses `Forward=16, Backwards=32`. Raw value comparisons will fail |
| 7 | `WoWClass` enum namespace | In `Styx.Combat.CombatRoutine` | Should be `Styx` or aliased — callers using `Styx.WoWClass` won't resolve |
| 8 | `StyxWoW.SleepForLagDuration()` | Base constant 50ms | HB uses 150ms. Actions may fire too early under lag |
| 9 | `LootTargeting.DefaultRemoveTargetsFilter` | Missing `Blacklist.Contains()` check | Blacklisted objects will still be targeted for looting |
| 10 | `BotPoi.Clear()` | Doesn't call `Navigator.Clear()` | Navigation path remains after POI clear, causing stale movement |
| 11 | `WoWUnit.IsPet` | Checks `SummonedBy != null` (object lookup) | Should check `SummonedByUnitGuid != 0` (faster, doesn't fail when summoner leaves ObjectManager) |
| 12 | `WoWGameObject.Model` | Returns `Name` | Should return model file path from cached info |
| 13 | `LocalPlayer.XPToNextLevel` | Hardcoded formula `level² × 100 + level × 500` | WoW uses a DBC lookup table; formula is inaccurate |
| 14 | `Mount.Dismount()` | No flying descent loop | Will dismount mid-air causing fall damage/death |
| 15 | `Mount.Dismount()` | Doesn't fire `OnDismount` event | Event subscribers won't be notified |

### P2 — Medium Bugs

| # | Location | Bug | Fix |
|---|----------|-----|-----|
| 16 | `TreeRoot` tick | Missing taxi check | Bot tries to act while on flight path taxi |
| 17 | `TreeRoot` tick | Missing fall time navigator clear | Navigation not cleared during long falls |
| 18 | `TreeRoot` OnBotStart | Missing CVars setup | `autoSelfCast`, `autoInteract`, `interactOnLeftClick` not set |
| 19 | `TreeRoot` | Missing WoW process exit handler | If WoW crashes, bot keeps running |
| 20 | `ProfileManager.GetProfileForLevel()` | Missing `ContinentId` check | May load wrong sub-profile on continent change |
| 21 | `LocalPlayer.Totems` | Returns `WoWTotemInfo[]` | HB returns `List<WoWTotemInfo>` — callers using `.Add()` will fail |
| 22 | `LocalPlayer.SetFocus()` | Returns `void` | HB returns `bool` — callers checking success will fail |
| 23 | `Targeting.DefaultTargetWeight` | Uses `CountUnitsNearLocation()` | HB uses `GetAggroWithin()` — scores count passive units not just hostile |
| 24 | `Targeting.PullDistance` | Uses `LevelbotSettings.Instance.PullDistance` | HB also checks `RoutineManager.Current.PullDistance` — CRs can't override |
| 25 | `WoWPulsator` | Missing `WoWMovement.Pulse()` call | Movement timeouts never expire |
| 26 | `WoWPulsator` | Missing `AvoidanceManager.Pulse()` call | Avoidance zones not updated |
| 27 | `WoWObject.InLineOfSight` | Bad distance/Z heuristic at base class level | Should use `GameWorld.IsInLineOfSight()`. Note: `WoWUnit` override is correct |
| 28 | `TreeRoot.TicksPerSecond` | Default 10 | HB defaults to 15 — slower response time |

### P3 — Minor / Cosmetic

| # | Location | Bug |
|---|----------|-----|
| 29 | `LootTargeting.DefaultTargetWeight` | Base score 200 vs HB's 400 |
| 30 | Health/mana/power return types | `int` in CB vs `uint` in HB — may break plugins expecting unsigned |
| 31 | `WoWDb` indexer | Returns `null` instead of throwing — silent failures |
| 32 | `WoWCache` entry stride | 136 bytes vs HB's 144 — needs verification for WotLK |

---

## 7. Recommended Porting Order

Priority is based on: (1) blocks compilation of existing code, (2) causes runtime bugs, (3) missing core functionality.

### Phase 1 — Fix Compilation Blockers (Est: 2-3 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 1.1 | **Merge SpellManagerEx into SpellManager** (or make it `partial class SpellManager`) | All combat routines using `SpellManager.CanBuff()` etc. won't compile | Low |
| 1.2 | **Move/alias `WoWClass` to `Styx` namespace** | External code using `Styx.WoWClass` fails | Low |
| 1.3 | **Add `ShutDown()` to `ICombatRoutine`** | Interface implementors won't compile | Low |
| 1.4 | **Add `WoWSpell.IsMeleeSpell`, `IsSelfOnlySpell`, `FromId(int)`** | Common usage in combat routines | Low |
| 1.5 | **Add `GameState` enum + `StyxWoW.IsInWorld`** | Referenced throughout bot logic | Low |
| 1.6 | **Add `SpecType` enum + `LocalPlayer.SpecType`** | Role detection for CRs | Medium |
| 1.7 | **Make `LootSlotInfo` public** with `LootItemId`, `LootQuantity`, `LootSlot` | Loot frame consumers | Low |

### Phase 2 — Fix Critical Runtime Bugs (Est: 3-4 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 2.1 | **Fix `CombatRoutine` Behavior property defaults** — return wrapped composites not null | Legacy CRs get no combat/heal/buff/rest behavior | Medium |
| 2.2 | **Fix `WoWUnit.CurrentCastTimeLeft`** — read end timestamp from memory | Interrupt timing wrong | Medium |
| 2.3 | **Fix `WoWMovement.ActiveMover`** — return `WoWUnit` not `WoWPoint` | All code accessing mover properties breaks | Medium |
| 2.4 | **Fix `WoWMovement.MovementDirection` enum values** | Raw flag comparisons wrong | Low |
| 2.5 | **Fix `TreeRoot.Start()`** — use `DoInitialize()` | Bot may double-initialize | Low |
| 2.6 | **Fix `Mount.Dismount()`** — add flying descent + fire event | Fall damage/death on mid-air dismount | Medium |
| 2.7 | **Fix `LootTargeting` blacklist check** | Blacklisted objects still looted | Low |
| 2.8 | **Fix `BotPoi.Clear()`** — add `Navigator.Clear()` | Stale navigation paths | Low |
| 2.9 | **Add `WoWMovement.Pulse()` to pulsator** | Movement timeouts never expire | Low |
| 2.10 | **Add `AvoidanceManager.Pulse()` to pulsator** | Avoidance zones ignored | Low |

### Phase 3 — Complete WoWUnit/WoWPlayer (Est: 3-5 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 3.1 | **Add 13 combat stat properties to WoWUnit** | CRs checking attack power, damage ranges | Medium |
| 3.2 | **Add 5 negative stat modifiers to WoWUnit** | Complete stat picture | Low |
| 3.3 | **Add PowerInfo infrastructure** (6+ members) | Power management system | Medium |
| 3.4 | **Add missing unit properties** (14 listed above) | Various CR/plugin usage | Medium |
| 3.5 | **Add ~50 combat stat properties to WoWPlayer** | Stat inspection, CR decisions | High |
| 3.6 | **Add `LocalPlayer.IsIndoors`/`IsOutdoors` overrides** | Mount/spell logic | Low |
| 3.7 | **Add `LocalPlayer.GetReputationWith()`/`GetReputationLevelWith()`** | Faction-gated quest logic | Medium |
| 3.8 | **Add `LocalPlayer.HearthstoneAreaId`** | Hearth logic | Low |

### Phase 4 — Complete Subsystems (Est: 4-6 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 4.1 | **Port `AreaTable` DBC wrapper** | Zone identification | Medium |
| 4.2 | **Port `LfgDungeons` + `MapDifficulty` DBC wrappers** | DungeonBuddy prerequisite | Medium |
| 4.3 | **Implement `StyxWoW.Camera` property** | Visual/UI features | Low |
| 4.4 | **Fix targeting filters** (AvoidMobs, Blackspots, party minions, critters) | Wrong targeting | Medium |
| 4.5 | **Port `ForceMailManager`** | Profile mailing support | Medium |
| 4.6 | **Port missing item types** (`BagType`, `WoWGlyphInfo`, etc.) | Item/container management | Low |
| 4.7 | **Add WoWSpell.Cast() instance method** | Convenience API | Low |
| 4.8 | **Fix TreeRoot tick** (taxi check, fall time clear, CVars) | Edge case robustness | Low |

### Phase 5 — GatherBuddy Completion (Est: 3-4 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 5.1 | **Add Flightor-based flying gather** | Northrend gathering | High |
| 5.2 | **Add vendor/repair integration** | Inventory management | Medium |
| 5.3 | **Add mining pick validation** | User experience | Low |
| 5.4 | **Add node blacklist persistence** | Efficiency | Low |

### Phase 6 — Battleground System (Est: 4-5 days, defer if no BGBuddy)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 6.1 | **Port Battlegrounds class** (15+ methods) | BG bot functionality | High |
| 6.2 | **Port BG enums** (Status, Winner, Type) | Dependencies | Low |
| 6.3 | **Port BG landmark files** (if BGBuddy planned) | BG navigation | High |

### Phase 7 — Cata-Only Stubs + Polish (Est: 1-2 days)

| # | Task | Impact | Effort |
|---|------|--------|--------|
| 7.1 | **Add Cata power type stubs** (SoulShards, Eclipse, HolyPower) | API completeness | Low |
| 7.2 | **Add `WoWPlayer.Mastery` stub** | API completeness | Low |
| 7.3 | **Add `GlueScreen` enum + property** | Login automation | Low |
| 7.4 | **Add `WoWGuid` compatibility wrapper** if needed for plugins | Plugin compat | Low |
| 7.5 | **Fix return type mismatches** (int vs uint on health/mana/power) | Plugin compat | Low |

---

## Appendix A: File Count Summary

| Area | HB 4.3.4 | CopilotBuddy | Coverage |
|------|----------|-------------|----------|
| WoWInternals/WoWObjects/ | 38 files | 33 files | 87% |
| Logic/Combat/ | 17 files | 14 files | 82% |
| Logic/Pathing/ | 16 files | 15 files | 94% |
| Logic/Profiles/ | 42 files | 45 files | 107% (CB enhanced) |
| Logic/Questing/ | 12 files | 16 files | 133% (CB enhanced) |
| Logic/AreaManagement/ | 12 files | 12 files | 100% |
| Logic/Inventory/Frames/ | 28 files | 32 files | 114% (CB enhanced) |
| Helpers/ | 46 files | 25 files | 54% |
| Plugins/ | 6 files | 6 files | 100% |
| Loaders/ | 2 files | 5 files | 250% (CB enhanced) |
| Database/ | 3 files | 4 files | 133% (CB enhanced) |
| Bots/ | 6 bots | 3 bots | 50% (DungeonBuddy planned) |
| ns* obfuscated | ~430 files | N/A | Covered by clean implementations |
| **Total Styx/** | ~220 files | ~412 files | High overall coverage |

## Appendix B: WotLK Compatibility Notes

| Feature | WotLK Status | Action |
|---------|-------------|--------|
| WoWGuid | 64-bit `ulong` | ✅ CB correct |
| Talents | 71 points, 3 trees | ✅ CB handles |
| Glyphs | 6 slots (Major/Minor) | Need `WoWGlyphInfo` port |
| Mastery | Doesn't exist | Stub returning 0 |
| Dungeon Finder | Added in 3.3 | Port `LfgDungeons` DBC |
| Raid Finder | Doesn't exist | Stub |
| Currency system | Emblems are items | No currency API needed |
| Spell IDs | Different from Cata | ✅ CB uses 3.3.5a SpellDb |
| `AcceptProposal()` | Needs hardware event | ✅ CB aware |
| Power types | No SoulShards/Eclipse/HolyPower | ✅ CB enum correct |
| Reforging | Doesn't exist | Stub |
| Transmogrification | Doesn't exist | Stub |
| Arena | Exists (2v2, 3v3, 5v5) | Low priority |
| Garrison | Doesn't exist | Skip entirely |
| Area Triggers | Doesn't exist | Skip entirely |
