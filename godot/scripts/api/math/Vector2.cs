namespace FrogCreator.Api.Math;

public class Vector2<T>
{
    private T _x, _y;

    public Vector2(T x, T y) { _x = x; _y = y; }

    public void SetX(T value) { _x = value; }
    public void SetY(T value) { _y = value; }
    public T GetX() { return _x; }
    public T GetY() { return _y; }
}
