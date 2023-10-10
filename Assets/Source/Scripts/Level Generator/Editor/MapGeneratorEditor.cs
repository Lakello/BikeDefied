#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LevelGenerator
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator generator = (MapGenerator)target;

            if(DrawDefaultInspector())
            {
                if (generator.IsAutoUpdate)
                    generator.GenerateMap();
            }

            if (GUILayout.Button ("Generate"))
                generator.GenerateMap();
        }
    }
}
#endif