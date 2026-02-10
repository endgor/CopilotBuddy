# CopilotBuddy — Gap Analysis vs Honorbuddy (4.3.4 / 5.4.8 / 6.2.3)

*Generated 2026-02-09 — Single comprehensive report*

---

## 1. HB API Evolution Summary (4.3.4 → 5.4.8 → 6.2.3)

| Area | 4.3.4 (Cata) | 5.4.8 (MoP) | 6.2.3 (WoD) |
|------|-------------|-------------|-------------|
| **Architecture** | Synchronous (TreeSharp ticks) | Hybrid — `CoroutineTask` introduced | Full coroutine, `ActionRunCoroutine` replaces many Actions |
| **Namespace layout** | `Styx/Logic/*`, `Styx/Helpers/*` | Reorganized: `Styx/CommonBot/*`, `Styx/Common/*` | Same as 5.4.8 + `Styx/Helpers/` for settings |
| **SpellManager** | Under `Styx/Logic/Combat/` | Moved to `Styx/CommonBot/SpellManager.cs` | Same as 5.4.8 |
| **TreeRoot/BotBase** | Under `Styx/Logic/BehaviorTree/` & `Styx/` | Moved to `Styx/CommonBot/` | Same |
| **Frames** | Under `Styx/Logic/Inventory/Frames/` | Moved to `Styx/CommonBot/Frames/` | Same + `GuildBankFrame`, `GarrisonMissionFrame` |
| **DBC tables** | 6 files (AreaTable, LfgDungeons, Map, MapDifficulty) | 21 files (added Faction, SpellEffect, SkillLine, ItemRandom, TaxiNodes, etc.) | 25 files + 43 DB2 files (Garrison, Scenarios, Vehicles, etc.) |
| **Blacklist** | Under `Styx/Logic/Blacklist.cs` | Moved to `Styx/CommonBot/Blacklist.cs` with `BlacklistFlags` | Same |
| **CharacterMgmt** | Basic (AutoEquipper, CharacterManager) | Added `TalentSelector`, `TalentPlacement`, `ClassProfile`, `WeaponStyle` | Same |
| **ActionBars** | Not present | `CommonBot/Bars/` (6 files: ActionBar, ActionButton, SpellActionButton) | Same |
| **TreeSharp** | Standalone `TreeSharp/` folder | Under `Styx/TreeSharp/` — adds `Sleep`, `WhileLoop`, `DynamicChildSelector`, `ProbabilitySelector` | Same |
| **Combat Routines** | Under `Styx/Combat/CombatRoutine/` | Moved to `Styx/CommonBot/Routines/` — adds `CapabilityManager`, `InvalidRoutineWrapper` | Same |
| **Avoidance** | Not present (DungeonBuddy only) | `Styx/Pathing/Avoidance/` (17 files) | Same |
| **Bots** | Grind, Quest, DungeonBuddy, BGBuddy, Gatherbuddy, Instancebuddy, ArchaeologyBuddy, PartyBot | Same + Professionbuddy, GrindBuddy2 | Same + GarrisonBuddy |
| **WoWGuid** | 64-bit ulong | Still ulong | 128-bit struct (`WoWGuid.cs`) |
| **WoWPoint** | `Styx/Logic/Pathing/WoWPoint.cs` | Moved to `Styx/WoWPoint.cs` (root) | Same |
| **XmlEngine** | Not present | `Styx/XmlEngine/` (4 files) | Same |
| **Localization** | Not present | `Styx/Localization/` (2 files) | Same |

**CopilotBuddy follows the 4.3.4 namespace layout**, which is correct per instructions. The reorganizations in 5.4.8+ are NOT needed.

---

## 2. Missing Files / Classes

### Critical — Will block bot operation

| Class | Purpose | HB Version(s) | WotLK? | Priority |
|-------|---------|---------------|--------|----------|
| `SpellCollection` | Typed wrapper for known spells, replaces Dict | 4.3.4+ | Yes | **Critical** |
| `SpellCooldownInfo` | Struct for spell CD tracking (start, duration, GCD) | 4.3.4+ | Yes | **Critical** |
| `AuctionFrame` | Auction house interaction: browse, bid, buyout, create | 4.3.4 (Misc/) | Yes | **High** |
| `AuctionHouse` | AH manager: search, filter, item listing | 4.3.4 (Misc/) | Yes | **High** |
| `WoWAuction` | Single auction entry data object | 4.3.4 (Misc/) | Yes | **High** |
| `ForceMailManager` | Forced mail items (profiles specify items to always mail) | 4.3.4 | Yes | **High** |

### High — Combat routines & plugins need these

| Class | Purpose | HB Version(s) | WotLK? | Priority |
|-------|---------|---------------|--------|----------|
| `RuneType` | DK rune type enum (Blood, Frost, Unholy, Death) | 4.3.4+ | Yes | **High** |
| `BagType` | Bag type enum (Unspecified, Quiver, AmmoPouch, etc.) | 4.3.4+ | Yes | **High** |
| `WoWGlyphInfo` | Glyph socket info (type, spellId, name) | 4.3.4+ | Yes (WotLK has 6 glyph slots) | **High** |
| `SpecType` | Spec type enum for talent spec identification | 4.3.4+ | Stub (WotLK has trees, not specs) | **Medium** |
| `ReputationFlags` | Rep standing flags (AtWar, Hidden, Inactive, etc.) | 4.3.4+ | Yes | **High** |
| `EmoteState` | Emote state enum (Stand, Sit, Sleep, Dance, etc.) | 4.3.4+ | Yes | **Medium** |
| `GameState` | Game state machine enum (LoggingIn, CharSelect, InGame) | 4.3.4+ | Yes | **Medium** |
| `WoWGameObjectState` | GO state enum (Active, Ready, ActiveAlternative) | 4.3.4+ | Yes | **High** |

### Medium — Useful but not blocking

| Class | Purpose | HB Version(s) | WotLK? | Priority |
|-------|---------|---------------|--------|----------|
| `AreaTable` (DBC) | Zone/subzone lookup from DBC | 4.3.4+ | Yes | **Medium** |
| `LfgDungeons` (DBC) | LFG dungeon database | 4.3.4+ | Yes (WotLK has LFD) | **Medium** |
| `Stable` / `StabledPet` | Pet stable system (Hunter pets) | 4.3.4 | Yes | **Medium** |
| `WoWArenaTeamInfo` | Arena team data | 4.3.4+ | Yes | **Low** |
| `WoWInebriationLevel` | Drunk state enum | 4.3.4+ | Yes | **Low** |
| `DurabilityCostEntry/QualityEntry` | Durability repair cost calculation | 4.3.4+ | Yes | **Low** |
| `GlueScreen` | Login/loading screen enum | 4.3.4+ | Yes | **Low** |
| `GraphicsApi` | Graphics backend enum (D3D9, D3D11, OpenGL) | 4.3.4+ | Stub (WotLK = D3D9 only) | **Low** |
| `Guard` | Argument validation helper | 4.3.4+ | Yes | **Low** |
| `PvPState` | PvP flag state enum | 4.3.4+ | Yes | **Medium** |
| `ActivitySetter` | IDisposable status text setter | 4.3.4+ | Yes | **Low** |
| `LuaState` | Lua VM state wrapper | 4.3.4+ | Yes | **Low** |
| `WoWCurrencyType` | Currency type enum | 4.3.4+ | Stub (WotLK uses items) | **Low** |

### Navigation interfaces (already work via Navigator, but interfaces missing)

| Class | Purpose | HB Version(s) | WotLK? | Priority |
|-------|---------|---------------|--------|----------|
| `INavigationProvider` | Abstraction for navigation backend | 4.3.4+ | Yes | **Low** |
| `IPlayerMover` | Abstraction for movement method | 4.3.4+ | Yes | **Low** |
| `MeshNavigator` | Mesh-based INavigationProvider impl | 4.3.4+ | Yes | **Low** |
| `MeshMovePath` | Path segment data for navigator | 4.3.4+ | Yes | **Low** |
| `ClickToMoveMover` | CTM-based IPlayerMover impl | 4.3.4 | Yes | **Low** |

### Entire Bot Systems (not in CopilotBuddy)

| Bot | Purpose | Status | Priority |
|-----|---------|--------|----------|
| **DungeonBuddy** | Automated LFD dungeon running | PLAN exists (`PLAN_DUNGEONBUDDY.md`) — ~50 files to create | **High** (planned) |
| **BGBuddy** | Automated battleground PvP | Not planned | **Low** |
| **Instancebuddy** | LFG queue manager | Mostly replaced by DungeonBuddy plan | **Low** |
| **PartyBot** | Follow/assist party leader | Not planned | **Low** |
| **Professionbuddy** | TradeSkill automation | Not present in HB 4.3.4 | **N/A** |
| **ArchaeologyBuddy** | Archaeology automation | WoD+, doesn't exist in WotLK | **N/A** |
| **GarrisonBuddy** | Garrison management | WoD+, doesn't exist in WotLK | **N/A** |

### Systems from 5.4.8/6.2.3 (NOT needed)

| System | Why not needed |
|--------|---------------|
| `Styx/CommonBot/Coroutines/` | CopilotBuddy is synchronous by design |
| `Styx/CommonBot/Bars/` (ActionBar system) | Nice-to-have but no bot currently needs it |
| `Styx/XmlEngine/` | Profile system already works without it |
| `TreeHooks` | 6.2.3 hook system; CopilotBuddy uses direct `TreeRoot` |
| `Buddy/Overlay/` | HB-specific overlay UI |
| `Buddy/Auth/`, `Buddy/Store/` | No auth system |
| `Styx/Localization/` | Not needed |
| All WoD DB2 files | Wrong expansion |
| `WoWGuid` 128-bit struct | WotLK uses 64-bit ulong |
| `WoWAreaTrigger` object + shapes | WoD only |
| `Garrison*` classes | WoD only |
| `WoWVehicle` (6.2.3 version) | WotLK has basic vehicle, but 6.2.3's is completely different |

---

## 3. Incomplete Files (exist but missing methods/properties)

### WoWUnit.cs — ~60% unported (HB: 4048 lines, CB: 1686 lines)

**Missing properties** (all exist in HB 4.3.4, all WotLK-compatible):

| Property | Type | Purpose |
|----------|------|---------|
| `IsTargetingMeOrPet` | `bool` | Unit is targeting player or player's pet |
| `Attackable` | `bool` | Can be attacked (checks UnitFlags) |
| `PvpFlagged` | `bool` | Has PvP flag |
| `CanSelect` | `bool` | Can be selected (not unselectable) |
| `CurrentCastTimeLeft` | `TimeSpan` | Time left on current cast |
| `ChannelTimeLeft` | `TimeSpan` | Time left on current channel |
| `CastingSpell` | `WoWSpell` | The spell being cast |
| `ChanneledCastingSpell` | `WoWSpell` | The spell being channeled |
| `AttackPower` | `int` | Melee attack power |
| `RangedAttackPower` | `int` | Ranged attack power  |
| `MinDamage` / `MaxDamage` | `float` | Weapon damage range |
| `MinOffHandDamage` / `MaxOffHandDamage` | `float` | Off-hand damage |
| `MinRangedDamage` / `MaxRangedDamage` | `float` | Ranged damage |
| `IsTotem` | `bool` | Is a totem object |
| `IsUndead` | `bool` | Is undead creature type |
| `IsBeast` | `bool` | Is beast creature type |
| `IsMechanical` | `bool` | Is mechanical creature type |
| `IsHumanoid` | `bool` | Is humanoid creature type |
| `IsElemental` | `bool` | Is elemental creature type |
| `IsDragonkin` | `bool` | Is dragonkin creature type |
| `GotAlivePet` | `bool` | Has a living pet out |
| `PetGuid` | `ulong` | GUID of active pet |
| `IsFlying` | `bool` | Currently flying |
| `IsSwimming` | `bool` | Currently swimming |
| `IsMoving` | `bool` | Currently moving |
| `HasAuraWithMechanic(WoWSpellMechanic)` | `bool` | Check for CC mechanic |
| `Stunned` / `Rooted` / `Silenced` / `Fleeing` / `Disarmed` | `bool` | CC state checks |
| `IsCrowdControlled` | `bool` | Any CC active |
| `KnownSpells` | `ReadOnlyCollection<WoWSpell>` | All known spells from DBC |
| `HealthPercent` calculation override | — | HB uses `CurrentHealth * 100.0 / MaxHealth` with double precision |
| `Shapeshift` | `ShapeshiftForm` | Current shapeshift form |
| `IsInMyPartyOrRaid` | `bool` | Convenience for party OR raid |

### WoWPlayer.cs — ~70% unported (HB: 1207 lines, CB: 347 lines)

**Missing properties**:

| Property | Type | Purpose |
|----------|------|---------|
| `BlockPercent` | `float` | Block chance % |
| `DodgePercent` | `float` | Dodge chance % |
| `ParryPercent` | `float` | Parry chance % |
| `CritPercent` | `float` | Crit chance % (melee) |
| `RangedCritPercent` | `float` | Ranged crit % |
| `SpellCritPercent` | `float` | Spell crit % |
| `IsAlive` override | `bool` | HB overrides WoWUnit's — checks ghost aura too |
| `IsGhost` | `bool` | Has ghost aura |
| `IsHorde` / `IsAlliance` | `bool` | Faction check by race |
| `Expertise` / `SpellPenetration` | `int` | Combat stats |
| `Resilience` | `int` | PvP resilience |
| `ArmorPenetration` | `float` | ArP rating |
| `HastePercent` | `float` | Haste % |

### WoWObject.cs — Missing distance helpers

| Method | Purpose |
|--------|---------|
| `DistanceSqr` | Squared distance (avoids sqrt — perf) |
| `Distance2D` | 2D XY distance |
| `Distance2DSqr` | Squared 2D distance |

### SpellManager.cs — Missing convenience methods

**Entire method families missing:**

| Family | Count | Purpose |
|--------|-------|---------|
| `CanBuff(...)` | 12 overloads | `CanCast()` + no-aura-check combined |
| `Buff(...)` | 8 overloads | `Cast()` only if target lacks aura |
| `CastRandom(...)` | 9 overloads | Cast random spell from list |
| `BuffRandom(...)` | 9 overloads | Buff random spell from list |
| `Cast(int)`, `Cast(int, WoWUnit)`, `Cast(WoWSpell, WoWUnit)` | 3 | Missing overloads |
| Various `CanCast(...)` overloads | ~5 | Int-based, WoWSpell-based, lag-tolerance |
| `HasSpell(WoWSpell)` | 1 | Takes WoWSpell object instead of string/int |

**Total: ~47 missing SpellManager overloads.**

### RoutineManager.cs — Partial

| Missing | Purpose |
|---------|---------|
| `AutoSelectClass` CLI arg support | `/customclass=ClassName` argument |
| Multi-routine selection dialog | When multiple CRs match class, show picker |
| `LegacySpellManager.Refresh()` call | Refresh spell cache on routine change |
| Thread-safe initialization | `lock` around init vs basic flag |

### LootFrame.cs — Wrong signature

| Method | CopilotBuddy | HB 4.3.4 |
|--------|-------------|-----------|
| `LootInfo(...)` | `string LootInfo(out bool locked)` | `LootSlotInfo LootInfo(int slot)` |

### MerchantFrame.cs — Missing overloads

| Missing | Purpose |
|---------|---------|
| `BuyItem(string name, int amount)` | Buy by item name |
| `BuyItem` return type | Should return `bool` not `void` |

### MailFrame.cs — Missing properties

| Missing | Purpose |
|---------|---------|
| `SendMailItemGuids` | GUIDs of items attached to outgoing mail |
| `SendMailItems` | `WoWItem[]` of attached items |

---

## 4. Stub Methods That Should Be Real

| Priority | File | Method | Returns | Should Do |
|----------|------|--------|---------|-----------|
| **Critical** | `WoWGameObject.cs` | `LockRecord` | `null` | Read lock entry from `ClientDb.Lock` DBC → determine if GO is locked/unlocked. **Blocks herb/mineral detection.** |
| **Critical** | `WoWGameObject.cs` | `GetDataSlot(uint, out int)` | `false, 0` | Read GO cache data slots. **Blocks GO interaction logic.** |
| **Critical** | `Quest.cs` | `GetData(out QuestDescriptorData)` | `false` | Read quest descriptors from player memory. **Blocks quest progress tracking.** |
| **High** | `Battlegrounds.cs` | `GetCurrentBattleground()` | `BattlegroundType.None` | Read BG type from memory. **Needed for any BG logic.** |
| **Medium** | `WoWPartyMember.cs` | `AreaTableId` | `0` | Read party member's zone ID from group info struct. |
| **Medium** | `WoWBag.cs` | `Name` (non-backpack) | `"Unknown"` | Look up bag's `WoWContainer` entry for its real name. |
| **Low** | `AreaManager.cs` | `SetAreaByIndex()` | no-op | Implement polygon area index selection. |
| **Low** | `QuestArea.cs` | `Triangulate()` | fan only | Ear-clipping for concave polygons. |
| **Low** | `BlackspotManager.cs` | Tile event subscription | missing | Subscribe to Navigation.dll tile load events. |
| **Low** | `Connection.cs` | `PATHDISTANCE` function | Euclidean | Use `Navigator.PathDistance` for DB queries. |

---

## 5. Bugs & Incorrect Ports

| Severity | File | Issue | Correct Behavior (HB 4.3.4) |
|----------|------|-------|------------------------------|
| **Critical** | `WoWSpell.cs` | `IsChanneled` checks `Attributes` field | Should check `AttributesEx` (field index 1, mask `0x44`). Currently misidentifies channeled spells. |
| **High** | `SpellManager.cs` | `Cast(string, WoWUnit)` targets unit first, then casts by name | HB casts by spell ID with target GUID directly. Target-first approach can fail in multi-target. |
| **High** | `SpellManager.cs` | `Cast(WoWSpell)` casts on self | HB defaults to `CurrentTarget`. Self-cast means AoE and offensive spells won't work. |
| **High** | `SpellManager.cs` | `RawSpells` returns same dict as `Spells` | HB: `RawSpells` = `ReadOnlyCollection<WoWSpell>` from `Me.KnownSpells`. CRs enumerating `RawSpells` get wrong data. |
| **High** | `SpellManager.cs` | `CanCast()` missing lag tolerance | HB allows casting if CD/cast time remaining < `2 × latency`. Missing this causes CRs to wait too long between casts. |
| **Medium** | `WoWSpell.cs` | `SpellRangeId` returns synthetic values (1/2/3) | Should return real DBC range IDs. Code checking specific range IDs > 3 breaks. |
| **Medium** | `LootFrame.cs` | `LootInfo` returns `string` | Should return `LootSlotInfo` object. External code calling this gets wrong type. |

---

## 6. Recommended Porting Order

### Phase 1 — Fix critical bugs (1-2 days)

These are wrong RIGHT NOW and affect all combat routines:

1. **Fix `WoWSpell.IsChanneled`** — change `Attributes` → `AttributesEx`
2. **Fix `SpellManager.Cast(WoWSpell)`** — default target to `Me.CurrentTarget`
3. **Fix `SpellManager.Cast(string, WoWUnit)`** — cast by ID with GUID, not target-first
4. **Fix `SpellManager.RawSpells`** — return `ReadOnlyCollection` from `Me.KnownSpells`
5. **Add `CanCast` lag tolerance** — allow casting when CD < 2× latency
6. **Fix `LootFrame.LootInfo`** — return `LootSlotInfo` with proper slot info

### Phase 2 — Critical stubs → real implementations (3-5 days)

These block entire subsystems:

7. **`WoWGameObject.LockRecord`** — implement DBC lock lookup (blocks GatherBuddy herb/ore detection)
8. **`WoWGameObject.GetDataSlot`** — implement cache data slot reads
9. **`Quest.GetData`** — implement quest descriptor reads (blocks quest progress in QuestBot)
10. **`WoWGameObjectState` enum** — needed for GO interaction checks

### Phase 3 — SpellManager completeness (2-3 days)

Combat routines need these to compile:

11. **Add `CanBuff` family** (12 overloads) — `CanCast + !HasAura` pattern
12. **Add `Buff` family** (8 overloads) — `CanBuff + Cast` pattern
13. **Add `Cast(int)`, `Cast(int, WoWUnit)`, `Cast(WoWSpell, WoWUnit)`** overloads
14. **Add remaining `CanCast` overloads** (int-based, WoWSpell-based)
15. **Add `HasSpell(WoWSpell)`** overload
16. **Add `SpellCollection`** class
17. **Add `SpellCooldownInfo`** struct

### Phase 4 — WoWUnit/WoWPlayer completeness (3-5 days)

Combat routines and targeting depend on these:

18. **CC state properties** — `Stunned`, `Rooted`, `Silenced`, `Fleeing`, `IsCrowdControlled`, `HasAuraWithMechanic`
19. **Creature type checks** — `IsTotem`, `IsUndead`, `IsBeast`, `IsMechanical`, etc.
20. **Cast state** — `CurrentCastTimeLeft`, `CastingSpell`, `ChanneledCastingSpell`, `ChannelTimeLeft`
21. **Combat stats** — `AttackPower`, `MinDamage/MaxDamage`, `RangedAttackPower`
22. **Pet helpers** — `GotAlivePet`, `PetGuid`
23. **Movement flags** — `IsFlying`, `IsSwimming`, `IsMoving`
24. **Targeting helpers** — `IsTargetingMeOrPet`, `Attackable`, `CanSelect`, `PvpFlagged`
25. **WoWPlayer stats** — `BlockPercent`, `DodgePercent`, `ParryPercent`, `CritPercent`, `IsAlive` override
26. **WoWObject distance** — `DistanceSqr`, `Distance2D`, `Distance2DSqr`

### Phase 5 — Missing enums & types (1-2 days)

27. **`RuneType`** — DK rune enum (Blood/Frost/Unholy/Death)
28. **`BagType`** — bag type enum
29. **`WoWGlyphInfo`** — glyph info struct
30. **`ReputationFlags`** — rep flags enum
31. **`EmoteState`** — emote enum
32. **`GameState`** — game state enum
33. **`PvPState`** — PvP state enum
34. **Missing root exceptions** — `InvalidExecutorException`, `InvalidObjectPointerException`

### Phase 6 — Auction House system (2-3 days)

35. **`AuctionFrame`** — browse/bid/buyout/create UI frame
36. **`AuctionHouse`** — AH manager
37. **`WoWAuction`** — auction entry data

### Phase 7 — Minor gaps (1-2 days)

38. **`ForceMailManager`** — profile-driven forced mail
39. **`Stable` / `StabledPet`** — Hunter pet stable
40. **`Battlegrounds.GetCurrentBattleground()`** — detect active BG
41. **`WoWPartyMember.AreaTableId`** — party member zone
42. **`WoWBag.Name`** for non-backpack bags
43. **`RoutineManager`** — multi-CR selection dialog, CLI arg support
44. **`MerchantFrame`** — missing `BuyItem(string, int)`, fix return type
45. **`MailFrame`** — `SendMailItemGuids` / `SendMailItems`

### Phase 8 — DungeonBuddy (follow PLAN_DUNGEONBUDDY.md)

46. Implement the ~50 files described in the plan

---

## Systems Confirmed Complete (no action needed)

| System | Status |
|--------|--------|
| ObjectManager | ✅ Complete (missing only `GetAnyObjectByGuid` — minor) |
| Lua / LuaEvents | ✅ Complete |
| WoWMovement | ✅ Complete |
| Navigator / Flightor | ✅ Complete |
| GameWorld (TraceLine) | ✅ Complete |
| WoWCamera | ✅ Complete |
| WoWFaction / WoWFactionTemplate | ✅ Complete |
| WoWChat | ✅ Complete |
| WoWPaperDoll | ✅ Complete |
| SpellDb | ✅ Complete |
| Mount / MountHelper | ✅ Complete |
| Targeting | ✅ Complete |
| GossipFrame | ✅ Complete |
| QuestFrame | ✅ Complete |
| TaxiFrame | ✅ Complete |
| TrainerFrame | ✅ Complete |
| ProfileManager + quest nodes | ✅ Complete |
| AreaManagement | ✅ Complete (minor polygon issue) |
| BotPoi system | ✅ Complete |
| TreeSharp behavior tree | ✅ Complete |
| WoWAura / WoWAuraCollection | ✅ Complete (better than HB) |
| InventoryManager | ✅ Complete |
| PluginManager | ✅ Complete |
| Settings / Logging / Helpers | ✅ Complete |
| Database (SQLite) | ✅ Complete |
| CommonBehaviors (Actions + Decorators) | ✅ Complete |
| GrindBot / QuestBot | ✅ Complete |
| GatherBuddy | ✅ Complete |

---

## Estimated Total Effort

| Phase | Estimate | Impact |
|-------|----------|--------|
| Phase 1 — Bug fixes | 1-2 days | **Fixes all CR issues** |
| Phase 2 — Critical stubs | 3-5 days | **Unblocks GatherBuddy + QuestBot** |
| Phase 3 — SpellManager | 2-3 days | **CRs can compile** |
| Phase 4 — WoWUnit/WoWPlayer | 3-5 days | **Full CR feature set** |
| Phase 5 — Enums/types | 1-2 days | **API completeness** |
| Phase 6 — Auction House | 2-3 days | **Economy features** |
| Phase 7 — Minor gaps | 1-2 days | **Polish** |
| Phase 8 — DungeonBuddy | 5-10 days | **Dungeon automation** |
| **Total** | **~18-32 days** | |
