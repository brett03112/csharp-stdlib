using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace csharp_stdlib
{
    /// <summary>
    /// The StdDraw class provides a basic capability for creating drawings with your programs.
    /// It uses a simple graphics model that allows you to create drawings consisting of points,
    /// lines, squares, circles, and other geometric shapes in a window on your computer.
    /// </summary>
    public class StdDraw
    {
        /// <summary>Predefined color black (RGB = 0, 0, 0)</summary>
        public static readonly Color BLACK = Color.Black;
        /// <summary>Predefined color blue (RGB = 0, 0, 255)</summary>
        public static readonly Color BLUE = Color.Blue;
        /// <summary>Predefined color cyan (RGB = 0, 255, 255)</summary>
        public static readonly Color CYAN = Color.Cyan;
        /// <summary>Predefined color dark gray (RGB = 64, 64, 64)</summary>
        public static readonly Color DARK_GRAY = Color.DarkGray;
        /// <summary>Predefined color gray (RGB = 128, 128, 128)</summary>
        public static readonly Color GRAY = Color.Gray;
        /// <summary>Predefined color green (RGB = 0, 255, 0)</summary>
        public static readonly Color GREEN = Color.Green;
        /// <summary>Predefined color light gray (RGB = 192, 192, 192)</summary>
        public static readonly Color LIGHT_GRAY = Color.LightGray;
        /// <summary>Predefined color magenta (RGB = 255, 0, 255)</summary>
        public static readonly Color MAGENTA = Color.Magenta;
        /// <summary>Predefined color orange (RGB = 255, 200, 0)</summary>
        public static readonly Color ORANGE = Color.Orange;
        /// <summary>Predefined color pink (RGB = 255, 175, 175)</summary>
        public static readonly Color PINK = Color.Pink;
        /// <summary>Predefined color red (RGB = 255, 0, 0)</summary>
        public static readonly Color RED = Color.Red;
        /// <summary>Predefined color white (RGB = 255, 255, 255)</summary>
        public static readonly Color WHITE = Color.White;
        /// <summary>Predefined color yellow (RGB = 255, 255, 0)</summary>
        public static readonly Color YELLOW = Color.Yellow;
        /// <summary>Predefined transparent color</summary>
        public static readonly Color TRANSPARENT = Color.Transparent;

        // Default settings
        private const int DEFAULT_SIZE = 512;
        private const double DEFAULT_PEN_RADIUS = 0.002;
        private static readonly Font DEFAULT_FONT = new Font("SansSerif", 16);
        private static readonly Color DEFAULT_PEN_COLOR = BLACK;
        private static readonly Color DEFAULT_BACKGROUND_COLOR = WHITE;

        // Current settings
        private static Color penColor = DEFAULT_PEN_COLOR;
        private static Color backgroundColor = DEFAULT_BACKGROUND_COLOR;
        private static double penRadius = DEFAULT_PEN_RADIUS;
        private static Font font = DEFAULT_FONT;
        private static bool defer = false;
        private static Pen currentPen = new Pen(DEFAULT_PEN_COLOR, (float)(DEFAULT_PEN_RADIUS * DEFAULT_SIZE));

        // Drawing surface
        private static Bitmap offscreenImage = new Bitmap(DEFAULT_SIZE, DEFAULT_SIZE);
        private static Graphics offscreen = Graphics.FromImage(offscreenImage);
        private static Form window = new Form();
        private static PictureBox canvas = new PictureBox();
        private static string _text = "Standard Draw";

        public static string Text
        {
            get => _text;
            set 
            {
                _text = value;
                window.Text = value;
            }
        }

        // Coordinate system
        private static double xmin = 0.0;
        private static double xmax = 1.0;
        private static double ymin = 0.0;
        private static double ymax = 1.0;

        // Mouse state
        private static bool isMousePressed = false;
        private static double mouseX = 0;
        private static double mouseY = 0;

        // Keyboard state
        private static List<char> keysTyped = new List<char>();
        private static HashSet<Keys> keysDown = new HashSet<Keys>();

        static StdDraw()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            window = new Form();
            window.Text = "Standard Draw";
            window.Size = new Size(DEFAULT_SIZE, DEFAULT_SIZE);
            window.FormBorderStyle = FormBorderStyle.FixedSingle;
            window.MaximizeBox = false;

            canvas = new PictureBox();
            canvas.Dock = DockStyle.Fill;
            canvas.BackColor = DEFAULT_BACKGROUND_COLOR;
            window.Controls.Add(canvas);

            InitializeCanvas();
            InitializeEventHandlers();
        }

        private static void InitializeCanvas()
        {
            offscreenImage = new Bitmap(canvas.Width, canvas.Height);
            offscreen = Graphics.FromImage(offscreenImage);
            offscreen.SmoothingMode = SmoothingMode.AntiAlias;
            offscreen.Clear(backgroundColor);
        }

        private static void InitializeEventHandlers()
        {
            canvas.MouseDown += (sender, e) => {
                isMousePressed = true;
                UpdateMousePosition(e);
            };

            canvas.MouseUp += (sender, e) => {
                isMousePressed = false;
            };

            canvas.MouseMove += (sender, e) => {
                UpdateMousePosition(e);
            };

            window.KeyDown += (sender, e) => {
                keysDown.Add(e.KeyCode);
            };

            window.KeyUp += (sender, e) => {
                keysDown.Remove(e.KeyCode);
            };

            window.KeyPress += (sender, e) => {
                keysTyped.Add(e.KeyChar);
            };
        }

        private static void UpdateMousePosition(MouseEventArgs e)
        {
            mouseX = userX(e.X);
            mouseY = userY(e.Y);
        }

        // Coordinate scaling methods
        private static double scaleX(double x) => canvas.Width * (x - xmin) / (xmax - xmin);
        private static double scaleY(double y) => canvas.Height * (ymax - y) / (ymax - ymin);
        private static double userX(double x) => xmin + x * (xmax - xmin) / canvas.Width;
        private static double userY(double y) => ymax - y * (ymax - ymin) / canvas.Height;

        public static void SetCanvasSize(int width, int height)
        {
            window.Size = new Size(width, height);
            InitializeCanvas();
        }

        /// <summary>
        /// Sets the x-scale to the specified range.
        /// </summary>
        /// <param name="min">the minimum value of the x-scale</param>
        /// <param name="max">the maximum value of the x-scale</param>
        /// <exception cref="ArgumentException">if min equals max</exception>
        public static void SetXscale(double min, double max)
        {
            if (min == max) throw new ArgumentException("min and max cannot be equal");
            xmin = min;
            xmax = max;
        }

        /// <summary>
        /// Sets the y-scale to the specified range.
        /// </summary>
        /// <param name="min">the minimum value of the y-scale</param>
        /// <param name="max">the maximum value of the y-scale</param>
        /// <exception cref="ArgumentException">if min equals max</exception>
        public static void SetYscale(double min, double max)
        {
            if (min == max) throw new ArgumentException("min and max cannot be equal");
            ymin = min;
            ymax = max;
        }

        /// <summary>
        /// Sets the pen radius to the specified size.
        /// </summary>
        /// <param name="radius">the radius of the pen</param>
        /// <exception cref="ArgumentOutOfRangeException">if radius is negative</exception>
        public static void SetPenRadius(double radius)
        {
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            penRadius = radius;
            currentPen = new Pen(penColor, (float)(radius * DEFAULT_SIZE));
        }

        /// <summary>
        /// Sets the pen color to the specified color.
        /// </summary>
        /// <param name="color">the color to make the pen</param>
        /// <exception cref="ArgumentNullException">if color is null</exception>
        public static void SetPenColor(Color color)
        {
            if (color == null) throw new ArgumentNullException(nameof(color));
            penColor = color;
            currentPen = new Pen(color, (float)(penRadius * DEFAULT_SIZE));
        }

        /// <summary>
        /// Clears the screen to the specified color.
        /// </summary>
        /// <param name="color">the color to make the background</param>
        /// <exception cref="ArgumentNullException">if color is null</exception>
        public static void Clear(Color color)
        {
            if (color == null) throw new ArgumentNullException(nameof(color));
            backgroundColor = color;
            offscreen.Clear(color);
            Draw();
        }

        /// <summary>
        /// Draws a point centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the point</param>
        /// <param name="y">the y-coordinate of the point</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        public static void Point(double x, double y)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            
            float radius = (float)(penRadius * DEFAULT_SIZE);
            offscreen.FillEllipse(new SolidBrush(penColor),
                (float)scaleX(x) - radius/2,
                (float)scaleY(y) - radius/2,
                radius, radius);
            Draw();
        }

        /// <summary>
        /// Draws a line between (x0, y0) and (x1, y1).
        /// </summary>
        /// <param name="x0">the x-coordinate of the starting point</param>
        /// <param name="y0">the y-coordinate of the starting point</param>
        /// <param name="x1">the x-coordinate of the destination point</param>
        /// <param name="y1">the y-coordinate of the destination point</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        public static void Line(double x0, double y0, double x1, double y1)
        {
            if (x0 < xmin || x0 > xmax || x1 < xmin || x1 > xmax) 
                throw new ArgumentException("x-coordinates are outside current scale");
            if (y0 < ymin || y0 > ymax || y1 < ymin || y1 > ymax)
                throw new ArgumentException("y-coordinates are outside current scale");
                
            offscreen.DrawLine(currentPen,
                (float)scaleX(x0), (float)scaleY(y0),
                (float)scaleX(x1), (float)scaleY(y1));
            Draw();
        }

        /// <summary>
        /// Draws a circle of the specified radius, centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the circle</param>
        /// <param name="y">the y-coordinate of the center of the circle</param>
        /// <param name="radius">the radius of the circle</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        /// <exception cref="ArgumentOutOfRangeException">if radius is negative</exception>
        public static void Circle(double x, double y, double radius)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            
            float diameter = (float)(2 * radius);
            offscreen.DrawEllipse(currentPen,
                (float)(scaleX(x) - diameter/2),
                (float)(scaleY(y) - diameter/2),
                diameter, diameter);
            Draw();
        }

        /// <summary>
        /// Draws a filled circle of the specified radius, centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the circle</param>
        /// <param name="y">the y-coordinate of the center of the circle</param>
        /// <param name="radius">the radius of the circle</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        /// <exception cref="ArgumentOutOfRangeException">if radius is negative</exception>
        public static void FilledCircle(double x, double y, double radius)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            
            float diameter = (float)(2 * radius);
            offscreen.FillEllipse(new SolidBrush(penColor),
                (float)(scaleX(x) - diameter/2),
                (float)(scaleY(y) - diameter/2),
                diameter, diameter);
            Draw();
        }

        /// <summary>
        /// Draws a square of the specified radius, centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the square</param>
        /// <param name="y">the y-coordinate of the center of the square</param>
        /// <param name="radius">half the length of any side</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        /// <exception cref="ArgumentOutOfRangeException">if radius is negative</exception>
        public static void Square(double x, double y, double radius)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            
            float size = (float)(2 * radius);
            offscreen.DrawRectangle(currentPen,
                (float)(scaleX(x) - radius),
                (float)(scaleY(y) - radius),
                size, size);
            Draw();
        }

        /// <summary>
        /// Draws a filled square of the specified radius, centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the square</param>
        /// <param name="y">the y-coordinate of the center of the square</param>
        /// <param name="radius">half the length of any side</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        /// <exception cref="ArgumentOutOfRangeException">if radius is negative</exception>
        public static void FilledSquare(double x, double y, double radius)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            
            float size = (float)(2 * radius);
            offscreen.FillRectangle(new SolidBrush(penColor),
                (float)(scaleX(x) - radius),
                (float)(scaleY(y) - radius),
                size, size);
            Draw();
        }

        /// <summary>
        /// Draws the given text string, centered at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the text</param>
        /// <param name="y">the y-coordinate of the center of the text</param>
        /// <param name="text">the text to draw</param>
        /// <exception cref="ArgumentException">if coordinates are outside current scale</exception>
        /// <exception cref="ArgumentNullException">if text is null</exception>
        public static void DrawText(double x, double y, string text)
        {
            if (x < xmin || x > xmax) throw new ArgumentException("x-coordinate is outside current scale");
            if (y < ymin || y > ymax) throw new ArgumentException("y-coordinate is outside current scale");
            if (text == null) throw new ArgumentNullException(nameof(text));
            
            offscreen.DrawString(text, font, new SolidBrush(penColor),
                (float)scaleX(x), (float)scaleY(y));
            Draw();
        }

        /// <summary>
        /// Displays the drawing window and renders any pending graphics.
        /// </summary>
        public static void Show()
        {
            canvas.Image = offscreenImage;
            window.Show();
        }

        /// <summary>
        /// Draws the specified picture in the drawing window.
        /// </summary>
        /// <param name="picture">the picture to draw</param>
        /// <exception cref="ArgumentNullException">if picture is null</exception>
        public static void Picture(Picture picture)
        {
            if (picture == null) throw new ArgumentNullException(nameof(picture));
            offscreen.DrawImage(picture.GetBitmap(), 0, 0);
            Draw();
        }

        /// <summary>
        /// Draws any pending graphics to the screen. If double buffering is enabled,
        /// this will copy the offscreen buffer to the screen.
        /// </summary>
        public static void Draw()
        {
            if (!defer)
            {
                Show();
            }
        }

        /// <summary>
        /// Enables double buffering. All drawing operations will be performed offscreen
        /// and only shown when Draw() is called.
        /// </summary>
        public static void EnableDoubleBuffering()
        {
            defer = true;
        }

        /// <summary>
        /// Disables double buffering. All drawing operations will be immediately
        /// visible on the screen.
        /// </summary>
        public static void DisableDoubleBuffering()
        {
            defer = false;
        }

        /// <summary>
        /// Returns true if the mouse is currently being pressed.
        /// </summary>
        /// <returns>true if the mouse is being pressed, false otherwise</returns>
        public static bool IsMousePressed() => isMousePressed;

        /// <summary>
        /// Returns the x-coordinate of the mouse in user coordinates.
        /// </summary>
        /// <returns>the x-coordinate of the mouse</returns>
        public static double MouseX() => mouseX;

        /// <summary>
        /// Returns the y-coordinate of the mouse in user coordinates.
        /// </summary>
        /// <returns>the y-coordinate of the mouse</returns>
        public static double MouseY() => mouseY;

        /// <summary>
        /// Returns true if the user has typed a key that has not yet been processed.
        /// </summary>
        /// <returns>true if the user has typed a key, false otherwise</returns>
        public static bool HasNextKeyTyped() => keysTyped.Count > 0;

        /// <summary>
        /// Returns the next key that was typed by the user.
        /// </summary>
        /// <returns>the next key typed</returns>
        /// <exception cref="InvalidOperationException">if no keys are available</exception>
        public static char NextKeyTyped()
        {
            if (keysTyped.Count == 0)
                throw new InvalidOperationException("No keys available");
            
            char key = keysTyped[0];
            keysTyped.RemoveAt(0);
            return key;
        }

        /// <summary>
        /// Returns true if the specified key is currently being pressed.
        /// </summary>
        /// <param name="keyCode">the key to check</param>
        /// <returns>true if the key is pressed, false otherwise</returns>
        public static bool IsKeyPressed(Keys keyCode) => keysDown.Contains(keyCode);

    }
}
