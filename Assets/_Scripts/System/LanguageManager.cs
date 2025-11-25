using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    public string CurrentLanguage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        CurrentLanguage = GetLanguage();
    }

    public string GetLanguage()
    {
        return Application.systemLanguage switch
        {
            SystemLanguage.Russian => "ru",
            SystemLanguage.English => "en",
            _ => "en" 
        };
    }

}
