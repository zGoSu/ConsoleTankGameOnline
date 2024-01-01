using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source.AI
{
    public class Enemy : CharacterBase
    {
        public Enemy(string skin) : base(skin)
        {
            Name = "bot";
        }

        public override void Action()
        {
            //throw new NotImplementedException();
        }
    }
}
