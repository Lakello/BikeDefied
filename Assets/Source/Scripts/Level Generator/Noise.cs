using UnityEngine;

namespace LevelGenerator
{
    public static class Noise
    {
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed,
            int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            System.Random random = new System.Random(seed);

            Vector2[] octavesOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) + offset.y;

                octavesOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
                scale = 0.0001f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency + octavesOffsets[i].x;
                        float sampleY = (y - halfWidth) / scale * frequency + octavesOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    noiseMap[x, y] = noiseHeight;
                }
            }

            return NormalizeNoiseMap(noiseMap, minNoiseHeight, maxNoiseHeight);
        }

        private static float[,] NormalizeNoiseMap(float[,] noiseMap, float minHeight, float maxHeight)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int x = 0; x < noiseMap.GetLength(0); x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}
