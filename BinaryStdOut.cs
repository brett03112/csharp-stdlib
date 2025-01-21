using System;
using System.IO;

namespace StdLib
{
    public sealed class BinaryStdOut : IDisposable
    {
        private static readonly Lazy<BinaryStdOut> _instance = new(() => new BinaryStdOut());
        private Stream _output;
        private int _buffer;
        private int _bitsRemaining;
        private bool _isInitialized;

        public static BinaryStdOut Instance => _instance.Value;

        private BinaryStdOut()
        {
            Initialize();
        }

        private void Initialize()
        {
            _output = Console.OpenStandardOutput();
            _buffer = 0;
            _bitsRemaining = 8;
            _isInitialized = true;
        }

        private void WriteBit(bool bit)
        {
            if (!_isInitialized) Initialize();

            _buffer <<= 1;
            if (bit) _buffer |= 1;

            _bitsRemaining--;
            if (_bitsRemaining == 0) ClearBuffer();
        }

        private void WriteByte(int x)
        {
            if (!_isInitialized) Initialize();

            if (x < 0 || x >= 256)
                throw new ArgumentOutOfRangeException(nameof(x), "x must be between 0 and 255");

            if (_bitsRemaining == 8)
            {
                try
                {
                    _output.WriteByte((byte)x);
                }
                catch (IOException)
                {
                    throw new InvalidOperationException("Error writing to output stream");
                }
                return;
            }

            for (int i = 0; i < 8; i++)
            {
                bool bit = ((x >> (8 - i - 1)) & 1) == 1;
                WriteBit(bit);
            }
        }

        private void ClearBuffer()
        {
            if (!_isInitialized) Initialize();

            if (_bitsRemaining == 8) return;

            if (_bitsRemaining > 0) _buffer <<= _bitsRemaining;
            try
            {
                _output.WriteByte((byte)_buffer);
            }
            catch (IOException)
            {
                throw new InvalidOperationException("Error writing to output stream");
            }
            _buffer = 0;
            _bitsRemaining = 8;
        }

        public void Flush()
        {
            ClearBuffer();
            try
            {
                _output.Flush();
            }
            catch (IOException)
            {
                throw new InvalidOperationException("Error flushing output stream");
            }
        }

        public void Close()
        {
            Flush();
            try
            {
                _output.Close();
            }
            catch (IOException)
            {
                throw new InvalidOperationException("Error closing output stream");
            }
            _isInitialized = false;
        }

        public void Write(bool x)
        {
            WriteBit(x);
        }

        public void Write(byte x)
        {
            WriteByte(x & 0xff);
        }

        public void Write(char x)
        {
            if (x < 0 || x > 255)
                throw new ArgumentOutOfRangeException(nameof(x), "x must be between 0 and 255");

            WriteByte(x);
        }

        public void Write(int x)
        {
            WriteByte((x >> 24) & 0xff);
            WriteByte((x >> 16) & 0xff);
            WriteByte((x >> 8) & 0xff);
            WriteByte(x & 0xff);
        }

        public void Write(int x, int r)
        {
            if (r == 32)
            {
                Write(x);
                return;
            }

            if (r < 1 || r > 32)
                throw new ArgumentOutOfRangeException(nameof(r), "r must be between 1 and 32");

            if (x >= (1 << r))
                throw new ArgumentOutOfRangeException(nameof(x), $"x must be less than {1 << r}");

            for (int i = 0; i < r; i++)
            {
                bool bit = ((x >> (r - i - 1)) & 1) == 1;
                WriteBit(bit);
            }
        }

        public void Write(double x)
        {
            Write(BitConverter.DoubleToInt64Bits(x));
        }

        public void Write(long x)
        {
            WriteByte((int)((x >> 56) & 0xff));
            WriteByte((int)((x >> 48) & 0xff));
            WriteByte((int)((x >> 40) & 0xff));
            WriteByte((int)((x >> 32) & 0xff));
            WriteByte((int)((x >> 24) & 0xff));
            WriteByte((int)((x >> 16) & 0xff));
            WriteByte((int)((x >> 8) & 0xff));
            WriteByte((int)(x & 0xff));
        }

        public void Write(float x)
        {
            Write(BitConverter.SingleToInt32Bits(x));
        }

        public void Write(short x)
        {
            WriteByte((x >> 8) & 0xff);
            WriteByte(x & 0xff);
        }

        public void Write(string s)
        {
            foreach (char c in s)
            {
                Write(c);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
