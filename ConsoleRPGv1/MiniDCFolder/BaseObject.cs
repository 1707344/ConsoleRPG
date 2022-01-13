using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    /// <summary>
    /// Anything that will want to be on the map or have components must inherit from this. 
    /// </summary>
    public abstract class BaseObject
    {
        List<Component> components;
        Map map;
        /// <summary>
        /// If true then the next Map Update cycle will destroy this object
        /// </summary>
        public bool destroy = false;


        public BaseObject(Map map)
        {
            this.map = map;
            components = new List<Component>();
        }

        /// <summary>
        /// Gets the map that the baseObject is located in
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return map;
        }

        public virtual void Update()
        {

        }

        public void Destroy()
        {
            components.ForEach(x => map.RemoveComponent(x));
            map.objects.Remove(this);
        }
        /// <summary>
        /// Returns component T. <br></br>
        /// EX:
        ///     obj.GetComponent&lt;Renderer&gt;(); 
        ///     
        ///     will return the renderer component on obj
        /// </summary>
        public T GetComponent<T>() where T: Component
        {
            Component component = components.Find(x => x.GetType().Name == typeof(T).Name);
            try
            {
                return (T)component;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Will add a component onto a baseObject
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            components.Add(component);
            map.AddComponent(component);
        }
    }
}
