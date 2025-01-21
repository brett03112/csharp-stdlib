using System;
using System.Drawing;
using csharp_stdlib;

namespace csharp_stdlib
{
    public class GrayscalePicture : Picture
    {
        public GrayscalePicture(int width, int height) : base(width, height)
        {
        }

        public GrayscalePicture(string filename) : base(filename)
        {
            ConvertToGrayscale();
        }

        public GrayscalePicture(Picture picture) : base(picture)
        {
            ConvertToGrayscale();
        }

        private void ConvertToGrayscale()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Color color = GetPixel(x, y);
                    int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
        }

        public override void SetPixel(int x, int y, Color color)
        {
            int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            base.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
        }

        public override Picture Clone()
        {
            return new GrayscalePicture(this);
        }
    }
}
