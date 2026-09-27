using System.Numerics;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.textures;
using ThinMatrix_Tutorial.src.toolbox;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class EntityRenderer
{
    private GL _gl = Program.GL;

    private StaticShader _shader;

    public EntityRenderer(StaticShader shader, Matrix4x4 projectionMatrix)
    {
        _shader = shader;

        shader.Start();
        shader.LoadProjectionMatrix(projectionMatrix);
        shader.Stop();
    }

    public unsafe void Render(Dictionary<TexturedModel, List<Entitiy>> entities)
    {
        foreach (TexturedModel model in entities.Keys)
        {
            PrepareTexturedModel(model);

            List<Entitiy> batch = entities[model];

            foreach (Entitiy entitiy in batch)
            {
                PrepareInstance(entitiy);

                _gl.DrawElements(PrimitiveType.Triangles, model.GetRawModel().GetVertexCount(), DrawElementsType.UnsignedInt, (void*)0);
            }

            UnbindTexturedModel();
        }
    }

    public void PrepareTexturedModel(TexturedModel model)
    {
        RawModel rawModel = model.GetRawModel();

        _gl.BindVertexArray(rawModel.GetVaoID());

        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);
        _gl.EnableVertexAttribArray(2);

        ModelTexture texture = model.GetTexture();
        _shader.LoadShineVariables(texture.GetShineDamper(), texture.GetReflectivity());

        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, model.GetTexture().GetID());
    }

    public void UnbindTexturedModel()
    {
        _gl.DisableVertexAttribArray(0);
        _gl.DisableVertexAttribArray(1);
        _gl.DisableVertexAttribArray(2);

        _gl.BindVertexArray(0);
    }

    private void PrepareInstance(Entitiy entitiy)
    {
        Matrix4x4 transformationMatrix = Maths.CreateTransformationMatrix(entitiy.GetPosition(), entitiy.GetRotX(), entitiy.GetRotY(), entitiy.GetRotZ(), entitiy.GetScale());
        _shader.LoadTransformationMatrix(transformationMatrix);
    }
}
