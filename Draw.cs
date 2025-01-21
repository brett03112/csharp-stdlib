using System;
using System.Drawing;
using System.Windows.Forms;

namespace StdLib
{
    public class Draw : Form
    {
        private static readonly int DEFAULT_SIZE = 512;
        private static readonly Color DEFAULT_PEN_COLOR = Color.Black;
        private static readonly Color DEFAULT_CLEAR_COLOR = Color.White;

        private Bitmap _offscreenImage;
        private Graphics _offscreen;
        private PictureBox _pictureBox;
        private Color _penColor;
        private System.Windows.Forms.Timer _timer;

        public Draw() : this(DEFAULT_SIZE, DEFAULT_SIZE) { }

        private DrawMouseListener _mouseListener;

        public Draw(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive");

            // Set up mouse listener
            _mouseListener = new DrawMouseListener(this);
            
            // Set up the form
            this.Text = "Draw";
            this.ClientSize = new Size(width, height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Set up the picture box
            _pictureBox = new PictureBox
            {
                Size = this.ClientSize,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(_pictureBox);

            // Set up offscreen image
            _offscreenImage = new Bitmap(width, height);
            _offscreen = Graphics.FromImage(_offscreenImage);
            Clear();

            // Set up timer for animation
            _timer = new System.Windows.Forms.Timer { Interval = 100 };
            _timer.Tick += OnTimerTick;

            // Set default pen color
            _penColor = DEFAULT_PEN_COLOR;

            // Handle paint event
            _pictureBox.Paint += OnPaint;

            // Handle mouse events
            _pictureBox.MouseDown += OnMouseDown;
            _pictureBox.MouseMove += OnMouseMove;
            _pictureBox.MouseUp += OnMouseUp;
        }

        // Mouse event handlers
        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            _mouseListener.MousePressed(e.X, e.Y);
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseListener.MouseDragged(e.X, e.Y);
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            _mouseListener.MouseReleased(e.X, e.Y);
        }

        // Methods called by DrawMouseListener
        private double _penRadius = 1.0;
        private PointF? _lastMousePosition;

        public void MousePressed(double x, double y)
        {
            _lastMousePosition = new PointF((float)x, (float)y);
            DrawPoint(x, y);
        }

        public void MouseDragged(double x, double y)
        {
            if (_lastMousePosition.HasValue)
            {
                DrawLine(_lastMousePosition.Value.X, _lastMousePosition.Value.Y, x, y);
            }
            _lastMousePosition = new PointF((float)x, (float)y);
        }

        public void MouseReleased(double x, double y)
        {
            _lastMousePosition = null;
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_offscreenImage, Point.Empty);
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _pictureBox.Invalidate();
        }

        public void Clear()
        {
            _offscreen.Clear(DEFAULT_CLEAR_COLOR);
            _pictureBox.Invalidate();
        }

        public void SetPenColor(Color color)
        {
            _penColor = color;
        }

        public void SetPenRadius(double radius)
        {
            if (radius < 0) throw new ArgumentException("Pen radius must be non-negative");
            _penRadius = radius;
        }

        public void DrawPoint(double x, double y)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
                throw new ArgumentException("Coordinates must be finite numbers");

            using (var pen = new Pen(_penColor, (float)_penRadius))
            {
                float radius = (float)_penRadius;
                _offscreen.DrawEllipse(pen, (float)x - radius/2, (float)y - radius/2, radius, radius);
            }
            _pictureBox.Invalidate();
        }

        public void DrawLine(double x0, double y0, double x1, double y1)
        {
            if (double.IsNaN(x0) || double.IsNaN(y0) || double.IsNaN(x1) || double.IsNaN(y1))
                throw new ArgumentException("Coordinates must be finite numbers");

            using (var pen = new Pen(_penColor))
            {
                _offscreen.DrawLine(pen, (float)x0, (float)y0, (float)x1, (float)y1);
            }
            _pictureBox.Invalidate();
        }

        public void Show(int t)
        {
            _timer.Interval = t;
            _timer.Start();
            Application.Run(this);
        }

        public new void Show()
        {
            Show(0);
        }

        public void Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Invalid filename");

            try
            {
                _offscreenImage.Save(filename);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error saving image: {e.Message}");
            }
        }
    }
}
