using System;
using System.Diagnostics;
using AuthGold.Contracts;

namespace AuthGold.Providers
{
    public class ElapsedTimeProvider : IElapsedTime
    {
        public Stopwatch Open()
        {
            return new Stopwatch();
        }

        public TimeSpan Close(Stopwatch stopwatch)
        {
            stopwatch.Stop();

            var elapsed_time = stopwatch.ElapsedMilliseconds;
            
            TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

            return t;
        }
    }
}