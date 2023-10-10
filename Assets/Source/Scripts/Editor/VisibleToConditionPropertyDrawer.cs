using UnityEditor;
using UnityEngine;

namespace UnityTool
{
    [CustomPropertyDrawer(typeof(VisibleToConditionAttribute))]
    public class VisibleToConditionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldDisplay(property))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUI.PropertyField(position, property, label, includeChildren: true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldDisplay(property)
                ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true)
                : 0;
        }

        private bool ShouldDisplay(SerializedProperty property)
        {
            var attr = (VisibleToConditionAttribute)attribute;
            var dependentProp = property.serializedObject.FindProperty(attr.PropertyName);
            return dependentProp.boolValue == attr.Condition;
        }
    }
}