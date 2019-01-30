using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Packets.Enums;

namespace PacketDefinitions420.PacketDefinitions.S2C
{
    public class EmptyNeutralCamp : BasePacket
    {
        public EmptyNeutralCamp(IJungleCamp jungleCamp, IChampion killer)
            : base(PacketCmd.PKT_S2C_NEUTRAL_CAMP_EMPTY, killer is null ? 0 : killer.NetId)
        {
            Write((int)jungleCamp.CampId); // camp index
            Write(jungleCamp.GetHashCode()); //TimerType
            Write(jungleCamp.NextSpawnTime); // TimerExpire (for baron -> 1200, dragon -> 150)
            Write(false); // DoPlayVo
        }
    }
}