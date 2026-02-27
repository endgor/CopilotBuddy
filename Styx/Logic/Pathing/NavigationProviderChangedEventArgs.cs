using System;

namespace Styx.Logic.Pathing
{
    public class NavigationProviderChangedEventArgs<T> : EventArgs
    {
        internal NavigationProviderChangedEventArgs(T oldProvider, T newProvider)
        {
            OldProvider = oldProvider;
            NewProvider = newProvider;
        }

        public T OldProvider { get; private set; }
        public T NewProvider { get; private set; }
    }
}