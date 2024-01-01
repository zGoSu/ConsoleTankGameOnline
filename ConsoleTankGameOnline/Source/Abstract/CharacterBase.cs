using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Structure;
using Newtonsoft.Json;

namespace ConsoleTankGameOnline.Source.Interface
{
    public class CharacterBase
    {
        public CharacterBase()
        {
            Weapon = new Weapon(this);
        }

        public CharacterBase(string skin)
        {
            Skins.Add(RotationEnum.Front, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_front.txt")]));
            Skins.Add(RotationEnum.Back, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_back.txt")]));
            Skins.Add(RotationEnum.Left, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_left.txt")]));
            Skins.Add(RotationEnum.Right, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_right.txt")]));
            Skin = Skins[RotationEnum.Front];
            Weapon = new Weapon(this);
        }

        private World? _world;
        protected Dictionary<RotationEnum, char[,]> Skins { get; private set; } = [];

        public World? World
        {
            get => _world;
            set
            {
                if (_world == null)
                {
                    _world = value;
                    SetStartPosition();
                }
            }
        }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public int Width { get; private set; }
        [JsonIgnore]
        public int Height { get; private set; }
        public Position Position { get; set; }
        public char[,]? Skin { get; private set; }
        [JsonIgnore]
        public readonly Weapon Weapon;
        public const string SkinPath = "Resurce/Skins";

        private void SetStartPosition()
        {
            var newPosition = new Position();
            var rand = new Random();

            while (true)
            {
                newPosition.X = rand.Next(1, World.MaxX - Height);
                newPosition.Y = rand.Next(1, World.MaxY - Width);

                if (!IsEnemyNearMe(newPosition))
                {
                    Position = newPosition;
                    break;
                }
            }
        }

        public virtual void Action() { }

        protected void Rotation(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Skin = Skins[RotationEnum.Front];
                    break;
                case ConsoleKey.DownArrow:
                    Skin = Skins[RotationEnum.Back];
                    break;
                case ConsoleKey.LeftArrow:
                    Skin = Skins[RotationEnum.Left];
                    break;
                case ConsoleKey.RightArrow:
                    Skin = Skins[RotationEnum.Right];
                    break;
                default:
                    break;
            }
        }

        protected bool IsEnemyNearMe(Position newPosition)
        {
            foreach (var enemyPosition in World.Characters.Where(c => c != this).Select(s => s.Position))
            {
                if ((newPosition.Y < enemyPosition.Y + Width) && (newPosition.Y > enemyPosition.Y - Width)
                    && (newPosition.X < enemyPosition.X + Height) && (newPosition.X > enemyPosition.X - Height))
                {
                    return true;
                }
            }

            return false;
        }

        private char[,] ConvertSkinToArray(string[] skin)
        {
            Height = skin.Length;
            Width = skin.Length;

            char[,] skinArray = new char[Height, Width];

            for (int i = 0; i < skin.Length; i++)
            {
                var line = skin[i].ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    skinArray[i, j] = line[j];
                }
            }

            return skinArray;
        }
    }
}
