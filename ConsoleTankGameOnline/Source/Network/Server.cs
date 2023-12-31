using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using System.Net;
using System.Net.Sockets;

namespace ConsoleTankGameOnline.Source.Network
{
    public class Server : INetwork, IDisposable
    {
        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        private readonly TcpListener _listener;
        private readonly List<Client> _clients = [];
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
                    Task.Run(client.Process);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server.Start: {ex.Message}");
            }
            finally
            {
                Dispose();
            }
        }

        public string GetCurrentAddress()
        {
            return $"{((IPEndPoint)_listener.LocalEndpoint).Address}:{((IPEndPoint)_listener.LocalEndpoint).Port}";
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
