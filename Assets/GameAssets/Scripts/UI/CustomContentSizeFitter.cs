using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomContentSizeFitter : MonoBehaviour
{
    [SerializeField] private bool _fitWidth = true;
    [SerializeField] private bool _fitHeight = true;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        FitToChildren();
    }

    public void FitToChildren()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        float maxRight = 0f;
        float maxTop = 0f;

        foreach (RectTransform child in transform)
        {
            if (!child.gameObject.activeSelf)
                continue;

            Vector2 size = GetChildPreferredSize(child);

            float right = child.anchoredPosition.x + size.x;
            float top = Mathf.Abs(child.anchoredPosition.y) + size.y;

            maxRight = Mathf.Max(maxRight, right);
            maxTop = Mathf.Max(maxTop, top);
        }

        if (_fitWidth)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxRight);
        if (_fitHeight)
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxTop);
    }

    private Vector2 GetChildPreferredSize(RectTransform child)
    {
        TMP_Text tmp = child.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            // Учитываем multiline текст, ограниченный по ширине
            return tmp.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
        }

        return new Vector2(
            child.rect.width > 0 ? child.rect.width : 100f,
            child.rect.height > 0 ? child.rect.height : 30f
        );
    }
}