using System;

namespace TreeSharp
{
    /// <summary>
    /// HB 6.2.3: A PrioritySelector whose single child is rebuilt each tick via a delegate.
    /// Useful for routing to a different composite at runtime without rebuilding the whole tree.
    /// </summary>
    public class DynamicChildSelector : PrioritySelector
    {
        private readonly Func<object, Composite> _childGetter;

        public DynamicChildSelector(Func<object, Composite> childGetter)
            : base(Array.Empty<Composite>())
        {
            _childGetter = childGetter;
        }

        public override void Start(object context)
        {
            Children.Clear();
            Children.Add(_childGetter(context));
            base.Start(context);
        }
    }
}
