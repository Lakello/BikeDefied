using UnityEngine;
using UnityEngine.UI;
using YG;

public class GridCellSizeSetter : MonoBehaviour
{
    [SerializeField] protected GridLayoutGroup Grid;
    [SerializeField] protected RectTransform Parent;
    [SerializeField] private int _countCells;

    private void Awake()
    {
        Grid.cellSize = CalculateSize();
    }

    private Vector2 CalculateSize()
    {
        Vector2 size;

        size.x = Parent.rect.size.x;
        size.y = Parent.rect.size.y / _countCells;

        return size;
    }
}
