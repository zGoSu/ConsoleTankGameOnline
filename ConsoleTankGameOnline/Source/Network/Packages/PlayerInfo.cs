using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    internal class PlayerInfo : PacketBase
    {
        public PlayerInfo(string skin) 
        {
            Name = nameof(PlayerInfo);
            Path = skin;
        }

        public string Path { get; set; }
    }
}
