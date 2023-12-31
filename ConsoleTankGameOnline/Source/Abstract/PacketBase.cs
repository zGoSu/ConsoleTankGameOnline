using Newtonsoft.Json;

namespace ConsoleTankGameOnline.Source.Abstract
{
    public abstract class PacketBase
    {
        public string Serialize() => JsonConvert.SerializeObject(this);

        public static PacketBase Deserialize(string json)
        {
            return (PacketBase)JsonConvert.DeserializeObject(json);
        }
    }
}
