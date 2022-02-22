using System.Collections.Generic;
using System.Linq;

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
            map.newObjects.Add(this);
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
            foreach (Component component in components)
            {
                component.Update();
            }
        }

        /// <summary>
        /// Happens after Update
        /// </summary>
        public virtual void LateUpdate()
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
        ///     obj.GetComponents&lt;Renderer&gt;(); 
        ///     
        ///     will return the renderer components on obj as a list
        /// </summary>
        public List<T> GetComponents<T>() where T : Component
        {
            List<Component> component = components.FindAll(x => x.GetType().Name == typeof(T).Name);
            try
            {
                return component.Cast<T>().ToList();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Returns component T. <br></br>
        /// EX:
        ///     obj.GetComponent&lt;Renderer&gt;(); 
        ///     
        ///     will return the renderer component on obj
        /// </summary>
        public T GetComponent<T>() where T : Component
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
            map.AddComponentToNewComponents(component);
        }
    }
}
