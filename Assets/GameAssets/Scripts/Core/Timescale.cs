using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Timescale : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _timescaleButtonText;

    [Space]
    [SerializeField] private float[] _times;

    private int _index = 3;

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

    private void Middle()
    {
        _index = 0;

        ApplyTimescale();
    }

    private void ApplyTimescale()
    {
        Time.timeScale = _times[_index];

        _timescaleButtonText.text = string.Format("TimeScale: {0}x", _times[_index]);
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
            Middle();
        }
    }
}