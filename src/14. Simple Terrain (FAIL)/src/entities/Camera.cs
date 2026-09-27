using System.Numerics;
using Silk.NET.GLFW;

namespace ThinMatrix_Tutorial.src.entities;

public class Camera
{
    private Vector3 _position = new Vector3(0.0f, 0.0f, 0.0f);
    private float _pitch;
    private float _yaw;
    private float _roll;

    public Camera()
    {
        
    }

    public void Move()
    {
        if (Program.IsKeyDown(Keys.W))
        {
            _position.Z -= 0.2f;
        }
        if (Program.IsKeyDown(Keys.S))
        {
            _position.Z += 0.2f;
        }
        if (Program.IsKeyDown(Keys.D))
        {
            _position.X += 0.2f;
        }
        if (Program.IsKeyDown(Keys.A))
        {
            _position.X -= 0.2f;
        }
        if (Program.IsKeyDown(Keys.Space))
        {
            _position.Y += 0.2f;
        }
        if (Program.IsKeyDown(Keys.ShiftLeft))
        {
            _position.Y -= 0.2f;
        }
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public float GetPitch()
    {
        return _pitch;
    }

    public float GetYaw()
    {
        return _yaw;
    }

    public float GetRoll()
    {
        return _roll;
    }
}
