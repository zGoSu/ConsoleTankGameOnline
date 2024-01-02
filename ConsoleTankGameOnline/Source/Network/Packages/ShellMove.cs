using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class ShellMove : PacketBase
    {
        public ShellMove(string ownerName, Position shellPosition)
        {
            ID = Enum.PacketEnum.ShellMove;
            Name = ownerName;
            Position = shellPosition;
        }

        public string Name { get; set; }
        public Position Position { get; set; }
    }
}
