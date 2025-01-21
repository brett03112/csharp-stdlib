using System;
using System.IO;
using System.Net.Sockets;

namespace csharp_stdlib
{
    /// <summary>
    /// BinaryOut provides methods for writing primitive data types to a binary stream.
    /// The stream can be a file, network socket, or standard output.
    /// Data is written in big-endian format.
    /// </summary>
    public class BinaryOut : IDisposable
    {
        private BufferedStream _outputStream = null!;
        private int _buffer;
        private int _n;

        public BinaryOut() : this(Console.OpenStandardOutput()) { }

        public BinaryOut(Stream outputStream)
        {
            _outputStream = new BufferedStream(outputStream) ?? throw new ArgumentNullException(nameof(outputStream));
            _buffer = 0;
            _n = 0;
        }

        /// <summary>
        /// Initializes a BinaryOut instance writing to the specified socket.
        /// </summary>
        /// <param name="socket">The socket to write to</param>
        /// <exception cref="IOException">Thrown if there is an error creating the network stream</exception>
        public BinaryOut(Socket socket)
        {
            try
            {
                _outputStream = new BufferedStream(new NetworkStream(socket));
                _buffer = 0;
                _n = 0;
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Could not write to socket: {socket}");
            }
        }

        /// <summary>
        /// Initializes a BinaryOut instance writing to the specified file.
        /// </summary>
        /// <param name="name">The name of the file to write to</param>
        /// <exception cref="IOException">Thrown if there is an error opening the file</exception>
        public BinaryOut(string name)
        {
            try
            {
                // Use FileShare.ReadWrite to allow concurrent access
                _outputStream = new BufferedStream(new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
                _buffer = 0;
                _n = 0;
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"Could not open: '{name}': {e.Message}");
                throw;
            }
        }

        private void WriteBit(bool bit)
        {
            _buffer <<= 1;
            if (bit) _buffer |= 1;

            _n++;
            if (_n == 8) ClearBuffer();
        }

        private void WriteByte(int x)
        {
            if (_n == 0)
            {
                try
                {
                _outputStream?.WriteByte((byte)x);
                }
            catch (IOException)
                {
                    Console.Error.WriteLine("Write error");
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
            if (_n == 0) return;

            if (_n > 0) _buffer <<= (8 - _n);
            try
            {
                _outputStream?.WriteByte((byte)_buffer);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Write error");
            }
            _n = 0;
            _buffer = 0;
        }

        public void Flush()
        {
            ClearBuffer();
            try
            {
                _outputStream?.Flush();
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Flush error");
            }
        }

    private bool _disposed = false;

    /// <summary>
    /// Closes and releases all resources used by the BinaryOut instance.
    /// </summary>
    public void Close()
    {
        Dispose();
    }

    /// <summary>
    /// Releases all resources used by the BinaryOut instance.
    /// </summary>
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
                    try
                    {
                        Flush();
                    }
                    catch (IOException)
                    {
                        // Ignore flush errors during dispose
                    }
                    
                    try
                    {
                        _outputStream?.Close();
                        _outputStream?.Dispose();
                    }
                    catch (IOException)
                    {
                        // Ignore close errors
                    }
                    finally
                    {
                        _outputStream = null;
                    }
                }
                _disposed = true;
            }
        }

    ~BinaryOut()
    {
        Dispose(false);
    }


        /// <summary>
        /// Writes a boolean value to the output stream.
        /// </summary>
        /// <param name="x">The boolean value to write</param>
        public void Write(bool x)
        {
            WriteBit(x);
        }

        /// <summary>
        /// Writes a byte value to the output stream.
        /// </summary>
        /// <param name="x">The byte value to write</param>
        public void Write(byte x)
        {
            WriteByte(x & 0xff);
        }

        /// <summary>
        /// Writes a character to the output stream.
        /// </summary>
        /// <param name="x">The character to write</param>
        /// <exception cref="ArgumentException">Thrown if the character is not an 8-bit character</exception>
        public void Write(char x)
        {
            if (x < 0 || x >= 256) throw new ArgumentException("Illegal 8-bit char = " + x);
            WriteByte(x);
        }

        /// <summary>
        /// Writes a character to the output stream using the specified number of bits.
        /// </summary>
        /// <param name="x">The character to write</param>
        /// <param name="r">The number of bits to use (1-16)</param>
        /// <exception cref="ArgumentException">Thrown if r is invalid or character cannot be represented with r bits</exception>
        public void Write(char x, int r)
        {
            if (r == 8)
            {
                Write(x);
                return;
            }
            if (r < 1 || r > 16) throw new ArgumentException("Illegal value for r = " + r);
            if (x >= (1 << r)) throw new ArgumentException("Illegal " + r + "-bit char = " + x);
            for (int i = 0; i < r; i++)
            {
                bool bit = ((x >> (r - i - 1)) & 1) == 1;
                WriteBit(bit);
            }
        }

        /// <summary>
        /// Writes a 16-bit integer to the output stream.
        /// </summary>
        /// <param name="x">The short value to write</param>
        public void Write(short x)
        {
            WriteByte((x >> 8) & 0xff);
            WriteByte(x & 0xff);
        }

        /// <summary>
        /// Writes a 32-bit integer to the output stream.
        /// </summary>
        /// <param name="x">The integer value to write</param>
        public void Write(int x)
        {
            WriteByte((x >> 24) & 0xff);
            WriteByte((x >> 16) & 0xff);
            WriteByte((x >> 8) & 0xff);
            WriteByte(x & 0xff);
        }

        /// <summary>
        /// Writes an integer to the output stream using the specified number of bits.
        /// </summary>
        /// <param name="x">The integer value to write</param>
        /// <param name="r">The number of bits to use (1-32)</param>
        /// <exception cref="ArgumentException">Thrown if r is invalid or integer cannot be represented with r bits</exception>
        public void Write(int x, int r)
        {
            if (r == 32)
            {
                Write(x);
                return;
            }
            if (r < 1 || r > 32) throw new ArgumentException("Illegal value for r = " + r);
            if (x < 0 || x >= (1 << r)) throw new ArgumentException("Illegal " + r + "-bit int = " + x);
            for (int i = 0; i < r; i++)
            {
                bool bit = ((x >> (r - i - 1)) & 1) == 1;
                WriteBit(bit);
            }
        }

        /// <summary>
        /// Writes a 64-bit integer to the output stream.
        /// </summary>
        /// <param name="x">The long value to write</param>
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

        /// <summary>
        /// Writes a 32-bit floating point number to the output stream.
        /// </summary>
        /// <param name="x">The float value to write</param>
        public void Write(float x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            foreach (byte b in bytes)
            {
                Write(b);
            }
        }

        /// <summary>
        /// Writes a 64-bit floating point number to the output stream.
        /// </summary>
        /// <param name="x">The double value to write</param>
        public void Write(double x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            foreach (byte b in bytes)
            {
                Write(b);
            }
        }

        /// <summary>
        /// Writes a string to the output stream by writing each character.
        /// </summary>
        /// <param name="s">The string to write</param>
        public void Write(string s)
        {
            for (int i = 0; i < s.Length; i++)
                Write(s[i]);
        }

        /// <summary>
        /// Writes a string to the output stream by writing each character using the specified number of bits.
        /// </summary>
        /// <param name="s">The string to write</param>
        /// <param name="r">The number of bits to use per character (1-16)</param>
        /// <exception cref="ArgumentException">Thrown if r is invalid or character cannot be represented with r bits</exception> 
        public void Write(string s, int r)
        {
            for (int i = 0; i < s.Length; i++)
                Write(s[i], r);
        }
    }
}
