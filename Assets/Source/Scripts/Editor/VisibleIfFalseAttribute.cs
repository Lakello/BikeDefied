using UnityEngine;

namespace UnityTool
{
#if UNITY_EDITOR
    public sealed class VisibleIfFalseAttribute : PropertyAttribute
    {
        public string PropertyName { get; }

        public VisibleIfFalseAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
#endif
}
