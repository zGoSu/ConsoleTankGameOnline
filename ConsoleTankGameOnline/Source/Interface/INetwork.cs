using ConsoleTankGameOnline.Source.Abstract;

namespace ConsoleTankGameOnline.Source.Interface
{
    public interface INetwork
    {
        public Task Start();
        public Task SendPacket(PacketBase packet);
    }
}
