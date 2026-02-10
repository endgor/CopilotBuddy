# HB 4.3.4 Decompiled Honorbuddy — Complete Directory Map

**Root:** `c:\Users\Texy\Desktop\.Reference\.hb 4.3.4\Honorbuddy\Honorbuddy\`

---

## Root-Level Files

```
app.manifest
App.xaml
App.xaml.cs
AutoEquip.FormWeightSelector.resources
d0.cs
DevToolsWindow.xaml
DevToolsWindow.xaml.cs
Honorbuddy.csproj
Honorbuddy.ico
IA.cs
InvalidProcessException.cs
LoginWindow.xaml
LoginWindow.xaml.cs
MainWindow.xaml
MainWindow.xaml.cs
PluginsWindow.xaml
PluginsWindow.xaml.cs
ProcessSelectorWindow.xaml
ProcessSelectorWindow.xaml.cs
r0.cs
SettingsWindow.xaml
SettingsWindow.xaml.cs
SettingsWrap.cs
UIHelpers.cs
```

**.cs files at root:** `d0.cs`, `IA.cs`, `InvalidProcessException.cs`, `r0.cs`, `SettingsWrap.cs`, `UIHelpers.cs`, `App.xaml.cs`, `DevToolsWindow.xaml.cs`, `LoginWindow.xaml.cs`, `MainWindow.xaml.cs`, `PluginsWindow.xaml.cs`, `ProcessSelectorWindow.xaml.cs`, `SettingsWindow.xaml.cs`

---

## Styx/ (Core Bot Engine)

### Styx/ (root files)
```
BotBase.cs
BotEvents.cs
BotManager.cs
CantCompileException.cs
EmoteState.cs
FactionId.cs
FrameLock.cs
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
MirrorTimerType.cs
PulseFlags.cs
PvPState.cs
QuestGiverStatus.cs
ShapeshiftForm.cs
SheathType.cs
SkillLine.cs
StatTypes.cs
StyxWoW.cs
ThreatStatus.cs
UnitNPCFlags.cs
UserException.cs
WoWBagSlot.cs
WoWCreatureSkinType.cs
WoWCreatureType.cs
WoWCursorType.cs
WoWEquipSlot.cs
WoWGameObjectState.cs
WoWGameObjectType.cs
WoWGender.cs
WoWInteractType.cs
WoWInventorySlot.cs
WoWItemAmmoType.cs
WoWItemArmorClass.cs
WoWItemBagFamily.cs
WoWItemBondType.cs
WoWItemClass.cs
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
WoWPowerType.cs
WoWPulsator.cs
WoWQuestType.cs
WoWRace.cs
WoWSocketColor.cs
WoWStateFlag.cs
WoWUnitClassificationType.cs
WoWUnitReaction.cs
```

### Styx/Auth/
```
UseServiceDelegate.cs
```

### Styx/Bot/
```
  Plugins/
    AutoEquip2/
      FormSettings.resources        (no .cs files)
```

### Styx/Combat/
```
  CombatRoutine/
    CombatRoutine.cs
    IBehaviors.cs
    ICombatRoutine.cs
    WoWClass.cs
```

### Styx/Database/
```
Connection.cs
NpcQueries.cs
NpcResult.cs
```

### Styx/Helpers/
```
ActivitySetter.cs
AllocatedMemory.cs
Arguments.cs
BGBotSettings.cs
BinaryExtensions.cs
CapacityQueue.cs
CharacterSettings.cs
CircularQueue.cs
ClassCollection.cs
CombatAssistSettings.cs
CommandLine.cs
DefaultValueAttribute.cs
DictionaryExtensions.cs
DualHashSet.cs
EncryptedAttribute.cs
EnumTypeConverter.cs
Extensions.cs
FieldDisplayNameAttribute.cs
FlagCheckedListBox.cs
FlagCheckedListBoxItem.cs
FlagEnumUIEditor.cs
GameDebugAddStringDelegate.cs
GatherbuddyHelper.cs
InactivityDetector.cs
IndexedList.cs
InfoPanel.cs
IRangeAble.cs
KeyboardManager.cs
KeyHelpers.cs
LevelbotSettings.cs
Logging.cs
LogType.cs
PerformanceTimer.cs
PVPSettings.cs
Range.cs
RangedDictionary.cs
SettingAttribute.cs
Settings.cs
StyxSettings.cs
UISettings.cs
Utilities.cs
ValuePair.cs
WaitTimer.cs
WaitTimerFinishedHandler.cs
WoWMathHelper.cs
XmlExtensions.cs
XmlUtils.cs
```

### Styx/Loaders/
```
DllLoader.cs
DynamicLoader.cs
FrameworkVersionDetection.cs
```

### Styx/Patchables/
```
ClientDb.cs
```

### Styx/Plugins/
```
CompilerErrorsException.cs
PluginContainer.cs
PluginManager.cs
PluginWrapper.cs
  PluginClass/
    HBPlugin.cs
    IHBPlugin.cs
```

### Styx/Resources/
```
StyxResources.cs
StyxResources.resources
```

---

### Styx/WoWInternals/ (root files)
```
Lua.cs
LuaEventArgs.cs
LuaEventHandlerDelegate.cs
LuaEvents.cs
LuaEventWait.cs
LuaNode.cs
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
TaxiNodeType.cs
WoWBag.cs
WoWCamera.cs
WoWChat.cs
WoWCurrency.cs
WoWCurrencyType.cs
WoWDb.cs
WoWFaction.cs
WoWFactionTemplate.cs
WoWMovement.cs
WoWPaperDoll.cs
WoWPlayerInventory.cs
```

### Styx/WoWInternals/DBC/
```
AreaTable.cs
LfgDungeonExpansion.cs
LfgDungeons.cs
Map.cs
MapDifficulty.cs
MapType.cs
```

### Styx/WoWInternals/Misc/
```
AuctionFrame.cs
AuctionHouse.cs
AuctionListType.cs
AuctionPostTime.cs
NetStats.cs
Stable.cs
StabledPet.cs
WoWAuction.cs
WoWClient.cs
  DBC/
    CreatureFamily.cs
    PetFoodFlags.cs
```

### Styx/WoWInternals/World/
```
GameWorld.cs
WorldLine.cs
```

### Styx/WoWInternals/WoWCache/
```
CacheDb.cs
WoWCache.cs
```

### Styx/WoWInternals/WoWObjects/
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
ReputationFlags.cs
RuneType.cs
SpecType.cs
UnitThreatInfo.cs
WoWAnimatedSubObject.cs
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
WoWMovementInfo.cs
WoWObject.cs
WoWPartyMember.cs
WoWPlayer.cs
WoWSubObject.cs
WoWTotem.cs
WoWTotemExtensions.cs
WoWTotemInfo.cs
WoWTotemType.cs
WoWUnit.cs
```

---

### Styx/Logic/ (root files)
```
AlteracValleyLandmark.cs
AlteracValleyLandmarkType.cs
ArathiBasinLandmark.cs
ArathiBasinLandmarkType.cs
ArenaType.cs
BattlefieldWinner.cs
BattleForGilneasLandmark.cs
BattleForGilneasLandmarkType.cs
BattlegroundJoinError.cs
Battlegrounds.cs
BattlegroundSide.cs
BattlegroundStatus.cs
BattlegroundType.cs
Blacklist.cs
BuyItemsEventArgs.cs
BuyItemsEventHandler.cs
CanMountDelegate.cs
EyeOfTheStormLandmark.cs
EyeOfTheStormLandmarkType.cs
FlightPathReason.cs
FlightPaths.cs
IncludeTargetsFilterDelegate.cs
IsleOfConquestLandmark.cs
IsleOfConquestLandmarkType.cs
LandmarkControlType.cs
Landmarks.cs
LandmarkType.cs
LocationRetriever.cs
LootTargeting.cs
MailItemsEventArgs.cs
MailItemsEventHandler.cs
Mount.cs
MountHelper.cs
MountType.cs
MountUpEventArgs.cs
QueuedBattlegroundInfo.cs
RaFHelper.cs
RemoveTargetsFilterDelegate.cs
ResearchSiteLandmark.cs
SellItemsEventArgs.cs
StrandOfTheAncientsLandmark.cs
StrandOfTheAncientsLandmarkType.cs
Targeting.cs
TargetListUpdateFinishedDelegate.cs
VendorItemsEventHandler.cs
Vendors.cs
WeighTargetsDelegate.cs
WoWLandMark.cs
WoWSkill.cs
XmlFlightNode.cs
```

### Styx/Logic/AreaManagement/
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
  Triangulation/
    Edge.cs
    Triangle.cs
```

### Styx/Logic/BehaviorTree/
```
StatusTextChangedEventArgs.cs
TreeRoot.cs
```

### Styx/Logic/Combat/
```
Healing.cs
LegacySpellManager.cs
RoutineManager.cs
SpellCollection.cs
SpellCooldownInfo.cs
SpellEffect.cs
SpellManager.cs
WoWApplyAuraType.cs
WoWAura.cs
WoWAuraCollection.cs
WoWDispelType.cs
WoWPetSpell.cs
WoWSpell.cs
WoWSpellEffectType.cs
WoWSpellFocus.cs
WoWSpellMechanic.cs
WoWSpellSchool.cs
```

### Styx/Logic/Common/
```
Rest.cs
```

### Styx/Logic/Inventory/
```
Consumable.cs
InventoryManager.cs
InventorySlot.cs
LootRoll.cs
Stat.cs
WeightSet.cs
WeightSetEx.cs
WoWPrice.cs
  Frames/
    Frame.cs
    Gossip/
      GossipEntry.cs
      GossipFrame.cs
      GossipQuestEntry.cs
    LootFrame/
      LootFrame.cs
      LootRarity.cs
      LootSlotInfo.cs
    MailBox/
      MailFrame.cs
    Merchant/
      ItemQuality.cs
      MerchantFrame.cs
      MerchantItem.cs
    Quest/
      QuestFrame.cs
    Taxi/
      TaxiFrame.cs
    Trainer/
      TrainerFrame.cs
      TrainerServiceFilter.cs
```

### Styx/Logic/Pathing/
```
AvoidanceManager.cs
BlackspotManager.cs
ClickToMoveMover.cs
Flightor.cs
INavigationProvider.cs
IPlayerMover.cs
IStuckHandler.cs
ITerrainHeightProvider.cs
MeshMovePath.cs
MeshNavigator.cs
MoveResult.cs
NavigationProviderChangedEventArgs.cs
Navigator.cs
PathGenerationFailStep.cs
WoWPoint.cs
  OnDemandDownloading/
    InvalidFileFormatException.cs
```

### Styx/Logic/POI/
```
BotPoi.cs
PoiType.cs
PoiTypeExtensions.cs
```

### Styx/Logic/Profiles/
```
Blackspot.cs
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
ProfileTagExpectedException.cs
ProfileUnknownAttributeException.cs
ProfileUnknownElementException.cs
ProtectedItemsManager.cs
UnknownProfileElementEventArgs.cs
Vendor.cs
VendorManager.cs
VendorTypeExtensions.cs
  Quest/
    AbandonQuestNode.cs
    CheckpointNode.cs
    ClearGrindAreaNode.cs
    ClearMailboxNode.cs
    ClearVendorNode.cs
    CodeNode.cs
    CollectFrom.cs
    CollectFromCollection.cs
    CollectFromType.cs
    CollectItemObjectiveInfo.cs
    ConditionHelper.cs
    DisableRepairNode.cs
    Else.cs
    ElseIf.cs
    EnableRepairNode.cs
    GrindToNode.cs
    IfNode.cs
    INodeContainer.cs
    KillMobObjectiveInfo.cs
    MoveToNode.cs
    ObjectiveInfo.cs
    ObjectiveNode.cs
    ObjectiveType.cs
    OrderNode.cs
    OrderNodeCollection.cs
    OrderNodeType.cs
    PickUpNode.cs
    ProfileHelperFunctionsBase.cs
    QuestBehaviorHelper.cs
    QuestInfo.cs
    QuestObjectType.cs
    SetGrindAreaNode.cs
    SetMailboxNode.cs
    SetVendorNode.cs
    TurnInNode.cs
    TurnInObjectiveInfo.cs
    UseItemNode.cs
    UseObjectObjectiveInfo.cs
    WhileNode.cs
```

### Styx/Logic/Questing/
```
CustomForcedBehavior.cs
PlayerQuest.cs
Quest.cs
QuestLog.cs
QuestLogEntry.cs
WoWDescriptorQuest.cs
WoWDescriptorQuestFlags.cs
WoWQuestCompletionInfo.cs
WoWQuestCurrentStep.cs
WoWQuestState.cs
WoWQuestStep.cs
WoWQuestStepsCollection.cs
```

---

## TreeSharp/
```
Action.cs
ActionDelegate.cs
ActionSucceedDelegate.cs
CanRunDecoratorDelegate.cs
Composite.cs
ContextChangeHandler.cs
Decorator.cs
DecoratorContinue.cs
GroupComposite.cs
PrioritySelector.cs
RetrieveSwitchParameterDelegate.cs
RunStatus.cs
Selector.cs
Sequence.cs
Switch.cs
SwitchArgument.cs
Wait.cs
WaitContinue.cs
WaitGetTimeoutDelegate.cs
```

---

## Tripper/

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

## CommonBehaviors/

### CommonBehaviors/ (root)
```
ActionWaitForLuaEvent.cs
PoiSelector.cs
PoiSequence.cs
WaitLuaEvent.cs
```

### CommonBehaviors/Actions/
```
ActionAlwaysFail.cs
ActionAlwaysSucceed.cs
ActionClearPoi.cs
ActionDebugString.cs
ActionIdle.cs
ActionInteract.cs
ActionMoveStop.cs
ActionMoveToPoi.cs
ActionMoveToPoint.cs
ActionSetActivity.cs
ActionSetPoi.cs
ActionSleep.cs
DebugStringDelegate.cs
GetPointDelegate.cs
NavigationAction.cs
RetrieveBotPoiDelegate.cs
SequenceOpenGossip.cs
```

### CommonBehaviors/Decorators/
```
DecoratorContextIs.cs
DecoratorFrameIsVisible.cs
DecoratorIsNotPoiType.cs
DecoratorIsPoiType.cs
DecoratorNeedToMoveToPoint.cs
```

### CommonBehaviors/Resources/
```
CommonBehaviorsResources.cs
CommonBehaviorsResources.resources
```

---

## Bots/

### Bots/ArchaeologyBuddy/
```
ArchBuddy.cs
ArchSettings.cs
Digsite.cs
Fragment.cs
  GUI/
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
LogicType.cs
MapBox.cs
RaidHelper.cs
WorldStatesUpdateDelegate.cs
  Forms/
    ConfigWindow.cs
    ConfigWindow.Designer.cs
    ConfigWindow.resources
  Helpers/
    Logger.cs
  Logic/
    Battlegrounds/
      LandmarkInfo.cs
  Resources/
    BGBuddyResources.cs
    BGBuddyResources.resources
```

### Bots/DungeonBuddy/
```
BossManager.cs
Dungeon.cs
DungeonBot.cs
DungeonManager.cs
  Actions/
    ActionLogger.cs
  Attributes/
    CallBehaviorMode.cs
    DynamicStringListAttribute.cs
    EncounterHandlerAttribute.cs
    ObjectHandlerAttribute.cs
  Avoidance/
    Avoid.cs
    AvoidanceManager.cs
    AvoidCluster.cs
    AvoidInfo.cs
    AvoidLocation.cs
    AvoidObject.cs
    AvoidPathNotFoundException.cs
    AvoidPathResult.cs
    AvoidSide.cs
    AvoidTracelineResult.cs
    ClusterHit.cs
    Helpers.cs
    LineCircleTangentPoints.cs
    LineClusterTangentPoints.cs
    PathResult.cs
  Enums/
    BossAvailableToFaction.cs
    CompleteReason.cs
    DungeonMode.cs
    LfgInvalidError.cs
    LfgState.cs
    LootMode.cs
    PartyMode.cs
    QueueType.cs
  Forms/
    FormConfig.cs
    FormConfig.Designer.cs
    FormConfig.resources
    PathView.cs
    PathView.Designer.cs
    PathView.resources
  Helpers/
    Action.cs
    Decorator.cs
    DecoratorContinue.cs
    DungeonArea.cs
    DungeonBuddySettings.cs
    DynamicStringListConverter.cs
    Error.cs
    ErrorCollection.cs
    ErrorType.cs
    Extra.cs
    Logger.cs
    ScriptHelpers.cs
    TargetingHelper.cs
    Wait.cs
    WaitContinue.cs
  Profiles/
    ElementAttributeAttribute.cs
    IXmlAutoProcessed.cs
    ObsoleteProfileElementAttribute.cs
    Profile.cs
    ProfileElementAttribute.cs
    ProfileManager.cs
    ValueRangeAttribute.cs
    Handlers/
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
  GUI/
    GbConfig.cs
    GbConfig.Designer.cs
    GbConfig.resources
```

### Bots/Grind/
```
LevelBot.cs
  Resources/
    LevelbotResources.cs
    LevelbotResources.resources
```

### Bots/Quest/
```
QuestBot.cs
QuestDebug.cs
QuestManager.cs
QuestState.cs
  Actions/
    ActionSelectQuest.cs
    ActionSelectReward.cs
    ForcedBehaviorExecutor.cs
  Objectives/
    ActionMoveToGrindArea.cs
    CollectItemObjective.cs
    DecoratorCanMoveToGrindArea.cs
    DropDatabase.cs
    GrindObjective.cs
    QuestObjective.cs
    UseGameObjectObjective.cs
  QuestOrder/
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
  Resources/
    QuestBotResources.cs
    QuestBotResources.resources
```

---

## BlueMagic/
```
Executor.cs
ExecutorRand.cs
Extensions.cs
FastSize.cs
IMemoryOperation.cs
Manager.cs
Memory.cs
Windows.cs
WindowShowStyle.cs
  Internals/
    Patch.cs
    PatchManager.cs
    PatternManager.cs
  Native/
    AccessRights.cs
    Context.cs
    ContextFlags.cs
    CopyOfAccessRights.cs
    CopyOfWaitValues.cs
    FLOATING_SAVE_AREA.cs
    Imports.cs
    MemoryAllocType.cs
    MemoryFreeType.cs
    MemoryProtectType.cs
    MEMORY_BASIC_INFORMATION.cs
    PeHeaderParser.cs
    Rect.cs
    ThreadFlags.cs
    WaitValues.cs
  Properties/
    BlueMagicResources.cs
    BlueMagicResources.resources
```

---

## BugSubmitter/
```
Submitter.cs
  Form/
    SubmitterForm.cs
    SubmitterForm.Designer.cs
    SubmitterForm.resources
```

## Bugzproxy/
```
Bug.cs
IProxy.cs
Product.cs
Server.cs
  ProxyStructs/
    AppendCommentParam.cs
    BugIds.cs
    BugInfo.cs
    CreateBugParam.cs
    CreateBugResult.cs
    GetBugsResult.cs
    GetLegalValuesForBugFieldParam.cs
    GetLegalValuesForBugFieldResult.cs
    GetProductsResult.cs
    GetTimezoneResult.cs
    GetVersionResult.cs
    LoginParam.cs
    LoginResult.cs
    ProductIds.cs
    ProductInfo.cs
    SetBugResolutionParam.cs
```

## CookComputing/
```
  XmlRpc/
    DateTime8601.cs
    Fault.cs
    IHttpRequest.cs
    IHttpRequestHandler.cs
    IHttpResponse.cs
    IXmlRpcProxy.cs
    MappingAction.cs
    RequestResponseLogger.cs
    SystemMethodsBase.cs
    Util.cs
    XmlRpcAsyncResult.cs
    XmlRpcBeginAttribute.cs
    XmlRpcBoolean.cs
    XmlRpcClientFormatterSink.cs
    XmlRpcClientFormatterSinkProvider.cs
    XmlRpcClientProtocol.cs
    XmlRpcDateTime.cs
    XmlRpcDocWriter.cs
    XmlRpcDouble.cs
    XmlRpcDupXmlRpcMethodNames.cs
    XmlRpcEndAttribute.cs
    XmlRpcException.cs
    XmlRpcFaultException.cs
    XmlRpcHttpRequest.cs
    XmlRpcHttpResponse.cs
    XmlRpcHttpServerProtocol.cs
    XmlRpcIllFormedXmlException.cs
    XmlRpcInt.cs
    XmlRpcInvalidParametersException.cs
    XmlRpcInvalidReturnType.cs
    XmlRpcInvalidXmlRpcException.cs
    XmlRpcListenerRequest.cs
    XmlRpcListenerResponse.cs
    XmlRpcListenerService.cs
    XmlRpcLogger.cs
    XmlRpcMappingSerializeException.cs
    XmlRpcMemberAttribute.cs
    XmlRpcMethodAttribute.cs
    XmlRpcMethodAttributeException.cs
    XmlRpcMethodInfo.cs
    XmlRpcMissingMappingAttribute.cs
    XmlRpcMissingUrl.cs
    XmlRpcNonRegularArrayException.cs
    XmlRpcNonSerializedMember.cs
    XmlRpcNonStandard.cs
    XmlRpcNullParameterException.cs
    XmlRpcNullReferenceException.cs
    XmlRpcParameterAttribute.cs
    XmlRpcParameterInfo.cs
    XmlRpcProxyGen.cs
    XmlRpcRequest.cs
    XmlRpcRequestEventArgs.cs
    XmlRpcRequestEventHandler.cs
    XmlRpcResponse.cs
    XmlRpcResponseEventArgs.cs
    XmlRpcResponseEventHandler.cs
    XmlRpcReturnValueAttribute.cs
    XmlRpcSerializer.cs
    XmlRpcServerException.cs
    XmlRpcServerFormatterSink.cs
    XmlRpcServerFormatterSinkProvider.cs
    XmlRpcServerProtocol.cs
    XmlRpcService.cs
    XmlRpcServiceAttribute.cs
    XmlRpcServiceInfo.cs
    XmlRpcStruct.cs
    XmlRpcType.cs
    XmlRpcTypeMismatchException.cs
    XmlRpcUnexpectedTypeException.cs
    XmlRpcUnsupportedMethodException.cs
    XmlRpcUnsupportedTypeException.cs
    XmlRpcUrlAttribute.cs
```

## HBRemoting/
```
BotMessage.cs
Cache.cs
IObserver.cs
```

## Headblender/
```
  XmlRpc/
    XmlRpcProxyCodeGen.cs
    XmlRpcProxyCodeGenOptions.cs
```

## Instancebuddy/
```
ActionIdle.cs
IBSettings.cs
Instancebuddy.cs
LfgDungeonInfo.cs
MyExtensions.cs
Profiles.cs
Profiles.resources
Scripts.cs
Scripts.resources
TalentManager.cs
  GUI/
    FarmingForm.cs
    FarmingForm.Designer.cs
    FarmingForm.resources
    FormConfiguration.cs
    FormConfiguration.Designer.cs
    FormConfiguration.resources
```

## Levelbot/
```
FormLevelbotSettings.cs
FormLevelbotSettings.Designer.cs
FormLevelbotSettings.resources
  Actions/
    Combat/
      ActionMoveToTarget.cs
      ActionPull.cs
      ActionSetTarget.cs
    Death/
      ActionMoveToCorpse.cs
      ActionReleaseFromCorpse.cs
      ActionRetrieveCorpse.cs
      ActionSuceedIfDeadOrGhost.cs
  Decorators/
    Combat/
      DecoratorNeedToFindTarget.cs
    Death/
      DecoratorInstanceRelease.cs
      DecoratorNeedToMoveToCorpse.cs
      DecoratorNeedToRelease.cs
      DecoratorNeedToTakeCorpse.cs
  ProfileCreation/
    ProfileVendorListViewItem.cs
    QualityFlags.cs
    Forms/
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

## MainDev/
```
  RemoteASM/
    AsmState.cs
    BitHelper.cs
    CallingConvention.cs
    CodeReader.cs
    CpuState.cs
    EFlags.cs
    FlagsHelper.cs
    FpuControl.cs
    FpuFlags.cs
    FpuState.cs
    FpuStatus.cs
    FpuTag.cs
    FpuTagValue.cs
    FpuWord.cs
    Imports.cs
    Memory.cs
    MmWord.cs
    ModRm.cs
    ModRmMode.cs
    Precision.cs
    Prefix1.cs
    Prefix2.cs
    Prefix3.cs
    Prefix4.cs
    PrefixCollection.cs
    ProcessMemory.cs
    ProcessMemoryReadOnly.cs
    Register.cs
    Registers.cs
    RemoteAsm.cs
    RemoteAsmException.cs
    RoundingControl.cs
    Sib.cs
    SseState.cs
    StateCapture.cs
    UnhandledPrefixException.cs
    UnknownInstructionException.cs
    XmmWord.cs
    Handlers/
      Adc.cs
      Add.cs
      And.cs
      Bt.cs
      Bts.cs
      Cmppd.cs
      Comiss.cs
      Cvtsi2ss.cs
      Cvttss2si.cs
      Dec.cs
      Divps.cs
      Fcomi.cs
      Inc.cs
      Mul.cs
      Neg.cs
      OpcodeHandler.cs
      OpcodeHandlerAttribute.cs
      OpcodeMnemonic.cs
      Or.cs
      Sar.cs
      Sbb.cs
      Setz.cs
      Shl.cs
      Shr.cs
      Shrd.cs
      Sqrtss.cs
      Sub.cs
      Test.cs
      Ucomisd.cs
      Ucomiss.cs
      Xor.cs
```

## MRG/
```
  Controls/
    UI/
      LoadingCircle.cs
      LoadingCircleToolStripMenuItem.cs
```

## NewMixedMode/
```
FormChooser.cs
FormChooser.Designer.cs
FormChooser.resources
MixedModeEx.cs
MixedModeSettings.cs
  Resources/
    NewMixedModeResources.cs
    NewMixedModeResources.resources
```

## PartyBot/
```
DiscoBot.cs
PartyBotSettings.cs
  Forms/
    FormConfig.cs
    FormConfig.Designer.cs
    FormConfig.resources
  IPC/
    ClientBotMessageRecievedEventArgs.cs
    RemotingClient.cs
  Navigation/
    PartyNavigator.cs
```

## SevenZip/
```
CoderPropID.cs
ICodeProgress.cs
ICoder.cs
ISetCoderProperties.cs
ISetDecoderProperties.cs
IWriteCoderProperties.cs
  Buffer/
    InBuffer.cs
    OutBuffer.cs
  CommandLineParser/
    CommandForm.cs
    Parser.cs
    SwitchForm.cs
    SwitchResult.cs
    SwitchType.cs
  Compression/
    LZ/
      BinTree.cs
      IInWindowStream.cs
      IMatchFinder.cs
      InWindow.cs
      OutWindow.cs
    LZMA/
      Decoder.cs
      Encoder.cs
```

## SmartAssembly/
```
  Attributes/
    DoNotCaptureFieldsAttribute.cs
  SmartExceptionsCore/
    ReportingService.cs
    SmartStackFrame.cs
    UploadReportLoginService.cs
    Resources/
      (image assets only — no .cs files)
```

## plugins/
```
  autoequip/
    weight sets/
      (22 .xml weight set files — no .cs files)
  eAuction/
    AuctionHouseForm.resources   (no .cs files)
```

## Other Non-Code Folders
```
Debug/
  BindingsDebugWindow.xaml
  BindingsDebugWindow.xaml.cs

images/
  (11 image files — no .cs files)

themes/
  expressiondark.xaml

xmlrpc/
  Tracer.cs

Properties/
  AssemblyInfo.cs
  Settings.Designer.cs
  Settings.settings

Resources/
  HonorbuddyResources.cs
  HonorbuddyResources.resources
```

---

## ns0/ through ns43/ (Obfuscated Namespaces)

### ns0/
```
Attribute10.cs, Attribute18.cs, Attribute20.cs, Class0.cs, Class264.cs, Class385.cs,
Class406.cs, Class621.cs, Class627.cs, Class640.cs, Class641.cs
```

### ns1/
```
Attribute5.cs, Class252.cs, Class300.cs, Class300.resources, Class321.cs, Class358.cs,
Class368.cs, Class435.cs, Class493.cs, Class521.cs, Class541.cs, Class597.cs, Delegate4.cs,
FormRoadMapper.cs, FormRoadMapper.Designer.cs, Stream1.cs
```

### ns2/
```
Class2.cs, Class363.cs, Class372.cs, Class387.cs, Class441.cs, Class471.cs, Class515.cs,
Struct91.cs
```

### ns3/
```
Attribute4.cs, Attribute6.cs, Class15.cs, Class16.cs, Class266.cs, Class27.cs, Class273.cs,
Class285.cs, Class290.cs, Class3.cs, Class339.cs, Class360.cs, Class38.cs, Class388.cs,
Class431.cs, Class546.cs, Class624.cs, Class654.cs, Class85.cs
```

### ns4/
```
Attribute19.cs, Class274.cs, Class4.cs, Class494.cs, Class504.cs, Class530.cs, Class608.cs,
Exception0.cs, RoutineSelectionForm.cs, RoutineSelectionForm.Designer.cs,
RoutineSelectionForm.resources
```

### ns5/
```
Class249.cs, Class258.cs, Class267.cs, Class309.cs, Class309.resources, Class316.cs,
Class316.resources, Class333.cs, Class337.cs, Class365.cs, Class454.cs, Class5.cs,
Class500.cs, Class666.cs, Class80.cs, Interface6.cs
```

### ns6/
```
Class260.cs, Class263.cs, Class393.cs, Class398.cs, Class544.cs, Class6.cs, Class646.cs,
Enum16.cs, Struct44.cs
```

### ns7/
```
Attribute15.cs, Attribute25.cs, Class254.cs, Class276.cs, Class312.cs, Class326.cs,
Class375.cs, Class409.cs, Class510.cs, Class580.cs, Class7.cs, EventArgs2.cs
```

### ns8/
```
Attribute7.cs, AuctionHouseForm.cs, AuctionHouseForm.Designer.cs, AuctionHouseForm.resources,
Class10.cs, Class146.cs, Class231.cs, Class247.cs, Class272.cs, Class386.cs, Class400.cs,
Class411.cs, EventArgs5.cs, Interface4.cs, Interface7.cs, Struct79.cs
```

### ns9/
```
Class25.cs, Class25.resources, Class253.cs, Class270.cs, Class293.cs, Class301.cs,
Class307.cs, Class327.cs, Class332.cs, Class39.cs, Class432.cs, Class45.cs, Class511.cs,
Class577.cs, Class8.cs, Exception1.cs
```

### ns10/
```
Class278.cs, Class343.cs, Class434.cs, Class482.cs, Class507.cs, Class656.cs, Class9.cs,
EventArgs1.cs
```

### ns11/
```
Class101.cs, Class11.cs, Class331.cs, Class342.cs, Class376.cs, Class429.cs, Class44.cs,
Class50.cs, Class593.cs, Class626.cs, Class81.cs
```

### ns12/
```
Class12.cs, Class19.cs, Class344.cs, Class495.cs, Class512.cs, Class594.cs, Enum0.cs
```

### ns13/
```
Class13.cs, Class268.cs, Class269.cs, Class275.cs, Class438.cs, Class505.cs, Class549.cs,
Class602.cs, Enum10.cs, Enum5.cs, Struct38.cs
```

### ns14/
```
Attribute2.cs, Class14.cs, Class238.cs, Class246.cs, Class288.cs, Class353.cs, Class377.cs,
Class390.cs, Class403.cs, Class440.cs, Class498.cs, Class508.cs, Class513.cs, Enum8.cs,
Struct32.cs
```

### ns15/
```
Attribute1.cs, Class17.cs, Class318.cs, Class448.cs, Class455.cs, Class502.cs, Class569.cs,
Class609.cs, Class614.cs, Control2.cs, Enum26.cs, Exception2.cs, Exception3.cs
```

### ns16/
```
Class18.cs, Class256.cs, Class308.cs, Class308.resources, Class402.cs, Class451.cs,
Class514.cs, Class99.cs, Struct58.cs, Struct65.cs
```

### ns17/
```
Attribute22.cs, Attribute8.cs, Class20.cs, Class287.cs, Class349.cs, Class366.cs,
Class384.cs, Class389.cs, Class40.cs, Class43.cs, Class539.cs, Class55.cs, Delegate3.cs,
Enum11.cs, Enum7.cs
```

### ns18/
```
Class21.cs, Class23.cs, Class257.cs, Class323.cs, Class369.cs, Class382.cs, Class582.cs,
Class587.cs, Class590.cs, Class592.cs, Class596.cs, Class611.cs, EventArgs3.cs
```

### ns19/
```
Attribute26.cs, Class347.cs, Class348.cs, Class370.cs, Class499.cs, Class586.cs,
Class598.cs, Class658.cs, Class665.cs, Class88.cs, Delegate0.cs, Struct111.cs, Struct93.cs
```

### ns20/
```
Attribute17.cs, Class24.cs, Class361.cs, Class371.cs, Class374.cs, Class404.cs, Class405.cs,
Class84.cs, Control3.cs
```

### ns21/
```
Attribute14.cs, Class26.cs, Class26.resources, Class280.cs, Class281.cs, Class322.cs,
Class378.cs, Class430.cs, Class625.cs, Struct63.cs, Struct64.cs
```

### ns22/
```
Class261.cs, Class294.cs, Class338.cs, Class37.cs, Class396.cs, Class399.cs, Class42.cs,
Class48.cs, Enum6.cs, Form1.cs, Form1.Designer.cs
```

### ns23/
```
Attribute12.cs, Class324.cs, Class357.cs, Class41.cs, Class496.cs, Class51.cs, Class591.cs
```

### ns24/
```
Class156.cs, Class410.cs, Class46.cs, Class481.cs, Class485.cs, Class610.cs, Class618.cs
```

### ns25/
```
Attribute13.cs, Attribute16.cs, Class265.cs, Class289.cs, Class291.cs, Class335.cs,
Class350.cs, Class47.cs, Class501.cs, Class52.cs, Delegate2.cs, Enum12.cs
```

### ns26/
```
Attribute9.cs, Class147.cs, Class286.cs, Class355.cs, Class356.cs, Class380.cs, Class391.cs,
Class49.cs, Class584.cs
```

### ns27/
```
Class259.cs, Class262.cs, Class423.cs, Class424.cs, Class53.cs, EventArgs0.cs
```

### ns28/
```
Attribute3.cs, Class425.cs, Class54.cs, Class613.cs, Class622.cs, Enum4.cs
```

### ns29/
```
Attribute0.cs, Attribute27.cs, Class153.cs, Class283.cs, Class319.cs, Class329.cs,
Class340.cs, Class395.cs, Class445.cs, Class452.cs, Class472.cs, Class479.cs, Class56.cs,
Delegate1.cs, Struct94.cs
```

### ns30/
```
Attribute23.cs, Class351.cs, Class392.cs, Class439.cs, Class545.cs, Class57.cs, Class588.cs,
Class620.cs, Class86.cs, Control0.cs, Struct112.cs
```

### ns31/
```
Attribute11.cs, Class152.cs, Class251.cs, Class271.cs, Class284.cs, Class311.cs, Class341.cs,
Class346.cs, Class373.cs, Class401.cs, Class407.cs, Class657.cs, Class72.cs, Struct31.cs
```

### ns32/
```
Class367.cs, Class394.cs, Class397.cs, Class503.cs, Class570.cs, Class76.cs, Enum9.cs,
Interface1.cs
```

### ns33/
```
Class330.cs, Class352.cs, Class383.cs, Class607.cs, Class87.cs, Interface3.cs, Struct110.cs
```

### ns34/
```
Class102.cs, Class279.cs, Class320.cs, Class345.cs, Class509.cs, Class585.cs, Struct92.cs
```

### ns35/
```
Class236.cs, Class255.cs, Class282.cs, Class359.cs, Class480.cs, Exception4.cs, Struct62.cs
```

### ns36/
```
Class248.cs, Class325.cs, Class379.cs, Class506.cs, Class619.cs, Interface2.cs, Interface5.cs
```

### ns37/
```
Class250.cs, Class292.cs, Class362.cs, Class422.cs, Class437.cs, Class617.cs, Class623.cs
```

### ns38/
```
Attribute21.cs, Class277.cs, Class328.cs, Class364.cs, Form0.cs, Form0.Designer.cs
```

### ns39/
```
Class310.cs, Class317.cs, Class497.cs, Class589.cs, Class595.cs, Struct72.cs
```

### ns40/
```
Class334.cs, Class547.cs, Class581.cs, Class612.cs
```

### ns41/
```
Class336.cs, Class354.cs, Class381.cs, Class408.cs, Class436.cs, Class583.cs, Class599.cs
```

### ns42/
```
Attribute24.cs, Class548.cs, Interface0.cs, Struct109.cs
```

### ns43/
```
Class433.cs, Class469.cs, Class615.cs, Control1.cs
```

---

## Summary Statistics

| Area | Directories | Approx .cs Files |
|------|-------------|-------------------|
| Styx/ (all) | ~30 | ~220 |
| TreeSharp/ | 1 | 19 |
| Tripper/ | 3 | 21 |
| CommonBehaviors/ | 4 | 24 |
| Bots/ (all) | ~25 | ~90 |
| BlueMagic/ | 4 | 26 |
| ns0–ns43/ | 44 | ~430 |
| Root-level | 1 | 13 |
| Other (Levelbot, MainDev, etc.) | ~20 | ~90 |
| **TOTAL** | **~132** | **~930+** |

> **Note:** The ns0–ns43 folders contain obfuscated decompiled code. Per project instructions, files starting with special characters should be ignored. Only files starting with a letter (Class*, Attribute*, Enum*, Struct*, Interface*, Delegate*, Exception*, EventArgs*, Form*, Control*, Stream*) are listed above.
