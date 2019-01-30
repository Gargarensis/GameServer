using GameServerCore.Domain;
using GameServerCore.Enums;
using GameServerCore.Packets.Enums;

namespace PacketDefinitions420.PacketDefinitions.S2C
{
    public class CreateMonsterCamp : BasePacket
    {
        public CreateMonsterCamp(IJungleCamp jC)
            : base(PacketCmd.PKT_S2C_CREATE_MONSTER_CAMP)
        {
            Write(jC.X);
            Write(jC.Z); //are you sure they are like this?
            Write(jC.Y);
			WriteConstLengthString(jC.IconName, 64);
            Write((byte)jC.CampId);
            Write((byte) 0); // revealAudioVOComponentEvent
            Write((byte) 2); //SideTeamId 
            Write(jC.GetHashCode());
            Write(jC.FirstSpawnTime);

            /*buffer.Write((byte)0x64); // <-|
            buffer.Write((byte)0x15); //   |
            buffer.Write((byte)0xFB); //   |-> Unk
            buffer.Write((byte)0x41); //   |
            buffer.Write((byte)0x0C); // <-|*/
            //Fill(0, 5);
            //Write(unk);
        }
    }
}