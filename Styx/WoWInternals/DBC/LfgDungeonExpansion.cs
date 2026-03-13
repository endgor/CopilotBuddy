using System.Runtime.InteropServices;
using Styx.Patchables;

namespace Styx.WoWInternals.DBC
{
    /// <summary>
    /// Represents a row from LfgDungeonExpansion.dbc.
    /// Ported from HB 4.3.4 LfgDungeonExpansion.
    /// </summary>
    public class LfgDungeonExpansion
    {
        private readonly WoWDb.Row _row;
        private readonly LfgDungeonExpansionRecord _record;

        public LfgDungeonExpansion(uint id)
        {
            _row = StyxWoW.Db![ClientDb.LfgDungeonExpansion].GetRow(id);
            if (_row != null && _row.IsValid)
                _record = _row.GetStruct<LfgDungeonExpansionRecord>();
        }

        public bool IsValid => _row != null && _row.IsValid;

        public uint Id => _record.Id;

        public uint LfgId => _record.LfgId;

        public uint ExpansionLevel => _record.ExpansionLevel;

        public uint RandomId => _record.RandomId;

        public uint HardLevelMin => _record.HardLevelMin;

        public uint HardLevelMax => _record.HardLevelMax;

        public uint TargetLevelMin => _record.TargetLevelMin;

        public uint TargetLevelMax => _record.TargetLevelMax;

        [StructLayout(LayoutKind.Sequential)]
        private struct LfgDungeonExpansionRecord
        {
            public uint Id;
            public uint LfgId;
            public uint ExpansionLevel;
            public uint RandomId;
            public uint HardLevelMin;
            public uint HardLevelMax;
            public uint TargetLevelMin;
            public uint TargetLevelMax;
        }
    }
}
