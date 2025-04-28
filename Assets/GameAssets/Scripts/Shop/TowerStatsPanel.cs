using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;

public class TowerStatsPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Slider _upgradeSlider;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private TMP_Text _sellMoneyText;
    [SerializeField] private TMP_Text _upgradeMoneyText;
    [SerializeField] private TMP_Text _totalDamageDealText;
    [SerializeField] private TMP_Text _totalEnemyKilledText;
    [SerializeField] private TMP_Text _totalXPEarnedText;
    [SerializeField] private TMP_Text _autoLevelText;
    [SerializeField] private TMP_Text _upgradeButtonText;
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private CustomButton _sellButton;
    [SerializeField] private CustomButton _upgradeButton;
    [SerializeField] private Image _upgradeButtonImage;

    private TowerStatsViewModel _viewModel;

    private void Awake()
    {
        _viewModel = new TowerStatsViewModel();
        _viewModel.Initialize();

        BindUI();

        _sellButton.OnClick.AddListener(() => _viewModel.SellCommand.Execute());
        _upgradeButton.OnClick.AddListener(() => _viewModel.UpgradeCommand.Execute());
    }

    private void Update()
    {
        _viewModel.Update();
    }

    public void Show(TowerTile tile)
    {
        _viewModel.Show(tile);
        _panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _viewModel.Hide();
        _panel.gameObject.SetActive(false);
    }

    private void BindUI()
    {
        _viewModel.TowerName.Subscribe(x => _nameText.text = x).AddTo(this);
        _viewModel.StatsText.Subscribe(x => _statsText.text = x).AddTo(this);
        _viewModel.SellPrice.Subscribe(x => _sellMoneyText.text = x).AddTo(this);
        _viewModel.UpgradePrice.Subscribe(x => _upgradeMoneyText.text = x).AddTo(this);
        _viewModel.TotalDamage.Subscribe(x => _totalDamageDealText.text = x).AddTo(this);
        _viewModel.TotalEnemiesKilled.Subscribe(x => _totalEnemyKilledText.text = x).AddTo(this);
        _viewModel.TotalXPEarned.Subscribe(x => _totalXPEarnedText.text = x).AddTo(this);
        _viewModel.AutoLevelText.Subscribe(x => _autoLevelText.text = x).AddTo(this);
        _viewModel.UpgradeButtonText.Subscribe(x => _upgradeButtonText.text = x).AddTo(this);
        _viewModel.UpgradeButtonColor.Subscribe(x => _upgradeButtonImage.color = x).AddTo(this);
        _viewModel.CurrentLevelText.Subscribe(x => _currentLevelText.text = x).AddTo(this);

        _viewModel.UpgradeSliderValue.Subscribe(Observer.Create<float>(v => _upgradeSlider.value = v)).AddTo(this);
        _viewModel.UpgradeSliderMax.Subscribe(Observer.Create<float>(v => _upgradeSlider.maxValue = v)).AddTo(this);
        _viewModel.LevelSliderValue.Subscribe(Observer.Create<float>(v => _levelSlider.value = v)).AddTo(this);
        _viewModel.LevelSliderMax.Subscribe(Observer.Create<float>(v => _levelSlider.maxValue = v)).AddTo(this);
    }
}