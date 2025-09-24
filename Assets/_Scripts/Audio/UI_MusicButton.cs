using UnityEngine;
using UnityEngine.UI;

public class UI_MusicButton : MonoBehaviour
{
    private Button _musicButton;
    private Image _buttonImage;

    [SerializeField] private Sprite _musicOnSprite;
    [SerializeField] private Sprite _musicOffSprite;

    private bool _isPlaying;


    private void Awake()
    {
        _musicButton = GetComponent<Button>();        
        _isPlaying = true;
        _buttonImage = GetComponent<Image>();
       
    }

    private void Start()
    {
        _musicButton.onClick.AddListener(ToggleSound);


        if (!BackgroundMusic.Instance.IsPlaying())
        {
            _buttonImage.sprite = _musicOffSprite;
            _isPlaying = false;
        }
        else
        {

            _buttonImage.sprite = _musicOnSprite;
            _isPlaying = true;

        }

    }

    private void OnDestroy()
    {
        _musicButton.onClick.RemoveListener(ToggleSound);

    }

    private void ToggleSound()
    {
        BackgroundMusic.Instance.ToggleMute();

        if (_isPlaying)
        {
            _buttonImage.sprite = _musicOffSprite;
            _isPlaying = false;
        }
        else
        {
            _buttonImage.sprite = _musicOnSprite;
            _isPlaying = true;
        }
    }
}
