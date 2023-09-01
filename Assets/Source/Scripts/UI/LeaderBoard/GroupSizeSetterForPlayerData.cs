using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GroupSizeSetterForPlayerData : GroupSizeSetter
{
    [SerializeField] private int _divisor = 1;

    private RectTransform _rectTransform;

    protected override int Divisor => _divisor;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        var childs = GetComponentsInChildren<RectTransform>();

        var x = childs.Length * childs[0].rect.x;

        _rectTransform.sizeDelta = new Vector2(x, CalculateSize().y);
        _rectTransform.position = new Vector2(_rectTransform.rect.size.x / 2, _rectTransform.position.y);
    }
}