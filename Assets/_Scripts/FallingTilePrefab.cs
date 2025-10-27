using UnityEngine;

public class FallingTilePrefab : MonoBehaviour
{
    private bool _canFall;
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private LayerMask whatIsWorm;

    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _canFall = CheckForWormUnderneath();

        if (!_canFall)
        {
            _body.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            _body.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private bool CheckForWormUnderneath()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            rayDistance,
            whatIsWorm
        );

        if (hit.collider != null)
        {
            return false;
        }

        return true;
    }
}