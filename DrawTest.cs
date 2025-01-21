using System;
using System.Drawing;
using StdLib;

namespace DrawTests
{
    class DrawTest
    {
        public void RunTests()
        {
            // Create a Draw instance
            var draw = new Draw(800, 600);
            
            // Set scale and pen color
            draw.SetPenColor(Color.Blue);
            
            // Draw a line
            draw.DrawLine(10, 10, 90, 90);
            
            // Draw a filled square
            draw.SetPenColor(Color.Red);
            draw.DrawLine(30, 30, 70, 70);
            draw.DrawLine(70, 30, 30, 70);
            
            // Draw text
            draw.SetPenColor(Color.Black);
            // Text drawing not implemented in Draw.cs yet
            
            // Draw a circle
            draw.SetPenColor(Color.Green);
            draw.DrawPoint(75, 25);
            
            // Show for 5 seconds
            draw.Show(5000);
        }
    }
}
