using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    public static class StartMenu
    {
        static List<Button> buttons = new List<Button>();
        static int buttonSelected;
        public static bool isShowing = true;
        static bool isHiding = false;
        static StartMenu()
        {
            buttonSelected = 0;
            buttons.Add(new Button(5, 10, "Start", ConsoleColor.White, ConsoleColor.Yellow, StartOption));
            buttons.Add(new Button(5, 11, "Help", ConsoleColor.White, ConsoleColor.Yellow, HelpOption));
            buttons.Add(new Button(5, 12, "Exit", ConsoleColor.White, ConsoleColor.DarkRed, ExitOption));

            InputHandler.AddListener(new KeyListener(delegate () { buttonSelected--; return true; }, ConsoleKey.UpArrow));
            InputHandler.AddListener(new KeyListener(delegate () { buttonSelected++; return true; }, ConsoleKey.DownArrow));
            InputHandler.AddListener(new KeyListener(delegate () { EnterClicked(); return true; }, ConsoleKey.Spacebar));

        }
        static void EnterClicked()
        {
            if (isShowing)
            {
                buttons[buttonSelected].func();
            }
        }
        public static void Display()
        {
            if (isHiding)
            {
                return;
            }

            Console.SetCursorPosition(0, 2);
            //https://www.coolgenerator.com/ascii-text-generator
            string title = "   ███████╗███████╗ ██████╗ █████╗ ██████╗ ███████╗" +
                         "\n   ██╔════╝██╔════╝██╔════╝██╔══██╗██╔══██╗██╔════╝" +
                         "\n   █████╗  ███████╗██║     ███████║██████╔╝█████╗  " +
                         "\n   ██╔══╝  ╚════██║██║     ██╔══██║██╔═══╝ ██╔══╝  " +
                         "\n   ███████╗███████║╚██████╗██║  ██║██║     ███████╗" +
                         "\n   ╚══════╝╚══════╝ ╚═════╝╚═╝  ╚═╝╚═╝     ╚══════╝";
            Console.Write(title);


            if (buttonSelected < 0)
            {
                buttonSelected = buttons.Count - 1;
            }
            else if (buttonSelected >= buttons.Count)
            {
                buttonSelected = 0;
            }

            foreach (Button button in buttons)
            {
                button.isSelected = false;
            }

            buttons[buttonSelected].isSelected = true;

            foreach (Button button in buttons)
            {
                button.Display();
            }
        }

        static bool StartOption()
        {
            isShowing = false;
            Console.Clear();
            return true;
        }


        static bool HelpOption()
        {
            isHiding = true;
            HelpMenu.Display();
            isHiding = false;
            return true;
        }

        static bool ExitOption()
        {
            Console.Clear();
            Environment.Exit(0);
            return true;
        }
    }
    class Button
    {
        int x;
        int y;
        string text;
        ConsoleColor textColor;
        ConsoleColor selectedColor;
        public bool isSelected;
        public Func<bool> func;

        public Button(int x, int y, string text, ConsoleColor textColor, ConsoleColor selectedColor, Func<bool> func)
        {
            this.func = func;
            this.x = x;
            this.y = y;
            this.text = text;
            this.textColor = textColor;
            this.selectedColor = selectedColor;
            isSelected = false;
        }

        public void Display()
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = isSelected ? selectedColor : textColor;
            Console.Write(text);

        }

        public void Display(int yOffset)
        {
            Console.SetCursorPosition(x, y + yOffset);
            Console.ForegroundColor = isSelected ? selectedColor : textColor;
            Console.Write(text);

        }
    }
}
