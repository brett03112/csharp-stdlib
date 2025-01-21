namespace StdLib;

internal class DrawMouseListener : DrawListener
{
    private readonly Draw _draw;

    public DrawMouseListener(Draw draw)
    {
        _draw = draw;
    }

    public void MousePressed(double x, double y)
    {
        _draw.MousePressed(x, y);
    }

    public void MouseDragged(double x, double y)
    {
        _draw.MouseDragged(x, y);
    }

    public void MouseReleased(double x, double y)
    {
        _draw.MouseReleased(x, y);
    }

    public void KeyTyped(char c) { }
    public void KeyPressed(int keycode) { }
    public void KeyReleased(int keycode) { }
}
