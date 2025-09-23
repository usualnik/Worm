using UnityEngine;

public class MainMenuCamAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _animationIndex = 0;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _animationIndex++;

            if (_animationIndex == 1)
            {
                _animator.SetTrigger("ToLevels");
            }
            else if (_animationIndex == 2)
            {
                _animator.SetTrigger("ToGameplay");
            }
        }
    }
}
