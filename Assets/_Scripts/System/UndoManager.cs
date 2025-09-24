using System;
using System.Collections.Generic;
using UnityEngine;
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
        //_wormMovement.OnWormPosChanged += WormMovement_OnWormPosChanged;
        _destroyTile.OnTileDestroyed += DestroyTile_OnTileDestroyed;
        LevelManager.Instance.OnLevelChanged += LevelManager_OnLevelChanged;
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

    //private void WormMovement_OnWormPosChanged(Vector3[] positions)
    //{
    //    DoAction action = new DoAction(DoAction.ActionType.WormMove, positions);
    //    PushActionToStack(action);
    //}

    private void PushActionToStack(DoAction action)
    {
        _actionStack.Push(action);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            
            PopActionFromStack();
            //Add cooldown?
        }
    }

    private void PopActionFromStack()
    {
        if (_actionStack.Count > 1)
        {
            AudioManager.Instance.Play("Rewind");
            DoAction action = _actionStack.Pop();
            OnUndoAction?.Invoke(action);
        }else if (_actionStack.Count == 1)
        {
            DoAction action = _actionStack.Peek();
            OnUndoAction?.Invoke(action);
        }
        else
            Debug.Log("Nothing to undo, stack is empty");
    }

    private void ClearStack()
    {
        _actionStack.Clear();
    }

  
}
