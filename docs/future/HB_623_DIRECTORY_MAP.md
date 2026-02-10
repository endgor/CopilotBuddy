# HB 6.2.3 (WoD) — Complete Directory Map

> **Root:** `c:\Users\Texy\Desktop\.Reference\.hb 6.2.3\Honorbuddy\`
> **Total:** ~1,638 .cs files across ~267 directories
> **Expansion:** Warlords of Draenor (6.2.3)

---

## Root-Level Files (.cs)

```
App.xaml.cs
Class0.cs, Class1.cs
Class55.cs – Class65.cs          (11 files)
Class90.cs – Class97.cs          (8 files)
Class195.cs, Class223.cs, Class237.cs
Class253.cs – Class291.cs        (39 files)
Class559.cs, Class560.cs
Class666.cs
Class691.cs – Class702.cs        (12 files)
Class835.cs – Class846.cs        (12 files)
Class1317.cs, Class1361.cs, Class1392.cs, Class1396.cs, Class1427.cs, Class1448.cs
ConfusedByAttribute.cs
DevToolsWindow.xaml.cs
EnumLocalizedDescriptionConverter.cs
ErrorWindow.xaml.cs
InvalidProcessException.cs
LogicalAndConverter.cs
LoginWindow.xaml.cs
MainWindow.xaml.cs
MultiBooleanConverter.cs
NegateConverter.cs
PluginsWindow.xaml.cs
ProcessSelectorWindow.xaml.cs
SettingsWindow.xaml.cs
SettingsWrap.cs
UpdateWindow.xaml.cs
WebProfileWindow.xaml.cs
```

---

## Root-Level Directories

| Directory | Purpose |
|-----------|---------|
| `Styx/` | Core bot engine (enums, objects, pathing, plugins, helpers) |
| `Bots/` | All bot implementations (7 bots) |
| `Buddy/` | Auth, coroutines, overlay, store (new in WoD) |
| `CommonBehaviors/` | Reusable behavior tree actions & decorators |
| `GarrisonBuddy/` | WoD Garrison automation (new in WoD) |
| `Levelbot/` | Leveling/grind bot actions & decorators |
| `PartyBot/` | Party/follower bot |
| `RoadMapper/` | Road/path recording tool |
| `Tripper/` | Navigation mesh system |
| `Tripwire/` | Anti-cheat/client communication |
| `NewMixedMode/` | Mixed-mode assembly helper |
| `SevenZip/` | 7-Zip LZMA compression |
| `Infralution/` | WPF localization framework |
| `JetBrains/` | JetBrains annotation attributes |
| `Debug/` | Debug/binding windows |
| `Wpf/` | WPF XAML resources (store browser) |
| `Properties/` | AssemblyInfo |
| `images/` | Image resources |
| `themes/` | WPF theme XAML |
| `ns0/` – `ns104/` | **105 obfuscated namespace folders** |
| `OVmgooAFpSqTGNVUwlNmgEBKXjPDc/` | Obfuscated folder (2 .cs files) |
| `uiHnQlyKuZiGdpWcHVumKGLStYGl/` | Obfuscated folder (1 .cs file) |

---

## Styx/ — Core Engine (Full Tree)

```
Styx/
├── StyxWoW.cs
├── Pulsator.cs
├── Guard.cs
├── BuildType.cs
├── CantCompileException.cs
├── HonorbuddyUnableToStartException.cs
├── InvalidExecutorException.cs
├── InvalidObjectPointerException.cs
├── UserException.cs
├── WoWPoint.cs
├── GameState.cs
├── GameError.cs
├── GraphicsApi.cs
├── NavType.cs
├── GeoRestriction.cs
├── DifficultyColor.cs
│
├── ── Enums (root-level) ──
├── AreaTriggerFlags.cs
├── AreaTriggerShapeType.cs
├── EmoteState.cs
├── FactionId.cs
├── GameObjectDataSlot.cs
├── InventoryType.cs
├── LfgCategory.cs
├── LfgState.cs
├── MirrorTimerType.cs
├── PvPState.cs
├── QuestGiverStatus.cs
├── ShapeshiftForm.cs
├── SheathType.cs
├── SkillLine.cs
├── SpellAttributes.cs
├── SpellAttributesEx.cs – SpellAttributesEx8.cs  (8 files)
├── StatType.cs
├── ThreatStatus.cs
├── UnitNPCFlags.cs
├── WoWBagSlot.cs
├── WoWClass.cs
├── WoWCreatureSkinType.cs
├── WoWCreatureType.cs
├── WoWCursorType.cs
├── WoWEquipSlot.cs
├── WoWFactionGroup.cs
├── WoWGameObjectState.cs
├── WoWGameObjectType.cs
├── WoWGender.cs
├── WoWInteractType.cs
├── WoWInventorySlot.cs
├── WoWItemArmorClass.cs
├── WoWItemBondType.cs
├── WoWItemClass.cs
├── WoWItemConsumableClass.cs
├── WoWItemContainerClass.cs
├── WoWItemGemClass.cs
├── WoWItemGlyphClass.cs
├── WoWItemKeyClass.cs
├── WoWItemMiscClass.cs
├── WoWItemProjectileClass.cs
├── WoWItemQuality.cs
├── WoWItemQuiverClass.cs
├── WoWItemRecipeClass.cs
├── WoWItemStatType.cs
├── WoWItemTradeGoodsClass.cs
├── WoWItemWeaponClass.cs
├── WoWObjectType.cs
├── WoWObjectTypeFlag.cs
├── WoWPowerType.cs
├── WoWQuestType.cs
├── WoWRace.cs
├── WoWSocketColor.cs
├── WoWSpec.cs
├── WoWStateFlag.cs
├── WoWUnitClassificationType.cs
├── WoWUnitReaction.cs
│
├── Common/
│   ├── AddCompositeListOperation.cs
│   ├── Arguments.cs
│   ├── AsmHelper.cs
│   ├── AssemblyLoader.cs
│   ├── Beta.cs
│   ├── CapacityQueue.cs
│   ├── CircularQueue.cs
│   ├── CommandLine.cs
│   ├── CompositeListOperation.cs
│   ├── DualHashSet.cs
│   ├── Extensions.cs
│   ├── FileCache.cs
│   ├── FinishedMeasuringCallback.cs
│   ├── Flash.cs
│   ├── FlashFlags.cs
│   ├── ForcedCulture.cs
│   ├── HookDescription.cs
│   ├── HookExecutor.cs
│   ├── Hotkey.cs
│   ├── HotkeysManager.cs
│   ├── IndexedList.cs
│   ├── InsertCompositeListOperation.cs
│   ├── IRangeAble.cs
│   ├── LineSegment.cs
│   ├── LinqExtensions.cs
│   ├── Logging.cs
│   ├── LogLevel.cs
│   ├── LruCache.cs
│   ├── MathEx.cs
│   ├── ModifierKeys.cs
│   ├── PerformanceTimer.cs
│   ├── Quaternion.cs
│   ├── Range.cs
│   ├── RangedDictionary.cs
│   ├── Ray.cs
│   ├── ReplaceCompositeListOperation.cs
│   ├── ShapeHelper.cs
│   ├── Sphere.cs
│   ├── StyxLog.cs
│   ├── ThreadSafeRandom.cs
│   ├── TimedRecordKeeper.cs
│   ├── TimestampType.cs
│   ├── TreeHooks.cs
│   ├── TypeLoader.cs
│   ├── TypeOnlyLoader.cs
│   ├── Utilities.cs
│   ├── ValuePair.cs
│   ├── Vector2.cs
│   ├── Vector3.cs
│   ├── Compiler/
│   │   └── CodeCompiler.cs
│   ├── Helpers/
│   │   ├── AutoTimer.cs
│   │   ├── ByteArray.cs
│   │   ├── WaitTimer.cs
│   │   └── WaitTimerFinishedHandler.cs
│   └── WpfControls/
│       ├── MenuButton.cs
│       └── SplitButton.cs
│
├── CommonBot/
│   ├── Blacklist.cs
│   ├── BlacklistFlags.cs
│   ├── BotBase.cs
│   ├── BotEvents.cs
│   ├── BotManager.cs
│   ├── BuyItemsEventArgs.cs
│   ├── BuyItemsEventHandler.cs
│   ├── CanMountDelegate.cs
│   ├── Chat.cs
│   ├── FlightPathReason.cs
│   ├── FlightPaths.cs
│   ├── GameStats.cs
│   ├── GoalTextChangedEventArgs.cs
│   ├── HealTargeting.cs
│   ├── HonorbuddyExitCode.cs
│   ├── InactivityDetector.cs
│   ├── IncludeTargetsFilterDelegate.cs
│   ├── Landmarks.cs
│   ├── LocationRetriever.cs
│   ├── LootPredictor.cs
│   ├── LootTargeting.cs
│   ├── MailItemsEventArgs.cs
│   ├── MailItemsEventHandler.cs
│   ├── Mount.cs
│   ├── MountType.cs
│   ├── MountUpEventArgs.cs
│   ├── PulseFlags.cs
│   ├── RaFHelper.cs
│   ├── RemoveTargetsFilterDelegate.cs
│   ├── Rest.cs
│   ├── SellItemsEventArgs.cs
│   ├── ShutdownRequestedEventArgs.cs
│   ├── SpellCollection.cs
│   ├── SpellFindResults.cs
│   ├── SpellManager.cs
│   ├── StatusTextChangedEventArgs.cs
│   ├── Targeting.cs
│   ├── TargetListUpdateFinishedDelegate.cs
│   ├── TreeRoot.cs
│   ├── TreeRootState.cs
│   ├── VendorItemsEventHandler.cs
│   ├── Vendors.cs
│   ├── WeighTargetsDelegate.cs
│   ├── XmlFlightNode.cs
│   ├── AreaManagement/
│   │   ├── Area.cs
│   │   ├── AreaManager.cs
│   │   ├── AreaType.cs
│   │   ├── GrindArea.cs
│   │   ├── Hotspot.cs
│   │   ├── HotspotExtensions.cs
│   │   ├── HotspotManager.cs
│   │   ├── PolygonArea.cs
│   │   ├── PvPArea.cs
│   │   ├── QuestArea.cs
│   │   └── Triangulation/
│   │       ├── Edge.cs
│   │       └── Triangle.cs
│   ├── Bars/
│   │   ├── ActionBar.cs
│   │   ├── ActionBarType.cs
│   │   ├── ActionButton.cs
│   │   ├── ActionButtonSubType.cs
│   │   ├── ActionButtonType.cs
│   │   └── SpellActionButton.cs
│   ├── CharacterManagement/
│   │   ├── AutoEquipper.cs
│   │   ├── CharacterManager.cs
│   │   ├── ClassProfile.cs
│   │   ├── ClassProfileLoadException.cs
│   │   ├── ClassProfileLocalization.cs
│   │   ├── DetailedWeaponStyle.cs
│   │   ├── RollType.cs
│   │   ├── TalentPlacement.cs
│   │   ├── TalentPlacementSet.cs
│   │   ├── TalentSelector.cs
│   │   ├── WeaponStyle.cs
│   │   ├── WeighableStatType.cs
│   │   └── WeightSet.cs
│   ├── Coroutines/
│   │   ├── CommonCoroutines.cs
│   │   ├── CoroutineCompositeExtensions.cs
│   │   ├── CoroutineTask.2.cs
│   │   ├── CoroutineTask.cs
│   │   ├── CoroutineTaskAwaiter.2.cs
│   │   └── CoroutineTaskAwaiter.cs
│   ├── Database/
│   │   ├── Connection.cs
│   │   └── NpcResult.cs
│   ├── Events/
│   │   └── Profile/
│   │       └── CodeCompositionEventArgs.cs
│   ├── Frames/
│   │   ├── AuctionFrame.cs
│   │   ├── AuctionListType.cs
│   │   ├── Frame.cs
│   │   ├── GarrisonMissionFrame.cs          ← NEW in WoD
│   │   ├── GossipEntry.cs
│   │   ├── GossipFrame.cs
│   │   ├── GossipQuestEntry.cs
│   │   ├── GuildBankFrame.cs
│   │   ├── GuildBankTab.cs
│   │   ├── ItemQuality.cs
│   │   ├── LootFrame.cs
│   │   ├── LootRarity.cs
│   │   ├── LootSlotInfo.cs
│   │   ├── MailFrame.cs
│   │   ├── MerchantFrame.cs
│   │   ├── MerchantItem.cs
│   │   ├── QuestFrame.cs
│   │   ├── TaxiFrame.cs
│   │   ├── TrainerFrame.cs
│   │   └── TrainerServiceFilter.cs
│   ├── Inventory/
│   │   ├── Consumable.cs
│   │   ├── EquipmentManager.cs
│   │   ├── EquipmentSet.cs
│   │   ├── InventoryManager.cs
│   │   ├── InventorySlot.cs
│   │   ├── LootRoll.cs
│   │   └── WoWPrice.cs
│   ├── ObjectDatabase/
│   │   ├── MailboxResult.cs
│   │   └── Query.cs
│   ├── POI/
│   │   ├── BotPoi.cs
│   │   ├── PoiType.cs
│   │   └── PoiTypeExtensions.cs
│   ├── Profiles/
│   │   ├── CompileExpressionAttribute.cs
│   │   ├── CompileStringAttribute.cs
│   │   ├── CustomBehaviorFileNameAttribute.cs
│   │   ├── CustomForcedBehavior.cs
│   │   ├── ForceMailManager.cs
│   │   ├── HotspotCollection.cs
│   │   ├── IXmlObject.cs
│   │   ├── Mailbox.cs
│   │   ├── MailboxManager.cs
│   │   ├── Profile.cs
│   │   ├── ProfileAttributeExpectedException.2.cs
│   │   ├── ProfileAttributeExpectedException.cs
│   │   ├── ProfileException.cs
│   │   ├── ProfileManager.cs
│   │   ├── ProfileMissingAttributeException.2.cs
│   │   ├── ProfileMissingAttributeException.cs
│   │   ├── ProfileMissingElementException.cs
│   │   ├── ProfileNotFoundException.cs
│   │   ├── ProfileTagExpectedException.cs
│   │   ├── ProfileUnknownAttributeException.cs
│   │   ├── ProfileUnknownElementException.cs
│   │   ├── ProtectedItemsManager.cs
│   │   ├── UnknownProfileElementEventArgs.cs
│   │   ├── Vendor.cs
│   │   ├── VendorManager.cs
│   │   ├── VendorTypeExtensions.cs
│   │   └── Quest/
│   │       ├── CollectFrom.cs
│   │       ├── CollectFromCollection.cs
│   │       ├── CollectFromType.cs
│   │       ├── CollectItemObjectiveInfo.cs
│   │       ├── KillMobObjectiveInfo.cs
│   │       ├── ObjectiveInfo.cs
│   │       ├── ObjectiveType.cs
│   │       ├── QuestInfo.cs
│   │       ├── TurnInObjectiveInfo.cs
│   │       ├── UseObjectObjectiveInfo.cs
│   │       └── Order/
│   │           ├── AbandonQuestNode.cs
│   │           ├── CheckpointNode.cs
│   │           ├── ClearAvoidMobsNode.cs
│   │           ├── ClearBlacklistNode.cs
│   │           ├── ClearGrindAreaNode.cs
│   │           ├── ClearMailboxNode.cs
│   │           ├── ClearVendorNode.cs
│   │           ├── CodeNode.cs
│   │           ├── CompileBatch.cs
│   │           ├── CompileError.cs
│   │           ├── ConditionHelper.cs
│   │           ├── DelayCompiledExpression.2.cs
│   │           ├── DelayCompiledExpression.cs
│   │           ├── DisableRepairNode.cs
│   │           ├── Else.cs
│   │           ├── ElseIf.cs
│   │           ├── EnableRepairNode.cs
│   │           ├── ExpressionError.cs
│   │           ├── ExpressionSet.cs
│   │           ├── GrindToNode.cs
│   │           ├── IfNode.cs
│   │           ├── INodeContainer.cs
│   │           ├── MoveToNode.cs
│   │           ├── ObjectiveNode.cs
│   │           ├── OrderNode.cs
│   │           ├── OrderNodeCollection.cs
│   │           ├── OrderNodeType.cs
│   │           ├── PickUpNode.cs
│   │           ├── ProfileHelperFunctionsBase.cs
│   │           ├── QuestBehaviorHelper.cs
│   │           ├── QuestObjectType.cs
│   │           ├── SetAvoidMobsNode.cs
│   │           ├── SetBlacklistNode.cs
│   │           ├── SetGrindAreaNode.cs
│   │           ├── SetLootMobsNode.cs
│   │           ├── SetLootRadiusNode.cs
│   │           ├── SetMailboxNode.cs
│   │           ├── SetNavTypeNode.cs
│   │           ├── SetTargetingDistanceNode.cs
│   │           ├── SetUseMountNode.cs
│   │           ├── SetVendorNode.cs
│   │           ├── ToggleBehaviorNode.cs
│   │           ├── TurnInNode.cs
│   │           ├── UseItemNode.cs
│   │           └── WhileNode.cs
│   └── Routines/
│       ├── CapabilityFlags.cs
│       ├── CapabilityManager.cs
│       ├── CapabilityManagerHandle.cs
│       ├── CapabilityState.cs
│       ├── CapabilityStateChangedArgs.cs
│       ├── CombatRoutine.cs
│       ├── InvalidRoutineWrapper.cs
│       └── RoutineManager.cs
│
├── Helpers/
│   ├── ActivitySetter.cs
│   ├── BGBotSettings.cs
│   ├── CachedValue.cs
│   ├── CharacterSettings.cs
│   ├── CombatAssistSettings.cs
│   ├── DefaultValueAttribute.cs
│   ├── DictionaryExtensions.cs
│   ├── EncryptedAttribute.cs
│   ├── Extensions.cs
│   ├── FlagCheckedListBox.cs
│   ├── FlagCheckedListBoxItem.cs
│   ├── FlagEnumUIEditor.cs
│   ├── GameDebugAddStringDelegate.cs
│   ├── GlobalSettings.cs
│   ├── KeyboardManager.cs
│   ├── KeyHelpers.cs
│   ├── LevelbotSettings.cs
│   ├── PerFrameCachedValue.cs
│   ├── PVPSettings.cs
│   ├── SettingAttribute.cs
│   ├── Settings.cs
│   ├── TimeCachedValue.cs
│   ├── UISettings.cs
│   ├── WoWItemQualityExtensions.cs
│   ├── WoWMathHelper.cs
│   ├── WoWSpecExtensions.cs
│   ├── XmlExtensions.cs
│   └── XmlUtils.cs
│
├── Loaders/
│   ├── DllLoader.cs
│   └── DynamicLoader.cs
│
├── Localization/
│   ├── Globalization.cs
│   ├── Globalization.Designer.cs
│   ├── LocalizedDescriptionAttribute.cs
│   └── (resources)
│
├── Offsets/
│   ├── WoWAreaTriggerFields.cs
│   ├── WoWContainerFields.cs
│   ├── WoWConversationFields.cs           ← NEW in WoD
│   ├── WoWCorpseFields.cs
│   ├── WoWDynamicObjectFields.cs
│   ├── WoWGameObjectFields.cs
│   ├── WoWItemFields.cs
│   ├── WoWObjectFields.cs
│   ├── WoWPlayerFields.cs
│   ├── WoWSceneObjectFields.cs            ← NEW in WoD
│   ├── WoWUnitFields.cs
│   └── Pending/
│       └── PendingOffsets.cs
│
├── Patchables/
│   ├── AuraFlags.cs
│   ├── ClientDb.cs
│   ├── IncomingHeal.cs
│   ├── LandMarkEntry.cs
│   ├── LootRollItemInfo.cs
│   ├── MouseButton.cs
│   └── QueuedBattlegroundInfo.cs
│
├── Pathing/
│   ├── BlackspotQueryFlags.cs
│   ├── Flightor.cs
│   ├── IPlayerMover.cs
│   ├── ITerrainHeightProvider.cs
│   ├── KeyboardMover.cs
│   ├── MeshMovePath.cs
│   ├── MeshNavigator.cs
│   ├── MoveResult.cs
│   ├── MoveResultExtensions.cs
│   ├── NavigationProvider.cs
│   ├── NavigationProviderChangedEventArgs.cs
│   ├── Navigator.cs
│   ├── PathGenerationFailStep.cs
│   ├── StuckHandler.cs
│   ├── FlightorAnnotation/
│   │   └── IndoorEntrance.cs
│   └── FlightorNavigation/
│       ├── Areas.cs
│       ├── BlackspotManager.cs
│       └── PolyNav.cs
│
├── Plugins/
│   ├── HBPlugin.cs
│   ├── PluginContainer.cs
│   ├── PluginManager.cs
│   └── PluginWrapper.cs
│
├── Resources/
│   ├── StyxResources.cs
│   └── StyxResources.Designer.cs
│
├── TreeSharp/
│   ├── Action.cs
│   ├── ActionDelegate.cs
│   ├── ActionSucceedDelegate.cs
│   ├── CanRunDecoratorDelegate.cs
│   ├── Composite.cs
│   ├── ContextChangeHandler.cs
│   ├── Decorator.cs
│   ├── DecoratorContinue.cs
│   ├── DynamicChildSelector.cs
│   ├── GroupComposite.cs
│   ├── PrioritySelector.cs
│   ├── ProbabilitySelector.cs
│   ├── RetrieveSwitchParameterDelegate.cs
│   ├── RunStatus.cs
│   ├── Selector.cs
│   ├── Sequence.cs
│   ├── Sleep.cs
│   ├── Switch.cs
│   ├── SwitchArgument.cs
│   ├── Wait.cs
│   ├── WaitContinue.cs
│   ├── WaitGetTimeoutDelegate.cs
│   ├── WaitGetTimeSpanTimeoutDelegate.cs
│   └── WhileLoop.cs
│
├── WoWInternals/
│   ├── ── Battleground Landmarks ──
│   ├── AlteracValleyLandmark.cs
│   ├── AlteracValleyLandmarkType.cs
│   ├── ArathiBasinLandmark.cs
│   ├── ArathiBasinLandmarkType.cs
│   ├── AreaPoiLandmark.cs
│   ├── BattleForGilneasLandmark.cs         ← NEW (post-Cata)
│   ├── BattleForGilneasLandmarkType.cs
│   ├── DeepwindGorgeLandmark.cs             ← NEW (MoP BG)
│   ├── DeepwindGorgeLandmarkType.cs
│   ├── EyeOfTheStormLandmark.cs
│   ├── EyeOfTheStormLandmarkType.cs
│   ├── IsleOfConquestLandmark.cs
│   ├── IsleOfConquestLandmarkType.cs
│   ├── LandmarkControlType.cs
│   ├── LandmarkType.cs
│   ├── StrandOfTheAncientsLandmark.cs
│   ├── StrandOfTheAncientsLandmarkType.cs
│   ├── ResearchSiteLandmark.cs
│   │
│   ├── ── Battlegrounds ──
│   ├── ArenaType.cs
│   ├── BattlefieldWinner.cs
│   ├── Battlegrounds.cs
│   ├── BattlegroundJoinError.cs
│   ├── BattlegroundSide.cs
│   ├── BattlegroundStatus.cs
│   ├── BattlegroundType.cs
│   │
│   ├── ── Input & Movement ──
│   ├── ClickToMoveInfo.cs
│   ├── GameInput.cs
│   ├── InputMouseButton.cs
│   ├── MoveFlags.cs
│   ├── WoWMovement.cs
│   ├── WoWMovementInfo.cs
│   ├── WoWSimpleMovementInfo.cs
│   │
│   ├── ── Lua System ──
│   ├── Lua.cs
│   ├── LuaEventArgs.cs
│   ├── LuaEventHandlerDelegate.cs
│   ├── LuaEvents.cs
│   ├── LuaNode.cs
│   ├── LuaRunStatus.cs
│   ├── LuaState.cs
│   ├── LuaTable.cs
│   ├── LuaTKey.cs
│   ├── LuaTString.cs
│   ├── LuaTValue.cs
│   ├── LuaType.cs
│   ├── LuaValue.cs
│   ├── NativeLuaCommonHeader.cs
│   ├── NativeLuaNode.cs
│   ├── NativeLuaTable.cs
│   ├── NativeLuaTKey.cs
│   ├── NativeLuaTString.cs
│   ├── NativeLuaTValue.cs
│   ├── NativeLuaValue.cs
│   │
│   ├── ── Core Objects ──
│   ├── NativeObject.cs
│   ├── ObjectListUpdateFinishedDelegate.cs
│   ├── ObjectManager.cs
│   ├── ItemContext.cs                        ← NEW in WoD
│   ├── PetStance.cs
│   ├── SpellChargeInfo.cs                    ← NEW (MoP+)
│   ├── SpellCooldownInfo.cs
│   ├── SpellDetailedPowerCost.cs             ← NEW
│   ├── TaxiNodeType.cs
│   │
│   ├── ── Quests ──
│   ├── PlayerQuest.cs
│   ├── Quest.cs
│   ├── QuestLog.cs
│   ├── WoWDescriptorQuest.cs
│   ├── WoWDescriptorQuestFlags.cs
│   ├── WoWQuestPOIInfo.cs
│   ├── WoWQuestState.cs
│   ├── WoWQuestStep.cs
│   ├── WoWQuestStepsCollection.cs
│   │
│   ├── ── Spells & Auras ──
│   ├── WoWApplyAuraType.cs
│   ├── WoWAura.cs
│   ├── WoWAuraCollection.cs
│   ├── WoWDispelType.cs
│   ├── WoWSpell.cs
│   ├── WoWSpellEffectType.cs
│   ├── WoWSpellFocus.cs
│   ├── WoWSpellMechanic.cs
│   ├── WoWSpellSchool.cs
│   │
│   ├── ── Items & Inventory ──
│   ├── WoWBag.cs
│   ├── WoWCamera.cs
│   ├── WoWCurrency.cs
│   ├── WoWCurrencyType.cs
│   ├── WoWGlyphInfo.cs (via WoWObjects/)
│   ├── WoWLandMark.cs
│   ├── WoWMissile.cs
│   ├── WoWPaperDoll.cs
│   ├── WoWPetBattleState.cs                 ← NEW (MoP)
│   ├── WoWPetControl.cs
│   ├── WoWPetSpell.cs
│   ├── WoWPlayerInventory.cs
│   ├── WoWSkill.cs
│   ├── WoWTotem.cs
│   ├── WoWTotemExtensions.cs
│   ├── WoWTotemInfo.cs
│   ├── WoWTotemType.cs
│   ├── WoWVehicle.cs
│   │
│   ├── ── Group & GUID ──
│   ├── WoWGroupInfo.cs
│   ├── WoWGuid.cs
│   ├── WoWGuidType.cs
│   │
│   ├── DB/   (WoD DB2 tables — significantly expanded)
│   │   ├── BattlePetSpecies.cs              ← NEW (MoP)
│   │   ├── CharShipment.cs                  ← NEW (WoD Garrison)
│   │   ├── CharShipmentContainer.cs         ← NEW (WoD Garrison)
│   │   ├── Creature.cs
│   │   ├── CriteriaTree.cs
│   │   ├── CurrencyType.cs
│   │   ├── Db2Table.cs                      ← NEW (DB2 format)
│   │   ├── GameObject.cs
│   │   ├── GarrAbility.cs                   ← NEW (WoD)
│   │   ├── GarrAbilityCategory.cs           ← NEW
│   │   ├── GarrAbilityEffect.cs             ← NEW
│   │   ├── GarrAbilityEffectCategory.cs     ← NEW
│   │   ├── GarrBuilding.cs                  ← NEW
│   │   ├── GarrClassSpec.cs                 ← NEW
│   │   ├── GarrEncounter.cs                 ← NEW
│   │   ├── GarrEncounterXMechanic.cs        ← NEW
│   │   ├── GarrFollower.cs                  ← NEW
│   │   ├── GarrisonBuildingType.cs          ← NEW
│   │   ├── GarrisonFollowerType.cs          ← NEW
│   │   ├── GarrisonMissionType.cs           ← NEW
│   │   ├── GarrMechanic.cs                  ← NEW
│   │   ├── GarrMechanicType.cs              ← NEW
│   │   ├── GarrMission.cs                   ← NEW
│   │   ├── GarrPlotInstance.cs              ← NEW
│   │   ├── GarrSiteLevel.cs                 ← NEW
│   │   ├── ItemDisenchantLoot.cs
│   │   ├── ItemEffect.cs
│   │   ├── ItemEffectList.cs
│   │   ├── ItemEffectTriggerType.cs
│   │   ├── ItemEntry.cs
│   │   ├── ItemExtendedCost.cs
│   │   ├── ItemSparseEntry.cs
│   │   ├── PetType.cs                       ← NEW (MoP)
│   │   ├── PlotType.cs                      ← NEW (WoD)
│   │   ├── Scenario.cs                      ← NEW (MoP)
│   │   ├── ScenarioStep.cs                  ← NEW
│   │   ├── ScenarioType.cs                  ← NEW
│   │   ├── SpellMissile.cs
│   │   ├── TotemCategory.cs
│   │   ├── UILocomotionType.cs
│   │   ├── Vehicle.cs
│   │   ├── VehicleFlags.cs
│   │   ├── WoWDb.cs
│   │   ├── WoWDbRow.cs
│   │   └── WoWDbTable.cs
│   │
│   ├── DBC/   (Classic DBC tables — carried forward)
│   │   ├── AreaPoi.cs
│   │   ├── AreaTable.cs
│   │   ├── CreatureFamily.cs
│   │   ├── Faction.cs
│   │   ├── FactionTemplate.cs
│   │   ├── InstanceType.cs
│   │   ├── ItemRandomProperties.cs
│   │   ├── ItemRandomSuffix.cs
│   │   ├── LfgDifficulty.cs
│   │   ├── LfgDungeons.cs
│   │   ├── LfgDungeonsFlags.cs
│   │   ├── LfgSubType.cs
│   │   ├── Map.cs
│   │   ├── MapDifficulty.cs
│   │   ├── MapType.cs
│   │   ├── PetFoodFlags.cs
│   │   ├── RecipeAcquireMethod.cs
│   │   ├── ResearchSite.cs
│   │   ├── ScalingStatDistribution.cs
│   │   ├── SkillLineAbility.cs
│   │   ├── SkillLineCategory.cs
│   │   ├── SkillLineInfo.cs
│   │   ├── SpellEffect.cs
│   │   ├── SpellItemEnchantment.cs
│   │   └── TaxiNodes.cs
│   │
│   ├── Garrison/                            ← ENTIRELY NEW (WoD)
│   │   ├── GarrisonBuilding.cs
│   │   ├── GarrisonFollower.cs
│   │   ├── GarrisonFollowerStatus.cs
│   │   ├── GarrisonInfo.cs
│   │   ├── GarrisonMission.cs
│   │   ├── GarrisonMissionReward.cs
│   │   ├── GarrisonMissionRewardInfo.cs
│   │   ├── GarrisonMissionSimulator.cs
│   │   ├── GarrisonPlot.cs
│   │   ├── GarrisonShipmentInfo.cs
│   │   ├── LandingPageShipmentInfo.cs
│   │   ├── MissionSimulatorOptions.cs
│   │   ├── MissionSimulatorResults.cs
│   │   ├── MissionState.cs
│   │   └── OwnedBuildingInfo.cs
│   │
│   ├── Misc/
│   │   ├── NetStats.cs
│   │   ├── Stable.cs
│   │   ├── StabledPet.cs
│   │   ├── WoWAuction.cs
│   │   └── WoWClient.cs
│   │
│   ├── TradeSkills/
│   │   ├── Ingredient.cs
│   │   ├── Recipe.cs
│   │   ├── RecipeDifficulty.cs
│   │   ├── Tool.cs
│   │   └── TradeSkill.cs
│   │
│   ├── UI/
│   │   ├── AnchorPoint.cs
│   │   ├── Backdrop.cs
│   │   ├── BlendMode.cs
│   │   ├── ButtonState.cs
│   │   ├── FrameStrata.cs
│   │   ├── Layer.cs
│   │   └── Orientation.cs
│   │
│   ├── World/
│   │   ├── AreaTable.cs
│   │   ├── GameWorld.cs
│   │   ├── JbnMap.cs
│   │   ├── JbnMapAreaTableEntry.cs
│   │   ├── TraceLineHitFlags.cs
│   │   ├── Triangle.cs
│   │   ├── UnitSpellLineOfSightTestEventArgs.cs
│   │   ├── WorldLine.cs
│   │   ├── WorldMap.cs
│   │   ├── WorldMapAreaTableEntry.cs
│   │   └── WorldScene.cs                    ← NEW (WoD phasing)
│   │
│   ├── WoWCache/
│   │   ├── CacheDb.cs
│   │   └── WoWCache.cs
│   │
│   └── WoWObjects/
│       ├── BagType.cs
│       ├── CorpseType.cs
│       ├── FactionStanding.cs
│       ├── GameObjectInfo.cs
│       ├── ILootableObject.cs
│       ├── ItemInfo.cs
│       ├── ItemStats.cs
│       ├── LocalPlayer.cs
│       ├── MirrorTimerInfo.cs
│       ├── ObjectInvalidateDelegate.cs
│       ├── RaidTargetMarker.cs
│       ├── ReputationFlags.cs
│       ├── SpecType.cs
│       ├── UnitThreatInfo.cs
│       ├── WoWAnimatedSubObject.cs
│       ├── WoWAreaTrigger.cs                ← NEW (WoD)
│       ├── WoWArenaTeamInfo.cs
│       ├── WoWChair.cs
│       ├── WoWContainer.cs
│       ├── WoWCorpse.cs
│       ├── WoWDoor.cs
│       ├── WoWDynamicObject.cs
│       ├── WoWFishingBobber.cs
│       ├── WoWGameObject.cs
│       ├── WoWGlyphInfo.cs
│       ├── WoWInebriationLevel.cs
│       ├── WoWItem.cs
│       ├── WoWLockType.cs
│       ├── WoWObject.cs
│       ├── WoWPartyMember.cs
│       ├── WoWPlayer.cs
│       ├── WoWPlayerCombatRating.cs
│       ├── WoWSubObject.cs
│       ├── WoWUnit.cs
│       ├── AreaTriggerShapes/               ← NEW (WoD)
│       │   ├── AreaTriggerBox.cs
│       │   ├── AreaTriggerCylinder.cs
│       │   ├── AreaTriggerPolygon.cs
│       │   ├── AreaTriggerShape.cs
│       │   ├── AreaTriggerShapeStruct.cs
│       │   └── AreaTriggerSphere.cs
│       └── SubObjects/
│           ├── GarrisonShipmentState.cs     ← NEW (WoD)
│           └── WoWGarrisonShipment.cs       ← NEW (WoD)
│
└── XmlEngine/
    ├── INamedAttribute.cs
    ├── XmlAttributeAttribute.cs
    ├── XmlElementAttribute.cs
    └── XmlEngine.cs
```

---

## Bots/ — All Bot Implementations

```
Bots/
├── ArchaeologyBuddy/
│   ├── ArchaeologyRace.cs
│   ├── ArchBuddy.cs
│   ├── ArchSettings.cs
│   ├── Digsite.cs
│   ├── Fragment.cs
│   └── GUI/
│       └── ArchBuddySettings.cs (+ Designer, resources)
│
├── BGBuddy/
│   ├── Battleground.cs
│   ├── BattlegroundSide.cs
│   ├── BgBotProfile.cs
│   ├── BGBuddy.cs
│   ├── BGBuddySettings.cs
│   ├── HeatmapWindow.cs (+ Designer, resources)
│   ├── LogicType.cs
│   ├── MapBox.cs
│   ├── RaidHelper.cs
│   ├── WorldStatesUpdateDelegate.cs
│   ├── Forms/
│   │   └── ConfigWindow.cs (+ Designer, resources)
│   ├── HeatMap/
│   │   └── Heatmap.cs
│   ├── Helpers/
│   │   └── Logger.cs
│   ├── Logic/
│   │   └── Battlegrounds/
│   │       └── LandmarkInfo.cs
│   └── Resources/
│       └── BGBuddyResources.cs (+ Designer, resources)
│
├── DungeonBuddy/
│   ├── AvoidanceNavigationProvider.cs
│   ├── BossManager.cs
│   ├── Dungeon.cs
│   ├── DungeonBot.cs
│   ├── DungeonManager.cs
│   ├── DynamicBlackspot.cs
│   ├── DynamicBlackspotManager.cs
│   ├── GroupMember.cs
│   ├── Attributes/
│   │   ├── CallBehaviorMode.cs
│   │   ├── DynamicStringListAttribute.cs
│   │   ├── EncounterHandlerAttribute.cs
│   │   ├── LocationHandlerAttribute.cs
│   │   ├── ObjectHandlerAttribute.cs
│   │   └── ScenarioStageAttribute.cs
│   ├── Avoidance/
│   │   ├── Avoid.cs
│   │   ├── AvoidanceManager.cs
│   │   ├── AvoidancePriority.cs
│   │   ├── AvoidCluster.cs
│   │   ├── AvoidInfo.cs
│   │   ├── AvoidLocation.cs
│   │   ├── AvoidLocationInfo.cs
│   │   ├── AvoidObject.cs
│   │   ├── AvoidObjectInfo.cs
│   │   ├── AvoidPathNotFoundException.cs
│   │   ├── AvoidPathResult.cs
│   │   ├── AvoidSide.cs
│   │   ├── AvoidTracelineResult.cs
│   │   ├── ClusterHit.cs
│   │   ├── Helpers.cs
│   │   ├── LineCircleTangentPoints.cs
│   │   ├── LineClusterTangentPoints.cs
│   │   └── PathResult.cs
│   ├── Behaviors/
│   │   └── ActionLogger.cs
│   ├── Enums/
│   │   ├── CompleteReason.cs
│   │   ├── DungeonType.cs
│   │   ├── GroupLootMode.cs
│   │   ├── LootMode.cs
│   │   ├── PartyMode.cs
│   │   ├── PlayerFactionAccessibility.cs
│   │   ├── RaidType.cs
│   │   └── ScenarioType.cs
│   ├── Forms/
│   │   ├── FormConfig.cs (+ Designer, resources)
│   │   └── PathView.cs (+ Designer, resources)
│   ├── Helpers/
│   │   ├── Action.cs, Alert.cs, AlwaysFailAction.cs
│   │   ├── Decorator.cs, DecoratorContinue.cs
│   │   ├── DungeonArea.cs, DungeonBuddySettings.cs
│   │   ├── DynamicStringListConverter.cs
│   │   ├── Error.cs, ErrorCollection.cs, ErrorType.cs
│   │   ├── Logger.cs
│   │   ├── ScenarioCriteria.cs, ScenarioInfo.cs, ScenarioStage.cs
│   │   ├── ScriptHelpers.cs
│   │   ├── SpellActionButton.cs
│   │   ├── StrafeManager.cs
│   │   └── WaitContinue.cs
│   └── Profiles/
│       ├── ElementAttributeAttribute.cs
│       ├── IXmlAutoProcessed.cs
│       ├── ObsoleteProfileElementAttribute.cs
│       ├── Profile.cs
│       ├── ProfileElementAttribute.cs
│       ├── ProfileManager.cs
│       ├── ValueRangeAttribute.cs
│       └── Handlers/
│           ├── Blackspot.cs
│           ├── Boss.cs
│           ├── Hotspot.cs
│           ├── MailBox.cs
│           ├── PullBlackspot.cs
│           └── Vendor.cs
│
├── Gatherbuddy/
│   ├── BagHelper.cs
│   ├── GatherbuddyBot.cs
│   ├── GatherbuddySettings.cs
│   ├── PathType.cs
│   ├── Profile.cs
│   └── GUI/
│       └── GbConfig.cs (+ Designer, resources)
│
├── Grind/
│   ├── BehaviorFlags.cs
│   ├── BehaviorFlagsExtensions.cs
│   └── LevelBot.cs
│
├── Professionbuddy/
│   ├── BankType.cs
│   ├── DataStore.cs
│   ├── DepositWithdrawAmount.cs
│   ├── GlobalPBSettings.cs
│   ├── IDeepCopy.cs
│   ├── ItemSelectionType.cs
│   ├── MainForm.cs (+ Designer, resources)
│   ├── PBBranch.cs
│   ├── PBLog.cs
│   ├── PbProfile.cs
│   ├── PbProfileSettingEntry.cs
│   ├── PbProfileSettings.cs
│   ├── PBXmlAttributeAttribute.cs
│   ├── PBXmlElementAttribute.cs
│   ├── ProfessionbuddyBot.cs
│   ├── ProfessionBuddySettings.cs
│   ├── SubCategoryType.cs
│   ├── TradeSkillListView.cs
│   ├── Util.cs
│   ├── BehaviorTree/
│   │   ├── Action.cs, Component.cs, Composite.cs
│   │   ├── Decorator.cs, DecoratorContinue.cs
│   │   ├── PrioritySelector.cs, Sequence.cs
│   │   ├── Wait.2.cs, Wait.cs
│   │   └── WaitContinue.2.cs, WaitContinue.cs
│   ├── ComponentBase/
│   │   ├── DynamicallyCompiledCodeAction.cs
│   │   ├── DynamicallyCompiledCodeComposite.cs
│   │   ├── FlowControlComposite.cs
│   │   ├── IPBComponent.cs
│   │   ├── PBAction.cs
│   │   └── PBComposite.cs
│   ├── Components/
│   │   ├── AttachToTreeHookAction.cs
│   │   ├── BuyItemAction.cs, BuyItemFromAhAction.cs
│   │   ├── CallSubRoutineAction.cs
│   │   ├── CancelAuctionAction.cs
│   │   ├── CastSpellAction.cs
│   │   ├── ChangeBotAction.cs
│   │   ├── CommentAction.cs, CustomAction.cs, DefineAction.cs
│   │   ├── DisenchantAction.cs
│   │   ├── FlyToAction.cs
│   │   ├── GetItemfromBankAction.cs, GetMailAction.cs
│   │   ├── IfComposite.cs
│   │   ├── InteractionAction.cs
│   │   ├── LoadProfileAction.cs, LoadProfileType.cs
│   │   ├── MailItemAction.cs
│   │   ├── MoveToAction.cs
│   │   ├── PutItemInBankAction.cs
│   │   ├── SellItemAction.cs, SellItemOnAhAction.cs
│   │   ├── SettingsAction.cs, StackItemsAction.cs
│   │   ├── SubRoutineComposite.cs
│   │   ├── TrainSkillAction.cs
│   │   ├── WaitAction.cs
│   │   └── WhileComposite.cs
│   ├── Dynamic/
│   │   ├── CodeDriverBase.cs
│   │   ├── CsharpCodeType.cs
│   │   ├── DynamicCodeCompiler.cs
│   │   ├── DynamicProperty.cs
│   │   ├── HBRelogApi.cs
│   │   ├── Helpers.cs
│   │   ├── IDynamicallyCompiledCode.cs
│   │   └── ProfileStatus.cs
│   ├── Icons/
│   │   └── save.png
│   ├── Localization/
│   │   └── Strings.cs (+ Designer, resources)
│   ├── Properties/
│   │   └── Settings.Designer.cs
│   └── PropertyGridUtilities/
│       ├── MetaProp.cs, MetaPropArgs.cs, PropertyBag.cs
│       ├── Converters/
│       │   └── GoldEditorConverter.cs
│       └── Editors/
│           ├── EntryEditor.cs
│           ├── FileLocationEditor.cs
│           ├── GoldEditor.cs
│           └── LocationEditor.cs
│
└── Quest/
    ├── QuestBot.cs
    ├── QuestDebug.cs
    ├── QuestManager.cs
    ├── QuestState.cs
    ├── Actions/
    │   ├── ActionSelectActiveQuest.cs
    │   ├── ActionSelectAvailableQuest.cs
    │   ├── ForcedBehaviorExecutor.cs
    │   └── Combat/
    │       ├── ActionMoveToTarget.cs
    │       ├── ActionPull.cs
    │       └── ActionSetTarget.cs
    ├── Decorators/
    │   └── Combat/
    │       └── DecoratorNeedToFindTarget.cs
    ├── Objectives/
    │   ├── CollectItemObjective.cs
    │   ├── DropDatabase.cs
    │   ├── GrindObjective.cs
    │   ├── QuestObjective.cs
    │   └── UseGameObjectObjective.cs
    ├── QuestOrder/
    │   ├── ForcedBehavior.cs
    │   ├── ForcedCodeBehavior.cs
    │   ├── ForcedGrindTo.cs
    │   ├── ForcedIf.cs
    │   ├── ForcedMoveTo.cs
    │   ├── ForcedNothing.cs
    │   ├── ForcedQuestObjective.cs
    │   ├── ForcedQuestPickUp.cs
    │   ├── ForcedQuestTurnIn.cs
    │   ├── ForcedSingleton.cs
    │   ├── ForcedUseItem.cs
    │   ├── ForcedWhile.cs
    │   └── QuestOrder.cs
    └── Resources/
        └── QuestBotResources.cs (+ Designer, resources)
```

---

## GarrisonBuddy/ — WoD-Only System (ENTIRELY NEW)

```
GarrisonBuddy/
├── GarrisonBuddy.cs
├── GarrisonBuddySettings.cs
├── JsonSettings.cs
├── TemporaryLuaEvent.cs
├── XmlSettings.cs
├── Helpers/
│   └── TradeskillFrame.cs
├── Logic/
│   ├── CommonBehaviors.cs
│   ├── Generic.cs
│   ├── MissionLogic.cs
│   └── Buildings/
│       ├── BuildingLogic.cs
│       ├── BuildingSettings.cs
│       ├── FactionQuestEntry.cs
│       ├── WorkOrderMaterial.cs
│       ├── Large/
│       │   ├── WarMillBuilding.cs
│       │   ├── WarMillBuildingSettings.cs
│       │   └── WarMillDailyQuestType.cs
│       ├── Medium/
│       │   ├── BarnBuilding.cs
│       │   ├── GladiatorsSanctumBuilding.cs
│       │   ├── InnBuilding.cs
│       │   ├── LumberMillBuilding.cs
│       │   ├── TradingPostBuilding.cs
│       │   └── TradingPostBuildingSettings.cs
│       ├── Prebuilt/
│       │   ├── HerbGardenBuilding.cs
│       │   ├── HerbGardenBuildingSettings.cs
│       │   ├── MineBuilding.cs
│       │   └── MineBuildingSettings.cs
│       └── Small/
│           ├── AlchemyBuilding.cs
│           ├── BlacksmithingBuilding.cs
│           ├── EnchantingBuilding.cs
│           ├── EngineeringBuilding.cs
│           ├── InscriptionBuilding.cs
│           ├── JewelcraftingBuilding.cs
│           ├── LeatherworkingBuilding.cs
│           ├── SalvageYardBuilding.cs
│           ├── StorehouseBuilding.cs
│           └── TailoringBuilding.cs
├── Planning/
│   ├── Followers/
│   │   └── FollowerCalculator.cs
│   └── Missions/
│       ├── DebugMissionStrategy.cs
│       ├── IMissionPlanSorter.cs
│       ├── MissionCalculator.cs
│       ├── MissionGroup.cs
│       ├── MissionPlan.cs
│       ├── MissionPlanner.cs
│       ├── MissionRewards.cs
│       └── MissionStrategy.cs
├── Quests/
│   ├── BuildingPreQuest.cs
│   ├── QuestMap.cs
│   ├── TradingPostQuest.cs
│   └── Buildings/
│       ├── HerbGardenQuest.cs
│       ├── LumberMillQuest.cs
│       ├── LumberMillQuestPart2.cs
│       ├── WorkOrderBuildingPreQuest.cs
│       └── Small/
│           ├── AlchemyQuest.cs
│           ├── BlacksmithingQuest.cs
│           ├── EnchantingQuest.cs
│           ├── EngineeringQuest.cs
│           ├── InscriptionQuest.cs
│           ├── JewelcraftingQuest.cs
│           ├── LeatherworkingQuest.cs
│           ├── SalvageYardQuest.cs
│           └── StorehouseQuest.cs
├── Tradeskills/
│   └── InscriptionHelper.cs
└── UI/
    ├── EnumToBooleanConverter.cs
    ├── InverseBooleanConverter.cs
    ├── InvertableBooleanToVisibilityConverter.cs
    ├── NullToVisibilityConverter.cs
    ├── SettingsWindow.xaml.cs
    ├── TypeToVisibilityConverter.cs
    ├── Vector3ToStringConverter.cs
    └── Controls/
        ├── BuildingSettingsControl.xaml.cs
        ├── TradingPostWorkBuyoutControl.xaml.cs
        ├── TradingPostWorkOrderLimitControl.xaml.cs
        └── WorkOrderLimitControl.xaml.cs
```

---

## Buddy/ — Auth, Coroutines, Overlay, Store (NEW)

```
Buddy/
├── Auth/
│   ├── AuthInfo.cs
│   ├── Region.cs
│   ├── Math/
│   │   ├── Vector2.cs
│   │   └── Vector3.cs
│   ├── Objects/
│   │   ├── StoreProduct.cs
│   │   ├── UsageInfo.cs
│   │   ├── WoWFragment.cs
│   │   ├── WoWMailbox.cs
│   │   ├── WoWMailboxEx.cs
│   │   └── WoWNpc.cs
│   └── SR/
│       ├── AClient.cs
│       ├── d0.cs
│       ├── IA.cs
│       ├── IAChannel.cs
│       └── r0.cs
├── Coroutines/
│   ├── Coroutine.cs
│   ├── CoroutineBehaviorException.cs
│   ├── CoroutineException.cs
│   ├── CoroutineStatus.cs
│   ├── CoroutineStoppedException.cs
│   ├── CoroutineUnhandledException.cs
│   └── ExternalTaskWaitResult.cs
├── Overlay/
│   ├── OverlayManager.cs
│   ├── OverlayUIComponent.cs
│   ├── OverlayUIComponentBase.cs
│   ├── Commands/
│   │   └── RelayCommand.cs
│   ├── Controls/
│   │   └── OverlayControl.cs
│   ├── Notifications/
│   │   ├── ToastSettings.cs
│   │   └── ToastUIComponent.cs
│   └── Properties/
│       └── Settings.Designer.cs
└── Store/
    └── StoreInfo.cs
```

---

## Other Top-Level Directories

```
CommonBehaviors/
├── ActionWaitForLuaEvent.cs
├── WaitLuaEvent.cs
├── Actions/
│   ├── ActionAlwaysFail.cs
│   ├── ActionAlwaysSucceed.cs
│   ├── ActionClearPoi.cs
│   ├── ActionDebugString.cs
│   ├── ActionIdle.cs
│   ├── ActionMoveToPoi.cs
│   ├── ActionRunCoroutine.cs            ← NEW (coroutine support)
│   ├── ActionSetActivity.cs
│   ├── ActionSetPoi.cs
│   ├── DebugStringDelegate.cs
│   ├── GetPointDelegate.cs
│   ├── NavigationAction.cs
│   ├── NavigationInfo.cs
│   ├── NavTypeDelegate.cs
│   ├── RetrieveBotPoiDelegate.cs
│   └── SleepForLagDuration.cs
├── Decorators/
│   ├── DecoratorContextIs.cs
│   ├── DecoratorFrameIsVisible.cs
│   ├── DecoratorIsNotPoiType.cs
│   └── DecoratorIsPoiType.cs
└── Resources/
    └── CommonBehaviorsResources.cs (+ Designer, resources)

Levelbot/
├── FormLevelbotSettings.cs (+ Designer, resources)
├── HandleCombatCoroutineTask.cs         ← NEW (coroutine)
├── Actions/
│   ├── Combat/
│   │   ├── ActionMoveToKillPoi.cs
│   │   ├── ActionMoveToTarget.cs
│   │   ├── ActionPull.cs
│   │   └── ActionSetTarget.cs
│   ├── Death/
│   │   ├── ActionReleaseFromCorpse.cs
│   │   └── ActionSuceedIfDeadOrGhost.cs
│   └── General/
│       ├── ActionMountVendor.cs
│       └── ActionSelectReward.cs
├── Decorators/
│   ├── Combat/
│   │   └── DecoratorNeedToFindTarget.cs
│   └── Death/
│       ├── DecoratorInstanceRelease.cs
│       ├── DecoratorNeedToMoveToCorpse.cs
│       ├── DecoratorNeedToRelease.cs
│       └── DecoratorNeedToTakeCorpse.cs
├── ProfileCreation/
│   ├── ProfileVendorListViewItem.cs
│   ├── QualityFlags.cs
│   └── Forms/
│       ├── FormFindVendors.cs (+ Designer, resources)
│       ├── FormProfileCreator.cs (+ Designer, resources)
│       └── FormSelectProfileName.cs (+ Designer, resources)
└── Resources/
    └── LevelbotResources.cs (+ Designer, resources)

PartyBot/
├── DiscoBot.cs
├── PartyBotSettings.cs
└── Forms/
    └── FormConfig.cs (+ Designer, resources)

RoadMapper/
├── FormRoadMapper.cs (+ Designer)
└── RoadMapper.cs

Tripper/
├── LZMACompression/
│   └── Lzma.cs
├── MeshMisc/
│   ├── AbilityFlags.cs
│   ├── AreaType.cs
│   ├── GraphicalHelper.cs
│   ├── InvalidTileDataException.cs
│   ├── IoCGate.cs
│   ├── MapConsts.cs
│   ├── MeshManager.cs
│   ├── MeshMapCalculator.cs
│   ├── SotAGate.cs
│   ├── TileDataHeader.cs
│   ├── TileDataVersionException.cs
│   └── TileIdentifier.cs
└── Navigation/
    ├── GarrisonMeshManager.cs            ← NEW (WoD)
    ├── IMeshManager.cs
    ├── MapLoadedEventArgs.cs
    ├── NavHelper.cs
    ├── NavigatorLogMessage.cs
    ├── PathFindProgressEventArgs.cs
    ├── PathFindResult.cs
    ├── PathFindStep.cs
    ├── PathPostProcessing.cs
    ├── TileLoadedEventArgs.cs
    ├── WorldMeshManager.cs
    ├── WowNavigator.cs
    └── WowQueryFilter.cs

Tripwire/
└── Client/
    └── Packets/
        ├── EndOfPacketException.cs
        └── PacketException.cs

NewMixedMode/
├── FormChooser.cs (+ Designer, resources)
├── MixedModeEx.cs
├── MixedModeSettings.cs
└── Resources/
    └── NewMixedModeResources.cs (+ Designer, resources)

SevenZip/
├── CoderPropID.cs
├── ICodeProgress.cs
├── ICoder.cs
├── ISetCoderProperties.cs
├── ISetDecoderProperties.cs
├── IWriteCoderProperties.cs
├── Buffer/
│   ├── InBuffer.cs
│   └── OutBuffer.cs
├── CommandLineParser/
│   ├── CommandForm.cs
│   ├── Parser.cs
│   ├── SwitchForm.cs
│   ├── SwitchResult.cs
│   └── SwitchType.cs
└── Compression/
    ├── LZ/
    │   ├── BinTree.cs
    │   ├── InWindow.cs
    │   └── OutWindow.cs
    └── LZMA/
        ├── Decoder.cs
        └── Encoder.cs

Infralution/
└── Localization/
    └── Wpf/
        ├── CultureManager.cs
        ├── CultureSelectWindow.xaml.cs
        ├── GetResourceHandler.cs
        ├── ManagedMarkupExtension.cs
        ├── MarkupExtensionManager.cs
        ├── ResourceEnumConverter.cs
        ├── ResxExtension.cs
        └── UICultureExtension.cs

JetBrains/
└── Annotations/
    └── (36 attribute .cs files — code analysis annotations)

Debug/
├── BindingsDebugWindow.xaml.cs
└── ReflectPropertyDescriptorInfo.cs

Properties/
└── AssemblyInfo.cs
```

---

## Obfuscated Namespace Folders (ns0 – ns104)

105 folders containing **~390 obfuscated .cs files** total. These are decompilation artifacts with mangled names.

| Folder | Files | Notable Contents |
|--------|-------|-----------------|
| ns0 | 1 | Class37.cs |
| ns1 | 6 | Class54, Enum0, Interface0–3 |
| ns2 | 1 | Class88 |
| ns3 | 4 | Class89 (+ Designer/resources) |
| ns4–ns15 | 1 each | Class98–Class110 |
| ns16 | 1 | Class138 |
| ns17 | 1 | Class164 |
| ns18 | 2 | Class165, Class339 |
| ns19 | 3 | Class179, Class185, Enum3 |
| ns20 | 3 | Class191–193 |
| ns21 | 10 | Class198–206, Delegate1, Enum4–5, EventArgs0 |
| ns22 | 1 | Class196 |
| ns23 | 18 | Class207–220, Enum6, Exception0–1, Interface4–10 |
| ns24 | 6 | Class221–222, Struct36–39 |
| ns25 | 1 | Class227 |
| ns26 | 12 | Class230, Delegate2, Enum8–12, Interface11–12, Struct50–52 |
| ns27 | 1 | Class229 |
| ns28 | 4 | Class231 (+ Designer/resources) |
| ns29 | 1 | Class232 |
| ns30 | 4 | Class233–236 |
| ns31 | 4 | Class238–240, Enum13 |
| ns32 | 1 | Class252 |
| ns33 | 4 | Class363 (+ Designer/resources) |
| ns34 | 1 | Class365 |
| ns35 | 2 | Class373–374 |
| ns36 | 3 | Class469, Class1039, Class1050 |
| ns37 | 2 | Class471, Class473 |
| ns38 | 1 | Class472 |
| ns39 | 1 | Class484 |
| ns40 | 1 | Enum18 |
| ns41 | 1 | Class490 |
| ns42 | 7 | Class561–584 |
| ns43 | 1 | Class605 |
| ns44 | 2 | Class664–665 |
| ns45 | 2 | Class675–676 |
| ns46 | 1 | Class677 |
| ns47 | 1 | Class678 |
| ns48 | 1 | Class679 |
| ns49 | 1 | Class680 |
| ns50 | 4 | Class685 (+ Designer/resources) |
| ns51 | 4 | Class690 (+ Designer/resources) |
| ns52 | 1 | Class703 |
| ns53 | 6 | Class714, 717, 718, 738, 739, 754 |
| ns54 | 4 | Class756 (+ Designer/resources) |
| ns55 | 1 | Interface13 |
| ns56 | 3 | Class779, 784, 798 |
| ns57 | 3 | Class823–824, Enum21 |
| ns58 | 1 | Class820 |
| ns59 | 9 | Class853, 856–857, Enum22–26, Exception3 |
| ns60 | 1 | Class858 |
| ns61 | 9 | Class860–877, 905, Struct330 |
| ns62 | 1 | Struct373 |
| ns63 | 1 | Class924 |
| ns64 | 1 | Struct379 |
| ns65 | 13 | Class928–938, Enum36–37 |
| ns66 | 1 | Class946 |
| ns67 | 3 | Class948, 950, Struct391 |
| ns68 | 4 | Struct400, 401, 403, 404 |
| ns69 | 6 | Class970, 1005, Enum43, Struct417–420 |
| ns70 | 1 | Class1008 |
| ns71 | 1 | Class1062 |
| ns72 | 10 | Class1066, 1449–1456, Enum54–55 |
| ns73 | 3 | Class1457–1458, Interface14 |
| ns74 | 9 | Class1068–1078, Enum45, EventArgs1, Struct427–428 |
| ns75 | 1 | Class1069 |
| ns76 | 12 | Class1080–1092, Interface15, Struct429–431 |
| ns77 | 1 | Class1088 |
| ns78 | 4 | Struct432–435 |
| ns79 | 5 | Struct436–440 |
| ns80 | 4 | Struct441–444 |
| ns81 | 8 | Class1103–1110, Enum47, Struct445–447 |
| ns82 | 1 | Class1109 |
| ns83 | 6 | Class1116–1153, Enum48, EventArgs2 |
| ns84 | 2 | Attribute1, Class1117 |
| ns85 | 1 | Class1172 |
| ns86 | 5 | Class1173, RoutineSelectionForm (+ Designer/resources) |
| ns87 | 3 | Class1185, 1195, 1196 |
| ns88 | 3 | Class1204, 1208, 1216 |
| ns89 | 1 | Class1253 |
| ns90 | 7 | Class1269, 1288–1298, Enum49 |
| ns91 | 2 | Class1323, 1331 |
| ns92 | 1 | Class1336 |
| ns93 | 18 | Class1338–1351, Enum51, Exception4–5, Interface16–22 |
| ns94 | 6 | Class1352–1353, Struct521–524 |
| ns95 | 2 | Class1386–1387 |
| ns96 | 1 | Class1397 |
| ns97 | 1 | Class1433 |
| ns98 | 3 | Class1437, Exception6–7 |
| ns99 | 6 | Class1438–1439, Struct594–597 |
| ns100 | 2 | Interface23–24 |
| ns101 | 1 | Class1440 |
| ns102 | 1 | Class1447 |
| ns103 | 1 | Class1464 |
| ns104 | 5 | Class1468, Enum57, Stream0–2 |

### Other Obfuscated Folders

| Folder | Files |
|--------|-------|
| `OVmgooAFpSqTGNVUwlNmgEBKXjPDc/` | BIYxMvvqkMOlDgGiCRGSoKuYuMxc.cs, SVLIgmQpSfAdlkzVtHMMPhArjGwsA.cs |
| `uiHnQlyKuZiGdpWcHVumKGLStYGl/` | nYgRYyscaCuJIGLBPMktsoIkcAFp.cs |

---

## Evolution Analysis: Styx/ Namespace Tree (4.3.4 → 5.4.8 → 6.2.3)

### Systems Present in 6.2.3 but NOT in 4.3.4

| System | Location in 6.2.3 | Notes |
|--------|--------------------|-------|
| **Garrison system** | `WoWInternals/Garrison/` (15 files), `WoWInternals/DB/Garr*.cs` (18 files) | WoD-only, no WotLK equivalent |
| **AreaTrigger objects** | `WoWObjects/AreaTriggerShapes/` (6 files), `WoWObjects/WoWAreaTrigger.cs` | WoD mechanic |
| **Scene objects** | `Offsets/WoWSceneObjectFields.cs`, `WoWInternals/World/WorldScene.cs` | WoD phasing |
| **Conversation fields** | `Offsets/WoWConversationFields.cs` | WoD |
| **DB2 table system** | `WoWInternals/DB/Db2Table.cs` replaces old DBC-only | Expanded data files |
| **Pet Battles** | `WoWInternals/WoWPetBattleState.cs`, `DB/BattlePetSpecies.cs`, `DB/PetType.cs` | MoP system |
| **Scenarios** | `DB/Scenario.cs`, `DB/ScenarioStep.cs`, `DB/ScenarioType.cs` | MoP system |
| **Coroutines** | `CommonBot/Coroutines/` (6 files), `Buddy/Coroutines/` (7 files) | Async support added |
| **Capability system** | `CommonBot/Routines/Capability*.cs` (5 files) | Combat routine capabilities |
| **Overlay** | `Buddy/Overlay/` (7 files) | In-game overlay UI |
| **Flightor navigation** | `Pathing/FlightorAnnotation/`, `Pathing/FlightorNavigation/` | Flying path system |
| **Spell charges** | `WoWInternals/SpellChargeInfo.cs` | MoP+ spell charge system |
| **Item context** | `WoWInternals/ItemContext.cs` | WoD item difficulty tiers |
| **WoWGuid types** | `WoWInternals/WoWGuid.cs`, `WoWGuidType.cs` | 128-bit GUID (MoP+) |
| **New BG landmarks** | `BattleForGilneas*`, `DeepwindGorge*` | Post-Cata BGs |

### Styx/ Structural Changes (4.3.4 → 6.2.3)

| 4.3.4 Structure | 6.2.3 Structure | Change |
|-----------------|-----------------|--------|
| `Styx.Logic.Combat/` | `Styx.CommonBot.Routines/` | Reorganized |
| `Styx.Logic.Pathing/` | `Styx.Pathing/` | Promoted to top-level |
| `Styx.Logic.Profiles/` | `Styx.CommonBot.Profiles/` | Moved under CommonBot |
| `Styx.Logic.Inventory/` | `Styx.CommonBot.Inventory/` | Moved under CommonBot |
| `Styx.Logic.Questing/` | `Styx.CommonBot.Profiles.Quest/` | Merged into Profiles |
| `Styx.Logic.AreaManagement/` | `Styx.CommonBot.AreaManagement/` | Moved under CommonBot |
| `Styx.Database/` | `Styx.CommonBot.Database/` | Moved under CommonBot |
| `Styx.Loaders/` | `Styx.Loaders/` | Same |
| `Styx.Helpers/` | `Styx.Helpers/` | Same (expanded) |
| No equivalent | `Styx.Common/` | NEW: utilities, math, hooks |
| No equivalent | `Styx.Patchables/` | NEW: runtime patchable structs |
| No equivalent | `Styx.XmlEngine/` | NEW: XML attribute engine |
| No equivalent | `Styx.Localization/` | NEW: i18n |
| No equivalent | `Styx.Resources/` | NEW: embedded resources |

### New Bot Types in 6.2.3 (not in 4.3.4)

| Bot | Notes |
|-----|-------|
| `Bots/ArchaeologyBuddy/` | Cata archaeology |
| `Bots/BGBuddy/` | Battleground automation |
| `Bots/DungeonBuddy/` | Dungeon automation with avoidance system |
| `Bots/Gatherbuddy/` | Gathering-specific bot |
| `Bots/Professionbuddy/` | Profession automation |
| `GarrisonBuddy/` | WoD garrison (separate from Bots/) |
| `PartyBot/` | Party follower bot |
| `RoadMapper/` | Path recording tool |

### Summary Statistics

| Metric | 4.3.4 | 6.2.3 |
|--------|-------|-------|
| Styx/ .cs files | ~200 | ~500+ |
| WoWInternals/ subdirs | 5 | 10 |
| DB/DBC tables | ~15 | 70+ |
| Bots | 2 (Grind, Quest) | 7 + GarrisonBuddy |
| Obfuscated ns* folders | 0 | 105 |
| Total .cs files | ~350 | ~1,638 |
| Coroutine support | No | Yes |
| TreeSharp (in Styx) | No | Yes (embedded copy) |
