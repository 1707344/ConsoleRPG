using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    /// <summary>
    /// The InputHandler handles input. You give it a KeyListener via AddListener and it call func whenever key is pressed. 
    /// </summary>
    public static class InputHandler
    {
        static List<KeyListener> listeners = new List<KeyListener>();

        public static void Update()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
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
            listeners.Add(listener);
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
