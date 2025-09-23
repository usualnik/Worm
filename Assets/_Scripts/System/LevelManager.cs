using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {  get; private set; }

    public event Action<Tilemap, Tilemap> OnLevelChanged;
    [SerializeField] private GameObject[] _levels;

    private int _currentLevel = 0;

    private Tilemap _currentObjectTileMap;
    private Tilemap _currentReferenceTileMap;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("More than one instance of Level manager");


        _levels[_currentLevel].SetActive(true);
        ChangeCurrentTilemaps();
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
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        _levels[_currentLevel].SetActive(false);
        _currentLevel++;
        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap,_currentReferenceTileMap);
    }

    private void ChangeCurrentTilemaps()
    {
        var objectTileMap = _levels[_currentLevel].GetComponentInChildren<ObjectTileMap>();
        _currentObjectTileMap = objectTileMap.GetComponent<Tilemap>();

        var referenceTileMap = _levels[_currentLevel].GetComponentInChildren<ReferenceTileMap>();
        _currentReferenceTileMap = referenceTileMap.GetComponent<Tilemap>();
    }
}
