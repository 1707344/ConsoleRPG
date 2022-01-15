using System;

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
    public class Renderer : Component
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

        public Renderer(BaseObject obj, char icon, int layer, Color color, bool isBackground) : base(obj)
        {
            this.layer = layer;
            this.icon = icon;
            this.color = color;
            this.isBackground = isBackground;
        }

        public void Display(int x, int y, Color alternateColor = null)
        {
            //ConsoleHandler.SetConsoleColor(textColor, backgroundColor);

            if (!isVisible)
            {
                return;
            }

            Color outputColor = alternateColor == null ? color : alternateColor;

            if (isBackground)
            {
                ConsoleHandler.consoleCharacters[x, y].backgroundColor = outputColor;

                if (backgroundStretches)
                {

                    ConsoleCharacter leftConsoleCharacter = ConsoleHandler.consoleCharacters[x - 1, y];

                    leftConsoleCharacter.backgroundColor = outputColor;


                    if (!obj.GetMap().emptySpaceWithColor.Exists(e => e.x + 1 == x && e.y == y))
                    {
                        //obj.GetMap().emptySpaceWithColor.Add((left - 1, top, this));
                    }

                    //obj.GetMap().emptySpaceReset.Add((left + 1, top));

                    Color black = new Color(Color.Colors.Black);
                    //ConsoleHandler.consoleCharacters[]
                }
            }
            else
            {
                ConsoleHandler.consoleCharacters[x, y].text = icon;
                ConsoleHandler.consoleCharacters[x, y].textColor = outputColor;


            }

        }
    }
}
