using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameLevelLoader : MonoBehaviour
{
    private const int MAIN_MENU_BUILD_INDEX = 1;
    private const int MAX_LEVEL_INDEX = 16;
    private const int FIRST_LEVEL_INDEX = 2;

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
       
        Invoke(nameof(LoadNextLevel), 2f);       
    }

    private void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < MAX_LEVEL_INDEX)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData.Instance.SetCurrentMaxLevel(PlayerData.Instance.GetCurrentLevel() + 1);

        }
        else
        {
            SceneManager.LoadScene(FIRST_LEVEL_INDEX);
        }

        
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }

    public void RestartScene()
    {
        YG2.InterstitialAdvShow();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
