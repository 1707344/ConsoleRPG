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
        public Color color;
        char icon;
        /// <summary>
        /// If true the icon will not be rendered and only the color as a backround will. 
        /// If false the icon will be rendered with the color as the text color.
        /// </summary>
        public bool isBackground;
        public int layer;
        public bool isVisible = true;

        /// <summary>
        /// Does the background also color in to the left and right(the empty spaces)?
        /// </summary>
        public bool backgroundStretches = false;

        public Renderer(BaseObject obj, char icon, int layer, Color color, bool isBackground): base(obj)
        {
            this.layer = layer;
            this.icon = icon;
            this.color = color;
            this.isBackground = isBackground;
        }

        public void Display(Color alternateColor=null) {
            //ConsoleHandler.SetConsoleColor(textColor, backgroundColor);

            if (!isVisible)
            {
                return;
            }

            Color outputColor = alternateColor == null ? color : alternateColor;

            if (isBackground)
            {
                Console.Write("\x1b[48;2;" + outputColor.r + ";" + outputColor.g + ";" + outputColor.b + "m");

                if (backgroundStretches)
                {
                    int left = Console.GetCursorPosition().Left;
                    int top = Console.GetCursorPosition().Top;

                    Console.SetCursorPosition(left - 1, top);
                    Console.Write(' ');
                    //Console.SetCursorPosition(left + 1, top);
                    //Console.Write(' ');

                    Console.SetCursorPosition(left, top);
                    if(!obj.GetMap().emptySpaceWithColor.Exists(e => e.x + 1 == left && e.y == top))
                    {
                        obj.GetMap().emptySpaceWithColor.Add((left - 1, top, this));
                    }

                    //obj.GetMap().emptySpaceReset.Add((left + 1, top));

                    Color black = new Color(Color.Colors.Black);
                    Console.Write("\x1b[48;2;" + black.r + ";" + black.g + ";" + black.b + "m");
                }
            }
            else
            {
                Console.Write("\x1b[38;2;" + outputColor.r + ";" + outputColor.g + ";" + outputColor.b + "m");
                Console.Write(icon);


            }

        }
    }
}
