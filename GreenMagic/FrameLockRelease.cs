using System;
using System.Diagnostics;
using System.Threading;

namespace GreenMagic
{
    /// <summary>
    /// Port of HB's GreyMagic.FrameLockRelease.  Releases an ongoing frame
    /// execution and optionally reacquires it on disposal.
    /// </summary>
    public class FrameLockRelease : IDisposable
    {
        private readonly ExecutorRand _executor;
        private bool _disposed;
        private bool _wasReleased;
        private bool _lastCacheState;
        private readonly bool _reacquireAsHardLock;

        public FrameLockRelease(ExecutorRand executor, bool reacquireAsHardLock)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _reacquireAsHardLock = reacquireAsHardLock;

            bool lockTaken = false;
            bool enterSucceeded = false;
            try
            {
                Monitor.Enter(executor.AssemblyLock, ref enterSucceeded);
                if (executor.IsExecutingContinuously)
                {
                    executor.EndExecute();
                    _lastCacheState = executor.Memory.CacheEnabled;
                    executor.Memory.DisableCache();
                    _wasReleased = true;
                }
                lockTaken = true;
            }
            finally
            {
                if (!lockTaken && enterSucceeded)
                    Monitor.Exit(executor.AssemblyLock);
            }
        }

        private void Restore()
        {
            if (!_disposed)
            {
                try
                {
                    if (_wasReleased)
                    {
                        Debug.Assert(! _executor.IsExecutingContinuously,
                            "We're still executing synchronously! Some nesting of FrameLock classes is horribly wrong..");
                        if (_lastCacheState)
                        {
                            _executor.Memory.EnableCache();
                            _executor.Memory.ClearCache();
                        }
                        else
                        {
                            _executor.Memory.DisableCache();
                        }
                        _executor.BeginExecute();
                        if (_reacquireAsHardLock)
                            _executor.GrabFrame();
                    }
                }
                catch
                {
                    // swallow any exceptions during restore
                }
                Monitor.Exit(_executor.AssemblyLock);
                _disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Restore();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}