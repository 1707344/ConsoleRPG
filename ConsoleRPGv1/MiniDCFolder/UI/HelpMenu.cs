using System;

namespace ConsoleRPG
{
    public static class HelpMenu
    {
        static HelpMenu()
        {
        }

        public static void Display()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Properties.Resources.HelpText);
            Console.WriteLine("\n\nPress any button to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
