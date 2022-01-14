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
        /// <summary>
        /// A value from 1 to 0. 
        /// 1 is completely opaque. 0 is copmletely transparent
        /// </summary>
        public float a;


        public void SetColor(int r, int g, int b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color()
        {

        }
        public Color(int r, int g, int b, float a=1)
        {
            SetColor(r, g, b, a);
        }

        public Color(Colors color)
        {
            Color rgbColor = new Color();
            switch (color)
            {
                case Colors.White:
                    rgbColor = new Color(200, 200, 200, 1);
                    break;
                case Colors.Black:
                    rgbColor = new Color(0, 0, 0, 1);
                    break;
            }

            r = rgbColor.r;
            g = rgbColor.g;
            b = rgbColor.b;
        }

        //Thank you, https://github.com/ProfJski/ArtColors. For subtractive color mixing
        //You are amazing
        public static Color ColorMixSub(Color a, Color b)
        {

            if(b == null)
            {
                return a;
            }

            Color returnColor;
            Color c, d, f;
            c = new Color();
            d = new Color();
            f = new Color();


            c = ColorInv(a);
            d = ColorInv(b);
            
            f.r = Math.Max(0, 255 - c.r - d.r);
            f.g = Math.Max(0, 255 - c.g - d.g);
            f.b = Math.Max(0, 255 - c.b - d.b);
            float cd = ColorDistance(a, b);
            cd = 4.0f * b.a * (1.0f - b.a) * cd;
            returnColor = ColorMixLin(ColorMixLin(a, b, b.a), f, cd);

            returnColor.a = 255;
            return returnColor;
        }

        static Color ColorInv(Color color)
        {
            return new Color(255 - color.r, 255 - color.g, 255 - color.b);
        }

        static float ColorDistance(Color a, Color b)
        {
            float distance = (float)((a.r - b.r) * (a.r - b.r) + (a.g - b.g) * (a.g - b.g) + (a.b - b.b) * (a.b - b.b));
            distance = (float)Math.Sqrt(distance) / ((float)Math.Sqrt(3.0) * 255); //scale to 0-1
            return distance;
        }

        static Color ColorMixLin(Color a, Color b, float blend)
        {
            Color color = new Color();
            color.r = (int)((1.0f - blend) * a.r + blend * b.r);
            color.g = (int)((1.0f - blend) * a.g + blend * b.g);
            color.b = (int)((1.0f - blend) * a.b + blend * b.b);
            color.a = (int)((1.0f - blend) * a.a + blend * b.a);

            return color;
        }
        //--------------------

        //Additive color mixing
        //Thanks https://gist.github.com/JordanDelcros/518396da1c13f75ee057
        public static Color ColorMixAdd(Color a, Color b)
        {//((b.r * b.a / (float)mix.a) + (a.r * a.a * (1f - b.a) / (float)mix.a))
            Color mix = new Color();

            if(b == null)
            {
                return a;
            }

            if(a.a == 0 && b.a == 0)
            {
                return new Color(0, 0, 0, 0);
            }
            else if(a.a == 0)
            {
                return b;
            }else if(b.a == 0)
            {
                return a;
            }

            mix.a = 1 - (1 - b.a) * (1 - a.a); // alpha
            mix.r = (int)((b.r * b.a / (float)mix.a) + (a.r * a.a * (1f - b.a) / (float)mix.a)); // red
            mix.g = (int)((b.g * b.a / (float)mix.a) + (a.g * a.a * (1f - b.a) / (float)mix.a)); // green
            mix.b = (int)((b.b * b.a / (float)mix.a) + (a.b * a.a * (1f - b.a) / (float)mix.a)); // blue

            return mix;
        }
    }
}
