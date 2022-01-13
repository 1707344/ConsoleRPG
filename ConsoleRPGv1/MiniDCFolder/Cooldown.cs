using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG
{
    /// <summary>
    /// Is a cooldown. Has a start and a check to see if the time is up.
    /// To reset just StartCooldown again.
    /// </summary>
    public class Cooldown
    {
        float startTime;
        float cooldownTime;

        public Cooldown(float time)
        {
            cooldownTime = time;
        }

        public void StartCooldown()
        {
            startTime = MiniDC.time.GetTotalTime();
        }

        public bool IsCooldownDone()
        {
            return MiniDC.time.GetTotalTime() - startTime >= cooldownTime;
        }
    }
}
