#nullable disable
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Styx.CommonBot.CharacterManagement
{
    /// <summary>
    /// Automatic equipment evaluation for quest rewards and loot
    /// Based on simple stat evaluation for WotLK (3.3.5)
    /// </summary>
    public class AutoEquipper
    {
        private static readonly Dictionary<WoWClass, Dictionary<WoWItemStatType, float>> StatWeights = new Dictionary<WoWClass, Dictionary<WoWItemStatType, float>>();

        static AutoEquipper()
        {
            InitializeStatWeights();
        }

        /// <summary>
        /// Evaluates quest reward items and returns the best one for the character
        /// </summary>
        /// <param name="rewards">Array of available rewards</param>
        /// <returns>Best reward item or null if none suitable</returns>
        public QuestRewardItem EvaluateRewards(QuestRewardItem[] rewards)
        {
            if (rewards == null || rewards.Length == 0)
                return null;

            // Filter out null entries
            QuestRewardItem[] validRewards = rewards.Where(r => r != null && r.ItemInfo != null).ToArray();
            if (validRewards.Length == 0)
                return null;

            QuestRewardItem bestReward = null;
            float bestScore = float.MinValue;

            foreach (QuestRewardItem reward in validRewards)
            {
                float score = EvaluateItem(reward.ItemInfo, reward.Count);
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestReward = reward;
                }
            }

            // Only return the item if it scores better than vendor price baseline
            if (bestReward != null && bestScore > 0)
            {
                Logging.Write("[AutoEquip] Selected {0} (score: {1:F2})", bestReward.ItemInfo.Name, bestScore);
                return bestReward;
            }

            return null;
        }

        /// <summary>
        /// Evaluates an item based on character class and basic stats
        /// </summary>
        private float EvaluateItem(ItemInfo item, int count)
        {
            if (item == null)
                return 0f;

            // Check if we can equip it
            if (!ObjectManager.Me.CanEquipItem(item))
                return 0f;

            float score = 0f;

            // Simple evaluation: bonus for correct armor type
            if (item.ItemClass == WoWItemClass.Armor && item.Armor > 0)
            {
                WoWItemArmorClass wantedArmorClass = GetWantedArmorClass();
                if (item.ArmorClass == wantedArmorClass)
                {
                    // Heavily favor correct armor type
                    score += item.Armor * 0.5f;
                    score += 50f; // Flat bonus for correct type
                }
                else if (item.ArmorClass != WoWItemArmorClass.None)
                {
                    // Light penalty for wrong armor type
                    score += item.Armor * 0.1f;
                }
            }

            // Bonus for weapon DPS
            if (item.ItemClass == WoWItemClass.Weapon && item.DPS > 0)
            {
                score += item.DPS * 3.0f; // DPS is very valuable
            }

            // Check if it's an upgrade over currently equipped item
            WoWInventorySlot slot = GetItemSlot(item);
            if (slot != WoWInventorySlot.None)
            {
                WoWItem equipped = StyxWoW.Me.Inventory.GetItemBySlot((uint)slot);
                if (equipped != null && equipped.ItemInfo != null)
                {
                    float equippedScore = EvaluateItem(equipped.ItemInfo, 1);
                    // Only consider it if it's actually an upgrade
                    if (score <= equippedScore * 1.05f) // Require at least 5% improvement
                    {
                        return 0f;
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// Gets the preferred armor class for the character
        /// </summary>
        private WoWItemArmorClass GetWantedArmorClass()
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Warrior:
                case WoWClass.Paladin:
                case WoWClass.DeathKnight:
                    return StyxWoW.Me.Level >= 40 ? WoWItemArmorClass.Plate : WoWItemArmorClass.Mail;
                case WoWClass.Hunter:
                case WoWClass.Shaman:
                    return StyxWoW.Me.Level >= 40 ? WoWItemArmorClass.Mail : WoWItemArmorClass.Leather;
                case WoWClass.Rogue:
                case WoWClass.Druid:
                    return WoWItemArmorClass.Leather;
                case WoWClass.Priest:
                case WoWClass.Mage:
                case WoWClass.Warlock:
                    return WoWItemArmorClass.Cloth;
                default:
                    return WoWItemArmorClass.None;
            }
        }

        /// <summary>
        /// Gets the inventory slot for an item
        /// </summary>
        private WoWInventorySlot GetItemSlot(ItemInfo item)
        {
            switch (item.InventoryType)
            {
                case InventoryType.Head: return WoWInventorySlot.Head;
                case InventoryType.Neck: return WoWInventorySlot.Neck;
                case InventoryType.Shoulder: return WoWInventorySlot.Shoulders;
                case InventoryType.Body: return WoWInventorySlot.Shirt;
                case InventoryType.Chest:
                case InventoryType.Robe: return WoWInventorySlot.Chest;
                case InventoryType.Waist: return WoWInventorySlot.Waist;
                case InventoryType.Legs: return WoWInventorySlot.Legs;
                case InventoryType.Feet: return WoWInventorySlot.Feet;
                case InventoryType.Wrist: return WoWInventorySlot.Wrists;
                case InventoryType.Hand: return WoWInventorySlot.Hands;
                case InventoryType.Finger: return WoWInventorySlot.Finger1; // Check both finger slots later
                case InventoryType.Trinket: return WoWInventorySlot.Trinket1; // Check both trinket slots later
                case InventoryType.Cloak: return WoWInventorySlot.Back;
                case InventoryType.Weapon:
                case InventoryType.TwoHandWeapon:
                case InventoryType.WeaponMainHand: return WoWInventorySlot.MainHand;
                case InventoryType.WeaponOffHand:
                case InventoryType.Shield:
                case InventoryType.Holdable: return WoWInventorySlot.OffHand;
                case InventoryType.Ranged:
                case InventoryType.RangedRight:
                case InventoryType.Thrown: return WoWInventorySlot.Ranged;
                case InventoryType.Tabard: return WoWInventorySlot.Tabard;
                default: return WoWInventorySlot.None;
            }
        }

        /// <summary>
        /// Initialize stat weights for each class based on WotLK priorities
        /// Simplified version - primarily for future expansion
        /// </summary>
        private static void InitializeStatWeights()
        {
            // Warrior - Physical DPS
            StatWeights[WoWClass.Warrior] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Strength, 2.0f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.CriticalStrikeRating, 1.5f },
                { WoWItemStatType.HitRating, 1.8f },
                { WoWItemStatType.ExpertiseRating, 1.6f },
                { WoWItemStatType.ArmorPenetration, 1.4f },
                { WoWItemStatType.Agility, 0.8f },
                { WoWItemStatType.Stamina, 0.5f }
            };

            // Paladin - Ret/Prot hybrid
            StatWeights[WoWClass.Paladin] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Strength, 2.0f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.CriticalStrikeRating, 1.3f },
                { WoWItemStatType.HitRating, 1.6f },
                { WoWItemStatType.ExpertiseRating, 1.4f },
                { WoWItemStatType.Stamina, 0.8f },
                { WoWItemStatType.Intellect, 0.5f },
                { WoWItemStatType.SpellPower, 0.7f }
            };

            // Hunter - Physical DPS
            StatWeights[WoWClass.Hunter] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Agility, 2.5f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.RangedPower, 1.1f },
                { WoWItemStatType.CriticalStrikeRating, 1.5f },
                { WoWItemStatType.HitRating, 1.8f },
                { WoWItemStatType.ArmorPenetration, 1.3f },
                { WoWItemStatType.Stamina, 0.5f }
            };

            // Rogue - Physical DPS
            StatWeights[WoWClass.Rogue] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Agility, 2.5f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.CriticalStrikeRating, 1.6f },
                { WoWItemStatType.HitRating, 1.9f },
                { WoWItemStatType.ExpertiseRating, 1.7f },
                { WoWItemStatType.ArmorPenetration, 1.4f },
                { WoWItemStatType.Stamina, 0.4f }
            };

            // Priest - Spell caster
            StatWeights[WoWClass.Priest] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.SpellPower, 2.0f },
                { WoWItemStatType.Intellect, 1.5f },
                { WoWItemStatType.Spirit, 1.2f },
                { WoWItemStatType.HitRating, 1.6f },
                { WoWItemStatType.CriticalStrikeRating, 1.3f },
                { WoWItemStatType.HasteRating, 1.2f },
                { WoWItemStatType.Stamina, 0.5f }
            };

            // Shaman - Hybrid
            StatWeights[WoWClass.Shaman] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.SpellPower, 1.8f },
                { WoWItemStatType.Intellect, 1.4f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.Agility, 1.2f },
                { WoWItemStatType.HitRating, 1.5f },
                { WoWItemStatType.CriticalStrikeRating, 1.3f },
                { WoWItemStatType.HasteRating, 1.1f },
                { WoWItemStatType.Stamina, 0.6f }
            };

            // Mage - Spell DPS
            StatWeights[WoWClass.Mage] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.SpellPower, 2.2f },
                { WoWItemStatType.Intellect, 1.6f },
                { WoWItemStatType.HitRating, 1.8f },
                { WoWItemStatType.CriticalStrikeRating, 1.4f },
                { WoWItemStatType.HasteRating, 1.3f },
                { WoWItemStatType.Spirit, 0.8f },
                { WoWItemStatType.Stamina, 0.4f }
            };

            // Warlock - Spell DPS
            StatWeights[WoWClass.Warlock] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.SpellPower, 2.0f },
                { WoWItemStatType.Intellect, 1.5f },
                { WoWItemStatType.HitRating, 1.8f },
                { WoWItemStatType.CriticalStrikeRating, 1.4f },
                { WoWItemStatType.HasteRating, 1.3f },
                { WoWItemStatType.Spirit, 1.0f },
                { WoWItemStatType.Stamina, 0.5f }
            };

            // Druid - Feral/Balance hybrid
            StatWeights[WoWClass.Druid] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Agility, 2.0f },
                { WoWItemStatType.Strength, 1.5f },
                { WoWItemStatType.SpellPower, 1.6f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.Intellect, 1.2f },
                { WoWItemStatType.CriticalStrikeRating, 1.3f },
                { WoWItemStatType.HitRating, 1.5f },
                { WoWItemStatType.Stamina, 0.7f }
            };

            // Death Knight - DPS/Tank hybrid
            StatWeights[WoWClass.DeathKnight] = new Dictionary<WoWItemStatType, float>
            {
                { WoWItemStatType.Strength, 2.2f },
                { WoWItemStatType.AttackPower, 1.0f },
                { WoWItemStatType.CriticalStrikeRating, 1.4f },
                { WoWItemStatType.HitRating, 1.7f },
                { WoWItemStatType.ExpertiseRating, 1.5f },
                { WoWItemStatType.ArmorPenetration, 1.3f },
                { WoWItemStatType.Stamina, 0.8f }
            };
        }
    }
}
