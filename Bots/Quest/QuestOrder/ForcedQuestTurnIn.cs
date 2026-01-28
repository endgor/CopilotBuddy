// Decompiled with JetBrains decompiler
// Type: Bots.Quest.QuestOrder.ForcedQuestTurnIn
// Assembly: Honorbuddy, Version=2.0.0.5999, Culture=neutral, PublicKeyToken=50a565ab5c01ae50
// MVID: FB7FEB85-27C0-4D17-B8DE-615FDFDA7752
// Assembly location: C:\Users\Texy6\Desktop\Honorbuddy-cleaned.exe

using Bots.Quest.Actions;
using CommonBehaviors.Actions;
using CommonBehaviors.Decorators;
using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory.Frames;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Pathing;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using TreeSharp;

#nullable disable
namespace Bots.Quest.QuestOrder;

public class ForcedQuestTurnIn : ForcedBehavior
{
    private readonly Frame frame_0 = new Frame("QuestTitleButton1");
    private static readonly Frame frame_1 = new Frame("QuestFrameCompleteButton");
    private int int_0;

    public ForcedQuestTurnIn(uint questId, string questName, uint npcId, WoWPoint location)
    {
        this.QuestId = questId;
        this.QuestName = questName;
        this.NpcId = npcId;
        this.Location = location;
    }

    public override bool IsDone => !ObjectManager.Me.QuestLog.ContainsQuest(this.QuestId);

    public uint QuestId { get; private set; }

    public string QuestName { get; private set; }

    public uint NpcId { get; private set; }

    public WoWPoint Location { get; private set; }

    // Dispose removed - we should NEVER abandon a quest we're trying to turn in!
    
    public override void OnStart() => TreeRoot.GoalText = this.method_0();

    private string method_0()
    {
        Styx.Logic.Questing.Quest quest = Styx.Logic.Questing.Quest.FromId(this.QuestId);
        if (quest != null)
            return string.Format("Turning in {0}", (object)quest.Name);
        return !string.IsNullOrEmpty(this.QuestName) ? string.Format("Turning in {0}", (object)this.QuestName) : string.Format("Turning in quest with ID {0}", (object)this.QuestId);
    }

    protected override Composite CreateBehavior()
    {
        // HB 4.3.4 behavior tree structure with 9 elements in the interaction sequence
        return (Composite)new DecoratorIsNotPoiType((IEnumerable<PoiType>)new PoiType[3]
        {
            PoiType.Harvest,
            PoiType.Skin,
            PoiType.Loot
        }, (Composite)new PrioritySelector((ContextChangeHandler)(object_0 => (object)null), new Composite[3]
        {
            (Composite)new Decorator(new CanRunDecoratorDelegate(this.method_1), (Composite)new ActionSetPoi(true, (RetrieveBotPoiDelegate)(object_0 => new BotPoi(PoiType.QuestTurnIn)
            {
                Entry = this.NpcId,
                Location = this.Location
            }))),
            (Composite)new Decorator((CanRunDecoratorDelegate)(object_0 => !(BotPoi.Current.AsObject != (WoWObject)null) ? (double)ForcedQuestTurnIn.Me.Location.DistanceSqr(BotPoi.Current.Location) > 16.0 : !BotPoi.Current.AsObject.WithinInteractRange), (Composite)new ActionMoveToPoi()),
            (Composite)new Decorator((CanRunDecoratorDelegate)(object_0 => BotPoi.Current.AsObject != (WoWObject)null && BotPoi.Current.AsObject.WithinInteractRange), (Composite)new Sequence((ContextChangeHandler)(object_0 => (object)BotPoi.Current.AsObject), new Composite[9]
            {
                // 1. Stop moving
                (Composite)new ActionMoveStop(),
                // 2. Close any open frames first (HB 4.3.4 - ActionDelegate)
                (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_2(object_0))),
                // 3. Target the NPC (ActionDelegate car retourne RunStatus)
                (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_3(object_0))),
                // 4. Interact with NPC (HB 4.3.4 - ActionDelegate)
                (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_4(object_0))),
                // 5. Wait for frame to open
                (Composite)new ActionSleep(250),
                // 6. Select quest from list while GossipFrame or QuestTitleButton1 visible
                (Composite)new DecoratorContinue(new CanRunDecoratorDelegate(this.method_5), (Composite)new Sequence(new Composite[2]
                {
                    (Composite)new ActionSelectQuest((int)this.QuestId),
                    (Composite)new ActionSleep(500)
                })),
                // 7. Complete quest when QuestFrame is visible
                (Composite)new DecoratorContinue(new CanRunDecoratorDelegate(this.method_6), (Composite)new Sequence(new Composite[4]
                {
                    // Click Continue if objectives shown
                    (Composite)new DecoratorContinue(new CanRunDecoratorDelegate(this.method_7), (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_8(object_0)))),
                    // Select reward if there are choices
                    (Composite)new DecoratorContinue(new CanRunDecoratorDelegate(this.method_9), (Composite)new Sequence(new Composite[3]
                    {
                        (Composite)new ActionSleep(750),
                        (Composite)new ActionSelectReward(),
                        (Composite)new ActionSleep(350)
                    })),
                    // Complete the quest (HB 4.3.4 - ActionDelegate)
                    (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_10(object_0))),
                    // Close frames after completion (HB 4.3.4 - ActionDelegate)
                    (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => this.method_2(object_0)))
                })),
                // 8. Clear target (HB 4.3.4 - ActionDelegate)
                (Composite)new TreeSharp.Action((ActionDelegate)(object_0 => ForcedQuestTurnIn.smethod_0(object_0))),
                // 9. Clear POI
                (Composite)new ActionClearPoi("Quest Completed #2")
            }))
        }));
    }

    private bool method_1(object object_0)
    {
        BotPoi current = BotPoi.Current;
        return current.Type != PoiType.QuestTurnIn || (int)current.Entry != (int)this.NpcId;
    }

    private RunStatus method_2(object object_0)
    {
        bool gossipVis = GossipFrame.Instance.IsVisible;
        bool questVis = QuestFrame.Instance.IsVisible;
        Logging.Write("[method_2] GossipFrame: {0}, QuestFrame: {1}", gossipVis, questVis);
        
        if (!gossipVis && !questVis)
        {
            Logging.Write("[method_2] No frames open, returning Success");
            return RunStatus.Success;
        }
        Logging.Write("[method_2] Closing frames...");
        GossipFrame.Instance.Close();
        QuestFrame.Instance.Close();
        Thread.Sleep(300);
        return RunStatus.Running;
    }

    private RunStatus method_3(object object_0)
    {
        // HB 4.3.4: Target the NPC if it's a unit
        WoWUnit woWunit = object_0 as WoWUnit;
        if ((WoWObject)woWunit != (WoWObject)null)
            woWunit.Target();
        return RunStatus.Success;
    }

    private RunStatus method_4(object object_0)
    {
        bool gossipVis = GossipFrame.Instance.IsVisible;
        bool questVis = QuestFrame.Instance.IsVisible;
        Logging.Write("[method_4] GossipFrame: {0}, QuestFrame: {1}", gossipVis, questVis);
        
        if (gossipVis || questVis)
        {
            Logging.Write("[method_4] Frame already open, returning Success");
            return RunStatus.Success;
        }
        WoWObject woWobject = (WoWObject)object_0;
        if (!woWobject.WithinInteractRange)
        {
            Logging.Write("[method_4] Not in range, returning Failure");
            return RunStatus.Failure;
        }
        Logging.Write("[method_4] Interacting...");
        woWobject.Interact();
        Thread.Sleep(300);
        return RunStatus.Running;
    }

    private static LocalPlayer Me => ObjectManager.Me;

    private bool method_5(object object_0)
    {
        // HB 4.3.4: Return true while GossipFrame visible OR QuestTitleButton1 visible
        bool gossipVis = GossipFrame.Instance.IsVisible;
        bool titleBtnVis = this.frame_0.IsVisible;
        bool result = gossipVis || titleBtnVis;
        Logging.Write("[method_5] GossipFrame: {0}, QuestTitleButton1: {1} => {2}", gossipVis, titleBtnVis, result);
        return result;
    }

    private bool method_6(object object_0)
    {
        // HB 4.3.4: Only enter completion sequence when QuestFrame is visible
        bool result = QuestFrame.Instance.IsVisible;
        Logging.Write("[method_6] QuestFrame.IsVisible: {0}", result);
        return result;
    }

    private bool method_7(object object_0)
    {
        bool result = ForcedQuestTurnIn.frame_1.IsVisible;
        Logging.Write("[method_7] QuestFrameCompleteButton.IsVisible: {0}", result);
        return result;
    }

    private RunStatus method_8(object object_0)
    {
        Logging.Write("[method_8] Clicking Continue button");
        QuestFrame.Instance.ClickContinue();
        Thread.Sleep(1000);
        Logging.Write("[method_8] After ClickContinue - QuestFrame.IsVisible: {0}", QuestFrame.Instance.IsVisible);
        return RunStatus.Success;
    }

    private bool method_9(object object_0)
    {
        uint shownId = QuestFrame.Instance.CurrentShownQuestId;
        Styx.Logic.Questing.Quest quest = Styx.Logic.Questing.Quest.FromId(shownId);
        Logging.Write("[method_9] CurrentShownQuestId: {0}, Quest found: {1}", shownId, quest != null);
        
        if (quest == null)
        {
            int luaChoices = Lua.GetReturnVal<int>("return GetNumQuestChoices()", 0U);
            Logging.Write("[method_9] No quest cache, Lua GetNumQuestChoices: {0}", luaChoices);
            return luaChoices >= 1;
        }
        for (uint index = 0; (long)index < (long)quest.InternalInfo.RewardChoiceItem.Length; ++index)
        {
            if (quest.InternalInfo.RewardChoiceItem[(IntPtr)index] != 0)
            {
                Logging.Write("[method_9] Found reward choice at index {0}", index);
                return true;
            }
        }
        int luaChoices2 = Lua.GetReturnVal<int>("return GetNumQuestChoices()", 0U);
        Logging.Write("[method_9] No cache rewards, Lua GetNumQuestChoices: {0}", luaChoices2);
        return luaChoices2 >= 1;
    }

    private RunStatus method_10(object object_0)
    {
        bool qfVisible = QuestFrame.Instance.IsVisible;
        uint shownId = QuestFrame.Instance.CurrentShownQuestId;
        Logging.Write("[method_10] QuestFrame.IsVisible: {0}, CurrentShownQuestId: {1}, Expected: {2}", 
            qfVisible, shownId, this.QuestId);
        
        if (qfVisible && (shownId == this.QuestId || shownId == 0))
        {
            if (this.int_0++ == 5)
            {
                Logging.Write("[method_10] Tried 5 times, closing QuestFrame");
                QuestFrame.Instance.Close();
                this.int_0 = 0;
                return RunStatus.Failure;
            }
            Logging.Write("[method_10] CompleteQuest attempt {0}/5", this.int_0);
            QuestFrame.Instance.CompleteQuest();
            Thread.Sleep(500);
            Logging.Write("[method_10] After CompleteQuest - QuestFrame.IsVisible: {0}", QuestFrame.Instance.IsVisible);
            return RunStatus.Running;
        }
        Logging.Write("[method_10] Conditions not met, returning Success");
        this.int_0 = 0;
        return RunStatus.Success;
    }

    private static RunStatus smethod_0(object object_0)
    {
        if (!ForcedQuestTurnIn.Me.GotTarget)
            return RunStatus.Success;
        ForcedQuestTurnIn.Me.ClearTarget();
        Thread.Sleep(300);
        return RunStatus.Running;
    }

    public override string ToString()
    {
        return string.Format("[ForcedQuestTurnIn QuestId: {0}, QuestName: {1}]", (object)this.QuestId, (object)this.QuestName);
    }
}
