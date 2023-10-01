using UnityEngine;

namespace UnityTool
{
    public sealed class VisibleIfTrueAttribute : PropertyAttribute
    {
        public string PropertyName { get; }

        public VisibleIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}