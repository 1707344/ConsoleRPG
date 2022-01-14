using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Monster: BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        PathFinder pathFinder;
        Cooldown movementCooldown;
        Collider collider;

        
        public Monster(Map map, int x, int y): base(map)
        {
            collider = new Collider(this, OnCollision);
            renderer = new Renderer(this, '@', 2, new Color(255, 43, 43), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            pathFinder = new PathFinder(this, 9);
            movementCooldown = new Cooldown(500);
        }

        public bool OnCollision(BaseObject baseObject)
        {
            //Can only kill an enemy when they stop moving
            if (movement.stopMovement)
            {
                destroy = true;
            }
            return true;
        }

        public override void Update()
        {
            if (movementCooldown.IsCooldownDone())
            {
                movementCooldown.StartCooldown();
                movement.Move(pathFinder.GetNextMove(MiniDC.player.GetComponent<Position>()));
            }
        }

    }
}
