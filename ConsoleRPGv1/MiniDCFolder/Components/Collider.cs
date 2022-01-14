using System;

namespace ConsoleRPG
{
    /// <summary>
    /// Gives BaseObjects the ability to collide. 
    /// In the constructor you give the Collider a function to call when it hits another collider. 
    /// </summary>
    public class Collider : Component
    {
        public bool active = true;
        public bool isTrigger;
        public Func<BaseObject, bool> OnCollisionFunc;
        public Collider(BaseObject obj, Func<BaseObject, bool> onCollision, bool isTrigger = false) : base(obj)
        {
            this.isTrigger = isTrigger;
            OnCollisionFunc = onCollision;
        }

        public void Collision(BaseObject baseObject)
        {
            if (active)
            {
                OnCollisionFunc(baseObject);
            }
        }
    }
}
