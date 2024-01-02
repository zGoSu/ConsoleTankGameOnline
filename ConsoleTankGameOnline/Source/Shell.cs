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

            if (World.Instance.IsWall[newPosition.X, newPosition.Y] || Killed())
            {
                Listener.DestroyShell(_owner);
                return;
            }

            Position = newPosition;
        }

        private bool Killed()
        {
            foreach (var enemy in World.Instance.Objects.Where(e => e != _owner))
            {
                if ((Position.X >= enemy.Position.X) && (Position.X <= enemy.Position.X + enemy.Height)
                    && (Position.Y >= enemy.Position.Y) && (Position.Y <= enemy.Position.Y + enemy.Width))
                {
                    Listener.Die(enemy, true);
                    return true;
                }
            }

            return false;
        }
    }
}
