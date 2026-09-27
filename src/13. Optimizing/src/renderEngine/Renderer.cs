using System.Numerics;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.textures;
using ThinMatrix_Tutorial.src.toolbox;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class Renderer
{
    private GL _gl = Program.GL;

    private static readonly float FOV = 70.0f;
    private static readonly float NEAR_PLANE = 0.1f;
    private static readonly float FAR_PLANE = 1000.0f;

    private Matrix4x4 _projectionMatrix;
    private StaticShader _shader;

    public Renderer(StaticShader shader)
    {
        _shader = shader;

        _gl.Enable(EnableCap.CullFace);
        _gl.CullFace(TriangleFace.Back);

        CreateProjectionMatrix();

        shader.Start();
        shader.LoadProjectionMatrix(_projectionMatrix);
        shader.Stop();
    }

    public void Prepare()
    {
        _gl.Enable(EnableCap.DepthTest);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        _gl.ClearColor(0.3f, 0.0f, 0.0f, 1.0f);
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

    private void CreateProjectionMatrix()
    {
        float aspectRatio = (float)Program.SCR_WIDTH / (float)Program.SCR_HEIGHT;
        float y_scale = (float)(1.0f / Math.Tan(MathHelper.DegreesToRadians(FOV / 2.0f))) * aspectRatio;
        float x_scale = y_scale / aspectRatio;
        float frustum_lenght = FAR_PLANE - NEAR_PLANE;

        _projectionMatrix = Matrix4x4.Identity;

        _projectionMatrix.M11 = x_scale;
        _projectionMatrix.M22 = y_scale;
        _projectionMatrix.M33 = -((FAR_PLANE + NEAR_PLANE) / frustum_lenght);
        _projectionMatrix.M34 = -1.0f;
        _projectionMatrix.M43 = -((2.0f * NEAR_PLANE * FAR_PLANE) / frustum_lenght);
        _projectionMatrix.M44 = 0.0f;
    }
}
