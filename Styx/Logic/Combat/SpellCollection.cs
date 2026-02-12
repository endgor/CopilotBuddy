using System.Collections.Generic;

namespace Styx.Logic.Combat
{
    /// <summary>
    /// FEAT-17: Typed wrapper for a player's known spells.
    /// Extends Dictionary for name-based lookup, matching HB 4.3.4 API.
    /// </summary>
    public class SpellCollection : Dictionary<string, WoWSpell>
    {
        public SpellCollection() : base() { }
        public SpellCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Gets a spell by name. Throws KeyNotFoundException if not known.
        /// </summary>
        public new WoWSpell this[string key]
        {
            get
            {
                if (TryGetValue(key, out WoWSpell? spell))
                    return spell;
                throw new KeyNotFoundException($"Spell '{key}' is not in the known spells collection.");
            }
            set => base[key] = value;
        }

        /// <summary>
        /// Whether the player knows a spell with this name.
        /// </summary>
        public bool HasSpell(string name) => ContainsKey(name);

        /// <summary>
        /// Whether the player knows a spell with this ID.
        /// </summary>
        public bool HasSpell(int spellId)
        {
            foreach (var kvp in this)
            {
                if (kvp.Value.Id == spellId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a spell by ID, or null if not known.
        /// </summary>
        public WoWSpell? GetById(int spellId)
        {
            foreach (var kvp in this)
            {
                if (kvp.Value.Id == spellId)
                    return kvp.Value;
            }
            return null;
        }
    }
}
