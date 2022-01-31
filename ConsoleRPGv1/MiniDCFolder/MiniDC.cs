using System;
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
        static Map[] maps;
        //static string[] maps;//List of map names

        public static void PlayGame()
        {
            Reset();

            LoadMaps();
            
            Console.CursorVisible = false;
            time = new Time();


            StartInputThread();

            ShowStartMenu();
            Console.Clear();

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

            EndScreen.Display();

        }

        public static void StartLevel(int level)
        {
            InputHandler.ClearListeners();
            ConsoleHandler.ClearActiveCharacters();
            Console.Clear();
            map = null;
            map = maps[level];
            CreatePlayer(map, 1, 1);


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
                            //new Monster(map, i, j);
                            EndScreen.startingNumMonsters++;
                            break;
                        case 'P':
                            CreatePlayer(map, i, j);
                            break;
                        case 'S':
                            new Stairs(map, i, j);
                            break;
                        default:
                            //objectInsideCell = null;
                            break;
                    }

                }
            }


            return map;

        }

        /// <summary>
        /// If player already exists stats like health will carry over
        /// </summary>
        static void CreatePlayer(Map map, int x, int y)
        {
            if(player == null)
            {
                player = new Player(map, x, y);
            }
            else
            {
                Health playerHealth = player.GetComponent<Health>();
                player = new Player(map, x, y);
                player.GetComponent<Health>().health = playerHealth.health;
                player.GetComponent<Health>().maxHealth = playerHealth.maxHealth;
            }

        }

        static void ShowStartMenu()
        {
            Console.Clear();
            StartMenu.LoadInputs();
            StartMenu.isShowing = true;
            while (StartMenu.isShowing)
            {
                StartMenu.Display();
            }
            StartMenu.Close();
            StartLevel(0);

        }

        static void StartInputThread()
        {
            if (InputHandler.thread == null || !InputHandler.thread.IsAlive)
            {
                Thread inputThread = new Thread(new ThreadStart(InputLoop));
                inputThread.Start();

                InputHandler.thread = inputThread;

            }
        }

        static void LoadMaps()
        {
            MapGenerator mapGen = new MapGenerator();

            maps = new Map[] { mapGen.CellsToMap(mapGen.GenerateMap()), mapGen.CellsToMap(mapGen.GenerateMap()) };
        }

        static void Reset()
        {
            player = null;
            map = null;
            startNextLevel = false;
            currentLevel = 0;
            gamePlaying = true;
            InputHandler.ClearListeners();

        }

    }
}
