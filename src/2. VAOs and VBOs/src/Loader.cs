using Silk.NET.OpenGL;

namespace ThinMatrix_Tutorial.src;

public class Loader
{
    private GL _gl = Program.GL;

    private List<uint> _vaos = new List<uint>();
    private List<uint> _vbos = new List<uint>();

    public RawModel LoadToVao(float[] positions)
    {
        uint vaoID = CreateVAO();

        StoreDataInAttibuteList(0, positions);
        UnbindVAO();

        return new RawModel(vaoID, (uint)(positions.Length / 3));
    }

    public void CleanUp()
    {
        foreach (uint vao in _vaos)
        {
            _gl.DeleteVertexArray(vao);
        }

        foreach (uint vbo in _vbos)
        {
            _gl.DeleteBuffer(vbo);
        }
    }

    private uint CreateVAO()
    {
        uint vaoID = _gl.GenVertexArray();
        _vaos.Add(vaoID);

        _gl.BindVertexArray(vaoID);

        return vaoID;
    }

    private unsafe void StoreDataInAttibuteList(uint attributeNumber, float[] data)
    {
        uint vboID = _gl.GenBuffer();
        _vbos.Add(vboID);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vboID);
        fixed (float* buf = data)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(data.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }

        _gl.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, false, 0, (void*)0);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }

    private void UnbindVAO()
    {
        _gl.BindVertexArray(0);
    }
}
