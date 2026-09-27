namespace ThinMatrix_Tutorial.src.textures;

public class ModelTexture
{
    private uint _textureID;

    private float _shineDamper = 1.0f;
    private float _reflectivity = 0.0f;

    public ModelTexture(uint id)
    {
        _textureID = id;
    }

    public uint GetID()
    {
        return _textureID;
    }

    public float GetShineDamper()
    {
        return _shineDamper;
    }

    public void SetShineDamper(float shineDamper)
    {
        _shineDamper = shineDamper;
    }

    public float GetReflectivity()
    {
        return _reflectivity;
    }

    public void SetReflectivity(float reflectivity)
    {
        _reflectivity = reflectivity;
    }
}
