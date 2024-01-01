using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class WorldInfo : PacketBase
    {
        public WorldInfo(char[,] map)
        {
            Map = map;
            ID = Enum.PacketEnum.WorldInfo;
        }

        public char[,] Map { get; set; }
    }
}
