namespace Styx.Logic
{
    public class StrandOfTheAncientsLandmark : WoWLandMark
    {
        public StrandOfTheAncientsLandmark(uint ptr)
            : base(ptr)
        {
        }

        public StrandOfTheAncientsLandmarkType LandmarkType => GetLandmarkType(this);

        public LandmarkControlType ControlType
        {
            get
            {
                switch (base.WorldState)
                {
                    case 2U: return LandmarkControlType.InConflict;
                    case 3U: case 6U: return LandmarkControlType.Destroyed;
                }
                switch (base.NormalIcon)
                {
                    case 13: return LandmarkControlType.HordeControlled;
                    case 15: return LandmarkControlType.AllianceControlled;
                    default: return LandmarkControlType.Unknown;
                }
            }
        }

        private static StrandOfTheAncientsLandmarkType GetLandmarkType(WoWLandMark lm)
        {
            switch (lm.Entry)
            {
                case 2111U: return StrandOfTheAncientsLandmarkType.GateOfThePurpleAmethyst;
                case 2114U: return StrandOfTheAncientsLandmarkType.GateOfTheRedSun;
                case 2117U: return StrandOfTheAncientsLandmarkType.GateOfTheBlueSapphire;
                case 2120U: return StrandOfTheAncientsLandmarkType.GateOfTheGreenEmerald;
                case 2127U: return StrandOfTheAncientsLandmarkType.HordeDefense;
                case 2128U: return StrandOfTheAncientsLandmarkType.AllianceDefense;
                case 2129U: case 2130U: return StrandOfTheAncientsLandmarkType.EastGraveyard;
                case 2132U: case 2324U: return StrandOfTheAncientsLandmarkType.WestGraveyard;
                case 2133U: case 2134U: return StrandOfTheAncientsLandmarkType.SouthGraveyard;
                case 2135U: return StrandOfTheAncientsLandmarkType.GateOfTheYellowMoon;
                case 2292U: return StrandOfTheAncientsLandmarkType.ChamberOfAncientRelics;
                default: return StrandOfTheAncientsLandmarkType.Unknown;
            }
        }
    }
}
