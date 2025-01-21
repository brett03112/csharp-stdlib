using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_stdlib
{
    public class StdOutTests : IDisposable
    {
        private readonly StringWriter _outputWriter;
        private readonly TextWriter _originalOutput;
        private bool _disposed = false;

        public StdOutTests()
        {
            _outputWriter = new StringWriter();
            _originalOutput = Console.Out;
            StdOut.ClearCapturedOutput();
            StdOut.StartCapture();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.SetOut(_originalOutput);
                    _outputWriter.Dispose();
                    StdOut.StopCapture();
                }
                _disposed = true;
            }
        }

        [Fact]
        public void TestPrint()
        {
            StdOut.Print("Test");
            Assert.Equal("Test", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintln()
        {
            StdOut.Println("Test");
            Assert.Equal("Test\r\n", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintlnEmpty()
        {
            StdOut.Println();
            Assert.Equal("\r\n", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintf()
        {
            StdOut.Printf("Test {0}", 123);
            Assert.Equal("Test 123", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintlnFormat()
        {
            StdOut.Println("Test {0}", 123);
            Assert.Equal("Test 123\r\n", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintfInvariant()
        {
            StdOut.PrintfInvariant("Test {0:0.00}", 1.23);
            Assert.Equal("Test 1.23", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestPrintlnInvariant()
        {
            StdOut.PrintlnInvariant("Test {0:0.00}", 1.23);
            Assert.Equal("Test 1.23\r\n", StdOut.GetCapturedOutput());
        }

        [Fact]
        public void TestThreadSafety()
        {
            const int iterations = 3000;
            var exceptions = new ConcurrentBag<Exception>();
            var countdown = new CountdownEvent(iterations);

            Parallel.For(0, iterations, i =>
            {
                try
                {
                    StdOut.Println($"Test {i}");
                    StdOut.IncrementCounter();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
                finally
                {
                    countdown.Signal();
                }
            });

            countdown.Wait();

            Assert.Empty(exceptions);
            Assert.Equal(iterations, StdOut.GetCapturedOutput().Split('\n').Length - 1);
        }

        [Fact]
        public void TestNullPrint()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => StdOut.Print(null));
            Assert.Equal("s", ex.ParamName);
        }

        [Fact]
        public void TestNullPrintln()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => StdOut.Println((string)null));
            Assert.Equal("s", ex.ParamName);
        }

        [Fact]
        public void TestNullPrintlnFormat()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => StdOut.Println(null, 123));
            Assert.Equal("format", ex.ParamName);
        }

        [Fact]
        public void TestNullPrintf()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => StdOut.Printf(null, 123));
            Assert.Equal("format", ex.ParamName);
        }

        [Fact]
        public void TestCultureInvariance()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                StdOut.PrintfInvariant("Test {0:0.00}", 1.23);
                Assert.Equal("Test 1.23", StdOut.GetCapturedOutput());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}
