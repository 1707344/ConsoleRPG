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
        Collider collider;
        int freezingTime;
        Thread enableThread;
        public FreezingTrap(Map map, int x, int y): base(map)
        {
            freezingTime = 1500;
            position = new Position(this, x, y);
            renderer = new Renderer(this, 'X', 0, new Color(145, 240, 255), new Color(Color.Colors.Black));
            collider = new Collider(this, OnCollision, true);

            collider.active = false;
            enableThread = new Thread(new ThreadStart(delegate ()
            {
                renderer.backgroundColor = new Color(145, 240, 255);
                Thread.Sleep(3000);
                renderer.backgroundColor = new Color(Color.Colors.Black);
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
                    Color color = baseObject.GetComponent<Renderer>().textColor;
                    baseObject.GetComponent<Renderer>().textColor = renderer.textColor;

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
                    
                    baseObject.GetComponent<Renderer>().textColor = color;
                    baseObject.GetComponent<Movement>().stopMovement = false;
                }));
                thread.Start();
            }
            return true;
        }

        
    }
}
