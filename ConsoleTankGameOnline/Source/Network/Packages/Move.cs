using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class Move : PacketBase
    {
        public Move(string objectName, Position position)
        {
            ID = Enum.PacketEnum.Move;
            ObjectName = objectName;
            Position = position;
        }

        public string ObjectName { get; set; }
        public Position Position { get; set; }
    }
}
