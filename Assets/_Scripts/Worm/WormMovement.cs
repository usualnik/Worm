using System;
using UnityEngine;
using UnityEngine.UI;

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


    [Header("Mobile Input Buttons")]
    [SerializeField] private Button _upInputButton;
    [SerializeField] private Button _leftInputButton;
    [SerializeField] private Button _downInputButton;
    [SerializeField] private Button _rightInputButton;

    private bool _upPressed = false;
    private bool _leftPressed = false;
    private bool _downPressed = false;
    private bool _rightPressed = false;


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


        SetupMobileInputButtons();

    }   

    private void OnDestroy()
    {
        _wormBody.OnBodyNotGrounded -= WormBody_OnBodyNotGrounded;
        UndoManager.Instance.OnUndoAction -= UndoManager_OnUndoAction;
        WinConditionManager.Instance.OnWin -= WinConditionManager_OnWin;


        if (_upInputButton != null)
            _upInputButton.onClick.RemoveAllListeners();
        if (_leftInputButton != null)
            _leftInputButton.onClick.RemoveAllListeners();
        if (_downInputButton != null)
            _downInputButton.onClick.RemoveAllListeners();
        if (_rightInputButton != null)
            _rightInputButton.onClick.RemoveAllListeners();

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
            
            if (!GameManager.Instance.IsPaused)
            {               
                CheckForInput();
                CheckForMobileInput();
            }           
        }
    }

    private void SetupMobileInputButtons()
    {
        // Настраиваем обработчики для кнопок
        if (_upInputButton != null)
        {
            _upInputButton.onClick.AddListener(() => HandleButtonInput(Vector2.up));

            // Добавляем обработчики для нажатия и отпускания (для непрерывного движения)
            var eventTrigger = _upInputButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            AddPointerHandlers(eventTrigger,
                () => _upPressed = true,
                () => _upPressed = false);
        }

        if (_leftInputButton != null)
        {
            _leftInputButton.onClick.AddListener(() => HandleButtonInput(Vector2.left));
            var eventTrigger = _leftInputButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            AddPointerHandlers(eventTrigger,
                () => _leftPressed = true,
                () => _leftPressed = false);
        }

        if (_downInputButton != null)
        {
            _downInputButton.onClick.AddListener(() => HandleButtonInput(Vector2.down));
            var eventTrigger = _downInputButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            AddPointerHandlers(eventTrigger,
                () => _downPressed = true,
                () => _downPressed = false);
        }

        if (_rightInputButton != null)
        {
            _rightInputButton.onClick.AddListener(() => HandleButtonInput(Vector2.right));
            var eventTrigger = _rightInputButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            AddPointerHandlers(eventTrigger,
                () => _rightPressed = true,
                () => _rightPressed = false);
        }
    }

    private void AddPointerHandlers(UnityEngine.EventSystems.EventTrigger eventTrigger,
       Action onPointerDown, Action onPointerUp)
    {
        // Обработчик нажатия
        var pointerDown = new UnityEngine.EventSystems.EventTrigger.Entry();
        pointerDown.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => onPointerDown());
        eventTrigger.triggers.Add(pointerDown);

        // Обработчик отпускания
        var pointerUp = new UnityEngine.EventSystems.EventTrigger.Entry();
        pointerUp.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => onPointerUp());
        eventTrigger.triggers.Add(pointerUp);

        // Обработчик выхода за пределы кнопки (на случай, если палец ушел за пределы)
        var pointerExit = new UnityEngine.EventSystems.EventTrigger.Entry();
        pointerExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((e) => onPointerUp());
        eventTrigger.triggers.Add(pointerExit);
    }
    private void CheckForMobileInput()
    {
        if (!_canMove) return;

        // Проверяем состояние кнопок в порядке приоритета
        if (_upPressed)
        {
            AudioManager.Instance.Play("Input");
            TryMove(Vector2.up);
        }
        else if (_leftPressed)
        {
            AudioManager.Instance.Play("Input");
            _wormBody.FlipHeadSprite(true);
            TryMove(Vector2.left);
        }
        else if (_downPressed)
        {
            AudioManager.Instance.Play("Input");
            TryMove(Vector2.down);
        }
        else if (_rightPressed)
        {
            AudioManager.Instance.Play("Input");
            _wormBody.FlipHeadSprite(false);
            TryMove(Vector2.right);
        }
    }

    private void HandleButtonInput(Vector2 direction)
    {
        if (!_canMove || _isFalling) return;

        // Для одиночных нажатий (если нужно именно по клику, а не удержанию)
        AudioManager.Instance.Play("Input");

        if (direction == Vector2.left)
            _wormBody.FlipHeadSprite(true);
        else if (direction == Vector2.right)
            _wormBody.FlipHeadSprite(false);

        TryMove(direction);
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
            AudioManager.Instance.Play("Input");
            TryMove(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            AudioManager.Instance.Play("Input");

            _wormBody.FlipHeadSprite(true);

            TryMove(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            AudioManager.Instance.Play("Input");

            TryMove(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            AudioManager.Instance.Play("Input");

            _wormBody.FlipHeadSprite(false);

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