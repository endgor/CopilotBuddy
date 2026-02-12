# Audit: Combat & Spell Systems — CB vs HB 4.3.4

**Date:** 2025-02-12  
**Scope:** SpellManager, WoWSpell, WoWAura, RoutineManager, CombatRoutine, Targeting, RaFHelper  
**CB:** `CopilotBuddy\Styx\Logic\Combat\` and related  
**HB:** `.hb 4.3.4\Honorbuddy\Honorbuddy\Styx\Logic\Combat\` and related

---

## Summary

| File | OK | STUB | MISSING | Notes |
|------|---:|-----:|--------:|-------|
| **SpellManager** | 30 | 2 | 24 | Missing many convenience overloads + int-based CastRandom/BuffRandom |
| **WoWSpell** | 30 | 1 | 2 | Very solid; missing `Description` property |
| **WoWAura** | 17 | 0 | 0 | Complete — CB has extras beyond HB |
| **RoutineManager** | 4 | 1 | 2 | Missing `Current` setter and routine selection UI |
| **CombatRoutine** | 23 | 1 | 1 | Missing `ToString()` override; `PullBehavior` simplified |
| **Targeting** | 22 | 0 | 3 | Missing `AllowEvents`, `Instance` setter, `IsNotWithinHotspotRange` |
| **RaFHelper** | 6 | 0 | 0 | Complete match |
| **TOTAL** | **132** | **5** | **32** | |

---

## 1. SpellManager

**Files:**  
- CB: `Styx/Logic/Combat/SpellManager.cs` (746 lines)  
- HB: `Styx/Logic/Combat/SpellManager.cs` (1199 lines)

### Properties

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Spells` | `SpellCollection` | **STUB** | CB returns `Dictionary<string,WoWSpell>`, HB returns `SpellCollection` — type mismatch |
| `RawSpells` | `ReadOnlyCollection<WoWSpell>` | **STUB** | CB returns `Dictionary<string,WoWSpell>`, HB returns `ReadOnlyCollection<WoWSpell>` |
| `GlobalCooldown` | `bool` | **OK** | Both use memory-based linked list walk |
| `GlobalCooldownLeft` | `TimeSpan` | **OK** | Both use memory-based linked list walk |

### HasSpell

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `HasSpell(string)` | `bool` | **OK** | |
| `HasSpell(int)` | `bool` | **OK** | |
| `HasSpell(WoWSpell)` | `bool` | **MISSING** | No WoWSpell overload in CB |

### CanCast (15 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `CanCast(string)` | `bool` | **OK** | CB wraps `CanCastSpell` |
| `CanCast(string, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanCast(string, bool checkRange)` | `bool` | **MISSING** | HB targets `CurrentTarget` with checkRange; CB has no `(string,bool)` overload |
| `CanCast(string, WoWUnit, bool)` | `bool` | **OK** | Via optional params |
| `CanCast(string, WoWUnit, bool, bool)` | `bool` | **OK** | Directly implemented |
| `CanCast(int)` | `bool` | **MISSING** | No standalone `int` overload |
| `CanCast(int, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanCast(int, bool)` | `bool` | **MISSING** | Needs `(int,bool)` → `(id, CurrentTarget, checkRange)` |
| `CanCast(int, WoWUnit, bool)` | `bool` | **OK** | Via optional params |
| `CanCast(WoWSpell)` | `bool` | **MISSING** | No standalone spell overload |
| `CanCast(WoWSpell, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanCast(WoWSpell, bool)` | `bool` | **MISSING** | Needs `(spell, CurrentTarget, checkRange)` |
| `CanCast(WoWSpell, WoWUnit, bool)` | `bool` | **OK** | Via optional params |
| `CanCast(WoWSpell, WoWUnit, bool, bool)` | `bool` | **OK** | Via optional params |
| `CanCast(WoWSpell, WoWUnit, bool, bool, bool)` | `bool` | **MISSING** | 5th param `accountForLagTolerance` not in CB |

### CanBuff (12 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `CanBuff(string)` | `bool` | **OK** | Via optional `target=null` |
| `CanBuff(string, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanBuff(string, bool checkRange)` | `bool` | **MISSING** | `(name, StyxWoW.Me, checkRange)` |
| `CanBuff(string, WoWUnit, bool)` | `bool` | **OK** | Directly implemented |
| `CanBuff(int)` | `bool` | **OK** | Via optional `target=null` |
| `CanBuff(int, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanBuff(int, bool)` | `bool` | **MISSING** | `bool` doesn't match `WoWUnit` param |
| `CanBuff(int, WoWUnit, bool)` | `bool` | **OK** | Via optional params |
| `CanBuff(WoWSpell)` | `bool` | **OK** | Via optional `target=null` |
| `CanBuff(WoWSpell, WoWUnit)` | `bool` | **OK** | Via optional params |
| `CanBuff(WoWSpell, bool)` | `bool` | **MISSING** | `bool` doesn't match `WoWUnit` param |
| `CanBuff(WoWSpell, WoWUnit, bool)` | `bool` | **OK** | Directly implemented |

### Cast (6 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Cast(string)` | `bool` | **OK** | |
| `Cast(string, WoWUnit)` | `bool` | **OK** | GUID-based casting |
| `Cast(int)` | `bool` | **OK** | |
| `Cast(int, WoWUnit)` | `bool` | **OK** | |
| `Cast(WoWSpell)` | `bool` | **OK** | |
| `Cast(WoWSpell, WoWUnit)` | `bool` | **OK** | |

### Buff (6 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Buff(string)` | `bool` | **OK** | Via `target ??= StyxWoW.Me` |
| `Buff(string, WoWUnit)` | `bool` | **OK** | |
| `Buff(int)` | `bool` | **OK** | Via optional `target=null` |
| `Buff(int, WoWUnit)` | `bool` | **OK** | |
| `Buff(WoWSpell)` | `bool` | **OK** | Via optional `target=null` |
| `Buff(WoWSpell, WoWUnit)` | `bool` | **OK** | |

### CastRandom (9 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `CastRandom(IEnumerable<string>, WoWUnit, bool)` | `bool` | **OK** | |
| `CastRandom(IEnumerable<string>, WoWUnit)` | `bool` | **MISSING** | No short overload |
| `CastRandom(IEnumerable<string>, bool)` | `bool` | **MISSING** | Uses `CurrentTarget` |
| `CastRandom(IEnumerable<int>, WoWUnit, bool)` | `bool` | **MISSING** | No `int` overloads at all |
| `CastRandom(IEnumerable<int>, WoWUnit)` | `bool` | **MISSING** | |
| `CastRandom(IEnumerable<int>, bool)` | `bool` | **MISSING** | |
| `CastRandom(IEnumerable<WoWSpell>, WoWUnit, bool)` | `bool` | **OK** | |
| `CastRandom(IEnumerable<WoWSpell>, WoWUnit)` | `bool` | **MISSING** | |
| `CastRandom(IEnumerable<WoWSpell>, bool)` | `bool` | **MISSING** | |

### BuffRandom (9 HB overloads)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `BuffRandom(IEnumerable<string>, WoWUnit, bool)` | `bool` | **OK** | |
| `BuffRandom(IEnumerable<string>, WoWUnit)` | `bool` | **MISSING** | |
| `BuffRandom(IEnumerable<string>, bool)` | `bool` | **MISSING** | |
| `BuffRandom(IEnumerable<int>, WoWUnit, bool)` | `bool` | **MISSING** | No `int` overloads |
| `BuffRandom(IEnumerable<int>, WoWUnit)` | `bool` | **MISSING** | |
| `BuffRandom(IEnumerable<int>, bool)` | `bool` | **MISSING** | |
| `BuffRandom(IEnumerable<WoWSpell>, WoWUnit, bool)` | `bool` | **OK** | |
| `BuffRandom(IEnumerable<WoWSpell>, WoWUnit)` | `bool` | **MISSING** | |
| `BuffRandom(IEnumerable<WoWSpell>, bool)` | `bool` | **MISSING** | |

### Other

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `StopCasting()` | `void` | **OK** | Both use `SpellStopCasting()` Lua |

**SpellManager totals: 30 OK, 2 STUB, 24 MISSING**

> Most MISSING are trivial convenience overloads that expand `(name/id/spell, CurrentTarget, defaultFlags)`. The 5-param `CanCast` with `accountForLagTolerance` is the only structurally significant gap.

---

## 2. WoWSpell

**Files:**  
- CB: `Styx/Logic/Combat/WoWSpell.cs` (505 lines)  
- HB: `Styx/Logic/Combat/WoWSpell.cs` (2142 lines — includes SpellEntry structs)

### Properties

| Member | HB Type | CB Status | Notes |
|--------|---------|-----------|-------|
| `IsMeleeSpell` | `bool` | **OK** | CB uses synthetic `SpellRangeId`, HB uses `InternalInfo.SpellRangeId` |
| `IsSelfOnlySpell` | `bool` | **OK** | Same approach |
| `IsValid` | `bool` | **OK** | |
| `BaseLevel` | `uint` | **OK** | |
| `Level` | `uint` | **OK** | |
| `ManaCostPercent` | `uint` | **OK** | |
| `Id` | `int` | **OK** | |
| `Category` | `uint` | **OK** | |
| `DispelType` | `WoWDispelType` | **OK** | |
| `Mechanic` | `WoWSpellMechanic` | **OK** | |
| `MaxTargets` | `uint` | **OK** | |
| `TargetType` | `WoWCreatureType` | **OK** | |
| `SpellEffect1` | `SpellEffect` | **OK** | |
| `SpellEffect2` | `SpellEffect` | **OK** | |
| `SpellEffect3` | `SpellEffect` | **OK** | |
| `SpellEffects` | `SpellEffect[]` | **OK** | CB builds inline; HB uses separate DBC table |
| `PowerType` | `WoWPowerType` | **OK** | |
| `InternalInfo` | `SpellEntry` | **OK** | Different struct layouts (3.3.5a flat vs 4.3.4 sub-tables) as expected |
| `PowerCost` | `int` | **OK** | Both use Lua `GetSpellInfo` cache |
| `IsFunnel` | `bool` | **OK** | |
| `IsChanneled` | `bool` | **OK** | Both check `AttributesEx & 0x44` |
| `CastTime` | `uint` | **OK** | |
| `MinRange` | `float` | **OK** | |
| `MaxRange` | `float` | **OK** | |
| `MaxStackCount` | `uint` | **OK** | |
| `Name` | `string` | **OK** | CB uses `SpellDb`, HB uses class548/memory |
| `Rank` | `string` | **OK** | CB uses `SpellDb`, HB reads memory ptr |
| `Tooltip` | `string` | **OK** | |
| `Description` | `string` | **MISSING** | HB reads from `spellEntry_0.Description` ptr; CB has no `Description` |
| `Cooldown` | `bool` | **OK** | Both Lua-based |
| `CooldownTimeLeft` | `TimeSpan` | **OK** | Both use `GetSpellCooldown` Lua |
| `BaseCooldown` | `uint` | **OK** | |
| `HasRange` | `bool` | **OK** | |
| `BaseDuration` | `int` | **OK** | |
| `DurationPerLevel` | `int` | **OK** | |
| `MaxDuration` | `int` | **OK** | |
| `School` | `WoWSpellSchool` | **OK** | |
| `CanCast` | `bool` | **OK** | Both use `IsUsableSpell` Lua |
| `RangeDescription` | `string` | **OK** | |
| `CreatesItemId` | `int`/`uint` | **OK** | CB gets from EffectItemType[0]; HB iterates SpellEffects for CreateItem type |
| `SpellRangeId` | `uint` | **OK** | CB synthetic from Lua ranges; HB from SpellEntry field |

### Methods

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `GetSpellEffect(int)` | `SpellEffect` | **OK** | |
| `Cast()` | `void` | **OK** | Both call `CastSpellById(Id)` |
| `FromId(int)` | `static WoWSpell` | **OK** | Both use row cache |
| `ToString()` | `string` | **OK** | Same format string |
| `Equals(WoWSpell)` | `bool` | **OK** | ID comparison |

### Nested Types

| Member | HB | CB Status | Notes |
|--------|------|-----------|-------|
| `SpellFlyoutEntry` struct | public | **STUB** | Cata-only flight spell UI — stub OK for WotLK |
| `Flag96` struct | public | **MISSING** | Internal struct used by SpellEntry; not needed if SpellEntry works |

**WoWSpell totals: 30 OK, 1 STUB, 2 MISSING**

> `Description` is the only meaningful gap. `Flag96` is structural and unlikely to be called externally.

---

## 3. WoWAura

**Files:**  
- CB: `Styx/Logic/Combat/WoWAura.cs` (380 lines)  
- HB: `Styx/Logic/Combat/WoWAura.cs` (348 lines)

| Member | HB Type | CB Status | Notes |
|--------|---------|-----------|-------|
| `ApplyAuraType` | `WoWApplyAuraType` | **OK** | |
| `CreatorGuid` | `ulong` | **OK** | |
| `SpellId` | `int` | **OK** | |
| `Flags` | `AuraFlags` | **OK** | |
| `Duration` | `uint` | **OK** | |
| `EndTime` | `uint` | **OK** | |
| `TimeLeft` | `TimeSpan` | **OK** | Both use `PerformanceCounter()` |
| `StackCount` | `uint` | **OK** | CB uses `ushort` internally but returns `uint` |
| `Level` | `uint`/`int` | **OK** | CB returns `int`, HB `uint` — minor, compatible |
| `IsHarmful` | `bool` | **OK** | |
| `IsActive` | `bool` | **OK** | |
| `IsPassive` | `bool` | **OK** | |
| `Cancellable` | `bool` | **OK** | |
| `Name` | `string` | **OK** | |
| `Spell` | `WoWSpell` | **OK** | Both lazy-load from `FromId` |
| `ToString()` | `string` | **OK** | |
| `Equals(object)` | `bool` | **OK** | |
| `operator ==` | `bool` | **OK** | |
| `operator !=` | `bool` | **OK** | |
| `Equals(WoWAura)` | `bool` | **OK** | ID comparison |
| `GetHashCode()` | `int` | **OK** | Both use `SpellId * 397 ^ Name` |
| `AuraFlags` enum | nested enum | **OK** | All values match |

CB has these **extras** not in HB (not a problem):
- `TimeLeftMs`, `HasNoDuration`, `HasCaster`, `Rank`, `TryCancel()`, `FromAddress()`, `IEquatable<WoWAura>` interface

**WoWAura totals: 17 OK, 0 STUB, 0 MISSING** ✅

---

## 4. RoutineManager

**Files:**  
- CB: `Styx/Logic/Combat/RoutineManager.cs` (196 lines)  
- HB: `Styx/Logic/Combat/RoutineManager.cs` (166 lines)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Init()` | `static void` | **OK** | HB's is empty (init in static ctor); CB does real work |
| `Current` getter | `CombatRoutine` | **OK** | Both auto-select on first access |
| `Current` setter | `CombatRoutine` | **MISSING** | HB has `set { combatRoutine_0 = value; }` |
| `InvalidRoutineWrapper` | nested class | **STUB** | CB has `DefaultCombatRoutine` — same concept, different name/sealed |
| Auto-select from CLI | `/customclass=` | **OK** | Both parse command line |
| Auto-select by class | `WoWClass` matching | **OK** | |
| Routine selection dialog | `RoutineSelectionForm` | **MISSING** | HB shows WinForms dialog for multi-match; CB auto-picks first match |

**RoutineManager totals: 4 OK, 1 STUB, 2 MISSING**

> The `Current` setter is important — some combat routines and plugins swap the active routine at runtime.

---

## 5. CombatRoutine

**Files:**  
- CB: `Styx/Combat/CombatRoutine/CombatRoutine.cs` (135 lines)  
- HB: `Styx/Combat/CombatRoutine/CombatRoutine.cs` (381 lines)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Dispose()` | `void` | **OK** | Calls `ShutDown()` |
| `Name` | `abstract string` | **OK** | |
| `Class` | `abstract WoWClass` | **OK** | |
| `PullDistance` | `virtual double?` | **OK** | |
| `NeedRest` | `virtual bool` | **OK** | |
| `Rest()` | `virtual void` | **OK** | |
| `NeedPreCombatBuffs` | `virtual bool` | **OK** | |
| `PreCombatBuff()` | `virtual void` | **OK** | |
| `NeedPullBuffs` | `virtual bool` | **OK** | |
| `PullBuff()` | `virtual void` | **OK** | |
| `Pull()` | `virtual void` | **OK** | |
| `NeedCombatBuffs` | `virtual bool` | **OK** | |
| `CombatBuff()` | `virtual void` | **OK** | |
| `Combat()` | `virtual void` | **OK** | |
| `NeedHeal` | `virtual bool` | **OK** | |
| `Heal()` | `virtual void` | **OK** | |
| `Initialize()` | `virtual void` | **OK** | |
| `OnButtonPress()` | `virtual void` | **OK** | |
| `WantButton` | `virtual bool` | **OK** | |
| `ButtonText` | `string` | **OK** | CB: `"Settings"`, HB: `"CC Configuration"` — cosmetic |
| `Pulse()` | `virtual void` | **OK** | |
| `RestBehavior` | `virtual Composite` | **OK** | `Decorator(NeedRest, Action(Rest))` |
| `PreCombatBuffBehavior` | `virtual Composite` | **OK** | |
| `PullBuffBehavior` | `virtual Composite` | **OK** | |
| `PullBehavior` | `virtual Composite` | **STUB** | CB: simple `Action(Pull)`. HB: full logic (dismount, dead check, tagged check, blacklist, pull buffs, then Pull). Logic should live in bot, not base class — stub acceptable |
| `CombatBuffBehavior` | `virtual Composite` | **OK** | |
| `CombatBehavior` | `virtual Composite` | **OK** | |
| `HealBehavior` | `virtual Composite` | **OK** | |
| `MoveToTargetBehavior` | `virtual Composite` | **OK** | Returns `null` |
| `ShutDown()` | `virtual void` | **OK** | |
| `ToString()` | `override string` | **MISSING** | HB returns `[{Class}] {Name}` — CB has no override |
| `IDisposable` | interface | **OK** | |
| `IBehaviors` | interface | **OK** | |
| `ICombatRoutine` | interface | **OK** | |

**CombatRoutine totals: 23 OK, 1 STUB, 1 MISSING**

---

## 6. Targeting

**Files:**  
- CB: `Styx/Logic/Targeting.cs` (722 lines)  
- HB: `Styx/Logic/Targeting.cs` (965 lines)

### Properties

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `AllowEvents` | `protected bool` get/set | **MISSING** | HB uses AllowEvents gate on event add/remove; CB uses lock-free CAS |
| `DisplayTargetingExceptions` | `bool` get/set | **OK** | |
| `Instance` getter | `static Targeting` | **OK** | |
| `Instance` setter | `static Targeting` | **MISSING** | HB has `set { targeting_0 = value; }` |
| `FirstUnit` | `WoWUnit` | **OK** | |
| `TargetList` | `List<WoWUnit>` | **OK** | |
| `ObjectList` | `protected List<WoWObject>` | **OK** | |
| `MaxTargets` | `int` get/set | **OK** | |
| `IncludeWorldPlayers` | `bool` get/set | **OK** | HB always returns `true`; CB checks BG |
| `IncludeElites` | `bool` get/set | **OK** | |
| `KillBetweenHotspots` | `bool` get | **OK** | |
| `PullDistance` | `static double` | **OK** | Both check CR override, then settings |
| `PullDistanceSqr` | `static double` | **OK** | |
| `CollectionRange` | `static double` | **OK** | |

### Events

| Member | HB | CB Status | Notes |
|--------|------|-----------|-------|
| `OnTargetListUpdateFinished` | event | **OK** | |
| `IncludeTargetsFilter` | event | **OK** | CB uses CAS; HB guards with AllowEvents |
| `RemoveTargetsFilter` | event | **OK** | Same |
| `WeighTargetsFilter` | event | **OK** | Same |

### Methods

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Clear()` | `void` | **OK** | |
| `GetInitialObjectList()` | `protected virtual List<WoWObject>` | **OK** | Same BG/Player/Unit filters |
| `Pulse()` | `virtual void` | **OK** | |
| `DefaultRemoveTargetsFilter()` | `protected virtual void` | **OK** | CB simplified but covers same checks |
| `DefaultIncludeTargetsFilter()` | `protected virtual void` | **OK** | CB simplified vs HB's quest-specific entries |
| `DefaultTargetWeight()` | `protected virtual void` | **OK** | CB has full weight logic with LOS check |
| `GetAggroOnMeWithin()` | `static int` | **OK** | |
| `GetAggroWithin()` | `static int` | **OK** | |
| `IsTooNearBlackspot()` | `static bool` | **OK** | |
| `IsNotWithinHotspotRange()` | `bool` | **MISSING** | HB checks hotspot distance vs CollectionRange |
| `TargetPriority` class | nested | **OK** | Same fields |

**Targeting totals: 22 OK, 0 STUB, 3 MISSING**

> `IsNotWithinHotspotRange` is used in HB's `DefaultRemoveTargetsFilter` and is notably absent from CB.

---

## 7. RaFHelper

**Files:**  
- CB: `Styx/Logic/RaFHelper.cs` (61 lines)  
- HB: `Styx/Logic/RaFHelper.cs` (48 lines)

| Member | HB Signature | CB Status | Notes |
|--------|-------------|-----------|-------|
| `Leader` | `static WoWPlayer` | **OK** | |
| `ClearLeader()` | `static void` | **OK** | |
| `SetLeader(uint ptr)` | `static void` | **OK** | Both filter by BaseAddress |
| `SetLeader(WoWPlayer)` | `static void` | **OK** | |
| `SetLeader(ulong guid)` | `static void` | **OK** | CB calls `GetObjectByGuid`; HB wraps SetLeader(WoWPlayer) |
| `SetLeader(string keyword)` | `static void` | **OK** | Both search by name; HB uses `smethod_6` (ObjectManager helper) |

**RaFHelper totals: 6 OK, 0 STUB, 0 MISSING** ✅

---

## Priority Fixes

### High Priority (breaks combat routine compatibility)
1. **SpellManager.`HasSpell(WoWSpell)`** — Singular and custom routines call this
2. **SpellManager overloads**: `CanCast(string, bool)`, `CanCast(int)`, `CanCast(WoWSpell)` — frequently used short-form calls
3. **RoutineManager.`Current` setter** — plugins swap routines at runtime
4. **WoWSpell.`Description`** — used by tooltip/debug displays

### Medium Priority (nice to have for full compatibility)
5. **SpellManager `CastRandom`/`BuffRandom` int overloads** — 6 overloads each
6. **SpellManager 5-param `CanCast` with `accountForLagTolerance`** — advanced lag compensation
7. **CombatRoutine.`ToString()`** — logging/debug
8. **Targeting.`IsNotWithinHotspotRange()`** — filtering logic
9. **Targeting.`Instance` setter** — some bots replace targeting

### Low Priority
10. **Targeting.`AllowEvents`** — internal event gating (CB uses CAS instead)
11. **SpellManager `Spells`/`RawSpells` type** — Dictionary works, but SpellCollection has semantics
12. **SpellManager short-form convenience overloads** — 12+ `CastRandom`/`BuffRandom` wrappers
