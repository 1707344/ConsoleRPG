using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class Color
    {
        public enum Colors
        {
           White,
           Black
        }

        public int r;
        public int g;
        public int b;

        public void SetColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color()
        {

        }
        public Color(int r, int g, int b)
        {
            SetColor(r, g, b);
        }

        public Color(Colors color)
        {
            Color rgbColor = new Color();
            switch (color)
            {
                case Colors.White:
                    rgbColor = new Color(200, 200, 200);
                    break;
                case Colors.Black:
                    rgbColor = new Color(0, 0, 0);
                    break;
            }

            r = rgbColor.r;
            g = rgbColor.g;
            b = rgbColor.b;
        }
    }
}
