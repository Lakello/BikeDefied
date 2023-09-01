using UnityEngine;
using UnityEngine.UI;
using YG;

public abstract class GroupSizeSetter : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    protected abstract int Divisor { get; }

    protected Vector2 CalculateSize()
    {
        Vector2 size;
        size.x = _parent.rect.size.x;
        size.y = _parent.rect.size.y / Divisor;
        return size;
    }
}
