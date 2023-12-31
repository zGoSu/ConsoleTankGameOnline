using ConsoleTankGameOnline.Source.AI;
using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class Game
    {
        public Game(World world)
        {
            _world = world;
            AddEnemy();
        }

        private readonly World _world;

        public void Play()
        {
            _world.Draw();

            foreach (var character in _world.Characters)
            {
                if (character is Player player)
                {
                    player.Action();
                }
            }
        }

        private void AddEnemy()
        {
            var rand = new Random();
            var skins = Directory.GetDirectories(CharacterBase.SkinPath);
            var randIndex = rand.Next(skins.Length);
            var enemy = new Enemy(skins[randIndex], _world)
            {
                Position = new() { X = 6, Y = 20 }
            };

            _world.AddCharacter(enemy);
        }
    }
}
