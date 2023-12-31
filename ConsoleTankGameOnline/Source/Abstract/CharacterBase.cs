using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Structure;
using System.Diagnostics;

namespace ConsoleTankGameOnline.Source.Interface
{
    public abstract class CharacterBase
    {
        public CharacterBase(string skin, World world)
        {
            ;
            _skins.Add(RotationEnum.Front, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_front.txt")]));
            _skins.Add(RotationEnum.Back, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_back.txt")]));
            _skins.Add(RotationEnum.Left, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_left.txt")]));
            _skins.Add(RotationEnum.Right, ConvertSkinToArray([.. File.ReadAllLines($"{skin}/{Path.GetFileName(skin)}_right.txt")]));
            Skin = _skins[RotationEnum.Front];
            Weapon = new Weapon(this);
            _world = world;
        }

        private readonly Dictionary<RotationEnum, char[,]> _skins = [];

        private World _world { get; }
        public string Name { get; set; } = string.Empty;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Position Position { get; set; }
        public char[,] Skin;
        public readonly Weapon Weapon;
        public const string SkinPath = "Resurce/Skins";

        public abstract void Action();

        protected void Rotation(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Skin = _skins[RotationEnum.Front];
                    break;
                case ConsoleKey.DownArrow:
                    Skin = _skins[RotationEnum.Back];
                    break;
                case ConsoleKey.LeftArrow:
                    Skin = _skins[RotationEnum.Left];
                    break;
                case ConsoleKey.RightArrow:
                    Skin = _skins[RotationEnum.Right];
                    break;
                default:
                    break;
            }
        }

        protected bool IsEnemyNearMe(Position newPosition)
        {
            foreach (var enemyPosition in _world.Characters.Where(c => c != this).Select(s => s.Position))
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
