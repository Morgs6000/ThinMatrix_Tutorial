using Silk.NET.OpenGL;

namespace ThinMatrix_Tutorial.src;

public class Renderer
{
    private GL _gl = Program.GL;

    public void Prepare()
    {
        _gl.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    public void Render(RawModel model)
    {
        _gl.BindVertexArray(model.GetVaoID());
        _gl.EnableVertexAttribArray(0);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, model.GetVertexCount());

        _gl.DisableVertexAttribArray(0);
        _gl.BindVertexArray(0);
    }
}
