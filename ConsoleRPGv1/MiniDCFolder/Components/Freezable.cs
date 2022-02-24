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
        public float frozenAmount;

        float frozenThreshold;//Once frozen amount gets to frozenThreshold the obj will be frozen
        bool isFrozen;
        public Freezable(BaseObject obj, float frozenThreshold) : base(obj)
        {
            this.frozenThreshold = frozenThreshold;
            frozenVisual = new Renderer(obj, ' ', 10, new Color(219, 241, 253, 0.8f), true  );
            SetIsFrozen(false);
        }

        public override void Update()
        {
            base.Update();
            if(frozenAmount >= frozenThreshold)
            {
                SetIsFrozen(true);
                frozenVisual.color.a = 0.8f;
            }
            else
            {
                SetColor();
            }
        }

        void SetColor()
        {
            frozenVisual.color.a = GetFrozenPercent();
        }

        public float GetFrozenPercent()
        {
            float frozenPercent = frozenAmount / frozenThreshold;
            if(frozenPercent > 1)
            {
                frozenPercent = 1;
            }else if(frozenPercent < 0)
            {
                frozenPercent = 0;
            }
            return frozenPercent;
        }

        public void SetIsFrozen(bool value)
        {
            isFrozen = value;
        }
        public bool GetIsFrozen()
        {
            return isFrozen;
        }
    }
}
