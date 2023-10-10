using UnityEngine;

namespace UnityTool
{
    public sealed class VisibleToConditionAttribute : PropertyAttribute
    {
        public string PropertyName { get; }
        public bool Condition { get; }

        public VisibleToConditionAttribute(string propertyName, bool condition = true)
        {
            PropertyName = propertyName;
            Condition = condition;
        }
    }
}