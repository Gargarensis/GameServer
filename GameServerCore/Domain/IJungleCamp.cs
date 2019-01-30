using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using System.Collections.Generic;

namespace GameServerCore.Domain
{
    public interface IJungleCamp
    {
        float X { get; }
        float Y { get; }
        float Z { get; }
        string IconName { get; }
        CampId CampId { get; }
        List<string> SmallMonsters { get; set; }
        string BigMonster { get; set; }
        float FirstSpawnTime { get; set; }
        float RespawnTime { get; }
        float NextSpawnTime { get; set; }
        float SpawnDuration { get; set; }
        bool IsAlive { get; set; }
        int TimesCleared { get; }
        IMonster BigMonsterRef { get; set; }
        List<IMonster> SmallMonstersRef { get; set; }

        void CampCleared(float time);
        bool ShouldRespawn(float gameTime);
    }
}
