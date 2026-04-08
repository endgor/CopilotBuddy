using System;
using TreeSharp;

namespace Styx.Combat.CombatRoutine
{
	public abstract class CombatRoutine : MarshalByRefObject, IBehaviors, IDisposable, ICombatRoutine
	{
		public void Dispose()
		{
			ShutDown();
		}

		public abstract string Name { get; }

		public abstract WoWClass Class { get; }

		/// <summary>
		/// Display string for routine selection UI. HB 4.3.4 pattern.
		/// </summary>
		public override string ToString()
		{
			return string.Format("[{0}] {1}", Class, Name);
		}

		public virtual double? PullDistance
		{
			get { return null; }
		}

		public virtual bool NeedRest
		{
			get { return false; }
		}

		public virtual void Rest()
		{
		}

		public virtual bool NeedPreCombatBuffs
		{
			get { return false; }
		}

		public virtual void PreCombatBuff()
		{
		}

		public virtual bool NeedPullBuffs
		{
			get { return false; }
		}

		public virtual void PullBuff()
		{
		}

		public virtual void Pull()
		{
		}

		public virtual bool NeedCombatBuffs
		{
			get { return false; }
		}

		public virtual void CombatBuff()
		{
		}

		public virtual void Combat()
		{
		}

		public virtual bool NeedHeal
		{
			get { return false; }
		}

		public virtual void Heal()
		{
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnButtonPress()
		{
		}

		public virtual bool WantButton
		{
			get { return false; }
		}

		public string ButtonText
		{
			get { return "Settings"; }
		}

		public virtual void Pulse()
		{
		}

		/// <summary>
		/// BUG-05 fix: Returns a Decorator wrapping the legacy Rest() method.
		/// HB 4.3.4 pattern: Decorator(NeedRest, Action(Rest))
		/// </summary>
		public virtual Composite RestBehavior
		{
			get { return new Decorator(ret => NeedRest, new TreeSharp.Action(ret => Rest())); }
		}

		public virtual Composite PreCombatBuffBehavior
		{
			get { return new Decorator(ret => NeedPreCombatBuffs, new TreeSharp.Action(ret => PreCombatBuff())); }
		}

		public virtual Composite PullBuffBehavior
		{
			get { return new Decorator(ret => NeedPullBuffs, new TreeSharp.Action(ret => PullBuff())); }
		}

		public virtual Composite PullBehavior
		{
			get { return new TreeSharp.Action(ret => Pull()); }
		}

		public virtual Composite CombatBuffBehavior
		{
			get { return new Decorator(ret => NeedCombatBuffs, new TreeSharp.Action(ret => CombatBuff())); }
		}

		public virtual Composite CombatBehavior
		{
			get { return new TreeSharp.Action(ret => Combat()); }
		}

		public virtual Composite HealBehavior
		{
			get { return new Decorator(ret => NeedHeal, new TreeSharp.Action(ret => Heal())); }
		}

		public virtual Composite MoveToTargetBehavior
		{
			get { return null; }
		}

		public virtual void ShutDown()
		{
		}

		protected CombatRoutine()
		{
		}
	}
}
