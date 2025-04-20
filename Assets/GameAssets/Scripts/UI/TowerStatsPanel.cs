using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private TMP_Text _sellMoneyText;

    public void Show(Sprite icon, string towerName, string stats)
    {
        _panel.gameObject.SetActive(true);

        _iconImage.sprite = icon;
        _nameText.text = towerName;
        _statsText.text = stats;
    }

    public void Hide() => _panel.gameObject.SetActive(false);
}