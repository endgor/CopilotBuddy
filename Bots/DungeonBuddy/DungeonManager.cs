using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bots.DungeonBuddy.Attributes;
using Bots.DungeonBuddy.Profiles;
using Styx;
using Styx.Helpers;
using Styx.Loaders;
using TreeSharp;

namespace Bots.DungeonBuddy
{
    /// <summary>
    /// Charge et gère les scripts de donjons.
    /// Scripts dynamiques depuis Dungeon Scripts\ + types compilés dans l'assembly.
    /// </summary>
    public static class DungeonManager
    {
        private static readonly Dictionary<uint, Type> _dungeonTypes = new();
        private static readonly Dictionary<uint, Dictionary<int, MethodInfo>> _encounterHandlers = new();
        private static readonly Dictionary<uint, Dictionary<int, MethodInfo>> _objectHandlers = new();
        
        private static Dungeon _currentDungeon;

        /// <summary>
        /// Donjon actif
        /// </summary>
        public static Dungeon CurrentDungeon => _currentDungeon;

        /// <summary>
        /// Charge les scripts de donjon depuis Dungeon Scripts\ (compilation dynamique, pattern HB 4.3.4),
        /// plus les scripts compilés dans l'assembly.
        /// </summary>
        public static void LoadDungeonScripts()
        {
            _dungeonTypes.Clear();
            _encounterHandlers.Clear();
            _objectHandlers.Clear();

            // 1. Dynamic loading from Dungeon Scripts\ folder (HB 4.3.4 pattern)
            // HB: string text = Path.Combine(Application.StartupPath, "Dungeon Scripts");
            string scriptDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon Scripts");
            if (Directory.Exists(scriptDir))
            {
                Logging.Write("[DungeonBuddy] Compiling dungeon scripts...");
                int errors = 0;

                var scriptPaths = new List<string>();
                scriptPaths.AddRange(Directory.GetFiles(scriptDir, "*.cs", SearchOption.TopDirectoryOnly));
                scriptPaths.AddRange(Directory.GetDirectories(scriptDir, "*", SearchOption.TopDirectoryOnly));

                foreach (string path in scriptPaths)
                {
                    try
                    {
                        var loader = new DynamicLoader<Dungeon>(path, true);
                        if (loader.CompilerResults != null &&
                            loader.CompilerResults.Errors.Cast<CompilerError>().Any(e => !e.IsWarning))
                        {
                            errors++;
                            Logging.Write("[DungeonBuddy] Could not compile dungeon script from {0}", path);
                        }
                        else
                        {
                            foreach (var dungeon in loader)
                                RegisterInstance(dungeon);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteDiagnostic("[DungeonBuddy] Error loading {0}: {1}", path, ex.Message);
                        errors++;
                    }
                }

                if (errors > 0)
                    Logging.Write("[DungeonBuddy] Compiling failed for {0} dungeon scripts. Check log for details.", errors);
            }

            // 2. Also scan the compiled assembly for any built-in scripts
            LoadDungeonTypes();

            if (_dungeonTypes.Count == 0)
                Logging.Write("[DungeonBuddy] No dungeon scripts found.");
            else
                Logging.Write("[DungeonBuddy] Loaded {0} dungeon scripts.", _dungeonTypes.Count);
        }

        /// <summary>
        /// Registers a dungeon instance: stores its Type and indexes its handlers.
        /// The instance is disposed after registration.
        /// </summary>
        private static void RegisterInstance(Dungeon dungeon)
        {
            var dungeonId = dungeon.DungeonId;
            if (!_dungeonTypes.ContainsKey(dungeonId))
            {
                _dungeonTypes[dungeonId] = dungeon.GetType();
                IndexHandlers(dungeon.GetType(), dungeonId);
            }
            dungeon.Dispose();
        }

        /// <summary>
        /// Scanne l'assembly compilée pour les types héritant de Dungeon.
        /// </summary>
        private static void LoadDungeonTypes()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var dungeonTypes = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Dungeon)) && !t.IsAbstract);

                foreach (var type in dungeonTypes)
                {
                    try
                    {
                        var instance = (Dungeon)Activator.CreateInstance(type);
                        var dungeonId = instance.DungeonId;
                        instance.Dispose();

                        if (!_dungeonTypes.ContainsKey(dungeonId))
                        {
                            _dungeonTypes[dungeonId] = type;
                            IndexHandlers(type, dungeonId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteDiagnostic($"[DungeonBuddy] Error loading {type.Name}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic($"[DungeonBuddy] Error scanning dungeon types: {ex.Message}");
            }
        }

        private static void IndexHandlers(Type dungeonType, uint dungeonId)
        {
            _encounterHandlers[dungeonId] = new Dictionary<int, MethodInfo>();
            _objectHandlers[dungeonId] = new Dictionary<int, MethodInfo>();

            foreach (var method in dungeonType.GetMethods())
            {
                // Index encounter handlers
                var encounterAttrs = method.GetCustomAttributes<EncounterHandlerAttribute>();
                foreach (var attr in encounterAttrs)
                {
                    _encounterHandlers[dungeonId][attr.BossEntry] = method;
                    BossManager.RegisterBoss((uint)attr.BossEntry, attr.BossName);
                }

                // Index object handlers
                var objectAttrs = method.GetCustomAttributes<ObjectHandlerAttribute>();
                foreach (var attr in objectAttrs)
                {
                    _objectHandlers[dungeonId][attr.ObjectEntry] = method;
                }
            }
        }

        /// <summary>
        /// Active le script de donjon approprié
        /// </summary>
        public static void SetDungeon(uint mapId)
        {
            // Détacher l'ancien donjon
            _currentDungeon?.Detach();
            _currentDungeon = null;

            // Trouver le type de donjon correspondant au MapId
            Type matchedType = null;
            uint matchedDungeonId = 0;
            foreach (var kvp in _dungeonTypes)
            {
                var dungeonId = kvp.Key;
                if (dungeonId == mapId || GetMapIdForDungeon(dungeonId) == mapId)
                {
                    matchedType = kvp.Value;
                    matchedDungeonId = dungeonId;
                    break;
                }
            }

            if (matchedType != null)
            {
                _currentDungeon = (Dungeon)Activator.CreateInstance(matchedType);
                _currentDungeon.Attach();
                BossManager.Initialize(_currentDungeon);
                ProfileManager.LoadProfileForDungeon(matchedDungeonId);
                Logging.Write($"[DungeonBuddy] Activated script: {_currentDungeon.Name}");
            }
            else
            {
                Logging.Write($"[DungeonBuddy] No script found for map {mapId}");
            }
        }

        /// <summary>
        /// Obtient le behavior pour un boss spécifique
        /// </summary>
        public static Composite GetEncounterBehavior(int bossEntryId)
        {
            if (_currentDungeon == null)
                return null;

            var dungeonId = _currentDungeon.DungeonId;
            
            if (_encounterHandlers.TryGetValue(dungeonId, out var handlers) &&
                handlers.TryGetValue(bossEntryId, out var method))
            {
                return method.Invoke(_currentDungeon, null) as Composite;
            }

            return null;
        }

        /// <summary>
        /// Obtient le behavior pour un objet spécifique
        /// </summary>
        public static Composite GetObjectBehavior(int objectEntryId)
        {
            if (_currentDungeon == null)
                return null;

            var dungeonId = _currentDungeon.DungeonId;
            
            if (_objectHandlers.TryGetValue(dungeonId, out var handlers) &&
                handlers.TryGetValue(objectEntryId, out var method))
            {
                return method.Invoke(_currentDungeon, null) as Composite;
            }

            return null;
        }

        private static uint GetMapIdForDungeon(uint lfgDungeonId)
        {
            // Mapping LFG DungeonId → MapId pour WotLK
            // Ces IDs viennent de LFG_Dungeons.dbc
            // Note: Les IDs normaux et héroïques mappent vers le même MapId
            // DungeonIds vérifiés depuis les 32 scripts dans Dungeon Scripts\Wrath of the Lich King\
            return lfgDungeonId switch
            {
                202 => 574,  // Utgarde Keep (Normal)
                242 => 574,  // Utgarde Keep (Heroic)
                203 => 575,  // Utgarde Pinnacle (Normal)
                205 => 575,  // Utgarde Pinnacle (Heroic)
                204 => 601,  // Azjol-Nerub (Normal)
                241 => 601,  // Azjol-Nerub (Heroic)
                206 => 578,  // The Oculus (Normal)
                211 => 578,  // The Oculus (Heroic)
                207 => 602,  // Halls of Lightning (Normal)
                212 => 602,  // Halls of Lightning (Heroic)
                208 => 599,  // Halls of Stone (Normal)
                213 => 599,  // Halls of Stone (Heroic)
                209 => 595,  // Culling of Stratholme (Normal)
                210 => 595,  // Culling of Stratholme (Heroic)
                214 => 600,  // Drak'Tharon Keep (Normal)
                215 => 600,  // Drak'Tharon Keep (Heroic)
                216 => 604,  // Gundrak (Normal)
                217 => 604,  // Gundrak (Heroic)
                218 => 619,  // Ahn'kahet (Normal)
                219 => 619,  // Ahn'kahet (Heroic)
                220 => 608,  // Violet Hold (Normal)
                221 => 608,  // Violet Hold (Heroic)
                225 => 576,  // The Nexus (Normal)
                226 => 576,  // The Nexus (Heroic)
                245 => 650,  // Trial of the Champion (Normal)
                249 => 650,  // Trial of the Champion (Heroic)
                251 => 632,  // Forge of Souls (Normal)
                252 => 632,  // Forge of Souls (Heroic)
                253 => 658,  // Pit of Saron (Normal)
                254 => 658,  // Pit of Saron (Heroic)
                255 => 668,  // Halls of Reflection (Normal)
                256 => 668,  // Halls of Reflection (Heroic)
                _ => 0
            };
        }

        public static void Clear()
        {
            _currentDungeon?.Detach();
            _currentDungeon = null;
            ProfileManager.UnloadProfile();
        }

        /// <summary>
        /// Reload dungeon scripts at runtime (HB 4.3.4: DungeonManager.ReloadDungeons()).
        /// </summary>
        public static void ReloadDungeons()
        {
            Logging.Write("[DungeonBuddy] Reloading dungeon scripts...");
            _currentDungeon?.Detach();
            _currentDungeon = null;
            LoadDungeonScripts();
        }
    }
}