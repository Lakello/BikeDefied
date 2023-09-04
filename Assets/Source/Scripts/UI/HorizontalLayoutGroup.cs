using UnityEngine;
using UnityEngine.UI;

public class HorizontalLayoutGroup : LayoutGroup
{
    [SerializeField] private float _spacing = 0;

    private bool _childForceExpandHeight = true;
    private bool _childControlHeight = true;

    public float Spacing { get { return _spacing; } private set { SetProperty(ref _spacing, value); } }

    protected override void OnEnable()
    {
        base.OnEnable();

        CalculateLayoutInputHorizontal();
        CalculateLayoutInputVertical();
        SetLayoutHorizontal();
        SetLayoutVertical();
    }

    protected void CalculationAlongAxis(int axis, bool isVertical)
    {
        float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);
        bool controlSize = (axis != 0 && _childControlHeight);
        bool childForceExpandSize = (axis != 0 && _childForceExpandHeight);

        float totalMin = combinedPadding;
        float totalPreferred = combinedPadding;
        float totalFlexible = 0;

        bool alongOtherAxis = (isVertical ^ (axis == 1));
        var rectChildrenCount = rectChildren.Count;
        for (int i = 0; i < rectChildrenCount; i++)
        {
            RectTransform child = rectChildren[i];
            float min, preferred, flexible;
            GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);

            if (alongOtherAxis)
            {
                totalMin = Mathf.Max(min + combinedPadding, totalMin);
                totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
                totalFlexible = Mathf.Max(flexible, totalFlexible);
            }
            else
            {
                totalMin += min + Spacing;
                totalPreferred += preferred + Spacing;

                totalFlexible += flexible;
            }
        }

        if (!alongOtherAxis && rectChildren.Count > 0)
        {
            totalMin -= Spacing;
            totalPreferred -= Spacing;
        }
        totalPreferred = Mathf.Max(totalMin, totalPreferred);
        SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
    }

    protected void SetChildrenAlongAxis(int axis, bool isVertical)
    {
        float size = rectTransform.rect.size[axis];
        bool controlSize = (axis != 0 && _childControlHeight);
        bool childForceExpandSize = (axis != 0 && _childForceExpandHeight);
        float alignmentOnAxis = GetAlignmentOnAxis(axis);

        bool alongOtherAxis = (isVertical ^ (axis == 1));
        float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

        if (alongOtherAxis)
        {
            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                float min, preferred, flexible;
                GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                float requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? size : preferred);

                float startOffset = GetStartOffset(axis, requiredSpace);

                SetChildAlongAxis(child, axis, startOffset, requiredSpace);
            }
        }
        else
        {
            float pos = (axis == 0 ? padding.left : padding.top);
            float itemFlexibleMultiplier = 0;
            float surplusSpace = size - GetTotalPreferredSize(axis);

            if (surplusSpace > 0)
            {
                if (GetTotalFlexibleSize(axis) == 0)
                    pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
                else if (GetTotalFlexibleSize(axis) > 0)
                    itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
            }

            float minMaxLerp = 0;
            if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
                minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                float min, preferred, flexible;

                if (child.gameObject.TryGetComponent(out SymmetrySize symmetry))
                {
                    GetChildSizes(child, 1, !controlSize, !childForceExpandSize, out min, out preferred, out flexible);

                    float requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? rectTransform.rect.size[1] : preferred);

                    GetChildSizes(child, 0, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                    float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;

                    child.anchorMin = Vector2.up;
                    child.anchorMax = Vector2.up;

                    Vector2 sizeDelta = child.sizeDelta;
                    sizeDelta[axis] = requiredSpace;
                    child.sizeDelta = sizeDelta;

                    Vector2 anchoredPosition = child.anchoredPosition;
                    anchoredPosition[axis] = (axis == 0) ? (pos + requiredSpace * child.pivot[axis] * 1f) : (-pos - requiredSpace * (1f - child.pivot[axis]) * 1f);
                    child.anchoredPosition = anchoredPosition;
                    pos += childSize + Spacing;
                }
                else
                {
                    GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                    float scaleFactor = 1f;

                    float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;
                    if (controlSize)
                    {
                        SetChildAlongAxisWithScale(child, axis, pos, childSize, scaleFactor);
                    }
                    else
                    {
                        float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                        SetChildAlongAxisWithScale(child, axis, pos + offsetInCell, scaleFactor);
                    }
                    pos += childSize * scaleFactor + Spacing;
                }
            }
        }
    }

    private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand,
        out float min, out float preferred, out float flexible)
    {
        if (!controlSize)
        {
            min = child.sizeDelta[axis];
            preferred = min;
            flexible = 0;
        }
        else
        {
            min = UnityEngine.UI.LayoutUtility.GetMinSize(child, axis);
            preferred = UnityEngine.UI.LayoutUtility.GetPreferredSize(child, axis);
            flexible = UnityEngine.UI.LayoutUtility.GetFlexibleSize(child, axis);
        }

        if (childForceExpand)
            flexible = Mathf.Max(flexible, 1);
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        CalculationAlongAxis(0, false);
    }

    public override void CalculateLayoutInputVertical()
    {
        CalculationAlongAxis(1, false);
    }

    public override void SetLayoutHorizontal()
    {
        SetChildrenAlongAxis(0, false);
    }

    public override void SetLayoutVertical()
    {
        SetChildrenAlongAxis(1, false);
    }

#if UNITY_EDITOR

    private int m_Capacity = 10;
    private Vector2[] m_Sizes = new Vector2[10];

    protected virtual void Update()
    {
        if (Application.isPlaying)
            return;

        int count = transform.childCount;

        if (count > m_Capacity)
        {
            if (count > m_Capacity * 2)
                m_Capacity = count;
            else
                m_Capacity *= 2;

            m_Sizes = new Vector2[m_Capacity];
        }

        bool dirty = false;
        for (int i = 0; i < count; i++)
        {
            RectTransform t = transform.GetChild(i) as RectTransform;
            if (t != null && t.sizeDelta != m_Sizes[i])
            {
                dirty = true;
                m_Sizes[i] = t.sizeDelta;
            }
        }

        if (dirty)
            UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }

#endif
}
