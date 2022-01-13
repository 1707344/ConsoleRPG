using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class PathFinder: Component
    {
        int furthestDistance;

        public PathFinder(BaseObject obj, int distance): base(obj)
        {
            furthestDistance = distance;
        }

        public Movement.Direction GetNextMove(Position goal)
        {
            List<Node> queue = new List<Node>();
            List<Node> visitedNodes = new List<Node>();
            Position position = obj.GetComponent<Position>();
            queue.Add(new Node(position.x, position.y, goal.x, goal.y, new List<Node>()));

            

            while (visitedNodes.Count < 150)
            {
                Node node = queue[0];
                

                //Check if node is goal
                if(node.x == goal.x && node.y == goal.y)
                {
                    break;
                }

                //Add new nodes to queue

                List<Node> previousNodes = node.previousNodes.ConvertAll(x => new Node(x.x, x.y, goal.x, goal.y, x.previousNodes));
                previousNodes.Add(node);

                AddToQueue(queue, visitedNodes, new Node(node.x + 1, node.y, goal.x, goal.y, previousNodes));
                AddToQueue(queue, visitedNodes, new Node(node.x - 1, node.y, goal.x, goal.y, previousNodes));
                AddToQueue(queue, visitedNodes, new Node(node.x, node.y + 1, goal.x, goal.y, previousNodes));
                AddToQueue(queue, visitedNodes, new Node(node.x, node.y - 1, goal.x, goal.y, previousNodes));


                //Remove used node
                visitedNodes.Add(node);
                queue.Remove(node);

                

                //Sort nodes by total cost

                queue.Sort(delegate (Node node1, Node node2)
                {
                    if(node1.totalCost == node2.totalCost)
                    {
                        return node1.previousNodes.Count.CompareTo(node2.previousNodes.Count);
                    }
                    return node1.totalCost.CompareTo(node2.totalCost);
                });

            }

            Node finalNode;

            if (queue[0].previousNodes.Count <= 1)
            {
                finalNode = new Node(goal.x, goal.y, 0, 0, new List<Node>());
            }
            else
            {
                finalNode = queue[0].previousNodes[1];
            }

            if(queue[0].previousNodes.Count > furthestDistance)
            {
                return Movement.Direction.None;
            }

            if(finalNode.x > position.x)
            {
                return Movement.Direction.East;
            }
            if(finalNode.x < position.x)
            {
                return Movement.Direction.West;
            }
            if(finalNode.y < position.y)
            {
                return Movement.Direction.North;
            }
            if(finalNode.y > position.y)
            {
                return Movement.Direction.South;
            }

            return Movement.Direction.None;
        }

        void AddToQueue(List<Node> queue, List<Node> visited, Node node)
        {
            List<Position> posAtPosition = obj.GetMap().GetObjectsAtPosition(node.x, node.y);

            if ( !posAtPosition.Exists(x => x.obj.GetType().Name == "Player") 
                && posAtPosition.Exists(x => x.obj.GetComponent<Collider>() != null && !x.obj.GetComponent<Collider>().isTrigger)
                || visited.Contains(node)
                || queue.Exists(x => x.x == node.x && x.y == node.y)
                ){
                return;
            }

            queue.Add(node);
        }
    }

    class Node 
    {
        public int x;
        public int y;
        public float heuristic;
        public float totalCost;
        public List<Node> previousNodes;


        public Node(int x, int y, int goalX, int goalY, List<Node> previousNodes)
        {
            this.x = x;
            this.y = y;
            heuristic = GetHeuristic(x, y, goalX, goalY);
            totalCost = GetTotalCost(heuristic, previousNodes);
            this.previousNodes = previousNodes;
        }

        float GetHeuristic(int x, int y, int goalX, int goalY)
        {
            return MathF.Sqrt(MathF.Pow(x - goalX, 2) + MathF.Pow(y - goalY, 2));
        }

        float GetTotalCost(float h, List<Node> nodes)
        {
            return h + nodes.Count;
        }
    }

}
