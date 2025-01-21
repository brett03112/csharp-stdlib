using System;
using System.IO;
using Xunit;

namespace csharp_stdlib
{
    public class StdInTests
    {
        [Fact]
        public void TestReadShort()
        {
            StdIn.SetInput("32767 -32768");
            Assert.Equal(32767, StdIn.ReadShort());
            Assert.Equal(-32768, StdIn.ReadShort());
        }

        [Fact]
        public void TestReadFloat()
        {
            StdIn.SetInput("3.14 -2.71");
            Assert.Equal(3.14f, StdIn.ReadFloat());
            Assert.Equal(-2.71f, StdIn.ReadFloat());
        }

        [Fact]
        public void TestReadLong()
        {
            StdIn.SetInput("9223372036854775807 -9223372036854775808");
            Assert.Equal(9223372036854775807L, StdIn.ReadLong());
            Assert.Equal(-9223372036854775808L, StdIn.ReadLong());
        }

        [Fact]
        public void TestMixedInputTypes()
        {
            StdIn.SetInput("true 42 3.14 hello");
            Assert.True(StdIn.ReadBoolean());
            Assert.Equal(42, StdIn.ReadInt());
            Assert.Equal(3.14, StdIn.ReadDouble());
            Assert.Equal("hello", StdIn.ReadString());
        }

        [Fact]
        public void TestWhitespaceHandling()
        {
            StdIn.SetInput("  \t\n 42  \n \t 3.14 \n ");
            Assert.Equal(42, StdIn.ReadInt());
            Assert.Equal(3.14, StdIn.ReadDouble());
        }

        [Fact]
        public void TestCultureInvariantNumbers()
        {
            // Should work regardless of system culture settings
            StdIn.SetInput("1.234,56"); // German-style number
            var ex = Assert.Throws<FormatException>(() => StdIn.ReadDouble());
            Assert.Contains("correct format for a double", ex.Message);
            
            // Test valid invariant format
            StdIn.SetInput("1234.56");
            Assert.Equal(1234.56, StdIn.ReadDouble());
        }

        [Fact]
        public void TestNumberFormatExceptions()
        {
            StdIn.SetInput("not_a_number");
            var formatEx = Assert.Throws<FormatException>(() => StdIn.ReadInt());
            Assert.Contains("correct format for an integer", formatEx.Message);
            
            StdIn.SetInput("999999999999999999999999999999");
            var overflowEx = Assert.Throws<OverflowException>(() => StdIn.ReadInt());
            Assert.Contains("too large or too small for an integer", overflowEx.Message);
        }

        [Fact]
        public void TestBooleanFormatExceptions()
        {
            StdIn.SetInput("yes");
            var ex = Assert.Throws<FormatException>(() => StdIn.ReadBoolean());
            Assert.Contains("'true' or 'false'", ex.Message);
        }

        [Fact]
        public void TestEmptyInputExceptions()
        {
            StdIn.SetInput("");
            var ex = Assert.Throws<InvalidOperationException>(() => StdIn.ReadInt());
            Assert.Contains("No more tokens available", ex.Message);
        }

        [Fact]
        public void TestReadString()
        {
            StdIn.SetInput("hello world");
            Assert.Equal("hello", StdIn.ReadString());
            Assert.Equal("world", StdIn.ReadString());
        }

        [Fact]
        public void TestReadInt()
        {
            StdIn.SetInput("123 456");
            Assert.Equal(123, StdIn.ReadInt());
            Assert.Equal(456, StdIn.ReadInt());
        }

        [Fact]
        public void TestReadDouble()
        {
            StdIn.SetInput("3.14 2.71");
            Assert.Equal(3.14, StdIn.ReadDouble());
            Assert.Equal(2.71, StdIn.ReadDouble());
        }

        [Fact]
        public void TestReadBoolean()
        {
            StdIn.SetInput("true false");
            Assert.True(StdIn.ReadBoolean());
            Assert.False(StdIn.ReadBoolean());
        }

        [Fact]
        public void TestReadLine()
        {
            StdIn.SetInput("line1\nline2");
            Assert.Equal("line1", StdIn.ReadLine());
            Assert.Equal("line2", StdIn.ReadLine());
        }

        [Fact]
        public void TestReadAll()
        {
            StdIn.SetInput("line1\nline2");
            Assert.Equal("line1\nline2", StdIn.ReadAll());
        }

        [Fact]
        public void TestReadAllStrings()
        {
            StdIn.SetInput("a b c\n1 2 3");
            var result = StdIn.ReadAllStrings();
            Assert.Equal(new[] { "a", "b", "c", "1", "2", "3" }, result);
        }

        [Fact]
        public void TestReadAllInts()
        {
            StdIn.SetInput("1 2 3\n4 5 6");
            var result = StdIn.ReadAllInts();
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Fact]
        public void TestReadAllDoubles()
        {
            StdIn.SetInput("1.1 2.2 3.3\n4.4 5.5 6.6");
            var result = StdIn.ReadAllDoubles();
            Assert.Equal(new[] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6 }, result);
        }

        [Fact]
        public void TestReadChar()
        {
            StdIn.SetInput(" a b c ");
            Assert.Equal('a', StdIn.ReadChar());
            Assert.Equal('b', StdIn.ReadChar());
            Assert.Equal('c', StdIn.ReadChar());
        }

        [Fact]
        public void TestReadByte()
        {
            StdIn.SetInput("1 255");
            Assert.Equal(1, StdIn.ReadByte());
            Assert.Equal(255, StdIn.ReadByte());
        }

        [Fact]
        public void TestEmptyInput()
        {
            StdIn.SetInput("");
            Assert.True(StdIn.IsEmpty);
            Assert.False(StdIn.HasNext());
            Assert.Throws<InvalidOperationException>(() => StdIn.ReadString());
        }

        [Fact]
        public void TestReset()
        {
            StdIn.SetInput("test");
            StdIn.Reset();
            Assert.True(StdIn.IsEmpty);
        }

        [Fact]
        public void TestSetInputFromStringReader()
        {
            using var reader = new StringReader("test input");
            StdIn.SetInput(reader);
            Assert.Equal("test", StdIn.ReadString());
            Assert.Equal("input", StdIn.ReadString());
        }

        [Fact]
        public void TestInvalidNumberFormat()
        {
            StdIn.SetInput("not_a_number");
            Assert.Throws<FormatException>(() => StdIn.ReadInt());
        }
    }
}
