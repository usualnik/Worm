using UnityEngine;

public class FingerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _fingerObject;

    private void Start()
    {
        if(PlayerData.Instance.GetCurrentLevel() == 3)
        {
            ShowFinger();
        }
    }

    private void ShowFinger()
    {
        _fingerObject.SetActive(true);
    }
}
