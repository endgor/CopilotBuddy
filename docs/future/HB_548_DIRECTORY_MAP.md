# HB 5.4.8 — Complete Directory Map

> **Root:** `c:\Users\Texy\Desktop\.Reference\.hb 5.4.8\Honorbuddy\`
> **Version:** MoP 5.4.8 (decompiled, obfuscated ns* folders)
> **Generated:** 2026-02-09

---

## Root Files

```
app.manifest
App.xaml
App.xaml.cs
DevToolsWindow.xaml
DevToolsWindow.xaml.cs
EnumLocalizedDescriptionConverter.cs
ErrorWindow.xaml
ErrorWindow.xaml.cs
Honorbuddy.csproj
Honorbuddy.ico
InvalidProcessException.cs
LogicalAndConverter.cs
LoginWindow.xaml
LoginWindow.xaml.cs
MainWindow.xaml
MainWindow.xaml.cs
NegateConverter.cs
PluginsWindow.xaml
PluginsWindow.xaml.cs
ProcessSelectorWindow.xaml
ProcessSelectorWindow.xaml.cs
SettingsWindow.xaml
SettingsWindow.xaml.cs
SettingsWrap.cs
UIHelpers.cs
UpdateWindow.xaml
UpdateWindow.xaml.cs
WebProfileWindow.xaml
WebProfileWindow.xaml.cs
```

---

## Root Directories (alphabetical)

```
Bots/
Buddy/
BuddyMonitor/
CommonBehaviors/
Debug/
GrindBuddy2/
images/
Infralution/
JetBrains/
Levelbot/
NewMixedMode/
ns0/ .. ns51/          (52 obfuscated namespace folders)
PartyBot/
Plugins/
Properties/
SevenZip/
Styx/
themes/
Tripper/
Tripwire/
```

---

## Styx/ — Core Bot Engine (FULL TREE)

### Styx/ root files
```
CantCompileException.cs
DifficultyColor.cs
EmoteState.cs
FactionId.cs
GameError.cs
GameObjectDataSlot.cs
GameState.cs
GlueScreen.cs
GraphicsApi.cs
Guard.cs
HonorbuddyUnableToStartException.cs
InvalidExecutorException.cs
InvalidObjectPointerException.cs
InventoryType.cs
LfgCategory.cs
LfgState.cs
MirrorTimerType.cs
NavType.cs
Pulsator.cs
PvPState.cs
QuestGiverStatus.cs
ShapeshiftForm.cs
SheathType.cs
SkillLine.cs
SpellAttributes.cs
SpellAttributesEx.cs
SpellAttributesEx2.cs
SpellAttributesEx3.cs
SpellAttributesEx4.cs
SpellAttributesEx5.cs
SpellAttributesEx6.cs
SpellAttributesEx7.cs
SpellAttributesEx8.cs
StatType.cs
StyxWoW.cs
ThreatStatus.cs
UnitNPCFlags.cs
UserException.cs
WoWBagSlot.cs
WoWClass.cs
WoWCreatureSkinType.cs
WoWCreatureType.cs
WoWCursorType.cs
WoWEquipSlot.cs
WoWFactionGroup.cs
WoWGameObjectState.cs
WoWGameObjectType.cs
WoWGender.cs
WoWInteractType.cs
WoWInventorySlot.cs
WoWItemArmorClass.cs
WoWItemBondType.cs
WoWItemClass.cs
WoWItemConsumableClass.cs
WoWItemContainerClass.cs
WoWItemGemClass.cs
WoWItemGlyphClass.cs
WoWItemKeyClass.cs
WoWItemMiscClass.cs
WoWItemProjectileClass.cs
WoWItemQuality.cs
WoWItemQuiverClass.cs
WoWItemRecipeClass.cs
WoWItemStatType.cs
WoWItemTradeGoodsClass.cs
WoWItemWeaponClass.cs
WoWObjectType.cs
WoWObjectTypeFlag.cs
WoWPoint.cs
WoWPowerType.cs
WoWQuestType.cs
WoWRace.cs
WoWSocketColor.cs
WoWSpec.cs
WoWStateFlag.cs
WoWUnitClassificationType.cs
WoWUnitReaction.cs
```

### Styx/Common/
```
AddCompositeListOperation.cs
Arguments.cs
AsmHelper.cs
AssemblyLoader.cs
Beta.cs
CapacityQueue.cs
CircularQueue.cs
CommandLine.cs
CompositeListOperation.cs
DualHashSet.cs
Extensions.cs
FileCache.cs
FinishedMeasuringCallback.cs
Flash.cs
FlashFlags.cs
ForcedCulture.cs
HookDescription.cs
HookExecutor.cs
Hotkey.cs
Hotkeys.cs
HotkeysManager.cs
INativeObject.cs
IndexedList.cs
InsertCompositeListOperation.cs
IRangeAble.cs
LineSegment.cs
LinqExtensions.cs
Logging.cs
LogLevel.cs
MathEx.cs
ModifierKeys.cs
PerformanceTimer.cs
Quaternion.cs
Range.cs
RangedDictionary.cs
Ray.cs
ReplaceCompositeListOperation.cs
ScriptManager.cs
ShapeHelper.cs
Sphere.cs
StyxLog.cs
TimedRecordKeeper.cs
TimestampType.cs
TreeHooks.cs
TypeLoader.cs
TypeOnlyLoader.cs
Utilities.cs
ValuePair.cs
Vector2.cs
Vector3.cs
```

#### Styx/Common/Compiler/
```
CodeCompiler.cs
```

#### Styx/Common/Helpers/
```
AutoTimer.cs
ByteArray.cs
WaitTimer.cs
WaitTimerFinishedHandler.cs
```

#### Styx/Common/WpfControls/
```
MenuButton.cs
SplitButton.cs
```

### Styx/CommonBot/
```
Blacklist.cs
BlacklistFlags.cs
BotBase.cs
BotEvents.cs
BotManager.cs
BuyItemsEventArgs.cs
BuyItemsEventHandler.cs
CanMountDelegate.cs
Chat.cs
FlightPathReason.cs
FlightPaths.cs
GameStats.cs
HealTargeting.cs
InactivityDetector.cs
IncludeTargetsFilterDelegate.cs
Landmarks.cs
LocationRetriever.cs
LootPredictor.cs
LootTargeting.cs
MailItemsEventArgs.cs
MailItemsEventHandler.cs
Mount.cs
MountType.cs
MountUpEventArgs.cs
PulseFlags.cs
RaFHelper.cs
RemoveTargetsFilterDelegate.cs
Rest.cs
SellItemsEventArgs.cs
ShutdownRequestedEventArgs.cs
SpellCollection.cs
SpellFindResults.cs
SpellManager.cs
StatusTextChangedEventArgs.cs
Targeting.cs
TargetListUpdateFinishedDelegate.cs
TreeRoot.cs
VendorItemsEventHandler.cs
Vendors.cs
WeighTargetsDelegate.cs
XmlFlightNode.cs
```

#### Styx/CommonBot/AreaManagement/
```
Area.cs
AreaManager.cs
AreaType.cs
GrindArea.cs
Hotspot.cs
HotspotExtensions.cs
HotspotManager.cs
PolygonArea.cs
PvPArea.cs
QuestArea.cs
```

##### Styx/CommonBot/AreaManagement/Triangulation/
```
Edge.cs
Triangle.cs
```

#### Styx/CommonBot/Bars/
```
ActionBar.cs
ActionBarType.cs
ActionButton.cs
ActionButtonSubType.cs
ActionButtonType.cs
SpellActionButton.cs
```

#### Styx/CommonBot/CharacterManagement/
```
AutoEquipper.cs
CharacterManager.cs
ClassProfile.cs
ClassProfileLoadException.cs
ClassProfileLocalization.cs
DetailedWeaponStyle.cs
RollType.cs
TalentPlacement.cs
TalentPlacementSet.cs
TalentSelector.cs
WeaponStyle.cs
WeighableStatType.cs
WeightSet.cs
```

#### Styx/CommonBot/Coroutines/
```
CommonCoroutines.cs
CoroutineCompositeExtensions.cs
CoroutineTask.2.cs
CoroutineTask.cs
CoroutineTaskAwaiter.2.cs
CoroutineTaskAwaiter.cs
```

#### Styx/CommonBot/Database/
```
Connection.cs
NpcQueries.cs
NpcResult.cs
```

#### Styx/CommonBot/Frames/
```
AuctionFrame.cs
AuctionListType.cs
AuctionPostTime.cs
Frame.cs
GossipEntry.cs
GossipFrame.cs
GossipQuestEntry.cs
ItemQuality.cs
LootFrame.cs
LootRarity.cs
LootSlotInfo.cs
MailFrame.cs
MerchantFrame.cs
MerchantItem.cs
QuestFrame.cs
TaxiFrame.cs
TrainerFrame.cs
TrainerServiceFilter.cs
```

#### Styx/CommonBot/Inventory/
```
Consumable.cs
EquipmentManager.cs
EquipmentSet.cs
InventoryManager.cs
InventorySlot.cs
LootRoll.cs
WoWPrice.cs
```

#### Styx/CommonBot/ObjectDatabase/
```
MailboxResult.cs
Query.cs
```

#### Styx/CommonBot/POI/
```
BotPoi.cs
PoiType.cs
PoiTypeExtensions.cs
```

#### Styx/CommonBot/Profiles/
```
Blackspot.cs
CustomBehaviorFileNameAttribute.cs
CustomForcedBehavior.cs
ForceMailManager.cs
HotspotCollection.cs
Mailbox.cs
MailboxManager.cs
Profile.cs
ProfileAttributeExpectedException.2.cs
ProfileAttributeExpectedException.cs
ProfileException.cs
ProfileManager.cs
ProfileMissingAttributeException.2.cs
ProfileMissingAttributeException.cs
ProfileMissingElementException.cs
ProfileNotFoundException.cs
ProfileTagExpectedException.cs
ProfileUnknownAttributeException.cs
ProfileUnknownElementException.cs
ProtectedItemsManager.cs
UnknownProfileElementEventArgs.cs
Vendor.cs
VendorManager.cs
VendorTypeExtensions.cs
```

##### Styx/CommonBot/Profiles/Quest/
```
CollectFrom.cs
CollectFromCollection.cs
CollectFromType.cs
CollectItemObjectiveInfo.cs
KillMobObjectiveInfo.cs
ObjectiveInfo.cs
ObjectiveType.cs
QuestInfo.cs
TurnInObjectiveInfo.cs
UseObjectObjectiveInfo.cs
```

###### Styx/CommonBot/Profiles/Quest/Order/
```
AbandonQuestNode.cs
CheckpointNode.cs
ClearAvoidMobsNode.cs
ClearBlacklistNode.cs
ClearGrindAreaNode.cs
ClearMailboxNode.cs
ClearVendorNode.cs
CodeNode.cs
ConditionHelper.cs
DelayCompiledExpression.cs
DisableRepairNode.cs
Else.cs
ElseIf.cs
EnableRepairNode.cs
ExpressionError.cs
ExpressionSet.cs
GrindToNode.cs
IfNode.cs
INodeContainer.cs
MoveToNode.cs
ObjectiveNode.cs
OrderNode.cs
OrderNodeCollection.cs
OrderNodeType.cs
PickUpNode.cs
ProfileHelperFunctionsBase.cs
QuestBehaviorHelper.cs
QuestObjectType.cs
SetAvoidMobsNode.cs
SetBlacklistNode.cs
SetGrindAreaNode.cs
SetLootMobsNode.cs
SetLootRadiusNode.cs
SetMailboxNode.cs
SetTargetingDistanceNode.cs
SetUseMountNode.cs
SetVendorNode.cs
ToggleBehaviorNode.cs
TurnInNode.cs
UseItemNode.cs
WhileNode.cs
```

#### Styx/CommonBot/Routines/
```
CapabilityFlags.cs
CapabilityState.cs
CombatRoutine.cs
IBehaviors.cs
ICombatRoutine.cs
InvalidRoutineWrapper.cs
RoutineManager.cs
```

### Styx/Helpers/
```
ActivitySetter.cs
BGBotSettings.cs
CachedValue.cs
CharacterSettings.cs
CombatAssistSettings.cs
DefaultValueAttribute.cs
DictionaryExtensions.cs
EncryptedAttribute.cs
Extensions.cs
FlagCheckedListBox.cs
FlagCheckedListBoxItem.cs
FlagEnumUIEditor.cs
GameDebugAddStringDelegate.cs
GlobalSettings.cs
KeyboardManager.cs
KeyHelpers.cs
LevelbotSettings.cs
PerFrameCachedValue.cs
PVPSettings.cs
SettingAttribute.cs
Settings.cs
SettingsEx.cs
TimeCachedValue.cs
UISettings.cs
WoWMathHelper.cs
WoWSpecExtensions.cs
XmlExtensions.cs
XmlUtils.cs
```

### Styx/Loaders/
```
DllLoader.cs
DynamicLoader.cs
```

### Styx/Localization/
```
Globalization.cs
Globalization.resources
LocalizedDescriptionAttribute.cs
```

### Styx/Offsets/
```
WoWAreaTriggerFields.cs
WoWContainerFields.cs
WoWCorpseFields.cs
WoWDynamicObjectFields.cs
WoWGameObjectFields.cs
WoWItemFields.cs
WoWObjectFields.cs
WoWPlayerFields.cs
WoWSceneObjectFields.cs
WoWUnitFields.cs
```

#### Styx/Offsets/Pending/
```
PendingOffsets.cs
```

### Styx/Patchables/
```
AuraFlags.cs
ClientDb.cs
IncomingHeal.cs
LandMarkEntry.cs
LootRollItemInfo.cs
MouseButton.cs
QueuedBattlegroundInfo.cs
```

### Styx/Pathing/
```
AvoidanceManager.cs
BlackspotManager.cs
BlackspotQueryFlags.cs
ClickToMoveMover.cs
Flightor.cs
FlightorLandSystem.cs
INavigationProvider.cs
IPlayerMover.cs
IStuckHandler.cs
ITerrainHeightProvider.cs
KeyboardMover.cs
LandBoxLocation.cs
LandLocation.cs
MeshMovePath.cs
MeshNavigator.cs
MoveResult.cs
MoveResultExtensions.cs
NavigationProvider.cs
NavigationProviderChangedEventArgs.cs
Navigator.cs
PathGenerationFailStep.cs
StuckHandler.cs
```

#### Styx/Pathing/Avoidance/
```
Avoid.cs
AvoidCluster.cs
AvoidInfo.cs
AvoidLocation.cs
AvoidLocationInfo.cs
AvoidObject.cs
AvoidObjectInfo.cs
AvoidPathNotFoundException.cs
AvoidPathResult.cs
AvoidSide.cs
AvoidTracelineResult.cs
ClusterHit.cs
Helpers.cs
LineCircleTangentPoints.cs
LineClusterTangentPoints.cs
MeshAvoidanceManager.cs
PathResult.cs
```

#### Styx/Pathing/FlightorNavigation/
```
Areas.cs
BlackspotManager.cs
PolyNav.cs
```

#### Styx/Pathing/OnDemandDownloading/
```
InvalidFileFormatException.cs
```

### Styx/Plugins/
```
HBPlugin.cs
PluginContainer.cs
PluginManager.cs
PluginWrapper.cs
```

### Styx/Resources/
```
StyxResources.cs
StyxResources.resources
```

### Styx/SAAttribs/ (SmartAssembly obfuscation attributes)
```
DoNotCaptureAttribute.cs
DoNotCaptureVariablesAttribute.cs
DoNotEncodeStringsAttribute.cs
DoNotMoveAttribute.cs
DoNotMoveMethodsAttribute.cs
DoNotObfuscateAttribute.cs
DoNotObfuscateControlFlowAttribute.cs
DoNotObfuscateTypeAttribute.cs
DoNotPruneAttribute.cs
DoNotPruneTypeAttribute.cs
DoNotSealTypeAttribute.cs
EncodeStringsAttribute.cs
ExcludeFromMemberRefsProxyAttribute.cs
ObfuscateControlFlowAttribute.cs
ObfuscateNamespaceToAttribute.cs
ObfuscateToAttribute.cs
ReportExceptionAttribute.cs
ReportUsageAttribute.cs
StayPublicAttribute.cs
```

### Styx/TreeSharp/
```
Action.cs
ActionDelegate.cs
ActionSucceedDelegate.cs
CanRunDecoratorDelegate.cs
Composite.cs
ContextChangeHandler.cs
Decorator.cs
DecoratorContinue.cs
DynamicChildSelector.cs
GroupComposite.cs
PrioritySelector.cs
ProbabilitySelector.cs
RetrieveSwitchParameterDelegate.cs
RunStatus.cs
Selector.cs
Sequence.cs
Sleep.cs
Switch.cs
SwitchArgument.cs
Wait.cs
WaitContinue.cs
WaitGetTimeoutDelegate.cs
WaitGetTimeSpanTimeoutDelegate.cs
WhileLoop.cs
```

### Styx/WoWInternals/
```
AlteracValleyLandmark.cs
AlteracValleyLandmarkType.cs
ArathiBasinLandmark.cs
ArathiBasinLandmarkType.cs
AreaPoiLandmark.cs
ArenaType.cs
BattlefieldWinner.cs
BattleForGilneasLandmark.cs
BattleForGilneasLandmarkType.cs
BattlegroundJoinError.cs
Battlegrounds.cs
BattlegroundSide.cs
BattlegroundStatus.cs
BattlegroundType.cs
DeepwindGorgeLandmark.cs
DeepwindGorgeLandmarkType.cs
EyeOfTheStormLandmark.cs
EyeOfTheStormLandmarkType.cs
GameInput.cs
InputMouseButton.cs
IsleOfConquestLandmark.cs
IsleOfConquestLandmarkType.cs
LandmarkControlType.cs
LandmarkType.cs
Lua.cs
LuaEventArgs.cs
LuaEventHandlerDelegate.cs
LuaEvents.cs
LuaNode.cs
LuaRunStatus.cs
LuaState.cs
LuaTable.cs
LuaTKey.cs
LuaTString.cs
LuaTValue.cs
LuaType.cs
LuaValue.cs
NativeLuaCommonHeader.cs
NativeLuaNode.cs
NativeLuaTable.cs
NativeLuaTKey.cs
NativeLuaTString.cs
NativeLuaTValue.cs
NativeLuaValue.cs
ObjectListUpdateFinishedDelegate.cs
ObjectManager.cs
PetStance.cs
PlayerQuest.cs
Quest.cs
QuestLog.cs
ResearchSiteLandmark.cs
SpellCooldownInfo.cs
StrandOfTheAncientsLandmark.cs
StrandOfTheAncientsLandmarkType.cs
TaxiNodeType.cs
WoWApplyAuraType.cs
WoWAura.cs
WoWAuraCollection.cs
WoWBag.cs
WoWCamera.cs
WoWCurrency.cs
WoWCurrencyType.cs
WoWDb.cs
WoWDescriptorQuest.cs
WoWDescriptorQuestFlags.cs
WoWDispelType.cs
WoWGroupInfo.cs
WoWGuidType.cs
WoWLandMark.cs
WoWMissile.cs
WoWMovement.cs
WoWMovementInfo.cs
WoWPaperDoll.cs
WoWPetBattleState.cs
WoWPetControl.cs
WoWPetSpell.cs
WoWPlayerInventory.cs
WoWQuestCurrentStep.cs
WoWQuestPOIInfo.cs
WoWQuestState.cs
WoWQuestStep.cs
WoWQuestStepsCollection.cs
WoWSimpleMovementInfo.cs
WoWSkill.cs
WoWSpell.cs
WoWSpellEffectType.cs
WoWSpellFocus.cs
WoWSpellMechanic.cs
WoWSpellSchool.cs
WoWTotem.cs
WoWTotemExtensions.cs
WoWTotemInfo.cs
WoWTotemType.cs
```

#### Styx/WoWInternals/DB/
```
CurrencyType.cs
```

#### Styx/WoWInternals/DBC/
```
AreaPoi.cs
AreaTable.cs
CreatureFamily.cs
Faction.cs
FactionTemplate.cs
ItemRandomProperties.cs
ItemRandomSuffix.cs
LfgDungeons.cs
Map.cs
MapDifficulty.cs
MapType.cs
PetFoodFlags.cs
RecipeAquireMethod.cs
ResearchSite.cs
ScalingStatDistribution.cs
SkillLineAbility.cs
SkillLineCategory.cs
SkillLineInfo.cs
SpellEffect.cs
SpellItemEnchantment.cs
TaxiNodes.cs
```

#### Styx/WoWInternals/Misc/
```
NetStats.cs
Stable.cs
StabledPet.cs
WoWAuction.cs
WoWClient.cs
```

#### Styx/WoWInternals/UI/
```
AnchorPoint.cs
Backdrop.cs
BlendMode.cs
ButtonClickAction.cs
ButtonState.cs
FrameStrata.cs
Layer.cs
Orientation.cs
```

#### Styx/WoWInternals/World/
```
AreaTable.cs
GameWorld.cs
JbnMap.cs
JbnMapAreaTableEntry.cs
TraceLineHitFlags.cs
Triangle.cs
WorldLine.cs
WorldMap.cs
WorldMapAreaTableEntry.cs
WorldScene.cs
```

#### Styx/WoWInternals/WoWCache/
```
CacheDb.cs
WoWCache.cs
```

#### Styx/WoWInternals/WoWObjects/
```
BagType.cs
CorpseType.cs
DurabilityCostEntry.cs
DurabilityQualityEntry.cs
FactionStanding.cs
GameObjectInfo.cs
ILootableObject.cs
ItemInfo.cs
ItemStats.cs
LocalPlayer.cs
MirrorTimerInfo.cs
ObjectInvalidateDelegate.cs
RaidTargetMarker.cs
ReputationFlags.cs
RuneType.cs
SpecType.cs
UnitThreatInfo.cs
WoWAnimatedSubObject.cs
WoWAreaTrigger.cs
WoWArenaTeamInfo.cs
WoWChair.cs
WoWContainer.cs
WoWCorpse.cs
WoWDoor.cs
WoWDynamicObject.cs
WoWFishingBobber.cs
WoWGameObject.cs
WoWGlyphInfo.cs
WoWInebriationLevel.cs
WoWItem.cs
WoWLockType.cs
WoWObject.cs
WoWPartyMember.cs
WoWPlayer.cs
WoWPlayerCombatRating.cs
WoWSubObject.cs
WoWUnit.cs
```

### Styx/XmlEngine/
```
INamedAttribute.cs
XmlAttributeAttribute.cs
XmlElementAttribute.cs
XmlEngine.cs
```

---

## Bots/

### Bots/ArchaeologyBuddy/
```
ArchBuddy.cs
ArchSettings.cs
Digsite.cs
Fragment.cs
```

#### Bots/ArchaeologyBuddy/GUI/
```
ArchBuddySettings.cs
ArchBuddySettings.Designer.cs
ArchBuddySettings.resources
```

### Bots/BGBuddy/
```
Battleground.cs
BattlegroundSide.cs
BgBotProfile.cs
BGBuddy.cs
BGBuddySettings.cs
HeatmapWindow.cs
HeatmapWindow.Designer.cs
HeatmapWindow.resources
LogicType.cs
MapBox.cs
RaidHelper.cs
WorldStatesUpdateDelegate.cs
```

#### Bots/BGBuddy/Forms/
```
ConfigWindow.cs
ConfigWindow.Designer.cs
ConfigWindow.resources
```

#### Bots/BGBuddy/HeatMap/
```
Heatmap.cs
```

#### Bots/BGBuddy/Helpers/
```
Logger.cs
```

#### Bots/BGBuddy/Logic/Battlegrounds/
```
LandmarkInfo.cs
```

#### Bots/BGBuddy/Resources/
```
BGBuddyResources.cs
BGBuddyResources.resources
```

### Bots/DungeonBuddy/
```
BossManager.cs
Dungeon.cs
DungeonBot.cs
DungeonManager.cs
DynamicBlackspot.cs
DynamicBlackspotManager.cs
GroupMember.cs
LfgDungeons.cs
```

#### Bots/DungeonBuddy/Attributes/
```
CallBehaviorMode.cs
DynamicStringListAttribute.cs
EncounterHandlerAttribute.cs
LocationHandlerAttribute.cs
ObjectHandlerAttribute.cs
ScenarioStageAttribute.cs
```

#### Bots/DungeonBuddy/Avoidance/
```
Avoid.cs
AvoidanceManager.cs
AvoidCluster.cs
AvoidInfo.cs
AvoidLocation.cs
AvoidLocationInfo.cs
AvoidObject.cs
AvoidObjectInfo.cs
AvoidPathNotFoundException.cs
AvoidPathResult.cs
AvoidSide.cs
AvoidTracelineResult.cs
ClusterHit.cs
Helpers.cs
LineCircleTangentPoints.cs
LineClusterTangentPoints.cs
PathResult.cs
```

#### Bots/DungeonBuddy/Behaviors/
```
ActionLogger.cs
```

#### Bots/DungeonBuddy/Enums/
```
BossAvailableToFaction.cs
CompleteReason.cs
DungeonType.cs
LfgInvalidError.cs
LootMode.cs
PartyMode.cs
RaidType.cs
ScenarioType.cs
```

#### Bots/DungeonBuddy/Forms/
```
FormConfig.cs
FormConfig.Designer.cs
FormConfig.resources
PathView.cs
PathView.Designer.cs
PathView.resources
```

#### Bots/DungeonBuddy/Helpers/
```
Action.cs
Alert.cs
AlwaysFailAction.cs
Decorator.cs
DecoratorContinue.cs
DungeonArea.cs
DungeonBuddySettings.cs
DynamicStringListConverter.cs
Error.cs
ErrorCollection.cs
ErrorType.cs
Logger.cs
ScenarioCriteria.cs
ScenarioInfo.cs
ScenarioStage.cs
ScriptHelpers.cs
SpellActionButton.cs
Wait.cs
WaitContinue.cs
```

#### Bots/DungeonBuddy/Profiles/
```
ElementAttributeAttribute.cs
IXmlAutoProcessed.cs
ObsoleteProfileElementAttribute.cs
Profile.cs
ProfileElementAttribute.cs
ProfileManager.cs
ValueRangeAttribute.cs
```

##### Bots/DungeonBuddy/Profiles/Handlers/
```
Blackspot.cs
Boss.cs
Hotspot.cs
PullBlackspot.cs
```

### Bots/Gatherbuddy/
```
BagHelper.cs
GatherbuddyBot.cs
GatherbuddySettings.cs
PathType.cs
Profile.cs
```

#### Bots/Gatherbuddy/GUI/
```
GbConfig.cs
GbConfig.Designer.cs
GbConfig.resources
```

### Bots/Grind/
```
BehaviorFlags.cs
BehaviorFlagsExtensions.cs
LevelBot.cs
```

### Bots/Professionbuddy/
```
BankType.cs
DataStore.cs
DepositWithdrawAmount.cs
GlobalPBSettings.cs
Help.rtf
IDeepCopy.cs
Ingredient.cs
IngredientSubClass.cs
ItemSelectionType.cs
MainForm.cs
MainForm.Designer.cs
MainForm.resources
PBBranch.cs
PBLog.cs
PbProfile.cs
PbProfileSettingEntry.cs
PbProfileSettings.cs
PBXmlAttributeAttribute.cs
PBXmlElementAttribute.cs
ProfessionbuddyBot.cs
ProfessionBuddySettings.cs
Recipe.cs
RecipeDifficulty.cs
SubCategoryType.cs
Tool.cs
TradeSkill.cs
TradeSkillListView.cs
Util.cs
```

#### Bots/Professionbuddy/BehaviorTree/
```
Action.cs
Component.cs
Composite.cs
Decorator.cs
DecoratorContinue.cs
PrioritySelector.cs
Sequence.cs
Wait.2.cs
Wait.cs
WaitContinue.2.cs
WaitContinue.cs
```

#### Bots/Professionbuddy/ComponentBase/
```
DynamicallyCompiledCodeAction.cs
DynamicallyCompiledCodeComposite.cs
FlowControlComposite.cs
IPBComponent.cs
PBAction.cs
PBComposite.cs
```

#### Bots/Professionbuddy/Components/
```
AttachToTreeHookAction.cs
BuyItemAction.cs
BuyItemFromAhAction.cs
CallSubRoutineAction.cs
CancelAuctionAction.cs
CastSpellAction.cs
ChangeBotAction.cs
CommentAction.cs
CustomAction.cs
DefineAction.cs
DisenchantAction.cs
FlyToAction.cs
GetItemfromBankAction.cs
GetMailAction.cs
IfComposite.cs
InteractionAction.cs
LoadProfileAction.cs
LoadProfileType.cs
MailItemAction.cs
MoveToAction.cs
PutItemInBankAction.cs
SellItemAction.cs
SellItemOnAhAction.cs
SettingsAction.cs
StackItemsAction.cs
SubRoutineComposite.cs
TrainSkillAction.cs
WaitAction.cs
WhileComposite.cs
```

#### Bots/Professionbuddy/Dynamic/
```
CsharpCodeType.cs
DynamicCodeCompiler.cs
DynamicProperty.cs
HBRelogApi.cs
Helpers.cs
IDynamicallyCompiledCode.cs
IDynamicProperty.cs
ProfileStatus.cs
```

#### Bots/Professionbuddy/Icons/
```
save.png
```

#### Bots/Professionbuddy/Localization/
```
Strings.cs
Strings.resources
```

#### Bots/Professionbuddy/Properties/
```
Settings.Designer.cs
Settings.settings
```

#### Bots/Professionbuddy/PropertyGridUtilities/
```
MetaProp.cs
MetaPropArgs.cs
PropertyBag.cs
```

##### Bots/Professionbuddy/PropertyGridUtilities/Converters/
```
GoldEditorConverter.cs
```

##### Bots/Professionbuddy/PropertyGridUtilities/Editors/
```
EntryEditor.cs
FileLocationEditor.cs
GoldEditor.cs
LocationEditor.cs
```

### Bots/Quest/
```
QuestBot.cs
QuestDebug.cs
QuestManager.cs
QuestState.cs
```

#### Bots/Quest/Actions/
```
ActionSelectActiveQuest.cs
ActionSelectAvailableQuest.cs
ActionSelectReward.cs
ForcedBehaviorExecutor.cs
```

##### Bots/Quest/Actions/Combat/
```
ActionMoveToTarget.cs
ActionPull.cs
ActionSetTarget.cs
```

#### Bots/Quest/Decorators/Combat/
```
DecoratorNeedToFindTarget.cs
```

#### Bots/Quest/Objectives/
```
CollectItemObjective.cs
DropDatabase.cs
GrindObjective.cs
QuestObjective.cs
UseGameObjectObjective.cs
```

#### Bots/Quest/QuestOrder/
```
ForcedBehavior.cs
ForcedCodeBehavior.cs
ForcedGrindTo.cs
ForcedIf.cs
ForcedMoveTo.cs
ForcedNothing.cs
ForcedQuestObjective.cs
ForcedQuestPickUp.cs
ForcedQuestTurnIn.cs
ForcedSingleton.cs
ForcedUseItem.cs
ForcedWhile.cs
QuestOrder.cs
```

#### Bots/Quest/Resources/
```
QuestBotResources.cs
QuestBotResources.resources
```

---

## CommonBehaviors/

```
ActionWaitForLuaEvent.cs
WaitLuaEvent.cs
```

### CommonBehaviors/Actions/
```
ActionAlwaysFail.cs
ActionAlwaysSucceed.cs
ActionClearPoi.cs
ActionDebugString.cs
ActionIdle.cs
ActionMoveToPoi.cs
ActionRunCoroutine.cs
ActionSetActivity.cs
ActionSetPoi.cs
DebugStringDelegate.cs
GetPointDelegate.cs
NavigationAction.cs
NavigationInfo.cs
NavTypeDelegate.cs
RetrieveBotPoiDelegate.cs
SleepForLagDuration.cs
```

### CommonBehaviors/Decorators/
```
DecoratorContextIs.cs
DecoratorFrameIsVisible.cs
DecoratorIsNotPoiType.cs
DecoratorIsPoiType.cs
```

### CommonBehaviors/Resources/
```
CommonBehaviorsResources.cs
CommonBehaviorsResources.resources
```

---

## Buddy/ (Auth & Store)

### Buddy/Auth/
```
UseServiceDelegate.cs
```

#### Buddy/Auth/Math/
```
Vector3.cs
```

#### Buddy/Auth/Objects/
```
StoreProduct.cs
UsageInfo.cs
WoWMailbox.cs
WoWNpc.cs
```

#### Buddy/Auth/SR/
```
d0.cs
IA.cs
r0.cs
```

### Buddy/Coroutines/
```
Coroutine.cs
CoroutineBehaviorException.cs
CoroutineException.cs
CoroutineStatus.cs
CoroutineStoppedException.cs
CoroutineUnhandledException.cs
ExternalTaskWaitResult.cs
```

### Buddy/Store/
```
ProductType.cs
```

#### Buddy/Store/Wpf/
```
StoreBrowser.xaml
StoreBrowser.xaml.cs
StoreProfileBrowserWindow.xaml
StoreProfileBrowserWindow.xaml.cs
```

---

## Tripper/ (Navigation)

### Tripper/LZMACompression/
```
Lzma.cs
```

### Tripper/MeshMisc/
```
AbilityFlags.cs
AreaType.cs
GraphicalHelper.cs
InvalidTileDataException.cs
IoCGate.cs
MapConsts.cs
MeshManager.cs
SotAGate.cs
TileDataHeader.cs
TileDataVersionException.cs
TileIdentifier.cs
```

### Tripper/Navigation/
```
IMeshProvider.cs
MapLoadedEventArgs.cs
NavHelper.cs
NavigatorLogMessage.cs
PathFindProgressEventArgs.cs
PathFindResult.cs
PathFindStep.cs
TileLoadedEventArgs.cs
WowNavigator.cs
WowQueryFilter.cs
```

---

## Levelbot/

```
FormLevelbotSettings.cs
FormLevelbotSettings.Designer.cs
FormLevelbotSettings.resources
```

### Levelbot/Actions/Combat/
```
ActionMoveToKillPoi.cs
ActionMoveToTarget.cs
ActionPull.cs
ActionSetTarget.cs
```

### Levelbot/Actions/Death/
```
ActionReleaseFromCorpse.cs
ActionSuceedIfDeadOrGhost.cs
```

### Levelbot/Actions/General/
```
ActionMountVendor.cs
ActionSelectReward.cs
```

### Levelbot/Decorators/Combat/
```
DecoratorNeedToFindTarget.cs
```

### Levelbot/Decorators/Death/
```
DecoratorInstanceRelease.cs
DecoratorNeedToMoveToCorpse.cs
DecoratorNeedToRelease.cs
DecoratorNeedToTakeCorpse.cs
```

### Levelbot/ProfileCreation/Forms/
```
FormFindVendors.cs
FormFindVendors.Designer.cs
FormFindVendors.resources
FormProfileCreator.cs
FormProfileCreator.Designer.cs
FormProfileCreator.resources
FormSelectProfileName.cs
FormSelectProfileName.Designer.cs
FormSelectProfileName.resources
```

### Levelbot/Resources/
```
LevelbotResources.cs
LevelbotResources.resources
```

---

## Other Root Directories

### BuddyMonitor/Common/
```
LogLevel.cs
```

### Debug/
```
BindingsDebugWindow.xaml
BindingsDebugWindow.xaml.cs
ReflectPropertyDescriptorInfo.cs
```

### GrindBuddy2/
```
GrindBuddy.cs
```

### Infralution/Localization/Wpf/
```
CultureManager.cs
CultureSelectWindow.xaml
CultureSelectWindow.xaml.cs
GetResourceHandler.cs
ManagedMarkupExtension.cs
MarkupExtensionManager.cs
ResourceEnumConverter.cs
ResxExtension.cs
UICultureExtension.cs
```

### JetBrains/Annotations/
```
AspMvcActionAttribute.cs
AspMvcActionSelectorAttribute.cs
AspMvcAreaAttribute.cs
AspMvcAreaMasterLocationFormatAttribute.cs
AspMvcAreaPartialViewLocationFormatAttribute.cs
AspMvcAreaViewLocationFormatAttribute.cs
AspMvcControllerAttribute.cs
AspMvcDisplayTemplateAttribute.cs
AspMvcEditorTemplateAttribute.cs
AspMvcMasterAttribute.cs
AspMvcMasterLocationFormatAttribute.cs
AspMvcModelTypeAttribute.cs
AspMvcPartialViewAttribute.cs
AspMvcPartialViewLocationFormatAttribute.cs
AspMvcSupressViewErrorAttribute.cs
AspMvcTemplateAttribute.cs
AspMvcViewAttribute.cs
AspMvcViewLocationFormatAttribute.cs
BaseTypeRequiredAttribute.cs
CanBeNullAttribute.cs
CannotApplyEqualityOperatorAttribute.cs
ContractAnnotationAttribute.cs
HtmlAttributeValueAttribute.cs
HtmlElementAttributesAttribute.cs
ImplicitUseKindFlags.cs
ImplicitUseTargetFlags.cs
InstantHandleAttribute.cs
InvokerParameterNameAttribute.cs
LocalizationRequiredAttribute.cs
MeansImplicitUseAttribute.cs
NotifyPropertyChangedInvocatorAttribute.cs
NotNullAttribute.cs
PathReferenceAttribute.cs
PublicAPIAttribute.cs
PureAttribute.cs
RazorSectionAttribute.cs
StringFormatMethodAttribute.cs
UsedImplicitlyAttribute.cs
```

### NewMixedMode/
```
FormChooser.cs
FormChooser.Designer.cs
FormChooser.resources
MixedModeEx.cs
MixedModeSettings.cs
```

#### NewMixedMode/Resources/
```
NewMixedModeResources.cs
NewMixedModeResources.resources
```

### PartyBot/
```
DiscoBot.cs
PartyBotSettings.cs
```

#### PartyBot/Forms/
```
FormConfig.cs
FormConfig.Designer.cs
FormConfig.resources
```

### Plugins/BuddyMonitor/
```
BuddyMonitorSettings.cs
```

### Properties/
```
AssemblyInfo.cs
Settings.Designer.cs
Settings.settings
```

### SevenZip/
```
CoderPropID.cs
ICodeProgress.cs
ICoder.cs
ISetCoderProperties.cs
ISetDecoderProperties.cs
IWriteCoderProperties.cs
```

#### SevenZip/Buffer/
```
InBuffer.cs
OutBuffer.cs
```

#### SevenZip/CommandLineParser/
```
CommandForm.cs
Parser.cs
SwitchForm.cs
SwitchResult.cs
SwitchType.cs
```

#### SevenZip/Compression/LZ/
```
BinTree.cs
IInWindowStream.cs
IMatchFinder.cs
InWindow.cs
OutWindow.cs
```

#### SevenZip/Compression/LZMA/
```
Decoder.cs
Encoder.cs
```

### Tripwire/Client/
```
TripwireClient.cs
```

### images/ (non-code assets)
```
bee2.png, bee2_christmas.png, boss.png, copy.png, hbwasp.ico,
hbwasp_christmas.ico, image1.png, key.png, lock.png, refresh.png,
store.ico, wasp.gif
```

### themes/
```
expressiondark.xaml
```

---

## Obfuscated Namespace Folders (ns0–ns51)

All 52 folders contain decompiler-generated Class/Attribute/Struct/Enum/Interface/Exception/Stream files. These are internal implementation details hidden by SmartAssembly obfuscation.

| Folder | Files |
|--------|-------|
| **ns0** | Class0, Class39, Class435, Class528, Class835 |
| **ns1** | Attribute42, Class1, Class131, Class20, Class413, Class524, Exception0, FormRoadMapper (+Designer), Struct289 |
| **ns2** | Attribute39, Class121, Class144, Class374, Class388, Class439, Class911, Enum0, MonitorSettings (+Designer+resources), Struct316 |
| **ns3** | Class3, Class36, Class682, Class684, Class75, Class823, Class86 |
| **ns4** | Attribute4, Attribute44, Class260, Class35, Class4, Class406, Class709, Enum5, Interface12, Stream3, Struct206, Struct209, Struct254 |
| **ns5** | Attribute9, Class115, Class189, Class26, Class28, Class5, Class564, Class668, Class672, Interface5 |
| **ns6** | Attribute12, Class119, Class127, Class233, Class433, Class518, Class6 (+resources), Class669, Enum22, Exception9, Interface1, Interface9, Struct317 |
| **ns7** | Class105, Class588, Class7, Class711, Class790, Class812, Class873, Struct354 |
| **ns8** | Attribute37, Class19, Class389, Class423, Class443, Class636, Class639, Class648, Interface6, Struct255 |
| **ns9** | Attribute36, Class21, Class93, Interface3 |
| **ns10** | Attribute17, Attribute31, Attribute32, Class116, Class22, Class383, Class545, Class807, Class822, Stream1, Struct210 |
| **ns11** | Class107, Class155, Class158, Class23, Class37, Class396, Class526, Class531, Class570, Class808, Class916, Enum35, Struct205 |
| **ns12** | Attribute35, Attribute6, Class24, Class426, Class434, Class76, Class802, Class834, Struct252 |
| **ns13** | Attribute19, Attribute38, Class25, Class412, Class469, Class532, Class644, Class675, Class799, Class801, Class810, Class96 |
| **ns14** | Class120, Class140, Class27, Class32, Class364, Class395, Class764 |
| **ns15** | Attribute1, Class29, Class381, Class390, Class424, Class425, Class565, Class872, Struct207, Struct355 |
| **ns16** | Attribute16, Attribute28, Class30, Class400, Class401 |
| **ns17** | Attribute13, Attribute5, Class31, Class474 |
| **ns18** | Attribute30, Class134, Class142, Class33, Class41, Class417, Class523, Class530, Class91 |
| **ns19** | Class34, Class398, Class438 (+resources), Class95, Enum8, EventArgs1, Interface0, Interface4 |
| **ns20** | Class101, Class128, Class141, Class38, Class637, Class702, Class804, Class89 |
| **ns21** | Class40, Class470, Class674, Class90, Class97, Interface2, Struct14 |
| **ns22** | Attribute23, Attribute7, Class42, Class805, Class809, Class903, Enum10, Enum7, Exception2 |
| **ns23** | Class384, Class407, Class408, Class43, Class806, Class811, Enum20, Exception8, RoutineSelectionForm (+Designer+resources) |
| **ns24** | Attribute10, Attribute2, Attribute33, Class420, Class66, Class74, Class800, Class894, Struct291, Struct297 |
| **ns25** | Class393, Class414, Class641, Class770, Class803, Class87, Enum2, EventArgs0 |
| **ns26** | Attribute22, Attribute3, Attribute41, Class399, Class458 (+resources), Class643, Class710, Class773, Class902, Class92, Interface7 |
| **ns27** | Attribute47, Class394, Class94, Class99, Enum23, Interface8, Stream6, Struct318 |
| **ns28** | Class123, Class820, Class899, Enum1, Interface11 |
| **ns29** | Class100, Class103, Class415, Class529, Class798, Class814, Exception4, Struct13 |
| **ns30** | Attribute20, Attribute43, Class404, Class405, Class419, Class671, Class901, Exception1 |
| **ns31** | Attribute18, Attribute45, Class132, Class402, Class418, Class630, Class633, Class720, Class846, Enum3, Exception3, Struct290 |
| **ns32** | Class104, Class772, Class850, Enum19, Struct15 |
| **ns33** | Attribute15, Class106, Class130, Class133, Exception7, Stream7 |
| **ns34** | Class110, Class129, Class411, Class436, Class520, Class521, Class563, Class571, Class813, Class844, Class851, Class910 |
| **ns35** | Class117, Class385, Class427, Class601, Class667, Class683, Class885, Enum15, Enum6, Stream0, Struct11 |
| **ns36** | Attribute29, Class118, Class122, Class403, Class543, Class613, Class746, Interface13, Struct144 |
| **ns37** | Attribute24, Attribute40, Class136, Class259, Class510, Class832, Struct12 |
| **ns38** | Attribute21, Class124, Class126, Class466, Class766, Struct286 |
| **ns39** | Class135, Class382, Class416, Class509, Class821, Class833, Struct319 |
| **ns40** | Attribute8, Class152, Class349, Class386, Class442 (+resources), Class670, Enum17, Enum18, Stream5 |
| **ns41** | Attribute11, Class666, Stream2, Struct148, Struct250 |
| **ns42** | Attribute14, Attribute34, Class392, Class631, Struct208 |
| **ns43** | Attribute25, Class421, Class612, Class769, Class817, Interface14 |
| **ns44** | Attribute26, Class827 |
| **ns45** | Attribute27, Class150, Class422, Class797, Exception6 |
| **ns46** | Class151, Class190, Interface10, Struct253 |
| **ns47** | Attribute46, Class522, Interface15, Struct288, Struct36 |
| **ns48** | Class391, Class562, Class673, Enum21, Enum9, Struct215, Struct356 |
| **ns49** | Class354, Class397, Class409, Class410, Class525, Class774, Class824, Exception10 |
| **ns50** | Class380, Class387, Class789, Enum31, Exception11, Struct357 |
| **ns51** | Class527, Class665, Struct251 |

---

## Summary Statistics

| Category | Count |
|----------|-------|
| Root directories | 22 (+ 52 ns* folders) |
| Styx/ subdirectories (recursive) | ~30 |
| Bots/ subdirectories (recursive) | ~35 |
| Obfuscated ns* folders | 52 (ns0–ns51) |
| Estimated total .cs files | ~900+ |
