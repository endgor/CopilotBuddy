using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;

namespace Levelbot.Actions.Death
{
    public class ActionMoveToCorpse : NavigationAction
    {
        protected override RunStatus Run(object context)
        {
            try
            {
                var me = ObjectManager.Me;
                if (me == null)
                {
                    Logging.WriteDebug("[ActionMoveToCorpse] ObjectManager.Me is null!");
                    return RunStatus.Failure;
                }

                WoWPoint corpse = me.CorpsePoint;
                // Debug log commented to reduce spam
                // Logging.WriteDebug("[ActionMoveToCorpse] CorpsePoint={0}, IsGhost={1}, MyPos={2}",
                //     corpse, me.IsGhost, me.Location);

                if (corpse == WoWPoint.Zero || corpse == WoWPoint.Empty)
                {
                    Logging.WriteDebug("[ActionMoveToCorpse] CorpsePoint is Zero/Empty — cannot navigate!");
                    return RunStatus.Failure;
                }

                return base.Run(corpse);
            }
            catch (System.Exception ex)
            {
                Logging.WriteDebug("[ActionMoveToCorpse] EXCEPTION: {0}", ex.Message);
                return RunStatus.Failure;
            }
        }
    }
}
