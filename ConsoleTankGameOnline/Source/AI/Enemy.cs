using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source.AI
{
    public class Enemy : CharacterBase
    {
        public Enemy(string skin, World world) : base(skin, world)
        {
            Name = "bot";
        }

        public override void Action()
        {
            //throw new NotImplementedException();
        }
    }
}
