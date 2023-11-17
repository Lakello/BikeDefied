using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI.Layout
{
    public class GridCellSizeSetter : MonoBehaviour, ILayoutGroup
    {
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private int _countCells;

        public void SetLayoutHorizontal()
        {
        }

        public void SetLayoutVertical() =>
            _grid.cellSize = CalculateSize();

        private Vector2 CalculateSize()
        {
            Vector2 size;

            size.x = _parent.rect.size.x - _grid.padding.right - _grid.padding.left;
            size.y = _parent.rect.size.y / _countCells;

            return size;
        }
    }
}