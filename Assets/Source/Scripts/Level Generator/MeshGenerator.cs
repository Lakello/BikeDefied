using UnityEngine;

namespace LevelGenerator
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);

            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            MeshData meshData = new(width, height);
            int vertexIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y] * heightMultiplier, topLeftZ - y);
                    meshData.UV[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        }
    }

    public class MeshData
    {
        public Vector2[] UV { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }

        private int _triangleIndex;

        public MeshData(int meshWidth, int meshHeight)
        {
            UV = new Vector2[meshWidth * meshHeight];
            Vertices = new Vector3[meshWidth * meshHeight];
            Triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c)
        {
            Triangles[_triangleIndex] = a;
            Triangles[_triangleIndex + 1] = b;
            Triangles[_triangleIndex + 2] = c;

            _triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new()
            {
                vertices = Vertices,
                triangles = Triangles,
                uv = UV
            };

            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
