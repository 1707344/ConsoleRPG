using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    public class Map
    {
        public List<BaseObject> objects;
        List<Renderer> renderers;
        List<Position> positions;
        public List<Func<bool>> addingObjecsFuncs;
        public int height;
        public int width;

        public Map(int sizeX, int sizeY)
        {
            addingObjecsFuncs = new List<Func<bool>>();
            objects = new List<BaseObject>();
            renderers = new List<Renderer>();
            positions = new List<Position>();
            height = sizeY;
            width = sizeX;
            CreateBackground(sizeX, sizeY);
        }

        void CreateBackground(int sizeX, int sizeY)
        {
            for(int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    objects.Add(new EmptySpace(this, x, y));
                }
            }

            /*for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Console.SetCursorPosition(x, y);
                    //Console.Write("░░");
                }
            }*/
        }
        /// <summary>
        /// Updates every base
        /// </summary>
        public void UpdateObjects()
        {
            foreach(BaseObject baseObject in objects)
            {
                baseObject.Update();
            }

            List<BaseObject> baseObjects = objects.FindAll(x => x.destroy);
            for(int i = 0; i < baseObjects.Count;i++)
            {
                baseObjects[i].Destroy();
            }

            for(int i = 0; i < addingObjecsFuncs.Count; i++)
            {
                addingObjecsFuncs[i]();
            }
            addingObjecsFuncs = new List<Func<bool>>();
        }

        public void Display()
        {
            UpdateObjects();

            foreach(Position position in positions)
            {
                
                List<Position> matchingPositions = GetObjectsAtPosition(position.x, position.y);

                if(matchingPositions.Count == 3)
                {

                }
                if (position.obj.GetComponent<Renderer>() != null 
                    && matchingPositions.Count <= 1 
                    || !matchingPositions.Exists(x => x.obj.GetComponent<Renderer>() != null 
                    && x.obj != position.obj && x.obj.GetComponent<Renderer>().layer > position.obj.GetComponent<Renderer>().layer))
                {
                    Console.SetCursorPosition(position.x * 2, position.y);
                    position.obj.GetComponent<Renderer>().Display();
                }
            }
        }

        public void AddComponent(Component component)
        {
            switch (component.GetType().Name)
            {
                case "Renderer":
                    renderers.Add((Renderer) component);
                    break;
                case "Position":
                    positions.Add((Position) component);
                    break;
            }
        }
        public void RemoveComponent(Component component)
        {
            switch (component.GetType().Name)
            {
                case "Renderer":
                    renderers.Remove((Renderer)component);
                    break;
                case "Position":
                    positions.Remove((Position)component);
                    break;
                    
            }
        }
        public List<Position> GetObjectsAtPosition(int posX, int posY)
        {
            return positions.FindAll(x => x.x == posX && x.y == posY);
        }
        

        //For testing
        public void ResetBackgroundColors()
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.backgroundColor = new Color(Color.Colors.Black);
            }
        }
    }
}
