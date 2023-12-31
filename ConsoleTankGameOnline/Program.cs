﻿using ConsoleTankGameOnline.Source;
using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Network;
using ConsoleTankGameOnline.Source.Network.Packages;
using System.Net;

namespace ConsoleTankGameOnline
{
    public class Program
    {
        public Program()
        {
            Listener.OnWorldCreated += Listener_OnWorldOpened;
        }

        private GameModeEnum _mode = GameModeEnum.Offline;
        private GameStepEnum _step = GameStepEnum.SelectGameMode;
        private GameOnlineModeEnum _onlineMode = GameOnlineModeEnum.Create;
        private const char _selectCursor = '→';
        private Game? _game;
        private PacketManager? _packetManager;
        private INetwork? _network;

        private static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.SetWindowSize(50, 50);
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

            if (_network == null)
            {
                _network = new Server();
                _network.Start();
            }

            _packetManager = new PacketManager(_network);

            while (!((Server)_network).Clients.Any())
            {
                Console.Write($"WE ARE EXPECTING PLAYERS\nTO CONNECT AT ADDRESS {((Server)_network).GetCurrentAddress()}...");
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

            if (_packetManager == null)
            {
                IPAddress? ip = null;
                int port = 0;

                do
                {
                    if (ip == null)
                    {
                        Console.Write("ENTER SERVER HOST (127.0.0.1): ");
                        if (!IPAddress.TryParse(Console.ReadLine(), out ip))
                        {
                            Console.Clear();
                            Console.WriteLine("INVALID IP ADDRESS. TRY AGAIN.");
                            continue;
                        }
                    }

                    Console.Write("ENTER SERVER PORT (7777): ");
                    if (!int.TryParse(Console.ReadLine(), out port))
                    {
                        Console.Clear();
                        Console.WriteLine("INVALID PORT. TRY AGAIN.");
                        continue;
                    }
                } while ((ip == null) || (port == 0));

                _network = new Client(ip.ToString(), port);
                _network.Start();
            }

            if (((Client)_network).IsConnected())
            {
                _packetManager = new PacketManager(_network);
                _step = GameStepEnum.SelectPlayer;
            }
            Console.Clear();
        }

        private void SelectMap()
        {
            if (_step != GameStepEnum.SelectGameMap)
            {
                return;
            }

            var maps = World.Locations.ToArray();
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
                    World.CreateWorld(maps[selectedMapIndex]);

                    if (World.Instance?.Map == null)
                    {
                        throw new Exception("ERROR WHEN CREATING THE WORLD!");
                    }

                    if ((_mode == GameModeEnum.Online) && (_onlineMode == GameOnlineModeEnum.Create))
                    {
                        _packetManager?.SendPacket(new WorldInfo(World.Instance.Map));
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
                while (World.Instance == null)
                {
                    Console.WriteLine("WAITING FOR WORLD CREATION BY SERVER...");
                    Thread.Sleep(1000);
                    Console.Clear();
                }

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
                    var player = new Player(players[selectedPlayerIndex])
                    {
                        Name = Guid.NewGuid().ToString(),
                    };

                    Listener.AddPlayer(player, true);
                    _game = new Game();
                    _step = GameStepEnum.LoadGame;
                    break;
                }
            }
        }

        private void Listener_OnWorldOpened(char[,] map)
        {
            World.CreateWorld(map);
        }

        private void CreateDirectory()
        {
            Console.WriteLine("LOADING MAPS...");
            World.CreateDirectory();
            Console.WriteLine("LOADING TANKS...");
            Directory.CreateDirectory(CharacterBase.SkinPath);
        }
    }
}
