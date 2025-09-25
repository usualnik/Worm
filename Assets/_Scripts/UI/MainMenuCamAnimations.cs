using UnityEngine;

public class MainMenuCamAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

   // private int _animationIndex = 0;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _animator.SetTrigger("ToGameplay");
        }
    }
}
