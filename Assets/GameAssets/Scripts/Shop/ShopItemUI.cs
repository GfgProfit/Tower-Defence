using GameAssets.Global.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _priceText;

    private ShopItemConfig _shopItemConfig;

    public void Setup(ShopItemConfig shopItemConfig)
    {
        _shopItemConfig = shopItemConfig;

        _iconImage.sprite = _shopItemConfig.TowerIcon;
        _priceText.text = Utils.FormatNumber(_shopItemConfig.Price, '.').ToString();

        if (TryGetComponent(out CustomButton customButton))
        {
            customButton.OnClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        GameController.Instance.EventBus.RaiseShopBuy(_shopItemConfig);
    }
}