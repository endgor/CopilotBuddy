using System.Collections.Generic;
using System.Linq;
using Bots.DungeonBuddy.Enums;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Bots.DungeonBuddy.Profiles.Handlers
{
    public class Boss
    {
        private CircularQueue<WoWPoint> _pathBreadCrumbs;

        public Boss()
        {
            IsAlive = true;
            Path = new List<WoWPoint>();
            Name = string.Empty;
            Faction = BossAvailableToFaction.Both;
        }

        public CircularQueue<WoWPoint> PathBreadCrumbs
        {
            get
            {
                if (_pathBreadCrumbs == null)
                {
                    _pathBreadCrumbs = new CircularQueue<WoWPoint>();
                    if (Path.Count > 0)
                    {
                        foreach (WoWPoint point in Path)
                            _pathBreadCrumbs.Add(point);
                    }
                    else
                    {
                        _pathBreadCrumbs.Add(Location);
                    }
                }

                return _pathBreadCrumbs;
            }
        }

        public bool IsAlive { get; private set; }

        public void MarkAsDead()
        {
            IsAlive = false;
        }

        public void Reset()
        {
            IsAlive = true;
        }

        public WoWUnit ToWoWUnit()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == Entry);
        }

        public bool IsFinal { get; set; }
        public uint Entry { get; set; }
        public int KillOrder { get; set; }
        public string Name { get; set; }
        public bool Optional { get; set; }
        public BossAvailableToFaction Faction { get; set; }
        public List<WoWPoint> Path { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public WoWPoint Location => new WoWPoint(X, Y, Z);
    }
}