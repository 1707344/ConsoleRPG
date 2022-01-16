using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    class ExplosionSource: BaseObject
    {
        List<ExplosionCell> explosionCells;
        List<ExplosionCell> newExplosionCells;
        Position position;
        Movement movement;

        public float transferSpeed;
        public float decaySpeed;
        public ExplosionSource(Map map, int x, int y) : base(map)
        {
            explosionCells = new List<ExplosionCell>();
            newExplosionCells = new List<ExplosionCell>();
            position = new Position(this, x, y);
            movement = new Movement(this);


            newExplosionCells.Add(new ExplosionCell(map, x, y, 0.5f, 0.05f, 50));
        }

        public override void Update()
        {
            

            if (Player.test)
            {
                Player.turnOffTest = true;
                newExplosionCells.Add(new ExplosionCell(GetMap(), position.x, position.y, 0.5f, 0.05f, 10));
            }
            else
            {
                //return;
            }

            foreach(ExplosionCell explosionCell in newExplosionCells)
            {
                explosionCells.Add(explosionCell);
            }

            newExplosionCells.Clear();

            foreach (ExplosionCell explosionCell in explosionCells)
            {
                explosionCell.energyLost = 0;
                ExplosionCell aNewExplosionCell = explosionCell.CalculateDirection((0, -1));
                ExplosionCell bNewExplosionCell = explosionCell.CalculateDirection((0, 1));
                ExplosionCell cNewExplosionCell = explosionCell.CalculateDirection((-1, 0));
                ExplosionCell dNewExplosionCell = explosionCell.CalculateDirection((1, 0));
                explosionCell.energy -= explosionCell.energyLost;

                if(aNewExplosionCell != null 
                    && !newExplosionCells.Exists(x => aNewExplosionCell.position.x == x.position.x && aNewExplosionCell.position.y == x.position.y)
                    && !explosionCells.Exists(x => aNewExplosionCell.position.x == x.position.x && aNewExplosionCell.position.y == x.position.y ))
                {
                    newExplosionCells.Add(aNewExplosionCell);
                }
                if (bNewExplosionCell != null
                    && !newExplosionCells.Exists(x => bNewExplosionCell.position.x == x.position.x && bNewExplosionCell.position.y == x.position.y )
                    && !explosionCells.Exists(x => bNewExplosionCell.position.x == x.position.x && bNewExplosionCell.position.y == x.position.y ))
                {
                    newExplosionCells.Add(bNewExplosionCell);
                }
                if (cNewExplosionCell != null
                    && !newExplosionCells.Exists(x => cNewExplosionCell.position.x == x.position.x && cNewExplosionCell.position.y == x.position.y)
                    && !explosionCells.Exists(x => cNewExplosionCell.position.x == x.position.x && cNewExplosionCell.position.y == x.position.y))
                {
                    newExplosionCells.Add(cNewExplosionCell);
                }
                if (dNewExplosionCell != null
                    && !newExplosionCells.Exists(x => dNewExplosionCell.position.x == x.position.x && dNewExplosionCell.position.y == x.position.y)
                    && !explosionCells.Exists(x => dNewExplosionCell.position.x == x.position.x && dNewExplosionCell.position.y == x.position.y))
                {
                    newExplosionCells.Add(dNewExplosionCell);
                }

            }


            List<ExplosionCell> destroyedExplosionCells = new List<ExplosionCell>();
            foreach (ExplosionCell explosionCell in explosionCells)
            {
                explosionCell.energy -= explosionCell.decaySpeed;
                if(explosionCell.energy <= 0)
                {
                    explosionCell.destroy = true;
                    destroyedExplosionCells.Add(explosionCell);
                }
            }

            foreach (ExplosionCell explosionCell in destroyedExplosionCells)
            {
                explosionCells.Remove(explosionCell);
            }

            foreach (ExplosionCell explosionCell in explosionCells)
            {
                explosionCell.renderer.color = explosionCell.GetColor();
            }

            if (explosionCells.Count == 0 && newExplosionCells.Count == 0)
            {
                destroy = true;
            }
        }

        /// <summary>
        /// Calculates for one direction
        /// </summary>
        
    }

    class ExplosionCell: BaseObject
    {
        public Renderer renderer;
        public Position position;

        public float transferSpeed;
        public float energy;
        public float decaySpeed;
        int numOfWallsAround;

        public float energyLost = 0;

        public ExplosionCell(Map map, int x, int y, float transferSpeed, float decaySpeed, float startEnergy=0) : base(map)
        {
            renderer = new Renderer(this, ' ', 5, new Color(Color.Colors.White), true);
            position = new Position(this, x, y);

            this.transferSpeed = transferSpeed;
            this.decaySpeed = decaySpeed;
            energy = startEnergy;

            bool north = GetMap().GetObjectsAtPosition(position.x, position.y - 1).Exists(x => x.obj.GetType() == typeof(Wall));
            bool south = GetMap().GetObjectsAtPosition(position.x, position.y + 1).Exists(x => x.obj.GetType() == typeof(Wall));
            bool west = GetMap().GetObjectsAtPosition(position.x - 1, position.y).Exists(x => x.obj.GetType() == typeof(Wall));
            bool east = GetMap().GetObjectsAtPosition(position.x + 1, position.y).Exists(x => x.obj.GetType() == typeof(Wall));

            if (north)
            {
                numOfWallsAround++;
            }if (south)
            {
                numOfWallsAround++;
            }if (west)
            {
                numOfWallsAround++;
            }if (east)
            {
                numOfWallsAround++;
            }

        }

        public Color GetColor()
        {
            float g = 100 + (energy / 10) * (255 - 100);
            float a = 0.4f + (energy / 5) * (0.99f);
            if(a >= 1)
            {
                a = 0.99f;
            }
            return new Color(255, (int)g, 5, a);
        }

        /// <summary>
        /// Calculates transfered energy for a direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>Returns a new Explosion cell</returns>
        public ExplosionCell CalculateDirection((int x, int y) direction)
        {
            float log = (float)Math.Log(energy, 10);
            float actualTransferSpeed = transferSpeed * ((float)Math.Log(energy, 10) * 100);

            if (!(energy - actualTransferSpeed * (4 - numOfWallsAround) >= 0) && numOfWallsAround != 4)
            {
                actualTransferSpeed = energy / (4 - numOfWallsAround);
            }else if(actualTransferSpeed < transferSpeed)
            {
                actualTransferSpeed = transferSpeed;
            }


            List<ExplosionCell> temp = GetMap().GetObjectsAtPosition(position.x + direction.x, position.y + direction.y).FindAll(x => x.obj.GetType() == typeof(ExplosionCell)).ConvertAll(x => (ExplosionCell)x.obj);
            ExplosionCell affectedCell = temp.Count == 0 ? null : temp[0];


            if (affectedCell == null)
            {
                if (!GetMap().GetObjectsAtPosition(position.x + direction.x, position.y + direction.y).Exists(x => x.obj.GetType() == typeof(Wall))
                    && (energy - (transferSpeed/2) * (4 - numOfWallsAround) >= 0)
)
                {                    //

                    energyLost += actualTransferSpeed / (4 - numOfWallsAround);
                    return new ExplosionCell(GetMap(), position.x + direction.x, position.y + direction.y, transferSpeed, decaySpeed, actualTransferSpeed / (4 - numOfWallsAround));
                }
            }
            else
            {
                if (affectedCell.energy < energy && energy - actualTransferSpeed * (4 - numOfWallsAround) >= 0)
                {
                    if (numOfWallsAround != 4)
                    {
                        affectedCell.energy += actualTransferSpeed / (4 - numOfWallsAround);
                        energyLost += actualTransferSpeed / (4 - numOfWallsAround);
                    }
                }
            }
            if(energy > 3)
            {
                int x = 1+1;
            }

            return null;
        }
    }
}
