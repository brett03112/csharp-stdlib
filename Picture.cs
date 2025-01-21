using System;
using System.Drawing;
using System.Windows.Forms;

namespace csharp_stdlib
{
    /// <summary>
    /// Represents a picture composed of pixels with color values.
    /// Implements IDisposable to properly release resources.
    /// </summary>
    public class Picture : IDisposable
    {
        protected readonly Bitmap bitmap;

        /// <summary>
        /// Gets the width of the picture in pixels.
        /// </summary>
        public int Width => bitmap.Width;

        /// <summary>
        /// Gets the height of the picture in pixels.
        /// </summary>
        public int Height => bitmap.Height;

        /// <summary>
        /// Initializes a new blank picture with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the picture in pixels.</param>
        /// <param name="height">The height of the picture in pixels.</param>
        public Picture(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        /// <summary>
        /// Initializes a new picture by loading from a file.
        /// </summary>
        /// <param name="filename">The path to the image file to load.</param>
        public Picture(string filename)
        {
            bitmap = new Bitmap(filename);
        }

        /// <summary>
        /// Initializes a new picture from an existing Bitmap.
        /// </summary>
        /// <param name="bitmap">The Bitmap to create the picture from.</param>
        public Picture(Bitmap bitmap)
        {
            this.bitmap = new Bitmap(bitmap);
        }

        /// <summary>
        /// Initializes a new picture by copying an existing Picture.
        /// </summary>
        /// <param name="picture">The Picture to copy.</param>
        protected Picture(Picture picture)
        {
            bitmap = new Bitmap(picture.bitmap);
        }

        /// <summary>
        /// Gets the color of the pixel at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <returns>The color of the pixel at (x, y).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if x or y are out of bounds.</exception>
        public Color GetPixel(int x, int y)
        {
            Validate(x, y);
            return bitmap.GetPixel(x, y);
        }

        /// <summary>
        /// Sets the color of the pixel at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <param name="color">The color to set the pixel to.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if x or y are out of bounds.</exception>
        public virtual void SetPixel(int x, int y, Color color)
        {
            Validate(x, y);
            bitmap.SetPixel(x, y, color);
        }

        /// <summary>
        /// Creates a deep copy of the picture.
        /// </summary>
        /// <returns>A new Picture that is a copy of this picture.</returns>
        public virtual Picture Clone()
        {
            return new Picture(this);
        }

        /// <summary>
        /// Saves the picture to a file.
        /// </summary>
        /// <param name="filename">The path to save the picture to.</param>
        public virtual void Save(string filename)
        {
            bitmap.Save(filename);
        }

        /// <summary>
        /// Validates that the given coordinates are within the picture bounds.
        /// </summary>
        /// <param name="x">The x-coordinate to validate.</param>
        /// <param name="y">The y-coordinate to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if x or y are out of bounds.</exception>
        protected void Validate(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), "X coordinate out of bounds");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), "Y coordinate out of bounds");
        }

        /// <summary>
        /// Returns a string representation of the picture.
        /// </summary>
        /// <returns>A string containing the picture's dimensions.</returns>
        public override string ToString()
        {
            return $"Picture (width = {Width}, height = {Height})";
        }

        /// <summary>
        /// Displays the picture in a new window.
        /// </summary>
        public void Show()
        {
            using (var form = new Form())
            {
                form.Text = "Picture Viewer";
                form.ClientSize = new Size(Width, Height);
                form.BackgroundImage = bitmap;
                form.BackgroundImageLayout = ImageLayout.Stretch;
                Application.Run(form);
            }
        }

        private bool disposed = false;

        /// <summary>
        /// Releases the unmanaged resources used by the Picture and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    bitmap?.Dispose();
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the Picture.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer for the Picture class.
        /// </summary>
        ~Picture()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the underlying Bitmap of the picture.
        /// </summary>
        /// <returns>The Bitmap representing the picture.</returns>
        public Bitmap GetBitmap()
        {
            return bitmap;
        }
    }
}

