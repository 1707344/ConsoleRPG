using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleRPG
{


    /// <summary>
    /// The ConsoleHandler is the intermediary between the game and the actual console. 
    /// Without this the console is too slow. 
    /// </summary>
    public static class ConsoleHandler
    {
        public static void SetConsoleColor(ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            if(textColor != Console.ForegroundColor)
            {
                Console.ForegroundColor = textColor;
                
            }

            if(backgroundColor != Console.BackgroundColor)
            {
                Console.BackgroundColor = backgroundColor;
            }
        }

        public static void StartFlashScreen(ConsoleColor color, int flashLength)
        {

            Console.BackgroundColor = color;
            Console.Clear();
            Thread.Sleep(flashLength);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

        }
    }
}
