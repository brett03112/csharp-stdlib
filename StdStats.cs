using System;
using System.Numerics;

namespace csharp_stdlib
{
    /// <summary>
    /// The <c>StdStats</c> class provides static methods for computing
    /// statistics such as min, max, mean, sample standard deviation, and
    /// sample variance.
    /// </summary>
    public static class StdStats
    {
        /// <summary>
        /// Returns the maximum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The maximum value in the array; <c>double.NaN</c> if the array is empty or contains <c>NaN</c>.</returns>
        public static double Max(double[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            
            double max = a[0];
            foreach (double val in a)
            {
                if (double.IsNaN(val)) return double.NaN;
                if (val > max) max = val;
            }
            return max;
        }

        /// <summary>
        /// Returns the maximum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The maximum value in the array.</returns>
        public static int Max(int[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) throw new ArgumentException("Array cannot be empty");
            
            int max = a[0];
            foreach (int val in a)
            {
                if (val > max) max = val;
            }
            return max;
        }

        /// <summary>
        /// Returns the maximum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The maximum value in the array; <c>double.NaN</c> if the array is empty or contains <c>NaN</c>.</returns>
        public static T Max<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            if (a.Length == 0) return (T)Convert.ChangeType(double.NaN, typeof(T));
            
            if (double.IsNaN(Convert.ToDouble(a[0]))) return a[0];
            
            T max = a[0];
            foreach (T val in a)
            {
                if (double.IsNaN(Convert.ToDouble(val))) return val;
                if (!double.IsNaN(Convert.ToDouble(max)) && val.CompareTo(max) > 0) max = val;
            }
            return max;
        }


        /// <summary>
        /// Returns the maximum value in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The maximum value in the specified subarray.</returns>
        public static T Max<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            if (hi - lo == 0) throw new ArgumentException("Subarray cannot be empty");

            T max = a[lo];
            for (int i = lo; i < hi; i++)
            {
                if (a[i].CompareTo(max) > 0) max = a[i];
            }
            return max;
        }

        /// <summary>
        /// Returns the minimum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The minimum value in the array; <c>double.NaN</c> if the array is empty or contains <c>NaN</c>.</returns>
        public static double Min(double[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            
            double min = a[0];
            foreach (double val in a)
            {
                if (double.IsNaN(val)) return double.NaN;
                if (val < min) min = val;
            }
            return min;
        }

        /// <summary>
        /// Returns the minimum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The minimum value in the array.</returns>
        public static int Min(int[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) throw new ArgumentException("Array cannot be empty");
            
            int min = a[0];
            foreach (int val in a)
            {
                if (val < min) min = val;
            }
            return min;
        }

        /// <summary>
        /// Returns the minimum value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The minimum value in the array; <c>double.NaN</c> if the array is empty or contains <c>NaN</c>.</returns>
        public static T Min<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            if (a.Length == 0) return (T)Convert.ChangeType(double.NaN, typeof(T));

            if (double.IsNaN(Convert.ToDouble(a[0]))) return a[0];
            
            T min = a[0];
            foreach (T val in a)
            {
                if (double.IsNaN(Convert.ToDouble(val))) return val;
                if (!double.IsNaN(Convert.ToDouble(min)) && val.CompareTo(min) < 0) min = val;
            }
            return min;
        }


        /// <summary>
        /// Returns the minimum value in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The minimum value in the specified subarray.</returns>
        public static T Min<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            if (hi - lo == 0) throw new ArgumentException("Subarray cannot be empty");

            T min = a[lo];
            for (int i = lo; i < hi; i++)
            {
                if (a[i].CompareTo(min) < 0) min = a[i];
            }
            return min;
        }

        /// <summary>
        /// Returns the average value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The average value in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Mean(double[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            return Sum(a) / a.Length;
        }

        /// <summary>
        /// Returns the average value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The average value in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Mean(int[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            return Sum(a) / a.Length;
        }

        /// <summary>
        /// Returns the average value in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The average value in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Mean<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            return Sum(a) / a.Length;
        }


        /// <summary>
        /// Returns the average value in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The average value in the specified subarray.</returns>
        public static double Mean<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            int length = hi - lo;
            if (length == 0) throw new ArgumentException("Subarray cannot be empty");
            return Convert.ToDouble(Sum(a, lo, hi)) / length;
        }

        /// <summary>
        /// Returns the sample variance in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample variance in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Var(double[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            double avg = Mean(a);
            double sum = 0.0;
            foreach (double val in a)
            {
                double diff = val - avg;
                sum += diff * diff;
            }
            return sum / (a.Length - 1);
        }

        /// <summary>
        /// Returns the sample variance in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample variance in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Var(int[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            double avg = Mean(a);
            double sum = 0.0;
            foreach (int val in a)
            {
                double diff = val - avg;
                sum += diff * diff;
            }
            return sum / (a.Length - 1);
        }

        /// <summary>
        /// Returns the sample variance in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample variance in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Var<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            double avg = Mean(a);
            double sum = 0.0;
            foreach (T val in a)
            {
                double diff = Convert.ToDouble(val) - avg;
                sum += diff * diff;
            }
            return sum / (a.Length - 1);
        }


        /// <summary>
        /// Returns the sample variance in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The sample variance in the specified subarray; <c>double.NaN</c> if the subarray is empty.</returns>
        public static double Var<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            int length = hi - lo;
            if (length == 0) throw new ArgumentException("Subarray cannot be empty");
            double avg = Mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                double diff = Convert.ToDouble(a[i]) - avg;
                sum += diff * diff;
            }
            return sum / (length - 1);
        }

        /// <summary>
        /// Returns the population variance in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The population variance in the specified array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Varp(double[] a)
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            double avg = Mean(a);
            double sum = 0.0;
            foreach (double val in a)
            {
                sum += (val - avg) * (val - avg);
            }
            return sum / a.Length;
        }

        /// <summary>
        /// Returns the population variance in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The population variance in the specified subarray; <c>double.NaN</c> if the subarray is empty.</returns>
        public static double Varp(double[] a, int lo, int hi)
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);

            int length = hi - lo;
            if (length == 0) return double.NaN;
            double avg = Mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / length;
        }

        /// <summary>
        /// Returns the sample standard deviation in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample standard deviation in the specified array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Stddev(double[] a)
        {
            ValidateNotNull(a);
            return Math.Sqrt(Var(a));
        }

        /// <summary>
        /// Returns the sample standard deviation in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample standard deviation in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Stddev(int[] a)
        {
            ValidateNotNull(a);
            return Math.Sqrt(Var(a));
        }

        /// <summary>
        /// Returns the sample standard deviation in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sample standard deviation in the array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Stddev<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            if (a.Length == 0) return double.NaN;
            return Math.Sqrt(Var(a));
        }


        /// <summary>
        /// Returns the sample standard deviation in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The sample standard deviation in the specified subarray; <c>double.NaN</c> if the subarray is empty.</returns>
        public static double Stddev<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            return Math.Sqrt(Var(a, lo, hi));
        }

        /// <summary>
        /// Returns the population standard deviation in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The population standard deviation in the specified array; <c>double.NaN</c> if the array is empty.</returns>
        public static double Stddevp(double[] a)
        {
            ValidateNotNull(a);
            return Math.Sqrt(Varp(a));
        }

        /// <summary>
        /// Returns the population standard deviation in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The population standard deviation in the specified subarray; <c>double.NaN</c> if the subarray is empty.</returns>
        public static double Stddevp(double[] a, int lo, int hi)
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            return Math.Sqrt(Varp(a, lo, hi));
        }

        /// <summary>
        /// Returns the sum of the values in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sum of the values in the array.</returns>
        public static double Sum(double[] a)
        {
            ValidateNotNull(a);
            double sum = 0.0;
            foreach (double val in a)
            {
                sum += val;
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of the values in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The sum of the values in the specified subarray.</returns>
        public static double Sum(double[] a, int lo, int hi)
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += a[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of the values in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sum of the values in the array.</returns>
        public static int Sum(int[] a)
        {
            ValidateNotNull(a);
            int sum = 0;
            foreach (int val in a)
            {
                sum += val;
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of the values in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The sum of the values in the specified subarray.</returns>
        public static int Sum(int[] a, int lo, int hi)
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            int sum = 0;
            for (int i = lo; i < hi; i++)
            {
                sum += a[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of the values in the specified array.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <returns>The sum of the values in the array.</returns>
        public static double Sum<T>(T[] a) where T : IComparable<T>
        {
            ValidateNotNull(a);
            double sum = 0.0;
            foreach (T val in a)
            {
                sum += Convert.ToDouble(val);
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of the values in the specified subarray.
        /// </summary>
        /// <param name="a">The array.</param>
        /// <param name="lo">The starting index of the subarray.</param>
        /// <param name="hi">The ending index of the subarray.</param>
        /// <returns>The sum of the values in the specified subarray.</returns>
        public static double Sum<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            ValidateNotNull(a);
            ValidateSubarrayIndices(lo, hi, a.Length);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += Convert.ToDouble(a[i]);
            }
            return sum;
        }

        private static void ValidateNotNull(object x)
        {
            if (x == null)
                throw new ArgumentNullException("argument is null");
        }

        private static void ValidateSubarrayIndices(int lo, int hi, int length)
        {
            if (lo < 0 || hi > length || lo > hi)
                throw new ArgumentOutOfRangeException("subarray indices out of bounds");
        }
    }
}
