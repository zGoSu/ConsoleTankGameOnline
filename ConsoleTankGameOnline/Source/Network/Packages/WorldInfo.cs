using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class WorldInfo : PacketBase
    {
        public WorldInfo(string path)
        {
            Path = path;
            Name = nameof(WorldInfo);
        }

        public string Path { get; set; }
    }
}
