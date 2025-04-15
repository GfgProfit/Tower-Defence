using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isHovered = false;

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DOTween.Sequence()
            .Append(transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                if (!_isHovered)
                {
                    transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack);
                }
            })
            .Append(transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;

        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;

        transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack);
    }
}