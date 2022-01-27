using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public static class RetryScreen
    {
        public static void Display()
        {

            int r = 184;
            int g = 0;
            int b = 18;
            Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m");
            Console.WriteLine(Properties.Resources.YouDiedText + "\n\n");
            Console.WriteLine("Click any button to restart");
            Console.ReadKey();

            Console.BackgroundColor = ConsoleColor.Black;

            MiniDC.PlayGame();
        }
    }
}
