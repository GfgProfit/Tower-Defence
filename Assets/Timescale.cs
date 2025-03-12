using TMPro;
using UnityEngine;

public class Timescale : MonoBehaviour
{
    [SerializeField] private TMP_Text _timescaleButtonText;

    [Space]
    [SerializeField] private int[] _times;
    
    private int _index = 0;

    public void OnClick()
    {
        _index++;

        if (_index > _times.Length - 1)
        {
            _index = 0;
        }

        Time.timeScale = _times[_index];

        _timescaleButtonText.text = $"TimeScale: {_times[_index]}x";
    }
}