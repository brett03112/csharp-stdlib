namespace StdLib;

public interface DrawListener
{
    void MousePressed(double x, double y);
    void MouseDragged(double x, double y);
    void MouseReleased(double x, double y);
    void KeyTyped(char c);
    void KeyPressed(int keycode);
    void KeyReleased(int keycode);
}
