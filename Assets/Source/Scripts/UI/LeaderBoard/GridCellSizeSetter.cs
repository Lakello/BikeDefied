using UnityEngine;
using UnityEngine.UI;

public class GridCellSizeSetter : LayoutGroup
{
    [SerializeField] protected GridLayoutGroup Grid;
    [SerializeField] protected RectTransform Parent;
    [SerializeField] private int _countCells;

    private Vector2 CalculateSize()
    {
        Vector2 size;

        size.x = Parent.rect.size.x - Grid.padding.right - Grid.padding.left;
        size.y = Parent.rect.size.y / _countCells;

        return size;
    }

    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal() { }

    public override void SetLayoutVertical()
    {
        Grid.cellSize = CalculateSize();
    }
}
