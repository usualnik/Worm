using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Sprite _pausedSprite;
    [SerializeField] private Sprite _unpausedSprite;

    public event Action OnPauseButtonClicked;
    
    private Button _pausebutton;
    private Image _pauseImage;


    private bool _isPaused = false;

    private void Awake()
    {
        _pausebutton = GetComponent<Button>();
        _pauseImage = GetComponent<Image>();
    }

    private void Start()
    {
        _pausebutton.onClick.AddListener(OnPuseButtonClicked);
        
    }

    private void OnDestroy()
    {
        _pausebutton.onClick.RemoveListener(OnPuseButtonClicked);

    }
    private void OnPuseButtonClicked()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            _pauseImage.sprite = _unpausedSprite;
        }
        else
        {
            _isPaused = false;
            _pauseImage.sprite = _pausedSprite;

        }

        OnPauseButtonClicked?.Invoke();
    }
}
