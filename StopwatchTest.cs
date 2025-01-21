using System;
using Xunit;

namespace csharp_stdlib
{
    public class StopwatchTests
    {
        [Fact]
        public void TestElapsedTime()
        {
            Stopwatch timer = new Stopwatch();
            System.Threading.Thread.Sleep(100);
            double elapsedTime = timer.ElapsedTime();
            Assert.True(elapsedTime >= 0.09 && elapsedTime <= 0.2, $"Elapsed time was {elapsedTime}, which is not within the expected range.");
        }
    }
}
