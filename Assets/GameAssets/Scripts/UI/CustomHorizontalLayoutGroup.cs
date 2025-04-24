using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomHorizontalLayoutGroup : MonoBehaviour
{
    [SerializeField] private float _spacing = 10f;
    [SerializeField] private float _paddingLeft = 10f;
    [SerializeField] private float _paddingRight = 10f;
    [SerializeField] private bool _controlChildWidth = true;

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
        _rectTransform = GetComponent<RectTransform>();
        float x = _paddingLeft;

        foreach (RectTransform child in transform)
        {
            if (!child.gameObject.activeSelf)
                continue;

            float width = GetPreferredWidth(child);

            child.anchorMin = new Vector2(0, 0.5f);
            child.anchorMax = new Vector2(0, 0.5f);
            child.pivot = new Vector2(0, 0.5f);

            if (_controlChildWidth)
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            child.anchoredPosition = new Vector2(x, 0f);
            x += width + _spacing;
        }

        float totalWidth = x - _spacing + _paddingRight;
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }

    private float GetPreferredWidth(RectTransform child)
    {
        TMP_Text tmpText = child.GetComponent<TMP_Text>();
        if (tmpText != null)
        {
            return tmpText.GetPreferredValues().x;
        }

        LayoutElement layout = child.GetComponent<LayoutElement>();
        if (layout != null && layout.preferredWidth > 0)
        {
            return layout.preferredWidth;
        }

        return child.rect.width > 0 ? child.rect.width : 100f; // fallback
    }
}
