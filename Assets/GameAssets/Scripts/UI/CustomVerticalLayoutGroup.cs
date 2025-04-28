using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomVerticalLayoutGroup : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] private float _spacing = 10f;
    [SerializeField] private float _paddingTop = 10f;
    [SerializeField] private float _paddingBottom = 10f;
    [SerializeField] private float _paddingLeft = 10f;
    [SerializeField] private float _paddingRight = 10f;

    [Header("Options")]
    [SerializeField] private bool _controlChildWidth = true;
    [SerializeField] private bool _controlChildHeight = true;
    [SerializeField] private bool _fitParentWidth = true;
    [SerializeField] private bool _fitParentHeight = true;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        ArrangeChildren();
    }

    private void ArrangeChildren()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        float y = -_paddingTop;
        float maxWidth = 0f;

        foreach (RectTransform child in transform)
        {
            if (!child.gameObject.activeSelf)
                continue;

            Vector2 preferredSize = GetPreferredSize(child);

            child.anchorMin = new Vector2(0, 1);
            child.anchorMax = new Vector2(0, 1);
            child.pivot = new Vector2(0, 1);

            if (_controlChildWidth)
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
            if (_controlChildHeight)
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);

            child.anchoredPosition = new Vector2(_paddingLeft, y);
            y -= preferredSize.y + _spacing;

            maxWidth = Mathf.Max(maxWidth, preferredSize.x);
        }

        float totalHeight = Mathf.Abs(y + _spacing) + _paddingBottom;
        float totalWidth = maxWidth + _paddingLeft + _paddingRight;

        if (_fitParentHeight)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
        if (_fitParentWidth)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }

    private Vector2 GetPreferredSize(RectTransform child)
    {
        if (child.TryGetComponent(out TMP_Text tmp))
        {
            Vector2 size = tmp.GetPreferredValues();
            return new Vector2(size.x, size.y);
        }

        return new Vector2(
            child.rect.width > 0 ? child.rect.width : 100f,
            child.rect.height > 0 ? child.rect.height : 30f
        );
    }
}