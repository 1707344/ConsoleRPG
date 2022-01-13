﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    /// <summary>
    /// Components will be attached to baseObjects. 
    /// Components give baseObjects functionallity that can be shared among baseObjects.
    /// For example the Renderer and Collider are both Components.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// The baseObject that the component is attached to.
        /// </summary>
        public BaseObject obj;
        public Component(BaseObject obj)
        {
            this.obj = obj;
            obj.AddComponent(this);

        }

    }
}