using System;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    public event Action OnTutorialEnded;

    [Header("UI PANELS")]
    //[SerializeField] private GameObject _youWinPanel;
    [SerializeField] private GameObject _youLosePanel;
    [SerializeField] private GameObject _chooseLevelPanel;
    [SerializeField] private GameObject _mobileInputPanel;
    [SerializeField] private GameObject _gamePlayUiPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("Tutorials")]
    [SerializeField] private GameObject _mainTutorial;
    [SerializeField] private GameObject _ruTutorial;
    [SerializeField] private GameObject _enTutorial;


    [SerializeField] private GameObject[] _ruTutorialImages;
    [SerializeField] private GameObject[] _enTutorialImages;


    [Header("SYSTEM")]
    [SerializeField] private DestroyTile _destroyTile;

 
    private bool _tutorialStarted = false;
    private int _tutorialCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of ui manager");
        }
    }
    private void Start()
    {
       // WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed += DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
        GameManager.Instance.OnPause +=  GameManager_OnPauseButtonClicked;
        TutorialButton.OnAnyTutorialButtonClicked += TutorialButton_OnAnyTutorialButtonClicked;


        _chooseLevelPanel.SetActive(true);
    }

  
    private void OnDestroy()
    {
       // WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;
        _destroyTile.OnDeathTileDestroyed -= DestroyTile_OnDeathTileDestroyed;
        LevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;
        GameManager.Instance.OnPause -=  GameManager_OnPauseButtonClicked;
        TutorialButton.OnAnyTutorialButtonClicked -= TutorialButton_OnAnyTutorialButtonClicked;

    }

    private void TutorialButton_OnAnyTutorialButtonClicked()
    {

       _tutorialStarted = true;

       _mainTutorial.SetActive(true);

        if (YG2.envir.language == "ru")
        {
            _ruTutorial.SetActive(true);
            _ruTutorialImages[0].SetActive(true);
        }
        else
        {
           _enTutorial.SetActive(true);
           _enTutorialImages[0].SetActive(true);
        }
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

    private void Update()
    {
        if (!_tutorialStarted) return;

        if (Input.anyKeyDown )
        {
            if(YG2.envir.language == "ru" && _tutorialCount < _ruTutorialImages.Length - 1)
            {
                _ruTutorialImages[_tutorialCount].SetActive(false);
                _tutorialCount++;
                _ruTutorialImages[_tutorialCount].SetActive(true);

            }
            else if(YG2.envir.language == "en" && _tutorialCount < _ruTutorialImages.Length - 1)
            {
                _enTutorialImages[_tutorialCount].SetActive(false);
                _tutorialCount++;
                _enTutorialImages[_tutorialCount].SetActive(true);
            }
            else
            {
              TutorialEnded();
            }

        }
      
    }

    private void TutorialEnded()
    {
        _tutorialCount = 0;
        _mainTutorial.SetActive(false);
        _ruTutorial.SetActive(false);
        _enTutorial.SetActive(false);

        foreach (var item in _ruTutorialImages)
        {
            item.SetActive(false);
        }

        foreach (var item in _enTutorialImages)
        {
            item.SetActive(false);
        }

        _tutorialStarted = false;
        OnTutorialEnded?.Invoke();
    }

}
