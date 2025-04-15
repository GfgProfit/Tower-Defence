using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTimeController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _timescaleButtonText;

    [Space]
    [SerializeField] private float[] _times;

    private int _index;

    private void Awake()
    {
        _index = System.Array.IndexOf(_times, 1f);
        
        if (_index == -1)
        {
            _index = 0;
        }

        ApplyTimescale();
    }

    private void Up()
    {
        _index++;

        if (_index > _times.Length - 1)
        {
            _index = 0;
        }

        ApplyTimescale();
    }

    private void Down()
    {
        _index--;

        if (_index < 0)
        {
            _index = _times.Length - 1;
        }

        ApplyTimescale();
    }

    private void ResetTime()
    {
        _index = 0;

        ApplyTimescale();
    }

    private void ApplyTimescale()
    {
        if (_times == null || _times.Length == 0)
        {
            return;
        }

        Time.timeScale = _times[_index];

        _timescaleButtonText.text = $"TimeScale: {_times[_index]:0.##}x";
        _timescaleButtonText.color = _times[_index] == 1f ? Color.white : Color.yellow;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Up();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Down();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            ResetTime();
        }
    }
}