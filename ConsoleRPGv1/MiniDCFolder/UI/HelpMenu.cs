using System;
using System.Threading;

namespace ConsoleRPG
{
    public static class HelpMenu
    {
        static HelpMenu()
        {
        }

        public static void Display()
        {
            Thread.Sleep(10);
            Console.Clear();
            Console.WriteLine(Properties.Resources.HelpText);
            Console.WriteLine("\n\nPress any button to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
