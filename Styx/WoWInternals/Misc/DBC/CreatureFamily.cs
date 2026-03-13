using System.Runtime.InteropServices;
using Styx.Patchables;
using Styx.WoWInternals.Misc.DBC;

namespace Styx.WoWInternals.Misc.DBC
{
    /// <summary>
    /// Represents a row from CreatureFamily.dbc.
    /// Ported from HB 4.3.4 CreatureFamily.
    /// </summary>
    public class CreatureFamily
    {
        private readonly WoWDb.Row _row;
        private readonly CreatureFamilyRecord _record;

        internal CreatureFamily(uint creatureFamilyEntry)
        {
            _row = StyxWoW.Db![ClientDb.CreatureFamily].GetRow(creatureFamilyEntry);
            if (_row.IsValid)
                _record = _row.GetStruct<CreatureFamilyRecord>();
        }

        public bool IsValid => _row.IsValid;

        public uint Id => _record.Id;

        public float MinScale => _record.MinScale;

        public int MinScaleLevel => _record.MinScaleLevel;

        public float MaxScale => _record.MaxScale;

        public int MaxScaleLevel => _record.MaxScaleLevel;

        public PetFoodFlags Diet => _record.Diet;

        public int PetTalentType => _record.PetTalentType;

        public string Name => ObjectManager.Wow!.Read<string>(new uint[] { _record.NamePtr });

        public override string ToString()
        {
            return string.Format("[{0}] Id:{1} MinScale:{2} MinScaleLevel:{3} MaxScale:{4} MaxScaleLevel:{5} Diet:{6} PetTalentType:{7}",
                Name, Id, MinScale, MinScaleLevel, MaxScale, MaxScaleLevel, Diet, PetTalentType);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CreatureFamilyRecord
        {
            public readonly uint    Id;
            public readonly float   MinScale;
            public readonly int     MinScaleLevel;
            public readonly float   MaxScale;
            public readonly int     MaxScaleLevel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public readonly uint[]  SkillLineIds;
            public readonly PetFoodFlags Diet;
            public readonly int     PetTalentType;
            private readonly int    _unused;
            public readonly uint    NamePtr;
        }
    }
}
