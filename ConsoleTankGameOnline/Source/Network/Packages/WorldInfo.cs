using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class WorldInfo : PacketBase
    {
        public WorldInfo(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}
