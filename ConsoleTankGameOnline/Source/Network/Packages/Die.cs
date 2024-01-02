using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class Die : PacketBase
    {
        public Die(string targetName)
        {
            ID = Enum.PacketEnum.Die;
            TargetName = targetName;
        }

        public string TargetName { get; set; }
    }
}
