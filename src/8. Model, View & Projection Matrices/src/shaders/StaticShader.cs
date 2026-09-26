using System.Numerics;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.toolbox;

namespace ThinMatrix_Tutorial.src.shaders;

public class StaticShader : ShaderProgram
{
    private static readonly string VERTEX_FILE = "res/shaders/vertex.glsl";
    private static readonly string FRAGMENT_FILE = "res/shaders/fragment.glsl";

    private int _location_transformationMatrix;
    private int _location_projectionMatrix;
    private int _location_viewMatrix;

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
        _location_projectionMatrix = GetUniformLocation("projectionMatrix");
        _location_viewMatrix = GetUniformLocation("viewMatrix");
    }

    public void LoadTransformationMatrix(Matrix4x4 matrix)
    {
        LoadMatrix(_location_transformationMatrix, matrix);
    }

    public void LoadViewMatrix(Camera camera)
    {
        Matrix4x4 viewMatrix = Maths.CreateViewMatrix(camera);        
        LoadMatrix(_location_viewMatrix, viewMatrix);
    }

    public void LoadProjectionMatrix(Matrix4x4 projection)
    {
        LoadMatrix(_location_projectionMatrix, projection);
    }
}
