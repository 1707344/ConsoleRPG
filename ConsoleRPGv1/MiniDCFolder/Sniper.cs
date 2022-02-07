using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class Sniper: BaseObject
    {
        Renderer renderer;
        Position position;
        Collider collider;
        Movement movement;
        Health health;
        PathFinder pathFinder;
        Patrol patrol;
        Cooldown movementCooldown;
        List<AimingIndicator> indicators;
        Color indicatorColor;
        bool isIndicatorColorIncreasing;

        bool isShooting = false;
        public Sniper(Map map, int x, int y, PatrolPoint[] patrolPoints): base(map)
        {
            collider = new Collider(this, OnCollision);
            health = new Health(this, 2, OnDeath);
            renderer = new Renderer(this, 'ô', 2, new Color(255, 200, 43), false);
            position = new Position(this, x, y);
            patrol = new Patrol(patrolPoints);
            movementCooldown = new Cooldown(400);
            movement = new Movement(this);
            pathFinder = new PathFinder(this, 100);

            indicatorColor = new Color(255, 0, 0);
        }

        public override void Update()
        {
            base.Update();

            if (isShooting)
            {
                if (isIndicatorColorIncreasing)
                {
                    indicatorColor.r += 5;
                }
                else
                {
                    indicatorColor.r -= 5;
                }

                if(indicatorColor.r <= 50)
                {
                    isIndicatorColorIncreasing = true;
                    indicatorColor.r = 50;
                }else if(indicatorColor.r >= 255)
                {
                    isIndicatorColorIncreasing = false;
                    indicatorColor.r = 255;
                }

                


                foreach(Renderer renderer in indicators.ConvertAll(x => x.GetComponent<Renderer>()))
                {
                    renderer.color = indicatorColor;
                }
                return;
            }

            if (CheckForPlayer())
            {
                CreateIndicators();
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

        void CreateIndicators()
        {
            indicators = new List<AimingIndicator>();
            Position playerPosition = MiniDC.player.GetComponent<Position>();
            (int x, int y) offset = (0, 0);
            if(playerPosition.x > position.x)
            {
                offset = (1, 0);
            }
            if(playerPosition.x < position.x)
            {
                offset = (-1, 0);
            }
            if(playerPosition.y > position.y)
            {
                offset = (0, 1);
            }
            if(playerPosition.y < position.y)
            {
                offset = (0, -1);
            }

            (int x, int y) loopPosition = (position.x, position.y);

            while(loopPosition.x > 0 && loopPosition.x < GetMap().width
                && loopPosition.y > 0 && loopPosition.y < GetMap().height)
            {
                loopPosition = (loopPosition.x + offset.x, loopPosition.y + offset.y);
                AimingIndicator indicator = new AimingIndicator(GetMap(), loopPosition.x, loopPosition.y, Movement.Direction.None);
                indicator.renderer.layer = 0;
                indicators.Add(indicator);
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
            return true;
        }
        public bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
    }
}
