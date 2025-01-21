using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace csharp_stdlib
{
    /// <summary>
    /// Provides methods for writing to standard output.
    /// </summary>
    public static class StdOut
    {
        private static readonly object _syncLock = new object();
        private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;
        private static readonly StringWriter _outputCapture = new StringWriter();
        private static TextWriter _originalOut = Console.Out;
        private static int _threadSafeCounter = 0;

        private static bool _isCapturing = false;

        private static void WriteOutput(string s)
        {
            lock (_syncLock)
            {
                Console.Write(s);
                if (_isCapturing)
                {
                    _outputCapture.Write(s);
                }
            }
        }

        private static void WriteLineOutput(string s = "")
        {
            lock (_syncLock)
            {
                Console.WriteLine(s);
                if (_isCapturing)
                {
                    _outputCapture.WriteLine(s);
                }
            }
        }

        /// <summary>
        /// Prints the string to standard output.
        /// </summary>
        /// <param name="s">The string to print.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null.</exception>
        public static void Print(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            
            lock (_syncLock)
            {
                WriteOutput(s);
            }
        }

        /// <summary>
        /// Prints a new line to standard output.
        /// </summary>
        public static void Println()
        {
            lock (_syncLock)
            {
                WriteLineOutput();
            }
        }

        /// <summary>
        /// Prints the string to standard output, followed by a new line.
        /// </summary>
        /// <param name="s">The string to print.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null.</exception>
        public static void Println(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            
            lock (_syncLock)
            {
                WriteLineOutput(s);
            }
        }

        /// <summary>
        /// Prints a formatted string to standard output, followed by a new line.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="format"/> is null.</exception>
        public static void Println(string format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            
            lock (_syncLock)
            {
                WriteLineOutput(string.Format(format, args));
            }
        }

        /// <summary>
        /// Prints a formatted string to standard output.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="format"/> is null.</exception>
        public static void Printf(string format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            
            lock (_syncLock)
            {
                WriteOutput(string.Format(format, args));
            }
        }

        /// <summary>
        /// Prints a formatted string to standard output using the invariant culture.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="format"/> is null.</exception>
        public static void PrintfInvariant(string format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            
            lock (_syncLock)
            {
                string formatted = string.Format(_invariantCulture, format, args);
                WriteOutput(formatted);
            }
        }

        /// <summary>
        /// Prints a formatted string to standard output using the invariant culture, followed by a new line.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="format"/> is null.</exception>
        public static void PrintlnInvariant(string format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            
            lock (_syncLock)
            {
                string formatted = string.Format(_invariantCulture, format, args);
                WriteLineOutput(formatted);
            }
        }

        /// <summary>
        /// Flushes the output buffer.
        /// </summary>
        public static void Flush()
        {
            lock (_syncLock)
            {
                Console.Out.Flush();
                _outputCapture.Flush();
            }
        }

        /// <summary>
        /// Gets the captured output.
        /// </summary>
        /// <returns>The captured output as a string.</returns>
        public static string GetCapturedOutput()
        {
            lock (_syncLock)
            {
                return _outputCapture.ToString();
            }
        }

        /// <summary>
        /// Clears the captured output.
        /// </summary>
        public static void ClearCapturedOutput()
        {
            lock (_syncLock)
            {
                _outputCapture.GetStringBuilder().Clear();
            }
        }

        /// <summary>
        /// Starts capturing the output.
        /// </summary>
        public static void StartCapture()
        {
            lock (_syncLock)
            {
                _isCapturing = true;
                _outputCapture.GetStringBuilder().Clear();
            }
        }

        /// <summary>
        /// Stops capturing the output.
        /// </summary>
        public static void StopCapture()
        {
            lock (_syncLock)
            {
                _isCapturing = false;
            }
        }

        /// <summary>
        /// Increments a thread-safe counter.
        /// </summary>
        /// <returns>The incremented counter value.</returns>
        public static int IncrementCounter()
        {
            lock (_syncLock)
            {
                return ++_threadSafeCounter;
            }
        }
    }
}

