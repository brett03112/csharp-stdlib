using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace csharp_stdlib
{
    /// <summary>
    /// Binary input stream for reading primitive data types from various sources.
    /// </summary>
    /// <remarks>
    /// This class provides methods for reading binary data from files, network streams,
    /// and other input sources. It supports reading primitive types like boolean, char,
    /// int, long, float, double, and strings.
    /// </remarks>
    public class BinaryIn : IDisposable
    {
        private const int EOF = -1;
        private BufferedStream _inputStream = null!;
        private int _buffer;
        private int _n;

        /// <summary>
        /// Initializes a binary input stream from standard input.
        /// </summary>
        public BinaryIn() : this(Console.OpenStandardInput()) { }

        /// <summary>
        /// Initializes a binary input stream from a specified stream.
        /// </summary>
        /// <param name="inputStream">The input stream to read from</param>
        /// <exception cref="ArgumentNullException">Thrown if inputStream is null</exception>
        public BinaryIn(Stream inputStream)
        {
            _inputStream = new BufferedStream(inputStream) ?? throw new ArgumentNullException(nameof(inputStream));
            FillBuffer();
        }

        /// <summary>
        /// Initializes a binary input stream from a network socket.
        /// </summary>
        /// <param name="socket">The network socket to read from</param>
        /// <exception cref="ArgumentNullException">Thrown if socket is null</exception>
        /// <exception cref="IOException">Thrown if there's an error reading from the socket</exception>
        public BinaryIn(Socket socket)
        {
            try
            {
            _inputStream = new BufferedStream(new NetworkStream(socket)) ?? throw new ArgumentNullException(nameof(socket));
                FillBuffer();
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Could not read socket: {socket}");
            }
        }

        private BinaryIn(BufferedStream stream)
        {
            _inputStream = stream ?? throw new ArgumentNullException(nameof(stream));
            FillBuffer();
        }

        /// <summary>
        /// Creates a BinaryIn instance from a URL asynchronously.
        /// </summary>
        /// <param name="url">The URL to read from</param>
        /// <returns>A Task that represents the asynchronous operation and returns a BinaryIn instance</returns>
        /// <exception cref="IOException">Thrown if there's an error reading from the URL</exception>
        public static async Task<BinaryIn> FromUrlAsync(Uri url)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                var stream = new BufferedStream(await response.Content.ReadAsStreamAsync());
                return new BinaryIn(stream);
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Could not open URL: '{url}'");
                return new BinaryIn(new BufferedStream(Stream.Null));
            }
        }

        /// <summary>
        /// Creates a BinaryIn instance from either a file path or URL asynchronously.
        /// </summary>
        /// <param name="name">The file path or URL to read from</param>
        /// <returns>A Task that represents the asynchronous operation and returns a BinaryIn instance</returns>
        /// <exception cref="ArgumentException">Thrown if the input cannot be read</exception>
        /// <exception cref="IOException">Thrown if there's an error reading from the source</exception>
        /// <remarks>
        /// This method first attempts to read from a local file. If the specified path doesn't exist,
        /// it will attempt to read from a URL. The method supports both local file paths and absolute URLs.
        /// When reading from files, it uses FileShare.ReadWrite to allow concurrent access.
        /// </remarks>
        public static async Task<BinaryIn> FromFileOrUrlAsync(string name)
        {
            try
            {
                if (File.Exists(name))
                {
                    // Use FileShare.ReadWrite to allow concurrent access
                    var fileStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var stream = new BufferedStream(fileStream);
                    return new BinaryIn(stream);
                }

                if (Uri.IsWellFormedUriString(name, UriKind.Absolute))
                {
                    return await FromUrlAsync(new Uri(name));
                }

                throw new ArgumentException($"Could not read: '{name}'");
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Could not open: '{name}'");
                return new BinaryIn(new BufferedStream(Stream.Null));
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine($"Could not open: '{name}'");
                return new BinaryIn(new BufferedStream(Stream.Null));
            }
        }

        /// <summary>
        /// Initializes a binary input stream from a file name.
        /// </summary>
        /// <param name="name">The name of the file to read from</param>
        /// <remarks>
        /// This constructor is kept for backward compatibility but should be avoided
        /// in favor of <see cref="FromFileOrUrlAsync(string)"/>.
        /// </remarks>
        public BinaryIn(string name) : this(new BufferedStream(Stream.Null))
        {
            // This constructor is kept for backward compatibility
            // but should be avoided in favor of FromFileOrUrlAsync
            if (File.Exists(name))
            {
                // Use FileShare.ReadWrite to allow concurrent access
                var fileStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                _inputStream = new BufferedStream(fileStream);
                FillBuffer();
            }
        }

        private void FillBuffer()
        {
            try
            {
                _buffer = _inputStream.ReadByte();
                _n = 8;
            }
            catch (IOException)
            {
                Console.Error.WriteLine("EOF");
                _buffer = EOF;
                _n = -1;
            }
        }

        /// <summary>
        /// Determines whether the input stream exists.
        /// </summary>
        /// <returns>true if the input stream exists; otherwise, false</returns>
        public bool Exists()
        {
            return _inputStream != null;
        }

        /// <summary>
        /// Determines whether the input stream is empty.
        /// </summary>
        /// <returns>true if the input stream is empty; otherwise, false</returns>
        public bool IsEmpty()
        {
            return _buffer == EOF;
        }

        /// <summary>
        /// Reads a single boolean value from the input stream.
        /// </summary>
        /// <returns>The boolean value read from the stream</returns>
        /// <exception cref="InvalidOperationException">Thrown if reading from an empty stream</exception>
        public bool ReadBoolean()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");
            _n--;
            bool bit = ((_buffer >> _n) & 1) == 1;
            if (_n == 0) FillBuffer();
            return bit;
        }

        /// <summary>
        /// Reads a single 8-bit character from the input stream.
        /// </summary>
        /// <returns>The character read from the stream</returns>
        /// <exception cref="InvalidOperationException">Thrown if reading from an empty stream</exception>
        public char ReadChar()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");

            if (_n == 8)
            {
                int x = _buffer;
                FillBuffer();
                return (char)((byte)(x & 0xff));
            }

            int temp = _buffer;
            temp <<= (8 - _n);
            int oldN = _n;
            FillBuffer();
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");
            _n = oldN;
            temp |= (_buffer >> _n);
            return (char)((byte)(temp & 0xff));
        }

        /// <summary>
        /// Reads a single character using a specified number of bits.
        /// </summary>
        /// <param name="r">The number of bits to use (1-16)</param>
        /// <returns>The character read from the stream</returns>
        /// <exception cref="ArgumentException">Thrown if r is not between 1 and 16</exception>
        /// <remarks>
        /// This method allows reading characters with custom bit lengths, which can be useful
        /// for specialized binary formats or compressed data. The character is constructed
        /// by reading 'r' bits from the stream and padding with zeros if necessary.
        /// </remarks>
        public char ReadChar(int r)
        {
            if (r < 1 || r > 16) throw new ArgumentException($"Illegal value of r = {r}");

            if (r == 8) return ReadChar();

            int x = 0;
            for (int i = 0; i < r; i++)
            {
                x <<= 1;
                bool bit = ReadBoolean();
                if (bit) x |= 1;
            }
            return (char)((byte)x);
        }

        /// <summary>
        /// Reads a string from the input stream.
        /// </summary>
        /// <returns>The string read from the stream</returns>
        /// <exception cref="InvalidOperationException">Thrown if reading from an empty stream</exception>
        public string ReadString()
        {
            if (IsEmpty()) throw new InvalidOperationException("Reading from empty input stream");

            var sb = new System.Text.StringBuilder();
            while (!IsEmpty())
            {
                char c = ReadChar();
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads a 16-bit signed integer from the input stream.
        /// </summary>
        /// <returns>The short value read from the stream</returns>
        public short ReadShort()
        {
            short x = 0;
            for (int i = 0; i < 2; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= (short)c;
            }
            return x;
        }

        /// <summary>
        /// Reads a 32-bit signed integer from the input stream.
        /// </summary>
        /// <returns>The int value read from the stream</returns>
        public int ReadInt()
        {
            int x = 0;
            for (int i = 0; i < 4; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= (int)c;
            }
            return x;
        }

        /// <summary>
        /// Reads a 32-bit signed integer using a specified number of bits.
        /// </summary>
        /// <param name="r">The number of bits to use (1-32)</param>
        /// <returns>The int value read from the stream</returns>
        /// <exception cref="ArgumentException">Thrown if r is not between 1 and 32</exception>
        /// <remarks>
        /// This method reads an integer value using exactly 'r' bits from the stream.
        /// The value is constructed by reading bits sequentially and shifting them into
        /// position. This is particularly useful for reading packed binary data where
        /// integers may be stored using fewer than 32 bits.
        /// </remarks>
        public int ReadInt(int r)
        {
            if (r < 1 || r > 32) throw new ArgumentException($"Illegal value of r = {r}");

            if (r == 32) return ReadInt();

            int x = 0;
            for (int i = 0; i < r; i++)
            {
                x <<= 1;
                bool bit = ReadBoolean();
                if (bit) x |= 1;
            }
            return x;
        }

        /// <summary>
        /// Reads a 64-bit signed integer from the input stream.
        /// </summary>
        /// <returns>The long value read from the stream</returns>
        public long ReadLong()
        {
            long x = 0;
            for (int i = 0; i < 8; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= (long)c;
            }
            return x;
        }

        /// <summary>
        /// Reads a 64-bit floating-point number from the input stream.
        /// </summary>
        /// <returns>The double value read from the stream</returns>
        public double ReadDouble()
        {
            byte[] buffer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                buffer[i] = ReadByte();
            }
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Reads a 32-bit floating-point number from the input stream.
        /// </summary>
        /// <returns>The float value read from the stream</returns>
        public float ReadFloat()
        {
            byte[] buffer = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                buffer[i] = ReadByte();
            }
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads a single byte from the input stream.
        /// </summary>
        /// <returns>The byte value read from the stream</returns>
        public byte ReadByte()
        {
            char c = ReadChar();
            return (byte)(c & 0xff);
        }

        private bool _disposed = false;

        /// <summary>
        /// Releases the unmanaged resources used by the BinaryIn and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        /// <remarks>
        /// This method is called by the public Dispose() method and the finalizer.
        /// When disposing is true, both managed and unmanaged resources are released.
        /// When disposing is false, only unmanaged resources are released.
        /// The method ensures that resources are only disposed once.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _inputStream?.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the BinaryIn.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer for the BinaryIn class.
        /// </summary>
        ~BinaryIn()
        {
            Dispose(false);
        }
    }
}
