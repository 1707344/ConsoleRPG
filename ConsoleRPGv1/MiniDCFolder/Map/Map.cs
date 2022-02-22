using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRPG
{
    public class Map
    {
        public List<BaseObject> objects;
        public List<BaseObject> newObjects;
        List<Renderer> renderers;
        List<Position> positions;

        List<Component> newComponents;


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
            newObjects = new List<BaseObject>();
            renderers = new List<Renderer>();
            positions = new List<Position>();
            newComponents = new List<Component>();
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
            ExplosionHandler.Update();

            foreach (BaseObject baseObject in objects)
            {
                baseObject.Update();
            }

            foreach (BaseObject newObject in newObjects)
            {
                objects.Add(newObject);
            }
            newObjects.Clear();

            if (Player.turnOffTest)
            {
                Player.test = false;
                Player.turnOffTest = false;
            }

            //Adding new components
            foreach(Component component in newComponents)
            {
                AddComponent(component);
            }
            newComponents.Clear();

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
            positions.Sort(delegate (Position position1, Position position2)
            {
                return position1.x.CompareTo(position1.x);
            });
            ConsoleHandler.ClearConsoleCharacters();



            UpdateObjects();



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
                        if (renderer.isVisible)
                        {
                            renderers.Add(renderer);
                        }
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


        public void AddComponentToNewComponents(Component component)
        {
            newComponents.Add(component);
        }

        void AddComponent(Component component)
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

        /// <summary>
        /// Adds the newObjects list into objects list
        /// </summary>
        public void ForceNewObjects()
        {
            foreach (BaseObject newObject in newObjects)
            {
                objects.Add(newObject);
            }
            newObjects.Clear();
        }

        /// <summary>
        /// Adds the new Components list into the components list
        /// </summary>
        public void ForceNewComponents()
        {
            //Adding new components
            foreach (Component component in newComponents)
            {
                AddComponent(component);
            }
            newComponents.Clear();

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
