using System;
using System.Linq;
using Xunit;

namespace csharp_stdlib
{
    public class StdRandomTest
    {
        [Fact]
        public void TestSetSeed()
        {
            StdRandom.SetSeed(42);
            double first = StdRandom.Uniform();
            StdRandom.SetSeed(42);
            double second = StdRandom.Uniform();
            Assert.Equal(first, second);
        }

        [Fact]
        public void TestUniform()
        {
            double value = StdRandom.Uniform();
            Assert.InRange(value, 0.0, 1.0);
        }

        [Fact]
        public void TestUniformInt()
        {
            int value = StdRandom.Uniform(100);
            Assert.InRange(value, 0, 99);
        }

        [Fact]
        public void TestUniformIntInvalid()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Uniform(-1));
        }

        [Fact]
        public void TestUniformDoubleRange()
        {
            double value = StdRandom.Uniform(1.5, 2.5);
            Assert.InRange(value, 1.5, 2.5);
        }

        [Fact]
        public void TestUniformDoubleInvalidRange()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Uniform(2.5, 1.5));
        }

        [Fact]
        public void TestBernoulli()
        {
            int trueCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (StdRandom.Bernoulli(0.3))
                    trueCount++;
            }
            Assert.InRange(trueCount, 200, 400);
        }

        [Fact]
        public void TestBernoulliInvalidProbability()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Bernoulli(-0.1));
            Assert.Throws<ArgumentException>(() => StdRandom.Bernoulli(1.1));
        }

        [Fact]
        public void TestGaussian()
        {
            double sum = 0;
            int n = 10000;
            for (int i = 0; i < n; i++)
            {
                sum += StdRandom.Gaussian();
            }
            double mean = sum / n;
            Assert.InRange(mean, -0.1, 0.1);
        }

        [Fact]
        public void TestGaussianWithParams()
        {
            double mu = 5.0;
            double sigma = 2.0;
            double value = StdRandom.Gaussian(mu, sigma);
            Assert.True(value > mu - 5 * sigma && value < mu + 5 * sigma);
        }

        [Fact]
        public void TestGeometric()
        {
            int value = StdRandom.Geometric(0.2);
            Assert.True(value >= 1);
        }

        [Fact]
        public void TestGeometricInvalidProbability()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Geometric(-0.1));
            Assert.Throws<ArgumentException>(() => StdRandom.Geometric(1.1));
        }

        [Fact]
        public void TestPoisson()
        {
            int value = StdRandom.Poisson(5.0);
            Assert.True(value >= 0);
        }

        [Fact]
        public void TestPoissonInvalidLambda()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Poisson(-1.0));
        }

        [Fact]
        public void TestPareto()
        {
            double value = StdRandom.Pareto(2.0);
            Assert.True(value >= 1.0);
        }

        [Fact]
        public void TestParetoInvalidAlpha()
        {
            Assert.Throws<ArgumentException>(() => StdRandom.Pareto(-1.0));
        }

        [Fact]
        public void TestCauchy()
        {
            double value = StdRandom.Cauchy();
            Assert.True(value > double.MinValue && value < double.MaxValue);
        }

        [Fact]
        public void TestShuffle()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            int[] original = array.ToArray();
            StdRandom.Shuffle(array);
            Assert.Equal(original.Length, array.Length);
            Assert.True(array.All(original.Contains));
            Assert.False(array.SequenceEqual(original));
        }

        [Fact]
        public void TestShuffleNull()
        {
            int[] array = null;
            Assert.Throws<ArgumentNullException>(() => StdRandom.Shuffle(array));
        }

        [Fact]
        public void TestThreadSafety()
        {
            const int numThreads = 10;
            const int iterations = 10000;
            double[] results = new double[numThreads * iterations];

            System.Threading.Thread[] threads = new System.Threading.Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                int threadIndex = i;
                threads[i] = new System.Threading.Thread(() =>
                {
                    for (int j = 0; j < iterations; j++)
                    {
                        results[threadIndex * iterations + j] = StdRandom.Uniform();
                    }
                });
            }

            foreach (var thread in threads) thread.Start();
            foreach (var thread in threads) thread.Join();

            // Verify all values are within expected range
            foreach (var value in results)
            {
                Assert.InRange(value, 0.0, 1.0);
            }
        }
    }
}
