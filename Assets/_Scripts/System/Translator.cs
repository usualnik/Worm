using TMPro;
using UnityEngine;

public class Translator : MonoBehaviour
{
    [SerializeField] private string _ruText;
    [SerializeField] private string _enText;

    private TextMeshProUGUI _textToTranslate;

    private void Awake()
    {
        _textToTranslate = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (_textToTranslate == null) return;

        _textToTranslate.text = LanguageManager.Instance.CurrentLanguage == "ru" ? _ruText : _enText;
    }
}