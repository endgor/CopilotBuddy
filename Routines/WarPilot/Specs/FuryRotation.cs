using Styx;
using TreeSharp;
using WarPilot.Core;

namespace WarPilot.Specs
{
    /// <summary>
    /// Fury warrior — NOT a planned spec for this character (leveling = Arms, dungeons = Prot).
    /// PHASE 1 STUB: minimal Berserker-stance attack so a misdetected/dual-spec Fury toon still
    /// fights, with a one-time notice. Not on the roadmap unless requested.
    /// </summary>
    public static class FuryRotation
    {
        private const string StubNotice =
            "[WarPilot] Fury is not implemented (use Arms for leveling). Running a minimal attack fallback.";

        public static Composite Combat()
        {
            return new PrioritySelector(
                WarCommon.WarnOnce(StubNotice),
                WarCommon.EnsureStance(ShapeshiftForm.BerserkerStance, "Berserker Stance"),
                WarCommon.CreateAutoAttack(),
                WarSpell.Cast("Bloodthirst"),
                WarSpell.Cast("Whirlwind"),
                WarSpell.Cast("Slam", ret => WarCommon.Me.HasAura("Bloodsurge")),
                WarSpell.Cast("Heroic Strike", ret => WarCommon.RagePercent >= 60 && WarCommon.CanQueueOnSwing),
                WarMovement.CreateFaceTarget(),
                WarMovement.CreateMoveToMelee()
            );
        }
    }
}
