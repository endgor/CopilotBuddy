namespace Styx.Logic.Pathing
{
    public abstract class StuckHandler
    {
        public abstract bool IsStuck();

        public abstract void Unstick();

        public virtual void Reset()
        {
        }

        public virtual void OnSetAsCurrent()
        {
        }

        public virtual void OnRemoveAsCurrent()
        {
        }
    }
}
