using UnityEngine;

public class ShowTutorialHandler : MonoBehaviour
{
    [SerializeField] private int _levelToShowTutorial;

    private void Start()
    {
        if(PlayerData.Instance.GetCurrentLevel() == _levelToShowTutorial)
        {
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {
        UIManager.Instance.TutorialButton_OnAnyTutorialButtonClicked();
    }
}
