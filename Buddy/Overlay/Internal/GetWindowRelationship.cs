using System;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal enum GetWindowRelationship : uint
    {
        GwHwndfirst,
        GwHwndlast,
        GwHwndnext,
        GwHwndprev,
        GwOwner,
        GwChild,
        GwEnabledpopup
    }
}
