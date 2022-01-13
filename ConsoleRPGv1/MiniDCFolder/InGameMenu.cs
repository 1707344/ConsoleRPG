using System;
using System.Collections.Generic;
using System.Text;

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
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, yOffset);
            Console.WriteLine("===========================================");

            DisplayStatusMenu();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            DisplayInventoryMenu();

        }

        static void DisplayStatusMenu()
        {
            
            Player player = MiniDC.player;
            Console.WriteLine($"Health: {player.GetComponent<Health>().health}/{player.GetComponent<Health>().maxHealth}       ");
        }
        static void DisplayInventoryMenu()
        {
            Inventory.DisplayInventory();
        }
    }
}
