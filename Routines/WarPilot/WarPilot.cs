using System;
using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.Combat.CombatRoutine;
using TreeSharp;
using WarPilot.Core;
using WarPilot.Specs;

namespace WarPilot
{
    /// <summary>
    /// WarPilot — a standalone Warrior combat routine for CopilotBuddy (WotLK 3.3.5a).
    /// Leveling = Arms, dungeons = Protection. The behavior tree is built ONCE and branches on
    /// the live-detected spec at tick time (SpecDetector), so a respec is picked up without a
    /// rebuild. Behaviors are wrapped in a no-op LockSelector — this is an OUT-OF-PROCESS bot, so
    /// real FrameLock would tank WoW's FPS (see project notes).
    ///
    /// Phase 1 status: Arms single-target rotation is WIRED; Protection/Fury are minimal stubs.
    /// </summary>
    public class WarPilot : CombatRoutine
    {
        private Composite _combat, _pull, _preCombat;

        public override string Name { get { return "WarPilot"; } }
        public override WoWClass Class { get { return WoWClass.Warrior; } }
        public override double? PullDistance { get { return 30; } }

        public override bool WantButton { get { return true; } }

        public override bool NeedRest { get { return false; } }
        public override bool NeedPreCombatBuffs { get { return true; } }
        public override bool NeedCombatBuffs { get { return false; } }
        public override bool NeedHeal { get { return false; } }
        public override bool NeedPullBuffs { get { return false; } }

        public override Composite CombatBehavior { get { return _combat; } }
        public override Composite PullBehavior { get { return _pull; } }
        public override Composite PreCombatBuffBehavior { get { return _preCombat; } }

        public override void Initialize()
        {
            Logging.Write(Color.FromArgb(200, 64, 47),
                "[WarPilot] loaded — Warrior routine (Phase 1: Arms wired, Prot/Fury stubbed).");
            SpecDetector.Refresh();
            BuildBehaviors();
        }

        private void BuildBehaviors()
        {
            // One tree, branched on the live spec. Order: Prot, Fury, else Arms (also lowbie default).
            // Target acquisition runs first (cross-spec) so the rotation always has a target to act on.
            _combat = new LockSelector(
                WarTargeting.CreateTargetAcquisition(),
                new Decorator(ret => SpecDetector.Current == WarSpec.Protection, ProtRotation.Combat()),
                new Decorator(ret => SpecDetector.Current == WarSpec.Fury,        FuryRotation.Combat()),
                ArmsRotation.Combat());

            _pull = new LockSelector(
                WarTargeting.CreateTargetAcquisition(),
                new Decorator(ret => SpecDetector.Current == WarSpec.Protection, ProtRotation.Pull()),
                ArmsRotation.Pull());

            _preCombat = new LockSelector(ArmsRotation.PreCombatBuffs());
        }

        public override void OnButtonPress()
        {
            try
            {
                var form = new GUI.WarPilotForm();
                form.ShowDialog();
                form.Dispose();
            }
            catch (Exception ex)
            {
                Logging.Write(Color.OrangeRed, "[WarPilot] settings window error: " + ex.Message);
            }
        }

        /// <summary>
        /// No-op FrameLock wrapper. CopilotBuddy is out-of-process; real FrameLock/BeginExecute
        /// freezes WoW's render thread and collapses FPS. TreeRoot.Tick already locks at tick level,
        /// so this just delegates (matches Singular's LockSelector).
        /// </summary>
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children) : base(children) { }
        }
    }
}
