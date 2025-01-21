using System;

namespace csharp_stdlib
{
    /// <summary>
    /// The <c>Stopwatch</c> data type is for measuring
    /// the time that elapses between the start and end of a
    /// programming task (wall-clock time).
    /// </summary>
    public class Stopwatch
    {
        private readonly long start;

        /// <summary>
        /// Initializes a new stopwatch.
        /// </summary>
        public Stopwatch()
        {
            start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Returns the elapsed time (in seconds) since the stopwatch was created.
        /// </summary>
        /// <returns>Elapsed time (in seconds) since the stopwatch was created.</returns>
        public double ElapsedTime()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return (now - start) / 1000.0;
        }
    }
}
