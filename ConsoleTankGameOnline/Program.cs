using ConsoleTankGameOnline.Source;
using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Network;
using ConsoleTankGameOnline.Source.Network.Packages;

namespace ConsoleTankGameOnline
{
    public class Program
    {
        public Program()
        {
            Listener.OnWorldSelected += Listener_OnWorldOpened;
        }

        private GameModeEnum _mode = GameModeEnum.Offline;
        private GameStepEnum _step = GameStepEnum.SelectGameMode;
        private GameOnlineModeEnum _onlineMode = GameOnlineModeEnum.Create;
        private const char _selectCursor = '→';
        private World? _word;
        private Game? _game;
        private Server? _server;
        private Client? _client;

        private static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Console Tank Game Online";

            new Program().Start();
        }

        private void Start()
        {
            CreateDirectory();
            Console.Clear();

            while (true)
            {
                SelectGameMode();
                SelectOnlineGameMode();
                CreateServer();
                ConnectionToServer();
                SelectMap();
                SelectPlayer();

                _game?.Play();
            }
        }

        private void SelectGameMode()
        {
            while (_step == GameStepEnum.SelectGameMode)
            {
                Console.WriteLine($"SELECT GAME MODE:\n" +
                    $"{((_mode == GameModeEnum.Offline) ? _selectCursor : ' ')}1. NEW GAME\n" +
                    $"{((_mode == GameModeEnum.Online) ? _selectCursor : ' ')}2. ONLINE GAME");

                var keyInfo = Console.ReadKey(true);

                if ((keyInfo.Key == ConsoleKey.UpArrow) || (keyInfo.Key == ConsoleKey.DownArrow))
                {
                    _mode = (GameModeEnum)(Convert.ToInt16(_mode) * -1);
                }

                Console.Clear();

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    _step = (_mode == GameModeEnum.Offline) ? GameStepEnum.SelectGameMap : GameStepEnum.SelectGameMode;
                    break;
                }
            }
        }

        private void SelectOnlineGameMode()
        {
            if (_mode == GameModeEnum.Offline)
            {
                return;
            }

            while (_step == GameStepEnum.SelectGameMode)
            {
                Console.WriteLine($"SELECT ONLINE GAME MODE:\n" +
                    $"{((_onlineMode == GameOnlineModeEnum.Create) ? _selectCursor : ' ')}1. CREATE A NEW SERVER.\n" +
                    $"{((_onlineMode == GameOnlineModeEnum.Join) ? _selectCursor : ' ')}2. CONNECT TO AN EXISTING SERVER.");

                var keyInfo = Console.ReadKey(true);

                if ((keyInfo.Key == ConsoleKey.UpArrow) || (keyInfo.Key == ConsoleKey.DownArrow))
                {
                    _onlineMode = (GameOnlineModeEnum)(Convert.ToInt16(_onlineMode) * -1);
                }

                Console.Clear();

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    _step = GameStepEnum.SelectNetwork;
                    break;
                }
            }
        }

        private void CreateServer()
        {
            if ((_mode != GameModeEnum.Online) || (_step != GameStepEnum.SelectNetwork) || (_onlineMode != GameOnlineModeEnum.Create))
            {
                return;
            }

            if (_server == null)
            {
                Console.Write("ENTER CONNECTION PORT: ");
                var port = Convert.ToInt32(Console.ReadLine());

                _server = new Server(port);
                _server.Start();

            }

            while (!_server.Clients.Any())
            {
                Console.Write($"WE ARE EXPECTING PLAYERS TO CONNECT AT ADDRESS {_server.GetCurrentAddress()}...");
                Thread.Sleep(1000);
                Console.Clear();
            }

            _step = GameStepEnum.SelectGameMap;
        }

        private void ConnectionToServer()
        {
            if ((_mode != GameModeEnum.Online) || (_step != GameStepEnum.SelectNetwork) || (_onlineMode != GameOnlineModeEnum.Join))
            {
                return;
            }

            if (_client == null)
            {
                Console.Write("ENTER SERVER HOST: ");
                var host = Console.ReadLine();
                Console.Write("ENTER SERVER PORT: ");
                var port = Convert.ToInt32(Console.ReadLine());

                _client = new Client(host, port);
                _client.Start();
            }

            if (_client.IsConnected())
            {
                _step = GameStepEnum.SelectPlayer;
            }
        }

        private void SelectMap()
        {
            if (_step != GameStepEnum.SelectGameMap)
            {
                return;
            }

            var maps = Directory.GetFiles(World.MapPath);
            int selectedMapIndex = 0;

            while (_step == GameStepEnum.SelectGameMap)
            {
                Console.WriteLine($"SELECT MAP:");

                for (int i = 0; i < maps.Length; i++)
                {
                    Console.WriteLine($"{((selectedMapIndex == i) ? _selectCursor : ' ')}{i + 1}. {Path.GetFileNameWithoutExtension(maps[i]).ToUpper()}");
                }

                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedMapIndex = Math.Min(Math.Max(0, selectedMapIndex++), maps.Length);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedMapIndex = Math.Min(Math.Max(0, selectedMapIndex--), maps.Length);
                        break;
                    default:
                        break;
                }

                Console.Clear();

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    _word = new World(maps[selectedMapIndex]);

                    if ((_mode == GameModeEnum.Online) && (_server != null))
                    {
                        _server.SendPacket(new WorldInfo(maps[selectedMapIndex]));
                    }

                    _step = GameStepEnum.SelectPlayer;
                    break;
                }
            }
        }

        private void SelectPlayer()
        {
            if (_step != GameStepEnum.SelectPlayer)
            {
                return;
            }

            var players = Directory.GetDirectories(CharacterBase.SkinPath);
            int selectedPlayerIndex = 0;

            while (_step == GameStepEnum.SelectPlayer)
            {
                Console.WriteLine($"SELECT TANK:");

                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine($"{((selectedPlayerIndex == i) ? _selectCursor : ' ')}{i + 1}. {Path.GetFileName(players[i]).ToUpper()}");
                }

                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedPlayerIndex = Math.Min(Math.Max(0, selectedPlayerIndex++), players.Length);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedPlayerIndex = Math.Min(Math.Max(0, selectedPlayerIndex--), players.Length);
                        break;
                    default:
                        break;
                }

                Console.Clear();

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    var player = new Player(players[selectedPlayerIndex], _word)
                    {
                        Name = Guid.NewGuid().ToString()
                    };

                    _word?.AddCharacter(player);
                    _game = new Game(_word);
                    _step = GameStepEnum.LoadGame;
                    break;
                }
            }
        }

        private void Listener_OnWorldOpened(string path)
        {
            _word = new World(path);
        }

        private void CreateDirectory()
        {
            Console.WriteLine("LOADING MAPS...");
            Directory.CreateDirectory(World.MapPath);
            Console.WriteLine("LOADING TANKS...");
            Directory.CreateDirectory(CharacterBase.SkinPath);
        }
    }
}
