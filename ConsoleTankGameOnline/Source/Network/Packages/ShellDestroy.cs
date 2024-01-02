using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class ShellDestroy : PacketBase
    {
        public ShellDestroy(string ownerName) 
        {
            ID = Enum.PacketEnum.ShellDestroy;
            Name = ownerName;
        }

        public string Name { get; set; }
    }
}
