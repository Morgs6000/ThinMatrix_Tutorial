using System.Numerics;

namespace ThinMatrix_Tutorial.src.shaders;

public class StaticShader : ShaderProgram
{
    private static readonly string VERTEX_FILE = "res/shaders/vertex.glsl";
    private static readonly string FRAGMENT_FILE = "res/shaders/fragment.glsl";

    private int _location_transformationMatrix;

    public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE)
    {
        
    }

    protected override void BindAttibutes()
    {
        BindAttibutes(0, "position");
        BindAttibutes(1, "textureCoords");
    }

    protected override void GetAllUniformLocations()
    {
        _location_transformationMatrix = GetUniformLocation("transformationMatrix");
    }

    public void LoadTransformationMatrix(Matrix4x4 matrix)
    {
        LoadMatrix(_location_transformationMatrix, matrix);
    }
}
