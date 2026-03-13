namespace Styx.Logic
{
    public class AlteracValleyLandmark : WoWLandMark
    {
        public AlteracValleyLandmark(uint ptr)
            : base(ptr)
        {
        }

        public AlteracValleyLandmarkType LandmarkType => GetLandmarkType(this);

        public LandmarkControlType ControlType
        {
            get
            {
                switch (base.NormalIcon)
                {
                    case 1: case 7: case 8:
                        return LandmarkControlType.Uncontrolled;
                    case 2: case 10: case 13:
                        return LandmarkControlType.HordeControlled;
                    case 3: case 11: case 15:
                        return LandmarkControlType.AllianceControlled;
                    case 4: case 9: case 12: case 14:
                        return LandmarkControlType.InConflict;
                    case 6:
                        return LandmarkControlType.Destroyed;
                    default:
                        return LandmarkControlType.Unknown;
                }
            }
        }

        private static AlteracValleyLandmarkType GetLandmarkType(WoWLandMark lm)
        {
            switch (lm.Entry)
            {
                case 1099U: return AlteracValleyLandmarkType.IrondeepMine;
                case 1100U: return AlteracValleyLandmarkType.StonehearthOutpost;
                case 1101U: return AlteracValleyLandmarkType.IcebloodGarrison;
                case 1102U: return AlteracValleyLandmarkType.ColdtoothMine;
                case 1103U: return AlteracValleyLandmarkType.FrostwolfKeep;
                case 1208U: return AlteracValleyLandmarkType.StormpikeGraveyard;
                case 1209U: return AlteracValleyLandmarkType.SnowfallGraveyard;
                case 1210U: return AlteracValleyLandmarkType.FrostwolfGraveyard;
                case 1249U: return AlteracValleyLandmarkType.DunBaldarSouthBunker;
                case 1250U: return AlteracValleyLandmarkType.DunBaldarNorthBunker;
                case 1251U: return AlteracValleyLandmarkType.IcewingBunker;
                case 1252U: return AlteracValleyLandmarkType.IcebloodTower;
                case 1254U: return AlteracValleyLandmarkType.TowerPoint;
                case 1255U: return AlteracValleyLandmarkType.EastFrostwolfTower;
                case 1328U: return AlteracValleyLandmarkType.DunBaldar;
                case 1347U: case 1389U: case 1390U: case 1391U:
                    return AlteracValleyLandmarkType.StonehearthBunker;
                case 1348U: case 1395U: case 1396U: case 1397U:
                    return AlteracValleyLandmarkType.StormpikeAidStation;
                case 1349U: case 1374U: case 1375U: case 1376U:
                    return AlteracValleyLandmarkType.IcebloodGraveyard;
                case 1350U: case 1392U: case 1393U: case 1394U:
                    return AlteracValleyLandmarkType.StonehearthGraveyard;
                case 1351U: case 1371U: case 1372U: case 1373U:
                    return AlteracValleyLandmarkType.FrostwolfReliefHut;
                case 1352U: case 1353U: case 1354U:
                    return AlteracValleyLandmarkType.DunBaldarNorthBunker;
                case 1355U: case 1356U: case 1357U:
                    return AlteracValleyLandmarkType.DunBaldarSouthBunker;
                case 1358U: case 1359U:
                    return AlteracValleyLandmarkType.ColdtoothMine;
                case 1362U: case 1363U: case 1364U:
                    return AlteracValleyLandmarkType.EastFrostwolfTower;
                case 1368U: case 1369U: case 1370U:
                    return AlteracValleyLandmarkType.FrostwolfGraveyard;
                case 1377U: case 1378U: case 1379U:
                    return AlteracValleyLandmarkType.IcebloodTower;
                case 1380U: case 1381U: case 1382U:
                    return AlteracValleyLandmarkType.IcewingBunker;
                case 1383U: case 1384U:
                    return AlteracValleyLandmarkType.IrondeepMine;
                case 1386U: case 1387U: case 1388U:
                    return AlteracValleyLandmarkType.SnowfallGraveyard;
                case 1398U: case 1399U: case 1400U:
                    return AlteracValleyLandmarkType.StormpikeGraveyard;
                case 1405U: case 1406U: case 1407U:
                    return AlteracValleyLandmarkType.TowerPoint;
                case 1527U: case 1528U: case 1529U: case 1530U:
                    return AlteracValleyLandmarkType.WestFrostwolfTower;
                case 1568U: return AlteracValleyLandmarkType.WildpawCavern;
                case 1569U: return AlteracValleyLandmarkType.IcewingCavern;
                case 1682U: return AlteracValleyLandmarkType.SnowfallGraveyard;
                default:    return AlteracValleyLandmarkType.Unknown;
            }
        }
    }
}
