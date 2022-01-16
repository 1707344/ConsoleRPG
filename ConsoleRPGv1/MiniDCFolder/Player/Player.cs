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

        float testStrength = 20;

        //Starts false. Player clicks shoot button. Starts aiming(true). Then clicks again then fireball fires
        public bool fireballAim;
        public bool fireballAimSwitch;
        Cooldown fireballAimFlashCooldown;

        Color baseColor;
        Color fireballAimFlashColor1;
        Color fireballAimFlashColor2;

        public Player(Map map, int x, int y) : base(map)
        {

            baseColor = new Color(92, 255, 133, 0.99f);
            fireballAimFlashColor1 = new Color(255, 100, 50, 0.99f);
            fireballAimFlashColor2 = new Color(200, 50, 50, 0.99f);

            health = new Health(this, 10, OnDeath);
            //☺
            //Θ
            renderer = new Renderer(this, '☺', 3, baseColor, false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);
            fireballAimFlashCooldown = new Cooldown(100);



            InputHandler.AddListener(new KeyListener(MoveUp, ConsoleKey.UpArrow));
            InputHandler.AddListener(new KeyListener(MoveDown, ConsoleKey.DownArrow));
            InputHandler.AddListener(new KeyListener(MoveLeft, ConsoleKey.LeftArrow));
            InputHandler.AddListener(new KeyListener(MoveRight, ConsoleKey.RightArrow));
            InputHandler.AddListener(new KeyListener(ShootFireball, ConsoleKey.F));
            InputHandler.AddListener(new KeyListener(UnSelectSpell, ConsoleKey.Spacebar));
            InputHandler.AddListener(new KeyListener(Test, ConsoleKey.B));
        }


        public override void Update()
        {
            base.Update();
            if(fireballAim && fireballAimFlashCooldown.IsCooldownDone())
            {
                fireballAimFlashCooldown.StartCooldown();
                fireballAimSwitch = !fireballAimSwitch;
                renderer.color = fireballAimSwitch ? fireballAimFlashColor1 : fireballAimFlashColor2;
            }
            else if(!fireballAim)
            {
                renderer.color = baseColor;
            }
        }

        public bool Test()
        {
            if(testStrength > 20)
            {
                testStrength = 20;
            }
            else
            {
                testStrength = 100;
            }
            return true;
        }
        public bool OnCollision(BaseObject baseObject)
        {
            if (baseObject.GetType().Name == "Monster")
            {
                //ConsoleHandler.StartFlashScreen(ConsoleColor.DarkRed, 5);
                //health.health--;
                //HealthCheck();

            }
            return true;
        }
        public bool OnDeath()
        {
            Console.Clear();
            Console.WriteLine("You DIED \nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
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
        public bool UnSelectSpell()
        {
            if(aimingIndicator == null)
            {
                return true;
            }
            fireballAim = false;
            
            aimingIndicator.destroy = true;
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
                if(!GetMap().GetObjectsAtPosition(aimingIndicator.position.x, aimingIndicator.position.y).Exists(x => x.obj.GetType() == typeof(Wall)))
                {
                    GetMap().addingObjecsFuncs.Add(SpawnFireball);
                }
                
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
            GetMap().newObjects.Add(new Fireball(GetMap(), aimingIndicator.position.x, aimingIndicator.position.y, aimingIndicator.direction, testStrength));
            return true;
        }



    }
}
