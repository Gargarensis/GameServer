using System;
using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.Other;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace LeagueSandbox.GameServer.Maps
{
    internal class SummonersRift : IMapProperties
    {
        private static readonly List<Vector2> BlueTopWaypoints = new List<Vector2>
        {
            new Vector2(1510.0f, 2090.0f),
            new Vector2(1498.0f, 4041.0f),
            new Vector2(1184.0f, 6459.0f),
            new Vector2(1263.0f, 10371.0f),
            new Vector2(1432.0f, 11621.0f),
            new Vector2(2891.0f, 12967.0f),
            new Vector2(4308.0f, 13577.0f),
            new Vector2(7916.0f, 13672.0f),
            new Vector2(10260.0f, 13462.0f),
            new Vector2(11142.0f, 13236.0f),
            new Vector2(12357.0f, 13080.0f)
        };
        private static readonly List<Vector2> BlueBotWaypoints = new List<Vector2>
        {
            new Vector2(2188.0f, 1439.0f),
            new Vector2(4602.0f, 1450.0f),
            new Vector2(7173.0f, 1128.0f),
            new Vector2(11031.0f, 1336.0f),
            new Vector2(13150.0f, 3030.0f),
            new Vector2(13515.0f, 4206.0f),
            new Vector2(13646.0f, 7916.0f),
            new Vector2(13429.0f, 10159.0f),
            new Vector2(13074.0f, 12358.0f)
        };
        private static readonly List<Vector2> BlueMidWaypoints = new List<Vector2>
        {
            new Vector2(1964.0f, 2047.0f),
            new Vector2(2997.0f, 2781.0f),
            new Vector2(4144.0f, 3746.0f),
            new Vector2(4472.0f, 4727.0f),
            new Vector2(8375.0f, 8366.0f),
            new Vector2(10948.0f, 10821.0f),
            new Vector2(12511.0f, 12776.0f)
        };
        private static readonly List<Vector2> RedTopWaypoints = new List<Vector2>
        {
            new Vector2(12778.0f, 13406.0f),
            new Vector2(10499.0f, 13341.0f),
            new Vector2(9591.0f, 13338.0f),
            new Vector2(7940.0f, 13663.0f),
            new Vector2(4248.0f, 13537.0f),
            new Vector2(2281.0f, 12674.0f),
            new Vector2(1370.0f, 11675.0f),
            new Vector2(1230.0f, 10125.0f),
            new Vector2(1184.0f, 6525.0f),
            new Vector2(1448.0f, 4074.0f),
            new Vector2(1713.0f, 2562.0f)
        };
        private static readonly List<Vector2> RedBotWaypoints = new List<Vector2>
        {
            new Vector2(13331.0f, 12795.0f),
            new Vector2(13326.0f, 10281.0f),
            new Vector2(13629.0f, 7919.0f),
            new Vector2(13466.0f, 4219.0f),
            new Vector2(13160.0f, 3056.0f),
            new Vector2(10995.0f, 1331.0f),
            new Vector2(2456.0f, 1552.0f),
            new Vector2(4278.0f, 1524.0f),
            new Vector2(2456.0f, 1552.0f)
        };
        private static readonly List<Vector2> RedMidWaypoints = new List<Vector2>
        {
            new Vector2(12873.0f, 12915.0f),
            new Vector2(12057.0f, 11587.0f),
            new Vector2(11150.0f, 10773.0f),
            new Vector2(8375.0f, 8366.0f),
            new Vector2(4472.0f, 4727.0f),
            new Vector2(3628.0f, 3148.0f),
            new Vector2(2324.0f, 2087.0f)
        };

        private static readonly List<MinionSpawnType> RegularMinionWave = new List<MinionSpawnType>
        {
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER
        };
        private static readonly List<MinionSpawnType> CannonMinionWave = new List<MinionSpawnType>
        {
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_CANNON,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER
        };
        private static readonly List<MinionSpawnType> SuperMinionWave = new List<MinionSpawnType>
        {
            MinionSpawnType.MINION_TYPE_SUPER,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER
        };
        private static readonly List<MinionSpawnType> DoubleSuperMinionWave = new List<MinionSpawnType>
        {
            MinionSpawnType.MINION_TYPE_SUPER,
            MinionSpawnType.MINION_TYPE_SUPER,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_MELEE,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER,
            MinionSpawnType.MINION_TYPE_CASTER
        };

        private static readonly Dictionary<TeamId, Vector3> EndGameCameraPosition = new Dictionary<TeamId, Vector3>
        {
            { TeamId.TEAM_BLUE, new Vector3(1170, 1470, 188) },
            { TeamId.TEAM_PURPLE, new Vector3(12800, 13100, 110) }
        };

        private static readonly Dictionary<TeamId, ITarget> SpawnsByTeam = new Dictionary<TeamId, ITarget>
        {
            {TeamId.TEAM_BLUE, new Target(25.90f, 280)},
            {TeamId.TEAM_PURPLE, new Target(13948, 14202)}
        };

        private static readonly Dictionary<TurretType, int[]> TurretItems = new Dictionary<TurretType, int[]>
        {
            { TurretType.OUTER_TURRET, new[] { 1500, 1501, 1502, 1503 } },
            { TurretType.INNER_TURRET, new[] { 1500, 1501, 1502, 1503, 1504 } },
            { TurretType.INHIBITOR_TURRET, new[] { 1501, 1502, 1503, 1505 } },
            { TurretType.NEXUS_TURRET, new[] { 1501, 1502, 1503, 1505 } }
        };


        private Game _game;
        private int _cannonMinionCount;
        private int _minionNumber;
        private readonly long _firstSpawnTime = 90 * 1000;
        private long _nextSpawnTime = 90 * 1000;
        private readonly long _spawnInterval = 30 * 1000;
        private readonly Dictionary<TeamId, Fountain> _fountains;

        public List<int> ExpToLevelUp { get; set; } = new List<int>
        {
            0,
            280,
            660,
            1140,
            1720,
            2400,
            3180,
            4060,
            5040,
            6120,
            7300,
            8580,
            9960,
            11440,
            13020,
            14700,
            16480,
            18360
        };

        public float GoldPerSecond { get; set; } = 1.9f;
        public float StartingGold { get; set; } = 475.0f;
        public bool HasFirstBloodHappened { get; set; } = false;
        public bool IsKillGoldRewardReductionActive { get; set; } = true;
        public int BluePillId { get; set; } = 2001;
        public long FirstGoldTime { get; set; } = 90 * 1000;
        public bool SpawnEnabled { get; set; }
        public SummonersRift(Game game)
        {
            _game = game;
            _fountains = new Dictionary<TeamId, Fountain>
            {
                { TeamId.TEAM_BLUE, new Fountain(game, TeamId.TEAM_BLUE, 11, 250, 1000) },
                { TeamId.TEAM_PURPLE, new Fountain(game, TeamId.TEAM_PURPLE, 13950, 14200, 1000) }
            };
            SpawnEnabled = _game.Config.MinionSpawnsEnabled;
        }

        public int[] GetTurretItems(TurretType type)
        {
            if (!TurretItems.ContainsKey(type))
            {
                return null;
            }

            return TurretItems[type];
        }

        public void Init()
        {
            // Announcer events
            _game.Map.AnnouncerEvents.Add(new Announce(_game, 30 * 1000, Announces.WELCOME_TO_SR, true)); // Welcome to SR
            if (_firstSpawnTime - 30 * 1000 >= 0.0f)
                _game.Map.AnnouncerEvents.Add(new Announce(_game, _firstSpawnTime - 30 * 1000, Announces.THIRY_SECONDS_TO_MINIONS_SPAWN, true)); // 30 seconds until minions spawn
            _game.Map.AnnouncerEvents.Add(new Announce(_game, _firstSpawnTime, Announces.MINIONS_HAVE_SPAWNED, false)); // Minions have spawned (90 * 1000)
            _game.Map.AnnouncerEvents.Add(new Announce(_game, _firstSpawnTime, Announces.MINIONS_HAVE_SPAWNED2, false)); // Minions have spawned [2] (90 * 1000)

            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_R_03_A", 10500.62f, 1217.73f, TeamId.TEAM_BLUE,
                TurretType.OUTER_TURRET, GetTurretItems(TurretType.OUTER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_R_02_A", 6990.53f, 1688.62f, TeamId.TEAM_BLUE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_07_A", 4275.26f, 1440.04f, TeamId.TEAM_BLUE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_R_03_A", 13746.0f, 4339.0f, TeamId.TEAM_PURPLE,
                TurretType.OUTER_TURRET, GetTurretItems(TurretType.OUTER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_R_02_A", 13363.0f, 8037.0f, TeamId.TEAM_PURPLE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_R_01_A", 13484.0f, 10387.0f, TeamId.TEAM_PURPLE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_05_A", 5950.02f, 6481.10f, TeamId.TEAM_BLUE,
                TurretType.OUTER_TURRET, GetTurretItems(TurretType.OUTER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_04_A", 5125.66f, 4918.91f, TeamId.TEAM_BLUE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_03_A", 3750.99f, 3785.24f, TeamId.TEAM_BLUE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_01_A", 1850.63f, 2450.98f, TeamId.TEAM_BLUE,
                TurretType.NEXUS_TURRET, GetTurretItems(TurretType.NEXUS_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_02_A", 2386.19f, 1985.47f, TeamId.TEAM_BLUE,
                TurretType.NEXUS_TURRET, GetTurretItems(TurretType.NEXUS_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_05_A", 8752.0f, 8323.0f, TeamId.TEAM_PURPLE,
               TurretType.OUTER_TURRET, GetTurretItems(TurretType.OUTER_TURRET)));
            //_game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_05_A", 8952.0f, 8523.0f, TeamId.TEAM_PURPLE,
            //   TurretType.OUTER_TURRET, GetTurretItems(TurretType.OUTER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_04_A", 9568.0f, 9888.0f, TeamId.TEAM_PURPLE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_03_A", 10937.0f, 10989.0f, TeamId.TEAM_PURPLE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_01_A", 12862.0f, 12428.0f, TeamId.TEAM_PURPLE,
                TurretType.NEXUS_TURRET, GetTurretItems(TurretType.NEXUS_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_C_02_A", 12412.0f, 12883.0f, TeamId.TEAM_PURPLE,
                TurretType.NEXUS_TURRET, GetTurretItems(TurretType.NEXUS_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_OrderTurretShrine_A", -436.05f, -253.32f, TeamId.TEAM_BLUE,
                TurretType.FOUNTAIN_TURRET, GetTurretItems(TurretType.FOUNTAIN_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_ChaosTurretShrine_A", 14490.0f, 14750.0f, TeamId.TEAM_PURPLE,
                TurretType.FOUNTAIN_TURRET, GetTurretItems(TurretType.FOUNTAIN_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_L_03_A", 1180.66f, 10450.47f, TeamId.TEAM_BLUE,
                TurretType.OUTER_TURRET, TurretItems[TurretType.OUTER_TURRET]));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_L_02_A", 1306.26f, 6714.25f, TeamId.TEAM_BLUE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T1_C_06_A", 1161.81f, 4463.36f, TeamId.TEAM_BLUE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_L_03_A", 4166.0f, 13787.0f, TeamId.TEAM_PURPLE,
                TurretType.OUTER_TURRET, TurretItems[TurretType.OUTER_TURRET]));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_L_02_A", 7788.0f, 13274.0f, TeamId.TEAM_PURPLE,
                TurretType.INNER_TURRET, GetTurretItems(TurretType.INNER_TURRET)));
            _game.ObjectManager.AddObject(new LaneTurret(_game, "Turret_T2_L_01_A", 10211.0f, 13375.0f, TeamId.TEAM_PURPLE,
                TurretType.INHIBITOR_TURRET, GetTurretItems(TurretType.INHIBITOR_TURRET)));

            _game.ObjectManager.AddObject(new LevelProp(_game, 13374.17f, 14245.673f, 194.9741f, 224.0f, 33.33f, 0.0f, 0.0f, -44.44f, "LevelProp_SRU_storeKeeperNorth", "SRU_storeKeeperNorth"));
            _game.ObjectManager.AddObject(new LevelProp(_game, -20.5613f, 755.6632f, 191.4039f, 250.33f, 0.0f, 0.0f, 0.0f, 0.0f, "LevelProp_SRU_storeKeeperSouth", "SRU_storeKeeperSouth"));

            //TODO
            var collisionRadius = 250;
            var sightRange = 1700;

            _game.ObjectManager.AddObject(new Inhibitor(_game, "OrderInhibitor", TeamId.TEAM_BLUE, collisionRadius, 1168, 3782, sightRange, 0xffd23c3e)); //top
            _game.ObjectManager.AddObject(new Inhibitor(_game, "OrderInhibitor", TeamId.TEAM_BLUE, collisionRadius, 3330, 3330, sightRange, 0xff4a20f1)); //mid
            _game.ObjectManager.AddObject(new Inhibitor(_game, "OrderInhibitor", TeamId.TEAM_BLUE, collisionRadius, 3682, 1247, sightRange, 0xff9303e1)); //bot
            _game.ObjectManager.AddObject(new Inhibitor(_game, "ChaosInhibitor", TeamId.TEAM_PURPLE, collisionRadius, 10960, 13450, sightRange, 0xff6793d0)); //top
            _game.ObjectManager.AddObject(new Inhibitor(_game, "ChaosInhibitor", TeamId.TEAM_PURPLE, collisionRadius, 11240, 11490, sightRange, 0xffff8f1f)); //mid
            _game.ObjectManager.AddObject(new Inhibitor(_game, "ChaosInhibitor", TeamId.TEAM_PURPLE, collisionRadius, 13544, 11028, sightRange, 0xff26ac0f)); //bot

            _game.ObjectManager.AddObject(new Nexus(_game, "OrderNexus", TeamId.TEAM_BLUE, collisionRadius * 2, 1850, 2150, sightRange, 0xfff97db5));
            _game.ObjectManager.AddObject(new Nexus(_game, "ChaosNexus", TeamId.TEAM_PURPLE, collisionRadius * 2, 12950, 12900, sightRange, 0xfff02c0f));
        }

        public void Update(float diff)
        {
            if (_game.GameTime >= 120 * 1000)
            {
                IsKillGoldRewardReductionActive = false;
            }

            if (SpawnEnabled)
            {
                if (_minionNumber > 0)
                {
                    if (_game.GameTime >= _nextSpawnTime + _minionNumber * 8 * 100)
                    { // Spawn new wave every 0.8s
                        if (Spawn())
                        {
                            _minionNumber = 0;
                            _nextSpawnTime += _spawnInterval;
                        }
                        else
                        {
                            _minionNumber++;
                        }
                    }
                }
                else if (_game.GameTime >= _nextSpawnTime)
                {
                    Spawn();
                    _minionNumber++;
                }
            }

            foreach (var fountain in _fountains.Values)
            {
                fountain.Update(diff);
            }
        }

        public ITarget GetRespawnLocation(TeamId team)
        {
            if (!SpawnsByTeam.ContainsKey(team))
            {
                return new Target(25.90f, 280);
            }

            return SpawnsByTeam[team];
        }

        public string GetMinionModel(TeamId team, MinionSpawnType type)
        {
            var teamDictionary = new Dictionary<TeamId, string>
            {
                {TeamId.TEAM_BLUE, "Blue"},
                {TeamId.TEAM_PURPLE, "Red"}
            };

            var typeDictionary = new Dictionary<MinionSpawnType, string>
            {
                {MinionSpawnType.MINION_TYPE_MELEE, "Basic"},
                {MinionSpawnType.MINION_TYPE_CASTER, "Wizard"},
                {MinionSpawnType.MINION_TYPE_CANNON, "MechCannon"},
                {MinionSpawnType.MINION_TYPE_SUPER, "MechMelee"}
            };

            if (!teamDictionary.ContainsKey(team) || !typeDictionary.ContainsKey(type))
            {
                return string.Empty;
            }

            return $"{teamDictionary[team]}_Minion_{typeDictionary[type]}";
        }

        public float GetGoldFor(IAttackableUnit u)
        {
            if (!(u is ILaneMinion m))
            {
                if (!(u is IChampion c))
                {
                    return 0.0f;
                }

                var gold = 300.0f; //normal gold for a kill
                if (c.KillDeathCounter < 5 && c.KillDeathCounter >= 0)
                {
                    if (c.KillDeathCounter == 0)
                    {
                        return gold;
                    }

                    for (var i = c.KillDeathCounter; i > 1; --i)
                    {
                        gold += gold * 0.165f;
                    }

                    return gold;
                }

                if (c.KillDeathCounter >= 5)
                {
                    return 500.0f;
                }

                if (c.KillDeathCounter >= 0)
                    return 0.0f;
                
                var firstDeathGold = gold - gold * 0.085f;

                if (c.KillDeathCounter == -1)
                {
                    return firstDeathGold;
                }

                for (var i = c.KillDeathCounter; i < -1; ++i)
                {
                    firstDeathGold -= firstDeathGold * 0.2f;
                }

                if (firstDeathGold < 50)
                {
                    firstDeathGold = 50;
                }

                return firstDeathGold;
            }

            var dic = new Dictionary<MinionSpawnType, float>
            {
                { MinionSpawnType.MINION_TYPE_MELEE, 19.8f + 0.2f * (int)(_game.GameTime / (90 * 1000)) },
                { MinionSpawnType.MINION_TYPE_CASTER, 16.8f + 0.2f * (int)(_game.GameTime / (90 * 1000)) },
                { MinionSpawnType.MINION_TYPE_CANNON, 40.0f + 0.5f * (int)(_game.GameTime / (90 * 1000)) },
                { MinionSpawnType.MINION_TYPE_SUPER, 40.0f + 1.0f * (int)(_game.GameTime / (180 * 1000)) }
            };

            if (!dic.ContainsKey(m.MinionSpawnType))
            {
                return 0.0f;
            }

            return dic[m.MinionSpawnType];
        }

        public float GetExperienceFor(IAttackableUnit u)
        {
            if (!(u is ILaneMinion m))
            {
                return 0.0f;
            }

            var dic = new Dictionary<MinionSpawnType, float>
            {
                { MinionSpawnType.MINION_TYPE_MELEE, 64.0f },
                { MinionSpawnType.MINION_TYPE_CASTER, 32.0f },
                { MinionSpawnType.MINION_TYPE_CANNON, 92.0f },
                { MinionSpawnType.MINION_TYPE_SUPER, 97.0f }
            };

            if (!dic.ContainsKey(m.MinionSpawnType))
            {
                return 0.0f;
            }

            return dic[m.MinionSpawnType];
        }

        public Tuple<TeamId, Vector2> GetMinionSpawnPosition(MinionSpawnPosition spawnPosition)
        {
            switch (spawnPosition)
            {
                case MinionSpawnPosition.SPAWN_BLUE_TOP:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_BLUE, new Vector2(1483, 2108));
                case MinionSpawnPosition.SPAWN_BLUE_BOT:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_BLUE, new Vector2(2155, 1467));
                case MinionSpawnPosition.SPAWN_BLUE_MID:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_BLUE, new Vector2(1982, 1957));
                case MinionSpawnPosition.SPAWN_RED_TOP:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_PURPLE, new Vector2(12645, 13473));
                case MinionSpawnPosition.SPAWN_RED_BOT:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_PURPLE, new Vector2(13370, 12572));
                case MinionSpawnPosition.SPAWN_RED_MID:
                    return new Tuple<TeamId, Vector2>(TeamId.TEAM_PURPLE, new Vector2(12662, 12686));
            }
            return new Tuple<TeamId, Vector2>(0, new Vector2());
        }

        public void SetMinionStats(ILaneMinion m)
        {
            // Same for all minions
            m.Stats.MoveSpeed.BaseValue = 325.0f;

            switch (m.MinionSpawnType)
            {
                case MinionSpawnType.MINION_TYPE_MELEE:
                    m.Stats.CurrentHealth = 475.0f + 20.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.HealthPoints.BaseValue = 475.0f + 20.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.AttackDamage.BaseValue = 12.0f + 1.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.Range.BaseValue = 180.0f;
                    m.Stats.AttackSpeedFlat = 1.250f;
                    m.AutoAttackDelay = 11.8f / 30.0f;
                    m.IsMelee = true;
                    break;
                case MinionSpawnType.MINION_TYPE_CASTER:
                    m.Stats.CurrentHealth = 279.0f + 7.5f * (int)(_game.GameTime / (90 * 1000));
                    m.Stats.HealthPoints.BaseValue = 279.0f + 7.5f * (int)(_game.GameTime / (90 * 1000));
                    m.Stats.AttackDamage.BaseValue = 23.0f + 1.0f * (int)(_game.GameTime / (90 * 1000));
                    m.Stats.Range.BaseValue = 600.0f;
                    m.Stats.AttackSpeedFlat = 0.670f;
                    m.AutoAttackDelay = 14.1f / 30.0f;
                    m.AutoAttackProjectileSpeed = 650.0f;
                    break;
                case MinionSpawnType.MINION_TYPE_CANNON:
                    m.Stats.CurrentHealth = 700.0f + 27.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.HealthPoints.BaseValue = 700.0f + 27.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.AttackDamage.BaseValue = 40.0f + 3.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.Range.BaseValue = 450.0f;
                    m.Stats.AttackSpeedFlat = 1.0f;
                    m.AutoAttackDelay = 9.0f / 30.0f;
                    m.AutoAttackProjectileSpeed = 1200.0f;
                    break;
                case MinionSpawnType.MINION_TYPE_SUPER:
                    m.Stats.CurrentHealth = 1500.0f + 200.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.HealthPoints.BaseValue = 1500.0f + 200.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.AttackDamage.BaseValue = 190.0f + 10.0f * (int)(_game.GameTime / (180 * 1000));
                    m.Stats.Range.BaseValue = 170.0f;
                    m.Stats.AttackSpeedFlat = 0.694f;
                    m.Stats.Armor.BaseValue = 30.0f;
                    m.Stats.MagicResist.BaseValue = -30.0f;
                    m.IsMelee = true;
                    m.AutoAttackDelay = 15.0f / 30.0f;
                    break;
            }
        }

        public void SpawnMinion(List<MinionSpawnType> list, int minionNo, MinionSpawnPosition pos, List<Vector2> waypoints)
        {
            if (list.Count <= minionNo)
            {
                return;
            }

            var m = new LaneMinion(_game, list[minionNo], pos, waypoints);
            _game.ObjectManager.AddObject(m);
        }

        public bool Spawn()
        {
            var positions = new List<MinionSpawnPosition>
            {
                MinionSpawnPosition.SPAWN_BLUE_TOP,
                MinionSpawnPosition.SPAWN_BLUE_BOT,
                MinionSpawnPosition.SPAWN_BLUE_MID,
                MinionSpawnPosition.SPAWN_RED_TOP,
                MinionSpawnPosition.SPAWN_RED_BOT,
                MinionSpawnPosition.SPAWN_RED_MID
            };

            var cannonMinionTimestamps = new List<Tuple<long, int>>
            {
                new Tuple<long, int>(0, 2),
                new Tuple<long, int>(20 * 60 * 1000, 1),
                new Tuple<long, int>(35 * 60 * 1000, 0)
            };

            var spawnToWaypoints = new Dictionary<MinionSpawnPosition, Tuple<List<Vector2>, uint>>
            {
                {MinionSpawnPosition.SPAWN_BLUE_BOT, Tuple.Create(BlueBotWaypoints, 0xff26ac0f)},
                {MinionSpawnPosition.SPAWN_BLUE_MID, Tuple.Create(BlueMidWaypoints, 0xffff8f1f)},
                {MinionSpawnPosition.SPAWN_BLUE_TOP, Tuple.Create(BlueTopWaypoints, 0xff6793d0)},
                {MinionSpawnPosition.SPAWN_RED_BOT, Tuple.Create(RedBotWaypoints, 0xff9303e1)},
                {MinionSpawnPosition.SPAWN_RED_MID, Tuple.Create(RedMidWaypoints, 0xff4a20f1)},
                {MinionSpawnPosition.SPAWN_RED_TOP, Tuple.Create(RedTopWaypoints, 0xffd23c3e)}
            };
            var cannonMinionCap = 2;

            foreach (var timestamp in cannonMinionTimestamps)
            {
                if (_game.GameTime >= timestamp.Item1)
                {
                    cannonMinionCap = timestamp.Item2;
                }
            }

            foreach (var pos in positions)
            {
                var waypoints = spawnToWaypoints[pos].Item1;
                var inhibitorId = spawnToWaypoints[pos].Item2;
                var inhibitor = _game.ObjectManager.GetInhibitorById(inhibitorId);
                var isInhibitorDead = inhibitor.InhibitorState == InhibitorState.DEAD && !inhibitor.RespawnAnnounced;

                var oppositeTeam = TeamId.TEAM_BLUE;
                if (inhibitor.Team == TeamId.TEAM_PURPLE)
                {
                    oppositeTeam = TeamId.TEAM_PURPLE;
                }

                var areAllInhibitorsDead = _game.ObjectManager.AllInhibitorsDestroyedFromTeam(oppositeTeam) && !inhibitor.RespawnAnnounced;

                var list = RegularMinionWave;
                if (_cannonMinionCount >= cannonMinionCap)
                {
                    list = CannonMinionWave;
                }

                if (isInhibitorDead)
                {
                    list = SuperMinionWave;
                }

                if (areAllInhibitorsDead)
                {
                    list = DoubleSuperMinionWave;
                }

                SpawnMinion(list, _minionNumber, pos, waypoints);
            }


            if (_minionNumber < 8)
            {
                return false;
            }

            if (_cannonMinionCount >= cannonMinionCap)
            {
                _cannonMinionCount = 0;
            }
            else
            {
                _cannonMinionCount++;
            }
            return true;
        }

        public Vector3 GetEndGameCameraPosition(TeamId team)
        {
            if (!EndGameCameraPosition.ContainsKey(team))
            {
                return new Vector3(0, 0, 0);
            }

            return EndGameCameraPosition[team];
        }
    }
}
