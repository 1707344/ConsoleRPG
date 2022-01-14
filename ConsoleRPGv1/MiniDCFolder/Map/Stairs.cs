namespace ConsoleRPG
{
    /// <summary>
    /// How you move between rooms
    /// </summary>
    public class Stairs : BaseObject
    {
        Position position;
        Renderer renderer;
        Collider collider;

        public Stairs(Map map, int x, int y) : base(map)
        {
            position = new Position(this, x, y);
            renderer = new Renderer(this, '/', 2, new Color(255, 244, 230), false);
            collider = new Collider(this, OnCollision);
        }

        public bool OnCollision(BaseObject baseObject)
        {
            MiniDC.gamePlaying = false;
            int numMonsters = GetMap().objects.FindAll(x => x.GetType().Name == "Monster").Count;

            if (numMonsters == 0)
            {
                EndScreen.ending = EndScreen.Ending.Slaughterer;
            }
            else if (numMonsters == EndScreen.startingNumMonsters)
            {
                EndScreen.ending = EndScreen.Ending.Pacifist;
            }
            return true;
        }
    }
}
