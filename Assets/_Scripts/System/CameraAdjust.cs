using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    private float _targetAspect = 16f / 9f;
    private Camera _mainCamera;
    
    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > _targetAspect || currentAspect < _targetAspect)
        {
            float difference = currentAspect / _targetAspect;
            _mainCamera.orthographicSize /= difference;
        }
       
       
    }
}

