using DG.Tweening;
using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    [Space]
    [SerializeField] private int _startedMoney = 100;

    public int Money { get; private set; }

    private void Awake()
    {
        GatherMoney(_startedMoney);
    }

    private void OnEnable()
    {
        GameManager.Instance.EventBus.OnMoneySpend += SpendMoney;
        GameManager.Instance.EventBus.OnMoneyGather += GatherMoney;
    }

    private void OnDisable()
    {
        GameManager.Instance.EventBus.OnMoneySpend -= SpendMoney;
        GameManager.Instance.EventBus.OnMoneyGather -= GatherMoney;
    }

    private void SpendMoney(int value)
    {
        if (Money < value)
        {
            return;
        }

        Money -= value;

        DOTween.Sequence()
            .Append(_moneyText.rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack))
            .Append(_moneyText.rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));

        DisplayMoney();
    }

    private void GatherMoney(int value)
    {
        Money += value;

        DisplayMoney();

        DOTween.Sequence()
            .Append(_moneyText.rectTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack))
            .Append(_moneyText.rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));
    }

    private void DisplayMoney()
    {
        _moneyText.text = $"<color=#FFC87F>$:</color> {Money}";
    }
}