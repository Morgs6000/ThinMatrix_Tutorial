namespace ThinMatrix_Tutorial.src;

public class RawModel
{
    private uint _vaoID;
    private uint _vertexCount;

    public RawModel(uint vaoID, uint vertexCount)
    {
        _vaoID = vaoID;
        _vertexCount = vertexCount;
    }

    public uint GetVaoID()
    {
        return _vaoID;
    }

    public uint GetVertexCount()
    {
        return _vertexCount;
    }
}
