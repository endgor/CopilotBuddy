using Styx;
using TreeSharp;
using WarPilot.Core;

namespace WarPilot.Specs
{
    /// <summary>
    /// Protection warrior — dungeon tank. PHASE 1 STUB.
    ///
    /// This is a deliberately MINIMAL fallback so a Prot-specced character still fights (won't
    /// stand idle in a dungeon), but the real tanking content — the 969 threat rotation, AoE
    /// threat, auto-taunt off party members, and defensive cooldowns — is NOT wired yet. It is
    /// scheduled for Phase 2. The Protection settings tab is greyed-out to make that obvious.
    /// </summary>
    public static class ProtRotation
    {
        private const string StubNotice =
            "[WarPilot] Protection is a Phase-1 STUB: basic attack only. Full tanking/threat/taunt comes in Phase 2. Use Arms for leveling.";

        public static Composite Combat()
        {
            return new PrioritySelector(
                WarCommon.WarnOnce(StubNotice),
                WarCommon.EnsureStance(ShapeshiftForm.DefensiveStance, "Defensive Stance"),
                WarCommon.CreateAutoAttack(),
                // Bare-minimum threat so the stub isn't useless; full priority/threat is Phase 2.
                WarSpell.Cast("Shield Slam"),
                WarSpell.Cast("Revenge"),
                WarSpell.Cast("Devastate"),
                WarMovement.CreateFaceTarget(),
                WarMovement.CreateMoveToMelee()
            );
        }

        public static Composite Pull()
        {
            return new PrioritySelector(
                WarCommon.EnsureStance(ShapeshiftForm.DefensiveStance, "Defensive Stance"),
                WarMovement.CreateFaceTarget(),
                WarMovement.CreateCharge(),
                WarCommon.CreateAutoAttack(),
                WarMovement.CreateMoveToMelee()
            );
        }
    }
}
