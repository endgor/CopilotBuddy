using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Bots.DungeonBuddy.Helpers;
using Bots.DungeonBuddy.Profiles.Handlers;
using Styx.Logic.Pathing;

namespace Bots.DungeonBuddy.Profiles
{
    public class Profile
    {
        private Profile(XElement element, string path = null)
        {
            Element = element;
            Path = path;
            Name = string.Empty;
            Blackspots = new List<Blackspot>();
            PullBlackspots = new List<PullBlackspot>();
            BossEncounters = new List<Boss>();
            HotSpots = new List<WoWPoint>();
        }

        public string Path { get; private set; }

        public XElement Element { get; private set; }

        public string Name { get; private set; }

        public List<Blackspot> Blackspots { get; private set; }

        public List<PullBlackspot> PullBlackspots { get; private set; }

        public uint DungeonId { get; private set; }

        public List<Boss> BossEncounters { get; private set; }

        // Compatibility for existing movement logic while Profile/Boss pathing is stabilized.
        public List<WoWPoint> HotSpots { get; private set; }

        public static Profile Load(string path, out ErrorCollection errors)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Path to profile element is not valid; file not found", path);

            XElement element = XElement.Load(path, LoadOptions.SetLineInfo);
            Profile profile = new Profile(element, path);
            profile.Parse(out errors);
            return profile;
        }

        public static Profile Load(XElement element, out ErrorCollection errors)
        {
            Profile profile = new Profile(element, null);
            profile.Parse(out errors);
            return profile;
        }

        private void Parse(out ErrorCollection errors)
        {
            errors = new ErrorCollection();

            Name = Element.Element("Name")?.Value ?? string.Empty;

            XElement dungeonIdElement = Element.Element("DungeonId");
            if (dungeonIdElement == null || !uint.TryParse(dungeonIdElement.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint dungeonId))
            {
                errors.Add(new Error("Missing or invalid DungeonId element", ErrorType.Error));
            }
            else
            {
                DungeonId = dungeonId;
            }

            Blackspots = ParseBlackspots(Element.Element("Blackspots"));
            PullBlackspots = ParsePullBlackspots(Element.Element("PullBlackspots"));
            BossEncounters = ParseBosses(Element.Element("BossEncounters"));
            HotSpots = ParseHotspots(Element.Element("HotSpots") ?? Element.Element("Hotspots"));
        }

        private static List<Blackspot> ParseBlackspots(XElement root)
        {
            var list = new List<Blackspot>();
            if (root == null)
                return list;

            foreach (XElement element in root.Elements("Blackspot"))
            {
                float x = ParseFloat(element, "x", "X");
                float y = ParseFloat(element, "y", "Y");
                float z = ParseFloat(element, "z", "Z");
                float radius = ParseFloat(element, "radius", "Radius");
                float height = ParseFloat(element, "height", "Height");
                list.Add(new Blackspot
                {
                    X = x,
                    Y = y,
                    Z = z,
                    Radius = radius,
                    Height = height
                });
            }

            return list;
        }

        private static List<PullBlackspot> ParsePullBlackspots(XElement root)
        {
            var list = new List<PullBlackspot>();
            if (root == null)
                return list;

            foreach (XElement element in root.Elements("PullBlackspot"))
            {
                list.Add(new PullBlackspot
                {
                    X = ParseFloat(element, "x", "X"),
                    Y = ParseFloat(element, "y", "Y"),
                    Z = ParseFloat(element, "z", "Z"),
                    Radius = ParseFloat(element, "radius", "Radius"),
                    Height = ParseFloat(element, "height", "Height")
                });
            }

            return list;
        }

        private static List<Boss> ParseBosses(XElement root)
        {
            var list = new List<Boss>();
            if (root == null)
                return list;

            foreach (XElement element in root.Elements("Boss"))
            {
                var boss = new Boss
                {
                    IsFinal = ParseBool(element, "isFinal", "IsFinal"),
                    Entry = ParseUInt(element, "entry", "Entry"),
                    KillOrder = ParseInt(element, "killOrder", "KillOrder"),
                    Name = ParseString(element, "name", "Name"),
                    Optional = ParseBool(element, "optional", "Optional"),
                    Faction = ParseEnum(element, "faction", "Faction"),
                    X = ParseFloat(element, "x", "X"),
                    Y = ParseFloat(element, "y", "Y"),
                    Z = ParseFloat(element, "z", "Z")
                };

                XElement pathElement = element.Element("Path");
                if (pathElement != null)
                {
                    foreach (XElement hotspot in pathElement.Elements("Hotspot"))
                    {
                        boss.Path.Add(new WoWPoint(
                            ParseFloat(hotspot, "x", "X"),
                            ParseFloat(hotspot, "y", "Y"),
                            ParseFloat(hotspot, "z", "Z")));
                    }
                }

                list.Add(boss);
            }

            return list;
        }

        private static List<WoWPoint> ParseHotspots(XElement root)
        {
            var list = new List<WoWPoint>();
            if (root == null)
                return list;

            foreach (XElement element in root.Elements("Hotspot"))
            {
                list.Add(new WoWPoint(
                    ParseFloat(element, "x", "X"),
                    ParseFloat(element, "y", "Y"),
                    ParseFloat(element, "z", "Z")));
            }

            return list;
        }

        private static float ParseFloat(XElement element, string lower, string upper)
        {
            string value = ParseString(element, lower, upper);
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                return result;

            return 0f;
        }

        private static uint ParseUInt(XElement element, string lower, string upper)
        {
            string value = ParseString(element, lower, upper);
            if (uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint result))
                return result;

            return 0;
        }

        private static int ParseInt(XElement element, string lower, string upper)
        {
            string value = ParseString(element, lower, upper);
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                return result;

            return 0;
        }

        private static bool ParseBool(XElement element, string lower, string upper)
        {
            string value = ParseString(element, lower, upper);
            if (bool.TryParse(value, out bool result))
                return result;

            return false;
        }

        private static Enums.BossAvailableToFaction ParseEnum(XElement element, string lower, string upper)
        {
            string value = ParseString(element, lower, upper);
            if (Enum.TryParse(value, true, out Enums.BossAvailableToFaction faction))
                return faction;

            return Enums.BossAvailableToFaction.Both;
        }

        private static string ParseString(XElement element, string lower, string upper)
        {
            XAttribute attribute = element.Attribute(lower) ?? element.Attribute(upper);
            return attribute?.Value ?? string.Empty;
        }
    }
}