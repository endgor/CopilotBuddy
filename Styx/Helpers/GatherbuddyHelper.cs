using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Styx.Helpers
{
    /// <summary>
    /// Helper used by GatherBuddy to list and load GatherBuddy profile files.
    ///
    /// This is based on Honorbuddy 4.3.4's GatherbuddyHelper, but adapted to
    /// CopilotBuddy's local profile storage (Profiles/GatherBuddy/).
    /// </summary>
    public static class GatherbuddyHelper
    {
        private const int ProfileListCacheMinutes = 20;

        private static readonly object SyncLock = new object();
        private static DateTime _lastProfileListRefresh = DateTime.MinValue;
        private static string[]? _cachedProfileNames;
        private static readonly Dictionary<string, string> CachedProfiles = new(StringComparer.OrdinalIgnoreCase);

        private static string GetProfileDirectory()
        {
            return Path.Combine(Logging.ApplicationPath, "Profiles", "GatherBuddy");
        }

        /// <summary>
        /// Returns the list of available GatherBuddy profile names (without extension).
        /// </summary>
        public static string[]? GetProfileList()
        {
            try
            {
                lock (SyncLock)
                {
                    if (_cachedProfileNames == null || (DateTime.Now - _lastProfileListRefresh).TotalMinutes > ProfileListCacheMinutes)
                    {
                        var profileDir = GetProfileDirectory();
                        if (!Directory.Exists(profileDir))
                        {
                            _cachedProfileNames = Array.Empty<string>();
                        }
                        else
                        {
                            _cachedProfileNames = Directory
                                .GetFiles(profileDir, "*.xml")
                                .Select(Path.GetFileNameWithoutExtension)
                                .ToArray();
                        }

                        _lastProfileListRefresh = DateTime.Now;
                    }

                    return _cachedProfileNames;
                }
            }
            catch (Exception ex)
            {
                Logging.Write($"Error getting Gatherbuddy profiles! {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Returns the content of a GatherBuddy profile XML file.
        /// </summary>
        /// <param name="profileName">Profile name (file name without extension) or full path.</param>
        public static string? GetProfile(string profileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(profileName))
                    return null;

                lock (SyncLock)
                {
                    if (CachedProfiles.TryGetValue(profileName, out var content))
                        return content;

                    var profilePath = profileName;
                    if (!Path.IsPathRooted(profilePath))
                    {
                        profilePath = Path.Combine(GetProfileDirectory(), profileName + ".xml");
                    }

                    if (!File.Exists(profilePath))
                        return null;

                    content = File.ReadAllText(profilePath, Encoding.UTF8);
                    CachedProfiles[profileName] = content;
                    return content;
                }
            }
            catch (Exception ex)
            {
                Logging.Write($"Error getting Gatherbuddy profile '{profileName}'! {ex.Message}");
                return null;
            }
        }
    }
}
