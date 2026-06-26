using System;
using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.Combat.CombatRoutine;
using TreeSharp;
using Action = TreeSharp.Action;
using PallyPilot.Core;
using PallyPilot.Specs;

namespace PallyPilot
{
    /// <summary>
    /// PallyPilot — a standalone Paladin combat routine for CopilotBuddy (WotLK 3.3.5a). Retribution
    /// for leveling/questing/dungeon-DPS and Holy for dungeon healing are both fully wired; Protection
    /// is a minimal fallback stub. The behavior tree is built ONCE and branches on the live-detected
    /// spec at tick time (SpecDetector), so a respec / dual-spec swap is picked up without a rebuild.
    /// Behaviors are wrapped in a no-op LockSelector — this is an OUT-OF-PROCESS bot, so a real
    /// FrameLock would tank WoW's FPS (see project notes / Singular's LockSelector).
    /// </summary>
    public class PallyPilot : CombatRoutine
    {
        private Composite _combat, _pull, _preCombat, _heal;

        public override string Name { get { return "PallyPilot"; } }
        public override WoWClass Class { get { return WoWClass.Paladin; } }
        public override double? PullDistance { get { return 28; } }

        public override bool WantButton { get { return true; } }

        public override bool NeedRest { get { return false; } }
        public override bool NeedPreCombatBuffs { get { return true; } }
        public override bool NeedCombatBuffs { get { return false; } }

        // Spec-aware: only advertise healing when actually Holy. Otherwise role-aware botbases like
        // LazyRaider see NeedHeal=true and park the paladin in healer mode (running HealBehavior, which
        // does nothing for Ret) instead of driving CombatBehavior — i.e. "it just stands there healing".
        // Read live each pulse, so a respec / dual-spec swap flips the role without a reload.
        public override bool NeedHeal { get { return SpecDetector.Current == PalSpec.Holy; } }
        public override bool NeedPullBuffs { get { return false; } }

        public override Composite CombatBehavior { get { return _combat; } }
        public override Composite PullBehavior { get { return _pull; } }
        public override Composite PreCombatBuffBehavior { get { return _preCombat; } }
        public override Composite HealBehavior { get { return _heal; } }

        public override void Initialize()
        {
            Logging.Write(Color.FromArgb(245, 140, 186),
                "[PallyPilot] loaded — Paladin routine (Retribution + Holy wired, Protection stub).");
            SpecDetector.Refresh();
            BuildBehaviors();
        }

        private void BuildBehaviors()
        {
            // Enemy target acquisition only for the melee/threat specs — Holy never grabs enemies.
            var acquire = new Decorator(ret => SpecDetector.Current != PalSpec.Holy,
                PalTargeting.CreateTargetAcquisition());

            _combat = new LockSelector(
                PalCommon.WarnOnce("[PallyPilot] CombatBehavior is running (engine is driving combat)."),
                acquire,
                Branch(PalSpec.Holy, HolyRotation.Combat()),
                Branch(PalSpec.Protection, ProtRotation.Combat()),
                RetRotation.Combat());                            // Retribution = default (also lowbie)

            _pull = new LockSelector(
                new Decorator(ret => SpecDetector.Current != PalSpec.Holy, PalTargeting.CreateTargetAcquisition()),
                Branch(PalSpec.Holy, HolyRotation.Combat()),
                Branch(PalSpec.Protection, ProtRotation.Pull()),
                RetRotation.Pull());

            _preCombat = new LockSelector(
                PalCommon.WarnOnce("[PallyPilot] PreCombatBuffBehavior is running."),
                Branch(PalSpec.Holy, HolyRotation.PreCombatBuffs()),
                Branch(PalSpec.Protection, new PrioritySelector(PalCommon.MaintainBlessing(), PalCommon.MaintainAura())),
                RetRotation.PreCombatBuffs());

            _heal = new LockSelector(
                PalCommon.WarnOnce("[PallyPilot] HealBehavior is running."),
                Branch(PalSpec.Holy, HolyRotation.Heal()));
        }

        /// <summary>
        /// A spec branch that is TERMINAL: when the spec is active it runs its rotation and stops the
        /// outer selector even if the rotation found nothing to do this tick — so a different spec's
        /// rotation (e.g. its seal/blessing upkeep) never bleeds through and fights ours.
        /// </summary>
        private static Composite Branch(PalSpec spec, Composite rotation)
        {
            return new Decorator(ret => SpecDetector.Current == spec,
                new PrioritySelector(
                    rotation,
                    new Action(ret => RunStatus.Success)));
        }

        public override void OnButtonPress()
        {
            try
            {
                var form = new GUI.PallyPilotForm();
                form.ShowDialog();
                form.Dispose();
            }
            catch (Exception ex)
            {
                Logging.Write(Color.OrangeRed, "[PallyPilot] settings window error: " + ex.Message);
            }
        }

        /// <summary>
        /// No-op FrameLock wrapper. CopilotBuddy is out-of-process; real FrameLock/BeginExecute freezes
        /// WoW's render thread and collapses FPS. TreeRoot.Tick already locks at tick level, so this
        /// just delegates (matches Singular's LockSelector).
        /// </summary>
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children) : base(children) { }
        }
    }
}
