using System.Numerics;
using ThinMatrix_Tutorial.src.models;

namespace ThinMatrix_Tutorial.src.renderEngine;

public class OBJLoader
{
    public static RawModel LoadObjModel(string fileName, Loader loader)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> textures = new List<Vector2>();
        List<uint> indices = new List<uint>();

        float[] verticesArray;
        float[] textureArray;
        uint[] indicesArray;

        string path = $"res/objects/{fileName}.obj";

        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"Arquivo OBJ não encontrado: {path}");
            Console.Error.WriteLine($"Diretório atual: {Directory.GetCurrentDirectory()}");
            throw new FileNotFoundException($"Não foi possível encontrar {path}");
        }

        using (StreamReader reader = new StreamReader(path))
        {
            string? line;

            // Primeira passada: lê vértices e texturas até encontrar a primeira face
            while ((line = reader.ReadLine()) != null)
            {
                string[] currentLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (currentLine.Length == 0) continue;

                if (line.StartsWith("v "))
                {
                    Vector3 vertex = new Vector3(
                        float.Parse(currentLine[1], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(currentLine[2], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(currentLine[3], System.Globalization.CultureInfo.InvariantCulture)
                    );
                    vertices.Add(vertex);
                }
                else if (line.StartsWith("vt "))
                {
                    Vector2 texture = new Vector2(
                        float.Parse(currentLine[1], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(currentLine[2], System.Globalization.CultureInfo.InvariantCulture)
                    );
                    textures.Add(texture);
                }
                else if (line.StartsWith("f "))
                {
                    // Achou a primeira face, para o primeiro loop
                    break;
                }
            }

            // Aloca os arrays de textura com base na quantidade de vértices
            textureArray = new float[vertices.Count * 2];

            // Segunda passada: processa as faces
            while (line != null)
            {
                if (!line.StartsWith("f "))
                {
                    line = reader.ReadLine();
                    continue;
                }

                string[] currentLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Suporta triângulos (3 vértices). Se quiser quads, adapte.
                string[] vertex1 = currentLine[1].Split('/');
                string[] vertex2 = currentLine[2].Split('/');
                string[] vertex3 = currentLine[3].Split('/');

                ProcessVertex(vertex1, indices, textures, textureArray);
                ProcessVertex(vertex2, indices, textures, textureArray);
                ProcessVertex(vertex3, indices, textures, textureArray);

                line = reader.ReadLine();
            }
        }

        // Monta o array final de vértices
        verticesArray = new float[vertices.Count * 3];
        int vertexPointer = 0;
        foreach (Vector3 vertex in vertices)
        {
            verticesArray[vertexPointer++] = vertex.X;
            verticesArray[vertexPointer++] = vertex.Y;
            verticesArray[vertexPointer++] = vertex.Z;
        }

        // Monta o array final de índices
        indicesArray = new uint[indices.Count];
        for (int i = 0; i < indices.Count; i++)
        {
            indicesArray[i] = indices[i];
        }

        // Console.WriteLine($"OBJ '{fileName}' carregado: {vertices.Count} vértices, {indices.Count} índices.");

        return loader.LoadToVao(verticesArray, textureArray, indicesArray);
    }

    private static void ProcessVertex(
        string[] vertexData,
        List<uint> indices,
        List<Vector2> textures,
        float[] textureArray)
    {
        uint currentVertexPointer = uint.Parse(vertexData[0]) - 1;
        indices.Add(currentVertexPointer);

        // Se o OBJ tiver coordenadas de textura
        if (vertexData.Length > 1 && !string.IsNullOrEmpty(vertexData[1]))
        {
            Vector2 currentTex = textures[int.Parse(vertexData[1]) - 1];
            textureArray[currentVertexPointer * 2] = currentTex.X;
            textureArray[currentVertexPointer * 2 + 1] = 1 - currentTex.Y;
        }
    }
}
