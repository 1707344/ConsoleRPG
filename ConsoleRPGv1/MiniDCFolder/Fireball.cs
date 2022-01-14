using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{


    class Fireball : BaseObject
    {
        Renderer renderer;
        Position position;
        Movement movement;
        Collider collider;

        public Fireball(Map map, int x, int y): base(map)
        {
            renderer = new Renderer(this, '☼', 3, new Color(255, 66, 41), false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);
        }

        public bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
    }
}
