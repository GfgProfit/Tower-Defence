using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CustomButton _quitButton;

    private void Awake()
    {
        _quitButton.OnClick.AddListener(() => Application.Quit());   
    }
}