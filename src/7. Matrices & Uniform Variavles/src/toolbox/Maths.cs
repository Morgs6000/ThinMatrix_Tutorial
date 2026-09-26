using System.Numerics;

namespace ThinMatrix_Tutorial.src.toolbox;

public class Maths
{
    public static Matrix4x4 CreateTransformationMatrix(Vector3 translation, float rx, float ry, float rz, float scale)
    {
        Matrix4x4 matrix = Matrix4x4.Identity;

        matrix *= Matrix4x4.CreateScale(new Vector3(scale, scale, scale));

        matrix *= Matrix4x4.CreateFromAxisAngle(
            Vector3.Normalize(new Vector3(0.0f, 0.0f, 1.0f)), 
            MathHelper.DegreesToRadians(rz)
        );
        matrix *= Matrix4x4.CreateFromAxisAngle(
            Vector3.Normalize(new Vector3(0.0f, 1.0f, 0.0f)), 
            MathHelper.DegreesToRadians(ry)
        );
        matrix *= Matrix4x4.CreateFromAxisAngle(
            Vector3.Normalize(new Vector3(1.0f, 0.0f, 0.0f)), 
            MathHelper.DegreesToRadians(rx)
        );

        matrix *= Matrix4x4.CreateTranslation(translation);

        return matrix;
    }
}
