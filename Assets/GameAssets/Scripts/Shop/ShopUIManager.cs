using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _shopPanelTransform;
    [SerializeField] private ShopItemUI _shopItemUIPrefab;
    [SerializeField] private ShopItemConfig[] _shopItemConfigs;

    private void Start()
    {
        InitializeShopUI();
    }

    private void InitializeShopUI()
    {
        _shopPanelTransform.gameObject.SetActive(true);

        foreach (var config in _shopItemConfigs)
        {
            var shopItemUI = Instantiate(_shopItemUIPrefab, _shopPanelTransform);
            shopItemUI.Setup(config);
        }

        _shopPanelTransform.gameObject.SetActive(false);
    }
}