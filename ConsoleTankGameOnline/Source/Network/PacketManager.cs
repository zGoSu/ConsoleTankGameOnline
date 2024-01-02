using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Network.Packages;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source.Network
{
    public class PacketManager
    {
        public PacketManager(INetwork network)
        {
            _network = network;
            EventInitialization();
        }

        private readonly INetwork? _network;

        private void EventInitialization()
        {
            Listener.OnPlayerAdded += Listener_OnPlayerAdded;
            Listener.OnMove += Listener_OnMove;
            Listener.OnShellMove += Listener_OnShellMove;
            Listener.OnDie += Listener_OnDie;
            Listener.OnShellDestroyed += Listener_OnShellDestroyed;
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

        private void Listener_OnMove(string objectName, Position position, bool sendPacket)
        {
            if (!sendPacket)
            {
                return;
            }

            SendPacket(new Move(objectName, position));
        }

        private void Listener_OnDie(CharacterBase character, bool sendPacket)
        {
            if (!sendPacket)
            {
                return;
            }

            SendPacket(new Die(character.Name));
        }

        private void Listener_OnShellMove(string objectName, Position position, bool sendPacket)
        {
            if (!sendPacket)
            {
                return;
            }

            SendPacket(new ShellMove(objectName, position));
        }

        private void Listener_OnShellDestroyed(string ownerName, bool sendPacket)
        {
            if (!sendPacket)
            {
                return;
            }

            SendPacket(new ShellDestroy(ownerName));
        }
    }
}
