using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class Weapon : IDisposable
    {
        public Weapon(CharacterBase character)
        {
            _character = character;
            Listener.OnShellDestroyed += Listener_OnShellDestroyed;
            Listener.OnShellMove += Listener_OnShellMove;
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
                    position.X += _character.Height;
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

            Shell = new Shell(_character)
            {
                Position = position
            };
        }

        private void Listener_OnShellDestroyed(CharacterBase owner)
        {
            if (_character == owner)
            {
                Shell = null;
            }
        }
        private void Listener_OnShellMove(string objectName, Structure.Position position, bool sendPacket)
        {
            if (sendPacket)
            {
                return;
            }

            var attacker = World.Instance.Objects[objectName];

            if (attacker != _character)
            {
                return;
            }

            if (!IsShoted)
            {
                Shell = new Shell(attacker);
            }

            Shell.Position = position;
        }

        public void Dispose()
        {
            Listener.OnShellDestroyed -= Listener_OnShellDestroyed;
            Listener.OnShellMove -= Listener_OnShellMove;
        }
    }
}
