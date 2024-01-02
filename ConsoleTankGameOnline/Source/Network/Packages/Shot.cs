using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class Shot : PacketBase
    {
        public Shot(string objectName)
        {
            ID = Enum.PacketEnum.Shot;
            Name = objectName;
        }

        public string Name { get; set; }
    }
}
