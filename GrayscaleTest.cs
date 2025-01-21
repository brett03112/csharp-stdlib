using System;
using csharp_stdlib;
using System.Drawing;

namespace GrayscaleTests
{
    class GrayscaleTest
    {
        public void RunTests(string filename)
        {
            try
            {
                // Create grayscale picture from file
                var picture = new GrayscalePicture(filename);
                
                // Display image information
                Console.WriteLine($"Image size: {picture.Width} x {picture.Height}");
                
                // Show the grayscale image
            StdDraw.Picture(picture);
            StdDraw.Show();
                
                // Save a copy
                string outputFilename = System.IO.Path.GetFileNameWithoutExtension(filename) + "_grayscale.png";
                picture.Save(outputFilename);
                Console.WriteLine($"Saved grayscale version to {outputFilename}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing image: {e.Message}");
            }
        }
    }
}
