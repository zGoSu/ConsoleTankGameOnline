using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class World
    {
        public World(string map)
        {
            _map = Map(map);
            MaxX = _map.GetLength(0);
            MaxY = _map.GetLength(1);
        }

        private readonly char[,] _map;
        private const char _wall = '■';
        private readonly List<CharacterBase> _characters = [];

        public const string MapPath = "Resurce/Maps";
        public IEnumerable<CharacterBase> Characters => _characters;
        public static bool[,] IsWall;
        public int MaxX { get; }
        public int MaxY { get; }

        public void Draw()
        {
            char[,] map = new char[_map.GetLength(0), _map.GetLength(1)];

            Array.Copy(_map, map, _map.Length);
            Clear();
            AddCharacterToMap(map);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }

        public void AddCharacter(CharacterBase character)
        {
            _characters.Add(character);
        }

        private void RemoveCharacter(CharacterBase character)
        {
            _characters.Remove(character);
        }

        private void AddCharacterToMap(char[,] map)
        {
            foreach (var character in _characters.ToList())
            {
                var shell = character.Weapon.Shell;
                shell?.UpdatePosition();
                if (character.Weapon.IsShoted)
                {
                    map[shell.Position.X, shell.Position.Y] = Shell.Symbol;
                }

                for (int i = 0; i < character.Height; i++)
                {
                    for (int j = 0; j < character.Width; j++)
                    {
                        if (map[character.Position.X + i, character.Position.Y + j] == Shell.Symbol)
                        {
                            Listener.ShellDestroy();
                            RemoveCharacter(character);
                            break;
                        }

                        map[character.Position.X + i, character.Position.Y + j] = character.Skin[i, j];
                    }

                    if (character == null)
                    {
                        break;
                    }
                }
            }
        }

        private void Clear()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.Write("\r{0}%");
            }
            Console.SetCursorPosition(0, 0);
        }

        private char[,] Map(string path)
        {
            var mapLine = File.ReadAllLines(path).ToArray();
            var map = new char[mapLine.Length, mapLine.Length];

            IsWall = new bool[mapLine.Length, mapLine.Length];

            for (int i = 0; i < mapLine.Length; i++)
            {
                var line = mapLine[i].ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    map[i, j] = line[j];
                    IsWall[i, j] = (line[j] == _wall);
                }
            }

            return map;
        }
    }
}
