using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source
{
    public class Shell
    {
        public Shell(CharacterBase owner)
        {
            _owner = owner;
        }

        private readonly CharacterBase _owner;
        public const char Symbol = '•';
        public Position Position { get; set; }

        public void UpdatePosition()
        {
            var newPosition = Position;

            switch (newPosition.Rotation)
            {
                case RotationEnum.Front:
                    newPosition.X--;
                    break;
                case RotationEnum.Back:
                    newPosition.X++;
                    break;
                case RotationEnum.Left:
                    newPosition.Y--;
                    break;
                case RotationEnum.Right:
                    newPosition.Y++;
                    break;
                default:
                    break;
            }

            if (World.Instance.IsWall[newPosition.X, newPosition.Y])
            {
                Listener.DestroyShell(_owner);
                return;
            }

            Position = newPosition;
        }
    }
}
