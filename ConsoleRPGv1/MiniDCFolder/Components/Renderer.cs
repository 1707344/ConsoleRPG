using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    /*
     * Using a way to get all of the colors simple example
     * You just use this to write what you want in any color.
     * Console.Write("\x1b[48;2;" + r + ";" + g + ";" + b + "m");   ---- Background
     * Console.Write("\x1b[38;2;" + (255-r) + ";" + (255-g) + ";" + (255-b) + "m");  --- Foreground
     * the r, g, and b are what colors you want them to be they need to be ints
     * 
     */
    //
    public class Renderer: Component
    {
        public Color textColor;
        public Color backgroundColor;
        char icon;
        public int layer;

        public Renderer(BaseObject obj, char icon, int layer, Color textColor, Color backgroundColor): base(obj)
        {
            this.layer = layer;
            this.icon = icon;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
        }

        public void Display() {
            //ConsoleHandler.SetConsoleColor(textColor, backgroundColor);
            Console.Write("\x1b[48;2;" + backgroundColor.r + ";" + backgroundColor.g + ";" + backgroundColor.b + "m");
            Console.Write("\x1b[38;2;" + textColor.r + ";" + textColor.g + ";" + textColor.b + "m");
            Console.Write(icon);
        }
    }
}
