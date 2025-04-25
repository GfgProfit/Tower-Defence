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
        Bootstrapper.Instance.EventBus.OnMoneySpend += SpendMoney;
        Bootstrapper.Instance.EventBus.OnMoneyGather += GatherMoney;
        Bootstrapper.Instance.EventBus.OnRequestMoney += HandleRequestMoney;
    }

    private void OnDisable()
    {
        Bootstrapper.Instance.EventBus.OnMoneySpend -= SpendMoney;
        Bootstrapper.Instance.EventBus.OnMoneyGather -= GatherMoney;
        Bootstrapper.Instance.EventBus.OnRequestMoney -= HandleRequestMoney;
    }

    private int HandleRequestMoney() => Money;

    private void SpendMoney(int value)
    {
        if (Money < value)
        {
            return;
        }

        Money -= value;

        DisplayMoney();
    }

    private void GatherMoney(int value)
    {
        Money += value;

        DisplayMoney();
    }

    private void DisplayMoney()
    {
        _moneyText.text = Utils.FormatNumber(Money, '.').ToString();
    }
}