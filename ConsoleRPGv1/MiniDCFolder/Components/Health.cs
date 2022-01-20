using System;
namespace ConsoleRPG
{
    public class Health : Component
    {
        public float health;
        public float maxHealth;
        Func<bool> onDeath;
        public Health(BaseObject obj, float health, Func<bool> onDeath) : base(obj)
        {
            this.health = health;
            this.maxHealth = health;
            this.onDeath = onDeath;
        }

        public override void Update()
        {
            if (health <= 0)
            {
                onDeath();
            }
        }
    }
}
