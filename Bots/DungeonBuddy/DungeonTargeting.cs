using System;
using System.Collections.Generic;
using System.Linq;
using Bots.DungeonBuddy.Enums;
using Bots.DungeonBuddy.Helpers;
using Bots.DungeonBuddy.Profiles;
using Styx;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace Bots.DungeonBuddy
{
    internal class DungeonTargeting : Targeting
    {
        private static Predicate<WoWObject> _removeFilter;
        private static Func<WoWObject, WoWUnit> _toUnitSelector;

        protected override void DefaultRemoveTargetsFilter(List<WoWObject> objects)
        {
            WoWPartyMember.GroupRole role = StyxWoW.Me.Role;
            if (_removeFilter == null)
            {
                _removeFilter = new Predicate<WoWObject>(ShouldRemoveUnit);
            }
            objects.RemoveAll(_removeFilter);
        }

        protected override void DefaultIncludeTargetsFilter(List<WoWObject> incomingObjects, HashSet<WoWObject> outgoingObjects)
        {
            if (_toUnitSelector == null)
            {
                _toUnitSelector = new Func<WoWObject, WoWUnit>(ToWoWUnit);
            }
            foreach (WoWUnit woWUnit in incomingObjects.Select(_toUnitSelector))
            {
                if (!StyxWoW.Me.IsTank())
                {
                    if (!woWUnit.Combat)
                    {
                        continue;
                    }
                    if (woWUnit.Combat && !woWUnit.TaggedByMe)
                    {
                        continue;
                    }
                }
                else if (!StyxWoW.Me.Combat)
                {
                    if ((woWUnit.DistanceSqr > 1600.0 && !woWUnit.IsTargetingMyPartyMember) || !woWUnit.IsHostile || ProfileManager.IsNpcInPullBlackspot(woWUnit) || (!woWUnit.IsFlying && !Navigator.CanNavigateFully(StyxWoW.Me.Location, woWUnit.Location)))
                    {
                        continue;
                    }
                    if (woWUnit.Entry == 4821U)
                    {
                        continue;
                    }
                }
                else if (!woWUnit.Combat && !woWUnit.TaggedByMe && !woWUnit.IsTargetingMyPartyMember && !woWUnit.IsTargetingMeOrPet)
                {
                    continue;
                }
                if (!woWUnit.IsCritter)
                {
                    if (woWUnit == ScriptHelpers.CurrentBoss && !BossManager.BossTimer.IsRunning)
                    {
                        BossManager.BossTimer.Start();
                    }
                    outgoingObjects.Add(woWUnit);
                }
            }
        }

        protected override void DefaultTargetWeight(List<Targeting.TargetPriority> objs)
        {
            foreach (Targeting.TargetPriority targetPriority in objs)
            {
                WoWUnit woWUnit = targetPriority.Object.ToUnit();
                targetPriority.Score = 100.0;
                if ((StyxWoW.Me.Role & WoWPartyMember.GroupRole.Tank) != WoWPartyMember.GroupRole.None)
                {
                    targetPriority.Score -= woWUnit.Distance;
                    if (woWUnit.IsTargetingMyPartyMember)
                    {
                        targetPriority.Score += 100.0;
                    }
                }
                else if (StyxWoW.Me.Combat)
                {
                    targetPriority.Score += (double)CountPartyMembersTargeting(woWUnit);
                    WoWPlayer tank = ScriptHelpers.Tank;
                    if (tank != null && woWUnit == tank.CurrentTarget)
                    {
                        targetPriority.Score += 100.0;
                    }
                }
                else
                {
                    targetPriority.Score -= woWUnit.HealthPercent;
                }
            }
        }

        private static int CountPartyMembersTargeting(WoWUnit unit)
        {
            PartyTargetChecker checker = new PartyTargetChecker();
            checker.Unit = unit;
            return StyxWoW.Me.PartyMembers.Count(new Func<WoWPlayer, bool>(checker.IsTargetingUnit)) * 100;
        }

        private static bool ShouldRemoveUnit(WoWObject obj)
        {
            bool result;
            if (!obj.IsValid)
            {
                result = true;
            }
            else
            {
                WoWUnit unit = obj as WoWUnit;
                if (unit == null)
                {
                    result = true;
                }
                else if (obj.Entry == 53488U)
                {
                    result = true;
                }
                else if (!unit.Dead && !unit.IsFriendly && !unit.IsNonCombatPet && unit.Attackable && unit.CanSelect && !(unit.ControllingPlayer != null))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        private static WoWUnit ToWoWUnit(WoWObject obj)
        {
            return obj.ToUnit();
        }

        private sealed class PartyTargetChecker
        {
            public bool IsTargetingUnit(WoWPlayer player)
            {
                bool result;
                if (player.CurrentTarget != null)
                {
                    result = player.CurrentTarget == this.Unit;
                }
                else
                {
                    result = false;
                }
                return result;
            }

            public WoWUnit Unit;
        }
    }
}