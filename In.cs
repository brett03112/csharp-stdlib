using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace csharp_stdlib
{
    public sealed class In : IDisposable
    {
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        
        private readonly StreamReader reader;
        private readonly string[] emptyTokens = Array.Empty<string>();

        public In()
        {
            reader = new StreamReader(Console.OpenStandardInput(), Encoding);
        }

        public In(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            try
            {
                var stream = new NetworkStream(socket);
                reader = new StreamReader(stream, Encoding);
            }
            catch (IOException e)
            {
                throw new ArgumentException("Could not open socket", e);
            }
        }

        public In(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            try
            {
                var webRequest = WebRequest.Create(url);
                var response = webRequest.GetResponse();
                var stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding);
            }
            catch (IOException e)
            {
                throw new ArgumentException("Could not read URL", e);
            }
        }

        public In(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            try
            {
                var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(stream, Encoding);
            }
            catch (IOException e)
            {
                throw new ArgumentException("Could not read file", e);
            }
        }

        public In(string name)
        {
            if (string.IsNullOrEmpty(name)) 
                throw new ArgumentException("Name cannot be null or empty");

            try
            {
                if (File.Exists(name))
                {
                    var stream = new FileStream(name, FileMode.Open, FileAccess.Read);
                    reader = new StreamReader(stream, Encoding);
                    return;
                }

                var url = new Uri(name);
                var webRequest = WebRequest.Create(url);
                var response = webRequest.GetResponse();
                var stream2 = response.GetResponseStream();
                reader = new StreamReader(stream2, Encoding);
            }
            catch (Exception e) when (e is IOException || e is UriFormatException)
            {
                throw new ArgumentException("Could not read from source", e);
            }
        }

        public bool Exists => reader != null;

        public bool IsEmpty()
        {
            return reader.Peek() == -1;
        }

        public bool HasNextLine()
        {
            return !IsEmpty();
        }

        public string ReadLine()
        {
            try
            {
                return reader.ReadLine();
            }
            catch (IOException)
            {
                return null;
            }
        }

        public string ReadAll()
        {
            try
            {
                return reader.ReadToEnd();
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }

        public string ReadString()
        {
            try
            {
                // Read until whitespace or end of stream
                var sb = new StringBuilder();
                while (true)
                {
                    int ch = reader.Read();
                    if (ch == -1 || char.IsWhiteSpace((char)ch))
                        break;
                    sb.Append((char)ch);
                }
                return sb.Length > 0 ? sb.ToString() : throw new InvalidOperationException("No more tokens available");
            }
            catch (IOException)
            {
                throw new InvalidOperationException("No more tokens available");
            }
        }

        public int ReadInt()
        {
            if (int.TryParse(ReadString(), out var result))
                return result;
            throw new FormatException("Could not parse integer");
        }

        public double ReadDouble()
        {
            if (double.TryParse(ReadString(), NumberStyles.Any, Culture, out var result))
                return result;
            throw new FormatException("Could not parse double");
        }

        public float ReadFloat()
        {
            if (float.TryParse(ReadString(), NumberStyles.Any, Culture, out var result))
                return result;
            throw new FormatException("Could not parse float");
        }

        public long ReadLong()
        {
            if (long.TryParse(ReadString(), out var result))
                return result;
            throw new FormatException("Could not parse long");
        }

        public short ReadShort()
        {
            if (short.TryParse(ReadString(), out var result))
                return result;
            throw new FormatException("Could not parse short");
        }

        public byte ReadByte()
        {
            if (byte.TryParse(ReadString(), out var result))
                return result;
            throw new FormatException("Could not parse byte");
        }

        public bool ReadBoolean()
        {
            var token = ReadString();
            if (bool.TryParse(token, out var result))
                return result;
            if (token == "1") return true;
            if (token == "0") return false;
            throw new FormatException("Could not parse boolean");
        }

        public string[] ReadAllStrings()
        {
            var text = ReadAll();
            return string.IsNullOrEmpty(text) 
                ? emptyTokens 
                : Regex.Split(text, @"\s+");
        }

        public int[] ReadAllInts()
        {
            return ReadAllStrings()
                .Select(int.Parse)
                .ToArray();
        }

        public double[] ReadAllDoubles()
        {
            return ReadAllStrings()
                .Select(s => double.Parse(s, Culture))
                .ToArray();
        }

        public void Close()
        {
            reader?.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
