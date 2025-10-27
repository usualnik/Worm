using System;
using UnityEngine;

public class LoseConditionManager : MonoBehaviour
{
    public event Action OnLose;
    public static LoseConditionManager Instance { get; private set; }

    private DestroyTile _tileDestroyer;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _tileDestroyer = FindAnyObjectByType<DestroyTile>();

        _tileDestroyer.OnObjectDestroyed += TileDestroyer_OnObjectDestroyed;
    }
    private void OnDestroy()
    {
        _tileDestroyer.OnObjectDestroyed -= TileDestroyer_OnObjectDestroyed;

    }
    private void TileDestroyer_OnObjectDestroyed(GameObject obj)
    {
        CheckLoseCondition();
    }

    private void CheckLoseCondition()
    {     

        if (WinConditionManager.Instance.GetRemainingObjectsCount() < 
            WinConditionManager.Instance.GetReferenceObjectsCount())
        {
            if(!GameManager.Instance.IsGameOver)
                OnLose?.Invoke();
        }
    }
}
