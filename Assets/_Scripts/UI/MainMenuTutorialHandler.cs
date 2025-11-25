using UnityEngine;
using YG;

public class MainMenuTutorialHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainTutorial;
    [SerializeField] private GameObject _ruTutorial;
    [SerializeField] private GameObject _enTutorial;


    [SerializeField] private GameObject[] _ruTutorialImages;
    [SerializeField] private GameObject[] _enTutorialImages;

    private bool _tutorialStarted = false;
    private int _tutorialCount = 0;

    public void TutorialButton_OnAnyTutorialButtonClicked()
    {

        _tutorialStarted = true;

        _mainTutorial.SetActive(true);

        if (LanguageManager.Instance.CurrentLanguage == "ru")
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

    private void Update()
    {
        if (!_tutorialStarted) return;

        if (Input.anyKeyDown)
        {
            if (LanguageManager.Instance.CurrentLanguage == "ru" && _tutorialCount < _ruTutorialImages.Length - 1)
            {
                _ruTutorialImages[_tutorialCount].SetActive(false);
                _tutorialCount++;
                _ruTutorialImages[_tutorialCount].SetActive(true);

            }
            else if (LanguageManager.Instance.CurrentLanguage == "en" && _tutorialCount < _ruTutorialImages.Length - 1)
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
        //OnTutorialEnded?.Invoke();
    }
}
