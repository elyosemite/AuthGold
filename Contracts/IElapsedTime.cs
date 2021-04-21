using System;
using System.Diagnostics;

namespace AuthGold.Contracts
{
    public interface IElapsedTime
    {
        Stopwatch Open();
        TimeSpan Close(Stopwatch stopwatch);
    }
}