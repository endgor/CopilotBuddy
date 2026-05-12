using System;

namespace Styx.Logic.Pathing
{
    /// <summary>
    /// WoD-style navigation provider contract used by Navigator.
    /// Direct port of HB 6.2.3 Styx.Pathing.NavigationProvider.
    /// </summary>
    public abstract class NavigationProvider
    {
        // HB 6.2.3: private backing field for StuckHandler (stuckHandler_0)
        private StuckHandler _stuckHandler;

        public abstract MoveResult MoveTo(WoWPoint location);

        public abstract float PathPrecision { get; set; }

        public abstract WoWPoint[] GeneratePath(WoWPoint from, WoWPoint to);

        public abstract bool AtLocation(WoWPoint point1, WoWPoint point2);

        // HB 6.2.3: IsCurrent is NOT virtual — simple reference equality check.
        public bool IsCurrent => Navigator.NavigationProvider == this;

        // HB 6.2.3 NavigationProvider.StuckHandler: only calls lifecycle methods when IsCurrent.
        public virtual StuckHandler StuckHandler
        {
            get => _stuckHandler;
            set
            {
                if (value == _stuckHandler)
                    return;
                if (IsCurrent)
                {
                    value?.OnSetAsCurrent();
                    _stuckHandler?.OnRemoveAsCurrent();
                }
                _stuckHandler = value;
            }
        }

        public virtual bool Clear()
        {
            return true;
        }

        public virtual bool CanNavigateWithin(WoWPoint from, WoWPoint to, float distanceTolerancy)
        {
            WoWPoint[] path = GeneratePath(from, to);
            return path != null && path.Length != 0 && path[path.Length - 1].DistanceSqr(to) < distanceTolerancy * distanceTolerancy;
        }

        public virtual bool CanNavigateFully(WoWPoint from, WoWPoint to)
        {
            WoWPoint[] path = GeneratePath(from, to);
            return path != null && path.Length != 0 && AtLocation(path[path.Length - 1], to);
        }

        public virtual float? PathDistance(WoWPoint from, WoWPoint to, float maxDistance = float.MaxValue)
        {
            WoWPoint[] path = GeneratePath(from, to);
            if (path == null || path.Length == 0)
                return null;

            float distance = from.Distance(path[0]);
            distance += path[path.Length - 1].Distance(to);

            for (int i = 0; i < path.Length - 1; i++)
            {
                if (distance > maxDistance)
                    return maxDistance;
                distance += path[i].Distance(path[i + 1]);
            }

            return distance > maxDistance ? maxDistance : distance;
        }

        // HB 6.2.3: delegates to StuckHandler lifecycle.
        public virtual void OnSetAsCurrent()
        {
            _stuckHandler?.OnSetAsCurrent();
        }

        public virtual void OnRemoveAsCurrent()
        {
            _stuckHandler?.OnRemoveAsCurrent();
        }
    }
}
