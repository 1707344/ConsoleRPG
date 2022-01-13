using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Player: BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        Collider collider;
        Health health;

        public Player(Map map, int x, int y): base(map)
        {
            health = new Health(this, 5);
            renderer = new Renderer(this, '@', 3, new Color(92, 255, 133), new Color(Color.Colors.Black));
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);


            InputHandler.AddListener(new KeyListener(MoveUp, ConsoleKey.UpArrow));
            InputHandler.AddListener(new KeyListener(MoveDown, ConsoleKey.DownArrow));
            InputHandler.AddListener(new KeyListener(MoveLeft, ConsoleKey.LeftArrow));
            InputHandler.AddListener(new KeyListener(MoveRight, ConsoleKey.RightArrow));
        }
        
        public bool OnCollision(BaseObject baseObject)
        {
            if(baseObject.GetType().Name == "Monster")
            {
                ConsoleHandler.StartFlashScreen(ConsoleColor.DarkRed, 5);
                health.health--;
                HealthCheck();

            }
            return true;
        }

        void HealthCheck()
        {
            if(health.health <= 0)
            {
                Console.Clear();
                Console.WriteLine("You DIED \nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        public bool MoveUp()
        {
            movement.Move(Movement.Direction.North);
            return true;
        }
        public bool MoveDown()
        {
            movement.Move(Movement.Direction.South);
            return true;
        }
        public bool MoveLeft()
        {
            movement.Move(Movement.Direction.West);
            return true;
        }
        public bool MoveRight()
        {
            movement.Move(Movement.Direction.East);
            return true;
        }

    }
}
