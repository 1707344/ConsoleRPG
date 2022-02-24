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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Offsets the cooldown by a given amount.</param>
        /// <returns></returns>
        public bool IsCooldownDone(float offset)
        {
            return MiniDC.time.GetTotalTime() - startTime >= cooldownTime + offset;
        }

        public float GetLength()
        {
            return cooldownTime;
        }
    }
}
