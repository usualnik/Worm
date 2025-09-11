using System;
using UnityEngine;

public class WormMovement : MonoBehaviour
{
    public event Action<Vector3> OnMove;

    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _collisionCheckDistance = 0.6f;
    [SerializeField] private Vector2 _collisionBoxSize = new Vector2(0.5f, 0.5f);

    private Vector2 _previousMovementDir = Vector2.right;
    private WormBody _wormBody;

    //private Vector3 _fallMovement = Vector3.down;
    private bool _isFalling = false;


    private const float OPPOSITE_DOT_PRODUCT = -1;

    private void Start()
    {
        _wormBody = GetComponentInChildren<WormBody>();

        _wormBody.OnBodyNotGrounded += WormBody_OnBodyNotGrounded;
    }

    private void OnDestroy()
    {
        _wormBody.OnBodyNotGrounded -= WormBody_OnBodyNotGrounded;

    }

    private void WormBody_OnBodyNotGrounded()
    {
        //Debug.LogWarning("NOT GROUNDED");
        //_isFalling = true;
    }

    private void Update()
    {
        //if (_isFalling)
        //{
        //    //HandleFalling();
        //}
       // else
       // {
            CheckForInput();
       // }
    }

    //private void HandleFalling()
    //{
    //    // Двигаем всю группу вниз
    //    transform.position += _fallMovement * 2f * Time.deltaTime;

    //    // При падении тоже обновляем позиции частей тела
    //    //_wormBody.UpdateBodyPositionsDuringFall(_fallMovement * 2f * Time.deltaTime);

    //    // Проверяем, не остановилось ли падение
    //    if (_wormBody.GetBodyParts()[0].GetGrounded() ||
    //        _wormBody.GetBodyParts()[1].GetGrounded() ||
    //        _wormBody.GetBodyParts()[2].GetGrounded())
    //    {
    //        _isFalling = false;
    //        //_shouldMove = false;
    //    }
    //}


    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(Vector2.right);
        }
    }

    private void TryMove(Vector3 moveDir)
    {
        if (CanMove(moveDir))
        {
            Move(moveDir);
        }
    }

    private void Move(Vector3 moveDir)
    {
        if (Vector2.Dot(moveDir, _previousMovementDir) != OPPOSITE_DOT_PRODUCT)
        {
            transform.position += moveDir * _movementSpeed;
            _previousMovementDir = moveDir;
            OnMove?.Invoke(moveDir);
        }
    }

    private bool CanMove(Vector3 moveDir)
    {
      
        Vector2 checkPosition = _wormBody.GetWormHead().transform.position;

   
        RaycastHit2D hit = Physics2D.BoxCast(
            origin: checkPosition,
            size: _collisionBoxSize,
            angle: 0f,
            direction: moveDir,
            distance: _collisionCheckDistance,
            layerMask: _obstacleLayerMask
        );

       
        return hit.collider == null;
    }

}