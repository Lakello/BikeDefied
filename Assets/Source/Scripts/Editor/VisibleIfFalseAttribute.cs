using UnityEngine;

namespace UnityTool
{
    public sealed class VisibleIfFalseAttribute : PropertyAttribute
    {
        public string PropertyName { get; }

        public VisibleIfFalseAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
