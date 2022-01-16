using System;

namespace ConsoleRPG
{
    public class Player : BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        Collider collider;
        Health health;

        AimingIndicator aimingIndicator;
        Movement.Direction direction;

        public static bool test;
        public static bool turnOffTest;

        //Starts false. Player clicks shoot button. Starts aiming(true). Then clicks again then fireball fires
        public bool fireballAim;

        public Player(Map map, int x, int y) : base(map)
        {
            health = new Health(this, 5);//∙☺
            renderer = new Renderer(this, '☼', 3, new Color(92, 255, 133, 0.99f), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);


            InputHandler.AddListener(new KeyListener(MoveUp, ConsoleKey.UpArrow));
            InputHandler.AddListener(new KeyListener(MoveDown, ConsoleKey.DownArrow));
            InputHandler.AddListener(new KeyListener(MoveLeft, ConsoleKey.LeftArrow));
            InputHandler.AddListener(new KeyListener(MoveRight, ConsoleKey.RightArrow));
            InputHandler.AddListener(new KeyListener(ShootFireball, ConsoleKey.V));
        }

        public bool OnCollision(BaseObject baseObject)
        {
            if (baseObject.GetType().Name == "Monster")
            {
                ConsoleHandler.StartFlashScreen(ConsoleColor.DarkRed, 5);
                health.health--;
                HealthCheck();

            }
            return true;
        }

        void HealthCheck()
        {
            if (health.health <= 0)
            {
                Console.Clear();
                Console.WriteLine("You DIED \nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        public bool MoveUp()
        {
            if (fireballAim)
            {
                aimingIndicator.SetPosition(position.x, position.y - 1);
                aimingIndicator.direction = Movement.Direction.North;
            }
            else
            {
                movement.Move(Movement.Direction.North);
            }
            direction = Movement.Direction.North;

            return true;
        }
        public bool MoveDown()
        {
            if (fireballAim)
            {
                aimingIndicator.SetPosition(position.x, position.y + 1);
                aimingIndicator.direction = Movement.Direction.South;

            }
            else
            {
                movement.Move(Movement.Direction.South);
            }

            direction = Movement.Direction.South;
            return true;
        }
        public bool MoveLeft()
        {
            if (fireballAim)
            {
                aimingIndicator.SetPosition(position.x - 1, position.y);
                aimingIndicator.direction = Movement.Direction.West;

            }
            else
            {
                movement.Move(Movement.Direction.West);
            }

            direction = Movement.Direction.West;
            return true;
        }
        public bool MoveRight()
        {
            if (fireballAim)
            {
                aimingIndicator.SetPosition(position.x + 1, position.y);
                aimingIndicator.direction = Movement.Direction.East;

            }
            else
            {
                movement.Move(Movement.Direction.East);
            }

            direction = Movement.Direction.East;
            return true;
        }

        public bool ShootFireball()
        {
            if (!fireballAim)
            {
                fireballAim = true;
                GetMap().addingObjecsFuncs.Add(SpawnIndicator);

            }
            else
            {
                fireballAim = false;
                aimingIndicator.destroy = true;
                GetMap().addingObjecsFuncs.Add(SpawnFireball);
            }
            return true;
        }
        public bool SpawnIndicator()
        {
            aimingIndicator = new AimingIndicator(GetMap(), position.x, position.y, direction);
            GetMap().newObjects.Add(aimingIndicator);
            return true;
        }
        public bool SpawnFireball()
        {
            GetMap().newObjects.Add(new Fireball(GetMap(), aimingIndicator.position.x, aimingIndicator.position.y, aimingIndicator.direction));
            return true;
        }

    }
}
