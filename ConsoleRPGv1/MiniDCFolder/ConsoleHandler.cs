using System;
using System.Threading;
using System.Collections.Generic;

namespace ConsoleRPG
{


    /// <summary>
    /// The ConsoleHandler is the intermediary between the game and the actual console. 
    /// Without this the console is too slow. 
    /// </summary>
    public static class ConsoleHandler
    {
        /// <summary>
        /// Holds everything that will be outputted to the console
        /// </summary>
        public static ConsoleCharacter[,] consoleCharacters = new ConsoleCharacter[50,50];
        /// <summary>
        /// The ConsoleCharacters that are actually displayed on the screen
        /// </summary>
        static ConsoleCharacter[,] activeCharacters = new ConsoleCharacter[50, 50];
        public static void SetConsoleColor(ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            if (textColor != Console.ForegroundColor)
            {
                Console.ForegroundColor = textColor;

            }

            if (backgroundColor != Console.BackgroundColor)
            {
                Console.BackgroundColor = backgroundColor;
            }
        }

        public static void StartFlashScreen(ConsoleColor color, int flashLength)
        {

            Console.BackgroundColor = color;
            Console.Clear();
            Thread.Sleep(flashLength);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            for (int x = 0; x < consoleCharacters.GetLength(0); x++)
            {
                for (int y = 0; y < consoleCharacters.GetLength(1); y++)
                {
                    activeCharacters[x, y] = new ConsoleCharacter(' ', new Color(Color.Colors.White), new Color(Color.Colors.Black));
                }
            }

        }

        static ConsoleHandler()
        {
            for (int x = 0; x < consoleCharacters.GetLength(0); x++)
            {
                for (int y = 0; y < consoleCharacters.GetLength(1); y++)
                {
                    consoleCharacters[x, y] = new ConsoleCharacter(' ', new Color(Color.Colors.White), new Color(Color.Colors.Black));
                    activeCharacters[x, y] = new ConsoleCharacter(' ', new Color(Color.Colors.White), new Color(Color.Colors.Black));
                }
            }
        }

        /// <summary>
        /// Updates the 
        /// </summary>
        public static void Display()
        {
            //Finding all positions that need to be updated
            List<(int x, int y)> needsToBeUpdated = new List<(int x, int y)>();   
            for (int x = 0; x < consoleCharacters.GetLength(0); x++)
            {
                for (int y = 0; y < consoleCharacters.GetLength(1); y++)
                {
                    if(consoleCharacters[x, y].IsDifferent(activeCharacters[x, y]))
                    {
                        needsToBeUpdated.Add((x, y));
                        
                    }
                }
            }

            //Updating activeCharacters and actually displaying to the console
            foreach ((int x, int y) position in needsToBeUpdated)
            {
                activeCharacters[position.x, position.y] = consoleCharacters[position.x, position.y];
                Console.SetCursorPosition(position.x, position.y);
                Console.Write("\x1b[48;2;" + activeCharacters[position.x, position.y].backgroundColor.r + ";" + activeCharacters[position.x, position.y].backgroundColor.g + ";" + activeCharacters[position.x, position.y].backgroundColor.b + "m");
                Console.Write("\x1b[38;2;" + activeCharacters[position.x, position.y].textColor.r + ";" + activeCharacters[position.x, position.y].textColor.g + ";" + activeCharacters[position.x, position.y].textColor.b + "m" + activeCharacters[position.x, position.y].text);
            }
        }

        public static void ClearConsoleCharacters()
        {
            for (int x = 0; x < consoleCharacters.GetLength(0); x++)
            {
                for (int y = 0; y < consoleCharacters.GetLength(1); y++)
                {
                    consoleCharacters[x, y] = new ConsoleCharacter(' ', new Color(Color.Colors.White), new Color(Color.Colors.Black));
                }
            }
        }
    }

    /// <summary>
    /// Holds the color of the character, the background color, and the actual char of the character
    /// </summary>
    public struct ConsoleCharacter
    {
        public char text;
        public Color textColor;
        public Color backgroundColor;

        public ConsoleCharacter(char text, Color textColor, Color backgroundColor)
        {
            this.text = text;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
        }

        public bool IsDifferent(ConsoleCharacter consoleCharacter)
        {

            (int r, int g, int b) backgroundColorA = (backgroundColor.r, backgroundColor.g, backgroundColor.b);
            (int r, int g, int b) backgroundColorB = (consoleCharacter.backgroundColor.r, consoleCharacter.backgroundColor.g, consoleCharacter.backgroundColor.b);
            (int r, int g, int b) textColorA = (textColor.r, textColor.g, textColor.b);
            (int r, int g, int b) textColorB = (consoleCharacter.textColor.r, consoleCharacter.textColor.g, consoleCharacter.textColor.b);

            if(backgroundColorA != backgroundColorB)
            {
                return true;
            }

            if(textColorA != textColorB)
            {
                return true;
            }

            if(consoleCharacter.text != text)
            {
                return true;
            }

            return false;
        }
    }
}
