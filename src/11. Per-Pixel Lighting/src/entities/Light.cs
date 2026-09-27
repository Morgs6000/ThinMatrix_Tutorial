using System.Numerics;

namespace ThinMatrix_Tutorial.src.entities;

public class Light
{
    private Vector3 _position;
    private Vector3 _color;

    public Light(Vector3 position, Vector3 color)
    {
        _position = position;
        _color = color;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public void SetPosition(Vector3 position)
    {
        _position = position;
    }

    public Vector3 GetColor()
    {
        return _color;
    }

    public void SetColor(Vector3 color)
    {
        _color = color;
    }
}
