using UnityEngine;

public class ShowFirstTutorialHandler : MonoBehaviour
{

    [SerializeField] private TutorialButton _tutorialButton;

    private static bool _isFirstTimeShowTutorial = true;

    private void Start()
    {
        if (_isFirstTimeShowTutorial)
        {
            ShowTutorial();
            _isFirstTimeShowTutorial = false;
        }
    }

    private void ShowTutorial()
    {
        _tutorialButton.Button_OnTutorialButtonClicked();
    }
}
