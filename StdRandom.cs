using System;
using System.Threading;

namespace csharp_stdlib
{
    /// <summary>
    /// Standard random number generator class.
    /// Provides methods for generating random numbers from various distributions.
    /// </summary>
    public static class StdRandom
    {
        private static Random random = new Random();
        private static object syncLock = new object();

        /// <summary>
        /// Sets the seed of the random number generator.
        /// </summary>
        /// <param name="seed">The seed value</param>
        public static void SetSeed(int seed)
        {
            lock (syncLock)
            {
                random = new Random(seed);
            }
        }

        /// <summary>
        /// Returns a random real number uniformly in [0, 1).
        /// </summary>
        /// <returns>A random real number in [0, 1)</returns>
        public static double Uniform()
        {
            lock (syncLock)
            {
                return random.NextDouble();
            }
        }

        /// <summary>
        /// Returns a random integer uniformly in [0, n).
        /// </summary>
        /// <param name="n">Number of possible integers</param>
        /// <returns>A random integer in [0, n)</returns>
        public static int Uniform(int n)
        {
            if (n <= 0) throw new ArgumentException("Parameter n must be positive");
            lock (syncLock)
            {
                return random.Next(n);
            }
        }

        /// <summary>
        /// Returns a random real number uniformly in [a, b).
        /// </summary>
        /// <param name="a">The left endpoint</param>
        /// <param name="b">The right endpoint</param>
        /// <returns>A random real number in [a, b)</returns>
        public static double Uniform(double a, double b)
        {
            if (a >= b) throw new ArgumentException("Invalid range");
            return a + Uniform() * (b - a);
        }

        /// <summary>
        /// Returns a random boolean from a Bernoulli distribution with success probability p.
        /// </summary>
        /// <param name="p">The probability of returning true</param>
        /// <returns>A random boolean value</returns>
        public static bool Bernoulli(double p)
        {
            if (p < 0.0 || p > 1.0)
                throw new ArgumentException("Probability must be between 0.0 and 1.0");
            return Uniform() < p;
        }

        /// <summary>
        /// Returns a random real number from a standard Gaussian distribution.
        /// </summary>
        /// <returns>A random real number from Gaussian(0, 1)</returns>
        public static double Gaussian()
        {
            double r, x, y;
            do
            {
                x = Uniform(-1.0, 1.0);
                y = Uniform(-1.0, 1.0);
                r = x * x + y * y;
            } while (r >= 1 || r == 0);
            return x * Math.Sqrt(-2 * Math.Log(r) / r);
        }

        /// <summary>
        /// Returns a random real number from a Gaussian distribution with mean mu and stddev sigma.
        /// </summary>
        /// <param name="mu">The mean of the distribution</param>
        /// <param name="sigma">The standard deviation of the distribution</param>
        /// <returns>A random real number from Gaussian(mu, sigma)</returns>
        public static double Gaussian(double mu, double sigma)
        {
            return mu + sigma * Gaussian();
        }

        /// <summary>
        /// Returns a random integer from a geometric distribution with success probability p.
        /// </summary>
        /// <param name="p">The probability of success</param>
        /// <returns>A random integer from geometric(p)</returns>
        public static int Geometric(double p)
        {
            if (p < 0.0 || p > 1.0)
                throw new ArgumentException("Probability must be between 0.0 and 1.0");
            return (int)Math.Ceiling(Math.Log(Uniform()) / Math.Log(1.0 - p));
        }

        /// <summary>
        /// Returns a random integer from a Poisson distribution with mean lambda.
        /// </summary>
        /// <param name="lambda">The mean of the distribution</param>
        /// <returns>A random integer from Poisson(lambda)</returns>
        public static int Poisson(double lambda)
        {
            if (lambda <= 0.0)
                throw new ArgumentException("Lambda must be positive");
            int k = 0;
            double p = 1.0;
            double L = Math.Exp(-lambda);
            do
            {
                k++;
                p *= Uniform();
            } while (p >= L);
            return k - 1;
        }

        /// <summary>
        /// Returns a random real number from a Pareto distribution with shape parameter alpha.
        /// </summary>
        /// <param name="alpha">Shape parameter</param>
        /// <returns>A random real number from Pareto(alpha)</returns>
        public static double Pareto(double alpha)
        {
            if (alpha <= 0.0)
                throw new ArgumentException("Alpha must be positive");
            return Math.Pow(1 - Uniform(), -1.0 / alpha);
        }

        /// <summary>
        /// Returns a random real number from a Cauchy distribution.
        /// </summary>
        /// <returns>A random real number from Cauchy(0, 1)</returns>
        public static double Cauchy()
        {
            return Math.Tan(Math.PI * (Uniform() - 0.5));
        }

        /// <summary>
        /// Rearranges the elements of the specified array in uniformly random order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array</typeparam>
        /// <param name="a">The array to shuffle</param>
        public static void Shuffle<T>(T[] a)
        {
            if (a == null) throw new ArgumentNullException("Argument array is null");
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + Uniform(n - i);
                T temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }
    }
}
