using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoadLevelManager : MonoBehaviour
{
    public static MenuLoadLevelManager Instance { get; private set; }

    [SerializeField] private LevelButton[] _levelButtons;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("More than one instance of Level manager");

    }

    private void Start()
    {

        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelClick += LevelButton_OnLevelClick;
        }
    }
   
    private void OnDestroy()
    {
        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelClick += LevelButton_OnLevelClick;
        }

    }

    private void LevelButton_OnLevelClick(int levelindex)
    {
       LoadLevel(levelindex);
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);       
    }

       
}
