using Silk.NET.OpenGL;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class Renderer
{
    private GL _gl = Program.GL;

    public void Prepare()
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _gl.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public unsafe void Render(RawModel model)
    {
        _gl.BindVertexArray(model.GetVaoID());
        _gl.EnableVertexAttribArray(0);

        _gl.DrawElements(PrimitiveType.Triangles, model.GetVertexCount(), DrawElementsType.UnsignedInt, (void*)0);

        _gl.DisableVertexAttribArray(0);
        _gl.BindVertexArray(0);
    }
}
