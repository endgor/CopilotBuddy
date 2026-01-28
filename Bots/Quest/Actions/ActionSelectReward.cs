// Decompiled with JetBrains decompiler
// Type: Bots.Quest.Actions.ActionSelectReward
// Assembly: Honorbuddy, Version=2.0.0.5999, Culture=neutral, PublicKeyToken=50a565ab5c01ae50
// Based on HB 4.3.4 ActionSelectReward

using Styx.CommonBot.CharacterManagement;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

#nullable disable
namespace Bots.Quest.Actions;

/// <summary>
/// HB 4.3.4 ActionSelectReward - ALWAYS returns Success
/// </summary>
public class ActionSelectReward : Action
{
    protected override RunStatus Run(object context)
    {
        Styx.Logic.Questing.Quest currentShownQuest = QuestManager.QuestFrame.CurrentShownQuest;
        float bestScore = float.MinValue;
        int bestIndex = -1;
        string bestName = "";
        
        // HB 4.3.4: Get quest info from cache
        if (currentShownQuest != null)
        {
            Styx.WoWInternals.WoWCache.WoWCache.QuestCacheEntry internalInfo = currentShownQuest.InternalInfo;
            // First pass: try to find equippable upgrade
            for (int index = 0; index < internalInfo.RewardChoiceItem.Length; ++index)
            {
                int itemId = internalInfo.RewardChoiceItem[index];
                int itemCount = internalInfo.RewardChoiceItemCount[index];
                
                if (itemId != 0 && itemCount != 0)
                {
                    ItemInfo itemInfo = ItemInfo.FromId((uint)itemId);
                    if (itemInfo != null && ObjectManager.Me.CanEquipItem(itemInfo))
                    {
                        string itemLink = Lua.GetReturnVal<string>($"return GetQuestItemLink('choice', {index + 1})", 0U);
                        if (!string.IsNullOrEmpty(itemLink))
                        {
                            // Simple score based on item level for now
                            float score = itemInfo.Level;
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestIndex = index;
                                bestName = itemInfo.Name;
                            }
                        }
                    }
                }
            }
            
            // Second pass: if no equippable found, select by vendor value
            if (bestIndex == -1)
            {
                float bestValue = float.MinValue;
                for (int index = 0; index < internalInfo.RewardChoiceItem.Length; ++index)
                {
                    int itemId = internalInfo.RewardChoiceItem[index];
                    int itemCount = internalInfo.RewardChoiceItemCount[index];
                    
                    if (itemId != 0 && itemCount > 0)
                    {
                        ItemInfo itemInfo = ItemInfo.FromId((uint)itemId);
                        if (itemInfo != null)
                        {
                            float sellValue = (float)(itemInfo.SellPrice * itemCount);
                            Logging.Write("{0}{1} sells for {2}", 
                                itemInfo.Name, 
                                itemCount > 1 ? ("x" + itemCount) : "", 
                                sellValue);
                            
                            if (sellValue > bestValue)
                            {
                                bestName = itemInfo.Name;
                                bestValue = sellValue;
                                bestIndex = index;
                            }
                        }
                    }
                }
            }
        }
        
        // HB 4.3.4: If still no selection, click first reward
        if (bestIndex == -1)
        {
            Logging.Write("Selecting first reward as the QuestCache seems messed up and contains no questreward choices but we have questrewards to choose from.");
            Lua.DoString("QuestInfoItem1:Click()");
            return RunStatus.Success;
        }
        
        Logging.Write("Choosing {0}", bestName);
        QuestFrame.Instance.SelectQuestReward(bestIndex);
        return RunStatus.Success;
    }
}
