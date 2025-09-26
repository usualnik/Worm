using System;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public event Action OnPause;
    public event Action OnRestart;
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }


    [SerializeField] private PauseButton pausedButton;
    [SerializeField] private DestroyTile _tileDestroyer;
    
    private int _losesCount = 0;
    private int _restartsCount;

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
        _tileDestroyer.OnDeathTileDestroyed += TileDestroyer_OnDeathTileDestroyed;
    }

 
    private void OnDestroy()
    {
        pausedButton.OnPauseButtonClicked -= PausedButton_OnPauseButtonClicked;
        _tileDestroyer.OnDeathTileDestroyed -= TileDestroyer_OnDeathTileDestroyed;


    }

    private void TileDestroyer_OnDeathTileDestroyed()
    {
      _losesCount++;
        if (_losesCount % 2 == 0)
        {
            YG2.InterstitialAdvShow();
        }
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

    public void Restart()
    {
        _restartsCount++;

        AudioManager.Instance.Play("Restart");
        UndoManager.Instance.PopAllActionsFromStack();
        OnRestart?.Invoke();

        if (_restartsCount % 3 == 0 )
        {
            YG2.InterstitialAdvShow();
        }
    }
}
