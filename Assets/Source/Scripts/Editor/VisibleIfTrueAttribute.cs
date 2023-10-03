using UnityEngine;

namespace UnityTool
{
#if UNITY_EDITOR
    public sealed class VisibleIfTrueAttribute : PropertyAttribute
    {
        public string PropertyName { get; }

        public VisibleIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
#endif
}