using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class Weapon : IDisposable
    {
        public Weapon(CharacterBase character)
        {
            _character = character;
            Listener.OnShellDestroyed += Listener_OnShellDestroyed;
        }

        private readonly CharacterBase _character;

        public Shell? Shell { get; private set; }
        public bool IsShoted => Shell != null;

        public void Shot()
        {
            if (IsShoted)
            {
                return;
            }

            var position = _character.Position;

            switch (position.Rotation)
            {
                case Enum.RotationEnum.Front:
                    position.Y = (position.Y * 2 + _character.Width) / 2;
                    break;
                case Enum.RotationEnum.Back:
                    position.Y = (position.Y * 2 + _character.Width) / 2;
                    position.X -= _character.Height;
                    break;
                case Enum.RotationEnum.Left:
                    position.X = (position.X * 2 + _character.Height) / 2;
                    break;
                case Enum.RotationEnum.Right:
                    position.Y += _character.Width;
                    position.X = (position.X * 2 + _character.Height) / 2;
                    break;
                default:
                    break;
            }

            Shell = new Shell
            {
                Position = position
            };
        }

        private void Listener_OnShellDestroyed()
        {
            Shell = null;
        }

        public void Dispose()
        {
            Listener.OnShellDestroyed -= Listener_OnShellDestroyed;
        }
    }
}
