using ConsoleTankGameOnline.Source;
using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline
{
    public class Program
    {
        private readonly GameStateEnum _state = GameStateEnum.None;
        private GameModeEnum _mode = GameModeEnum.Offline;
        private GameStepEnum _step = GameStepEnum.SelectGameMode;
        private const char _selectCursor = '→';
        private World? _word;
        private Game? _game;

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
                SelectMap();
                SelectPlayer();

                _game?.Play();
            }
        }

        private void SelectGameMode()
        {
            if (_state != GameStateEnum.None)
            {
                return;
            }

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
                    _step = GameStepEnum.SelectGameMap;
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

            _step = GameStepEnum.SelectGameMode;
            Console.WriteLine("I'm sorry, but the online mode has not yet been implemented 😔".ToUpper());
            Console.ReadKey();
            Console.Clear();
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
                        Name = "Alexey",
                        Position = new() { X = 44, Y = 20 }
                    };
                    _word?.AddCharacter(player);
                    _game = new Game(_word);
                    _step = GameStepEnum.LoadGame;
                    break;
                }
            }
        }

        private void CreateDirectory()
        {
            Console.WriteLine("LOADING MAPS...");
            Directory.CreateDirectory(World.MapPath);
            Console.WriteLine("LOADING TANKS...");
            Directory.CreateDirectory(Player.SkinPath);
        }
    }
}
