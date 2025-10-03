using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    
    public static event Action OnAnyTutorialButtonClicked;

    private Button _tutorialbutton;
   

    private void Awake()
    {
        _tutorialbutton = GetComponent<Button>();
     
    }

    private void Start()
    {
        _tutorialbutton.onClick.AddListener(Button_OnTutorialButtonClicked);

    }

    private void OnDestroy()
    {
        _tutorialbutton.onClick.RemoveListener(Button_OnTutorialButtonClicked);

    }
    private void Button_OnTutorialButtonClicked()
    {
        OnAnyTutorialButtonClicked?.Invoke();
    }
}
