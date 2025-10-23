using UnityEngine;

public class WormHeadAnimation : MonoBehaviour
{
    [SerializeField] private DestroyTile _tiledestroyer;
    private Animator _animator;

   // private bool _isBiting = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _tiledestroyer.OnObjectDestroyed += Tiledestroyer_OnTileDestroyed;
    }
    private void OnDestroy()
    {
        _tiledestroyer.OnObjectDestroyed -= Tiledestroyer_OnTileDestroyed;
    }

    private void Tiledestroyer_OnTileDestroyed(GameObject gameObject)
    {
        _animator.SetBool("IsBiting", true);
    }

    public void OnBiteAnimationEnded()
    {
        _animator.SetBool("IsBiting", false);

    }


}
