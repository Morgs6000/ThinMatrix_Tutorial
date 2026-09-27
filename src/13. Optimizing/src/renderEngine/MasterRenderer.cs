using ThinMatrix_Tutorial.src.entities;
using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.shaders;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class MasterRenderer
{
    private static StaticShader _shader = new StaticShader();
    private Renderer _renderer = new Renderer(_shader);

    private Dictionary<TexturedModel, List<Entitiy>> _entities = new Dictionary<TexturedModel, List<Entitiy>>();

    public void Render(Light sun, Camera camera)
    {
        _renderer.Prepare();

        _shader.Start();
        _shader.LoadLight(sun);
        _shader.LoadViewMatrix(camera);

        _renderer.Render(_entities);

        _shader.Stop();

        _entities.Clear();
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
    }
}
