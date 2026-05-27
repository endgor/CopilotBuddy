namespace Styx.Helpers
{
    /// <summary>
    /// HB 6.2.3: Extension methods on WoWSpec for role/style detection.
    /// WotLK adaptation: DruidGuardian (Cata split) → DruidFeral; Monk specs removed (don't exist in 3.3.5a).
    /// </summary>
    public static class WoWSpecExtensions
    {
        public static bool IsTank(this WoWSpec spec)
        {
            return spec == WoWSpec.PaladinProtection
                || spec == WoWSpec.WarriorProtection
                || spec == WoWSpec.DruidFeral        // WotLK bear tank = Feral (no Guardian in 3.3.5a)
                || spec == WoWSpec.DeathKnightBlood;
        }

        public static bool IsHealer(this WoWSpec spec)
        {
            return spec == WoWSpec.PaladinHoly
                || spec == WoWSpec.DruidRestoration
                || spec == WoWSpec.PriestDiscipline
                || spec == WoWSpec.PriestHoly
                || spec == WoWSpec.ShamanRestoration;
        }

        public static bool IsDps(this WoWSpec spec)
        {
            return !spec.IsTank() && !spec.IsHealer();
        }

        public static bool IsRange(this WoWSpec spec)
        {
            switch (spec)
            {
                case WoWSpec.MageArcane:
                case WoWSpec.MageFire:
                case WoWSpec.MageFrost:
                case WoWSpec.PaladinHoly:
                case WoWSpec.DruidBalance:
                case WoWSpec.DruidRestoration:
                case WoWSpec.HunterBeastMastery:
                case WoWSpec.HunterMarksmanship:
                case WoWSpec.PriestDiscipline:
                case WoWSpec.PriestHoly:
                case WoWSpec.PriestShadow:
                case WoWSpec.ShamanElemental:
                case WoWSpec.ShamanRestoration:
                case WoWSpec.WarlockAffliction:
                case WoWSpec.WarlockDemonology:
                case WoWSpec.WarlockDestruction:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsMelee(this WoWSpec spec)
        {
            return !spec.IsRange();
        }
    }
}
