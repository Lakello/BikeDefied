using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GridCellSizeSetterForPlayerData : GridCellSizeSetter
{
    [SerializeField] private GridCellSizeSetterForLeaderBoard _setter;
    [SerializeField] private HorizontalLayoutGroup _group;
    [SerializeField] private int _divisor = 1;

    protected override int Divisor => _divisor;

    protected override void Awake()
    {
        StartCoroutine(WaitAndSetSize());
    }

    protected override Vector2 CalculateSize()
    {
        float y = Parent.rect.size.y;
        float x = y * 2;

        Vector2 size;

        size.y = y;
        size.x = Parent.rect.size.x - x;

        return size;
    }

    private IEnumerator WaitAndSetSize()
    {
        yield return _setter.Setted;
        yield return _group.IsSizeSetted;
        
        Grid.cellSize = CalculateSize();
    }
}