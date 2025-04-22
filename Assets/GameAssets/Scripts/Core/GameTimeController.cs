using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeController : MonoBehaviour
{
    [SerializeField] private TimeData[] _timeDatas;

    [Space]
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _pauseImage;
    [SerializeField] private Sprite _pauseIcon;
    [SerializeField] private Sprite _resumeIcon;
    [SerializeField] private CustomButton _timeButton;
    [SerializeField] private CustomButton _pauseButton;

    private int _index;
    private bool _isPaused;

    private void Start()
    {
        if (_timeButton != null)
            _timeButton.OnClick.AddListener(ChangeTimeScale);

        if (_pauseButton != null)
            _pauseButton.OnClick.AddListener(TogglePause);

        ApplyTimescale();
    }

    private void ChangeTimeScale()
    {
        if (_timeDatas == null || _timeDatas.Length == 0)
            return;

        _index = (_index + 1) % _timeDatas.Length;

        if (!_isPaused)
        {
            ApplyTimescale();
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        UpdatePauseIcon();

        Time.timeScale = _isPaused ? 0f : _timeDatas[_index].TimeFactor;
    }

    private void ApplyTimescale()
    {
        if (_timeDatas == null || _timeDatas.Length == 0)
            return;

        Time.timeScale = _timeDatas[_index].TimeFactor;

        if (_timeText != null)
            _timeText.text = _timeDatas[_index].TimeText;
    }

    private void UpdatePauseIcon()
    {
        if (_pauseImage != null)
            _pauseImage.sprite = _isPaused ? _resumeIcon : _pauseIcon;
    }
}

[System.Serializable]
public class TimeData
{
    [SerializeField] private string _timeText;
    [SerializeField] private float _timeFactor = 1f; // <-- Лучше float для Time.timeScale

    public string TimeText => _timeText;
    public float TimeFactor => _timeFactor;
}
