# CopilotBuddy — Complete Directory & Code Map

> Auto-generated map of all directories & .cs files with metadata.
> **412 .cs files under Styx/**, plus files in Bots/, CommonBehaviors/, TreeSharp/, GreenMagic/, Tripper/, UI/, and root.

---

## Table of Contents

1. [Styx/ (Core Engine)](#styx-core-engine)
2. [Bots/](#bots)
3. [CommonBehaviors/](#commonbehaviors)
4. [TreeSharp/](#treesharp)
5. [GreenMagic/](#greenmagic)
6. [Tripper/](#tripper)
7. [UI/](#ui)
8. [Root-Level .cs Files](#root-level-cs-files)
9. [Summary Statistics](#summary-statistics)

---

## Styx/ (Core Engine)

### Directory Tree

```
Styx/
├── Bot/Properties/
├── Combat/CombatRoutine/
├── Common/
├── CommonBot/
│   └── CharacterManagement/
├── Database/
├── Helpers/
├── Loaders/
├── Logic/
│   ├── AreaManagement/
│   │   └── Triangulation/
│   ├── BehaviorTree/
│   ├── Combat/
│   ├── Common/
│   ├── Inventory/
│   │   └── Frames/
│   │       ├── AuctionHouse/
│   │       ├── Gossip/
│   │       ├── LootFrame/
│   │       ├── MailBox/
│   │       ├── Merchant/
│   │       ├── Quest/
│   │       ├── Taxi/
│   │       └── Trainer/
│   ├── Pathing/
│   │   └── Interop/
│   ├── POI/
│   ├── Profiles/
│   │   └── Quest/
│   └── Questing/
├── Offsets/
├── Patchables/
├── Plugins/
│   └── PluginClass/
├── RemotableObjects/
├── Resources/
└── WoWInternals/
    ├── DBC/
    ├── Misc/
    ├── World/
    ├── WoWCache/
    └── WoWObjects/
```

---

### Styx/ (root-level files — 59 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| BotBase.cs | `Styx` | `public abstract class BotBase` | 6 | 4 |
| BotEvents.cs | `Styx` | `public static class BotEvents` | 15 | 0 |
| BotManager.cs | `Styx` | `public class BotManager` | 10 | 1 |
| CantCompileException.cs | `Styx` | `public class CantCompileException : ApplicationException` | 0 | 0 |
| FactionId.cs | `Styx` | `public enum FactionId : uint` | 0 | 0 |
| FrameLock.cs | `Styx` | `public class FrameLock : IDisposable` | 2 | 0 |
| GameError.cs | `Styx` | `public enum GameError` | 0 | 0 |
| GameObjectDataSlot.cs | `Styx` | `public enum GameObjectDataSlot` | 0 | 0 |
| Global.cs | `Styx` | `public static class Global` | 0 | 0 |
| HonorbuddyUnableToStartException.cs | `Styx` | `public class HonorbuddyUnableToStartException : UserException` | 3 | 0 |
| InventoryType.cs | `Styx` | `public enum InventoryType` | 0 | 0 |
| MirrorTimerType.cs | `Styx` | `public enum MirrorTimerType` | 0 | 0 |
| PulseFlags.cs | `Styx` | `public enum PulseFlags : uint` | 0 | 0 |
| QuestGiverStatus.cs | `Styx` | `public enum QuestGiverStatus` | 0 | 0 |
| ShapeshiftForm.cs | `Styx` | `public enum ShapeshiftForm` | 0 | 0 |
| SheathType.cs | `Styx` | `public enum SheathType : sbyte` | 0 | 0 |
| SkillLine.cs | `Styx` | `public enum SkillLine` | 0 | 0 |
| StatTypes.cs | `Styx` | `public enum StatTypes` | 0 | 0 |
| StyxWoW.cs | `Styx` | `public static class StyxWoW` | 4 | 0 |
| ThreatStatus.cs | `Styx` | `public enum ThreatStatus` | 0 | 0 |
| UnitNPCFlags.cs | `Styx` | `public enum UnitNPCFlags` | 0 | 0 |
| UserException.cs | `Styx` | `public class UserException : Exception` | 3 | 0 |
| WoWBagSlot.cs | `Styx` | `public enum WoWBagSlot` | 0 | 0 |
| WoWCreatureSkinType.cs | `Styx` | `public enum WoWCreatureSkinType` | 0 | 0 |
| WoWCreatureType.cs | `Styx` | `public enum WoWCreatureType` | 0 | 0 |
| WoWCursorType.cs | `Styx` | `public enum WoWCursorType` | 0 | 0 |
| WoWEquipSlot.cs | `Styx` | `public enum WoWEquipSlot` | 0 | 0 |
| WoWGameObjectType.cs | `Styx` | `public enum WoWGameObjectType : byte` | 0 | 0 |
| WoWGender.cs | `Styx` | `public enum WoWGender` | 0 | 0 |
| WoWInteractType.cs | `Styx` | `public enum WoWInteractType` | 0 | 0 |
| WoWInventorySlot.cs | `Styx` | `public enum WoWInventorySlot` | 0 | 0 |
| WoWItemAmmoType.cs | `Styx` | `public enum WoWItemAmmoType` | 0 | 0 |
| WoWItemArmorClass.cs | `Styx` | `public enum WoWItemArmorClass` | 0 | 0 |
| WoWItemBagFamily.cs | `Styx` | `public enum WoWItemBagFamily` | 0 | 0 |
| WoWItemBondType.cs | `Styx` | `public enum WoWItemBondType` | 0 | 0 |
| WoWItemClass.cs | `Styx` | `public enum WoWItemClass` | 0 | 0 |
| WoWItemContainerClass.cs | `Styx` | `public enum WoWItemContainerClass` | 0 | 0 |
| WoWItemGemClass.cs | `Styx` | `public enum WoWItemGemClass` | 0 | 0 |
| WoWItemGlyphClass.cs | `Styx` | `public enum WoWItemGlyphClass` | 0 | 0 |
| WoWItemKeyClass.cs | `Styx` | `public enum WoWItemKeyClass` | 0 | 0 |
| WoWItemMiscClass.cs | `Styx` | `public enum WoWItemMiscClass` | 0 | 0 |
| WoWItemProjectileClass.cs | `Styx` | `public enum WoWItemProjectileClass` | 0 | 0 |
| WoWItemQuality.cs | `Styx` | `public enum WoWItemQuality : uint` | 0 | 0 |
| WoWItemQuiverClass.cs | `Styx` | `public enum WoWItemQuiverClass` | 0 | 0 |
| WoWItemRecipeClass.cs | `Styx` | `public enum WoWItemRecipeClass` | 0 | 0 |
| WoWItemStatType.cs | `Styx` | `public enum WoWItemStatType` | 0 | 0 |
| WoWItemTradeGoodsClass.cs | `Styx` | `public enum WoWItemTradeGoodsClass` | 0 | 0 |
| WoWItemWeaponClass.cs | `Styx` | `public enum WoWItemWeaponClass` | 0 | 0 |
| WoWObjectType.cs | `Styx` | `public enum WoWObjectType : uint` | 0 | 0 |
| WoWObjectTypeFlag.cs | `Styx` | `public enum WoWObjectTypeFlag` | 0 | 0 |
| WoWPowerType.cs | `Styx` | `public enum WoWPowerType` | 0 | 0 |
| WoWPulsator.cs | `Styx` | `public static class WoWPulsator` | 1 | 0 |
| WoWQuestType.cs | `Styx` | `public enum WoWQuestType : uint` | 0 | 0 |
| WoWRace.cs | `Styx` | `public enum WoWRace` | 0 | 0 |
| WoWSocketColor.cs | `Styx` | `public enum WoWSocketColor : uint` | 0 | 0 |
| WoWStateFlag.cs | `Styx` | `public enum WoWStateFlag` | 0 | 0 |
| WoWUnitClassificationType.cs | `Styx` | `public enum WoWUnitClassificationType` | 0 | 0 |
| WoWUnitReaction.cs | `Styx` | `public enum WoWUnitReaction` | 0 | 0 |

### Styx/Bot/Properties/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Settings.cs | `Styx.Bot.Properties` | `internal sealed partial class Settings : ApplicationSettingsBase` | 0 | 0 |

### Styx/Combat/CombatRoutine/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| CombatRoutine.cs | `Styx.Combat.CombatRoutine` | `public abstract class CombatRoutine : MarshalByRefObject, IBehaviors, IDisposable, ICombatRoutine` | 12 | 2 |
| IBehaviors.cs | `Styx.Combat.CombatRoutine` | `public interface IBehaviors` | 0 | 0 |
| ICombatRoutine.cs | `Styx.Combat.CombatRoutine` | `public interface ICombatRoutine : IDisposable, IBehaviors` | 0 | 0 |
| WoWClass.cs | `Styx.Combat.CombatRoutine` | `public enum WoWClass : uint` | 0 | 0 |

### Styx/Common/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| MathEx.cs | `Styx.Common` | `public static class MathEx` | 14 | 0 |

### Styx/CommonBot/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| GameStats.cs | `Styx.CommonBot` | `public static class GameStats` | 12 | 16 |

### Styx/CommonBot/CharacterManagement/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AutoEquipper.cs | `Styx.CommonBot.CharacterManagement` | `public class AutoEquipper` | 1 | 0 |
| CharacterManager.cs | `Styx.CommonBot.CharacterManagement` | `public static class CharacterManager` | 0 | 0 |
| CharacterSettings.cs | `Styx.CommonBot.CharacterManagement` | `public class CharacterSettings` | 0 | 1 |
| QuestRewardItem.cs | `Styx.CommonBot.CharacterManagement` | `public class QuestRewardItem` | 0 | 4 |

### Styx/Database/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Connection.cs | `Styx.Database` | `public static class Connection` | 2 | 0 |
| CreatureSpawnQueries.cs | `Styx.Database` | `public static class CreatureSpawnQueries` | 4 | 0 |
| NpcQueries.cs | `Styx.Database` | `public static class NpcQueries` | 4 | 0 |
| NpcResult.cs | `Styx.Database` | `public class NpcResult` | 1 | 11 |

### Styx/Helpers/ (35 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AllocatedMemory.cs | `Styx.Helpers` | `public class AllocatedMemory : IDisposable` | 14 | 0 |
| AssemblyFactory.cs | `Styx.Helpers` | `public class AssemblyFactory` | 4 | 0 |
| BinaryExtensions.cs | `Styx.Helpers` | `public static class BinaryExtensions` | 2 | 0 |
| CapacityQueue.cs | `Styx.Helpers` | `public class CapacityQueue<T> : Queue<T>` | 2 | 1 |
| CharacterSettings.cs | `Styx.Helpers` | `public class CharacterSettings : Settings, INotifyPropertyChanged` | 3 | 1 |
| CircularQueue.cs | `Styx.Helpers` | `public class CircularQueue<T> : Queue<T>` | 6 | 1 |
| ClassCollection.cs | `Styx.Helpers` | `public class ClassCollection<T> : List<T>, IDisposable where T : class` | 3 | 0 |
| CombatAssistSettings.cs | `Styx.Helpers` | `public class CombatAssistSettings : Settings` | 2 | 1 |
| DefaultValueAttribute.cs | `Styx.Helpers` | `public class DefaultValueAttribute : Attribute` | 1 | 0 |
| DictionaryExtensions.cs | `Styx.Helpers` | `public static class DictionaryExtensions` | 3 | 0 |
| DualHashSet.cs | `Styx.Helpers` | `public class DualHashSet<T1, T2> : IEnumerable<object>` | 11 | 2 |
| Extensions.cs | `Styx.Helpers` | `public static class Extensions` | 20 | 0 |
| InactivityDetector.cs | `Styx.Helpers` | `public static class InactivityDetector` | 3 | 0 |
| InfoPanel.cs | `Styx.Helpers` | `public static class InfoPanel` | 12 | 12 |
| IRangeAble.cs | `Styx.Helpers` | `public interface IRangeAble` | 0 | 0 |
| KeyboardManager.cs | `Styx.Helpers` | `public static class KeyboardManager` | 7 | 0 |
| KeyHelpers.cs | `Styx.Helpers` | `public static class KeyHelpers` | 1 | 0 |
| LevelbotSettings.cs | `Styx.Helpers` | `public class LevelbotSettings : Settings` | 2 | 3 |
| Logging.cs | `Styx.Helpers` | `public enum LogLevel` (+ Logging class) | 42 | 9 |
| PerformanceTimer.cs | `Styx.Helpers` | `public class PerformanceTimer : IDisposable` | 11 | 0 |
| PerFrameCachedValue.cs | `Styx.Helpers` | `public class PerFrameCachedValue<T>` | 2 | 0 |
| PluginSourceEnum.cs | `Styx.Helpers` | `public enum PluginSourceEnum` | 0 | 0 |
| PVPSettings.cs | `Styx.Helpers` | `public class PVPSettings : Settings` | 2 | 3 |
| QuestSettings.cs | `Styx.Helpers` | `public class QuestSettings : Settings` | 2 | 2 |
| Range.cs | `Styx.Helpers` | `public struct Range : IEquatable<Range>` | 6 | 2 |
| RangedDictionary.cs | `Styx.Helpers` | `public class RangedDictionary<T> : Dictionary<Range, List<T>> where T : IRangeAble` | 7 | 0 |
| SettingAttribute.cs | `Styx.Helpers` | `public class SettingAttribute : Attribute` | 3 | 0 |
| Settings.cs | `Styx.Helpers` | `public abstract class Settings` | 9 | 0 |
| StyxSettings.cs | `Styx.Helpers` | `public class StyxSettings : Settings` | 1 | 0 |
| UISettings.cs | `Styx.Helpers` | `public class UISettings : Settings` | 2 | 11 |
| Utilities.cs | `Styx.Helpers` | `public static class Utilities` | 2 | 0 |
| ValuePair.cs | `Styx.Helpers` | `public struct ValuePair<T1, T2>` | 1 | 0 |
| WaitTimer.cs | `Styx.Helpers` | `public class WaitTimer` | 6 | 3 |
| WoWMathHelper.cs | `Styx.Helpers` | `public static class WoWMathHelper` | 19 | 0 |
| XmlExtensions.cs | `Styx.Helpers` | `public static class XmlExtensions` | 14 | 0 |

### Styx/Loaders/ (6 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AssemblyVerifier.cs | `Styx.Loaders` | `public static class AssemblyVerifier` | 1 | 0 |
| CustomClassLoader.cs | `Styx.Loaders` | `public static class CustomClassLoader` | 1 | 0 |
| DllLoader.cs | `Styx.Loaders` | `public class DllLoader<T> : List<T>` | 1 | 0 |
| DynamicLoader.cs | `Styx.Loaders` | `public class DynamicLoader<T> : List<T>` | 1 | 3 |
| FrameworkVersionDetection.cs | `Styx.Loaders` | `public static class FrameworkVersionDetection` | 0 | 0 |
| SourceCompiler.cs | `Styx.Loaders` | `internal class SourceCompiler` | 4 | 7 |

### Styx/Logic/ (root-level — 41 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| ArenaType.cs | `Styx.Logic` | `public enum ArenaType` | 0 | 0 |
| BattlefieldWinner.cs | `Styx.Logic` | `public enum BattlefieldWinner` | 0 | 0 |
| BattlegroundJoinError.cs | `Styx.Logic` | `public enum BattlegroundJoinError` | 0 | 0 |
| Battlegrounds.cs | `Styx.Logic` | `public static class Battlegrounds` | 1 | 2 |
| BattlegroundSide.cs | `Styx.Logic` | `public enum BattlegroundSide` | 0 | 0 |
| BattlegroundStatus.cs | `Styx.Logic` | `public enum BattlegroundStatus : uint` | 0 | 0 |
| BattlegroundType.cs | `Styx.Logic` | `public enum BattlegroundType` | 0 | 0 |
| Blacklist.cs | `Styx.Logic` | `public static class Blacklist` | 6 | 0 |
| BotSequence.cs | `Styx.Logic` | `public enum BotSequence` | 0 | 0 |
| BuyItemsEventHandler.cs | `Styx.Logic` | `public class BuyItemsEventArgs` | 1 | 1 |
| CanMountDelegate.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| DestroyedGateFlags.cs | `Styx.Logic` | `public enum DestroyedGateFlags` | 0 | 0 |
| FlightPaths.cs | `Styx.Logic` | `public enum FlightPathReason` (+ FlightPaths class) | 16 | 11 |
| Iconflags.cs | `Styx.Logic` | `public enum Iconflags` | 0 | 0 |
| IncludeTargetsFilterDelegate.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| Landmarks.cs | `Styx.Logic` | `public class Landmarks` | 6 | 0 |
| LocationRetriever.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| LootTargeting.cs | `Styx.Logic` | `public class LootTargeting : Targeting` | 3 | 0 |
| MailItemsEventArgs.cs | `Styx.Logic` | `public class MailItemsEventArgs` | 1 | 1 |
| MailItemsEventHandler.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| Mount.cs | `Styx.Logic` | `public static class Mount` | 16 | 0 |
| MountHelper.cs | `Styx.Logic` | `public static class MountHelper` | 1 | 8 |
| MountUpEventArgs.cs | `Styx.Logic` | `public class MountUpEventArgs : EventArgs` | 2 | 3 |
| QueuedBattlegroundInfo.cs | `Styx.Logic` | `public struct QueuedBattlegroundInfo` | 0 | 0 |
| RaFHelper.cs | `Styx.Logic` | `public static class RaFHelper` | 5 | 0 |
| RemoveTargetsFilterDelegate.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| SellItemsEventArgs.cs | `Styx.Logic` | `public class SellItemsEventArgs` | 1 | 2 |
| SequenceManager.cs | `Styx.Logic` | `public static class SequenceManager` | 5 | 0 |
| SotAGate.cs | `Styx.Logic` | `public class SotAGate : WoWLandMark` | 1 | 0 |
| SotAGateType.cs | `Styx.Logic` | `public enum SotAGateType` | 0 | 0 |
| SotaLandmarks.cs | `Styx.Logic` | `public enum SotaLandmarks : uint` | 0 | 0 |
| SotAObjective.cs | `Styx.Logic` | `public enum SotAObjective` | 0 | 0 |
| Targeting.cs | `Styx.Logic` | `public class Targeting` | 5 | 0 |
| TargetListUpdateFinishedDelegate.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| TaxiNodeInfo.cs | `Styx.Logic` | `public class TaxiNodeInfo` | 6 | 1 |
| VendorItemsEventHandler.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| Vendors.cs | `Styx.Logic` | `public static class Vendors` | 5 | 7 |
| WeighTargetsDelegate.cs | `Styx.Logic` | *(delegate)* | 1 | 0 |
| WoWLandMark.cs | `Styx.Logic` | `public class WoWLandMark` | 9 | 0 |
| WoWSkill.cs | `Styx.Logic` | `public class WoWSkill` | 3 | 0 |

### Styx/Logic/AreaManagement/ (10 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Area.cs | `Styx.Logic.AreaManagement` | `public abstract class Area : IEquatable<Area>` | 8 | 3 |
| AreaManager.cs | `Styx.Logic.AreaManagement` | `public class AreaManager` | 4 | 0 |
| AreaType.cs | `Styx.Logic.AreaManagement` | `public enum AreaType` | 0 | 0 |
| GrindArea.cs | `Styx.Logic.AreaManagement` | `public class GrindArea : Area` | 4 | 9 |
| Hotspot.cs | `Styx.Logic.AreaManagement` | `public class Hotspot` | 8 | 1 |
| HotspotExtensions.cs | `Styx.Logic.AreaManagement` | `public static class HotspotExtensions` | 1 | 0 |
| HotspotManager.cs | `Styx.Logic.AreaManagement` | `public class HotspotManager` | 6 | 1 |
| PolygonArea.cs | `Styx.Logic.AreaManagement` | `public abstract class PolygonArea : Area` | 1 | 0 |
| PvPArea.cs | `Styx.Logic.AreaManagement` | `public class PvPArea : PolygonArea` | 2 | 0 |
| QuestArea.cs | `Styx.Logic.AreaManagement` | `public class QuestArea : GrindArea` | 2 | 2 |

### Styx/Logic/AreaManagement/Triangulation/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Edge.cs | `Styx.Logic.AreaManagement.Triangulation` | `public struct Edge : IEquatable<Edge>` | 6 | 0 |
| Triangle.cs | `Styx.Logic.AreaManagement.Triangulation` | `public struct Triangle` | 1 | 0 |

### Styx/Logic/BehaviorTree/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| LogicType.cs | `Styx.Logic.BehaviorTree` | `public enum LogicType` | 0 | 0 |
| StatusTextChangedEventArgs.cs | `Styx.Logic.BehaviorTree` | `public class StatusTextChangedEventArgs : EventArgs` | 0 | 2 |
| TreeRoot.cs | `Styx.Logic.BehaviorTree` | `public static class TreeRoot` | 3 | 2 |
| TreeRootState.cs | `Styx.Logic.BehaviorTree` | `public enum TreeRootState` | 0 | 0 |

### Styx/Logic/Combat/ (17 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Healing.cs | `Styx.Logic.Combat` | `public static class Healing` | 4 | 0 |
| LegacySpellManager.cs | `Styx.Logic.Combat` | `public static class LegacySpellManager` | 12 | 0 |
| RoutineManager.cs | `Styx.Logic.Combat` | `public static class RoutineManager` | 3 | 0 |
| SpellEffect.cs | `Styx.Logic.Combat` | `public class SpellEffect` | 1 | 17 |
| SpellEntry.cs | `Styx.Logic.Combat` | `public struct SpellEntry` | 3 | 0 |
| SpellManager.cs | `Styx.Logic.Combat` | `public static class SpellManager` | 22 | 0 |
| SpellManagerEx.cs | `Styx.Logic.Combat` | `public class SpellManagerEx` | 59 | 0 |
| WoWApplyAuraType.cs | `Styx.Logic.Combat` | `public enum WoWApplyAuraType` | 0 | 0 |
| WoWAura.cs | `Styx.Logic.Combat` | `public class WoWAura : IEquatable<WoWAura>` | 18 | 0 |
| WoWAuraCollection.cs | `Styx.Logic.Combat` | `public class WoWAuraCollection : List<WoWAura>` | 18 | 0 |
| WoWDispelType.cs | `Styx.Logic.Combat` | `public enum WoWDispelType` | 0 | 0 |
| WoWPetSpell.cs | `Styx.Logic.Combat` | `public class WoWPetSpell` | 1 | 5 |
| WoWSpell.cs | `Styx.Logic.Combat` | `public class WoWSpell : IEquatable<WoWSpell>` | 5 | 0 |
| WoWSpellEffectType.cs | `Styx.Logic.Combat` | `public enum WoWSpellEffectType` | 0 | 0 |
| WoWSpellFocus.cs | `Styx.Logic.Combat` | `public enum WoWSpellFocus` | 0 | 0 |
| WoWSpellMechanic.cs | `Styx.Logic.Combat` | `public enum WoWSpellMechanic` | 0 | 0 |
| WoWSpellSchool.cs | `Styx.Logic.Combat` | `public enum WoWSpellSchool` | 0 | 0 |

### Styx/Logic/Common/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Rest.cs | `Styx.Logic.Common` | `public static class Rest` | 3 | 4 |

### Styx/Logic/Inventory/ (8 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Consumable.cs | `Styx.Logic.Inventory` | `public static class Consumable` | 4 | 0 |
| InventoryManager.cs | `Styx.Logic.Inventory` | `public static class InventoryManager` | 3 | 0 |
| InventorySlot.cs | `Styx.Logic.Inventory` | `public enum InventorySlot` | 0 | 0 |
| LootRoll.cs | `Styx.Logic.Inventory` | `public static class LootRoll` | 6 | 0 |
| Stat.cs | `Styx.Logic.Inventory` | `public enum Stat` | 0 | 0 |
| WeightSet.cs | `Styx.Logic.Inventory` | `public class WeightSet` | 5 | 2 |
| WeightSetEx.cs | `Styx.Logic.Inventory` | `public class WeightSetEx : IDisposable` | 9 | 4 |
| WoWPrice.cs | `Styx.Logic.Inventory` | `public class WoWPrice : IEquatable<WoWPrice>, IComparable<WoWPrice>` | 29 | 3 |

### Styx/Logic/Inventory/Frames/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Frame.cs | `Styx.Logic.Inventory.Frames` | `public class Frame` | 3 | 1 |

### Styx/Logic/Inventory/Frames/AuctionHouse/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AuctionEntry.cs | `Styx.Logic.Inventory.Frames.AuctionHouse` | `public struct AuctionEntry` | 0 | 0 |
| AuctionType.cs | `Styx.Logic.Inventory.Frames.AuctionHouse` | `public enum AuctionType` | 0 | 0 |
| PostTime.cs | `Styx.Logic.Inventory.Frames.AuctionHouse` | `public enum PostTime` | 0 | 0 |
| WoWAucEnchantInfo.cs | `Styx.Logic.Inventory.Frames.AuctionHouse` | `public struct WoWAucEnchantInfo` | 0 | 0 |

### Styx/Logic/Inventory/Frames/Gossip/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| GossipEntry.cs | `Styx.Logic.Inventory.Frames.Gossip` | `public struct GossipEntry` | 0 | 0 |
| GossipFrame.cs | `Styx.Logic.Inventory.Frames.Gossip` | `public class GossipFrame : Frame` | 7 | 0 |
| GossipQuestEntry.cs | `Styx.Logic.Inventory.Frames.Gossip` | `public class GossipQuestEntry` | 0 | 0 |

### Styx/Logic/Inventory/Frames/LootFrame/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| LootFrame.cs | `Styx.Logic.Inventory.Frames.LootFrame` | `public class LootFrame : Frame` | 8 | 0 |
| LootRarity.cs | `Styx.Logic.Inventory.Frames.LootFrame` | `public enum LootRarity` | 0 | 0 |
| LootSlotInfo.cs | `Styx.Logic.Inventory.Frames.LootFrame` | `public class LootSlotInfo` | 1 | 0 |

### Styx/Logic/Inventory/Frames/MailBox/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| MailFrame.cs | `Styx.Logic.Inventory.Frames.MailBox` | `public class MailFrame : Frame` | 12 | 0 |

### Styx/Logic/Inventory/Frames/Merchant/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| ItemQuality.cs | `Styx.Logic.Inventory.Frames.Merchant` | `public enum ItemQuality` | 0 | 0 |
| MerchantFrame.cs | `Styx.Logic.Inventory.Frames.Merchant` | `public class MerchantFrame : Frame` | 16 | 0 |
| MerchantItem.cs | `Styx.Logic.Inventory.Frames.Merchant` | `public class MerchantItem` | 3 | 1 |

### Styx/Logic/Inventory/Frames/Quest/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| QuestFrame.cs | `Styx.Logic.Inventory.Frames.Quest` | `public class QuestFrame : Frame` | 8 | 0 |

### Styx/Logic/Inventory/Frames/Taxi/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| TaxiFrame.cs | `Styx.Logic.Inventory.Frames.Taxi` | `public class TaxiFrame : Frame` | 6 | 1 |

### Styx/Logic/Inventory/Frames/Trainer/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| TrainerFrame.cs | `Styx.Logic.Inventory.Frames.Trainer` | `public class TrainerFrame : Frame` | 11 | 0 |
| TrainerServiceFilter.cs | `Styx.Logic.Inventory.Frames.Trainer` | `public enum TrainerServiceFilter` | 0 | 0 |

### Styx/Logic/Pathing/ (11 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AvoidanceManager.cs | `Styx.Logic.Pathing` | `public static class AvoidanceManager` | 8 | 0 |
| BlackspotManager.cs | `Styx.Logic.Pathing` | `public static class BlackspotManager` | 19 | 2 |
| Flightor.cs | `Styx.Logic.Pathing` | `public static class Flightor` | 6 | 0 |
| IStuckHandler.cs | `Styx.Logic.Pathing` | `public interface IStuckHandler` | 0 | 0 |
| MeshHeightHelper.cs | `Styx.Logic.Pathing` | `public static class MeshHeightHelper` | 2 | 0 |
| MoveResult.cs | `Styx.Logic.Pathing` | `public enum MoveResult` | 0 | 0 |
| Navigator.cs | `Styx.Logic.Pathing` | `public static class Navigator` | 19 | 3 |
| PointLocationType.cs | `Styx.Logic.Pathing` | `public enum PointLocationType` | 0 | 0 |
| StuckDetector.cs | `Styx.Logic.Pathing` | `public static class StuckDetector` | 0 | 0 |
| StuckHandler.cs | `Styx.Logic.Pathing` | `internal class StuckHandler : IStuckHandler` | 4 | 0 |
| WoWPoint.cs | `Styx.Logic.Pathing` | `public struct WoWPoint : IEquatable<WoWPoint>, IRangeAble` | 40 | 0 |

### Styx/Logic/Pathing/Interop/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| KeyboardMover.cs | `Styx.Logic.Pathing.Interop` | `public class KeyboardMover : IMover` | 6 | 0 |
| LocalPlayerMover.cs | `Styx.Logic.Pathing.Interop` | `public enum MoveDirection` (+ LocalPlayerMover) | 6 | 0 |
| WorldInfoProvider.cs | `Styx.Logic.Pathing.Interop` | `public interface IWorldInfoProvider` | 1 | 0 |
| WorldObject.cs | `Styx.Logic.Pathing.Interop` | `public enum WorldObjectType` (+ WorldObject class) | 2 | 0 |

### Styx/Logic/POI/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| BotPoi.cs | `Styx.Logic.POI` | `public class BotPoi` | 10 | 4 |
| PoiType.cs | `Styx.Logic.POI` | `public enum PoiType` | 0 | 0 |
| PoiTypeExtensions.cs | `Styx.Logic.POI` | `public static class PoiTypeExtensions` | 1 | 0 |

### Styx/Logic/Profiles/ (21 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Blackspot.cs | `Styx.Logic.Profiles` | `public struct Blackspot : IEquatable<Blackspot>` | 8 | 0 |
| HotspotCollection.cs | `Styx.Logic.Profiles` | `public class HotspotCollection : List<WoWPoint>` | 4 | 0 |
| Mailbox.cs | `Styx.Logic.Profiles` | `public class Mailbox : IEquatable<Mailbox>` | 6 | 1 |
| MailboxManager.cs | `Styx.Logic.Profiles` | `public class MailboxManager` | 4 | 3 |
| Profile.cs | `Styx.Logic.Profiles` | `public class Profile : IEquatable<Profile>` | 11 | 9 |
| ProfileAttributeExpectedException.cs | `Styx.Logic.Profiles` | `public class ProfileAttributeExpectedException : ProfileException` | 2 | 0 |
| ProfileException.cs | `Styx.Logic.Profiles` | `public class ProfileException : Exception` | 3 | 0 |
| ProfileHelper.cs | `Styx.Logic.Profiles` | `internal static class ProfileHelper` | 4 | 0 |
| ProfileManager.cs | `Styx.Logic.Profiles` | `public static class ProfileManager` | 4 | 1 |
| ProfileMissingAttributeException.cs | `Styx.Logic.Profiles` | `public class ProfileMissingAttributeException : ProfileException` | 1 | 0 |
| ProfileMissingAttributeExceptionT.cs | `Styx.Logic.Profiles` | `public class ProfileMissingAttributeException<T> : ProfileException` | 1 | 0 |
| ProfileMissingElementException.cs | `Styx.Logic.Profiles` | `public class ProfileMissingElementException : ProfileException` | 1 | 0 |
| ProfileTagExpectedExceptionT.cs | `Styx.Logic.Profiles` | `public class ProfileTagExpectedException<T> : ProfileException` | 2 | 0 |
| ProfileUnknownAttributeException.cs | `Styx.Logic.Profiles` | `public class ProfileUnknownAttributeException : ProfileException` | 2 | 0 |
| ProfileUnknownElementException.cs | `Styx.Logic.Profiles` | `public class ProfileUnknownElementException : ProfileException` | 2 | 0 |
| ProtectedItemsManager.cs | `Styx.Logic.Profiles` | `public static class ProtectedItemsManager` | 9 | 0 |
| Trainer.cs | `Styx.Logic.Profiles` | `public class Trainer : IEquatable<Trainer>` | 6 | 4 |
| TrainerManager.cs | `Styx.Logic.Profiles` | `public class TrainerManager` | 1 | 0 |
| Vendor.cs | `Styx.Logic.Profiles` | `public class Vendor : IEquatable<Vendor>` | 5 | 6 |
| VendorManager.cs | `Styx.Logic.Profiles` | `public class VendorManager` | 4 | 3 |
| VendorTypeExtensions.cs | `Styx.Logic.Profiles` | `public static class VendorTypeExtensions` | 1 | 0 |

### Styx/Logic/Profiles/Quest/ (39 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| AbandonQuestNode.cs | `Styx.Logic.Profiles.Quest` | `public class AbandonQuestNode : OrderNode` | 3 | 1 |
| CheckpointNode.cs | `Styx.Logic.Profiles.Quest` | `public class CheckpointNode : OrderNode` | 4 | 1 |
| ClearGrindAreaNode.cs | `Styx.Logic.Profiles.Quest` | `public class ClearGrindAreaNode : OrderNode` | 3 | 0 |
| ClearMailboxNode.cs | `Styx.Logic.Profiles.Quest` | `public class ClearMailboxNode : OrderNode` | 3 | 0 |
| ClearVendorNode.cs | `Styx.Logic.Profiles.Quest` | `public class ClearVendorNode : OrderNode` | 3 | 0 |
| CodeNode.cs | `Styx.Logic.Profiles.Quest` | `public class CodeNode : OrderNode` | 3 | 3 |
| CollectFrom.cs | `Styx.Logic.Profiles.Quest` | `public class CollectFrom` | 2 | 3 |
| CollectFromCollection.cs | `Styx.Logic.Profiles.Quest` | `public class CollectFromCollection : List<CollectFrom>` | 6 | 0 |
| CollectFromType.cs | `Styx.Logic.Profiles.Quest` | `public enum CollectFromType` | 0 | 0 |
| CollectItemObjectiveInfo.cs | `Styx.Logic.Profiles.Quest` | `public class CollectItemObjectiveInfo : ObjectiveInfo` | 1 | 6 |
| ConditionHelper.cs | `Styx.Logic.Profiles.Quest` | `public class ConditionHelper` | 10 | 0 |
| DisableRepairNode.cs | `Styx.Logic.Profiles.Quest` | `public class DisableRepairNode : OrderNode` | 3 | 0 |
| Else.cs | `Styx.Logic.Profiles.Quest` | `public class Else` | 2 | 1 |
| ElseIf.cs | `Styx.Logic.Profiles.Quest` | `public class ElseIf` | 2 | 2 |
| EnableRepairNode.cs | `Styx.Logic.Profiles.Quest` | `public class EnableRepairNode : OrderNode` | 3 | 0 |
| GrindToNode.cs | `Styx.Logic.Profiles.Quest` | `public class GrindToNode : OrderNode` | 4 | 3 |
| IfNode.cs | `Styx.Logic.Profiles.Quest` | `public class IfNode : OrderNode, INodeContainer` | 6 | 4 |
| INodeContainer.cs | `Styx.Logic.Profiles.Quest` | `public interface INodeContainer` | 0 | 0 |
| KillMobObjectiveInfo.cs | `Styx.Logic.Profiles.Quest` | `public class KillMobObjectiveInfo : ObjectiveInfo` | 1 | 5 |
| MoveToNode.cs | `Styx.Logic.Profiles.Quest` | `public class MoveToNode : OrderNode` | 3 | 4 |
| ObjectiveInfo.cs | `Styx.Logic.Profiles.Quest` | `public class ObjectiveInfo` | 2 | 1 |
| ObjectiveNode.cs | `Styx.Logic.Profiles.Quest` | `public class ObjectiveNode : OrderNode` | 3 | 6 |
| ObjectiveType.cs | `Styx.Logic.Profiles.Quest` | `public enum ObjectiveType` | 0 | 0 |
| OrderNode.cs | `Styx.Logic.Profiles.Quest` | `public abstract class OrderNode` | 1 | 2 |
| OrderNodeCollection.cs | `Styx.Logic.Profiles.Quest` | `public class OrderNodeCollection : List<OrderNode>, INodeContainer` | 5 | 1 |
| OrderNodeType.cs | `Styx.Logic.Profiles.Quest` | `public enum OrderNodeType` | 0 | 0 |
| PickUpNode.cs | `Styx.Logic.Profiles.Quest` | `public class PickUpNode : OrderNode` | 3 | 6 |
| ProfileHelperFunctionsBase.cs | `Styx.Logic.Profiles.Quest` | `public class ProfileHelperFunctionsBase` | 0 | 0 |
| QuestBehaviorHelper.cs | `Styx.Logic.Profiles.Quest` | `public class QuestBehaviorHelper` | 4 | 1 |
| QuestInfo.cs | `Styx.Logic.Profiles.Quest` | `public class QuestInfo` | 6 | 3 |
| QuestObjectType.cs | `Styx.Logic.Profiles.Quest` | `public enum QuestObjectType` | 0 | 0 |
| SetGrindAreaNode.cs | `Styx.Logic.Profiles.Quest` | `public class SetGrindAreaNode : OrderNode` | 4 | 1 |
| SetMailboxNode.cs | `Styx.Logic.Profiles.Quest` | `public class SetMailboxNode : OrderNode` | 4 | 1 |
| SetVendorNode.cs | `Styx.Logic.Profiles.Quest` | `public class SetVendorNode : OrderNode` | 4 | 1 |
| TurnInNode.cs | `Styx.Logic.Profiles.Quest` | `public class TurnInNode : OrderNode` | 3 | 6 |
| TurnInObjectiveInfo.cs | `Styx.Logic.Profiles.Quest` | `public class TurnInObjectiveInfo : ObjectiveInfo` | 1 | 1 |
| UseItemNode.cs | `Styx.Logic.Profiles.Quest` | `public class UseItemNode : OrderNode` | 4 | 6 |
| UseObjectObjectiveInfo.cs | `Styx.Logic.Profiles.Quest` | `public class UseObjectObjectiveInfo : ObjectiveInfo` | 2 | 3 |
| WhileNode.cs | `Styx.Logic.Profiles.Quest` | `public class WhileNode : OrderNode, INodeContainer` | 4 | 2 |

### Styx/Logic/Questing/ (16 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| CustomForcedBehavior.cs | `Styx.Logic.Questing` | `public abstract class CustomForcedBehavior` | 76 | 2 |
| PlayerQuest.cs | `Styx.Logic.Questing` | `public class PlayerQuest : Quest` | 3 | 0 |
| Quest.cs | `Styx.Logic.Questing` | `public class Quest` | 7 | 1 |
| QuestDescriptorData.cs | `Styx.Logic.Questing` | `public struct QuestDescriptorData` | 3 | 0 |
| Questing.cs | `Styx.Logic.Questing` | `public static class Questing` | 2 | 0 |
| QuestLog.cs | `Styx.Logic.Questing` | `public class QuestLog` | 12 | 0 |
| QuestLogEntry.cs | `Styx.Logic.Questing` | `public struct QuestLogEntry` | 1 | 0 |
| QuestStepLocation.cs | `Styx.Logic.Questing` | `public struct QuestStepLocation` | 0 | 0 |
| Vector2i.cs | `Styx.Logic.Questing` | `public struct Vector2i` | 1 | 0 |
| WoWDescriptorQuest.cs | `Styx.Logic.Questing` | `public struct WoWDescriptorQuest` | 0 | 0 |
| WoWDescriptorQuestFlags.cs | `Styx.Logic.Questing` | `public enum WoWDescriptorQuestFlags : uint` | 0 | 0 |
| WoWQuestCompletionInfo.cs | `Styx.Logic.Questing` | `public struct WoWQuestCompletionInfo` | 0 | 0 |
| WoWQuestCurrentStep.cs | `Styx.Logic.Questing` | `public struct WoWQuestCurrentStep` | 0 | 0 |
| WoWQuestState.cs | `Styx.Logic.Questing` | `public enum WoWQuestState` | 0 | 0 |
| WoWQuestStep.cs | `Styx.Logic.Questing` | `public struct WoWQuestStep` | 0 | 0 |
| WoWQuestStepsCollection.cs | `Styx.Logic.Questing` | `public struct WoWQuestStepsCollection` | 0 | 0 |

### Styx/Offsets/ (11 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| GlobalOffsets.cs | `Styx.Offsets` | `public static class GlobalOffsets` | 0 | 0 |
| NpcFlags.cs | `Styx` | `public enum NpcFlags : uint` | 0 | 0 |
| UnitFlags.cs | `Styx` | `public enum UnitFlags : uint` | 0 | 0 |
| WoWContainerFields.cs | `Styx.Offsets` | `public enum WoWContainerFields : uint` | 0 | 0 |
| WoWCorpseFields.cs | `Styx.Offsets` | `public enum WoWCorpseFields : uint` | 0 | 0 |
| WoWDynamicObjectFields.cs | `Styx.Offsets` | `public enum WoWDynamicObjectFields : uint` | 0 | 0 |
| WoWGameObjectFields.cs | `Styx.Offsets` | `public enum WoWGameObjectFields : uint` | 0 | 0 |
| WoWItemFields.cs | `Styx.Offsets` | `public enum WoWItemFields : uint` | 0 | 0 |
| WoWObjectFields.cs | `Styx.Offsets` | `public enum WoWObjectFields` | 0 | 0 |
| WoWPlayerFields.cs | `Styx.Offsets` | `public enum WoWPlayerFields : uint` | 0 | 0 |
| WoWUnitFields.cs | `Styx.Offsets` | `public enum WoWUnitFields` | 0 | 0 |

### Styx/Patchables/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| ClientDb.cs | `Styx.Patchables` | `public enum ClientDb` | 0 | 0 |
| GlobalOffsets.cs | `Styx.Patchables` | `public enum GlobalOffsets` | 0 | 0 |

### Styx/Plugins/ (4 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| CompilerErrorsException.cs | `Styx.Plugins` | `public class CompilerErrorsException : Exception` | 1 | 0 |
| PluginContainer.cs | `Styx.Plugins` | `public class PluginContainer : INotifyPropertyChanged` | 1 | 1 |
| PluginManager.cs | `Styx.Plugins` | `public static class PluginManager` | 5 | 3 |
| PluginWrapper.cs | `Styx.Plugins` | `public class PluginWrapper` | 4 | 0 |

### Styx/Plugins/PluginClass/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| HBPlugin.cs | `Styx.Plugins.PluginClass` | `public abstract class HBPlugin` | 6 | 3 |
| IHBPlugin.cs | `Styx.Plugins.PluginClass` | `public interface IHBPlugin : IEquatable<IHBPlugin>, IDisposable` | 0 | 0 |

### Styx/RemotableObjects/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| BotMessage.cs | `Styx.RemotableObjects` | `public class BotMessage : MarshalByRefObject, ISerializable` | 4 | 3 |
| Cache.cs | `Styx.RemotableObjects` | `public class Cache` | 1 | 1 |
| IObserver.cs | `Styx.RemotableObjects` | `public interface IObserver` | 0 | 0 |

### Styx/Resources/ (1 file)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| ResourceManager.cs | `Styx.Resources` | `public static class ResourceManager` | 1 | 0 |

### Styx/WoWInternals/ (40 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| BagStructure.cs | `Styx.WoWInternals` | `internal struct BagStructure` | 0 | 0 |
| ChatMessageEventArgs.cs | `Styx.WoWInternals` | `public class ChatMessageEventArgs : EventArgs` | 1 | 1 |
| ChatMessageHandler.cs | `Styx.WoWInternals` | *(delegate)* | 1 | 0 |
| ChatType.cs | `Styx.WoWInternals` | `public enum ChatType : byte` | 0 | 0 |
| Lua.cs | `Styx.WoWInternals` | `public static class Lua` | 22 | 1 |
| LuaEventArgs.cs | `Styx.WoWInternals` | `public class LuaEventArgs : EventArgs` | 1 | 3 |
| LuaEventHandlerDelegate.cs | `Styx.WoWInternals` | *(delegate)* | 1 | 0 |
| LuaEvents.cs | `Styx.WoWInternals` | `public class LuaEvents` | 5 | 0 |
| LuaEventWait.cs | `Styx.WoWInternals` | `public class LuaEventWait : IDisposable` | 5 | 1 |
| LuaNode.cs | `Styx.WoWInternals` | `public class LuaNode` | 2 | 1 |
| LuaTable.cs | `Styx.WoWInternals` | `public class LuaTable` | 3 | 1 |
| LuaTKey.cs | `Styx.WoWInternals` | `public class LuaTKey` | 1 | 1 |
| LuaTString.cs | `Styx.WoWInternals` | `public class LuaTString` | 1 | 1 |
| LuaTValue.cs | `Styx.WoWInternals` | `public class LuaTValue` | 2 | 1 |
| LuaType.cs | `Styx.WoWInternals` | `public enum LuaType` | 0 | 0 |
| LuaValue.cs | `Styx.WoWInternals` | `public class LuaValue` | 2 | 1 |
| NativeLuaCommonHeader.cs | `Styx.WoWInternals` | `public struct NativeLuaCommonHeader` | 0 | 0 |
| NativeLuaNode.cs | `Styx.WoWInternals` | `public struct NativeLuaNode` | 0 | 0 |
| NativeLuaTable.cs | `Styx.WoWInternals` | `public struct NativeLuaTable` | 0 | 0 |
| NativeLuaTKey.cs | `Styx.WoWInternals` | `public struct NativeLuaTKey` | 0 | 0 |
| NativeLuaTString.cs | `Styx.WoWInternals` | `public struct NativeLuaTString` | 0 | 0 |
| NativeLuaTValue.cs | `Styx.WoWInternals` | `public struct NativeLuaTValue` | 0 | 0 |
| NativeLuaValue.cs | `Styx.WoWInternals` | `public struct NativeLuaValue` | 0 | 0 |
| ObjectListUpdateFinishedDelegate.cs | `Styx.WoWInternals` | *(delegate)* | 1 | 0 |
| ObjectManager.cs | `Styx.WoWInternals` | `public static class ObjectManager` | 7 | 4 |
| SpellDb.cs | `Styx.WoWInternals` | `public static class SpellDb` | 5 | 3 |
| TaxiNodeType.cs | `Styx.WoWInternals` | `public enum TaxiNodeType` | 0 | 0 |
| UnitPvPStateFlags.cs | `Styx.WoWInternals` | `public enum UnitPvPStateFlags : byte` | 0 | 0 |
| WoWBag.cs | `Styx.WoWInternals` | `public class WoWBag` | 6 | 2 |
| WoWCamera.cs | `Styx.WoWInternals` | `public class WoWCamera` | 1 | 0 |
| WoWChat.cs | `Styx.WoWInternals` | `public static class WoWChat` | 1 | 0 |
| WoWChatMessage.cs | `Styx.WoWInternals` | `public class WoWChatMessage` | 2 | 0 |
| WoWCurrency.cs | `Styx.WoWInternals` | `public class WoWCurrency` | 3 | 0 |
| WoWDb.cs | `Styx.WoWInternals` | `public class WoWDb` | 4 | 0 |
| WoWFaction.cs | `Styx.WoWInternals` | `public class WoWFaction` | 6 | 1 |
| WoWFactionTemplate.cs | `Styx.WoWInternals` | `public class WoWFactionTemplate` | 4 | 2 |
| WoWGroupInfo.cs | `Styx.WoWInternals` | `public class WoWGroupInfo` | 9 | 0 |
| WoWMovement.cs | `Styx.WoWInternals` | `public static class WoWMovement` | 28 | 0 |
| WoWPaperDoll.cs | `Styx.WoWInternals` | `public class WoWPaperDoll : WoWBag` | 20 | 0 |
| WoWPlayerInventory.cs | `Styx.WoWInternals` | `public class WoWPlayerInventory : WoWBag` | 0 | 6 |

### Styx/WoWInternals/DBC/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| Map.cs | `Styx.WoWInternals.DBC` | `public class Map` | 7 | 0 |
| MapType.cs | `Styx.WoWInternals.DBC` | `public enum MapType` | 0 | 0 |

### Styx/WoWInternals/Misc/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| NetStats.cs | `Styx.WoWInternals.Misc` | `public struct NetStats` | 0 | 0 |
| WoWClient.cs | `Styx.WoWInternals.Misc` | `public class WoWClient` | 2 | 0 |

### Styx/WoWInternals/World/ (2 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| GameWorld.cs | `Styx.WoWInternals.World` | `public static class GameWorld` | 14 | 0 |
| WorldLine.cs | `Styx.WoWInternals.World` | `public struct WorldLine` | 1 | 0 |

### Styx/WoWInternals/WoWCache/ (3 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| CacheDb.cs | `Styx.WoWInternals.WoWCache` | `public enum CacheDb` | 0 | 0 |
| DBItemCache.cs | `Styx.WoWInternals.WoWCache` | `public static class DBItemCache` | 1 | 0 |
| WoWCache.cs | `Styx.WoWInternals.WoWCache` | `public class WoWCache` | 3 | 2 |

### Styx/WoWInternals/WoWObjects/ (31 files)

| File | Namespace | Declaration | Pub Methods | Pub Props |
|------|-----------|-------------|:-----------:|:---------:|
| CorpseType.cs | `Styx.WoWInternals.WoWObjects` | `public enum CorpseType : uint` | 0 | 0 |
| FactionStanding.cs | `Styx.WoWInternals.WoWObjects` | `public struct FactionStanding` | 1 | 0 |
| GameObjectInfo.cs | `Styx.WoWInternals.WoWObjects` | `public class GameObjectInfo` | 5 | 0 |
| ILootableObject.cs | `Styx.WoWInternals.WoWObjects` | `public interface ILootableObject` | 0 | 0 |
| ItemInfo.cs | `Styx.WoWInternals.WoWObjects` | `public class ItemInfo` | 19 | 1 |
| ItemStats.cs | `Styx.WoWInternals.WoWObjects` | `public class ItemStats` | 5 | 0 |
| LocalPlayer.cs | `Styx.WoWInternals.WoWObjects` | `public enum RuneType : byte` (+ LocalPlayer class) | 49 | 0 |
| MirrorTimerInfo.cs | `Styx.WoWInternals.WoWObjects` | `public struct MirrorTimerInfo` | 1 | 0 |
| ObjectInvalidateDelegate.cs | `Styx.WoWInternals.WoWObjects` | *(delegate)* | 1 | 0 |
| UnitThreatInfo.cs | `Styx.WoWInternals.WoWObjects` | `public class UnitThreatInfo` | 3 | 0 |
| WoWAnimatedSubObject.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWAnimatedSubObject : WoWSubObject` | 0 | 0 |
| WoWChair.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWChair : WoWSubObject` | 0 | 0 |
| WoWContainer.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWContainer : WoWItem` | 7 | 0 |
| WoWCorpse.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWCorpse : WoWObject` | 11 | 0 |
| WoWDoor.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWDoor : WoWAnimatedSubObject` | 2 | 0 |
| WoWDynamicObject.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWDynamicObject : WoWObject` | 7 | 0 |
| WoWFishingBobber.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWFishingBobber : WoWAnimatedSubObject` | 0 | 0 |
| WoWGameObject.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWGameObject : WoWObject, IComparable<WoWGameObject>, IComparable<WoWUnit>, IComparer<WoWUnit>, IComparer<WoWGameObject>` | 30 | 3 |
| WoWItem.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWItem : WoWObject` | 65 | 13 |
| WoWLockType.cs | `Styx.WoWInternals.WoWObjects` | `public enum WoWLockType` | 0 | 0 |
| WoWMovementInfo.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWMovementInfo` | 16 | 0 |
| WoWObject.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWObject : IComparable<WoWObject>, IEquatable<WoWObject>` | 29 | 1 |
| WoWPartyMember.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWPartyMember : IEquatable<WoWPartyMember>` | 21 | 0 |
| WoWPlayer.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWPlayer : WoWUnit` | 12 | 0 |
| WoWSubObject.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWSubObject` | 4 | 1 |
| WoWTotem.cs | `Styx.WoWInternals.WoWObjects` | `public enum WoWTotem` | 0 | 0 |
| WoWTotemExtensions.cs | `Styx.WoWInternals.WoWObjects` | `public static class WoWTotemExtensions` | 2 | 0 |
| WoWTotemInfo.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWTotemInfo` | 3 | 0 |
| WoWTotemType.cs | `Styx.WoWInternals.WoWObjects` | `public enum WoWTotemType` | 0 | 0 |
| WoWUnit.cs | `Styx.WoWInternals.WoWObjects` | `public class WoWUnit : WoWObject, ILootableObject` | **149** | 0 |

---

## Bots/

### Directory Tree

```
Bots/
├── Gather/
├── Grind/
│   └── Levelbot/
│       ├── Actions/
│       │   ├── Combat/
│       │   └── Death/
│       └── Decorators/
│           ├── Combat/
│           └── Death/
└── Quest/
    ├── Actions/
    ├── Objectives/
    └── QuestOrder/
```

### Bots/Gather/ (3 files)

| File | Namespace | Declaration |
|------|-----------|-------------|
| GatherBuddySettings.cs | `Bots.Gather` | settings class |
| NodeTracker.cs | `Bots.Gather` | node tracking |
| PathType.cs | `Bots.Gather` | enum |

### Bots/Grind/ (1 + 12 files)

| File | Namespace | Declaration |
|------|-----------|-------------|
| LevelBot.cs | `Bots.Grind` | `public class LevelBot : BotBase` |
| Actions/Combat/ActionMoveToTarget.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Combat/ActionPull.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Combat/ActionSetTarget.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Death/ActionMoveToCorpse.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Death/ActionReleaseFromCorpse.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Death/ActionRetrieveCorpse.cs | `Bots.Grind.Levelbot` | action composite |
| Actions/Death/ActionSuceedIfDeadOrGhost.cs | `Bots.Grind.Levelbot` | action composite |
| Decorators/Combat/DecoratorNeedToFindTarget.cs | `Bots.Grind.Levelbot` | decorator composite |
| Decorators/Death/DecoratorInstanceRelease.cs | `Bots.Grind.Levelbot` | decorator composite |
| Decorators/Death/DecoratorNeedToMoveToCorpse.cs | `Bots.Grind.Levelbot` | decorator composite |
| Decorators/Death/DecoratorNeedToRelease.cs | `Bots.Grind.Levelbot` | decorator composite |
| Decorators/Death/DecoratorNeedToTakeCorpse.cs | `Bots.Grind.Levelbot` | decorator composite |

### Bots/Quest/ (17 files)

| File | Namespace | Declaration |
|------|-----------|-------------|
| QuestBot.cs | `Bots.Quest` | `public class QuestBot : BotBase` |
| QuestManager.cs | `Bots.Quest` | quest manager |
| QuestState.cs | `Bots.Quest` | state enum |
| Actions/ActionSelectQuest.cs | `Bots.Quest.Actions` | action composite |
| Actions/ActionSelectReward.cs | `Bots.Quest.Actions` | action composite |
| Actions/ForcedBehaviorExecutor.cs | `Bots.Quest.Actions` | executor |
| Objectives/ActionMoveToGrindArea.cs | `Bots.Quest.Objectives` | objective |
| Objectives/CollectItemObjective.cs | `Bots.Quest.Objectives` | objective |
| Objectives/DecoratorCanMoveToGrindArea.cs | `Bots.Quest.Objectives` | decorator |
| Objectives/DropDatabase.cs | `Bots.Quest.Objectives` | database |
| Objectives/GrindObjective.cs | `Bots.Quest.Objectives` | objective |
| Objectives/QuestObjective.cs | `Bots.Quest.Objectives` | objective |
| Objectives/UseGameObjectObjective.cs | `Bots.Quest.Objectives` | objective |
| QuestOrder/ForcedBehavior.cs | `Bots.Quest.QuestOrder` | forced behavior |
| QuestOrder/ForcedCodeBehavior.cs | `Bots.Quest.QuestOrder` | code behavior |
| QuestOrder/ForcedGrindTo.cs | `Bots.Quest.QuestOrder` | forced grind |
| QuestOrder/ForcedIf.cs | `Bots.Quest.QuestOrder` | conditional |
| QuestOrder/ForcedMoveTo.cs | `Bots.Quest.QuestOrder` | movement |
| QuestOrder/ForcedNothing.cs | `Bots.Quest.QuestOrder` | no-op |
| QuestOrder/ForcedQuestObjective.cs | `Bots.Quest.QuestOrder` | objective |
| QuestOrder/ForcedQuestPickUp.cs | `Bots.Quest.QuestOrder` | pick up |
| QuestOrder/ForcedQuestTurnIn.cs | `Bots.Quest.QuestOrder` | turn in |
| QuestOrder/ForcedSingleton.cs | `Bots.Quest.QuestOrder` | singleton |
| QuestOrder/ForcedUseItem.cs | `Bots.Quest.QuestOrder` | use item |
| QuestOrder/ForcedWhile.cs | `Bots.Quest.QuestOrder` | while loop |
| QuestOrder/QuestOrder.cs | `Bots.Quest.QuestOrder` | order base |

---

## CommonBehaviors/

### Directory Tree

```
CommonBehaviors/
├── Actions/        (17 files)
├── Decorators/     (5 files)
├── ActionWaitForLuaEvent.cs
├── PoiSelector.cs
├── PoiSequence.cs
└── WaitLuaEvent.cs
```

### CommonBehaviors/ (root — 4 files)

| File | Namespace | Declaration |
|------|-----------|-------------|
| ActionWaitForLuaEvent.cs | `CommonBehaviors` | wait for lua event action |
| PoiSelector.cs | `CommonBehaviors` | POI selection composite |
| PoiSequence.cs | `CommonBehaviors` | POI sequence composite |
| WaitLuaEvent.cs | `CommonBehaviors` | wait lua event helper |

### CommonBehaviors/Actions/ (17 files)

| File | Namespace |
|------|-----------|
| ActionAlwaysFail.cs | `CommonBehaviors.Actions` |
| ActionAlwaysSucceed.cs | `CommonBehaviors.Actions` |
| ActionClearPoi.cs | `CommonBehaviors.Actions` |
| ActionDebugString.cs | `CommonBehaviors.Actions` |
| ActionIdle.cs | `CommonBehaviors.Actions` |
| ActionInteract.cs | `CommonBehaviors.Actions` |
| ActionMoveStop.cs | `CommonBehaviors.Actions` |
| ActionMoveToPoi.cs | `CommonBehaviors.Actions` |
| ActionMoveToPoint.cs | `CommonBehaviors.Actions` |
| ActionSetActivity.cs | `CommonBehaviors.Actions` |
| ActionSetPoi.cs | `CommonBehaviors.Actions` |
| ActionSleep.cs | `CommonBehaviors.Actions` |
| DebugStringDelegate.cs | `CommonBehaviors.Actions` |
| GetPointDelegate.cs | `CommonBehaviors.Actions` |
| NavigationAction.cs | `CommonBehaviors.Actions` |
| RetrieveBotPoiDelegate.cs | `CommonBehaviors.Actions` |
| SequenceOpenGossip.cs | `CommonBehaviors.Actions` |

### CommonBehaviors/Decorators/ (5 files)

| File | Namespace |
|------|-----------|
| DecoratorContextIs.cs | `CommonBehaviors.Decorators` |
| DecoratorFrameIsVisible.cs | `CommonBehaviors.Decorators` |
| DecoratorIsNotPoiType.cs | `CommonBehaviors.Decorators` |
| DecoratorIsPoiType.cs | `CommonBehaviors.Decorators` |
| DecoratorNeedToMoveToPoint.cs | `CommonBehaviors.Decorators` |

---

## TreeSharp/ (19 files)

All files: namespace `TreeSharp`

| File | Declaration |
|------|-------------|
| Action.cs | `public class Action : Composite` |
| ActionDelegate.cs | `public delegate RunStatus ActionDelegate(object context)` |
| ActionSucceedDelegate.cs | `public delegate void ActionSucceedDelegate(object context)` |
| CanRunDecoratorDelegate.cs | `public delegate bool CanRunDecoratorDelegate(object context)` |
| Composite.cs | `public abstract class Composite` |
| ContextChangeHandler.cs | `public delegate object ContextChangeHandler(object context)` |
| Decorator.cs | `public class Decorator : GroupComposite` |
| DecoratorContinue.cs | `public class DecoratorContinue : Decorator` |
| GroupComposite.cs | `public abstract class GroupComposite : Composite` |
| PrioritySelector.cs | `public class PrioritySelector : GroupComposite` |
| RetrieveSwitchParameterDelegate.cs | delegate |
| RunStatus.cs | `public enum RunStatus` |
| Selector.cs | `public class Selector : GroupComposite` |
| Sequence.cs | `public class Sequence : GroupComposite` |
| Switch.cs | `public class Switch : GroupComposite` |
| SwitchArgument.cs | `public class SwitchArgument : Decorator` |
| Wait.cs | `public class Wait : Decorator` |
| WaitContinue.cs | `public class WaitContinue : DecoratorContinue` |
| WaitGetTimeoutDelegate.cs | delegate |

---

## GreenMagic/ (29 files)

### Directory Tree

```
GreenMagic/
├── Internals/      (3 files)
├── Native/         (15 files)
├── EventWaitHandleCompat.cs
├── Executor.cs
├── ExecutorRand.cs
├── Extensions.cs
├── FastSize.cs
├── IMemoryOperation.cs
├── InjectionSEHException.cs
├── Manager.cs
├── Memory.cs
├── Windows.cs
└── WindowShowStyle.cs
```

| File | Namespace |
|------|-----------|
| EventWaitHandleCompat.cs | `GreenMagic` |
| Executor.cs | `GreenMagic` |
| ExecutorRand.cs | `GreenMagic` |
| Extensions.cs | `GreenMagic` |
| FastSize.cs | `GreenMagic` |
| IMemoryOperation.cs | `GreenMagic` |
| InjectionSEHException.cs | `GreenMagic` |
| Manager.cs | `GreenMagic` |
| Memory.cs | `GreenMagic` |
| Windows.cs | `GreenMagic` |
| WindowShowStyle.cs | `GreenMagic` |
| Internals/Patch.cs | `GreenMagic.Internals` |
| Internals/PatchManager.cs | `GreenMagic.Internals` |
| Internals/PatternManager.cs | `GreenMagic.Internals` |
| Native/AccessRights.cs | `GreenMagic.Native` |
| Native/Context.cs | `GreenMagic.Native` |
| Native/ContextFlags.cs | `GreenMagic.Native` |
| Native/CopyOfAccessRights.cs | `GreenMagic.Native` |
| Native/CopyOfWaitValues.cs | `GreenMagic.Native` |
| Native/FLOATING_SAVE_AREA.cs | `GreenMagic.Native` |
| Native/Imports.cs | `GreenMagic.Native` |
| Native/MEMORY_BASIC_INFORMATION.cs | `GreenMagic.Native` |
| Native/MemoryAllocType.cs | `GreenMagic.Native` |
| Native/MemoryFreeType.cs | `GreenMagic.Native` |
| Native/MemoryProtectType.cs | `GreenMagic.Native` |
| Native/PeHeaderParser.cs | `GreenMagic.Native` |
| Native/Rect.cs | `GreenMagic.Native` |
| Native/ThreadFlags.cs | `GreenMagic.Native` |
| Native/WaitValues.cs | `GreenMagic.Native` |

---

## Tripper/ (16 files)

### Directory Tree

```
Tripper/
├── Navigation/     (12 files)
├── Tools/Math/     (2 files)
└── XNAMath/        (2 files)
```

| File | Namespace |
|------|-----------|
| Navigation/AbilityFlags.cs | `Tripper.Navigation` |
| Navigation/AreaType.cs | `Tripper.Navigation` |
| Navigation/NativeMethods.cs | `Tripper.Navigation` |
| Navigation/Navigator.cs | `Tripper.Navigation` |
| Navigation/PathFindResult.cs | `Tripper.Navigation` |
| Navigation/PathFindStep.cs | `Tripper.Navigation` |
| Navigation/PathPostProcessing.cs | `Tripper.Navigation` |
| Navigation/PathPostProcessor.cs | `Tripper.Navigation` |
| Navigation/PolygonReference.cs | `Tripper.Navigation` |
| Navigation/Status.cs | `Tripper.Navigation` |
| Navigation/StraightPathFlags.cs | `Tripper.Navigation` |
| Navigation/TileIdentifier.cs | `Tripper.Navigation` |
| Tools/Math/Matrix.cs | `Tripper.Tools.Math` |
| Tools/Math/Vector3.cs | `Tripper.Tools.Math` |
| XNAMath/Vector2.cs | `Tripper.XNAMath` |
| XNAMath/Vector3.cs | `Tripper.XNAMath` |

---

## UI/ (5 files)

| File | Namespace |
|------|-----------|
| App.xaml.cs | `CopilotBuddy.UI` |
| DeveloperToolsWindow.xaml.cs | `CopilotBuddy.UI` |
| MainWindow.xaml.cs | `CopilotBuddy.UI` |
| PluginsWindow.xaml.cs | `CopilotBuddy.UI` |
| SettingsWindow.xaml.cs | `CopilotBuddy.UI` |

---

## Root-Level .cs Files (1 file)

| File | Purpose |
|------|---------|
| AssemblyInfo.cs | Assembly metadata |

---

## Summary Statistics

| Area | Directories | .cs Files | Top Public Methods/Props |
|------|:-----------:|:---------:|:------------------------:|
| **Styx/** | 36 | 412 | WoWUnit (149 methods), CustomForcedBehavior (76), WoWItem (65), SpellManagerEx (59), LocalPlayer (49) |
| **Bots/** | 10 | 33 | LevelBot, QuestBot |
| **CommonBehaviors/** | 2 | 26 | Actions (17), Decorators (5) |
| **TreeSharp/** | 0 | 19 | Composite, GroupComposite, PrioritySelector, Sequence |
| **GreenMagic/** | 2 | 29 | Executor, Manager, Memory |
| **Tripper/** | 3 | 16 | Navigator, NativeMethods |
| **UI/** | 0 | 5 | MainWindow, SettingsWindow |
| **Root** | 0 | 1 | AssemblyInfo |
| **TOTAL** | **53** | **541** | |

### Inheritance Hierarchy (Key WoWObject Classes)

```
WoWObject (Styx.WoWInternals.WoWObjects)
├── WoWUnit : WoWObject, ILootableObject      (149 public methods)
│   ├── WoWPlayer : WoWUnit                   (12 public methods)
│   │   └── LocalPlayer : WoWPlayer           (49 public methods)
│   └── WoWSubObject                          (4 public methods)
│       ├── WoWAnimatedSubObject              
│       │   ├── WoWDoor                        
│       │   └── WoWFishingBobber              
│       └── WoWChair                          
├── WoWItem : WoWObject                       (65 public methods)
│   └── WoWContainer : WoWItem               (7 public methods)
├── WoWGameObject : WoWObject                 (30 public methods)
├── WoWCorpse : WoWObject                     (11 public methods)
└── WoWDynamicObject : WoWObject              (7 public methods)
```

### Namespace Summary (36 unique namespaces under Styx)

```
Styx
Styx.Bot.Properties
Styx.Combat.CombatRoutine
Styx.Common
Styx.CommonBot
Styx.CommonBot.CharacterManagement
Styx.Database
Styx.Helpers
Styx.Loaders
Styx.Logic
Styx.Logic.AreaManagement
Styx.Logic.AreaManagement.Triangulation
Styx.Logic.BehaviorTree
Styx.Logic.Combat
Styx.Logic.Common
Styx.Logic.Inventory
Styx.Logic.Inventory.Frames
Styx.Logic.Inventory.Frames.AuctionHouse
Styx.Logic.Inventory.Frames.Gossip
Styx.Logic.Inventory.Frames.LootFrame
Styx.Logic.Inventory.Frames.MailBox
Styx.Logic.Inventory.Frames.Merchant
Styx.Logic.Inventory.Frames.Quest
Styx.Logic.Inventory.Frames.Taxi
Styx.Logic.Inventory.Frames.Trainer
Styx.Logic.Pathing
Styx.Logic.Pathing.Interop
Styx.Logic.POI
Styx.Logic.Profiles
Styx.Logic.Profiles.Quest
Styx.Logic.Questing
Styx.Offsets
Styx.Patchables
Styx.Plugins
Styx.Plugins.PluginClass
Styx.RemotableObjects
Styx.Resources
Styx.WoWInternals
Styx.WoWInternals.DBC
Styx.WoWInternals.Misc
Styx.WoWInternals.World
Styx.WoWInternals.WoWCache
Styx.WoWInternals.WoWObjects
```
