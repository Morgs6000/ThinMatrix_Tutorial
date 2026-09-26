using System.Numerics;
using ThinMatrix_Tutorial.src.models;

namespace ThinMatrix_Tutorial.src.entities;

public class Entitiy
{
    private TexturedModel _model;
    private Vector3 _position;
    private float _rotX, _rotY, _rotZ;
    private float _scale;

    public Entitiy(TexturedModel model, Vector3 position, float rotX, float rotY, float rotZ, float scale)
    {
        _model = model;

        _position = position;

        _rotX = rotX;
        _rotY = rotY;
        _rotZ = rotZ;

        _scale = scale;
    }

    public void IncreasePosition(float dx, float dy, float dz)
    {
        _position.X += dx;
        _position.Y += dy;
        _position.Z += dz;
    }

    public void IncreaseRotation(float dx, float dy, float dz)
    {
        _rotX += dx;
        _rotY += dy;
        _rotZ += dz;
    }

    public TexturedModel GetModel()
    {
        return _model;
    }

    public void SetModel(TexturedModel model)
    {
        _model = model;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public void SetPosition(Vector3 position)
    {
        _position = position;
    }

    public float GetRotX()
    {
        return _rotX;
    }

    public void SetRotX(float rotX)
    {
        _rotX = rotX;
    }

    public float GetRotY()
    {
        return _rotY;
    }

    public void SetRotY(float rotY)
    {
        _rotY = rotY;
    }

    public float GetRotZ()
    {
        return _rotZ;
    }

    public void SetRotZ(float rotZ)
    {
        _rotZ = rotZ;
    }

    public float GetScale()
    {
        return _scale;
    }

    public void SetScale(float scale)
    {
        _scale = scale;
    }
}
