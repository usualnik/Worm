using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI PANELS")]
    //[SerializeField] private GameObject _youWinPanel;
    [SerializeField] private GameObject _youLosePanel;
    [SerializeField] private GameObject _chooseLevelPanel;
    [SerializeField] private GameObject _mobileInputPanel;
    [SerializeField] private GameObject _gamePlayUiPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("SYSTEM")]
    [SerializeField] private DestroyTile _destroyTile;   

   
    private void Start()
    {
       // WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed += DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
        GameManager.Instance.OnPause +=  GameManager_OnPauseButtonClicked;


        _chooseLevelPanel.SetActive(true);
    }

    

    private void OnDestroy()
    {
       // WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed -= DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;
        GameManager.Instance.OnPause -=  GameManager_OnPauseButtonClicked;
    }

    private void  GameManager_OnPauseButtonClicked()
    {
        _pausePanel.SetActive(GameManager.Instance.IsPaused);
    }

    private void LevelManager_OnLevelChanged(UnityEngine.Tilemaps.Tilemap arg1, 
        UnityEngine.Tilemaps.Tilemap arg2)
    {
        _chooseLevelPanel.SetActive(false);
        _gamePlayUiPanel.SetActive(true);
        
        _mobileInputPanel.SetActive(Application.isMobilePlatform);
       
    }
    private void DestroyTile_OnDeathTileDestroyed()
    {
        AudioManager.Instance.Play("Loss");
        _youLosePanel.SetActive(true);        
    }

    private void WinConditionManager_OnWin()
    {
        //_youWinPanel.SetActive(true);
    }
}
