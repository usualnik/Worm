using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public event Action<int> OnLevelClick;
    [SerializeField] private int _levelIndex;
    
    private Button _levelButton;

    private void Awake()
    {
        _levelButton = GetComponent<Button>();
    }


    private void Start()
    {
        _levelButton.onClick.AddListener(Click);
    }
    private void OnDestroy()
    {
        _levelButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        OnLevelClick?.Invoke(_levelIndex);
    }

}
