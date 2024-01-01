using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Network.Packages;

namespace ConsoleTankGameOnline.Source.Network
{
    public class PacketManager
    {
        public PacketManager(INetwork network)
        {
            _network = network;
            EventInitialization();
        }

        public readonly INetwork? _network;

        private void EventInitialization()
        {
            Listener.OnPlayerAdded += Listener_OnPlayerAdded;
        }
        public void SendPacket(PacketBase packet)
        {
            if (_network == null)
            {
                return;
            }

            _network.SendPacket(packet);
        }

        private void Listener_OnPlayerAdded(CharacterBase character, bool sendPacket)
        {
            if (!sendPacket)
            {
                return;
            }

            SendPacket(new PlayerInfo(character));
        }

    }
}
