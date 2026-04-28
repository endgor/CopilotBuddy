using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bots.DungeonBuddy.Avoidance;
using Bots.DungeonBuddy.Enums;
using Bots.Grind;
using CommonBehaviors.Actions;
using CommonBehaviors.Decorators;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Bots.DungeonBuddy
{
    /// <summary>
    /// DungeonBuddy - BotBase pour Dungeon Finder automatique
    /// WotLK 3.3.5a (patch 3.3 — Dungeon Finder ajouté)
    /// 
    /// State machine:
    ///   NotInLfg → SetRole + Queue → InQueue → Proposal → Accept → InDungeon
    ///   InDungeon → Combat/Follow → DungeonComplete → TeleportOut → Requeue
    ///   
    /// LFG state détecté via GetLFGMode() (API canonique WotLK 3.3).
    /// Events LFG via Lua.Events.AttachEvent (confirmé dans CopilotBuddy).
    /// </summary>
    public class DungeonBuddy : BotBase
    {
        public override string Name => "DungeonBuddy";
        public override bool IsPrimaryType => true;
        public override bool RequiresProfile => false;
        public override bool RequirementsMet => true;
        public override PulseFlags PulseFlags => PulseFlags.All;

        /// <summary>
        /// Return the configuration window used by DungeonBuddy.
        /// This allows the main UI's "Bot Config" button to work when
        /// DungeonBuddy is selected (previously ConfigurationForm was null).
        /// </summary>
        public override object ConfigurationForm => new Forms.FormConfig();

        private PrioritySelector? _root;
        private static CombatRoutine Routine => RoutineManager.Current;
        
        // Timers
        private readonly Stopwatch _proposalDelay = new();
        private readonly Stopwatch _requeueDelay = new();
        private readonly Random _rng = new();
        private int _proposalWaitMs;  // Délai aléatoire avant AcceptProposal

        // State tracking
        private uint _lastMapId;
        private bool _hasSetRole;

        // Active dungeon behavior — updated when dungeon changes (fixes stale reference at tree-build time)
        private Composite? _activeDungeonBehavior;
        private readonly WaitTimer _soloFarmExitTimer = new WaitTimer(TimeSpan.FromSeconds(30.0));
        private readonly Stopwatch _debugMoveLogThrottle = new();
        private readonly Stopwatch _debugDungeonLogThrottle = new();
        private readonly Stopwatch _soloFarmStatusLogThrottle = new();
        private bool _soloFarmResetInstancesPending;
        private WoWPoint _outsideFlyPoint = WoWPoint.Zero;
        private readonly List<uint> _healthstoneEntries = new List<uint> { 51999U, 52000U, 52001U, 52002U, 52003U, 52004U, 52005U, 67248U, 67250U };
        private readonly uint[] _mageTableEntries = { 186812U, 207386U, 207387U };

        public override Composite Root => _root ??= CreateRootBehavior();

        // ═══════════════════════════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════════════════════════

        public override void Start()
        {
            Logging.Write("[DungeonBuddy] Starting...");
            
            var settings = DungeonBuddySettings.Instance;
            Logging.Write($"[DungeonBuddy] Config: QueueType={settings.QueueType}, SelectedDungeons=[{string.Join(",", settings.SelectedDungeonIds)}]");
            
            // Charger les scripts de donjon (réflection sur l'assembly)
            DungeonManager.LoadDungeonScripts();
            Targeting.Instance = new DungeonTargeting();

            Logging.Write("[DungeonBuddy] Script folder: {0}", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon Scripts"));
            Logging.Write("[DungeonBuddy] Profile folder: {0}", System.IO.Path.Combine(Logging.ApplicationPath, "Default Profiles\\DungeonBuddy\\"));
            
            // Attacher les événements LFG
            LfgManager.AttachLfgEvents();
            
            _hasSetRole = false;
            _lastMapId = StyxWoW.Me.MapId;
            _activeDungeonBehavior = null;

            // SoloFarm HB parity: précharger le donjon sélectionné même hors instance
            // pour avoir CurrentDungeon + profile disponibles avant d'entrer.
            if (settings.QueueType == QueueType.SoloFarm &&
                settings.SelectedDungeonIds.Length > 0)
            {
                DungeonManager.SetDungeonById(settings.SelectedDungeonIds[0]);
            }
            
            Logging.Write("[DungeonBuddy] Started successfully!");
        }

        public override void Stop()
        {
            Logging.Write("[DungeonBuddy] Stopping...");
            
            LfgManager.DetachLfgEvents();
            Targeting.Instance = new Targeting();
            DungeonManager.Clear();
            BossManager.Reset();
            Bots.DungeonBuddy.Avoidance.AvoidanceManager.Clear();
            _activeDungeonBehavior = null;
        }

        public override void Pulse()
        {
            // NOTE: HB 4.3.4 wrappe Root.Tick() dans un FrameLock (ObjectManager.Update + lock).
            // Si on observe des incohérences d'état (objets désync), envisager:
            //   using (StyxWoW.Memory.AcquireFrame()) { Root.Tick(...); }
            // À valider en jeu — pour l'instant on pulse sans FrameLock comme LevelBot.
            
            // Détecter changement de map (entrée/sortie donjon)
            var currentMap = (uint)StyxWoW.Me.MapId;
            if (currentMap != _lastMapId)
            {
                _lastMapId = currentMap;
                OnMapChanged(currentMap);
            }
            
            // Mettre à jour l'avoidance
            Bots.DungeonBuddy.Avoidance.AvoidanceManager.Update();

            // Keep targeting list fresh every pulse (HB behavior depends on a live FirstUnit/TargetList).
            if (StyxWoW.Me.IsInInstance)
                Targeting.Instance.Pulse();

            // Recovery: si on est dans un donjon mais CurrentDungeon == null (race condition
            // pendant le loading screen — OnMapChanged a pu firer avant IsInInstance = true),
            // réinitialiser immédiatement.
            if (StyxWoW.Me.CurrentMap.IsDungeon && DungeonManager.CurrentDungeon == null)
            {
                var settings2 = DungeonBuddySettings.Instance;
                if (settings2.QueueType == QueueType.SoloFarm && settings2.SelectedDungeonIds.Length > 0)
                    DungeonManager.SetDungeonById(settings2.SelectedDungeonIds[0]);
                else
                    DungeonManager.SetDungeon(StyxWoW.Me.MapId);
            }

            // Completion check stays in Pulse; dead-boss marking itself is now the dedicated
            // HB parity tree branch: BossManager.CreateCheckForDeadBossBehavior().
            if (StyxWoW.Me.IsInInstance && DungeonManager.CurrentDungeon != null &&
                !LfgManager.DungeonCompleted && BossManager.AreAllRequiredBossesDead())
            {
                Logging.Write("[DungeonBuddy] All required bosses dead — dungeon complete!");
                _soloFarmExitTimer.Reset();
                LfgManager.DungeonCompleted = true;
            }
        }

        private void OnMapChanged(uint newMapId)
        {
            // HB 4.3.4 method_41: détermine la direction (entrée/sortie) via CurrentMap.IsDungeon,
            // PAS IsInInstance. IsInInstance peut être false pendant le loading screen même
            // si MapId est déjà 389 (dungeon) — ce qui causait un faux « Left instance »
            // suivi de DungeonManager.Clear() + perte de CurrentDungeon.
            bool isInDungeonMap = StyxWoW.Me.CurrentMap.IsDungeon;

            if (isInDungeonMap)
            {
                Logging.Write($"[DungeonBuddy] Entered instance (MapId={newMapId})");

                var settings = DungeonBuddySettings.Instance;
                if (settings.QueueType == QueueType.SoloFarm && settings.SelectedDungeonIds.Length > 0)
                    DungeonManager.SetDungeonById(settings.SelectedDungeonIds[0]);
                else
                    DungeonManager.SetDungeon(newMapId);

                _activeDungeonBehavior = null;
                LfgManager.DungeonCompleted = false;
                _soloFarmResetInstancesPending = false;
                _outsideFlyPoint = WoWPoint.Zero;
            }
            else
            {
                // Quitte l'instance — seulement clear si on était effectivement dans un donjon
                if (DungeonManager.CurrentDungeon != null)
                {
                    Logging.Write($"[DungeonBuddy] Left instance");
                    DungeonManager.Clear();
                    BossManager.Reset();
                    _activeDungeonBehavior = null;

                    if (LfgManager.DungeonCompleted)
                        _soloFarmResetInstancesPending = true;

                    // SoloFarm: re-sélectionner immédiatement le donjon cible hors instance
                    // pour garder script/profile actifs entre deux runs.
                    var settings = DungeonBuddySettings.Instance;
                    if (settings.QueueType == QueueType.SoloFarm && settings.SelectedDungeonIds.Length > 0)
                        DungeonManager.SetDungeonById(settings.SelectedDungeonIds[0]);
                }
            }
        }

        private static bool ShouldLog(Stopwatch throttle, int intervalMs)
        {
            if (!throttle.IsRunning || throttle.ElapsedMilliseconds >= intervalMs)
            {
                throttle.Restart();
                return true;
            }

            return false;
        }

        // ═══════════════════════════════════════════════════════════
        // BEHAVIOR TREE
        // ═══════════════════════════════════════════════════════════

        private PrioritySelector CreateRootBehavior()
        {
            return new PrioritySelector(
                // HB method_12 parity: only run this block while we're on a dungeon map.
                new Decorator(
                    ctx => StyxWoW.Me.CurrentMap.IsDungeon,
                    new PrioritySelector(
                        // 1) method_27
                        CreateDeathBehavior(),

                        // 2) method_45
                        CreateLfgBehavior(),

                        // 3) BossManager.CreateCheckForDeadBossBehavior
                        BossManager.CreateCheckForDeadBossBehavior(),

                        // 4) method_11
                        CreateDungeonBehavior(),

                        // 5) method_0
                        CreateCombatBehavior(),

                        // 6) method_13 guarded by smethod_127
                        new Decorator(
                            ctx => DungeonBuddySettings.Instance.QueueType == QueueType.SoloFarm &&
                                   LfgManager.DungeonCompleted &&
                                   _soloFarmExitTimer.IsFinished,
                            CreateSoloFarmExitBehavior()),

                        // 7) method_19
                        CreateLootBehavior(),

                        // 8) method_4
                        CreateInDungeonSupportBehavior()
                    )
                ),

                // HB method_14-equivalent outside dungeon for SoloFarm portal travel.
                CreateOutsideDungeonBehavior(),

                new ActionIdle()
            );
        }

        private Composite CreateOutsideDungeonBehavior()
        {
            IEnumerable<PoiType> vendorPois = new[]
            {
                PoiType.Sell,
                PoiType.Buy,
                PoiType.Repair,
                PoiType.Train
            };

            return new Decorator(
                ctx => !StyxWoW.Me.CurrentMap.IsDungeon,
                new PrioritySelector(
                    new Decorator(
                        ctx => DungeonBuddySettings.Instance.QueueType == QueueType.SoloFarm &&
                               DungeonManager.CurrentDungeon != null,
                        new DecoratorIsNotPoiType(
                            vendorPois,
                            new PrioritySelector(
                                new Decorator(
                                    ctx => _soloFarmResetInstancesPending,
                                    new Sequence(
                                        new ActionSetActivity("Reseting Instances"),
                                        new Action(ctx =>
                                        {
                                            Logging.Write("Reseting Instances");
                                            Lua.DoString("ResetInstances();");
                                            return RunStatus.Success;
                                        }),
                                        new Action(ctx =>
                                        {
                                            _soloFarmResetInstancesPending = false;
                                            _outsideFlyPoint = WoWPoint.Zero;
                                            return RunStatus.Success;
                                        })
                                    )
                                ),

                                new Decorator(
                                    ctx => !DungeonManager.CurrentDungeon.IsFlyingCorpseRun,
                                    new PrioritySelector(
                                        new ActionSetActivity("Moving to Instance Portal on foot"),
                                        new Decorator(
                                            ctx => !ObjectManager.Me.Mounted &&
                                                   Mount.ShouldMount(DungeonManager.CurrentDungeon.Entrance) &&
                                                   Mount.CanMount(),
                                            new PrioritySelector(
                                                new Decorator(
                                                    ctx => StyxWoW.Me.IsMoving,
                                                    new Action(ctx =>
                                                    {
                                                        WoWMovement.MoveStop();
                                                        return RunStatus.Success;
                                                    })
                                                ),
                                                new Action(ctx =>
                                                {
                                                    Mount.MountUp(() => DungeonManager.CurrentDungeon.Entrance);
                                                    return RunStatus.Success;
                                                })
                                            )
                                        ),
                                        CreateSoloFarmBehavior()
                                    )
                                ),

                                new Decorator(
                                    ctx => DungeonManager.CurrentDungeon.IsFlyingCorpseRun,
                                    new PrioritySelector(
                                        new ActionSetActivity("Flying to Instance Portal"),
                                        new Decorator(
                                            ctx => DungeonManager.CurrentDungeon.CorpseRunBreadCrumb == null ||
                                                   DungeonManager.CurrentDungeon.CorpseRunBreadCrumb.Count == 0,
                                            new Action(ctx =>
                                            {
                                                Flightor.MoveTo(DungeonManager.CurrentDungeon.Entrance);
                                                return RunStatus.Running;
                                            })
                                        ),
                                        new Sequence(
                                            new Action(ctx =>
                                            {
                                                var crumbs = DungeonManager.CurrentDungeon.CorpseRunBreadCrumb;
                                                if (_outsideFlyPoint == WoWPoint.Zero && crumbs != null && crumbs.Count > 0)
                                                {
                                                    crumbs.CycleTo(crumbs.First);
                                                    _outsideFlyPoint = crumbs.Dequeue();
                                                }
                                                return RunStatus.Success;
                                            }),
                                            new Action(ctx =>
                                            {
                                                var crumbs = DungeonManager.CurrentDungeon.CorpseRunBreadCrumb;
                                                if (crumbs == null || crumbs.Count == 0)
                                                {
                                                    Flightor.MoveTo(DungeonManager.CurrentDungeon.Entrance);
                                                    return RunStatus.Running;
                                                }

                                                if (StyxWoW.Me.Location.Distance2DSqr(_outsideFlyPoint) < 225f)
                                                {
                                                    _outsideFlyPoint = crumbs.Dequeue();
                                                    if (_outsideFlyPoint == crumbs.First)
                                                        return RunStatus.Success;
                                                }

                                                Flightor.MoveTo(_outsideFlyPoint);
                                                return RunStatus.Running;
                                            })
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }

        // ═══════════════════════════════════════════════════════════
        // HB METHOD_19 (UTILITY INTERACTIONS)
        // ═══════════════════════════════════════════════════════════

        private Composite CreateLootBehavior()
        {
            // HB method_19 = method_20 + method_21
            return new PrioritySelector(
                CreateRitualAssistBehavior(),
                CreateConsumableObjectBehavior());
        }

        private WoWGameObject? Ritual
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .FirstOrDefault(go => go.SubType == WoWGameObjectType.Ritual &&
                                          go.CreatedByGuid != StyxWoW.Me.Guid &&
                                          StyxWoW.Me.PartyMembers.Any(p => p.Guid == go.CreatedByGuid));
            }
        }

        private WoWGameObject? MageTable
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .FirstOrDefault(go => _mageTableEntries.Contains(go.Entry) &&
                                          (StyxWoW.Me.PartyMembers.Any(p => p.Guid == go.CreatedByGuid) ||
                                           StyxWoW.Me.Guid == go.CreatedByGuid));
            }
        }

        private WoWGameObject? SoulWell
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .FirstOrDefault(go => go.Entry == 181621U &&
                                          (StyxWoW.Me.PartyMembers.Any(p => p.Guid == go.CreatedByGuid) ||
                                           StyxWoW.Me.Guid == go.CreatedByGuid));
            }
        }

        private int CarriedMageFoodCount
        {
            get
            {
                return StyxWoW.Me.CarriedItems.Sum(item =>
                {
                    if (item == null || item.ItemInfo == null || item.ItemInfo.ItemClass != WoWItemClass.Consumable)
                        return 0;

                    if (item.ItemSpells == null || item.ItemSpells.Count == 0 || item.ItemSpells[0].ActualSpell == null)
                        return 0;

                    return item.ItemSpells[0].ActualSpell.Name.Contains("Refreshment") ? (int)item.StackCount : 0;
                });
            }
        }

        private bool HasHearthStone
        {
            get
            {
                return StyxWoW.Me.CarriedItems.Any(item =>
                    item != null &&
                    item.ItemSpells != null &&
                    item.ItemSpells.Any(spell => spell.ActualSpell != null && spell.ActualSpell.Name == "Healthstone"));
            }
        }

        private Composite CreateRitualAssistBehavior()
        {
            return new Decorator(
                ctx => !StyxWoW.Me.IsCasting,
                new PrioritySelector(
                    new ContextChangeHandler(ctx => Ritual),
                    new Decorator(
                        ctx => ctx is WoWGameObject,
                        new Sequence(
                            new Action(ctx =>
                            {
                                var ritual = (WoWGameObject)ctx;
                                Logging.Write("Assisting with ritual casted by {0}", ritual.CreatedBy.Name);
                                return RunStatus.Success;
                            }),
                            new DecoratorContinue(
                                ctx => ((WoWGameObject)ctx).DistanceSqr > 36.0,
                                new Action(ctx =>
                                {
                                    var ritual = (WoWGameObject)ctx;
                                    Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, ritual.Location, 6f));
                                    return RunStatus.Success;
                                })
                            ),
                            new DecoratorContinue(
                                ctx => StyxWoW.Me.IsMoving,
                                new Sequence(
                                    new Action(ctx =>
                                    {
                                        WoWMovement.MoveStop();
                                        return RunStatus.Success;
                                    }),
                                    new WaitContinue(4, ctx => !StyxWoW.Me.IsMoving, new ActionAlwaysSucceed())
                                )
                            ),
                            new Action(ctx =>
                            {
                                ((WoWGameObject)ctx).Interact();
                                return RunStatus.Success;
                            }),
                            new WaitContinue(2, ctx => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                            new WaitContinue(25, ctx => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed())
                        )
                    )
                )
            );
        }

        private Composite CreateConsumableObjectBehavior()
        {
            return new PrioritySelector(
                new PrioritySelector(
                    new ContextChangeHandler(ctx => MageTable),
                    new Decorator(
                        ctx => ctx is WoWGameObject && CarriedMageFoodCount < 80 && StyxWoW.Me.FreeNormalBagSlots > 1,
                        new Sequence(
                            new Action(ctx =>
                            {
                                Logging.Write("Getting Mage food");
                                return RunStatus.Success;
                            }),
                            new DecoratorContinue(
                                ctx => ((WoWGameObject)ctx).DistanceSqr > 25.0,
                                new Action(ctx => Navigator.GetRunStatusFromMoveResult(
                                    Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, ((WoWGameObject)ctx).Location, 5f))))),
                            new Action(ctx =>
                            {
                                ((WoWGameObject)ctx).Interact();
                                return RunStatus.Success;
                            }),
                            new WaitContinue(2, ctx => false, new ActionAlwaysSucceed())
                        )
                    )
                ),
                new PrioritySelector(
                    new ContextChangeHandler(ctx => SoulWell),
                    new Decorator(
                        ctx => ctx is WoWGameObject && !HasHearthStone && StyxWoW.Me.FreeNormalBagSlots > 1,
                        new Sequence(
                            new Action(ctx =>
                            {
                                Logging.Write("Getting Warlock Healthstone");
                                return RunStatus.Success;
                            }),
                            new DecoratorContinue(
                                ctx => ((WoWGameObject)ctx).DistanceSqr > 25.0,
                                new Action(ctx =>
                                {
                                    Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, ((WoWGameObject)ctx).Location, 5f));
                                    return RunStatus.Success;
                                })),
                            new Action(ctx =>
                            {
                                ((WoWGameObject)ctx).Interact();
                                return RunStatus.Success;
                            }),
                            new WaitContinue(1, ctx => false, new ActionAlwaysSucceed())
                        )
                    )
                )
            );
        }

        private Composite CreateLfgBehavior()
        {
            return new PrioritySelector(
                // --- IN DUNGEON (LFG): Dungeon completed → teleport out + requeue ---
                // SoloFarm se gère séparément via CreateSoloFarmExitBehavior (walk to portal).
                new Decorator(
                    ctx => LfgManager.CurrentState == LfgState.InDungeon &&
                           LfgManager.DungeonCompleted &&
                           DungeonBuddySettings.Instance.QueueType != QueueType.SoloFarm,
                    new Sequence(
                        new Action(ctx =>
                        {
                            Logging.Write("[DungeonBuddy] Dungeon complete! Teleporting out...");
                            LfgManager.TeleportOut();
                            LfgManager.DungeonCompleted = false;
                            _requeueDelay.Restart();
                            return RunStatus.Success;
                        }),
                        new WaitContinue(
                            TimeSpan.FromSeconds(10),
                            ctx => !StyxWoW.Me.IsInInstance,
                            new ActionAlwaysSucceed()
                        )
                    )
                ),

                // --- ABANDONED IN DUNGEON: Teleport out ---
                new Decorator(
                    ctx => LfgManager.CurrentState == LfgState.AbandonedInDungeon,
                    new Action(ctx =>
                    {
                        Logging.Write("[DungeonBuddy] Abandoned in dungeon, teleporting out...");
                        LfgManager.TeleportOut();
                        return RunStatus.Success;
                    })
                ),

                // --- NOT IN LFG: Set role + Queue ---
                new Decorator(
                    ctx => LfgManager.CurrentState == LfgState.NotInLfg &&
                           DungeonBuddySettings.Instance.QueueType != QueueType.SoloFarm,
                    new Sequence(
                        // Set role si pas encore fait
                        new DecoratorContinue(
                            ctx => !_hasSetRole,
                            new Action(ctx =>
                            {
                                LfgManager.SetRole(PartyRole.Dps);
                                _hasSetRole = true;
                                Logging.Write("[DungeonBuddy] Role set to Dps");
                                return RunStatus.Success;
                            })
                        ),
                        // Attendre un peu après teleport out avant requeue
                        new Decorator(
                            ctx => !_requeueDelay.IsRunning || _requeueDelay.ElapsedMilliseconds > 3000,
                            new Action(ctx =>
                            {
                                var settings = DungeonBuddySettings.Instance;
                                switch (settings.QueueType)
                                {
                                    case QueueType.RandomDungeon:
                                        LfgManager.QueueForRandomDungeon();
                                        break;
                                    case QueueType.RandomHeroic:
                                        LfgManager.QueueForRandomHeroic();
                                        break;
                                    case QueueType.Specific:
                                        if (settings.SelectedDungeonIds.Length > 0)
                                            LfgManager.QueueForSpecificDungeon(settings.SelectedDungeonIds[0]);
                                        break;
                                }
                                _requeueDelay.Restart();
                                return RunStatus.Success;
                            })
                        )
                    )
                ),

                // --- IN QUEUE: Idle, afficher timer ---
                new Decorator(
                    ctx => LfgManager.CurrentState == LfgState.InQueue,
                    new ActionAlwaysFail()
                ),

                new ActionAlwaysFail()
            );
        }

        // ═══════════════════════════════════════════════════════════
        // DEATH BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        private Composite CreateDeathBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ctx => StyxWoW.Me.IsDead,
                    new Sequence(
                        new Action(ctx =>
                        {
                            Logging.Write("[DungeonBuddy] Died! Releasing...");
                            Lua.DoString("RepopMe()");
                        }),
                        new WaitContinue(5, ctx => StyxWoW.Me.IsGhost, new ActionAlwaysSucceed())
                    )
                ),
                new Decorator(
                    ctx => StyxWoW.Me.IsGhost && StyxWoW.Me.IsInInstance,
                    new Sequence(
                        new Action(ctx =>
                        {
                            var entrance = DungeonManager.CurrentDungeon?.Entrance ?? WoWPoint.Zero;
                            if (entrance != WoWPoint.Zero)
                                Navigator.MoveTo(entrance);
                            return RunStatus.Running;
                        })
                    )
                )
            );
        }

        // ═══════════════════════════════════════════════════════════
        // SOLO FARM BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        private Composite CreateSoloFarmBehavior()
        {
            return new Decorator(
                ctx =>
                {
                    var me = StyxWoW.Me;
                    if (me == null) return false;
                    bool notDungeon = !me.CurrentMap.IsDungeon;
                    bool isSoloFarm = DungeonBuddySettings.Instance.QueueType == QueueType.SoloFarm;
                    return notDungeon && isSoloFarm;
                },
                new Sequence(
                    new Action(ctx =>
                    {
                        var settings = DungeonBuddySettings.Instance;
                        if (settings.SelectedDungeonIds.Length == 0)
                        {
                            if (ShouldLog(_soloFarmStatusLogThrottle, 5000))
                                Logging.Write("[DungeonBuddy] SoloFarm: no dungeon selected in settings");
                            TreeRoot.StatusText = "SoloFarm: No dungeon selected";
                            return RunStatus.Failure;
                        }

                        uint selectedDungeonId = settings.SelectedDungeonIds[0];

                        // Utiliser le script déjà sélectionné (HB parity), fallback sur lookup direct.
                        if (DungeonManager.CurrentDungeon == null || DungeonManager.CurrentDungeon.DungeonId != selectedDungeonId)
                            DungeonManager.SetDungeonById(selectedDungeonId);

                        var entranceFromCurrent = DungeonManager.CurrentDungeon?.Entrance ?? WoWPoint.Zero;
                        var entrance = DungeonManager.GetEntranceForDungeon(selectedDungeonId);
                        if (entranceFromCurrent != WoWPoint.Zero)
                            entrance = entranceFromCurrent;

                        if (entrance == WoWPoint.Zero)
                        {
                            if (ShouldLog(_soloFarmStatusLogThrottle, 5000))
                                Logging.Write("[DungeonBuddy] SoloFarm: entrance unavailable for dungeonId={0}", selectedDungeonId);
                            TreeRoot.StatusText = "SoloFarm: Entrance unavailable";
                            return RunStatus.Failure;
                        }

                        float distSq = StyxWoW.Me.Location.DistanceSqr(entrance);
                        if (ShouldLog(_soloFarmStatusLogThrottle, 5000))
                        {
                            double dist = Math.Sqrt(distSq);
                            Logging.Write("[DungeonBuddy] SoloFarm: moving to entrance (dungeonId={0}, distance={1:F1})", selectedDungeonId, dist);
                        }

                        // HB outside-instance portal travel keeps driving toward entrance until map changes.
                        TreeRoot.StatusText = "SoloFarm: Moving to dungeon entrance";
                        Navigator.MoveTo(entrance);
                        return RunStatus.Running;
                    })
                )
            );
        }

        // ═══════════════════════════════════════════════════════════
        // SOLO FARM EXIT BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Equivalent de HB method_13() — SoloFarm exit sequence.
        /// Quand tous les boss sont morts : marche vers ExitLocation (ou tente LeaveBattlefield).
        /// Une fois sorti de l'instance, OnMapChanged reset DungeonCompleted + BossManager,
        /// puis CreateSoloFarmBehavior retourne vers l'entrée pour un nouveau run.
        /// </summary>
        private Composite CreateSoloFarmExitBehavior()
        {
            return new Decorator(
                ctx => DungeonBuddySettings.Instance.QueueType == QueueType.SoloFarm &&
                       LfgManager.DungeonCompleted &&
                       _soloFarmExitTimer.IsFinished &&
                       StyxWoW.Me.IsInInstance,
                new Action(ctx =>
                {
                    TreeRoot.StatusText = "SoloFarm: Dungeon complete — moving to exit";

                    var exit = DungeonManager.CurrentDungeon?.ExitLocation ?? WoWPoint.Zero;
                    if (exit != WoWPoint.Zero)
                    {
                        if (StyxWoW.Me.Location.DistanceSqr(exit) > 4 * 4)
                        {
                            Navigator.MoveTo(exit);
                            return RunStatus.Running;
                        }
                        // Reached exit portal — OnMapChanged will fire and reset the bot state.
                    }
                    else
                    {
                        Logging.WriteDebug("[DungeonBuddy] SoloFarm: ExitLocation not defined — bot will idle at end of run.");
                    }

                    return RunStatus.Running;
                })
            );
        }

        // ═══════════════════════════════════════════════════════════
        // HOTSPOT MOVEMENT BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        private int _hotspotIndex;

        /// <summary>
        /// Equivalent de HB method_11() — mouvement vers le prochain hotspot du profil.
        /// Quand il n'y a rien à tuer à portée, avance vers le prochain point du circuit.
        /// Si pas de profil, marche vers le prochain boss enregistré non-mort.
        /// </summary>
        private Composite CreateHotspotMovementBehavior()
        {
            return new Decorator(
                ctx => StyxWoW.Me.IsInInstance &&
                       !StyxWoW.Me.Combat &&
                       !LfgManager.DungeonCompleted &&
                       Targeting.Instance.TargetList.Count == 0,
                new Action(ctx =>
                {
                    // Try profile hotspots first
                    var profile = Bots.DungeonBuddy.Profiles.ProfileManager.CurrentProfile;
                    if (profile != null && profile.HotSpots.Count > 0)
                    {
                        if (_hotspotIndex >= profile.HotSpots.Count)
                            _hotspotIndex = 0;

                        var target = profile.HotSpots[_hotspotIndex];
                        if (StyxWoW.Me.Location.DistanceSqr(target) < 5 * 5)
                        {
                            _hotspotIndex = (_hotspotIndex + 1) % profile.HotSpots.Count;
                            target = profile.HotSpots[_hotspotIndex];
                        }

                        TreeRoot.StatusText = $"Moving to hotspot [{_hotspotIndex + 1}/{profile.HotSpots.Count}]";
                        Navigator.MoveTo(target);
                        return RunStatus.Running;
                    }

                    // Fallback: walk toward next living registered boss
                    var nextBoss = BossManager.CurrentBoss;
                    if (nextBoss != null)
                    {
                        TreeRoot.StatusText = $"Moving to boss: {nextBoss.Name}";
                        Navigator.MoveTo(nextBoss.Location);
                        return RunStatus.Running;
                    }

                    return RunStatus.Failure;
                })
            );
        }

        // ═══════════════════════════════════════════════════════════
        // AVOIDANCE BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        private Composite CreateAvoidanceBehavior()
        {
            return new Decorator(
                ctx =>
                {
                    var me = StyxWoW.Me;
                    return me != null && me.IsInInstance &&
                           Bots.DungeonBuddy.Avoidance.AvoidanceManager.IsInAvoidance(me.Location);
                },
                new Action(ctx =>
                {
                    var me = StyxWoW.Me;
                    if (me == null)
                        return RunStatus.Failure;

                    var safePoint = Bots.DungeonBuddy.Avoidance.AvoidanceManager.GetSafePoint(me.Location);
                    Navigator.MoveTo(safePoint);
                    return RunStatus.Running;
                })
            );
        }

        private Composite CreateDungeonBehavior()
        {
            // IMPORTANT: do NOT pass DungeonManager.CurrentDungeonBehavior as a constructor argument —
            // that evaluates the property ONCE at tree-build time (when CurrentDungeon is still null),
            // storing a permanent empty PrioritySelector.  Instead, re-evaluate each tick via Action
            // and manage Start/Stop manually using the _activeDungeonBehavior instance field.
            return new Decorator(
                ctx =>
                {
                    var me = StyxWoW.Me;
                    return me != null && me.IsInInstance && DungeonManager.CurrentDungeon != null;
                },
                new Action(ctx =>
                {
                    var current = DungeonManager.CurrentDungeonBehavior;
                    if (current == null)
                    {
                        if (ShouldLog(_debugDungeonLogThrottle, 1000))
                            Logging.WriteDebug($"[DB:DungeonTick] CurrentDungeonBehavior=null MapId={StyxWoW.Me.MapId} IsInInstance={StyxWoW.Me.IsInInstance}");
                        return RunStatus.Failure;
                    }

                    // Call Start() once when the behavior instance changes (new dungeon loaded).
                    // HB equivalent: composite_0, nulled only on dungeon change (method_41/method_43),
                    // not on tick result. Do NOT Stop/null here on Failure — that would cause a
                    // tight Start/Stop loop every pulse when the script has nothing to do.
                    if (_activeDungeonBehavior != current)
                    {
                        Logging.WriteDebug($"[DB:DungeonTick] Switching dungeon behavior instance: {DungeonManager.CurrentDungeon?.Name ?? "<null>"}");
                        _activeDungeonBehavior?.Stop(ctx);
                        _activeDungeonBehavior = current;
                        _activeDungeonBehavior.Start(ctx);
                    }

                    RunStatus result = _activeDungeonBehavior.Tick(ctx);
                    if (ShouldLog(_debugDungeonLogThrottle, 1000))
                    {
                        var first = Targeting.Instance.FirstUnit;
                        string targetInfo = first == null ? "none" : $"{first.Name}({first.Entry}) dist={Math.Sqrt(first.DistanceSqr):F1}";
                        Logging.WriteDebug($"[DB:DungeonTick] result={result} Poi={BotPoi.Current.Type} Target={targetInfo} InCombat={StyxWoW.Me.Combat}");
                    }

                    return result;
                })
            );
        }


        // ═══════════════════════════════════════════════════════════
        // COMBAT BEHAVIOR
        // ═══════════════════════════════════════════════════════════

        // HB 4.3.4 DungeonBot.waitTimer_0 = 2.0s (retarget cooldown)
        private readonly WaitTimer _retargetTimer = new WaitTimer(TimeSpan.FromSeconds(2.0));
        // HB 4.3.4 DungeonBot.waitTimer_1 = 3.0s (aggro grace period)
        private readonly WaitTimer _aggroTimer = new WaitTimer(TimeSpan.FromSeconds(3.0));

        // Champ pour tracker le behavior d'encounter actif et le boss associé
        // IMPORTANT: Le Composite NE DOIT PAS être re-Start() à chaque pulse.
        // Start() réinitialise l'état interne (Sequences, WaitContinue, etc.)
        // On doit Start() UNE SEULE FOIS quand le boss change, puis Tick() à chaque pulse.
        // Référence: HB 4.3.4 construit les encounter behaviors dans le Root tree
        // via réflection, pas manuellement. Ici on simule ce pattern.
        private Composite? _activeEncounterBehavior;
        private uint _activeEncounterBossEntry;

        private Composite CreateCombatBehavior()
        {
            // HB 4.3.4 DungeonBot.method_0() — exact port
            return new PrioritySelector(

                // ── BRANCHE 1 : Hors combat (smethod_24 = !Me.Combat) ──────────
                new Decorator(
                    ctx => !StyxWoW.Me.Combat,
                    new PrioritySelector(
                        Routine?.RestBehavior ?? new ActionAlwaysFail(),
                        Routine?.PreCombatBuffBehavior ?? new ActionAlwaysFail(),

                        // Set Kill POI from targeting list when idle (no active POI)
                        new DecoratorContinue(
                            ctx => BotPoi.Current.Type == PoiType.None &&
                                   Targeting.Instance.FirstUnit != null &&
                                   !Targeting.Instance.FirstUnit.Dead,
                            new Action(ctx =>
                            {
                                BotPoi.Current = new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill);
                                return RunStatus.Success;
                            })
                        ),

                        new DecoratorIsPoiType(PoiType.Kill, new PrioritySelector(

                            // If kill POI object is temporarily unresolved (streaming/object cache),
                            // move toward the POI location so we do not idle waiting for aggro.
                            new Decorator(
                                ctx => BotPoi.Current.AsObject == null &&
                                       BotPoi.Current.Location != WoWPoint.Zero &&
                                       StyxWoW.Me.Location.DistanceSqr(BotPoi.Current.Location) > 4 * 4,
                                new Sequence(
                                    new Action(ctx =>
                                    {
                                        if (ShouldLog(_debugMoveLogThrottle, 1000))
                                            Logging.WriteDebug($"[DB:CombatMove] POI object unresolved -> moving to POI location {BotPoi.Current.Location}");
                                        return RunStatus.Success;
                                    }),
                                    new NavigationAction(ctx => BotPoi.Current.Location)
                                )
                            ),

                            // [0] smethod_25 : TargetList vide/mort OU (aggroTimer pas expiré ET mob ni agressif ni taggué)
                            new Decorator(
                                ctx =>
                                {
                                    if (Targeting.Instance.TargetList.Count == 0 || Targeting.Instance.FirstUnit.Dead)
                                        return true;
                                    if (!_aggroTimer.IsFinished && !Targeting.Instance.FirstUnit.IsTargetingMyPartyMember)
                                        return !Targeting.Instance.FirstUnit.TaggedByMe;
                                    return false;
                                },
                                new Sequence(
                                    new ActionClearPoi("No targets in list1"),
                                    // smethod_26/27/28 : clear current target if set, wait until cleared
                                    new DecoratorContinue(
                                        ctx => StyxWoW.Me.CurrentTarget != null,
                                        new Sequence(
                                            new Action(ctx => { StyxWoW.Me.ClearTarget(); return RunStatus.Success; }),
                                            new WaitContinue(5, ctx => StyxWoW.Me.CurrentTarget == null, new ActionAlwaysSucceed())
                                        )
                                    )
                                )
                            ),

                            // [1] smethod_29/30/31 : BotPoi ne pointe pas FirstUnit → mettre à jour le POI
                            new Decorator(
                                ctx => BotPoi.Current.AsObject != null &&
                                       BotPoi.Current.AsObject.ToUnit() != Targeting.Instance.FirstUnit,
                                new Sequence(
                                    new Action(ctx => { Logging.WriteDebug("Current POI is not the best target. Changing."); return RunStatus.Success; }),
                                    new ActionSetPoi(true, ctx => new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill))
                                )
                            ),

                            // [2] smethod_32/33/34/35 : pas en LOS → naviguer
                            new Decorator(
                                ctx => !Targeting.Instance.FirstUnit.InLineOfSpellSight,
                                new PrioritySelector(
                                    new Decorator(
                                        ctx => !Navigator.CanNavigateFully(StyxWoW.Me.Location, Targeting.Instance.FirstUnit.Location),
                                        new Action(ctx =>
                                        {
                                            Blacklist.Add(Targeting.Instance.FirstUnit, TimeSpan.FromMinutes(25.0));
                                            return RunStatus.Success;
                                        })
                                    ),
                                    new NavigationAction(ctx => Targeting.Instance.FirstUnit.Location)
                                )
                            ),

                            // [3] smethod_36/37/38 : pas encore ciblé → Target() + wait 5s
                            new Decorator(
                                ctx => (_retargetTimer.IsFinished && StyxWoW.Me.CurrentTarget != BotPoi.Current.AsObject.ToUnit())
                                       || StyxWoW.Me.CurrentTarget == null,
                                new Sequence(
                                    new Action(ctx =>
                                    {
                                        BotPoi.Current.AsObject.ToUnit().Target();
                                        _retargetTimer.Reset();
                                        return RunStatus.Success;
                                    }),
                                    new WaitContinue(5,
                                        ctx => StyxWoW.Me.CurrentTarget != null &&
                                               StyxWoW.Me.CurrentTarget == BotPoi.Current.AsObject.ToUnit(),
                                        new ActionAlwaysSucceed())
                                )
                            ),

                            // [4] PullBuff (smethod_39)
                            Routine?.PullBuffBehavior ?? new ActionAlwaysFail(),

                            // [5] MoveToTarget si disponible (smethod_39)
                            new Decorator(
                                ctx => Routine?.MoveToTargetBehavior != null,
                                Routine?.MoveToTargetBehavior ?? new ActionAlwaysFail()
                            ),

                                // [5b] Navigate to melee range when Routine has no MoveToTargetBehavior.
                                // HB method_0(): NavigationAction navigates toward FirstUnit after targeting.
                                // Without this, melee routines fail to pull when target is in LOS but out
                                // of melee range — the pull behavior returns Failure and the tree idles.
                                new Decorator(
                                    ctx => Routine?.MoveToTargetBehavior == null &&
                                           StyxWoW.Me.CurrentTarget != null &&
                                           StyxWoW.Me.CurrentTarget.DistanceSqr > 4 * 4,
                                    new Sequence(
                                        new Action(ctx =>
                                        {
                                            if (ShouldLog(_debugMoveLogThrottle, 1000))
                                                Logging.WriteDebug($"[DB:CombatMove] Moving to melee range of {StyxWoW.Me.CurrentTarget?.Name} dist={Math.Sqrt(StyxWoW.Me.CurrentTarget?.DistanceSqr ?? 0):F1}");
                                            return RunStatus.Success;
                                        }),
                                        new NavigationAction(ctx => StyxWoW.Me.CurrentTarget.Location)
                                    )
                                ),

                                // [6] Pull
                                Routine?.PullBehavior ?? new ActionAlwaysFail()
                            ))
                        )
                    ),

                // ── BRANCHE 2 : En combat (smethod_40 = Me.Combat) ────────────
                new Decorator(
                    ctx => StyxWoW.Me.Combat,
                    new PrioritySelector(

                        // smethod_41 : reset aggro timer, toujours Failure pour continuer
                        new Action(ctx => { _aggroTimer.Reset(); return RunStatus.Failure; }),

                        new DecoratorIsPoiType(PoiType.Kill, new PrioritySelector(

                            // [0] smethod_42/43/44/45 : FirstUnit mort/null ou POI invalide → ClearPoi
                            new Decorator(
                                ctx => Targeting.Instance.FirstUnit == null ||
                                       Targeting.Instance.FirstUnit.Dead ||
                                       BotPoi.Current.AsObject == null ||
                                       !BotPoi.Current.AsObject.IsValid,
                                new Sequence(
                                    new ActionClearPoi("No targets in list2"),
                                    // smethod_43/44/45 : clear current target if set, wait until cleared
                                    new DecoratorContinue(
                                        ctx => StyxWoW.Me.CurrentTarget != null,
                                        new Sequence(
                                            new Action(ctx => { StyxWoW.Me.ClearTarget(); return RunStatus.Success; }),
                                            new WaitContinue(2, ctx => StyxWoW.Me.CurrentTarget == null, new ActionAlwaysSucceed())
                                        )
                                    )
                                )
                            ),

                            // [1] smethod_46/47 : BotPoi != FirstUnit → change POI, retourne Failure
                            new Decorator(
                                ctx => BotPoi.Current.AsObject.ToUnit() != Targeting.Instance.FirstUnit,
                                new Sequence(
                                    new Action(ctx =>
                                    {
                                        Logging.WriteDebug("Current POI is not the best pull target. Changing.");
                                        BotPoi.Current = new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill);
                                        return RunStatus.Failure;
                                    })
                                )
                            ),

                            // [2] smethod_48/49/50 : retarget en combat si nécessaire
                            new Decorator(
                                ctx => (_retargetTimer.IsFinished && StyxWoW.Me.CurrentTarget != BotPoi.Current.AsObject.ToUnit())
                                       || StyxWoW.Me.CurrentTarget == null,
                                new Sequence(
                                    new Action(ctx =>
                                    {
                                        BotPoi.Current.AsObject.ToUnit().Target();
                                        _retargetTimer.Reset();
                                        return RunStatus.Success;
                                    }),
                                    new WaitContinue(2,
                                        ctx => StyxWoW.Me.CurrentTarget != null &&
                                               BotPoi.Current.AsObject != null &&
                                               StyxWoW.Me.CurrentTarget == BotPoi.Current.AsObject.ToUnit(),
                                        new ActionAlwaysSucceed())
                                )
                            ),

                            // Encounter handler (CopilotBuddy — boss encounter scripts via réflexion)
                            new Decorator(
                                // Utilise la liste enregistrée au lieu du flag IsBoss (cassé pour les boss de bas niveau)
                        ctx => StyxWoW.Me.CurrentTarget != null &&
                               BossManager.Bosses.Any(b => b.EntryId == StyxWoW.Me.CurrentTarget.Entry),
                                new Action(ctx =>
                                {
                                    var boss = StyxWoW.Me.CurrentTarget;
                                    if (boss.Entry != _activeEncounterBossEntry)
                                    {
                                        try { _activeEncounterBehavior?.Stop(boss); } catch { }
                                        _activeEncounterBehavior = null;
                                        _activeEncounterBossEntry = boss.Entry;
                                        _activeEncounterBehavior = DungeonManager.GetEncounterBehavior((int)boss.Entry);
                                        _activeEncounterBehavior?.Start(boss);
                                    }
                                    if (_activeEncounterBehavior != null)
                                    {
                                        var result = _activeEncounterBehavior.Tick(boss);
                                        if (result != RunStatus.Running)
                                        {
                                            _activeEncounterBehavior.Stop(boss);
                                            _activeEncounterBehavior = null;
                                            _activeEncounterBossEntry = 0;
                                        }
                                        return result;
                                    }
                                    return RunStatus.Failure;
                                })
                            ),

                            // [3] HealBehavior
                            Routine?.HealBehavior ?? new ActionAlwaysFail(),

                            // [4] CombatBuffBehavior
                            Routine?.CombatBuffBehavior ?? new ActionAlwaysFail(),

                            // [5] CombatBehavior
                            Routine?.CombatBehavior ?? new ActionAlwaysFail()
                        ))
                    )
                )
            );
        }

        // ═══════════════════════════════════════════════════════════
        // IN-DUNGEON SUPPORT (HB method_4 / method_5)
        // ═══════════════════════════════════════════════════════════

        private Composite CreateInDungeonSupportBehavior()
        {
            return new Decorator(
                ctx => StyxWoW.Me.IsInInstance,
                new PrioritySelector(
                    new Decorator(
                        ctx => DungeonBuddySettings.Instance.QueueType == QueueType.SoloFarm,
                        new Sequence(
                            CreateSoloFarmSupportBehavior(),
                            new ActionAlwaysSucceed()
                        )
                    ),

                    CreateFollowBehavior()
                )
            );
        }

        private Composite CreateSoloFarmSupportBehavior()
        {
            return new PrioritySelector(
                // HB smethod_91..95 guards (maintenance/vendor checks).
                // Keep behavior non-blocking here; movement/targeting remains the primary in-instance action.
                new Decorator(ShouldRepairInSoloFarm, new ActionAlwaysFail()),
                new Decorator(ShouldBuyDrinksInSoloFarm, new ActionAlwaysFail()),
                new Decorator(ShouldSellItemsInSoloFarm, new ActionAlwaysFail()),
                new Decorator(ShouldTrainInSoloFarm, new ActionAlwaysFail()),
                new Decorator(IsInWrongInstanceInSoloFarm, new ActionAlwaysFail()),

                new PrioritySelector(
                    // HB smethod_96/smethod_97: set kill POI from targeting when POI is not Kill.
                    new Decorator(
                        ShouldSetSoloFarmKillPoi,
                        new ActionSetPoi(true, CreateSoloFarmKillPoi)
                    ),

                    // HB smethod_98 + method_22: move when not actually in combat.
                    new Decorator(
                        ShouldMoveInSoloFarm,
                        CreateSoloFarmMovementBehavior()
                    )
                )
            );
        }

        private Composite CreateSoloFarmMovementBehavior()
        {
            WoWPoint moveTo = WoWPoint.Zero;

            return new PrioritySelector(
                new ContextChangeHandler(ctx => moveTo = GetSoloFarmMoveToPoint()),
                new Decorator(
                    ctx => moveTo != WoWPoint.Zero,
                    new PrioritySelector(
                        Helpers.ScriptHelpers.CreateMountBehavior(() => moveTo),
                        new Action(ctx =>
                        {
                            Navigator.MoveTo(moveTo);
                            return RunStatus.Success;
                        })
                    )
                )
            );
        }

        /// <summary>
        /// Port de HB 4.3.4 DungeonBot.method_23() — calcul du point de mouvement solo farm.
        /// 6 priorités: in-combat membre → mort → distant (>=2) → Hotspot POI → boss ObjectManager → breadcrumbs.
        /// </summary>
        private WoWPoint GetSoloFarmMoveToPoint()
        {
            // Case 1: In-combat party member (HB method_23 priority 1)
            var inCombatMember = StyxWoW.Me.PartyMemberInfos
                .Select(pm => pm.ToPlayer())
                .FirstOrDefault(p => p != null && p.Combat);
            if (inCombatMember != null)
            {
                TreeRoot.StatusText = $"Moving towards in combat party member {inCombatMember.Name}";
                return inCombatMember.Location;
            }

            // Case 2: Dead party member (HB method_23 priority 2)
            var deadMember = StyxWoW.Me.PartyMemberInfos
                .Select(pm => pm.ToPlayer())
                .FirstOrDefault(p => p != null && p.IsDead);
            if (deadMember != null)
            {
                TreeRoot.StatusText = $"Moving towards dead party member {deadMember.Name}";
                return deadMember.Location;
            }

            // Case 3: 2+ distant party members (HB method_23 priority 3)
            var followDist = DungeonBuddySettings.Instance.FollowingDistance;
            var distantMembers = StyxWoW.Me.PartyMemberInfos
                .Select(pm => pm.ToPlayer())
                .Where(p => p != null && StyxWoW.Me.Location.DistanceSqr(p.Location) > followDist * followDist * 4.0)
                .ToArray();
            if (distantMembers.Length >= 2)
            {
                TreeRoot.StatusText = $"Moving towards distant party member {distantMembers[0].Name}";
                return distantMembers[0].Location;
            }

            // Case 4: Hotspot POI (HB method_23 priority 4)
            if (BotPoi.Current.Type == PoiType.Hotspot && BotPoi.Current.Location != WoWPoint.Zero)
            {
                var hotspot = BotPoi.Current.Location;
                if (StyxWoW.Me.Location.DistanceSqr(hotspot) <= 16f)
                    BotPoi.Clear("Reached Hotspot location");
                return hotspot;
            }

            // Case 5: Find boss unit directly in ObjectManager (HB method_23 priority 5 / method_24)
            var bossUnit = FindCurrentBossUnit();
            if (bossUnit != null)
            {
                TreeRoot.StatusText = $"Moving towards boss {bossUnit.Name}";
                return bossUnit.Location;
            }

            // Case 6: Boss PathBreadCrumbs (HB method_23 priority 6)
            TreeRoot.StatusText = string.Empty;
            var currentBoss = BossManager.Bosses.FirstOrDefault(b => !b.IsDead);
            if (currentBoss != null && currentBoss.PathBreadCrumbs.Count > 0)
            {
                var crumb = currentBoss.PathBreadCrumbs.Peek();
                if (StyxWoW.Me.Location.DistanceSqr(crumb) < 25f)
                    currentBoss.PathBreadCrumbs.Dequeue();
                return crumb;
            }

            return WoWPoint.Zero;
        }

        /// <summary>
        /// Port de HB 4.3.4 DungeonBot.method_24() — cherche le prochain boss à tuer dans
        /// l'ObjectManager directement (bypass targeting), ordonné par KillOrder puis distance.
        /// </summary>
        private static WoWUnit? FindCurrentBossUnit()
        {
            var bosses = BossManager.Bosses;
            return ObjectManager.GetObjectsOfType<WoWUnit>()
                .Where(u => u.IsValid && u.IsAlive && IsTargetableBossUnit(u, bosses))
                .OrderBy(u =>
                {
                    for (int i = 0; i < bosses.Count; i++)
                        if (bosses[i].Entry == u.Entry) return i;
                    return int.MaxValue;
                })
                .ThenBy(u => u.DistanceSqr)
                .FirstOrDefault();
        }

        /// <summary>
        /// Port de HB 4.3.4 DungeonBot.method_25() — filtre de validité boss pour navigation.
        /// </summary>
        private static bool IsTargetableBossUnit(WoWUnit unit, IReadOnlyList<BossManager.Boss> bosses)
        {
            var currentBoss = bosses.FirstOrDefault(b => !b.IsDead);
            if (currentBoss != null && unit.Entry == currentBoss.Entry)
                return true;
            var unitBoss = bosses.FirstOrDefault(b => b.Entry == unit.Entry);
            if (unitBoss != null && !unitBoss.IsOptional && !unitBoss.IsDead)
                return true;
            return false;
        }

        private WoWPoint GetSoloFarmFollowPoint(WoWPlayer player)
        {
            bool inLineOfSpellSight = player.InLineOfSpellSight;
            if (player.DistanceSqr <= DungeonBuddySettings.Instance.FollowingDistance * DungeonBuddySettings.Instance.FollowingDistance && inLineOfSpellSight)
                return WoWPoint.Zero;

            WoWPoint moveTo = player.Location;
            bool foundAdjustedPoint = false;
            if (inLineOfSpellSight)
            {
                for (float distance = 12f; distance <= 18f; distance += 1f)
                {
                    WoWPoint point = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, player.Location, distance);
                    if (Navigator.CanNavigateFully(StyxWoW.Me.Location, point))
                    {
                        foundAdjustedPoint = true;
                        moveTo = point;
                        break;
                    }
                }
            }

            if (!foundAdjustedPoint &&
                !Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo) &&
                player.DistanceSqr < 10000.0 &&
                Math.Abs(player.Z - StyxWoW.Me.Z) < 30f)
            {
                Navigator.PlayerMover.MoveTowards(new Tripper.XNAMath.Vector3(moveTo.X, moveTo.Y, moveTo.Z));
            }

            return moveTo;
        }

        // HB smethod_91
        private static bool ShouldRepairInSoloFarm(object context)
        {
            return StyxWoW.Me.LowestDurabilityPercent <= 0.1;
        }

        // HB smethod_92
        private static bool ShouldBuyDrinksInSoloFarm(object context)
        {
            return StyxWoW.Me.PowerType == WoWPowerType.Mana &&
                   CharacterSettings.Instance.DrinkAmount > 0 &&
                   !Consumable.GetDrinks().Any();
        }

        // HB smethod_93
        private static bool ShouldSellItemsInSoloFarm(object context)
        {
            return StyxWoW.Me.FreeBagSlots < DungeonBuddySettings.Instance.MinFreeBagSlots &&
                   GetItemsToSellCount() > 0;
        }

        // HB smethod_94
        private static bool ShouldTrainInSoloFarm(object context)
        {
            return Vendors.NeedClassTraining && LfgManager.DungeonCompleted;
        }

        // HB smethod_95
        private static bool IsInWrongInstanceInSoloFarm(object context)
        {
            if (LfgManager.CurrentLfgDungeonId == 0)
                return false;

            uint currentDungeonId = Bots.DungeonBuddy.Profiles.ProfileManager.GetLfgDungeonIdFromMapId(StyxWoW.Me.MapId);
            return currentDungeonId != LfgManager.CurrentLfgDungeonId;
        }

        // HB smethod_96
        private static bool ShouldSetSoloFarmKillPoi(object context)
        {
            return BotPoi.Current.Type != PoiType.Kill &&
                   Targeting.Instance.FirstUnit != null &&
                   Targeting.Instance.FirstUnit.IsAlive;
        }

        // HB smethod_97
        private static BotPoi CreateSoloFarmKillPoi(object context)
        {
            return new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill);
        }

        // HB smethod_98
        private static bool ShouldMoveInSoloFarm(object context)
        {
            return !StyxWoW.Me.IsActuallyInCombat;
        }

        private static int GetItemsToSellCount()
        {
            return StyxWoW.Me.CarriedItems.Count(item =>
                item.IsValid &&
                item.SellPrice > 0 &&
                item.Quality == WoWItemQuality.Poor &&
                !Styx.Logic.Profiles.ProtectedItemsManager.Contains(item.Entry));
        }

        // ═══════════════════════════════════════════════════════════
        // FOLLOW BEHAVIOR (DPS/Healer suit le Tank)
        // ═══════════════════════════════════════════════════════════

        private Composite CreateFollowBehavior()
        {
            return new Decorator(
                ctx => StyxWoW.Me.IsInInstance && !StyxWoW.Me.Combat &&
                       !StyxWoW.Me.IsTank(), // IsTank() = extension method (role-based via UnitGroupRolesAssigned)
                new Action(ctx =>
                {
                    var tank = Helpers.ScriptHelpers.Tank;
                    if (tank == null || !tank.IsAlive)
                        return RunStatus.Failure;
                    
                    float followDist = DungeonBuddySettings.Instance.FollowingDistance;
                    if (StyxWoW.Me.Location.DistanceSqr(tank.Location) > followDist * followDist)
                    {
                        Navigator.MoveTo(tank.Location);
                        return RunStatus.Running;
                    }
                    
                    return RunStatus.Failure;
                })
            );
        }
    }
}