using UnityEngine;

public class FallingTilePrefab : MonoBehaviour
{
    private bool _canFall;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask whatIsWorm;
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);

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
            _body.linearVelocity = Vector2.zero;
            _body.angularVelocity = 0f;
        }
        else
        {
            _body.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private bool CheckForWormUnderneath()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            boxSize,
            0f,
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