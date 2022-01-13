using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Renderer: Component
    {
        public ConsoleColor textColor;
        public ConsoleColor backgroundColor;
        char icon;
        public int layer;

        public Renderer(BaseObject obj, char icon, int layer, ConsoleColor textColor, ConsoleColor backgroundColor): base(obj)
        {
            this.layer = layer;
            this.icon = icon;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
        }

        public void Display() {
            ConsoleHandler.SetConsoleColor(textColor, backgroundColor);
            Console.Write(icon);
        }
    }
}
