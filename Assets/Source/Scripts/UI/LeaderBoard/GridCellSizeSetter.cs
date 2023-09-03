using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public abstract class GridCellSizeSetter : MonoBehaviour
{
    [SerializeField] protected GridLayoutGroup Grid;
    [SerializeField] protected RectTransform Parent;

    protected abstract int Divisor { get; }

    public bool Setted { get; private set; }

    protected virtual void Awake()
    {
        Grid.cellSize = CalculateSize();
        Setted = true;
    }

    protected virtual Vector2 CalculateSize()
    {
        Vector2 size;

        size.x = Parent.rect.size.x;
        size.y = Parent.rect.size.y / Divisor;

        return size;
    }
}
