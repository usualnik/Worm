using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DoAction
{
    public enum ActionType
    {
        WormMove,
        TileDestruction
    }

    private ActionType _actionType;
    private Vector3Int _tilepositionVector;

    private Vector3[] _wormPositions;


    public DoAction(ActionType actionType, Vector3Int posVector)
    {
        this._actionType = actionType;
        this._tilepositionVector = posVector;
    }
    public DoAction(ActionType actionType, Vector3[] positions)
    {
        this._actionType = actionType;
        this._wormPositions = positions;
    }

    public ActionType GetActionType() => _actionType;
    public Vector3Int GetTilePosVector() => _tilepositionVector;
    public Vector3[] GetWormPos() => _wormPositions;
}
public class UndoManager : MonoBehaviour
{
    public static UndoManager Instance {  get; private set; }
    public event Action<DoAction> OnUndoAction;

    [SerializeField] private WormMovement _wormMovement;
    [SerializeField] private DestroyTile _destroyTile;
    [SerializeField] private Button _undoButton;

    private bool _undoPressed;
    private Stack<DoAction> _actionStack = new Stack<DoAction>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of UndoManager");
        }
    }

    private void Start()
    {      
        _destroyTile.OnTileDestroyed += DestroyTile_OnTileDestroyed;
        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;

        if (_undoButton != null)
        {
            _undoButton.onClick.AddListener(() => PopActionFromStack());
        }
    }


    private void OnDestroy()
    {
       // _wormMovement.OnWormPosChanged -= WormMovement_OnWormPosChanged;
        _destroyTile.OnTileDestroyed -= DestroyTile_OnTileDestroyed;
        LevelManager.Instance.OnLevelChanged -= LevelManager_OnLevelChanged;

    }

    private void LevelManager_OnLevelChanged(UnityEngine.Tilemaps.Tilemap arg1, UnityEngine.Tilemaps.Tilemap arg2)
    {
        ClearStack();
    }


    private void DestroyTile_OnTileDestroyed(Vector3Int pos)
    {
        DoAction action = new DoAction(DoAction.ActionType.TileDestruction, pos);
        PushActionToStack(action);
    }

    private void PushActionToStack(DoAction action)
    {
        Debug.Log("ACTION PUSHED " + action.GetTilePosVector()); 
        _actionStack.Push(action);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && CanUndo())
        {            
            PopActionFromStack();
            //Add cooldown?
        }
    }

    private void PopActionFromStack()
    {        

        if (_actionStack.Count > 0)
        {
            AudioManager.Instance.Play("Rewind");
            DoAction action = _actionStack.Pop();
            OnUndoAction?.Invoke(action);
        }
        //}else if (_actionStack.Count == 1)
        //{
        //    DoAction action = _actionStack.Peek();
        //    OnUndoAction?.Invoke(action);
        //}
        else
            Debug.Log("Nothing to undo, stack is empty");
    }

    private void ClearStack()
    {
        _actionStack.Clear();
    } 
    public void PopAllActionsFromStack()
    {
        while (_actionStack.Count > 0)
        {
            DoAction action = _actionStack.Pop();
            OnUndoAction?.Invoke(action);
        }

        ClearStack();
    }

    private bool CanUndo() => !GameManager.Instance.IsPaused;

  
}
