# API Surface Audit: CopilotBuddy vs HB 4.3.4

> **Generated**: Read-only analysis comparing public API surfaces.  
> **Scope**: WoWObject, WoWUnit, WoWPlayer, LocalPlayer, WoWItem, WoWGameObject  
> **Legend**: ❌ Missing — 🔶 Stub — ⚠️ Incorrect/Mismatch

---

## 1. WoWObject

### ❌ Missing from CopilotBuddy

| Property/Method | HB 4.3.4 Signature | Notes |
|---|---|---|
| `InteractRangeSqr` | `float InteractRangeSqr { get; }` | Returns `InteractRange * InteractRange`; used in distance-squared checks |
| `RelativeLocation` | `virtual WoWPoint RelativeLocation { get; }` | Position relative to transport/parent; WoWGameObject overrides this |
| `ToDynamicObject()` | `WoWDynamicObject ToDynamicObject()` | Cast helper (DynamicObject type exists in 3.3.5) |
| `GetWorldMatrix()` | `virtual Matrix GetWorldMatrix()` | Returns world transform matrix via vtable call |
| `DistanceSqr` | `virtual double DistanceSqr { get; }` | Squared distance (avoids sqrt) |
| `Distance2D` | `virtual double Distance2D { get; }` | 2D (XY) distance |
| `Distance2DSqr` | `virtual double Distance2DSqr { get; }` | Squared 2D distance |

### ⚠️ Potential Issues

| Item | Issue |
|---|---|
| `X`, `Y`, `Z` | CopilotBuddy declares these `virtual`; HB 4.3.4 has them non-virtual (derived from `Location`). Minor but could cause unexpected override behavior. |
| `Location` | HB uses a vtable call (offset-based); CopilotBuddy constructs from X/Y/Z fields. Functionally equivalent for units but may diverge for GameObjects/DynamicObjects. |

---

## 2. WoWUnit

### ❌ Missing from CopilotBuddy

#### Cast Timing
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `CurrentCastStartTime` | `DateTime CurrentCastStartTime` | **High** — CRs use this |
| `CurrentCastEndTime` | `DateTime CurrentCastEndTime` | **High** — CRs use this |
| `CurrentCastTimeLeft` | `TimeSpan CurrentCastTimeLeft` | **High** — CRs use this |
| `CastingSpell` | `WoWSpell CastingSpell { get; }` | **High** — Returns WoWSpell object, not just ID |
| `ChannelObject` | Reference to channel target | Medium |

#### Power System (PowerInfo struct)
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `GetPowerInfo(WoWPowerType)` | `PowerInfo GetPowerInfo(type)` | Medium — Structured power data |
| `GetPowerRegenFlat(WoWPowerType)` | `float` | Low |
| `GetPowerRegenInterrupted(WoWPowerType)` | `float` | Low |
| `GetPowerCostModifier(WoWPowerType)` | `uint` | Low |
| `GetPowerCostMultiplier(WoWPowerType)` | `float` | Low |
| `ManaInfo` | `PowerInfo ManaInfo` | Low — Convenience wrapper |
| `RageInfo` | `PowerInfo RageInfo` | Low |
| `EnergyInfo` | `PowerInfo EnergyInfo` | Low |
| `FocusInfo` | `PowerInfo FocusInfo` | Low |
| `RunicPowerInfo` | `PowerInfo RunicPowerInfo` | Low |
| `HappinessInfo` | `PowerInfo HappinessInfo` | Low |
| `PowerInfo` struct | struct with Type, Current, Max, Percent, Regen, Cost fields | Medium |

#### Combat Stats
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `AttackPower` | `uint AttackPower` | **High** |
| `AttackPowerMultiplier` | `float AttackPowerMultiplier` | Medium |
| `RangedAttackPower` | `uint RangedAttackPower` | **High** |
| `RangedAttackPowerMultiplier` | `float RangedAttackPowerMultiplier` | Medium |
| `BaseAttackTime` | `uint BaseAttackTime` | Medium |
| `BaseOffHandAttackTime` | `uint BaseOffHandAttackTime` | Low |
| `BaseRangedAttackTime` | `uint BaseRangedAttackTime` | Low |
| `MinDamage` | `uint MinDamage` | **High** |
| `MaxDamage` | `float MaxDamage` | **High** |
| `MinOffHandDamage` | `float MinOffHandDamage` | Medium |
| `MaxOffHandDamage` | `float MaxOffHandDamage` | Medium |
| `MinRangedDamage` | `float MinRangedDamage` | Medium |
| `MaxRangedDamage` | `float MaxRangedDamage` | Medium |
| `BaseHealth` | `uint BaseHealth` | Low |
| `MaxHealthModifier` | `float MaxHealthModifier` | Low |
| `CastSpeedModifier` | `float CastSpeedModifier` | Medium |

#### Stat Modifiers (Positive/Negative)
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `StrengthPositiveModifier` | `uint` | Low |
| `StrengthNegativeModifier` | `uint` | Low |
| `AgilityPositiveModifier` | `uint` | Low |
| `AgilityNegativeModifier` | `uint` | Low |
| `StaminaPositiveModifier` | `uint` | Low |
| `StaminaNegativeModifier` | `uint` | Low |
| `IntellectPositiveModifier` | `uint` | Low |
| `IntellectNegativeModifier` | `uint` | Low |
| `SpiritPositiveModifier` | `uint` | Low |
| `SpiritNegativeModifier` | `uint` | Low |

> CopilotBuddy has `StrengthBonus`, `AgilityBonus` etc. (single modifier) — HB splits into positive/negative.

#### Descriptor Properties
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `RenderFacing` | `float RenderFacing` | Low |
| `NativeDisplayId` | `uint NativeDisplayId` | Medium |
| `MountDisplayId` | `uint MountDisplayId` | Medium |
| `HoverHeight` | `float HoverHeight` | Low |
| `CreatedBySpellId` | `uint CreatedBySpellId` | Low |
| `Flags` (raw uint) | `uint Flags` | Already exists internally |
| `Flags2` (raw uint) | `uint Flags2` | Already exists internally |
| `DynamicFlags` (raw uint) | `uint DynamicFlags` | Already exists internally |
| `NpcFlags` (raw uint) | `uint NpcFlags` | Already exists internally |
| `NpcEmoteState` | `EmoteState NpcEmoteState` | Low |
| `AuraState` | `uint AuraState` | Medium |
| `PvPState` | `PvPState PvPState` | Low |
| `SheathType` | `SheathType SheathType` (internal) | Low |
| `MaxItemLevel` | `int MaxItemLevel` | Low (Cata-specific) |
| `VirtualItemSlotIds` | `uint[] VirtualItemSlotIds` (3 slots) | Low |

#### Pet Properties
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `PetNumber` | `uint PetNumber` | Low |
| `PetNameTimestamp` | `uint PetNameTimestamp` | Low |
| `PetExperience` | `uint PetExperience` | Low |
| `PetNextLevelExperience` | `uint PetNextLevelExperience` | Low |
| `IsPet` | `bool IsPet` | Medium |
| `GotAlivePet` | `bool GotAlivePet` | **High** — CRs use this |
| `PetInCombat` | `bool PetInCombat` | Medium |
| `PetAggro` | `bool PetAggro` | Medium |

#### Targeting Helpers
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `IsTargetingMeOrPet` | `bool IsTargetingMeOrPet` | **High** |
| `IsTargetingAnyMinion` | `bool IsTargetingAnyMinion` | Medium |
| `IsTargetingPet` | `bool IsTargetingPet` | Medium |
| `IsTargetingMyPartyMember` | `bool IsTargetingMyPartyMember` | **High** |
| `IsTargetingMyRaidMember` | `bool IsTargetingMyRaidMember` | Medium |

#### Unit Flags & Booleans
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `Fleeing` | `bool Fleeing` | Medium |
| `Pacified` | `bool Pacified` | Medium |
| `Dazed` | `bool Dazed` | Low |
| `Disarmed` | `bool Disarmed` | Medium |
| `Attackable` | `bool Attackable` | **High** |
| `PvpFlagged` | `bool PvpFlagged` | **High** |
| `CanSelect` | `bool CanSelect` | **High** |
| `Possessed` | `bool Possessed` | Low |
| `Elite` | `bool Elite` | Medium |
| `Looting` | `bool Looting` | Low |
| `OnTaxi` | `bool OnTaxi` | Medium |
| `PlayerControlled` | `bool PlayerControlled` | Medium |
| `RafLinked` | `bool RafLinked` | Low |
| `TappedByAllThreatLists` | `bool TappedByAllThreatLists` | Low |
| `IsAutoAttacking` | `bool IsAutoAttacking` | Medium |
| `IsPlayer` | `bool IsPlayer` | Medium (can use `Type == Player`) |
| `IsUnit` | `bool IsUnit` | Low |
| `IsAlive` | `virtual bool IsAlive` | **High** — exists? check |
| `KilledByMe` | `bool KilledByMe` | Medium |
| `IsNeutral` | `bool IsNeutral` | Medium |

#### Creature Type Checks
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `IsBeast` | `bool IsBeast` | Medium |
| `IsCritter` | `bool IsCritter` | Medium |
| `IsDemon` | `bool IsDemon` | Medium |
| `IsDragon` | `bool IsDragon` | Low |
| `IsElemental` | `bool IsElemental` | Low |
| `IsGasCloud` | `bool IsGasCloud` | Low |
| `IsGiant` | `bool IsGiant` | Low |
| `IsHumanoid` | `bool IsHumanoid` | Medium |
| `IsMechanical` | `bool IsMechanical` | Low |
| `IsNonCombatPet` | `bool IsNonCombatPet` | Medium |
| `IsTotem` | `bool IsTotem` | **High** — CRs use this |
| `IsUndead` | `bool IsUndead` | Medium |
| `IsGhostVisible` | `bool IsGhostVisible` | Low |
| `IsExotic` | `bool IsExotic` | Low |

#### Other
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `Faction` | `WoWFaction Faction` (from `WoWFaction.FromId`) | Medium |
| `FactionTemplate` | `WoWFactionTemplate FactionTemplate` | Medium |
| `CreatureFamilyInfo` | `CreatureFamily CreatureFamilyInfo` | Low |
| `SubName` | `string SubName` | Low |
| `MyAggroRange` | `float MyAggroRange` | Medium |
| `MyStealthDetectionRange` | `float MyStealthDetectionRange` | Low |
| `Behind(WoWUnit)` | `bool Behind(WoWUnit obj)` | Medium |

### ⚠️ Type Mismatches

| Property | CopilotBuddy | HB 4.3.4 | Impact |
|---|---|---|---|
| `GetCurrentPower()` | returns `int` | returns `uint` | **Medium** — negative values impossible for power, but `int` is safe for WotLK |
| `GetMaxPower()` | returns `int` | returns `uint` | Same as above |
| `Armor` | `int Armor` | `uint Armor` | Low — negative armor shouldn't occur |
| `ResistHoly` etc. | `int` | `uint` | Low |
| `Strength` etc. | `int` | `uint` | Low |
| `CurrentTargetGuid` | Check type | `ulong` | Verify |

### 🔶 CopilotBuddy Properties That Look Correct
The following exist in both and appear correctly implemented:
- Health/Mana/Rage/Energy/Focus/Happiness/RunicPower (current, max, percent)
- Level, FactionId, Race, Class, Gender, PowerType
- Shapeshift, StateFlag, Combat, Skinnable, Stunned, Silenced, Rooted
- NPC flags (CanGossip, IsQuestGiver, IsTrainer, etc.)
- Dynamic flags (TaggedByMe, TaggedByOther, Lootable, Dead/IsDead)
- Casting (CastingSpellId, ChannelSpellId, IsCasting, IsChanneling)
- Relationships (CharmedBy, SummonedBy, OwnedByUnit, ControllingPlayer)
- BoundingRadius, CombatReach, MeleeReach, DisplayId
- Auras system, Reaction system, Threat system
- Face(), Target(), InLineOfSight, InLineOfSpellSight

---

## 3. WoWPlayer

### ❌ Missing from CopilotBuddy

#### Appearance
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `SkinType` | `byte` (PlayerBytes1) | Low |
| `FaceType` | `byte` | Low |
| `HairStyle` | `byte` | Low |
| `HairColor` | `byte` | Low |
| `FacialHair` | `byte` | Low |

#### Guild
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `GuildRank` | `uint GuildRank` | Low |
| `GuildTimestamp` | `uint GuildTimestamp` | Low |

#### Combat Stats
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `BlockPercent` | `float BlockPercent` | **High** |
| `DodgePercent` | `float DodgePercent` | **High** |
| `ParryPercent` | `float ParryPercent` | **High** |
| `CritPercent` | `float CritPercent` | **High** |
| `RangedCritPercent` | `float RangedCritPercent` | Medium |
| `OffHandCritPercent` | `float OffHandCritPercent` | Low |
| `Expertise` | `float Expertise` | Medium |
| `ExpertiseOffHand` | `float ExpertiseOffHand` | Low |
| `ShieldBlock` | `float ShieldBlock` | Low |

#### Spell Power / Healing
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `PhysicalBonusPositive` | `uint` | Low |
| `HolyBonusPositive/Negative/Percent` | `uint` | Low |
| `FireBonusPositive/Negative/Percent` | `uint` | Low |
| `NatureBonusPositive/Negative/Percent` | `uint` | Low |
| `FrostBonusPositive/Negative/Percent` | `uint` | Low |
| `ShadowBonusPositive/Negative/Percent` | `uint` | Low |
| `ArcaneBonusPositive/Negative/Percent` | `uint` | Low |
| `HealingBonusPositive` | `uint` | Medium |
| `HealingModifierPercent` | `float` | Medium |
| `HealingBonusPercent` | `float` | Low |
| `SpellPowerModifierPercent` | `float` | Medium |
| `TargetResistanceModifier` | `uint` | Low |
| `TargetArmorModifier` | `uint` | Low |

#### Spell Crit Per School
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `PhysicalCritPercent` | `float` | Low |
| `HolyCritPercent` | `float` | Low |
| `FireCritPercent` | `float` | Low |
| `NatureCritPercent` | `float` | Low |
| `FrostCritPercent` | `float` | Low |
| `ShadowCritPercent` | `float` | Low |
| `ArcaneCritPercent` | `float` | Low |

#### Experience & PvP
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `RestedExperience` | `uint RestedExperience` | Low |
| `HasRestedXp` | `bool HasRestedXp` | Low |
| `CharacterPoints` | `uint CharacterPoints` | Low |
| `HonorableKills` | `uint HonorableKills` | Low |
| `LifetimeHonorableKills` | `uint LifetimeHonorableKills` | Low |
| `PvpMedalCount` | `uint PvpMedalCount` | Low |
| `WatchedFactionIndex` | `int WatchedFactionIndex` | Low |
| `SelfResurrectSpellId` | `uint SelfResurrectSpellId` | Low |
| `MaxLevel` | `uint MaxLevel` | Low |
| `BattlefieldArenaFaction` | `int BattlefieldArenaFaction` | Low |

#### Other
| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `ChosenTitle` | `uint ChosenTitle` | Low |
| `Inebriation` | `WoWInebriationLevel Inebriation` | Low |
| `LevelFraction` | `float LevelFraction` | Low |
| `BankBagSlotCount` | `uint BankBagSlotCount` | Low |
| `IsInMyPartyOrRaid` | `bool IsInMyPartyOrRaid` | Medium |
| `IsTrackingStealthed` | `bool IsTrackingStealthed` | Low |
| `ReleaseTimerIsVisible` | `bool ReleaseTimerIsVisible` | Low |
| `ArenaTeams` | `List<WoWArenaTeamInfo> ArenaTeams` | Low |
| `RuneRegen` | `float[] RuneRegen` | Low |
| `Glyphs` | `List<WoWGlyphInfo> Glyphs` | Low |
| `GlyphsEnabled` | `uint GlyphsEnabled` | Low |
| `PetSpellPower` | `uint PetSpellPower` | Low |
| `IsAlive` override | `override bool IsAlive` (checks `!Dead && !IsGhost`) | **High** |

### ⚠️ Potential Issues

| Item | Issue |
|---|---|
| `Coinage` | CopilotBuddy uses `uint`; HB 4.3.4 (Copper) uses `ulong`. Gold amounts > ~42 gold would overflow `uint`. Should be `ulong`. |
| `DuelTeam` | CopilotBuddy has `DuelTeam`; HB has `DuelTeamId`. Verify field name matches what CRs expect. |

---

## 4. LocalPlayer

### ❌ Missing from CopilotBuddy

| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `HearthstoneAreaId` | `uint HearthstoneAreaId` | Low |
| `Totems` | `List<WoWTotemInfo> Totems` | **High** — Shaman CRs |
| `GetTotemBarSpells` | Returns totem bar spell list | Medium |
| `Stable` | `Stable Stable` | Low |
| `LearnableSpells` | `List<WoWSpell> LearnableSpells` | Low |
| `SpecType` | `SpecType SpecType` | Medium — Returns Tank/Healer/MeleeDps/RangedDps |
| `CanEquipItem(WoWItem)` | `bool CanEquipItem(...)` (multiple overloads) | Medium |
| `CanUseItem(uint, out GameError)` | `bool` | Medium |
| `GetEstimatedRepairCost()` | `WoWPrice GetEstimatedRepairCost()` | Low |
| `GetMirrorTimerInfo(type)` | `MirrorTimerInfo` | Low |
| `GetReputationWith(uint)` | `int GetReputationWith(uint factionId)` | Medium |
| `GetReputationLevelWith(uint)` | `WoWUnitReaction` | Medium |
| `HasPendingSpell(int/string/WoWSpell)` | `bool` (3 overloads) | Low |
| `IsBehind(WoWUnit)` | `bool IsBehind(WoWUnit)` | Medium — `WoWMathHelper` based |
| `QuestLog` | `QuestLog QuestLog` field | Medium |
| `GetAllSkills` | (via Skills dictionary) | CopilotBuddy has this |

### ⚠️ Potential Issues

| Item | Issue |
|---|---|
| `Role` | CopilotBuddy reads via Lua (`GetLFGRoles`); HB 4.3.4 reads directly from a memory offset. Lua approach is **fragile** — LFG might not be loaded, or Lua environment may not be ready during early init. Consider implementing direct memory read. |
| `PartyMemberGuids` | CopilotBuddy iterates `PartyMember1GUID` through `PartyMember4GUID`; HB reads from a contiguous offset array. Functionally equivalent but verify all 4 GUIDs are correct. |

---

## 5. WoWItem

### ❌ Missing from CopilotBuddy

| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `GiftCreatorGuid` | `ulong GiftCreatorGuid` | Low |
| `RandomSuffix` | `WoWItemRandomSuffix RandomSuffix` | Low |
| `RandomPropertiesId` | `int RandomPropertiesId` | Medium |
| `PropertySeed` | `int PropertySeed` | Low |
| `GetMinDamage(int)` | `float GetMinDamage(int index)` | Low |
| `GetMaxDamage(int)` | `float GetMaxDamage(int index)` | Low |
| `GetDamageType(int)` | `int` | Low |
| `GetSocketColor(int)` | `WoWSocketColor` | Low |
| `GetEnchantmentById(uint)` | `WoWItemEnchantment GetEnchantmentById(uint id)` | Medium |
| `GetEnchantment(string)` | `WoWItemEnchantment GetEnchantment(string name)` | Medium |
| `PickUp()` | `void PickUp()` | Low |
| `UseContainerItem()` | `void UseContainerItem()` | Low |
| `Use(ulong, bool)` | `bool Use(ulong targetGuid, bool forceUse)` | Medium |
| `CooldownTimeLeft` | `TimeSpan CooldownTimeLeft` | Medium |
| `GetItemName()` | `string GetItemName()` (with random properties suffix) | Low |
| `ItemLink` | `string ItemLink` | Low |
| `IsGiftWrapped` | `bool IsGiftWrapped` | Low |
| `IsTotem` | `bool IsTotem` | Low |
| `TriggersSpell` | `bool TriggersSpell` | Low |
| `HasEquipCooldown` | `bool HasEquipCooldown` | Low |
| `IsWand` | `bool IsWand` | Low |
| `IsWrappingPaper` | `bool IsWrappingPaper` | Low |
| `IsCharter` | `bool IsCharter` | Low |
| `IsReadable` | `bool IsReadable` | Low |
| `IsPvPItem` | `bool IsPvPItem` | Low |
| `CanExpire` | `bool CanExpire` | Low |
| `CanProspect` | `bool CanProspect` | Low |
| `IsUniqueEquipped` | `bool IsUniqueEquipped` | Low |
| `IsThrownWeapon` | `bool IsThrownWeapon` | Low |
| `IsEnchantScroll` | `bool IsEnchantScroll` | Low |
| `IsMillable` | `bool IsMillable` | Low |
| `WoWItemRandomProperties` class | Nested helper for random properties | Low |
| `WoWItemRandomSuffix` class | Nested helper for random suffixes | Low |

### 🔶 Stub / Present but Correct
CopilotBuddy's WoWItem appears to have good coverage of the core API:
- OwnerGuid, ContainerGuid, CreatorGuid, StackCount, Duration, SpellCharges, Flags
- Durability, MaxDurability, DurabilityPercent
- Enchantments (GetEnchantment by index, TemporaryEnchantment)
- Stats (GetStat, GetStatType, GetStatValue)
- ItemSpells, GetSpell
- ItemInfo integration (Quality, RequiredLevel, ItemLevel, etc.)
- BagIndex, BagSlot
- Use(), Cooldown, Usable
- Flag properties (IsSoulbound, IsConjured, IsOpenable, IsAccountBound)

---

## 6. WoWGameObject

### ❌ Missing from CopilotBuddy

| Property/Method | HB 4.3.4 Signature | Priority |
|---|---|---|
| `RelativeLocation` override | `override WoWPoint RelativeLocation` | Low |
| `FactionTemplateId` | `uint FactionTemplateId` | Low (CopilotBuddy has `Faction` uint) |
| `FactionTemplate` | `WoWFactionTemplate FactionTemplate` | Medium |
| `AnimationState` | `byte AnimationState` | Low |
| `FlagsUint` | `uint FlagsUint` | Low (CopilotBuddy uses Flags directly) |
| `SubObj` | `WoWSubObject SubObj` | Medium — HB has full sub-object system (WoWDoor, WoWChair, WoWFishingBobber, WoWAnimatedSubObject) |
| `Model` | `string Model` | Low |
| `LockTypeEntry` struct | Lock type name/process/internal name | Low |
| `CanUseNow(out GameError)` | `bool CanUseNow(out GameError reason)` | Medium |

### 🔶 Stubs in CopilotBuddy

| Property/Method | Current Behavior | Impact |
|---|---|---|
| `GetCachedInfo()` | **Returns `false` always** (TODO comment) | **Critical** — Blocks `IsHerb`, `IsMineral` detection for gathering bots since `LockRecord` depends on `GetDataSlot` which depends on cached info |
| `GetDataSlot()` | **Returns `false` always** (TODO comment) | **Critical** — `SpellFocus`, `RequiredSkill`, `LockRecord`, `CanLoot` all depend on this |
| `LockRecord` | **Returns `null`** (depends on `GetDataSlot`) | **Critical** — `IsHerb`, `IsMineral`, `CanLoot`, `CanMine`, `CanHarvest` all broken |
| `InteractRange` | Returns hardcoded `4.5f` | ⚠️ HB derives from `SubObj.InteractDistance - 0.25f`. Hardcoded value may be wrong for many GO types |

### ⚠️ Potential Issues

| Item | Issue |
|---|---|
| `IsHerb` / `IsMineral` | Both depend on `LockType` → `LockRecord` → `GetDataSlot` → `GetCachedInfo`. All of these are stubs. **GatherBuddy cannot work** until this chain is implemented. |
| `CanLoot` | Depends on `RequiredSkill` → `GetDataSlot`. Currently broken. |
| `Position` | CopilotBuddy reads from hardcoded offsets `0xE8-0xF8`; HB reads via `StyxWoW.Offsets.method_0(7)`. Verify these offsets are correct for 3.3.5a. |

---

## Summary: Priority Fix List

### 🔴 Critical (Blocking bots from working)

1. **WoWGameObject.GetCachedInfo / GetDataSlot / LockRecord chain** — GatherBuddy is dead without this
2. **WoWGameObject.InteractRange** — Hardcoded `4.5f` is wrong for many GO types

### 🟠 High (CRs and common bot logic depend on these)

3. **WoWUnit**: `IsTargetingMeOrPet`, `Attackable`, `PvpFlagged`, `CanSelect`
4. **WoWUnit**: `CurrentCastTimeLeft` / `CurrentCastStartTime` / `CurrentCastEndTime`, `CastingSpell`
5. **WoWUnit**: `GotAlivePet`, `IsTotem`, `IsAlive` (virtual)
6. **WoWUnit**: `AttackPower`, `RangedAttackPower`, `MinDamage`, `MaxDamage`
7. **WoWPlayer**: `BlockPercent`, `DodgePercent`, `ParryPercent`, `CritPercent`
8. **WoWPlayer**: `IsAlive` override (checks `!Dead && !IsGhost`)
9. **WoWObject**: `DistanceSqr`, `Distance2D`, `Distance2DSqr` (performance-critical)
10. **LocalPlayer**: `Totems` (Shaman CRs)

### 🟡 Medium (Nice-to-have, improves fidelity)

11. **WoWUnit**: Creature type checks (`IsBeast`, `IsHumanoid`, `IsUndead`, etc.)
12. **WoWUnit**: `NativeDisplayId`, `MountDisplayId`, `CastSpeedModifier`
13. **WoWUnit**: `PowerInfo` struct system
14. **WoWPlayer**: Spell power bonuses, combat expertise
15. **WoWItem**: `GetEnchantmentById`, `CooldownTimeLeft`, `Use(guid, force)` overloads
16. **WoWGameObject**: `FactionTemplate`, `SubObj` system
17. **LocalPlayer**: `SpecType`, `GetReputationWith`, `CanEquipItem`
18. **WoWObject**: `GetWorldMatrix()`, `RelativeLocation`

### Type Mismatch Cleanup

19. **WoWUnit power methods**: `int` → should match HB's `uint` (or document why `int` is intentional for WotLK)
20. **WoWPlayer.Coinage**: `uint` → should be `ulong` to handle large gold amounts

---

## Notes

- Many "missing" properties from HB 4.3.4 are **Cata-specific** (SoulShards, Eclipse, HolyPower, GuildLevel, GuildDeleteDate, Mastery, ResearchSites). Per project rules, these should get **stub signatures returning neutral values** but are lowest priority.
- The WoWUnit file in HB 4.3.4 is 4048 lines vs CopilotBuddy's 1686 — roughly 60% of API surface area is not yet ported.
- The WoWPlayer file in HB 4.3.4 is 1207 lines vs CopilotBuddy's 347 — roughly 70% not yet ported.
