using DG.Tweening;
using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    [Space]
    [SerializeField] private int _startedMoney = 100;

    public int Money { get; private set; }

    private void Awake()
    {
        Money = _startedMoney;

        DisplayMoney();
    }

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnMoneySpend += SpendMoney;
        GameController.Instance.EventBus.OnMoneyGather += GatherMoney;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnMoneySpend -= SpendMoney;
        GameController.Instance.EventBus.OnMoneyGather -= GatherMoney;
    }

    private void SpendMoney(int value)
    {
        if (Money < value)
        {
            return;
        }

        Money -= value;

        AnimateMoneyText(0.9f);

        DisplayMoney();
    }

    private void GatherMoney(int value)
    {
        Money += value;

        DisplayMoney();

        AnimateMoneyText(1.1f);
    }

    private void DisplayMoney()
    {
        _moneyText.text = $"<color=#FFC87F>$:</color> {Money}";
    }

    private void AnimateMoneyText(float scale)
    {
        DOTween.Sequence()
            .Append(_moneyText.rectTransform.DOScale(scale, 0.2f).SetEase(Ease.OutBack))
            .Append(_moneyText.rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));
    }
}