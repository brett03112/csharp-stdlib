using System;
using Xunit;

namespace csharp_stdlib.Tests
{
    public class StdStatsTests
    {
        private readonly double[] testDoubles = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        private readonly int[] testInts = { 1, 2, 3, 4, 5 };

        [Fact]
        public void Sum_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(15.0, StdStats.Sum(testDoubles));
            Assert.Equal(15.0, StdStats.Sum(testDoubles, 0, 5));
            Assert.Equal(9.0, StdStats.Sum(testDoubles, 1, 4));
        }

        [Fact]
        public void Sum_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(15, StdStats.Sum(testInts));
            Assert.Equal(15, StdStats.Sum(testInts, 0, 5));
            Assert.Equal(9, StdStats.Sum(testInts, 1, 4));
        }

        [Fact]
        public void Max_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(5.0, StdStats.Max(testDoubles));
        }

        [Fact]
        public void Max_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(4.0, StdStats.Max(testDoubles, 1, 4));
        }

        [Fact]
        public void Max_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(5, StdStats.Max(testInts));
        }

        [Fact]
        public void Min_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(1.0, StdStats.Min(testDoubles));
        }

        [Fact]
        public void Min_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(2.0, StdStats.Min(testDoubles, 1, 4));
        }

        [Fact]
        public void Min_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(1, StdStats.Min(testInts));
        }

        [Fact]
        public void Mean_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(3.0, StdStats.Mean(testDoubles));
        }

        [Fact]
        public void Mean_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(3.0, StdStats.Mean(testDoubles, 1, 4));
        }

        [Fact]
        public void Mean_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(3.0, StdStats.Mean(testInts));
        }

        [Fact]
        public void Var_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(2.5, StdStats.Var(testDoubles));
        }

        [Fact]
        public void Var_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(1.0, StdStats.Var(testDoubles, 1, 4));
        }

        [Fact]
        public void Var_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(2.5, StdStats.Var(testInts));
        }

        [Fact]
        public void Varp_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(2.0, StdStats.Varp(testDoubles));
        }

        [Fact]
        public void Varp_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(0.6666666666666666, StdStats.Varp(testDoubles, 1, 4), 15);
        }

        [Fact]
        public void Stddev_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(Math.Sqrt(2.5), StdStats.Stddev(testDoubles));
        }

        [Fact]
        public void Stddev_DoubleSubarray_ReturnsCorrectValue()
        {
            Assert.Equal(1.0, StdStats.Stddev(testDoubles, 1, 4));
        }

        [Fact]
        public void Stddev_IntArray_ReturnsCorrectValue()
        {
            Assert.Equal(Math.Sqrt(2.5), StdStats.Stddev(testInts));
        }

        [Fact]
        public void Stddevp_DoubleArray_ReturnsCorrectValue()
        {
            Assert.Equal(Math.Sqrt(2.0), StdStats.Stddevp(testDoubles));
        }

        [Fact]
        public void Max_EmptyArray_ReturnsNaN()
        {
            Assert.Equal(double.NaN, StdStats.Max<double>(new double[0]));
        }

        [Fact]
        public void Min_EmptyArray_ReturnsNaN()
        {
            Assert.Equal(double.NaN, StdStats.Min<double>(new double[0]));
        }

        [Fact]
        public void Mean_EmptyArray_ReturnsNaN()
        {
            Assert.Equal(double.NaN, StdStats.Mean<double>(new double[0]));
        }

        [Fact]
        public void Var_EmptyArray_ReturnsNaN()
        {
            Assert.Equal(double.NaN, StdStats.Var<double>(new double[0]));
        }

        [Fact]
        public void Stddev_EmptyArray_ReturnsNaN()
        {
            Assert.Equal(double.NaN, StdStats.Stddev<double>(new double[0]));
        }

        [Fact]
        public void Max_NullArray_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StdStats.Max<double>(null));
        }

        [Fact]
        public void Min_NullArray_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StdStats.Min<double>(null));
        }

        [Fact]
        public void Mean_NullArray_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StdStats.Mean<double>(null));
        }

        [Fact]
        public void Var_NullArray_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StdStats.Var<double>(null));
        }

        [Fact]
        public void Stddev_NullArray_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StdStats.Stddev<double>(null));
        }

        [Fact]
        public void Max_InvalidSubarray_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Max(testDoubles, -1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Max(testDoubles, 2, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Max(testDoubles, 3, 2));
        }

        [Fact]
        public void Min_InvalidSubarray_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Min(testDoubles, -1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Min(testDoubles, 2, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Min(testDoubles, 3, 2));
        }

        [Fact]
        public void Mean_InvalidSubarray_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Mean(testDoubles, -1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Mean(testDoubles, 2, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Mean(testDoubles, 3, 2));
        }

        [Fact]
        public void Var_InvalidSubarray_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Var(testDoubles, -1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Var(testDoubles, 2, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Var(testDoubles, 3, 2));
        }

        [Fact]
        public void Stddev_InvalidSubarray_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Stddev(testDoubles, -1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Stddev(testDoubles, 2, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => StdStats.Stddev(testDoubles, 3, 2));
        }

        [Fact]
        public void Max_WithNaN_ReturnsNaN()
        {
            double[] withNaN = { 1.0, double.NaN, 3.0 };
            Assert.Equal(double.NaN, StdStats.Max(withNaN));
        }

        [Fact]
        public void Min_WithNaN_ReturnsNaN()
        {
            double[] withNaN = { 1.0, double.NaN, 3.0 };
            Assert.Equal(double.NaN, StdStats.Min(withNaN));
        }
    }
}
