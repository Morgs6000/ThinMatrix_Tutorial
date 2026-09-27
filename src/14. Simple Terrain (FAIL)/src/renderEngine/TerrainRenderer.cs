using System.Numerics;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.terrains;
using ThinMatrix_Tutorial.src.textures;
using ThinMatrix_Tutorial.src.toolbox;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class TerrainRenderer
{
    private GL _gl = Program.GL;

    private TerrainShader _shader;

    public TerrainRenderer(TerrainShader shader, Matrix4x4 projectionMatrix)
    {
        _shader = shader;

        _shader.Start();
        _shader.LoadProjectionMatrix(projectionMatrix);
        _shader.Stop();
    }

    public unsafe void Render(List<Terrain> terrains)
    {
        foreach (Terrain terrain in terrains)
        {
            PrepareTerrain(terrain);
            LoadModelMatrix(terrain);

            _gl.DrawElements(PrimitiveType.Triangles, terrain.GetModel().GetVertexCount(), DrawElementsType.UnsignedInt, (void*)0);
            
            UnbindTexturedModel();
        }
    }

    public void PrepareTerrain(Terrain terrain)
    {
        RawModel rawModel = terrain.GetModel();

        _gl.BindVertexArray(rawModel.GetVaoID());

        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);
        _gl.EnableVertexAttribArray(2);

        ModelTexture texture = terrain.GetTexture();
        _shader.LoadShineVariables(texture.GetShineDamper(), texture.GetReflectivity());

        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, texture.GetID());
    }

    public void UnbindTexturedModel()
    {
        _gl.DisableVertexAttribArray(0);
        _gl.DisableVertexAttribArray(1);
        _gl.DisableVertexAttribArray(2);

        _gl.BindVertexArray(0);
    }

    private void LoadModelMatrix(Terrain terrain)
    {
        Matrix4x4 transformationMatrix = Maths.CreateTransformationMatrix(new Vector3(terrain.GetX(), 0.0f, terrain.GetZ()), 0.0f, 0.0f, 0.0f, 1.0f);
        _shader.LoadTransformationMatrix(transformationMatrix);
    }
}
