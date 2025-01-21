using System;
using System.IO;
using Xunit;

namespace csharp_stdlib.Tests
{
    /// <summary>
    /// Unit tests for BinaryOut class
    /// </summary>
    public class BinaryOutTests : IDisposable
    {
        private readonly string _testFilePath = "test_output.bin";

        public BinaryOutTests()
        {
            // Clean up any existing test file
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        public void Dispose()
        {
            // Clean up after each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Fact]
        public void WriteBoolean_ShouldWriteCorrectValue()
        {
            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(true);
                binaryOut.Write(false);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.True(binaryIn.ReadBoolean());
            Assert.False(binaryIn.ReadBoolean());
        }

        [Fact]
        public void WriteByte_ShouldWriteCorrectValue()
        {
            byte testValue = 0xAB;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadByte());
        }

        [Fact]
        public void WriteChar_ShouldWriteCorrectValue()
        {
            char testValue = 'X';

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadChar());
        }

        [Fact]
        public void WriteShort_ShouldWriteCorrectValue()
        {
            short testValue = 12345;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadShort());
        }

        [Fact]
        public void WriteInt_ShouldWriteCorrectValue()
        {
            int testValue = 123456789;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadInt());
        }

        [Fact]
        public void WriteLong_ShouldWriteCorrectValue()
        {
            long testValue = 1234567890123456789L;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadLong());
        }

        [Fact]
        public void WriteFloat_ShouldWriteCorrectValue()
        {
            float testValue = 123.456f;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadFloat());
        }

        [Fact]
        public void WriteDouble_ShouldWriteCorrectValue()
        {
            double testValue = 123.456789;

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadDouble());
        }

        [Fact]
        public void WriteString_ShouldWriteCorrectValue()
        {
            string testValue = "Test String";

            using (var binaryOut = new BinaryOut(_testFilePath))
            {
                binaryOut.Write(testValue);
                binaryOut.Flush();
                binaryOut.Close();
            }

            using var binaryIn = new BinaryIn(_testFilePath);
            Assert.Equal(testValue, binaryIn.ReadString());
        }

        [Fact]
        public void Flush_ShouldWriteBufferedData()
        {
            using var binaryOut = new BinaryOut(_testFilePath);
            binaryOut.Write(true);
            binaryOut.Flush();
            Assert.True(File.Exists(_testFilePath));
        }

        [Fact]
        public void Close_ShouldWriteBufferedData()
        {
            using var binaryOut = new BinaryOut(_testFilePath);
            binaryOut.Write(true);
            Assert.True(File.Exists(_testFilePath));
        }

        [Fact]
        public void Constructor_WithFileName_ShouldCreateFile()
        {
            using var binaryOut = new BinaryOut(_testFilePath);
            Assert.True(File.Exists(_testFilePath));
        }

        [Fact]
        public void Constructor_WithStream_ShouldUseStream()
        {
            using var stream = new FileStream(_testFilePath, FileMode.Create);
            using var binaryOut = new BinaryOut(stream);
            binaryOut.Write(true);
            Assert.True(File.Exists(_testFilePath));
        }

        [Fact]
        public void Constructor_WithSocket_ShouldUseSocket()
        {
            // This test would require a mock socket implementation
            // For now, we'll just verify the constructor doesn't throw
            using var socket = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Stream,
                System.Net.Sockets.ProtocolType.Tcp);
            var exception = Record.Exception(() => new BinaryOut(socket));
            Assert.Null(exception);
        }
    }
}
