using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public static event Action OnAnyLevelChosen;
    public event Action<int> OnLevelClick;
    [SerializeField] private int _levelIndex;
    [SerializeField] private bool _isLevelUnloked;
    [SerializeField] private GameObject _lockIcon;

    private Button _levelButton;

    private void Awake()
    {
        _levelButton = GetComponent<Button>();
    }


    private void Start()
    {
        _levelButton.onClick.AddListener(Click);


        if (_levelIndex <= PlayerData.Instance.GetCurrentMaxLevel() - 1)
        {
            _isLevelUnloked = true;
            _lockIcon.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        _levelButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        if (_isLevelUnloked)
        {
            OnLevelClick?.Invoke(_levelIndex);
            OnAnyLevelChosen?.Invoke();
        }
    }

}
