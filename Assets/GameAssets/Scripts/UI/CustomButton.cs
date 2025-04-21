using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField]
    public UnityEvent OnClick { get; private set; } = new UnityEvent();

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InvokeClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill(true);
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill(true);
        transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack);
    }

    public void AddListener(UnityAction action)
    {
        OnClick.AddListener(action);
    }

    public void RemoveListener(UnityAction action)
    {
        OnClick.RemoveListener(action);
    }

    public void InvokeClick()
    {
        OnClick?.Invoke();
    }
}
