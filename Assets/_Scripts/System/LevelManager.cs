using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public event Action<Tilemap, Tilemap> OnLevelChanged;
    [SerializeField] private GameObject[] _levels;

    [SerializeField] private LevelButton[] _levelButtons;

    private int _currentLevel = 0;

    private Tilemap _currentObjectTileMap;
    private Tilemap _currentReferenceTileMap;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("More than one instance of Level manager");

    }

    private void Start()
    {
        WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;

        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelClick += LevelButton_OnLevelClick;
        }

        _currentLevel = PlayerData.Instance.GetCurrentLevel();

        _levels[_currentLevel].SetActive(true);
        ChangeCurrentTilemaps();
    }
   
    private void OnDestroy()
    {
        WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;
        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelClick += LevelButton_OnLevelClick;
        }

    }

    private void LevelButton_OnLevelClick(int levelindex)
    {
       LoadLevel(levelindex);
    }

    private void WinConditionManager_OnWin()
    {
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        _levels[_currentLevel].SetActive(false);
        _currentLevel++;

        PlayerData.Instance.SetCurrentLevel(_currentLevel);

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }

    private void ChangeCurrentTilemaps()
    {
        var objectTileMap = _levels[_currentLevel].GetComponentInChildren<ObjectTileMap>();
        _currentObjectTileMap = objectTileMap.GetComponent<Tilemap>();

        var referenceTileMap = _levels[_currentLevel].GetComponentInChildren<ReferenceTileMap>();
        _currentReferenceTileMap = referenceTileMap.GetComponent<Tilemap>();
    }

    private void LoadLevel(int levelIndex)
    {
        _levels[_currentLevel].SetActive(false);
        _currentLevel = levelIndex;

        PlayerData.Instance.SetCurrentLevel(_currentLevel);

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }

    public void RestartLevel()
    {
        //OnlevelRestart

        _levels[_currentLevel].SetActive(false);       

        PlayerData.Instance.SetCurrentLevel(_currentLevel);

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }
}
