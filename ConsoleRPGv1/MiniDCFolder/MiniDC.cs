﻿using System;
using System.Threading;

namespace ConsoleRPG
{
    ////░▒▓
    public static class MiniDC
    {
        public static Player player;
        static Map map;
        public static Time time;
        public static bool gamePlaying = true;
        static int highestFrame;
        static bool inputLoop;
        public static bool startNextLevel;
        public static int currentLevel;
        static string[] maps;//List of map names

        public static void PlayGame()
        {
            MapGenerator mapGen = new MapGenerator();
            Cell[,] mapCells = mapGen.GenerateMap();


            maps = new string[] { mapGen.CellsToString(mapGen.GenerateMap()), Properties.Resources.Map2 };
            Console.CursorVisible = false;
            time = new Time();

            Thread inputThread = new Thread(new ThreadStart(InputLoop));
            inputThread.Start();


            map = TextFileToMap(mapGen.CellsToString(mapGen.GenerateMap()));

            ShowStartMenu();
            Console.Clear();





            //EndScreen.Display();

        }

        public static void GameLoop()
        {
            player.LoadInput();

            while (gamePlaying)
            {
                time.SetFrameStartTime();

                Console.SetCursorPosition(0, 0);

                map.Display();
                ConsoleHandler.Display();
                Console.WriteLine();


                InGameMenu.Display(map.height);

                //Console.BackgroundColor = ConsoleColor.Black;
                time.SetFrameEndTime();
                time.SetDeltaTime();


                Console.SetCursorPosition(60, 2);
                Console.WriteLine(time.GetDeltaTime() + "            ");

            }

            if (startNextLevel)
            {
                startNextLevel = false;
                gamePlaying = true;
                currentLevel++;
                if (currentLevel >= maps.Length)
                {
                    EndScreen.Display();
                    return;
                }

                StartLevel(currentLevel);
            }

        }

        public static void StartLevel(int level)
        {
            InputHandler.ClearListeners();
            ConsoleHandler.ClearActiveCharacters();
            Console.Clear();
            map = null;
            map = TextFileToMap(maps[level]);

            Color black = new Color(Color.Colors.Black);
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width * 2; x++)
                {
                    Console.SetCursorPosition(x, y);


                    Console.Write("\x1b[48;2;" + black.r + ";" + black.g + ";" + black.b + "m");
                    Console.Write("\x1b[38;2;" + black.r + ";" + black.g + ";" + black.b + "m");
                    Console.Write(" ");
                }
            }

            GameLoop();//Start game loop
        }



        public static void InputLoop()
        {
            while (true)
            {
                InputHandler.Update();
            }
        }

        static Map TextFileToMap(string mapString)
        {
            string[] text = new string[10];
            try
            {

                text = mapString.Replace("\r", "").Split('\n');
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = text[i].Replace(" ", "");
            }
            char[,] charMap = new char[text.Length, text[0].Length];
            //Cell[,] cellMap = new Cell[text.Length, text[0].Length];
            Map map = new Map(charMap.GetLength(1), charMap.GetLength(0));




            for (int x = 0; x < text[0].Length; x++)
            {
                for (int y = 0; y < text.Length; y++)
                {
                    charMap[y, x] = text[y][x];
                }
            }



            for (int i = 0; i < charMap.GetLength(1); i++)
            {
                for (int j = 0; j < charMap.GetLength(0); j++)
                {


                    switch (charMap[j, i])
                    {
                        case '#':
                            new Wall(map, i, j);
                            break;
                        case 'F':
                            new FreezingTrap(map, i, j);
                            break;
                        case 'M':
                            new Monster(map, i, j);
                            EndScreen.startingNumMonsters++;
                            break;
                        case 'P':
                            player = new Player(map, i, j);
                            break;
                        case 'S':
                            new Stairs(map, i, j);
                            break;
                        default:
                            //objectInsideCell = null;
                            break;
                    }

                    //cellMap[j, i].objectInsideCell = objectInsideCell;
                }
            }


            return map;

        }

        static void ShowStartMenu()
        {
            StartMenu.isShowing = true;
            while (StartMenu.isShowing)
            {
                StartMenu.Display();
            }

            StartLevel(0);

        }

    }
}
