using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private PauseButton pauseButton;
    private Animator animator;

    private void Awake()
    {
        pauseButton = GetComponentInChildren<PauseButton>();
        animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        pauseButton.OnPauseButtonClicked += PauseButton_OnPauseButtonClicked;
    }
    private void OnDestroy()
    {
        pauseButton.OnPauseButtonClicked -= PauseButton_OnPauseButtonClicked;

    }

    private void PauseButton_OnPauseButtonClicked()
    {
       animator.SetBool("IsPause", GameManager.Instance.IsPaused);
    }
}
