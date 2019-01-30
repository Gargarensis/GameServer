using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Packets.Enums;

namespace PacketDefinitions420.PacketDefinitions.S2C
{
    public class NeutralMinionTimerUpdate : BasePacket
    {
        public NeutralMinionTimerUpdate(IJungleCamp jungleCamp)
            : base(PacketCmd.PKT_S2C_NEUTRAL_MINION_TIMER_UPDATE)
        {
            // maybe used when an enemy champion spots a jungle camp that he did not know was cleared so he needs the timer
            Write(jungleCamp.GetHashCode()); //Type Hash -> TimerType?
            Write(jungleCamp.NextSpawnTime); // Expire -> TimerExpire?
        }
    }
}