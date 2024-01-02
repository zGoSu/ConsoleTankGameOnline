using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Structure;
using Newtonsoft.Json;

namespace ConsoleTankGameOnline.Source
{
    public class World
    {
        public World()
        {
            Listener.OnPlayerAdded += Listener_OnPlayerAdded;
            Listener.OnMove += Listener_OnMove;
            Listener.OnShot += Listener_OnShot;
        }

        private readonly Dictionary<string, CharacterBase> _objects = [];
        private const char _wall = '■';
        private const string _path = "Resurce/Maps";

        [JsonIgnore]
        public static World? Instance { get; private set; }

        [JsonIgnore]
        public IEnumerable<CharacterBase> Objects => _objects.Values;
        [JsonIgnore]
        public static IEnumerable<string> Locations = Directory.GetFiles(_path);
        [JsonIgnore]
        public int MaxMapX { get; private set; }
        [JsonIgnore]
        public int MaxMapY { get; private set; }
        [JsonIgnore]
        public bool[,]? IsWall { get; private set; }
        public char[,] Map { get; set; }

        public static void CreateDirectory()
        {
            Directory.CreateDirectory(_path);
        }
        public static void CreateWorld(char[,] map)
        {
            Instance = new World
            {
                Map = map,
                IsWall = new bool[map.GetLength(0), map.GetLength(1)],
                MaxMapX = map.GetLength(0),
                MaxMapY = map.GetLength(1)
            };

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Instance.IsWall[i, j] = (map[i, j] == _wall);
                }
            }
        }
        public static void CreateWorld(string path)
        {
            var mapLine = File.ReadAllLines(path).ToArray();

            Instance = new World
            {
                Map = new char[mapLine.Length, mapLine.Length],
                IsWall = new bool[mapLine.Length, mapLine.Length],
                MaxMapX = mapLine.Length,
                MaxMapY = mapLine.Length
            };

            for (int i = 0; i < mapLine.Length; i++)
            {
                var line = mapLine[i].ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    Instance.Map[i, j] = line[j];
                    Instance.IsWall[i, j] = (line[j] == _wall);
                }
            }
        }
        public void Draw()
        {
            if (Instance?.Map == null)
            {
                return;
            }

            char[,] map = new char[Instance.Map.GetLength(0), Instance.Map.GetLength(1)];

            Array.Copy(Instance.Map, map, Instance.Map.Length);
            Clear();
            AddObjectToMap(map);
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

        private void AddObjectToMap(char[,] map)
        {
            foreach (var character in Objects.ToList())
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
                            Listener.DestroyShell();
                            RemoveObject(character);
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
        private void AddObject(CharacterBase character)
        {
            _objects.Add(character.Name, character);
        }
        private void RemoveObject(CharacterBase character)
        {
            _objects.Remove(character.Name);
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
        private void Listener_OnPlayerAdded(CharacterBase character, bool sendPacket)
        {
            AddObject(character);
        }
        private void Listener_OnMove(string objectName, Position position, bool sendPacket)
        {
            if (sendPacket)
            {
                return;
            }

            var player = _objects[objectName];

            player.Position = position;
            player.Rotation();

        }
        private void Listener_OnShot(string objectName, bool sendPacket)
        {
            if (sendPacket)
            {
                return;
            }

            var player = _objects[objectName];

            if (player.Weapon.IsShoted)
            {
                return;
            }

            player.Weapon.Shot();
        }
    }
}
