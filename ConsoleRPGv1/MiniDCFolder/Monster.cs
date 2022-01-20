using System;
using System.Threading;
namespace ConsoleRPG
{
    public class Monster : BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        PathFinder pathFinder;
        Cooldown movementCooldown;
        Collider collider;
        Health health;
        Cooldown flashingCooldown;
        bool flashing;
        bool flashingSwitch;
        int explosionTime = 500;

        bool isExploding = false;

        public Monster(Map map, int x, int y) : base(map)
        {
            collider = new Collider(this, OnCollision);
            health = new Health(this, 2, OnDeath);
            renderer = new Renderer(this, '@', 2, new Color(255, 43, 43), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            pathFinder = new PathFinder(this, 9);
            movementCooldown = new Cooldown(500);
            flashingCooldown = new Cooldown(75);
        }

        public bool OnCollision(BaseObject baseObject)
        {

            return true;
        }

        public bool OnDeath()
        {
            Explode();
            destroy = true;
            return true;
        }



        public override void Update()
        {

            base.Update();

            Position playerPos = MiniDC.player.GetComponent<Position>();

            if (flashing && flashingCooldown.IsCooldownDone())
            {
                flashingCooldown.StartCooldown();
                renderer.color.r = flashingSwitch ? 255 : 43;
                flashingSwitch = !flashingSwitch;
            }

            if (isExploding)
            {
                return;
            }

            if (MathF.Sqrt(MathF.Pow(playerPos.x - position.x, 2) + MathF.Pow(playerPos.y - position.y, 2)) <= 1.5f)
            {
                isExploding = true;
                PrepareToExplode();
            }
            else if (movementCooldown.IsCooldownDone())
            {
                movementCooldown.StartCooldown();
                movement.Move(pathFinder.GetNextMove(MiniDC.player.GetComponent<Position>()));
            }
        }

        void PrepareToExplode()
        {
            flashing = true;
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                movement.stopMovement = true;
                Thread.Sleep(explosionTime);
                Explode();
            }));
            thread.Start();
        }

        void Explode()
        {
            GetMap().addingObjecsFuncs.Add(SpawnExplosion);
        }

        public bool SpawnExplosion()
        {
            ExplosionHandler.SpawnNewExplosion(GetMap(), position.x, position.y, 0.2f, 0.02f, 20);
            return true;
        }

    }
}
