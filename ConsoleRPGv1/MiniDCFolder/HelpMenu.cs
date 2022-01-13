using System;
using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine("Help Menu: ");
            Console.WriteLine("Use arrow keys and space to navigate the menus");
            Console.WriteLine("Use arrow keys to move around the world");
            Console.WriteLine("You can only kill a monster when they are immobile");
            Console.WriteLine("\nYou can place traps by hitting the corresponding key");
            Console.WriteLine("Trap description format below");
            Console.WriteLine("\t{The Trap Name}: {How many you have} ({The key to place})");
            Console.WriteLine("\n\nPress any enter to exit...");
            Console.ReadLine();
            Console.Clear();
        }

    }
}
