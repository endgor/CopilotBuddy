namespace Styx.Logic
{
    public class EyeOfTheStormLandmark : WoWLandMark
    {
        public EyeOfTheStormLandmark(uint ptr)
            : base(ptr)
        {
        }

        public EyeOfTheStormLandmarkType LandmarkType => GetLandmarkType(this);

        public LandmarkControlType ControlType
        {
            get
            {
                switch (base.NormalIcon)
                {
                    case 6:  return LandmarkControlType.Uncontrolled;
                    case 9:
                    case 12: return LandmarkControlType.InConflict;
                    case 10: return LandmarkControlType.HordeControlled;
                    case 11: return LandmarkControlType.AllianceControlled;
                    default: return LandmarkControlType.Unknown;
                }
            }
        }

        private static EyeOfTheStormLandmarkType GetLandmarkType(WoWLandMark lm)
        {
            switch (lm.Entry)
            {
                case 1941U: case 1942U: case 1943U: case 1959U: case 1960U:
                    return EyeOfTheStormLandmarkType.BloodElfTower;
                case 1944U: case 1945U: case 1946U: case 1957U:
                    return EyeOfTheStormLandmarkType.FelReaverRuins;
                case 1947U: case 1948U: case 1949U: case 1955U: case 1956U:
                    return EyeOfTheStormLandmarkType.MageTower;
                case 1950U: case 1951U: case 1952U: case 1953U: case 1954U:
                    return EyeOfTheStormLandmarkType.DraeneiRuins;
                case 1958U: case 9158U:
                default:
                    return EyeOfTheStormLandmarkType.Unknown;
            }
        }
    }
}
