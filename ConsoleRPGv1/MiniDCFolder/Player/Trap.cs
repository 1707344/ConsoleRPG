using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleRPG
{
    public class FreezingTrap: BaseObject
    {
        Position position;
        Renderer renderer;
        Renderer backgroundRenderer;
        Collider collider;
        int freezingTime;
        Thread enableThread;
        public FreezingTrap(Map map, int x, int y): base(map)
        {
            freezingTime = 1500;
            position = new Position(this, x, y);
            renderer = new Renderer(this, 'X', 2, new Color(145, 240, 255, 0.7f), false);
            backgroundRenderer = new Renderer(this, ' ', 5, new Color(145, 240, 255, 0.7f), true);
            collider = new Collider(this, OnCollision, true);

            backgroundRenderer.backgroundStretches = true;
            //TEST
            InputHandler.AddListener(new KeyListener(ChangeOpacityUp, ConsoleKey.A));
            InputHandler.AddListener(new KeyListener(ChangeOpacityDown, ConsoleKey.S));

            collider.active = false;
            enableThread = new Thread(new ThreadStart(delegate ()
            {
                //renderer.color = new Color(145, 240, 255);
                Thread.Sleep(3000);
                backgroundRenderer.isVisible = false;
                collider.active = true;
            }));
            
        }

        public override void Update()
        {
            if(collider.active == false && enableThread.ThreadState == ThreadState.Unstarted)
            {
                enableThread.Start();
            }
        }

        public bool OnCollision(BaseObject baseObject)
        {
            if(baseObject.GetType().Name == "Player" || baseObject.GetType().Name == "Monster")
            {
                
                baseObject.GetComponent<Movement>().stopMovement = true;
                Thread thread = new Thread(new ThreadStart(delegate ()
                {
                    Color color = baseObject.GetComponent<Renderer>().color;
                    baseObject.GetComponent<Renderer>().color = renderer.color;

                    collider.isTrigger = false;

                    if(baseObject.GetType().Name == "Player") 
                    { 
                        Thread.Sleep(freezingTime/2); 
                    } 
                    else
                    {
                        Thread.Sleep(freezingTime);
                    }

                    collider.isTrigger = true;
                    
                    baseObject.GetComponent<Renderer>().color = color;
                    baseObject.GetComponent<Movement>().stopMovement = false;
                }));
                thread.Start();
            }
            return true;
        }

        public bool ChangeOpacityUp()
        {
            renderer.color.a += 0.1f;
            return true;
        }
        public bool ChangeOpacityDown()
        {

            renderer.color.a -= 0.1f;
            return true;
        }
    }
}
