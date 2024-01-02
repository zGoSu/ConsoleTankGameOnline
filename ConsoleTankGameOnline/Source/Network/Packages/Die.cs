using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using Newtonsoft.Json;

namespace ConsoleTankGameOnline.Source.Network.Packages
{
    public class Die : PacketBase
    {
        public Die(CharacterBase target)
        {
            ID = Enum.PacketEnum.Die;
            Target = target;
            TargetName = target.Name;
        }

        public string TargetName { get; set; }
        [JsonIgnore]
        public CharacterBase Target { get; private set; }
    }
}
