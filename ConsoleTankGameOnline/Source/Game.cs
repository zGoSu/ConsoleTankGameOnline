namespace ConsoleTankGameOnline.Source
{
    public class Game
    {
        public Game()
        {
            AddEnemy();
        }

        public void Play()
        {
            if (World.Instance == null)
            {
                throw new Exception("THE WORLD DOESN'T EXIST!");
            }

            World.Instance.Draw();

            foreach (var character in World.Instance.Objects)
            {
                if (character is Player player)
                {
                    player.Action();
                }
            }
        }

        private void AddEnemy()
        {
            //var rand = new Random();
            //var skins = Directory.GetDirectories(CharacterBase.SkinPath);
            //var randIndex = rand.Next(skins.Length);
            //var enemy = new Enemy(skins[randIndex], _world)
            //{
            //    Position = new() { X = 6, Y = 20 }
            //};

            //_world.AddCharacter(enemy);
        }
    }
}
