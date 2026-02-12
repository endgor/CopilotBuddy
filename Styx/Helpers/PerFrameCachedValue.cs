using System;

namespace Styx.Helpers
{
    public class PerFrameCachedValue<T>
    {
        private readonly Func<T> _producer;
        private T _cachedValue;
        private int _lastFrameCount;
        
        public PerFrameCachedValue(Func<T> producer)
        {
            if (producer == null)
                throw new ArgumentNullException("producer");
            _producer = producer;
            _lastFrameCount = -1;
            _cachedValue = default!;
        }
        
        public T Value
        {
            get
            {
                // FEAT-38: Use Environment.TickCount as frame counter for per-tick caching
                int currentTick = Environment.TickCount;
                if (currentTick != _lastFrameCount)
                {
                    _cachedValue = _producer();
                    _lastFrameCount = currentTick;
                }
                return _cachedValue;
            }
        }

        /// <summary>Forces a cache refresh on the next access.</summary>
        public void Invalidate()
        {
            _lastFrameCount = -1;
        }
        
        public static implicit operator T(PerFrameCachedValue<T> pfcv)
        {
            return pfcv.Value;
        }
    }
}
