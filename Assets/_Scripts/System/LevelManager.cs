using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public event Action<Tilemap, Tilemap> OnLevelChanged;
    [SerializeField] private GameObject[] _levels;

    [SerializeField] private LevelButton[] _levelButtons;

    [SerializeField]
    private int _currentLevel = 0;

    private Tilemap _currentObjectTileMap;
    private Tilemap _currentReferenceTileMap;

    private const int UNLOCKED_LEVELS_AMOUNT = 5;
    //private const int MAX_LEVELS_COUN = 25;
    private const float DELAY_BETWEEN_LOAD_LEVEL = 3.0f;


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

        //_currentLevel = PlayerData.Instance.GetCurrentMaxLevel();

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
        Invoke(nameof(LoadNextLevel), DELAY_BETWEEN_LOAD_LEVEL);     
    }

    private void LoadNextLevel()
    {
        _levels[_currentLevel].SetActive(false);
        _currentLevel++;

        if (_currentLevel >= PlayerData.Instance.GetCurrentMaxLevel())
            UnlockNewLevels();

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }
   

    private void UnlockNewLevels()
    {
        PlayerData.Instance.SetCurrentMaxLevel(PlayerData.Instance.GetCurrentMaxLevel() 
            + UNLOCKED_LEVELS_AMOUNT);
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

        //PlayerData.Instance.SetCurrentLevel(_currentLevel);

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }

    public void RestartLevel()
    {
        //OnlevelRestart

        _levels[_currentLevel].SetActive(false);       

        //PlayerData.Instance.SetCurrentLevel(_currentLevel);

        _levels[_currentLevel].SetActive(true);

        ChangeCurrentTilemaps();

        OnLevelChanged?.Invoke(_currentObjectTileMap, _currentReferenceTileMap);
    }
}
