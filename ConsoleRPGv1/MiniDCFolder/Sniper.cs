using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class Sniper: BaseObject
    {
        Dictionary<Movement.Direction, char> laserChar;
        Renderer renderer;
        Position position;
        Collider collider;
        Movement movement;
        Health health;
        Freezable freezable;
        PathFinder pathFinder;
        Patrol patrol;
        Cooldown movementCooldown;
        Cooldown shootStartupCooldown;//How long the indicators will be there/how long until it does damage
        Cooldown shootingTime;//How long the laser is active
        Cooldown startingCooldown;
        bool startingCooldownStarted;
        List<AimingIndicator> indicators;
        Color indicatorColor;
        bool isIndicatorColorIncreasing;

        bool isShooting = false;
        bool isLaserActivated = false;

        Movement.Direction shootingDirection = Movement.Direction.None;

        float laserDamage = 0.5f;

        public Sniper(Map map, int x, int y, PatrolPoint[] patrolPoints): base(map)
        {
            //Components
            collider = new Collider(this, OnCollision);
            health = new Health(this, 2, OnDeath);
            freezable = new Freezable(this, 2000);
            renderer = new Renderer(this, 'ô', 2, new Color(255, 200, 43), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            pathFinder = new PathFinder(this, 100);


            patrol = new Patrol(patrolPoints);

            indicatorColor = new Color(255, 0, 0);

            //Cooldowns
            movementCooldown = new Cooldown(400);
            shootStartupCooldown = new Cooldown(800);
            shootingTime = new Cooldown(400);
            startingCooldown = new Cooldown(500);

            laserChar = new Dictionary<Movement.Direction, char>(4);
            laserChar.Add(Movement.Direction.North, '║');
            laserChar.Add(Movement.Direction.South, '║');
            laserChar.Add(Movement.Direction.East, '═');
            laserChar.Add(Movement.Direction.West, '═');

            startingCooldownStarted = false;
        }

        public override void Update()
        {
            base.Update();

            if (freezable.GetIsFrozen())
            {
                DestroyIndicators();
                return;
            }

            if (!startingCooldownStarted)
            {
                startingCooldown.StartCooldown();
                startingCooldownStarted = true;
            }

            if (isShooting)
            {
                

                if (shootStartupCooldown.IsCooldownDone() && !isLaserActivated)
                {
                    foreach (Renderer indicatorRenderer in indicators.ConvertAll(x => x.renderer))
                    {
                        indicatorRenderer.icon = laserChar[shootingDirection];
                        indicatorRenderer.color = new Color(255, 100, 0);
                    }

                    shootingTime.StartCooldown();

                    isLaserActivated = true;
                    
                }
                else if(!isLaserActivated)
                {
                    IndicatorColorPulse();
                }

                if (isLaserActivated)
                {
                    LaserHurt();
                }

                if (shootingTime.IsCooldownDone() && isLaserActivated)
                {
                    isShooting = false;
                    isLaserActivated = false;
                    foreach (AimingIndicator aimingIndicator in indicators)
                    {
                        aimingIndicator.destroy = true;
                    }
                }

                return;
            }

            if (CheckForPlayer() && startingCooldown.IsCooldownDone())
            {
                CreateIndicators();
                shootStartupCooldown.StartCooldown();
                isShooting = true;
                return;
            }

            if (movementCooldown.IsCooldownDone())
            {
                PatrolPointMove();
                movementCooldown.StartCooldown();
            }
        }
        /// <summary>
        /// Checks to see if the player is in firing range
        /// </summary>
        /// <returns></returns>
        bool CheckForPlayer()
        {
            Position playerPosition = MiniDC.player.GetComponent<Position>();
            if (playerPosition.x == position.x || playerPosition.y == position.y)
            {
                return true;
            }
            return false;
        }

        void LaserHurt()
        {
            Map map = GetMap();
            foreach(Position aimingIndicatorPos in indicators.ConvertAll(x => x.position))
            {
                List<Health> healthsAtPosition = map.GetObjectsAtPosition(aimingIndicatorPos.x, aimingIndicatorPos.y).FindAll(x => x.obj.GetComponent<Health>() != null).ConvertAll(x => x.obj.GetComponent<Health>());


                foreach(Health health in healthsAtPosition)
                {
                    health.health -= laserDamage;
                    new LaserHitIndicator(map, aimingIndicatorPos.x, aimingIndicatorPos.y);
                }
                
            }
        }

        void CreateIndicators()
        {


            shootingDirection = Movement.Direction.None;

            indicators = new List<AimingIndicator>();
            Position playerPosition = MiniDC.player.GetComponent<Position>();
            (int x, int y) offset = (0, 0);
            if(playerPosition.x > position.x)
            {
                offset = (1, 0);
                shootingDirection = Movement.Direction.East;
            }
            if(playerPosition.x < position.x)
            {
                offset = (-1, 0);
                shootingDirection = Movement.Direction.West;
            }
            if(playerPosition.y > position.y)
            {
                offset = (0, 1);
                shootingDirection = Movement.Direction.South;
            }
            if(playerPosition.y < position.y)
            {
                offset = (0, -1);
                shootingDirection = Movement.Direction.North;
            }

            (int x, int y) loopPosition = (position.x, position.y);

            while(loopPosition.x > 0 && loopPosition.x < GetMap().width
                && loopPosition.y > 0 && loopPosition.y < GetMap().height)
            {
                loopPosition = (loopPosition.x + offset.x, loopPosition.y + offset.y);
                AimingIndicator indicator = new AimingIndicator(GetMap(), loopPosition.x, loopPosition.y, Movement.Direction.None);
                indicators.Add(indicator);
            }
        }
        void DestroyIndicators()
        {
            if(indicators == null)
            {
                return;
            }

            foreach(AimingIndicator aimingIndicator in indicators)
            {
                aimingIndicator.destroy = true;
            }
        }
        void IndicatorColorPulse()
        {
            if (isIndicatorColorIncreasing)
            {
                indicatorColor.r += 5;
            }
            else
            {
                indicatorColor.r -= 5;
            }

            if (indicatorColor.r <= 50)
            {
                isIndicatorColorIncreasing = true;
                indicatorColor.r = 50;
            }
            else if (indicatorColor.r >= 255)
            {
                isIndicatorColorIncreasing = false;
                indicatorColor.r = 255;
            }


            foreach (Renderer renderer in indicators.ConvertAll(x => x.GetComponent<Renderer>()))
            {
                renderer.color = indicatorColor;
            }
        }

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

        public bool OnDeath()
        {
            DestroyIndicators();
            destroy = true;
            return true;
        }
        public bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
    }

    /// <summary>
    /// Visually a way to inform the player that something is being hit by laser
    /// </summary>
    class LaserHitIndicator : BaseObject
    {
        Position position;
        Renderer renderer;
        Cooldown duration;
        public LaserHitIndicator(Map map, int x, int y) : base(map)
        {
            position = new Position(this, x, y);
            renderer = new Renderer(this, ' ', 10, new Color(255, 100, 0, 0.6f), true);
            duration = new Cooldown(300);

            duration.StartCooldown();
        }

        public override void Update()
        {
            base.Update();

            if (duration.IsCooldownDone())
            {
                destroy = true;
            }
        }

    }
}
