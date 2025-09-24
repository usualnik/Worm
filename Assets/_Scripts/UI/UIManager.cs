using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI PANELS")]
    //[SerializeField] private GameObject _youWinPanel;
    [SerializeField] private GameObject _youLosePanel;
    [SerializeField] private GameObject _chooseLevelPanel;


    [Header("SYSTEM")]
    [SerializeField] private DestroyTile _destroyTile;

    private void Awake()
    {
       // _youWinPanel.SetActive(false);
        _youLosePanel.SetActive(false);
    }

    private void Start()
    {
       // WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed += DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
    }

   

    private void OnDestroy()
    {
       // WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed -= DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;


    }

    private void LevelManager_OnLevelChanged(UnityEngine.Tilemaps.Tilemap arg1, 
        UnityEngine.Tilemaps.Tilemap arg2)
    {
        _chooseLevelPanel.SetActive(false);
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
