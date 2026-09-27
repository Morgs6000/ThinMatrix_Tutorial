using ThinMatrix_Tutorial.src.models;
using ThinMatrix_Tutorial.src.renderEngine;
using ThinMatrix_Tutorial.src.textures;

namespace ThinMatrix_Tutorial.src.terrains;

public class Terrain
{
    private static readonly float SIZE = 800.0f;
    private static readonly int VERTEX_COUNT = 128;

    private float _x;
    private float _z;

    private RawModel _model;
    private ModelTexture _texture;

    public Terrain(int gridX, int gridZ, Loader loader, ModelTexture texture)
    {
        _texture = texture;

        _x = gridX * SIZE;
        _z = gridZ * SIZE;

        _model = GenerateTerrain(loader);
    }

    public float GetX()
    {
        return _x;
    }

    public float GetZ()
    {
        return _z;
    }

    public RawModel GetModel()
    {
        return _model;
    }

    public ModelTexture GetTexture()
    {
        return _texture;
    }

    private RawModel GenerateTerrain(Loader loader)
    {
        int count = VERTEX_COUNT * VERTEX_COUNT;

        float[] vertices = new float[count * 3];
        float[] normals = new float[count * 3];
        float[] textureCoords = new float[count * 2];
        uint[] indices = new uint[6 * (VERTEX_COUNT - 1) * (VERTEX_COUNT - 1)];

        int vertexPointer = 0;

        for (int i = 0; i < VERTEX_COUNT; i++)
        {
            for (int j = 0; j < VERTEX_COUNT; j++)
            {
                vertices[vertexPointer * 3] = j / ((float)VERTEX_COUNT - 1) * SIZE;
                vertices[vertexPointer * 3 + 1] = 0.0f;
                vertices[vertexPointer * 3 + 2] = i / ((float)VERTEX_COUNT - 1) * SIZE;

                normals[vertexPointer * 3] = 0.0f;
                normals[vertexPointer * 3 + 1] = 1.0f;
                normals[vertexPointer * 3 + 2] = 0.0f;

                textureCoords[vertexPointer * 2] = j / ((float)VERTEX_COUNT - 1);
                textureCoords[vertexPointer * 2 + 1] = i / ((float)VERTEX_COUNT - 1);

                vertexPointer++;
            }
        }

        int pointer = 0;

        for (int gz = 0; gz < VERTEX_COUNT - 1; gz++)
        {
            for (int gx = 0; gx < VERTEX_COUNT - 1; gx++)
            {
                int topLeft = (gz * VERTEX_COUNT) + gx;
                int topRight = topLeft + 1;
                int bottomLeft = ((gz + 1) * VERTEX_COUNT) + gx;
                int bottomRight = bottomLeft + 1;

                indices[pointer++] = (uint)topLeft;
                indices[pointer++] = (uint)bottomLeft;
                indices[pointer++] = (uint)topRight;
                indices[pointer++] = (uint)topRight;
                indices[pointer++] = (uint)bottomLeft;
                indices[pointer++] = (uint)bottomRight;
            }
        }

        return loader.LoadToVao(vertices, textureCoords, normals, indices);
    }
}
