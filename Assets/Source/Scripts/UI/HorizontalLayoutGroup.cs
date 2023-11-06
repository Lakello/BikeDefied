using System;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI
{
    public class HorizontalLayoutGroup : LayoutGroup
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _spacing = 0;

        private bool _childForceExpandHeight = true;
        private bool _childControlHeight = true;

        public float Spacing 
        {
            get => _spacing; 
            private set => SetProperty(ref _spacing, value); 
        }

        public event Action LayoutUpdated;

        protected enum Axis
        {
            Width,
            Height
        };

        protected void CalculationAlongAxis(Axis axis)
        {
            float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);
            bool controlSize = (axis != 0 && _childControlHeight);
            bool childForceExpandSize = (axis != 0 && _childForceExpandHeight);

            float totalMin = combinedPadding;
            float totalPreferred = combinedPadding;
            float totalFlexible = 0;

            var rectChildrenCount = rectChildren.Count;

            for (int i = 0; i < rectChildrenCount; i++)
            {
                RectTransform child = rectChildren[i];
                float min, preferred, flexible;
                GetChildSizes(child, (int)axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                if (axis == Axis.Height)
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

            if (axis == Axis.Width && rectChildren.Count > 0)
            {
                totalMin -= Spacing;
                totalPreferred -= Spacing;
            }

            totalPreferred = Mathf.Max(totalMin, totalPreferred);
            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, (int)axis);
        }

        protected void SetChildrenAlongAxis(Axis axis)
        {
            float size = rectTransform.rect.size[(int)axis];
            bool controlSize = (axis != 0 && _childControlHeight);
            bool childForceExpandSize = (axis != 0 && _childForceExpandHeight);
            float alignmentOnAxis = GetAlignmentOnAxis((int)axis);

            float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

            bool isWidthLessHeigth = _rect.rect.width <= _rect.rect.height;

            if (axis == Axis.Height)
            {
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    RectTransform child = rectChildren[i];

                    float min, preferred, flexible;
                    GetChildSizes(child, (int)Axis.Height, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                    float requiredSpace;

                    if (isWidthLessHeigth)
                    {
                        flexible = preferred = _rect.rect.width / _rect.rect.height;
                        requiredSpace = Mathf.Clamp(innerSize, min, flexible * size);
                    }
                    else
                    {
                        requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? size : preferred);
                    }

                    float startOffset = GetStartOffset((int)Axis.Height, requiredSpace);

                    SetChildAlongAxis(child, (int)Axis.Height, startOffset, requiredSpace);
                }
            }
            else
            {
                float pos = (axis == 0 ? padding.left : padding.top);
                float itemFlexibleMultiplier = 0;
                float surplusSpace = size - GetTotalPreferredSize((int)Axis.Width);

                if (surplusSpace > 0)
                {
                    if (GetTotalFlexibleSize((int)Axis.Width) == 0)
                        pos = GetStartOffset((int)Axis.Width, GetTotalPreferredSize((int)Axis.Width) - (axis == 0 ? padding.horizontal : padding.vertical));
                    else if (GetTotalFlexibleSize((int)Axis.Width) > 0)
                        itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize((int)Axis.Width);
                }

                float minMaxLerp = 0;
                if (GetTotalMinSize((int)Axis.Width) != GetTotalPreferredSize((int)Axis.Width))
                    minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize((int)Axis.Width)) / (GetTotalPreferredSize((int)Axis.Width) - GetTotalMinSize((int)Axis.Width)));

                for (int i = 0; i < rectChildren.Count; i++)
                {
                    RectTransform child = rectChildren[i];
                    float min, preferred, flexible;

                    if (child.gameObject.TryGetComponent(out EquilateralSize equilateral))
                    {
                        GetChildSizes(child, (int)Axis.Height, !controlSize, !childForceExpandSize, out min, out preferred, out flexible);

                        float requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? rectTransform.rect.size[(int)Axis.Height] : preferred);

                        GetChildSizes(child, (int)Axis.Width, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                        float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                        childSize += flexible * itemFlexibleMultiplier;

                        child.anchorMin = Vector2.up;
                        child.anchorMax = Vector2.up;

                        Vector2 sizeDelta = child.sizeDelta;
                        sizeDelta[(int)Axis.Width] = requiredSpace;
                        child.sizeDelta = sizeDelta;

                        Vector2 anchoredPosition = child.anchoredPosition;
                        anchoredPosition[(int)Axis.Width] = (axis == 0) ? (pos + requiredSpace * child.pivot[(int)Axis.Width] * 1f) : (-pos - requiredSpace * (1f - child.pivot[(int)Axis.Width]) * 1f);
                        child.anchoredPosition = anchoredPosition;
                        pos += childSize + Spacing;
                    }
                    else
                    {
                        GetChildSizes(child, (int)Axis.Width, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                        float scaleFactor = 1f;

                        float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                        childSize += flexible * itemFlexibleMultiplier;
                        if (controlSize)
                        {
                            SetChildAlongAxisWithScale(child, (int)Axis.Width, pos, childSize, scaleFactor);
                        }
                        else
                        {
                            float offsetInCell = (childSize - child.sizeDelta[(int)Axis.Width]) * alignmentOnAxis;
                            SetChildAlongAxisWithScale(child, (int)Axis.Width, pos + offsetInCell, scaleFactor);
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
                min = LayoutUtility.GetMinSize(child, axis);
                preferred = LayoutUtility.GetPreferredSize(child, axis);
                flexible = LayoutUtility.GetFlexibleSize(child, axis);
            }

            if (childForceExpand)
                flexible = Mathf.Max(flexible, 1);
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalculationAlongAxis(Axis.Width);
        }

        public override void CalculateLayoutInputVertical() =>
            CalculationAlongAxis(Axis.Height);

        public override void SetLayoutHorizontal() =>
            SetChildrenAlongAxis(Axis.Width);

        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(Axis.Height);
            LayoutUpdated?.Invoke();
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
}