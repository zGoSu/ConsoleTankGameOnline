using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ConsoleTankGameOnline.Source.Network
{
    public class Server : INetwork, IDisposable
    {
        public Server()
        {
            _listener = new TcpListener(IPAddress.Any, 0);
            _externalAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(network => (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet) || (network.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                .Select(network => network.GetIPProperties().UnicastAddresses.FirstOrDefault(address =>
                (address.Address.AddressFamily == AddressFamily.InterNetwork) && address.Address.ToString().StartsWith("192.168")))
            .FirstOrDefault()?.Address?.ToString() ?? "127.0.0.1";

        }

        private readonly TcpListener _listener;
        private readonly List<Client> _clients = [];
        private readonly string _externalAddress;

        public IEnumerable<Client> Clients => _clients;

        public async Task Start()
        {
            try
            {
                _listener.Start();

                while (true)
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    var client = new Client(tcpClient, this);

                    _clients.Add(client);
                    Task.Run(client.ReceivePacket);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Write(ex);
            }
            finally
            {
                Dispose();
            }
        }

        public string GetCurrentAddress()
        {
            return $"{_externalAddress}:{((IPEndPoint)_listener.LocalEndpoint).Port}";
        }

        public async Task SendPacket(PacketBase packet)
        {
            foreach (var client in _clients)
            {
                await client.Writer.WriteLineAsync(packet.Serialize());
                await client.Writer.FlushAsync();
            }
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client);
        }

        public void Dispose()
        {
            foreach (var client in _clients)
            {
                client?.Dispose();
            }
            _listener?.Dispose();
        }
    }
}
