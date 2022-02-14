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

        float transferSpeed = 0.5f;
        float decaySpeed = 0.05f;

        public Fireball(Map map, int x, int y, Movement.Direction direction, float fireballStrength, float fireballSpeed) : base(map)
        {
            renderer = new Renderer(this, '☼', 3, new Color(255, 66, 41), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);
            movementCooldown = new Cooldown(fireballSpeed);
            this.direction = direction;
            startEnergy = fireballStrength;
        }

        public Fireball(Map map, int x, int y, Movement.Direction direction, float fireballStrength, float fireballSpeed, float transferSpeed, float decaySpeed) : this(map, x, y, direction, fireballStrength, fireballSpeed)
        {
            this.transferSpeed = transferSpeed;
            this.decaySpeed = decaySpeed;
        }

        public bool OnCollision(BaseObject baseObject)
        {
            //GetMap().newObjects.Add(new ExplosionHandler(GetMap(), position.x, position.y)); 0.5 0.05
            ExplosionHandler.SpawnNewExplosion(GetMap(), position.x, position.y, transferSpeed, decaySpeed, startEnergy);


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
