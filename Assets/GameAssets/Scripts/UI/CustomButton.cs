using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public UnityEvent OnClick { get; private set; } = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData) => InvokeClick();

    public void AddListener(UnityAction action) => OnClick.AddListener(action);

    public void RemoveListener(UnityAction action) => OnClick.RemoveListener(action);

    public void InvokeClick() => OnClick?.Invoke();
}