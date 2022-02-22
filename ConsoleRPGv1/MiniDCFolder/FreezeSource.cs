using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class FreezeSource: BaseObject
    {
        List<FreezeCell> freezeCells;
        List<FreezeCell> newFreezeCells;



        public FreezeSource(Map map, (int x, int y) startingLocation, int spreadDist) : base(map)
        {
            freezeCells = new List<FreezeCell>();
            newFreezeCells = new List<FreezeCell>();
            newFreezeCells.Add(new FreezeCell(map, startingLocation.x, startingLocation.y, spreadDist));
        }

        public override void Update()
        {
            AddNewFreezeCells();

            UpdateFreezeSpread();

            UpdateFreezing(); 
        }

        void AddNewFreezeCells()
        {
            foreach(FreezeCell freezeCell in newFreezeCells)
            {
                //GetMap().newObjects.Add(freezeCell);
                freezeCells.Add(freezeCell);
            }

            newFreezeCells.Clear();
        }

        void UpdateFreezeSpread()
        {

            Map map = GetMap();
            foreach (FreezeCell freezeCell in freezeCells)
            {
                int x = freezeCell.GetComponent<Position>().x;
                int y = freezeCell.GetComponent<Position>().y;
                if (freezeCell.movesLeft <= 0)
                {
                    continue;
                }
                else
                {
                    freezeCell.movesLeft -= 1;
                }
                FreezeCell freezeCellN = new FreezeCell(map, x, y - 1, freezeCell.movesLeft);
                FreezeCell freezeCellE = new FreezeCell(map, x + 1, y, freezeCell.movesLeft);
                FreezeCell freezeCellS = new FreezeCell(map, x, y + 1, freezeCell.movesLeft);
                FreezeCell freezeCellW = new FreezeCell(map, x - 1, y, freezeCell.movesLeft);


                FreezeCell freezeCellNE = new FreezeCell(map, x + 1, y - 1, freezeCell.movesLeft);
                FreezeCell freezeCellSE = new FreezeCell(map, x + 1, y + 1, freezeCell.movesLeft);
                FreezeCell freezeCellSW = new FreezeCell(map, x - 1, y + 1, freezeCell.movesLeft);
                FreezeCell freezeCellNW = new FreezeCell(map, x - 1, y - 1, freezeCell.movesLeft);

                CreateNewFreezeCell(freezeCellN);
                CreateNewFreezeCell(freezeCellE);
                CreateNewFreezeCell(freezeCellS);
                CreateNewFreezeCell(freezeCellW);

                CreateNewFreezeCell(freezeCellNE);
                CreateNewFreezeCell(freezeCellSE);
                CreateNewFreezeCell(freezeCellSW);
                CreateNewFreezeCell(freezeCellNW);

            }
        }

        void CreateNewFreezeCell(FreezeCell freezeCell)
        {

            //Making sure is doesn't already exist or there is a wall at that position
            if(freezeCell != null
            && !newFreezeCells.Exists(x => freezeCell.position.x == x.position.x && freezeCell.position.y == x.position.y)
            && !freezeCells.Exists(x => freezeCell.position.x == x.position.x && freezeCell.position.y == x.position.y)
            && !freezeCell.GetMap().GetObjectsAtPosition(freezeCell.position.x, freezeCell.position.y).Exists(x => x.obj.GetType() == typeof(Wall)))
            {
                newFreezeCells.Add(freezeCell);
            }
            else
            {
                freezeCell.destroy = true;
            }
        }

        /// <summary>
        /// Looks for freezables under a freeze cell. Will freeze if there is one
        /// </summary>
        void UpdateFreezing()
        {
            foreach(FreezeCell freezeCell in freezeCells)
            {
                List<Freezable> freezables = GetMap().GetObjectsAtPosition(freezeCell.position.x, freezeCell.position.y)
                    .FindAll(x => x.obj.GetComponent<Freezable>() != null)
                    .ConvertAll(x => x.obj.GetComponent<Freezable>());
                foreach (Freezable freezable in freezables)
                {
                    freezable.SetIsFrozen(true);
                }
            }
        }

        void UpdateDecay()
        {
            foreach(FreezeCell freezeCell in freezeCells)
            {

            }
        }
    }

    class FreezeCell: BaseObject
    {
        public int movesLeft;
        public Renderer renderer;
        public Position position;
        public float timeLeft;//How long the freezing will last on the ground


        public FreezeCell(Map map, int x, int y, int movesLeft): base(map)
        {
            //timeLeft = placeTime;

            this.movesLeft = movesLeft;
            renderer = new Renderer(this, ' ', 11, new Color(210, 220, 255, 0.4f), true);
            position = new Position(this, x, y);
        }
    }
}
