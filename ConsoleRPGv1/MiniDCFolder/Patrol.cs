using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class Patrol
    {
        PatrolPoint[] patrolPoints;
        int currentPatrolPoint = 0;
        public Patrol(PatrolPoint[] patrolPoints)
        {
            this.patrolPoints = patrolPoints;
        }

        /// <summary>
        /// Increments the patrol point based off the mode
        /// </summary>
        public void NextPatrolPoint()
        {
            currentPatrolPoint++;
            if(currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
        }

        public Position GetCurrentPatrolPointPosition()
        {
            return patrolPoints[currentPatrolPoint].GetComponent<Position>();
        }

    }

    public class PatrolPoint: BaseObject
    {
        Position position;
        public PatrolPoint(Map map, int x, int y): base(map)
        {
            position = new Position(this, x, y);
        }
    }
}
