namespace ConsoleRPG
{
    public class Health : Component
    {
        public int health;
        public int maxHealth;
        public Health(BaseObject obj, int health) : base(obj)
        {
            this.health = health;
            this.maxHealth = health;
        }
    }
}
