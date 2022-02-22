using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleRPG
{
    /// <summary>
    /// The InputHandler handles input. You give it a KeyListener via AddListener and it call func whenever key is pressed. 
    /// </summary>
    public static class InputHandler
    {
        static List<KeyListener> listeners = new List<KeyListener>();
        static List<KeyListener> newListeners = new List<KeyListener>();
        static bool clearListeners = false;
        public static Thread thread;//The thread InputHandler is running on


        public static void Update()
        {
            
            //Add newListeners


            //Update Listeners
            ConsoleKeyInfo key = Console.ReadKey(true);
            //Clear listeners if clearListners is true
            if (clearListeners)
            {
                listeners.Clear();
                clearListeners = false;
            }


            foreach (KeyListener keyListener in newListeners)
            {
                listeners.Add(keyListener);
            }


            newListeners.Clear();

            foreach (KeyListener listener in listeners)
            {
                if (listener.key == key.Key)
                {
                    listener.func();
                }
            }
        }

        public static void AddListener(KeyListener listener)
        {
            newListeners.Add(listener);
        }

        public static void ClearListeners()
        {
            clearListeners = true;
        }
    }

    public class KeyListener
    {
        /// <summary>
        /// The function that will be called when key is pressed
        /// </summary>
        public Func<bool> func;
        public ConsoleKey key;
        public KeyListener(Func<bool> func, ConsoleKey key)
        {
            this.func = func;
            this.key = key;
        }
    }
}
