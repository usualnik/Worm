using UnityEngine;

public class MainMenuCamAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        LevelButton_OnAnyLevelChosen();
    }

    private void LevelButton_OnAnyLevelChosen()
    {
        _animator.SetTrigger("ToGameplay");
    }
}
