using System.Numerics;
using Silk.NET.OpenGL;
using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.shaders;
using ThinMatrix_Tutorial.src.terrains;
using ThinMatrix_Tutorial.src.toolbox;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class MasterRenderer
{
    private GL _gl = Program.GL;

    private static readonly float FOV = 70.0f;
    private static readonly float NEAR_PLANE = 0.1f;
    private static readonly float FAR_PLANE = 1000.0f;

    private Matrix4x4 _projectionMatrix;

    private StaticShader _shader = new StaticShader();
    private EntityRenderer _renderer;

    private TerrainRenderer _terrainRenderer;
    private TerrainShader _terrainShader = new TerrainShader();

    private Dictionary<TexturedModel, List<Entitiy>> _entities = new Dictionary<TexturedModel, List<Entitiy>>();
    private List<Terrain> _terrains = new List<Terrain>();

    public MasterRenderer()
    {
        _gl.Enable(EnableCap.CullFace);
        _gl.CullFace(TriangleFace.Back);

        CreateProjectionMatrix();

        _renderer = new EntityRenderer(_shader, _projectionMatrix);
        _terrainRenderer = new TerrainRenderer(_terrainShader, _projectionMatrix);
    }

    public void Render(Light sun, Camera camera)
    {
        Prepare();

        _shader.Start();
        _shader.LoadLight(sun);
        _shader.LoadViewMatrix(camera);
        _renderer.Render(_entities);
        _shader.Stop();

        _terrainShader.Start();
        _terrainShader.LoadLight(sun);
        _terrainShader.LoadViewMatrix(camera);
        _terrainRenderer.Render(_terrains);
        _terrainShader.Stop();
        
        _terrains.Clear();
        _entities.Clear();
    }

    public void ProcessTerrain(Terrain terrain)
    {
        _terrains.Add(terrain);
    }

    public void ProcessEntitiy(Entitiy entitiy)
    {
        TexturedModel entityModel = entitiy.GetModel();
        List<Entitiy> batch = _entities[entityModel];

        if (batch != null)
        {
            batch.Add(entitiy);
        }
        else
        {
            List<Entitiy> newBatch = new List<Entitiy>();
            newBatch.Add(entitiy);
            _entities.Add(entityModel, newBatch);
        }
    }

    public void CleanUp()
    {
        _shader.CleanUp();
        _terrainShader.CleanUp();
    }

    public void Prepare()
    {
        _gl.Enable(EnableCap.DepthTest);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        _gl.ClearColor(0.3f, 0.0f, 0.0f, 1.0f);
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
