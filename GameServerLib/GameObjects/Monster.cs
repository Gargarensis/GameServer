using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace LeagueSandbox.GameServer.GameObjects
{
    public class Monster : Minion, IMonster
    {
        public Vector2 Facing { get; private set; }
        public string Name { get; private set; }
        public string SpawnAnimation { get; private set; }
        public byte CampId { get; private set; }
        public byte CampUnk { get; private set; }
        public float SpawnAnimationTime { get; private set; }
        public bool IsEngaged { get; set; }
        public Vector2 OriginalPosition { get; }
        public bool IsEvading { get; set; }

        public Monster(
            Game game,
            float x,
            float y,
            float facingX,
            float facingY,
            string model,
            string name,
            string spawnAnimation = "",
            byte campId = 0x01,
            byte campUnk = 0x2A,
            float spawnAnimationTime = 0.0f,
            uint netId = 0
        ) : base(game, null, x, y, model, name, 0, netId)
        {
            SetTeam(TeamId.TEAM_NEUTRAL);

            var teams = Enum.GetValues(typeof(TeamId)).Cast<TeamId>();
            foreach (var team in teams)
            {
                SetVisibleByTeam(team, true);
            }

            MoveOrder = MoveOrder.MOVE_ORDER_MOVE;
            Facing = new Vector2(facingX, facingY);
            Name = name;
            SpawnAnimation = spawnAnimation;
            CampId = campId;
            CampUnk = campUnk;
            SpawnAnimationTime = spawnAnimationTime;
            OriginalPosition = new Vector2(x, y);
        }

        public override void OnAdded()
        {
            //base.OnAdded();
            _game.PacketNotifier.NotifySpawn(this);
        }

        public override void Update(float diff)
        {
            base.Update(diff);
            if (!IsDead && IsEngaged)
            {
                Logger.Debug("ENGAGED");
                if (IsDashing || _aiPaused)
                {
                    Logger.Debug("AIPAUSED");
                    Replication.Update();
                    return;
                }

                if (!IsEvading && ScanForNearestTarget())
                {
                    Logger.Debug("SCANNING");
                    if (!RecalculateAttackPosition())
                    {
                        Logger.Debug("KEEPFOCUSING");
                        KeepFocusingTarget();
                    }
                }
            }

            if (IsEvading)
                Logger.Debug("EVADING");

            if (IsEvading && GetPosition().X == OriginalPosition.X && GetPosition().Y == OriginalPosition.Y)
                IsEvading = false;

            if (IsEvading)
                KeepEvading();

            Replication.Update();
        }

        private void KeepEvading()
        {
            SetWaypoints(new List<Vector2> { GetPosition(), OriginalPosition });
            if (Stats.CurrentHealth < Stats.HealthPoints.Total)
            {
                Stats.CurrentHealth += 50;
                if (Stats.CurrentHealth > Stats.HealthPoints.Total)
                    Stats.CurrentHealth = Stats.HealthPoints.Total;
            }
        }

        private bool ScanForNearestTarget()
        {
            IAttackableUnit nextTarget = null;
            var objects = _game.ObjectManager.GetObjects();
            foreach (var it in objects.OrderBy(x => GetDistanceTo(x.Value)))
            {
                if (!(it.Value is IAttackableUnit u) || it.Value is ILaneMinion || u.IsDead || u.Team == Team)
                    continue;

                    nextTarget = u;
                    break;
            }
            if (nextTarget != null)
            {
                TargetUnit = nextTarget;
                _game.PacketNotifier.NotifySetTarget(this, nextTarget);
                return true;
            }
            _game.PacketNotifier.NotifyStopAutoAttack(this);
            IsAttacking = false;
            IsEvading = true;
            return false;
        }

        protected new void KeepFocusingTarget()
        {
            if (IsAttacking && (TargetUnit == null || TargetUnit.IsDead))
            // If target is dead or out of range
            {
                _game.PacketNotifier.NotifyStopAutoAttack(this);
                IsAttacking = false;
                IsEvading = true;
            }
        }

        public override void TakeDamage(IAttackableUnit attacker, float damage, DamageType type, DamageSource source, DamageText damageText)
        {
            base.TakeDamage(attacker, damage, type, source, damageText);
            Logger.Debug("TOOK DAMAGE!");
            if (!IsEvading)
                IsEngaged = true;
        }
    }
}
