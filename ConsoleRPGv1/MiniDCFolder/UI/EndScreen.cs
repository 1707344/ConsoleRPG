using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public static class EndScreen
    {
        public enum Ending
        {
            Slaughterer,//Killed all monsters
            Pacifist,//Killed no monsters
            None,
        }

        public static int startingNumMonsters;
        public static Ending ending = Ending.None;

        public static void Display()
        {
            Console.Clear();
            Console.WriteLine("YOU WIN\n\n");
            switch (ending)
            {
                case Ending.Slaughterer:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Achievement: SLAUGHTERER\nYou killed every monster");
                    break;
                case Ending.Pacifist:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Achievment: Pacifist\nYou killed NO monsters");
                    break;
            }
        }
    }
}
