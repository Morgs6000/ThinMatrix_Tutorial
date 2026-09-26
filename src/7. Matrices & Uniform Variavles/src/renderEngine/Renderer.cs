using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.models;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class Renderer
{
    private GL _gl = Program.GL;

    public void Prepare()
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _gl.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public unsafe void Render(TexturedModel texturedModel)
    {
        RawModel model = texturedModel.GetRawModel();

        _gl.BindVertexArray(model.GetVaoID());

        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);

        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, texturedModel.GetTexture().GetID());

        _gl.DrawElements(PrimitiveType.Triangles, model.GetVertexCount(), DrawElementsType.UnsignedInt, (void*)0);

        _gl.DisableVertexAttribArray(0);
        _gl.DisableVertexAttribArray(1);

        _gl.BindVertexArray(0);
    }
}
