using System;
using Xunit;
using csharp_stdlib;

namespace csharp_stdlib.Tests
{
    public class StdDrawTest : IDisposable
    {
        public void Dispose()
        {
            StdDraw.Clear(StdDraw.WHITE);
        }

        
        [Fact]
        public void TestCanvasSetup()
        {
            StdDraw.SetCanvasSize(800, 600);
            StdDraw.SetPenRadius(0.05);
            
            StdDraw.SetPenColor(StdDraw.BLUE);
            StdDraw.Point(0.5, 0.5);
            
            StdDraw.SetPenColor(StdDraw.RED);
            StdDraw.Line(0.2, 0.2, 0.8, 0.8);
        }

        [Fact]
        public void TestTextDrawing()
        {
            StdDraw.SetPenColor(StdDraw.BLACK);
            StdDraw.DrawText(0.5, 0.95, "StdDraw Test");
        }

        [Fact]
        public void TestShapes()
        {
            StdDraw.SetPenColor(StdDraw.GREEN);
            StdDraw.Circle(0.3, 0.7, 0.2);
            
            StdDraw.SetPenColor(StdDraw.ORANGE);
            StdDraw.Square(0.7, 0.3, 0.2);
        }

        [Fact]
        public void TestFilledShapes()
        {
            StdDraw.SetPenColor(StdDraw.PINK);
            StdDraw.FilledCircle(0.3, 0.3, 0.2);
            
            StdDraw.SetPenColor(StdDraw.CYAN);
            StdDraw.FilledSquare(0.7, 0.7, 0.2);
        }
        
    }
}
