using System.Diagnostics;

namespace ConsoleRPG
{
    public class Time
    {
        float deltaTime;
        float frameStartTime;
        float frameEndTime;

        Stopwatch time;

        public Time()
        {
            time = new Stopwatch();
            time.Start();
        }

        public long GetTotalTime()
        {
            return time.ElapsedMilliseconds;
        }

        public void SetDeltaTime()
        {
            deltaTime = frameEndTime - frameStartTime;
        }

        public float GetDeltaTime()
        {
            return deltaTime;
        }

        public void SetFrameStartTime()
        {
            frameStartTime = GetTotalTime();
        }
        public void SetFrameEndTime()
        {
            frameEndTime = GetTotalTime();
        }
    }
}
