using ConsoleTankGameOnline.Source.Network.Packages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleTankGameOnline.Source.Abstract
{
    public class PacketBase
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public PacketBase? Deserialize(string json)
        {
            var jsonObject = JObject.Parse(json);
            var name = jsonObject["name"]?.ToString();

            return name switch
            {
                nameof(WorldInfo) => JsonConvert.DeserializeObject<WorldInfo>(json),
                nameof(PlayerInfo) => JsonConvert.DeserializeObject<PlayerInfo>(json),
                _ => JsonConvert.DeserializeObject<PacketBase>(json),
            };
        }

        public void HandlePacket()
        {
            switch (Name)
            {
                case nameof(WorldInfo):
                    Listener.SelectWorld(((WorldInfo)this).Path);
                    break;
                case nameof(PlayerInfo):
                    Listener.SelecterPlayer(((PlayerInfo)this).Path);
                    break;
                default:
                    break;
            }
        }
    }
}
