using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public event Action OnPauseButtonClicked;
    
    private Button _pausebutton;

    private void Awake()
    {
        _pausebutton = GetComponent<Button>();
    }

    private void Start()
    {
        _pausebutton.onClick.AddListener(OnPuseButtonClicked);
        
    }

    private void OnDestroy()
    {
        _pausebutton.onClick.RemoveListener(OnPuseButtonClicked);

    }
    private void OnPuseButtonClicked()
    {
        OnPauseButtonClicked?.Invoke();
    }
}
