using UnityEngine;

namespace LevelGenerator
{
    public class MapDisplay : MonoBehaviour
    {
        [SerializeField] private Renderer _textureRender;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        public void DrawTexture(Texture2D texture)
        {
            _textureRender.sharedMaterial.mainTexture = texture;
            _textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            _meshFilter.sharedMesh = meshData.CreateMesh();
            _meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}