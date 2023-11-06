using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI.Layout
{
    public class GridCellSizeSetter : MonoBehaviour, ILayoutGroup
    {
        [SerializeField] protected GridLayoutGroup Grid;
        [SerializeField] protected RectTransform Parent;
        [SerializeField] private int _countCells;

        public void SetLayoutHorizontal() { }

        public void SetLayoutVertical() =>
            Grid.cellSize = CalculateSize();

        private Vector2 CalculateSize()
        {
            Vector2 size;

            size.x = Parent.rect.size.x - Grid.padding.right - Grid.padding.left;
            size.y = Parent.rect.size.y / _countCells;

            return size;
        }
    }
}