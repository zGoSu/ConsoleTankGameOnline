using ConsoleTankGameOnline.Source.Enum;
using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class Player : CharacterBase
    {
        public Player(string skin) : base(skin)
        {
        }

        private Task? _task;

        public override void Action()
        {
            if ((_task != null) && !_task.IsCompleted)
            {
                return;
            }

            _task = new Task(KeyPress);
            _task.Start();
        }

        private void KeyPress()
        {
            var keyInfo = Console.ReadKey(true);
            var newPosition = Position;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newPosition.X--;
                    newPosition.Rotation = RotationEnum.Front;
                    break;
                case ConsoleKey.DownArrow:
                    newPosition.X++;
                    newPosition.Rotation = RotationEnum.Back;
                    break;
                case ConsoleKey.LeftArrow:
                    newPosition.Y--;
                    newPosition.Rotation = RotationEnum.Left;
                    break;
                case ConsoleKey.RightArrow:
                    newPosition.Y++;
                    newPosition.Rotation = RotationEnum.Right;
                    break;
                case ConsoleKey.Spacebar:
                    Weapon.Shot();
                    break;
                default:
                    break;
            }

            if (!World.Instance.IsWall[newPosition.X, newPosition.Y] && !World.Instance.IsWall[newPosition.X + Height - 1, newPosition.Y + Width - 1] && !IsEnemyNearMe(newPosition))
            {
                Position = newPosition;
                Rotation(keyInfo.Key);
                Listener.MoveTo(Name, Position, true);
            }
        }
    }
}
