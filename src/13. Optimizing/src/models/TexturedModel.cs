using ThinMatrix_Tutorial.src.textures;

namespace ThinMatrix_Tutorial.src.models;

public class TexturedModel
{
    private RawModel _rawModel;
    private ModelTexture _texture;

    public TexturedModel(RawModel model, ModelTexture texture)
    {
        _rawModel = model;
        _texture = texture;
    }

    public RawModel GetRawModel()
    {
        return _rawModel;
    }

    public ModelTexture GetTexture()
    {
        return _texture;
    }
}
