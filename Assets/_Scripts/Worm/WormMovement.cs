using System;
using UnityEngine;

public class WormMovement : MonoBehaviour
{
    public event Action<Vector3> OnMove;
    public event Action<Vector3[]> OnWormPosChanged;

    [Header("Movement + Falling")]
    [SerializeField] private Transform _wormStartPosition;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _fallingSpeed = 0.5f;
    [SerializeField] private float _moveCooldownTime = 0.5f;

    [Header("Collisions")]
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _collisionCheckDistance = 0.6f;
    [SerializeField] private Vector2 _collisionBoxSize = new Vector2(0.5f, 0.5f);

    //Move
    private Vector2 _previousMovementDir = Vector2.right;
    private float _moveCooldownTimer = 0f;
    private bool _canMove = true;

    //Fall
    private Vector3 _fallMovement = Vector3.down;
    private bool _isFalling = true;

    //System
    private WormBody _wormBody;

    private const float OPPOSITE_DOT_PRODUCT = -1;

    private void Awake()
    {
        transform.position = _wormStartPosition.position;
    }

    private void Start()
    {
        _wormBody = GetComponentInChildren<WormBody>();
        _wormBody.OnBodyNotGrounded += WormBody_OnBodyNotGrounded;
        UndoManager.Instance.OnUndoAction += UndoManager_OnUndoAction;
        WinConditionManager.Instance.OnWin += WinConditionManager_OnWin;
    }

   

    private void OnDestroy()
    {
        _wormBody.OnBodyNotGrounded -= WormBody_OnBodyNotGrounded;
        UndoManager.Instance.OnUndoAction -= UndoManager_OnUndoAction;
        WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;

    }

    private void WinConditionManager_OnWin()
    {
        transform.position = _wormStartPosition.position;
    }



    private void UndoManager_OnUndoAction(DoAction action)
    {
        //Undo movemvent if it was movemvent
        if (action.GetActionType() == DoAction.ActionType.WormMove)
        {
            UndoMove(action.GetWormPos());
        }

    }

    private void WormBody_OnBodyNotGrounded()
    {
        _isFalling = true;
    }

    private void Update()
    {
        if (_isFalling)
        {
            HandleFalling();
        }
        else
        {
            HandleMovementCooldown();
            CheckForInput();
        }
    }

    private void HandleMovementCooldown()
    {
        if (!_canMove)
        {
            _moveCooldownTimer += Time.deltaTime;
            if (_moveCooldownTimer >= _moveCooldownTime)
            {
                _canMove = true;
                _moveCooldownTimer = 0f;
            }
        }
    }

    private void CheckForInput()
    {
        if (!_canMove) return;

        if (Input.GetKey(KeyCode.W))
        {
            TryMove(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            TryMove(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            TryMove(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TryMove(Vector2.right);
        }
    }

    private void TryMove(Vector3 moveDir)
    {
        if (CanMove(moveDir))
        {
            Move(moveDir);
            _canMove = false;
        }
    }

    private void HandleFalling()
    {
        transform.position += _fallMovement * _fallingSpeed;
        _wormBody.UpdateBodyPosDuringFall();

        if (_wormBody.GetBodyParts()[0].GetGrounded() ||
            _wormBody.GetBodyParts()[1].GetGrounded() ||
            _wormBody.GetBodyParts()[2].GetGrounded())
        {
            _isFalling = false;
            
            OnWormPosChanged?.Invoke(GetWormPos());
        }
    }

    private void Move(Vector3 moveDir)
    {
        if (Vector2.Dot(moveDir, _previousMovementDir) != OPPOSITE_DOT_PRODUCT)
        {
            transform.position += moveDir * _movementSpeed;
            _previousMovementDir = moveDir;

            var wormPos = GetWormPos();

            OnWormPosChanged?.Invoke(wormPos);
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
    private void UndoMove(Vector3[] positions)
    {  
         transform.position = positions[0];
        _wormBody.UndoBodyPos(positions);
    }

    private Vector3[] GetWormPos()
    {
        var bodyPos = _wormBody.GetBodyParts();

        Vector3[] wormPos = new Vector3[4] { transform.position, bodyPos[0].transform.position,
            bodyPos[1].transform.position,  bodyPos[2].transform.position };

        return wormPos;
    }
}