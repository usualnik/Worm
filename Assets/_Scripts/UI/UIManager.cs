using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _youWinPanel;

    private void Awake()
    {
        _youWinPanel.SetActive(false);
    }

    private void Start()
    {
        WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
    }

    private void OnDestroy()
    {
        WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;

    }

    private void WinConditionManager_OnWin()
    {
        _youWinPanel.SetActive(true);
    }
}
