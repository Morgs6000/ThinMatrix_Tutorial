using System.Numerics;
using Silk.NET.OpenGL;

namespace ThinMatrix_Tutorial.src.shaders;

public abstract class ShaderProgram
{
    private static GL _gl = Program.GL;

    private uint _programID;
    private uint _vertexShaderID;
    private uint _fragmentShaderID;

    public ShaderProgram(string vertexFile, string fragmentFile)
    {
        _vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
        _fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);

        _programID = _gl.CreateProgram();
        _gl.AttachShader(_programID, _vertexShaderID);
        _gl.AttachShader(_programID, _fragmentShaderID);

        BindAttibutes();

        _gl.LinkProgram(_programID);
        _gl.ValidateProgram(_programID);

        GetAllUniformLocations();
    }

    protected abstract void GetAllUniformLocations();

    protected int GetUniformLocation(string uniformName)
    {
        return _gl.GetUniformLocation(_programID, uniformName);
    }

    public void Start()
    {
        _gl.UseProgram(_programID);
    }

    public void Stop()
    {
        _gl.UseProgram(0);
    }

    public void CleanUp()
    {
        Stop();

        _gl.DetachShader(_programID, _vertexShaderID);
        _gl.DetachShader(_programID, _fragmentShaderID);

        _gl.DeleteShader(_vertexShaderID);
        _gl.DeleteShader(_fragmentShaderID);

        _gl.DeleteProgram(_programID);
    }

    protected abstract void BindAttibutes();

    protected void BindAttibutes(uint attribute, string variableName)
    {
        _gl.BindAttribLocation(_programID, attribute, variableName);
    }

    protected void LoadFloat(int location, float value)
    {
        _gl.Uniform1(location, value);
    }

    protected void LoadVector(int location, Vector3 vector)
    {
        _gl.Uniform3(location, vector);
    }

    protected void LoadBool(int location, bool value)
    {
        _gl.Uniform1(location, value ? 1 : 0);
    }

    protected unsafe void LoadMatrix(int location, Matrix4x4 matrix)
    {
        _gl.UniformMatrix4(location, 1, false, (float*)&matrix);
    }

    private static uint LoadShader(string file, ShaderType type)
    {
        string shaderSource = string.Empty;

        try
        {
            shaderSource = File.ReadAllText(file);
        }
        catch (Exception e)
        {
            Console.WriteLine("Não foi possível ler o arquivo!");
            Console.WriteLine(e.StackTrace);
        }

        uint shaderID = _gl.CreateShader(type);
        _gl.ShaderSource(shaderID, shaderSource);
        _gl.CompileShader(shaderID);

        if (_gl.GetShader(shaderID, ShaderParameterName.CompileStatus) == 0)
        {
            Console.WriteLine(_gl.GetShaderInfoLog(shaderID));
            Console.WriteLine("Não foi possível compilar o shader.");
        }

        return shaderID;
    }
}
