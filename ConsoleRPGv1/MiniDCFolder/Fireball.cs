namespace ConsoleRPG
{


    class Fireball : BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        Collider collider;
        Cooldown movementCooldown;
        Movement.Direction direction;

        float startEnergy;

        public Fireball(Map map, int x, int y, Movement.Direction direction, float fireballStrength) : base(map)
        {
            renderer = new Renderer(this, '☼', 3, new Color(255, 66, 41), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);
            movementCooldown = new Cooldown(100);
            this.direction = direction;
            startEnergy = fireballStrength;
        }

        public bool OnCollision(BaseObject baseObject)
        {
            //GetMap().newObjects.Add(new ExplosionHandler(GetMap(), position.x, position.y));
            ExplosionHandler.SpawnNewExplosion(GetMap(), position.x, position.y, 0.5f, 0.05f, startEnergy);


            destroy = true;
            return true;
        }

        public override void Update()
        {
            base.Update();

            if (movementCooldown.IsCooldownDone())
            {
                movementCooldown.StartCooldown();
                movement.Move(direction);
            }
        }
    }
}
