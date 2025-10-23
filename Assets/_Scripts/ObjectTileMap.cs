using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectTileMap : MonoBehaviour
{
    [SerializeField] private TileBase _objectTile;
    [SerializeField] protected DestroyTile _tileDestroyer;

    private Tilemap _objectTileMap;

    private void Awake()
    {
        _objectTileMap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        UndoManager.Instance.OnUndoAction += UndoManager_OnUndoAction;
    }


    private void OnDestroy()
    {
        UndoManager.Instance.OnUndoAction -= UndoManager_OnUndoAction;


    }

    private void UndoManager_OnUndoAction(DoAction action)
    {
        //Restore tile if it was tile destr
        if (action.GetActionType() == DoAction.ActionType.TileDestruction)
        {
            _objectTileMap.SetTile(action.GetTilePosVector(), _objectTile );
        }
    }

   

}
