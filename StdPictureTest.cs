using System;
using System.IO;
using Xunit;

namespace csharp_stdlib
{
    public class StdPictureTests : IDisposable
    {
        private const string TestImagePath = "test_image.png";
        private const string TempOutputPath = "test_output.png";
        private bool _disposed = false;

        public StdPictureTests()
        {
            // Create a small test image
            using (var bitmap = new System.Drawing.Bitmap(10, 10))
            {
                bitmap.Save(TestImagePath);
            }
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
                    // Clean up test files with retry logic
                    TryDeleteFile(TestImagePath);
                    TryDeleteFile(TempOutputPath);
                }
                _disposed = true;
            }
        }

        private void TryDeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException)
                {
                    // Ignore if file is locked
                }
            }
        }

        [Fact]
        public void TestCreateFromFile()
        {
            using (var picture = StdPicture.CreateFromFile(TestImagePath))
            {
                Assert.NotNull(picture);
                Assert.Equal(10, picture.Width);
                Assert.Equal(10, picture.Height);
            }
        }

        [Fact]
        public void TestCreateFromFile_InvalidPath()
        {
            Assert.Throws<FileNotFoundException>(() => StdPicture.CreateFromFile("nonexistent.png"));
        }

        [Fact]
        public void TestCreate()
        {
            using (var picture = StdPicture.Create(100, 50))
            {
                Assert.NotNull(picture);
                Assert.Equal(100, picture.Width);
                Assert.Equal(50, picture.Height);
            }
        }

        [Fact]
        public void TestCreate_InvalidDimensions()
        {
            Assert.Throws<ArgumentException>(() => StdPicture.Create(0, 10));
            Assert.Throws<ArgumentException>(() => StdPicture.Create(10, 0));
            Assert.Throws<ArgumentException>(() => StdPicture.Create(-1, 10));
        }

        [Fact]
        public void TestSave()
        {
            using (var picture = StdPicture.Create(10, 10))
            {
                StdPicture.Save(picture, TempOutputPath);
                Assert.True(File.Exists(TempOutputPath));
            }
        }

        [Fact]
        public void TestSave_NullPicture()
        {
            Assert.Throws<ArgumentNullException>(() => StdPicture.Save(null, TempOutputPath));
        }

        [Fact]
        public void TestSave_InvalidPath()
        {
            using (var picture = StdPicture.Create(10, 10))
            {
                Assert.Throws<ArgumentNullException>(() => StdPicture.Save(picture, null));
                Assert.Throws<ArgumentNullException>(() => StdPicture.Save(picture, ""));
            }
        }

        [Fact]
        public void TestShow()
        {
            using (var picture = StdPicture.Create(10, 10))
            {
                // Can't easily test window display, but we can verify it doesn't throw
                StdPicture.Show(picture);
            }
        }

        [Fact]
        public void TestShow_NullPicture()
        {
            Assert.Throws<ArgumentNullException>(() => StdPicture.Show(null));
        }

        [Fact]
        public void TestGetWidth()
        {
            using (var picture = StdPicture.Create(100, 50))
            {
                Assert.Equal(100, StdPicture.GetWidth(picture));
            }
        }

        [Fact]
        public void TestGetWidth_NullPicture()
        {
            Assert.Throws<ArgumentNullException>(() => StdPicture.GetWidth(null));
        }

        [Fact]
        public void TestGetHeight()
        {
            using (var picture = StdPicture.Create(100, 50))
            {
                Assert.Equal(50, StdPicture.GetHeight(picture));
            }
        }

        [Fact]
        public void TestGetHeight_NullPicture()
        {
            Assert.Throws<ArgumentNullException>(() => StdPicture.GetHeight(null));
        }
    }
}
