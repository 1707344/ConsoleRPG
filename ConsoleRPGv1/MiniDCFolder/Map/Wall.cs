using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Wall: BaseObject
    {
        Renderer renderer;
        Position position;
        Collider collider;
        public Wall(Map map, int x, int y): base(map)
        {
            collider = new Collider(this, OnCollision);
            position = new Position(this, x, y);
            renderer = new Renderer(this, '#', 1, new Color(Color.Colors.White), new Color(Color.Colors.Black));
        }

        bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
    }
}
