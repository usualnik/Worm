using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnPause;
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }


    [SerializeField] private PauseButton pausedButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.Log("More than one instance of gamemanager");
    }
    private void Start()
    {
        pausedButton.OnPauseButtonClicked += PausedButton_OnPauseButtonClicked;
    }
    private void OnDestroy()
    {
        pausedButton.OnPauseButtonClicked -= PausedButton_OnPauseButtonClicked;

    }
    private void PausedButton_OnPauseButtonClicked()
    {
        if (!IsPaused)
        {
            IsPaused = true;
            OnPause?.Invoke();
        }
        else
        {
            IsPaused = false;      
            OnPause?.Invoke();
        }
    }
}
