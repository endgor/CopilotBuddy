namespace Styx.Logic
{
    public class IsleOfConquestLandmark : WoWLandMark
    {
        public IsleOfConquestLandmark(uint ptr)
            : base(ptr)
        {
        }

        public IsleOfConquestLandmarkType LandmarkType => GetLandmarkType(this);

        public LandmarkControlType ControlType
        {
            get
            {
                switch (base.NormalIcon)
                {
                    case 6: case 16:
                    case 135: case 140: case 145: case 150:
                        return LandmarkControlType.Uncontrolled;

                    case 9: case 12: case 17: case 19:
                    case 137: case 139: case 142: case 144:
                    case 147: case 149: case 152: case 154:
                        return LandmarkControlType.InConflict;

                    case 10: case 20: case 77:
                    case 138: case 143: case 148: case 153:
                        return LandmarkControlType.HordeControlled;

                    case 11: case 18: case 80:
                    case 136: case 141: case 146: case 151:
                        return LandmarkControlType.AllianceControlled;

                    case 79: case 82:
                        return LandmarkControlType.Destroyed;

                    default:
                        return LandmarkControlType.Unknown;
                }
            }
        }

        private static IsleOfConquestLandmarkType GetLandmarkType(WoWLandMark lm)
        {
            switch (lm.Entry)
            {
                case 2345U: case 2346U: case 2347U: case 2348U: case 2349U:
                    return IsleOfConquestLandmarkType.Workshop;
                case 2350U: case 2352U: case 2353U: case 2354U: case 2355U:
                    return IsleOfConquestLandmarkType.Hangar;
                case 2356U: case 2357U: case 2358U: case 2359U: case 2360U:
                    return IsleOfConquestLandmarkType.Docks;
                case 2361U: case 2362U: case 2363U: case 2364U: case 2365U:
                    return IsleOfConquestLandmarkType.Quarry;
                case 2366U: case 2367U: case 2368U: case 2369U: case 2370U:
                    return IsleOfConquestLandmarkType.Refinery;
                case 2371U: case 2372U:
                    return IsleOfConquestLandmarkType.HordeGateFront;
                case 2373U: case 2374U:
                    return IsleOfConquestLandmarkType.HordeGateEast;
                case 2375U: case 2376U:
                    return IsleOfConquestLandmarkType.HordeGateWest;
                case 2377U: case 2379U:
                    return IsleOfConquestLandmarkType.AllianceGateEast;
                case 2378U: case 2382U:
                    return IsleOfConquestLandmarkType.AllianceGateFront;
                case 2380U: case 2381U:
                    return IsleOfConquestLandmarkType.AllianceGateWest;
                case 2383U: case 2384U: case 2385U: case 2386U: case 2387U:
                    return IsleOfConquestLandmarkType.AllianceKeep;
                case 2388U: case 2389U: case 2390U: case 2391U: case 2392U:
                    return IsleOfConquestLandmarkType.HordeKeep;
                default:
                    return IsleOfConquestLandmarkType.Unknown;
            }
        }
    }
}
