using Silk.NET.OpenGL;
using StbImageSharp;
using ThinMatrix_Tutorial.src.models;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class Loader
{
    private GL _gl = Program.GL;

    private List<uint> _vaos = new List<uint>();
    private List<uint> _vbos = new List<uint>();
    private List<uint> _textures = new List<uint>();

    public RawModel LoadToVao(float[] positions, float[] textureCoords, float[] normals, uint[] indices)
    {
        uint vaoID = CreateVAO();
        
        BindIndicesBuffer(indices);

        StoreDataInAttibuteList(0, 3, positions);
        StoreDataInAttibuteList(1, 2, textureCoords);
        StoreDataInAttibuteList(2, 3, normals);
        
        UnbindVAO();

        return new RawModel(vaoID, (uint)indices.Length);
    }

    public unsafe uint LoadTexture(string fileName)
    {
        uint textureID = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, textureID);

        // define os parâmetros de repetição da textura
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // definir parâmetros de filtragem de textura
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // carregar imagem, criar textura e gerar mipmaps
        int width, height;
        byte[] data;

        // StbImage.stbi_set_flip_vertically_on_load(1); // instrui a stb_image.h a inverter as texturas carregadas no eixo Y.

        using (FileStream stream = File.OpenRead($"res/textures/{fileName}.png"))
        {
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.Default);

            width = image.Width;
            height = image.Height;
            data = image.Data;
        }

        if (data != null)
        {
            fixed (byte* ptr = data)
            {
                _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)width, (uint)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
            }
        }
        else
        {
            Console.WriteLine("Falha ao carregar a textura");
        }

        _textures.Add(textureID);

        return textureID;
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

        foreach (uint texture in _textures)
        {
            _gl.DeleteTexture(texture);
        }
    }

    private uint CreateVAO()
    {
        uint vaoID = _gl.GenVertexArray();
        _vaos.Add(vaoID);

        _gl.BindVertexArray(vaoID);

        return vaoID;
    }

    private unsafe void StoreDataInAttibuteList(uint attributeNumber, int coordinateSize, float[] data)
    {
        uint vboID = _gl.GenBuffer();
        _vbos.Add(vboID);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vboID);
        fixed (float* buf = data)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(data.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }

        _gl.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, (void*)0);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }

    private void UnbindVAO()
    {
        _gl.BindVertexArray(0);
    }

    private unsafe void BindIndicesBuffer(uint[] indices)
    {
        uint vboID = _gl.GenBuffer();
        _vbos.Add(vboID);

        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, vboID);
        fixed (uint* buf = indices)
        {
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (uint)(indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);
        }
    }
}
