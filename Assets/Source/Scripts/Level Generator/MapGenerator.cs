using UnityEngine;

namespace LevelGenerator
{
    [RequireComponent(typeof(MapDisplay))]
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private DrawMode _drawMode;

        [SerializeField] private float _meshHeightMultiplier;

        [SerializeField] private int _mapWidth;
        [SerializeField] private int _mapHeight;
        [SerializeField] private float _noiseScale;
        [SerializeField] private bool _isAutoUpdate;

        [SerializeField] private int _octaves;
        [SerializeField, Range(0f, 1f)] private float _persistance;
        [SerializeField] private float _lacunarity;

        [SerializeField] private int _seed;
        [SerializeField] private Vector2 _offset;

        [SerializeField] TerrainType[] _regions;

        private MapDisplay _mapDisplay;

        public bool IsAutoUpdate => _isAutoUpdate;

        private enum DrawMode
        {
            NoiseMap,
            ColourMap,
            Mesh
        }

        private void OnValidate()
        {
            if (_mapDisplay == null)
                _mapDisplay = GetComponent<MapDisplay>();

            if (_mapHeight < 1)
                _mapHeight = 1;

            if (_mapWidth < 1)
                _mapWidth = 1;

            if (_lacunarity < 1)
                _lacunarity = 1;

            if (_octaves < 0)
                _octaves = 0;
        }

        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeight, _noiseScale, _seed,
                                                       _octaves, _persistance, _lacunarity, _offset);

            Color[] colourMap = new Color[_mapWidth * _mapHeight];

            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    float currentHeight = noiseMap[x, y];

                    for (int i = 0; i < _regions.Length; i++)
                    {
                        if (currentHeight <= _regions[i].Height)
                        {
                            colourMap[y * _mapWidth + x] = _regions[i].Colour;
                            break;
                        }
                    }
                }
            }

            if (_drawMode == DrawMode.NoiseMap)
                _mapDisplay.DrawTexture(TextureGenerator.GetTextureFromHeightMap(noiseMap));
            else if (_drawMode == DrawMode.ColourMap)
                _mapDisplay.DrawTexture(TextureGenerator.GetTextureFromColourMap(colourMap, _mapWidth, _mapHeight));
            else if (_drawMode == DrawMode.Mesh)
                _mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, _meshHeightMultiplier),
                                     TextureGenerator.GetTextureFromColourMap(colourMap, _mapWidth, _mapHeight));
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string Name;
        public float Height;
        public Color Colour;
    }
}