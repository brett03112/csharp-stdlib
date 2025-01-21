using System;
using System.IO;
using System.Text;

namespace csharp_stdlib
{
    public static class StdIn
    {
        private static TextReader reader = Console.In;
        private static string[] tokens = Array.Empty<string>();
        private static int index = 0;

        public static bool IsEmpty => !HasNextChar();
        public static bool HasNext() => HasNextToken();
        public static bool HasNextLine() => reader.Peek() != -1;

        /// <summary>
        /// Reads all available input and returns it as a string.
        /// If the input is from the console, reads a single line.
        /// Otherwise, reads to the end of the input.
        /// </summary>
        /// <returns>The input as a string.</returns>
        public static string ReadAll()
        {
            // If reading from console, read a single line
            if (reader == Console.In)
            {
                return reader.ReadLine() ?? string.Empty;
            }
            // Otherwise read to end (for string/file input)
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads all remaining tokens from standard input and returns them as an array of strings.
        /// </summary>
        /// <returns>all remaining strings on standard input, as an array</returns>
        public static string[] ReadAllStrings()
        {
            return ReadAll().Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Reads all remaining tokens from standard input, parses them as integers, and returns
        /// them as an array of integers.
        /// </summary>
        /// <returns>all remaining integers on standard input, as an array</returns>
        public static int[] ReadAllInts()
        {
            return Array.ConvertAll(ReadAllStrings(), int.Parse);
        }

        /// <summary>
        /// Reads all remaining tokens from standard input, parses them as doubles, and returns
        /// them as an array of doubles.
        /// </summary>
        /// <returns>all remaining doubles on standard input, as an array</returns> 
        public static double[] ReadAllDoubles()
        {
            return Array.ConvertAll(ReadAllStrings(), double.Parse);
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a short, and returns the short.
        /// </summary>
        /// <returns>the next short on standard input</returns>
        /// <summary>
        /// Reads the next token from standard input, parses it as a short, and returns the short.
        /// </summary>
        /// <returns>the next short on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than short.MinValue or greater than short.MaxValue</exception>
        public static short ReadShort()
        {
            try
            {
                return short.Parse(NextToken());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for a short", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for a short", ex);
            }
        }


        /// <summary>
        /// Reads a single line of text from standard input.
        /// </summary>
        /// <returns>The line read from standard input, or an empty string if no more lines are available.</returns>
        public static string ReadLine()
        {
            return reader.ReadLine() ?? string.Empty;
        }

        /// <summary>
        /// Reads a single string token from standard input.
        /// </summary>
        /// <returns>The next string token</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        public static string ReadString()
        {
            return NextToken();
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as an integer, and returns the integer.
        /// </summary>
        /// <returns>the next integer on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than int.MinValue or greater than int.MaxValue</exception>
        public static int ReadInt()
        {
            try
            {
                return int.Parse(NextToken());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for an integer", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for an integer", ex);
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a double, and returns the double.
        /// Uses invariant culture to ensure consistent parsing regardless of system settings.
        /// </summary>
        /// <returns>the next double on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than double.MinValue or greater than double.MaxValue</exception>
        public static double ReadDouble()
        {
            try
            {
                return double.Parse(NextToken(), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for a double", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for a double", ex);
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a float, and returns the float.
        /// Uses invariant culture to ensure consistent parsing regardless of system settings.
        /// </summary>
        /// <returns>the next float on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than float.MinValue or greater than float.MaxValue</exception>
        public static float ReadFloat()
        {
            try
            {
                return float.Parse(NextToken(), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for a float", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for a float", ex);
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a long, and returns the long.
        /// </summary>
        /// <returns>the next long on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than long.MinValue or greater than long.MaxValue</exception>
        public static long ReadLong()
        {
            try
            {
                return long.Parse(NextToken());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for a long", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for a long", ex);
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a boolean, and returns the boolean.
        /// Valid values are "true" or "false" (case-insensitive).
        /// </summary>
        /// <returns>the next boolean on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not "true" or "false"</exception>
        public static bool ReadBoolean()
        {
            try
            {
                return bool.Parse(NextToken());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not 'true' or 'false'", ex);
            }
        }

        public static char ReadChar()
        {
            while (char.IsWhiteSpace((char)reader.Peek()))
            {
                reader.Read();
            }
            return (char)reader.Read();
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a byte, and returns the byte.
        /// </summary>
        /// <returns>the next byte on standard input</returns>
        /// <exception cref="InvalidOperationException">No more tokens available</exception>
        /// <exception cref="FormatException">Input token is not in a valid format</exception>
        /// <exception cref="OverflowException">Input token represents a number less than byte.MinValue or greater than byte.MaxValue</exception>
        public static byte ReadByte()
        {
            try
            {
                return byte.Parse(NextToken());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Input string was not in a correct format for a byte", ex);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Input number was too large or too small for a byte", ex);
            }
        }

        private static bool HasNextChar()
        {
            while (reader.Peek() != -1 && char.IsWhiteSpace((char)reader.Peek()))
            {
                reader.Read();
            }
            return reader.Peek() != -1;
        }

        private static bool HasNextToken()
        {
            if (index < tokens.Length) return true;

            // Read all available input until we get non-empty tokens
            while (true)
            {
                string? line = reader.ReadLine();
                if (line == null) return false;

                tokens = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 0)
                {
                    index = 0;
                    return true;
                }
            }
        }

        private static string NextToken()
        {
            if (!HasNextToken())
                throw new InvalidOperationException("No more tokens available");
            return tokens[index++];
        }

        public static void Reset()
        {
            // Clear any buffered input
            while (reader.Peek() != -1)
            {
                reader.Read();
            }

            // Reset token buffer and index
            tokens = Array.Empty<string>();
            index = 0;

            // If reading from console, reset to console input
            if (reader != Console.In)
            {
                reader = Console.In;
            }
        }

        public static void SetInput(TextReader input)
        {
            // Close previous reader if it's not console
            if (reader != Console.In)
            {
                reader.Close();
            }

            reader = input;
            tokens = Array.Empty<string>();
            index = 0;
        }

        public static void SetInput(string input)
        {
            // Close previous reader if it's not console
            if (reader != Console.In)
            {
                reader.Close();
            }

            reader = new StringReader(input);
            tokens = Array.Empty<string>();
            index = 0;
        }
    }
}
