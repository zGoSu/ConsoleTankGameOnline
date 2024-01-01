using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class PlayerInfo : PacketBase
    {
        public PlayerInfo(CharacterBase character) 
        {
            ID = Enum.PacketEnum.PlayerInfo;
            Character = character;
        }

        public CharacterBase Character;
    }
}
