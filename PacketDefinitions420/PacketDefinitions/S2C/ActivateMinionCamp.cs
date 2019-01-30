using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Packets.Enums;

namespace PacketDefinitions420.PacketDefinitions.S2C
{
    public class ActivateMinionCamp : BasePacket
    {
        public ActivateMinionCamp(IJungleCamp jungleCamp)
            : base(PacketCmd.PKT_S2C_ACTIVATE_MINION_CAMP)
        {
            Write(jungleCamp.X);
            Write(jungleCamp.Y);
            Write(jungleCamp.Z);
            Write(jungleCamp.SpawnDuration); // spawn duration, how much time does it take for the spawn animation?
            Write((byte)jungleCamp.CampId);  // Camp index
            Write(jungleCamp.GetHashCode()); // Timer Type
        }
    }
}