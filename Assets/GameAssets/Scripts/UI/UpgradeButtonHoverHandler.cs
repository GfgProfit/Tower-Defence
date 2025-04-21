using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UpgradeButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsHovered { get; private set; }

    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
        OnPointerExitEvent?.Invoke();
    }
}