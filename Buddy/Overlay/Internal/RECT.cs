using System;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal struct RECT : IEquatable<RECT>
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public bool Equals(RECT other)
        {
            return Bottom == other.Bottom && Right == other.Right &&
                Top == other.Top && Left == other.Left;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is RECT && Equals((RECT)obj);
        }

        public override int GetHashCode()
        {
            return ((Bottom * 397 ^ Right) * 397 ^ Top) * 397 ^ Left;
        }

        public static bool operator ==(RECT left, RECT right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RECT left, RECT right)
        {
            return !left.Equals(right);
        }
    }
}
