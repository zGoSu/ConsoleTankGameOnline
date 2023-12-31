using ConsoleTankGameOnline.Source.Abstract;
using System.Net.Sockets;

namespace ConsoleTankGameOnline.Source.Network
{
    public class Client : IDisposable
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

        public async void Start()
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

                Task.Run(ReceiveMessage);

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

        public async Task Process()
        {
            try
            {
                while (true)
                {
                    var message = await _reader.ReadLineAsync();
                    //var packet = PacketBase.Deserialize(message);

                    throw new Exception("WHAT IS IT?????");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client.Process: {ex.Message}");
            }
            finally
            {
                _server.RemoveClient(this);
            }
        }

        private async Task ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var message = await _reader.ReadLineAsync();
                    var packet = new PacketBase().Deserialize(message);

                    packet.HandlePacket();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client.ReceiveMessage: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            Writer?.Dispose();
            _reader?.Dispose();
            _client?.Dispose();
        }
    }
}
