using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class MapGenerator
    {
        /// <summary>
        /// Thanks https://en.wikipedia.org/wiki/Maze_generation_algorithm
        /// </summary>
        Random random;
        public MapGenerator()
        {
            random = new Random((int)DateTime.Now.Ticks);
        }

        /// <summary>
        /// Also know as the Randomized depth-first search
        /// 
        /// </summary>
        public Cell[,] GenerateMap()
        {
            Cell[,] cells = new Cell[10, 10];

            InitCells(cells);

            AddEmptyZones(cells);
            


            int currentLoop = 0;
            int maxLoop = 1000;

            (int x, int y) currentPosition = (0, 0);

            //The list of cells that acts as a way for the computer to backtrack
            //when it hits a dead end
            List<Cell> stack = new List<Cell>();

            bool backTracking = false;

            Cell startingCell = cells[currentPosition.x, currentPosition.y];
            Cell currentCell = startingCell;
            currentCell.visited = true;
            while(currentLoop < maxLoop)
            {
                
                if (AllVisited(cells))
                {
                    break;
                }

                if (backTracking)
                {
                    backTracking = false;
                }

                Cell nextCell = GetRandomCellAround(currentCell);

                if(nextCell == null)
                {
                    BackTrack(stack, ref currentCell);
                    continue;
                }

                RemoveWallBetweenCells(currentCell, nextCell);


                //Add new cell to the stack
                stack.Add(nextCell);


                nextCell.visited = true;
                currentCell = nextCell;

                
                currentLoop++;
                

            }

            return cells;
        }



        /// <summary>
        /// Converts a cell array to a string
        /// </summary>
        /// <returns></returns>
        public string CellsToString(Cell[,] cells)
        {
            char[,] charMap = CellsToCharArr(cells);

            //Adding corner pieces
            AddCornerPieces(charMap);

            //Adding player and enemies and stuff

            AddObjects(charMap);
            

            return CharArrToString(charMap);
        }
        /// <summary>
        /// Adds objects like the player and the enemies
        /// </summary>
        /// <param name="charMap"></param>
        void AddObjects(char[,] charMap)
        {
            charMap[1, 1] = 'P';
            charMap[charMap.GetLength(0) - 1, charMap.GetLength(1) - 1] = 'S';

            for (int x = 5; x < charMap.GetLength(0); x++)
            {
                for (int y = 5; y < charMap.GetLength(1); y++)
                {

                    if (charMap[x, y] == '.' && random.Next(0, 20) <= 1)
                    {
                        charMap[x, y] = 'M';
                    }
                }
            }

        }
        void AddCornerPieces(char[,] charMap)
        {
            for (int x = 0; x < charMap.GetLength(0); x += 2)
            {
                for (int y = 0; y < charMap.GetLength(1); y += 2)
                {
                    if ((x + 1 < charMap.GetLength(0) && charMap[x + 1, y] == '#')
                     || (y + 1 < charMap.GetLength(1) && charMap[x, y + 1] == '#')
                     || (x - 1 > 0 && charMap[x - 1, y] == '#')
                     || (y - 1 > 0 && charMap[x, y - 1] == '#'))
                    {
                        charMap[x, y] = '#';
                    }
                }
            }
        }
        Cell GetRandomCellAround(Cell currentCell)
        {
            //Get all the unvisitedCells
            List<Cell> unVisitedCells = currentCell.cellsAround.ToList().FindAll(x => x != null && !x.visited);
            if(unVisitedCells.Count != 0)
            {
                return unVisitedCells[random.Next(0, unVisitedCells.Count)];

            }

            return null;

        }
        string CharArrToString(char[,] charMap)
        {
            string map = "";
            for (int x = 0; x < charMap.GetLength(0) + 1; x++)
            {
                for (int y = 0; y < charMap.GetLength(1) + 1; y++)
                {
                    if (x == 0)
                    {
                        map += '#';
                        continue;
                    }
                    else if (x == charMap.GetLength(0))
                    {

                        map += '#';
                        continue;
                    }
                    else if (y == 0)
                    {
                        map += '#';
                        continue;
                    }
                    else if (y == charMap.GetLength(1))
                    {
                        map += '#';
                        continue;

                    }
                    if (charMap[x, y] == '#')
                    {
                        map += "#";
                    }
                    else
                    {
                        map += charMap[x, y];
                    }
                }
                map += "\n";
            }

            map = map.Remove(map.Length - 1);

            return map;
        }
        (int x, int y) GetXYFromDirection(int direction)
        {
            switch (direction)
            {
                case 0:
                    return (-1, 0);
                case 1:
                    return (0, 1);
                case 2:
                    return (1, 0);
                case 3:
                    return (0, -1);
                default:
                    return (0, 0);
            }
        }
        char[,] CellsToCharArr(Cell[,] cells)
        {
            char[,] charMap = new char[cells.GetLength(0) * 2, cells.GetLength(1) * 2];

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {

                    charMap[x * 2 + 1, y * 2 + 1] = '.';


                    if (x + 1 < cells.GetLength(0) && HasWallBetween(cells[x, y], cells[x + 1, y]))//Horizontal walls
                    {
                        charMap[2 * (x + 1), 2 * y + 1] = '#';
                    }
                    if (y + 1 < cells.GetLength(1) && HasWallBetween(cells[x, y], cells[x, y + 1]))//Vertical Walls
                    {
                        charMap[2 * x + 1, 2 * (y + 1)] = '#';
                    }



                }
            }

            return charMap;
        }
        void RemoveWallBetweenCells(Cell a, Cell b)
        {
            int direction = -1;
            for (int i = 0; i < a.cellsAround.Length; i++)
            {
                if(a.cellsAround[i] != null && a.cellsAround[i].x == b.x && a.cellsAround[i].y == b.y)
                {
                    direction = i;
                    break;
                }
            }
            if(direction != -1)
            {
                a.wallsAround[direction] = false;

                if(direction < 2)
                {
                    direction += 2;
                }
                else
                {
                    direction -= 2;
                }

                b.wallsAround[direction] = false;

            }

        }
        bool HasWallBetween(Cell a, Cell b)
        {
            for (int i = 0; i < a.cellsAround.Length; i++)
            {
                if (a.cellsAround[i] != null && a.cellsAround[i].x == b.x && a.cellsAround[i].y == b.y)
                {
                    return a.wallsAround[i];
                }
            }

            return true;

        }
        void InitCells(Cell[,] cells)
        {
            //Init the cells
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = new Cell(x, y);
                }
            }

            //Setting up the cell connections
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    for (int i = 0; i < cells[x, y].cellsAround.Length; i++)
                    {
                        (int x, int y) direction = GetXYFromDirection(i);
                        (int x, int y) pos = (x + direction.x, y + direction.y);

                        //If cells out of bounds return
                        if (pos.x < 0 || pos.x >= cells.GetLength(0)
                            || pos.y < 0 || pos.y >= cells.GetLength(1))
                        {
                            cells[x, y].cellsAround[i] = null;
                            cells[x, y].wallsAround[i] = true;
                            continue;
                        }
                        Cell otherCell = cells[pos.x, pos.y];
                        cells[x, y].cellsAround[i] = otherCell;
                        cells[x, y].wallsAround[i] = true;
                    }
                }
            }
        }
        void AddEmptyZones(Cell[,] cells)
        {
            for (int i = 0; i < 8; i++)
            {
                int sizeX = random.Next(2, 5);
                int sizeY = random.Next(2, 5);
                int posX = random.Next(0, cells.GetLength(0) - sizeX);
                int posY = random.Next(0, cells.GetLength(1) - sizeY);

                for (int x = posX; x < posX + sizeX; x++)
                {
                    for (int y = posY; y < sizeY + posY; y++)
                    {
                        for (int j = 0; j < cells[x, y].wallsAround.Length; j++)
                        {
                            int rand = random.Next(0, 3);
                            cells[x, y].wallsAround[j] = rand > 1 ? true : false;
                            //cells[x, y].visited = true;
                        }
                    }
                }
            }
        }
        void BackTrack(List<Cell> stack, ref Cell currentCell)
        {
            stack.Reverse();

            foreach (Cell cell in stack)
            {
                Cell newCell = GetRandomCellAround(cell);

                if (newCell != null)
                {
                    currentCell = cell;
                    break;
                }
            }
            stack.Reverse();
        }
        /// <summary>
        /// Checks to see if all cells have been visited
        /// </summary>
        /// <param name="cells"></param>
        /// <returns>True if all have been visited. False if any haven't</returns>
        bool AllVisited(Cell[,] cells)
        {
            bool allVisited = true;
            //Checking to see if there are any cells that need to be visited
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (!cells[x, y].visited)
                    {
                        allVisited = false;
                        break;
                    }
                }
            }

            return allVisited;
        }
    }

    /// <summary>
    /// Is a cell. Can either be a wall or an empty space
    /// </summary>
    class Cell
    {
        /// <summary>
        /// If true then is a wall <br></br>
        /// Directions: [0] North; [1] East; [2] South; [3] West;
        /// </summary>
        public Cell[] cellsAround;
        public bool[] wallsAround;
        public bool visited;
        public int x;
        public int y;
        public Cell(int x, int y)
        {
            cellsAround = new Cell[4];
            wallsAround = new bool[4];

            this.x = x;
            this.y = y;

            visited = false;

        }
    }

    class CellConnection
    {
        public Cell otherCell;
        /// <summary>
        /// Is there a wall in between the cells?
        /// </summary>
        public bool hasWall;
    }
}
