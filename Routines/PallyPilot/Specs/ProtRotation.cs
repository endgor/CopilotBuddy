using Styx;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using PallyPilot.Core;

namespace PallyPilot.Specs
{
    /// <summary>
    /// Protection paladin — STUB. PallyPilot targets Retribution (leveling) and Holy (dungeon healing);
    /// Protection tanking is out of scope for now. This is a deliberately minimal fallback so a
    /// Prot-specced character still fights rather than standing idle: Righteous Fury for threat, seal
    /// upkeep, and the basic Prot strikes. Full threat/969/taunt/defensives are not implemented.
    /// </summary>
    public static class ProtRotation
    {
        private const string StubNotice =
            "[PallyPilot] Protection is a STUB (threat fallback only). PallyPilot is built for Retribution (leveling) and Holy (healing).";

        public static Composite Combat()
        {
            return new PrioritySelector(
                PalCommon.WarnOnce(StubNotice),
                PalCommon.CreateAutoAttack(),
                PalSpell.BuffSelf("Righteous Fury"),
                PalCommon.MaintainSeal(PalSpell.Known("Seal of Vengeance") ? "Seal of Vengeance"
                                       : (PalSpell.Known("Seal of Corruption") ? "Seal of Corruption" : "Seal of Righteousness")),
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),
                PalSpell.Cast("Hammer of the Righteous"),
                PalSpell.Cast("Shield of Righteousness"),
                PalSpell.Cast("Judgement of Wisdom"),
                PalSpell.Cast("Consecration", ret => PalCommon.ManaPercent >= 40),
                PalSpell.Cast("Holy Shield"),
                PalMovement.CreateFaceTarget(),
                PalMovement.CreateMoveToMelee()
            );
        }

        public static Composite Pull()
        {
            return new PrioritySelector(
                PalSpell.BuffSelf("Righteous Fury"),
                PalCommon.CreateAutoAttack(),
                PalMovement.CreateFaceTarget(),
                PalSpell.Cast("Judgement of Wisdom",
                    ret => StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.Distance > PalMovement.MeleeRange),
                PalMovement.CreateMoveToMelee()
            );
        }
    }
}
