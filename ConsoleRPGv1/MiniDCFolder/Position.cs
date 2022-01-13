using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Position: Component
    {
        public int x;
        public int y;
        public Position(BaseObject obj, int x, int y): base(obj)
        {
            this.x = x;
            this.y = y;
        }
    }
}
