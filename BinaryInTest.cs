using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;

namespace csharp_stdlib.Tests
{
    public class BinaryInTests
    {
        [Fact]
        public void TestDefaultConstructor()
        {
            using var bin = new BinaryIn();
            Assert.NotNull(bin);
        }

        [Fact]
        public void TestStreamConstructor()
        {
            using var memStream = new MemoryStream(new byte[] { 0x01, 0x02 });
            using var bin = new BinaryIn(memStream);
            Assert.False(bin.IsEmpty());
        }

        [Fact]
        public void TestSocketConstructor()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            using var bin = new BinaryIn(socket);
            Assert.NotNull(bin);
        }

        [Fact]
        public async Task TestFromUrlAsync()
        {
            var url = new Uri("https://www.example.com");
            using var bin = await BinaryIn.FromUrlAsync(url);
            Assert.NotNull(bin);
        }

        [Fact]
        public async Task TestFromFileOrUrlAsync_WithFile()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllBytes(tempFile, new byte[] { 0x01, 0x02 });
                
                using (var bin = await BinaryIn.FromFileOrUrlAsync(tempFile))
                {
                    Assert.False(bin.IsEmpty());
                    Assert.Equal(0x01, bin.ReadByte());
                    Assert.Equal(0x02, bin.ReadByte());
                }
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void TestReadBoolean()
        {
            using var memStream = new MemoryStream(new byte[] { 0b10101010 });
            using var bin = new BinaryIn(memStream);
            
            Assert.True(bin.ReadBoolean());
            Assert.False(bin.ReadBoolean());
            Assert.True(bin.ReadBoolean());
            Assert.False(bin.ReadBoolean());
        }

        [Fact]
        public void TestReadChar()
        {
            using var memStream = new MemoryStream(new byte[] { 0x41, 0x42 }); // 'A', 'B'
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal('A', bin.ReadChar());
            Assert.Equal('B', bin.ReadChar());
        }

        [Fact]
        public void TestReadString()
        {
            var text = "Hello World";
            using var memStream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(text));
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(text, bin.ReadString());
        }

        [Fact]
        public void TestReadShort()
        {
            using var memStream = new MemoryStream(new byte[] { 0x12, 0x34 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(0x1234, bin.ReadShort());
        }

        [Fact]
        public void TestReadInt()
        {
            using var memStream = new MemoryStream(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(0x12345678, bin.ReadInt());
        }

        [Fact]
        public void TestReadLong()
        {
            using var memStream = new MemoryStream(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(0x123456789ABCDEF0, bin.ReadLong());
        }

        [Fact]
        public void TestReadDouble()
        {
            var value = 3.14159;
            var bytes = BitConverter.GetBytes(value);
            using var memStream = new MemoryStream(bytes);
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(value, bin.ReadDouble());
        }

        [Fact]
        public void TestReadFloat()
        {
            var value = 3.14f;
            var bytes = BitConverter.GetBytes(value);
            using var memStream = new MemoryStream(bytes);
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(value, bin.ReadFloat());
        }

        [Fact]
        public void TestReadByte()
        {
            using var memStream = new MemoryStream(new byte[] { 0x42 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Equal(0x42, bin.ReadByte());
        }

        [Fact]
        public void TestIsEmpty()
        {
            using var memStream = new MemoryStream(new byte[0]);
            using var bin = new BinaryIn(memStream);
            
            Assert.True(bin.IsEmpty());
        }

        [Fact]
        public void TestExists()
        {
            using var memStream = new MemoryStream(new byte[0]);
            using var bin = new BinaryIn(memStream);
            
            Assert.True(bin.Exists());
        }

        [Fact]
        public void TestReadFromEmptyStream()
        {
            using var memStream = new MemoryStream(new byte[0]);
            using var bin = new BinaryIn(memStream);
            
            Assert.Throws<InvalidOperationException>(() => bin.ReadBoolean());
        }

        [Fact]
        public void TestInvalidCharBits()
        {
            using var memStream = new MemoryStream(new byte[] { 0x01 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Throws<ArgumentException>(() => bin.ReadChar(17));
        }

        [Fact]
        public void TestInvalidIntBits()
        {
            using var memStream = new MemoryStream(new byte[] { 0x01 });
            using var bin = new BinaryIn(memStream);
            
            Assert.Throws<ArgumentException>(() => bin.ReadInt(33));
        }
    }
}
