using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeController : MonoBehaviour
{
    [SerializeField] private TimeData[] _timeDatas;

    [Space]
    [SerializeField] private Image _timeFactorImage;
    [SerializeField] private Image _pauseImage;
    [SerializeField] private Sprite _pauseIcon;
    [SerializeField] private Sprite _resumeIcon;
    [SerializeField] private CustomButton _timeButton;
    [SerializeField] private CustomButton _pauseButton;

    private int _index;
    private bool _isPaused = false;

    private void Start()
    {
        _timeButton.OnClick.AddListener(() => ChangeTimeScale());
        _pauseButton.OnClick.AddListener(() => Pause());
    }

    private void ChangeTimeScale()
    {
        _index++;

        if (_index > _timeDatas.Length - 1)
        {
            _index = 0;
        }

        ApplyTimescale();
    }

    private void Pause()
    {
        _isPaused = !_isPaused;

        _pauseImage.sprite = _isPaused ? _resumeIcon : _pauseIcon;
        Time.timeScale = _isPaused ? 0 : _timeDatas[_index].TimeFactor;
    }

    private void ApplyTimescale()
    {
        if (_timeDatas == null || _timeDatas.Length == 0 || _isPaused)
        {
            return;
        }

        Time.timeScale = _timeDatas[_index].TimeFactor;
        _timeFactorImage.sprite = _timeDatas[_index].TimeIcon;
    }
}

[System.Serializable]
public class TimeData
{
    [SerializeField] private Sprite _timeIcon;
    [SerializeField] private int _timeFactor;

    public Sprite TimeIcon => _timeIcon;
    public int TimeFactor => _timeFactor;
}