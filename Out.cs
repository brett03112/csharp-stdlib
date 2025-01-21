using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace csharp_stdlib
{
    public sealed class Out : IDisposable
    {
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        private readonly TextWriter writer;

        public Out()
        {
            writer = Console.Out;
        }

        public Out(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            try
            {
                var stream = new NetworkStream(socket);
                writer = new StreamWriter(stream, Encoding) { AutoFlush = true };
            }
            catch (IOException e)
            {
                throw new ArgumentException("Could not create output stream from socket", e);
            }
        }

        public Out(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");

            try
            {
                var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(stream, Encoding) { AutoFlush = true };
            }
            catch (IOException e)
            {
                throw new ArgumentException($"Could not create file '{filename}' for writing", e);
            }
        }

        public Out(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            writer = new StreamWriter(stream, Encoding) { AutoFlush = true };
        }

        public void Close()
        {
            writer?.Close();
        }

        public void Dispose()
        {
            Close();
        }

        public void Print()
        {
            writer.Flush();
        }

        public void Print(object obj)
        {
            writer.Write(obj);
            writer.Flush();
        }

        public void Print(bool value)
        {
            writer.Write(value);
            writer.Flush();
        }

        public void Print(char value)
        {
            writer.Write(value);
            writer.Flush();
        }

        public void Print(double value)
        {
            writer.Write(value.ToString(Culture));
            writer.Flush();
        }

        public void Print(float value)
        {
            writer.Write(value.ToString(Culture));
            writer.Flush();
        }

        public void Print(int value)
        {
            writer.Write(value);
            writer.Flush();
        }

        public void Print(long value)
        {
            writer.Write(value);
            writer.Flush();
        }

        public void Print(byte value)
        {
            writer.Write(value);
            writer.Flush();
        }

        public void Println()
        {
            writer.WriteLine();
            writer.Flush();
        }

        public void Println(object obj)
        {
            writer.WriteLine(obj);
            writer.Flush();
        }

        public void Println(bool value)
        {
            writer.WriteLine(value);
            writer.Flush();
        }

        public void Println(char value)
        {
            writer.WriteLine(value);
            writer.Flush();
        }

        public void Println(double value)
        {
            writer.WriteLine(value.ToString(Culture));
            writer.Flush();
        }

        public void Println(float value)
        {
            writer.WriteLine(value.ToString(Culture));
            writer.Flush();
        }

        public void Println(int value)
        {
            writer.WriteLine(value);
            writer.Flush();
        }

        public void Println(long value)
        {
            writer.WriteLine(value);
            writer.Flush();
        }

        public void Println(byte value)
        {
            writer.WriteLine(value);
            writer.Flush();
        }

        public void Printf(string format, params object[] args)
        {
            writer.Write(string.Format(Culture, format, args));
            writer.Flush();
        }

        public void Printf(CultureInfo culture, string format, params object[] args)
        {
            writer.Write(string.Format(culture, format, args));
            writer.Flush();
        }
    }
}
