using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class MapGenerator
    {
        struct WallSection
        {
            public int x;
            public int y;
            public int length;
            public Movement.Direction direction;

            public WallSection(int x, int y, int length, Movement.Direction direction)
            {
                this.x = x;
                this.y = y;
                this.length = length;
                this.direction = direction;
            }
        }

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


        #region CellsToString
        /// <summary>
        /// Converts a cell array to a string
        /// </summary>
        /// <returns></returns>
        public Map CellsToMap(Cell[,] cells, int difficulty)
        {
            char[,] charMap = CellsToCharArr(cells);

            //Adding corner pieces
            AddCornerPieces(charMap);

            Map map = CharArrToMap(charMap);

            //Adding player and enemies and stuff
            AddObjects(map, difficulty);
            

            return map;
        }
        /// <summary>
        /// Adds objects like the player and the enemies
        /// </summary>
        /// <param name="charMap"></param>
        void AddObjects(Map map, int difficulty)
        {
            //charMap[1, 1] = 'P';
            new Stairs(map, map.width - 2, map.height - 2);

            //Getting all empty Spaces
            List<Position> emptySpaces = new List<Position>();

            for (int x = 5; x < map.width; x++)
            {
                for (int y = 5; y < map.height; y++)
                {
                    Position position4 = map.GetObjectsAtPosition(x, y)[0];
                    if (!map.GetObjectsAtPosition(x, y).Exists(x => x.obj.GetType() == typeof(Wall)))
                    {
                        emptySpaces.Add(position4);
                    }
                }
            }


            if(difficulty == -1)
            {
                return;
            }

            if (difficulty > 0)
            {
                AddSnipers(map, difficulty);
            }


            AddBasicEnemies(emptySpaces, map, difficulty + 5);
        }

        void AddSnipers(Map map, int numOfSnipers)
        {
            //Find All walls
            bool[,] walls = new bool[map.width, map.height];//if true is a wall, false if not



            //Set up walls array
            for (int x = 1; x < map.width - 1; x++)
            {
                for (int y = 1; y < map.height - 1; y++)
                {
                    if (map.GetObjectsAtPosition(x, y).Exists(x => x.obj.GetType() == typeof(Wall)))
                    {
                        walls[x, y] = true;
                    }
                    else
                    {
                        walls[x, y] = false;
                    }
                }
            }


            List<WallSection> wallSections = new List<WallSection>();
            SetWallSections(wallSections, walls, map);

            float minDistAwayFromPlayer = 8;

            wallSections.RemoveAll(section => MathF.Sqrt(MathF.Pow(0 - section.x, 2) + MathF.Pow(0 - section.y, 2)) < minDistAwayFromPlayer);
            wallSections.Sort((WallSection a, WallSection b) => b.length.CompareTo(a.length));

            for (int i = 0; i < numOfSnipers; i++)
            {
                WallSection wallSection = wallSections[0];
                (int x, int y) offset = (0, 0);
                (int x, int y) position = (wallSection.x, wallSection.y);

                switch (wallSection.direction)
                {
                    case Movement.Direction.North:
                        offset = (0, -1);
                        break;
                    case Movement.Direction.East:
                        offset = (1, 0);
                        break;
                    case Movement.Direction.South:
                        offset = (0, 1);
                        break;
                    case Movement.Direction.West:
                        offset = (-1, 0);
                        break;
                }
                (int x, int y) startOffset = wallSection.direction == Movement.Direction.North || wallSection.direction == Movement.Direction.South ? (-1, 0) : (0, 1);


                position = (position.x + startOffset.x, position.y + startOffset.y);

                for (int j = 0; j < wallSection.length; j++)
                {
                    if (position.x == 0 || position.x >= map.width
                        || position.y == 0 || position.y >= map.height)
                    {
                        continue;
                    }

                    Position wall = map.GetObjectsAtPosition(position.x, position.y).Find(x => x.obj.GetType() == typeof(Wall));
                    if (wall != null)
                    {
                        wall.obj.destroy = true;
                    }


                    position = (position.x + offset.x, position.y + offset.y);
                }

                (int x, int y) position1 = (wallSection.x + startOffset.x, wallSection.y + startOffset.y);
                (int x, int y) position2 = (position.x - offset.x, position.y - offset.y);
                Sniper sniper = new Sniper(map, position1.x, position1.y, new PatrolPoint[] { new PatrolPoint(map, position1.x, position1.y), new PatrolPoint(map, position2.x, position2.y) });

                wallSections.Remove(wallSection);
            }
            
        }
        void GetWallsAround(bool[] wallsAround, bool[,] walls, int x, int y)
        {
            try
            {
                wallsAround[0] = walls[x, y - 1];
            }
            catch
            {
                wallsAround[0] = false;
            }
            try
            {
                wallsAround[1] = walls[x + 1, y];
            }
            catch
            {
                wallsAround[1] = false;
            }
            try
            {
                wallsAround[2] = walls[x, y + 1];
            }
            catch
            {
                wallsAround[2] = false;
            }
            try
            {
                wallsAround[3] = walls[x - 1, y];
            }
            catch
            {
                wallsAround[3] = false;
            }

        }
        void SetWallSections(List<WallSection> wallSections, bool[,] walls, Map map)
        {
            List<(int x, int y)> wallsToIgnore = new List<(int x, int y)>();

            for (int x = 0; x < walls.GetLength(0); x++)
            {
                for (int y = 0; y < walls.GetLength(1); y++)
                {

                    


                    bool currentWall = walls[x, y];
                    bool[] wallsAround = new bool[4];
                    if (currentWall)
                    {
                        GetWallsAround(wallsAround, walls, x, y);
                        //If north and south are true or east and west are true then it is a middle piece
                        if ((wallsAround[0] && wallsAround[2]) || (wallsAround[1] && wallsAround[3])
                            || wallsToIgnore.Exists(wall => wall.x == x && wall.y == y))
                        {
                            continue;
                        }
                        else if ((wallsAround[0] && wallsAround[1]) ||
                            (wallsAround[1] && wallsAround[2]) ||
                            (wallsAround[2] && wallsAround[3]) ||
                            (wallsAround[3] && wallsAround[0])) // If two adjacent walls then it is a corner and both directions need to be checked
                        {

                            (int x, int y) position = (x, y);
                            (int x, int y)[] offset = new (int x, int y)[2] { (0, 0), (0, 0) };
                            int[] dir = new int[2] { -1, -1 };
                            for (int i = 0; i < wallsAround.Length; i++)
                            {
                                if (wallsAround[i])
                                {
                                    if (dir[0] == -1)
                                    {
                                        dir[0] = i;
                                    }
                                    else if (dir[1] == -1)
                                    {
                                        dir[1] = i;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            //If there are no walls around continue
                            if (dir[0] == -1 || dir[1] == -1)
                            {
                                continue;
                            }

                            for (int i = 0; i < dir.Length; i++)
                            {
                                switch (dir[i])
                                {
                                    case 0:
                                        offset[i] = (0, -1);
                                        break;
                                    case 1:
                                        offset[i] = (1, 0);
                                        break;
                                    case 2:
                                        offset[i] = (0, 1);
                                        break;
                                    case 3:
                                        offset[i] = (-1, 0);
                                        break;
                                }

                            }

                            for (int i = 0; i < dir.Length; i++)
                            {

                                position = (x, y);

                                wallsToIgnore.Add((position.x, position.y));
                                while (true)
                                {
                                    position = (offset[i].x + position.x, offset[i].y + position.y);
                                    bool nextWall;
                                    try
                                    {
                                        nextWall = walls[position.x, position.y];
                                    }
                                    catch
                                    {
                                        break;
                                    }

                                    if (!nextWall)
                                    {
                                        break;
                                    }

                                    wallsToIgnore.Add((position.x, position.y));
                                }

                                int length = MathF.Abs(position.x - x) > MathF.Abs(position.y - y) ? (int)MathF.Abs(position.x - x) : (int)MathF.Abs(position.y - y);
                                wallSections.Add(new WallSection(x, y, length, (Movement.Direction)dir[i]));
                            }

                        }
                        else
                        {
                            (int x, int y) position = (x, y);
                            (int x, int y) offset = (0, 0);
                            int dir = -1;
                            for (int i = 0; i < wallsAround.Length; i++)
                            {
                                if (wallsAround[i])
                                {
                                    dir = i;
                                    break;
                                }
                            }
                            //If there are no walls around continue
                            if (dir == -1)
                            {
                                continue;
                            }

                            switch (dir)
                            {
                                case 0:
                                    offset = (0, -1);
                                    break;
                                case 1:
                                    offset = (1, 0);
                                    break;
                                case 2:
                                    offset = (0, 1);
                                    break;
                                case 3:
                                    offset = (-1, 0);
                                    break;
                            }

                            while (true)
                            {
                                position = (offset.x + position.x, offset.y + position.y);
                                bool nextWall;
                                try
                                {
                                    nextWall = walls[position.x, position.y];
                                }
                                catch
                                {
                                    break;
                                }

                                if (!nextWall)
                                {
                                    break;
                                }

                                wallsToIgnore.Add((position.x, position.y));
                            }

                            int length = MathF.Abs(position.x - x) > MathF.Abs(position.y - y) ? (int)MathF.Abs(position.x - x) : (int)MathF.Abs(position.y - y);
                            wallSections.Add(new WallSection(x, y, length, (Movement.Direction)dir));

                        }
                    }
                }
            }
        }
        void AddBasicEnemies(List<Position> emptySpaces, Map map, int numOfEnemies)
        {


            for (int i = 0; i < numOfEnemies; i++)
            {
                if (emptySpaces.Count <= 1)
                {
                    break;
                }

                //Creating and enemy and their patrol points
                int randIndex = 0;
                PatrolPoint[] patrolPoints = new PatrolPoint[3];
                for (int j = 0; j < patrolPoints.Length; j++)
                {
                    randIndex = random.Next(0, emptySpaces.Count - 1);
                    patrolPoints[j] = new PatrolPoint(map, emptySpaces[randIndex].x, emptySpaces[randIndex].y);

                }

                randIndex = random.Next(0, emptySpaces.Count - 1);
                new Monster(map, emptySpaces[randIndex].x, emptySpaces[randIndex].y, patrolPoints);
                emptySpaces.RemoveAt(randIndex);
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
        char[,] CellsToCharArr(Cell[,] cells)
        {
            char[,] charMap = new char[cells.GetLength(0) * 2 + 1, cells.GetLength(1) * 2 + 1];

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {

                    charMap[x * 2 + 1, y * 2 + 1] = '.';


                    if (x + 1 < cells.GetLength(0) && HasWallBetween(cells[x, y], cells[x + 1, y]))//Horizontal walls
                    {
                        charMap[2 * (x + 1), 2 * y + 1] = '#';
                    }else if(!(x + 1 < cells.GetLength(0))){//Left Wall
                        charMap[0, 2 * y] = '#';
                        charMap[0, (2 * y) + 1] = '#';
                    }
                    if (y + 1 < cells.GetLength(1) && HasWallBetween(cells[x, y], cells[x, y + 1]))//Vertical Walls
                    {
                        charMap[2 * x + 1, 2 * (y + 1)] = '#';
                    }else if( !(y + 1 < cells.GetLength(1)))//Top Wall
                    {
                        charMap[2 * x, 0] = '#';
                        charMap[(2 * x) + 1, 0] = '#';
                    }

                    if(y * 2 >= (cells.GetLength(1) - 1) * 2)//Bottom Wall
                    {
                        charMap[2 * x, cells.GetLength(1) * 2] = '#';
                        charMap[(2 * x) + 1, cells.GetLength(1) * 2] = '#';
                    }

                    if (x * 2 >= (cells.GetLength(0) - 1) * 2)//Right wall
                    {
                        charMap[cells.GetLength(0) * 2, 2 * y] = '#';
                        charMap[cells.GetLength(0) * 2, (2 * y) + 1] = '#';
                    }
                }
            }

            return charMap;
        }

        Map CharArrToMap(char[,] charMap)
        {
            Map map = new Map(charMap.GetLength(0), charMap.GetLength(1));
            for (int x = 0; x < charMap.GetLength(0); x++)
            {
                for (int y = 0; y < charMap.GetLength(1); y++)
                {
                    switch (charMap[x, y])
                    {
                        case '#':
                            new Wall(map, x, y);
                            break;
                        case 'F':
                            new FreezingTrap(map, x, y);
                            break;
                        case 'M':
                            //new Monster(map, x, y);
                            EndScreen.startingNumMonsters++;
                            break;
                        case 'S':
                            new Stairs(map, x, y);
                            break;
                        default:
                            //objectInsideCell = null;
                            break;
                    }

                }
            }

            return map;
        }

        #endregion

        #region GenerateMap Helpers
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
        (int x, int y) GetXYFromDirection(int direction)
        {
            switch (direction)
            {
                case 0:
                    return (0, -1);
                case 1:
                    return (1, 0);
                case 2:
                    return (0, 1);
                case 3:
                    return (-1, 0);
                default:
                    return (0, 0);
            }
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
            for (int i = 0; i < 10; i++)
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
        #endregion
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
}
