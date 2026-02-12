# WoWObject Hierarchy Audit — HB 4.3.4 vs CopilotBuddy

> **Generated:** Comprehensive comparison of all public/protected members.
> **Reference:** `.hb 4.3.4\Honorbuddy\Honorbuddy\Styx\WoWInternals\WoWObjects\`
> **Subject:** `CopilotBuddy\Styx\WoWInternals\WoWObjects\`

## Legend

| Category | Meaning |
|----------|---------|
| **MISSING** | Member exists in HB 4.3.4 but has no equivalent in CB — needs implementation |
| **STUB-OK** | Cata-only feature — absent or returns neutral value, as expected |
| **STUB-REVIEW** | WotLK feature that is stubbed/incomplete — needs review |
| **RENAME** | Present in CB under a different name — verify external consumers match |
| **TYPE-DIFF** | Present but return type or signature differs |
| **PRESENT** | Fully implemented (not listed individually — only gaps are called out) |

---

## 1. WoWObject.cs

**HB:** 876 lines | **CB:** 632 lines

All core members are present: `BaseAddress`, `IsValid`, `Entry`, `Type`, `TypeFlags`, `Guid`, `ObjectFlags`, `InteractRange`/`Sqr`, `WithinInteractRange`, `IsDisabled`, `DescriptorGuid`, `X`/`Y`/`Z`, `Rotation`, `RotationDegrees`, `Distance`/`Sqr`/`2D`/`2DSqr`, `IsMe`, `Name`, `Location`, `WorldLocation`, `RelativeLocation`, `QuestGiverStatus`, `InteractType`, `IsOutdoors`, `IsIndoors`, `HasFlag()`, `GetHashCode`/`ToString`/`Equals`/operators/`CompareTo`, `To*()` cast methods, `InLineOfSight`/`OCD`, `IsFacing`/`IsSafelyFacing`/`IsBehind`/`IsSafelyBehind`, `Interact()`, `GetPosition()`, `GetObjectName()`.

CB adds: `IsUnderground`.

| Status | Member | HB Signature | Notes |
|--------|--------|-------------|-------|
| MISSING | `GetWorldMatrix()` | `Matrix GetWorldMatrix()` | Not in WoWObject base. CB has it in `WoWGameObject` only. Consumers calling on a generic WoWObject will fail. |
| MISSING | `OnInvalidate` | `event EventHandler OnInvalidate` | Event fired when object becomes invalid. No equivalent in CB. |

**Verdict:** 2 gaps. `GetWorldMatrix()` on base WoWObject is rare — most callers use the GameObj override. `OnInvalidate` may be needed if any bot subscribes to object invalidation.

---

## 2. WoWUnit.cs

**HB:** 4048 lines (~180+ public members) | **CB:** 2083 lines

### 2a. MISSING — WotLK features not present

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `Faction` | `WoWFaction Faction { get; }` | **High** | CB has `FactionId` (uint) but not the `WoWFaction` wrapper object. Several bot routines reference `.Faction`. |
| `FactionTemplate` | `WoWFactionTemplate FactionTemplate { get; }` | **High** | CB has `FactionTemplateId` but not the template object. Used in reaction/hostility checks. |
| `CurrentCastStartTime` | `uint CurrentCastStartTime { get; }` | Medium | Timestamp when current cast started. Useful for cast-bar timing. |
| `CurrentCastEndTime` | `uint CurrentCastEndTime { get; }` | Medium | Timestamp when current cast will end. |
| `RenderFacing` | `float RenderFacing { get; }` | Low | Visual facing (may differ from movement facing). |
| `VanityPetGuid` | `ulong VanityPetGuid { get; }` | Low | Companion pet GUID (WotLK critters). |
| `VanityPet` | `WoWUnit VanityPet { get; }` | Low | Companion pet unit. |
| `CreatureFamilyInfo` | `WoWCreatureFamilyInfo CreatureFamilyInfo { get; }` | Low | Hunter pet family info (Wolf, Cat, etc.). |
| `MovementInfo` | `MovementInfo MovementInfo { get; }` | Medium | Full movement struct (flags, transport, jump, fall). CB reads individual fields. |
| `TransportGuid` | `ulong TransportGuid { get; }` | Low | GUID of transport (boat/zeppelin). |

### 2b. RENAME — Different names in CB

| HB Name | CB Name | Notes |
|---------|---------|-------|
| `CharmedByUnitGuid` | `CharmedByGuid` | Same field, shorter name |
| `SummonedByUnitGuid` | `SummonedByGuid` | Same field, shorter name |
| `CreatedByUnitGuid` | `CreatedByGuid` | Same field, shorter name |
| `CurrentTargetGuid` | `CurrentTargetGuid` | Same ✓ |
| `TargetGuid` | `CurrentTargetGuid` | HB has both `TargetGuid` and `CurrentTargetGuid` as aliases |
| `BonusStrength` | `BonusStrength` | Same ✓ — HB also has `StrengthPositiveModifier`/`NegativeModifier` variants |
| `CurrentCastId` | `CastingSpellId` | Same underlying field |
| `CurrentChannelId` | `ChanneledCastingSpellId` | Same underlying field |

### 2c. MISSING — Stat modifier variants (WotLK)

HB exposes both `Bonus{Stat}` and `{Stat}PositiveModifier`/`{Stat}NegativeModifier` for each stat. CB only has `Bonus{Stat}`. If any external routine references the modifier variants, they will fail:

| Missing | Type |
|---------|------|
| `StrengthPositiveModifier` | `int` |
| `StrengthNegativeModifier` | `int` |
| `AgilityPositiveModifier` | `int` |
| `AgilityNegativeModifier` | `int` |
| `StaminaPositiveModifier` | `int` |
| `StaminaNegativeModifier` | `int` |
| `IntellectPositiveModifier` | `int` |
| `IntellectNegativeModifier` | `int` |
| `SpiritPositiveModifier` | `int` |
| `SpiritNegativeModifier` | `int` |

> These read from adjacent descriptor fields. Implementation is trivial if needed.

### 2d. STUB-OK — Cata power types (expected stubs)

| Member | CB Value | Notes |
|--------|----------|-------|
| `CurrentSoulShards` | `0` | Cata-only (CATA-03) |
| `CurrentEclipse` | `0` | Cata-only (CATA-03) |
| `CurrentHolyPower` | `0` | Cata-only (CATA-03) |
| `MaxSoulShards` | `0` | Cata-only |
| `MaxEclipse` | `0` | Cata-only |
| `MaxHolyPower` | `0` | Cata-only |

### 2e. TYPE-DIFF

| Member | HB Type | CB Type | Notes |
|--------|---------|---------|-------|
| `BonusArmor` | `uint` | `int` | Sign difference — likely harmless |
| Various bonus stats | `uint` | `int` | CB uses `int` consistently for stat bonuses |

### 2f. Present and implemented (major members)

All of these are confirmed present in CB: `Health`, `MaxHealth`, `HealthPercent`, `CurrentMana`/`MaxMana`, `CurrentRage`/`MaxRage`, `CurrentEnergy`/`MaxEnergy`, `CurrentRunicPower`/`MaxRunicPower`, `CurrentPower`/`MaxPower`/`PowerType`, `Level`, `Race`, `Class`, `Gender`, `DisplayId`, `NativeDisplayId`, `MountDisplayId`, `CreatureType`, `CreatureRank`, `CastingSpellId`, `ChanneledCastingSpellId`, `IsCasting`, `IsChanneling`, `CurrentCastTimeLeft`, `CurrentChannelTimeLeft`, `Auras`/`ActiveAuras`/`PassiveAuras`, `HasAura`/`GetAuraByName`/`GetAllAuras`, `Strength`/`Agility`/`Stamina`/`Intellect`/`Spirit`, `BaseAttackTime`, `RangedAttackTime`, `BoundingRadius`, `CombatReach`, `NpcFlags`, `UnitFlags`/`UnitFlags2`, `IsFlying`, `IsSwimming`, `IsFriendly`, `IsHostile`, `IsNeutral`, `IsPet`, `IsTotem`, `IsDead`, `IsAlive`, `IsGhost`, `IsStunned`, `IsFleeing`, `IsSilenced`, `IsDisarmed`, `IsSkinnable`, `IsLootable`, `IsTameable`, `InCombat`, `IsMoving`, `Mounted`, `IsFacing(WoWUnit)`, `GetReactionTowards`, `GotTarget`, `IsTargetingMeOrPet`, `IsTargetingMyRaidMember`, `ThreatInfo`, `SheatheState`, `StandState`, `Bytes0`–`Bytes2`, `HitCount`, `BaseResistances`, `BonusResistances`, `Resistances`, `MinDamage`/`MaxDamage`/`MinOffHandDamage`/`MaxOffHandDamage`/`MinRangedDamage`/`MaxRangedDamage`, `AttackPower`/`AttackPowerModifier`/`RangedAttackPower`/`RangedAttackPowerModifier`, `QuestGiverStatus`, `Interact()`/`Face()`, `Name`, `Location`, `Rotation`.

---

## 3. WoWPlayer.cs

**HB:** 1207 lines | **CB:** 718 lines

### 3a. MISSING — WotLK features

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `GuildTimestamp` | `uint GuildTimestamp { get; }` | Low | Descriptor field — when player joined guild |
| `ChosenTitle` | `int ChosenTitle { get; }` | Low | Active title index |
| `Inebriation` | `uint Inebriation { get; }` | Low | Drunk level (0–100) |
| `PvpMedalCount` | `uint PvpMedalCount { get; }` | Low | PvP medals |
| `BattlefieldArenaFaction` | `uint BattlefieldArenaFaction { get; }` | Low | Which arena faction |
| `IsTrackingStealthed` | `bool IsTrackingStealthed { get; }` | Medium | Whether tracking stealth (Hunter ability) |
| `ReleaseTimerIsVisible` | `bool ReleaseTimerIsVisible { get; }` | Medium | Whether spirit release timer is showing |
| `ArenaTeams` | `List<WoWArenaTeamInfo> ArenaTeams { get; }` | Low | Full arena team info objects |
| `PhysicalCritPercent` | `float PhysicalCritPercent { get; }` | Medium | Separate from school crit — melee physical crit |

### 3b. RENAME

| HB Name | CB Name | Notes |
|---------|---------|-------|
| `DuelTeamId` | `DuelTeam` | Same field |
| `ExpertiseOffHand` | `OffHandExpertise` | Same field |
| `Glyphs` (List\<WoWGlyphInfo\>) | `GetGlyph(int)` / `GetGlyphSlot(int)` | CB exposes per-slot accessors instead of a list. Consumers expecting `Glyphs[i]` need adaptation. |
| `HonorableKills` (single total) | `HonorableKillsToday` + `HonorableKillsYesterday` | CB splits into today/yesterday. HB had a single total from descriptors. **May need a combined property.** |

### 3c. STUB-OK — Cata-only

| Member | CB Value | Notes |
|--------|----------|-------|
| `Mastery` | `0f` | CATA-02 — Mastery stat does not exist in WotLK |
| `GuildDeleteDate` | — | CATA — guild deletion timer, not applicable |
| `GuildLevel` | — | CATA — guild leveling system, not in WotLK |

### 3d. Present and implemented

Confirmed present: `IsHorde`, `IsAlliance`, `PlayerFlags` (BitVector32), `IsGhost`, `IsGroupLeader`, `IsAFKFlagged`, `IsDNDFlagged`, `IsGM`, `IsResting`, `IsFFAPvPFlagged`, `ContestedPvPFlagged`, `IsPvPFlagged`, `IsHidingHelm`, `IsHidingCloak`, `IsOutOfBounds`, `IsPvPTimerActive`, `IsInsideSanctuary`, `GuildRank`, `Experience`, `NextLevelExperience`, `CharacterPoints`, `BlockPercent`, `DodgePercent`, `ParryPercent`, `Expertise`, `CritPercent`, `RangedCritPercent`, `OffHandCritPercent`, `ShieldBlock`, `ShieldBlockCritPercent`, `RestedExperience`, `SelfResurrectSpellId`, `LifetimeHonorableKills`, `WatchedFactionIndex`, `MaxLevel`, `DuelArbiterGuid`, `BankBagSlotCount`, `Skin`/`FaceType`/`HairStyle`/`HairColor`/`FacialHair`, `HasRestedXp`, all school CritPercents, all school BonusPositive/Negative/Percent, `HealingBonusPositive`, `HealingModifierPercent`, `HealingBonusPercent`, `SpellPowerModifierPercent`, `TargetResistanceModifier`, `TargetArmorModifier`, `RuneRegen`, `GlyphsEnabled`, `PetSpellPower`, `Copper`/`Silver`/`Gold`, `LevelFraction`, `IsInMyParty`/`IsInMyRaid`/`IsInMyPartyOrRaid`, `IsAlive` (override), `Mounted` (override), `Minions`.

CB adds: `IsMale`, `IsFemale`, `IsTank`, `IsHealer`, `IsCaster`, `IsMelee`, `IsDueling`, `GetCombatRating()`, `Resilience`, `ArmorPenetration`, `HasteRating`, `ExpertiseRating`, `MainhandEntryId`, `OffhandEntryId`, `MasteryPercent` (stub).

---

## 4. LocalPlayer.cs

**HB:** 1998 lines | **CB:** 2704 lines (larger — CB adds extra features)

### 4a. MISSING — WotLK features

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `HearthstoneAreaId` | `uint HearthstoneAreaId { get; }` | Medium | CB has `HearthstoneBindLocation` (WoWPoint) but not the area ID. Some routines may check zone. |
| `Stable` | `List<WoWUnit> Stable { get; }` | Low | Pet stable contents — hunter-specific |
| `LearnableSpells` | `List<WoWSpell> LearnableSpells { get; }` | Low | Spells available to learn from trainer |
| `GetRaidMember(int)` | `WoWPartyMember GetRaidMember(int index)` | Medium | Returns WoWPartyMember by raid index. CB has `GetRaidMemberGuid` but not this overload. |
| `LoadingScreen` | `bool LoadingScreen { get; }` | — | CB has this marked `[Obsolete]` — functionally present but deprecated |

### 4b. RENAME

| HB Name | CB Name | Notes |
|---------|---------|-------|
| `SpecType` (property) | `Specialization` | Same return type (`SpecType`), different property name |
| `AutoRepeatingSpellId` | `AuthRepeatingSpellId` | CB has both — `AutoRepeatingSpellId` is an alias for `AuthRepeatingSpellId` |
| `CurrentPendingCursorSpell` | `CurrentPendingCursorSpell` + `CurrentCursorSpell` | CB has both; `CurrentCursorSpell` has different implementation |

### 4c. STUB-OK — Cata-only

| Member | Notes |
|--------|-------|
| `ResearchSiteIds` | Archaeology — Cata-only. Not present in CB. Correct. |
| `GetTotemBarSpells(int)` | Multi-cast totem bar — Cata-only. CB returns empty list. Correct. |

### 4d. Present and implemented

Confirmed present (all core functionality): `AccountName`, `RealmName`, `MinimapZoneText`, `ZoneId`, `MapId`, `CurrentMap`, `RealZoneText`, `ZoneText`, `SubZoneText`, `MapName`, `CorpsePoint`, `InstanceDeathLocation`, `InstanceCorpseLocation`, `IsActuallyInCombat`, `BloodRuneCount`/`FrostRuneCount`/`UnholyRuneCount`/`DeathRuneCount`, `GetRuneCount()` (2 overloads), `GetRuneType()`, `ComboPoints`, `RawComboPoints`, `AutoRepeatingSpellId`, `IsAutoRepeatingSpell`, `IsInInstance`, `Durability`, `MaxDurability`, `DurabilityPercent`, `LowestDurabilityPercent`, `BagsFull`, `FreeBagSlots`, `NormalBagsFull`, `FreeNormalBagSlots`, `ToggleAttack()`, `ClearTarget()`, `TargetLastTarget()`, `GetFactionStanding()` (2 overloads), `PartyMember1–4`/`GUIDs`, `IsInParty`, `IsInRaid`, `NumRaidMembers`, `RaidMembers`, `PartyMemberGuids`, `RaidMemberGuids`, `PartyMemberInfos`, `RaidMemberInfos`, `PartyMembers`, `IsBehind()`, `Inventory`, `GetBag()`/`GetBagAtIndex()`/`GetBagGuidAtIndex()`, `BagItemGuids`, `CarriedItemGuids`, `BagItems`, `CarriedItems`, `CanEquipItem()` (4 overloads), `CanUseItem()`, `GetMirrorTimerInfo()`, `KnownSpells`, `GetSkill()` (2 overloads), `CanSkinLevel`, `SetFacing()` (4 overloads), `FocusedUnitGuid`/`FocusedUnit`, `SetFocus()`, `CurrentPendingCursorSpell`, `HasPendingSpell()` (3 overloads), `Totems`, `QuestLog`, `GetEstimatedRepairCost()`, `PetSpells`, `HearthstoneBindLocation`, `GetReputationWith()`, `GetReputationLevelWith()`, `Role`, `IsOutdoors`/`IsIndoors` (overrides), `LastRedErrorMessage`, `AllSkills`, `GroupInfo`.

CB adds: `RealmName`, `ContinentName`, `CurrentXP`, `XPToNextLevel`, `XPPercent`, `LocalTarget`/`Guid`, `HasPet`, `IsMounted`, `IsTravelForm`, `IsAscending`, `IsStealthed` (override), `Stance`, `IsStunned`, `IsRooted`, `Snared`, `ManaRegenRate`, `PowerPercent`, `CurrentCursorSpell`, `GroupInfo`.

---

## 5. WoWItem.cs

**HB:** 1624 lines | **CB:** 732 lines

### 5a. MISSING — WotLK features

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `WoWItemRandomSuffix` (nested class) | Full class with `Prefix[]`, suffix data | Medium | HB has both `WoWItemRandomProperties` AND `WoWItemRandomSuffix`. CB only has `WoWItemRandomProperties`. Items with random suffixes (e.g., "…of the Bear") may not display correctly. |
| `RandomSuffix` (property) | `WoWItemRandomSuffix RandomSuffix { get; }` | Medium | Returns the random suffix object. CB has `RandomPropertiesId` but not the suffix wrapper. |

### 5b. RENAME

| HB Name | CB Name | Notes |
|---------|---------|-------|
| `ItemLink` | `Link` | Same concept — the `[item:…]` link string |

### 5c. Present and implemented

Confirmed present: `Name`, `Location` (override → Zero), `Distance` overrides, `Interact()`→`Use()`, `GetItemName()`, `ItemStats`, `BagIndex`, `BagSlot`, `OwnerGuid`, `ContainerGuid`, `CreatorGuid`, `GiftCreatorGuid`, `StackCount`, `Duration`, `SpellCharges`, `Flags`, `PropertySeed`, `RandomPropertiesId`, `Durability`, `MaxDurability`, `DurabilityPercent`, `TemporaryEnchantment`, `GetEnchantment()` (overloads), `GetEnchantmentById()`, `GetStat()`/`GetStatType()`/`GetStatValue()`, `GetMinDamage()`/`GetMaxDamage()`/`GetDamageType()`, `ItemSpells`/`GetSpell()`, `GetSocketColor()`, `ItemInfo`, `Quality`, `UseContainerItem()`, `PickUp()`, `Use()` (overloads), all flag properties (`IsSoulbound` through `IsMillable`), `Cooldown`, `CooldownTimeLeft`, `Usable`, `WoWItemEnchantment` (nested), `WoWItemSpell` (nested), `WoWItemStat` (nested).

CB adds: `RequiredLevel`, `ItemLevel`, `ItemClass`, `EquipSlot`, `SellPrice`, `BuyPrice`, `IsBroken`, `CreatePlayedTime`, `IsGift`.

---

## 6. WoWGameObject.cs

**HB:** 835 lines | **CB:** 564 lines

### 6a. MISSING

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `RelativeLocation` (override) | `WoWPoint RelativeLocation { get; }` | Low | HB reads from memory offset 7 as WoWPoint. Base returns `WoWPoint.Empty`. CB inherits base. Only matters for objects on transports. |
| `AnimationState` | `byte AnimationState { get; }` | Low | Reads byte from memory offset 6. |
| `FlagsUint` | `uint FlagsUint { get; }` | Low | Raw uint flags — CB has `Flags` as `GameObjectFlags` enum which should suffice. |
| `FactionTemplate` | `WoWFactionTemplate FactionTemplate { get; }` | Medium | CB has `FactionTemplateId` but not the template object. Same issue as WoWUnit. |
| `LockTypeEntry` (struct) | Full struct | Low | Sub-struct of LockEntry. CB has `LockEntry` but not `LockTypeEntry`. |

### 6b. TYPE-DIFF

| Member | HB Type | CB Type | Impact |
|--------|---------|---------|--------|
| `ParentRotation` | `float` (single value from descriptor) | `Vector4` (quaternion) | HB reads ONE float from `GAMEOBJECT_PARENTROTATION + 2`; CB reads full 4-float quaternion. CB is more complete but consumers expecting `float` will get a compile error. |
| `Flags` | `BitVector32` | `GameObjectFlags` (enum) | Functionally equivalent but different access pattern |

### 6c. Present and implemented

Confirmed present: `ToString()`, `DisplayId`, `Bytes1`, `CreatedByGuid`, `CreatedBy`, `Flags`, `DynamicFlags`/`FlagsDynamic`, `Level`, `FactionTemplateId`, `State`, `SubType`, `SubObj`, `SpellFocus`, `InteractRange` (override), `ArtKit`, `AnimationProgress`, `Locked`, `Transport`, `InUse`, `Triggered`, `IsHerb`, `LockType`, `LockRecord`, `IsMineral`, `IsChest`, `Model`, `GetWorldMatrix()`, `GetCachedInfo`, `GetDataSlot()` (overloads), `CanUse`, `CanUseNow()` (overloads), `GetReactionTowards()`, `RequiredSkill`, `CanLoot`, `CompareTo`/`Compare`, `LockEntry` struct.

CB adds: `IsTransport`, `CanMine`, `CanHarvest`, `CanFish`, `IsDoor`, `IsButton`, `IsQuestGiver`, `IsMailbox`, `WorldMatrix` (property), `GameObjectSubData` class.

---

## 7. WoWPartyMember.cs

**HB:** 720 lines | **CB:** 522 lines

HB reads party/raid info from **memory structs** (`PartyMemberInfo`, `Struct51`) via ASM calls.
CB reads equivalent data via **Lua calls** (`UnitHealth`, `UnitLevel`, etc.).
Functionally equivalent but implementation differs significantly.

### 7a. MISSING

| Member | HB Signature | Priority | Notes |
|--------|-------------|----------|-------|
| `FFAPvpFlagged` | `bool FFAPvpFlagged { get; }` | Low | Free-for-all PvP flag. CB can add via `UnitIsFFA()` if needed. |
| `RAFLinked` | `bool RAFLinked { get; }` | Low | Recruit-A-Friend linked. Niche feature. |
| `ContinentId` | `ushort ContinentId { get; }` | Medium | Continent/map of the party member. Used to check if member is on same continent. |
| `VehicleSeatIndex` | `uint VehicleSeatIndex { get; }` | Low | Vehicle seat — mostly Cata. WotLK has some vehicle fights but rarely queried. |

### 7b. STUB-REVIEW

| Member | HB Behavior | CB Behavior | Notes |
|--------|-------------|-------------|-------|
| `AreaTableId` | Reads from memory struct | Returns `0` always | CB has a comment acknowledging this. Needs Lua implementation if zone tracking of party members is required. |
| `Location` | Returns `Vector2` (map coords from memory) | Returns `WoWPoint` (from ToPlayer() or map coords) | Different return type. HB's `Vector2` vs CB's `WoWPoint`. CB's `Location3D` property matches HB's `Location3D`. |

### 7c. TYPE-DIFF

| Member | HB Type | CB Type | Notes |
|--------|---------|---------|-------|
| `Location` | `Vector2` | `WoWPoint` | HB returns 2D map coords; CB returns 3D point (prefers `ToPlayer().Location` when in range). |
| Constructor | `WoWPartyMember(ulong, bool)` | `WoWPartyMember(string unitId, int index, bool isRaidMember)` | Different construction pattern. CB adds factory methods `FromPartyIndex`/`FromRaidIndex`. Internal callers must match. |

### 7d. Present and implemented

Confirmed present: `Guid`, `IsMainTank`, `IsMainAssist`, `IsOnline`, `PvpFlagged`, `Dead`, `Ghost`, `GroupLeader`, `DNDFlagged`, `PowerType`, `Health`, `HealthMax`, `Power`, `PowerMax`, `Level`, `Location3D`, `GroupNumber`, `RaidRank`, `Role`, `HasRole()`, `ToPlayer()`, `GroupRole` enum, `Equals`/`GetHashCode`/operators, `ToString`.

CB adds: `Name`, `UnitId`, `AFKFlagged`, `IsTank`, `IsHealer`, `Class`, `HealthPercent`, `FromPartyIndex()`/`FromRaidIndex()` factory methods.

---

## 8. WoWDynamicObject.cs

**HB:** ~80 lines (minimal) | **CB:** ~170 lines (more complete)

### 8a. CB has everything HB has, plus extras

| HB Member | CB Status |
|-----------|-----------|
| `SpellId` | ✓ (`uint` in CB vs `int` in HB — minor type diff) |
| `Spell` | ✓ |
| `Caster` | ✓ |
| `CasterGuid` | ✓ |
| `Bytes` (internal) | ✓ |
| `Radius` | ✓ |
| `CastTime` | ✓ |

CB adds: `X`/`Y`/`Z`/`Location` overrides, `IsMine`, `IsHostile`, `AmIInRange`, `IsPointInRange()`, `DynObjType` enum, `ToString()`.

### 8b. TYPE-DIFF

| Member | HB Type | CB Type | Impact |
|--------|---------|---------|--------|
| `SpellId` | `int` | `uint` | Minor — sign difference, unlikely to matter |

**Verdict:** No gaps. CB's WoWDynamicObject is a superset of HB's.

---

## Summary — All Gaps by Priority

### HIGH Priority (likely to cause bot failures)

| File | Member | Issue |
|------|--------|-------|
| WoWUnit | `Faction` (WoWFaction) | Many routines reference `.Faction` for faction checks |
| WoWUnit | `FactionTemplate` (WoWFactionTemplate) | Used in reaction calculations |
| WoWGameObject | `FactionTemplate` (WoWFactionTemplate) | Same issue — template object, not just ID |

### MEDIUM Priority (may affect some bot routines)

| File | Member | Issue |
|------|--------|-------|
| WoWUnit | `CurrentCastStartTime` | Cast-bar timing |
| WoWUnit | `CurrentCastEndTime` | Cast-bar timing |
| WoWUnit | `MovementInfo` | Full movement struct — CB reads fields individually |
| WoWPlayer | `IsTrackingStealthed` | Hunter stealth tracking |
| WoWPlayer | `ReleaseTimerIsVisible` | Death/release UI state |
| WoWPlayer | `PhysicalCritPercent` | Melee physical crit |
| WoWPlayer | `HonorableKills` (combined) | CB splits into Today/Yesterday, no total |
| WoWPlayer | `Glyphs` (as List) | CB has per-slot access, not list |
| LocalPlayer | `HearthstoneAreaId` | Zone-based hearth checks |
| LocalPlayer | `GetRaidMember(int)` | Returns WoWPartyMember object |
| WoWItem | `RandomSuffix` + class | Items with random suffixes |
| WoWPartyMember | `ContinentId` | Same-continent checks |
| WoWGameObject | `ParentRotation` type | `float` vs `Vector4` — compile error risk |

### LOW Priority (niche or rarely used)

| File | Member | Issue |
|------|--------|-------|
| WoWObject | `GetWorldMatrix()` on base | Rare — most callers use GameObj |
| WoWObject | `OnInvalidate` event | Only if bots subscribe |
| WoWUnit | `RenderFacing` | Visual-only facing |
| WoWUnit | `VanityPetGuid`/`VanityPet` | Companion pets |
| WoWUnit | `CreatureFamilyInfo` | Hunter pet families |
| WoWUnit | `TransportGuid` | Boat/zeppelin transport |
| WoWUnit | Stat modifier variants (×10) | Rarely used vs `Bonus{Stat}` |
| WoWPlayer | `GuildTimestamp` | Guild join date |
| WoWPlayer | `ChosenTitle` | Active title |
| WoWPlayer | `Inebriation` | Drunk level |
| WoWPlayer | `PvpMedalCount` | PvP medals |
| WoWPlayer | `BattlefieldArenaFaction` | Arena faction |
| WoWPlayer | `ArenaTeams` | Full arena team info |
| LocalPlayer | `Stable` | Pet stable list |
| LocalPlayer | `LearnableSpells` | Trainer spell list |
| WoWGameObject | `RelativeLocation` | Transport-relative position |
| WoWGameObject | `AnimationState` | GO animation byte |
| WoWGameObject | `FlagsUint` | Raw uint flags |
| WoWGameObject | `LockTypeEntry` | Lock sub-struct |
| WoWPartyMember | `FFAPvpFlagged` | FFA PvP flag |
| WoWPartyMember | `RAFLinked` | Recruit-A-Friend |
| WoWPartyMember | `VehicleSeatIndex` | Vehicle seat |
| WoWPartyMember | `AreaTableId` (functional) | Returns 0 always |

---

## Recommended Actions

1. **Implement `WoWFaction`/`WoWFactionTemplate` wrapper objects** — The `FactionId`/`FactionTemplateId` fields exist. Creating the wrapper classes and adding `.Faction`/`.FactionTemplate` properties on WoWUnit and WoWGameObject resolves the 3 HIGH-priority gaps.

2. **Add `CurrentCastStartTime`/`CurrentCastEndTime`** to WoWUnit — these are descriptor fields at known WotLK offsets. Trivial to add.

3. **Add combined `HonorableKills` property** to WoWPlayer — can sum Today+Yesterday or read from descriptor total field.

4. **Add `Glyphs` list property** to WoWPlayer — wraps existing `GetGlyph()`/`GetGlyphSlot()` into a `List<WoWGlyphInfo>`.

5. **Resolve `ParentRotation` type mismatch** in WoWGameObject — either add a `float` overload reading the single value HB expects, or ensure all consumers use the `Vector4`.

6. **Add `GetRaidMember(int)` returning `WoWPartyMember`** to LocalPlayer — wraps existing `GetRaidMemberGuid` + factory.

7. **Add `WoWItemRandomSuffix`** nested class to WoWItem — needed for items with random suffixes (common in WotLK loot).
