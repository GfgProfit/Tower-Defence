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

        DisplayMoney();
    }

    private void GatherMoney(int value)
    {
        Money += value;

        DisplayMoney();
    }

    private void DisplayMoney()
    {
        _moneyText.text = $"<color=#FFC87F>$:</color> {Money}";
    }
}