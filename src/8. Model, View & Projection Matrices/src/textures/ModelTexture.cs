namespace ThinMatrix_Tutorial.src.textures;

public class ModelTexture
{
    private uint _textureID;

    public ModelTexture(uint id)
    {
        _textureID = id;
    }

    public uint GetID()
    {
        return _textureID;
    }
}
