using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Network.Packages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleTankGameOnline.Source.Abstract
{
    public class PacketBase
    {
        [JsonProperty("id")]
        public PacketEnum ID { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public PacketBase? Deserialize(string json)
        {
            var jsonObject = JObject.Parse(json);
            var id = (PacketEnum)Convert.ToByte(jsonObject["id"]);

            return id switch
            {
                PacketEnum.WorldInfo => JsonConvert.DeserializeObject<WorldInfo>(json),
                PacketEnum.PlayerInfo => JsonConvert.DeserializeObject<PlayerInfo>(json),
                PacketEnum.Move => JsonConvert.DeserializeObject<Move>(json),
                PacketEnum.Shot => JsonConvert.DeserializeObject<Shot>(json),
                _ => JsonConvert.DeserializeObject<PacketBase>(json),
            };
        }

        public void HandlePacket()
        {
            switch (ID)
            {
                case PacketEnum.WorldInfo:
                    Listener.CreateWorld(((WorldInfo)this).Map);
                    break;
                case PacketEnum.PlayerInfo:
                    Listener.AddPlayer(((PlayerInfo)this).Character, false);
                    break;
                case PacketEnum.Move:
                    var character = (Move)this;
                    Listener.MoveTo(character.ObjectName, character.Position, false);
                    break;
                case PacketEnum.Shot:
                    Listener.Shot(((Shot)this).Name, false);
                    break;
                default:
                    break;
            }
        }
    }
}
