using UnityEngine;

namespace BikeDefied.UnityTool
{
    public sealed class VisibleToConditionAttribute : PropertyAttribute
    {
        public VisibleToConditionAttribute(string propertyName, bool condition = true)
        {
            PropertyName = propertyName;
            Condition = condition;
        }
        
        public string PropertyName { get; }
        
        public bool Condition { get; }
    }
}