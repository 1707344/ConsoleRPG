using System.Collections.Generic;

namespace ConsoleRPG
{
    /// <summary>
    /// Movement class requires the object to have a Position component
    /// </summary>
    public class Movement : Component
    {
        public bool stopMovement = false;

        public enum Direction
        {
            North,
            South,
            East,
            West,
            None
        }


        public Movement(BaseObject obj) : base(obj)
        {

        }

        public bool Move(Direction dir)
        {
            if (stopMovement)
            {
                return false;
            }

            int dx = 0;
            int dy = 0;

            switch (dir)
            {
                case Direction.North:
                    dy = -1;
                    break;
                case Direction.South:
                    dy = 1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
            }

            Position position = obj.GetComponent<Position>();
            List<BaseObject> objects = obj.GetMap().GetObjectsAtPosition(position.x + dx, position.y + dy).ConvertAll(x => x.obj);

            foreach (BaseObject baseObject in objects.FindAll(x => x.GetComponent<Collider>() != null))
            {
                baseObject.GetComponent<Collider>().Collision(obj);
            }

            if (objects.Exists(x => x.GetComponent<Collider>() != null && !x.GetComponent<Collider>().isTrigger))
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.Collision(objects.Find(x => x.GetComponent<Collider>() != null && !x.GetComponent<Collider>().isTrigger));
                }
                return false;
            }

            position.x += dx;
            position.y += dy;
            return true;

        }
    }
}
