using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    public static class InGameMenu
    {
        static int menuSelected;
        static List<Button> menus;
        static InGameMenu()
        {
            menus = new List<Button>();
            menus.Add(new Button(5, 0, "Status", ConsoleColor.White, ConsoleColor.Magenta, Nothing));
            menus.Add(new Button(16, 0, "Inventory", ConsoleColor.White, ConsoleColor.Magenta, Nothing));
            menus.Add(new Button(30, 0, "Settings", ConsoleColor.White, ConsoleColor.Magenta, Nothing));
            InputHandler.AddListener(new KeyListener(delegate () { menuSelected = 0; return true; }, ConsoleKey.A));
            InputHandler.AddListener(new KeyListener(delegate () { menuSelected = 1; return true; }, ConsoleKey.S));
            InputHandler.AddListener(new KeyListener(delegate () { menuSelected = 2; return true; }, ConsoleKey.D));

            Console.Write("\x1b[48;2;" + 0 + ";" + 0 + ";" + 0 + "m");
        }



        static bool Nothing()
        {
            return true;
        }

        public static void Display(int yOffset)
        {
            UpdateMenu();
            DisplayMenu(yOffset);
        }

        static void UpdateMenu()
        {
            if (menuSelected < 0)
            {
                menuSelected = menus.Count - 1;
            }
            else if (menuSelected >= menus.Count)
            {
                menuSelected = 0;
            }

            foreach (Button button in menus)
            {
                button.isSelected = false;
            }
            menus[menuSelected].isSelected = true;
        }

        static void DisplayMenu(int yOffset)
        {

            Console.Write("\x1b[48;2;" + 0 + ";" + 0 + ";" + 0 + "m");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, yOffset);
            Console.WriteLine("===========================================");

            DisplayHealthBar();

            Console.WriteLine("\n===========================================");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            DisplayInventoryMenu();

        }

        static void DisplayHealthBar()
        {

            Player player = MiniDC.player;
            Health health = player.GetComponent<Health>();
            int healthBarSize = 40;
            //Console.WriteLine($"Health: {player.GetComponent<Health>().health}/{player.GetComponent<Health>().maxHealth}       ");
            for (int i = 0; i < healthBarSize; i++)
            {
                float r1 = 0;//Low end of gradient
                float r2 = 255;//High end of gradient
                float r4 = 1 - (health.health/health.maxHealth);//0-1 how far the gradient is translated
                float t = (float)i/(float)healthBarSize;//0-1 which part of gradient
                float r3 = 3;//How steep the gradient is
                float r = (r1 + -(float)Math.Pow((t + r4), r3)) * (r2-r1) + r2;

                Console.Write("\x1b[48;2;" + (int)r + ";" + 0 + ";" + 0 + "m");
                Console.Write(" ");
            }

            Console.Write("\x1b[48;2;" + 0 + ";" + 0 + ";" + 0 + "m");
        }
        static void DisplayInventoryMenu()
        {
            Inventory.DisplayInventory();
        }
    }
}
