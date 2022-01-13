using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class EmptySpace: BaseObject
    {
        Renderer renderer;
        Position position;
        public EmptySpace(Map map, int x, int y): base(map)
        {
            position = new Position(this, x, y);
            renderer = new Renderer(this, '.', -1, new Color(Color.Colors.White), new Color(Color.Colors.Black));
        }
    }
}
