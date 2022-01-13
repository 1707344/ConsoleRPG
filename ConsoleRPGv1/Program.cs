using System;
using System.Runtime.InteropServices;

namespace ConsoleRPGv1
{
    class Program
    {
        //For full RGB colors----------
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        //------------------------------


        static void Main(string[] args)
        {
            ActivateColors();
            int width = 55;
            int height = 20;
            int b = 0;
            Console.CursorVisible = false;
            int num = 255;

            while (false)
            {
                b++;
                Console.SetCursorPosition(0, 0);
                for (int g = 0; g < num; g += num / height)
                {
                    for (int r = 0; r < num; r += num / width)
                    {
                        Console.Write("\x1b[48;2;" + r + ";" + g + ";" + b + "m");
                        Console.Write("\x1b[38;2;" + (255-r) + ";" + (255-g) + ";" + (255-b) + "m");
                        Console.Write("=");
                    }
                    Console.WriteLine();
                }
                if(b >= 255)
                {
                    b = 0;
                }
            }
            ConsoleRPG.MiniDC.PlayGame();
        }

        static void ActivateColors()
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
        }
    }
}
