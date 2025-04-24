using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomGridLayoutGroup : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _columns = 3;
    [SerializeField] private Vector2 _cellSize = new Vector2(100f, 100f);
    [SerializeField] private Vector2 _spacing = new Vector2(10f, 10f);
    [SerializeField] private Vector2 _padding = new Vector2(10f, 10f);

    [Header("Auto Resize Parent")]
    [SerializeField] private bool _fitParentWidth = true;
    [SerializeField] private bool _fitParentHeight = true;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        LayoutChildren();
    }

    private void LayoutChildren()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        int row = 0;
        int column = 0;

        float maxWidth = 0f;
        float maxHeight = 0f;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeSelf)
                continue;

            child.anchorMin = new Vector2(0, 1);
            child.anchorMax = new Vector2(0, 1);
            child.pivot = new Vector2(0, 1);

            float x = _padding.x + column * (_cellSize.x + _spacing.x);
            float y = -_padding.y - row * (_cellSize.y + _spacing.y);

            child.anchoredPosition = new Vector2(x, y);
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _cellSize.x);
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _cellSize.y);

            column++;
            if (column >= _columns)
            {
                column = 0;
                row++;
            }

            maxWidth = Mathf.Max(maxWidth, x + _cellSize.x);
            maxHeight = Mathf.Max(maxHeight, -y + _cellSize.y);
        }

        if (_fitParentWidth)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth + _padding.x);
        if (_fitParentHeight)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight + _padding.y);
    }
}