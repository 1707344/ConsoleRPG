using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    /// <summary>
    /// Gives a baseObject the ability to be frozen. 
    /// The baseObject will need to implement what the freezing does. 
    /// This component only allows the freezing to happen.
    /// </summary>
    class Freezable : Component
    {
        Renderer frozenVisual;
        public bool isFrozen;
        public Freezable(BaseObject obj): base(obj)
        {
            frozenVisual = new Renderer(obj, ' ', 10, new Color(219, 241, 253, 0.8f), true);
            
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
