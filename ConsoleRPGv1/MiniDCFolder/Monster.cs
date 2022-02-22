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
        Patrol patrol;
        Freezable freezable;
        bool flashing;
        bool flashingSwitch;
        int explosionTime = 500;
        int chasePlayerDist = 10;

        bool isExploding = false;

        public Monster(Map map, int x, int y, PatrolPoint[] patrolPoints) : base(map)
        {
            
            Random random = new Random((int)DateTime.Now.Ticks);

            freezable = new Freezable(this);
            collider = new Collider(this, OnCollision);
            health = new Health(this, 2, OnDeath);
            renderer = new Renderer(this, 'Θ', 2, new Color(255, 43, 43), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            pathFinder = new PathFinder(this, 100);
            movementCooldown = new Cooldown(500 + random.Next(-50, 50));
            flashingCooldown = new Cooldown(75);
            patrol = new Patrol(patrolPoints);
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

            if (freezable.GetIsFrozen())
            {
                return;
            }

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
                if(pathFinder.GetMoveNum(MiniDC.player.GetComponent<Position>()) < chasePlayerDist)
                {
                    movement.Move(pathFinder.GetNextMove(MiniDC.player.GetComponent<Position>()));
                }
                else
                {
                    PatrolPointMove();

                }

                movementCooldown.StartCooldown();
                

            }
        }

        /// <summary>
        /// Called when the monster is moving to a patrol point
        /// </summary>
        void PatrolPointMove()
        {
            Movement.Direction direction = pathFinder.GetNextMove(patrol.GetCurrentPatrolPointPosition());

            if (direction == Movement.Direction.None)
            {
                patrol.NextPatrolPoint();
                direction = pathFinder.GetNextMove(patrol.GetCurrentPatrolPointPosition());
            }

            movement.Move(direction);

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
