

UPDATE 4
==========================

Bug fixes:
--------------
  * Extractor (mmaps)
    - minRegionArea: rcSqr(12) → rcSqr(20) — removes small isolated navmesh regions on rough hilltops, reducing unreachable areas the bot tries to path into

  * Bot (Navigator.cs)
    - PathPrecision: 2.0f → 1.6f — matches HB 3.3.5a/6.2.3 exact value, bot is now more precise when determining if it has reached a waypoint or destination

  * Singular (Shaman)
    - Added a new Shaman totem parameter to Singular config to fix fire totem selection(or not fixe)

  * Combat (Targeting.cs)
    - Use `IsBeingAttacked` alongside `Combat` and minion combat checks when including targets, preventing transient combat state drops from losing valid enemies.

  * ProfessionBuddy
    - Fixed CombatBot compatibility crash caused by a null reference during forced quest turn-in handling.

  * Plugin
    - Removed `Thread.Sleep(500)` from AutoEquip2 item equip handling to avoid freezing the WoW client when equipping items.

  *  Profile
    - Restored legacy `SetHearthstone` behavior in QuestBot: all `SetHearthstone` profile entries were removed.

  * Misc
    - Minor cleanup in `Bots/Quest/QuestOrder/ForcedQuestTurnIn.cs` and formatting consistency in `Styx/WoWInternals/WoWObjects/WoWItem.cs`.

  * DungeonBuddy
    - Fixed LFG random queue failures by updating `LfgManager.cs` to dynamically select the correct `LFG_Dungeons.dbc` ID based on the player's level (258, 259, 260, 261, 262) instead of hardcoding the level 80 queue.
    - Added missing methods to `ScriptHelpers.cs` (`PartyIncludingMe`, `IsBossAlive`, `CreateInteractWithObject`, `CreateRunAwayFromBad`) to fix script compilation errors for WotLK 3.3.5a dungeons.
      * DungeonBuddy — Bug fixes (DungeonBuddy.cs)
    - Fixed `ShouldRequeue()` and `CanAcceptLfgProposal()`: `needsMaintenance` now includes
      `ShouldMailItemsInSoloFarm(this)` — bot no longer re-queues while mail items are pending.
    - Fixed Leader party mode: added invite block in `CreateLfgBehavior()` that iterates
      `PartyMembers`, calls `InviteUnit()` for each absent member, throttled by
      `_inviteRetryTimer` (10s). HB waitTimer_5 parity.

  * DungeonBuddy — Secret tab bug fixes (FormConfig.xaml / FormConfig.xaml.cs)
    - Fixed `button1_Click` (Target Info): was logging `boss.IsDead` from `BossManager.Bosses`.
      Now logs `boss.IsAlive` from `ProfileManager.CurrentProfile.BossEncounters` — exact HB parity.
    - Fixed `dbgButton2_Click` (Navigation): was a copy-paste of button1. Replaced with correct
      implementation: `boss.Optional || Navigator.CanNavigateFully(Me.Location, breadcrumb.Peek())`
      per HB 4.3.4. Optional bosses are now correctly skipped.
    - Fixed `btnToggleMovement_Click`: now calls `WoWMovement.MoveStop()` when disabling movement
      and the character is moving — HB parity.
    - Fixed `GenerateTreeView` debug mode: Dungeon List tab now appends `[Map Id: X]` after each
      dungeon name, matching HB 4.3.4. Required for profile authoring (breadcrumb MapId lookup).
    - Fixed `ToggleControlForSelection`: previously only toggled the warning label. Now correctly
      hides/shows the dungeon tree and Select All / Unselect All buttons when `QueueType` is not
      Specific or SoloFarm — HB parity.
    - Fixed `ShowAllDungeons` not persisted: `FormConfig_Load` was hardcoding `cbShowAll = false`.
      Now reads `DungeonBuddySettings.Instance.ShowAllDungeons`. `cbShowAll_CheckedChanged` now
      writes back to the setting before rebuilding the tree.
    - Fixed Secret tab button row: changed `StackPanel` to `WrapPanel` so the 6 buttons wrap to a
      second row on narrow windows (360px) instead of overflowing.

  * DungeonBuddy — PathView (new file: Bots/DungeonBuddy/Forms/PathView.cs)
    - Ported HB 4.3.4 `PathView.cs` in full. Opens as a standalone WinForms window (784×761)
      from the "Show Path" button on the Secret tab (toggle: click again to close).
    - Renders live (100ms refresh, GDI+ double-buffered):
        · Purple semi-transparent circles  = active avoidance zones (AvoidanceManager)
        · Red dots + black lines           = current navigation path (CurrentMovePath / CurrentAvoidPath)
        · Green dot + green line           = player position + heading direction (10 yard ray)
    - Mouse wheel = zoom 1× to 6×. Left drag = pan the view.
    - Singleton pattern (`PathView.Instance`) — only one window can be open at a time.
    - In HB 4.3.4 the "Show Path" button handler (`debugBtn3_Click`) was empty; PathView existed
      but was never wired. CopilotBuddy now wires it correctly.
    - vendre fixe for solofarm. 

