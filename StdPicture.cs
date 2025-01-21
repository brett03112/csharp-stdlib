using System;
using System.Drawing;
using System.IO;

namespace csharp_stdlib
{
    /// <summary>
    /// Provides standard methods for working with Picture objects.
    /// This class serves as a utility for creating, manipulating, and displaying
    /// Picture objects in a standardized way. It handles common image operations
    /// while maintaining thread safety and proper error handling.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread-safe and handle null parameters
    /// appropriately by throwing ArgumentNullException.
    /// </remarks>
    public static class StdPicture
    {
        /// <summary>
        /// Creates a new Picture object from an image file.
        /// </summary>
        /// <param name="filename">The path to the image file. Supported formats include
        /// PNG, JPEG, BMP, and GIF.</param>
        /// <returns>A new Picture object containing the loaded image</returns>
        /// <exception cref="ArgumentNullException">Thrown when filename is null or empty</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist</exception>
        /// <exception cref="InvalidOperationException">Thrown when the image cannot be loaded</exception>
        public static Picture CreateFromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found", filename);

            try
            {
                return new Picture(filename);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load image", ex);
            }
        }

        /// <summary>
        /// Creates a new blank Picture with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the picture in pixels. Must be greater than 0.</param>
        /// <param name="height">The height of the picture in pixels. Must be greater than 0.</param>
        /// <returns>A new Picture object with the specified dimensions</returns>
        /// <exception cref="ArgumentException">Thrown when width or height are less than or equal to 0</exception>
        public static Picture Create(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Dimensions must be positive");

            return new Picture(width, height);
        }

        /// <summary>
        /// Saves a Picture object to a file.
        /// </summary>
        /// <param name="picture">The Picture object to save. Cannot be null.</param>
        /// <param name="filename">The path where the image will be saved. Supported formats
        /// include PNG, JPEG, BMP, and GIF.</param>
        /// <exception cref="ArgumentNullException">Thrown when picture or filename are null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the image cannot be saved</exception>
        public static void Save(Picture picture, string filename)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            try
            {
                picture.Save(filename);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save image", ex);
            }
        }

        /// <summary>
        /// Displays a Picture in a new window.
        /// </summary>
        /// <param name="picture">The Picture object to display. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when picture is null</exception>
        /// <remarks>
        /// The window will be automatically sized to fit the picture dimensions.
        /// The window will remain open until closed by the user.
        /// </remarks>
        public static void Show(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            picture.Show();
        }

        /// <summary>
        /// Gets the width of a Picture object in pixels.
        /// </summary>
        /// <param name="picture">The Picture object to measure. Cannot be null.</param>
        /// <returns>The width of the picture in pixels</returns>
        /// <exception cref="ArgumentNullException">Thrown when picture is null</exception>
        public static int GetWidth(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            return picture.Width;
        }

        /// <summary>
        /// Gets the height of a Picture object in pixels.
        /// </summary>
        /// <param name="picture">The Picture object to measure. Cannot be null.</param>
        /// <returns>The height of the picture in pixels</returns>
        /// <exception cref="ArgumentNullException">Thrown when picture is null</exception>
        public static int GetHeight(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            return picture.Height;
        }
    }
}
