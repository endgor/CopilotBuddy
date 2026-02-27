using System;

namespace Styx.WoWInternals
{
    /// <summary>
    /// Represents the 6-bit type portion of a WoW GUID.  Values are taken from
    /// HonorBuddy 6.2.3 and WotLK memory layout.
    /// </summary>
    public enum WoWGuidType : uint
    {
        None,
        Uniq,
        Player,
        Item,
        WorldTransaction,
        StaticDoor,
        Transport,
        Conversation,
        Creature,
        Vehicle,
        Pet,
        GameObject,
        DynamicObject,
        AreaTrigger,
        Corpse,
        LootObject,
        SceneObject,
        Scenario,
        AIGroup,
        DynamicDoor,
        ClientActor,
        Vignette,
        CallForHelp,
        AIResource,
        AILock,
        AILockTicket,
        ChatChannel,
        Party,
        Guild,
        WowAccount,
        BNetAccount,
        GMTask,
        MobileSession,
        RaidGroup,
        Spell,
        Mail,
        WebObj,
        LFGObject,
        LFGList,
        UserRouter,
        PVPQueueGroup,
        UserClient,
        PetBattle,
        UniqUserClient,
        BattlePet,
        CommerceObj,
        ClientSession,
        Cast
    }
}