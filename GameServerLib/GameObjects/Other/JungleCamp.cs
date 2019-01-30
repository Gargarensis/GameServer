using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using System.Collections.Generic;

namespace LeagueSandbox.GameServer.GameObjects
{
    public class JungleCamp : IJungleCamp
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public string IconName { get; }
        public CampId CampId { get; }
        public List<string> SmallMonsters { get; set; }
        public string BigMonster { get; set; }
        public float FirstSpawnTime { get; set; }
        public float RespawnTime { get; }
        public float NextSpawnTime { get; set; }
        public bool IsAlive { get; set; }
        public int TimesCleared { get; private set; }
        public IMonster BigMonsterRef { get; set; }
        public List<IMonster> SmallMonstersRef { get; set; } = new List<IMonster>();

        public JungleCamp(float X, float Y, float Z, string iconName, CampId campId, List<string> smallMonsters, string bigMonster, float firstSpawnTime = 105 * 1000, float respawnTime = 60 * 1000)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            IconName = iconName;
            CampId = campId;
            BigMonster = bigMonster;
            SmallMonsters = smallMonsters;
            FirstSpawnTime = firstSpawnTime;
            RespawnTime = respawnTime;
            NextSpawnTime = firstSpawnTime;
            IsAlive = false;
        }

        public bool ShouldRespawn(float gameTime)
        {
            if (!IsAlive && gameTime >= NextSpawnTime)
            {
                NextSpawnTime = 0;
                IsAlive = true;

                return true;
            }

            return false;
        }

        public void CampCleared(float time)
        {
            IsAlive = false;
            NextSpawnTime = time + RespawnTime;
            TimesCleared++;

            for (int i = SmallMonstersRef.Count - 1; i >= 0; i--)
            {
                SmallMonstersRef[i].SetToRemove();
                SmallMonstersRef.RemoveAt(i);
            }

            if (BigMonsterRef is null)
                return;

            BigMonsterRef.SetToRemove();
            BigMonsterRef = null;
        }
    }
}
