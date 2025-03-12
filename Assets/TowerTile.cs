using UnityEngine;

public class TowerTile : MonoBehaviour
{
    private Renderer _renderer;
    private Color _originalColor;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.material.color;
    }

    public void Select()
    {
        _renderer.material.color = Color.green;
    }

    public void Deselect()
    {
        _renderer.material.color = _originalColor;
    }
}