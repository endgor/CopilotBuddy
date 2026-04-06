using System;
using System.Collections.Generic;

namespace Styx.WoWInternals.WoWObjects
{
    public class WoWContainer : WoWItem
    {
        #region Descriptor Offsets - CONTAINER_FIELD
        
        // Container descriptors start after OBJECT_END (6 indices) + ITEM_END (0x43 = 67 indices) = 73 absolute indices.
        // Byte offset from descriptor base: 73 * 4 = 292 (0x124)
        private const int CONTAINER_FIELDS_BASE = (6 + 0x43) * 4;  // 292 (0x124)

        // Relative indices within the container descriptor section (from WoWContainerFields.cs)
        private const int CONTAINER_FIELD_NUM_SLOTS = 0x0;     // uint — number of slots
        private const int CONTAINER_ALIGN_PAD = 0x1;           // padding
        private const int CONTAINER_FIELD_SLOT_1 = 0x2;        // Array of 36 x 2 indices (ulong each)
        
        #endregion
        
        #region Constructor
        public WoWContainer(uint baseAddress) : base(baseAddress)
        {
        }
        
        #endregion

        #region BagType

        /// <summary>
        /// FEAT-31: Gets the bag type from the item's SubClass.
        /// WotLK bag SubClass mapping: 0=Normal, 1=SoulShard, 2=Herb, 3=Enchanting,
        /// 4=Engineering, 5=Gem, 6=Mining, 7=Leatherworking, 8=Inscription, 9=Ammo.
        /// </summary>
        public BagType BagType
        {
            get
            {
                try
                {
                    var info = ItemInfo;
                    if (info != null)
                    {
                        int subClass = (int)info.SubClassId;
                        if (Enum.IsDefined(typeof(BagType), subClass))
                            return (BagType)subClass;
                    }
                }
                catch { }
                return BagType.Normal;
            }
        }

        #endregion
        
        #region Container Properties
        public uint NumSlots
        {
            get
            {
                return GetDescriptorField<uint>(CONTAINER_FIELDS_BASE + CONTAINER_FIELD_NUM_SLOTS * 4);
            }
        }
        public uint FreeSlotsCount
        {
            get
            {
                uint free = 0;
                uint slots = NumSlots;
                for (uint i = 0; i < slots; i++)
                {
                    if (GetSlotGuid(i) == 0)
                        free++;
                }
                return free;
            }
        }

        /// <summary>
        /// Gets the number of free slots.
        /// Ported from HB 3.3.5a WoWContainer.FreeSlots
        /// </summary>
        public uint FreeSlots
        {
            get
            {
                try
                {
                    return FreeSlotsCount;
                }
                catch (Exception ex)
                {
                    Styx.Helpers.Logging.WriteException(ex);
                    return 0;
                }
            }
        }

        public bool IsFull => FreeSlotsCount == 0;
        public bool IsEmpty => FreeSlotsCount == NumSlots;

        /// <summary>
        /// Alias for NumSlots. Ported from HB 4.3.4 WoWContainer.Slots.
        /// </summary>
        public uint Slots => NumSlots;

        /// <summary>
        /// Number of occupied slots. Ported from HB 4.3.4 WoWContainer.UsedSlots.
        /// </summary>
        public uint UsedSlots => NumSlots - FreeSlotsCount;

        /// <summary>
        /// Gets the bag slot index (0–10) this container occupies, or -1 if not found.
        /// Ported from HB 4.3.4 WoWContainer.BagIndex.
        /// </summary>
        public new int BagIndex
        {
            get
            {
                for (uint i = 0U; i <= 10U; i++)
                {
                    if (ObjectManager.Me.GetBagGuidAtIndex(i) == Guid)
                        return (int)i;
                }
                return -1;
            }
        }
        
        #endregion
        
        #region Slot Access
        public ulong GetSlotGuid(uint slotIndex)
        {
            if (slotIndex >= NumSlots)
                return 0;
            
            // Each slot is 2 descriptor indices (8 bytes for ulong GUID)
            int offset = CONTAINER_FIELDS_BASE + (CONTAINER_FIELD_SLOT_1 + (int)(slotIndex * 2)) * 4;
            return GetDescriptorField<ulong>(offset);
        }
        public WoWItem? GetSlotItem(uint slotIndex)
        {
            ulong guid = GetSlotGuid(slotIndex);
            if (guid == 0)
                return null;
                
            return ObjectManager.GetObjectByGuid<WoWItem>(guid);
        }
        public List<WoWItem> Items
        {
            get
            {
                var items = new List<WoWItem>();
                uint slots = NumSlots;
                for (uint i = 0; i < slots; i++)
                {
                    var item = GetSlotItem(i);
                    if (item != null)
                        items.Add(item);
                }
                return items;
            }
        }

        public ulong[] ItemGuids
        {
            get
            {
                uint slots = NumSlots;
                var guids = new ulong[slots];
                for (uint i = 0; i < slots; i++)
                    guids[i] = GetSlotGuid(i);
                return guids;
            }
        }

        /// <summary>
        /// Gets GUIDs of physical (non-zero) items.
        /// Ported from HB 3.3.5a WoWContainer.PhysicalItemGuids
        /// </summary>
        public ulong[] PhysicalItemGuids
        {
            get
            {
                try
                {
                    var guids = new List<ulong>();
                    uint slots = NumSlots;
                    for (uint i = 0; i < slots; i++)
                    {
                        ulong guid = GetSlotGuid(i);
                        if (guid != 0UL)
                            guids.Add(guid);
                    }
                    return guids.ToArray();
                }
                catch (Exception ex)
                {
                    Styx.Helpers.Logging.WriteException(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets non-null items as an array. Ported from HB 4.3.4 WoWContainer.PhysicalItems.
        /// </summary>
        public WoWItem[] PhysicalItems => Items.ToArray();

        /// <summary>
        /// Returns the item GUID at the given slot. Ported from HB 4.3.4 WoWContainer.GetItemGuidBySlot.
        /// </summary>
        public ulong GetItemGuidBySlot(uint slot) => GetSlotGuid(slot);

        /// <summary>
        /// Returns the item at the given slot. Ported from HB 4.3.4 WoWContainer.GetItemBySlot.
        /// </summary>
        public WoWItem? GetItemBySlot(uint slot) => GetSlotItem(slot);
        
        #endregion
        
        #region Search Methods
        public int FindFirstFreeSlot()
        {
            uint slots = NumSlots;
            for (uint i = 0; i < slots; i++)
            {
                if (GetSlotGuid(i) == 0)
                    return (int)i;
            }
            return -1;
        }
        public WoWItem? FindItemByEntry(uint entryId)
        {
            uint slots = NumSlots;
            for (uint i = 0; i < slots; i++)
            {
                var item = GetSlotItem(i);
                if (item != null && item.Entry == entryId)
                    return item;
            }
            return null;
        }
        public uint CountItemsByEntry(uint entryId)
        {
            uint count = 0;
            uint slots = NumSlots;
            for (uint i = 0; i < slots; i++)
            {
                var item = GetSlotItem(i);
                if (item != null && item.Entry == entryId)
                    count += item.StackCount;
            }
            return count;
        }
        
        #endregion
        
        #region ToString
        public override string ToString()
        {
            return $"[Container: {Name} (Slots: {NumSlots}, Free: {FreeSlotsCount})]";
        }
        
        #endregion
    }
}
