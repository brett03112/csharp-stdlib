/******************************************************************************
 *  A library for reading in 1D and 2D arrays of integers, doubles,
 *  and booleans from standard input and printing them out to
 *  standard output.
 *
 *  Based on StdArrayIO.java from Princeton's StdLib
 *  Converted to C# by Cline
 ******************************************************************************/

using System;

namespace csharp_stdlib
{
    /// <summary>
    /// The StdArrayIO class provides static methods for reading
    /// in 1D and 2D arrays from standard input and printing out to
    /// standard output.
    /// </summary>
    public static class StdArrayIO
    {
        /// <summary>
        /// Reads a 1D array of doubles from standard input and returns it.
        /// </summary>
        /// <returns>The 1D array of doubles</returns>
        public static double[] ReadDouble1D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            if (!int.TryParse(input, out int n))
            {
                throw new FormatException("Invalid integer format");
            }
            
            double[] a = new double[n];
            for (int i = 0; i < n; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !double.TryParse(input, out a[i]))
                {
                    throw new FormatException($"Invalid double format at index {i}");
                }
            }
            return a;
        }

        /// <summary>
        /// Prints an array of doubles to standard output.
        /// </summary>
        /// <param name="a">The 1D array of doubles</param>
        public static void Print(double[] a)
        {
            int n = a.Length;
            Console.WriteLine(n);
            for (int i = 0; i < n; i++)
            {
                Console.Write("{0,9:F5} ", a[i]);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Reads a 2D array of doubles from standard input and returns it.
        /// </summary>
        /// <returns>The 2D array of doubles</returns>
        public static double[][] ReadDouble2D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            string[] dimensions = input.Split();
            if (dimensions.Length != 2)
            {
                throw new FormatException("Expected two dimensions");
            }
            
            if (!int.TryParse(dimensions[0], out int m) || !int.TryParse(dimensions[1], out int n))
            {
                throw new FormatException("Invalid dimension format");
            }
            
            double[][] a = new double[m][];
            for (int i = 0; i < m; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    throw new InvalidOperationException("Input cannot be null or empty");
                }
                
                string[] values = input.Split();
                if (values.Length != n)
                {
                    throw new FormatException($"Expected {n} values but got {values.Length} at row {i}");
                }
                
                a[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    if (!double.TryParse(values[j], out a[i][j]))
                    {
                        throw new FormatException($"Invalid double format at row {i}, column {j}");
                    }
                }
            }
            return a;
        }

        /// <summary>
        /// Prints the 2D array of doubles to standard output.
        /// </summary>
        /// <param name="a">The 2D array of doubles</param>
        public static void Print(double[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            Console.WriteLine("{0} {1}", m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0,9:F5} ", a[i][j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Reads a 1D array of integers from standard input and returns it.
        /// </summary>
        /// <returns>The 1D array of integers</returns>
        public static int[] ReadInt1D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            if (!int.TryParse(input, out int n))
            {
                throw new FormatException("Invalid integer format");
            }
            
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out a[i]))
                {
                    throw new FormatException($"Invalid integer format at index {i}");
                }
            }
            return a;
        }

        /// <summary>
        /// Prints an array of integers to standard output.
        /// </summary>
        /// <param name="a">The 1D array of integers</param>
        public static void Print(int[] a)
        {
            int n = a.Length;
            Console.WriteLine(n);
            for (int i = 0; i < n; i++)
            {
                Console.Write("{0,9} ", a[i]);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Reads a 2D array of integers from standard input and returns it.
        /// </summary>
        /// <returns>The 2D array of integers</returns>
        public static int[][] ReadInt2D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            string[] dimensions = input.Split();
            if (dimensions.Length != 2)
            {
                throw new FormatException("Expected two dimensions");
            }
            
            if (!int.TryParse(dimensions[0], out int m) || !int.TryParse(dimensions[1], out int n))
            {
                throw new FormatException("Invalid dimension format");
            }
            
            int[][] a = new int[m][];
            for (int i = 0; i < m; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    throw new InvalidOperationException("Input cannot be null or empty");
                }
                
                string[] values = input.Split();
                if (values.Length != n)
                {
                    throw new FormatException($"Expected {n} values but got {values.Length} at row {i}");
                }
                
                a[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    if (!int.TryParse(values[j], out a[i][j]))
                    {
                        throw new FormatException($"Invalid integer format at row {i}, column {j}");
                    }
                }
            }
            return a;
        }

        /// <summary>
        /// Print a 2D array of integers to standard output.
        /// </summary>
        /// <param name="a">The 2D array of integers</param>
        public static void Print(int[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            Console.WriteLine("{0} {1}", m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0,9} ", a[i][j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Reads a 1D array of booleans from standard input and returns it.
        /// </summary>
        /// <returns>The 1D array of booleans</returns>
        public static bool[] ReadBoolean1D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            if (!int.TryParse(input, out int n))
            {
                throw new FormatException("Invalid integer format");
            }
            
            bool[] a = new bool[n];
            for (int i = 0; i < n; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !bool.TryParse(input, out a[i]))
                {
                    throw new FormatException($"Invalid boolean format at index {i}");
                }
            }
            return a;
        }

        /// <summary>
        /// Prints a 1D array of booleans to standard output.
        /// </summary>
        /// <param name="a">The 1D array of booleans</param>
        public static void Print(bool[] a)
        {
            int n = a.Length;
            Console.WriteLine(n);
            for (int i = 0; i < n; i++)
            {
                Console.Write(a[i] ? "1 " : "0 ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Reads a 2D array of booleans from standard input and returns it.
        /// </summary>
        /// <returns>The 2D array of booleans</returns>
        public static bool[][] ReadBoolean2D()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidOperationException("Input cannot be null or empty");
            }
            
            string[] dimensions = input.Split();
            if (dimensions.Length != 2)
            {
                throw new FormatException("Expected two dimensions");
            }
            
            if (!int.TryParse(dimensions[0], out int m) || !int.TryParse(dimensions[1], out int n))
            {
                throw new FormatException("Invalid dimension format");
            }
            
            bool[][] a = new bool[m][];
            for (int i = 0; i < m; i++)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    throw new InvalidOperationException("Input cannot be null or empty");
                }
                
                string[] values = input.Split();
                if (values.Length != n)
                {
                    throw new FormatException($"Expected {n} values but got {values.Length} at row {i}");
                }
                
                a[i] = new bool[n];
                for (int j = 0; j < n; j++)
                {
                    if (!bool.TryParse(values[j], out a[i][j]))
                    {
                        throw new FormatException($"Invalid boolean format at row {i}, column {j}");
                    }
                }
            }
            return a;
        }

        /// <summary>
        /// Prints a 2D array of booleans to standard output.
        /// </summary>
        /// <param name="a">The 2D array of booleans</param>
        public static void Print(bool[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            Console.WriteLine("{0} {1}", m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i][j] ? "1 " : "0 ");
                }
                Console.WriteLine();
            }
        }

    }
}
