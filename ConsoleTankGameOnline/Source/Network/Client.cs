using ConsoleTankGameOnline.Source.Abstract;
using ConsoleTankGameOnline.Source.Interface;
using System.Net.Sockets;

namespace ConsoleTankGameOnline.Source.Network
{
    public class Client : INetwork, IDisposable
    {
        public Client(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
            _reader = new StreamReader(client.GetStream());
            Writer = new StreamWriter(client.GetStream());

        }

        public Client(string host, int port)
        {
            _host = host;
            _port = port;
        }

        private readonly string _host;
        private readonly int _port;
        private readonly TcpClient _client = new();
        private readonly Server? _server;
        private StreamReader? _reader;
        public StreamWriter? Writer { get; private set; }

        public async Task Start()
        {
            try
            {
                _client.Connect(_host, _port);
                _reader = new StreamReader(_client.GetStream());
                Writer = new StreamWriter(_client.GetStream());

                if ((_reader == null) || (Writer == null))
                {
                    throw new Exception("Неполучилось создать поток на чтение и/или запись.");
                }

                Task.Run(ReceivePacket);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client.Start: {ex.Message}");
            }
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        public async Task ReceivePacket()
        {
            try
            {
                while (true)
                {
                    var message = await _reader.ReadLineAsync();
                    var packet = new PacketBase().Deserialize(message);

                    packet?.HandlePacket();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex);
                Thread.Sleep(1000);
            }
            finally
            {
                _server?.RemoveClient(this);
            }
        }

        public async Task SendPacket(PacketBase packet)
        {
            await Writer?.WriteLineAsync(packet.Serialize());
            await Writer?.FlushAsync();
        }

        public void Dispose()
        {
            Writer?.Dispose();
            _reader?.Dispose();
            _client?.Dispose();
        }
    }
}
