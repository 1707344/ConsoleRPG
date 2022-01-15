using System;
using System.Collections.Generic;

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

        public List<(int x, int y, Renderer r)> emptySpaceWithColor;
        public bool resetEmptySpace;

        bool testBool = false;

        public Map(int sizeX, int sizeY)
        {
            emptySpaceWithColor = new List<(int x, int y, Renderer r)>();
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
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    objects.Add(new EmptySpace(this, x, y));
                }
            }

        }
        /// <summary>
        /// Updates every base
        /// </summary>
        public void UpdateObjects()
        {
            foreach (BaseObject baseObject in objects)
            {
                baseObject.Update();
            }
            //Destroys all baseObjects that have destroy set to true
            List<BaseObject> baseObjects = objects.FindAll(x => x.destroy);
            for (int i = 0; i < baseObjects.Count; i++)
            {
                baseObjects[i].Destroy();
            }

            for (int i = 0; i < addingObjecsFuncs.Count; i++)
            {
                addingObjecsFuncs[i]();
            }
            addingObjecsFuncs = new List<Func<bool>>();
        }

        public void Display()
        {
            ConsoleHandler.ClearConsoleCharacters();

            if (!testBool)
            {
                testBool = true;
                Color backgroundColor1 = new Color(Color.Colors.Black);
                Color textColor = new Color(Color.Colors.White);
                //Console.Write("\x1b[48;2;" + backgroundColor1.r + ";" + backgroundColor1.g + ";" + backgroundColor1.b + "m");
                //Console.Write("\x1b[38;2;" + textColor.r + ";" + textColor.g + ";" + textColor.b + "m");
                for (int x = 0; x < width * 3; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        //Console.SetCursorPosition(x, y);
                        //Console.Write("░");
                    }
                }
            }

            UpdateObjects();
            /*bool test = true;
            if (test)
            {
                //Cleaning up empty space
                Color emptySpaceColor = new Color(Color.Colors.Black);
                Console.Write("\x1b[48;2;" + emptySpaceColor.r + ";" + emptySpaceColor.g + ";" + emptySpaceColor.b + "m");
                for (int x = 1; x < width * 2; x += 2)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (!emptySpaceWithColor.Exists(e => e.x == x && e.y == y))
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write(" ");
                        }
                        else if (emptySpaceWithColor.Exists(e => e.r.obj == null
                         || (e.x - 1 != e.r.obj.GetComponent<Position>().x || e.y != e.r.obj.GetComponent<Position>().y)
                         || e.r.isVisible == false))
                        {
                            emptySpaceWithColor.Remove(emptySpaceWithColor.Find(e => e.x == x && e.y == y));
                        }
                    }
                }

                foreach ((int x, int y, Renderer r) pos in emptySpaceWithColor)
                {
                    //Console.SetCursorPosition(pos.x, pos.y);
                    ////Console.Write(" ");
                }
            }*/


            foreach (Position position in positions)
            {

                //All of the backgrounds (in a position)
                List<Renderer> backgrounds = new List<Renderer>();
                //The top most icon. Will be the only icon in the position that is rendered
                //The backgrounds in front will affect its color
                Color newIconColor;

                List<Position> matchingPositions = GetObjectsAtPosition(position.x, position.y);
                List<Renderer> renderers = new List<Renderer>();

                List<Renderer> backgroundsInFrontOfIcon = new List<Renderer>();

                //Getting all renderers in position
                foreach (Position position1 in matchingPositions.FindAll(x => x.obj.GetComponent<Renderer>() != null))
                {
                    foreach (Renderer renderer in position1.obj.GetComponents<Renderer>())
                    {
                        renderers.Add(renderer);
                    }
                }


                //Finding top most icon
                Renderer highestLayerIcon = null;
                foreach (Renderer renderer in renderers.FindAll(x => !x.isBackground))
                {
                    if (highestLayerIcon == null)
                    {
                        highestLayerIcon = renderer;
                    }
                    else if (renderer.layer > highestLayerIcon.layer)
                    {
                        highestLayerIcon = renderer;
                    }
                }

                backgrounds = renderers.FindAll(x => x.isBackground);
                backgroundsInFrontOfIcon = backgrounds.FindAll(x => x.layer >= highestLayerIcon.layer);
                //Orders backgrounds in desending order so top layer to bottom layer
                backgroundsInFrontOfIcon.Sort((x, y) => y.layer.CompareTo(x.layer));

                //Mix the backgrounds in front of the icon(additive) then mix that color into the icons color(subtractive)

                Color colorInFrontOfIcon = null;
                foreach (Renderer renderer1 in backgroundsInFrontOfIcon)
                {
                    if (colorInFrontOfIcon == null)
                    {
                        colorInFrontOfIcon = renderer1.color;
                    }
                    else if (colorInFrontOfIcon.a >= 1)
                    {
                        break;
                    }
                    else
                    {
                        colorInFrontOfIcon = Color.ColorMixAdd(colorInFrontOfIcon, renderer1.color);
                    }
                }
                if (highestLayerIcon.color == null)
                {
                    highestLayerIcon.color = new Color(0, 0, 0, 0);
                }

                if (colorInFrontOfIcon == null)
                {
                    colorInFrontOfIcon = new Color(0, 0, 0, 0);
                }
                newIconColor = Color.ColorMixSub(highestLayerIcon.color, colorInFrontOfIcon);


                //Mixing all background colors in the position
                backgrounds.Sort((x, y) => y.layer.CompareTo(x.layer));
                Color backgroundColor = null;
                foreach (Renderer background in backgrounds)
                {
                    if (backgroundColor == null)
                    {
                        backgroundColor = background.color;
                    }
                    else if (backgroundColor.a >= 1)
                    {
                        break;
                    }
                    else
                    {
                        backgroundColor = Color.ColorMixAdd(backgroundColor, background.color);
                    }
                }
                Random random = new Random(DateTime.Now.Millisecond);

                backgroundColor = Color.ColorMixSub(new Color(Color.Colors.Black), backgroundColor);

                //newIconColor = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

                //Display background and icon to the screen
                if (backgrounds.Count > 0)
                {
                    backgrounds[0].Display(position.x * 2, position.y, backgroundColor);
                }
                highestLayerIcon.Display(position.x * 2, position.y, newIconColor);

            }
        }

        public void AddComponent(Component component)
        {
            switch (component.GetType().Name)
            {
                case "Renderer":
                    renderers.Add((Renderer)component);
                    break;
                case "Position":
                    positions.Add((Position)component);
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
                renderer.color = new Color(Color.Colors.Black);
            }
        }
    }
}
