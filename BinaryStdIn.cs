using System;
using System.IO;
using System.Text;

namespace StdLib
{
    public sealed class BinaryStdIn : IDisposable
    {
        private const int EOF = -1;
        private static readonly Lazy<BinaryStdIn> _instance = new(() => new BinaryStdIn());
        private Stream _input;
        private int _buffer;
        private int _bitsRemaining;
        private bool _isInitialized;

        public static BinaryStdIn Instance => _instance.Value;

        private BinaryStdIn()
        {
            Initialize();
        }

        private void Initialize()
        {
            _input = Console.OpenStandardInput();
            _buffer = 0;
            _bitsRemaining = 0;
            FillBuffer();
            _isInitialized = true;
        }

        private void FillBuffer()
        {
            try
            {
                _buffer = _input.ReadByte();
                _bitsRemaining = 8;
            }
            catch (IOException)
            {
                _buffer = EOF;
                _bitsRemaining = -1;
            }
        }

        public void Close()
        {
            if (!_isInitialized) Initialize();
            _input.Close();
            _isInitialized = false;
        }

        public bool IsEmpty()
        {
            if (!_isInitialized) Initialize();
            return _buffer == EOF;
        }

        public bool ReadBoolean()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");
            _bitsRemaining--;
            bool bit = ((_buffer >> _bitsRemaining) & 1) == 1;
            if (_bitsRemaining == 0) FillBuffer();
            return bit;
        }

        public char ReadChar()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");

            if (_bitsRemaining == 8)
            {
                byte b = (byte)(_buffer & 0xff);
                FillBuffer();
                return (char)b;
            }

            int temp = _buffer;
            temp <<= (8 - _bitsRemaining);
            int oldN = _bitsRemaining;
            FillBuffer();
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");
            _bitsRemaining = oldN;
            temp |= (_buffer >> _bitsRemaining);
            return (char)(byte)(temp & 0xff);
        }

        public char ReadChar(int r)
        {
            if (r < 1 || r > 16) 
                throw new ArgumentOutOfRangeException(nameof(r), "r must be between 1 and 16");

            if (r == 8) return ReadChar();

            char x = '\0';
            for (int i = 0; i < r; i++)
            {
                x = (char)(x << 1);
                if (ReadBoolean()) x = (char)(x | 1);
            }
            return x;
        }

        public string ReadString()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");

            var sb = new StringBuilder();
            while (!IsEmpty())
            {
                sb.Append(ReadChar());
            }
            return sb.ToString();
        }

        public short ReadShort()
        {
            short x = 0;
            for (int i = 0; i < 2; i++)
            {
                x <<= 8;
                x |= (short)(ReadChar() & 0xff);
            }
            return x;
        }

        public int ReadInt()
        {
            int x = 0;
            for (int i = 0; i < 4; i++)
            {
                x <<= 8;
                x |= (int)(ReadChar() & 0xff);
            }
            return x;
        }

        public int ReadInt(int r)
        {
            if (r < 1 || r > 32)
                throw new ArgumentOutOfRangeException(nameof(r), "r must be between 1 and 32");

            if (r == 32) return ReadInt();

            int x = 0;
            for (int i = 0; i < r; i++)
            {
                x <<= 1;
                if (ReadBoolean()) x |= 1;
            }
            return x;
        }

        public long ReadLong()
        {
            long x = 0;
            for (int i = 0; i < 8; i++)
            {
                x <<= 8;
                x |= (long)(ReadChar() & 0xff);
            }
            return x;
        }

        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
        }

        public float ReadFloat()
        {
            return BitConverter.Int32BitsToSingle(ReadInt());
        }

        public byte ReadByte()
        {
            return (byte)ReadChar();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
