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
                PacketEnum.ShellMove => JsonConvert.DeserializeObject<ShellMove>(json),
                PacketEnum.Die => JsonConvert.DeserializeObject<Die>(json),
                PacketEnum.ShellDestroy => JsonConvert.DeserializeObject<ShellDestroy>(json),
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
                case PacketEnum.ShellMove:
                    var shell = (ShellMove)this;
                    Listener.ShellMoveTo(shell.Name, shell.Position, false);
                    break;
                case PacketEnum.Die:
                    var target = World.Instance.Objects[((Die)this).TargetName];
                    Listener.Die(target, false);
                    break;
                case PacketEnum.ShellDestroy:
                    Listener.ShellDestroy(((ShellDestroy)this).Name, false);
                    break;
                default:
                    break;
            }
        }
    }
}
