using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI PANELS")]
    [SerializeField] private GameObject _youWinPanel;
    [SerializeField] private GameObject _youLosePanel;


    [Header("SYSTEM")]
    [SerializeField] private DestroyTile _destroyTile;

    private void Awake()
    {
        _youWinPanel.SetActive(false);
        _youLosePanel.SetActive(false);
    }

    private void Start()
    {
        WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed += DestroyTile_OnDeathTileDestroyed;
    }

    
    private void OnDestroy()
    {
        WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed -= DestroyTile_OnDeathTileDestroyed;


    }
    private void DestroyTile_OnDeathTileDestroyed()
    {
        _youLosePanel.SetActive(true);        
    }


    private void WinConditionManager_OnWin()
    {
        //_youWinPanel.SetActive(true);
    }
}
